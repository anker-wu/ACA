/**
 * <pre>
 * 
 *  Accela
 *  File: ProcessLoading.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ProcessLoading.js 183565 2011-01-24 03:26:44Z ACHIEVO\hans.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *
 * </pre>
 */


function ProcessLoading() {
    this.initControlLoading = initControlLoading;
    this.showLoading = showLoading;
    this.isShowing = isShowing;
    this.hideLoading = hideLoading;
    this.createLoading = createLoading;
    this.setTitle = setTitle;
    this.loadingId = 'divGlobalLoading';
    this.loadingMaskId = 'divGlobalLoadingMask';
    this.loadingImgId = 'divGlobalLoadingImg';
    this.originalScrollTop = 0;
}

function initControlLoading(useParent) {
    var loadingId = this.loadingId;    
    var processLoading = this;

    $('a').not($('.NotShowLoading')).each(function() {
        attachProcessLoading(this, useParent);
    });

    if (isPopup()) {
        var domLoadingMask = $(getParentDocument()).find("#dvACADialogLayerMask");
        domLoadingMask.css('z-index', "999");
        $(getRootDocument4Popup()).find('#' + this.loadingId).hide();
    }
}

function attachProcessLoading(obj, useParent) {
    if ($(obj).attr('disabled') == 'disabled' || $(obj).attr('disabled') == true) {
        return;
    }

    var id = $(obj).attr('id');
    var href = $(obj).attr('href');
    var target = $(obj).attr('target');
    var needValidate = true;

    if (typeof (target) != 'undefined' && $.trim(target) != '' && target != '_parent'
            && target != '_self' && target != '_top') {
        return;
    }

    if ($(obj).hasClass('NeedValidate') == false && ($.isEmptyObject(href) || href.indexOf('javascript:WebForm_DoPostBackWithOptions') == -1)) {
        needValidate = false;
    }

    // href has not function please bind the click.
    if ($.isEmptyObject(href) || href.indexOf('javascript') == -1 || href.indexOf('javascript:void(0)') != -1) {
        $(obj).click(function(e) {
            if (!(e.ctrlKey || e.shiftKey)) {
                processLoading.showLoading(needValidate);
            }
        });
    }
    else {
        if (href.indexOf('showLoading') != -1) {
            return;
        }

        // append the loading function
        if (href.charAt(href.length - 1) == ';') {
            href = href.substr(0, href.length - 1);
        }

        if (useParent) {
            $(obj).attr('href', href + ';parent.ShowLoading();');
        }
        else {
            $(obj).attr('href', href + ';var p = new ProcessLoading();p.showLoading(' + needValidate + ');');
        }
    }
}

function isPopup() {
    return window.location.href.indexOf("isPopup") > 0;
}

// show loading, needValidate: it need validate or not
function showLoading(needValidate) {

    this.createLoading();
    
    if (!needValidate || isAllValidatorsValid()) {
        var parentDom = getParentDocument();
        var domLoading = isPopup() ? $(getRootDocument4Popup()).find("#" + this.loadingId) : $('#' + this.loadingId);
        var domLoadingImg = isPopup() ? $(getRootDocument4Popup()).find("#divGlobalLoadingImg") : $('#divGlobalLoadingImg');
        var domLoadingMask = isPopup() ? $(parentDom).find("#dvACADialogLayerMask") : $('#' + this.loadingMaskId);
        
        domLoading.show();
        try{
            domLoading.corner();
        }
        catch (e) { }
        domLoading.css('z-index', '20000');
        domLoadingMask.css('z-index', '2000');
        domLoadingMask.show();
        
        // if exists vertical scroll bar, the height will be great, so set the body's height.
        var bodyHeight = 0;

        if (!isPopup()) {
            bodyHeight = $(document).height();
        }
        else {
            var ifra = getOwnIframe(parent);

            if (ifra && ifra.offsetHeight) 
            {
                bodyHeight = ifra.offsetHeight;
            }
            else {
                bodyHeight = $(parentDom).height();
            }
        }

        domLoadingMask.height(bodyHeight);

        if (!isPopup()) {
            adjustPosition(domLoading);
        }
        else {
            var top = 0;
            //if the dialog more that two layer, display the loading in the screen center.
            if (!isCrossDomain() && typeof (window.parent) != "undefined" && parent.location && parent.location.href.indexOf("isPopup") > 0) {
                top = ProcessLoadingUtil.getDialogTop();
            }
            else {
                top = parseInt($(parentDom).find('#dvACADialogLayer').css("top"));
                var height = $(parentDom).find('#dvACADialogLayer').height();
                top += height / 2;
            }
            
            $(getRootDocument4Popup()).find('#' + this.loadingId).css("top", top);
        }
        scrollLoading(domLoading);
        
        domLoadingImg.focus();
        domLoadingImg.keydown(function (event) {
            var ev = event || window.event;
            if (ev.keyCode == 9) {
                return false;
            }
        });
    }
}

