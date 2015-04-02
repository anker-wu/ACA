#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationDetail.ascx.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// education detail page behind.
    /// </summary>
    public partial class EducationDetail : BaseUserControl
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

            if (!string.IsNullOrEmpty(agencyCode) && !string.IsNullOrEmpty(eduId))
            {
                ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
                EducationModel education =
                    licenseCertificationBll.GetEducationModelByPK(
                        new EducationPKModel() { educationNbr = long.Parse(eduId), serviceProviderCode = agencyCode });

                if (education == null)
                {
                    return;
                }

                this.lblName.Value = education.educationName;
                this.lblDegree.Value = education.degree;
                this.lblYearGraduated.Value = education.yearGraduated;
                this.lblYearJoined.Value = education.yearAttended;
                this.lblRequire.Value = ModelUIFormat.FormatYNLabel(education.requiredFlag, true);
                this.lblApproved.Value = ModelUIFormat.FormatYNLabel(education.approvedFlag, true);
                this.lblComments.Value = education.comments;
                this.genericTemplate.Display(education.template);

                if (education.providerDetailModel == null)
                {
                    return;
                }

                this.lblProviderName.Value = education.providerName;
                this.lblProviderNumber.Value = education.providerNo;
                this.lblCity.Value = education.providerDetailModel.city;
                this.lblZip.Value = ModelUIFormat.FormatZipShow(education.providerDetailModel.zip, education.providerDetailModel.countryCode);
                this.lblState.Value = I18nUtil.DisplayStateForI18N(education.providerDetailModel.state, education.providerDetailModel.countryCode);
                this.lblPhone1.Value = ModelUIFormat.FormatPhoneShow(education.providerDetailModel.phone1CountryCode, education.providerDetailModel.phone1, education.providerDetailModel.countryCode);
                this.lblPhone2.Value = ModelUIFormat.FormatPhoneShow(education.providerDetailModel.phone2CountryCode, education.providerDetailModel.phone2, education.providerDetailModel.countryCode);
                this.lblFax.Value = ModelUIFormat.FormatPhoneShow(education.providerDetailModel.faxCountryCode, education.providerDetailModel.fax, education.providerDetailModel.countryCode);
                this.lblEmail.Value = education.providerDetailModel.email;
                this.lblCountry.Value = StandardChoiceUtil.GetCountryByKey(education.providerDetailModel.countryCode);

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

                this.lblAddressLines.Value = addressLine;
            }
        }
    }
}