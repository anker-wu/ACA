/**
 * <pre>
 * 
 *  Accela
 *  File: GeneralNameList.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 * To deal with some logic for Education.
 *  Notes:
 * $Id: GeneralNameList.js 143792 2009-08-19 01:29:08Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//Show General Name List Pop Up Panel
var currentObj;
var clickInPanel = false;
var generalNameList_doUIChange;
var generalNameList_firstNameID;
var generalNameList_lastNameID;
var generalNameList_beginFocusID;

function EscapeNameListByHotKey(e) {
    if (e.keyCode == 27 && currentObj != null) {
        currentObj.focus();
        currentObj = null;
        generalNameListClose();
    }
}

function selectGeneralNameList(searchKey, nameLists, noRecordsMsg, title, callBack, x, y, needSearch, doUIChange) {
    var div = document.getElementById("divGeneralNameListPanel");
    generalNameList_firstNameID = "generalNameList_firstName";
    generalNameList_lastNameID = "generalNameList_lastName";
    generalNameList_doUIChange = doUIChange;

    if (!div) {
        //Create Pop Up Div
        div = createGeneralNameListPanel(noRecordsMsg, needSearch);
    }

    div.style.display = 'block';

    if ($.global.isRTL) {
        div.style.right = x + "px";
    }
    else {
        div.style.left = x + "px";
    }
    div.style.top = y + "px";

    var lblTitle = document.getElementById('lblGeneralNameListTitle');
    var txtSearchKey = $('#txtGeneralNameListSearchKey');
    var txtNameLists = document.getElementById('txtGeneralNameListDataSource');
    var txtCallBack = document.getElementById('txtGeneralNameListCallBackMethodName');

    txtSearchKey.attr("title", title);
    lblTitle.innerHTML = title;
    txtNameLists.value = nameLists;
    txtCallBack.value = callBack;

    if (needSearch) {
        txtSearchKey.show();
        txtSearchKey.val(searchKey);
        filterGeneralNameList(noRecordsMsg);
    } else {
        if (txtSearchKey) {
            txtSearchKey.hide();
        }

        filterGeneralNameList(noRecordsMsg, searchKey);
    }

    showNameList = false;
    if (needSearch) {
        txtSearchKey.focus();
    }

    if (txtSearchKey.is(":visible")) {
        generalNameList_beginFocusID = txtSearchKey.attr("id");
    }
}

// Hidden Panel
function generalNameListPanelClose() {
    var div = document.getElementsByName("divGeneralNameListPanel");
}

//Filter Name List by Search Key
function filterGeneralNameList(noRecordsMsg, filterValue) {
    if (typeof (filterValue) == 'undefined') {
        //Not pass the 'filterValue' parameter.
        filterValue = $('#txtGeneralNameListSearchKey').val();
    }

    var allNames = document.getElementById('txtGeneralNameListDataSource').value.split('\f');
    var txtCallBack = document.getElementById('txtGeneralNameListCallBackMethodName').value;

    //Filter allNames by filterValue
    var filterNames = new Array();

    if (filterValue && filterValue.length > 0) {
        for (var i = 0; i < allNames.length; i++) {
            var name = allNames[i].split('\b')[0];
            if (name.toLowerCase().indexOf(JsonEncode(filterValue.toLowerCase())) > -1) {
                filterNames[filterNames.length] = allNames[i];
            }
        }
    }
    else {
        filterNames = allNames;
    }

    //Build Examination List
    createGeneralNameListTable(noRecordsMsg, filterNames, txtCallBack);
}

//Create Panel   
function createGeneralNameListPanel(noRecordsMsg, needSearch) {
    var divPanel = document.createElement("div");
    divPanel.setAttribute("id", "divGeneralNameListPanel");
    divPanel.setAttribute("class", "ACA_ListForm4Education");
    divPanel.setAttribute("className", "ACA_ListForm4Education");

    $addHandler(divPanel, 'keydown', Function.createDelegate(this, generalNameListPopup_onkeydown));

    var table = document.createElement("table");
    table.setAttribute("width", "100%");
    table.setAttribute("role", "presentation");

    var tbbody = document.createElement("tbody");
    var tr = document.createElement("tr");
    var td = document.createElement("td");

    var title = document.createElement("label");
    title.setAttribute("id", "lblGeneralNameListTitle");
    title.setAttribute("class", "ACA_Label ACA_Label_FontSize");
    title.setAttribute("className", "ACA_Label ACA_Label_FontSize");
    title.setAttribute("width", "100%");
    title.setAttribute("for", "txtGeneralNameListSearchKey");
    td.appendChild(title);
    tr.appendChild(td);

    td = document.createElement("td");
    td.setAttribute("valign", "top");
    td.setAttribute("align", "right");

    var image = document.createElement("img");
    //image.onclick = generalNameListClose;
    image.setAttribute("style", "cursor: pointer;border-width:0px;");
    image.setAttribute("src", "../app_themes/Default/assets/close.png");
    image.setAttribute("alt", getText.global_js_close_alt);
    image.setAttribute("title", getText.global_js_close_alt);

    var lnk = document.createElement("a");
    $addHandler(lnk, 'click', Function.createDelegate(this, generalNameListCloseButton_onclick));
    lnk.setAttribute("id", "lnkGeneralNameListClose");
    lnk.href = "javascript:void(0);";
    lnk.className = "NotShowLoading";

    lnk.appendChild(image);
    td.appendChild(lnk);
    tr.appendChild(td);
    tbbody.appendChild(tr);

    tr = document.createElement("tr");
    td = document.createElement("td");

    var searchKey = document.createElement("input");
    searchKey.setAttribute("type", "text");
    searchKey.setAttribute("id", "txtGeneralNameListSearchKey");
    searchKey.setAttribute("class", "ACA_NLonger");
    searchKey.setAttribute("className", "ACA_NLonger");
    searchKey.setAttribute("onkeyup", "filterGeneralNameList('" + noRecordsMsg + "')");
    searchKey.setAttribute("onclick", "stopBublle()");
    
    if (needSearch) {
        searchKey.setAttribute('style', 'display:none');
    } 

    td.appendChild(searchKey);
    tr.appendChild(td);


    td = document.createElement("td");
    var nameLists = document.createElement("input");
    nameLists.setAttribute("id", "txtGeneralNameListDataSource");
    nameLists.setAttribute("type", "hidden");
    td.appendChild(nameLists);
    var nameLists = document.createElement("input");
    nameLists.setAttribute("id", "txtGeneralNameListCallBackMethodName");
    nameLists.setAttribute("type", "hidden");
    td.appendChild(nameLists);
    tr.appendChild(td);

    tbbody.appendChild(tr);
    table.appendChild(tbbody);
    divPanel.appendChild(table);

    var divNameList = document.createElement("div");
    divNameList.setAttribute("id", "divGeneralNameList");
    divNameList.setAttribute("class", "ACA_SearchListForm4Education");
    divNameList.setAttribute("className", "ACA_SearchListForm4Education");

    // For Firefox, prevent this DIV to focus.
    divNameList.setAttribute("tabIndex", "-1");

    divPanel.appendChild(divNameList);

    document.body.appendChild(divPanel);

    return divPanel;
}

//Create List
// Note: Need to set tabindex attribute as '0' for Tab moving in Opera.
function createGeneralNameListTable(noRecordsMsg, list, methodName) {
    //Create new Table
    var tb = document.createElement("table");
    tb.setAttribute("role", "presentation");

    if (list != null && list.length > 0 && list[0] != '') {
        for (var i = 0; i < list.length; i++) {
            var row = tb.insertRow(-1);
            var cell = row.insertCell(-1);
            var recordStyle;
            var record = list[i].split('\b');
            var name = list[i];
            var eduDegree = '';

            if (record && record.length == 3) {
                eduDegree = record[2];
            }

            if (record != null && record.length > 0) {
                name = record[0];
            }

            if (i % 2 == 0) {
                recordStyle = 'ACA_TabRow_Single_Line font12px';
            }
            else {
                recordStyle = 'ACA_TabRow_Double_Line font12px';
            }

            var cellInnerHtml = "<a";

            if (i == 0) {
                cellInnerHtml += " id='" + generalNameList_firstNameID + "'";

                if (list.length == 1) {
                    generalNameList_lastNameID = generalNameList_firstNameID;
                }
            } else if (i == (list.length - 1)) {
                cellInnerHtml += " id='" + generalNameList_lastNameID + "'";
            }

            cellInnerHtml += " tabindex='0' title='" + name + eduDegree + "' href='javascript:void(0);' style='color:#666666; cursor:pointer' onclick=\"showNameList=false;generalNameListClose();" + methodName + "('" + JsonDecode(list[i]).replace(/'/g, "\\'") + "');\" class='" + recordStyle + "'><span>" + TruncateString(name) + eduDegree + "</span></a>";
            cell.innerHTML = cellInnerHtml;
        }
    }
    else {
        var row = tb.insertRow(-1);
        var cell = row.insertCell(-1);
        cell.setAttribute("class", "ACA_TabRow_Single_Line font12px");
        cell.innerHTML = "<a id='" + generalNameList_firstNameID + "' tabindex='0' href='javascript:void(0)' onclick='generalNameListClose();' style='text-decoration: none; color:#666666; cursor:pointer'>" + noRecordsMsg + "</a>";
        generalNameList_lastNameID = generalNameList_firstNameID;
    }

    // Remove old Table ,add new
    var div = document.getElementById('divGeneralNameList');

    for (var i = 0; i < div.childNodes.length; i++) {
        div.removeChild(div.childNodes[i]);
    }
    div.appendChild(tb);
}

///When current record is last record after enter tab key then focus current obj
function focusCurrentObj(e) {
    if (!e.shiftKey && e.keyCode == 9 && currentObj != null) {
        currentObj.focus();
        currentObj = null;
        if (window.event) {
            window.event.returnValue = false;
        }
        else {
            e.preventDefault();  //for firefox
        }

        generalNameListClose();
    }
}

// Focus the popup dialog
function focusGeneralNameListPopup(e) {
    if ($("#txtGeneralNameListSearchKey").is(":visible")) {
        generalNameList_beginFocusID = "txtGeneralNameListSearchKey";
    } else {
        generalNameList_beginFocusID = generalNameList_firstNameID;
    }

    FocusObject(e, generalNameList_beginFocusID);
}

var showNameList = false;

//Hidden Pop Up Name List
function generalNameListClose() {
    if (typeof (SetNotAsk) != 'undefined') {
        SetNotAsk(true);
    }

    if (generalNameList_doUIChange != undefined) {
        generalNameList_doUIChange = undefined;
    }

    //hidden Div
    var div = document.getElementById("divGeneralNameListPanel");

    if (div != null) {
        if (showNameList) {
            div.style.display = 'block';
        }
        else {
            div.style.display = 'none';
            if (currentObj != null && $(currentObj).is(":visible")) {
                currentObj.focus();
                currentObj = null;
            }
        }

        showNameList = false;
    }
}

function generalNameListPopup_onkeydown(e) {
    var target = e.target;

    // Tab: 9
    switch (e.keyCode) {
        case 9:
            if (e.shiftKey) {
                if (target.id == generalNameList_beginFocusID) {
                    e.preventDefault();
                    currentObj.focus();
                    generalNameListClose();
                } else if (target.id == "lnkGeneralNameListClose") {
                    FocusObject(e, generalNameList_lastNameID);
                }
            } else {
                if (target.id == generalNameList_lastNameID) {
                    FocusObject(e, "lnkGeneralNameListClose");
                }
            }

            break;
        default:
            EscapeNameListByHotKey(e);
            break;
    }
};

function generalNameListCloseButton_onclick(e) {
    generalNameListClose();

    if (generalNameList_doUIChange) {
        eval(generalNameList_doUIChange + '()');
    }

    e.stopPropagation();
};

//autosearch
(function ($) {
    var delay = 300;
    var searching;

    $.fn.autosearch = function (searchFunc) {
        $(this).keydown(function (e) {
            //handle tab key
            var code = e.keyCode || e.which;

            if (code == '9') {
                if ($("#divGeneralNameListPanel").is(":visible")) {
                    focusGeneralNameListPopup(e);
                }
            } else {
                setSearchTimeOut();
            }
        });

        function setSearchTimeOut() {
            if (searching) {
                clearTimeout(searching);
            }

            searching = setTimeout(function () {
                eval(searchFunc + '(false)');
            }, delay);
        }
    };

    // Click the outside area to close the popup name list panel.
    $(document).click(function (e) {
        if($("#divGeneralNameListPanel").is(":visible")){
            var obj = $(e.target);

            if (obj) {
                clickInPanel = obj.parents("#divGeneralNameListPanel").length > 0;
            }

            if (!clickInPanel) {
                showNameList = false;
                generalNameListClose();
            }
        }
    });

})(jQuery);

function stopBublle() {
    var e = null;

    if (typeof (getEvent) != "undefined") {
        e = getEvent();
    }

    if (e && e.stopPropagation) {
        e.stopPropagation();
    } else if (e && e.cancelBubble) {
        e.cancelBubble = true;
    } else if (window.event) {
        window.event.cancelBubble = true;
    }
}

//*********************** Grading Style ***************************************//
function setGradingStyle(controlName, gradingStyle) {
    //Set Grading Style 
    fillGradingStyleValue(controlName, gradingStyle);

    //Display difference Score form by GradingStyle
    displayGradingStyle(controlName);
}

