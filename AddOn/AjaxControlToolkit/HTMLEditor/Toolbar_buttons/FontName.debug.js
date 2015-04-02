Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName.prototype = {
    initialize : function() {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName.callBaseMethod(this, "initialize");
    },
    
    callMethod : function(select,event) {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName.callBaseMethod(this, "callMethod")) return false;

        try {
            var editor = this._designPanel;
            editor._execCommand("fontname",false,select.options.item(select.selectedIndex).value);
        } catch(e) {}
    },
    
    checkState : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName.callBaseMethod(this, "checkState")) return false;
    
        var editor = this._designPanel;
        var param =null;
        try{param = (Function.createDelegate(editor, AjaxControlToolkit.HTMLEditor._queryCommandValue))("fontname");} catch(e){}

        if(!editor._FontNotSet)
        if(!param || param.length==0) {
            param = AjaxControlToolkit.HTMLEditor.getStyle(editor._doc.body,"font-family");
        }

        var el   = this._select;
        var i=0;
        if(param && param.length > 0) {
            var seek = param.toLowerCase().split(",")[0].replace(/^(['"])/,"").replace(/(['"])$/,"");

            for(i=0; i< el.options.length; i++) {
                var cur = el.options.item(i).value.toLowerCase().split(",")[0];
                if(cur==seek) break
            }
            if(i==el.options.length) {
                try {
                    var newopt = document.createElement("OPTION");
                    newopt.value = param.replace(/^(['"])/,"").replace(/(['"])$/,"");
                    newopt.text  = param.split(",")[0].replace(/^(['"])/,"").replace(/(['"])$/,"");
                    el.add(newopt,(AjaxControlToolkit.HTMLEditor.isIE)?i:null);
                    $addHandler(newopt, "click", this._onclick_option$delegate);
                } catch(e) {
                    i = 0;
                }
            }
        }
        el.selectedIndex = i;
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName", AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton);

