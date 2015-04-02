/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TabBar.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: TabBar.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;

[assembly: TagPrefix("Accela.Web.Controls.Navigation", "ACA")]
[assembly: WebResource("Accela.Web.Controls.TabBar.TabUtil.js", "application/x-javascript")]
[assembly: WebResource("Accela.Web.Controls.TabBar.TabBar.js", "application/x-javascript")]
[assembly: WebResource("Accela.Web.Controls.TabBar.menu.js", "application/x-javascript")]

namespace Accela.Web.Controls.Navigation
{
    #region Enumerations

    /// <summary>
    /// Define the link's text align
    /// </summary>
    public enum TextAlign
    {
        /// <summary>
        /// align left
        /// </summary>
        Left,

        /// <summary>
        /// align right
        /// </summary>
        Right,

        /// <summary>
        /// align center
        /// </summary>
        Center
    }

    #endregion Enumerations

    /// <summary>
    /// Navigation Bar
    /// </summary>
    public class TabBar : WebControl
    {
        #region Fields

        /// <summary>
        /// string builder for buffer
        /// </summary>
        private StringBuilder _buf;

        /// <summary>
        /// current tab name
        /// </summary>
        private string _currentTabName;

        /// <summary>
        /// drop down menu template
        /// </summary>
        private TabTemplate _dropDownMenuTemplate;

        /// <summary>
        /// item template object
        /// </summary>
        private TabTemplate _itemTemplate;

        /// <summary>
        /// link item template
        /// </summary>
        private TabTemplate _linkItemTemplate;

        /// <summary>
        /// more link button template
        /// </summary>
        private ServerTemplate _moreLinkBtnTemplate;

        /// <summary>
        /// more template
        /// </summary>
        private ServerTemplate _moreTemplate;

        /// <summary>
        /// selected item template
        /// </summary>
        private TabTemplate _selectedItemTemplate;

