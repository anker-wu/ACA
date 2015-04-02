/*
 * <pre>
 *  Accela Citizen Access
 *  File: global.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: Global.js 77905 2007-10-15 12:49:28Z ACHIEVO\lytton.cheng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
*/

var GlobalConst = new function() {
    this.ChildControlIdPrefix = '_ChildControl';
};

function hideMessage(containerId) {
    if (containerId) {
        showMessage(containerId, "", "", true, 0);
    }
    else {
        showMessage("messageSpan", "", "", true, 0);
    }
}

function showMessage4Popup(msg, msgtype) {
    showMessage("messageSpan", msg, msgtype, true, 1, true);

    $(document).ready(function () {
        //Delay 1 ms to hide the 'please wait' dialog before the pop the modal dialog (e.g.: alert).
        window.setTimeout(function () { window.scrollTo(0, 0); }, 1);
    });
}

function showNormalMessage(msg, msgtype)
{
    showMessage("messageSpan",msg,msgtype,true,1);
}

function showMessage(containerId, msg, msgtype, candelete, maxmsgcount, notScrollIntoView) {
    //get the container object.
    var container = document.getElementById(containerId);
   
    if (container == null ||typeof (container) == "undefined") 
    {
        return;
    }

    var parentDiv = container.parentNode;
    if (typeof (parentDiv) != 'undefined' && parentDiv.tagName.toUpperCase() == 'DIV') {
        if (msg == '' && msgtype == '' && maxmsgcount == 0) {
            if ((' ' + parentDiv.className + ' ').indexOf(' ACA_Hide ', 0) < 0) {
                parentDiv.className += ' ACA_Hide';
            }
        }
        else {
            parentDiv.className = (' ' + parentDiv.className + ' ').replace(/\s+ACA_Hide\s+/gi, ' ').replace(/^\s+/, '').replace(/\s+$/, '');
        }
    }

    //get msgdiv's class, icon's class, icon's title, icon's alt,message.
    var msgclass = "";
    var iconclass = "";
    var icontitle = "";
    var iconalt = "";
    var iconsrc = "";
    var message = "";

    if (msgtype == "Success")
    {
        msgclass  = "ACA_Message_Success ACA_Message_Success_FontSize";
        iconclass = "ACA_Success_Icon";
        icontitle = getText.global_js_showConfirm_title;
        iconalt   = getText.global_js_showConfirm_title;
        iconsrc   = getText.global_js_showConfirm_src;
        message   = msg;
    }
    else if (msgtype == "Error")
    {
        msgclass  = "ACA_Message_Error ACA_Message_Error_FontSize";
        iconclass = "ACA_Error_Icon";
        icontitle = getText.global_js_showError_title;
        iconalt   = getText.global_js_showError_title;
        iconsrc = getText.global_js_showError_src;
        
        if (msg!="")
        {
            message = icontitle + '<br/>' + msg;
        }
    }
    else if (msgtype == "Notice")
    {
        msgclass  = "ACA_Message_Notice ACA_Message_Notice_FontSize";
        iconclass = "ACA_Notice_Icon";
        icontitle = getText.global_js_showNotice_title;
        iconalt   = getText.global_js_showNotice_title;
        iconsrc = getText.global_js_showNotice_src;
        
        if (msg!="")
        {
            message = icontitle + ':<br/>' + msg;
        }
    }

    // create one message bar
    var messageBar = null;
    
    if (msg != '') {
        messageBar = createOneMessageBar(message, icontitle, iconalt, msgclass, iconclass, iconsrc, candelete);
    }
                
    if (container.childNodes.length == 0 ) 
    {
       //CREATE MESSAGE BAR
       if(msg != '' && maxmsgcount != 0 && messageBar != null)
       {
           var divMessages = document.createElement('div');
           divMessages.id = containerId + '_messages';
           divMessages.appendChild(messageBar);       
           container.appendChild(divMessages);
       }
    }
    else
    {
        var divMessages = document.getElementById(containerId + '_messages');
    
        if (divMessages == null || typeof (divMessages) != "undefined")
        {
            if (msg != '' && messageBar != null)
            {
                if (divMessages.childNodes.length > 0) {
                    var isSkip = false;
                    
                    for (var i = 0; i < divMessages.childNodes.length; i++) {
                        if ($(divMessages.childNodes[i]).text() == $(messageBar).text()) {
                            isSkip = true;
                            break;
                        }
                    }

                    if (!isSkip) {
                        messageBar.style.marginBottom = '5px';
                        divMessages.insertBefore(messageBar, divMessages.childNodes[0]);
                    }
                }
                else
                {
                    divMessages.appendChild(messageBar);
                }   
            }
            
            var canDelDivCount = 0; 
              
            //if there are messages that can be deleted(Attribute(CanDelete)="true") 
            //and the messages'count bigger than the parameter(maxmsgcount).
            
            for(var i = 0; i < divMessages.childNodes.length;i++)
            {
                if(typeof(divMessages.childNodes[i].getAttribute("candelete")) != "undefined" && divMessages.childNodes[i].getAttribute("candelete") =="candelete")
                {
                    canDelDivCount = canDelDivCount + 1;
                    
                    if (maxmsgcount >=0 && canDelDivCount > maxmsgcount)
                    {
                        divMessages.removeChild(divMessages.childNodes[i]);
                        i = i -1;
                    }
                }
            }
        }
        
        if (divMessages.childNodes.length > 0)
        {
            divMessages.childNodes[divMessages.childNodes.length -1].style.marginBottom ="0px";
        }
    }

    if (containerId !='' && msg != '' && maxmsgcount != 0) {
        if (!notScrollIntoView) {
            goToMessageBlock(containerId);
        }

        $(document).ready(function () {
            //Delay 1 ms to hide the 'please wait' dialog before the pop the modal dialog (e.g.: alert).
            window.setTimeout(function () { showMessageForSection508(message) }, 1);
        });
    }
}

// create div for message
function createOneMessageBar(message, icontitle, iconalt, divclass, iconclass, iconsrc, candelete, needbottom) 
{
    // create the outer message div
    var divMessage = document.createElement('div');
    divMessage.className = divclass;
    if (candelete)
    {
            divMessage.setAttribute("candelete","candelete");    
    }
//    divMessage.setAttribute("divtype","onemessage");

    // create notice icon
    var divMessageIcon = document.createElement('div');
    divMessageIcon.className = iconclass;
    
    //create img icon
    var imgMessageIcon = document.createElement('img');
    imgMessageIcon.title = icontitle;
    imgMessageIcon.alt = iconalt;
    imgMessageIcon.src = iconsrc;
    
    // create space div
    var divSpace = document.createElement('div');
    divSpace.innerHTML = "&nbsp;";
    var divMessageInfo = document.createElement('div');
    
    divMessageInfo.innerHTML = message;

    var table = document.createElement("table");
    table.setAttribute("role", "presentation");
    table.setAttribute("border","0");
    table.setAttribute("cellpadding","0");
    table.setAttribute("cellspacing","0");
    table.setAttribute("table-layout", "fixed");
   
    var tbody = document.createElement("tbody");
    var tr = document.createElement("tr");
    var tdIcon = document.createElement("td");
    $(tdIcon).addClass("ACA_Message_Icon");

    var tdSpace = document.createElement("td");
    $(tdSpace).addClass("ACA_XShoter");

    var tdMessage = document.createElement("td");
    $(tdMessage).addClass("ACA_Message_Content");
    
    // the first td loads the notice icon, the second loads the message.
    divMessageIcon.appendChild(imgMessageIcon);
    tdIcon.appendChild(divMessageIcon);
    tdSpace.appendChild(divSpace);
    tdMessage.appendChild(divMessageInfo);
    tr.appendChild(tdIcon);
    tr.appendChild(tdSpace);
    tr.appendChild(tdMessage);
    tbody.appendChild(tr);
    table.appendChild(tbody);

    divMessage.appendChild(table);
    
    return divMessage;
}

function goToMessageBlock(containerId)
{
    var a = $get(containerId);

    if (a) {
        if (typeof(firstErrorMsgId) == "undefined" || !firstErrorMsgId) {
            scrollIntoView(containerId);
            firstErrorMsgId = containerId;
        } else {
            scrollIntoView(firstErrorMsgId);
        }
    }
}

function show(obj)
{
    var div = document.getElementById(obj);
    
    if (div != null)
    {
        div.style.display = "block";
    }
}

function SetNotAsk() {
    NeedAsk = false;
    window.setTimeout('NeedAsk=true', 1500);
}

function hide(obj)
{
    var div = document.getElementById(obj);
    
    if (div != null)
    {
        div.style.display = "none";
    }
} 

// true: browser's type is fireFox
// false: browser's type isn't fireFox
function isFireFox()
{
    if(navigator.userAgent.indexOf("Firefox")>0)
    {
        return true;
    } 
    else
    {
        return false; 
    }  
}

