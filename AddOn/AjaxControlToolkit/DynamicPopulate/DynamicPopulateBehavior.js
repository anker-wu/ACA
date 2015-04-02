Type.registerNamespace("AjaxControlToolkit");AjaxControlToolkit.DynamicPopulateBehavior=function(c){var b=null,a=this;AjaxControlToolkit.DynamicPopulateBehavior.initializeBase(a,[c]);a._servicePath=b;a._serviceMethod=b;a._contextKey=b;a._cacheDynamicResults=false;a._populateTriggerID=b;a._setUpdatingCssClass=b;a._clearDuringUpdate=true;a._customScript=b;a._clickHandler=b;a._callID=0;a._currentCallID=-1;a._populated=false};AjaxControlToolkit.DynamicPopulateBehavior.prototype={initialize:function(){var a=this;AjaxControlToolkit.DynamicPopulateBehavior.callBaseMethod(a,"initialize");$common.prepareHiddenElementForATDeviceUpdate();if(a._populateTriggerID){var b=$get(a._populateTriggerID);if(b){a._clickHandler=Function.createDelegate(a,a._onPopulateTriggerClick);$addHandler(b,"click",a._clickHandler)}}},dispose:function(){var a=this;if(a._populateTriggerID&&a._clickHandler){var b=$get(a._populateTriggerID);if(b)$removeHandler(b,"click",a._clickHandler);a._populateTriggerID=null;a._clickHandler=null}AjaxControlToolkit.DynamicPopulateBehavior.callBaseMethod(a,"dispose")},populate:function(contextKey){var a=this;if(a._populated&&a._cacheDynamicResults)return;if(a._currentCallID==-1){var eventArgs=new Sys.CancelEventArgs;a.raisePopulating(eventArgs);if(eventArgs.get_cancel())return;a._setUpdating(true)}if(a._customScript){var scriptResult=eval(a._customScript);a._setTargetHtml(scriptResult);a._setUpdating(false)}else{a._currentCallID=++a._callID;if(a._servicePath&&a._serviceMethod){Sys.Net.WebServiceProxy.invoke(a._servicePath,a._serviceMethod,false,{contextKey:contextKey?contextKey:a._contextKey},Function.createDelegate(a,a._onMethodComplete),Function.createDelegate(a,a._onMethodError),a._currentCallID);$common.updateFormToRefreshATDeviceBuffer()}}},_onMethodComplete:function(b,a){if(a!=this._currentCallID)return;this._setTargetHtml(b);this._setUpdating(false)},_onMethodError:function(b,c){var a=this;if(c!=a._currentCallID)return;if(b.get_timedOut())a._setTargetHtml(AjaxControlToolkit.Resources.DynamicPopulate_WebServiceTimeout);else a._setTargetHtml(String.format(AjaxControlToolkit.Resources.DynamicPopulate_WebServiceError,b.get_statusCode()));a._setUpdating(false)},_onPopulateTriggerClick:function(){this.populate(this._contextKey)},_setUpdating:function(b){var a=this;a.setStyle(b);if(!b){a._currentCallID=-1;a._populated=true;a.raisePopulated(a,Sys.EventArgs.Empty)}},_setTargetHtml:function(b){var a=this.get_element();if(a)if(a.tagName=="INPUT")a.value=b;else a.innerHTML=b},setStyle:function(c){var a=this,b=a.get_element();if(a._setUpdatingCssClass)if(!c){b.className=a._oldCss;a._oldCss=null}else{a._oldCss=b.className;b.className=a._setUpdatingCssClass}if(c&&a._clearDuringUpdate)a._setTargetHtml("")},get_ClearContentsDuringUpdate:function(){return this._clearDuringUpdate},set_ClearContentsDuringUpdate:function(a){if(this._clearDuringUpdate!=a){this._clearDuringUpdate=a;this.raisePropertyChanged("ClearContentsDuringUpdate")}},get_ContextKey:function(){return this._contextKey},set_ContextKey:function(a){if(this._contextKey!=a){this._contextKey=a;this.raisePropertyChanged("ContextKey")}},get_PopulateTriggerID:function(){return this._populateTriggerID},set_PopulateTriggerID:function(a){if(this._populateTriggerID!=a){this._populateTriggerID=a;this.raisePropertyChanged("PopulateTriggerID")}},get_ServicePath:function(){return this._servicePath},set_ServicePath:function(a){if(this._servicePath!=a){this._servicePath=a;this.raisePropertyChanged("ServicePath")}},get_ServiceMethod:function(){return this._serviceMethod},set_ServiceMethod:function(a){if(this._serviceMethod!=a){this._serviceMethod=a;this.raisePropertyChanged("ServiceMethod")}},get_cacheDynamicResults:function(){return this._cacheDynamicResults},set_cacheDynamicResults:function(a){if(this._cacheDynamicResults!=a){this._cacheDynamicResults=a;this.raisePropertyChanged("cacheDynamicResults")}},get_UpdatingCssClass:function(){return this._setUpdatingCssClass},set_UpdatingCssClass:function(a){if(this._setUpdatingCssClass!=a){this._setUpdatingCssClass=a;this.raisePropertyChanged("UpdatingCssClass")}},get_CustomScript:function(){return this._customScript},set_CustomScript:function(a){if(this._customScript!=a){this._customScript=a;this.raisePropertyChanged("CustomScript")}},add_populating:function(a){this.get_events().addHandler("populating",a)},remove_populating:function(a){this.get_events().removeHandler("populating",a)},raisePopulating:function(b){var a=this.get_events().getHandler("populating");if(a)a(this,b)},add_populated:function(a){this.get_events().addHandler("populated",a)},remove_populated:function(a){this.get_events().removeHandler("populated",a)},raisePopulated:function(b){var a=this.get_events().getHandler("populated");if(a)a(this,b)}};AjaxControlToolkit.DynamicPopulateBehavior.registerClass("AjaxControlToolkit.DynamicPopulateBehavior",AjaxControlToolkit.BehaviorBase);