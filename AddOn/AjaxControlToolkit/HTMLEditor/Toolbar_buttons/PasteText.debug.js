Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.PasteText = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.PasteText.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.PasteText.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.PasteText.callBaseMethod(this, "callMethod")) return false;
        var editor = this._designPanel;

        if(AjaxControlToolkit.HTMLEditor.isIE)
        {
            editor._saveContent();
            editor.openWait();
            setTimeout(function(){editor._paste(false); editor.closeWait();},0)
        } else {
            var sel   = editor._getSelection();
            var range = editor._createRange(sel);
            var useVerb = String.format(AjaxControlToolkit.Resources.HTMLEditor_toolbar_button_Use_verb, (AjaxControlToolkit.HTMLEditor.isSafari && navigator.userAgent.indexOf("mac") != -1)?"Apple-V":"Ctrl-V" );
            var mess = String.format(AjaxControlToolkit.Resources.HTMLEditor_toolbar_button_OnPastePlainText, useVerb);

            alert(mess);

            setTimeout(function(){
                editor._removeAllRanges(sel);
                editor._selectRange(sel,range);
            },0);
            editor.isPlainText = true;
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.PasteText.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.PasteText", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

