#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaLabel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaLabel.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using AccelaWebControlExtender;

namespace Accela.Web.Controls
{
    #region Enumerations

    /// <summary>
    /// label type
    /// </summary>
    public enum LabelType
    {
        /// <summary>
        /// label text
        /// </summary>
        LabelText = 0,

        /// <summary>
        /// body text type
        /// </summary>
        BodyText = 1,

        /// <summary>
        /// section text
        /// </summary>
        SectionText = 2,

        /// <summary>
        /// applicant text
        /// </summary>
        ApplicantText = 14,

        /// <summary>
        /// hidden label
        /// </summary>
        HidableLabel = 17,

        /// <summary>
        /// section text
        /// </summary>
        SectionExText = 18,

        /// <summary>
        /// only needs configure the label text value
        /// </summary>
        SimpleLabelText = 25,

        /// <summary>
        /// sub section text
        /// </summary>
        SubSectionText = 28,

        /// <summary>
        /// only needs configure section title and instruction
        /// </summary>
        SectionTitle = 29,

        /// <summary>
        /// section title, and has other controls in the section
        /// we need to render the sub label in the web page.
        /// </summary>
        SectionTitleWithBar = 30,

        /// <summary>
        /// Page title text, if select this label, all elements in page should be selected.
        /// </summary>
        PageTitle = 35,

        /// <summary>
        /// Page instruction text.
        /// </summary>
        PageInstruction = 36,

        /// <summary>
        /// section text, and has available fields that can set visible or not.
        /// </summary>
        SectionTextWithField = 37,

        /// <summary>
        /// Pop up label title.
        /// </summary>
        PopUpTitle = 38,

        /// <summary>
        /// The type is used as the section title of the edit form.
        /// Admin user can configure the form layout and field's visibility/require properties.
        /// The label is placed inside the "legend" element.
        /// </summary>
        FieldSetTitle = 41,

        /// <summary>
        /// The section which can set the section form editable.
        /// </summary>
        SectionEditable = 42
    }

    #endregion Enumerations

    /// <summary>
    /// provide a label to show text
    /// </summary>
    public class AccelaLabel : Label, IAccelaNonInputControl
    {
        #region Fields

        /// <summary>
        /// Default module name label key
        /// </summary>
        private const string DEFAULT_MODULE_NAME = "aca_sys_default_module_name";

        /// <summary>
        /// is display label or not
        /// </summary>
        private bool _isDisplayLabel = true;

        /// <summary>
        /// is need encode or not
        /// </summary>
        private bool _isNeedEncode = true;

        /// <summary>
        /// is not hide or not
        /// </summary>
        private bool _isNotHided = true;

        /// <summary>
        /// label key string
        /// </summary>
        private string _labelKey;

        /// <summary>
        /// label type
        /// </summary>
        private LabelType _labelType = LabelType.LabelText;

        /// <summary>
        /// string module name
        /// </summary>
        private string _moduleName = null;

        /// <summary>
        /// restrict display
        /// </summary>
        private UserRoleType _restrictDisplay;

        /// <summary>
        /// default displayed line in ellipsis function.
        /// </summary>
        private int _collapseLines = 2;

        /// <summary>
        /// section id value
        /// </summary>
        private string _sectionId;