        /// <summary>
        /// tab items.
        /// </summary>
        private TabItemCollection _tabItems;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the position of the current tab
        /// </summary>
        [DefaultValue(0)]
        public int CurrentTabIndex
        {
            get
            {
                object o = ViewState["CurrentTabIndex"];

                return o == null ? 0 : (int)o;
            }

            set
            {
                ViewState["CurrentTabIndex"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the current tab user selected
        /// </summary>
        public string CurrentTabName
        {
            get
            {
                if (_currentTabName == null)
                {
                    return string.Empty;
                }

                return _currentTabName;
            }

            set
            {
                _currentTabName = value;
            }
        }

        /// <summary>
        /// Gets or sets Template for the dropdown menu
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TabTemplate DropDownMenuTemplate
        {
            get
            {
                return _dropDownMenuTemplate;
            }

            set
            {
                _dropDownMenuTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets home tab name
        /// </summary>
        public string HomeTabName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is admin mode
        /// </summary>
        public bool IsAdmin
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether if first load, we should set the 'Home' tab active
        /// </summary>
        public bool IsFirstLoad
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is Right to Left?
        /// Use for Arabic layout
        /// </summary>
        public bool IsRTL
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets Template for the normal tab
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TabTemplate ItemTemplate
        {
            get
            {
                return _itemTemplate;
            }

            set
            {
                _itemTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets the link alignment
        /// </summary>
        [DefaultValue(TextAlign.Center)]
        [Description("The link alignment")]
        public TextAlign LinkAlign
        {
            get
            {
                object o = ViewState["LinkAlign"];
                return o == null ? TextAlign.Center : Utilities.ParseTextAlign(o);
            }

            set
            {
                ViewState["LinkAlign"] = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets template for the sub links 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TabTemplate LinkItemTemplate
        {
            get
            {
                return _linkItemTemplate;
            }

            set
            {
                _linkItemTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets more link template
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ServerTemplate MoreLinkButtonTemplate
        {
            get
            {
                if (_moreLinkBtnTemplate == null)
                {
                    return new ServerTemplate();
                }

                return _moreLinkBtnTemplate;
            }

            set
            {
                _moreLinkBtnTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets more template
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ServerTemplate MoreTemplate
        {
            get
            {
                if (_moreTemplate == null)
                {
                    return new ServerTemplate();
                }

                return _moreTemplate;
            }

            set
            {
                _moreTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets template for the selected tab
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TabTemplate SelectedItemTemplate
        {
            get
            {
                return _selectedItemTemplate;
            }

            set
            {
                _selectedItemTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets Tabs and Links Data
        /// </summary>
        public TabItemCollection TabItems
        {
            get
            {
                if (_tabItems == null)
                {
                    return new TabItemCollection();
                }

                return _tabItems;
            }

            set
            {
                _tabItems = value;
            }
        }

        /// <summary>
        /// Gets TabBar Client ID
        /// </summary>
        internal string SaneID
        {
            get
            {
                return "__Tab";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Demarcate client script
        /// </summary>
        /// <param name="script">script content</param>
        /// <returns>demarcated client script</returns>
        internal string DemarcateClientScript(string script)
        {
            StringBuilder result = new StringBuilder();
            bool includeCDATA = true;

            if (this.IsInUpdatePanel() && this.Context.Request.UserAgent.ToLower().IndexOf("safari") >= 0)
            {
                includeCDATA = false; // Safari has problems with CDATA sections in UpdatePanels.
            }

            result.Append("<script type=\"text/javascript\">\n");
            result.Append(includeCDATA ? "//<![CDATA[\n" : null);
            result.Append(script);
            result.Append("\n");
            result.Append(includeCDATA ? "//]]>\n" : null);
            result.Append("</script>\n");
            return result.ToString();
        }

        /// <summary>
        /// Check if the script is in update panel
        /// </summary>
        /// <returns>true indicate is in update panel</returns>
        internal bool IsInUpdatePanel()
        {
            for (Control ctrl = this.Parent; ctrl != null; ctrl = ctrl.Parent)
            {
                if (ctrl is UpdatePanel)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Register script
        /// </summary>
        /// <param name="output">object HtmlTextWriter</param>
        /// <param name="script">script string</param>
        internal void WriteStartupScript(HtmlTextWriter output, string script)
        {
            if (this.IsInUpdatePanel())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), SaneID + "_" + Guid.NewGuid(), script, false);
            }
            else
            {
                output.Write(script);
            }
        }

        /// <summary>
        /// Override OnPreRender
        /// </summary>
        /// <param name="e">event args</param>
        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptResource(this.GetType(), "Accela.Web.Controls.TabBar.TabUtil.js");
            Page.ClientScript.RegisterClientScriptResource(this.GetType(), "Accela.Web.Controls.TabBar.TabBar.js");
            Page.ClientScript.RegisterClientScriptResource(this.GetType(), "Accela.Web.Controls.TabBar.menu.js");

            InstantiateServerTemplate();
            base.OnPreRender(e);
        }

        /// <summary>
        /// Override Render
        /// </summary>
        /// <param name="output">HtmlTextWriter object</param>
        protected override void Render(HtmlTextWriter output)
        {
            if (this._tabItems == null || this._tabItems.Count == 0)
            {
                return;
            }

            RenderServerTemplates(output);

            output.Write("<table role='presentation' style='visibility:hidden'><tr><td id='tdTabContainer'></td></tr></table>");

            output.Write("<div id='nav_parent_container' class='" + CssClass + "'>");
            output.Write("<span id='tab_item_place_holder' style='visibility:hidden;'></span>");
            output.Write("<span id='more_tab_place_holder'></span>");
            output.Write("<div id='divNavMenu' style='display:none;position:absolute;'></div>");
            output.Write("</div>");

            output.Write("<div id='nav_link_place_holder' class='ACA_SubMenuList' align='center'><b>");
            output.Write("<span id='nav_link_content'><span id='nav_span_links' class='font11px'></span><span id='nav_span_more_link'></span></span>");
            output.Write("<div id='divLinkMenu' class='font11px' style='display:none;'></div>");
            output.Write("</b></div>");

            _buf = new StringBuilder();

            //Define a javascript entrance.
            _buf.Append("window." + SaneID + "Init = function() {\n");

            //Check for whether everything we need is loaded,and a retry after a delay in case it isn't.
            _buf.Append("if(!window.NavBar_Loaded || !window.TabBar_Utils_Loaded)\n");
            _buf.Append("\t{window.setTimeout('window." + SaneID + "Init()', 50); return; }\n\n");

            //Create TabBar instance and call it's initialize method.
            _buf.AppendFormat("window.{0} = new TabBar();\n", SaneID);

            GenerateTabItems();
            GenerateItemTemplate();
            GenerateSelectedItemTemplate();
            GenerateDropDownMenuTemplate();
            GenerateLinkItemTemplate();
            GenerateTabProperty();

            //Set TabBar properties
            _buf.AppendFormat("window.setProperties({0}, tab_properties);\n", SaneID);

            //Start to build tabs
            _buf.Append(SaneID + ".preRender();\n");

            _buf.Append("}\n");
            _buf.Append("\n" + SaneID + "Init();\n");

            //render scripts string to the page
            WriteStartupScript(output, DemarcateClientScript(_buf.ToString()));
        }

        /// <summary>
        /// Get DropDownMenuTemplate and convert to javascript properties
        /// </summary>
        private void GenerateDropDownMenuTemplate()
        {
            if (this._dropDownMenuTemplate == null)
            {
                throw new Exception("Must specify the DropDownMenuTemplate for the Tab.");
            }

            string tmpl = this.DropDownMenuTemplate.InnerHtml.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("'", "\\'");
            _buf.Append(SaneID + ".DropDownMenuTemplate='" + tmpl + "';\n");
        }

        /// <summary>
        /// Get ItemTemplate and convert to javascript properties
        /// </summary>
        private void GenerateItemTemplate()
        {
            if (this._itemTemplate == null)
            {
                throw new Exception("Must specify the ItemTemplate for the Tab.");
            }

            string tmpl = this.ItemTemplate.InnerHtml.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("'", "\\'");
            _buf.Append(SaneID + ".ItemTemplate='" + tmpl + "';\n");
        }

        /// <summary>
        /// Get LinkItemTemplate and convert to javascript properties
        /// </summary>
        private void GenerateLinkItemTemplate()
        {
            if (this._linkItemTemplate == null)
            {
                throw new Exception("Must specify the LinkItemTemplate for the Tab.");
            }

            string tmpl = this.LinkItemTemplate.InnerHtml.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("'", "\\'");
            _buf.Append(SaneID + ".LinkItemTemplate='" + tmpl + "';\n");
        }

        /// <summary>
        /// Get SelectedItemTemplate and convert to javascript properties
        /// </summary>
        private void GenerateSelectedItemTemplate()
        {
            if (this._selectedItemTemplate == null)
            {
                throw new Exception("Must specify the SelectedItemTemplate for the Tab.");
            }

            string tmpl = this.SelectedItemTemplate.InnerHtml.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("'", "\\'");
            _buf.Append(SaneID + ".SelectedItemTemplate='" + tmpl + "';\n");
        }

        /// <summary>
        /// Get tabs data and convert to javascript TabItems properties
        /// </summary>
        /// <param name="items">TabItemCollection object</param>
        /// <param name="name">string name</param>
        /// <param name="lastComma">string last comma</param>
        private void GenerateTabItem(TabItemCollection items, string name, string lastComma)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            _buf.Append("['" + name + "',[");
            string comma = string.Empty;
            int order = 0;

            foreach (TabItem tab in items)
            {
                if (comma == string.Empty)
                {
                    comma = ",";
                }
                else
                {
                    _buf.Append(comma);
                }

                _buf.Append("[");

                //Generate Sub Links
                GenerateTabItem(tab.ChildItems, "Links", ",");

                //set string value
                if (tab.Label != string.Empty)
                {
                    _buf.Append("['Label'," + Utilities.ConvertToJsonString(tab.Label) + "],");
                }

                if (tab.Key != string.Empty)
                {
                    _buf.Append("['Key'," + Utilities.ConvertToJsonString(tab.Key) + "],");
                }

                if (tab.Title != string.Empty)
                {
                    _buf.Append("['Title'," + Utilities.ConvertToJsonString(tab.Title) + "],");
                }

                if (tab.Url != string.Empty)
                {
                    _buf.Append("['URL'," + Utilities.ConvertToJsonString(tab.Url) + "],");
                }

                if (tab.Module != string.Empty)
                {
                    _buf.Append("['Module'," + Utilities.ConvertToJsonString(tab.Module) + "],");
                }

                //set integer value
                _buf.Append("['Order'," + order + "]");

                _buf.Append("]");
                order++;
            }

            _buf.Append("]]" + lastComma);
        }

        /// <summary>
        /// Get Tab and Link Data and convert to javascript TabItems properties
        /// </summary>
        private void GenerateTabItems()
        {
            string tabList = string.Empty;

            if (HttpContext.Current.Request.QueryString["TabList"] != null)
            {
                tabList = HttpContext.Current.Request.QueryString["TabList"];
            }
            else
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("TabNav");

                if (cookie != null)
                {
                    tabList = HttpUtility.UrlDecode(cookie.Value);
                }
            }

            if (!string.IsNullOrEmpty(tabList))
            {
                //spectial char used to split tabs' status
                char split = '|';

                //When user click link from 'more' tab's dropdown menu, the tabs' order in tab bar
                //will be reordered, we use tabList to maintains the tags' status and reorder them before redirect
                string[] parms = tabList.Split(split);
                Hashtable ht = new Hashtable();

                try
                {
                    for (int i = 0; i < parms.Length;)
                    {
                        string key = parms[i];
                        string order = parms[i + 1];

                        if (!ht.ContainsKey(key) && !string.IsNullOrEmpty(order))
                        {
                            ht.Add(key, order);
                        }

                        i += 2;
                    }
                }
                catch (Exception)
                {
                }

                foreach (TabItem tabItem in TabItems)
                {
                    if (ht.ContainsKey(tabItem.Key))
                    {
                        int order = -1;
                        bool chk = int.TryParse(ht[tabItem.Key].ToString(), out order);

                        if (chk)
                        {
                            tabItem.Order = order;
                        }
                    }
                }

                if (ht.ContainsKey("CurrentTabIndex"))
                {
                    CurrentTabIndex = int.Parse(ht["CurrentTabIndex"].ToString());
                }
            }
            else
            {
                IsFirstLoad = true;
            }

            TabItems.Sort();

            _buf.Append(SaneID + ".TabItems= [");
            GenerateTabItem(TabItems, "Tabs", string.Empty);
            _buf.Append("];\n");
        }

        /// <summary>
        /// Get TabBar properties and convert to javascript properties
        /// </summary>
        private void GenerateTabProperty()
        {
            _buf.Append("var tab_properties = [");

            //set bool value
            if (IsAdmin)
            {
                _buf.Append("['IsAdmin',1],");
            }

            if (IsRTL)
            {
                _buf.Append("['IsRTL',1],");
            }

            if (IsFirstLoad)
            {
                _buf.Append("['IsFirstLoad',1],");
            }

            //set string value
            _buf.Append("['LinkAlign'," + Utilities.ObjectToJavaScriptString(LinkAlign.ToString().ToLower()) + "],");
            _buf.Append("['CurrentTabName'," + Utilities.ObjectToJavaScriptString(CurrentTabName.ToString().ToLower().Replace("<", "&lt;").Replace(">", "&gt;")) + "],");
            _buf.Append("['DefaultTabName'," + Utilities.ObjectToJavaScriptString(HomeTabName.ToString().ToLower().Replace("<", "&lt;").Replace(">", "&gt;")) + "],");

            //set integer value
            _buf.Append("['CurrentTabIndex'," + CurrentTabIndex + "]");

            _buf.Append("];\n");
        }

        /// <summary>
        /// Add More Tab
        /// </summary>
        private void InstantiateServerTemplate()
        {
            ServerTemplate oTemplate = this.MoreTemplate;

            if (oTemplate != null)
            {
                ServerTemplateContainer oContainer = new ServerTemplateContainer();
                oTemplate.Template.InstantiateIn(oContainer);
                oContainer.ID = "__divMoreTemplate";
                oContainer.DataBind();

                this.Controls.Add(oContainer);
            }

            ServerTemplate moreLinkBtnTemplate = this.MoreLinkButtonTemplate;

            if (moreLinkBtnTemplate != null)
            {
                ServerTemplateContainer oContainer = new ServerTemplateContainer();
                moreLinkBtnTemplate.Template.InstantiateIn(oContainer);
                oContainer.ID = "__divMoreLinkBtnTemplate";
                oContainer.DataBind();

                this.Controls.Add(oContainer);
            }
        }

        /// <summary>
        /// Get the server template of More Tab and build it.
        /// </summary>
        /// <param name="output">HtmlTextWriter object</param>
        private void RenderServerTemplates(HtmlTextWriter output)
        {
            if (this.Controls.Count > 0)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Id, "divMoreTabTemplate");
                output.AddStyleAttribute("display", "none");
                output.RenderBeginTag(HtmlTextWriterTag.Div);

                foreach (Control ctrl in this.Controls)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Id, ctrl.ID);
                    output.RenderBeginTag(HtmlTextWriterTag.Div);
                    ctrl.RenderControl(output);
                    output.RenderEndTag();
                }

                output.RenderEndTag();
            }
        }

        #endregion Methods
    }
}