Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.IncreaseIndent = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.IncreaseIndent.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.IncreaseIndent.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.IncreaseIndent.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("Indent");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.IncreaseIndent.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.IncreaseIndent", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

