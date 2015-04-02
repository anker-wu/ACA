Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.OrderedList = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.OrderedList.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.OrderedList.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.OrderedList.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("InsertOrderedList");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.OrderedList.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.OrderedList", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

