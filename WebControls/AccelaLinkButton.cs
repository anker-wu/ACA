#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaLinkButton.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:This class is used for customizing AccelaGridview control header
 *
 *  Notes:
 * $Id: AccelaLinkButton.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using AccelaWebControlExtender;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide a link button
    /// </summary>
    public class AccelaLinkButton : LinkButton, IAccelaNonInputControl
    {
        #region Fields

        /// <summary>
        /// enable configure Url or not
        /// </summary>
        private bool _enableConfigureURL;
        
        /// <summary>
        /// enable Record Type Filter or not
        /// </summary>
        private bool _enableRecordTypeFilter;

        /// <summary>
        /// display label or not
        /// </summary>
        private bool _isDisplayLabel = true;

        /// <summary>
        /// string label key 
        /// </summary>
        private string _labelKey;

        /// <summary>
        /// module name.
        /// </summary>
        private string _moduleName = null;

        /// <summary>
        /// string report id
        /// </summary>
        private string _reportId;

        /// <summary>
        /// Used to store the extender controls.
        /// </summary>
        private ControlCollection _children;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets child control collection.
        /// Get children control.
        /// because AccelaWebControlExtenderExtender is lost after async PostBack. So AccelaLinkButton use the special logic store 
        /// the Extender Controls to Children collection and registered in 'OnPreRender' method.
        /// </summary>
        public ControlCollection Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new ControlCollection(this);
                }

                return _children;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button can configure URL in ACA Admin
        /// </summary>
        public bool EnableConfigureURL
        {
            get
            {
                return _enableConfigureURL;
            }

            set
            {
                _enableConfigureURL = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button can setting record type filter in ACA Admin
        /// </summary>
        public bool EnableRecordTypeFilter
        {
            get
            {
                return _enableRecordTypeFilter;
            }

            set
            {
                _enableRecordTypeFilter = value;
            }
        }        

        /// <summary>
        /// Gets or sets a value indicating whether control label span is or isn't display
        /// </summary>
        public bool IsDisplayLabel
        {
            get
            {
                return _isDisplayLabel;
            }

            set
            {
                _isDisplayLabel = value;
            }
        }

        /// <summary>
        /// Gets or sets Label Key 
        /// </summary>
        public string LabelKey
        {
            get
            {
                return _labelKey;
            }

            set
            {
                // empty the Text property when label key changed.
                Text = string.Empty;

                _labelKey = value;
            }
        }

        /// <summary>
        /// Gets or sets module name
        /// </summary>
        public string ModuleName
        {
            get
            {
                return _moduleName;
            }

            set
            {
                _moduleName = value;
            }
        }

        /// <summary>
        /// Gets or sets report ID
        /// </summary>
        public string ReportID
        {
            get
            {
                return _reportId;
            }

            set
            {
                _reportId = value;
            }
        }

        /// <summary>
        /// Gets or sets text. Override Text attribute to avoid malicious code attack (updated by peter 12/6/2007)
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = Accela.ACA.Common.Common.ScriptFilter.FilterScript(value, false);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether hide this control or not when this text is empty.
        /// </summary>
        public bool HideOnEmptyText { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the button status in script.
        /// </summary>
        /// <param name="disable">if set to <c>true</c> [disable].</param>
        public void SetButtonStatus(bool disable)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RandomKey" + ClientID, "SetWizardButtonDisable('" + ClientID + "', " + disable.ToString().ToLower() + ");", true);
        }

        /// <summary>
        /// get default label
        /// </summary>
        /// <returns>default label</returns>
        public string GetDefaultLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// get default language text
        /// </summary>
        /// <returns>default language text</returns>
        public string GetDefaultLanguageText()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetDefaultLanguageTextByKey(LabelKey, GetModuleName()), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Default Language Sub Label of control.
        /// </summary>
        /// <returns>Default language sub label of control</returns>
        public string GetDefaultLanguageSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetDefaultLanguageGlobalTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Default Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
        public string GetDefaultSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
        public string GetSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return LabelConvertUtil.GetTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX, this);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module, returns string.Empty
        /// </summary>
        /// <returns>module name.</returns>
        public string GetModuleName()
        {
            if (string.IsNullOrEmpty(_moduleName))
            {
                if (this.Page is IPage)
                {
                    return (Page as IPage).GetModuleName();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return _moduleName;
            }
        }

        /// <summary>
        /// Initial extender control
        /// </summary>
        public void InitExtenderControl()
        {
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            IAccelaControlRender render = ObjectFactory.GetObject<IAccelaControlRender>();
            render.OnPreRender(this);

            if (this.Visible)
            {
                if (_children != null && _children.Count > 0)
                {
                    //ExtenderControl depend on ScriptManager.
                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

                    foreach (Control control in _children)
                    {
                        //register AccelaWebControlExtenderExtender
                        if (control is AccelaWebControlExtenderExtender)
                        {
                            AccelaWebControlExtenderExtender extender = control as AccelaWebControlExtenderExtender;
                            scriptManager.RegisterExtenderControl<AccelaWebControlExtenderExtender>(extender, this);
                            scriptManager.RegisterScriptDescriptors(extender);
                        }
                    }
                }
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the link button on the client.
        /// </summary>
        /// <param name="w">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter w)
        {
            if (DesignMode)
            {
                return;
            }

            bool isHelperExtenderRendered = false;

            if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(_labelKey))
            {
                string text = LabelConvertUtil.GetTextByKey(_labelKey, GetModuleName(), this);

                if (text == LabelConvertUtil.GetGlobalTextByKey("aca_sys_default_module_name") && !string.IsNullOrEmpty(ModuleName))
                {
                    text = ModuleName;
                }

                Text = "<span>" + text + "</span>";
            }

            if (HideOnEmptyText && string.IsNullOrEmpty(Text))
            {
                return;
            }

            foreach (Control control in Controls)
            {
                if (isHelperExtenderRendered)
                {
                    break;
                }
                else if (control is HelperExtender && !isHelperExtenderRendered)
                {
                    control.RenderControl(w);
                    isHelperExtenderRendered = true;
                }
            }

            base.Render(w);
        }

        #endregion Methods
    }
}