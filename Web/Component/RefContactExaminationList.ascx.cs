#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContactExaminationList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefContactExaminationList.ascx.cs 238264 2013-10-7 09:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Display the examination list which look up by the ref contact.
    /// </summary>
    public partial class RefContactExaminationList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets Examination data source.
        /// </summary>
        public IList<ExaminationModel> GridViewDataSource
        {
            get
            {
                if (ViewState["ExaminationList"] == null)
                {
                    ViewState["ExaminationList"] = new List<ExaminationModel>();
                }

                return ViewState["ExaminationList"] as IList<ExaminationModel>;
            }

            set
            {
                ViewState["ExaminationList"] = value;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Get selected examinations.
        /// </summary>
        /// <returns>Selected Examination</returns>
        public IList<ExaminationModel> GetSelectedExamination()
        {
            IList<ExaminationModel> selectedExamination = new List<ExaminationModel>();

            if (!string.IsNullOrWhiteSpace(hdnRequiredFlags.Value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Dictionary<string, string> dicRequiredMapping = javaScriptSerializer.Deserialize<Dictionary<string, string>>(hdnRequiredFlags.Value);
                IEnumerable<KeyValuePair<string, string>> dicSelectedMapping = dicRequiredMapping.Where(f => ValidationUtil.IsYes(f.Value));

                foreach (var keyValuePair in dicSelectedMapping)
                {
                    selectedExamination.Add(GridViewDataSource.FirstOrDefault(o => o.examinationPKModel.examNbr == long.Parse(keyValuePair.Key)));   
                }
            }

            return selectedExamination;
        }

        /// <summary>
        /// Bind examination with data source.
        /// </summary>
        public void BindExaminationList()
        {
            gdvExaminationList.DataSource = GridViewDataSource;
            SetRequiredMappingData();

            //Display the required examinations in the front. 
            gdvExaminationList.Sort("requiredFlag", SortDirection.Ascending);
            gdvExaminationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvExaminationList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                GridViewBuildHelper.SetHiddenColumn(gdvExaminationList, new[] { "Action" });
            }

            GridViewBuildHelper.SetSimpleViewElements(gdvExaminationList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Examination row bind event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event arguments.</param>
        protected void ExaminationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ExaminationModel exam = e.Row.DataItem as ExaminationModel;

                if (exam == null)
                {
                    return;
                }

                HiddenField hfExamNbr = (HiddenField)e.Row.FindControl("hfExamNbr");
                AccelaLabel lblRequired = (AccelaLabel)e.Row.FindControl("lblRequired");
                AccelaLabel lblApproved = (AccelaLabel)e.Row.FindControl("lblApproved");
                hfExamNbr.Value = exam.examinationPKModel.examNbr.ToString();
                lblRequired.Text = ModelUIFormat.FormatYNLabel(exam.requiredFlag, true);
                lblApproved.Text = ModelUIFormat.FormatYNLabel(exam.approvedFlag, true);

                //format examinationdate
                AccelaLabel lblExaminationDate = (AccelaLabel)e.Row.FindControl("lblExaminationDate");

                if (lblExaminationDate != null && exam.examDate != null)
                {
                    lblExaminationDate.Text = I18nDateTimeUtil.FormatToDateStringForUI(exam.examDate.Value);
                }

                //format examination start time
                AccelaLabel lblExaminationStartTime = (AccelaLabel)e.Row.FindControl("lblExaminationStartTime");
                
                if (lblExaminationStartTime != null && exam.startTime != null)
                {
                    lblExaminationStartTime.Text = I18nDateTimeUtil.FormatToTimeStringForUI(exam.startTime.Value, false);
                }

                //format examination end time
                AccelaLabel lblExaminationEndTime = (AccelaLabel)e.Row.FindControl("lblExaminationEndTime");
                
                if (lblExaminationEndTime != null && exam.endTime != null)
                {
                    lblExaminationEndTime.Text = I18nDateTimeUtil.FormatToTimeStringForUI(exam.endTime.Value, false);
                }

                //format display final socre. 
                AccelaLabel lblFinalScore = (AccelaLabel)e.Row.FindControl("lblFinalScore");

                if (lblFinalScore != null)
                {
                    lblFinalScore.Text = EducationUtil.FormatScore(exam.gradingStyle, exam.finalScore == null ? string.Empty : I18nNumberUtil.FormatNumberForWebService(exam.finalScore.Value));
                }

                //format display passing score.
                AccelaLabel lblPassingScore = (AccelaLabel)e.Row.FindControl("lblPassingScore");

                if (lblPassingScore != null)
                {
                    lblPassingScore.Text = EducationUtil.FormatScore(exam.gradingStyle, lblPassingScore.Text, true);
                }

                //format display passing score.
                AccelaLabel lblExamStatus = (AccelaLabel)e.Row.FindControl("lblExamStatus");

                if (lblExamStatus != null)
                {
                    lblExamStatus.Text = ExaminationUtil.GetExamStatusLabel(exam.examStatus, this.ModuleName);
                }

                AccelaDiv imgExpand = (AccelaDiv)e.Row.FindControl("divLogo");

                //Display or Hidden Common Column/the comment cell container
                if (string.IsNullOrEmpty(exam.comments))
                {
                    if (imgExpand != null)
                    {
                        imgExpand.Visible = false;
                    }

                    var lblCommonsValue = e.Row.FindControl("lblCommonsValue");
                    var dataCell4Comment = lblCommonsValue == null ? null : lblCommonsValue.Parent as DataControlFieldCell;

                    //if Expand icon is invisible, set the cell container to invisible also for section 508
                    if (dataCell4Comment != null && imgExpand != null && imgExpand.Visible == false)
                    {
                        dataCell4Comment.Visible = false;
                    }
                }

                //set Expand column visibile for section 508
                AccelaGridView currentGridView = sender as AccelaGridView;
                var dataCell4ExpandIcon = imgExpand == null ? null : imgExpand.Parent as DataControlFieldCell;

                if (currentGridView != null && dataCell4ExpandIcon != null && e.Row.RowIndex >= 0 && AccessibilityUtil.AccessibilityEnabled)
                {
                    if (e.Row.RowIndex == 0)
                    {
                        dataCell4ExpandIcon.ContainingField.Visible = false;
                    }

                    if (e.Row.RowIndex >= 0 && imgExpand.Visible == true)
                    {
                        dataCell4ExpandIcon.ContainingField.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Set the required mapping data to hidden field.
        /// </summary>
        private void SetRequiredMappingData()
        {
            IDictionary<string, string> requiredMapping = new Dictionary<string, string>();

            foreach (ExaminationModel exam in GridViewDataSource)
            {
                requiredMapping.Add(Convert.ToString(exam.examinationPKModel.examNbr), exam.requiredFlag);
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            hdnRequiredFlags.Value = javaScriptSerializer.Serialize(requiredMapping);
        }

        #endregion Method
    }
}