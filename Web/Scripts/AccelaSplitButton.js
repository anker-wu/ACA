/**
* <pre>
* 
*  Accela Citizen Access
*  File: AccelaSplitButton.js
* 
*  Accela, Inc.
*  Copyright (C): 2011-2014
* 
*  Description:
* 
*  Notes:
* $Id: AccelaSplitButton.js 270886 2014-05-06 08:53:10Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

function SplitButton_ShowMenu(menuId, btnId) {
    var divMenu = $get(menuId);
    $(divMenu).slideToggle('fast');
    var divBtn = $get(btnId);
    var pos = $(divBtn).offset();

    if ($.global.isRTL) {
        divMenu.style.left = pos.left + divBtn.offsetWidth - divMenu.offsetWidth + 'px';
    }
    else {
        divMenu.style.left = pos.left + 'px';
    }

    divMenu.style.top = pos.top + divBtn.offsetHeight + 'px';    
}

function SplitButton_MenuFocus(menuId) {
    var divMenu = $get(menuId);
    if (divMenu != null) {
        clearTimeout(divMenu.timerId);
        divMenu.timerId = null;
    }
}

function SplitButton_MenuBlur(menuId) {
    var divMenu = $get(menuId);
    if (divMenu != null) {
        clearTimeout(divMenu.timerId);
        divMenu.timerId = null;
        divMenu.timerId = setTimeout(function () {
            $(divMenu).hide();
        }, 700);
    }
}

function SplitButton_HideMenu(menuId) {
    $("#" + menuId).hide();
}