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

[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Images.ed_html_n.gif", "image/gif")]
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Images.ed_html_a.gif", "image/gif")]
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Toolbar_buttons.HtmlMode.js", "application/x-javascript")]
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Toolbar_buttons.HtmlMode.debug.js", "application/x-javascript")]

#endregion

namespace AjaxControlToolkit.HTMLEditor.ToolbarButton
{
    [ToolboxItem(false)]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ClientScriptResource("AjaxControlToolkit.HTMLEditor.ToolbarButton.HtmlMode", "AjaxControlToolkit.HTMLEditor.Toolbar_buttons.HtmlMode.js")]
    public class HtmlMode : ModeButton
    {
        #region [ Methods ]

        protected override void OnPreRender(EventArgs e)
        {
            RegisterButtonImages("ed_html");
            ActiveMode = ActiveModeType.Html;
            base.OnPreRender(e);
        }

        #endregion
    }
}
