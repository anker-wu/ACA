Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorSelector = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorSelector.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorSelector.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorSelector.callBaseMethod(this, "callMethod")) return false;
    },
    
    setColor : function(color) {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorSelector.callBaseMethod(this, "setColor", [color]);
        this._designPanel._execCommand("forecolor", false, color);
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorSelector.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorSelector", AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector);

