#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExaminationDetail.aspx.cs 140040 2009-07-21 06:06:55Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web.Services;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// Page for ExaminationDetail detail.
    /// </summary>
    public partial class ExaminationDetail : BasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the search entity.
        /// </summary>
        /// <value>The search entity.</value>
        private ExamScheduleSearchModel SearchEntity
        {
            get { return ViewState["ExamScheduleSearch"] as ExamScheduleSearchModel; }

            set { ViewState["ExamScheduleSearch"] = value; }
        }

        /// <summary>
        /// Gets or sets examination number.
        /// </summary>
        private string ExaminationNbr
        {
            get { return ViewState["ExaminationNbr"].ToString(); }

            set { ViewState["ExaminationNbr"] = value; }
        } 

        #endregion

        #region Methods

        /// <summary>
        /// Validates the schedule date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="module">The module.</param>
        /// <returns>The error message when validate schedule date failed.</returns>
        [WebMethod(Description = "ValidateScheduleDate", EnableSession = true)]
        public static string ValidateScheduleDate(string startDate, string endDate, string module)
        {
            return ExaminationScheduleUtil.ValidateScheduleDate(startDate, endDate, module);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    refProviderList.DataSource = new DataTable();
                    refProviderList.BindProviderList(0, string.Empty);
                    RefAvailableExaminationScheduleSearchForm.BindDDL(ACAConstant.AgencyCode, string.Empty);
                    RefAvailableExaminationScheduleList.DataSource = new DataTable();
                    RefAvailableExaminationScheduleList.BindAvailableExaminationScheduleList(true, false, true);
                }
                else
                {
                    IRefExaminationBll refExaminationBll = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
                    RefExaminationModel4WS refExaminationModel = refExaminationBll.GetRefExaminationByPK(GetExaminationPK());

                    ITimeZoneBll providerBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
                    DateTime agencyTime = providerBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);

                    if (refExaminationModel == null)
                    {
                        return;
                    }

                    //1. display education detail.
                    refExaminationDetail.Display(refExaminationModel);
                    ExaminationNbr = refExaminationModel.refExamNbr.ToString();
                   
                    //2. display provider list.
                    DataTable dtProviderInfo = refProviderList.ConvertProviderListToDataTable(refExaminationModel.providerModels);
                    refProviderList.DataSource = dtProviderInfo;
                    refProviderList.BindProviderList(0, string.Empty);

                    //3. Display Title info
                    lblPropertyInfo.Text = refExaminationModel.examName;

                    SearchEntity = new ExamScheduleSearchModel()
                    {
                        examName = refExaminationModel.examName,
                        entityID = refExaminationModel.refExamNbr,
                        entityType = DocumentEntityType.Examination,
                        servProvCode = ACAConstant.AgencyCode,
                        fromACA = true,
                        startDate = agencyTime,
                        endDate = agencyTime.Date.AddYears(1).AddDays(1).AddSeconds(-1)
                    };

                    RefAvailableExaminationScheduleSearchForm.BindDDL(ACAConstant.AgencyCode, refExaminationModel.refExamNbr.ToString());

                    SearchExamSchedule(SearchEntity);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Search Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            Page.FocusElement(btnSearch.ClientID);
            SearchEntity.city = null;
            SearchEntity.state = null;
            SearchEntity.providerNbr = null;
            SearchEntity.startDate = null;
            SearchEntity.endDate = null;

            if (RefAvailableExaminationScheduleSearchForm.GetScheduleSearchModel(SearchEntity) != null)
            {
                SearchExamSchedule(SearchEntity);
            }
        }

        /// <summary>
        /// Searches the exam schedule.
        /// </summary>
        /// <param name="examScheduleSearchModel">The exam schedule search model.</param>
        private void SearchExamSchedule(ExamScheduleSearchModel examScheduleSearchModel)
        {
            IExaminationBll refExamBll = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));
            RefAvailableExaminationScheduleList.DataSource = ExaminationScheduleUtil.ConvertExaminationSchduleModelToDataTable(
                                                                refExamBll.GetAvailableSchedule(examScheduleSearchModel));
            RefAvailableExaminationScheduleList.ExaminationNbr = ExaminationNbr;
            RefAvailableExaminationScheduleList.ExaminationName = SearchEntity.examName;
            RefAvailableExaminationScheduleList.BindAvailableExaminationScheduleList(true, true, true);
        }

        /// <summary>
        /// Get examinationPK by URL parameter.
        /// </summary>
        /// <returns>a refExaminationPKModel</returns>
        private RefExaminationPKModel4WS GetExaminationPK()
        {
            RefExaminationPKModel4WS refExaminationModel = new RefExaminationPKModel4WS();

            if (ValidationUtil.IsInt(Request["refExamNbr"]))
            {
                refExaminationModel.refExamNbr = long.Parse(Request["refExamNbr"]);
            }

            refExaminationModel.serviceProviderCode = ConfigManager.AgencyCode;

            return refExaminationModel;
        }

        #endregion Methods
    }
}
