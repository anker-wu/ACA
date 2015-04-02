Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.JustifyRight = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.JustifyRight.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.JustifyRight.prototype = {
    checkState : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.JustifyRight.callBaseMethod(this, "checkState")) return false;
        return this._designPanel._textAlignState("right");
    },
    
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.JustifyRight.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("JustifyRight");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.JustifyRight.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.JustifyRight", AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton);

