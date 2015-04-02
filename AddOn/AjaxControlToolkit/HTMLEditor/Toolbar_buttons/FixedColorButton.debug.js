Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton.initializeBase(this, [element]);
    
    this._defaultColor = "#000000";
    this._colorDiv = null;
    this._methodButton = null;
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton.prototype = {
    get_defaultColor : function() {
        return this._defaultColor;
    },
    set_defaultColor : function(value) {
        this._defaultColor = value;
        if (this._colorDiv != null) {
            this._colorDiv.get_element().style.backgroundColor = value;
        }
    },
    
    get_colorDiv : function() {
        return this._colorDiv;
    },
    set_colorDiv : function(value) {
        this._colorDiv = value;
    },
    
    get_methodButton : function() {
        return this._methodButton;
    },
    set_methodButton : function(value) {
        this._methodButton = value;
    },
    
    callMethod : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton.callBaseMethod(this, "callMethod")) return false;
        this.setColor(this.get_defaultColor());
    },
    
    setColor : function(color) {
    },

    initialize : function() {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton.callBaseMethod(this, "initialize");
        
        if (this._methodButton != null) {
            this._methodButton.callMethod = Function.createDelegate(this, this.callMethod);
        }
        if (this._colorDiv != null) {
            this._colorDiv.callMethod = Function.createDelegate(this, this.callMethod);
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.FixedColorButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton);

