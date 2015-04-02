/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AccelaWebControlExtenderBehavior.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AccelaWebControlExtenderBehavior.js 72643 2007-07-10 21:52:06Z weiky.chen $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
 
function GetColumnTable(th)
{
    var pn = th.parentNode;
    while(pn && pn.tagName != 'TABLE')
    {
        pn = pn.parentNode;
    }
    
    return pn;
}
 
  function DropDownObj(showType,items){
        this.ShowType = showType;
        this.Items = items;
    }
    
    function ItemObj(value,resValue,description,resDescription)
    {
       this.value =value;
       this.resValue = resValue;
       this.description = description;
       this.resDescription = resDescription;
    }
    
 Type.registerNamespace('AccelaWebControlExtender');

AccelaWebControlExtender.AccelaWebControlExtenderBehavior = function(element) {
    if (element == null) {
        return;
    }

    for (var comp in Sys.Application._components) {
        var component = Sys.Application._components[comp];
        var id = null;
        if (component != null && typeof (component._element) != "undefined") {
            id = component._element.id;
        }

        if (typeof (element) != "undefined" && element.id != "" && id == element.id) {
            Sys.Application.removeComponent(component);
        }
    }

    AccelaWebControlExtender.AccelaWebControlExtenderBehavior.initializeBase(this, [element]);

    this._elementTypeValue = null;
    this._clientVisibleValue = true;
    this._stdCategoryValue = null;
    this._sectionIdValue = null;
    this._viewElementIdValue = null;
    this._moduleNameValue = null;
    this._controlIdValue = null;
    this._sourceTypeValue = null;
    this._labelTypeValue = null;
    this._defaultLabelValue = null;
    this._defaultSubLabelValue = null;
    this._reportIdValue = null;
    this._enableConfigureURLValue = null;
    this._enableRecordTypeFilterValue = null;
    this._gridViewColumnsVisible = null;
    this._labelKeyValue = null;
    this._restrictDisplayValue = null;
    this._sectionFields = null;
    this._tabs = null;
    this._divClickHandler = null;
    this._div = null;
    this._defaultLanguageTextValue = null;
    this._defaultLanguageSubLabelValue = null;
    this._showTypeValue = null;
    this._listTypeValue = null;
    this._autoFillTypeValue = null;
    this._positionIDValue = null;
    this._gridViewHeadWidths = null;
    this._gridViewRealWidthValue = 0;
    this._maxLengthValue = 255;
    this._isTemplateField = null;
    this._templateAttribute = null;
    this._isHiddenLabel = null;
    this._gridViewAllowPaging = false;
    this._gridViewPageSize = null;
    this._divTypeValue = null;
    this._templateGenus = null;
    this._permissionValueId = null;
    this._subContainerClientID = null;
};

 AccelaWebControlExtender.AccelaWebControlExtenderBehavior.prototype = {
     _isInHTMLEditor: function (elementA) {
         var count = 10; // loop 10 level to find the element A
         var isFound = false;
         var parentNode = elementA.parentNode;

         while (count > 0) {
             if (!parentNode) {
                 isFound = false;
                 break;
             }

             if ((typeof (parentNode.getAttribute) != 'undefined' && parentNode.getAttribute('ControlType') == '1') ||
                  (parentNode.className && parentNode.className.indexOf("ACA_Section_Instruction") != -1)) //HTML Editor
             {
                 isFound = true;
                 break;
             }

             parentNode = parentNode.parentNode;
             count--;
         }

         return isFound;
     },

     _disableLink: function () {
         var flag = this._element.ownerDocument.disableLinkFlag;
         if (typeof (flag) == 'undefined') {
             // find all A element in current document
             var aTags = this._element.ownerDocument.getElementsByTagName('a');

             // disable all A element except A is in body text for Html Editor to edit it.
             for (var i = 0; i < aTags.length; i++) {
                 if (!this._isInHTMLEditor(aTags[i])) {
                     aTags[i].href = 'javascript:function DoNothing(){return false;}';
                 }
             }

             this._element.ownerDocument.disableLinkFlag = true;
         }
     },

     _hideControl: function () {
         var div = document.getElementById(this._element.id + '_element_group');

         if (div) {
             div.style.display = 'none';
             if (typeof (parent.SetFieldParentVisible) != "undefined") {
                 parent.SetFieldParentVisible(div, false);
             }
         }
     },

     _setGridViewColumnVisible: function () {
         if (this._gridViewColumnsVisible == null || this._gridViewColumnsVisible == '') {
             return;
         }

         var json = eval('(' + this._gridViewColumnsVisible + ')');
         var isAdmin = json.IsAdmin;
         var defaultTableWidth = json.DefaultTableWidth;
         var visibles = json.GridViewColumnVisibleList;
         var nodes = this._element.getElementsByTagName('TH');
         var isFirefox = navigator.userAgent.indexOf("Firefox") > 0 ? true : false;
         var isSafari = navigator.userAgent.indexOf("Safari") > 0 ? true : false;
         var isIE9 = navigator.userAgent.indexOf("MSIE 9") > 0 ? true : false;
         var isIE10 = navigator.userAgent.indexOf("MSIE 10") > 0 ? true : false;
         var nodeIndex = isFirefox || isSafari ? 1 : 0;
         var grid = null;

         for (var j = 0; j < visibles.length; j++) {
             for (var k = 0; k < nodes.length; k++) {
                 var childNodes = nodes[k].childNodes;
                 for (var i = 0; i < childNodes.length; i++) {
                     if (!childNodes[i]) {
                         continue;
                     }
                     var node = childNodes[i];
                     var id = '';
                     var hasFound = false;
                     while (node) {
                         var id = node == null ? null : node.id;
                         if (id != null && id.substr(id.lastIndexOf('_') + 1) == visibles[j].Name) {
                             // record the original column width
                             var th = this._getColumnTH(node);
                             if (th) {
                                 // get table and set default width
                                 if (grid) {
                                     grid.defaultWidth = defaultTableWidth;
                                 } else {
                                     grid = GetColumnTable(th);
                                 }

                                 if (visibles[j].FixWidth != null && visibles[j].FixWidth != "" && visibles[j].FixWidth != "-1" && visibles[j].FixWidth != "0") {
                                     th.columnWidth = visibles[j].FixWidth;
                                     th.originalWidth = visibles[j].FixWidth;
                                 } else {
                                     th.columnWidth = th.style.width;
                                 }
                             }
                             hasFound = true;
                             break;
                         }

                         node = isIE9 || isIE10 ? node.firstElementChild : node.childNodes[nodeIndex];
                     }
                     if (hasFound) {
                         var visible = visibles[j].Visible;
                         childNodes[i].style.display = visible ? '' : 'none';
                         if (!visible) {
                             var th = this._getColumnTH(childNodes[i]);
                             if (th) {
                                 th.originalWidth = th.style.width;
                                 th.columnWidth = th.originalWidth;
                                 th.style.width = '0px';
                                 th.style.display = "none";
                             }
                         }
                         break;
                     }
                 }
             }
         }
     },

     _isButton: function () {
         return this._elementTypeValue == 'GridViewHeaderLabel' || this._elementTypeValue == 'AccelaLinkButton' || this._elementTypeValue == 'AccelaButton';
     },

     _getColumnWidth: function () {
         var width = '-1';
         if (this._gridViewHeadWidths != null) {
             for (var i = 0; i < this._gridViewHeadWidths.length; i++) {
                 if (this._gridViewHeadWidths[i].Name == this._viewElementIdValue) {
                     width = this._gridViewHeadWidths[i].Width.trim();
                     break;
                 }
             }
         }

         if (width == '-1' || width == '0' || width == '') {
             var th = this._getColumnTH(this._element);
             width = th.style.width;

         }
         else {
             width += 'px';
         }
         //th.columnWidth = width;
         return width;
     },

     _getColumnTH: function (node) {
         var pn = node.parentNode;
         while (pn && pn.tagName != 'TH') {
             pn = pn.parentNode;
         }

         return pn;
     },

     _setColumnWidth: function (width) {
         if (width == '') {
             width = '-1';
         }
         else {
             width = width.replace('px', '');
         }
         if (this._gridViewHeadWidths != null) {
             for (var i = 0; i < this._gridViewHeadWidths.length; i++) {
                 if (this._gridViewHeadWidths[i].Name == this._viewElementIdValue) {
                     this._gridViewHeadWidths[i].Width = width;
                     break;
                 }
             }
         }
     },

     initialize: function () {
         if (!this.get_element()) {
             return;
         }

         AccelaWebControlExtender.AccelaWebControlExtenderBehavior.callBaseMethod(this, 'initialize');
         if (!this._clientVisibleValue) {
             this._hideControl();
         }

         if (typeof (this._element.attributes['watermarkText']) != 'undefined') {
             this._setWatermarkText('watermarkText', this._element.attributes['watermarkText'].value);
         }

         if (typeof (this._element.attributes['watermarkText1']) != 'undefined') {
             this._setWatermarkText('watermarkText1', this._element.attributes['watermarkText1'].value);
         }

         if (typeof (this._element.attributes['watermarkText2']) != 'undefined') {
             this._setWatermarkText('watermarkText2', this._element.attributes['watermarkText2'].value);
         }

         switch (this._elementTypeValue) {
             case 'GridViewHeaderLabel':
             case 'AccelaLinkButton':
                 var parentEle = this._element.parentNode;
                 if (typeof (parentEle) != 'undefined' && parentEle.tagName == 'DIV') {
                     var className = parentEle.className;
                     if (className.indexOf('ACA_LinkButton') == -1 && (className.indexOf('Button') > -1 || className.indexOf('Btn') > -1 || className.indexOf('ACA_ForRight') > -1)) {
                         this._div = parentEle;
                         break;
                     }
                 }
                 this._div = this._element;

                 if (this._elementTypeValue == 'GridViewHeaderLabel') {

                     if (this._gridViewColumnsVisible != '') {
                         this._gridViewHeadWidths = eval('(' + this._gridViewColumnsVisible + ')');
                     }
                 }
                 break;
             case 'AccelaButton':
                 var parentEle = this._element.parentNode;
                 if (typeof (parentEle) != 'undefined' && parentEle.tagName == 'DIV') {
                     var className = parentEle.className;
                     if (className.indexOf('Button') > -1 || className.indexOf('Btn') > -1 || className.indexOf('ACA_ForRight') > -1) {
                         this._div = parentEle;
                         break;
                     }
                 }
                 this._div = this._element;
                 break;
             case "AccelaSectionTitleBar":
             case 'AccelaLabel':
                 if (this._labelTypeValue == 30) {
                     this._labelTypeValue = 29;
                 }
                 if (this._labelTypeValue == 1 || this._labelTypeValue == 36) {//add a div
                     this._div = document.createElement('div');
                     var p = this._element.parentNode;
                     p.insertBefore(this._div, this._element);
                     this._div.appendChild(this._element);
                 }
                 else if (this._labelTypeValue == 14) {
                     this._div = this._element.parentNode;
                     while (this._div.tagName != 'DIV') {
                         this._div = this._div.parentNode;
                     }
                 }
                 else if (this._labelTypeValue == 17) {
                     this._div = this._element.parentNode;
                 }
                 else if (this._labelTypeValue == 41) {
                     var fieldsets = $(this._element).parents('fieldset');

                     if (fieldsets.length > 0) {
                         this._div = fieldsets[0];
                     }
                     else {
                         this._div = this._element;
                     }
                 }
                 else {
                     this._div = this._element;
                 }
                 if (this._viewElementIdValue && this._gridViewColumnsVisible != '') {
                     this._gridViewHeadWidths = eval('(' + this._gridViewColumnsVisible + ')');
                 }
                 break;
             case 'AccelaRadioButton':
                 this._div = this._element.parentNode;
                 break;
             case 'AccelaDiv':
                 this._div = this._element;
                 break;
             case 'AccelaGridView':
                 this._div = this._element;
                 if (this._gridViewRealWidthValue != 0) {
                     this._element.currentWidth = this._gridViewRealWidthValue;
                 }
                 else {
                     this._element.currentWidth = null;
                 }
                 this._setGridViewColumnVisible();
                 break;
             case "AccelaRadioButtonList":
                 if (this._listTypeValue == "3") {
                     var divID = this._element.id.substr(0, this._element.id.lastIndexOf("radioListContactPermission")) + "divContact_parentGrid";
                     this._div = $get(divID);
                 }
                 else {
                     this._div = $get(this._element.id + '_table');
                 }
                 break;
             case "AccelaCheckBoxList":
                 if (this._element.id.indexOf("ckbCustomPermissionList") > -1) {
                     var divID = this._element.id.substr(0, this._element.id.lastIndexOf("ckbCustomPermissionList")) + "divContact_parentGrid";
                     this._div = $get(divID);
                 }
                 else {
                     this._div = $get(this._element.id + '_table');
                 }
                 break;
             default:
                 this._element.readOnly = true;
                 this._element.hideFocus = true;
                 if (this._elementTypeValue == 'AccelaCalendarText') {
                     //this._div = this._element.parentNode;
                     this._div = $get(this._element.id + '_table');

                     var calendarButton = $get(this._element.id + '_calendar_button');
                     if (calendarButton) {
                         calendarButton.disabled = true;
                         calendarButton.title = '';
                     }
                 }
                 else {
                     if (this._elementTypeValue == 'AccelaPhoneText' && $get(this._element.id + '_IDD') != null) {
                         //this._div = this._element.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
                         this._div = $get(this._element.id + '_table');
                     }
                     else {
                         //this._div = this._element.parentNode.parentNode;
                         this._div = $get(this._element.id + '_table');
                     }
                 }
                 break;
         }
         this._disableLink();
         this._div.style.cursor = 'default';
         this._divClickHandler = Function.createDelegate(this, this._onClick);
         $addHandler(this._isButton() ? this._element : this._div, 'click', this._divClickHandler);
         this._element.parentObj = this;
     },

     dispose: function () {
         if (this._divClickHandler) {
             //$removeHandler(this._div, 'click', this._divClickHandler); 
             this._divClickHandler = null;
         }
         AccelaWebControlExtender.AccelaWebControlExtenderBehavior.callBaseMethod(this, 'dispose');
     },
     _updateMoreTabName: function () {
         //update more tab innerHtml
         if (__Tab) {
             __Tab.updateMoreTabName();
         }
     },
     _updateMoreLinkButton: function () {
         //update more link button innerHtml
         if (__Tab) {
             __Tab.updateMoreLinkButton();
         }
     },
     _updateTabName: function (innerHTML, value, isTextUpdated) {
         if (this._elementTypeValue == 'AccelaLabel' && this._moduleNameValue != null && this._moduleNameValue != '') {
             var as = this._element.ownerDocument.getElementsByTagName('a');
             for (var i = 0; i < as.length; i++) {
                 if (as[i].getAttribute('module') == this._moduleNameValue) {
                     parent.ModifyMark();
                     if (__Tab) {
                         __Tab.updateTabName(this._moduleNameValue, innerHTML, value, isTextUpdated);
                     }
                     break;
                 }
             }
         }
     },

     /*
     -1 -- DDL source type HardCodeForSectionTitle
     0  -- LabelText
     1  -- BodyText
     2  -- SectionText
     3  -- normal AccelaRadioButtonList or normal AccelaTextBox or default
     4  -- DDL source type Database
     5  -- DDL source type StandardChoice
     6  -- DDL source type HardCode
     7  -- AccelaCheckBox
     8  -- 
     9  -- GridViewHeaderLabel
     10 -- AccelaLinkButton withd report id
     11 -- AccelaRadioButton
     12 -- AccelaDiv
     13 -- Accela grid view
     14 -- ApplicantText
     15 -- DDL Permit Type
     16 -- DDL for caphome & apo lookup search dropdownlist
     17 -- HidableLabel and Logo
     18 -- SectionExText
     19 -- AccelaLinkButton enable config URL
     20 -- DDL STDandXPolicy
     21 -- AccelaRadioButtonList PaymentHardCode
     22 -- TextBox  City Text Box
     23 -- TextBox State Text Box
     24 -- DDL state control's dropdownlist
     
     26 -- AccelaLinkButton enableRecordTypeFilter

     101 -- 
     35 -- page flow
     36 -- page properties panel
     37 -- load section that has available fileds that can setting visable or not
     39 -- div control contains standard component
     40 -- div control contains customize component
     50 -- SectionExRadio
     */
     _getType: function () {
         var type;
         switch (this._elementTypeValue) {
             case "AccelaSectionTitleBar":
             case 'AccelaLabel':
             case 'AccelaNameValueLabel':
                 type = this._labelTypeValue; //0,1,2,14,17,18
                 break;
             case 'AccelaDropDownList':
                 if (this._sectionIdValue != null) {
                     type = 16; // for caphome & apo lookup search dropdownlist
                 }
                 else if (this._autoFillTypeValue == 2)// the control is state.
                 {
                     type = 24;
                 }
                 else {
                     type = this._sourceTypeValue + 4; //4,5,6
                 }
                 break;
             case 'AccelaCheckBox':
                 type = 7;
                 break;
             case 'AccelaImageButton':
             case 'AccelaLinkButton':
             case 'AccelaButton':
             case 'GridViewHeaderLabel':
                 if (this._enableConfigureURLValue) {
                     type = 19;
                 }
                 else if (this._reportIdValue && this._reportIdValue != '') {
                     type = 10;
                 }
                 else if (this._enableRecordTypeFilterValue) {
                     type = 26;
                 }
                 else {
                     type = 9;
                 }
                 break;
             case 'AccelaRadioButton':
                 type = this._labelTypeValue;
                 break;
             case 'AccelaDiv':
                 if (document.tabs) {
                     this._tabs = document.tabs;
                 }

                 // set the control type for div
                 if (this._divTypeValue == 1) {
                     // use for standard component
                     type = 39;
                 }
                 else if (this._divTypeValue == 2) {
                     // use for customize component
                     type = 40;
                 }
                 else {
                     type = 12;
                 }

                 break;
             case 'AccelaGridView':
                 type = 13;
                 break;
             case 'AccelaRadioButtonList':
                 if (this._listTypeValue == "1") {
                     type = 21;
                 }
                 else {
                     type = 3;
                 }
                 break;
             case 'AccelaTextBox':
                 if (this._autoFillTypeValue == 1) //Auto Fill City Text Box
                 {
                     type = 22;
                 }
                 else if (this._autoFillTypeValue == 2) // Auto Fill State Text Box
                 {
                     type = 23;
                 }
                 else {
                     type = 3;
                 }
                 break;

             default:
                 type = 3;
                 break;
         }

         return type;
     },

     _setTabsOrder: function () {
         //        if(this._tabs != null)
         //        {
         //            var as = this._div.ownerDocument.getElementById('divNaviMenu').getElementsByTagName('a');
         //            for(var i=0;i<this._tabs.length;i++)
         //            {
         //                as[i].innerHTML = this._tabs[i][0];
         //                as[i].title = as[i].textContent?as[i].textContent:as[i].innerText; 
         //                as[i].setAttribute('module',this._tabs[i][1]) ;
         //            }
         //            document.tabs = this._tabs; 
         //        }    
     },

     _onClick: function (e) {
         if (e) {
             e.stopPropagation();
         }
         else {
             window.event.cancelBubble = true;
         }

         if (this._isButton() && this._labelKeyValue == null) {
             return;
         }

         var isOneLevel = typeof (parent.LoadProperties) != 'undefined';

         var oldSelectedObj = isOneLevel ? this._div.ownerDocument.currentSelectedObj : parent.document.currentSelectedObj;
         if (typeof (oldSelectedObj) == 'undefined' || typeof (oldSelectedObj._element) == 'undefined') {
             oldSelectedObj = null;
         }
         if (oldSelectedObj) {
             if (oldSelectedObj == this) {
                 return;
             }
             else {
                 this._unselectElement(oldSelectedObj);
             }
         }

         if (this._elementTypeValue == 'AccelaCheckBox' || this._elementTypeValue == 'AccelaRadioButton') {
             this._element.checked = false;
         }
         else if (this._elementTypeValue == 'AccelaCheckBoxList' || this._elementTypeValue == 'AccelaRadioButtonList') {
             if (this._element.childNodes.length > 0) {
                 var childNodes = this._element.childNodes[0].childNodes;
                 if (this._listTypeValue != "1" && this._listTypeValue != "2") {
                     for (var i = 0; i < childNodes.length; i++) {
                         childNodes[i].firstChild.firstChild.checked = false;
                     }
                 }
             }
         }

         this._selectElement();

         if (isOneLevel) {
             this._div.ownerDocument.currentSelectedObj = this;
             parent.LoadProperties(this._getType(), this);
         }
         else if (typeof (parent.parent.LoadProperties) != 'undefined') {
             parent.document.currentSelectedObj = this;
             parent.parent.LoadProperties(this._getType(), this);
         }
     },

     _selectUpdatePanelDiv: function (obj, select) {
         if (obj != null) {
             var p = obj.parentNode;
             var count = 0;
             while ((p.id == '' || p.id.indexOf(obj.id) == 0) && count < 50) {
                 p = p.parentNode;
                 count++;
             }

             p.className = select ? 'ACA_Control_HighLight' : '';
         }
     },

     _selectElement: function () {
         if (this._element != this._div) {
            $(this._getChildControl(this._element.id)).each(function() {
                $(this).css('backgroundColor', '#FCFBC9');
                $(this).attr('disabled', true);
            });

             if (this._isButton()) {
                 this._div.className = this._div.className.replace(new RegExp('Button', "gm"), 'ButtonHighLight');
             }
             else {
                 if (this._elementTypeValue == 'AccelaDropDownList' && this._sectionIdValue != null) {
                     this._selectUpdatePanelDiv(this._element, true);
                 }
                 if (this._elementTypeValue == 'AccelaPhoneText' && $get(this._element.id + '_IDD') != null) {
                     $get(this._element.id + '_IDD').style.backgroundColor = '#FCFBC9';
                     $get(this._element.id + '_IDD').disabled = true;
                 }
                 this._element.style.backgroundColor = '#FCFBC9';
                 this._div.className += ' ACA_Control_HighLight';
             }
         }
         else if (this._elementTypeValue == 'AccelaGridView') {
             this._div.class_backup = this._div.className;
             this._div.className = 'ACA_Grid_Caption ACA_Control_HighLight';
             this._div.style_backup = this._div.style.cssText;
             var width = this._div.style.width;
             this._div.style.cssText = '';
             this._div.style.width = width;
         }
         else if (this._labelTypeValue == 28 || this._labelTypeValue == 29 || this._labelTypeValue == 38) {  // section or sub section title
             var divInstruction = $get(this._element.id + '_sub_label');
             if (divInstruction) {
                 divInstruction.parentNode.className += ' ACA_Control_HighLight';
                 this._expandSection(divInstruction);
             }
         }
         else if (this._labelTypeValue == 35) {  // page title
             var targetCtrl = $get(this._element["TargetCtrlId"]);
             if (targetCtrl) {
                 targetCtrl.className += ' ACA_Control_HighLight';
             }
         }
         else {
             if (this._labelTypeValue == 2 || this._labelTypeValue == 18 || this._labelTypeValue == 37 || this._labelTypeValue == 42) {
                 this._element.parentNode.parentNode.parentNode.parentNode.className = 'ACA_Control_HighLight';
             }
             this._div.className += ' ACA_Control_HighLight';
         }

         //page instruction
         if (this._elementTypeValue == 'AccelaLabel' && this._labelTypeValue == 36) {
             if (Sys.UI.DomElement.containsCssClass(this._element, 'ACA_Page_Instruction_Watermark')) {
                 this._setLabel('');
                 Sys.UI.DomElement.removeCssClass(this._element, 'ACA_Page_Instruction_Watermark');
                 Sys.UI.DomElement.addCssClass(this._element, 'ACA_Placeholder_Height');
             }
         }

         if (this._elementTypeValue == 'AccelaRadioButton' && this._subContainerClientID != '') {
             $('#' + this._subContainerClientID).addClass('ACA_Control_HighLight');
         }
     },

     _unselectElement: function (obj) {
         if (obj._element != obj._div) {
             $(this._getChildControl(obj._element.id)).each(function () {
                 $(this).css('backgroundColor', '');
                 $(this).attr('disabled', false);
             });

             if (obj._isButton()) {
                 obj._div.className = obj._div.className.replace(new RegExp('ButtonHighLight', "gm"), 'Button');
             }
             else {
                 if (obj._elementTypeValue == 'AccelaDropDownList' && obj._sectionIdValue != null) {
                     obj._selectUpdatePanelDiv(obj._element, false);
                 }
                 if (obj._elementTypeValue == 'AccelaPhoneText' && $get(obj._element.id + '_IDD') != null) {
                     $get(obj._element.id + '_IDD').style.backgroundColor = '';
                     $get(obj._element.id + '_IDD').disabled = false;
                 }

                 obj._element.style.backgroundColor = '';
                 obj._div.className = obj._div.className.replace('ACA_Control_HighLight', '').trim();
             }
         }
         else if (obj._elementTypeValue == 'AccelaGridView') {
             obj._div.className = obj._div.class_backup;
             var width = obj._div.style.width;
             obj._div.style.cssText = obj._div.style_backup;
             obj._div.style.width = width;
         }
         else if (obj._labelTypeValue == 28 || obj._labelTypeValue == 29 || obj._labelTypeValue == 38) {  // section title
             var divInstruction = obj._element.ownerDocument.getElementById(obj._element.id + '_sub_label');
             var sectionObj = divInstruction ? divInstruction.parentNode : obj._element.parentNode;
             sectionObj.className = sectionObj.className.replace('ACA_Control_HighLight', '').trim();
         }
         else if (obj._labelTypeValue == 35) {  // page title
             var targetCtrl = $get(obj._element["TargetCtrlId"]);
             if (targetCtrl) {
                 targetCtrl.className = targetCtrl.className.replace('ACA_Control_HighLight', '').trim();
             }
         }
         else {
             if (obj._labelTypeValue == 2 || obj._labelTypeValue == 18 || obj._labelTypeValue == 37 || obj._labelTypeValue == 42) {
                 obj._element.parentNode.parentNode.parentNode.parentNode.className = '';
             }
             obj._div.className = obj._div.className.replace('ACA_Control_HighLight', '').trim();
         }

         //page instruction
         if (obj._elementTypeValue == 'AccelaLabel' && obj._labelTypeValue == 36) {
             if (obj._getLabel() == '' && typeof (obj._element.attributes['watermarkText4PageIns']) != 'undefined') {
                 if (!Sys.UI.DomElement.containsCssClass(obj._element, 'ACA_Page_Instruction_Watermark')) {
                     Sys.UI.DomElement.addCssClass(obj._element, 'ACA_Page_Instruction_Watermark');
                 }

                 obj._setLabel(obj._element.attributes['watermarkText4PageIns'].value);
                 Sys.UI.DomElement.removeCssClass(obj._element, 'ACA_Placeholder_Height');
             }
         }

         if (obj._elementTypeValue == 'AccelaRadioButton' && obj._subContainerClientID != '') {
             $('#' + obj._subContainerClientID).removeClass('ACA_Control_HighLight');
         }
     },

     _expandSection: function (instructionObj) {
         if (instructionObj) {
             var icon = this._element.parentNode.firstChild;

             if (!icon || icon.nodeName != 'A') {
                 icon = instructionObj.parentNode.getElementsByTagName('A')[0];
             }

             if (icon) {
                 var img = icon.firstChild;
                 if (img && img.nameProp == "caret_collapsed.gif") {
                     icon.click();
                 }
             }
         }
     },

     _getLabel: function () {
         if (this._isButton()) {
             if (this._labelKeyValue == null) {
                 return null;
             }
             if (typeof (this._element.firstChild.innerHTML) != 'undefined') {
                 return this._element.firstChild.innerHTML;
             }
             else {
                 return this._element.firstChild.data;
             }
         }
         if (this._elementTypeValue == 'AccelaCheckBox' || this._elementTypeValue == 'AccelaRadioButton') {
             return this._element.parentNode.lastChild.innerHTML;
         }

         if (this._elementTypeValue == 'AccelaLabel' || this._elementTypeValue == 'AccelaNameValueLabel') {
             if (this._element.childNodes != null && this._element.childNodes.length == 1
                && this._element.childNodes[0].id == this._element.id + '_add_div') {
                 return this._element.childNodes[0].innerHTML;
             }

             return this._element.innerHTML;
         }

         var label = $get(this._element.id + '_label_1');
         if (label) {
             return label.innerHTML;
         }

         return null;
     },

     _setLabel: function (value) {
         if (this._isButton()) {
             if (this._labelKeyValue == null) {
                 return;
             }
             if (typeof (this._element.firstChild.innerHTML) != 'undefined') {
                 this._element.firstChild.innerHTML = value;
             }
             else {
                 this._element.firstChild.data = value;
             }
         }
         else if (this._elementTypeValue == 'AccelaCheckBox' || this._elementTypeValue == 'AccelaRadioButton') {
             this._element.parentNode.lastChild.innerHTML = value;
         }
         else if (this._elementTypeValue == 'AccelaLabel' || this._elementTypeValue == "AccelaSectionTitleBar") {
             var div = document.createElement('div');
             div.id = this._element.id + '_add_div';
             div.innerHTML = value;

             while (this._element.childNodes.length > 0) {
                 this._element.removeChild(this._element.childNodes[0]);
             }

             this._element.appendChild(div);
         }
         else {
             var label = $get(this._element.id + '_label_1');
             if (label) {
                 label.innerHTML = value;
             }
         }
     },

     _getSubLabel: function () {
         var label = $get(this._element.id + '_sub_label');
         if (label) {
             return label.innerHTML;
         }

         return null;
     },

     _setSubLabel: function (value) {
         var label = $get(this._element.id + '_sub_label');
         if (label) {
             label.innerHTML = value;
             var helpIcon = $get(this._element.id + '_help');
             if (helpIcon) {
                 if ('' == label.innerHTML) {
                     helpIcon.className = helpIcon.className.replace('ACA_Show', 'ACA_Hide');
                 }
                 else {
                     helpIcon.className = helpIcon.className.replace('ACA_Hide', 'ACA_Show');
                 }
             }

             if (this._labelTypeValue == 28 || this._labelTypeValue == 29 || this._labelTypeValue == 2 || this._labelTypeValue == 18 || this._labelTypeValue == 37 || this._labelTypeValue == 38 || this._labelTypeValue == 42) {
                 if (!value) {
                     label.className = label.className.replace('ACA_Section_Instruction ACA_Section_Instruction_FontSize', 'ACA_Hide');
                 }
                 else {
                     label.className = label.className.replace('ACA_Hide', 'ACA_Section_Instruction ACA_Section_Instruction_FontSize');
                 }
             }
         }
     },

     _getWatermarkText: function (watermarkId) {
         if (typeof (this._element.attributes[watermarkId]) != 'undefined') {
             return this._element.attributes[watermarkId].value;
         }
         else {
             return '';
         }
     },

     _setWatermarkText: function (watermarkId, value) {
         if (typeof (this._element.attributes[watermarkId]) == 'undefined' || typeof (this._element.attributes['watermarkCssClass']) == 'undefined') {
             return;
         }

         var oldwatermarkText = this._element.attributes[watermarkId].value;
         this._element.attributes[watermarkId].value = value;
         var subControl;

         if (watermarkId == "watermarkText1") {
             subControl = $get(this._id.substring(0, this._id.lastIndexOf("_")) + GlobalConst.ChildControlIdPrefix + "0");
         } else if (watermarkId == "watermarkText2") {
             subControl = $get(this._id.substring(0, this._id.lastIndexOf("_")) + GlobalConst.ChildControlIdPrefix + "1");
         }
         else {
             subControl = this._element;
         }

         if ('' == value && subControl.value == oldwatermarkText) {
             subControl.value = '';
             Sys.UI.DomElement.removeCssClass(subControl, this._element.attributes['watermarkCssClass'].value);
         } else if ('' == subControl.value || subControl.value == oldwatermarkText) {
             subControl.value = value;
             if (!Sys.UI.DomElement.containsCssClass(subControl, this._element.attributes['watermarkCssClass'].value)) {
                 Sys.UI.DomElement.addCssClass(subControl, this._element.attributes['watermarkCssClass'].value);
             }
         }
     },

     _getRadioButtonItemsActivity: function () {
         if (this._elementTypeValue == 'AccelaRadioButtonList') {
             var value;
             if (this._listTypeValue == 1) {
                 var items = new Array();
                 for (var i = 0; i < this._element.rows.length; i++) {
                     if (isFireFox()) {
                         value = this._element.rows[i].cells[0].childNodes[0].value;
                     }
                     else {
                         value = this._element.cells[i].childNodes[0].value;
                     }
                     items.push(value.indexOf('-') == -1);
                 }

                 return items;
             }
         }

         return null;
     },

     _setRadioButtonActivity: function (array) {
         if (array && this._elementTypeValue == 'AccelaRadioButtonList') {
             if (this._listTypeValue == 1) {
                 var items = new Array();
                 for (var i = 0; i < this._element.rows.length && i < array.length; i++) {
                     var elementCell;
                     if (isFireFox()) {
                         elementCell = this._element.rows[i].cells[0];
                     }
                     else {
                         elementCell = this._element.cells[i];
                     }

                     elementCell.childNodes[0].value = elementCell.childNodes[0].value.replace('-', '');

                     if (array[i] != true) {
                         elementCell.childNodes[0].value = '-' + elementCell.childNodes[0].value;
                     }
                 }
             }
         }
     },


     _getDropdownItemsActivity: function () {
         if (this._elementTypeValue == 'AccelaDropDownList') {
             if (this._sourceTypeValue == 2) {
                 var items = new Array();
                 for (var i = 0; i < this._element.length; i++) {
                     var value = this._element[i].value;
                     items.push(value.indexOf('-') == -1);
                 }

                 return items;
             }
             else if (this._getType() == 20)// the control is contact type.
             {
                 var items = new Array();

                 for (var i = 0; i < this._element.length; i++) {
                     var value = this._element[i].value;
                     if (value != '') {
                         var strs = value.split('||');
                         if (strs.length == 2) {
                             items.push(strs[0].indexOf('-') == -1);
                         } else {
                             items.push(true);
                         }
                     }
                 }

                 return items;
             }
             else {
                 var visibles = this._element.getAttribute('Visibles');
                 if (visibles && visibles != '') {
                     if (typeof (visibles) == 'string') {
                         var array = visibles.split('|');
                         for (var i = 0; i < array.length; i++) {
                             array[i] = array[i] == '1';
                         }

                         return array;
                     }

                     return visibles;
                 }
             }
         }

         return null;
     },

     _setDropdownItemsActivity: function (array) {
         if (array && this._elementTypeValue == 'AccelaDropDownList') {
             if (this._sourceTypeValue == 2) {
                 var items = new Array();
                 for (var i = 0; i < this._element.length && i < array.length; i++) {
                     this._element[i].value = this._element[i].value.replace('-', '');
                     if (array[i] != true) {
                         this._element[i].value = '-' + this._element[i].value;
                     }
                 }
             }
             else if (this._getType() == 20)// the control is contact type.
             {
                 var j = 0;

                 for (var i = 0; i < this._element.length && j < array.length; i++) {
                     if (this._element[i].value == '') {
                         continue;
                     }

                     if (array[j] != true && this._element[i].value != '') {
                         this._element[i].value = '-' + '||' + this._element[i].value;
                     }

                     j++;
                 }
             }
             else {
                 var visibles = this._element.getAttribute('Visibles');
                 if (visibles) {
                     this._element.setAttribute('Visibles', array);
                 }
             }
         }
     },

     _getRadioButtonListItems: function () {
         if (this._elementTypeValue != 'AccelaRadioButtonList') {
             return null;
         }

         var items = new Array();
         var j = 0;
         var key;
         var text;

         for (var i = 0; i < this._element.rows.length; i++) {
             if (isFireFox()) {
                 key = this._element.rows[i].cells[0].childNodes[0].value;
                 text = this._element.rows[i].cells[0].textContent;
             }
             else {
                 key = this._element.cells[i].childNodes[0].value;
                 text = this._element.cells[i].innerText;
             }

             // i=0 is 'select'
             if (key != '') {
                 var index = key.indexOf('||');
                 if (index != -1)
                     key = key.substring(index + 2);
                 if (key != '')
                     items.push(key + '||' + text);

                 j++;
             }
         }
         return items;
     },

     _setRadioButtonItems: function (array, showType) {
         if (array && this._elementTypeValue == 'AccelaRadioButtonList') {
             var inflate = 0;
             var firstCellValue;
             if (isFireFox()) {
                 firstCellValue = this._element.rows[0].cells[0].value;
             }
             else {
                 firstCellValue = this._element.cells[0].value;
             }

             if (this._element.rows.length > 0 && firstCellValue == '') {
                 inflate = 1;
             }

             for (var i = 0; i < array.length; i++) {
                 var elementCell;
                 if (isFireFox()) {
                     elementCell = this._element.rows[i + inflate].cells[0];
                 }
                 else {
                     elementCell = this._element.cells[i + inflate];
                 }

                 if (array[i].indexOf('||') != -1) {
                     dataArray = array[i].split('||');
                     elementCell.innerHTML = dataArray[1];
                     elementCell.childNodes[1].value = dataArray[0];
                 }
                 else {
                     elementCell.childNodes[1].innerHTML = array[i];
                 }
             }
         }
     },

     _getDropdownItems: function () {
         if (this._elementTypeValue != 'AccelaDropDownList') {
             return null;
         }

         var items = new Array();
         var j = 0;

         for (var i = 0; i < this._element.length; i++) {
             var key = this._element[i].value;
             var text = this._element[i].text;

             // i=0 is 'select'
             if (key != '') {
                 if (this._sourceTypeValue != 1 && this._sourceTypeValue != 16)  // not stard choice
                 {
                     var index = key.indexOf('||');
                     if (index != -1)
                         key = key.substring(index + 2);
                     if (key != '')
                         items.push(key + '||' + text);
                     else {
                         items.push(text + '||' + key);
                     }
                 }
                 else {
                     if (this._showTypeValue == 1 || this._showTypeValue == 2)  // show description or show value and description
                     {
                         dataArray = key.split('||');  // 1: bizDomainValue, 2: resBizDomainValue, 3:description, 4: resDescription
                         if (dataArray[1] != '') //if value doesn't is null or empty.
                         {
                             items[j] = new ItemObj(dataArray[0], dataArray[1], dataArray[2], dataArray[3]);
                         }
                     }
                     else {
                         items[j] = new ItemObj(key, text, null, null);
                     }
                 }

                 j++;
             }
         }
         if (this._sourceTypeValue != 1 && this._sourceTypeValue != 16) {
             return items;
         }
         var dropDownItems = new DropDownObj(this._showTypeValue, items);

         // return items;
         return dropDownItems;
     },

     _setDropdownItems: function (array, showType) {
         if (array && this._elementTypeValue == 'AccelaDropDownList') {
             var inflate = 0;
             if (this._element.length > 0 && this._element[0].value == '') {
                 inflate = 1;
             }

             this._element.length = array.length + inflate;

             for (var i = 0; i < array.length; i++) {
                 if (showType == 1 || showType == 2) {
                     dataArray = array[i].split('||');  // 0: bizDomainValue, 1: resBizDomainValue, 2:description, 3: resDescription
                     this._element[i + inflate].value = array[i];
                     this._element[i + inflate].innerHTML = JsonEncode(showType == 1 ? dataArray[3] : dataArray[1] + ' - ' + dataArray[3]);
                 }
                 else {
                     if (array[i].indexOf('||') != -1) {
                         dataArray = array[i].split('||');
                         this._element[i + inflate].innerHTML = JsonEncode(dataArray[1]);
                         this._element[i + inflate].value = dataArray[0];
                     }
                     else {
                         this._element[i + inflate].innerHTML = JsonEncode(array[i]);
                     }
                 }
             }
         }
     },

    _getChildControl: function (parentId) {
        var idNumber = 0;
        var idPrefix = parentId + GlobalConst.ChildControlIdPrefix;
        var $child = $('#' + idPrefix + idNumber);
        var children = new Array();

        while ($.exists($child)) {
            children[idNumber] = $child;

            idNumber++;
            $child = $('#' + idPrefix + idNumber);
        }
        return children;
    },

     getPermissionValue: function () {
         if (this._permissionValueId != null || this._permissionValueId != '') {
             return $get(this._permissionValueId).value.replace('-||', '');
         }
         else {
             return null;
         }
     },

     get_ElementType: function () {
         return this._elementTypeValue;
     },

     set_ElementType: function (value) {
         this._elementTypeValue = value;
     },

     get_ClientVisible: function () {
         return this._clientVisibleValue;
     },

     set_ClientVisible: function (value) {
         this._clientVisibleValue = value;
     },

     get_StdCategory: function () {
         return this._stdCategoryValue;
     },

     set_StdCategory: function (value) {
         this._stdCategoryValue = value;
     },

     get_ModuleName: function () {
         return this._moduleNameValue;
     },

     set_ModuleName: function (value) {
         this._moduleNameValue = value;
     },

     get_SectionID: function () {
         return this._sectionIdValue;
     },

     set_SectionID: function (value) {
         this._sectionIdValue = value;
     },

     get_ViewElementID: function () {
         return this._viewElementIdValue;
     },

     set_ViewElementID: function (value) {
         this._viewElementIdValue = value;
     },

     get_PositionID: function () {
         return this._positionIDValue;
     },

     set_PositionID: function (value) {
         this._positionValue = value;
     },

     get_ControlID: function () {
         return this._controlIdValue;
     },

     set_ControlID: function (value) {
         this._controlIdValue = value;
     },

     get_SourceType: function () {
         return this._sourceTypeValue;
     },

     set_SourceType: function (value) {
         this._sourceTypeValue = value;
     },

     get_LabelType: function () {
         return this._labelTypeValue;
     },

     set_LabelType: function (value) {
         this._labelTypeValue = value;
     },

     get_GridViewRealWidth: function () {
         return this._gridViewRealWidthValue;
     },

     set_GridViewRealWidth: function (value) {
         this._gridViewRealWidthValue = value;
     },

     get_RestrictDisplay: function () {
         return this._restrictDisplayValue;
     },

     set_RestrictDisplay: function (value) {
         this._restrictDisplayValue = value;
     },

     get_DefaultLabel: function () {
         return this._defaultLabelValue;
     },

     set_DefaultLabel: function (value) {
         this._defaultLabelValue = value;
     },

     get_DefaultSubLabel: function () {
         return this._defaultSubLabelValue;
     },

     set_DefaultSubLabel: function (value) {
         this._defaultSubLabelValue = value;
     },

     get_LabelKey: function () {
         return this._labelKeyValue;
     },

     set_LabelKey: function (value) {
         this._labelKeyValue = value;
     },

     get_GridColumnsVisible: function () {
         return this._gridViewColumnsVisible;
     },

     set_GridColumnsVisible: function (value) {
         this._gridViewColumnsVisible = value;
     },

     get_ReportID: function () {
         return this._reportIdValue;
     },

     set_ReportID: function (value) {
         this._reportIdValue = value;
     },

     get_EnableConfigureURL: function () {
         return this._enableConfigureURLValue;
     },

     set_EnableConfigureURL: function (value) {
         this._enableConfigureURLValue = value;
     },

     get_EnableRecordTypeFilter: function () {
         return this._enableRecordTypeFilterValue;
     },

     set_EnableRecordTypeFilter: function (value) {
         this._enableRecordTypeFilterValue = value;
     },

     get_DefaultLanguageText: function () {
         return this._defaultLanguageTextValue;
     },

     set_DefaultLanguageText: function (value) {
         this._defaultLanguageTextValue = value;
     },

     get_DefaultLanguageSubLabel: function () {
         return this._defaultLanguageSubLabelValue;
     },

     set_DefaultLanguageSubLabel: function (value) {
         this._defaultLanguageSubLabelValue = value;
     },

     get_ShowType: function () {
         return this._showTypeValue;
     },

     set_ShowType: function (value) {
         this._showTypeValue = value;
     },

     get_ListType: function () {
         return this._listTypeValue;
     },

     set_ListType: function (value) {
         this._listTypeValue = value;
     },

     get_AutoFillType: function () {
         return this._autoFillTypeValue;
     },

     set_AutoFillType: function (value) {
         this._autoFillTypeValue = value;
     },

     get_MaxLength: function () {
         return this._maxLengthValue;
     },

     set_MaxLength: function (value) {
         this._maxLengthValue = value;
     },

     get_IsTemplateField: function () {
         return this._isTemplateField;
     },

     set_IsTemplateField: function (value) {
         this._isTemplateField = value;
     },

     get_TemplateAttribute: function () {
         return this._templateAttribute;
     },

     set_TemplateAttribute: function (value) {
         this._templateAttribute = value;
     },

     get_IsHiddenLabel: function () {
         return this._isHiddenLabel;
     },

     set_IsHiddenLabel: function (value) {
         this._isHiddenLabel = value;
     },

     get_GridViewAllowPaging: function () {
         return this._gridViewAllowPaging;
     },

     set_GridViewAllowPaging: function (value) {
         this._gridViewAllowPaging = value;
     },

     get_GridViewPageSize: function () {
         return this._gridViewPageSize;
     },

     set_GridViewPageSize: function (value) {
         this._gridViewPageSize = value;
     },

     get_DivType: function () {
         return this._divTypeValue;
     },

     set_DivType: function (value) {
         this._divTypeValue = value;
     },

     get_TemplateGenus: function () {
         return this._templateGenus;
     },

     set_TemplateGenus: function (value) {
         this._templateGenus = value;
     },

     get_PermissionValueId: function () {
         return this._permissionValueId;
     },

     set_PermissionValueId: function (value) {
         this._permissionValueId = value;
     },

     get_SubContainerClientID: function () {
         return this._subContainerClientID;
     },

     set_SubContainerClientID: function (value) {
         this._subContainerClientID = value;
     }
 };

AccelaWebControlExtender.AccelaWebControlExtenderBehavior.registerClass('AccelaWebControlExtender.AccelaWebControlExtenderBehavior', AjaxControlToolkit.BehaviorBase);

/*
After postback in updatepanel, the "disable link" flag need to clear.
Because all the Anchor object need to set disable again after the Anchor be rendered again.
*/
Sys.Application.add_load(function (sender, args) {
    if (typeof (document.disableLinkFlag) != 'undefined') {
        delete document.disableLinkFlag;
    }
});