// true: browser's type is Safari
// false: browser's type isn't Safari
function isSafari()
{
    if(navigator.userAgent.indexOf("Safari")>0)
    {
        return true;
    } 
    else
    {
        return false; 
    }  
}

// true: browser's type is Chrome
// false: browser's type isn't Chrome
function isChrome() {
    if (navigator.userAgent.indexOf("Chrome") > 0) {
        return true;
    }

    return false;   
}

//parse currency from formatted value for JavaScript calculation, such as from 123,45.67 to 12345.67 in german culture
function I18nParseCurrencyForJS(formattedCurrency){
    //if GLOBAL_SERVICE_PROVIDER_CULTURE etc. is not defined, return original value
    if(formattedCurrency==null
        || formattedCurrency.replace(/ /g,'')==''
        || typeof(GLOBAL_SERVICE_PROVIDER_CULTURE)=="undefined"
        || typeof(GLOBAL_CURRENCY_SYMBOL)=="undefined"
        || typeof(GLOBAL_CURRENCY_GROUP_SEPARATOR)=="undefined"
        || typeof(GLOBAL_CURRENCY_DECIMAL_SEPARATOR)=="undefined"
       )
    {
        return formattedCurrency;
    }
    var result = formattedCurrency.replace(new RegExp("(\\" + GLOBAL_CURRENCY_SYMBOL + ")", "g"), "");
    result = result.replace(new RegExp("(\\" + GLOBAL_CURRENCY_GROUP_SEPARATOR + ")", "g"), "") + '';
    result = result.replace(new RegExp("(\\" + GLOBAL_CURRENCY_DECIMAL_SEPARATOR + ")", "g"), ".") + '';
    return result;
}

//parse currency from formatted value by culture, such as from 123,45.67 to 12345,67 in german culture
function I18nParseCurrencyForCulture(formattedCurrency){
    var result = I18nParseCurrencyForJS(formattedCurrency); // refers to I18nParseCurrencyForJS
    return result.toString().replace(new RegExp("(\\.)", "g"), GLOBAL_CURRENCY_DECIMAL_SEPARATOR);
}

//Format number to I18n style with group symbol, such as from 12345.67 to $12,345.67
function I18nFormatNumberToCurrency(num) {
    if (num == null
        || num.replace(/ /g, '') == ''
        || typeof (GLOBAL_SERVICE_PROVIDER_CULTURE) == "undefined"
        || typeof (GLOBAL_CURRENCY_SYMBOL) == "undefined"
        || typeof (GLOBAL_CURRENCY_GROUP_SEPARATOR) == "undefined"
        || typeof (GLOBAL_CURRENCY_DECIMAL_SEPARATOR) == "undefined"
       ) {
        return num;
    }

    if (num < 0) {
        num = 0 - num;
    }

    num = Math.floor(num * 100 + 0.50000000001);
    var cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10) {
        cents = "0" + cents;
    }
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++) {
        num = num.substring(0, num.length - (4 * i + 3)) + GLOBAL_CURRENCY_GROUP_SEPARATOR +
            num.substring(num.length - (4 * i + 3));
    }
    var newNumString = num + GLOBAL_CURRENCY_DECIMAL_SEPARATOR + cents;
    var result = GLOBAL_CURRENCY_PATTERN != null && GLOBAL_CURRENCY_PATTERN.indexOf("{0}") != -1 ? GLOBAL_CURRENCY_PATTERN.replace("{0}", newNumString) : newNumString;
    result = result == GLOBAL_CURRENCY_PATTERN ? newNumString : result;
    return result;
}

// adjust the IFrame height according to the page height.
function AdjustHeight(newHeight)
{
    if(typeof(newHeight)=="undefined" || parseFloat(newHeight)<=0)
    {
        return false;
    }
    if(window.parent!=window)
    {
        var ifrm = getParentDocument().getElementById("ACAFrame");
        var h=newHeight;
        if(ifrm != null && ifrm.contentWindow==window)
        {
            if(isFireFox() == false)
            {
                ifrm.style.height = ifrm.parentNode.style.height = h +"px";
            }
            else
            {
                ifrm.height = h;
            }
            return true;
        }
    }
    return false;
}

//disable/enable a button
function DisableButton(buttonId, disabled) {

    if (!buttonId) {
        return;
    }

    var button = $get(buttonId);

    if (!button) {
        return;
    }

    if (button.tagName.toUpperCase() == 'A') {

        if (disabled && button.getAttribute('href') != null && typeof (button.getAttribute('href')) != 'undefined') {
            if (button.getAttribute('href_disabled') == null || typeof (button.getAttribute('href_disabled')) == 'undefined') {
                button.setAttribute('href_disabled', button.getAttribute('href'));
            }

            button.setAttribute('href', 'javascript:void(0);');
        }
        else if (button.getAttribute('href_disabled') != null && typeof (button.getAttribute('href_disabled')) != 'undefined') {
            button.setAttribute('href', button.getAttribute('href_disabled'));
        }

        if (disabled && button.getAttribute('onclick') != null && typeof (button.getAttribute('onclick')) != 'undefined') {
            if (button.getAttribute('onclick_disabled') == null || typeof (button.getAttribute('onclick_disabled')) == 'undefined') {
                button.setAttribute('onclick_disabled', button.getAttribute('onclick'));
            }

            button.setAttribute('onclick', 'return false;');
        }
        else if (button.getAttribute('onclick_disabled') != null && typeof (button.getAttribute('onclick_disabled')) != 'undefined') {
            button.setAttribute('onclick', button.getAttribute('onclick_disabled'));
            button.removeAttribute("onclick_disabled");
        }
    }

    if (disabled) {
        button.setAttribute('disabled', 'disabled');
        $(button).addClass('ButtonDisabled');
    }
    else {
        button.removeAttribute('disabled', 0);
        $(button).removeClass('ButtonDisabled');
    }
}

