Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.Paste = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.Paste.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Paste.prototype = {
    canBeShown : function() {
        return AjaxControlToolkit.HTMLEditor.isIE;
    },
    
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Paste.callBaseMethod(this, "callMethod")) return false;
        var editor = this._designPanel;

        if(AjaxControlToolkit.HTMLEditor.isIE) {
            editor._saveContent();
            editor.openWait();
            setTimeout(function(){editor._paste(true); editor.closeWait();},0)
        } else {
            var sel   = editor._getSelection();
            var range = editor._createRange(sel);

            editor._removeAllRanges(sel);
            alert(String.format(AjaxControlToolkit.Resources.HTMLEditor_toolbar_button_Use_verb, (AjaxControlToolkit.HTMLEditor.isSafari && navigator.userAgent.indexOf("mac") != -1)?"Apple-V":"Ctrl-V" ));
            editor._selectRange(sel,range);
            editor.isWord = false;
            editor.isPlainText = false;
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Paste.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.Paste", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