function fillGradingStyleValue(controlName, gradingStyle) {
    //Fill Grading Style
    var txtGradingStyle = document.getElementById(controlName + '_txtGradingStyle');

    if (txtGradingStyle != null) {
        txtGradingStyle.value = gradingStyle;
    }
}

function displayGradingStyle(controlName) {
    //Display Form by Type
    var divPassFail = document.getElementById(controlName + '_divPassFail');
    var divPassScore = document.getElementById(controlName + '_divPassScore');
    var divPercentageScore = document.getElementById(controlName + '_divPercentageScore');
    var divScore = document.getElementById(controlName + '_divScore');
    var txtgradingStyle = document.getElementById(controlName + '_txtGradingStyle');

    if (divPassFail != null && divPassScore != null && divPercentageScore != null && txtgradingStyle != null) {
        var gradingStyleValue = txtgradingStyle.value;

        if (gradingStyleValue == "passfail") {
            divPassFail.style.display = 'block';
            divPassScore.style.display = 'none';
            divPercentageScore.style.display = 'none';
            divScore.style.display = 'none';
        }
        else if (gradingStyleValue == "score") {
            divPassFail.style.display = 'none';
            divPassScore.style.display = 'block';
            divPercentageScore.style.display = 'none';
            divScore.style.display = 'none';
        }
        else if (gradingStyleValue == "percentage") {
            divPassFail.style.display = 'none';
            divPassScore.style.display = 'none';
            divPercentageScore.style.display = 'block';
            divScore.style.display = 'none';
        }
        else {
            divPassFail.style.display = 'none';
            divPassScore.style.display = 'none';
            divPercentageScore.style.display = 'none';
            divScore.style.display = 'block';
        }
    }
}

Function.prototype.bind = function() {
    var self = this;
    var arg = arguments;
    return function() {
        self.apply(null, arg);
    };
};
//*****************************End Grading Style**********************************//

