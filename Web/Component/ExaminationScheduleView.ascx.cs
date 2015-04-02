#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationScheduleView.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */
#endregion Header

using System;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Examination Detail Component
    /// </summary>
    public partial class ExaminationScheduleView : BaseUserControl
    {
        /// <summary>
        /// Displays the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="examinationModel">The examination model.</param>
        public void Display(ExamScheduleViewModel model, ExaminationModel examinationModel)
        {
            this.Visible = true;

            if (examinationModel.finalScore != null)
            {
                imgPassingIcon.Visible = true;

                if (ExaminationUtil.IsPassedExamination(examinationModel.gradingStyle, examinationModel.finalScore, examinationModel.passingScore))
                {
                    imgPassingIcon.Src = ImageUtil.GetImageURL("icon_exam_score_passing.png");
                    imgPassingIcon.Attributes.Add("title", LabelUtil.GetGlobalTextByKey("aca_common_pass"));
                    imgPassingIcon.Attributes.Add("alt", LabelUtil.GetGlobalTextByKey("aca_common_pass"));
                }
                else
                {
                    imgPassingIcon.Src = ImageUtil.GetImageURL("icon_exam_score_fail.png");
                    imgPassingIcon.Attributes.Add("title", LabelUtil.GetGlobalTextByKey("aca_common_fail"));
                    imgPassingIcon.Attributes.Add("alt", LabelUtil.GetGlobalTextByKey("aca_common_fail"));
                }
            }

            lblScoreValue.Text = EducationUtil.FormatScore(examinationModel.gradingStyle, I18nNumberUtil.ConvertNumberToInvariantString(examinationModel.finalScore));

            if (GradingStyle.GradingStyleType.percentage.ToString().Equals(examinationModel.gradingStyle, StringComparison.OrdinalIgnoreCase)
                || GradingStyle.GradingStyleType.score.ToString().Equals(examinationModel.gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                lblPassingScoreValue.Text = EducationUtil.FormatScore(examinationModel.gradingStyle, I18nNumberUtil.ConvertNumberToInvariantString(examinationModel.passingScore), true);
            }
            else if (GradingStyle.GradingStyleType.passfail.ToString().Equals(examinationModel.gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                lblPassingScoreValue.Text = LabelUtil.GetGlobalTextByKey("aca_common_pass");
            }
            else if (GradingStyle.GradingStyleType.none.ToString().Equals(examinationModel.gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                lblPassingScoreValue.Text = string.Empty;
            }

            lblRosterIdValue.Text = examinationModel.userExamID;
            lblExaminationValue.Text = model.examName + "(" + ExaminationUtil.GetRequiredOrOptional(examinationModel.requiredFlag, ModuleName) + ")";
            lblProviderValue.Text = model.providerName;
            lblLanguageValue.Text = model.supportLang;
            string undefined = LabelUtil.GetTextByKey("aca_examination_examlist_field_undefined", ModuleName);
            string examWeekDay = examinationModel.examDate == null ? string.Empty : I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.DayNames[(int)examinationModel.examDate.Value.DayOfWeek];
            string examDate = examinationModel.examDate == null ? undefined : I18nDateTimeUtil.FormatToDateStringForUI(examinationModel.examDate.Value) + " " + examWeekDay;
            string startTime = examinationModel.startTime == null ? string.Empty : I18nDateTimeUtil.FormatToTimeStringForUI(examinationModel.startTime.Value, false);
            string endTime = examinationModel.endTime == null ? string.Empty : " ~ " + I18nDateTimeUtil.FormatToTimeStringForUI(examinationModel.endTime.Value, false);
            string timeString = examDate + " " + startTime + endTime;

            if (timeString.Length > 1)
            {
                lblTimeValue.Text = timeString;
            }

            lblExamInstructionsValue.Text = examinationModel.comments;

            if (examinationModel.examProviderDetailModel != null)
            {
                //build the location model for display.
                RProviderLocationModel exminationProviderDetail = new RProviderLocationModel()
                {
                    address1 = examinationModel.examProviderDetailModel.address1,
                    address2 = examinationModel.examProviderDetailModel.address2,
                    address3 = examinationModel.examProviderDetailModel.address3,
                    city = examinationModel.examProviderDetailModel.city,
                    zip = examinationModel.examProviderDetailModel.zip,
                    state = examinationModel.examProviderDetailModel.state,
                    countryCode = examinationModel.examProviderDetailModel.countryCode
                };

                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                string site = addressBuilderBll.BuildAddress4ProviderLocation(exminationProviderDetail);
                lblLocationValue.Text = site;

                lblAccessibilityValue.Text = ModelUIFormat.FormatYNLabel(examinationModel.examProviderDetailModel.isHandicapAccessible);
                lblAccessbilityDescValue.Text = examinationModel.examProviderDetailModel.handicapAccessibleDes;
                lblDrivingDescValue.Text = examinationModel.examProviderDetailModel.drivingDirections;

                imgAccessiblity.Visible = false;

                if (ValidationUtil.IsYes(examinationModel.examProviderDetailModel.isHandicapAccessible))
                {
                    imgAccessiblity.Src = ImageUtil.GetImageURL("accessibility-directory.png");
                    imgAccessiblity.Attributes.Add("title", LabelUtil.GetGlobalTextByKey("aca_common_label_accessibility"));
                    imgAccessiblity.Attributes.Add("alt", LabelUtil.GetGlobalTextByKey("aca_common_label_accessibility"));
                    imgAccessiblity.Visible = true;
                }
            }

            genericTemplate.Display(examinationModel.template);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    imgAccessiblity.Visible = false;
                }
            }
        }
    }
}