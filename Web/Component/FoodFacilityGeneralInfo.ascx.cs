#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FoodFacilityGeneralInfo.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FoodFacilityGeneralInfo.ascx.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// The licensee general for food facility inspection information.
    /// </summary>
    public partial class FoodFacilityGeneralInfo : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Display food facility general information.
        /// </summary>
        /// <param name="licensee">licensee model</param>
        public void Display(LicenseModel4WS licensee)
        {
            try
            {
                if (licensee != null)
                {
                    lblTypeValue.Text = I18nStringUtil.GetString(licensee.resLicenseType, licensee.licenseType);

                    StringBuilder sbContact = new StringBuilder();
                    sbContact.Append(licensee.contactFirstName);
                    sbContact.Append(string.IsNullOrEmpty(licensee.contactFirstName) ? string.Empty : ACAConstant.HTML_NBSP);
                    sbContact.Append(licensee.contactMiddleName);
                    sbContact.Append(string.IsNullOrEmpty(licensee.contactMiddleName) ? string.Empty : ACAConstant.HTML_NBSP);
                    sbContact.Append(licensee.contactLastName);
                    lblContactNameValue.Text = sbContact.ToString();

                    lblLicenseeNumberValue.Text = licensee.stateLicense;
                    lblContactTypeValue.Text = DropDownListBindUtil.GetTypeFlagTextByValue(licensee.typeFlag);
                    lblLicenseStateValue.Text = I18nStringUtil.GetString(licensee.resLicState, licensee.licState);
                    lblBoardValue.Text = I18nStringUtil.GetString(licensee.resLicenseBoard, licensee.licenseBoard);
                    lblBusinessNameValue.Text = licensee.businessName;

                    string country = string.Empty;

                    if (!string.IsNullOrEmpty(licensee.countryCode))
                    {
                        country = StandardChoiceUtil.GetCountryByKey(licensee.countryCode);
                    }

                    IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                    lblAddressValue.Text = addressBuilderBll.Build4License(licensee, country);

                    lblBusinessNumberValue.Text = licensee.businessLicense;
                    lblBusinessExpirationDateValue.Text2 = licensee.businessLicExpDate;
                    lblTelephone1Value.Text = ModelUIFormat.FormatPhoneShow(licensee.phone1CountryCode, licensee.phone1, licensee.countryCode);
                    lblIssueDateValue.Text2 = licensee.licenseIssueDate;
                    lblTelephone2Value.Text = ModelUIFormat.FormatPhoneShow(licensee.phone2CountryCode, licensee.phone2, licensee.countryCode);
                    lblExpirationDateValue.Text2 = licensee.licenseExpirationDate;
                    lblFaxValue.Text = ModelUIFormat.FormatPhoneShow(licensee.faxCountryCode, licensee.fax, licensee.countryCode);
                    lblEmailValue.Text = licensee.emailAddress;

                    bool isLPTemplateDisplaySucceeded = licenseeGeneralInfoLPTemplate.Display(licensee.templateAttributes);
                    hsLPTemplate.Visible = isLPTemplateDisplaySucceeded;
                    bool isLPInfoTableDisplaySucceeded = licenseeGeneralInfoLPPeopleInfoTable.Display(licensee.infoTableGroup);
                    hsLPPeopleInfoTable.Visible = isLPInfoTableDisplaySucceeded;
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// Page load function.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetControlVisible();
            }
        } 

        /// <summary>
        /// Set the control is visible or not.
        /// </summary>
        private void SetControlVisible()
        {
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(ConfigManager.AgencyCode, GviewID.FoodFacilityDetail);

            if (models != null && models.Length > 0)
            {
                foreach (SimpleViewElementModel4WS model in models)
                {
                    string id = model.viewElementName;
                    WebControl ctrl = FindControl(id) as WebControl;

                    bool visible = ACAConstant.VALID_STATUS.Equals(model.recStatus, StringComparison.InvariantCulture);

                    if (ctrl != null && !visible)
                    {
                        if (AppSession.IsAdmin)
                        {
                            ctrl.Attributes.Add("style", "display:none");

                            HtmlGenericControl parent = ctrl.Parent as HtmlGenericControl;
                            if (parent != null)
                            {
                                parent.Attributes.Add("style", "display:none");
                            }
                        }
                        else
                        {
                            ctrl.Visible = false;
                            if (ctrl.Parent != null)
                            {
                                ctrl.Parent.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}
