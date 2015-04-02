using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Globalization;
using System.CodeDom;
using System.Drawing;
using System.IO;
using AjaxControlToolkit;

#region [ Resources ]

[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.js", "application/x-javascript")]
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.debug.js", "application/x-javascript")]

#endregion

namespace AjaxControlToolkit.HTMLEditor.Popups
{
    [ParseChildren(true)]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [ClientScriptResource("AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup", "AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.js")]
    internal class AttachedPopup : Popup
    {
    }
}
