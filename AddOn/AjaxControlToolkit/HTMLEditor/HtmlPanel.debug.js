Type.registerNamespace("AjaxControlToolkit.HTMLEditor");

AjaxControlToolkit.HTMLEditor.HtmlPanel = function(element) {
    AjaxControlToolkit.HTMLEditor.HtmlPanel.initializeBase(this, [element]);
    this._onfocus$delegate = Function.createDelegate(this, this._onfocus);
    if(AjaxControlToolkit.HTMLEditor.isIE && document.compatMode != "BackCompat") {
        this._onresize$delegate = Function.createDelegate(this, this._onresize);
    }
}

AjaxControlToolkit.HTMLEditor.HtmlPanel.prototype = {
    _activate: function(value) {
        this._shouldResize = false;
        var element = this.get_element();
        if (AjaxControlToolkit.HTMLEditor.isIE && Sys.Browser.version > 6 && document.compatMode != "BackCompat" && element.parentNode.clientHeight > 0) {
            this._shouldResize = true;
        }
        AjaxControlToolkit.HTMLEditor.HtmlPanel.callBaseMethod(this, "_activate");

        if (AjaxControlToolkit.HTMLEditor.isIE) {
            element.value = "";
            var panel = this;
            setTimeout(function() { element.value = value; }, 0);
        } else {
            element.value = value;
        }

        $addHandlers(element, {
            focus: this._onfocus$delegate
        });
        if (this._shouldResize) {
            $addHandlers(element, {
                resize: this._onresize$delegate
            });
        }
        this._activateFinished();
    },

    _deactivate: function() {
        var element = this.get_element();
        if (this._shouldResize) {
            $common.removeHandlers(element, {
                resize: this._onresize$delegate
            });
        }
        this._shouldResize = false;

        $common.removeHandlers(element, {
            focus: this._onfocus$delegate
        });
        element.value = "";
        AjaxControlToolkit.HTMLEditor.HtmlPanel.callBaseMethod(this, "_deactivate");
    },

    _getContent: function() {
        return this.get_element().value;
    },

    _setContent: function(value) {
        this.get_element().value = value;
        try { // some browsers fail when invisible
            var textarea = this.get_element();
            textarea.focus();
            setTimeout(function() { AjaxControlToolkit.HTMLEditor.setSelectionRange(textarea, 0, 0); }, 0);
        } catch (e) { }
    },

    _focus: function() {
        try { // some browsers fail when invisible
            var textarea = this.get_element();
            textarea.focus();
            setTimeout(function() { AjaxControlToolkit.HTMLEditor.setSelectionRange(textarea, 0, 0); }, 0);
        } catch (e) { }
        var panel = this;
        setTimeout(function() { panel._focused(); }, 0);
    },

    _onfocus: function(e) {
        this._really_focused();
        var textarea = this.get_element();
        setTimeout(function() { textarea.focus(); }, 0);
        return true;
    },

    _onresize: function(e) {
        var element = this.get_element();
        var clientHeight = element.parentNode.clientHeight;
        if (clientHeight > 0) {
            element.style.height = clientHeight + "px";
        }
    }
}

AjaxControlToolkit.HTMLEditor.HtmlPanel.registerClass("AjaxControlToolkit.HTMLEditor.HtmlPanel",AjaxControlToolkit.HTMLEditor.ModePanel);
