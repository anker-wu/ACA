Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.Redo = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.Redo.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Redo.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Redo.callBaseMethod(this, "callMethod")) return false;
        this._designPanel.redo();
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Redo.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.Redo", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

