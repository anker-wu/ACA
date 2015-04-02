#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContactContEducationDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefContactContEducationDetail.ascx.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
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
    /// Reference contact Continuing education detail page
    /// </summary>
    public partial class RefContactContEducationDetail : BaseUserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string agencyCode = Request.QueryString[UrlConstant.AgencyCode];
            string eduId = Request.QueryString["conEduId"];

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(eduId))
            {
                return;
            }

            ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
            ContinuingEducationPKModel contEduPKModel = new ContinuingEducationPKModel
                                                            {
                                                                contEduNbr = long.Parse(eduId),
                                                                serviceProviderCode = agencyCode
                                                            };

            ContinuingEducationModel contEducation = licenseCertificationBll.GetContEducationModelByPK(contEduPKModel);

            if (contEducation == null)
            {
                return;
            }

            lblName.Value = contEducation.contEduName;
            lblClass.Value = contEducation.className;
            lblHourOfClass.Value = I18nNumberUtil.FormatNumberForUI(contEducation.hoursCompleted);
            lblCompletionDate.Value = I18nDateTimeUtil.FormatToDateStringForUI(contEducation.dateOfClass);
            lblFinalScore.Value = EducationUtil.FormatScore(contEducation.gradingStyle, I18nNumberUtil.FormatNumberForUI(contEducation.finalScore));
            lblApproved.Value = ModelUIFormat.FormatYNLabel(contEducation.approvedFlag, true);
            lblComments.Value = contEducation.comments;
            genericTemplate.Display(contEducation.template);

            if (contEducation.providerDetailModel == null)
            {
                return;
            }

            lblProviderName.Value = contEducation.providerName;
            lblProviderNumber.Value = contEducation.providerNo;
            lblCity.Value = contEducation.providerDetailModel.city;
            lblZip.Value = ModelUIFormat.FormatZipShow(contEducation.providerDetailModel.zip, contEducation.providerDetailModel.countryCode);
            lblState.Value = I18nUtil.DisplayStateForI18N(contEducation.providerDetailModel.state, contEducation.providerDetailModel.countryCode);
            lblPhone1.Value = ModelUIFormat.FormatPhoneShow(contEducation.providerDetailModel.phone1CountryCode, contEducation.providerDetailModel.phone1, contEducation.providerDetailModel.countryCode);
            lblPhone2.Value = ModelUIFormat.FormatPhoneShow(contEducation.providerDetailModel.phone2CountryCode, contEducation.providerDetailModel.phone2, contEducation.providerDetailModel.countryCode);
            lblFax.Value = ModelUIFormat.FormatPhoneShow(contEducation.providerDetailModel.faxCountryCode, contEducation.providerDetailModel.fax, contEducation.providerDetailModel.countryCode);
            lblEmail.Value = contEducation.providerDetailModel.email;
            lblCountry.Value = StandardChoiceUtil.GetCountryByKey(contEducation.providerDetailModel.countryCode);
            string addressLine = string.Empty;

            if (!string.IsNullOrEmpty(contEducation.providerDetailModel.address1))
            {
                addressLine += contEducation.providerDetailModel.address1;
            }

            if (!string.IsNullOrEmpty(contEducation.providerDetailModel.address2))
            {
                addressLine += "<br>" + contEducation.providerDetailModel.address2;
            }

            if (!string.IsNullOrEmpty(contEducation.providerDetailModel.address3))
            {
                addressLine += "<br>" + contEducation.providerDetailModel.address3;
            }

            lblAddressLines.Value = addressLine;
        }
    }
}