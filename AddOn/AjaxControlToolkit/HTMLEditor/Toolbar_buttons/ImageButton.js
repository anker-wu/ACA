Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton=function(b){var a=this;AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton.initializeBase(a,[b]);a._normalSrc="";a._hoverSrc="";a._downSrc="";a._activeSrc="";a._downTimer=null};AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton.prototype={get_normalSrc:function(){return this._normalSrc},set_normalSrc:function(b){this._normalSrc=b;var a=this.get_element();if(/none$/.test(a.src))a.src=b},get_hoverSrc:function(){return this._hoverSrc},set_hoverSrc:function(a){this._hoverSrc=a},get_downSrc:function(){return this._downSrc},set_downSrc:function(a){this._downSrc=a},get_activeSrc:function(){return this._activeSrc},set_activeSrc:function(a){this._activeSrc=a},isImage:function(){return true},_onmouseover:function(){var a=this;if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton.callBaseMethod(a,"_onmouseover"))return false;if(a._hoverSrc.length>0)a.get_element().src=a._hoverSrc;return true},_onmouseout:function(){var a=this,b=a.get_element();if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton.callBaseMethod(a,"_onmouseout"))return false;if(a._hoverSrc.length>0)if(Sys.UI.DomElement.containsCssClass(b,a._cssClass+"_mousedown")&&a._downSrc.length>0)b.src=a._downSrc;else if(Sys.UI.DomElement.containsCssClass(b,a._cssClass+"_active")&&a._activeSrc.length>0)b.src=a._activeSrc;else b.src=a._normalSrc;return true},_onmousedown:function(){var a=this;if(AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton.callBaseMethod(a,"_onmousedown")===null)return null;if(a._downSrc.length>0){a.get_element().src=a._downSrc;a._downTimer=setTimeout(Function.createDelegate(a,a._onmouseup),1e3)}return true},_onmouseup:function(){var a=this,b=a.get_element();if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton.callBaseMethod(a,"_onmouseup"))return false;if(a._downSrc.length>0){if(Sys.UI.DomElement.containsCssClass(b,a._cssClass+"_hover")&&a._hoverSrc.length>0)b.src=a._hoverSrc;else if(Sys.UI.DomElement.containsCssClass(b,a._cssClass+"_active")&&a._activeSrc.length>0)b.src=a._activeSrc;else b.src=a._normalSrc;if(a._downTimer!=null){clearTimeout(a._downTimer);a._downTimer=null}}return true},setActivity:function(c){var a=this,b=a.get_element();if(a._activeSrc.length>0)if(c){if(!Sys.UI.DomElement.containsCssClass(b,a._cssClass+"_active"))b.src=a._activeSrc}else if(Sys.UI.DomElement.containsCssClass(b,a._cssClass+"_active"))if(Sys.UI.DomElement.containsCssClass(b,a._cssClass+"_mousedown")&&a._downSrc.length>0)b.src=a._downSrc;else if(Sys.UI.DomElement.containsCssClass(b,a._cssClass+"_hover")&&a._hoverSrc.length>0)b.src=a._hoverSrc;else b.src=a._normalSrc;AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton.callBaseMethod(a,"setActivity",[c])}};AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton",AjaxControlToolkit.HTMLEditor.ToolbarButton.CommonButton);