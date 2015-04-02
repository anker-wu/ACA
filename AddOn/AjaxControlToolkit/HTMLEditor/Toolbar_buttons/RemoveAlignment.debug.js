Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveAlignment = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveAlignment.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveAlignment.prototype = {
    checkState : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveAlignment.callBaseMethod(this, "checkState")) return false;
        return this._designPanel._textAlignState(null);
    },
    
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveAlignment.callBaseMethod(this, "callMethod")) return false;
        var editor = this._designPanel;
        var editPanel = this._editPanel;
        editor._saveContent();
        editor.MSIE_justify("left", true);
        editor.onContentChanged();
        setTimeout(function(){editPanel.updateToolbar();},0);
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveAlignment.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveAlignment", AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton);

