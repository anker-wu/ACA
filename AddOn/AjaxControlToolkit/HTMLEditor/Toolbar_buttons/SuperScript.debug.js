Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.SuperScript = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.SuperScript.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.SuperScript.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.SuperScript.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("superScript", false, null);
    },
    
    checkState : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.SuperScript.callBaseMethod(this, "checkState")) return false;
        return this._designPanel._queryCommandState("superScript");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.SuperScript.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.SuperScript", AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton);

