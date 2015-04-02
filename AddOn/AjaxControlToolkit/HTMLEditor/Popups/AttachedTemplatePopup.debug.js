Type.registerNamespace("AjaxControlToolkit.HTMLEditor.Popups");

AjaxControlToolkit.HTMLEditor.Popups.AttachedTemplatePopup = function(element) {
    AjaxControlToolkit.HTMLEditor.Popups.AttachedTemplatePopup.initializeBase(this, [element]);

    this._contentDiv = null;
}

AjaxControlToolkit.HTMLEditor.Popups.AttachedTemplatePopup.prototype = {

    get_contentDiv : function() {
        return this._contentDiv;
    },

    set_contentDiv : function(value) {
        this._contentDiv = value;
    },
    
    initialize : function() {
        AjaxControlToolkit.HTMLEditor.Popups.AttachedTemplatePopup.callBaseMethod(this, "initialize");
        this.set_initialContent(this._contentDiv.innerHTML);
    },
    
    dispose : function() {
        AjaxControlToolkit.HTMLEditor.Popups.AttachedTemplatePopup.callBaseMethod(this, "dispose");
    }
}

AjaxControlToolkit.HTMLEditor.Popups.AttachedTemplatePopup.registerClass("AjaxControlToolkit.HTMLEditor.Popups.AttachedTemplatePopup", AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup);
