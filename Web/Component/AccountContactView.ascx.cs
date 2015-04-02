#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccountContactView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AccountContactView.ascx.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AccountContactView.
    /// </summary>
    public partial class AccountContactView : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// long view id
        /// </summary>
        private const long VIEW_ID = 5050;

        /// <summary>
        /// Display Salutation
        /// </summary>
        private bool displaySalutation = true;

        /// <summary>
        /// Display Title
        /// </summary>
        private bool displayTitle = true;
        
        #endregion Fields

        #region Methods

        /// <summary>
        /// display data from model
        /// </summary>
        /// <param name="publicUser">The PublicUserModel</param>
        public void Display(PublicUserModel4WS publicUser)
        {
            if (AppSession.IsAdmin || publicUser == null || publicUser.peopleModel == null || publicUser.peopleModel.Length == 0)
            {
                contactAddressList.Display(null);
                return;
            }

            accountView.Display(publicUser);
            PeopleModel4WS model = publicUser.peopleModel.FirstOrDefault(p => p.serviceProviderCode.Equals(ConfigManager.AgencyCode, StringComparison.InvariantCultureIgnoreCase));

            if (model != null)
            {
                HiddenStandardFields(model);
                string salutation = displaySalutation ? StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, model.salutation) : string.Empty;
                string title = displayTitle ? model.title : string.Empty;                
                string[] displayNameArray;

                if (!string.IsNullOrEmpty(model.fullName))
                {
                    displayNameArray = !I18nCultureUtil.IsChineseCultureEnabled 
                        ? new[] { salutation, model.fullName } 
                        : new[] { model.fullName, salutation };
                }
                else if (!string.IsNullOrEmpty(model.firstName) || !string.IsNullOrEmpty(model.middleName) || !string.IsNullOrEmpty(model.lastName))
                {
                    displayNameArray = !I18nCultureUtil.IsChineseCultureEnabled
                        ? new[] { salutation, model.firstName, model.middleName, model.lastName }
                        : new[] { model.firstName, model.middleName, model.lastName, salutation };
                }
                else
                {
                    displayNameArray = !I18nCultureUtil.IsChineseCultureEnabled
                        ? new[] { salutation, publicUser.userID }
                        : new[] { publicUser.userID, salutation };
                }

                lblName.Text = I18nStringUtil.FormatToPlainRow(displayNameArray, ACAConstant.BLANK);
                lblTitle.Text = title;
                this.lblConAdd1.Text = model.compactAddress.addressLine1;
                this.lblConTelHome.Text = ModelUIFormat.FormatPhoneShow(model.phone1CountryCode, model.phone1, model.countryCode);
                this.lblConTelWork.Text = ModelUIFormat.FormatPhoneShow(model.phone3CountryCode, model.phone3, model.countryCode);
                this.lblConMobile.Text = ModelUIFormat.FormatPhoneShow(model.phone2CountryCode, model.phone2, model.countryCode);
                this.lblConFax.Text = ModelUIFormat.FormatPhoneShow(model.faxCountryCode, model.fax, model.countryCode);
                this.lblConEmail.Text = model.email;
                this.lblPreContact.Text = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CONTACT_PREFERRED_CHANNEL, model.preferredChannel);
                this.lblBusinessName.Text = string.IsNullOrEmpty(model.businessName) ? string.Empty : model.businessName;
                this.lblBusinessName2.Text = string.IsNullOrEmpty(model.businessName2) ? string.Empty : model.businessName2;
            }

            List<ContactAddressModel> list = null;

            if (model.contactAddressList != null)
            {
                list = model.contactAddressList.ToList();
            }

            contactAddressList.Display(list);
        }

        /// <summary>
        /// On Initialize event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]))
            {
                contactAddressList.ContactSectionPosition = ACAConstant.ContactSectionPosition.RegisterClerkComplete;    
            }
            else
            {
                contactAddressList.ContactSectionPosition = ACAConstant.ContactSectionPosition.RegisterAccountComplete;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin && !StandardChoiceUtil.IsEnableContactAddress())
            {
                divContactAddressList.Visible = false;
            }
        }

        /// <summary>
        /// Hide Standard Fields.
        /// </summary>
        /// <param name="people">The people model.</param>
        private void HiddenStandardFields(PeopleModel4WS people)
        {
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            GFilterScreenPermissionModel4WS permission =
                ControlBuildHelper.GetPermissionWithGenericTemplate(GviewID.RegistrationContactForm, GViewConstant.PERMISSION_PEOPLE, people.contactType, people.template);
            SimpleViewElementModel4WS[] models = 
                gviewBll.GetSimpleViewElementModel(string.Empty, permission, GviewID.RegistrationContactForm, AppSession.User.UserID);

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (ACAConstant.VALID_STATUS.Equals(model.recStatus, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                switch (model.viewElementName)
                {
                    case "txtTitle":
                        displayTitle = false;
                        break;
                    case "ddlAppSalutation":
                        displaySalutation = false;
                        break;
                    case "txtAppOrganizationName":
                        lblBusinessName.Visible = false;
                        break;
                    case "txtAppPhone1":
                        lblConTelHomeTr.Visible = lblConTelHomeTitle.Visible = lblConTelHome.Visible = false;
                        break;
                    case "txtAppPhone3":
                        lblConTelWorkTr.Visible = lblConTelWorkTitle.Visible = lblConTelWork.Visible = false;
                        break;
                    case "txtAppPhone2":
                        lblConMobileTr.Visible = lblConMobileTitle.Visible = lblConMobile.Visible = false;
                        break;
                    case "txtAppFax":
                        lblConFaxTr.Visible = lblConFaxTitle.Visible = lblConFax.Visible = false;
                        break;
                    case "txtAppEmail":
                        lblConEmail.Visible = false;
                        break;
                    case "ddlPreferredChannel":
                        trConPreferredChannel.Visible = lblPreContactTitle.Visible = lblPreContact.Visible = false;
                        break;
                    case "txtBusinessName2":
                        lblBusinessName2.Visible = false;
                        break;
                }
            }           
        }

        #endregion Methods
    }
}