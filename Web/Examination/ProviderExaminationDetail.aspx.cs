#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ProviderExaminationDetail.cs
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
using System.Web.UI;

using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Provider examination detail page.
    /// </summary>
    public partial class ProviderExaminationDetail : PopupDialogBasePage
    {
        /// <summary>
        /// Initial page load
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_exam_provider_examination_title");
            this.SetDialogMaxHeight("600");

            if (!Page.IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    ExaminationParameter parameter = ExaminationParameterUtil.BuildModelFromURL();

                    ExamScheduleViewModel examScheduleViewModel = new ExamScheduleViewModel();

                    if (!string.IsNullOrEmpty(parameter.ExamScheduleScheduleId))
                    {
                        examScheduleViewModel.scheduleID = long.Parse(parameter.ExamScheduleScheduleId);
                    }

                    if (!string.IsNullOrEmpty(parameter.ExamScheduleLocationId))
                    {
                        examScheduleViewModel.locationID = long.Parse(parameter.ExamScheduleLocationId);
                    }

                    if (!string.IsNullOrEmpty(parameter.ExamScheduleCalendarId))
                    {
                        examScheduleViewModel.calendarID = long.Parse(parameter.ExamScheduleCalendarId);
                    }

                    if (!string.IsNullOrEmpty(parameter.ExamScheduleProviderNbr))
                    {
                        examScheduleViewModel.providerNbr = long.Parse(parameter.ExamScheduleProviderNbr);
                    }

                    examScheduleViewModel.availableSeats = int.Parse(parameter.ExamScheduleSeats);
                    examScheduleViewModel.examName = parameter.ExaminationName;

                    if (!string.IsNullOrEmpty(parameter.ExamScheduleStartTime))
                    {   
                        DateTime dtStartTime;

                        if (I18nDateTimeUtil.TryParseFromUI(parameter.ExamScheduleStartTime, out dtStartTime))
                        {
                            examScheduleViewModel.startTime = dtStartTime;
                        }
                    }

                    if (!string.IsNullOrEmpty(parameter.ExamScheduleEndTime))
                    {
                        DateTime dtEndTime;

                        if (I18nDateTimeUtil.TryParseFromUI(parameter.ExamScheduleEndTime, out dtEndTime))
                        {
                            examScheduleViewModel.endTime = dtEndTime;
                        }
                    }

                    if (!string.IsNullOrEmpty(parameter.ExamScheduleDate))
                    {
                        DateTime dtDate;

                        if (I18nDateTimeUtil.TryParseFromUI(parameter.ExamScheduleDate, out dtDate))
                        {
                            examScheduleViewModel.date = dtDate;
                        }
                    }

                    examScheduleViewModel.weekday = parameter.ExamScheduleWeekDay;
                    examScheduleViewModel.serviceProviderCode = ACAConstant.AgencyCode;

                    if (!string.IsNullOrEmpty(parameter.ExaminationNbr))
                    {
                        examScheduleViewModel.refExamNbr = long.Parse(parameter.ExaminationNbr);
                    }

                    IExaminationBll refExamBll = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));
                    ExamScheduleViewModel examScheduleViewModels = refExamBll.GetExamScheduleViewModel(examScheduleViewModel);
                    RefFeeDsecVO[] feeItems = GetFeeItemString(parameter.ExaminationNbr, parameter.ExamScheduleProviderNbr);
                    refProviderExaminationDetail.Display(examScheduleViewModels, feeItems);
                }
            }
        }

        /// <summary>
        /// Gets the fee item string.
        /// </summary>
        /// <param name="examinationNbr">The examination number.</param>
        /// <param name="providerNbr">The provider number.</param>
        /// <returns>The fee item.</returns>
        private RefFeeDsecVO[] GetFeeItemString(string examinationNbr, string providerNbr)
        {
            IProviderBll providerBll = (IProviderBll)ObjectFactory.GetObject(typeof(IProviderBll));
            RefFeeDsecVO[] refFeeDsecVo = providerBll.GetProviderFeeItems(ACAConstant.AgencyCode, examinationNbr, "EXAM", providerNbr);
            return refFeeDsecVo;
        }
    }
}