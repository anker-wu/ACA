#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PlanList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PlanList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Plan;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation PlanList.
    /// </summary>
    public partial class PlanList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// data source type upload.
        /// </summary>
        protected const string DATA_SOURCE_TYPE_UPLOADED = "Uploaded";

        /// <summary>
        /// plan delete.
        /// </summary>
        protected readonly string PlanDelete = BasePage.GetStaticTextByKey("ACA_AttachmentList_Label_Delete");

        /// <summary>
        /// column plan sequence.
        /// </summary>
        private const string COLUMN_PLANSEQ = "planSeq";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets data source type
        /// </summary>
        public string DataSourceType
        {
            get
            {
                return ViewState["SOURCETYPE"] as string;
            }

            set
            {
                ViewState["SOURCETYPE"] = value;
            }
        }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                if (ViewState["PermitList"] == null)
                {
                    ViewState["PermitList"] = CreateDataSource();
                }

                return (DataTable)ViewState["PermitList"];
            }

            set
            {
                ViewState["PermitList"] = value;
            }
        }

        /// <summary>
        /// Gets or sets sorting direction
        /// </summary>
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                {
                    ViewState["sortDirection"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["sortDirection"];
            }

            set
            {
                ViewState["sortDirection"] = value;
            }
        }

        /// <summary>
        /// Gets plans sequence.
        /// </summary>
        public long[] PlansSeq
        {
            get
            {
                if (GridViewDataSource == null)
                {
                    return null;
                }

                if (GridViewDataSource.Rows.Count == 1 && GridViewDataSource.Rows[0].IsNull(COLUMN_PLANSEQ))
                {
                    return null;
                }

                long[] plans = new long[GridViewDataSource.Rows.Count];

                for (int i = 0; i < GridViewDataSource.Rows.Count; i++)
                {
                    plans[i] = (long)GridViewDataSource.Rows[i][COLUMN_PLANSEQ];
                }

                return plans;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind plan list.
        /// </summary>
        public void BindPlanList()
        {
            if (AppSession.IsAdmin)
            {
                gdvPermitList.DataSource = ConstructDataTableStructure();
                gdvPermitList.DataBind();
                return;
            }

            gdvPermitList.DataSource = GridViewDataSource;
            gdvPermitList.DataBind();
        }

        /// <summary>
        /// Create data source for plan list.
        /// </summary>
        /// <returns>DataTable of the data source</returns>
        public DataTable CreateDataSource()
        {
            DataTable dataSource = DataSourceType == DATA_SOURCE_TYPE_UPLOADED ? CreateUploadedPlanDataSource() : CreateDataSourceByViewType(0);

            return dataSource;
        }

        /// <summary>
        /// Rebind plan list.
        /// </summary>
        public void RebindPlanList()
        {
            GridViewDataSource = CreateDataSource();

            gdvPermitList.DataSource = GridViewDataSource;
            gdvPermitList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvPermitList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gdvPermitList.PagerSettings.PreviousPageText = BasePage.GetStaticTextByKey("ACA_NextPrevNumbericPagerTemplate_PrevText");
                gdvPermitList.PagerSettings.NextPageText = BasePage.GetStaticTextByKey("ACA_NextPrevNumbericPagerTemplate_NextText");
                lblViewTitle.Text = BasePage.GetStaticTextByKey("ACA_PlanList_PlanFilterTitle");
                BindPlanList();
            }
        }

        /// <summary>
        /// PagerButton Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PagerButton_Click(object sender, EventArgs e)
        {
            int pageIndx = Convert.ToInt32(txtCurrentPage.Value);
            int totals = GridViewDataSource.Rows.Count;
            int pageSize = gdvPermitList.PageSize;
            int pages = (totals % pageSize) == 0 ? (totals / pageSize) : ((totals / pageSize) + 1);
            string arg = ((LinkButton)sender).CommandArgument.ToLowerInvariant();

            switch (arg)
            {
                case "prev":

                    if (pageIndx > 0)
                    {
                        pageIndx -= 1;
                    }

                    break;
                case "next":

                    if (pageIndx < pages - 1)
                    {
                        pageIndx += 1;
                    }

                    break;
            }

            gdvPermitList.PageIndex = pageIndx;
            txtCurrentPage.Value = pageIndx.ToString();
            gdvPermitList.DataSource = GridViewDataSource;
            gdvPermitList.DataBind();
        }

        /// <summary>
        /// SelectPlan Dropdown List SelectedIndexChanged
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SelectPlanDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlSelectPlan.SelectedValue;

            int viewType = 0;

            if (value.Equals("active", StringComparison.InvariantCulture))
            {
                viewType = 1;
            }

            if (value.Equals("inactive", StringComparison.InvariantCulture))
            {
                viewType = 2;
            }

            DataTable td = CreateDataSourceByViewType(viewType);

            if (td.Rows.Count == 0)
            {
                td.Rows.Add(td.NewRow());
            }

            gdvPermitList.DataSource = td;
            gdvPermitList.DataBind();
            ViewState["PermitList"] = td;
        }

        /// <summary>
        /// GridView PermitList PageIndexChanging
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvPermitList.PageIndex = e.NewPageIndex;
            gdvPermitList.DataSource = GridViewDataSource;
            gdvPermitList.DataBind();
        }

        /// <summary>
        /// GridView PermitList PreRender
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_PreRender(object sender, EventArgs e)
        {
            SetCustomNavigation(gdvPermitList.BottomPagerRow);
        }

        /// <summary>
        /// GridView PermitList PreRender
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string temp = (string)e.CommandArgument;
            if (e.CommandName == "deleteRow")
            {
                if (!string.IsNullOrEmpty(temp))
                {
                    PublicUserPlanModel4WS model = new PublicUserPlanModel4WS();
                    model.servProvCode = ConfigManager.AgencyCode;
                    model.publicUserID = AppSession.User.PublicUserId;
                    model.planSeqNbr = Convert.ToInt32(temp);

                    IPlanBll planBll = ObjectFactory.GetObject<IPlanBll>();
                    planBll.RemovePublicUserPlan(model);
                    RebindPlanList();
                }
            }

            if (e.CommandName == "pay")
            {
                if (!string.IsNullOrEmpty(temp))
                {
                    HttpContext.Current.Response.Redirect("~/plan/PlanPay.aspx?planSeqNbr=" + temp, true);
                }
            }

            if (e.CommandName == "view")
            {
                if (!string.IsNullOrEmpty(temp))
                {
                }
            }

            if (e.CommandName == "Header")
            {
                RebindPlanList();
            }
        }

        /// <summary>
        /// GridView PermitList RowCreated
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                SetCustomNavigation(e.Row);
            }
        }

        /// <summary>
        /// GridView PermitList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaLinkButton lblDelete = (AccelaLinkButton)e.Row.FindControl("btnDelete");
                lblDelete.Text = PlanDelete;
            }
        }

        /// <summary>
        /// GridView PermitList Sorting
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_Sorting(object sender, GridViewSortEventArgs e)
        {
            Sort(e);
        }

        /// <summary>
        /// Construct the structure of the data source for plan list.
        /// </summary>
        /// <returns>DataTable Structure</returns>
        private DataTable ConstructDataTableStructure()
        {
            DataTable dataSource = new DataTable();

            dataSource.Columns.Add(new DataColumn("planName", typeof(string)));
            dataSource.Columns.Add(new DataColumn("fileName", typeof(string)));
            dataSource.Columns.Add(new DataColumn("ruleset", typeof(string)));
            dataSource.Columns.Add(new DataColumn("reviewStatus", typeof(string)));
            dataSource.Columns.Add(new DataColumn("submitDate", typeof(DateTime)));
            dataSource.Columns.Add(new DataColumn(COLUMN_PLANSEQ, typeof(long)));

            return dataSource;
        }

        /// <summary>
        /// Create data source for plan list by view type.
        /// </summary>
        /// <param name="viewType">View type Number</param>
        /// <returns>DataTable of the data source</returns>
        private DataTable CreateDataSourceByViewType(int viewType)
        {
            DataTable dataSource = ConstructDataTableStructure();

            IPlanBll planBll = ObjectFactory.GetObject<IPlanBll>();

            Array result = planBll.GetPublicUserPlans(ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            foreach (PublicUserPlanModel4WS model in result)
            {
                if (viewType == 1 && !model.planStatus.Equals(PlanReviewStatus.Completed.ToString(), StringComparison.InvariantCulture))
                {
                    continue;
                }

                if (viewType == 2 && model.planStatus.Equals(PlanReviewStatus.Completed.ToString(), StringComparison.InvariantCulture))
                {
                    continue;
                }

                dataSource.Rows.Add(new object[]
                                        {
                                            model.planName, model.fileDisplayName, GetRulesets(model.ruleSet), model.planStatus,
                                            I18nDateTimeUtil.FormatToDateStringForWebService(I18nDateTimeUtil.ParseFromWebService(model.submitDate)), model.planSeqNbr
                                        });
            }

            return dataSource;
        }

        /// <summary>
        /// Create data source for the uploaded plan list.
        /// </summary>
        /// <returns>plan dataTable</returns>
        private DataTable CreateUploadedPlanDataSource()
        {
            DataTable dataSource = ConstructDataTableStructure();

            IPlanBll planBll = ObjectFactory.GetObject<IPlanBll>();
            Array result = planBll.GetPublicUserUploadedPlans();

            foreach (PublicUserPlanModel4WS model in result)
            {
                dataSource.Rows.Add(new object[] { model.planName, model.fileDisplayName, GetRulesets(model.ruleSet), model.planStatus, model.submitDate, model.planSeqNbr });
            }

            return dataSource;
        }

        /// <summary>
        /// Generate the RuleSet string with the multiple RuleSets.
        /// </summary>
        /// <param name="rulesets">string for rule sets.</param>
        /// <returns>string for rules</returns>
        private string GetRulesets(string[] rulesets)
        {
            StringBuilder tmp = new StringBuilder();
            string rule = string.Empty;
            foreach (string ruleset in rulesets)
            {
                rule = ruleset;
                GetRuseFromMultipleLanguage(ref rule);
                tmp.Append(rule);
                tmp.Append("<br />");
            }

            tmp.Remove(tmp.Length - 6, 6);

            return tmp.ToString();
        }

        /// <summary>
        /// Get Ruse from Multiple Language
        /// </summary>
        /// <param name="rule">string for rule.</param>
        private void GetRuseFromMultipleLanguage(ref string rule)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> items = bizBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_SELF_PLAN_RULESET, false);
            foreach (ItemValue item in items)
            {
                if (item.Key == rule)
                {
                    rule = item.Value.ToString();
                    return;
                }
            }
        }

        /// <summary>
        /// Set Custom Navigation
        /// </summary>
        /// <param name="pagerRow">pager Row of GV</param>
        private void SetCustomNavigation(GridViewRow pagerRow)
        {
            string prevDisabled = string.Empty;
            string nextDisabled = string.Empty;
            int pageIndx = gdvPermitList.PageIndex;
            int totals = GridViewDataSource.Rows.Count;
            int pageSize = gdvPermitList.PageSize;
            int pages = (totals % pageSize) == 0 ? (totals / pageSize) : ((totals / pageSize) + 1);

            int prevPage = gdvPermitList.PageIndex == 0 ? 1 : gdvPermitList.PageIndex;
            int nextPage = gdvPermitList.PageIndex + 2 >= pages ? pages : gdvPermitList.PageIndex + 2;

            if (pageIndx == 0)
            {
                prevDisabled = "disabled=\"disabled\"";
                if (totals <= pageSize)
                {
                    nextDisabled = "disabled=\"disabled\"";
                }
            }
            else if (pageIndx == pages - 1)
            {
                nextDisabled = "disabled=\"disabled\"";
            }
            else
            {
                prevDisabled = string.Empty;
                nextDisabled = string.Empty;
            }

            pagerRow.Cells[0].ColumnSpan = pagerRow.Cells[0].ColumnSpan + 5;
            Table indicatorTable = (Table)pagerRow.Cells[0].Controls[0];
            TableCell tcPrev = new TableCell();
            tcPrev.HorizontalAlign = Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft ? HorizontalAlign.Right : HorizontalAlign.Left;
            string url = "<a id =\"btnPrev\" href=\"javascript:__doPostBack('" + gdvPermitList.UniqueID + "','Page$" + prevPage.ToString() + "')\"" + prevDisabled + ">Prev</a>";
            tcPrev.Controls.Add(new LiteralControl(url));
            indicatorTable.Rows[0].Cells.AddAt(0, tcPrev);
            TableCell tcPrevSpace = new TableCell();
            tcPrev.Controls.Add(
                new LiteralControl(
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
            indicatorTable.Rows[0].Cells.AddAt(1, tcPrevSpace);
            TableCell tcLabel = new TableCell();
            tcLabel.Controls.Add(new LiteralControl("<span>Additional Results:</span>"));
            indicatorTable.Rows[0].Cells.AddAt(2, tcLabel);

            TableCell tcNextSpace = new TableCell();
            tcNextSpace.Controls.Add(
                new LiteralControl(
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
            indicatorTable.Rows[0].Cells.AddAt(indicatorTable.Rows[0].Cells.Count, tcNextSpace);
            TableCell tcNext = new TableCell();
            tcNext.HorizontalAlign = Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft ? HorizontalAlign.Left : HorizontalAlign.Right;
            url = "<a  id =\"btnPrev\" href=\"javascript:__doPostBack('" + gdvPermitList.UniqueID + "','Page$" + nextPage.ToString() + "')\"" + nextDisabled + ">Next</a>";
            tcNext.Controls.Add(new LiteralControl(url));
            indicatorTable.Rows[0].Cells.AddAt(indicatorTable.Rows[0].Cells.Count, tcNext);

            if (GridViewDataSource.Rows[0]["fileName"] == null || GridViewDataSource.Rows[0]["fileName"].ToString() == string.Empty)
            {
                int columnCount = gdvPermitList.Rows[0].Cells.Count;
                gdvPermitList.Rows[0].Cells.Clear();
                gdvPermitList.Rows[0].Cells.Add(new TableCell());
                gdvPermitList.Rows[0].Cells[0].ColumnSpan = columnCount;
                gdvPermitList.Rows[0].Cells[0].Text = BasePage.GetStaticTextByKey("ACA_PlanList_NoRecordsFound");
            }
        }

        /// <summary>
        /// sort for grid view.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        private void Sort(GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortDirection = string.Empty;

            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                sortDirection = "DESC";
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                sortDirection = "ASC";
            }

            DataTable dt = (DataTable)GridViewDataSource;
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + " " + sortDirection;
            gdvPermitList.PageIndex = 0;
            gdvPermitList.DataSource = dv;
            gdvPermitList.DataBind();
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// This class provide the ability to operation CapModelDataComparer.
        /// </summary>
        public class CapModelDataComparer : IComparer
        {
            #region Fields

            /// <summary>
            /// sort expression.
            /// </summary>
            private string sortExpression;

            #endregion Fields

            #region Properties

            /// <summary>
            /// Gets or sets sort expression.
            /// </summary>
            public string SortExpression
            {
                get
                {
                    return sortExpression;
                }

                set
                {
                    sortExpression = value;
                }
            }

            #endregion Properties

            #region Methods

            /// <summary>
            /// Object compare
            /// </summary>
            /// <param name="x">x: object data.</param>
            /// <param name="y">y: object data.</param>
            /// <returns>Result for compare.</returns>
            public int Compare(object x, object y)
            {
                //if (x == null || y == null) return 0;

                //CapModel4WS model1 = x as CapModel4WS;
                //CapModel4WS model2 = y as CapModel4WS;

                //if (model1.sortExpression > model2.sortExpression)
                //    return 1;
                //else
                return 0;
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}