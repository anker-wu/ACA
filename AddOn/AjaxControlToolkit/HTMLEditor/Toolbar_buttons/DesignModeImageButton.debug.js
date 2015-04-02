Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.initializeBase(this, [element]);
    this._designPanel = null;
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.prototype = {
    _onmousedown : function(e) {
        if(this._designPanel == null) return false;
        if(this._designPanel.isPopup()) return false;
        if(AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.callBaseMethod(this, "_onmousedown",[e])===null) return false;
        this.callMethod();
        return false;
    },
    
    onEditPanelActivity : function() {
        this._designPanel = this._editPanel.get_activePanel();
    },
    
    callMethod : function() {
        if(this._designPanel == null) return false;
        if(this._designPanel.isPopup()) return false;
        return true;
    },
    
    initialize : function() {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.callBaseMethod(this, "initialize");
        if(AjaxControlToolkit.HTMLEditor.isOpera) {
            var node = this.get_element();
            function setDisplay(element) {
                element.style.display = "";
            }
            
            function setVisibility(element) {
                if (element.style.visibility == "hidden") {
                    element.style.visibility = "visible";
                }
            }
            
            if(this.canBeShown()) {
                setDisplay(node);
                node.style.visibility = "hidden";
            }
            
            this.hideButton = function() {
                node.style.visibility = "hidden";
            };
            
            this.showButton = function() {
                if (node.style.display == "none") {
                    node.style.display = "";
                }
                setVisibility(node);
            };
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton);