//Disable or Enable button with container.
function SetWizardButtonDisable(btnId, disabled) {
    var $btnWithContainer = $("#"+ btnId);
    DisableButton(btnId, disabled);
    
    if ($.exists($btnWithContainer)) {
        if (disabled) {
            $btnWithContainer.parent().removeClass("ACA_LgButton ACA_LgButton_FontSize");
            $btnWithContainer.parent().addClass("ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
        }
        else {
            $btnWithContainer.parent().removeClass("ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
            $btnWithContainer.parent().addClass("ACA_LgButton ACA_LgButton_FontSize");
            
	    if (!$.global.isAdmin) {
                attachProcessLoading($btnWithContainer);
            }
       }
    }
}

function KeyValuePair() {
    var ArrayList = new Array();

    this.GetArray = function() {
        return ArrayList;
    };

    this.Add = function(key, value) {
        if (value) {
            var index = this.IndexOf(key);
            if (index > -1) {
                ArrayList[index].value = value;
            } else {
                var obj = new Object();
                obj.key = key;
                obj.value = value;
                ArrayList.push(obj);
            }
        }
    };

    this.IndexOf = function(key) {
        for (var i = 0; i < ArrayList.length; i++) {
            if (ArrayList[i].key == key) {
                return i;
            }
        }

        return -1;
    };

    this.GetByKey = function(key) {
        var index = this.IndexOf(key);
        if (index > -1) {
            return ArrayList[index].value;
        }

        return null;
    };
}

///<Summary>
///this js class is used in SPEAR form and Confirmation page to validate multiple-supported comonent
///such as multiple contacts, multiple address
///</Summary>
function ValidationHelper() {
    var SectionIDs = new Array();
    var CurrentValidationSectionID;
    var _excludedIDsInCurrentSection = [];

    this.AddSectionID = function(sectionId) {
        SectionIDs.push(sectionId);
    };

    this.SetCurrentValidationSectionID = function(sectionId, excludedControlIDs) {
        CurrentValidationSectionID = sectionId;

        if (excludedControlIDs) {
            _excludedIDsInCurrentSection = excludedControlIDs;
        } else {
            _excludedIDsInCurrentSection = [];
        }
    };

    this.IsNeedValidate = function(controlId) {
        if (SectionIDs.length == 0) {
            return true;
        }

        // Always vlidate the field if user input a invalid value and left it.
        var e = getEvent();

        if (e != null) {
            if (e.type == "blur") {
                return true;
            }
        }

        if (CurrentValidationSectionID == null || CurrentValidationSectionID == '')//don't need to validate the field in editor form
        {
            if (_excludedIDsInCurrentSection.contains(controlId)) {
                return false;
            }

            for (var i = 0; i < SectionIDs.length; i++) {
                if (controlId.indexOf(SectionIDs[i]) > -1) {
                    return false;
                }
            }

            return true;
        } else// validate the fields of section which indicated by "CurrentValidationSectionID"
        {
            var isInCurrentSection = controlId.indexOf(CurrentValidationSectionID) > -1;
            var isExcludedControl = _excludedIDsInCurrentSection.contains(controlId);

            return isInCurrentSection && !isExcludedControl;
        }
    };
}

var ValidationHelperInstance = null;

//Add the section ID which need to support multiple
function AddValidationSectionID(sectionID) {
    if (!ValidationHelperInstance) {
        ValidationHelperInstance = new ValidationHelper();
    }
    ValidationHelperInstance.AddSectionID(sectionID);
}

//Set current validation section ID to indicate current only validate that section
function SetCurrentValidationSectionID(sectionID, excludedControlIDs) {
    if (!ValidationHelperInstance) {
        ValidationHelperInstance = new ValidationHelper();
    }

    ValidationHelperInstance.SetCurrentValidationSectionID(sectionID, excludedControlIDs);
}

//check if the control need to validate
function IsNeedValidation(controlId) {
    if (ValidationHelperInstance) {
        return ValidationHelperInstance.IsNeedValidate(controlId);
    }

    return true;
}

function getElementTop(obj) {
    var top = obj.offsetTop;

    var parentobj = obj.offsetParent;
    while (parentobj) {
        if (parentobj.offsetTop == undefined) break;
        top += parentobj.offsetTop;
        parentobj = parentobj.offsetParent;
    }

    return top;
}

//only validate section in SPARE Form.
function Section_ClientValidate(sectionID, validationGroup) 
{
    //if control array length is not 0.
    if (typeof(Page_Validators) != "undefined" && Page_Validators !=  null && Page_Validators.length > 0) 
    {
        for (var i = 0; i < Page_Validators.length; i++)
        {
            var controlValidator = Page_Validators[i];
            if (controlValidator!= null && controlValidator.id != null && controlValidator.id.indexOf(sectionID) != -1 && typeof(ValidatorValidate) != "undefined") {
                var $validateControl = $("#" + controlValidator.controltovalidate);

                if ($validateControl.attr("ValidateDisabledControl") == "Y") {
                    continue;
                }
               
                ValidatorValidate(controlValidator, validationGroup, null);

                if (controlValidator.id.indexOf("strRequiredMK_") == 0) {
                    // Expression Validator is always displayed
                    controlValidator.style.visibility = "visible";
                }
            }
        }
    }
}

function Field_ClientValidate(controlId, validationGroup) {
    if (typeof (Page_Validators) != "undefined" && Page_Validators != null && Page_Validators.length > 0) {
        for (var i = 0; i < Page_Validators.length; i++) {
            var controlValidator = Page_Validators[i];

            if (controlValidator && controlValidator.controltovalidate && controlValidator.controltovalidate == controlId && typeof (ValidatorValidate) != "undefined") {
                ValidatorValidate(controlValidator, validationGroup, null);
            }
        }
    }
}

function getElementLeft(obj)
{
    var left = obj.offsetLeft;
    
    var parentobj = obj.offsetParent;
    while (parentobj)
    {
        if (parentobj.offsetLeft == undefined) {
            break;
        }
        left += parentobj.offsetLeft;
        parentobj = parentobj.offsetParent;
    }
    return left;
}

function getEvent() {
    if (document.all) {
        return window.event;
    }

    var func = getEvent.caller;
    var MAX_LOOPS = 20;
    var index = 0;

    while (func != null) {
        /*
        End the current loop if the loop count is greater than the MAX loop count.
        The specified logic is just for sloving the endless loop when the function "getEvent" is called by the system event "endRequest".
        */
        if (index >= MAX_LOOPS) {
            break;
        }

        var arg = func.arguments[0];
        if (arg) {
            if ((arg.constructor == Event || arg.constructor == MouseEvent) || (typeof (arg) == "object" && arg.preventDefault && arg.stopPropagation)) {
                return arg;
            }
        }

        func = func.caller;
        index++;
    }

    return null;
}

//Break word of text of HTML element in IE, Opera, Safari and Firefox.
function breakWord(element) {
    if (!element) {
        return false;
    }
    else if (element.currentStyle && typeof element.currentStyle.wordBreak === 'string') {
        breakWord = function(element) {
            //For Internet Explorer
            element.runtimeStyle.wordBreak = 'break-all';
            return true;
        };

        return breakWord(element);
    }
    else if (document.createTreeWalker) {
    var trim = function (str) {
        str = str.replace(/^\s\s*/, '');
        var ws = /\s/;
        var i = str.length;
        while (ws.test(str.charAt(--i)));
        return str.slice(0, i + 1);
    };

        breakWord = function(element) {
            //For Opera, Safari, and Firefox
            var dWalker = document.createTreeWalker(element, NodeFilter.SHOW_TEXT, null, false);
            //'8203' is line break.
            var node, s, c = String.fromCharCode('8203');

            while (dWalker.nextNode()) {
                node = dWalker.currentNode;
                //we need to trim String otherwise Firefox will display 
                //incorect text-indent with space characters
                s = trim(node.nodeValue).split('').join(c);
                node.nodeValue = s;
            }

            return true;
        };

        return breakWord(element);
    }
    else {
        return false;
    }
}

//check spell
function DoSpellCheck(relativePath, elementId, e) {
    var height = 200;
    var width = 310;

    var element = document.getElementById(elementId);

    if ($(element).attr('disabled')) {
        return;
    }

    var left;
    var top;

    if (e) {
        left = e.screenX;
        top = e.screenY;
    }

    if (this.screen.availHeight < top + height + 80) {
        top -= (element.offsetHeight + height + 100);

        if (Sys.Browser.name == "Safari") {
            top -= 25;
        }
    }

    if (relativePath.length > 0 && relativePath.substr(relativePath.length - 1, 1) != '/') {
        relativePath += '/';
    }
    
    var wind = window.open(relativePath + 'SpellCheck/SpellChecker.aspx?id=' + elementId, elementId, "height=" + height + ",width=" + width + ",left=" + left + ",top=" + top + ",location=yes,menubar=no,resizable=no,scrollbars=no,status=yes,titlebar=yes,toolbar=no");
    wind.focus();
}

function JsonDecode(json) {
    if (!json || json == "") {
        return '';
    }

    return json.replace(/&#92;r&#92;n/g, "\r\n").replace(/&#92;n/g, "\n").replace(/&#91;/g, "[").replace(/&#93;/g, "]").replace(/&#123;/g, "{").replace(/&#125;/g, "}").replace(/&quot;/g, "\"").replace(/&acute;/g, "´").replace(/&#39;/g, "'").replace(/&sbquo;/g, ",").replace(/&lt;/g, "<").replace(/&gt;/g, ">").replace(/&#47;/g, "/").replace(/&#92;/g, "\\").replace(/&#94;/g, "^").replace(/&#42;/g, "*");
}

function JsonEncode(json) {
    if (json == null || json == undefined || json == '') {
        return '';
    }

    return json.replace(/\[/g, "&#91;").replace(/\]/g, "&#93;").replace(/\{/g, "&#123;").replace(/\}/g, "&#125;").replace(/\"/g, "&quot;").replace(/\´/g, "&acute;").replace(/\'/g, "&#39;").replace(/\,/g, "&sbquo;").replace(/\</g, "&lt;").replace(/\>/g, "&gt;").replace(/\//g, "&#47;").replace(/\\/g, "&#92;").replace(/\^/g, "&#94;").replace(/\*/g, "&#42;");
}

String.IsNullOrEmpty = function(value) {
    var isNullOrEmpty = false;

    if (value == null || value == "") {
        isNullOrEmpty = true;
    }

    return isNullOrEmpty;
};

// Sample: GetSiblingNode("ddl_id","div_id","div[attr='contact']");
function GetSiblingNode(sourceId,targetId, pattern,type) {
    var parentDom = null;
    var targetDom = null;
    var nodeType = type || "div";

    if (typeof(sourceId) =="string") {
         $("#"+sourceId).parents(pattern).each(function () {
            parentDom=this;
            return false;
        });
    }else{
        $(sourceId).parents(pattern).each(function () {
            parentDom=this;
            return false;
        });
    }
    
    if (parentDom != null) {
        $(parentDom).find(nodeType).each(function () {
            if (this.id == targetId) {
                targetDom=this;
                return false;
            }
        });
    }
    
    return targetDom;
}

function expandComment(obj, oLink, lnkObj, title)
{
    if (obj == 'undefined') {
        return;
    }

    var patt = "[attr='condition_notice']";
    if (typeof(oLink) == "undefined") {
        oLink = "lnkToggle" + obj;
    }
  
    var div = null;
    
    if ($("#"+obj).length != 1) {
        div = GetSiblingNode(oLink,obj,patt);
    }else{
        div = $get(obj);
    }
    
    if (typeof(oLink) == "string") {
        oLink = $get(oLink);
    }

    if (typeof (lnkObj) == "string") {
        lnkObj = $get(lnkObj);
    }
    
    if (div.style.display == "none")
    {
        div.style.display = "";
        Expanded(oLink, getText.global_js_section_ETreeTop, getText.global_js_section_altCollapsed);
        AppendTitle(lnkObj, getText.global_js_section_altCollapsed, title);
    }
    else
    {
        div.style.display = "none";
        Collapsed(oLink, getText.global_js_section_CTreeTop, getText.global_js_section_altExpanded);
        AppendTitle(lnkObj, getText.global_js_section_altExpanded, title);
    }
}

//Invoke click event for control
function invokeClick(element, executeHrefScripts4Anchor) {
    if (arguments.length > 1 && executeHrefScripts4Anchor == true && element.tagName == "A" && element.getAttribute("href").indexOf("javascript:") != -1) {
        var onclickString = element.getAttribute("onclick");
        eval(onclickString);
        var hrefString = element.getAttribute("href").replace("javascript:","");
        eval(hrefString);
    }
    else if(element){
        if (element.click) {
            element.click();
        }
        else if (element.fireEvent) {
            element.fireEvent('onclick');
        }
        else if (document.createEvent) {
            var evt = document.createEvent("MouseEvents");
            evt.initEvent("click", true, true);
            element.dispatchEvent(evt);
        }
    }
}

//Expand status
function Expanded(obj, src, alt){
    if (obj){
        obj.src = src;
        obj.alt = alt;
    }
}

//Collapse status
function Collapsed(obj, src, alt){
    if (obj){
        obj.src = src;
        obj.alt = alt;
    }
}

function AppendTitle(obj, alt, title) {
    if (obj) {
        obj.title = alt;
        if (title) {
            obj.title = alt + " " + title;
        }
    }
}

function AddTitle(obj, alt, lblValue) {
    if (obj) {
        obj.title = alt;
        if (lblValue && lblValue.innerHTML != "") {
            obj.title = RemoveHTMLFlag(alt + " " + lblValue.innerHTML);
        }
    }
}

function ExpandOrCollapseSection(objId, imgObjId, lnkObjId, lblValue) {
    var obj = $("#"+objId);
    var imgObj = $("#"+imgObjId);
    var lnkObj = $("#"+lnkObjId);
        
    if (obj.is(":visible")) {
        Collapsed(imgObj[0], getText.global_js_section_CTreeTop, getText.global_js_section_altExpanded);
        AddTitle(lnkObj[0], getText.global_js_section_altExpanded, lblValue);
        obj.hide();
    }
    else {
        Expanded(imgObj[0], getText.global_js_section_ETreeTop, getText.global_js_section_altCollapsed);
        AddTitle(lnkObj[0], getText.global_js_section_altCollapsed, lblValue);
        obj.show();
    }
}

// section header manager
var SectionHeaderManager = function(sectionHeaderID) {
    this.sectionHeader = $get(sectionHeaderID + "_divSectionHeader");
    this.collapsible = this.sectionHeader ? this.sectionHeader.getAttribute("collapsible").toUpperCase() == "TRUE" : false;
    this.collapsed = this.sectionHeader ? this.sectionHeader.getAttribute("collapsed").toUpperCase() == "TRUE" : false;
    this.sectionBody = this.sectionHeader ? $get(this.sectionHeader.getAttribute("sectionBodyID")) : null;
    this.titleLabel = this.sectionHeader ? $get(this.sectionHeader.getAttribute("titleLabelID")) : null;
    this.instruction = this.sectionHeader ? $get(this.sectionHeader.getAttribute("instructionID")) : null;
    this.instructionHiddenCssClass = this.instruction ? this.instruction.getAttribute("HiddenCssClass") : null;
    this.instructionShownCssClass = this.instruction ? this.instruction.getAttribute("ShownCssClass") : null;
    this.link = this.sectionHeader ? $get(this.sectionHeader.getAttribute("linkID")) : null;
    this.image = this.sectionHeader ? $get(this.sectionHeader.getAttribute("imageID")) : null;
    this.collapsedImageURL = this.image ? this.image.getAttribute("CollapsedImageURL") : null;
    this.expandedImageURL = this.image ? this.image.getAttribute("ExpandedImageURL") : null;
    this.collapsedAltText = this.image ? this.image.getAttribute("CollapsedAltText") : null;
    this.expandedAltText = this.image ? this.image.getAttribute("ExpandedAltText") : null;
};

//toggle method of section header manager
SectionHeaderManager.prototype.toggle = function(toCollapseExplicitly) {
    if (this.sectionHeader && this.collapsible) {
        var toCollapse = !(this.sectionHeader.getAttribute("collapsed").toUpperCase() == "TRUE");
        toCollapse = typeof(toCollapseExplicitly) != "undefined" ? toCollapseExplicitly : toCollapse;
        this.sectionHeader.setAttribute("collapsed", toCollapse.toString());
        var targetAltText = toCollapse ? this.expandedAltText : this.collapsedAltText;
        var targetImageURL = toCollapse ? this.collapsedImageURL : this.expandedImageURL;

        //set instruction
        if (this.instruction) {
            var toggleCssClass = toCollapse == true ? this.instructionHiddenCssClass : this.instructionShownCssClass;
            this.instruction.setAttribute("className", toggleCssClass); //for IE 7 className
            this.instruction.setAttribute("class", toggleCssClass);

            if (this.instruction.innerHTML == "") {
                this.instruction.setAttribute("className", this.instructionHiddenCssClass); //for IE 7 className
                this.instruction.setAttribute("class", this.instructionHiddenCssClass);
            }
        }

        //set section body
        if (this.sectionBody) {
            var sectionBodyVisible = !toCollapse;
            Sys.UI.DomElement.setVisible(this.sectionBody, sectionBodyVisible);

            if (sectionBodyVisible && Sys.UI.DomElement.containsCssClass(this.sectionBody, "ACA_Hide")) {
                Sys.UI.DomElement.removeCssClass(this.sectionBody, "ACA_Hide");
            } else if (!sectionBodyVisible && !Sys.UI.DomElement.containsCssClass(this.sectionBody, "ACA_Hide")) {
                Sys.UI.DomElement.addCssClass(this.sectionBody, "ACA_Hide");
            }
        }

        //set image
        if (this.image) {
            this.image.src = targetImageURL;
            this.image.alt = targetAltText;
        }

        //set link
        if (this.link) {
            AddTitle(this.link, targetAltText, this.titleLabel);
        }
    }
};

//collapse method of section header manager
SectionHeaderManager.prototype.collapse = function() {
    this.toggle(true);
};

//expand method of section header manager
SectionHeaderManager.prototype.expand = function() {
    this.toggle(false);
};


/*
*	Get byte length for UTF-8 coded chars.
*/
function getByteLength4String(value) {
    var totalLength = 0;
    var charCode;

    for (var i = 0; i < value.length; i++) {
        charCode = value.charCodeAt(i);

        if (charCode < 0x007f) {
            totalLength++;
        }
        else if ((0x0080 <= charCode) && (charCode <= 0x07ff)) {
            totalLength += 2;
        }
        else if ((0x0800 <= charCode) && (charCode <= 0xffff)) {
            totalLength += 3;
        }
        else {
            totalLength += 4;
        }
    }

    return totalLength;
}

// show message for sectino 508
function showMessageForSection508(message) {
    var isAccessibilityEnabled = typeof (accessibilityEnabled) == "function" ? accessibilityEnabled() : false;
    if (isAccessibilityEnabled) {
        alert($("<span>" + message + "</span>").text());
    }
}

//skip to target for section 508, iframeID parameter is for cross iframe invocation.
function skipTo(iframeID) {
    var expectedArgLength = skipTo.length;
    var oIframe = null;
    var oDoc = document;

    if (iframeID) {
        oIframe = window.frames[iframeID] ? window.frames[iframeID] : document.getElementById(iframeID);
        oDoc = oIframe ? oIframe.document || oIframe.contentDocument : null;
    }
    else {
        oIframe = window;
        oDoc = window.document;
    }

    var oAnchor = null;
    if (oDoc && arguments.length > expectedArgLength) {
        for (i = expectedArgLength; i < arguments.length; i++) {
            oAnchor = oDoc.getElementById(arguments[i]);
            if (oAnchor != null) {
                break;
            }
        }
    }

    var originalNeedAsk = oIframe && oIframe.NeedAsk ? oIframe.NeedAsk : null;
    if (originalNeedAsk != null) {
        oIframe.SetNotAsk(false);
    }
    if (oAnchor) {
        //oAnchor.scrollIntoView();
        oAnchor.focus();
    }
    if (originalNeedAsk != null) {
        oIframe.SetNotAsk(originalNeedAsk);
    }
}

//skip to beginning of ACA for section 508
function skipToBeginningOfACA() {
    skipTo(null, "ctl00_hlSkipToolBar", "SecondAnchorInACAMainContent");
}

//skip to main content for section 508
function skipToMainContent() {
    skipTo(null, "SecondAnchorInACAMainContent", "FirstAnchorInACAMainContent");
}

function skipToAdminMainContent() {
    var tabs = Ext.getCmp('tabs');
    var activeTab = tabs ? tabs.activeTab : null;
    if (activeTab) {
        var theIframe = activeTab.getEl().child("iframe");
        if (theIframe != null) {
            skipTo(theIframe.id, "FirstAnchorInAdminMainContent");
        }
    }
}

// true: support
// false: unsupport
function accessibilityEnabled() {
    var result = false;

    if (typeof (GLOBAL_ACCESSIBILITY_ENABLED) != "undefined") {
        result = GLOBAL_ACCESSIBILITY_ENABLED;
    }

    return result;
}

function OverrideTabKey(e, shitValue, focusId) {
    if (e.shiftKey == shitValue && e.keyCode == 9) {
        FocusObject(e, focusId);
    }

    if (typeof (SetNotAsk) != "undefined") {
        SetNotAsk(false);
    }
}

/*
Use Jquery to handle the keydown event when user press Tab key to change the Tab orders.
e - key down event.
obj - html or juqery object you want to focus on it.
*/
function OverrideTabKey2(e, obj) {
    if (e && e.which == 9 && obj) {
        obj.focus();
    }
}

function FocusObject(e, focusId) {
    if (typeof (e.preventDefault) != "undefined") {
        e.preventDefault();
    } else if (window.event) {
        window.event.returnValue = false;
    }

    var focusObj = document.getElementById(focusId);
    
    if (focusObj != null) {
        focusObj.focus();
    }
}

//Make links 'tab-able' in Opera
function SetTabIndexForOpera() {
    $("a[href][tabindex!=-1]").attr('tabindex', 0);
}

function confirmMsg(message) {
    return confirm(message.replace(/<br>|<br\/>|<br \/>/gi, '\r'));  
}

function CheckAndSetNoAsk() {
    if (typeof (SetNotAsk) != "undefined") {
        SetNotAsk(true);
    }
}

// ---Get/Set field value Form/To Watermarked fields---
function GetValue(control) {
    if (control) {
        if (isHijriCalendarControl(control.id)) {
            return $("#" + control.id).attr("gDate");
        }

        if (typeof (AjaxControlToolkit) != 'undefined' && typeof (AjaxControlToolkit.TextBoxWrapper) != 'undefined') {
            var wrapper = AjaxControlToolkit.TextBoxWrapper.get_Wrapper(control);
            if (wrapper && wrapper.get_IsWatermarked()) {
                return '';
            }
            else {
                return control.value;
            }
        }
        else {
            return control.value;
        }
    }
    else {
        return '';
    }
}

function GetValueById(id) {
    return GetValue($get(id));
}

function SetValue(control, value) {
    if (control) {
        var watermarkBhv = $find(control.id + '_watermark_bhv');
        if (watermarkBhv) {
            watermarkBhv._onFocus();
        }
	
            control.value = value;

        if (watermarkBhv) {
            watermarkBhv._onBlur();
        }
    }
}

function SetValueById(id,value) {
    SetValue($get(id), value);

    // Update expression readOnly Value
    var control = $get(id);
    if (control != null
        && control.readOnly == true
        && typeof(GetReadOnlyControlById) == "function") {

        var objControl = GetReadOnlyControlById(id);
        if (objControl != null) {
            objControl.Value = value;
        }
    }
}

function SetValueById4TemplateDDL(id, value) {
    var ddlControl = $('#' + id);

    ddlControl.children('option').each(function () {
        var thisItem = $(this);
        if (thisItem.text() == value || thisItem.val() == value) {
            ddlControl.val(thisItem.val());
        }
    });
}

function SetRadioListValue(id, value) {
    var control = $get(id);
    if (control) {
        control = control.getElementsByTagName("input");
        var optionLength = control.length;
        for (var i = 0; i < optionLength; i++) {
            control[i].checked = control[i].value == value;
        }
    }
}

//------------------------------------------------------

//remove html flag from content
function RemoveHTMLFlag(strHTML) {
    strHTML = strHTML.replace(/<\/?[^>]*>/g, ''); //remove HTML tag
    strHTML = strHTML.replace(/[ | ]*\n/g, '\n'); //remove blank content
    strHTML = strHTML.replace(/\n[\s| | ]*\r/g, '\n'); //remove null row
    return strHTML;
}

function FormatPhoneShow(countryCode, phone, isCountryCodeEnabled) {
    var result = '';

    if (phone != null && phone.length > 0) {
        if (isCountryCodeEnabled) {
            result = '(+' + countryCode + ')' + phone;
        }
        else {
            result = phone;
        }        
    }

    return result;
}

function GetValidCssClassName(str) {
    var className = str.trim().replace(/\W/g, '_');
    if (/^\d{1}.*/.test(className)) {
        className = '_' + className;
    }

    return className;
}

//usage:Similar to the string.Format function in C#
String.prototype.format = function() {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function(m, i) {
            return args[i];
        }
    );
};

//To implement startWidth function in javascript.
String.prototype.startWith = function(str) {
    var rgx = new RegExp('^' + str);
    return rgx.test(this);
};

//To implement endWidth function in javascript.
String.prototype.endWith = function(str) {
    var rgx = new RegExp(str + '$');
    return rgx.test(this);
};

// used to create collapsible element
var CollapsibleElement = function(elementID) {
    this.elementID = elementID;
    this.element = $('#' + this.elementID);
    this.isCollapsed = true;
};

CollapsibleElement.prototype.expand = function() {
    if (this.isCollapsed && this.element.hasClass('ACA_Hide')) {
        this.isCollapsed = false;
        this.element.removeClass('ACA_Hide');
    }
};

CollapsibleElement.prototype.collapse = function() {
    if (!this.isCollapsed && !this.element.hasClass('ACA_Hide')) {
        this.isCollapsed = true;
        this.element.addClass('ACA_Hide');
    }
};

CollapsibleElement.prototype.toggle = function() {
    if (this.isCollapsed) {
        this.expand();
    } else {
        this.collapse();
    }
};

// get the current checked values split by 'splitChar' which the checkbox click.
// obj: the checkbox object
// checkedValue: the current row value which the checkbox click
// existValue: the exist values
// splitChar: the split char
function GetCurrentCheckedValues(obj, checkedValue, existValue, splitChar) {
    var result = existValue;

    if ($(obj).attr('checked')) {
        if (result == null || result == '') {
            result = splitChar + checkedValue + splitChar;
        }
        else if (result.indexOf(splitChar + checkedValue + splitChar) == -1) {
            result = result + checkedValue + splitChar;
        }
    }
    else {
        if (result.indexOf(splitChar + checkedValue + splitChar) != -1) {
            result = result.replace(splitChar + checkedValue + splitChar, splitChar);

            if (result == splitChar) {
                result = '';
            }
        }
    }

    return result;
}

function EncodeHTMLTag(value) {
    if (value) {
        value = value.replace(/&/gi, "&amp;").replace(/\"/g, "&quot;").replace(/\'/g, "&#39;").replace(/\u00BB/gi, "&raquo;").replace(/\u00AB/gi, "&laquo;");
    }

    return value;
}

function DecodeHTMLTag(value) {
    if (value) {
        value = value.replace(/&lt;/gi, "<").replace(/&gt;/gi, ">").replace(/&quot;/g, "\"").replace(/&#39;/g, "\'").replace(/&raquo;/gi, "\u00BB").replace(/&laquo;/gi, "\u00AB").replace(/&nbsp;/gi, " ").replace(/&nbsp;/gi, " ").replace(/&amp;/gi, "&");
    }

    return value;
}

function SetNotAskForSPEAR() {
    if (typeof (SetNotAsk) != 'undefined') {
        SetNotAsk(true);
    }
}

function IsTrue(obj) {
    var isTrue = false;

    if (typeof (obj) == "boolean" && obj == true) {
        isTrue = true;
    } else if (typeof (obj) == "string" && obj.toUpperCase() == "TRUE") {
        isTrue = true;
    }

    return isTrue;
}

// usage: if ( ['a','b','c','d'].contains('b') ) { ... }
Array.prototype.contains = function(value) {
    for (var key in this) {
        if (this[key] === value) {
            return true;
        }
    }
    return false;
};

// usage: if ( ['a','b','c','d'].containsDuplicate() ) { ... }
// if array contain duplicate element, then return true.
Array.prototype.containsDuplicate = function(ignorCase) {

    
    if (this.duplicateIndexOf(ignorCase) > -1){
    	return true;
    }
    
    return false;
};

Array.prototype.duplicateIndexOf = function(ignorCase) {
    if (this.length < 2) {
        return -1;
    }

    var lista = this;
    var listb = this;

    for (var i = 0; i < lista.length; i++) {
        for (var j = i + 1; j < listb.length; j++) {
            if (ignorCase) {
                if (i != j && lista[i].toString().toUpperCase() == listb[j].toString().toUpperCase()) {
                    return j;
                }
            }
            else {
                if (i != j && lista[i] == listb[j]) {
                    return j;
                }
            }
        }
    }
    return -1;
};

if (!Array.prototype.indexOf) {
    //For support indexOf method in IE 8 and earlier.
    Array.prototype.indexOf = function(obj, start) {
        for (var i = (start || 0), j = this.length; i < j; i++) {
            if (this[i] === obj) {
                return i;
            }
        }
        return -1;
    };
}

function RoundDecimal(numValue, decimalLength, isNeedComma, isNeedNegative) {

    if (numValue == null || numValue.trim() == "") {
        return "";
    }

    //remove group separator
    numValue = I18nParseCurrencyForJS(numValue); // refers to I18nParseCurrency

    //replace decimal separator with dot separator
    if (GLOBAL_CURRENCY_DECIMAL_SEPARATOR.trim() != "") {
        var regex4DecimalSeparator = new RegExp("\\" + GLOBAL_CURRENCY_DECIMAL_SEPARATOR, "g");
        numValue = numValue.toString().replace(regex4DecimalSeparator, '.');
    }

    if (isNaN(numValue)) numValue = "0";
    var isNegative = false;
    var hasDecimal = false;

    if (numValue < 0) {
        numValue = 0 - numValue;
        isNegative = true;
    }

    if (decimalLength > 0) {
        if (numValue.indexOf(".") != -1) {
            hasDecimal = true;
            
            if((numValue.length - numValue.indexOf(".") - 1) > decimalLength) {
                numValue = Math.round(numValue * Math.pow(10, decimalLength)) / Math.pow(10, decimalLength);
            }
        }
    }
    
    if (isNeedComma) {
        for (var i = 0; i < Math.floor((numValue.length - (1 + i)) / 3); i++) {
            numValue = numValue.substring(0, numValue.length - (4 * i + 3)) + GLOBAL_CURRENCY_GROUP_SEPARATOR + numValue.substring(numValue.length - (4 * i + 3));
        }
    }

    numValue = isNegative && isNeedNegative ? 0 - numValue : numValue;

    if (hasDecimal) {
        return numValue.toString().replace(".", GLOBAL_CURRENCY_DECIMAL_SEPARATOR);
    }
    else {
        return numValue;
    }
}

function replaceDecimalSeparator(currentObject, event) {
    if (GLOBAL_NUMBER_DECIMAL_SEPARATOR != "." && currentObject.value.indexOf(".") != -1) {
        currentObject.value = currentObject.value.replace(/\./g, GLOBAL_NUMBER_DECIMAL_SEPARATOR);
    }
}

/*
Get own iframe for given window object.
Return iframe object or null.
*/
function getOwnIframe(w) {
    var result = null;
    var iframeObjs = getParentDocument().getElementsByTagName('iframe');

    for (var i = 0; i < iframeObjs.length; i++) {
        var fra = iframeObjs.item(i);
        if (fra.contentWindow == w) {
            result = fra;
            break;
        }
    }

    return result;
}

function addPrintErrors2SubmitButton() {
    if (!accessibilityEnabled()) {
        $("a[href*='javascript:WebForm_DoPostBackWithOptions']").each(function () {
            var funPrintError = "if (typeof(myValidationErrorPanel)!=\'undefined\') myValidationErrorPanel.printErrors();";
            var href = $(this).attr('href');

            if (href.indexOf(funPrintError) == -1) {
                $(this).attr('href', href + ';' + funPrintError);
            }
        });
    }
}

function getparmByUrl(parmName, url) {
    if (!url) {
        url = window.location.toString();
    }

    if (url.indexOf("?") == -1) {
        return "";
    }
    var arr = url.split("?");
    var parms = arr[1];
    var parmList = parms.split("&");
    var parmTemp;

    for (var i = 0; i < parmList.length; i++) {
        parmTemp = parmList[i].split("=");
        if (parmTemp[0] == parmName) {
            return parmTemp[1];
        }
    }
    return "";
}

/* --------------------------------------------------------------------------------------------
 *
 *  Common Functions to set ReadOnly Property to Accela Control(Starts)
 * 
 * ------------------------------------------------------------------------------------------- */
function SetFieldToDisabled(id, isReadOnly) {
    var oField = $get(id); 
    if (oField == null) {
        return;
    }

    isReadOnly = GetReadonlyStatus(oField, isReadOnly);
    var fieldType = oField.type;

    if (typeof (fieldType) == "undefined") {
        SetRadioButtonListToDisable(oField, isReadOnly);
    }
    else {
        SetSingleFieldToDisabled(id, isReadOnly);
    }
}

function SetRadioButtonListToDisable(radioList, isReadOnly) {
    SetSingleFieldToDisabled(radioList.id, isReadOnly);
    var radios = radioList.getElementsByTagName("input");

    for (var j = 0; j < radios.length; j++) {
        var radio = radios[j];
        SetSingleFieldToDisabled(radios[j].id, isReadOnly);
    }
}

function GetReadonlyStatus(ctr, isReadOnly) {
    /*
    The "data-editable" attribute:
    Usage 1:
        People template has a Always Editable setting,
            so if "data-editable" is "true", means the field always keep the Editable status.
    Usage 2:
        Standard field can be set as Always Readonly in ACA Admin, such as Contact Edit form,
            so if "data-editable" is "false", means the standard field always keep the Readonly status.
    */

    var editable = $(ctr).attr("data-editable");
    
    if (typeof (editable) != "undefined") {
        if (editable == "true") {
            isReadOnly = false;
        } else {
            isReadOnly = true;
        }
    }
    
    return isReadOnly;
}

function SetSingleFieldToDisabled(id, isReadOnly) {

    var control = $("#" + id);

    if (isReadOnly) {
        control.addClass("ACA_ReadOnly");
        control.attr("readonly", true);
    }
    else {
        control.removeClass("ACA_ReadOnly");
        control.removeAttr("readonly");
    }

    var oField = $get(id);
    if (oField == null) {
        return;
    }

    var fieldType = oField.type;

    if (typeof (fieldType) == "undefined") {
        fieldType = "RADIO";
    }
    else {
        fieldType = fieldType.toUpperCase();
    }
    
    // 1. Dropdown List
    if (fieldType.indexOf("SELECT-ONE") > -1) {
        // Delete attribute 'disabled' maybe exist in Select control.
        control.removeAttr('disabled');

        insertReadOnlyShade(oField, isReadOnly);

        // Disable or enable all options except the selected one
        var options = oField.options;
        for (var i = 0; i < options.length; i++) {

            if (i != oField.selectedIndex && isReadOnly) {
                $(options[i]).attr('disabled', 'disabled');
            } else {
                $(options[i]).removeAttr('disabled');
            }
        }
    }
    //2. Radio and Check Box
    else if (fieldType == "RADIO" || fieldType == "CHECKBOX") {
        if (fieldType == "RADIO") {
            // Special Case(For ASI/ASIT): If radio is disabled before, its parent node is not Div element. 
            var radioField = oField;
            if (radioField.disabled == true) {
                radioField = radioField.parentNode;
            }

            if (isReadOnly) {
                $(radioField).addClass("ACA_ReadOnly");
            } else {
                $(radioField).removeClass("ACA_ReadOnly");
            }

            insertReadOnlyShade(radioField, isReadOnly);
        }
        else {

            if (isReadOnly) {
                $("label[for='" + id + "']").attr("disabled", "disabled");
            } else {
                $("label[for='" + id + "']").removeAttr("disabled");
            }

            insertReadOnlyShade(oField, isReadOnly);
        }

        // Prevent click or keydown event for CheckBox and Radio
        disableClickAndKeyDown4RadioCheckBox(id, isReadOnly);
    }
    else {
        // 3. Calendar, Disable or enable calendar image.
        var calendar = $find(id + "_calendar_bhv");
        if (calendar != null) {
            calendar.set_enabled(!isReadOnly);

            var calendarButton = calendar.get_button();

            // Set enabled status for calendar button
            if (calendarButton != null) {
                DisableButton(calendarButton.id, isReadOnly);
            }
        }
    }
}

function insertReadOnlyShade(control, isReadOnly) {
    if (isReadOnly) {
        $readonlyObj.enable(control);
    } else {
        $readonlyObj.remove(control);
    }
}

function disableClickAndKeyDown4RadioCheckBox(id, isReadOnly) {

    if (isReadOnly) {
        $("#" + id).bind('keydown', disableKeyDown4RadioCheckBox);
        $("#" + id).bind('click', disableClick4RadioCheckBox);
    }
    else {
        $("#" + id).unbind('keydown', disableKeyDown4RadioCheckBox);
        $("#" + id).unbind('click', disableClick4RadioCheckBox);
    }
}

function disableKeyDown4RadioCheckBox(e) {
    var keyCode = e.keyCode || e.which;

    if (keyCode == 32 || keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40) {
        return false;
    }
}

function disableClick4RadioCheckBox(e) {
    return false;
}

/***********************************************************************************************
*
* Func: $.readonly
* Desc: set the readio button to readonly
* Para: o-- the radio button parent node (div)
***********************************************************************************************/
var $readonlyObj = {
    getE: function (o) {
        if (typeof (o) == "string") {
            return document.getElementById(o);
        }
        return o;
    },

    shade: function (div, o) {
        div.addClass('ReadonlyShade');

        /*
        The control may be hidden in initial status.
        In order to get the correct Width/Height, need to set the control as visible first,
        and then restore the control display status after got the correct Width/Height.
        */

        var allHiddenParents;
        var hiddenParentsDisplayValues;
        var $o = $(o);

        if (!$o.is(':visible')) {
            allHiddenParents = $o.parents(':hidden');
            hiddenParentsDisplayValues = [];

            allHiddenParents.each(function () {
                /*
                If there is a css style made the element hidden, when use $(element).css('display') method to get the style.display attribute,
                will get the 'none' value, the 'none' value is not the really value of style.display, so use element.style.display method to get the really value.
                */
                var display = this.style ? this.style.display : '';
                hiddenParentsDisplayValues.push(display);
            });

            allHiddenParents.show();
        }

        div.css('width', o.offsetWidth + 2 + "px");
        div.css('height', o.offsetHeight + 2 + "px");

        //Restore the display status of the parent controls.
        if (allHiddenParents && hiddenParentsDisplayValues) {
            allHiddenParents.each(function () {
                $(this).css('display', hiddenParentsDisplayValues.shift());
            });
        }

        div.css('opacity', '0');
        div.css('filter', 'alpha(opacity:0)');
    },

    enable: function (o) {
        o = this.getE(o);
        if (!o) {
            return;
        }

        var id = o.id;
        if (id == "") {
            id = o.firstChild.id;
        }

        // check if the shade div is exist
        var tmpDiv = document.getElementById(id + "_div");
        if (tmpDiv) {
            this.shade($(tmpDiv), o);
            return;
        }

        var shadeDiv = $("<div id='" + id + "_div" + "'></div>");
        $(shadeDiv).insertBefore(o);
        this.shade(shadeDiv, o);

        if ($(o).parent().hasClass("ReadOnlyPosition") == false) {
            //Parent element must define the Position to prevent the shade position issue, see bug#49325.
            $(o).parent().children().wrapAll("<div class='ReadOnlyPosition ACA_ReadOnly'></div>");
        }
    },

    remove: function (o) {
        o = this.getE(o);
        if (!o) {
            return;
        }

        var id = o.id;
        if (id == "") {
            id = o.firstChild.id;
        }

        var shadeDiv = document.getElementById(id + "_div");
        if (shadeDiv) {

            if ($(shadeDiv).parent().hasClass("ReadOnlyPosition")) {
                $(shadeDiv).unwrap();
            }

            $(shadeDiv).remove();
        }
    }
};

/* --------------------------------------------------------------------------------------------
*
*  Common Functions to set ReadOnly Property to Accela Control(Ends)
* 
* ------------------------------------------------------------------------------------------- */

// set the Accela Control's tip
// id: control's id
// requiredText: if the control is required, set the required text
function setAccelaControlTip(id, requiredText) {
    var tip = $('#' + id + '_label_1').html();  // field label
    var watermark;
    var watermarkBhv = $find(id + '_watermark_bhv');

    if (watermarkBhv != null) {
        watermark = watermarkBhv.get_WatermarkText();
    }

    if (requiredText != undefined && requiredText != null && requiredText.length > 0) {
        tip = tip + ' ' + requiredText;
    }
    if (watermark != undefined && watermark != null && watermark.length > 0) {
        tip = tip + ' ' + watermark;
    }

    $('#' + id).attr('title', tip);
}

function scrollIntoView(objetID) {
    if (typeof (iframeAutoFit) != "undefined") {
        iframeAutoFit();
    }

    var object = document.getElementById(objetID);

    if (object != null) {
        //To resolve the Location issue on Chrome and Safari.
        var ifrm = getParentDocument().getElementById("ACAFrame");
        var divmask = null;
        var isPop = typeof(isPopup) != "undefined" && isPopup();

        //Add the special logic to fix the dialog UI issue.
        if (isPop) {
            var divmask = getParentDocument().getElementById(ACADialog.maskId);
            if (divmask != null && divmask.style.display != "none") {
                originalDisplay = divmask.style.display;
                divmask.style.display = "none";
            }
        }

        // If current page is a popup dialog, cannot use the method scrollIntoView in IE.
        if ($.browser.msie && !isPop) {
            object.scrollIntoView();
        }
        else {
            // To resolved the browsers can not location to the element which need to focus.
            try {
                var offset = $(object).offset();
                var win = window.parent;

                if (isCrossDomain() || isPop) {
                    win = window;
                }

                win.scrollTo(offset.left, offset.top);
            }
            catch (e) {
            }
        }

        if (isPop && divmask != null) {
            divmask.style.display = "";
        }
    }
}

function getParentDocument() {
    var parentDoc;
    if (isCrossDomain()) {
        parentDoc = window.document;
    } else {
        parentDoc = parent.document;
    }

    return parentDoc;
}

function getRootDocument4Popup() {
    var parentDoc;
    if (isCrossDomain()) {
        parentDoc = window.document;
    } else {
        var ancestor = parent;
        
        while (ancestor && ancestor.location && ancestor.location.href.indexOf("isPopup") > 0) {
            //if the root page has parameter "isPopup", it will always loop.
            if (ancestor == ancestor.parent) {
                break;
            }
            
            ancestor = ancestor.parent;
        }
        parentDoc = ancestor.document;
    }

    return parentDoc;
}

function getTopDocument() {
    var topDoc;
    if (isCrossDomain()) {
        topDoc = document;
    }
    else {
        topDoc = top.document;
    }

    return topDoc;
}

function isCrossDomain(win) {
    var result = false;
    try {
        var parentBody;

        if (win) {
            parentBody = win.parent.document.body;
        } else {
            parentBody = window.parent.document.body;
        }
        if (parentBody != null) {
            result = false;
        }
    }
    catch (e) {
        result = true;
    }

    return result;
}

//Get radio button list selected value.
function GetRadioButtonSelectedValue(radioButtonControl) {
    if (radioButtonControl) {
        radioButtonControl = radioButtonControl.getElementsByTagName("input");

        if (radioButtonControl) {
            var optionLength = radioButtonControl.length;

            for (var i = 0; i < optionLength; i++) {
                if (radioButtonControl[i].checked == true) {
                    return radioButtonControl[i].value;
                }
            }
        }
    }

    return '';
}

function AdjustTabList(tabId) {
    var tabList = document.getElementById(tabId);
    if (tabList) {
        var rowCount = tabList.rows.length;

        if (rowCount > 1 && !tabList.rows[rowCount - 1].cells[1].innerHTML) {
            tabList.rows[rowCount - 1].cells[1].style.display = 'none';
        }
    }
}

function print_onclick(url) {
    hideMessage();
    var a = window.open(url, "_blank", "top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
}

function ToUpperCase(txtControl) {
    var control = $(txtControl);
    control.val(control.val().toUpperCase());
}

//Enable or disable Soundex Search.
function SwitchSoundexSearch(isChecked, parentId) {
    var soundexFields = null;

    if (parentId) {
        soundexFields = $("#" + parentId).find("span[name$='_soundex']");
    }
    else {
        soundexFields = $("span[name$='_soundex']");
    }

    if (isChecked) {
        soundexFields.show();
    }
    else {
        soundexFields.hide();
    }
}

var delayShowLoadingTimer;
function delayShowLoading() {
    if ($('#divGlobalLoading').is(":not(:visible)")) {
        if (Sys.WebForms.PageRequestManager != null && Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
            var p = new ProcessLoading();
            p.showLoading();
        }
    } else {
        delayShowLoadingTimer = window.setTimeout(function () {
            delayShowLoading();
        }, 100);
    }
}

function ShowSectionRequiredMsg(containerId, errorMessage, imgUrl) {
    var container = $('#' + containerId);
    container.addClass('ACA_Label ACA_Label_FontSize');
    container.html('<div class="ACA_Error_Label"><table role="presentation"><tr><td><div class="ACA_Error_Indicator_In_Same_Row"><img class="ACA_NoBorder" alt="" src=' + imgUrl + ' /></div></td><td>' + errorMessage + '</td></tr></table></div>');

    if (containerId != '' && errorMessage != '') {
        goToMessageBlock(containerId);
    }

    $(document).ready(function() {
        //Delay 1 ms to hide the 'please wait' dialog before the pop the modal dialog (e.g.: alert).
        window.setTimeout(function() { showMessageForSection508(errorMessage); }, 1);
    });
}

function CallPostBackFunction(uniqueID) {
    var p = new ProcessLoading();
    p.showLoading();
    __doPostBack(uniqueID, '');

    return false;
}

function TriggerEventWithConfirm(btnUniqueID, confirmMessage) {
    if (confirmMsg(confirmMessage)) {
        CallPostBackFunction(btnUniqueID);
    }

    return false;
}

function getElementTop(obj) {
    var top = obj.offsetTop;
    var parentobj = obj.offsetParent;

    while (parentobj) {
        if (parentobj.offsetTop == undefined) {
            break;
        }
        top += parentobj.offsetTop;
        parentobj = parentobj.offsetParent;
    }
    return top;
}

function getElementLeft(obj) {
    var left = obj.offsetLeft;
    var parentobj = obj.offsetParent;

    while (parentobj) {
        if (parentobj.offsetLeft == undefined) {
            break;
        }
        left += parentobj.offsetLeft;
        parentobj = parentobj.offsetParent;
    }
    return left;
}

function getElementsLeftWithControlWidth(obj) {
    var left = getElementLeft(obj);

    //in Arabic, get the right width
    if ($.global.isRTL) {
        left = obj.ownerDocument.body.offsetWidth - left;
    }
    else {
        left += obj.offsetWidth;
    }

    return left;
}

function isNull(obj) {
    if (typeof (obj) == 'undefined' || obj == null) {
        return true;
    }
    return false;
}

function isNullOrEmpty(obj) {
    return isNull(obj) || obj.trim() == '';
}

function SelectAll(selectAllObj, selectItems) {
    var selectAllChecked = selectAllObj.checked;
    selectItems.each(function () {
        $(this).attr("checked", selectAllChecked);
    });
}

function CheckSilverlightInstalled() {
    var isSilverlightInstalled = false;

    try {
        var slControl = new ActiveXObject('AgControl.AgControl');
        isSilverlightInstalled = true;
    } catch (e) {
        if (navigator.plugins["Silverlight Plug-In"]) {
            isSilverlightInstalled = true;
        }
    }

    return isSilverlightInstalled;
}

// update the select all check box status
function UpdateStatus4SelectAll($selectAllObj, $childOptions) {
    if ($selectAllObj == null || $selectAllObj.length == 0) {
        return;
    }

    var isCheckAll = true;

    if ($childOptions == null || $childOptions.length == 0) {
        isCheckAll = false;
    } else {
        var $unCheckOptions = $childOptions.not(":checked");

        if ($unCheckOptions.length > 0) {
            isCheckAll = false;
        }
    }

    $selectAllObj.attr("checked", isCheckAll);
}

//Add a title to describe what will happen if current page will be refreshed after the current field has been changed.
function UpdateTextboxToolTip(textClientID, textLabel) {
    var toolTip = $('#' + textClientID).attr('title') ;
    var textKey = textLabel;
    var noticeTip = getText.global_js_changevalue_tip;

    if (!isNullOrEmpty(textKey) && textKey[textKey.length - 1] == ':') {
        textKey = textLabel.substr(0, textLabel.length - 1);
    }
    
    toolTip = toolTip + " " + noticeTip.replace('{0}', textKey);
    $('#' + textClientID).attr("title", toolTip);
}

function isNumber(value) {
    var patt = /^[\d]+$/;
    return patt.test(value);
}

function triggerPreventDefault(e, keyCode) {
    var evt = window.event || e;
    var keynum = evt.keyCode || evt.which;

    if (keynum == keyCode) {
        // Prevent to fire other onkey events such as popup in Global Search.
        if (typeof (e.preventDefault) != "undefined") {
            e.preventDefault();
        } else if (window.event) {
            window.event.returnValue = false;
        }

        return true;
    }

    return false;
}

var ACAGlobal = {};
ACAGlobal.Dialog = new function() {
    this.adjustPosition = adjustPosition;

    function adjustPosition(container, popupMask, unit, opts) {
        opts = opts || {};
        var win = window.top;

        //check win is cross domain
        if (isCrossDomain()) {
            win = window;
        }

        var ifra = getOwnIframe(window);
        var isPopup = window.location.href.indexOf('isPopup') > -1;

        if (isPopup && ifra) {
            win = ifra.contentWindow;
        }

        var clientWidth;
        var clientHeight;
        var scrollTop;
        var scrollHeight;

        if (win.document.compatMode == "BackCompat") {
            clientWidth = win.document.body.clientWidth;
            clientHeight = win.document.body.clientHeight;
            scrollTop = win.document.body.scrollTop;
            scrollHeight = win.document.body.scrollHeight;
        } else {
            clientWidth = win.document.documentElement.clientWidth;
            clientHeight = win.document.documentElement.clientHeight;
            //Chrome & Opera need get body.scrollTop.
            scrollTop = win.document.documentElement.scrollTop || win.document.body.scrollTop;
            scrollHeight = win.document.documentElement.scrollHeight;
        }

        // When popup the first window, need to get the parent window to get the correct scroll height.
        if (!isPopup && ifra) {
            win = ifra.contentWindow;
            scrollHeight = $(win.document).height();
        }

        var dTop = 0;

        if (opts.height != null) {
            container.style.minHeight = convert2Str(opts.height, unit);
            container.style.height = convert2Str(opts.height, unit);
            dTop = scrollTop + (clientHeight - opts.height) / 2 - getOffsetTop();
        }
        else {
            dTop = scrollTop + (clientHeight - container.offsetHeight) / 2 - getOffsetTop();
        }

        if (dTop < 10) {
            dTop = 10;
        }

        if (isPopup) {
            var dialogHeader = getParentDocument().getElementById("ACA_Dialog_Header");
            dTop = dTop - parseInt(dialogHeader.scrollHeight) / 2;
        }

        container.style.top = convert2Str(dTop, unit);
        container.top = convert2Str(dTop, unit);

        if (opts.width != null) {
            container.style.width = convert2Str(opts.width, unit);
            var dLeft = ($(document.body).width() - opts.width) / 2;
            container.style.left = convert2Str(dLeft, unit);

            clientWidth = Math.max(clientWidth, opts.width);
        }

        popupMask.style.height = convert2Str(scrollHeight, unit);
    }

    function convert2Str(num, vUnit) {
        if (vUnit === "px") {
            return num + "px";
        } else if (vUnit === "em") {
            return num / 10 + "em";
        } else if (unit === "px") {
            return num;
        } else if (unit === "em") {
            return num / 10;
        } else {
            return num;
        }
    }

    function getOffsetTop() {
        var offsetTop = 0;
        var elem = getParentDocument().getElementById("ACAFrame");
        while (elem) {
            offsetTop += elem.offsetTop;
            elem = elem.offsetParent;
        }
        return offsetTop;
    }
};

function pressEnter2TriggerClick(event, btn){
    if (!$.global.isAdmin) {
        var e = event ? event : (window.event ? window.event : null);
        var target = e.target ? e.target : e.srcElement;
        if (e.keyCode == 13
            && (!(target.type && (target.type.toLowerCase() == "textarea" || target.type.toLowerCase() == "button")))
            && target.nodeName.toLowerCase() != "a"
            && target.nodeName.toLowerCase() != "img"
            && btn.is(":visible")) {
            // Prevent the enter event 
            if (e.preventDefault) {
                e.preventDefault();
            } else {
                window.event.returnValue = false;
            }
                
            btn[0].focus();
            invokeClick(btn[0]);
            return true;
        }
    }
    
    return false;
}

function isRequiredAndEmptyField(controlId) {
    if ($.exists("#" + controlId) && $.exists('#' + controlId + '_label_0')) {
        var requireText = $('#' + controlId + '_label_0').text();
        if (requireText == '*' && String.IsNullOrEmpty($('#' + controlId).val())) {
            return true;
        }
    }
    return false;
}

function SetLastFocus(focusObject) {
    //Set focus object for Section508.
    if (focusObject) {
        var objectID = focusObject;

        if (typeof (focusObject) != 'string') {
            objectID = focusObject.id;
        }

        if (!objectID) {
            return;
        }

        if (theForm.elements["__LASTFOCUS_ID"] == null) {
            var _lastFocus = document.createElement('input');
            _lastFocus.id = '__LASTFOCUS_ID';
            _lastFocus.name = '__LASTFOCUS_ID';
            _lastFocus.type = 'hidden';
            theForm.appendChild(_lastFocus);
        }

        theForm.elements["__LASTFOCUS_ID"].value = objectID;
    }
}

function isHijriCalendarControl(controlId) {
    if ($.exists("#" + controlId + GLOBAL_CALENDARTEXT_CLIENTSTATE) && $("#" + controlId + GLOBAL_CALENDARTEXT_CLIENTSTATE).val() == GLOBAL_CALENDARTEXT_ISLAMIC_CALENDAR) {
        return true;
    }

    return false;
}

function trimInput(elementId) {
    var element = $('#' + elementId);

    element.val(element.val().trim());
}