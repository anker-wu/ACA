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
using AjaxControlToolkit.HTMLEditor.ToolbarButton;

#region [ Resources ]

[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Toolbar.js", "application/x-javascript")]
[assembly: WebResource("AjaxControlToolkit.HTMLEditor.Toolbar.debug.js", "application/x-javascript")]

#endregion

namespace AjaxControlToolkit.HTMLEditor
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [ClientScriptResource("AjaxControlToolkit.HTMLEditor.Toolbar", "AjaxControlToolkit.HTMLEditor.Toolbar.js")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Toolbar")]
    public abstract class Toolbar : ScriptControlBase
    {
        #region [ Fields ]

        private Collection<CommonButton> _buttons;
        private bool _wasPreRender;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new Toolbar
        /// </summary>
        protected Toolbar()
            : base(false, HtmlTextWriterTag.Div)
        {
        }

        #endregion

        #region [ Properties ]

        [DefaultValue(false)]
        [Category("Behavior")]
        [ExtenderControlProperty]
        [ClientPropertyName("alwaysVisible")]
        public bool AlwaysVisible
        {
            get { return (bool)(ViewState["AlwaysVisible"] ?? false); }
            set { ViewState["AlwaysVisible"] = value; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Collection<CommonButton> Buttons
        {
            get
            {
                if (_buttons == null)
                {
                    _buttons = new Collection<CommonButton>();
                }
                return _buttons;
            }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Will be called in Design (future)")]
            internal set
            {
                _buttons = value;
            }
        }

        [Browsable(false)]
        [ExtenderControlProperty]
        [ClientPropertyName("buttonIds")]
        public string ButtonIds
        {
            get
            {
                string result = "";
                for (int i = 0; i < Buttons.Count; i++)
                {
                    if (i > 0) result += ";";
                    result += Buttons[i].ClientID;
                }
                return result;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeButtonIds()
        {
            return IsRenderingScript;
        }

        [DefaultValue("")]
        [Category("Appearance")]
        [Description("Folder used for toolbar's buttons' images")]
        public string ButtonImagesFolder
        {
            get { return (string)(ViewState["ButtonImagesFolder"] ?? ""); }
            set
            {
                string temp = LocalResolveUrl(value);
                if (temp.Length > 0)
                {
                    string lastCh = temp.Substring(temp.Length - 1, 1);
                    if (lastCh != "\\" && lastCh != "/") temp += "/";
                    ViewState["ButtonImagesFolder"] = temp;
                }
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Resolves Url of file.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Resolved Url.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        protected string LocalResolveUrl(string path)
        {
            string temp = base.ResolveUrl(path);
            Regex _Regex = new Regex(@"(\(S\([A-Za-z0-9_]+\)\)/)", RegexOptions.Compiled);
            temp = _Regex.Replace(temp, "");
            return temp;
        }

        protected override void CreateChildControls()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                Controls.Add(Buttons[i]);
                if (!AlwaysVisible)
                    Buttons[i].Style[HtmlTextWriterStyle.Display] = "none";
                for (int j = 0; j < Buttons[i].ExportedControls.Count; j++)
                {
                    Controls.Add(Buttons[i].ExportedControls[j]);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is caught only for debugging purposes")]
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
            }
            catch { }
            _wasPreRender = true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!_wasPreRender) this.OnPreRender(new EventArgs());
            base.Render(writer);
        }
        internal void CreateChilds(DesignerWithMapPath designer)
        {
            this.Controls.Clear();
            this.CreateChildControls();
            for (int i = 0; i < Controls.Count; i++)
            {
                CommonButton button = Controls[i] as CommonButton;
                if (button != null)
                    button.CreateChilds(designer);
            }
        }

        #endregion
    }
}
