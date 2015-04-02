Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton.prototype = {
    onEditPanelActivity: function() {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton.callBaseMethod(this, "onEditPanelActivity");
        this.setActivity(this.checkState());
    },

    checkState: function() {
        if (!this.checkRangeInDesign()) return false;
        return true;
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.EditorToggleButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton);

