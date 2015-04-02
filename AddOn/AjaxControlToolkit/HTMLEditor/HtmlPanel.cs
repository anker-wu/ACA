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

[assembly: WebResource("AjaxControlToolkit.HTMLEditor.HtmlPanel.js", "application/x-javascript")]
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.HtmlPanel.debug.js", "application/x-javascript")]

[assembly: WebResource("AjaxControlToolkit.HTMLEditor.HtmlPanel.css", "text/css", PerformSubstitution = true)]

#endregion

namespace AjaxControlToolkit.HTMLEditor
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ClientCssResource("AjaxControlToolkit.HTMLEditor.HtmlPanel.css")]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [ClientScriptResource("AjaxControlToolkit.HTMLEditor.HtmlPanel", "AjaxControlToolkit.HTMLEditor.HtmlPanel.js")]
    internal class HtmlPanel : ModePanel
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new HtmlPanel
        /// </summary>
        public HtmlPanel()
            : base(HtmlTextWriterTag.Textarea)
        {
        }

        #endregion

        #region [ Methods ]


        #endregion
    }
}
