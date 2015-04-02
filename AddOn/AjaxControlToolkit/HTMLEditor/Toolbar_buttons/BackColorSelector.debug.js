Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorSelector = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorSelector.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorSelector.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorSelector.callBaseMethod(this, "callMethod")) return false;
    },
    
    setColor : function(color) {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorSelector.callBaseMethod(this, "setColor", [color]);
        this._designPanel._execCommand("backcolor", false, color);
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorSelector.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorSelector", AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector);

