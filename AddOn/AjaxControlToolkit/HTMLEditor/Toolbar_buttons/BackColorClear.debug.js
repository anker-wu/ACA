Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorClear = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorClear.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorClear.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorClear.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("backcolor", false, "");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorClear.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.BackColorClear", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

