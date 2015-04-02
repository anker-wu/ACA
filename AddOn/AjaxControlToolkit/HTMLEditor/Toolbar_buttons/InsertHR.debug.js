Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertHR = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertHR.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertHR.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertHR.callBaseMethod(this, "callMethod")) return false;
        var editor = this._designPanel;

        try {
            editor._saveContent();

            var _div = editor._doc.createElement("div");
            _div.innerHTML = "<hr>";

            var el = _div.firstChild;
            var place =editor._getSafePlace();
            if(!place) return;

            var parent=place.parentNode;

            parent.insertBefore(el,place);
            parent.removeChild (place);

            el = (el.nextSibling)?el.nextSibling:el;
            AjaxControlToolkit.HTMLEditor._setCursor(el,editor);
            setTimeout(function() {editor.onContentChanged();editor._editPanel.updateToolbar();}, 0);

            editor.focusEditor();
            return true;
        } catch(e){}
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertHR.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertHR", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);
