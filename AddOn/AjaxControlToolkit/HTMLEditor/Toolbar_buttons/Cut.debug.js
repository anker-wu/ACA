Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.Cut = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.Cut.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Cut.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Cut.callBaseMethod(this, "callMethod")) return false;
        var editor = this._designPanel;

        if(AjaxControlToolkit.HTMLEditor.isIE) {
            editor.openWait();
            setTimeout(function(){editor.isShadowed(); editor._copyCut('x',true); editor.closeWait();},0)
        } else {
            editor._copyCut('x',true);
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Cut.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.Cut", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

