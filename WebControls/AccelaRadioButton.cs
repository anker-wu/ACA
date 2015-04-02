#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaLabel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaRadioButton.cs 277735 2014-08-20 09:26:13Z ACHIEVO\james.shi $.
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
    /// the radio button type
    /// </summary>
    public enum RadioButtonType
    {
        /// <summary>
        /// normal radio button
        /// </summary>
        Normal = 11,

        /// <summary>
        /// section radio button
        /// </summary>
        SectionExRadio = 50
    }

    /// <summary>
    /// Accela RadioButton.
    /// </summary>
    public class AccelaRadioButton : RadioButton, IAccelaNonInputControl
    {
        #region Fields

        /// <summary>
        /// whether display label,default value is true.
        /// </summary>
        private bool _isDisplayLabel = true;

        /// <summary>
        /// The section ID
        /// </summary>
        private string _sectionID = string.Empty;

        /// <summary>
        /// The module name
        /// </summary>
        private string _moduleName = string.Empty;

        /// <summary>
        /// label key string.
        /// </summary>
        private string _labelKey;

        /// <summary>
        /// Used to store the extender controls.
        /// </summary>
        private ControlCollection _children;

        /// <summary>
        /// radio button type
        /// </summary>
        private RadioButtonType _type = RadioButtonType.Normal;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether control label span is or not display
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
                _labelKey = value;
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
        /// Gets or sets of Section ID
        /// </summary>
        public string SectionID
        {
            get { return _sectionID; }
            set { _sectionID = value; }
        }

        /// <summary>
        /// Gets or sets of module name
        /// </summary>
        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        /// <summary>
        /// Gets child control collection.
        /// Get children control.
        /// because AccelaWebControlExtenderExtender is lost after async PostBack. So use the special logic store 
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
        /// Gets or sets radio button's type
        /// </summary>
        public RadioButtonType Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        /// <summary>
        /// Gets or sets the radio button's sub-container's client id.
        /// Consider the radio button as a container and obtain the sub-elements by this property.
        /// Use its sub-element, it can do something, for example highlight the sub-elements in ACA admin. 
        /// </summary>
        public string SubContainerClientID
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

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
            return string.Empty;
        }

        /// <summary>
        /// Get Default Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
        public string GetDefaultSubLabel()
        {
            return string.Empty;
        }

        /// <summary>
        /// Get Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
        public string GetSubLabel()
        {
            return string.Empty;
        }

        /// <summary>
        /// initial daily extender control
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
            if (string.IsNullOrEmpty(this.CssClass))
            {
                this.CssClass = "aca_checkbox aca_checkbox_fontsize";
            }
            else
            {
                this.CssClass += " aca_checkbox aca_checkbox_fontsize";
            }

            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));
            render.OnPreRender(this);

            if (Visible && _children != null && _children.Count > 0)
            {
                //ExtenderControl depend on ScriptManager.
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

                foreach (Control child in _children)
                {
                    if (child is AccelaWebControlExtenderExtender)
                    {
                        AccelaWebControlExtenderExtender extender = child as AccelaWebControlExtenderExtender;
                        scriptManager.RegisterExtenderControl<AccelaWebControlExtenderExtender>(extender, this);
                        scriptManager.RegisterScriptDescriptors(extender);
                    }
                }
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the Radio button on the client.
        /// </summary>
        /// <param name="w">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter w)
        {
            if (DesignMode)
            {
                return;
            }

            if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(_labelKey))
            {
                Text = LabelConvertUtil.GetTextByKey(_labelKey, this);
            }

            // Set the title value for Input control
            if (!string.IsNullOrEmpty(Text))
            {
                InputAttributes.Add("title", LabelConvertUtil.RemoveHtmlFormat(Text));
            }
            else if (!string.IsNullOrEmpty(ToolTip))
            {
                InputAttributes.Add("title", ToolTip);

                // Remove attribute title for its parent (Span control).
                ToolTip = string.Empty;
            }

            base.Render(w);
            RenderChildren(w);
            Controls.Clear();
        }

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module, returns string.Empty
        /// </summary>
        /// <returns>module name.</returns>
        private string GetModuleName()
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

        #endregion Methods
    }
}