Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectButton.initializeBase(this, [element]);
    
    this._onclick_option$delegate = Function.createDelegate(this, this._onclick_option);
    this._onchange$delegate = Function.createDelegate(this, this._onchange);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectButton.prototype = {
    
    initialize : function() {
        var nodeId = this.get_element().id;
        AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectButton.callBaseMethod(this, "initialize");
        this._select =  $get(nodeId+"_select");
        
        $addHandler(this._select, "change", this._onchange$delegate);
        for(var i=0; i < this._select.options.length; i++) {
            var option = this._select.options[i];
            $addHandler(option, "click", this._onclick_option$delegate);
        }
    },
    
    dispose : function() {
        for(var i=0; i < this._select.options.length; i++) {
            var option = this._select.options[i];
            $removeHandler(option, "click", this._onclick_option$delegate);
        }

        $removeHandler(this._select, "change", this._onchange$delegate);
        AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectButton.callBaseMethod(this, "dispose");
    },
    
    isImage : function() {
        return false;
    },
    
    
    callMethod : function(select,e) {
        return true;
    },
    
    _onclick_option : function(e) {
        if (!this.isEnable()) {
            return false;
        }
        var option = e.target;
        option.parentNode.value=option.value;
        AjaxControlToolkit.HTMLEditor._stopEvent(e);
        if(!AjaxControlToolkit.HTMLEditor.isSafari) return false;
        this.callMethod(option.parentNode,e);
        return true;
    },
    
    _onchange : function(e) {
        if (!this.isEnable()) {
            return false;
        }
        var select = e.target;
        AjaxControlToolkit.HTMLEditor._stopEvent(e);
        this.callMethod(select,e);
        return true;
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.CommonButton);

