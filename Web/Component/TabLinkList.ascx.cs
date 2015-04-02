/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TabLinkList.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: TabLinkList.ascx.cs 228255 2012-07-31 13:35:06Z ACHIEVO\grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// TabLinkList Control.
    /// </summary>
    public partial class TabLinkList : BaseUserControl
    {
        /// <summary>
        /// Link Click Event
        /// </summary>
        public event CommonEventHandler LinkClickEvent;

        /// <summary>
        /// Build Tab Item
        /// </summary>
        /// <param name="tab">The instance of TabItem</param>
        /// <param name="setLabel">Is set label</param>
        public static void BuildTabItem(TabItem tab, bool setLabel)
        {
            string label = LabelUtil.GetSuperAgencyTextByKey(tab.Label, tab.Module);

            if (label == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
            {
                label = DataUtil.AddBlankToString(tab.Module);
            }

            tab.Title = LabelUtil.RemoveHtmlFormat(label);

            if (setLabel)
            {
                tab.Label = label;
            }
        }

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindLinks();
        }

        /// <summary>
        /// TabsDataList ItemDataBound
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">CommandEventArgs e</param>
        protected void TabsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Get the Links for the Selected Index
            if (e.Item != null)
            {
                TabItem currentItem = (TabItem)e.Item.DataItem;
                DataList dlItems = e.Item.FindControl("LinksDataList") as DataList;
                if (dlItems != null)
                {
                    dlItems.DataSource = currentItem.Children;
                    dlItems.DataBind();
                }
            }
        }

        /// <summary>
        /// Setting Enable Record Type filter.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">CommandEventArgs e</param>
        protected void LinksDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item != null)
            {
                LinkItem currentItem = (LinkItem)e.Item.DataItem;

                if (currentItem.NeedFilter)
                {
                    AccelaLinkButton linkItem = e.Item.FindControl("LinkItemUrl") as AccelaLinkButton;
                    linkItem.EnableRecordTypeFilter = true;
                }
            }
        }

        /// <summary>
        /// LinkItem OnItemCommand
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">CommandEventArgs e</param>
        protected void LinkItem_OnItemCommand(object sender, CommandEventArgs e)
        {
            string url = e.CommandArgument as string;
            if (url != null)
            {
                if (LinkClickEvent != null)
                {
                    CommonEventArgs args = new CommonEventArgs(url);
                    LinkClickEvent(sender, args);
                }

                if (url.IndexOf("http://", StringComparison.InvariantCulture) == -1 && url.IndexOf("https://", StringComparison.InvariantCulture) == -1)
                {
                    url = FileUtil.AppendApplicationRoot(url);

                    if (url.IndexOf("reportType=" + ACAConstant.PRINT_HOMEPAGE_REPORT, StringComparison.InvariantCulture) > 0)
                    {
                        string script = "javascript:print_onclick('" + Accela.ACA.Common.Common.ScriptFilter.EncodeUrlEx(url) + "');";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "ShowReportJS", script, true);
                        return;
                    }

                    if (url.IndexOf("/Inspection/InspectionResultUpload.aspx", StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        if (AppSession.User == null
                            || !AppSession.User.IsInspector
                            || !StandardChoiceUtil.IsEnabledUploadInspectionResult()
                            || AppSession.User.Licenses == null || AppSession.User.Licenses.Length == 0)
                        {
                            MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("aca_uploadinspresult_msg_professionalaccessdeny"));
                            return;
                        }
                    }
                }
            }
            else
            {
                url = FileUtil.AppendApplicationRoot(ACAConstant.URL_DEFAULT);
            }

            Response.Redirect(url);            
        }

        /// <summary>
        /// Gets report link block.
        /// </summary>
        /// <returns>TabLinkItem object.</returns>
        private TabItem GetReportLink()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            // Get the report name from standard choice.
            string reportName = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_PRINT_REPORT_NAME);

            // If the report name is configruated, add the link.otherwise no report link block.
            if (!string.IsNullOrEmpty(reportName))
            {
                //Home Page Report URL
                string homePageReportUrl = string.Format("/Report/ShowReport.aspx?reportType={0}&reportName={1}&reportID={2}", ACAConstant.PRINT_HOMEPAGE_REPORT, reportName, ACAConstant.NONASSIGN_NUMBER);

                LinkItem linkItem = new LinkItem();
                linkItem.Label = "com_welcome_label_print";
                linkItem.Url = homePageReportUrl;

                TabItem reportTab = new TabItem();
                reportTab.Label = "com_welcome_label_print_title";
                reportTab.Url = homePageReportUrl;
                reportTab.Order = 999; // set enough big order to ensure it is the last tab.
                reportTab.Children.Add(linkItem);

                return reportTab;
            }

            return null;
        }

        /// <summary>
        /// Bind links
        /// </summary>
        private void BindLinks()
        {
            // Get the Tabs List
            IList<TabItem> tabItems = GetTabsList();
            TabsDataList.DataSource = tabItems;
            TabsDataList.DataBind();

            if (tabItems.Count == 1)
            {
                TabsDataList.RepeatColumns = 1;
                TabsDataList.Style.Add("width", "50%");
            }
            else if (tabItems.Count % 2 != 0)
            {
                string script = string.Format("AdjustTabList('{0}');", TabsDataList.ClientID);
                ScriptManager.RegisterStartupScript(this, GetType(), "RemoveEmptyItem", script, true);
            }
        }

        /// <summary>
        /// Gets all of link blocks need to displayed to the current page.
        /// </summary>
        /// <returns>All link blocks.</returns>
        private IList<TabItem> GetTabsList()
        {
            bool isAdminModeRegistered = !string.IsNullOrEmpty(Request.QueryString["registered"]);

            // Get all defined tabs and links.
            IList<TabItem> tabsList = TabUtil.GetTabList(isAdminModeRegistered);

            IList<TabItem> linkTabs = new List<TabItem>();

            foreach (TabItem tab in tabsList)
            {
                // If the tab needn't to be showed in home page as link block
                if (!tab.BlockVisible || string.IsNullOrEmpty(tab.Label))
                {
                    continue;
                }

                BuildTabItem(tab, false);

                // Found the block links in home page.
                if (tab.Children.Count > 0)
                {
                    ICapTypeFilterBll capTypFiltereBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
                    List<LinkItem> listItemList = new List<LinkItem>();

                    foreach (LinkItem subLink in tab.Children)
                    {
                        // In ACA admin, if it is [Welcome Global] page, the upload inspection result link need hide.
                        if (AppSession.IsAdmin
                            && BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS_UPLOAD_INSPECTION.Equals(subLink.Key, StringComparison.InvariantCultureIgnoreCase)
                            && Request.Url.AbsolutePath.EndsWith(ACAConstant.URL_WELCOME_PAGE, StringComparison.InvariantCultureIgnoreCase)
                            && !ValidationUtil.IsYes(Request["registered"]))
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(subLink.Label) || !TabUtil.HasPermission(subLink))
                        {
                            continue;
                        }

                        string filterName = capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, subLink.Module, subLink.Label);

                        // Append tab name to url to ensure tab can be selected correctly.
                        subLink.Url = TabUtil.RebuildUrl(subLink.Url, tab.Key, filterName);
                        listItemList.Add(subLink);
                    }

                    tab.Children = listItemList;
                    linkTabs.Add(tab);
                }
            }

            // Report block is configurated in standard choice,which is diffrent with other block.
            TabItem reportBlock = GetReportLink();
            if (reportBlock != null)
            {
                linkTabs.Add(reportBlock);
            }

            return linkTabs;
        }
    }
}