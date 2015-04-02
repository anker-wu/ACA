Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedBackColor = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedBackColor.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedBackColor.prototype = {
    setColor : function(color) {
        this._designPanel._execCommand("backcolor", false, color);
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedBackColor.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedBackColor", AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton);

