Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorButton.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorButton.prototype = {

    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorButton.callBaseMethod(this, "callMethod")) return false;
        this.openPopup(Function.createDelegate(this, this._onopened));
        return true;
    },
    
    _onopened : function(contentWindow) {
        contentWindow.setColor = Function.createDelegate(this, this.setColor);
    },
    
    setColor : function(color) {
        this.closePopup();
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModePopupImageButton);
