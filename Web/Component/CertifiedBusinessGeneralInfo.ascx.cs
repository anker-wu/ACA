#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CertifiedBusinessGeneralInfo.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CertifiedBusinessGeneralInfo.ascx.cs 190488 2011-02-17 10:00:36Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Controller for displaying certified business general information.
    /// </summary>
    public partial class CertifiedBusinessGeneralInfo : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Display certified business general information.
        /// </summary>
        /// <param name="license">License selected from certified business list.</param>
        public void DisplayGerneralInfo(LicenseModel4WS license)
        {
            try
            {
                if (license == null)
                {
                    return;
                }

                lblCertifiedBusinessNameValue.Text = license.businessName;
                lblBusinessNameDbaValue.Text = license.busName2;
                lblBusinessDescriptionValue.Text = license.comment;
                lblTelphoneValue.Text = ModelUIFormat.FormatPhoneShow(license.phone1CountryCode, license.phone1, license.countryCode);
                lblFaxValue.Text = ModelUIFormat.FormatPhoneShow(license.faxCountryCode, license.fax, license.countryCode);
                lblEstablishmentDateValue.Text = I18nDateTimeUtil.FormatToDateStringForUI(license.licenseIssueDate);
                lblEmailValue.Text = license.emailAddress;
                lblContactValue.Text = UserUtil.FormatToFullName(license.contactFirstName, license.contactMiddleName, license.contactLastName);
                license.zip = ModelUIFormat.FormatZipShow(license.zip, license.countryCode);
                lblAddressValue.Text = SetLicenseAddressInfo(license);

                object region = LicenseUtil.GetValueFromTemplateFroms(license.template, ACAConstant.NIGP_FIELD_REGION);
                lblRegionValue.Text = region == null ? string.Empty : region.ToString();

                object website = LicenseUtil.GetValueFromTemplateFroms(license.template, ACAConstant.NIGP_FIELD_WEBSITE);
                lblWebSiteValue.Text = website == null ? string.Empty : website.ToString();

                List<object> ownerEthnicity = new List<object>();
                ownerEthnicity = LicenseUtil.GetValueFromTemplateTables(license.template, ACAConstant.NIGP_FIELD_ETHNICITY);
                lblOwnerEthnicityValue.Text = LicenseUtil.ConcatTemplateValue(ownerEthnicity);

                List<object> validThru = LicenseUtil.GetValueFromTemplateTables(license.template, ACAConstant.NIGP_FIELD_CERTIFICATION_EXPIRATION_DATE);
                List<object> certifiedAs = LicenseUtil.GetValueFromTemplateTables(license.template, ACAConstant.NIGP_FIELD_CERTIFICATION_TYPE);

                if (validThru != null && certifiedAs != null)
                {
                    List<KeyValuePair<string, DateTime>> pairList = new List<KeyValuePair<string, DateTime>>();
                    pairList = SortCeritifications(certifiedAs, validThru);
                    DisplayCertifications(pairList);
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// Sort the certification and valid date pair by pair based on date.
        /// </summary>
        /// <param name="certificationList">Certification list</param>
        /// <param name="validList">Valid date list</param>
        /// <returns>Sorted list containing certification and valid date.</returns>
        private List<KeyValuePair<string, DateTime>> SortCeritifications(List<object> certificationList, List<object> validList)
        {
            List<KeyValuePair<string, DateTime>> pairList = new List<KeyValuePair<string, DateTime>>();

            if (certificationList.Count == validList.Count && certificationList.Count > 0)
            {
                int index = -1;
                foreach (object certification in certificationList)
                {
                    index++;

                    if (certification == null || validList[index] == null)
                    {
                        continue;
                    }

                    DateTime validDate  = DateTime.MinValue;
                    I18nDateTimeUtil.TryParseFromUI(I18nDateTimeUtil.FormatToDateStringForUI(validList[index]), out validDate);

                    pairList.Add(new KeyValuePair<string, DateTime>(certification.ToString(), validDate));
                }

                pairList.Sort(
                        delegate(KeyValuePair<string, DateTime> x, KeyValuePair<string, DateTime> y)
                        {
                            return x.Value.CompareTo(y.Value);
                        });
            }
            
            return pairList;
        }

        /// <summary>
        /// Display certification and valid date.
        /// </summary>
        /// <param name="pairList">The list containing certification and valid date in pairs.</param>
        private void DisplayCertifications(List<KeyValuePair<string, DateTime>> pairList)
        {
            if (pairList == null || pairList.Count == 0)
            {
                return;
            }

            string certifiedText = string.Empty;
            string validText = string.Empty;

            for (int i = 0; i < pairList.Count; i++)
            {
                validText += I18nDateTimeUtil.FormatToDateStringForUI(pairList[i].Value) + ACAConstant.COMMA_BLANK;
                certifiedText += pairList[i].Key + ACAConstant.COMMA_BLANK;
            }

            lblValidThruValue.Text = validText.Substring(0, validText.Length - ACAConstant.COMMA_BLANK.Length);
            lblCertifiedAsValue.Text = certifiedText.Substring(0, certifiedText.Length - ACAConstant.COMMA_BLANK.Length);
        }

        /// <summary>
        /// Set license address information.
        /// </summary>
        /// <param name="license">License to be displayed.</param>
        /// <returns>License Address information</returns>
        private string SetLicenseAddressInfo(LicenseModel4WS license)
        {
            string addressInfo = string.Empty;
            string country = string.Empty;

            if (!string.IsNullOrEmpty(license.countryCode))
            {
                country = StandardChoiceUtil.GetCountryByKey(license.countryCode);
            }

            IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
            addressInfo = addressBuilderBll.Build4License(license, country);

            return addressInfo;
        }

        #endregion Methods
    }
}
