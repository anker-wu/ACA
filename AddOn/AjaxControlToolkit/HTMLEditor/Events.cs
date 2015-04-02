using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;

#region Assembly Resource Attribute
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Events.js", "text/javascript")]
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Events.debug.js", "text/javascript")]

#endregion

namespace AjaxControlToolkit.HTMLEditor
{
    [ClientScriptResource(null, "AjaxControlToolkit.HTMLEditor.Events.js")]
    internal static class Events
    {
    }
}
