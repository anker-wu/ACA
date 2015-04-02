Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize.initializeBase(this, [element]);
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize.prototype = {
    initialize : function() {
        AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize.callBaseMethod(this, "initialize");
    },
    
    callMethod : function(select,event) {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize.callBaseMethod(this, "callMethod")) return false;

        try {
            var editor = this._designPanel;
            editor._execCommand("FontSize",false,AjaxControlToolkit.HTMLEditor.fontSizeSeek(select.options.item(select.selectedIndex).value));
        } catch(e) {}
    },
    
    checkState : function() {
        if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize.callBaseMethod(this, "checkState")) return false;
    
        var editor = this._designPanel;

        var param =null;
        var _id = this._select.id;

        try{param =(Function.createDelegate(editor, AjaxControlToolkit.HTMLEditor._queryCommandValue))("fontsize",_id);} catch(e){}

        if(param) param = param.toString();

        if(!editor._FontNotSet)
        if(!param || param.length==0) {
            param = AjaxControlToolkit.HTMLEditor.getStyle(editor._doc.body,"font-size");
            param = AjaxControlToolkit.HTMLEditor._TryTransformFromPxToPt(param, this, _id);
        }

        try {
            var el = this._select;
            var i=0;
            if(param && param.length > 0) {
                var seek = param.toLowerCase().split(",")[0];
                for(i=0; i< el.options.length; i++) {
                    var cur = AjaxControlToolkit.HTMLEditor.fontSizeSeek(el.options.item(i).value.toLowerCase().split(",")[0]);

                    if(cur==seek) break;
                }
                if(i==el.options.length) {
                    try {
                        var newopt = document.createElement("OPTION");
                        newopt.value = param;
                        newopt.text  = param;
                        el.add(newopt,(AjaxControlToolkit.HTMLEditor.isIE)?i:null);
                        $addHandler(newopt, "click", this._onclick_option$delegate);
                    } catch (e) {
                        i = 0;
                    }
                }
            }
            el.selectedIndex = i;
        } catch(e) {}
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize", AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeSelectButton);

