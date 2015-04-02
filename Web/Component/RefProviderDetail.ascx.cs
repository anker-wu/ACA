#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefProviderDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefProviderDetail.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.UI.HtmlControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UC for refProviderDetail.
    /// </summary>
    public partial class RefProviderDetail : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Display provider detail info by licenseModel.
        /// </summary>
        /// <param name="providerModel">a providerModel4WS</param>
        public void DisplayProviderDetail(ProviderModel4WS providerModel)
        {
            if (providerModel == null || providerModel.refLicenseProfessionalModel == null)
            {
                return;
            }

            lblProviderNameValue.Text = providerModel.providerName;
            lblProviderNumberValue.Text = providerModel.providerNo;

            lblEducationProviderValue.Text = EducationUtil.ConvertRequiredField2Display(providerModel.offerEducation);
            lblContEducationProviderValue.Text = EducationUtil.ConvertRequiredField2Display(providerModel.offerContinuing);
            lblExaminationProviderValue.Text = EducationUtil.ConvertRequiredField2Display(providerModel.offerExamination);

            RefLicenseProfessionalModel4WS refLPModel = providerModel.refLicenseProfessionalModel;

            lblContactNameValue.Text = GetContactName(refLPModel.contact);

            lblLicenseNumberValue.Text = refLPModel.stateLicense;
            lblIssueDateValue.Text2 = refLPModel.licenseIssueDate;
            lblExpirationDateValue.Text2 = refLPModel.licenseExpirationDate;

            lblAddressValue.Text = GetAddress(refLPModel);

            lblPhone1Value.Text = ModelUIFormat.FormatPhoneShow(refLPModel.phone1CountryCode, refLPModel.phone1, refLPModel.countyCode);
            lblPhone2Value.Text = ModelUIFormat.FormatPhoneShow(refLPModel.phone2CountryCode, refLPModel.phone2, refLPModel.countyCode);
            lblFaxValue.Text = ModelUIFormat.FormatPhoneShow(refLPModel.faxCountryCode, refLPModel.fax, refLPModel.countyCode);
            lblEmailValue.Text = refLPModel.email;
        }

        /// <summary>
        /// Get contact name, it contains first name, middle name, last name.
        /// </summary>
        /// <param name="contact">contact model</param>
        /// <returns>contact name contains first name, middle name, last name</returns>
        private string GetContactName(ContactModel4WS contact)
        {
            StringBuilder sbContactName = new StringBuilder();

            if (contact != null)
            {
                if (!string.IsNullOrEmpty(contact.contactFirstName))
                {
                    sbContactName.Append(ScriptFilter.FilterScript(contact.contactFirstName));
                    sbContactName.Append(ACAConstant.HTML_NBSP);
                }

                if (!string.IsNullOrEmpty(contact.contactMiddleName))
                {
                    sbContactName.Append(ScriptFilter.FilterScript(contact.contactMiddleName));
                    sbContactName.Append(ACAConstant.HTML_NBSP);
                }

                if (!string.IsNullOrEmpty(contact.contactLastName))
                {
                    sbContactName.Append(ScriptFilter.FilterScript(contact.contactLastName));
                }
            }

            return sbContactName.ToString();
        }

        /// <summary>
        /// Get address, it contains address1,address2,address3,city,state,zip.
        /// </summary>
        /// <param name="licensePro">ref license professional model</param>
        /// <returns>address1, address2, address3, city, state, zip</returns>
        private string GetAddress(RefLicenseProfessionalModel4WS licensePro)
        {
            StringBuilder sbAddress = new StringBuilder();

            if (licensePro != null)
            {
                if (!string.IsNullOrEmpty(licensePro.address1))
                {
                    sbAddress.Append(ScriptFilter.FilterScript(licensePro.address1));
                    sbAddress.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(ScriptFilter.FilterScript(licensePro.address2)))
                {
                    sbAddress.Append(licensePro.address2);
                    sbAddress.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licensePro.address3))
                {
                    sbAddress.Append(ScriptFilter.FilterScript(licensePro.address3));
                    sbAddress.Append(ACAConstant.HTML_BR);
                }

                if (!string.IsNullOrEmpty(licensePro.city))
                {
                    sbAddress.Append(ScriptFilter.FilterScript(licensePro.city));
                    sbAddress.Append(ACAConstant.COMMA);
                    sbAddress.Append(ACAConstant.HTML_NBSP);
                }

                if (!string.IsNullOrEmpty(licensePro.state))
                {
                    sbAddress.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licensePro.state, licensePro.countyCode)));
                    sbAddress.Append(ACAConstant.HTML_NBSP);
                }

                if (!string.IsNullOrEmpty(licensePro.zip))
                {
                    sbAddress.Append(ModelUIFormat.FormatZipShow(licensePro.zip, licensePro.countyCode));
                }

                if (!string.IsNullOrEmpty(licensePro.countyCode))
                {
                    sbAddress.Append(ACAConstant.HTML_BR);
                    sbAddress.Append(StandardChoiceUtil.GetCountryByKey(licensePro.countyCode));
                }
            }

            return sbAddress.ToString();
        }

        #endregion Methods
    }
}
