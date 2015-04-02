Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.Ltr = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.Ltr.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Ltr.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Ltr.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._doc.body.style.direction=(!this.checkState())?"":"rtl";

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
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Ltr.callBaseMethod(this, "checkState")) return false;
        if(!(this._designPanel._doc.body.style.direction && this._designPanel._doc.body.style.direction=="rtl")) return true;
        return false;
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Ltr.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.Ltr", AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton);

