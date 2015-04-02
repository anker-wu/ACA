Type.registerNamespace("AjaxControlToolkit.HTMLEditor");

AjaxControlToolkit.HTMLEditor.Editor = function(element) {
    AjaxControlToolkit.HTMLEditor.Editor.initializeBase(this, [element]);
    
    this._editPanel = null;
    this._changingToolbar = null;
    if(AjaxControlToolkit.HTMLEditor.isIE && Sys.Browser.version == 8 && document.compatMode != "BackCompat") {
        this._onresize$delegate = Function.createDelegate(this, this._onresize);
    }
}

AjaxControlToolkit.HTMLEditor.Editor.prototype = {
    get_autofocus: function() {
        return this._editPanel.get_autofocus();
    },
    set_autofocus: function(value) {
        this._editPanel.set_autofocus(value);
    },

    get_content: function() {
        return this._editPanel.get_content();
    },
    set_content: function(value) {
        this._editPanel.set_content(value);
    },

    get_activeMode: function() {
        return this._editPanel.get_activeMode();
    },
    set_activeMode: function(value) {
        this._editPanel.set_activeMode(value);
    },

    get_editPanel: function() {
        return this._editPanel;
    },
    set_editPanel: function(value) {
        this._editPanel = value;
    },

    get_changingToolbar: function() {
        return this._changingToolbar;
    },
    set_changingToolbar: function(value) {
        this._changingToolbar = value;
    },

    add_propertyChanged: function(handler) {
        this._editPanel.add_propertyChanged(handler);
    },
    remove_propertyChanged: function(handler) {
        this._editPanel.remove_propertyChanged(handler);
    },

    initialize: function() {
        AjaxControlToolkit.HTMLEditor.Editor.callBaseMethod(this, "initialize");

        var element = this.get_element();
        var oldStyle = element.className;

        Sys.UI.DomElement.removeCssClass(element, oldStyle);
        Sys.UI.DomElement.addCssClass(element, "ajax__htmleditor_editor_base");
        Sys.UI.DomElement.addCssClass(element, oldStyle);

        if (!AjaxControlToolkit.HTMLEditor.isIE && document.compatMode != "BackCompat") {
            this.get_element().style.height = "100%";
        }

        if (AjaxControlToolkit.HTMLEditor.isIE && Sys.Browser.version == 8 && document.compatMode != "BackCompat") {
            if (this.get_changingToolbar() != null) {
                this._saved_set_activeEditPanel = this.get_changingToolbar().set_activeEditPanel;
                this._saved_disable = this.get_changingToolbar().disable;

                this.get_changingToolbar().set_activeEditPanel = Function.createDelegate(this, this._set_activeEditPanel);
                this.get_changingToolbar().disable = Function.createDelegate(this, this._disable);
            }
        }
    },

    _set_activeEditPanel: function(par) {
        var changingToolbar = this.get_changingToolbar().get_element();
        var editPanel = this.get_editPanel().get_element();
        var height1 = changingToolbar.parentNode.clientHeight;
        (Function.createDelegate(this.get_changingToolbar(), this._saved_set_activeEditPanel))(par);
        var height2 = changingToolbar.parentNode.clientHeight;

        var clientHeight = editPanel.parentNode.clientHeight;
        if (clientHeight > 0) {
            editPanel.style.height = clientHeight - (height2 - height1) + "px";
        }
    },

    _disable: function(par) {
        var changingToolbar = this.get_changingToolbar().get_element();
        var editPanel = this.get_editPanel().get_element();
        var height1 = changingToolbar.clientHeight;
        (Function.createDelegate(this.get_changingToolbar(), this._saved_disable))(par);
        var height2 = changingToolbar.clientHeight;

        var clientHeight = editPanel.parentNode.clientHeight;
        if (clientHeight > 0) {
            editPanel.style.height = clientHeight - (height2 - height1) + "px";
        }
    },

    dispose: function() {
        if (AjaxControlToolkit.HTMLEditor.isIE && Sys.Browser.version == 8 && document.compatMode != "BackCompat") {
            this.get_changingToolbar().set_activeEditPanel = this._saved_set_activeEditPanel;
            this.get_changingToolbar().disable = this._saved_disable;
        }

        AjaxControlToolkit.HTMLEditor.Editor.callBaseMethod(this, "dispose");
    },

    _onresize: function(e) {
        var changingToolbar = this.get_changingToolbar().get_element();
        var editPanel = this.get_editPanel().get_element();
        var clientHeight = editPanel.parentNode.clientHeight;
        if (clientHeight > 0) {
            editPanel.style.height = clientHeight - (changingToolbar.clientHeight - this._changingToolbarHeight) + "px";
        }
        this._changingToolbarHeight = changingToolbar.clientHeight;
    }
}

AjaxControlToolkit.HTMLEditor.Editor.registerClass("AjaxControlToolkit.HTMLEditor.Editor", Sys.UI.Control);

AjaxControlToolkit.HTMLEditor.Editor.MidleCellHeightForIE = function(table, row) {
    var height = "100%";
    if (AjaxControlToolkit.HTMLEditor.isIE && document.compatMode != "BackCompat") {
        try {
            var decrease = 2;
            for (var i = 0; i < table.rows.length; i++) {
                if (table.rows[i] != row) {
                    decrease += (table.rows[i].offsetHeight + 1);
                }
            }
            height = ((table.clientHeight - decrease) / table.clientHeight * 100) + '%';
        } catch (e) {
            height = "";
        }
    }
    return height;
}
