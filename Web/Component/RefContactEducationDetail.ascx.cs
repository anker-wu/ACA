#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContactEducationDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefContactEducationDetail.ascx.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Reference contact education detail page behind.
    /// </summary>
    public partial class RefContactEducationDetail : BaseUserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string agencyCode = Request.QueryString[UrlConstant.AgencyCode];
            string eduId = Request.QueryString["eduId"];

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(eduId))
            {
                return;
            }

            ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
            EducationPKModel eduPKModel = new EducationPKModel
                                              {
                                                  educationNbr = long.Parse(eduId),
                                                  serviceProviderCode = agencyCode
                                              };

            EducationModel education = licenseCertificationBll.GetEducationModelByPK(eduPKModel);

            if (education == null)
            {
                return;
            }

            lblName.Value = education.educationName;
            lblDegree.Value = education.degree;
            lblYearGraduated.Value = education.yearGraduated;
            lblYearJoined.Value = education.yearAttended;
            lblApproved.Value = ModelUIFormat.FormatYNLabel(education.approvedFlag, true);
            lblComments.Value = education.comments;
            genericTemplate.Display(education.template);

            if (education.providerDetailModel == null)
            {
                return;
            }

            lblProviderName.Value = education.providerName;
            lblProviderNumber.Value = education.providerNo;
            lblCity.Value = education.providerDetailModel.city;
            lblZip.Value = ModelUIFormat.FormatZipShow(education.providerDetailModel.zip, education.providerDetailModel.countryCode);
            lblState.Value = I18nUtil.DisplayStateForI18N(education.providerDetailModel.state, education.providerDetailModel.countryCode);
            lblPhone1.Value = ModelUIFormat.FormatPhoneShow(education.providerDetailModel.phone1CountryCode, education.providerDetailModel.phone1, education.providerDetailModel.countryCode);
            lblPhone2.Value = ModelUIFormat.FormatPhoneShow(education.providerDetailModel.phone2CountryCode, education.providerDetailModel.phone2, education.providerDetailModel.countryCode);
            lblFax.Value = ModelUIFormat.FormatPhoneShow(education.providerDetailModel.faxCountryCode, education.providerDetailModel.fax, education.providerDetailModel.countryCode);
            lblEmail.Value = education.providerDetailModel.email;
            lblCountry.Value = StandardChoiceUtil.GetCountryByKey(education.providerDetailModel.countryCode);

            string addressLine = string.Empty;

            if (!string.IsNullOrEmpty(education.providerDetailModel.address1))
            {
                addressLine += education.providerDetailModel.address1;
            }

            if (!string.IsNullOrEmpty(education.providerDetailModel.address2))
            {
                addressLine += "<br>" + education.providerDetailModel.address2;
            }

            if (!string.IsNullOrEmpty(education.providerDetailModel.address3))
            {
                addressLine += "<br>" + education.providerDetailModel.address3;
            }

            lblAddressLines.Value = addressLine;
        }
    }
}