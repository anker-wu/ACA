#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseEdit.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation LicenseEdit.
    /// </summary>
    public partial class LicenseEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// A value to distinguish the refresh single license section.
        /// </summary>
        protected const string REFRESH_LICENSE = "$RefreshLicense$";

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(LicenseEdit));

        /// <summary>
        /// indicate the license form is editable or not.
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        /// R-Reference,need reference data
        /// T-Transactional, user input data.
        /// N-No Limitation,both reference and daily
        /// </summary>
        private string _validateFlag = string.Empty;

        /// <summary>
        /// parent control id.
        /// </summary>
        private string _parentControlID = string.Empty;

        /// <summary>
        /// Remove license event by clicking remove button
        /// </summary>
        public event CommonEventHandler LicenseRemovedEvent;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets focus click id.
        /// </summary>
        public string SkippingToParentClickID
        {
            get
            {
                return ViewState["SkippingToParentClickID"] as string;
            }

            set
            {
                ViewState["SkippingToParentClickID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["LicensePros"];
            }

            set
            {
                ViewState["LicensePros"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether all required field, False skip validate required field
        /// </summary>
        public bool IsSectionRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for license
        /// </summary>
        public bool IsValidate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether data comes from reference or not.
        /// </summary>
        public string ValidateFlag
        {
            get
            {
                return _validateFlag;
            }

            set
            {
                _validateFlag = value;

                if (ComponentDataSource.Reference.Equals(value))
                {
                    IsValidate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }

            set
            {
                _isEditable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form be used in Multiple LP edit form.
        /// </summary>
        public bool IsMultipleLicensedProfessional { get; set; }

        /// <summary>
        /// Gets or sets component name.
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// Gets or sets parent control id.
        /// </summary>
        public string ParentControlID
        {
            get
            {
                if (!IsMultipleLicensedProfessional)
                {
                    return ClientID;
                }
                
                return _parentControlID;
            }

            set
            {
                _parentControlID = value;
            }
        }

        /// <summary>
        /// Gets edit license function name
        /// </summary>
        public string EditLicenseFunction
        {
            get
            {
                return ClientID + "_EditLicense";
            }
        }

        /// <summary>
        /// Gets current SubAgencyCap type
        /// </summary>
        protected string IsSubAgencyCap
        {
            get { return Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]; }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Presents the license professional information to control.
        /// </summary>
        /// <param name="license">An LicenseProfessionalModel data to be presented.</param>
        public void DisplayLicense(LicenseProfessionalModel license)
        {
            ucConditon.HideCondition();

            if (license == null || (!string.IsNullOrEmpty(license.licSeqNbr) && !ShowCondition(long.Parse(license.licSeqNbr))))
            {
                return;
            }

            // If exists LP data in LP section, hide the Select From Account/Add New/Lookup buttons.
            if (!AppSession.IsAdmin && !IsMultipleLicensedProfessional)
            {
                divLicenseButton.Visible = false;
            }

            divLicenseView.Visible = true;
            LicenseSummary.Display(license);
            hfLicenseProId.Value = license.licSeqNbr;
            hfLicenseNbr.Value = license.licenseNbr;
            hfLicenseType.Value = license.licenseType;
            hfLicenseTempID.Value = license.TemporaryID;
        }

        /// <summary>
        /// Indicating the form data whether is valid.
        /// </summary>
        /// <returns>True if the license is valid</returns>
        public bool IsDataValid()
        {
            bool isValid = !IsEditable
                           || !IsValidate
                           || string.IsNullOrEmpty(hfLicenseProId.Value)
                           || (!string.IsNullOrEmpty(hfLicenseProId.Value)
                               && ComponentDataSource.Reference.Equals(ValidateFlag)
                               && hfLicenseProId.Value != ACAConstant.DAILY_LICENSE_NUMBER);

            return isValid;
        }

        /// <summary>
        /// Hide the condition
        /// </summary>
        public void HideCondition()
        {
            ucConditon.HideCondition();
        }

        /// <summary>
        /// Shows the validate error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowValidateErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageUtil.ShowMessageByControl(errorMessageLabel, MessageType.Error, message);
            }
        }

        /// <summary>
        /// According action type to display message.
        /// </summary>
        /// <param name="actionType">action type</param>
        public void ShowActionNoticeMessage(ActionType actionType)
        {
            // Show message for contact
            string labelKey = null;
            divActionNotice.Visible = true;
            divImgFailed.Visible = false;
            divImgSuccess.Visible = false;
            lblActionNoticeAddSuccess.Visible = false;
            lblActionNoticeEditSuccess.Visible = false;
            lblActionNoticeDeleteSuccess.Visible = false;
            lblActionNoticeAddFailed.Visible = false;
            lblActionNoticeEditFailed.Visible = false;
            lblActionNoticeDeleteFailed.Visible = false;

            switch (actionType)
            {
                case ActionType.AddSuccess:
                    divImgSuccess.Visible = true;
                    lblActionNoticeAddSuccess.Visible = true;
                    labelKey = lblActionNoticeAddSuccess.LabelKey;
                    break;
                case ActionType.UpdateSuccess:
                    divImgSuccess.Visible = true;
                    lblActionNoticeEditSuccess.Visible = true;
                    labelKey = lblActionNoticeEditSuccess.LabelKey;
                    break;
                case ActionType.DeleteSuccess:
                    divImgSuccess.Visible = true;
                    lblActionNoticeDeleteSuccess.Visible = true;
                    labelKey = lblActionNoticeDeleteSuccess.LabelKey;
                    break;
                case ActionType.AddFailed:
                    divImgFailed.Visible = true;
                    lblActionNoticeAddFailed.Visible = true;
                    labelKey = lblActionNoticeAddFailed.LabelKey;
                    break;
                case ActionType.UpdateFailed:
                    divImgFailed.Visible = true;
                    lblActionNoticeEditFailed.Visible = true;
                    labelKey = lblActionNoticeEditFailed.LabelKey;
                    break;
                case ActionType.DeleteFailed:
                    divImgFailed.Visible = true;
                    lblActionNoticeDeleteFailed.Visible = true;
                    labelKey = lblActionNoticeDeleteFailed.LabelKey;
                    break;
            }

            if (AccessibilityUtil.AccessibilityEnabled)
            {
                MessageUtil.ShowAlertMessage(this, GetTextByKey(labelKey));
            }
        }

        /// <summary>
        /// Hide action notice message. 
        /// </summary>
        public void HideActionNoticeMessage()
        {
            divActionNotice.Visible = false;
            divImgFailed.Visible = false;
            divImgSuccess.Visible = false;
            lblActionNoticeAddSuccess.Visible = false;
            lblActionNoticeEditSuccess.Visible = false;
            lblActionNoticeDeleteSuccess.Visible = false;
            lblActionNoticeAddFailed.Visible = false;
            lblActionNoticeEditFailed.Visible = false;
            lblActionNoticeDeleteFailed.Visible = false;
            LicenseEditPanel.Update();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            RefreshLicenseSection();
            InitTemplateService();
            InitUI();
            spanLicenseNotice.Hide();
            hlEnd.NextControlClientID = SkippingToParentClickID;

            btnLookUpLP.OnClientClick = "return " + ClientID + "_LookUpLP();";
            btnAddNewLP.OnClientClick = "return " + ClientID + "_AddNewLP();";
            btnSelectLPFromAccount.OnClientClick = "return " + ClientID + "_SelectLPFromAccount();";
            btnEdit.OnClientClick = "return " + EditLicenseFunction + "();";
            btnRemove.OnClientClick = "return " + ClientID + "_RemoveLicense();";
        }

        /// <summary>
        /// OnPreRender event.
        /// </summary>
        /// <param name="e">event argument object</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsMultipleLicensedProfessional)
            {
                string errorMessage = IsReferenceDataValidate();

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ShowValidateErrorMessage(errorMessage);

                    //if the License validate failed then should return to prevent the required validate message overwrite.
                    return;
                }

                SetFieldRequiredValidation();   
                SetSectionRequiredValidation();
            }
        }

        /// <summary>
        /// Initials the template web method.
        /// </summary>
        protected void InitTemplateService()
        {
            ScriptManager smg = ScriptManager.GetCurrent(Page);
            smg.EnablePageMethods = true;
            ServiceReference srTemplate = new ServiceReference("~/WebService/TemplateService.asmx");
            smg.Services.Add(srTemplate);
        }

        /// <summary>
        /// Remove the license.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void BtnRemoveClick(object sender, EventArgs e)
        {
            try
            {
                LicenseRemovedEvent(sender, new CommonEventArgs(ComponentName));
                divLicenseView.Visible = false;
                divLicenseButton.Visible = true;
                ucConditon.HideCondition();
                hfLicenseProId.Value = string.Empty;
                ShowActionNoticeMessage(ActionType.DeleteSuccess);
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                ShowActionNoticeMessage(ActionType.DeleteFailed);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Clear ViewState
        /// </summary>
        private void ClearViewState()
        {
            GridViewDataSource = null;
        }

        /// <summary>
        /// get license condition 
        /// </summary>
        /// <param name="licenseSeqNumber">license Sequence Number</param>
        /// <returns>license pro model.</returns>
        private LicenseModel4WS GetLicenseCondition(long licenseSeqNumber)
        {
            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            LicenseModel4WS licenseModel;

            if (!CapUtil.IsSuperCAP(ModuleName))
            {
                licenseModel = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, licenseSeqNumber, AppSession.User.PublicUserId);
                return licenseModel;
            }

            ServiceModel[] services = AppSession.GetSelectedServicesFromSession();
            IList<string> agencies = new List<string>();

            if (services != null)
            {
                foreach (ServiceModel service in services)
                {
                    if (!agencies.Contains(service.servPorvCode))
                    {
                        agencies.Add(service.servPorvCode);
                    }
                }
            }
            else
            {
                // from resume,get agencies from child caps.
                CapModel4WS parentCap = AppSession.GetCapModelFromSession(ModuleName);

                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                CapIDModel4WS[] childCapIDs = capBll.GetChildCaps(parentCap.capID);

                if (childCapIDs != null)
                {
                    foreach (CapIDModel4WS childCap in childCapIDs)
                    {
                        if (!agencies.Contains(childCap.serviceProviderCode))
                        {
                            agencies.Add(childCap.serviceProviderCode);
                        }
                    }
                }
            }

            string[] agencyList = new string[agencies.Count];
            agencies.CopyTo(agencyList, 0);

            licenseModel = licenseBll.GetLicenseCondition(agencyList, licenseSeqNumber);
            return licenseModel;
        }

        /// <summary>
        /// Initial UI method.
        /// </summary>
        private void InitUI()
        {
            /* 
               when set conpoment property data source 'Transactional',
               should hidden search button and clean button.
             */
            btnRemove.Visible = IsEditable;
            LicenseModel4WS[] userLicenses = LicenseUtil.GetPublicUserLicenses(ModuleName);
            btnSelectLPFromAccount.Visible = IsEditable && userLicenses != null && userLicenses.Length > 0;
            btnAddNewLP.Visible = IsEditable && !ComponentDataSource.Reference.Equals(ValidateFlag, StringComparison.InvariantCultureIgnoreCase);
            btnLookUpLP.Visible = IsEditable && StandardChoiceUtil.IsEnableRefLPSearch() && !ComponentDataSource.Transactional.Equals(ValidateFlag, StringComparison.InvariantCultureIgnoreCase);
            divLicenseButton.Visible = btnAddNewLP.Visible || btnSelectLPFromAccount.Visible || btnLookUpLP.Visible;

            if (AppSession.IsAdmin)
            {
                divActionNotice.Visible = true;
                lblActionNoticeAddFailed.Visible = true;
                lblActionNoticeAddSuccess.Visible = true;
                lblActionNoticeDeleteFailed.Visible = true;
                lblActionNoticeDeleteSuccess.Visible = true;
                lblActionNoticeEditFailed.Visible = true;
                lblActionNoticeEditSuccess.Visible = true;
                btnAddNewLP.Visible = true;
                btnLookUpLP.Visible = true;
                btnSelectLPFromAccount.Visible = true;
                divLicenseView.Visible = true;
            }
        }

        /// <summary>
        /// if conditionModel is not null, show locked, hold or notice message.
        /// </summary>
        /// <param name="licenseSeqNumber">the license sequence number</param>
        /// <returns>true or false.</returns>
        private bool ShowCondition(long licenseSeqNumber)
        {
            bool isLocked = false;

            LicenseModel4WS licenseModel = GetLicenseCondition(licenseSeqNumber);

            if (licenseModel == null || licenseModel.noticeConditions == null || licenseModel.noticeConditions.Length == 0)
            {
                return true;
            }

            if (licenseModel.hightestCondition == null || string.IsNullOrEmpty(licenseModel.hightestCondition.impactCode))
            {
                return true;
            }

            isLocked = ucConditon.IsShowCondition(licenseModel.noticeConditions, licenseModel.hightestCondition, ConditionType.License);

            if (!isLocked)
            {
                ClearViewState();
            }

            return !isLocked;
        }

        /// <summary>
        /// Validate section required when section is set required and no data input 
        /// Has some required fields not input and some field format not correct.
        /// </summary>
        private void SetFieldRequiredValidation()
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            LicenseProfessionalModel4WS licenseProfessional4WS = LicenseUtil.FindLicenseProfessionalWithComponentName(capModel, ComponentName);
            LicenseProfessionalModel licenseProfessional = TempModelConvert.ConvertToLicenseProfessionalModel(licenseProfessional4WS);

            if (!LicenseUtil.ValidateRequiredField4SingleLicense(licenseProfessional, ModuleName, IsEditable, IsValidate))
            {
                MessageUtil.ShowMessageByControl(errorMessageLabel, MessageType.Error, GetTextByKey("aca_license_msg_required_validate"));
                txtValidateSectionRequired.ValidationByHiddenTextBox = true;
                txtValidateSectionRequired.CheckControlValueValidateFunction = ClientID + "_ValidateFieldRequired4License";
            }
        }

        /// <summary>
        /// Determines whether is reference data validate.
        /// </summary>
        /// <returns>Error Message</returns>
        private string IsReferenceDataValidate()
        {
            string errorMessage = string.Empty;

            if (!IsDataValid())
            {
               errorMessage = LabelUtil.GetTextByKey("per_license_error_searchClickedRequired", ModuleName);
            }

            return errorMessage;
        }

        /// <summary>
        /// Section required that no data input.
        /// </summary>
        private void SetSectionRequiredValidation()
        {
            if (IsEditable && IsSectionRequired && !divLicenseView.Visible)
            {
                txtValidateSectionRequired.ValidationByHiddenTextBox = true;
                txtValidateSectionRequired.CheckControlValueValidateFunction = ClientID + "_CheckSectionRequired4License";    
            }
        }

        /// <summary>
        /// Refresh license section.
        /// </summary>
        private void RefreshLicenseSection()
        {
            string postSourceID = Request.Form[Page.postEventSourceID];

            if ((LicenseEditPanel.UniqueID + REFRESH_LICENSE).Equals(postSourceID, StringComparison.InvariantCultureIgnoreCase))
            {
                string senderArgs = Request.Form[Page.postEventArgumentID];
                string[] paras = senderArgs.Split(ACAConstant.SPLIT_CHAR6);
                string componentName = paras[0];
                ActionType actionType = EnumUtil<ActionType>.Parse(paras[1]);
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                LicenseProfessionalModel4WS licenseProfessional = LicenseUtil.FindLicenseProfessionalWithComponentName(capModel, componentName);
                DisplayLicense(TempModelConvert.ConvertToLicenseProfessionalModel(licenseProfessional));

                // Show message
                ShowActionNoticeMessage(actionType);

                string focusId = IsMultipleLicensedProfessional ? Request.Form["__LASTFOCUS_ID"] : btnEdit.ClientID;
                Page.FocusElement(focusId);
            }
        }

        #endregion Methods
    }
}