#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefExaminationExtenalOnlineScheduleConfirm.ascx.cs
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
using Accela.ACA.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Examination External Online Schedule Confirm
    /// </summary>
    public partial class RefExaminationExtenalOnlineScheduleConfirm : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Displays the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="feeItems">The fee items.</param>
        /// <param name="userModel">The user model.</param>
        public void Display(ExaminationParameter model, RefFeeDsecVO[] feeItems, PublicUserModel4WS userModel)
        {
            this.Visible = true;

            lblExaminationValue.Text = model.ExaminationName;
            lblProviderValue.Text = model.ExamScheduleProviderName;
            lblLanguageValue.Text = model.ExamScheduleLang;

            // Does not display fee items in reschedule process.
            if (!string.IsNullOrEmpty(model.IsReschedule) && bool.Parse(model.IsReschedule))
            {
                ExaminationFeeItemTemplate.Visible = false;
            }
            else
            {
                ExaminationFeeItemTemplate.BindDataSource(feeItems, model.ExamScheduleTotalFee);
            }

            lblExamInstructionsValue.Text = model.ExamInstructions;

            if (userModel != null)
            {
                string text = string.Empty;

                if (!string.IsNullOrEmpty(model.RecordContactFirstName))
                {
                    text += model.RecordContactFirstName + " ";
                }

                if (!string.IsNullOrEmpty(model.RecordContactMiddleName))
                {
                    text += model.RecordContactMiddleName + " ";
                }

                if (!string.IsNullOrEmpty(model.RecordContactLastName))
                {
                    text += model.RecordContactLastName;
                }

                if (!string.IsNullOrEmpty(text))
                {
                    text += ACAConstant.HTML_BR;
                }

                lblUserAccountValue.IsNeedEncode = false;
                lblUserAccountValue.Text = text + userModel.email;
            }
            else
            {
                divUserAccount.Visible = false;
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