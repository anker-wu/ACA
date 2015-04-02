/**
 * <pre>
 * 
 *  Accela
 *  File: popUpDialog.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PopUpDialog.js 242718 2013-01-17 08:12:14Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *
 * </pre>
 */

//when popup need popup , should setting container.
function popUpDialog(popupBox, hiddenFocus, closeImgURL, container, backFocusId) {
    this.popupBox = popupBox;
    this.hiddenFocus = hiddenFocus; 
    this.showPopUp = showPopUp;
    this.container = container;
    this.cancelPopupBox = cancelPopupBox;
    this.cancel = cancel;
    this.adjustLocation = adjustLocation;
    this.closeImgURL = closeImgURL;
    this.showPopupDialogLoading = showPopupDialogLoading;
    this.hidePopupDialogLoading = hidePopupDialogLoading;
    this.backFocusId = backFocusId;
}

function showPopUp() {
    if (this.closeImgURL != null) {
        createCloseImg(this.popupBox, this.closeImgURL);
    }
    
    var objDeck = createPopupMaskDiv(this.popupBox);

    if(objDeck){
        objDeck.className = "ACA_MaskDiv";
        $(objDeck).css("height", $(document.body).height());
    }

    Sys.UI.DomElement.removeCssClass(this.popupBox, 'ACA_Hide');

    this.adjustLocation();

    var m = $(getParentDocument()).find("#ACA_Dialog_Header_Mask");
    m.removeClass("ACA_Hide");
    m.height($(getParentDocument()).find("#ACA_Dialog_Header").height());

}

function cancelPopupBox() {
    Sys.UI.DomElement.addCssClass(this.popupBox, 'ACA_Hide');
    var m = $(getParentDocument()).find("#ACA_Dialog_Header_Mask");
    m.addClass("ACA_Hide");
}

function cancel() {
    Sys.UI.DomElement.addCssClass(this.popupBox, 'ACA_Hide');
    $get(this.popupBox.id + 'Mask').className = "ACA_Hide";
    var m = $(getParentDocument()).find("#ACA_Dialog_Header_Mask");
    m.addClass("ACA_Hide");

    if (this.backFocusId != null && $get(this.backFocusId) != null) {
        $get(this.backFocusId).focus();
    }
}

function createPopupMaskDiv(popupBox) {
    var maskId = popupBox.id + 'Mask';
    var popupMask = $get(maskId);
    
    if (popupMask == null) {
        popupMask = document.createElement('div');
        popupMask.id = maskId;
        popupMask.className = 'ACA_Hide';
        var boxZindex = $(popupBox).css('z-index');
        $(popupMask).css('z-index', boxZindex - 1);
        document.body.appendChild(popupMask);
    }

    return popupMask;
}

function createCloseImg(popupBox, closeImgURL) {
    var imgParentDivID = popupBox.id + 'ImgParentDiv';
    var imgParentDiv = $get(imgParentDivID);

    if (imgParentDiv == null) {
        imgParentDiv = document.createElement('div');
        imgParentDiv.id = imgParentDivID;
        imgParentDiv.className = 'ACA_AlignRightOrLeft CloseImage ACA_FRight';
        imgParentDiv.innerHTML = GetDivHtml(popupBox.id, closeImgURL);
        
        if (popupBox.firstChild != null) {
            popupBox.insertBefore(imgParentDiv, popupBox.firstChild);
        }
    }
}

function GetDivHtml(popupBoxId, closeImgURL) {
    return "<a href='javascript:ClosePopup(\"" + popupBoxId + "\");' class='NotShowLoading'><img class='ACA_ActionIMG' src='" + closeImgURL + "' alt='close' /></a>";
}

function ClosePopup(popupBoxId) {
    Sys.UI.DomElement.addCssClass($get(popupBoxId), 'ACA_Hide');
    $get(popupBoxId + 'Mask').className = "ACA_Hide";
    var m = $(getParentDocument()).find("#ACA_Dialog_Header_Mask");
    m.addClass("ACA_Hide");
}

function adjustLocation() {
    var win = window.top;

    //check win is cross domain
    if (isCrossDomain()) {
        win = window;
    }
    
    var clientHeight = win.document.compatMode == "BackCompat" ? win.document.body.clientHeight : win.document.documentElement.clientHeight;
    var scrollTop = win.document.compatMode == "BackCompat" ? win.document.body.scrollTop : win.document.documentElement.scrollTop;

    var obox = this.popupBox;

    if (obox != null && obox.className != "ACA_Hide") {

        if (this.container != null) {
            //oLeft = (document.body.clientWidth - obox.offsetWidth) / 2 - this.container.offsetLeft + "px";
            this.container.style.position = "relative";
            oLeft = (document.body.clientWidth - obox.offsetWidth) / 2 + "px";
            oTop = (this.container.offsetHeight - obox.offsetHeight) / 2;
        } else {
            var offsetTop = 0;
            var elem = getParentDocument().getElementById("ACAFrame");
            
            while (elem) {
                offsetTop += elem.offsetTop;
                elem = elem.offsetParent;
            }
            
            var oLeft = (document.body.clientWidth - obox.offsetWidth) / 2 + "px";
            var oTop = (clientHeight - obox.offsetHeight) / 2 + scrollTop - offsetTop;
        }

        oTop = oTop < 0 ? 3 : oTop;

        obox.style.left=oLeft;
        obox.style.top = oTop + "px";

        if (this.hiddenFocus){
            this.hiddenFocus.focus();
        }
    }
}

function showPopupDialogLoading(loadingTitle) {
    var p = new ProcessLoading();
    var loading = $('#' + p.loadingId);
    var loadingMask = $('#' + p.loadingMaskId);

    if ($.exists(loading)) {
        var popupBox = $('#' + this.popupBox.id);

        // change title
        if (loadingTitle != undefined && loadingTitle != null) {
            var p = new ProcessLoading();
            p.setTitle(loadingTitle);
        }

        // adjust the position
        var left = (popupBox.width() - loading.width()) / 2 + popupBox.position().left + 'px';
        var top = (popupBox.height() - loading.height()) / 2 + popupBox.position().top + 'px';
        var boxZindex = popupBox.css('z-index');

        loading.css('left', left).css('top', top);
        loading.css('z-index',  parseInt(boxZindex) + 2);

        loading.show();
        loading.corner();
    }

    if ($.exists(loadingMask)) {
        loadingMask.css('z-index', parseInt(boxZindex) + 1);
        loadingMask.show();
    }
}

function hidePopupDialogLoading() {
    var p = new ProcessLoading();
    p.hideLoading();
}