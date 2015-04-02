Type.registerNamespace("AjaxControlToolkit.HTMLEditor");
Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.ModeButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.ModeButton.initializeBase(this, [element]);
    
    this._activeMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Design;
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ModeButton.prototype = {
    get_activeMode : function() {
        return this._activeMode;
    },
    set_activeMode : function(value) {
        this._activeMode = value;
    },
 
    _onclick : function(e) {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ModeButton.callBaseMethod(this, "_onclick")) return false;
        var modeButton = this;
        setTimeout(function() {
                modeButton._editPanel.set_activeMode(modeButton._activeMode);
            },0);
        return true;
    },
    
    onEditPanelActivity : function() {
        this.setActivity(this._editPanel.get_activeMode() == this._activeMode);
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.ModeButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.ModeButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton);

