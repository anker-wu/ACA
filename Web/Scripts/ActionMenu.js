/**
* <pre>
* 
*  Accela
*  File: ActionMenu.js
* 
*  Accela, Inc.
*  Copyright (C): 2010-2014
* 
*  Description:
* To deal with inspection action menu .
*  Notes:
* $Id: ActionMenu.js 264505 2014-01-08 13:30:58Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

// inspection action menu begion
if (typeof (actionMenusTimeOutId) == "undefined") {
    var actionMenusTimeOutId;
}

// inspection action menu id of previous click 
if (typeof (preActionMenuId) == "undefined") {
    var preActionMenuId;
}

// act: the action link object, menuId: the container's id for show the action menu
function ShowActionMenus(act, menuId, isRTL) {
    if (typeof preActionMenuId != "undefined") {
        HideActionMenus(preActionMenuId);
    }
    preActionMenuId = menuId;

    //when show the action div, don't need pop up the query window.
    CheckAndSetNoAsk();

    var $menuId = $('#' + menuId);
    $menuId.show();

    var pos = $(act).offset();
    var left = pos.left;
    var parentLeft = 0;
    var menuTop = 0;

    if (isRTL != null && isRTL.toUpperCase() == 'TRUE') {
        parentLeft = left + 12;
        menuTop = "-2";
    }
    else {
        parentLeft = left - $menuId[0].offsetWidth + act.offsetWidth;
        menuTop = "-2";
    }

    if (parentLeft < 0) {
        parentLeft = 2;
    }

    $menuId.parent().css("left", parentLeft + "px");
    $menuId.css("top", menuTop + "px");

    expandOwnIframe();
    clearTimeout(actionMenusTimeOutId);
    actionMenusTimeOutId = null;
    actionMenusTimeOutId = setTimeout('HideActionMenus("' + menuId + '")', 3000);
}

function EnterActionMenus(menuId) {
    clearTimeout(actionMenusTimeOutId);
    actionMenusTimeOutId = null;
}

function ExitActionMenus(menuId) {
    clearTimeout(actionMenusTimeOutId);
    actionMenusTimeOutId = null;
    actionMenusTimeOutId = setTimeout('HideActionMenus("' + menuId + '")', 700);
}

function HideActionMenus(menuId) {
    $('#' + menuId).hide();
    collapseOwnIframe();
}

function SetFocusInForActionMenu() {
    clearTimeout(actionMenusTimeOutId);
    actionMenusTimeOutId = null;
}

function SetBlurOutForActionMenu() {
    ExitActionMenus(preActionMenuId);
}

function expandOwnIframe() {
    var ifra = getOwnIframe(window);

    if (ifra && ifra.scrolling.toLowerCase() == 'no') {
        this._ownIframe = ifra;
        var height = isChrome() ? ifra.contentWindow.document.body.scrollHeight : ifra.contentWindow.document.documentElement.scrollHeight;

        if (ifra.contentWindow.document.height) {
            height = Math.max(height, ifra.contentWindow.document.height);
        }

        if (ifra.offsetHeight < height) {
            this._ownIframeHeight = ifra.height;
            ifra.height = height;
        }
        else {
            this._ownIframeHeight = null;
        }
    }
}

function collapseOwnIframe() {
    if (this._ownIframe && this._ownIframeHeight) {
        this._ownIframe.height = this._ownIframeHeight;
    }
}