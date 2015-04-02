Type.registerNamespace("AjaxControlToolkit.HTMLEditor.Popups");

AjaxControlToolkit.HTMLEditor.Popups.LinkProperties = function(element) {
    AjaxControlToolkit.HTMLEditor.Popups.LinkProperties.initializeBase(this, [element]);
    
    this._defaultTarget = "_self";
    this._targetTextHolder = null;
    this._urlTextHolder = null;
}

AjaxControlToolkit.HTMLEditor.Popups.LinkProperties.prototype = {
    get_defaultTarget : function() {
        return this._defaultTarget;
    },
    set_defaultTarget : function(value) {
        this._defaultTarget = value;
    }
}

AjaxControlToolkit.HTMLEditor.Popups.LinkProperties.registerClass("AjaxControlToolkit.HTMLEditor.Popups.LinkProperties", AjaxControlToolkit.HTMLEditor.Popups.OkCancelAttachedTemplatePopup);
