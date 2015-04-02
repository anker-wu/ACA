#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationReasonList.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamScheduleViewModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Data;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Examination Reason List
    /// </summary>
    public partial class ExaminationReasonList : BaseUserControl
    {
        #region Events

        /// <summary>
        /// grid view page index changing event
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets cap contact data table.
        /// </summary>
        /// <value>The data source.</value>
        public DataTable DataSource
        {
            get
            {
                if (ViewState["ExaminationReasonListDataSource"] == null)
                {
                    ViewState["ExaminationReasonListDataSource"] = new DataTable();
                }

                return (DataTable)ViewState["ExaminationReasonListDataSource"];
            }

            set
            {
                ViewState["ExaminationReasonListDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets the hidden field value.
        /// </summary>
        /// <value>The hidden field value.</value>
        public string HfSelectedReasonValue
        {
            get { return hfSelectedReason.Value; }
        }

        #endregion

        #region Method

        /// <summary>
        /// Gets the selected reason.
        /// </summary>
        /// <returns>select reason</returns>
        public string GetSelectedReason()
        {
            string reason = string.Empty;

            foreach (DataRow dataRow in DataSource.Rows)
            {
                if (dataRow["ID"].ToString() == hfSelectedReason.Value)
                {
                    reason = dataRow["ReasonString"].ToString();
                }
            }

            return reason;
        }

        /// <summary>
        /// Loads the reason.
        /// </summary>
        /// <param name="selectid">The selected id.</param>
        /// <param name="selectedRowIndex">Index of the selected row.</param>
        public void BindReason(string selectid, int selectedRowIndex)
        {
            int pageIndex = 0;
            if (selectedRowIndex > 0)
            {
                pageIndex = selectedRowIndex / gvReasonExamination.PageSize;
            }

            BizDomainModel4WS[] reasonLists = GetReasonLists();

            DataSource = ExaminationScheduleUtil.ConvertReasonListToDataTable(reasonLists, selectid);

            if (!string.IsNullOrEmpty(selectid) && hfSelectedReason.Value != selectid)
            {
                hfSelectedReason.Value = selectid;
            }

            gvReasonExamination.DataSource = DataSource;
            gvReasonExamination.PageIndex = pageIndex;
            gvReasonExamination.DataBind();
        }

        /// <summary>
        /// Handles the RowDataBound event of the GridView ReasonExamination control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void ReasonExamination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaRadioButton accelaRadioButton = e.Row.Cells[0].FindControl("rdReasonExamination") as AccelaRadioButton;

                if (accelaRadioButton != null)
                {
                    accelaRadioButton.Attributes.Add("onclick", "SelectExaminationRadio(this,'" + gvReasonExamination.ClientID + "','" + hfSelectedReason.ClientID + "');SetButtonStatus();");
                }
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the GridView ReasonExamination control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ReasonExamination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int selectedRowIndex = e.NewPageIndex * gvReasonExamination.PageSize;

            BindReason(hfSelectedReason.Value, selectedRowIndex);

            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// Gets the reason lists.
        /// </summary>
        /// <returns>standard choice list</returns>
        private BizDomainModel4WS[] GetReasonLists()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            return bizBll.GetBizDomainValue(
                                            CapUtil.GetAgencyCode(ModuleName),
                                            BizDomainConstant.BIZDOMAIN_RESCHED_CANCEL_REASON,
                                            new QueryFormat4WS(), 
                                            false, 
                                            I18nCultureUtil.UserPreferredCulture);
        }

        #endregion
    }
}