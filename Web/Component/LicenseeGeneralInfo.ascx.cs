#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseeGeneralInfo.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseeGeneralInfo.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Text;
using System.Web.UI;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// licensee general information
    /// </summary>
    public partial class LicenseeGeneralInfo : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// The template collapse line.
        /// </summary>
        private string _templateCollapseLine;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseeGeneralInfo"/> class.
        /// </summary>
        public LicenseeGeneralInfo()
            : base(GviewID.LicenseeDetail)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the template collapse line.
        /// </summary>
        /// <value>The template collapse line.</value>
        public string TemplateCollapseLine
        {
            get
            {
                return _templateCollapseLine;
            }

            set
            {
                _templateCollapseLine = value;
                licenseeGeneralInfoLPTemplate.CollapseLine = value;
            }
        }

        /// <summary>
        /// Gets or sets Permission Model
        /// </summary>
        /// <value></value>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                if (base.Permission == null)
                {
                    base.Permission = new GFilterScreenPermissionModel4WS();
                }

                base.Permission.permissionLevel = GViewConstant.PERMISSION_PEOPLE;
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// display licensee general information
        /// </summary>
        /// <param name="licensee">licensee model</param>
        public void Display(LicenseModel4WS licensee)
        {
            try
            {
                if (licensee == null)
                {
                    return;
                }

                lblLicenseeType.Value = I18nStringUtil.GetString(licensee.resLicenseType, licensee.licenseType);
                Permission.permissionValue = licensee.licenseType;
                StringBuilder sbContact = new StringBuilder();
                sbContact.Append(licensee.contactFirstName);
                sbContact.Append(string.IsNullOrEmpty(licensee.contactFirstName) ? string.Empty : ACAConstant.HTML_NBSP);
                sbContact.Append(licensee.contactMiddleName);
                sbContact.Append(string.IsNullOrEmpty(licensee.contactMiddleName) ? string.Empty : ACAConstant.HTML_NBSP);
                sbContact.Append(licensee.contactLastName);
                lblContactName.Value = sbContact.ToString();

                lblLicenseeNumber.Value = licensee.stateLicense;
                lblContactType.Value = DropDownListBindUtil.GetTypeFlagTextByValue(licensee.typeFlag);
                lblLicenseeState.Value = I18nUtil.DisplayStateForI18N(licensee.licState, licensee.countryCode);
                lblLicenseeBoard.Value = I18nStringUtil.GetString(licensee.resLicenseBoard, licensee.licenseBoard);
                lblLicenseeBusinessName.Value = licensee.businessName;

                StringBuilder sbAddress = new StringBuilder();
                StringBuilder sbCityStateZip = new StringBuilder();
                sbAddress.Append(licensee.address1);
                sbAddress.Append(string.IsNullOrEmpty(licensee.address1) ? string.Empty : ACAConstant.HTML_BR);
                sbAddress.Append(licensee.address2);
                sbAddress.Append(string.IsNullOrEmpty(licensee.address2) ? string.Empty : ACAConstant.HTML_BR);
                sbAddress.Append(licensee.address3);
                sbAddress.Append(string.IsNullOrEmpty(licensee.address3) ? string.Empty : ACAConstant.HTML_BR);

                string resState = I18nUtil.DisplayStateForI18N(licensee.state, licensee.countryCode);
                sbCityStateZip.Append(licensee.city);
                sbCityStateZip.Append(string.IsNullOrEmpty(licensee.city) ? string.Empty : ACAConstant.HTML_NBSP);
                sbCityStateZip.Append(resState);
                sbCityStateZip.Append(string.IsNullOrEmpty(resState) ? string.Empty : ACAConstant.HTML_NBSP);
                sbCityStateZip.Append(ModelUIFormat.FormatZipShow(licensee.zip, licensee.countryCode));
                sbAddress.Append(sbCityStateZip);
                sbAddress.Append(string.IsNullOrEmpty(sbCityStateZip.ToString()) ? string.Empty : ACAConstant.HTML_BR);

                sbAddress.Append(!string.IsNullOrEmpty(licensee.countryCode) ? StandardChoiceUtil.GetCountryByKey(licensee.countryCode) : string.Empty);
                lblLicenseeAddress.Value = sbAddress.ToString();

                lblBusinessNumber.Value = licensee.businessLicense;
                lblBusinessExpirationDate.DateValue = licensee.businessLicExpDate;
                lblLicenseeTelephone1.Value = ModelUIFormat.FormatPhoneShow(licensee.phone1CountryCode, licensee.phone1, licensee.countryCode);
                lblLicenseIssueDate.DateValue = licensee.licenseIssueDate;
                lblLicenseeTelephone2.Value = ModelUIFormat.FormatPhoneShow(licensee.phone2CountryCode, licensee.phone2, licensee.countryCode);
                lblExpirationDate.DateValue = licensee.licenseExpirationDate;
                lblLicenseeFAX.Value = ModelUIFormat.FormatPhoneShow(licensee.faxCountryCode, licensee.fax, licensee.countryCode);
                lblLicenseeEmail.Value = licensee.emailAddress;
                lblLicenseeTitle.Value = licensee.title;

                lblBusinessName2.Value = licensee.busName2;
                lblInsuranceCompany.Value = licensee.insuranceCo;
                lblInsurancePolicy.Value = licensee.policy;

                licenseeGeneralInfoLPTemplate.Display(licensee.templateAttributes);
                licenseeGeneralInfoLPPeopleInfoTable.Display(licensee.infoTableGroup);
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// Initializes License generalInfo
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (AppSession.IsAdmin)
            {
                AccelaDropDownList dropDownList = new AccelaDropDownList();
                dropDownList.ID = "ddlContactType";
                dropDownList.AutoPostBack = false;
                dropDownList.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
                dropDownList.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");
                DropDownListBindUtil.BindLicenseType(dropDownList);
                sectionTitleBar.AddToolBarControls(dropDownList);
                sectionTitleBar.PermissionValueId = dropDownList.ClientID;

                //license type changed event handler in aca admin.
                dropDownList.SelectedIndexChanged += (sender, args) =>
                {
                    this.Permission.permissionValue = dropDownList.SelectedValue;
                    if (!string.IsNullOrEmpty(dropDownList.SelectedValue))
                    {
                        ITemplateBll templateBll = ObjectFactory.GetObject(typeof(ITemplateBll)) as ITemplateBll;
                        TemplateAttributeModel[] attributes = templateBll.GetPeopleTemplateAttributes(dropDownList.SelectedValue, ConfigManager.AgencyCode, AppSession.User.PublicUserId);

                        licenseeGeneralInfoLPTemplate.Display(attributes);
                    }
                    else
                    {
                        licenseeGeneralInfoLPTemplate.Display(null);
                    }
                };
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string prefix = lblLicenseeType.ClientID.Replace(lblLicenseeType.ID, string.Empty);
            sectionTitleBar.SectionID = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR + "{2}" + ACAConstant.SPLIT_CHAR + "{3}", ModuleName, GviewID.LicenseeDetail, prefix, TemplateCollapseLine);
            phContent.TemplateControlIDPrefix = "template_Licensee_Detail_";
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            InitFormDesignerPlaceHolder(phContent, Permission.permissionValue);
        }

        #endregion Methods
    }
}