function isShowing() {
    var domLoading = isPopup() ? $(getRootDocument4Popup()).find("#" + this.loadingId) : $('#' + this.loadingId);

    if ($.exists(domLoading) && domLoading.is(":visible")) {
        return true;
    }

    return false;
}

function scrollLoading(popupBox) {
    if ($.exists(popupBox) == false || popupBox.is(":visible") == false) {
        return;
    }

    if (!isPopup()) {
        try {
            var bodyScrollTop = getParentDocument().body.scrollTop;

            if (getParentDocument().documentElement.scrollTop != 0) {
                bodyScrollTop = getParentDocument().documentElement.scrollTop;
            }
            
            if (bodyScrollTop != this.originalScrollTop) {
                this.originalScrollTop = bodyScrollTop;
                adjustPosition(popupBox);
            }
        }
        catch (ex) {
            var bodyScrollTop = window.document.body.scrollTop;
            
            if (window.document.documentElement.scrollTop != 0) {
                bodyScrollTop = window.document.documentElement.scrollTop;
            }
            
            if (bodyScrollTop != this.originalScrollTop) {
                this.originalScrollTop = bodyScrollTop;
                adjustPosition(popupBox);
            }
        }
    }
    else {
        var top;
        //if the dialog more that two layer, display the loading in the screen center.
        if (!isCrossDomain() && typeof (window.parent) != "undefined" && parent.location && parent.location.href.indexOf("isPopup") > 0) {
            top = ProcessLoadingUtil.getDialogTop();
        }
        else {
            top = parseInt($(getParentDocument()).find('#dvACADialogLayer').css("top"));
            var height = $(getParentDocument()).find('#dvACADialogLayer').height();
            top += height / 2;
        }
        
        $(getRootDocument4Popup()).find('#' + this.loadingId).css("top", top);
    }

    setTimeout(function () { scrollLoading(popupBox); }, 100);
}


function hideLoading(needHide) {
    if ((needHide == undefined || needHide == false) && ProcessLoadingUtil.NotNeedHide) {
        return;
    }

    var domLoading = isPopup() ? $(getRootDocument4Popup()).find("#" + this.loadingId) : $('#' + this.loadingId);
    //var domLoadingMask = $('#' + this.loadingMaskId);
    var domLoadingMask = isPopup() ? $(getParentDocument()).find("#dvACADialogLayerMask") : $('#' + this.loadingMaskId);
    if ($.exists(domLoading)) {
        if (isChrome()) {
            domLoading.fadeOut('fast');
        }
        else {
            domLoading.hide();
        }

        // reset the title
        this.setTitle(getText.global_js_showLoading_title);
    }

    if (!isPopup()) {
        if ($.exists(domLoadingMask)) {
            if (isChrome()) {
                domLoadingMask.fadeOut('fast');
            }
            else {
                domLoadingMask.hide();
            }
        }
    } else {
        if ($.exists(domLoadingMask)) {
            domLoadingMask.css('z-index', "999");
        }
    }
}

function createLoading() {
    var domLoading = isPopup() ? $(getRootDocument4Popup().body).find("#" + this.loadingId) : $('#' + this.loadingId);
    if ($.exists(domLoading) == false) {
        var imgUrl = getText.global_js_showLoading_src;
        var title = getText.global_js_showLoading_title;

        var divContainer = $('<div>').addClass('ACA_Global_Loading').attr('id', this.loadingId).appendTo('body');

        // if direction is right to left, render title first.
        if ($(divContainer).css('direction') == 'rtl') {
            createLoadingTitle(divContainer, title);
            createLoadingImage(divContainer, imgUrl, title);
        }
        else {
            createLoadingImage(divContainer, imgUrl, title);
            createLoadingTitle(divContainer, title);
        }
    }

    if (!isPopup()) {
        var domLoadingMask = $('#' + this.loadingMaskId);
        if ($.exists(domLoadingMask) == false) {
            domLoadingMask = $('<div>');
            domLoadingMask.addClass('ACA_MaskDiv ACA_Hide').attr('id', this.loadingMaskId).appendTo('body');
            $('<iframe>').html(getText.iframe_nonsrc_nonsupport_message).attr('title', getText.masked_iframe_title).addClass('mask_iframe').appendTo(domLoadingMask);
        }
    }
}

function createLoadingImage(pContainer, imgUrl, title) {
    var divImg = $('<div>').addClass('ACA_Global_Loading_ImgDiv').css('float', 'left').appendTo(pContainer);
    var divLink = $('<a>').attr({ id: 'divGlobalLoadingImg', title: title, href: '#' }).appendTo(divImg);
    var img = $('<img>').attr({ src: imgUrl, alt: title }).appendTo(divLink);
}

