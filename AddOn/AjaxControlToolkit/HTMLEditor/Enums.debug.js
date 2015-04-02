Type.registerNamespace("AjaxControlToolkit.HTMLEditor");

AjaxControlToolkit.HTMLEditor.ActiveModeType = function() { }
AjaxControlToolkit.HTMLEditor.ActiveModeType.prototype = {
    Design : 0x00,
    Html : 0x01,
    Preview: 0x02
}
AjaxControlToolkit.HTMLEditor.ActiveModeType_checkValue = function(value) {
    if(value >= 0 && value <= 2) return true;
    return false;
}

AjaxControlToolkit.HTMLEditor.ActiveModeType.registerEnum("AjaxControlToolkit.HTMLEditor.ActiveModeType", true);


