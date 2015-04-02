Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.BulletedList = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.BulletedList.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.BulletedList.prototype = {
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.BulletedList.callBaseMethod(this, "callMethod")) return false;
        this._designPanel._execCommand("InsertUnorderedList");
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.BulletedList.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.BulletedList", AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton);

