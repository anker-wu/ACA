 /**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ext-extension.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ext-extension.js 72643 2011-06-30 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
var check = function(regex){
    return regex.test(Ext.userAgent);
}

Ext.SSL_SECURE_URL = "about:blank";
Ext.docMode = document.documentMode;
Ext.isIE9 = Ext.isIE && (check(/msie 9/) && Ext.docMode != 7 && Ext.docMode != 8 || Ext.docMode == 9);
Ext.isIE10 = Ext.isIE && /msie 10/.test(navigator.userAgent.toLowerCase());

if ((typeof Range !== "undefined") && !Range.prototype.createContextualFragment && Ext.isIE9) {
    Range.prototype.createContextualFragment = function (html) {
        var frag = document.createDocumentFragment(),
		div = document.createElement("div");
        frag.appendChild(div);
        div.outerHTML = html;
        return frag;
    };
}

//alert(Ext.Element.prototype.getAttributeNS);
Ext.Element.prototype.getAttributeNS = Ext.isIE ? function (ns, name) {
    var d = this.dom;
    var type = typeof d[ns + ":" + name];
    if (type != 'undefined' && type != 'unknown') {
        return d[ns + ":" + name];
    }

    if (Ext.isIE9 || Ext.isIE10) {
        return d.getAttribute(ns + ":" + name);
    } else {
        return d[name];
    }
} : function (ns, name) {
    var d = this.dom;
    return d.getAttributeNS(ns, name) || d.getAttribute(ns + ":" + name) || d.getAttribute(name) || d[name];
};


/*
Fix the "Permission denied" error in ext framework.
Refer to: http://www.sencha.com/forum/archive/index.php/t-30636.html
Related bug id: 42955 issue 2, 45358 issue 1.
*/
clearInterval(Ext.Element.collectorThreadId);

Ext.Element.garbageCollect = function () {
    if (!Ext.enableGarbageCollector) {
        clearInterval(Ext.Element.collectorThreadId);
        return;
    }
    for (var eid in Ext.Element.cache) {
        var el = Ext.Element.cache[eid], d = el.dom;
        try {
            if (!d || !d.parentNode || (!d.offsetParent && !document.getElementById(eid))) {
                delete Ext.Element.cache[eid];
                if (d && Ext.enableListenerCollection) {
                    E.purgeElement(d);
                }
            }
        } catch (e) {
            delete Ext.Element.cache[eid];
        }
    }
};

Ext.Element.collectorThreadId = setInterval(Ext.Element.garbageCollect, 30000);

// Resolve the Ext shadow issue under IE10.
Ext.override(Ext.Shadow, {
    constructor: function () {
        Ext.apply(this, config);
        if (typeof this.mode != "string") {
            this.mode = this.defaultMode;
        }
        var o = this.offset,
            a = {h: 0},
            rad = Math.floor(this.offset / 2);
        switch (this.mode.toLowerCase()) {
            case "drop":
                a.w = 0;
                a.l = a.t = o;
                a.t -= 1;
                if (Ext.isIE && !Ext.isIE10) { //Does not include IE10
                    a.l -= this.offset + rad;
                    a.t -= this.offset + rad;
                    a.w -= rad;
                    a.h -= rad;
                    a.t += 1;
                }
                break;
            case "sides":
                a.w = (o * 2);
                a.l = -o;
                a.t = o - 1;
                if (Ext.isIE && !Ext.isIE10) {
                    a.l -= (this.offset - rad);
                    a.t -= this.offset + rad;
                    a.l += 1;
                    a.w -= (this.offset - rad) * 2;
                    a.w -= rad + 1;
                    a.h -= 1;
                }
                break;
            case "frame":
                a.w = a.h = (o * 2);
                a.l = a.t = -o;
                a.t += 1;
                a.h -= 2;
                if (Ext.isIE && !Ext.isIE10) {
                    a.l -= (this.offset - rad);
                    a.t -= (this.offset - rad);
                    a.l += 1;
                    a.w -= (this.offset + rad + 1);
                    a.h -= (this.offset + rad);
                    a.h += 1;
                }
                break;
        };

        this.adjusts = a;
    },
    realign: function (l, t, w, h) {
        if (!this.el) {
            return;
        }
        var a = this.adjusts,
            d = this.el.dom,
            s = d.style,
            iea = 0,
            sw = (w + a.w),
            sh = (h + a.h),
            sws = sw + "px",
            shs = sh + "px",
            cn,
            sww;
        s.left = (l + a.l) + "px";
        s.top = (t + a.t) + "px";
        if (s.width != sws || s.height != shs) {
            s.width = sws;
            s.height = shs;
            if (!(Ext.isIE && !Ext.isIE10)) {
                cn = d.childNodes;
                sww = Math.max(0, (sw - 12)) + "px";
                cn[0].childNodes[1].style.width = sww;
                cn[1].childNodes[1].style.width = sww;
                cn[2].childNodes[1].style.width = sww;
                cn[1].style.height = Math.max(0, (sh - 12)) + "px";
            }
        }
    }
});

Ext.Shadow.Pool = function () {
    var p = [],
        markup = Ext.isIE && !Ext.isIE10 ?
            '<div class="x-ie-shadow"></div>' :
            '<div class="x-shadow"><div class="xst"><div class="xstl"></div><div class="xstc"></div><div class="xstr"></div></div><div class="xsc"><div class="xsml"></div><div class="xsmc"></div><div class="xsmr"></div></div><div class="xsb"><div class="xsbl"></div><div class="xsbc"></div><div class="xsbr"></div></div></div>';
    return {
        pull: function () {
            var sh = p.shift();
            if (!sh) {
                sh = Ext.get(Ext.DomHelper.insertHtml("beforeBegin", document.body.firstChild, markup));
                sh.autoBoxAdjust = false;
            }
            return sh;
        },

        push: function (sh) {
            p.push(sh);
        }
    };
} ();

Ext.form.NumberField.prototype.initValue = function () {
    if (!isNaN(this.maxLength) 
        && parseInt(this.maxLength) > 0
        && (this.maxLength != Number.MAX_VALUE)) {
        this.el.dom.maxLength = this.maxLength;
    }
};  