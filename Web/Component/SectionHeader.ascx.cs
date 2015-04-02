#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SectionHeader.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description: section header user control
 *
 *  Notes:
 *      $Id: SectionHeader.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    #region Delegates

    /// <summary>                                                                                                                                                                                               
    /// Delegate for SearchEventHandler.                                                                                                                                                                                         
    /// </summary>                                                                                                                                                                                              
    /// <param name="sender">Object sender</param>                                                                                                                                                              
    /// <param name="e">EventArgs e</param>                                                                                                                                                                          
    public delegate void SearchEventHandler(object sender, CommonEventArgs e);

    #endregion Delegates

    /// <summary>                                                                                                                                                                                               
    /// Section header class                                                                                                                                                                                    
    /// </summary>                                                                                                                                                                                              
    public partial class SectionHeader : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Property name of instruction hidden CSS class.
        /// </summary>
        private const string PROPERTY_NAME_INSTRUCTION_HIDDEN_CSS_CLASS = "HiddenCssClass";

        /// <summary>
        /// Property name of instruction shown CSS class.
        /// </summary>
        private const string PROPERTY_NAME_INSTRUCTION_SHOWN_CSS_CLASS = "ShownCssClass";

        /// <summary>
        /// Property name of collapsible
        /// </summary>
        private const string PROPERTY_NAME_COLLAPSIBLE = "collapsible";

        /// <summary>
        /// Property name of collapsed
        /// </summary>
        private const string PROPERTY_NAME_COLLAPSED = "collapsed";

        /// <summary>
        /// Property name of section body id
        /// </summary>
        private const string PROPERTY_NAME_SECTION_BODY_ID = "sectionBodyID";

        /// <summary>
        /// Property name of title label id
        /// </summary>
        private const string PROPERTY_NAME_TITLE_LABEL_ID = "titleLabelID";

        /// <summary>
        /// Property name of instruction id
        /// </summary>
        private const string PROPERTY_NAME_INSTRUCTION_ID = "instructionID";

        /// <summary>
        /// Property name of link id
        /// </summary>
        private const string PROPERTY_NAME_LINK_ID = "linkID";

        /// <summary>
        /// Property name of image id
        /// </summary>
        private const string PROPERTY_NAME_IMAGE_ID = "imageID";

        /// <summary>
        /// Property name of collapsed image URL
        /// </summary>
        private const string PROPERTY_NAME_COLLAPSED_IMAGE_URL = "CollapsedImageURL";

        /// <summary>
        /// Property name of expanded image URL
        /// </summary>
        private const string PROPERTY_NAME_EXPAND_IMAGE_URL = "ExpandedImageURL";

        /// <summary>
        /// Property name of OnClick
        /// </summary>
        private const string PROPERTY_NAME_ONCLICK = "onclick";

        /// <summary>
        /// Property name of collapsed alt text
        /// </summary>
        private const string PROPERTY_NAME_COLLAPSED_ALT_TEXT = "CollapsedAltText";

        /// <summary>
        /// Property name of expanded alt text
        /// </summary>
        private const string PROPERTY_NAME_EXPANDED_ALT_TEXT = "ExpandedAltText";

        /// <summary>
        /// Property name of CSS class
        /// </summary>
        private const string PROPERTY_NAME_CSS_CLASS = "class";

        /// <summary>
        /// Sub label postfix
        /// </summary>
        private const string SUBLABEL_POSTFIX = "|sub";

        /// <summary>
        /// SEMI-colon char
        /// </summary>
        private const string SEMI_COLON = ";";

        /// <summary>
        /// On client click script pattern
        /// </summary>
        private const string ON_CLIENT_CLICK_SCRIPT_PATTERN = "if ({0}) {{ var collapsed = {1}; if (collapsed) {2}; else {3}; }}";

        /// <summary>
        /// Toggle section script pattern.
        /// </summary>
        private const string TOGGLE_SECTION_SCRIPT_PATTERN = "new SectionHeaderManager('{0}').toggle();";

        /// <summary>
        /// Expand section script key
        /// </summary>
        private const string EXPAND_SECTION_SCRIPT_KEY = "CollapseOrExpandSection";

        /// <summary>
        /// Void function
        /// </summary>
        private const string VOID_FUNCTION = "void(0)";

        /// <summary>
        /// Expand label key
        /// </summary>
        private const string EXPAND_LABEL_KEY = "img_alt_expand_icon";

        /// <summary>
        /// Collapse label key
        /// </summary>
        private const string COLLAPSE_LABEL_KEY = "img_alt_collapse_icon";

        /// <summary>
        /// Expand image name
        /// </summary>
        private const string EXPAND_IMAGE_NAME = "section_header_expanded.gif";

        /// <summary>
        /// Collapse image name
        /// </summary>
        private const string COLLAPSE_IMAGE_NAME = "section_header_collapsed.gif";

        #endregion Fields

        #region Events
        /// <summary>                                                                                                                                                                                           
        /// An event for search                                                                                                                                                                                 
        /// </summary>                                                                                                                                                                                          
        public event SearchEventHandler SearchEvent;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the collapsible.
        /// </summary>
        /// <value>The collapsible.</value>
        public bool? Collapsible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is using customized layout.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is using customized layout; otherwise, <c>false</c>.
        /// </value>
        public bool IsUsingCustomizedLayout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the section body client ID.
        /// </summary>
        /// <value>The section body client ID.</value>
        public string SectionBodyClientID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collapsed.
        /// </summary>
        /// <value>The collapsed.</value>
        public bool? Collapsed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>The CSS class.</value>
        public string CssClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the instruction hidden CSS class.
        /// </summary>
        /// <value>The instruction hidden CSS class.</value>
        public string InstructionHiddenCssClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the instruction shown CSS class.
        /// </summary>
        /// <value>The instruction shown CSS class.</value>
        public string InstructionShownCssClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the on client clicking.
        /// </summary>
        /// <value>The on client clicking.</value>
        public string OnClientClicking
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the on client expanded.
        /// </summary>
        /// <value>The on client expanded.</value>
        public string OnClientExpanded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the on client collapsed.
        /// </summary>
        /// <value>The on client collapsed.</value>
        public string OnClientCollapsed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the label.
        /// </summary>
        /// <value>The type of the label.</value>
        public LabelType? TitleLabelType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title label key.
        /// </summary>
        /// <value>The title label key.</value>
        public string TitleLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                var sectionLabel = GetSectionLabel();
                return sectionLabel.Text;
            }

            set
            {
                var sectionLabel = GetSectionLabel();
                sectionLabel.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the section ID.
        /// </summary>
        /// <value>The section ID.</value>
        public string SectionID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the header template.
        /// </summary>
        /// <value>The header template.</value>
        [TemplateContainer(typeof(TemplatedCustomControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate HeaderTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the section is expanded or not.
        /// </summary>
        /// <value>The expanded.</value>
        public bool EnableExpand
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the header instruct.
        /// </summary>
        /// <value>The header instruct.</value>
        public string Instruction
        {
            get
            {
                return lblSectionTitle_sub_label.Text;
            }

            set
            {
                lblSectionTitle_sub_label.Text = value;
            }
        }

        /// <summary>                                                                                                                                                                                           
        /// Gets or sets the search parameter type                                                                                                                                                              
        /// </summary>                                                                                                                                                                                          
        /// <value>                                                                                                                                                                                             
        /// Text, TextArea, DropdownList, Checkbox.                                                                                                                                                                 
        /// </value>                                                                                                                                                                                            
        public string SearchType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the instruction of section shown or not.
        /// </summary>
        /// <value>
        /// Show the instruction of section.
        /// </value>
        public bool ShowInstruction
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Searches the current naming container for a server control with the specified <paramref name="id"/> parameter.
        /// </summary>
        /// <param name="id">The identifier for the control to be found.</param>
        /// <returns>
        /// The specified control, or null if the specified control does not exist.
        /// </returns>
        public override Control FindControl(string id)
        {
            Control result = null;

            if (phSectionHeaderTemplate != null && phSectionHeaderTemplate.Controls != null && phSectionHeaderTemplate.Controls.Count > 0)
            {
                result = phSectionHeaderTemplate.Controls[0].FindControl(id);
            }

            return result;
        }

        /// <summary>
        /// Handles the Initial event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (HeaderTemplate != null)
            {
                var container = new TemplatedCustomControl();
                HeaderTemplate.InstantiateIn(container);
                phSectionHeaderTemplate.Controls.Add(container);
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //begin to setup section Css Class
            SetupCssClass();

            //begin to setup section title and instruction
            SetupTitle();
            SetupInstruction();

            //begin to setup label place holder
            InitSectionByType();

            //begin to setup image and link
            SetupImageLink();

            //collapse or expand section
            CollapseOrExpandSection();

            //display search menu                                                                                                                                                                               
            DisplaySearchMenu();
        }

        /// <summary>
        /// Handles the search click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (SearchEvent != null)
            {
                CommonEventArgs args = new CommonEventArgs(txtSearchCondition.Text);

                SearchEvent(sender, args);
            }

            AccessibilityUtil.FocusElement(btnSubmitSearch.ClientID);
        }

        /// <summary>
        /// Get watermark
        /// </summary>
        /// <returns>watermark label</returns>
        protected string GetWaterMark()
        {
            string waterMark = GetTextByKey("aca_sectionsearch_label_search");

            return LabelUtil.RemoveHtmlFormat(waterMark).Replace("'", "&#39;").Replace("\"", "&quot;");
        }

        /// <summary>                                                                                                                                                                                           
        /// Setups the CSS class.                                                                                                                                                                               
        /// </summary>                                                                                                                                                                                          
        private void SetupCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                divSectionHeader.Attributes[PROPERTY_NAME_CSS_CLASS] = CssClass;
            }

            if (!string.IsNullOrEmpty(InstructionHiddenCssClass))
            {
                divSectionHeader.Attributes[PROPERTY_NAME_INSTRUCTION_HIDDEN_CSS_CLASS] = InstructionHiddenCssClass;
            }

            if (!string.IsNullOrEmpty(InstructionShownCssClass))
            {
                divSectionHeader.Attributes[PROPERTY_NAME_INSTRUCTION_SHOWN_CSS_CLASS] = InstructionShownCssClass;
            }
        }

        /// <summary>
        /// Initializes the type of the section by.
        /// </summary>
        private void InitSectionByType()
        {
            phCustomizedLayout.Visible = IsUsingCustomizedLayout;
            phDefaultLayout.Visible = !IsUsingCustomizedLayout;
        }

        /// <summary>
        /// Setups the section title.
        /// </summary>
        private void SetupTitle()
        {
            var sectionLabel = GetSectionLabel();
            sectionLabel.LabelType = TitleLabelType == null ? LabelType.SectionTitleWithBar : TitleLabelType.Value;
            sectionLabel.LabelKey = TitleLabelKey;
            sectionLabel.SectionID = SectionID;

            if (EnableExpand)
            {
                sectionLabel.Attributes.Add(ACAConstant.ENABLE_EXPAND, ACAConstant.COMMON_Y);
            }
        }

        /// <summary>
        /// Setups the section instruction
        /// </summary>
        private void SetupInstruction()
        {
            if (!IsUsingCustomizedLayout)
            {
                string subLabelKey = string.IsNullOrEmpty(TitleLabelKey) ? string.Empty : TitleLabelKey + SUBLABEL_POSTFIX;
                lblSectionTitle_sub_label.Text = ScriptFilter.FilterScript(GetTextByKey(subLabelKey), false);

                bool collapsible = IsCollapsible();
                string hiddenCssClass = lblSectionTitle_sub_label.Attributes[PROPERTY_NAME_INSTRUCTION_HIDDEN_CSS_CLASS];
                string shownCssClass = lblSectionTitle_sub_label.Attributes[PROPERTY_NAME_INSTRUCTION_SHOWN_CSS_CLASS];
                string cssClass = (collapsible && !ShowInstruction) || string.IsNullOrEmpty(lblSectionTitle_sub_label.Text) ? hiddenCssClass : shownCssClass;
                lblSectionTitle_sub_label.CssClass = cssClass;
            }
        }

        /// <summary>
        /// Setups the image link.
        /// </summary>
        private void SetupImageLink()
        {
            if (!IsUsingCustomizedLayout)
            {
                //begin to choose and set section type.
                bool collapsible = IsCollapsible();

                if (collapsible)
                {
                    //begin to setup current control
                    divSectionHeader.Attributes[PROPERTY_NAME_COLLAPSIBLE] = collapsible.ToString();
                    if (IsPostBack)
                    {
                        divSectionHeader.Attributes[PROPERTY_NAME_COLLAPSED] = IsCollapsed() ? ACAConstant.COMMON_TRUE : ACAConstant.COMMON_FALSE;
                    }
                    else
                    { 
                        divSectionHeader.Attributes[PROPERTY_NAME_COLLAPSED] = ACAConstant.COMMON_TRUE;
                    }

                    divSectionHeader.Attributes[PROPERTY_NAME_SECTION_BODY_ID] = SectionBodyClientID;
                    var sectionLabel = GetSectionLabel();
                    divSectionHeader.Attributes[PROPERTY_NAME_TITLE_LABEL_ID] = sectionLabel.ClientID;
                    divSectionHeader.Attributes[PROPERTY_NAME_INSTRUCTION_ID] = lblSectionTitle_sub_label.ClientID;
                    divSectionHeader.Attributes[PROPERTY_NAME_LINK_ID] = lnkCollapseOrExpand.ClientID;
                    divSectionHeader.Attributes[PROPERTY_NAME_IMAGE_ID] = imgCollapseOrExpand.ClientID;

                    //begin to setup link
                    string onClientClicking = string.IsNullOrEmpty(OnClientClicking) ? ACAConstant.COMMON_TRUE : OnClientClicking.Replace(SEMI_COLON, string.Empty);
                    string onClientExpanded = string.IsNullOrEmpty(OnClientExpanded) ? VOID_FUNCTION : OnClientExpanded.Replace(SEMI_COLON, string.Empty);
                    string onClientCollapsed = string.IsNullOrEmpty(OnClientCollapsed) ? VOID_FUNCTION : OnClientCollapsed.Replace(SEMI_COLON, string.Empty);
                    string toggleSectionScript = string.Format(TOGGLE_SECTION_SCRIPT_PATTERN, ClientID).Replace(SEMI_COLON, string.Empty);
                    string onClientClickScript = string.Format(ON_CLIENT_CLICK_SCRIPT_PATTERN, onClientClicking, toggleSectionScript, onClientCollapsed, onClientExpanded);
                    lnkCollapseOrExpand.Attributes[PROPERTY_NAME_ONCLICK] = onClientClickScript;

                    //get expand or collapse labek key
                    string expandOrCollapseLabelKey = IsCollapsed() ? EXPAND_LABEL_KEY : COLLAPSE_LABEL_KEY;

                    //begin to setup link title
                    lnkCollapseOrExpand.Title = string.IsNullOrEmpty(TitleLabelKey) ? string.Empty : GetTitleByKey(expandOrCollapseLabelKey, TitleLabelKey, false);

                    //begin to setup image
                    imgCollapseOrExpand.Alt = GetTextByKey(expandOrCollapseLabelKey);
                    string collapsedImageURL = ImageUtil.GetImageURL(COLLAPSE_IMAGE_NAME);
                    string expandedImageURL = ImageUtil.GetImageURL(EXPAND_IMAGE_NAME);
                    string collapsedAltText = GetTextByKey(COLLAPSE_LABEL_KEY);
                    string expandedAltText = GetTextByKey(EXPAND_LABEL_KEY);
                    string imageURL = IsCollapsed() ? collapsedImageURL : expandedImageURL;
                    imgCollapseOrExpand.Src = imageURL;
                    imgCollapseOrExpand.Attributes[PROPERTY_NAME_COLLAPSED_IMAGE_URL] = collapsedImageURL;
                    imgCollapseOrExpand.Attributes[PROPERTY_NAME_EXPAND_IMAGE_URL] = expandedImageURL;
                    imgCollapseOrExpand.Attributes[PROPERTY_NAME_COLLAPSED_ALT_TEXT] = collapsedAltText;
                    imgCollapseOrExpand.Attributes[PROPERTY_NAME_EXPANDED_ALT_TEXT] = expandedAltText;
                }
                else
                {
                    lnkCollapseOrExpand.Visible = false;
                }
            }
        }

        /// <summary>
        /// Collapses the or expand section.
        /// </summary>
        private void CollapseOrExpandSection()
        {
            if (!Page.IsPostBack && !IsCollapsed())
            {
                string script = string.Format(TOGGLE_SECTION_SCRIPT_PATTERN, ClientID);
                ScriptManager.RegisterStartupScript(this, this.GetType(), EXPAND_SECTION_SCRIPT_KEY + "_" + ClientID, script, true);
            }
        }

        /// <summary>
        /// Gets the section label.
        /// </summary>
        /// <returns>the section label.</returns>
        private AccelaLabel GetSectionLabel()
        {
            var result = IsUsingCustomizedLayout ? lblCustomizedLayoutSectionTitle : lblSectionTitle;
            return result;
        }

        /// <summary>
        /// Determines whether this instance is collapsible.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is collapsible; otherwise, <c>false</c>.
        /// </returns>
        private bool IsCollapsible()
        {
            bool result = Collapsible != null && Collapsible.Value == true;
            return result;
        }

        /// <summary>
        /// Determines whether this instance is collapsed.
        /// </summary>
        /// <returns>
        ///  <c>true</c> if this instance is collapsed; otherwise, <c>false</c>.
        /// </returns>
        private bool IsCollapsed()
        {
            bool result = Collapsed == null || (Collapsed != null && Collapsed.Value == true);
            return result;
        }

        /// <summary>
        /// Display search menu
        /// </summary>
        private void DisplaySearchMenu()
        {
            if (string.IsNullOrEmpty(SearchType))
            {
                return;
            }

            if (SearchType.Equals("TextType"))
            {
                divSearchArea.Visible = true;
                this.txtSearchCondition.Attributes.Add("onkeydown", this.ClientID + "_Keydown(event)");
                this.txtSearchCondition.Attributes.Add("onclick", this.ClientID + "_CleanText(this)");
                this.txtSearchCondition.Attributes.Add("onblur", this.ClientID + "_SearchBlur(this)");
                this.txtSearchCondition.ToolTip = GetTextByKey("aca_condition_label_conditionsofapproval_filter");

                if (string.IsNullOrEmpty(txtSearchCondition.Text))
                {
                    txtSearchCondition.Text = GetWaterMark();
                }
            }
            else
            {
                divSearchArea.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                /* Add each image button for daily and admin site. Because asp:imagebutton do not work well in IE10 and take error.
                 * And ACA:AccelaImageButton work well in IE10 in daily site but it's property do not display in admin.
                 */
                if (AppSession.IsAdmin)
                {
                    btnSubmitSearch4Admin.Visible = true;
                    btnSubmitSearch4Admin.ImageUrl = ImageUtil.GetImageURL("gsearch_disabled.png");
                    btnSubmitSearch4Admin.Attributes.Add("onclick", "javascript:void(0); return false;");

                    btnSubmitSearch.Visible = false;

                    lblSearchCondition.Visible = true;
                    txtSearchCondition.Visible = false;
                }
                else
                {
                    btnSubmitSearch4Admin.Visible = false;

                    btnSubmitSearch.Visible = true;
                    btnSubmitSearch.ImageUrl = ImageUtil.GetImageURL("gsearch.png");
                    btnSubmitSearch.AlternateText = GetTextByKey("aca_sectionsearch_label_search");
                    lblSearchCondition.Visible = false; 
                    txtSearchCondition.Visible = true;
                }
            }

            if (string.Equals(this.txtSearchCondition.Text, GetWaterMark(), StringComparison.InvariantCultureIgnoreCase))
            {
                this.txtSearchCondition.Attributes.Add("class", "gs_search_box watermark INPUT");
            }
            else
            {
                this.txtSearchCondition.Attributes.Add("class", "gs_search_box INPUT");
            }
        }

        #endregion Methods

        #region Internal classes

        /// <summary>
        /// this attribute is important because it tells the ASP.NET parser that your tag in the ASP.NET page will contain nested tags inside it
        /// </summary>
        [ParseChildren(true)]
        public class TemplatedCustomControl : Control, INamingContainer
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TemplatedCustomControl"/> class.
            /// </summary>
            internal TemplatedCustomControl()
            {
            }

            /// <summary>
            /// Gets or sets the template.
            /// </summary>
            /// <value>The template.</value>
            /// Property to get access to the template of the control
            [PersistenceMode(PersistenceMode.InnerProperty),
            TemplateContainer(typeof(TemplateControl))]
            public ITemplate Template
            {
                get;
                set;
            }

            /// <summary>
            /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
            /// </summary>
            protected override void CreateChildControls()
            {
                base.CreateChildControls();
            }

            /// <summary>
            /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
            protected override void OnInit(EventArgs e)
            {
                // call the template's instantiate in method
                // to actually instantiate all of the template's controls
                // in the placeholder's control container
                if (Template != null)
                {
                }

                base.OnInit(e);
            }
        }

        #endregion Internal classes
    }
}