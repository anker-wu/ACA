Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedForeColor = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedForeColor.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedForeColor.prototype = {
    setColor : function(color) {
        this._designPanel._execCommand("forecolor", false, color);
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedForeColor.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedForeColor", AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton);

