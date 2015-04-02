Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector.initializeBase(this, [element]);
    
    this._fixedColorButton = null;
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector.prototype = {

    get_fixedColorButton: function() {
        return this._fixedColorButton;
    },

    set_fixedColorButton: function(value) {
        this._fixedColorButton = value;
    },

    callMethod: function() {
        if (!AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector.callBaseMethod(this, "callMethod")) return false;
//        var button = this;
//        setTimeout(function() { button.openPopup(Function.createDelegate(button, button._onopened)); }, 0);
        this.openPopup(Function.createDelegate(this, this._onopened));

        return true;
    },

    _onopened: function(contentWindow) {
        contentWindow.setColor = Function.createDelegate(this, this.setColor);
    },

    setColor: function(color) {
        this.closePopup();
        if (this._fixedColorButton != null) {
            this._fixedColorButton.set_defaultColor(color);
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector", AjaxControlToolkit.HTMLEditor.ToolbarButton.Selector);

