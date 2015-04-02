Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.Rtl = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.Rtl.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Rtl.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Rtl.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._doc.body.style.direction=(!this.checkState())?"rtl":"";

        if(!AjaxControlToolkit.HTMLEditor.isIE) {
            var sel   = this._designPanel._getSelection();
            var range = this._designPanel._createRange(sel);

            this._designPanel._removeAllRanges(sel);

            this._designPanel._selectRange(sel,range);
            this._designPanel.focusEditor();
        }
        var button = this;
        setTimeout(function(){button._editPanel.updateToolbar();},0);
    },
    
    checkState : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Rtl.callBaseMethod(this, "checkState")) return false;
        if(this._designPanel._doc.body.style.direction && this._designPanel._doc.body.style.direction=="rtl") return true;
        return false;
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Rtl.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.Rtl", AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton);

