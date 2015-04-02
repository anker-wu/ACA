Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.Paragraph = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.Paragraph.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Paragraph.prototype = {
    checkState : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Paragraph.callBaseMethod(this, "checkState")) return false;
        return this._designPanel._textAlignState("");
    },
    
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Paragraph.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("Paragraph");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Paragraph.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.Paragraph", AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton);

