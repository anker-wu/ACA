using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

[assembly: WebResource("AjaxControlToolkit.Common.Threading.js", "application/x-javascript")]
[assembly: WebResource("AjaxControlToolkit.Common.Threading.debug.js", "application/x-javascript")]

namespace AjaxControlToolkit
{
    [ClientScriptResource(null, "AjaxControlToolkit.Common.Threading.js")]
    public static class ThreadingScripts
    {
    }
}
