using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;

#region Assembly Resource Attribute
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Enums.js", "text/javascript")]
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Enums.debug.js", "text/javascript")]

#endregion

namespace AjaxControlToolkit.HTMLEditor
{
    public enum ActiveModeType { Design, Html, Preview };

    [ClientScriptResource(null, "AjaxControlToolkit.HTMLEditor.Enums.js")]
    internal static class Enums
    {
    }
}
