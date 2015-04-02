#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UserLicenseDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: UserLicenseDetail.aspx.cs 278843 2014-09-16 05:29:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;

using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// The user license detail page.
    /// </summary>
    public partial class UserLicenseDetail : PopupDialogBasePage
    {
        #region Properties

        /// <summary>
        /// Gets the license type
        /// </summary>
        private string LicenseType
        {
            get
            {
                return Request.QueryString["licenseType"];
            }
        }

        /// <summary>
        /// Gets license sequence number.
        /// </summary>
        private string LicenseSeqNbr
        {
            get
            {
                return Request.QueryString["licenseSeqNbr"];
            }
        }

        #endregion Properties

        #region Public Events

        /// <summary>
        /// The page load method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("per_licensee_label_title");
            SetPageTitleVisible(false);
            DisplayUserLicenseDetail();
        }

        #endregion Public events

        #region Private methods

        /// <summary>
        /// Display user license detail.
        /// </summary>
        private void DisplayUserLicenseDetail()
        {
            ContractorLicenseModel4WS[] contractorLicenses = AppSession.User.AllContractorLicenses == null || AppSession.User.AllContractorLicenses.Count == 0 ? null : AppSession.User.AllContractorLicenses[ConfigManager.AgencyCode];

            if (contractorLicenses == null || contractorLicenses.Length == 0)
            {
                ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                contractorLicenses = licenseBll.GetContrLicListByUserSeqNBR(ConfigManager.AgencyCode, Request.QueryString[UrlConstant.USER_SEQ_NUM]);
            }

            ContractorLicenseModel4WS contractor = null;

            if (contractorLicenses != null && contractorLicenses.Length != 0)
            {
                contractor = contractorLicenses.Where(p => p.servProvCode == ConfigManager.AgencyCode && p.licenseType == LicenseType && p.licenseSeqNBR == LicenseSeqNbr).SingleOrDefault();
            }

            if (contractor != null)
            {
                LicenseModel4WS license = contractor.license;

                lblLicenseType.Value = I18nStringUtil.GetString(license.resLicenseType, license.licenseType);
                lblContactName.Value = DataUtil.ConcatStringWithSplitChar(new string[] { license.contactFirstName, license.contactLastName }, ACAConstant.BLANK);
                lblStateLicense.Value = license.stateLicense;
                lblBusinessName.Value = license.businessName;
                lblLicenseIssueDate.DateValue = license.licenseIssueDate;
                lblBusinessName2.Value = license.busName2;
                lblLicenseExpirationDate.DateValue = license.licenseExpirationDate;
                lblBusinessLicense.Value = license.businessLicense;
                lblInsuredMax.Value = I18nNumberUtil.FormatMoneyForUI(license.insuranceAmount);
                lblAddress1.Value = license.address1;
                lblAddress2.Value = license.address2;
                lblAddress3.Value = license.address3;
                lblContractorLicNO.Value = license.contrLicNo;
                lblContractorBusiName.Value = license.contLicBusName;
                lblStatus.Value = GetStatusForI18NDisplay(contractor.status);
                lblCity.Value = license.city;
                lblState.Value = I18nUtil.DisplayStateForI18N(license.state, license.countryCode);
                lblZip.Value = ModelUIFormat.FormatZipShow(license.zip, license.countryCode);
                lblHomePhone.Value = ModelUIFormat.FormatPhoneShow(license.phone1CountryCode, license.phone1, license.countryCode);
                lblMobilePhone.Value = ModelUIFormat.FormatPhoneShow(license.phone2CountryCode, license.phone2, license.countryCode);
                lblFax.Value = ModelUIFormat.FormatPhoneShow(license.faxCountryCode, license.fax, license.countryCode);
                lblCountry.Value = StandardChoiceUtil.GetCountryByKey(license.countryCode);
            }
        }

        /// <summary>
        /// Convert the status for I18N display
        /// </summary>
        /// <param name="objStatus">standard english status</param>
        /// <returns>I18N status</returns>
        private string GetStatusForI18NDisplay(object objStatus)
        {
            if (objStatus == null || string.IsNullOrEmpty(objStatus.ToString()))
            {
                return string.Empty;
            }

            string status = objStatus.ToString();
            string resultStatus = status;

            switch (status.ToLowerInvariant())
            {
                case ContractorLicenseStatus.Pending:
                    resultStatus = GetTextByKey("ACA_ContractorLicense_Status_Pending");
                    break;
                case ContractorLicenseStatus.Rejected:
                    resultStatus = GetTextByKey("ACA_ContractorLicense_Status_Rejected");
                    break;
                case ContractorLicenseStatus.Approved:
                    resultStatus = GetTextByKey("ACA_ContractorLicense_Status_Approved");
                    break;
            }

            return resultStatus;
        }

        #endregion Private methods
    }
}