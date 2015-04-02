Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton.initializeBase(this, [element]);
    this._designPanel = null;
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton.prototype = {
    onEditPanelActivity: function() {
        this._designPanel = this._editPanel.get_activePanel();
        this.checkState()
    },


    checkState: function() {
        if (!this.checkRangeInDesign()) return false;
        return true;
    },

    initialize: function() {
        var node = this.get_element();
        AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton.callBaseMethod(this, "initialize");

        if (AjaxControlToolkit.HTMLEditor.isOpera) {
            if (this.canBeShown()) {
                node.style.display = "";
                node.style.visibility = "hidden";
            }

            this.hideButton = function() {
                node.style.visibility = "hidden";
            };

            this.showButton = function() {
                if (node.style.display == "none") {
                    node.style.display = "";
                }
                node.style.visibility = "visible";
            };
        }
    },

    callMethod: function(select, e) {
        if (!AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton.callBaseMethod(this, "callMethod")) return false;
        if (this._designPanel == null) return false;
        if (this._designPanel.isPopup()) return false;

        return true;
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectButton);

