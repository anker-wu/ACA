/**
* <pre>
* 
*  Accela Citizen Access
*  File: HelperBehavior.js
* 
*  Accela, Inc.
*  Copyright (C): 2010-2014
* 
*  Description:
* 
*  Notes:
* $Id: HelperBehavior.js 171222 2010-04-23 17:32:00Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

Type.registerNamespace("AccelaWebControlExtender");

AccelaWebControlExtender.HelperBehavior = function (element) {
    if (!element) {
        //element is null.
        return;
    }

    AccelaWebControlExtender.HelperBehavior.initializeBase(this, [element]);

    this._isRTL = false;
    this._title = null;
    this._closeTitle = null;

    this._helpbtn$delegates = {
        click: Function.createDelegate(this, this.show)
    }

    this._closebtn$delegates = {
        click: Function.createDelegate(this, this.hide)
    }

    this._frame$delegates = {
        click: Function.createDelegate(this, function (e) {
            e.stopPropagation();
            var obj = e.srcElement ? e.srcElement : e.target
            var parentA = this.findParentAnchor(obj);
            if (parentA != null && !parentA.href.startsWith('javascript')) {
                parentA.target = '_blank';
            }
            else {
                e.preventDefault();
            }
        })
    }

    this._ownerdoc$delegates = {
        click: Function.createDelegate(this, this.hide)
    }

    this._hotkey$delegates = {
        keydown: Function.createDelegate(this, this.hot_key)
    }

    this._scrollobj$delegates = {
        scroll: Function.createDelegate(this, this.setpos)
    }
}

AccelaWebControlExtender.HelperBehavior.prototype = {

    initialize: function () {
        if (!this.get_element()) {
            //element is null.
            return;
        }

        AccelaWebControlExtender.HelperBehavior.callBaseMethod(this, "initialize");

        this._isIE = window.navigator.userAgent.indexOf('MSIE') > -1;
        this._isFirefox = window.navigator.userAgent.indexOf('Firefox') > -1;
        this._isSafari = window.navigator.userAgent.indexOf('Safari') > -1;
        this._hasDocType = this._isIE ? document.firstChild.nodeValue != null : document.doctype != null;

        this._isShowing = false;

        this._frame = $get('divHelpFrame');
        if (!this._frame) {
            this._frame = this.create();
        }

        this._content = $get('divHelpContent');
        this._helpbtn = $get(this.get_element().id + '_help');
        this._sublabel = $get(this.get_element().id + '_sub_label');
        this._closebtn = $get('btnHelpClose');
        this._ownerdoc = this._frame.ownerDocument;
        this._subContentLnk = null;
        this._subContent = null;
        this._helpHeadLnk = $get('lnkHelpHead');
        this._helpHeadLnk.innerHTML = this.get_title();
        this._overPanel4Safari = null;
        if (this._isSafari) {
            this._overPanel4Safari = $get('divOverPanel4Safari');
        }

        this._scrollObjs = [];

        this._pt_tl = $get('helpPtTopLeft'); //pointer cell: top left
        this._pt_tr = $get('helpPtTopRight'); //pointer cell: top right
        this._pt_bl = $get('helpPtBottomLeft'); //pointer cell: bottom left
        this._pt_br = $get('helpPtBottomRight'); //pointer cell: bottom right
        this._bg_ml = 'url(<%=WebResource("Accela.Web.Controls.Helper.help-bg-ml.png")%>) repeat-y'; //cell's background image: middle left
        this._bg_mr = 'url(<%=WebResource("Accela.Web.Controls.Helper.help-bg-mr.png")%>) repeat-y'; //cell's background image: middle right
        this._bg_pl = 'url(<%=WebResource("Accela.Web.Controls.Helper.help-bg-pl.png")%>) no-repeat'; //cell's background image: left pointer
        this._bg_pr = 'url(<%=WebResource("Accela.Web.Controls.Helper.help-bg-pr.png")%>) no-repeat'; //cell's background image: right pointer
        this._td_ml = $get('helpTdMiddleLeft'); //cell: the left side of the cell contents
        this._td_mr = $get('helpTdMiddleRight'); //cell: the right side cell of the contents
        this._fontsizeHandler = $get(this._frame.id + '_fsh');
        this._lastAnchor = null;
        if (this._helpbtn != null) {
            $addHandlers(this._helpbtn, this._helpbtn$delegates);
        }
    },

    dispose: function () {
        if (this._helpbtn) {
            $common.removeHandlers(this._helpbtn, this._helpbtn$delegates);
            this._helpbtn = null;
        }

        this._closebtn = null;
        this._frame = null;

        AccelaWebControlExtender.HelperBehavior.callBaseMethod(this, "dispose");
    },

    get_isRTL: function () {
        return this._isRTL;
    },

    set_isRTL: function (value) {
        if (this._isRTL != value) {
            this._isRTL = value;
            this.raisePropertyChanged("isRTL");
        }
    },

    get_title: function () {
        return this._title;
    },

    set_title: function (value) {
        if (this._title != value) {
            this._title = value;
            this.raisePropertyChanged("title");
        }
    },

    get_closeTitle: function () {
        return this._closeTitle;
    },

    set_closeTitle: function (value) {
        if (this._closeTitle != value) {
            this._closeTitle = value;
            //this.raisePropertyChanged("closeTitle");
        }
    },

    create: function () {
        var result = document.createElement('div');
        result.id = 'divHelpFrame';
        result.className = 'ACA_Help_Frame ACA_Hide';
        if (this._isRTL) {
            Sys.UI.DomElement.addCssClass(result, 'ACA_Help_Frame_RTL');
        }
        else {
            Sys.UI.DomElement.addCssClass(result, 'ACA_Help_Frame_LTR');
        }

        if (this._isSafari) {
            result.innerHTML = '<div id="divOverPanel4Safari" class="SafariOverPanel"></div>';
        }
        result.innerHTML +=
            '<a id="btnHelpClose" href="javascript:void(0);" onclick = "return false;" class="closebutton NotShowLoading" title="' + this._closeTitle + '"><img src="<%=WebResource("Accela.Web.Controls.Helper.help-close.png")%>" alt="' + this._closeTitle + '" /></a>' +
            '<table role="presentation" cellpadding="0" cellspacing="0" dir="ltr">' +
            '        <tr>' +
            '            <td class="bgTopLeft"></td>' +
            '            <td class="bgTopMiddle"></td>' +
            '            <td class="bgTopRight"></td>' +
            '        </tr>' +
            '        <tr>' +
            '            <td id="helpPtTopLeft" class="bgPointer"></td>' +
            '            <td><span  ' + (this._isRTL ? 'style="float:right;"' : '') + '><a href="javascript:void(0);" class="title NotShowLoading" id="lnkHelpHead"></a></span></td>' +
            '            <td id="helpPtTopRight" class="bgPointer"></td>' +
            '        </tr>' +
            '        <tr>' +
            '            <td id="helpTdMiddleLeft" class="bgMiddleLeft" ></td>' +
            '            <td rowspan="2" valign="top">' +
            '                <div id="divHelpContent" class="content" ' + (this._isRTL ? 'dir="rtl"' : 'dir="ltr"') + '></div>' +
            '            </td>' +
            '            <td id="helpTdMiddleRight" class="bgMiddleRight"></td>' +
            '        </tr>' +
            '        <tr>' +
            '            <td id="helpPtBottomLeft" class="bgPointer"></td>' +
            '            <td id="helpPtBottomRight" class="bgPointer"></td>' +
            '        </tr>' +
            '        <tr>' +
            '            <td class="bgBottomLeft"></td>' +
            '            <td class="bgBottomMiddle"></td>' +
            '            <td class="bgBottomRight"></td>' +
            '        </tr>' +
            '</table>';

        //---Detecting font size change---
        var fsh = document.createElement('div');
        fsh.id = result.id + '_fsh';
        fsh.style.fontSize = '1.1em';
        fsh.style.visibility = 'hidden';
        fsh.innerHTML = 'text for font size change';
        this._fontsizeHandler = fsh;
        result.appendChild(fsh);
        //--------------------------------
        document.body.appendChild(result);
        return result;
    },

    show: function (e) {
        e.stopPropagation();
        e.preventDefault();

        if (this._frame._bhvObj) {
            this._frame._bhvObj.hide();
        }
        this.clearScrollBind();

        //show help frame & help message
        this._frame.className = this._frame.className.replace('ACA_Hide', 'ACA_Show');
        this.loadcontent();

        if (this._isSafari && this._overPanel4Safari) {
            this._overPanel4Safari.style.top = (this._frame.offsetHeight - 28) + 'px';
        }

        //set position of the close button
        var btnHelpClose = this._closebtn;
        if (!this._isRTL) {
            btnHelpClose.style.left = '246px'; //ltr
        }
        else {
            btnHelpClose.style.left = '17px'; //rtl
        }

        this._posfirst = true;
        this.setpos();
        this.expandOwnerIframe();
        this._helpHeadLnk.focus();

        if (this._fontsizeHandler) {
            this._fontsizeHandler._bhvObj = this;
            $addHandler(this._fontsizeHandler, 'resize', this._fontsizeHandler._bhvObj.reloadcontent);
        }
        this._frame._bhvObj = this;

        //----------Bind Handlers----------
        $addHandlers(this._closebtn, this._closebtn$delegates);
        $addHandlers(this._frame, this._frame$delegates);
        $addHandlers(this._frame, this._hotkey$delegates);
        if (this._ownerdoc) {
            $addHandlers(this._ownerdoc, this._ownerdoc$delegates);
        }
        //---------------------------------

        this._isShowing = true;
        return false;
    },

    loadcontent: function () {
        this._frame.style.height = '150px';
        this._content.style.width = '246px';
        this._content.style.height = '112px';
        this._td_ml.style.height = '94px';
        this._td_mr.style.height = '94px';

        this._subContentLnk = document.createElement('a');
        this._subContentLnk.className = 'subcontentlnk';
        this._subContentLnk.href = 'javascript:void(0);';
        this._subContentLnk.onclick = 'return false;';
        this._content.innerHTML = '';
        this._content.appendChild(this._subContentLnk);

        this._subContent = document.createElement('div');
        this._subContent.className = 'subcontent';

        if (this._sublabel) {
            this._subContent.innerHTML = this._sublabel.innerHTML;
            this._subContentLnk.appendChild(this._subContent);
        }

        this._lastAnchor = null;
        var anchors = this._subContent.getElementsByTagName('A');
        if (anchors != null && anchors.length > 0) {
            this._lastAnchor = anchors[anchors.length - 1];
            if (this._isFirefox) {
                this._lastAnchor.style.outline = 'none';
            }
        }

        $addHandlers(this._subContentLnk, this._hotkey$delegates);

        //Determine whether to expand height of div from 150px to 350px
        this.expand();
    },

    clearcontent: function () {
        if (this._subContentLnk) {
            $common.removeHandlers(this._subContentLnk, this._hotkey$delegates);
            this._subContentLnk = null;
            this._subContent = null;
            this._content.innerHTML = '';
            return true;
        }
        return false;
    },

    reloadcontent: function (e) {
        var obj = e.srcElement ? e.srcElement : e.target;
        if (obj._bhvObj) {
            obj._bhvObj.clearcontent();
            obj._bhvObj.loadcontent();
        }
    },

    expand: function () {
        if (this._frame.offsetHeight != 350 && this._subContent.offsetHeight > this._content.offsetHeight) {
            this._frame.style.height = '350px';
            this._content.style.height = '312px';
            this._td_ml.style.height = '294px';
            this._td_mr.style.height = '294px';
            if (this._content.offsetWidth > this._content.clientWidth) {
                //has scroll bar
                this._subContentLnk.style.width = this._subContentLnk.offsetWidth - (this.getScrollbarWidth() - 8) + 'px';
                this._subContent.style.width = this._subContentLnk.style.width;
                var scrollBarSide = this.getScrollbarSide();
                //leave 5 pixels space in scrollbar side.
                this._subContent.style.width = (this._subContent.offsetWidth - 5) + 'px';

                if ((this._isFirefox || this._isSafari) && this._isRTL && scrollBarSide == 0) {
                    //Firefox or Safari,and is RTL,and scrollbar at right side
                    this._subContentLnk.style.marginLeft = '8px';
                    this._subContentLnk.style.marginRight = '0px';
                    this._subContent.style.marginRight = '5px';
                }
                else if (this._isFirefox && !this._isRTL && scrollBarSide == 1) {
                    //Firefox,and is LTR,and scrollbar at left side
                    this._subContentLnk.style.marginLeft = '0px';
                    this._subContentLnk.style.marginRight = '8px';
                }
                else if (scrollBarSide == 1) {
                    this._subContent.style.marginLeft = '5px';
                }
            }
            return true;
        }
        return false;
    },

    setpos: function () {
        var helpBtnPos = this.getOffsetPos(this._helpbtn);
        var ptOffset = 19; //:10+18/2

        //Special case for Firefox on RTL.
        if (!this._frame._showAgain && this._isFirefox && this._isRTL) {
            helpBtnPos.l += this.getScrollbarWidth();
            this._frame._showAgain = true;
        }

        var tl_space = this.getTLSpace(this._helpbtn, window);
        var br_space = this.getBRSpace(this._helpbtn, window);
        var t_space = tl_space.ts;
        var l_space = tl_space.ls;
        var r_space = br_space.rs;
        var b_space = br_space.bs;

        var t_havespace = t_space >= (this._frame.offsetHeight - ptOffset - this._helpbtn.offsetHeight / 2);
        var l_havespace = l_space >= this._frame.offsetWidth;
        var b_havespace = b_space >= (this._frame.offsetHeight - ptOffset - this._helpbtn.offsetHeight / 2);
        var r_havespace = r_space >= this._frame.offsetWidth;

        //set position of the help frame
        var direction = !this._isRTL ? 4 : 3;
        /* direction
        1: up-left;
        2: up-right;
        3: down-left;
        4: down-right;
        |------|   |------|
        |   1  |   |  2   |
        |      |   |      |
        |______> ? <______|

        |------| ? |------|
        |   3  >   <  4   |
        |      |   |(def) |
        |______|   |______|
        */
        if (!this._isRTL) {
            if (r_havespace || !l_havespace) {
                if (b_havespace || !t_havespace) {
                    direction = 4;
                }
                else {
                    direction = 2;
                }
            }
            else {
                if (b_havespace || !t_havespace) {
                    direction = 3;
                }
                else {
                    direction = 1;
                }
            }
        }
        else {
            if (l_havespace || !r_havespace) {
                if (b_havespace || !t_havespace) {
                    direction = 3;
                }
                else {
                    direction = 1;
                }
            }
            else {
                if (b_havespace || !t_havespace) {
                    direction = 4;
                }
                else {
                    direction = 2;
                }
            }
        }

        switch (direction) {
            case 1: //up-left
                this._pt_tl.style.background = this._bg_ml;
                this._pt_tr.style.background = this._bg_mr;
                this._pt_bl.style.background = this._bg_ml;
                this._pt_br.style.background = this._bg_pr;
                this._frame.style.top = helpBtnPos.t - this._frame.offsetHeight + ptOffset + parseInt(this._helpbtn.offsetHeight / 2) + 'px';
                this._frame.style.left = helpBtnPos.l - this._frame.offsetWidth + 'px';
                break;
            case 2: //up-right
                this._pt_tl.style.background = this._bg_ml;
                this._pt_tr.style.background = this._bg_mr;
                this._pt_bl.style.background = this._bg_pl;
                this._pt_br.style.background = this._bg_mr;
                this._frame.style.top = helpBtnPos.t - this._frame.offsetHeight + ptOffset + parseInt(this._helpbtn.offsetHeight / 2) + 'px';
                this._frame.style.left = helpBtnPos.l + this._helpbtn.offsetWidth + 'px';
                break;
            case 3: //down-left
                this._pt_tl.style.background = this._bg_ml;
                this._pt_tr.style.background = this._bg_pr;
                this._pt_bl.style.background = this._bg_ml;
                this._pt_br.style.background = this._bg_mr;
                this._frame.style.top = helpBtnPos.t - ptOffset + parseInt(this._helpbtn.offsetHeight / 2) + 'px';
                this._frame.style.left = helpBtnPos.l - this._frame.offsetWidth + 'px';
                break;
            case 4: //down-right
                this._pt_tl.style.background = this._bg_pl;
                this._pt_tr.style.background = this._bg_mr;
                this._pt_bl.style.background = this._bg_ml;
                this._pt_br.style.background = this._bg_mr;
                this._frame.style.top = helpBtnPos.t - ptOffset + parseInt(this._helpbtn.offsetHeight / 2) + 'px';
                this._frame.style.left = helpBtnPos.l + this._helpbtn.offsetWidth + 'px';
                break;
        }

        if (this._posfirst) {
            this._posfirst = false;
        }
    },

    hide: function () {
        //e.stopPropagation();
        //e.preventDefault();

        this.clearScrollBind();

        if (this.clearcontent()) {
            this._frame.className = this._frame.className.replace('ACA_Show', 'ACA_Hide');
            this.collapseOwnerIframe();
        }

        if (this._fontsizeHandler && this._fontsizeHandler._bhvObj) {
            $removeHandler(this._fontsizeHandler, 'resize', this._fontsizeHandler._bhvObj.reloadcontent);
            this._fontsizeHandler._bhvObj = null;
        }

        this._frame._bhvObj = null;

        //----------Unbind Handlers----------
        if (this._isShowing) {
            if (this._closebtn) {
                $common.removeHandlers(this._closebtn, this._closebtn$delegates);
            }

            if (this._frame) {
                $common.removeHandlers(this._frame, this._frame$delegates);
                $common.removeHandlers(this._frame, this._hotkey$delegates);
            }

            if (this._ownerdoc) {
                $common.removeHandlers(this._ownerdoc, this._ownerdoc$delegates);
            }
        }
        //-----------------------------------

        this._isShowing = false;
        //return false;
    },

    expandOwnerIframe: function () {
        var ifra = this.getOwnerIframe(window);

        if (ifra && ifra.scrolling.toLowerCase() == 'no') {
            this._ownerIframe = ifra;
            this._ownerIframeHeight = ifra.height;
            if (this._isSafari) {
                if (ifra.offsetHeight < ifra.contentWindow.document.body.scrollHeight) {
                    ifra.height = ifra.contentWindow.document.body.scrollHeight;
                }
            }
            else {
                if (ifra.offsetHeight < ifra.contentWindow.document.documentElement.scrollHeight) {
                    ifra.height = ifra.contentWindow.document.documentElement.scrollHeight;
                }
            }
        }
    },

    collapseOwnerIframe: function () {
        if (this._ownerIframe && this._ownerIframeHeight) {
            this._ownerIframe.height = this._ownerIframeHeight;
        }
    },

    hot_key: function (e) {
        var target = e.srcElement ? e.srcElement : e.target;

        if (!e.ctrlKey && !e.shiftKey && !e.altKey && e.keyCode == 9) {
            if (target === this._helpHeadLnk) {
                if (this._subContentLnk) {
                    try { this._subContentLnk.focus(); } catch (exp) { ; }
                }
                e.preventDefault();
            }

            if (this._lastAnchor != null) {
                if (target === this._lastAnchor) {
                    this.hide();
                    this._helpbtn.focus();
                    e.preventDefault();
                }
            }
            else if (target === this._subContentLnk) {
                this.hide();
                this._helpbtn.focus();
                e.preventDefault();
            }

            e.stopPropagation();
        }

        if (e.keyCode == 27) {
            this.hide();
            e.preventDefault();
        }
    },

    clearScrollBind: function () {
        while (this._scrollObjs && this._scrollObjs.length > 0) {
            var scrollobj = this._scrollObjs.pop();
            $common.removeHandlers(scrollobj, this._scrollobj$delegates);
        }
    },

    getOffsetPos: function (obj) {
        var top = obj.offsetTop;
        var left = obj.offsetLeft;

        var parentobj = obj.offsetParent;
        while (parentobj) {
            if (parentobj.offsetTop) {
                top += parentobj.offsetTop;
            }

            if (parentobj.offsetLeft) {
                left += parentobj.offsetLeft;
            }

            parentobj = parentobj.offsetParent;
        }

        var parentnode = obj.parentNode;
        while (parentnode && parentnode != parentobj && parentnode.tagName) {
            if (parentnode.tagName == 'BODY') {
                break;
            }

            if (parentnode.style.position.toLowerCase() == 'fixed') {
                continue;
            }

            if (parentnode.scrollTop) {
                top -= parentnode.scrollTop;
            }

            if (parentnode.scrollLeft) {
                left -= parentnode.scrollLeft;
            }

            if (this._posfirst && (parentnode.scrollWidth > parentnode.clientWidth || parentnode.scrollHeight > parentnode.clientHeight)) {
                $addHandlers(parentnode, this._scrollobj$delegates);
                this._scrollObjs.push(parentnode);
            }

            parentnode = parentnode.parentNode;
        }
        return { t: top, l: left };
    },

    getScrollbarWidth: function () {
        var div = document.createElement('div');
        div.innerHTML = '<br/><br/><br/>';
        div.style.border = "0px";
        div.style.margin = "0px";
        div.style.padding = "0px";
        div.style.width = '50px';
        div.style.height = '50px';
        div.style.overflow = 'scroll';
        div.style.display = 'block';
        document.body.appendChild(div);
        var width = div.offsetWidth - div.clientWidth;
        document.body.removeChild(div);
        div = null;
        return width;
    },

    getScrollbarSide: function () {
        //0:right | 1:left
        var div = document.createElement('div');
        div.style.border = "0px";
        div.style.margin = "0px";
        div.style.padding = "0px";
        div.style.overflow = 'scroll';
        div.style.display = 'block';
        document.body.appendChild(div);
        var subdiv = document.createElement('div');
        subdiv.style.display = 'block';
        div.appendChild(subdiv);
        var pos = 0;
        if ((subdiv.offsetLeft - div.offsetLeft) > 0) {
            pos = 1;
        }
        else {
            pos = 0;
        }
        document.body.removeChild(div);
        return pos;
    },

    getOwnerIframe: function (w) {
        var result = null;
        try {
            var iframeObjs = w.parent.document.getElementsByTagName('iframe');
            for (var i = 0; i < iframeObjs.length; i++) {
                var fra = iframeObjs.item(i);
                if (fra.contentWindow == w) {
                    result = fra;
                    break;
                }
            }
        }
        catch (ex) {
        
        }
        return result;
    },

    getVisualWidth: function (doc) {
        var width = 0;
        if (this._hasDocType && (this._isFirefox || this._isSafari)) {
            width = doc.body.offsetWidth;
        }
        else {
            width = doc.body.clientWidth;
        }

        return width;
    },

    getVisualHeight: function (doc) {
        var height = 0;
        height = doc.body.clientHeight;

        return height;
    },

    getRectPos: function (obj) {
        var rect = obj.getBoundingClientRect();
        return { t: rect.top, l: rect.left };
    },

    // top/left space
    getTLSpace: function (obj, win) {
        var rect = this.getRectPos(obj);
        var ts = rect.t;
        var ls = rect.l;
        try {
            while (win != win.parent) {
                var pwin = win.parent;
                var iframeObjs = pwin.document.getElementsByTagName('iframe');
                for (var i = 0; i < iframeObjs.length; i++) {
                    var fra = iframeObjs.item(i);
                    if (fra.contentWindow == win) {
                        var fraRect = this.getRectPos(fra);
                        if (fraRect.t < 0) {
                            ts += fraRect.t;
                        }

                        if (fraRect.l < 0) {
                            ls += fraRect.l;
                        }
                    }
                    else {
                        continue;
                    }
                }
                win = win.parent;
            }
        }
        catch (ex) {

        }

        return { ts: ts, ls: ls };
    },

    // bottom/right space
    getBRSpace: function (obj, win) {
        var tlspace = this.getTLSpace(obj, win);
        var bs = this.getVisualHeight(win.document) - (tlspace.ts + obj.offsetHeight);
        var rs = this.getVisualWidth(win.document) - (tlspace.ls + obj.offsetWidth);
        try {
            while (win != win.parent) {
                var pwin = win.parent;
                var iframeObjs = pwin.document.getElementsByTagName('iframe');
                for (var i = 0; i < iframeObjs.length; i++) {
                    var fra = iframeObjs.item(i);
                    if (fra.contentWindow == win) {
                        var fraTLSpace = this.getTLSpace(fra, pwin);
                        var fraBRSpace = this.getBRSpace(fra, pwin);
                        if (fraTLSpace.ts < 0) {
                            bs += fraTLSpace.ts;
                        }

                        if (fraTLSpace.ls < 0) {
                            rs += fraTLSpace.ls;
                        }

                        if (fraBRSpace.bs < 0) {
                            bs += fraBRSpace.bs;
                        }

                        if (fraBRSpace.rs < 0) {
                            rs += fraBRSpace.rs;
                        }
                    }
                    else {
                        continue;
                    }
                }
                win = win.parent;
            }
        }
        catch (ex) {
        }

        return { bs: bs, rs: rs };
    },

    findParentAnchor: function (elt) {
        var pA = null;
        if (elt.tagName.toUpperCase() == 'A') {
            pA = elt;
        }
        else {
            while (elt != this._frame && elt.parentNode != this._frame && elt != this._subContentLnk && elt.parentNode != this._subContentLnk) {
                if (elt.parentNode.tagName.toUpperCase() == 'A') {
                    pA = elt.parentNode;
                    break;
                }

                elt = elt.parentNode;
            }
        }

        return pA;
    },

    //getScrollPos is unused
    getScrollPos: function (w) {
        var scrollTop = 0;
        var scrollLeft = 0;
        if (this._hasDocType && (this._isIE || this._isFirefox)) {
            scrollTop = w.document.documentElement.scrollTop;
            scrollLeft = w.document.documentElement.scrollLeft;
        }
        else {
            scrollTop = w.document.body.scrollTop;
            scrollLeft = w.document.body.scrollLeft;
        }
        return { t: scrollTop, l: scrollLeft };
    }
}

AccelaWebControlExtender.HelperBehavior.registerClass("AccelaWebControlExtender.HelperBehavior", AjaxControlToolkit.BehaviorBase);
