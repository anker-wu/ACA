#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefExaminationScheduleConfirm.ascx.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Examination Schedule Confirm
    /// </summary>
    public partial class RefExaminationScheduleConfirm : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Displays the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="feeItems">The fee items.</param>
        public void Display(ExaminationParameter model, RefFeeDsecVO[] feeItems)
        {
            Visible = true;

            lblExaminationValue.Text = model.ExaminationName;
            lblProviderValue.Text = model.ExamScheduleProviderName;
            lblLanguageValue.Text = model.ExamScheduleLang;
            lblLocationValue.Text = model.ExamScheduleLocation;
            lblAvailableSiteValue.Text = model.ExamScheduleSeats == "0" ? string.Empty : model.ExamScheduleSeats;

            string accessibility = string.Empty;
            imgAccessiblity.Visible = false;

            if (!string.IsNullOrEmpty(model.ExamScheduleHandlecap))
            {
                if (ValidationUtil.IsYes(model.ExamScheduleHandlecap))
                {
                    imgAccessiblity.Src = ImageUtil.GetImageURL("accessibility-directory.png");
                    imgAccessiblity.Attributes.Add("title", LabelUtil.GetGlobalTextByKey("aca_common_label_accessibility"));
                    imgAccessiblity.Attributes.Add("alt", LabelUtil.GetGlobalTextByKey("aca_common_label_accessibility"));
                    imgAccessiblity.Visible = true;
                }

                accessibility = ModelUIFormat.FormatYNLabel(model.ExamScheduleHandlecap);
            }

            lblAccessibilityValue.Text = accessibility;

            string timeString = model.ExamScheduleDate + " " + model.ExamScheduleWeekDay + " " +
                                model.ExamScheduleStartTime + " ~ " + model.ExamScheduleEndTime;

            if (timeString.Trim().Length > 1)
            {
                lblTimeValue.Text = timeString;
            }

            lblAccessbilityDescValue.Text = model.AccessiblityDesc;

            lblExamInstructionsValue.Text = model.ExamInstructions;

            lblDrivingDescValue.Text = model.DrivingDesc;

            // Does not display fee items in reschedule process.
            if (!string.IsNullOrEmpty(model.IsReschedule) && bool.Parse(model.IsReschedule))
            {
                ExaminationFeeItemTemplate.Visible = false;
            }
            else
            {
                ExaminationFeeItemTemplate.BindDataSource(feeItems, model.ExamScheduleTotalFee);   
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion
    }
}