        /// <summary>
        /// Used to store the extender controls.
        /// </summary>
        private ControlCollection _children;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets child control collection.
        /// Get children control.
        /// because AccelaWebControlExtenderExtender is lost after async PostBack. So AccelaLabel use the special logic store 
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
        /// Gets or sets a value indicating whether the label is used as GridView column head label
        /// </summary>
        public bool IsGridViewHeadLabel
        {
            get;
            set;
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
        /// Gets or sets a value indicating whether current label whether need to encode html and filter perilous scripts.
        /// the default value is false.
        /// It should be set to true if this label is for showing the data user entered.
        /// </summary>
        public bool IsNeedEncode
        {
            get
            {
                return _isNeedEncode;
            }

            set
            {
                _isNeedEncode = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the label is not hided by admin
        /// </summary>
        public bool IsNotHided
        {
            get
            {
                return _isNotHided;
            }

            set
            {
                _isNotHided = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is display super agency text.
        /// </summary>
        /// <value><c>true</c> if this instance is display super agency text; otherwise, <c>false</c>.</value>
        public bool IsDisplaySuperAgencyText
        {
            get;
            set;
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
                // When the label has changed, change the sub label
                if (!string.IsNullOrEmpty(value) && !value.Equals(_labelKey))
                {
                    SubLabel = null;
                }

                _labelKey = value;
            }
        }

        /// <summary>
        /// Gets or sets Label Type
        /// </summary>
        public LabelType LabelType
        {
            get
            {
                return _labelType;
            }

            set
            {
                _labelType = value;
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
        /// Gets or sets restrict display.When the LabelType is ApplicantText, this property indicate the restrict display option
        /// </summary>
        public UserRoleType RestrictDisplay
        {
            get
            {
                return _restrictDisplay;
            }

            set
            {
                _restrictDisplay = value;
            }
        }

        /// <summary>
        /// Gets or sets section ID, if current label is section label,this property identify the id of the section
        /// </summary>
        public string SectionID
        {
            get
            {
                return _sectionId;
            }

            set
            {
                _sectionId = value;
            }
        }

        /// <summary>
        /// Gets or sets Text. Override Text attribute to avoid malicious code attack
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                //Admin user edit label with label key, we alwasys regard it as valid text, needn't to encode.
                if (!string.IsNullOrEmpty(_labelKey))
                {
                    this._isNeedEncode = false;
                }

                if (_isNeedEncode)
                {
                    // if the value is changed, need to re-encode the new value.
                    if (base.Text != value)
                    {
                        this.IsEncoded = false;
                    }

                    // If the value has ever been encoded, needn't to encode any more to avoid duplicated encode.
                    if (IsEncoded)
                    {
                        base.Text = value;
                    }
                    else
                    {
                        this.IsEncoded = true;
                        base.Text = ScriptFilter.FilterScript(value, true);
                    }
                }
                else
                {
                    this.IsEncoded = false;
                    base.Text = ScriptFilter.FilterScript(value, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets the sub label.
        /// </summary>
        public string SubLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether enable the ellipsis behavior.
        /// </summary>
        public bool EnableEllipsis
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the container ID for auto-expand the parent container.
        /// </summary>
        public string EllipsisContainerID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the displayed lines when Ellipsis function is effective.
        /// </summary>
        public int CollapseLines
        {
            get
            {
                return _collapseLines;
            }

            set
            {
                _collapseLines = value;
            }
        }

        /// <summary>
        /// Gets the value label client ID.
        /// </summary>
        public virtual string ValueLabelClientID
        {
            get
            {
                return ClientID;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the AccelaLabel's Text is already encoded 
        /// </summary>
        private bool IsEncoded
        {
            get
            {
                if (this.ViewState["IsEncoded"] == null)
                {
                    return false;
                }

                return (bool)this.ViewState["IsEncoded"];
            }

            set
            {
                this.ViewState["IsEncoded"] = value;
            }
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
                return GetLabel(LabelConvertUtil.GetGUITextByKey(LabelKey));
            }

            return string.Empty;
        }

        /// <summary>
        /// get default language text
        /// </summary>
        /// <returns>default language text</returns>
        public string GetDefaultLanguageText()
        {
            string value = GetLabel(LabelConvertUtil.GetDefaultLanguageTextByKey(LabelKey, GetModuleName()));

            return ScriptFilter.FilterScript(value, false);
        }

        /// <summary>
        /// Get Default Language Sub Label of control.
        /// </summary>
        /// <returns>Default language sub label of control</returns>
        public string GetDefaultLanguageSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGlobalTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// get default language sub label
        /// </summary>
        /// <returns>default language sub label</returns>
        public string GetDefaultSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get sub label
        /// </summary>
        /// <returns>sub label string</returns>
        public string GetSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey) && string.IsNullOrEmpty(this.SubLabel))
            {
                this.SubLabel = LabelConvertUtil.GetTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX, this);
            }

            return this.SubLabel;
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
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (this.LabelType == LabelType.BodyText)
            {
                if (string.IsNullOrEmpty(this.CssClass))
                {
                    this.CssClass = "ACA_Body_Text ACA_Body_Text_FontSize";
                }
                else
                {
                    this.CssClass += " ACA_Body_Text ACA_Body_Text_FontSize";
                }
            }
            else if (this.LabelType == LabelType.PageInstruction)
            {
                string className = "ACA_Page_Instruction ACA_Page_Instruction_FontSize";

                if (string.IsNullOrEmpty(this.CssClass))
                {
                    this.CssClass = className;
                }
                else
                {
                    this.CssClass += " " + className;
                }
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));
            render.OnPreRender(this);

            if (this.Visible)
            {
                if (_children != null && _children.Count > 0)
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
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the label on the client.
        /// </summary>
        /// <param name="w">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter w)
        {
            if (DesignMode)
            {
                return;
            }

            //If Label control has child controls, it will not render the 'Text' attribute.
            //So, need to remove all the children before the control rendering.
            this.RenderChildren(w);
            this.Controls.Clear();

            string watermarkText4PageIns = string.Empty;
            if (AccelaControlRender.IsAdminRender(this) && this.LabelType == LabelType.PageInstruction)
            {
                watermarkText4PageIns = LabelConvertUtil.GetGlobalTextByKey("aca_page_instruction_watermark");
                this.Attributes.Add("watermarkText4PageIns", watermarkText4PageIns);
            }

            if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(_labelKey))
            {
                if (IsDisplaySuperAgencyText && !AccelaControlRender.IsAdminRender(this))
                {
                    this.Text = LabelConvertUtil.GetSuperAgencyTextByKey(_labelKey, this);
                }
                else
                {
                    this.Text = LabelConvertUtil.GetTextByKey(_labelKey, GetModuleName(), this);
                }

                if (this.Text == LabelConvertUtil.GetGlobalTextByKey("aca_sys_default_module_name") && !string.IsNullOrEmpty(ModuleName))
                {
                    this.Text = DataUtil.AddBlankToString(ModuleName);
                }

                if (string.IsNullOrEmpty(this.Text) && this.LabelType == LabelType.PageInstruction)
                {
                    if (AccelaControlRender.IsAdminRender(this))
                    {
                        this.Text = watermarkText4PageIns;
                        this.CssClass += " ACA_Page_Instruction_Watermark";
                    }
                    else
                    {
                        this.CssClass = string.Empty;
                    }
                }
            }

            //If text inner html exist 'font-size' element. the child element need restore as 1em. 
            if (IsExistFontSize())
            {
                string className = Attributes["class"];
                if (!string.IsNullOrEmpty(className))
                {
                    if (className.IndexOf("font") > -1 && className.IndexOf("px") > -1)
                    {
                        Attributes["class"] = className + " ACA_Label_FontSize_Restore";
                    }
                    else if (className.IndexOf("FontSizeRestore") == -1)
                    {
                        Attributes["class"] = className + " FontSizeRestore";
                    }
                }
                else
                {
                    this.CssClass += " FontSizeRestore";
                }
            }

            if (this.LabelType == LabelType.SectionExText || this.LabelType == LabelType.SectionText
                || LabelType == LabelType.SectionTitle || LabelType == LabelType.SectionTextWithField || LabelType == LabelType.SectionEditable)
            {
                w.Write("<div class='ACA_TabRow'><div class='ACA_Title_Bar'><h1>");
                base.Render(w);
                w.Write("</h1></div>");
                
                w.Write(CreateSubLabelHtml());

                w.Write("</div>");
            }
            else if (LabelType == LabelType.PopUpTitle)
            {
                w.Write("<div><div><h1 class='font15px'>");
                base.Render(w);
                w.Write("</h1></div>");

                w.Write(CreateSubLabelHtml());

                w.Write("</div>");
            }
            else if (LabelType == LabelType.SubSectionText)
            {
                w.Write("<div class='Header_h6'>");
                base.Render(w);
                w.Write("</div>");
            }
            else
            {
                base.Render(w);
            }

            if (this.LabelType == LabelType.SubSectionText)
            {
                w.Write(CreateSubLabelHtml());
            }

            if (EnableEllipsis)
            {
                RegisterEllipsisScript();
            }
        }

        /// <summary>
        /// The flag for whether exist element 'font-size' in text HTML.
        /// </summary>
        /// <returns>true or false</returns>
        private bool IsExistFontSize()
        {
            string lowerText = Text.ToLower();
            return !string.IsNullOrEmpty(Text) && Regex.IsMatch(lowerText, @"font-size\s*:\s*[1-9]+[.]?[1-9]*\s*em", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// When the label key is 'aca_sys_default_module_name' show real module name. 
        /// </summary>
        /// <param name="text">module name value</param>
        /// <returns>Return the label.</returns>
        private string GetLabel(string text)
        {
            if (DEFAULT_MODULE_NAME.Equals(LabelKey) && !string.IsNullOrEmpty(ModuleName))
            {
                text = DataUtil.AddBlankToString(ModuleName);
            }       

            return ScriptFilter.FilterScript(text, false);
        }

        /// <summary>
        /// Create html for sub label control
        /// </summary>
        /// <returns>html string for sub label</returns>
        private string CreateSubLabelHtml()
        {
            string subLabel = GetSubLabel();
            string subLabelCls = string.IsNullOrEmpty(subLabel) ? "ACA_Hide" : "ACA_Section_Instruction ACA_Section_Instruction_FontSize";
            string filteredSubLabel = ScriptFilter.FilterScript(subLabel, false);
            string subLabelHtml = string.Format("<div id='{0}_sub_label' class='{1}'>{2}</div>", this.ClientID, subLabelCls, filteredSubLabel);
           
            return subLabelHtml;
        }
      
        /// <summary>
        /// Get register script string.
        /// </summary>
        private void RegisterEllipsisScript()
        {
            string collapse = LabelConvertUtil.GetGlobalTextByKey("inspection_resultcommentlist_collapselink");
            string readMore = LabelConvertUtil.GetGlobalTextByKey("inspection_resultcommentlist_readmorelink");
            StringBuilder sbScripts = new StringBuilder();
            sbScripts.AppendFormat("$('#{0}').Collapse", ValueLabelClientID);
            sbScripts.Append("({");
            sbScripts.AppendFormat("readMore:'{0}',collapse:'{1}',containerId:'{2}',line:'{3}'", readMore, collapse, EllipsisContainerID, CollapseLines);
            sbScripts.Append("});\n");
            ScriptManager.RegisterStartupScript(this, typeof(AccelaLabel), ClientID, sbScripts.ToString(), true);
        }

        #endregion Methods
    }
}