function createLoadingTitle(pContainer, title) {
    var divTitle = $('<div>').addClass('ACA_Global_Loading_Title').css('float', 'left').appendTo(pContainer);
    var span = $('<span>').addClass('ACA_SmLabel ACA_SmLabel_FontSize').text(title).appendTo(divTitle);
}

function setTitle(title) {
    var domLoading = isPopup() ? $(getRootDocument4Popup()).find("#" + this.loadingId) : $('#' + this.loadingId);

    if ($.exists(domLoading)) {
        $(domLoading).find('.ACA_Global_Loading_Title').children('span').text(title);
        $(domLoading).find('img').attr('alt', title);
        adjustPosition(domLoading);
    }
}

function GetAbsoluteLocation(element) {
    if (arguments.length != 1 || element == null) {
        return null;
    }
    var elmt = element;
    var offsetTop = elmt.offsetTop;
    var offsetLeft = elmt.offsetLeft;
    var offsetHeight = elmt.offsetHeight;
    while (elmt = elmt.offsetParent) {
        // add this judge
        if (elmt.style.position == 'absolute' || elmt.style.position == 'relative'
            || (elmt.style.overflow != 'visible' && elmt.style.overflow != '')) {
            break;
        }
        offsetTop += elmt.offsetTop;
        offsetLeft += elmt.offsetLeft;
    }
    return { absoluteTop: offsetTop, absoluteLeft: offsetLeft, offsetHeight: offsetHeight };
}

function adjustPosition(popupBox) {
    var dom = getParentDocument();    
   
    if (popupBox != undefined && popupBox != null && popupBox.is(':visible')) {
        //Chrome and Opera need get the body.scrollTop.
        var scrollTop = dom.compatMode == "BackCompat" ? dom.body.scrollTop : dom.documentElement.scrollTop || dom.body.scrollTop;
        var clientHeight = dom.compatMode == "BackCompat" ? dom.body.clientHeight : dom.documentElement.clientHeight;
        var oLeft = (document.body.clientWidth - popupBox.width()) / 2 + "px";
        var oTop = 0;

        if (!isCrossDomain()) {
            if (this.frameElement != "undefined" && this.frameElement != null) {
                if (this.frameElement.src.indexOf("isPopup=Y") == -1) {//is not a popup iframe  
                    var iframe = GetAbsoluteLocation(this.parent.document.getElementById(this.frameElement.id));
                    
                    if (window.parent != null && iframe.offsetHeight < (clientHeight - iframe.absoluteTop)) {
                        oTop = (iframe.offsetHeight - popupBox.height()) / 2 + scrollTop;
                    } else {
                        oTop = (clientHeight - iframe.absoluteTop - popupBox.height()) / 2 + scrollTop;
                    }
                } else {
                    oTop = (clientHeight - scrollTop - popupBox.height()) / 2 + scrollTop + "px";
                }
            }
            else {
                oTop = (clientHeight - popupBox.height()) / 2 + "px";
            }
        }
        else{
            oTop = (880 - popupBox.height()) / 2+ scrollTop+ "px";
        }
        
        popupBox.css('left', oLeft);
        popupBox.css('top', oTop);
    }
}

function isAllValidatorsValid() {
    if ((typeof (Page_Validators) != "undefined") && (Page_Validators != null)) {
        for (var i = 0; i < Page_Validators.length; i++) {
            if (!Page_Validators[i].isvalid) {
                return false;
            }
        }
    }
    return true;
}

var ProcessLoadingUtil = new function () {
    this.getDialogTop = getDialogTop;
    this.NotNeedHide = false;
    
    function getDialogTop() {
        var top;
        var rootDom = parent;
        var rootParentTopHeight = 0;

        while (rootDom && rootDom.location && rootDom.location.href.indexOf("isPopup") > 0) {
            //if the root page has parameter "isPopup", it will always loop.
            if (rootDom == rootDom.parent) {
                break;
            }
            
            rootDom = rootDom.parent;
        }

        if (rootDom.parent) {
            rootDom = rootDom.parent;
            rootParentTopHeight = $.exists($(rootDom.document).find("#ACAFrame")) ? $(rootDom.document).find("#ACAFrame").offset().top : 0;
        }

        rootDom = rootDom.document;
        //Chrome and Opera need get the body.scrollTop.
        var scrollTop = rootDom.compatMode == "BackCompat" ? rootDom.body.scrollTop : rootDom.documentElement.scrollTop || rootDom.body.scrollTop;
        var clientHeight = rootDom.compatMode == "BackCompat" ? rootDom.body.clientHeight : rootDom.documentElement.clientHeight;

        top = scrollTop + (clientHeight - $(getRootDocument4Popup()).find('#' + this.loadingId).height()) / 2 - rootParentTopHeight;
        return top;
    }
}