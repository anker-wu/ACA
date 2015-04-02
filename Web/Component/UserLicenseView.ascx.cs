#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UserLicenseView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2012
 *
 *  Description:
 *
 *  Notes:
 *      $Id: UserLicenseView.ascx.cs 233940 2012-09-27 07:49:28Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ablity to operation UserLicenseView.
    /// </summary>
    public partial class UserLicenseView : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets control states which includes View or Edit to control the remove button whether requires or not.
        /// </summary>
        public string EditState
        {
            get
            {
                if (ViewState["Edit_State"] == null)
                {
                    ViewState["Edit_State"] = "View";
                }

                return ViewState["Edit_State"].ToString();
            }

            set
            {
                ViewState["Edit_State"] = value;
            }
        }

        /// <summary>
        /// Gets or sets licenses list.
        /// </summary>
        private ArrayList GridViewDataSource
        {
            get
            {
                if (ViewState["GridViewDataSource"] == null)
                {
                    return new ArrayList(0);
                }

                return ViewState["GridViewDataSource"] as ArrayList;
            }

            set
            {
                ViewState["GridViewDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether remove button state enable statue.
        /// </summary>
        private bool RemoveBtnEnabled
        {
            get
            {
                if (ViewState["RemoveBtnEnabled"] == null)
                {
                    ViewState["RemoveBtnEnabled"] = true;
                }

                return (bool) ViewState["RemoveBtnEnabled"];
            }

            set
            {
                ViewState["RemoveBtnEnabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets user sequence number.
        /// </summary>
        private string UserSeqNum
        {
            get
            {
                return ViewState["User_Seq_Num"].ToString();
            }

            set
            {
                ViewState["User_Seq_Num"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Display license list for current user.
        /// </summary>
        /// <param name="userSeqNum">user sequence number.</param>
        public void DisplayLicenseList(string userSeqNum)
        {
            ArrayList licenseList = new ArrayList();

            if (AppSession.IsAdmin)
            {
                ContractorLicenseModel4WS emptyModel = new ContractorLicenseModel4WS();
                licenseList.Add(emptyModel);

                GridViewDataSource = licenseList;
                BindLicenseList();
                return;
            }

            // stores the userSeqNum to be used when remove click event raise.
            this.UserSeqNum = userSeqNum;
            ContractorLicenseModel4WS[] contractorLicenses = LicenseUtil.GetContractorLicenseByUserSeqNumber(ConfigManager.AgencyCode, userSeqNum);

            if (contractorLicenses != null && contractorLicenses.Length > 0)
            {
                foreach (ContractorLicenseModel4WS conLicModel in contractorLicenses)
                {
                    if (conLicModel == null || conLicModel.license == null)
                    {
                        continue;
                    }

                    conLicModel.license.zip = ModelUIFormat.FormatZipShow(conLicModel.license.zip, conLicModel.license.countryCode);
                    conLicModel.license.phone1 = ModelUIFormat.FormatPhone4EditPage(conLicModel.license.phone1, conLicModel.license.countryCode);
                    conLicModel.license.phone2 = ModelUIFormat.FormatPhone4EditPage(conLicModel.license.phone2, conLicModel.license.countryCode);
                    conLicModel.license.fax = ModelUIFormat.FormatPhone4EditPage(conLicModel.license.fax, conLicModel.license.countryCode);
                    conLicModel.license.licState = I18nUtil.DisplayLicenseStateForI18N(conLicModel.license.licState);
                    conLicModel.license.state = I18nUtil.DisplayLicenseStateForI18N(conLicModel.license.state);
                    licenseList.Add(conLicModel);
                }
            }

            GridViewDataSource = licenseList;
            BindLicenseList();
        }

        /// <summary>
        /// chang remove button enable statue.
        /// </summary>
        /// <param name="enableStatus">enable status</param>
        public void IsEnableRemoveBtn(bool enableStatus)
        {
            if (!this.IsPostBack)
            {
                return;
            }

            RemoveBtnEnabled = enableStatus;
            BindLicenseList();
        }

        /// <summary>
        /// Format Date.
        /// </summary>
        /// <param name="fulldatatime">full data time</param>
        /// <returns>format date</returns>
        protected static string FormatDate(object fulldatatime)
        {
            if (fulldatatime != null)
            {
                return I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(fulldatatime.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// Format license state information for displaying in UI.
        /// </summary>
        /// <param name="licenseState">The license state</param>
        /// <param name="punctuation">The punctuation for connecting license information,such as a blank or a hyphen</param>
        /// <returns>string after formating license state.</returns>
        protected string Format4LicenseState(string licenseState, string punctuation)
        {
            string licenseInfo = string.Empty;

            if (!String.IsNullOrEmpty(licenseState) && StandardChoiceUtil.IsDisplayLicenseState())
            {
                licenseInfo = String.Format("{0}{1}", licenseState, punctuation);
            }

            return licenseInfo;
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
                if (GridViewDataSource.Count > 0 && EditState.Equals("Edit", StringComparison.InvariantCultureIgnoreCase))
                {
                    divLicLine.Visible = true;
                }
            }
        }

        /// <summary>
        /// removes the selected license.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lbkRemoveLicenseItem_OnClick(object sender, EventArgs e)
        {
            LinkButton lbndel = (LinkButton) sender;
            string seqNbr = lbndel.Attributes["SeqNbr"];
            string type = lbndel.Attributes["Type"];

            LicenseModel4WS licenseModel = new LicenseModel4WS();
            licenseModel.licenseType = type;
            licenseModel.licSeqNbr = seqNbr;
            licenseModel.serviceProviderCode = ConfigManager.AgencyCode;

            ILicenseBLL licenseBll = (ILicenseBLL) ObjectFactory.GetObject(typeof(ILicenseBLL));
            licenseBll.DeleteContracotrLicensePK(ConfigManager.AgencyCode, licenseModel, AppSession.User.PublicUserId, AppSession.User.UserSeqNum);
            //Update license of user session.
            LicenseModel4WS[] licenses = AppSession.User.Licenses;

            if (licenses != null && licenses.Length > 0)
            {
                List<LicenseModel4WS> licenseList = new List<LicenseModel4WS>();

                foreach (LicenseModel4WS license in licenses)
                {
                    if (license.serviceProviderCode.Equals(ConfigManager.AgencyCode, StringComparison.InvariantCultureIgnoreCase)
                        && license.licenseType.Equals(type, StringComparison.InvariantCultureIgnoreCase)
                        && license.licSeqNbr.Equals(seqNbr, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    licenseList.Add(license);
                }

                AppSession.User.UserModel4WS.licenseModel = licenseList.ToArray();
            }

            AppSession.User.AllContractorLicenses = null;
            DisplayLicenseList(UserSeqNum);
        }

        /// <summary>
        /// rptLicense ItemDataBind.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rptLicense_ItemDataBind(object sender, RepeaterItemEventArgs e)
        {
            LinkButton lbnDel = (LinkButton) e.Item.FindControl("removeLicenseItem");

            // view state
            //if (EditState.ToUpper() == "VIEW")
            if (EditState.Equals("VIEW", StringComparison.InvariantCultureIgnoreCase))
            {
                lbnDel.Visible = false;
                return;
            }
            else
            {
                //The remove licnese link enable is configured in ACA admin.
                if (StandardChoiceUtil.DisabledRemoveLicense())
                {
                    lbnDel.Visible = false;
                }
                else
                {
                    if (IsDisableRevButton())
                    {
                        lbnDel.Enabled = false;
                        DisableButton(lbnDel);
                        return;
                    }

                    if (RemoveBtnEnabled)
                    {
                        // edit state
                        lbnDel.Visible = true;

                        // the SeqNbr and Type are used to get the seqNbr and type value when removing a license.
                        HiddenField hdnLicSeqNbr = (HiddenField)e.Item.FindControl("hdnLicSeqNbr");
                        HiddenField hdnLicType = (HiddenField)e.Item.FindControl("hdnLicType");
                        lbnDel.Attributes.Add("SeqNbr", hdnLicSeqNbr.Value);
                        lbnDel.Attributes.Add("Type", hdnLicType.Value);
                    }
                    else
                    {
                        lbnDel.Enabled = false;
                    }

                    if (!lbnDel.Enabled)
                    {
                        DisableButton(lbnDel);
                    }
                }
            }
        }

        /// <summary>
        /// bind license item data
        /// </summary>
        private void BindLicenseList()
        {
            rptLicense.DataSource = GridViewDataSource;
            rptLicense.DataBind();
        }

        /// <summary>
        /// change button style when disable button. 
        /// </summary>
        /// <param name="lbnDel">delete link button.</param>
        private void DisableButton(LinkButton lbnDel)
        {
            lbnDel.OnClientClick = string.Empty;
        }

        /// <summary>
        /// Convert the status for I18N display
        /// </summary>
        /// <param name="objStatus">standard english status</param>
        /// <returns>I18N status</returns>
        protected string GetStatusForI18NDisplay(object objStatus)
        {
            if (objStatus == null || String.IsNullOrEmpty(objStatus.ToString()))
            {
                return String.Empty;
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

        /// <summary>
        /// need disable remove button by option value in stander choice.
        /// </summary>
        /// <returns>ture or false.</returns>
        private bool IsDisableRevButton()
        {
            bool isRegisterLPuser = StandardChoiceUtil.IsRequiredLicense();

            //if require license to register and license count is one, the remove Button should disable.
            if (GridViewDataSource.Count == 1 && isRegisterLPuser)
            {
                return true;
            }

            return false;
        }

        #endregion Methods
    }
}