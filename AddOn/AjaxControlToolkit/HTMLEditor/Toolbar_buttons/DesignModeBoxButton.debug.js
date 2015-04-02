Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.initializeBase(this, [element]);

    this._designPanel = null;
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.prototype = {
    _onmousedown : function(e) {
        if(this._designPanel == null) return false;
        if(this._designPanel.isPopup()) return false;
        if(AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.callBaseMethod(this, "_onmousedown",[e])===null) return false;
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
        AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.callBaseMethod(this, "initialize");
        if(AjaxControlToolkit.HTMLEditor.isOpera) {
            var node = this.get_element();
            function setDisplay(element) {
                element.style.display = "";
                for(var i = 0; i < element.childNodes.length; i++) {
                    var child = element.childNodes.item(i);
                    if(child.nodeType == 1) {
                        setDisplay(child);
                    }
                }
            }
            
            function setVisibility(element, value) {
                if (element.style.visibility != value) {
                    element.style.visibility = value;
                }
                for(var i = 0; i < element.childNodes.length; i++) {
                    var child = element.childNodes.item(i);
                    if(child.nodeType == 1) {
                        setVisibility(child, value);
                    }
                }
            }
            
            if(this.canBeShown()) {
                setDisplay(node);
                node.style.visibility = "hidden";
            }
            
            this.hideButton = function() {
                setVisibility(node, "hidden");
            };
            
            this.showButton = function() {
                if (node.style.display == "none") {
                    node.style.display = "";
                }
                setVisibility(node, "visible");
            };
        }
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.BoxButton);

