Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.BoxButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.BoxButton.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.BoxButton.prototype = {
    initialize : function() {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.BoxButton.callBaseMethod(this, "initialize");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.BoxButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.BoxButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.CommonButton);

