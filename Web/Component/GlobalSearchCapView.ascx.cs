#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchCapView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description: Display global search CAP list
 *
 *  Notes:
 *      $Id: GlobalSearchCapView.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// global search CAP view
    /// </summary>
    public partial class GlobalSearchCapView : BaseUserControl
    {
        #region Events

        /// <summary>
        /// grid view page index changing event
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Occurs when [list loading].
        /// </summary>
        public event EventHandler ListLoading;

        /// <summary>
        /// Occurs when [list loaded].
        /// </summary>
        public event EventHandler ListLoaded;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets DataSource
        /// </summary>
        public List<CAPView4UI> DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = new List<CAPView4UI>();
                }

                return (List<CAPView4UI>)ViewState["DataSource"];
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount
        {
            get
            {
                return gdvPermitList.CustomizedTotalCount;
            }
        }

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return gdvPermitList.PageSize;
            }
        }

        /// <summary>
        /// Gets the total count in GridView.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gdvPermitList.CountSummary;
            }
        }

        /// <summary>
        /// Gets or sets the moduleName of base user control.
        /// </summary>
        protected override string ModuleName
        {
            get
            {
                if (ViewState["SectionModuleName"] != null)
                {
                    return ViewState["SectionModuleName"].ToString();
                }
                else
                {
                    return Request.QueryString[ACAConstant.MODULE_NAME];
                }
            }

            set
            {
                ViewState["SectionModuleName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading history.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is loading history; otherwise, <c>false</c>.
        /// </value>
        private bool IsLoadingHistory
        {
            get;
            set;
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Displays the specified list.
        /// </summary>
        /// <param name="capList">The cap list.</param>
        /// <param name="isLoadingHistory">if set to <c>true</c> [is loading history].</param>
        public void Display(List<CAPView4UI> capList, bool isLoadingHistory)
        {
            IsLoadingHistory = isLoadingHistory;

            if (isLoadingHistory)
            {
                SetupViewByHistory();
            }

            GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);

            if (historyParameter != null)
            {
                gdvPermitList.CustomizedTotalCount = historyParameter.TotalRecordsFromWS;
            }

            gdvPermitList.DataSource = capList;
            gdvPermitList.DataBind();
        }

        /// <summary>
        /// Sets the grid view sort info.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public void SetGridViewSortInfo(string sortColumn, string sortDirection)
        {
            gdvPermitList.GridViewSortExpression = sortColumn;
            gdvPermitList.GridViewSortDirection = sortDirection;
        }

        /// <summary>
        /// Gets the selected modules.
        /// </summary>
        /// <returns>the selected modules</returns>
        public string[] GetSelectedModules()
        {
            string selectedModuleName = string.Empty;

            if (ddlModule.Items.Count > 0)
            {
                selectedModuleName = ddlModule.SelectedValue;
            }

            return selectedModuleName.Split(ACAConstant.COMMA_CHAR);
        }

        /// <summary>
        /// Navigate To cap detail page.
        /// </summary>
        /// <param name="capViewList">cap view UI list model</param>
        /// <returns>IsNavigate Flag</returns>
        public bool RedirectToCapDetail(List<CAPView4UI> capViewList)
        {
            bool isNavigate = false;

            if (capViewList != null && capViewList.Count == 1)
            {
                var capView = capViewList[0];

                if (!CapUtil.IsPartialCap(capView.CapClass))
                {
                    ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

                    CapIDModel4WS capIDModel = new CapIDModel4WS
                    {
                        serviceProviderCode = !string.IsNullOrEmpty(capView.AgencyCode) ? capView.AgencyCode : ConfigManager.AgencyCode,
                        customID = capView.PermitNumber
                    };

                    CapModel4WS capModel = capBll.GetCapByAltID(capIDModel);

                    // Can not redirect to detail page if it is the Complete Paid status.
                    if (capModel != null && !ACAConstant.PAYMENT_STATUS_PAID.Equals(capModel.paymentStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isNavigate = true;
                        Response.Redirect(ConstructDetailUrl(capView));
                    }
                }
            }

            return isNavigate;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvPermitList, ModuleName, AppSession.IsAdmin);
            InitalExport();
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetupModules();

                ControlBuildHelper.SetInstructionValue(lblCAPViewTitle_sub_label, GetTextByKey("per_globalsearch_label_cap|sub"));
                ddlModule.ToolTip = LabelUtil.GetTextByKey("global_select_module_title", ModuleName);
            }
        }

        /// <summary>
        /// Module Dropdown List IndexChange
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ModuleDropdown_IndexChanged(object sender, EventArgs e)
        {
            if (ListLoading != null)
            {
                ListLoading(sender, e);
            }

            string[] moduleArray = string.IsNullOrEmpty(ddlModule.SelectedValue) ? new string[] { string.Empty } : ddlModule.SelectedValue.Split(ACAConstant.COMMA_CHAR);

            //when module is changed in cap result,should set TotalRecordsFromWS=0
            GlobalSearchParameter parameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);
            parameter.TotalRecordsFromWS = 0;
            string[] hiddenViewElementNames = ControlBuildHelper.GetHiddenViewElementNames(GviewID.GlobalSearchCapList, ModuleName);
            List<CAPView4UI> capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, moduleArray, hiddenViewElementNames);
            gdvPermitList.PageIndex = 0;
            Display(capList);

            if (ListLoaded != null)
            {
                ListLoaded(sender, e);
            }
        }

        /// <summary>
        /// fire page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (ListLoading != null)
            {
                ListLoading(sender, e);
            }

            gdvPermitList.PageIndex = e.NewPageIndex;

            GlobalSearchParameter globalSearchParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);

            if (globalSearchParameter != null)
            {
                globalSearchParameter.PageIndex = e.NewPageIndex;
            }

            if (globalSearchParameter != null && globalSearchParameter.NeedRequestNewRecords())
            {
                string[] hiddenViewElementNames = ControlBuildHelper.GetHiddenViewElementNames(GviewID.GlobalSearchCapList, ModuleName);
                List<CAPView4UI> capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, e.NewPageIndex, hiddenViewElementNames);
                Display(capList);
            }
            else
            {
                gdvPermitList.DataBind();
            }

            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }

            if (ListLoaded != null)
            {
                ListLoaded(sender, e);
            }
        }

        /// <summary>
        /// GridView PermitList RowCommand
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void PermitList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ("Header".Equals(e.CommandName, StringComparison.OrdinalIgnoreCase) && e.CommandSource is GridViewHeaderLabel)
            {
                if (ListLoading != null)
                {
                    ListLoading(sender, e);
                }

                AccelaGridView currentGridView = sender as AccelaGridView;
                currentGridView.PageIndex = 0;
                string newSortExpression = ((GridViewHeaderLabel)e.CommandSource).SortExpression;
                string newSortDirection = ACAConstant.ORDER_BY_ASC.Equals(currentGridView.GridViewSortDirection, StringComparison.OrdinalIgnoreCase) ? ACAConstant.ORDER_BY_DESC : ACAConstant.ORDER_BY_ASC;

                string[] hiddenViewElementNames = ControlBuildHelper.GetHiddenViewElementNames(GviewID.GlobalSearchCapList, ModuleName);
                List<CAPView4UI> capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, newSortExpression, newSortDirection, hiddenViewElementNames);
                Display(capList);

                if (ListLoaded != null)
                {
                    ListLoaded(sender, e);
                }
            }
            else if (e.CommandName == "Action")
            {
                string[] parameters = e.CommandArgument.ToString().Split(',');
                string capID1 = parameters[0];
                string capID2 = parameters[1];
                string capID3 = parameters[2];
                string agencyCode = parameters[3];
                string moduleName = parameters[4];

                // if complete paid record, show message
                ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                CapIDModel4WS capIdModel = new CapIDModel4WS() { id1 = capID1, id2 = capID2, id3 = capID3, serviceProviderCode = agencyCode };
                CapModel4WS capModel = capBll.GetCapByPK(capIdModel);

                if (capModel != null && ACAConstant.PAYMENT_STATUS_PAID.Equals(capModel.paymentStatus, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Disallow to view record detail if the record in Paid status.
                    MessageUtil.ShowMessageByControl(Page, MessageType.Notice, GetTextByKey("aca_globalsearch_msg_completepaidrecordview"));
                    return;
                }

                string url = string.Format(
                       "../Cap/CapDetail.aspx?Module={0}&TabName={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}",
                       ScriptFilter.AntiXssUrlEncode(UrlEncode(moduleName)),
                       UrlEncode(capID1),
                       UrlEncode(capID2),
                       UrlEncode(capID3),
                       UrlConstant.AgencyCode,
                       UrlEncode(agencyCode));

                Response.Redirect(url);
            }
        }

        /// <summary>
        /// GridView PermitList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void PermitList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem is CAPView4UI)
            {
                CAPView4UI currentItem = e.Row.DataItem as CAPView4UI;
                Label lblPermitNum = e.Row.FindControl("lblPermitNumber") as Label;
                LinkButton lnkPermitNumber = e.Row.FindControl("lnkPermitNumber") as LinkButton;
                Literal litRelatedRecords = e.Row.FindControl("litRelatedRecords") as Literal;

                string[] capIDPartArray = currentItem.CapID.Split(new[] { ACAConstant.SPLIT_CHAR4 }, StringSplitOptions.None);
                string capID1 = capIDPartArray[0];
                string capID2 = capIDPartArray[1];
                string capID3 = capIDPartArray[2];
                string agencyCode = currentItem.AgencyCode;
                string moduleName = currentItem.ModuleName;

                if (ACAConstant.COMPLETED.Equals(currentItem.CapClass, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(currentItem.CapClass))
                {
                    lblPermitNum.Visible = false;
                    lnkPermitNumber.Visible = true;

                    lnkPermitNumber.CommandArgument = string.Format(
                        "{1}{0}{2}{0}{3}{0}{4}{0}{5}",
                        ACAConstant.COMMA,
                        capID1,
                        capID2,
                        capID3,
                        agencyCode,
                        moduleName);
                }
                else
                {
                    lblPermitNum.Visible = true;
                    lnkPermitNumber.Visible = false;
                }

                if (litRelatedRecords != null)
                {
                    if (currentItem.RelatedRecords > 0)
                    {
                        Random rd = new Random();
                        string lnkRelatedRecordID = Convert.ToString(rd.Next());
                        byte[] args = Encoding.UTF8.GetBytes(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", ACAConstant.SPLIT_CHAR, capID1, capID2, capID3, agencyCode, moduleName));
                        string url = string.Format(ResolveUrl("~/Cap/RelatedRecords.aspx?module={0}&args={1}"), moduleName, HttpUtility.UrlEncode(Convert.ToBase64String(args)));
                        string showRelatedJs = "ACADialog.popup({url:'" + url + "',width:800,height:500,objectTarget:'" + lnkRelatedRecordID + "'});";
                        litRelatedRecords.Text = string.Format("<a id='{2}' href=\"javascript:{0}\">{1}</a> ", showRelatedJs, currentItem.RelatedRecords, lnkRelatedRecordID);
                    }
                    else
                    {
                        litRelatedRecords.Text = "0";
                    }
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Displays the specified list.
        /// </summary>
        /// <param name="list">The CAPView4UI list.</param>
        private void Display(List<CAPView4UI> list)
        {
            Display(list, false);
        }

        /// <summary>
        /// Setups the view by history.
        /// </summary>
        private void SetupViewByHistory()
        {
            GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);

            if (historyParameter != null)
            {
                gdvPermitList.PageIndex = historyParameter.PageIndex;
                gdvPermitList.GridViewSortExpression = historyParameter.SortColulmn;
                gdvPermitList.GridViewSortDirection = historyParameter.SortDirection;
            }
        }

        /// <summary>
        /// Initializes the GridView's export link visible. 
        /// </summary>
        private void InitalExport()
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvPermitList.ShowExportLink = true;
                gdvPermitList.ExportFileName = "Record";
            }
            else
            {
                gdvPermitList.ShowExportLink = false;
            }
        }

        /// <summary>
        /// Setups the modules.
        /// </summary>
        private void SetupModules()
        {
            if (AppSession.IsAdmin)
            {
                ddlModule.Visible = false;
            }
            else
            {
                ddlModule.Visible = true;
                DropDownListBindUtil.BindModules(ddlModule, true);
                ddlModule.Attributes.Add("onchange", string.Format(CultureInfo.InvariantCulture, "showLoadingPanel('{0}');", ddlModule.ClientID));

                GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);

                if (IsLoadingHistory && historyParameter != null)
                {
                    string selectedModule = string.Join(ACAConstant.COMMA, historyParameter.ModuleArray);
                    ListItem selectedItem = ddlModule.Items.FindByValue(selectedModule);

                    if (selectedItem != null)
                    {
                        ddlModule.SelectedIndex = -1;
                        selectedItem.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Construct detail page URL
        /// </summary>
        /// <param name="capView">Cap view UI model</param>
        /// <returns>URL string</returns>
        private string ConstructDetailUrl(CAPView4UI capView)
        {
           var capIDPartArray = capView.CapID.Split(new[] { ACAConstant.SPLIT_CHAR4 }, StringSplitOptions.None);
            var capID1 = capIDPartArray[0];
            var capID2 = capIDPartArray[1];
            var capID3 = capIDPartArray[2];

            var url = string.Format(
                        CultureInfo.InvariantCulture,
                        "Cap/CapDetail.aspx?Module={0}&TabName={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}",
                        ScriptFilter.AntiXssUrlEncode(UrlEncode(capView.ModuleName)),
                        UrlEncode(capID1),
                        UrlEncode(capID2),
                        UrlEncode(capID3),
                        UrlConstant.AgencyCode,
                        UrlEncode(capView.AgencyCode));

            return FileUtil.AppendApplicationRoot(url);
        }

        #endregion Private Methods
    }
}