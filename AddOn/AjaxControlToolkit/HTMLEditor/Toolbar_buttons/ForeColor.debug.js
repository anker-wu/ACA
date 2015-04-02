Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColor = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColor.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColor.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColor.callBaseMethod(this, "callMethod")) return false;
    },
    
    setColor : function(color) {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColor.callBaseMethod(this, "setColor", [color]);
        this._designPanel._execCommand("forecolor", false, color);
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColor.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.ForeColor", AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorButton);

