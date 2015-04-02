Type.registerNamespace("AjaxControlToolkit.HTMLEditor.Popups");

AjaxControlToolkit.HTMLEditor.Popups.OkCancelAttachedTemplatePopup = function(element) {
    AjaxControlToolkit.HTMLEditor.Popups.OkCancelAttachedTemplatePopup.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.Popups.OkCancelAttachedTemplatePopup.prototype = {
    
    initialize : function() {
        AjaxControlToolkit.HTMLEditor.Popups.OkCancelAttachedTemplatePopup.callBaseMethod(this, "initialize");
    },
    
    dispose : function() {
        AjaxControlToolkit.HTMLEditor.Popups.OkCancelAttachedTemplatePopup.callBaseMethod(this, "dispose");
    }
}

AjaxControlToolkit.HTMLEditor.Popups.OkCancelAttachedTemplatePopup.registerClass("AjaxControlToolkit.HTMLEditor.Popups.OkCancelAttachedTemplatePopup", AjaxControlToolkit.HTMLEditor.Popups.AttachedTemplatePopup);
