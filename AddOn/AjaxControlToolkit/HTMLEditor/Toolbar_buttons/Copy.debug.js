Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.Copy = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.Copy.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Copy.prototype = {
    canBeShown : function() {
        return AjaxControlToolkit.HTMLEditor.isIE;
    },
    
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.Copy.callBaseMethod(this, "callMethod")) return false;
        var editor = this._designPanel;

        if(AjaxControlToolkit.HTMLEditor.isIE) {
            editor.openWait();
            setTimeout(function(){editor.isShadowed(); editor._copyCut('c',true); editor.closeWait(); editor._ifShadow();},0)
        } else {
            editor._copyCut('c',true);
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.Copy.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.Copy", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

