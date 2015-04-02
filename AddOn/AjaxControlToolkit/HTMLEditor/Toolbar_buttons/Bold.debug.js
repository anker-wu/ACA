Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("bold", false, null);
    },
    
    checkState : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold.callBaseMethod(this, "checkState")) return false;
        return this._designPanel._queryCommandState("bold");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold", AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton);

