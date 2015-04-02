#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GeneralDrillDown.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: GeneralDrillDown.aspx.cs 189386 2011-01-24 01:51:47Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.Web.Controls;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// The general drill down to select.
    /// </summary>
    public partial class GeneralDrillDown : PopupDialogBasePage
    {
        #region Field

        /// <summary>
        /// The split char
        /// </summary>
        protected const char SplitChar = '|';

        /// <summary>
        /// The constant for first step
        /// </summary>
        protected const string STEP_TYPE_FIRST = "FirstStep";
        
        /// <summary>
        /// The constant for last step
        /// </summary>
        protected const string STEP_TYPE_LAST = "LastStep";

        /// <summary>
        /// The NIGP type for all
        /// </summary>
        private const string NIGP_TYPE_ALL = "All Goods and Services";

        #endregion Field

        #region Properties

        /// <summary>
        /// Gets or sets the NIGP type, ex: Goods or Services
        /// </summary>
        public string NigpType
        {
            get
            {
                if (ViewState["NigpType"] != null)
                {
                    return (string)ViewState["NigpType"];
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                ViewState["NigpType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the NIGP class codes, split by comma
        /// </summary>
        public string NigpClassCodes
        {
            get
            {
                if (ViewState["NigpClassCodes"] != null)
                {
                    return (string)ViewState["NigpClassCodes"];
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                ViewState["NigpClassCodes"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of step.
        /// </summary>
        public string StepType
        {
            get
            {
                if (ViewState["StepType"] != null)
                {
                    return (string)ViewState["StepType"];
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                ViewState["StepType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the first step's grid view data source
        /// </summary>
        public DataTable FirstStepGridViewDataSource
        {
            get
            {
                if (ViewState["FirstStepGridViewDataSource"] != null)
                {
                    return (DataTable)ViewState["FirstStepGridViewDataSource"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                ViewState["FirstStepGridViewDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the last step's grid view data source
        /// </summary>
        public DataTable LastStepGridViewDataSource
        {
            get
            {
                if (ViewState["LastStepGridViewDataSource"] != null)
                {
                    return (DataTable)ViewState["LastStepGridViewDataSource"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                ViewState["LastStepGridViewDataSource"] = value;
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Raises the initial event
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gvDrillDownList, ModuleName, AppSession.IsAdmin);

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            NigpType = Request["nigpType"];

            if (!IsPostBack)
            {
                SetFirstStepStatus();
                BindFirstStepDataSource();
            }
            else
            {
                if (string.IsNullOrEmpty(StepType) || StepType == STEP_TYPE_FIRST)
                {
                    SetFirstStepStatus();
                    BindFirstStepDataSource();
                }
                else
                {
                    SetLastStepStatus();
                    BindLastStepDataSource();
                }
            }
        }

        /// <summary>
        /// DrillDownList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void DrillDownList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ListItemType itemType = (ListItemType)e.Row.RowType;

            switch (itemType)
            {
                case ListItemType.Header:
                    GridViewHeaderLabel lblKeyHeader = e.Row.FindControl("lblKeyHeader") as GridViewHeaderLabel;
                    CheckBox chkAll = e.Row.FindControl("chkAll") as CheckBox;
                    
                    if (chkAll != null)
                    {
                        chkAll.Visible = true;
                        chkAll.InputAttributes.Add("title", GetTextByKey("aca_selectallrecords_checkbox"));
                    }

                    if (STEP_TYPE_FIRST.Equals(StepType))
                    {
                        lblKeyHeader.LabelKey = "aca_certbusiness_label_drilldown_nigpclasslist_classcode";
                    }
                    else
                    {
                        lblKeyHeader.LabelKey = "aca_certbusiness_label_drilldown_nigpsubclasslist_classcode";
                    }

                    break;
                case ListItemType.Item:
                    CheckBox cb = e.Row.FindControl("chkSingle") as CheckBox;
                    AccelaLabel lblID = e.Row.FindControl("lblID") as AccelaLabel;

                    if (cb != null && lblID != null)
                    {
                        cb.InputAttributes.Add("value", lblID.Text);
                        cb.InputAttributes.Add("title", GetTextByKey("aca_selectonerecord_checkbox"));
                        string selectedValue = string.Empty;

                        if (StepType == STEP_TYPE_FIRST)
                        {
                            selectedValue = hdThreeDigitSelectedValue.Value;
                        }
                        else
                        {
                            selectedValue = hdFiveDigitSelectedValue.Value;                   
                        }

                        if (!string.IsNullOrEmpty(selectedValue) && selectedValue.IndexOf(SplitChar + lblID.Text + SplitChar) != -1)
                        {
                            cb.Checked = true;
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Raises the Next button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void NextButton_Click(object sender, EventArgs e)
        {
            SetLastStepStatus();

            string selectedValue = hdThreeDigitSelectedValue.Value;
            NigpClassCodes = selectedValue;
            txtSearch.Text = string.Empty;

            if (!string.IsNullOrEmpty(selectedValue))
            {
                string[] threeDigitNigpCodes = selectedValue.Split(SplitChar);

                BindLastStepDataSource(threeDigitNigpCodes, null, true, true);
            }
        }

        /// <summary>
        /// Raises the Back button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void BackButton_Click(object sender, EventArgs e)
        {
            SetFirstStepStatus();

            NigpClassCodes = string.Empty;
            txtSearch.Text = string.Empty;

            BindFirstStepDataSource(null, true, true);
        }

        /// <summary>
        /// Raises the Search button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string searchNigpCode = txtSearch.Text.Trim();

            if (StepType == STEP_TYPE_FIRST)
            {
                BindFirstStepDataSource(searchNigpCode, true, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(NigpClassCodes))
                {
                    string[] parentNigpCodes = NigpClassCodes.Split(SplitChar);
                    BindLastStepDataSource(parentNigpCodes, searchNigpCode, true, true);
                }
            }
        }

        /// <summary>
        /// Bind first step data source
        /// </summary>
        private void BindFirstStepDataSource()
        {
            BindFirstStepDataSource(null, false, false);
        }

        /// <summary>
        /// Bind the first step data source.
        /// </summary>
        /// <param name="searchNigpCode">The search NIGP code.</param>
        /// <param name="isResetPageIndex">Indicate if reset page index.</param>
        /// <param name="isReloadData">Indicate if reload data.</param>
        private void BindFirstStepDataSource(string searchNigpCode, bool isResetPageIndex, bool isReloadData)
        {
            DataTable dtDataSource = null;

            if (isReloadData || FirstStepGridViewDataSource == null)
            {
                dtDataSource = GetFirstStepDataSource(searchNigpCode);

                // set the grid's sort for the first step by default.
                gvDrillDownList.GridViewSortExpression = "ID";
                gvDrillDownList.GridViewSortDirection = ACAConstant.ORDER_BY_ASC;
                DataView dataView = new DataView(dtDataSource);
                dataView.Sort = "ID ASC";
                dtDataSource = dataView.ToTable();
            }
            else
            {
                dtDataSource = FirstStepGridViewDataSource;
            }
 
            // set the data source and bind the grid view
            FirstStepGridViewDataSource = dtDataSource;
            BindGridDataSource(gvDrillDownList, dtDataSource, isResetPageIndex);
        }

        /// <summary>
        /// Bind the last step data source
        /// </summary>
        private void BindLastStepDataSource()
        {
            BindLastStepDataSource(null, null, false, false);
        }

        /// <summary>
        /// Bind the last step data source
        /// </summary>
        /// <param name="parentNigpCodes">The parent NIGP code list.</param>
        /// <param name="searchNigpCode">The search NIGP code.</param>
        /// <param name="isResetPageIndex">Indicate if reset page index.</param>
        /// <param name="isReloadData">Indicate if reload data.</param>
        private void BindLastStepDataSource(string[] parentNigpCodes, string searchNigpCode, bool isResetPageIndex, bool isReloadData)
        {
            DataTable dtDataSource = null;

            if (isReloadData || LastStepGridViewDataSource == null)
            {
                dtDataSource = GetLastStepDataSource(parentNigpCodes, searchNigpCode);

                // set the grid's sort foe last step by default.
                gvDrillDownList.GridViewSortExpression = "ID";
                gvDrillDownList.GridViewSortDirection = ACAConstant.ORDER_BY_ASC;
                DataView dataView = new DataView(dtDataSource);
                dataView.Sort = "ID ASC";
                dtDataSource = dataView.ToTable();
            }
            else
            {
                dtDataSource = LastStepGridViewDataSource;
            }

            // set the data source and bind the grid view
            LastStepGridViewDataSource = dtDataSource;
            BindGridDataSource(gvDrillDownList, dtDataSource, isResetPageIndex);
        }

        /// <summary>
        /// Bind the grid data source
        /// </summary>
        /// <param name="gridView">The AccelaGridView control.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="isResetPageIndex">Indicate if reset page index.</param>
        private void BindGridDataSource(AccelaGridView gridView, DataTable dataSource, bool isResetPageIndex)
        {
            gridView.DataSource = dataSource;
            gridView.EmptyDataText = GetTextByKey("per_permitList_messagel_noRecord");

            if (isResetPageIndex)
            {
                gridView.PageIndex = 0;
            }

            gridView.DataBind();
            gridView.Attributes.Add("PageCount", gvDrillDownList.PageCount.ToString());
        }

        /// <summary>
        /// Get the first step data source
        /// </summary>
        /// <param name="searchNigpCode">The search NIGP code.</param>
        /// <returns>Return the first step data source.</returns>
        private DataTable GetFirstStepDataSource(string searchNigpCode)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CERT_BUSINESS_NIGP_CLASS, false);

            DataTable dtDataSource = CreateDataTable();

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
                    // judge the NIGP type
                    if (!string.IsNullOrEmpty(NigpType) && NigpType != NIGP_TYPE_ALL)
                    {
                        if (item.Value == null || item.Value.ToString() != NigpType)
                        {
                            continue;
                        }
                    }

                    if (item.Key == null || item.Key.Trim().Length < 3)
                    {
                        continue;
                    }

                    // judge the search condition
                    if (string.IsNullOrEmpty(searchNigpCode) || (!string.IsNullOrEmpty(searchNigpCode) && item.Key.ToLower().Contains(searchNigpCode.ToLower())))
                    {
                        // add the datarow
                        DataRow dr = dtDataSource.NewRow();
                        dr["ID"] = item.Key.Trim().Substring(0, 3);
                        dr["ClassCode"] = item.Key.Trim();
                        dtDataSource.Rows.Add(dr);
                    }
                }
            }

            return dtDataSource;
        }

        /// <summary>
        /// Get the last step data source
        /// </summary>
        /// <param name="parentNigpCodes">The parent NIGP code list.</param>
        /// <param name="searchNigpCode">The search NIGP code.</param>
        /// <returns>Return the last step data source</returns>
        private DataTable GetLastStepDataSource(string[] parentNigpCodes, string searchNigpCode)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CERT_BUSINESS_NIGP_SUBCLASS, false);

            DataTable dtDataSource = CreateDataTable();

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (string parentNigpCode in parentNigpCodes)
                {
                    if (string.IsNullOrEmpty(parentNigpCode))
                    {
                        continue;
                    }

                    foreach (ItemValue item in stdItems)
                    {
                        // judge the NIGP type
                        if (!string.IsNullOrEmpty(NigpType) && NigpType != NIGP_TYPE_ALL)
                        {
                            if (item.Value == null || item.Value.ToString() != NigpType)
                            {
                                continue;
                            }
                        }

                        if (item.Key == null || item.Key.StartsWith(parentNigpCode) == false)
                        {
                            continue;
                        }

                        // judge the search condition
                        if (string.IsNullOrEmpty(searchNigpCode) || (!string.IsNullOrEmpty(searchNigpCode) && item.Key.ToLower().Contains(searchNigpCode.ToLower())))
                        {
                            // add the datarow
                            DataRow dr = dtDataSource.NewRow();
                            dr["ID"] = item.Key.Trim();
                            dr["ClassCode"] = item.Key.Trim();
                            dtDataSource.Rows.Add(dr);
                        }
                    }
                }
            }

            return dtDataSource;
        }

        /// <summary>
        /// Set the first step's status.
        /// </summary>
        private void SetFirstStepStatus()
        {
            tdNext.Visible = true;
            tdBack.Visible = false;
            tdFinish.Visible = false;
            tdFinishSpace.Visible = false;

            StepType = STEP_TYPE_FIRST;
            SetPageTitleKey("aca_certbusiness_label_drilldown_nigpclass_title");
        }

        /// <summary>
        /// Set the last step's status.
        /// </summary>
        private void SetLastStepStatus()
        {
            tdNext.Visible = false;
            tdBack.Visible = true;
            tdFinish.Visible = true;
            tdFinishSpace.Visible = true;

            StepType = STEP_TYPE_LAST;
            SetPageTitleKey("aca_certbusiness_label_drilldown_nigpsubclass_title");
        }

        /// <summary>
        /// Create the DataTable.
        /// </summary>
        /// <returns>Return the DataTable created.</returns>
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("ClassCode", typeof(string));

            return dt;
        }

        #endregion Method
    }
}
