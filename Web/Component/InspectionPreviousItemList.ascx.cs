#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionPreviousItemList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Provide the inspection previous item list.
 *
 *  Notes:
 *      $Id: InspectionPreviousItemList.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Inspection;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Inspection previous item list
    /// </summary>
    public partial class InspectionPreviousItemList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(InspectionPreviousItemList));

        #endregion

        /// <summary>
        /// Displays the previous inspections.
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        public void DisplayPreviousInspections(List<InspectionViewModel> viewModels)
        {
            gdvPreviousInspectionList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("ins_inspectionList_message_noRecord");
            gdvPreviousInspectionList.DataSource = viewModels;
            gdvPreviousInspectionList.DataBind();
        }

        /// <summary>
        /// Gets the help text URL.
        /// </summary>
        /// <param name="titleLabelKey">The title label key.</param>
        /// <param name="contentLabelKey">The content label key.</param>
        /// <param name="additionalTextID">The additional text ID.</param>
        /// <returns>the help text URL.</returns>
        protected string GetHelpTextURL(string titleLabelKey, string contentLabelKey, string additionalTextID)
        {
            string urlPattern = "../Common/PopupHelp.aspx?titleLabelKey={0}&contentLabelKey={1}&additionalTextID={2}";
            string url = string.Format(urlPattern, titleLabelKey, contentLabelKey, additionalTextID);
            return url;
        }

        /// <summary>
        /// Rewrite <c>OnInit</c> method to initialize component.
        /// </summary>
        /// <param name="e">A System.EventArgs Object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvPreviousInspectionList, ModuleName, AppSession.IsAdmin);
            DialogUtil.RegisterScriptForDialog(Page);
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DisplayPageForAdmin();
            }
            catch (Exception ex)
            {
                HandleExecption(ex);
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the GridView PreviousInspectionList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void PreviousInspectionList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                var isDataRow = e.Row.RowType == DataControlRowType.DataRow;
                var viewModel = isDataRow ? e.Row.DataItem as InspectionViewModel : null;

                if (viewModel != null)
                {
                    var lblInspectionResult = e.Row.FindControl("lblInspectionResult") as AccelaLabel;
                    var lblMajorViolations = isDataRow ? e.Row.FindControl("lblMajorViolations") as AccelaLabel : null;
                    var lblScheduledDateTime = e.Row.FindControl("lblScheduledDateTime") as AccelaLabel;
                    var lblInspectionTypeText = e.Row.FindControl("lblInspectionTypeText") as AccelaLabel;
                    var lblScheduledDate = e.Row.FindControl("lblScheduledDate") as AccelaLabel;
                    var lblScheduledTime = e.Row.FindControl("lblScheduledTime") as AccelaLabel;
                    var lblInspectionResultComment = e.Row.FindControl("lblInspectionResultComment") as AccelaLabel;

                    if (lblInspectionTypeText != null)
                    {
                        string lnkInspectionTypeTextID = lblInspectionTypeText.ClientID.Replace("lbl", "lnk");
                        lblInspectionTypeText.Text = string.Format(
                                                                   "<a id='{0}' href='javascript:void(0);' onclick=\"ShowInspectionTypeHelpText('{0}');return false;\" class='NotShowLoading'>{1}</a>",
                                                                   lnkInspectionTypeTextID,
                                                                   viewModel.TypeText);
                    }

                    if (lblInspectionResult != null)
                        {
                            if (!string.IsNullOrEmpty(viewModel.Result))
                            {
                                string linkPattern4Result = "<a id='" + new Random().Next() + "' href='javascript:void(0);' onclick=\"ShowInspectionResultHelpText(this.id);return false;\" class='NotShowLoading'>{0}</a>";
                                string resultText = string.Format(linkPattern4Result, viewModel.Result);
                                lblInspectionResult.Text = resultText;
                            }
                        }

                        if (lblMajorViolations != null)
                        {
                            string linkPattern4MajorViolations = "<span id='{0}' class='ACA_Hide'>{1}</span><a id='{3}' href='javascript:void(0);' onclick=\"ShowInspectionMajorViolationHelpText('{0}',this.id);return false;\" class='NotShowLoading'>{2}</a>";
                            lblMajorViolations.Text = InspectionViewUtil.GetMajorViolationsText(viewModel, lblMajorViolations.ClientID, linkPattern4MajorViolations);
                        }

                        var isPendingStatus = viewModel.InspectionDataModel.Status == InspectionStatus.PendingByACA
                                              || viewModel.InspectionDataModel.Status == InspectionStatus.PendingByV360;

                        if (lblScheduledDateTime != null)
                        {
                            lblScheduledDateTime.Text = isPendingStatus ? viewModel.RequestedDateTimeText : viewModel.ScheduledDateTimeText;
                        }

                        if (lblScheduledDate != null)
                        {
                            lblScheduledDate.Text = isPendingStatus ? viewModel.RequestedDateText : viewModel.ScheduledDateText;
                        }

                        if (lblScheduledTime != null)
                        {
                            lblScheduledTime.Text = isPendingStatus ? viewModel.RequestedTimeText : viewModel.ScheduledTimeText;
                        }

                        if (lblInspectionResultComment != null)
                        {
                            lblInspectionResultComment.Text = viewModel.ResultComments;
                        }
                    }
            }
            catch (Exception ex)
            {
                HandleExecption(ex);
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The exception object.</param>
        private void HandleExecption(Exception ex)
        {
            Logger.Error(ex);
            MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
        }

        /// <summary>
        /// Display a empty page in ACA admin side.
        /// </summary>
        private void DisplayPageForAdmin()
        {
            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    lblAvailableHelpTexts.Attributes["class"] = string.Empty;
                    DisplayPreviousInspections(null);
                }
            }
        }
    }
}