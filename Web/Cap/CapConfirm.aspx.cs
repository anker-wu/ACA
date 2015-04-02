#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapConfirm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapConfirm.aspx.cs 279341 2014-10-20 07:25:40Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.Payment;
using Accela.ACA.Web.Util;
using Accela.ACA.Web.VO;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation cap confirm. 
    /// </summary>
    public partial class CapConfirm : BasePage
    {
        #region Fields

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapConfirm));
       
        /// <summary>
        /// hash table user controls.
        /// </summary>
        private Hashtable _htUserControls = new Hashtable();

        /// <summary>
        /// collection edit button of ASI, ASIT and license section.
        /// </summary>
        private List<AccelaButton> _editButtonList = new List<AccelaButton>();

        /// <summary>
        /// The ASI group and AppSpecificInfoGroupModel mapping which create the ASI control.
        /// </summary>
        private Dictionary<string, AppSpecificInfoGroupModel4WS[]> _dictASIGroupList = new Dictionary<string, AppSpecificInfoGroupModel4WS[]>();

        /// <summary>
        /// The ASIT group and AppSpecificTableGroupModel mapping which create the ASIT control.
        /// </summary>
        private Dictionary<string, AppSpecificTableGroupModel4WS[]> _dictASITGroupList = new Dictionary<string, AppSpecificTableGroupModel4WS[]>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the continue client ID.
        /// </summary>
        /// <value>
        /// The continue client ID.
        /// </value>
        protected string BtnContinueClientID
        {
            get
            {
                return actionBarTop.BtnContinueClientID;
            }
        }

        /// <summary>
        /// Gets or sets the button AppSpecInfo
        /// </summary>
        protected string BtnAppSpecInfo { get; set; }

        /// <summary>
        /// Gets or sets the button AppSpecTableInfo
        /// </summary>
        protected string BtnAppSpecTableInfo { get; set; }

        /// <summary>
        /// Gets or sets the button Valuation Calculator
        /// </summary>
        protected string BtnValuationCalculatorInfo { get; set; }

        /// <summary>
        /// Gets or sets the button AppSpec Info
        /// </summary>
        protected string BtnApplicantInfo { get; set; }

        /// <summary>
        /// Gets or sets the button contact1 Info
        /// </summary>
        protected string BtnContact1Info { get; set; }

        /// <summary>
        /// Gets or sets the button contact2 info
        /// </summary>
        protected string BtnContact2Info { get; set; }

        /// <summary>
        /// Gets or sets the button contact3 info
        /// </summary>
        protected string BtnContact3Info { get; set; }

        /// <summary>
        /// Gets or sets the button description info
        /// </summary>
        protected string BtnDescriptionInfo { get; set; }

        /// <summary>
        /// Gets or sets the button detail info
        /// </summary>
        protected string BtnDetailInfo { get; set; }

        /// <summary>
        /// Gets or sets the button license Info
        /// </summary>
        protected string BtnLicenseInfo { get; set; }

        /// <summary>
        /// Gets or sets the button owner info
        /// </summary>
        protected string BtnOwnerInfo { get; set; }

        /// <summary>
        /// Gets or sets the button parcel info.
        /// </summary>
        protected string BtnParcelInfo { get; set; }

        /// <summary>
        /// Gets or sets the Button MultiContacts Info
        /// </summary>
        protected string BtnMultiContactsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Button MultiLicenses Info
        /// </summary>
        protected string BtnMultiLicensesInfo { get; set; }

        /// <summary>
        /// Gets or sets the Button Education Info
        /// </summary>
        protected string BtnEducationInfo { get; set; }

        /// <summary>
        /// Gets or sets the Button Examination Info
        /// </summary>
        protected string BtnExaminationInfo { get; set; }

        /// <summary>
        /// Gets or sets the Button ContinuingEducation Info
        /// </summary>
        protected string BtnContinuingEducationInfo { get; set; }

        /// <summary>
        /// Gets or sets the Button Attachment Info.
        /// </summary>
        protected string BtnAttachmentInfo { get; set; }

        /// <summary>
        /// Gets or sets the button work location info.
        /// </summary>
        protected string BtnWorkLocationInfo { get; set; }

        /// <summary>
        /// Gets a value indicating whether current request come from shopping cart or not.
        /// </summary>
        private bool IsFromShoppingCart
        {
            get
            {
                return ACAConstant.COMMON_Y.Equals(Request.QueryString[ACAConstant.FROMSHOPPINGCART]);
            }
        }

        /// <summary>
        /// Gets Current CAP ID
        /// </summary>
        private CapIDModel4WS CurrentCapID
        {
            get
            {
                if (ViewState["CurrentCapID"] == null)
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    
                    if (capModel != null)
                    {
                        ViewState["CurrentCapID"] = capModel.capID;
                    }
                }

                return ViewState["CurrentCapID"] as CapIDModel4WS;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Displays the required contact type indicator.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="componentName">Name of the component.</param>
        /// <returns>The required contact type indicator.</returns>
        [WebMethod(Description = "Display Required Contact Type Indicator", EnableSession = true)]
        public static string DisplayRequiredContactTypeIndicator(string moduleName, string componentName)
        {
            return ContactUtil.DisplayRequiredContactTypeIndicator(moduleName, componentName);
        }

        /// <summary>
        /// <c>OnInit</c> page event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                //Clear UI data before initialize sections.
                UIModelUtil.ClearUIData();

                //Reset the IsPageFlowTraceUpdated flag.
                PageFlowUtil.IsPageFlowTraceUpdated = false;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            PageFlowGroupModel pageflowGroup = PageFlowUtil.GetPageflowGroup(capModel);
            Session.Remove(SessionConstant.SESSION_CREATE_RECORD_BY_MAP);

            /*
             * Update the component names for contact/LP records for the capModel if click Edit link for the partial record in Cart List 
             * or Click View link for partial record in Associated Form back to Review page.
             */
            capModel = PageFlowUtil.UpdateRecordDataForCapModel(capModel, pageflowGroup);

            /*
             * If we select LP before select cap type for clone cap, it's sequence number will be set to CapModel.licSeqNbr,
             * so we should clear it if we delete all LPs in spear form.
             */
            if (capModel.licSeqNbr != null && (capModel.licenseProfessionalList == null || capModel.licenseProfessionalList.Length == 0))
            {
                capModel.licSeqNbr = null;
            }

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                ExpressionUtil.RunExpressionForOnload(ModuleName);
            }

            BindPageflowGroup(capModel, pageflowGroup);

            if (!ValidationUtil.IsYes(pageflowGroup.displayReCertification))
            {
                placeDisclaimer.Visible = false;
            }

            base.OnInit(e);

            bool isFromAssoForm = Request.UrlReferrer != null
                && Request.UrlReferrer.AbsolutePath.EndsWith("AssociatedForms.aspx", StringComparison.OrdinalIgnoreCase);

            if (!IsPostBack && (IsFromShoppingCart || isFromAssoForm))
            {
                BreadCrumpUtil.RebuildBreadCrumb(pageflowGroup, this.ModuleName, false);
            }
        }

        /// <summary>
        /// Page load event method.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DialogUtil.RegisterScriptForDialog(Page);

            // Redirect error page when CAP Model in session is invalid
            if (CapUtil.IsCAPUpdatedInSession(CurrentCapID, true, ModuleName))
            {
                Logger.ErrorFormat(
                                    "CAP Model is updated. The original ID is {0}-{1}-{2}({3})",
                                    CurrentCapID.id1,
                                    CurrentCapID.id2,
                                    CurrentCapID.id3,
                                    CurrentCapID.serviceProviderCode);
                throw new InvalidOperationException(GetTextByKey("aca_cap_updated_error"));
            }

            actionBarTop.ContinueButtonClick += ContinueButton_Click;
            actionBarBottom.ContinueButtonClick += ContinueButton_Click;
            actionBarTop.PayAtCounterClick += PayAtCounterButton_Click;
            actionBarBottom.PayAtCounterClick += PayAtCounterButton_Click;

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    BreadCrumpToolBar.PageFlow = BreadCrumpUtil.GetPageFlowConfig(this.ModuleName, false);

                    var expressionResult = ExpressionUtil.GetExpressionResultFromSession();

                    if (expressionResult != null)
                    {
                        foreach (var exp in expressionResult)
                        {
                            ExpressionUtil.HandleASITExpressionResult(exp.Value.Key, exp.Value.Value, false);
                        }
                    }
                }

                if (!AppSession.IsAdmin && BizDomainConstant.Create_Application_Model.SaveDataInConfirmPageModel.Equals(StandardChoiceUtil.CreateApplicationModel(), StringComparison.OrdinalIgnoreCase))
                {
                    if (!SaveData())
                    {
                        actionBarTop.DisableButtonWhenSaveDataFail();
                        actionBarBottom.DisableButtonWhenSaveDataFail();
                    }

                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    var isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());

                    if (!CapUtil.HasFee(capModel.capID, false, isAssoFormEnabled) || CapUtil.IsSkipCapFeePage(capModel.capID, ModuleName))
                    {
                        divFeeList.Visible = false;
                    }
                    else
                    {
                        divFeeList.Visible = true;
                        capFeeList.ShowFeeInformation();
                        capFeeList.DisplayConditions();
                    }
                }
            }
            else if (Request["__EVENTTARGET"] == "SaveAndResume")
            {
                SaveAndResume();
            }
            else if (Request["__EVENTTARGET"] == "Continue")
            {
                ContinueButton_Click(null, null);
            }

            if (BizDomainConstant.Create_Application_Model.SaveDataInConfirmPageModel.Equals(StandardChoiceUtil.CreateApplicationModel(), StringComparison.OrdinalIgnoreCase)
                && !AppSession.User.IsAnonymous
                && StandardChoiceUtil.IsEnableDeferPayment()
                && divFeeList.Visible)
            {
                actionBarTop.DisplayPayAtCounterButton();
                actionBarBottom.DisplayPayAtCounterButton();
            }

            bool is4FeeEstimator = Request["isFeeEstimator"] == ACAConstant.COMMON_Y;

            actionBarTop.Is4FeeEstimator = is4FeeEstimator;
            actionBarBottom.Is4FeeEstimator = is4FeeEstimator;
        }

        /// <summary>
        /// handle AppSpecInfo cancel button event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AppSpecInfoButton_Click(object sender, EventArgs e)
        {
            AccelaButton btnEdit = sender as AccelaButton;

            if (btnEdit == null)
            {
                return;
            }

            BackToASIOrASITSection(btnEdit.CommandArgument, (long)PageFlowComponent.ASI);
        }

        /// <summary>
        /// handle AppSpecTable cancel button event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AppSpecTableInfoButton_Click(object sender, EventArgs e)
        {
            AccelaButton btnEdit = sender as AccelaButton;

            if (btnEdit == null)
            {
                return;
            }

            BackToASIOrASITSection(btnEdit.CommandArgument, (long)PageFlowComponent.ASI_TABLE);
        }

        /// <summary>
        /// handle the Edit button event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            AccelaLinkButton linkButton = sender as AccelaLinkButton;

            if (linkButton == null)
            {
                return;
            }

            string argument = linkButton.CommandArgument;
                
            if (!string.IsNullOrEmpty(argument))
            {
                long componentId = 0;
                long componentSeqNbr = 0;
                string[] strComponent = argument.Split(ACAConstant.SPLIT_CHAR);
                    
                if (strComponent.Length == 3 
                    && long.TryParse(strComponent[0], out componentId)
                    && long.TryParse(strComponent[1], out componentSeqNbr))
                {
                    string sectionName = strComponent[2];
                    BackToPageContainSection(componentId, componentSeqNbr, sectionName);
                }
            }
        }

        /// <summary>
        /// handle continue button event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            if (placeDisclaimer.Visible)
            {
                bool validateReCertificationSuccess = capReviewCertification1.Validate();

                if (!validateReCertificationSuccess)
                {
                    return;
                }
            }

            if (BizDomainConstant.Create_Application_Model.SaveDataInConfirmPageModel.Equals(StandardChoiceUtil.CreateApplicationModel(), StringComparison.OrdinalIgnoreCase))
            {
                ContinueToPayment();
                return;
            }

            if (!SaveData())
            {
                return;
            }

            Redirect();
        }

        /// <summary>
        /// Continue to edit application.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PayAtCounterButton_Click(object sender, EventArgs e)
        {
            capFeeList.PayAtCounter();
        }

        /// <summary>
        /// Continue to payment.
        /// </summary>
        private void ContinueToPayment()
        {
            capFeeList.ContinueToPayment();
        }

        /// <summary>
        /// Save cap model to DB.
        /// </summary>
        /// <returns>Indicating save success or not.</returns>
        private bool SaveData()
        {
            bool isSaveSuccessed = true;

            try
            {
                //setup education model into session.
                SetupEducationData();

                // setup continuing education models to session.
                SetupContEducationData();

                SetupExaminationData();

                // setup certification to session
                SetupCertificationData();

                CapModel4WS tempCapModel = AppSession.GetCapModelFromSession(this.ModuleName);

                //reset lps' sequence number.
                tempCapModel = ResetLPSeqNum(tempCapModel);

                //init Etisalat Online payment.
                EtisalatAdapter.InitEtisalatOnlinePayment();

                if (!CapUtil.ValidateLicenseProfessionType(tempCapModel.licenseProfessionalList))
                {
                    string errMsg = GetTextByKey("aca_cap_detail_validation_licensetype_msg", ModuleName);
                    HandleSubmitException(errMsg);
                    return false;
                }

                // validate list when some required field is empty.
                if (!CapUtil.ValidateRequiredFields4List(_htUserControls, tempCapModel, ModuleName))
                {
                    return false;
                }

                if (PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ATTACHMENT))
                {
                    IEDMSDocumentBll edmsDocBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                    CapIDModel capId = TempModelConvert.Trim4WSOfCapIDModel(tempCapModel.capID);
                    DocumentModel[] documentModels = null;

                    try
                    {
                        documentModels = edmsDocBll.GetRecordDocumentList(ConfigManager.AgencyCode, ModuleName, AppSession.User.PublicUserId, capId, true);
                    }
                    catch (ACAException ex)
                    {
                        Logger.Error(ex.Message);
                    }

                    if (!ValidateRequiredDocumentType(tempCapModel, documentModels))
                    {
                        return false;
                    }

                    ResetDocumentList(tempCapModel, documentModels);
                }

                SavePartialCap();
            }
            catch (ACAException ex)
            {
                HandleSubmitException(ex.Message);
                isSaveSuccessed = false;
            }

            return isSaveSuccessed;
        }

        /// <summary>
        /// Reset document list.
        /// </summary>
        /// <param name="capModel">Cap model</param>
        /// <param name="documentModels">Document models of the current record.</param>
        private void ResetDocumentList(CapModel4WS capModel, DocumentModel[] documentModels)
        {
            if (documentModels == null || !documentModels.Any())
            {
                return;
            }

            var peoples = GetPeoplesFromCurrentRecord(capModel);
            List<DocumentModel> needUpdateDocumentList = new List<DocumentModel>();

            foreach (DocumentModel item in documentModels)
            {
                /**
                 * if we delete the reference people in spear form after document attach to this people, 
                 * we need to update the document to remove the relationship of this people and document, 
                 * and this document only be a daily side document attached to the record.
                 */
                if ((item.sourceDocNbr == null || item.sourceDocNbr == 0)
                    && !string.IsNullOrEmpty(item.sourceEntityID)
                    && !string.IsNullOrEmpty(item.sourceEntityType)
                    && !peoples.Contains(string.Format("{0}{1}{2}", item.sourceEntityType, ACAConstant.SPLIT_CHAR4URL1, item.sourceEntityID)))   
                {
                    item.sourceRecfulnam = null;
                    item.sourceEntityType = null;
                    item.sourceEntityID = null;
                    item.sourceSpc = null;
                    item.fileOwnerPermission = null;    

                    needUpdateDocumentList.Add(item);
                }
            }

            if (needUpdateDocumentList.Count > 0)
            {
                IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                edmsBll.UpdateDocumentSourceInfo(needUpdateDocumentList.ToArray());    
            }
        }

        /// <summary>
        /// Get peoples from current record.
        /// </summary>
        /// <param name="capModel">Cap model</param>
        /// <returns>Return people's refNumber list of the current record.</returns>
        private List<string> GetPeoplesFromCurrentRecord(CapModel4WS capModel)
        {
            List<string> peoples = new List<string>();

            if (capModel != null && capModel.licenseProfessionalList != null)
            {
                var recordLPList = capModel.licenseProfessionalList.Where(lp => !string.IsNullOrEmpty(lp.licSeqNbr) && !ACAConstant.DAILY_LICENSE_NUMBER.Equals(lp.licSeqNbr, StringComparison.InvariantCulture));
                
                foreach (LicenseProfessionalModel4WS lp in recordLPList)
                {
                    string people = string.Format("{0}{1}{2}", DocumentEntityType.LP, ACAConstant.SPLIT_CHAR4URL1, lp.licSeqNbr);

                    if (!peoples.Contains(people))
                    {
                        peoples.Add(people);    
                    }
                }
            }

            if (capModel != null && capModel.contactsGroup != null)
            {
                foreach (var capContactModel in capModel.contactsGroup)
                {
                    string people = string.Format("{0}{1}{2}", DocumentEntityType.RefContact, ACAConstant.SPLIT_CHAR4URL1, capContactModel.refContactNumber);

                    if (!peoples.Contains(people))
                    {
                        peoples.Add(people);
                    }
                }
            }

            if (capModel != null
                && capModel.capContactModel != null
                && capModel.capContactModel.refContactNumber != null)
            {
                string people = string.Format("{0}{1}{2}", DocumentEntityType.RefContact, ACAConstant.SPLIT_CHAR4URL1, capModel.capContactModel.refContactNumber);

                if (!peoples.Contains(people))
                {
                    peoples.Add(people);    
                }
            }

            return peoples;
        }

        /// <summary>
        /// Validate required document type
        /// </summary>
        /// <param name="capModel">cap Model</param>
        /// <param name="documentModels">Document models of the current record.</param>
        /// <returns>If pass validate return true,else return false</returns>
        private bool ValidateRequiredDocumentType(CapModel4WS capModel, DocumentModel[] documentModels)
        {
            bool result = true;

            if (AttachmentUtil.IsDisabledEDMS(ConfigManager.AgencyCode, ModuleName))
            {
                return true;
            }

            if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ATTACHMENT))
            {
                return true;
            }

            if (capModel == null || capModel.capID == null)
            {
                return false;
            }
            
            Dictionary<string, string> requireDocumentTypes = CapUtil.GetRequiredDocumentTypeList(capModel.capType);

            if (requireDocumentTypes == null || requireDocumentTypes.Count == 0)
            {
                return true;
            }
            
            List<string> missRequiredDocumentTypes = CapUtil.GetMissRequiredDocumentTypes(requireDocumentTypes, documentModels);

            if (missRequiredDocumentTypes != null && missRequiredDocumentTypes.Count > 0)
            {
                List<string> errMsgs = new List<string>();
                errMsgs.Add(GetTextByKey("aca_capconfirm_required_document_msg_failed", ModuleName));
                errMsgs.AddRange(missRequiredDocumentTypes);

                HandleSubmitException(DataUtil.ConcatStringWithSplitChar(errMsgs, ACAConstant.HTML_BR));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Redirect to payment or cap fee page.
        /// </summary>
        private void Redirect()
        {
            CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(ModuleName);
            CapTypeModel parentCapType = AppSession.GetParentCapTypeFromSession();
            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(parentCapType);

            if (isAssoFormEnabled)
            {
                string url = CapUtil.GetAssoFormUrl();
                Response.Redirect(url);
            }
            else
            {
                CapUtil.ToPaymentApplication(capModel4WS, ModuleName, IsFromShoppingCart);
            }
        }

        /// <summary>
        /// Load sections according to page flow config
        /// </summary>
        /// <param name="capModel">cap model for ACA</param>
        /// <param name="pageflowGroup">PageFlowGroupModel model</param>
        private void BindPageflowGroup(CapModel4WS capModel, PageFlowGroupModel pageflowGroup)
        {
            if (capModel == null || capModel.capType == null)
            {
                return;
            }

            if (PageFlowUtil.IsPageFlowTraceUpdated || PageFlowUtil.IsPageflowChanged(capModel, ModuleName, pageflowGroup))
            {
                BreadCrumpToolBar.Enabled = false;

                bool pageFlowTraceUpdate = PageFlowUtil.IsPageFlowTraceUpdated;

                // The PageFlowUtil.IsPageFlowTraceUpdated is used in the function CapUtil.BuildRedirectUrl(), so the value should be set before the related function used.
                PageFlowUtil.IsPageFlowTraceUpdated = true;

                string url = CapUtil.BuildRedirectUrl(null, string.Empty, pageflowGroup, string.Empty);
                string message = string.Format(GetTextByKey("aca_capconfirm_msg_pageflowchange_notice"), url);
                MessageUtil.ShowMessage(Page, MessageType.Notice, message);

                if (!pageFlowTraceUpdate)
                {
                    PageFlowUtil.UpdatePageFlowTraceByPageFlowSetting(capModel, ModuleName, pageflowGroup);

                    PageFlowUtil.ResetPageTrace(capModel);
                    AppSession.SetPageflowGroupToSession(null);
                }
            }

            CapTypeModel capTypeModel = capModel.capType;

            lblPermitType.Text = CAPHelper.GetAliasOrCapTypeLabel(capTypeModel); //permittype;

            if (pageflowGroup == null || pageflowGroup.stepList == null || pageflowGroup.stepList.Length < 1)
            {
                return;
            }

            try
            {
                for (int step = 0; step < pageflowGroup.stepList.Length; step++)
                {
                    StepModel tmpStep = pageflowGroup.stepList[step];

                    for (int page = 0; page < tmpStep.pageList.Length; page++)
                    {
                        if (PageFlowUtil.IsHidden(capModel, step, page))
                        {
                            continue;
                        }

                        LoadPage(tmpStep.pageList[page], capModel);
                    }
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// load user control
        /// </summary>
        /// <param name="controls">a control collection that the user control added to </param>
        /// <param name="key">a unique key to identity the control in a hashtable</param>
        /// <param name="controlName">user control name</param>
        private void CreateControl(ControlCollection controls, string key, string controlName)
        {
            Control control = LoadControl(string.Format("../Component/{0}.ascx", controlName));
            controls.Add(control);
            _htUserControls.Add(key, control);
        }

        /// <summary>
        /// create section title label
        /// </summary>
        /// <param name="controls">a control collection that the label added to </param>
        /// <param name="labelKey">the key in UI DB that will load the text to the label</param>
        /// <param name="title">the text to the label</param>
        /// <param name="isEnableExpand">is enable expand or not.</param>
        /// <returns>string label id</returns>
        private string CreateLabel(ControlCollection controls, string labelKey, string title, bool isEnableExpand)
        {
            AccelaLabel label = new AccelaLabel();

            if (string.IsNullOrEmpty(title) || title.Trim() == string.Empty || AppSession.IsAdmin)
            {
                label.LabelKey = labelKey;
            }
            else
            {
                label.Text = title;
            }

            label.LabelType = LabelType.SectionTitleWithBar;
            label.ID = "lbl" + CommonUtil.GetRandomUniqueID().Substring(0, 8).Replace("-", string.Empty);
            label.CssClass = "ACA_FLeft";

            if (isEnableExpand)
            {
                label.Attributes.Add(ACAConstant.ENABLE_EXPAND, ACAConstant.COMMON_Y);
            }

            controls.Add(label);

            return label.ClientID;
        }

        /// <summary>
        /// create link button
        /// </summary>
        /// <param name="controls">a control collection that the link button added to</param>
        /// <param name="componentModel">the component model</param>
        /// <param name="sectionName">the section name</param>
        /// <returns>the client id of the button</returns>
        private string CreateLinkButton(ControlCollection controls, ComponentModel componentModel, string sectionName)
        {
            AccelaButton button = new AccelaButton();
            button.ID = string.Format("btn{0}Info", sectionName);
            button.LabelKey = "per_permitConfirm_label_editButton";
            button.DivEnableCss = "ACA_SmButton ACA_SmButton_FontSize ACA_Button_Text";
            button.CausesValidation = false;
            button.OnClientClick = "return ClientClick();";
            button.Click += BtnEdit_Click;
            button.CommandArgument = string.Format(
                                            "{1}{0}{2}{0}{3}", 
                                            ACAConstant.SPLIT_CHAR, 
                                            componentModel.componentID, 
                                            componentModel.componentSeqNbr, 
                                            sectionName);

            controls.Add(button);

            if (!_editButtonList.Contains(button))
            {
                _editButtonList.Add(button);
            }

            return button.ID;
        }

        /// <summary>
        /// create literal
        /// </summary>
        /// <param name="controls">a control collection that the literal added to </param>
        /// <param name="text">the text of the literal</param>
        private void CreateLiteral(ControlCollection controls, string text)
        {
            Literal literal = new Literal();
            literal.Text = text;
            controls.Add(literal);
        }

        /// <summary>
        /// create section
        /// </summary>
        /// <param name="componentModel">the component model</param>
        /// <param name="sectionName">section name</param>
        /// <param name="controlName">user control name</param>
        /// <param name="titleKey">the key of UI DB to load title for section,it is the default tittle if the parameter of <paramref name="title"/> is empty</param>
        /// <param name="title">the section title</param>
        /// <param name="isViewPage">True indicating that section's file name end up with "xxxView.ascx"; otherwise "xxxEdit.ascx"</param>
        /// <returns>the edit button client ID</returns>
        private string CreateSection(ComponentModel componentModel, string sectionName, string controlName, string titleKey, string title, bool isViewPage)
        {
            return CreateSection(componentModel, sectionName, controlName, titleKey, title, null, isViewPage);
        }

        /// <summary>
        /// create section
        /// </summary>
        /// <param name="componentModel">the component model</param>
        /// <param name="sectionName">section name</param>
        /// <param name="controlName">user control name</param>
        /// <param name="titleKey">the key of UI DB to load title for section,it is the default tittle if the parameter of <paramref name="title"/> is empty</param>
        /// <param name="title">the section title</param>
        /// <param name="sectionInstruction">section instructions</param>
        /// <param name="isViewPage">True indicating that section's file name end up with "xxxView.ascx"; otherwise "xxxEdit.ascx"</param>
        /// <returns>the edit button client ID</returns>
        private string CreateSection(ComponentModel componentModel, string sectionName, string controlName, string titleKey, string title, string sectionInstruction, bool isViewPage)
        {
            bool isEnableExpandReiewSection = StandardChoiceUtil.IsEnableExpandReviewSection(ConfigManager.AgencyCode);

            if (isEnableExpandReiewSection)
            {
                CreateLiteral(placeHolder.Controls, @"<div><div class=""ACA_Title_Bar""> <table role='presentation' width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td>");

                string strExpandId = "divExpand" + sectionName;
                string strImg = "img" + sectionName;
                string strLnk = "lnk" + sectionName;
                string strTitle = GetTitleByKey("img_alt_expand_icon", titleKey);
                string strAlt = GetTextByKey("img_alt_expand_icon");
                string strSrc = ImageUtil.GetImageURL("section_header_collapsed.gif");
                string expandScript = "ControlSectionDisplay('" + strExpandId + "','" + strImg + "','" + strLnk + "',null, '" + componentModel.componentName.ToLower() + "');";
                RegisterExpandScript(titleKey, "Expand_" + sectionName, expandScript);

                CreateLiteral(placeHolder.Controls, string.Format(@"<a href=""#"" class=""NotShowLoading ACA_FLeft"" onclick='ControlSectionDisplay(""{0}"",""{1}"",""{2}"",null,""{3}"")' title=""{4}"" id=""{5}"">", strExpandId, strImg, strLnk, componentModel.componentName.ToLower(), strTitle, strLnk));
                CreateLiteral(placeHolder.Controls, string.Format(@"<img class=""ExpandImg"" alt=""{0}"" src=""{1}"" id=""{2}"" /></a><h1 class=""ACA_FLeft"">", strAlt, strSrc, strImg));
            }
            else
            {
                CreateLiteral(placeHolder.Controls, @"<div><div class=""ACA_Title_Bar""> <table role='presentation' width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td><h1>");
            }

            string labelId = CreateLabel(placeHolder.Controls, titleKey, title, isEnableExpandReiewSection);

            CreateLiteral(placeHolder.Controls, string.Format(@"</h1></td><td><div class=""ACA_FRight""><table role='presentation'><tr><td><div class=""ACA_Title_Button"" id=""divEdit{0}"">", sectionName));

            string editButtonClientID = string.Empty;

            if (!sectionName.StartsWith(PageFlowConstant.SECTION_NAME_ASI) && !sectionName.StartsWith(PageFlowConstant.SECTION_NAME_ASIT))
            {
                string tempControlId = CreateLinkButton(placeHolder.Controls, componentModel, sectionName);
                editButtonClientID = placeHolder.FindControl(tempControlId).ClientID;
            }

            CreateLiteral(placeHolder.Controls, @"</div></td><td><div class=""ACA_Title_Button"">&nbsp;&nbsp;&nbsp;</div></td></tr></table></div></td></tr></table></div>");
            string instruction = !string.IsNullOrEmpty(sectionInstruction) ? sectionInstruction : GetTextByKey(titleKey + "|sub");
            string instructionCls = string.IsNullOrEmpty(instruction) ? "ACA_Hide" : "ACA_Section_Instruction ACA_Section_Instruction_FontSize";
            CreateLiteral(placeHolder.Controls, string.Format(@"<div id=""{0}_sub_label"" class=""{1}"" >{2}</div></div>", labelId, instructionCls, ScriptFilter.FilterScript(instruction, false)));

            CreateUpdatePanel(placeHolder.Controls, sectionName, controlName, string.Format("UpdatePanel{0}", sectionName), isViewPage);

            return editButtonClientID;
        }

        /// <summary>
        /// Register expand section title script.
        /// </summary>
        /// <param name="labelKey">the label key.</param>
        /// <param name="scriptKey">the script key.</param>
        /// <param name="script">the script.</param>
        private void RegisterExpandScript(string labelKey, string scriptKey, string script)
        {
            if (!AppSession.IsAdmin)
            {
                var isExpanded = StandardChoiceUtil.IsEnableForData4AsKey(XPolicyConstant.IS_EXPANDED_SECTION, ModuleName, labelKey);

                if (isExpanded && !string.IsNullOrEmpty(scriptKey))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), scriptKey, script, true);
                }
            }
        }

        /// <summary>
        /// create update panel
        /// </summary>
        /// <param name="controls">a control collection that the UpdatePanel added to </param>
        /// <param name="sectionName">section name</param>
        /// <param name="controlName">user control name</param>
        /// <param name="id">UpdatePanel's id</param>
        /// <param name="isViewPage">true indicating that create those which panel's file name end up with "xxxView.ascx"; "xxxEdit.ascx" otherwise</param>
        private void CreateUpdatePanel(ControlCollection controls, string sectionName, string controlName, string id, bool isViewPage)
        {
            UpdatePanel panel = new UpdatePanel();
            panel.ID = id;
            panel.UpdateMode = UpdatePanelUpdateMode.Conditional;
            string strExpandId = "divExpand" + sectionName;

            if (isViewPage)
            {
                sectionName += "View";
                controlName += "View";
            }
            else
            {
                sectionName += "Edit";
                controlName += "Edit";
            }

            bool isEnableExpandReiewSection = StandardChoiceUtil.IsEnableExpandReviewSection(ConfigManager.AgencyCode);

            if (isEnableExpandReiewSection)
            {
                CreateLiteral(panel.ContentTemplateContainer.Controls, string.Format("<div style=\"display: none; \" id=\"{0}\"><div id=\"div{1}\" class=\"section_body\">", strExpandId, sectionName));
                CreateControl(panel.ContentTemplateContainer.Controls, sectionName, controlName);
                CreateLiteral(panel.ContentTemplateContainer.Controls, "</div></div>");
            }
            else
            {
                CreateLiteral(panel.ContentTemplateContainer.Controls, string.Format("<div id=\"div{0}\" class=\"section_body\">", sectionName));
                CreateControl(panel.ContentTemplateContainer.Controls, sectionName, controlName);
                CreateLiteral(panel.ContentTemplateContainer.Controls, "</div>");
            }

            controls.Add(panel);
            panel.Visible = AppSession.IsAdmin ? false : true;
            _htUserControls.Add(id, panel);
        }

        /// <summary>
        /// Display Application Specific info
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        /// <param name="groups">AppSpecificInfoGroupModel4WS array.</param>
        /// <param name="componentSeqNbr">The component sequence number.</param>
        private void DisplayAppSpec(string sectionName, AppSpecificInfoGroupModel4WS[] groups, long componentSeqNbr)
        {
            string viewName = string.Format("{0}View", sectionName);
            AppSpecInfoView view = (AppSpecInfoView)_htUserControls[viewName];

            if (view != null)
            {
                string editBtnClientClick = string.Format("return ClientClick(this,'{0}');", sectionName);

                view.Display(groups, editBtnClientClick, AppSpecInfoButton_Click, sectionName, componentSeqNbr);
            }
        }

        /// <summary>
        /// Display AppSpec Table
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="appSpecTableGroups">The app spec table groups.</param>
        /// <param name="componentSeqNbr">The component sequence number.</param>
        private void DisplayAppSpecTable(string sectionName, AppSpecificTableGroupModel4WS[] appSpecTableGroups, long componentSeqNbr)
        {
            string viewName = string.Format("{0}View", sectionName);
            AppSpecInfoTableView view = (AppSpecInfoTableView)_htUserControls[viewName];

            if (view != null)
            {
                string editBtnClientClick = string.Format("return ClientClick(this,'{0}');", sectionName);
                view.Display(appSpecTableGroups, editBtnClientClick, AppSpecTableInfoButton_Click, sectionName, componentSeqNbr);
            }
        }

        /// <summary>
        /// display applicant information in cap confirm page.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="applicantModel">applicant model</param>
        private void DisplayApplicant(ComponentModel component, CapContactModel4WS applicantModel)
        {
            string key = string.Format("{0}_{1}View", PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX, component.componentSeqNbr);
            ContactView view = (ContactView)_htUserControls[key];
            view.IsInConfirmPage = true;

            view.Display(applicantModel);
        }

        /// <summary>
        /// display four contacts information in cap confirm page.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model information</param>
        /// <param name="sectionName">The section name</param>
        private void DisplayContractor(ComponentModel component, CapModel4WS capModel, string sectionName)
        {
            string key = string.Format("{0}View", sectionName);
            ContactView view = (ContactView)_htUserControls[key];
            view.IsInConfirmPage = true;

            CapContactModel4WS contactModel = ContactUtil.FindContactWithComponentName(capModel.contactsGroup, sectionName);

            if (contactModel != null)
            {
                contactModel.validateFlag = component.validateFlag;
            }

            view.Display(contactModel);
        }

        /// <summary>
        /// display additional information in aca cap confirm page.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model information</param>
        /// <param name="sectionName">The section name.</param>
        private void DisplayDescription(ComponentModel component, CapModel4WS capModel, string sectionName)
        {
            string key = string.Format("{0}View", sectionName);
            CapDescriptionView additionalView = (CapDescriptionView)_htUserControls[key];

            // convert BO to VO for presentation
            AddtionalInfo addtionalInfo = CapUtil.BuildAddtionalInfo(capModel);

            additionalView.Display(addtionalInfo);
        }

        /// <summary>
        /// display detail information in aca cap confirm page.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model information</param>
        /// <param name="sectionName">The section name.</param>
        private void DisplayDetailInfo(ComponentModel component, CapModel4WS capModel, string sectionName)
        {
            string key = string.Format("{0}View", sectionName);
            DetailInfoView detailView = (DetailInfoView)_htUserControls[key];

            // convert BO to VO for presentation
            AddtionalInfo addtionalInfo = CapUtil.BuildAddtionalInfo(capModel);

            detailView.Display(addtionalInfo);
        }
        
        /// <summary>
        /// create work location section and initial section's data.
        /// </summary>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model information</param>
        private void InitAddress(string value, ComponentModel component, CapModel4WS capModel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_ADDRESS;
            BtnWorkLocationInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_ADDRESS, "per_permitConfirm_label_workLocation", value, true);

            string key = string.Format("{0}View", sectionName);
            AddressView view = (AddressView)_htUserControls[key];

            if (!IsPostBack)
            {
                view.IsInConfirmPage = true;
                view.Display(capModel.addressModel);
            }
        }

        /// <summary>
        /// Initial the assets.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="value">The value.</param>
        /// <param name="capModel">The cap model.</param>
        private void InitAssets(ComponentModel component, string value, CapModel4WS capModel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_ASSETS;
            CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_ASSETS, "aca_capconfirm_label_assettitle", value, false);

            string key = string.Format("{0}Edit", sectionName);
            AssetListEdit assetView = (AssetListEdit)_htUserControls[key];

            if (!IsPostBack)
            {
                assetView.ConfirmOrDetailPage = ACAConstant.ASSETLIST_LAYOUT_CAP_CONFIRM;
                if (capModel != null && capModel.assetList != null)
                {
                    assetView.Display(capModel.assetList.ToList());
                }
            }
        }

        /// <summary>
        /// Initial the ASI section.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="asiGroupModelList">The AppSpecificInfo Group Model list.</param>
        private void InitAppSpec(ComponentModel component, AppSpecificInfoGroupModel4WS[] asiGroupModelList)
        {
            if (asiGroupModelList == null || asiGroupModelList.Length == 0 || !ASIBaseUC.ExistsASIFields(asiGroupModelList))
            {
                return;
            }

            // set ASI control and display
            string groupInstruction = string.Empty;

            if (!CapUtil.IsSuperCAP(ModuleName))
            {
                groupInstruction = I18nStringUtil.GetCurrentLanguageString(asiGroupModelList[0].resGroupInstruction, asiGroupModelList[0].groupInstruction);
            }

            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading);
            string sectionName = CapUtil.GetSectionName4ASI(component, asiGroupModelList);
            _dictASIGroupList.Add(sectionName, asiGroupModelList);

            BtnAppSpecInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_ASI, "per_permitConfirm_label_appSpecInfo", customHeadingText, groupInstruction, true);

            DisplayAppSpec(sectionName, asiGroupModelList, component.componentSeqNbr);
        }

        /// <summary>
        /// Initialize the application specific table.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="asitGroupModelList">The asit group model list.</param>
        private void InitAppSpecTable(ComponentModel component, AppSpecificTableGroupModel4WS[] asitGroupModelList)
        {
            if (asitGroupModelList == null || asitGroupModelList.Length == 0 || !ASIBaseUC.ExistsASITFields(asitGroupModelList))
            {
                return;
            }

            string groupInstruction = string.Empty;

            if (!CapUtil.IsSuperCAP(ModuleName))
            {
                groupInstruction = I18nStringUtil.GetCurrentLanguageString(asitGroupModelList[0].resInstruction, asitGroupModelList[0].instruction);
            }

            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading);
            string sectionName = CapUtil.GetSectionName4ASIT(component, asitGroupModelList);
            _dictASITGroupList.Add(sectionName, asitGroupModelList);

            BtnAppSpecTableInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_ASIT, "per_permitConfirm_label_appSpecInfoTable", customHeadingText, groupInstruction, true);

            DisplayAppSpecTable(sectionName, asitGroupModelList, component.componentSeqNbr);
        }

        /// <summary>
        /// create applicant section and initial section's data.
        /// </summary>
        /// <param name="customHeadingText">the text showed in the section title bar</param>
        /// <param name="component">component model</param>
        /// <param name="applicantModel">contact model for applicant</param>
        private void InitApplicant(string customHeadingText, ComponentModel component, CapContactModel4WS applicantModel)
        {
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX, component.componentSeqNbr);
            BtnApplicantInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_CONTACT, "per_permitconfirm_label_multi_contact", customHeadingText, true);

            if (applicantModel != null)
            {
                applicantModel.validateFlag = component.validateFlag;
            }

            DisplayApplicant(component, applicantModel);
        }

        /// <summary>
        /// Creates the attachment section with the specified title and the specified content.
        /// </summary>
        /// <param name="title">The title of the section to create.</param>
        /// <param name="component">The content of the section to create.</param>
        /// <param name="isForConditionDocument">Is for condition document or not.</param>
        private void InitAttachment(string title, ComponentModel component, bool isForConditionDocument)
        {
            if (isForConditionDocument)
            {
                BtnAttachmentInfo = CreateSection(component, PageFlowConstant.SECTION_NAME_CONDITIONDOCUMENT, PageFlowConstant.CONTROL_NAME_ATTACHMENT, "aca_requireddocument_label_documenttitle", title, false);

                string key = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_CONDITIONDOCUMENT);
                AttachmentEdit attachmentEdit = (AttachmentEdit)_htUserControls[key];
                attachmentEdit.IsForConditionDocument = true;
                attachmentEdit.IsInConfirmPage = true;
            }
            else
            {
                string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_ATTACHMENT_PREFIX, component.componentSeqNbr);
                BtnAttachmentInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_ATTACHMENT, "per_attachment_Label_attachTitle", title, false);

                string key = string.Format("{0}Edit", sectionName);
                AttachmentEdit attachmentEdit = (AttachmentEdit)_htUserControls[key];
                attachmentEdit.ComponentName = sectionName;

                /*
                 * Make sure every instance of AttachmentEdit control has its own id which will be used 
                 * (1) to distinguish each other since there may be multiple instances in the same page.
                 * (2) when receiving the update request made by JavaScript in an iframe, 
                 *     in order to update the content of a specific attachment list, the back-end C# code needs a way to figure out which iframe made that request. 
                 */
                if (string.IsNullOrEmpty(attachmentEdit.ID))
                {
                    attachmentEdit.ID = sectionName;
                }

                attachmentEdit.IsInConfirmPage = true;
            }
        }

        /// <summary>
        /// create education section and initial section's data
        /// </summary>
        /// <param name="component">Component model</param>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="capModel">education model list</param>
        private void InitEducation(ComponentModel component, string value, CapModel4WS capModel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_EDUCATION;
            BtnEducationInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_EDUCATION, "per_confirm_education_section_name", value, false);

            string key = string.Format("{0}Edit", sectionName);
            EducationEdit edit = (EducationEdit)_htUserControls[key];
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.EducationSectionPosition = EducationOrExamSectionPosition.CapConfirm;

            if (edit.IsEditable)
            {
                edit.SetSectionRequired(CapUtil.Convert2BooleanValue(component.requiredFlag));
            }

            if (capModel.educationList == null)
            {
                IEducationBll educationBLL = (IEducationBll)ObjectFactory.GetObject(typeof(IEducationBll));
                RefEducationModel4WS[] refEducations = educationBLL.GetRefEducationModelsByCapType(capModel.capType);
                capModel.educationList = educationBLL.ConvertRefEducations2Educations(refEducations, AppSession.User.PublicUserId);
            }

            edit.EducationsChanged += EducationSaved;

            if (!IsPostBack)
            {
                edit.DisplayEducations(capModel.educationList);
            }
        }

        /// <summary>
        /// triggered after education saved
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the education models.</param>
        private void EducationSaved(object sender, CommonEventArgs arg)
        {
            if (arg != null)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null)
                {
                    capModel.educationList = (EducationModel4WS[])arg.ArgObject;
                }
            }
        }

        /// <summary>
        /// create examination section and initial section's data.
        /// </summary>
        /// <param name="component">Component model</param>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="capModel">examination model list</param>
        private void InitExamination(ComponentModel component, string value, CapModel4WS capModel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_EXAMINATION;
            BtnExaminationInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_EXAMINATION, "examination_confirm_section_name", value, false);

            string key = string.Format("{0}Edit", sectionName);
            ExaminationEdit edit = (ExaminationEdit)_htUserControls[key];
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ExaminationSectionPosition = EducationOrExamSectionPosition.CapConfirm;

            if (edit.IsEditable)
            {
                edit.IsRequired = CapUtil.Convert2BooleanValue(component.requiredFlag);
            }

            if (capModel.examinationList == null)
            {
                IRefExaminationBll refExaminationBLL = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
                RefExaminationModel4WS[] refExamination = refExaminationBLL.GetRefExaminationList(capModel.capType);

                capModel.examinationList = ExaminationUtil.ConvertRefExaminationsToExaminations(refExamination);
            }

            edit.DataChanged += ExaminationChanged;

            if (!IsPostBack)
            {
                edit.DisplayExamination(capModel.examinationList);
            }
        }

        /// <summary>
        /// triggered after education saved
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the education models.</param>
        private void ExaminationChanged(object sender, CommonEventArgs arg)
        {
            if (arg != null && arg.ArgObject != null)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null)
                {
                    capModel.examinationList = (ExaminationModel[])arg.ArgObject;
                }
            }
        }

        /// <summary>
        /// Create valuation calculator section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="capModel">The cap model</param>
        private void InitValuationCalculator(ComponentModel component, string value, CapModel4WS capModel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_VALUATION_CALCULATOR;
            BtnValuationCalculatorInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_VALUATION_CALCULATOR, "valuationcalculator_confirm_section_name", value, true);
            if (!AppSession.IsAdmin)
            {
                string key = string.Format("{0}View", sectionName);
                ValuationCalculatorView view = (ValuationCalculatorView)_htUserControls[key];

                //Get reference valuation calculator and make data source setting            
                IValuationCalculatorBll valcalBLL = (IValuationCalculatorBll)ObjectFactory.GetObject(typeof(IValuationCalculatorBll));
                BCalcValuatnModel4WS[] calvals = valcalBLL.GetBCalcValuationListByCapType(capModel.capType, capModel.capID);

                if (capModel.bCalcValuationListField == null)
                {
                    capModel.bCalcValuationListField = calvals;
                }

                if (!IsPostBack)
                {
                    view.DisplayValuationCalculator(capModel.bCalcValuationListField);
                }
            }
        }

        /// <summary>
        /// Create continuing education section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="value">section title</param>
        /// <param name="capModel">cap model contain continuing education models</param>
        private void InitContEducation(ComponentModel component, string value, CapModel4WS capModel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_CONT_EDUCATION;
            BtnContinuingEducationInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_CONT_EDUCATION, "per_permitconfirm_label_continuing_education", value, false);

            string key = string.Format("{0}Edit", sectionName);
            ContinuingEducationEdit contEducationEdit = (ContinuingEducationEdit)_htUserControls[key];
            contEducationEdit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            contEducationEdit.ContEducationSectionPosition = EducationOrExamSectionPosition.CapConfirm;

            if (contEducationEdit.IsEditable)
            {
                contEducationEdit.SetContEducationSectionRequired(CapUtil.Convert2BooleanValue(component.requiredFlag));
            }

            if (capModel.contEducationList == null)
            {
                IRefContinuingEducationBll refContEducationBll = (IRefContinuingEducationBll)ObjectFactory.GetObject(typeof(IRefContinuingEducationBll));
                RefContinuingEducationModel4WS[] refContEducations = refContEducationBll.GetRefContEducationsByCapType(capModel.capType);
                capModel.contEducationList = EducationUtil.ConvertRefContEducations2ContEducations(refContEducations, ACAConstant.COMMON_Y);
            }

            contEducationEdit.ContEducationsChanged += SaveContEducation;

            if (!IsPostBack)
            {
                contEducationEdit.DislayContEducations(capModel.contEducationList);
            }
        }

        /// <summary>
        /// Triggered this method after education saved
        /// </summary>
        /// <param name="sender">an object that contains the event sender.</param>
        /// <param name="arg">a CommonEventArgs object containing the education models.</param>
        private void SaveContEducation(object sender, CommonEventArgs arg)
        {
            if (arg != null)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null)
                {
                    capModel.contEducationList = (ContinuingEducationModel4WS[])arg.ArgObject;
                }
            }
        }

        /// <summary>
        /// create Multiple Contacts section and initial section's data
        /// </summary>
        /// <param name="barTitle">the text showed in the section title bar</param>
        /// <param name="component">the component model</param>
        /// <param name="capModel">cap model value</param>
        private void InitMultipleContacts(string barTitle, ComponentModel component, CapModel4WS capModel)
        {
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX, component.componentSeqNbr);
            BtnMultiContactsInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_MULTI_CONTACTS, "per_permitconfirm_label_multi_contact", barTitle, false);

            string key = string.Format("{0}Edit", sectionName);
            MultiContactsEdit edit = (MultiContactsEdit)_htUserControls[key];
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ComponentName = sectionName;

            if (edit.IsEditable)
            {
                edit.SetSectionRequired(CapUtil.Convert2BooleanValue(component.requiredFlag));
            }

            edit.ValidateFlag = component.validateFlag;
            edit.IsInConfirmPage = true;

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                IList<CapContactModel4WS> contactsGroupList = new List<CapContactModel4WS>();

                /* When rendering the content of ContactList control, we have to remove those items which do not belong to the ContactList.
                 * It means an item which belongs to Applicant, Contact1, Contact2, or Contact3 will be removed.
                 */
                if (capModel.contactsGroup != null && capModel.contactsGroup.Length != 0)
                {
                    foreach (CapContactModel4WS contact in capModel.contactsGroup)
                    {
                        if (contact == null || !sectionName.Equals(contact.componentName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        
                        contact.validateFlag = component.validateFlag;
                        contactsGroupList.Add(contact);
                    }

                    //Initial RowIndex for contact records in Contact List section.
                    for (int index = 0; index < contactsGroupList.Count; index++)
                    {
                        contactsGroupList[index].people.RowIndex = index;
                    }
                }

                edit.DisplayContacts(contactsGroupList.ToArray());
            }

            //AddOwnerToContactAutoFillDropdownList(edit);
        }

        /// <summary>
        /// create multiple licenses section and initial section's data
        /// </summary>
        /// <param name="component">Component Model4WS</param>
        /// <param name="barTitle">the text showed in the section title bar</param>
        /// <param name="capModel">CapModel4WS object</param>
        private void InitMultipleLicenses(ComponentModel component, string barTitle, CapModel4WS capModel)
        {
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_LICENSE_PREFIX, component.componentSeqNbr);
            BtnMultiLicensesInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_MULTI_LICENSES, "per_permitconfirm_label_multi_license", barTitle, false);

            string key = string.Format("{0}Edit", sectionName);
            MultiLicensesEdit edit = (MultiLicensesEdit)_htUserControls[key];
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ComponentName = sectionName;

            if (edit.IsEditable)
            {
                edit.SetSectionRequired(CapUtil.Convert2BooleanValue(component.requiredFlag));
            }

            edit.ValidateFlag = component.validateFlag;
            edit.IsInConfirmPage = true;

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                /* When rendering the content of LicenseProfessionalList control, we have to remove those items which do not belong to it.
                 * It means an item which belongs to License Professional will be removed.
                 */
                LicenseProfessionalModel4WS[] lpBelongToLPList = LicenseUtil.FindLicenseProfessionalsWithComponentName(capModel, sectionName);
                string serviceProviderCode = capModel.capID != null ? capModel.capID.serviceProviderCode : string.Empty;
                LicenseProfessionalModel[] licensees = LicenseUtil.ResetLicenseeAgency(lpBelongToLPList, serviceProviderCode);

                edit.DisplayLicenses(licensees);
            }
        }

        /// <summary>
        /// create the first contact section and initial section's data
        /// </summary>
        /// <param name="customHeadingText">the text showed in the section title bar</param>
        /// <param name="component">component model</param>
        /// <param name="capModel">the cap model</param>
        private void InitContractor1(string customHeadingText, ComponentModel component, CapModel4WS capModel)
        {
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT1_PREFIX, component.componentSeqNbr);
            BtnContact1Info = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_CONTACT, "per_permitconfirm_label_multi_contact", customHeadingText, true);

            DisplayContractor(component, capModel, sectionName);
        }

        /// <summary>
        /// create the second contact section and initial section's data
        /// </summary>
        /// <param name="customHeadingText">the text showed in the section title bar</param>
        /// <param name="component">component model</param>
        /// <param name="capModel">the cap model</param>
        private void InitContractor2(string customHeadingText, ComponentModel component, CapModel4WS capModel)
        {
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT2_PREFIX, component.componentSeqNbr);
            BtnContact2Info = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_CONTACT, "per_permitconfirm_label_multi_contact", customHeadingText, true);

            DisplayContractor(component, capModel, sectionName);
        }

        /// <summary>
        /// create the third contact section and initial section's data.
        /// </summary>
        /// <param name="customHeadingText">the text showed in the section title bar</param>
        /// <param name="component">component model</param>
        /// <param name="capModel">the cap model</param>
        private void InitContractor3(string customHeadingText, ComponentModel component, CapModel4WS capModel)
        {
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT3_PREFIX, component.componentSeqNbr);
            BtnContact3Info = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_CONTACT, "per_permitconfirm_label_multi_contact", customHeadingText, true);

            DisplayContractor(component, capModel, sectionName);
        }

        /// <summary>
        /// create Additional Info section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="capModel">cap model information</param>
        private void InitAdditionalInfo(ComponentModel component, string value, CapModel4WS capModel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_ADDITIONAL_INFO;
            BtnDescriptionInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_ADDITIONAL_INFO, "per_permitConfirm_label_description", value, true);

            DisplayDescription(component, capModel, sectionName);
        }

        /// <summary>
        /// create detail information section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="capModel">cap model information</param>
        private void InitDetailInfo(ComponentModel component, string value, CapModel4WS capModel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_DETAIL_INFO;
            BtnDetailInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_DETAIL_INFO, "per_permitReg_label_detailinfo", value, true);

            DisplayDetailInfo(component, capModel, sectionName);
        }

        /// <summary>
        /// create license professional section and initial section's data
        /// </summary>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model information</param>
        private void InitLicense(string value, ComponentModel component, CapModel4WS capModel)
        {
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_LICENSE_PREFIX, component.componentSeqNbr);
            BtnLicenseInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_LICENSE, "per_permitconfirm_label_multi_license", value, true);

            string key = string.Format("{0}View", sectionName);
            LicenseView view = (LicenseView)_htUserControls[key];

            if (!IsPostBack)
            {
                if (capModel.licenseProfessionalList == null)
                {
                    return;
                }

                LicenseProfessionalModel4WS[] licenseProfessionalList = capModel.licenseProfessionalList;
                string serviceProviderCode = null;
                if (capModel.capID != null)
                {
                    serviceProviderCode = capModel.capID.serviceProviderCode;
                }

                LicenseUtil.ResetLicenseeAgency(licenseProfessionalList, serviceProviderCode);

                // Find the approprate license from the list of license professional list of capModel
                LicenseProfessionalModel4WS licenseProfessional = null;

                foreach (var lp in licenseProfessionalList)
                {
                    if (!string.IsNullOrEmpty(lp.componentName) && lp.componentName.Equals(sectionName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        licenseProfessional = lp;

                        if (string.IsNullOrEmpty(licenseProfessional.TemporaryID))
                        {
                            licenseProfessional.TemporaryID = CommonUtil.GetRandomUniqueID();
                        }

                        break;
                    }
                }

                view.IsInConfirmPage = true;
                view.Display(TempModelConvert.ConvertToLicenseProfessionalModel(licenseProfessional));
            }
        }

        /// <summary>
        /// create owner section and initial section's data
        /// </summary>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="component">component model</param>
        /// <param name="owner">owner model</param>
        private void InitOwner(string value, ComponentModel component, RefOwnerModel owner)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_OWNER;
            BtnOwnerInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_OWNER, "per_permitConfirm_label_owner", value, true);

            string key = string.Format("{0}View", sectionName);
            OwnerView view = (OwnerView)_htUserControls[key];

            if (!IsPostBack)
            {
                view.DisplayOwner(owner);
            }
        }

        /// <summary>
        /// create parcel section and initial section's data.
        /// </summary>
        /// <param name="value">the text showed in the section title bar</param>
        /// <param name="component">component model</param>
        /// <param name="parcel">parcel model</param>
        private void InitParcel(string value, ComponentModel component, CapParcelModel parcel)
        {
            string sectionName = PageFlowConstant.SECTION_NAME_PARCEL;
            BtnParcelInfo = CreateSection(component, sectionName, PageFlowConstant.CONTROL_NAME_PARCEL, "per_permitConfirm_label_parcel", value, true);

            string key = string.Format("{0}View", sectionName);
            ParcelView view = (ParcelView)_htUserControls[key];

            if (!IsPostBack)
            {
                view.DisplayParcel(parcel);
            }
        }

        /// <summary>
        /// Load Page event method.
        /// </summary>
        /// <param name="page">page model.</param>
        /// <param name="capModel">The cap model .</param>
        private void LoadPage(PageModel page, CapModel4WS capModel)
        {
            CapUtil.SetLPTemporaryID(capModel);

            // get the ASI component list
            List<ComponentModel> asiComponentList = new List<ComponentModel>();
            List<ComponentModel> asitComponentList = new List<ComponentModel>();

            // get the ASI/ASIT component list
            foreach (ComponentModel component in page.componentList)
            {
                if (component.componentName.ToUpperInvariant() == GViewConstant.SECTION_ASI)
                {
                    asiComponentList.Add(component);
                }
                else if (component.componentName.ToUpperInvariant() == GViewConstant.SECTION_ASIT)
                {
                    asitComponentList.Add(component);
                }
            }

            PageFlowGroupModel pfGroupModel = PageFlowUtil.GetPageflowGroup(capModel);
            List<string> displayedASI = new List<string>();

            foreach (ComponentModel component in page.componentList)
            {
                string customHeadingKey = component.customHeading;
                string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading);
                string asiKey = string.Format("{0}_{1}_{2}", component.componentID, component.portletRange1, component.customHeading);

                switch (component.componentName.ToUpperInvariant())
                {
                    case GViewConstant.SECTION_ADDITIONAL_INFO:
                        InitAdditionalInfo(component, customHeadingText, capModel);
                        break;
                    case GViewConstant.SECTION_ASI:
                        if (!displayedASI.Contains(asiKey))
                        {
                            AppSpecificInfoGroupModel4WS[] asiGroupModelList = CapUtil.GetASIGroupModelList(ModuleName, component, asiComponentList, pfGroupModel);
                            InitAppSpec(component, asiGroupModelList);

                            // add the displayed ASI to list
                            displayedASI.Add(asiKey);
                        }

                        break;
                    case GViewConstant.SECTION_ASIT:
                        if (!displayedASI.Contains(asiKey))
                        {
                            AppSpecificTableGroupModel4WS[] asitGroupModelList = CapUtil.GetASITGroupModelList(ModuleName, component, asitComponentList, pfGroupModel);
                            InitAppSpecTable(component, asitGroupModelList);

                            // add the displayed ASIT to list
                            displayedASI.Add(asiKey);
                        }

                        break;
                    case GViewConstant.SECTION_LICENSE:
                        InitLicense(customHeadingText, component, capModel);
                        break;
                    case GViewConstant.SECTION_ADDRESS:
                        InitAddress(customHeadingText, component, capModel);
                        break;
                    case GViewConstant.SECTION_PARCEL:
                        InitParcel(customHeadingText, component, capModel.parcelModel);
                        break;
                    case GViewConstant.SECTION_OWNER:
                        InitOwner(customHeadingText, component, capModel.ownerModel);
                        break;
                    case GViewConstant.SECTION_APPLICANT:
                        customHeadingText = StandardChoiceUtil.GetContactTypeByKey(customHeadingKey);

                        string componentName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX, component.componentSeqNbr);
                        CapContactModel4WS applicant = ContactUtil.FindContactWithComponentName(capModel.contactsGroup, componentName);
                        
                        InitApplicant(customHeadingText, component, applicant);
                        break;
                    case GViewConstant.SECTION_CONTACT1:
                        customHeadingText = StandardChoiceUtil.GetContactTypeByKey(customHeadingKey);
                        InitContractor1(customHeadingText, component, capModel);
                        break;
                    case GViewConstant.SECTION_CONTACT2:
                        customHeadingText = StandardChoiceUtil.GetContactTypeByKey(customHeadingKey);
                        InitContractor2(customHeadingText, component, capModel);
                        break;
                    case GViewConstant.SECTION_CONTACT3:
                        customHeadingText = StandardChoiceUtil.GetContactTypeByKey(customHeadingKey);
                        InitContractor3(customHeadingText, component, capModel);
                        break;
                    case GViewConstant.SECTION_DETAIL:
                        InitDetailInfo(component, customHeadingText, capModel);
                        break;
                    case GViewConstant.SECTION_MULTIPLE_CONTACTS:
                        InitMultipleContacts(customHeadingText, component, capModel);
                        break;
                    case GViewConstant.SECTION_ATTACHMENT:
                        InitAttachment(customHeadingText, component, false);
                        break;
                    case GViewConstant.SECTION_CONDITION_DOCUMENT:
                        InitAttachment(customHeadingText, component, true);
                        break;
                    case GViewConstant.SECTION_EDUCATOIN:
                        InitEducation(component, customHeadingText, capModel);
                        break;
                    case GViewConstant.SECTION_CONTINUING_EDUCATION:
                        InitContEducation(component, customHeadingText, capModel);
                        break;
                    case GViewConstant.SECTION_EXAMINATION:
                        InitExamination(component, customHeadingText, capModel);
                        break;
                    case GViewConstant.SECTION_MULTIPLE_LICENSES:
                        InitMultipleLicenses(component, customHeadingText, capModel);
                        break;
                    case GViewConstant.SECTION_VALUATION_CALCULATOR:
                        InitValuationCalculator(component, customHeadingText, capModel);
                        break;
                    case GViewConstant.SECTION_ASSETS:
                        InitAssets(component, customHeadingText, capModel);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// save the cap when "pressing the save and resume later" button
        /// </summary>
        private void SaveAndResume()
        {
            try
            {
                //set up Education data to CAP model and set it to session.
                SetupEducationData();

                // setup continuing education models to session.
                SetupContEducationData();

                SetupExaminationData();

                CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(this.ModuleName);
                capModel4WS = CapUtil.ConstructCapModel(capModel4WS, this.ModuleName, Request["isRenewal"]);

                if (capModel4WS.capClass == ACAConstant.INCOMPLETE_TEMP_CAP)
                {
                    capModel4WS.capClass = ACAConstant.INCOMPLETE_CAP;
                }

                capModel4WS = UpdatePartialCapModelWrapper(capModel4WS);

                EmseUtil.TriggerEMSESaveAndResume(capModel4WS);

                CapUtil.SaveResumeRedirect(Response, ModuleName, capModel4WS.capID, Request["isRenewal"]);
            }
            catch (ACAException err)
            {
                HandleSubmitException(err.Message);
            }
        }

        /// <summary>
        /// setup education models to session.
        /// </summary>
        private void SetupEducationData()
        {
            CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(this.ModuleName);
            CapModel4WS cloneCapModel = (CapModel4WS)capModel4WS.Clone();

            string key = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EDUCATION);
            if (_htUserControls.ContainsKey(key))
            {
                EducationEdit educationEdit = (EducationEdit)_htUserControls[key];
                cloneCapModel.educationList = educationEdit.GetEducationModelList();
            }

            AppSession.SetCapModelToSession(ModuleName, cloneCapModel);
        }

        /// <summary>
        /// Setup continuing education models to session.
        /// </summary>
        private void SetupContEducationData()
        {
            CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(this.ModuleName);
            CapModel4WS cloneCapModel = (CapModel4WS)capModel4WS.Clone();

            string key = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_CONT_EDUCATION);

            if (_htUserControls.ContainsKey(key))
            {
                ContinuingEducationEdit contEducationEdit = (ContinuingEducationEdit)_htUserControls[key];
                cloneCapModel.contEducationList = contEducationEdit.GetContEducations();
            }

            AppSession.SetCapModelToSession(ModuleName, cloneCapModel);
        }

        /// <summary>
        /// setup examination models to session.
        /// </summary>
        private void SetupExaminationData()
        {
            CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(this.ModuleName);
            CapModel4WS cloneCapModel = (CapModel4WS)capModel4WS.Clone();

            string key = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EXAMINATION);

            if (_htUserControls.ContainsKey(key))
            {
                ExaminationEdit examinationEdit = (ExaminationEdit)_htUserControls[key];

                cloneCapModel.examinationList = (examinationEdit.DataSource as List<ExaminationModel>).ToArray();
            }

            AppSession.SetCapModelToSession(ModuleName, cloneCapModel);
        }

        /// <summary>
        /// setup certification checked data to session
        /// </summary>
        private void SetupCertificationData()
        {
            if (placeDisclaimer.Visible)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                CapModel4WS cloneCapModel = (CapModel4WS)capModel.Clone();

                cloneCapModel.certificationApplied = ACAConstant.COMMON_Y;
                cloneCapModel.certificationDate = capReviewCertification1.GetCertificationDateValue();

                AppSession.SetCapModelToSession(ModuleName, cloneCapModel);
            }
        }

        /// <summary>
        /// Save Partial Cap
        /// </summary>
        private void SavePartialCap()
        {
            CapModel4WS feeEstimateModel = AppSession.GetCapModelFromSession(this.ModuleName);
            CapModel4WS capModel4WS = CapUtil.ConstructCapModel(feeEstimateModel, this.ModuleName, Request["isRenewal"]);

            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            PeopleUtil.SetSearchAndSyncFlag4Contact(capModel4WS, ModuleName, false);

            if (CapUtil.IsSuperCAP(ModuleName))
            {
                CapUtil.FilterSameLicenseType(capModel4WS);
                capModel4WS = capBll.SavePartialCaps(capModel4WS);
            }
            else
            {
                capModel4WS = capBll.SaveWrapperForPartialCap(capModel4WS, string.Empty, false);
            }

            //Due to the licenseProfessionalModel has value from biz server, this value NOT to use for ACA. 
            capModel4WS.licenseProfessionalModel = null;

            PageFlowUtil.UpdatePageTrace(capModel4WS);

            CapUtil.CopyTemplateValueToCapModel(feeEstimateModel, capModel4WS);
            CapUtil.CopyASILayoutToCapModel(feeEstimateModel, capModel4WS);
            AppSession.SetCapModelToSession(ModuleName, capModel4WS);
        }

        /// <summary>
        /// update partial cap model to DB
        /// </summary>
        /// <param name="capModel4WS">cap model for ACA.</param>
        /// <returns>a CapModel4WS</returns>
        private CapModel4WS UpdatePartialCapModelWrapper(CapModel4WS capModel4WS)
        {
            PeopleUtil.SetSearchAndSyncFlag4Contact(capModel4WS, ModuleName, true);

            // update the partical cap model.
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            CapModel4WS model;

            if (CapUtil.IsSuperCAP(ModuleName))
            {
                CapUtil.FilterSameLicenseType(capModel4WS);
                model = capBll.UpdatePartialCaps(capModel4WS);
            }
            else
            {
                string agencyCode = capModel4WS.capID.serviceProviderCode;

                model = capBll.UpdatePartialCapModelWrapper(agencyCode, capModel4WS, AppSession.User.PublicUserId);
            }

            CapUtil.CopyTemplateValueToCapModel(capModel4WS, model);

            return model;
        }

        /// <summary>
        /// reset the LP sequence number with reference LP's sequence number which have same LP type and LP number.
        /// </summary>
        /// <param name="cap">cap model.</param>
        /// <returns>cap model with correct LP sequence number</returns>
        private CapModel4WS ResetLPSeqNum(CapModel4WS cap)
        {
            if (cap.licenseProfessionalModel == null && (cap.licenseProfessionalList == null || cap.licenseProfessionalList.Length <= 0))
            {
                return cap;
            }

            if (cap.licenseProfessionalModel != null)
            {
                LicenseProfessionalModel lpModel = ResetOneLPSeqNum(TempModelConvert.ConvertToLicenseProfessionalModel(cap.licenseProfessionalModel));

                cap.licenseProfessionalModel = TempModelConvert.ConvertToLicenseProfessionalModel4WS(lpModel);
            }

            if (cap.licenseProfessionalList != null && cap.licenseProfessionalList.Length > 0)
            {
                for (int i = 0; i < cap.licenseProfessionalList.Length; i++)
                {
                    LicenseProfessionalModel lpModel = TempModelConvert.ConvertToLicenseProfessionalModel(cap.licenseProfessionalList[i]);

                    cap.licenseProfessionalList[i] = TempModelConvert.ConvertToLicenseProfessionalModel4WS(ResetOneLPSeqNum(lpModel));
                }
            }

            return cap;
        }

        /// <summary>
        /// reset one LP sequence number with reference LP's sequence number which have same LP type and LP number.
        /// </summary>
        /// <param name="licenseProfessional">License Professional Model4WS.</param>
        /// <returns>cLicense Professional Model4WS with correct LP sequence number</returns>
        private LicenseProfessionalModel ResetOneLPSeqNum(LicenseProfessionalModel licenseProfessional)
        {
            if (licenseProfessional == null || string.IsNullOrEmpty(licenseProfessional.licenseNbr) || string.IsNullOrEmpty(licenseProfessional.licenseType))
            {
                return licenseProfessional;
            }

            LicenseModel4WS searchLicense = new LicenseModel4WS();
            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));

            searchLicense.licenseType = licenseProfessional.licenseType;
            searchLicense.stateLicense = licenseProfessional.licenseNbr;
            searchLicense.serviceProviderCode =
            string.IsNullOrEmpty(licenseProfessional.agencyCode) ? ConfigManager.AgencyCode : licenseProfessional.agencyCode;
            LicenseModel4WS resultLicensee = licenseBll.GetLicenseByStateLicNbr(searchLicense);

            if (resultLicensee == null)
            {
                licenseProfessional.licSeqNbr = string.Empty;
            }
            else
            {
                licenseProfessional.licSeqNbr = resultLicensee.licSeqNbr;
            }

            return licenseProfessional;
        }

        /// <summary>
        /// Handle Exception/Error when the current page is submit
        /// </summary>
        /// <param name="message">message for showing error.</param>
        private void HandleSubmitException(string message)
        {
            // 1. Show Error Message
            MessageUtil.ShowMessage(Page, MessageType.Error, message);

            //Handle the Exception during the child CAP creations
            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            if (isAssoFormEnabled && !CapUtil.IsAssoFormChild(ModuleName))
            {
                HandleChildCapCreateException();
            }
        }

        /// <summary>
        /// Handle the Exception during the child CAP creations.
        /// Get related child CAP IDs from the ASIT entries and set to the current CapModel.
        /// </summary>
        private void HandleChildCapCreateException()
        {
            IAppSpecificInfoBll asiBll = (IAppSpecificInfoBll)ObjectFactory.GetObject(typeof(IAppSpecificInfoBll));

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            if (capModel == null)
            {
                return;
            }

            try
            {
                AppSpecificTableGroupModel4WS asitFromBll = asiBll.GetAppSpecificTableGroupModelByCapID(CurrentCapID, AppSession.User.PublicUserId);
                AppSpecificTableGroupModel4WS asitFromCap = (capModel.appSpecTableGroups != null && capModel.appSpecTableGroups.Length > 0) ? capModel.appSpecTableGroups[0] : null;

                if (asitFromBll == null || asitFromCap == null ||
                    asitFromBll.tablesMapValues == null || asitFromCap.tablesMapValues == null)
                {
                    return;
                }

                string fieldName4RelatedCapID = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_ACA_CONFIGS, BizDomainConstant.ASSOCIATED_FORMS_ASIT_RELATIONSHIP_FIELD);
                if (string.IsNullOrEmpty(fieldName4RelatedCapID))
                {
                    return;
                }

                //Put all existing ASI Table(Sub Group) to hash table, tableName as key.
                Hashtable tablesFromBll = new Hashtable();
                foreach (AppSpecificTableModel4WS asiTable in asitFromBll.tablesMapValues)
                {
                    tablesFromBll.Add(asiTable.tableName, asiTable);
                }

                for (int i = 0; i < asitFromCap.tablesMapValues.Length; i++)
                {
                    AppSpecificTableModel4WS tableD = asitFromCap.tablesMapValues[i];
                    AppSpecificTableModel4WS tableS;

                    //Find ASI Table from hash table by the table name.
                    if (tablesFromBll.ContainsKey(tableD.tableName))
                    {
                        tableS = (AppSpecificTableModel4WS)tablesFromBll[tableD.tableName];
                    }
                    else
                    {
                        continue;
                    }

                    if (tableD.tableField == null || tableS.tableField == null)
                    {
                        continue;
                    }

                    //Put all existing ASIT field to hash table, row index + column Name as key.
                    Hashtable fieldsFromBll = new Hashtable();
                    for (int j = 0; j < tableS.tableField.Length; j++)
                    {
                        int colIdx = j % tableS.columns.Length;
                        int rowIdx = (j - colIdx) / tableS.columns.Length;
                        fieldsFromBll.Add(rowIdx + tableS.columns[colIdx].columnName, tableS.tableField[j]);
                    }

                    for (int j = 0; j < tableD.tableField.Length; j++)
                    {
                        int colIdx = j % tableD.columns.Length;
                        int rowIdx = (j - colIdx) / tableD.columns.Length;

                        AppSpecificTableColumnModel4WS colD = tableD.columns[colIdx];
                        AppSpecificTableField4WS fieldS;
                        if (fieldsFromBll.ContainsKey(rowIdx + colD.columnName))
                        {
                            fieldS = (AppSpecificTableField4WS)fieldsFromBll[rowIdx + colD.columnName];

                            if (colD.columnName.Equals(fieldName4RelatedCapID, StringComparison.InvariantCulture) &&
                                fieldS.inputValue != null)
                            {
                                tableD.tableField[j].inputValue = fieldS.inputValue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// back to the spear form page which contain the section
        /// </summary>
        /// <param name="componentId">the component id</param>
        /// <param name="componentSeqNbr">The component sequence number.</param>
        /// <param name="updatePanelName">update panel name for the section position</param>
        private void BackToPageContainSection(long componentId, long componentSeqNbr, string updatePanelName)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            PageFlowGroupModel pageflowGroup = PageFlowUtil.GetPageflowGroup(capModel);
            ComponentModel searchCptModel = new ComponentModel
                                                {
                                                    componentID = componentId,
                                                    componentSeqNbr = componentSeqNbr
                                                };

            CapUtil.BackToPageContainSection(searchCptModel, updatePanelName, pageflowGroup, string.Empty);
        }

        /// <summary>
        /// Back to the spear form page which contain the section for ASI or ASIT
        /// </summary>
        /// <param name="argument">the argument contain the <c>componentSeqNbr</c> and sectionName, split by '/f'</param>
        /// <param name="componentId">the component id</param>
        private void BackToASIOrASITSection(string argument, long componentId)
        {
            if (string.IsNullOrEmpty(argument))
            {
                return;
            }

            string[] strComponent = argument.Split(ACAConstant.SPLIT_CHAR);
            long componentSeqNbr = 0;

            if (strComponent.Length == 2 && long.TryParse(strComponent[0], out componentSeqNbr))
            {
                if (componentSeqNbr > 0)
                {
                    string sectionName = strComponent[1];
                    BackToPageContainSection(componentId, componentSeqNbr, sectionName);
                }
            }
        }

        #endregion Methods
    }
}
