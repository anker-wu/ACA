Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorClear = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorClear.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorClear.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorClear.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("forecolor", false, "");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorClear.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColorClear", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

