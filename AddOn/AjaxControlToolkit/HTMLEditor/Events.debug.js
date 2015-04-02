Type.registerNamespace("AjaxControlToolkit.HTMLEditor");

AjaxControlToolkit.HTMLEditor.ActiveModeChangedArgs = function(oldMode, newMode, editPanel)     
{     
  if (arguments.length != 3) throw Error.parameterCount();     
  
  //Calling the base class constructor     
  AjaxControlToolkit.HTMLEditor.ActiveModeChangedArgs.initializeBase(this);     
  this._oldMode = oldMode;     
  this._newMode = newMode;     
  this._editPanel = editPanel;     
}     
  
AjaxControlToolkit.HTMLEditor.ActiveModeChangedArgs.prototype =     
{     
  get_oldMode : function()     
  {     
    return this._oldMode;     
  },       
  get_newMode : function()     
  {     
    return this._newMode;     
  },
  get_editPanel : function()     
  {     
    return this._editPanel;     
  }     
}     
  
AjaxControlToolkit.HTMLEditor.ActiveModeChangedArgs.registerClass('AjaxControlToolkit.HTMLEditor.ActiveModeChangedArgs', Sys.EventArgs);
