#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefProviderExaminationDetail.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
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
    /// RefProviderExaminationDetail Control.
    /// </summary>
    public partial class RefProviderExaminationDetail : BaseUserControl
    {
        /// <summary>
        /// Displays the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="feeItems">The fee items.</param>
        public void Display(ExamScheduleViewModel model, RefFeeDsecVO[] feeItems)
        {
            this.Visible = true;

            lblExaminationValue.Text = model.examName;
            lblProviderValue.Text = model.providerName;
            string sWeek = model.weekday;
            string sSTime = model.startTime == null ? string.Empty : I18nDateTimeUtil.FormatToTimeStringForUI(model.startTime.Value, false);
            string sETime = model.endTime == null ? string.Empty : I18nDateTimeUtil.FormatToTimeStringForUI(model.endTime.Value, false);
            string examDate = model.date == null ? string.Empty : I18nDateTimeUtil.FormatToDateStringForUI(model.date.Value);
            examDate = string.IsNullOrEmpty(examDate) ? examDate : examDate + " ";
            sWeek = string.IsNullOrEmpty(sWeek) ? sWeek : sWeek + " ";
            sETime = string.IsNullOrEmpty(sSTime) || string.IsNullOrEmpty(sETime) ? sETime : (" - " + sETime);
            lblTimeValue.Text = examDate + sWeek + sSTime + sETime;
            lblLanguageValue.Text = model.supportLang;
            lblAvailableSiteValue.Text = model.availableSeats == 0 ? string.Empty : model.availableSeats.ToString();
            lblExamInstructionsValue.Text = model.instructions;

            RProviderLocationModel lc = model.locationModel;

            if (lc != null)
            {
                lblAccessbilityDescValue.Text = lc.handicapAccessible;
                lblDrivingDescValue.Text = lc.drivingDirections;
                string accessibility = string.Empty;
                accessibility = ModelUIFormat.FormatYNLabel(lc.isHandicapAccessible);
                lblAccessibilityValue.Text = accessibility;
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                string site = addressBuilderBll.BuildAddress4ProviderLocation(lc);
                lblLocationValue.Text = site;
            }

            imgAccessiblity.Visible = false;

            if (lc != null && ValidationUtil.IsYes(lc.isHandicapAccessible))
            {
                imgAccessiblity.Src = ImageUtil.GetImageURL("accessibility-directory.png");
                imgAccessiblity.Attributes.Add("title", LabelUtil.GetGlobalTextByKey("aca_common_label_accessibility"));
                imgAccessiblity.Attributes.Add("alt", LabelUtil.GetGlobalTextByKey("aca_common_label_accessibility"));
                imgAccessiblity.Visible = true;
            }

            ExaminationFeeItemTemplate.BindDataSource(feeItems, model.totalFeeAmount);
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