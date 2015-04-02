#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExaminationDetail.ascx.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Examination detail page
    /// </summary>
    public partial class ExaminationDetail : BaseUserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string agencyCode = Request.QueryString[UrlConstant.AgencyCode];
            string examId = Request.QueryString["ExaminationNum"];

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(examId))
            {
                return;
            }

            IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
            ExaminationPKModel examinPKModel = new ExaminationPKModel
                                                    {
                                                        examNbr = long.Parse(examId),
                                                        serviceProviderCode = agencyCode
                                                    };

            ExaminationModel examination = examinationBll.GetExamByPK(examinPKModel);

            if (examination == null)
            {
                return;
            }

            lblName.Value = examination.examName;
            lblExaminationDate.DateValue = examination.examDate;
            
            if (examination.startTime.HasValue)
            {
                lblStartTime.Value = I18nDateTimeUtil.FormatToTimeStringForUI(examination.startTime.Value, false);
            }

            if (examination.endTime.HasValue)
            {
                lblEndTime.Value = I18nDateTimeUtil.FormatToTimeStringForUI(examination.endTime.Value, false);
            }

            lblRosterID.Value = examination.userExamID;
            lblComments.Value = examination.comments;
            lblRequired.Value = ModelUIFormat.FormatYNLabel(examination.requiredFlag);
            lblApproved.Value = ModelUIFormat.FormatYNLabel(examination.approvedFlag, true);
            genericTemplate.Display(examination.template);

            if (examination.examProviderDetailModel == null)
            {
                return;
            }

            lblProviderName.Value = examination.providerName;
            lblProviderNumber.Value = examination.providerNo;
            lblCity.Value = examination.examProviderDetailModel.city;
            lblZip.Value = ModelUIFormat.FormatZipShow(examination.examProviderDetailModel.zip, examination.examProviderDetailModel.countryCode);
            lblState.Value = I18nUtil.DisplayStateForI18N(examination.examProviderDetailModel.state, examination.examProviderDetailModel.countryCode);
            lblPhone1.Value = ModelUIFormat.FormatPhoneShow(examination.examProviderDetailModel.phone1CountryCode, examination.examProviderDetailModel.phone1, examination.examProviderDetailModel.countryCode);
            lblPhone2.Value = ModelUIFormat.FormatPhoneShow(examination.examProviderDetailModel.phone2CountryCode, examination.examProviderDetailModel.phone2, examination.examProviderDetailModel.countryCode);
            lblFax.Value = ModelUIFormat.FormatPhoneShow(examination.examProviderDetailModel.faxCountryCode, examination.examProviderDetailModel.fax, examination.examProviderDetailModel.countryCode);
            lblEmail.Value = examination.examProviderDetailModel.email;
            lblCountry.Value = StandardChoiceUtil.GetCountryByKey(examination.examProviderDetailModel.countryCode);
            string finalScore = examination.finalScore.HasValue ? examination.finalScore.ToString() : string.Empty;
            lblFinalScore.Value = EducationUtil.FormatScore(examination.gradingStyle, finalScore);

            string addressLine = string.Empty;

            if (!string.IsNullOrEmpty(examination.examProviderDetailModel.address1))
            {
                addressLine += examination.examProviderDetailModel.address1;
            }

            if (!string.IsNullOrEmpty(examination.examProviderDetailModel.address2))
            {
                addressLine += "<br>" + examination.examProviderDetailModel.address2;
            }

            if (!string.IsNullOrEmpty(examination.examProviderDetailModel.address3))
            {
                addressLine += "<br>" + examination.examProviderDetailModel.address3;
            }

            lblAddressLines.Value = addressLine;
        }
    }
}