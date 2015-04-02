Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.HorizontalSeparator = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.HorizontalSeparator.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.HorizontalSeparator.prototype = {
    
    isImage : function() {
        return false;
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.HorizontalSeparator.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.HorizontalSeparator", AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton);

