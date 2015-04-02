#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CertifiedExperienceList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CertifiedExperienceList.ascx.cs 190488 2011-02-17 10:00:36Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Class for certified experience list
    /// </summary>
    public partial class CertifiedExperienceList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Certified Business Field Name.
        /// </summary>
        private const string CERTIFIED_BUSINESS_FILENAME = "ExperienceList";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets experience list data source.
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                if (ViewState["GridViewDataSource"] == null)
                {
                    return null;
                }

                return (DataTable)ViewState["GridViewDataSource"];
            }

            set
            {
                ViewState["GridViewDataSource"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind experience list.
        /// </summary>
        /// <param name="dt">Certified experience DataTable.</param>
        public void BindExperienceList(DataTable dt)
        {
            DataTable dataSource = null;
            if (dt == null)
            {
                dataSource = GridViewDataSource;
            }
            else
            {
                dataSource = dt;
                GridViewDataSource = dt;
            }

            gdvCertExperienceList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_certbusiness_msg_experiencelist_norecord");
            gdvCertExperienceList.DataSource = dataSource;
            gdvCertExperienceList.DataBind();
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack || AppSession.IsAdmin)
            {
                BindExperienceList(null);
            }
        }

        /// <summary>
        /// Rewrite <c>OnInit</c> method to initialize component.
        /// </summary>
        /// <param name="e">A System.EventArgs Object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            // set the simple view element before the GridViewNumber is setted.
            GridViewBuildHelper.SetSimpleViewElements(gdvCertExperienceList, ModuleName, AppSession.IsAdmin);

            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvCertExperienceList.ShowExportLink = true;
                gdvCertExperienceList.ExportFileName = CERTIFIED_BUSINESS_FILENAME;
            }
            else
            {
                gdvCertExperienceList.ShowExportLink = false;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// GridView CertExperienceList row data bound event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void CertExperienceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AccelaLabel lblJobValue = e.Row.FindControl("lblJobValue") as AccelaLabel; 
            AccelaLabel lblWorkDate = e.Row.FindControl("lblWorkDate") as AccelaLabel;
            DataRowView dataRowView = e.Row.DataItem as DataRowView;

            if (dataRowView != null)
            {
                decimal jobValue = (decimal)dataRowView["JobValue"];
                lblJobValue.Text = jobValue == 0 ? string.Empty : I18nNumberUtil.FormatMoneyForUI(jobValue);

                DateTime workDate = (DateTime)dataRowView["WorkDate"];
                lblWorkDate.Text = workDate == DateTime.MinValue ? string.Empty : I18nDateTimeUtil.FormatToDateStringForUI(workDate);
            }
        }

        #endregion Methods
    }
}
