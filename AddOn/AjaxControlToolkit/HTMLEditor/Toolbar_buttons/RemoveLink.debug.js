Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveLink = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveLink.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveLink.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveLink.callBaseMethod(this, "callMethod")) return false;
        var editor = this._designPanel;
        var sel   = editor._getSelection();
        var range = editor._createRange(sel);
        var parent= AjaxControlToolkit.HTMLEditor.getSelParent(editor);
        
        if (parent.nodeType == 3) {
            parent = parent.parentNode;
        }

        while(parent && AjaxControlToolkit.HTMLEditor.isStyleTag(parent.tagName) && parent.tagName.toUpperCase() != "A") {
            parent = parent.parentNode;
        }
        
        if(parent && parent.tagName.toUpperCase() == "A") {
            editor._saveContent();
            var el = parent.firstChild;

            while(parent.firstChild) parent.parentNode.insertBefore(parent.firstChild,parent);

            parent.parentNode.removeChild(parent);
            if(el) AjaxControlToolkit.HTMLEditor._setCursor(el,editor);
            setTimeout(function() {editor._editPanel.updateToolbar();}, 0);
            editor.onContentChanged();
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveLink.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.RemoveLink", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);
