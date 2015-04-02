#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapEdit.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapEdit.aspx.cs 278418 2014-09-03 08:54:25Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.People;
using Accela.ACA.Web.Util;
using Accela.ACA.Web.VO;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.UI;
using Accela.ACA.WSProxy.WSModel;
using Accela.Web.Controls;
using log4net;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation CapEdit. 
    /// </summary>
    public partial class CapEdit : BasePage
    {
        #region Fields

        /// <summary>
        /// The customize component control's key in <c>_htUserControls</c>
        /// </summary>
        private const string CUSTOMIZE_COMPONENT = "CustomizeComponent";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapEdit));

        /// <summary>
        /// current page
        /// </summary>
        private int _currentPage = 0;

        /// <summary>
        /// current step.
        /// </summary>
        private int _currentStep = 0;

        /// <summary>
        /// step number
        /// </summary>
        private int _stepNumber = 0;

        /// <summary>
        /// page number
        /// </summary>
        private int _pageNumber = 0;

        /// <summary>
        /// emse error message.
        /// </summary>
        private string _emseErrorMsg = string.Empty;

        /// <summary>
        /// current page model.
        /// </summary>
        private PageModel _currentPageModel = null;

        /// <summary>
        /// hash table user controls.
        /// </summary>
        private Hashtable _htUserControls = new Hashtable();

        /// <summary>
        /// Is for fee estimator.
        /// </summary>
        private bool _is4FeeEstimator;

        /// <summary>
        /// Is APO locked.
        /// </summary>
        private bool _isAPOLocked = false;

        /// <summary>
        /// Is redirect to fee page
        /// </summary>
        private bool _isRedirectToFeePage;

        /// <summary>
        /// Is from the confirm page
        /// </summary>
        private bool _isFromConfirmPage;

        /// <summary>
        /// Is from the shopping cart
        /// </summary>
        private bool _isFromShoppingCart;

        /// <summary>
        /// The is amendment
        /// </summary>
        private bool _isAmendment;

        /// <summary>
        /// Is Single contact valid
        /// </summary>
        private bool _isSingleContactValid = true;

        /// <summary>
        /// The ASI group and AppSpecificInfoGroupModel mapping which create the ASI control.
        /// </summary>
        private Dictionary<string, AppSpecificInfoGroupModel4WS[]> _dictASIGroupList = new Dictionary<string, AppSpecificInfoGroupModel4WS[]>();

        /// <summary>
        /// The ASIT group and AppSpecificTableGroupModel mapping which create the ASIT control.
        /// </summary>
        private Dictionary<string, AppSpecificTableGroupModel4WS[]> _dictASITGroupList = new Dictionary<string, AppSpecificTableGroupModel4WS[]>();

        /// <summary>
        /// The contact type for contact component. key: component key, value: contact type
        /// </summary>
        private Dictionary<string, string> _dictComponentContactTypesList = new Dictionary<string, string>();

        /// <summary>
        /// The Applicant component. key: component key, value: component name store in DB
        /// </summary>
        private Dictionary<string, string> _dictKeysAndNamesForMultipleApplicant = new Dictionary<string, string>();
        
        /// <summary>
        /// The Contact1 component. key: component key, value: component name store in DB 
        /// </summary>
        private Dictionary<string, string> _dictKeysAndNamesForMultipleContact1 = new Dictionary<string, string>();

        /// <summary>
        /// The Contact2 component. key: component key, value: component name store in DB 
        /// </summary>
        private Dictionary<string, string> _dictKeysAndNamesForMultipleContact2 = new Dictionary<string, string>();

        /// <summary>
        /// The Contact3 component. key: component key, value: component name store in DB 
        /// </summary>
        private Dictionary<string, string> _dictKeysAndNamesForMultipleContact3 = new Dictionary<string, string>();

        /// <summary>
        /// The Contact List component. key: component key, value: component name store in DB 
        /// </summary>
        private Dictionary<string, string> _dictKeysAndNamesForMultipleContactList = new Dictionary<string, string>();

        /// <summary>
        /// The LicenseProfessional component. key for component key used in <c>_htUserControls</c>; value for component name stored in DB.
        /// </summary>
        private Dictionary<string, string> _dictKeysAndNamesForMultipleLP = new Dictionary<string, string>();

        /// <summary>
        /// The License Professional List component. key for component key used in <c>_htUserControls</c>; value for component name stored in DB.
        /// </summary>
        private Dictionary<string, string> _dictKeysAndNamesForLPList = new Dictionary<string, string>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether from the authorized agent or not.
        /// </summary>
        protected bool IsFromAuthAgentPage
        {
            get
            {
                return AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether parcel section exists.
        /// </summary>
        protected bool IsParcelSectionExists { get; set; }

        /// <summary>
        /// Gets or sets Source Sequence Number.
        /// </summary>
        private long? SourceSequenceNumber
        {
            get
            {
                if (ViewState["SourceSequenceNumber"] != null)
                {
                    return (long?)ViewState["SourceSequenceNumber"];
                }

                return 0;
            }

            set
            {
                ViewState["SourceSequenceNumber"] = value;
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
                    
                    ViewState["CurrentCapID"] = capModel.capID;
                }

                return ViewState["CurrentCapID"] as CapIDModel4WS;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactAddressIndex">Index of the contact address.</param>
        /// <param name="processType">Type of the process.</param>
        /// <param name="callbackName">Name of the callback.</param>
        /// <param name="parameterString">The parameter string.</param>
        [WebMethod(Description = "Operation Contact Address Session", EnableSession = true)]
        public static void OperationContactAddressSession(string moduleName, string contactAddressIndex, string processType, string callbackName, string parameterString)
        {
            ContactSessionParameter sessionParameter = null;

            if (string.IsNullOrEmpty(parameterString))
            {
                sessionParameter = AppSession.GetContactSessionParameter();
            }
            else
            {
                sessionParameter = new JavaScriptSerializer().Deserialize<ContactSessionParameter>(parameterString);
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
                CapContactModel4WS capContact = capModel.contactsGroup.FirstOrDefault(o => o.componentName.Equals(sessionParameter.PageFlowComponent.ComponentName));
                sessionParameter.Data.DataObject = capContact;
            }

            if (!string.IsNullOrEmpty(processType))
            {
                sessionParameter.Process.ContactAddressProcessType = EnumUtil<ContactAddressProcessType>.Parse(processType);
            }

            if (!string.IsNullOrEmpty(contactAddressIndex))
            {
                sessionParameter.Data.ContactAddressRowIndex = int.Parse(contactAddressIndex);
            }
            else
            {
                sessionParameter.Data.ContactAddressRowIndex = null;
            }

            sessionParameter.Process.CACallbackFunctionName = callbackName;
            AppSession.SetContactSessionParameter(sessionParameter);
        }

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactIndex">Index of the contact.</param>
        /// <param name="contactAddressIndex">Index of the contact address.</param>
        /// <param name="processType">Type of the process.</param>
        /// <param name="parameterString">The parameter string.</param>
        /// <returns>contact type is null</returns>
        [WebMethod(Description = "Creates the contact parameters session", EnableSession = true)]
        public static bool CreateContactParametersSession(string moduleName, string contactIndex, string contactAddressIndex, string processType, string parameterString)
        {
            bool contactTypeNullOrDisabled = false;
            ContactSessionParameter sessionParameter = new JavaScriptSerializer().Deserialize<ContactSessionParameter>(parameterString);

            if (!string.IsNullOrEmpty(processType))
            {
                sessionParameter.Process.ContactProcessType = EnumUtil<ContactProcessType>.Parse(processType);
            }

            CapContactModel4WS capContact = null;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            if (sessionParameter.Process.ContactProcessType == ContactProcessType.Edit)
            {
                /* For Contact Form, get contact by component name.
                 * For Contact List, get contact by component name and row index.
                 */
                capContact = capModel.contactsGroup.FirstOrDefault(o => o.componentName.Equals(sessionParameter.PageFlowComponent.ComponentName)
                            && (string.IsNullOrEmpty(contactIndex) || o.people.RowIndex == int.Parse(contactIndex)));

                if (capContact != null && capContact.people != null)
                {
                    contactTypeNullOrDisabled = string.IsNullOrEmpty(capContact.people.contactType) || !ContactUtil.IsContactTypeEnable(capContact.people.contactType, ContactTypeSource.Transaction, moduleName, capContact.people.serviceProviderCode);
                }
            }
            else
            {
                capContact = new CapContactModel4WS
                    {
                        componentName = sessionParameter.PageFlowComponent.ComponentName,
                        people = new PeopleModel4WS { contactType = sessionParameter.ContactType }
                    };
            }

            sessionParameter.Data.DataObject = capContact;

            AppSession.SetContactSessionParameter(sessionParameter);
            return contactTypeNullOrDisabled;
        }

        /// <summary>
        /// Displays the required contact type indicator.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="componentName">Name of the component.</param>
        /// <returns>A html string indicating the required contact type.</returns>
        [WebMethod(Description = "Display Required Contact Type Indicator", EnableSession = true)]
        public static string DisplayRequiredContactTypeIndicator(string moduleName, string componentName)
        {
            return ContactUtil.DisplayRequiredContactTypeIndicator(moduleName, componentName);
        }

        /// <summary>
        /// Get Owner Condition
        /// </summary>
        /// <param name="value">value string. <c>OwnerItem|sourceSeqNumber|ownerNumber|ownerUID</c></param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>String of Owner Condition</returns>
        [System.Web.Services.WebMethod(Description = "GetOwnerCondition", EnableSession = true)]
        public static string GetOwnerCondition(string value, string moduleName)
        {
            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(value);

                if (parameter != null && parameter.AutoFillType != ACAConstant.AutoFillType4SpearForm.None)
                {
                    StringBuilder sb = new StringBuilder();

                    //Parameters: vs[1] source sequence number, vs[2] reference owner id, vs[3] external unique id
                    sb = ConditionsUtil.GetOwnerCondition(parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName);

                    if (sb != null)
                    {
                        returnValue = sb.ToString();
                    }
                }
            }

            return returnValue;
        }
        
        /// <summary>
        /// Get Address Condition
        /// </summary>
        /// <param name="value">value string. <c>AddressItem|sourceSeqNumber|refAddressID|addressUID</c></param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>String of Address Condition</returns>
        [System.Web.Services.WebMethod(Description = "GetAddressCondition", EnableSession = true)]
        public static string GetAddressCondition(string value, string moduleName)
        {
            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(value);

                if (parameter != null && parameter.AutoFillType != ACAConstant.AutoFillType4SpearForm.None)
                {
                    StringBuilder sb = new StringBuilder();

                    //Parameters: vs[1] source sequence number; vs[2] reference address id; vs[3] external unique id
                    sb = ConditionsUtil.GetAddressConditions(parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName);

                    if (sb != null)
                    {
                        returnValue = sb.ToString();
                    }
                }
            }

            return returnValue;
        }
        
        /// <summary>
        /// Get Parcel Condition
        /// </summary>
        /// <param name="value">value string. <c>"ParcelItem|SourceSeqNumber|ParcelNumber|UID"</c></param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>Owner Condition</returns>
        [System.Web.Services.WebMethod(Description = "GetParcelCondition", EnableSession = true)]
        public static string GetParcelCondition(string value, string moduleName)
        {
            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(value);

                if (parameter != null && parameter.AutoFillType != ACAConstant.AutoFillType4SpearForm.None)
                {
                    StringBuilder sb = new StringBuilder();

                    //Parameters: vs[1] source sequence number, vs[2] reference parcel id, vs[3] external unique id
                    sb = ConditionsUtil.GetParcelConditions(parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName);

                    if (sb != null)
                    {
                        returnValue = sb.ToString();
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Get Owner Condition Message
        /// </summary>
        /// <param name="value">value string. <c>"OwnerItem|sourceSeqNumber|ownerNumber|ownerUID"</c></param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>Owner Condition Message</returns>
        [System.Web.Services.WebMethod(Description = "GetOwnerCondtionMessage", EnableSession = true)]
        public static string GetOwnerCondtionMessage(string value, string moduleName)
        {
            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(value);

                if (parameter != null && parameter.AutoFillType != ACAConstant.AutoFillType4SpearForm.None)
                {
                    StringBuilder sb = new StringBuilder();

                    //Parameters: vs[1] source sequence number, vs[2] reference owner id, vs[3] external unique id
                    sb = ConditionsUtil.GetOwnerCondtionMessage(parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName);

                    if (sb != null)
                    {
                        returnValue = sb.ToString();
                    }
                }
            }

            return returnValue;           
        }
                
        /// <summary>
        /// Get Address Condition Message
        /// </summary>
        /// <param name="value">value string. <c>"AddressItem|sourceSeqNumber|refAddressID|addressUID"</c></param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>Address Condition Message</returns>
        [System.Web.Services.WebMethod(Description = "GetAddressCondtionMessage", EnableSession = true)]
        public static string GetAddressCondtionMessage(string value, string moduleName)
        {
            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(value);

                if (parameter != null && parameter.AutoFillType != ACAConstant.AutoFillType4SpearForm.None)
                {
                    //Parameters: vs[1] source sequence number, vs[2] reference address id, vs[3] external unique id
                    returnValue = ConditionsUtil.GetAddressCondtionMessage(parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Get Parcel Condition Message
        /// </summary>
        /// <param name="value">value string. <c>"ParcelItem|SourceSeqNumber|ParcelNumber|UID"</c></param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>Parcel Condition Message</returns>
        [System.Web.Services.WebMethod(Description = "GetParcelCondtionMessage", EnableSession = true)]
        public static string GetParcelCondtionMessage(string value, string moduleName)
        {
            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(value);

                if (parameter != null && parameter.AutoFillType != ACAConstant.AutoFillType4SpearForm.None)
                {
                    //Parameters: vs[1] source sequence number, vs[2] reference parcel id, vs[3] external unique id
                    returnValue = ConditionsUtil.GetParcelCondtionMessage(parameter.EntityId, parameter.EntityType, parameter.EntityRefId, moduleName);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// build contact information with JSON format to client
        /// </summary>
        /// <param name="autoFillValue">value string.</param>
        /// <param name="moduleName">the module name.</param>
        /// <param name="agencyCode">The agencyCode</param>
        /// <param name="isFromAuthAgent">Is from authorize agent or not.</param>
        /// <returns>Public User Model</returns>
        [System.Web.Services.WebMethod(Description = "GetPublicUserModel", EnableSession = true)]
        public static string GetPublicUserModel(string autoFillValue, string moduleName, string agencyCode, string isFromAuthAgent)
        {
            return CapUtil.GetPublicUserModel(autoFillValue, moduleName, agencyCode, ValidationUtil.IsTrue(isFromAuthAgent));
        }

        /// <summary>
        /// save condition document to EDMS.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="conditionNum">condition number</param>
        /// <param name="clientId">client Id</param>
        /// <param name="fileInfo">user file information</param>
        /// <returns>Return load document result.</returns>
        [System.Web.Services.WebMethod(Description = "SaveConditionDocToEDMS", EnableSession = true)]
        public static string SaveConditionDocToEDMS(string moduleName, string conditionNum, string clientId, string fileInfo)
        {
            long conditionNumber = long.Parse(conditionNum);
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            var result = new { Success = false, Messages = string.Empty, ClientId = clientId };

            try
            {
                FileUploadInfo fileUploadInfo = jsSerializer.Deserialize<FileUploadInfo>(fileInfo);
                fileUploadInfo.DocumentModel = AttachmentUtil.ConstructDocumentModel(moduleName, fileUploadInfo, conditionNumber);
                List<FileUploadInfo> fileUploadInfos = new List<FileUploadInfo> { fileUploadInfo };
                AttachmentUtil.DeleteConditionDocument(moduleName, conditionNumber);

                string errorMessage = AttachmentUtil.UploadFile(moduleName, fileUploadInfos);

                result = !string.IsNullOrEmpty(errorMessage)
                        ? new { Success = false, Messages = errorMessage, ClientId = clientId }
                        : new { Success = true, Messages = LabelUtil.GetTextByKey("per_permitDetail_message_asyUploadSuccess", moduleName), ClientId = clientId };
            }
            catch (ACAException ex)
            {
                result = new { Success = false, Messages = ex.Message, ClientId = clientId };
            }

            return jsSerializer.Serialize(result);
        }

        /// <summary>
        /// On Initial event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            ParseUrlArg();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            LoadCapModelFromMap(capModel);

            PageFlowGroupModel pageFlowGroup = PageFlowUtil.GetPageflowGroup(capModel);
            bool hasResetPageTrace = false;

            //if the first step and page load then to reset the page trace data because this page may be come from review page.
            if (!IsPostBack && _stepNumber == 2 && _pageNumber == 1 && PageFlowUtil.IsPageflowChanged(capModel, ModuleName, pageFlowGroup))
            {
                hasResetPageTrace = true;

                PageFlowUtil.ResetPageTrace(capModel);
                AppSession.SetPageflowGroupToSession(null);
                pageFlowGroup = PageFlowUtil.GetPageflowGroup(capModel);
            }

            if (!IsPostBack)
            {
                //Clear UI data before initialize sections.
                UIModelUtil.ClearUIData();

                if (!AppSession.IsAdmin)
                {
                    ContactUtil.InitializeContactsGroup4CapModel(capModel);
                    ContactUtil.PrepareContactsForCopyingRecord(capModel, pageFlowGroup);
                    LicenseUtil.PrepareLicensesForCopyingRecord(capModel, pageFlowGroup);
                }
            }

            BindPageflowGroup(capModel, pageFlowGroup, hasResetPageTrace);
            DialogUtil.RegisterScriptForDialog(Page);

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Click event of the Refresh address button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Event Args instance containing the event data.</param>
        protected void BtnRefreshAddress_Click(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                AddressEdit addressEdit = (AddressEdit)_htUserControls["WorkLocationEdit"];

                if (addressEdit != null)
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(this.ModuleName);

                    if (capModel != null && capModel.addressModel != null)
                    {
                        addressEdit.DisplayAddress(capModel.addressModel, false, true);
                        addressEdit.UpdateAfterRefresh();
                    }
                }
            }
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Don't run expression in administration model.
            if (!AppSession.IsAdmin)
            {
                //Register the Expression script resources.
                if (!ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
                {
                    ExpressionUtil.RegisterScriptLibToCurrentPage(this);
                }

                if (!Page.IsPostBack)
                {
                    // Clear some temporary data stored in seestion for expression 
                    ExpressionUtil.ClearExpressionVariables();

                    RegisterExpressionOnLoad();
                }
                
                RegisterExpressionOnSubmit();
                ExpressionUtil.ResetJsExpression(this);
            }
        }

        /// <summary>
        /// Handle the PreRenderComplete event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            //Register Validator lib for expression.
            ScriptManager.RegisterClientScriptResource(Page, typeof(AjaxControlToolkit.ExtenderControlBase), "AjaxControlToolkit.ExtenderBase.BaseScripts.js");
            ScriptManager.RegisterClientScriptResource(Page, typeof(AjaxControlToolkit.CommonToolkitScripts), "AjaxControlToolkit.Common.Common.js");
            ScriptManager.RegisterClientScriptResource(Page, typeof(AjaxControlToolkit.PopupExtender), "AjaxControlToolkit.PopupExtender.PopupBehavior.js");
            ScriptManager.RegisterClientScriptResource(Page, typeof(AjaxControlToolkit.ValidatorCallbackExtender), "Accela.Web.Controls.ValidatorCallback.ValidatorCallbackBehavior.js");
        }

        /// <summary>
        /// FEIN or SSN duplicate button ok click.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void BtnOk_Click(object sender, EventArgs e)
        {
            ContinueToConfrim(null);
        }

        /// <summary>
        /// continue button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void BtnContinueToConfirm_Click(object sender, EventArgs e)
        {
            // execute customize component's [SaveBefore] action
            BaseCustomizeComponent customizeComponent = (BaseCustomizeComponent)_htUserControls[CUSTOMIZE_COMPONENT];
            if (customizeComponent != null)
            {
                ResultMessage result = customizeComponent.SaveBefore();

                if (!result.IsSuccess)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, result.Message);
                }
            }

            try
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                bool isValidateForRefLicense = LicenseUtil.IsReferenceDataForLicense(_htUserControls, ModuleName, _dictKeysAndNamesForMultipleLP, _dictKeysAndNamesForLPList);
                bool isValidateForRefContact = ContactUtil.IsReferenceDataForContact(
                                                            _htUserControls,
                                                            capModel,
                                                            _dictKeysAndNamesForMultipleApplicant,
                                                            _dictKeysAndNamesForMultipleContact1,
                                                            _dictKeysAndNamesForMultipleContact2,
                                                            _dictKeysAndNamesForMultipleContact3,
                                                            _dictKeysAndNamesForMultipleContactList);

                if (!isValidateForRefLicense || !isValidateForRefContact)
                {
                    return;
                }

                bool isValidateRequiredForContact = ContactUtil.ValidateRequiredFields4ContactSection(capModel, ModuleName, _htUserControls);

                if (!isValidateRequiredForContact)
                {
                    return;
                }

                string errMsg = SetupConfirmData4EMSE();

                if (string.IsNullOrEmpty(errMsg)
                    && (!string.IsNullOrEmpty(_emseErrorMsg) && _emseErrorMsg.StartsWith(ACAConstant.SSNORFEIN_ERRORCODE)))
                {
                    string message = _emseErrorMsg.Substring(1, _emseErrorMsg.Length - 1);
                    lblEmseMessage.Text = message + ACAConstant.BLANK + LabelUtil.GetTextByKey("popupmessage_continue_confirm", ModuleName);
                    hlBegin.Text = message;
                    string scriptStr = "<script language='JavaScript'>showContinueDlg('" + hlBegin.ClientID + "');</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "showDuplicateMessage", scriptStr);

                    return;
                }

                ContinueToConfrim(errMsg);
            }
            catch (ACAException ex)
            {
                HandleSubmitException(ex.Message);
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //set the CapEdit page no cache.
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

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

            actionBarBottom.ContinueButtonClick += BtnContinueToConfirm_Click;

            if (!IsPostBack)
            {
                SetBreadcrumbPageFlow();

                int stepNumber = int.Parse(Request.QueryString["stepNumber"]);

                if (_is4FeeEstimator && _isRedirectToFeePage)
                {
                    RedirectToCapFeePage(Request.QueryString["sourceFrom"], stepNumber);
                }
                                
                RegisterHotKey();

                if (!AppSession.IsAdmin)
                {
                    // execute customize component's [Show] action
                    BaseCustomizeComponent customizeComponent = (BaseCustomizeComponent)_htUserControls[CUSTOMIZE_COMPONENT];
                    if (customizeComponent != null)
                    {
                        ResultMessage result = customizeComponent.Show();
                        if (!result.IsSuccess)
                        {
                            MessageUtil.ShowMessage(Page, MessageType.Error, result.Message);
                        }
                    }

                    //javascript set page scroll to anchor.
                    string gotoName = Request.QueryString["goto"];
                    string script = string.Empty;

                    if (!string.IsNullOrEmpty(gotoName))
                    {
                        script = string.Format("scrollIntoView('{0}');", gotoName);
                    }

                    /*
                     * Build the script of the popup page opened from confirm page.
                     * Note: This script must be placed after "scrollIntoView". Otherwise, the popup's position is incorrect.
                     */
                    script += BuildScript4PopupFromConfirm();

                    if (!string.IsNullOrEmpty(script))
                    {
                        ScriptManager.RegisterStartupScript(
                            Page,
                            GetType(),
                            "scrollOrPopup",
                            string.Format("$(document).ready(function(){{{0}}});", script),
                            true);
                    }
                }
            }
            else if (Request["__EVENTTARGET"] == "SaveAndResume")
            {
                SaveAndResume();
            }

            actionBarBottom.Is4FeeEstimator = _is4FeeEstimator;
        }

        /// <summary>
        /// Handle the LoadComplete event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                //Handle ASIT expression behaviors.
                ExpressionUtil.HandleASITPostbackBehavior(Page);
            }
        }

        /// <summary>
        /// Refresh education, examination and continuing education list event triggered by select data from ref contact.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefreshEduExamListButton_Click(object sender, EventArgs e)
        {
            string postData = Request.Form[Page.postEventArgumentID];

            if (string.IsNullOrEmpty(postData))
            {
                return;
            }

            string[] postArgs = postData.Split(ACAConstant.SPLIT_CHAR);
            string componentName = postArgs[0];
            string componentType = postArgs[1];
            string refContactSeqNbr = postArgs[2];
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            string eduEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EDUCATION);
            string examEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EXAMINATION);
            string contEduEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_CONT_EDUCATION);

            EducationEdit eduEdit = _htUserControls[eduEditKey] as EducationEdit;
            ExaminationEdit examEdit = _htUserControls[examEditKey] as ExaminationEdit;
            ContinuingEducationEdit contEduEdit = _htUserControls[contEduEditKey] as ContinuingEducationEdit;

            // Gets selected examinations/educations/continuing educations in contact look up process.
            SelectedContactLicenseCertificationModel selectedLicenseCertification = AppSession.GetSelectedContactLicenseCertification();
            IList<EducationModel4WS> educationModels = selectedLicenseCertification.SelectedEducations;
            IList<ContinuingEducationModel4WS> continuingEducationModels = selectedLicenseCertification.SelectedContEdus;
            IList<ExaminationModel> examinationModels = selectedLicenseCertification.SelectedExaminations;

            PageFlowComponent pageFlowComponent = EnumUtil<PageFlowComponent>.Parse(componentType);
            RefreshContactSection(componentName, pageFlowComponent);

            int eduDuplicateCount = 0;
            int examRestrictedCount = 0;
            int contEduDuplicateCount = 0;
            int eduSuccessCount = 0;
            int examSuccessCount = 0;
            int contEduSuccessCount = 0;
            StringBuilder sbMsg = new StringBuilder();

            if (educationModels != null && educationModels.Any() && PageFlowUtil.IsComponentExist(GViewConstant.SECTION_EDUCATOIN))
            {
                RefreshEducationList(educationModels, capModel, eduEdit, ref eduDuplicateCount);
                eduSuccessCount = educationModels.Count - eduDuplicateCount;
                ConstructSelectFromContactMsg(PageFlowComponent.EDUCATION, eduSuccessCount, eduDuplicateCount, ref sbMsg);
            }

            if (examinationModels != null && examinationModels.Any() && PageFlowUtil.IsComponentExist(GViewConstant.SECTION_EXAMINATION))
            {
                RefreshExamList(examinationModels, capModel, examEdit, ref examRestrictedCount);
                examSuccessCount = examinationModels.Count - examRestrictedCount;

                if (!string.IsNullOrEmpty(sbMsg.ToString()))
                {
                    sbMsg.Append("<br/>");
                }

                ConstructSelectFromContactMsg(PageFlowComponent.EXAMINATION, examSuccessCount, examRestrictedCount, ref sbMsg);
            }

            if (continuingEducationModels != null && continuingEducationModels.Any() && PageFlowUtil.IsComponentExist(GViewConstant.SECTION_CONTINUING_EDUCATION))
            {
                RefreshContEducationList(continuingEducationModels, capModel, contEduEdit, ref contEduDuplicateCount);
                contEduSuccessCount = continuingEducationModels.Count - contEduDuplicateCount;

                if (!string.IsNullOrEmpty(sbMsg.ToString()))
                {
                    sbMsg.Append("<br/>");
                }

                ConstructSelectFromContactMsg(PageFlowComponent.CONTINUING_EDUCATION, contEduSuccessCount, contEduDuplicateCount, ref sbMsg);
            }

            if (eduSuccessCount > 0 || examSuccessCount > 0 || contEduSuccessCount > 0)
            {
                string primaryComponentName = SetCapPrimaryContact(capModel, refContactSeqNbr);
                ChangeMsg4RemoveContact(capModel, primaryComponentName);
            }

            if (!string.IsNullOrEmpty(sbMsg.ToString()))
            {
                bool isSuccess = eduDuplicateCount + examRestrictedCount + contEduDuplicateCount == 0;
                DisplaySelectFromContactNotice(pageFlowComponent, isSuccess, sbMsg, eduEdit, examEdit, contEduEdit);
            }
            
            if (pageFlowComponent == PageFlowComponent.EDUCATION 
                || pageFlowComponent == PageFlowComponent.EXAMINATION
                || pageFlowComponent == PageFlowComponent.CONTINUING_EDUCATION
                || string.IsNullOrEmpty(sbMsg.ToString()))
            {
                ContactEdit contactEdit = _htUserControls[componentName + "Edit"] as ContactEdit;
                string focusId = Request.Form["__LASTFOCUS_ID"];

                if (contactEdit != null)
                {
                    focusId = contactEdit.EditClientID;
                }

                Page.FocusElement(focusId);
            }

            //clear session when operation completed.
            AppSession.SetSelectedContactLicenseCertification(null);
        }

        /// <summary>
        /// Judge whether default job value needs be display in additional information section.
        /// </summary>
        /// <param name="capModel">a CapModel4WS</param>
        /// <returns>true or false.</returns>
        private static bool IsNeedShowDefaultJobValue(CapModel4WS capModel)
        {
            bool isNeed = false;

            if (capModel.bvaluatnModel == null || string.IsNullOrEmpty(capModel.bvaluatnModel.estimatedValue) || !ValidationUtil.IsNumber(capModel.bvaluatnModel.estimatedValue))
            {
                isNeed = true;
            }

            return isNeed;
        }

        /// <summary>
        /// Change the confirm message for remove contact.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="primaryComponentName">The component name which contain primary contact.</param>
        private void ChangeMsg4RemoveContact(CapModel4WS capModel, string primaryComponentName)
        {
            if (string.IsNullOrEmpty(primaryComponentName))
            {
                return;
            }

            string key = string.Format("{0}Edit", primaryComponentName);

            if (primaryComponentName.StartsWith(PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX))
            {
                MultiContactsEdit multiContactsEdit = _htUserControls[key] as MultiContactsEdit;

                if (multiContactsEdit != null)
                {
                    //Need get the contact records match the component name of current Component to display in Contact List.
                    List<CapContactModel4WS> contactsGroupList = new List<CapContactModel4WS>();

                    foreach (CapContactModel4WS contact in capModel.contactsGroup)
                    {
                        if (contact == null || !multiContactsEdit.ComponentName.Equals(contact.componentName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        contact.validateFlag = multiContactsEdit.ValidateFlag;
                        contactsGroupList.Add(contact);
                    }

                    multiContactsEdit.DisplayContacts(contactsGroupList.ToArray());
                }
            }
            else if (primaryComponentName.StartsWith(PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX)
                    || primaryComponentName.StartsWith(PageFlowConstant.SECTION_NAME_CONTACT1_PREFIX)
                    || primaryComponentName.StartsWith(PageFlowConstant.SECTION_NAME_CONTACT2_PREFIX)
                    || primaryComponentName.StartsWith(PageFlowConstant.SECTION_NAME_CONTACT3_PREFIX))
            {
                ContactEdit contactEdit = _htUserControls[key] as ContactEdit;

                if (contactEdit != null)
                {
                    contactEdit.ChangeMsg4RemoveContact();
                    RefreshUpdatePanel(primaryComponentName);
                }
            }
        }

        /// <summary>
        /// Refresh the contact section after: Select from Account-&gt; Select from Contact's educations and exams.
        /// </summary>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="componentType">The component type.</param>
        private void RefreshContactSection(string componentName, PageFlowComponent componentType)
        {
            switch (componentType)
            {
                case PageFlowComponent.CONTACT_LIST:
                    MultiContactsEdit multiContacts = _htUserControls[componentName + "Edit"] as MultiContactsEdit;

                    if (multiContacts != null)
                    {
                        multiContacts.RefreshContactList(false);
                    }

                    break;
                case PageFlowComponent.CONTACT_1:
                case PageFlowComponent.CONTACT_2:
                case PageFlowComponent.CONTACT_3:
                case PageFlowComponent.APPLICANT:
                    ContactEdit contactEdit = _htUserControls[componentName + "Edit"] as ContactEdit;

                    if (contactEdit != null)
                    {
                        contactEdit.RefreshContact();
                        RefreshUpdatePanel(componentName);
                    }

                    break;
            }
        }

        /// <summary>
        /// Display select from contact education, exam or continuing education data.
        /// </summary>
        /// <param name="pageFlowComponent">The page flow component.</param>
        /// <param name="isSuccess">Is success.</param>
        /// <param name="sbMsg">The message.</param>
        /// <param name="eduEdit">The education edit control.</param>
        /// <param name="examEdit">The examination edit control.</param>
        /// <param name="contEduEdit">The continuing education edit control.</param>
        private void DisplaySelectFromContactNotice(PageFlowComponent pageFlowComponent, bool isSuccess, StringBuilder sbMsg, EducationEdit eduEdit, ExaminationEdit examEdit, ContinuingEducationEdit contEduEdit)
        {
            switch (pageFlowComponent)
            {
                case PageFlowComponent.EDUCATION:
                    if (eduEdit != null)
                    {
                        eduEdit.DisplayAddFromSavedNotice(isSuccess, sbMsg.ToString());
                        RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_EDUCATION);
                    }

                    break;
                case PageFlowComponent.EXAMINATION:
                    if (examEdit != null)
                    {
                        examEdit.DisplayAddFromSavedNotice(isSuccess, sbMsg.ToString());
                        RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_EXAMINATION);
                    }

                    break;
                case PageFlowComponent.CONTINUING_EDUCATION:
                    if (contEduEdit != null)
                    {
                        contEduEdit.DisplayAddFromSavedNotice(isSuccess, sbMsg.ToString());
                        RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_CONT_EDUCATION);
                    }

                    break;
                case PageFlowComponent.CONTACT_LIST:
                case PageFlowComponent.APPLICANT:
                case PageFlowComponent.CONTACT_1:
                case PageFlowComponent.CONTACT_2:
                case PageFlowComponent.CONTACT_3:
                    MessageUtil.ShowMessageByControl(Page, isSuccess ? MessageType.Success : MessageType.Notice, sbMsg.ToString());

                    break;
            }
        }

        /// <summary>
        /// Refresh continuing education list.
        /// </summary>
        /// <param name="contEduModels">The continuing EDU models.</param>
        /// <param name="capModel">The cap model.</param>
        /// <param name="contEduEdit">The continuing education edit control.</param>
        /// <param name="contEduDuplicateCount">The duplicate continuing  education count.</param>
        private void RefreshContEducationList(IList<ContinuingEducationModel4WS> contEduModels, CapModel4WS capModel, ContinuingEducationEdit contEduEdit, ref int contEduDuplicateCount)
        {
            IList<ContinuingEducationModel4WS> newContEduList = ObjectConvertUtil.ConvertArrayToList(capModel.contEducationList);

            if (newContEduList == null || !newContEduList.Any())
            {
                /*
                 * If there is no any continuing education data exist in CapModel try to get the required continuing education data,
                 *  and fill the required continuing education into capmodel and update the continuing education section.
                 */
                CapAssociateLicenseCertification4WS capAssociateLicenseCertification = AppSession.GetCapTypeAssociateLicenseCertification();

                if (capAssociateLicenseCertification != null && capAssociateLicenseCertification.capAssociateContEducation != null)
                {
                    var contEduArray = EducationUtil.ConvertToContEdu4WSModelArray(capAssociateLicenseCertification.capAssociateContEducation);
                    newContEduList = new List<ContinuingEducationModel4WS>(contEduArray);
                }
                else
                {
                    newContEduList = new List<ContinuingEducationModel4WS>();
                }
            }

            foreach (var contactContEduModel in contEduModels)
            {
                if (contactContEduModel == null)
                {
                    continue;
                }

                contactContEduModel.entityID = Convert.ToString(contactContEduModel.continuingEducationPKModel.contEduNbr);
                contactContEduModel.continuingEducationPKModel.contEduNbr = null;
                contactContEduModel.entityType = ACAConstant.CAP_CONT_EDUCATION_ENTITY_TYPE;
                contactContEduModel.RowIndex = newContEduList.Count;
                contactContEduModel.syncFlag = ACAConstant.COMMON_Y;

                if (EducationUtil.ExistDuplicateContEducation(contactContEduModel, newContEduList))
                {
                    contEduDuplicateCount++;
                    continue;
                }

                //Get the cap required and not edited continuing education.
                var ceduFormCapAss = newContEduList.FirstOrDefault(o => contactContEduModel.contEduName.Equals(o.contEduName) && o.FromCapAssociate);

                if (ceduFormCapAss != null)
                {
                    /*
                     * Find cap required and not edited continuing education position in list.
                     * change the row index.
                     * Replace it
                     */
                    int index = newContEduList.IndexOf(ceduFormCapAss);
                    contactContEduModel.RowIndex = ceduFormCapAss.RowIndex;
                    newContEduList[index] = contactContEduModel;
                }
                else
                {
                    newContEduList.Add(contactContEduModel);
                }
            }

            if (contEduEdit != null)
            {
                contEduEdit.DislayContEducations(newContEduList.ToArray());
                RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_CONT_EDUCATION);
            }

            capModel.contEducationList = newContEduList.ToArray();
        }

        /// <summary>
        /// Refresh exam list.
        /// </summary>
        /// <param name="examinationModels">The examination models.</param>
        /// <param name="capModel">The cap model.</param>
        /// <param name="examEdit">The exam edit control.</param>
        /// <param name="examDuplicateCount">The duplicate exam count.</param>
        private void RefreshExamList(IList<ExaminationModel> examinationModels, CapModel4WS capModel, ExaminationEdit examEdit, ref int examDuplicateCount)
        {
            IList<ExaminationModel> newExamList = ObjectConvertUtil.ConvertArrayToList(capModel.examinationList);
            IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();

            if (newExamList == null || !newExamList.Any())
            {
                /*
                 * If there is no any examination data exist in CapModel try to get the required examination data,
                 *  and fill the required examination into capmodel and update the examination section.
                 */
                CapAssociateLicenseCertification4WS capAssociateLicenseCertification = AppSession.GetCapTypeAssociateLicenseCertification();

                if (capAssociateLicenseCertification != null && capAssociateLicenseCertification.capAssociateExamination != null)
                {
                    var examinationArray = ExaminationUtil.ConvertToExaminationModelArray(capAssociateLicenseCertification.capAssociateExamination);
                    newExamList = new List<ExaminationModel>(examinationArray);
                }
                else
                {
                    newExamList = new List<ExaminationModel>();
                }
            }

            foreach (var examModel in examinationModels)
            {
                if (examModel == null)
                {
                    continue;
                }

                if (examinationBll.IsWrokflowRestricted(examModel, capModel, ACAConstant.SPEAR_FORM, AppSession.User.PublicUserId))
                {
                    examDuplicateCount++;
                    continue;
                }

                examModel.entityID = examModel.examinationPKModel.examNbr;
                examModel.examinationPKModel.examNbr = null;
                examModel.entityType = ACAConstant.CAP_EXAMINATION_ENTITY_TYPE;
                examModel.syncFlag = ACAConstant.COMMON_Y;

                //Get the cap required and not edited examination.
                var examFormCapAss = newExamList.FirstOrDefault(o => examModel.examName.Equals(o.examName) && o.FromCapAssociate);

                if (examFormCapAss != null)
                {
                    /*
                     * Find cap required and not edited examination position in list.
                     * change the row index.
                     * Replace it
                     */
                    int index = newExamList.IndexOf(examFormCapAss);
                    examModel.RowIndex = examFormCapAss.RowIndex;
                    newExamList[index] = examModel;
                }
                else
                {
                    examModel.RowIndex = newExamList.Count;
                    newExamList.Add(examModel);
                }
            }

            if (examEdit != null)
            {
                examEdit.UpdateExamList(newExamList);
            }

            capModel.examinationList = newExamList.ToArray();
        }

        /// <summary>
        /// Refresh education list.
        /// </summary>
        /// <param name="educationModels">The education models.</param>
        /// <param name="capModel">The cap model.</param>
        /// <param name="eduEdit">The education edit control.</param>
        /// <param name="eduDuplicateCount">The duplicate education count.</param>
        private void RefreshEducationList(IList<EducationModel4WS> educationModels, CapModel4WS capModel, EducationEdit eduEdit, ref int eduDuplicateCount)
        {
            IList<EducationModel4WS> newEducationList = ObjectConvertUtil.ConvertArrayToList(capModel.educationList);

            if (newEducationList == null || !newEducationList.Any())
            {
                /*
                 * If there is no any education data exist in CapModel try to get the required education data,
                 *  and fill the required education into capmodel and update the education section.
                 */

                CapAssociateLicenseCertification4WS capAssociateLicenseCertification = AppSession.GetCapTypeAssociateLicenseCertification();

                if (capAssociateLicenseCertification != null && capAssociateLicenseCertification.capAssociateEducation != null)
                {
                    var educationArray = EducationUtil.ConvertToEducation4WSModelArray(capAssociateLicenseCertification.capAssociateEducation);
                    newEducationList = new List<EducationModel4WS>(educationArray);
                }
                else
                {
                    newEducationList = new List<EducationModel4WS>();
                }
            }

            foreach (var contactEduModel in educationModels)
            {
                if (contactEduModel == null)
                {
                    continue;
                }

                contactEduModel.entityID = contactEduModel.educationPKModel.educationNbr;
                contactEduModel.educationPKModel.educationNbr = null;
                contactEduModel.entityType = ACAConstant.CAP_EDUCATION_ENTITY_TYPE;
                contactEduModel.RowIndex = newEducationList.Count;
                contactEduModel.syncFlag = ACAConstant.COMMON_Y;

                if (EducationUtil.IsExistDuplicateEducation(contactEduModel, newEducationList))
                {
                    eduDuplicateCount++;
                    continue;
                }

                //Get the cap required and not edited education
                var eduFromCapAss = newEducationList.FirstOrDefault(o => contactEduModel.educationName.Equals(o.educationName) && o.FromCapAssociate);

                if (eduFromCapAss != null)
                {
                    /*
                     * Find cap required and not edited education position in list.
                     * change the row index.
                     * Replace it
                     */
                    int index = newEducationList.IndexOf(eduFromCapAss);
                    contactEduModel.RowIndex = eduFromCapAss.RowIndex;
                    newEducationList[index] = contactEduModel;
                }
                else
                {
                    newEducationList.Add(contactEduModel);
                }
            }

            if (eduEdit != null)
            {
                eduEdit.UpdateEduList(newEducationList);
            }

            capModel.educationList = newEducationList.ToArray();
        }

        /// <summary>
        /// Flag the primary contact when add the cap education from ref contact.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="refContactSeqNbr">The ref contact sequence number.</param>
        /// <returns>The primary component name.</returns>
        private string SetCapPrimaryContact(CapModel4WS capModel, string refContactSeqNbr)
        {
            string primaryComponentName = string.Empty;

            CapContactModel4WS primaryCapContact = Examination.ExaminationScheduleUtil.GetCapPrimaryContact(capModel);

            if (primaryCapContact != null && primaryCapContact.refContactNumber == refContactSeqNbr)
            {
                return primaryCapContact.componentName;
            }

            if (capModel.contactsGroup != null && capModel.contactsGroup.Any())
            {
                CapContactModel4WS needSetContact = capModel.contactsGroup.LastOrDefault(f => f.people != null && string.Equals(f.refContactNumber, refContactSeqNbr));

                if (needSetContact != null)
                {
                    primaryComponentName = needSetContact.componentName;
                    needSetContact.people.flag = ACAConstant.COMMON_Y;
                }

                // because needSetContact from the capModel.contactsGroup, the object has same pointer.
                foreach (var capContact in capModel.contactsGroup.Where(f => f.people != null && f != needSetContact))
                {
                    capContact.people.flag = ACAConstant.COMMON_N;
                }
            }

            return primaryComponentName;
        }

        /// <summary>
        /// Construct the Select from Contact education and exams message.
        /// </summary>
        /// <param name="componentType">The component type.</param>
        /// <param name="successCount">The success count.</param>
        /// <param name="failCount">The fail count.</param>
        /// <param name="sbMsg">The message.</param>
        private void ConstructSelectFromContactMsg(PageFlowComponent componentType, int successCount, int failCount, ref StringBuilder sbMsg)
        {
            string labelSuccess = string.Empty;
            string labelFail = string.Empty;

            switch (componentType)
            {
                case PageFlowComponent.EDUCATION:
                    labelSuccess = "aca_education_msg_addfromsaved_success";
                    labelFail = "aca_education_msg_addfromsaved_duplicate";
                    break;
                case PageFlowComponent.EXAMINATION:
                    labelSuccess = "aca_examination_msg_addfromsaved_success";
                    labelFail = "aca_examination_msg_addfromsaved_workflow_restricted";
                    break;
                case PageFlowComponent.CONTINUING_EDUCATION:
                    labelSuccess = "aca_conteducation_msg_addfromsaved_success";
                    labelFail = "aca_conteducation_msg_addfromsaved_duplicate";
                    break;
            }

            if (successCount > 0)
            {
                sbMsg.AppendFormat(GetTextByKey(labelSuccess), successCount);
            }

            if (failCount > 0)
            {
                sbMsg.AppendFormat(GetTextByKey(labelFail), failCount);
            }
        }

        /// <summary>
        /// Load CapModel from Map.
        /// </summary>
        /// <param name="capModel">capModel object</param>
        private void LoadCapModelFromMap(CapModel4WS capModel)
        {
            string isFromMap = Request.QueryString["IsFromMap"];
            if (!string.Equals(isFromMap, ACAConstant.COMMON_TRUE))
            {
                Session.Remove(SessionConstant.SESSION_APO_CONDITION);
            }

            if (!IsPostBack && Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] != null)
            {
                ACAGISModel model = Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] as ACAGISModel;
                if (model == null || model.ModuleName != this.ModuleName)
                {
                    return;
                }

                if (model.ParcelInfoModels != null && model.ParcelInfoModels.Length == 1)
                {
                    bool isExistAddress = PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ADDRESS);
                    bool isExistParcel = PageFlowUtil.IsComponentExist(GViewConstant.SECTION_PARCEL);

                    if (isExistAddress)
                    {
                        capModel.addressModel = CapUtil.ConvertRefAddressModel2AddressModel(model.ParcelInfoModels[0].RAddressModel);
                    }

                    if (isExistParcel)
                    {
                        capModel.parcelModel = CapUtil.ConvertParcelModel2CapParcelModel(model.ParcelInfoModels[0].parcelModel);
                    }

                    if (PageFlowUtil.IsComponentExist(GViewConstant.SECTION_OWNER))
                    {
                        capModel.ownerModel = CapUtil.ConvertOwnerModel2RefOwnerModel(model.ParcelInfoModels[0].ownerModel);
                    }

                    if ((isExistAddress || isExistParcel) && model.CapIDModels == null)
                    {
                        Session[SessionConstant.SESSION_APO_CONDITION] = GetConditionByAPO(model.ParcelInfoModels[0]);
                    }

                    capModel.APOConditionType = (model.RefAddressModels != null && model.RefAddressModels.Length > 0)
                        ? APOConditionType.Address
                        : APOConditionType.Parcel;

                    AppSession.SetCapModelToSession(ModuleName, capModel);
                    GISUtil.RemoveACAGISModelFromSession(ModuleName);
                }
            }
        }

        /// <summary>
        /// Get Condition 
        /// </summary>
        /// <param name="parcelInfo">ParcelInfo model</param>
        /// <returns>return ParcelModel including condition information.</returns>
        private ParcelModel GetConditionByAPO(ParcelInfoModel parcelInfo)
        {
            ParcelModel parcelModel = null;

            if (parcelInfo != null)
            {
                if (parcelInfo.parcelModel != null)
                {
                    IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();
                    parcelModel = parcelBll.GetParcelCondition(CapUtil.GetAgencyCodeList(ModuleName), parcelInfo);
                }
                else if (parcelInfo.RAddressModel != null)
                {
                    IRefAddressBll refAddressBll = ObjectFactory.GetObject<IRefAddressBll>();
                    RefAddressModel address = refAddressBll.GetAddressCondition(CapUtil.GetAgencyCodeList(ModuleName), parcelInfo);

                    if (address != null)
                    {
                        parcelModel = new ParcelModel();
                        parcelModel.hightestCondition = address.hightestCondition;
                        parcelModel.noticeConditions = address.noticeConditions;
                    }
                }
            }

            return parcelModel;
        }

        /// <summary>
        ///   Register hot key to cancel button
        /// </summary>
        private void RegisterHotKey()
        {
            const string JsFormat = "OverrideTabKey(event, {0}, '{1}')";
            btnCancel.Attributes.Add("onkeydown", string.Format(JsFormat, "false", btnOK.ClientID));
            btnOK.Attributes.Add("onkeydown", string.Format(JsFormat, "true", btnCancel.ClientID));
        }

        /// <summary>
        /// continue to confirm method.
        /// </summary>
        /// <param name="errMsg">error message</param>
        private void ContinueToConfrim(string errMsg)
        {
            if (!_isSingleContactValid)
            {
                return;
            }

            // execute customize component's [Save] action
            BaseCustomizeComponent customizeComponent = (BaseCustomizeComponent)_htUserControls[CUSTOMIZE_COMPONENT];
            if (customizeComponent != null)
            {
                ResultMessage result = customizeComponent.Save();

                if (!result.IsSuccess)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, result.Message);
                }
            }

            //EMSE script error
            if (!string.IsNullOrEmpty(_emseErrorMsg))
            {
                HandleSubmitException(_emseErrorMsg);
                return;
            }

            if (errMsg != null)
            {
                HandleSubmitException(errMsg);
                return;
            }

            // add the step number for parameter.
            CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(this.ModuleName);

            //after button
            EMSEResultModel4WS emseResult = EmseUtil.ExecuteEMSE(ref capModel4WS, _currentPageModel, EmseEventType.AfterButtonEvent, _isFromConfirmPage);

            AppSession.SetCapModelToSession(ModuleName, capModel4WS);

            if (emseResult != null)
            {
                string stepNumberStr = EmseUtil.GetEMSEReturnData(emseResult, "PageFlow", "StepNumber");
                string pageNumberStr = EmseUtil.GetEMSEReturnData(emseResult, "PageFlow", "PageNumber");

                int stepNumber = 0;
                int pageNumber = 0;

                if (!string.IsNullOrEmpty(stepNumberStr))
                {
                    stepNumber = int.Parse(stepNumberStr);
                }

                if (!string.IsNullOrEmpty(pageNumberStr))
                {
                    pageNumber = int.Parse(pageNumberStr);
                }

                if (stepNumber > 0 && pageNumber > 0)
                {
                    ContinueToNextPage(stepNumber, pageNumber);
                }

                _emseErrorMsg = emseResult.errorMessage;
            }

            //EMSE script error
            if (!string.IsNullOrEmpty(_emseErrorMsg))
            {
                HandleSubmitException(_emseErrorMsg);
                return;
            }

            if (StandardChoiceUtil.IsSuperAgency())
            {
                if (!CapUtil.ValidateLicenseProfessionType(capModel4WS.licenseProfessionalList))
                {
                    errMsg = GetTextByKey("aca_cap_detail_validation_licensetype_msg", ModuleName);
                    HandleSubmitException(errMsg);
                    return;
                }
            }

            // validate list when some required field is empty.
            if (!CapUtil.ValidateRequiredFields4List(_htUserControls, capModel4WS, ModuleName))
            {
                return;
            }

            AppSession.SetCapModelToSession(ModuleName, capModel4WS);

            if (_is4FeeEstimator)
            {
                int stepNumber = 3; //If needs to do fee calulation, the fee estimate page should be step 3.
                SavePartialCapModel();
                Response.Redirect(string.Format("CapFees.aspx?stepNumber={0}&Module={1}&isFeeEstimator={2}&isFromShoppingCart={3}", stepNumber, ModuleName, ACAConstant.COMMON_Y, Request.QueryString[ACAConstant.FROMSHOPPINGCART]));
            }

            // execute customize component's [Save After] action
            if (customizeComponent != null)
            {
                ResultMessage result = customizeComponent.SaveAfter();

                if (!result.IsSuccess)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, result.Message);
                }
            }

            int stepIndex = int.Parse(Request.QueryString["stepNumber"]);

            int curPageNumber = 1;

            if (ValidationUtil.IsInt(Request.QueryString["pageNumber"]) == true)
            {
                curPageNumber = int.Parse(Request.QueryString["pageNumber"]);
            }

            int pageIndex = string.IsNullOrEmpty(Request.QueryString["stepNumber"]) ? 1 : curPageNumber;

            bool completed = ParseStepAndPage(ref stepIndex, ref pageIndex, 0, 0);

            if (completed || _isFromConfirmPage)
            {
                RedirectToConfirmPage();
            }
            else
            {
                RedirectToEditPage(stepIndex, pageIndex);
            }
        }

        /// <summary>
        /// Clean up the redundant data of the specified capModel by removing all data belongs to the component in the hidden page.
        /// </summary>
        /// <param name="capModel">The instance of CapModel4WS to be checked.</param>
        private void CleanUpCapModel(CapModel4WS capModel)
        {
            PageFlowGroupModel pageFlowGroup = PageFlowUtil.GetPageflowGroup(capModel);
            PageFlowUtil.RemoveComponentDataInHiddenPage(pageFlowGroup, capModel);

            ContactUtil.RemoveRedundantContacts(capModel);
            LicenseUtil.RemoveRedundantLPs(capModel);
        }

        /// <summary>
        /// continue to next page.
        /// </summary>
        /// <param name="nextStepNumber">The next step number.</param>
        /// <param name="nextPageNumber">The next page number.</param>
        private void ContinueToNextPage(int nextStepNumber = 0, int nextPageNumber = 0)
        {
            if (_is4FeeEstimator)
            {
                //If needs to do fee calulation, the fee estimate page should be step 3.
                int stepNumber = 3;
                SavePartialCapModel();

                Response.Redirect(string.Format(
                    "CapFees.aspx?stepNumber={0}&Module={1}&isFeeEstimator={2}&isFromShoppingCart={3}",
                    stepNumber,
                    ModuleName,
                    ACAConstant.COMMON_Y,
                    Request.QueryString[ACAConstant.FROMSHOPPINGCART]));
            }

            int stepIndex = int.Parse(Request.QueryString["stepNumber"]);
            int curPageNumber = 1;

            if (ValidationUtil.IsInt(Request.QueryString["pageNumber"]))
            {
                curPageNumber = int.Parse(Request.QueryString["pageNumber"]);
            }

            bool completed = ParseStepAndPage(ref stepIndex, ref curPageNumber, nextStepNumber, nextPageNumber);

            if (completed)
            {
                RedirectToConfirmPage();
            }
            else
            {
                RedirectToEditPage(stepIndex, curPageNumber);
            }
        }

        /// <summary>
        /// add owner to contact user control's auto fill dropdown list
        /// </summary>
        /// <param name="ownerMode">owner model</param>
        /// <param name="parcelModel">parcel model</param>
        private void AddParcelPKToContactAutoFill(RefOwnerModel ownerMode, CapParcelModel parcelModel)
        {
            if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_OWNER))
            {
                return;
            }

            if (SourceSequenceNumber <= 0)
            {
                IServiceProviderBll serProviderBll = ObjectFactory.GetObject<IServiceProviderBll>();
                ServiceProviderModel serProviderModel = serProviderBll.GetServiceProviderByPK(ConfigManager.AgencyCode, null);
                SourceSequenceNumber = serProviderModel.sourceNumber;
            }

            if (Session[SessionConstant.APO_SESSION_PARCELMODEL] == null && ownerMode != null && parcelModel != null && parcelModel.parcelModel != null
                && (!string.IsNullOrEmpty(parcelModel.parcelModel.parcelNumber) || !string.IsNullOrEmpty(parcelModel.parcelModel.UID))
                && SourceSequenceNumber > 0)
            {
                ParcelModel parcelPK = new ParcelModel();
                parcelPK.sourceSeqNumber = SourceSequenceNumber;
                parcelPK.parcelNumber = parcelModel.parcelModel.parcelNumber;
                parcelPK.UID = parcelModel.parcelModel.UID;

                Session[SessionConstant.APO_SESSION_PARCELMODEL] = parcelPK;
            }
        }

        /// <summary>
        /// Load sections according to page flow config
        /// </summary>
        /// <param name="capModel4WS">cap model for ACA.</param>
        /// <param name="pageflowGroup">PageFlowGroupModel model</param>
        /// <param name="hasResetPageTrace">Has the page trace reset.</param>
        private void BindPageflowGroup(CapModel4WS capModel4WS, PageFlowGroupModel pageflowGroup, bool hasResetPageTrace)
        {
            if (capModel4WS == null || capModel4WS.capType == null)
            {
                return;
            }

            if (pageflowGroup == null || pageflowGroup.stepList == null || pageflowGroup.stepList.Length < 1)
            {
                RedirectToConfirmPage();

                return;
            }

            try
            {
                /* Judge whether it is the first time to load the SPEAR Form. 
                 * It use PageTrace(store in session) to trace the page flow's hidden page.
                 * If it first time load, it need reset the PageTrace. Or else it will use the previous PageTrace that cause issue.
                 */
                bool isFirstLoad = Request.UrlReferrer != null
                                && !Request.UrlReferrer.LocalPath.EndsWith("Cap/CapEdit.aspx", StringComparison.InvariantCultureIgnoreCase)
                                && !Request.UrlReferrer.LocalPath.EndsWith("Cap/CapConfirm.aspx", StringComparison.InvariantCultureIgnoreCase)
                                && !Request.UrlReferrer.LocalPath.EndsWith("Cap/CapFees.aspx", StringComparison.InvariantCultureIgnoreCase)
                                && !Request.UrlReferrer.LocalPath.EndsWith("Cap/AssociatedForms.aspx", StringComparison.InvariantCultureIgnoreCase)
                                && !Request.UrlReferrer.LocalPath.EndsWith("Cap/CapPayment.aspx", StringComparison.InvariantCultureIgnoreCase)
                                && !IsRefreshOnCurrentpage();

                if (!AppSession.IsAdmin)
                {
                    bool isFromShoppingCart = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FROM_SHOPPING_CART]);
                    var capAssociateLicense = AppSession.GetCapTypeAssociateLicenseCertification();
                    bool isNeedToRetrieveData = capAssociateLicense == null
                                                || !capAssociateLicense.CapTypeKey.Equals(capModel4WS.capType.ToKey(), StringComparison.InvariantCulture);

                    if ((isFirstLoad || (isFromShoppingCart && isNeedToRetrieveData)) && PageFlowUtil.IsEduExamComponentExist())
                    {
                        ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
                        capAssociateLicense = licenseCertificationBll.GetCapAssociateLicenseCertification(capModel4WS.capType);
                        capAssociateLicense.CapTypeKey = capModel4WS.capType.ToKey();
                        AppSession.SetCapTypeAssociateLicenseCertification(capAssociateLicense);
                    }

                    if (isFirstLoad && !hasResetPageTrace)
                    {
                        PageFlowUtil.RefreshPageStateTracking(capModel4WS);
                    }
                }

                /*
                 * To trace the page load, there are a few basic steps that you'll need to follow:
                 * 1. Set up the trace.
                 * 2. Mark the page as loaded when finishing loading a page.
                 * 3. Review the trace results.
                 */

                // Make sure the initialization of page trace executes one and only one time.
                if (!PageFlowUtil.IsSetupPageTrace(capModel4WS))
                {
                    PageFlowUtil.SetupPageTrace(capModel4WS, pageflowGroup);
                }

                if (_is4FeeEstimator)
                {
                    LoadFeeForm(pageflowGroup.stepList, capModel4WS);
                }
                else
                {
                    LoadPage(pageflowGroup.stepList[_currentStep].pageList[_currentPage], capModel4WS);
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// build section information
        /// </summary>
        /// <param name="sectionId">section id</param>
        /// <param name="controlPrefix">section control client id's prefix</param>
        /// <returns>Section Info</returns>
        private string BuildSectionInfo(string sectionId, string controlPrefix)
        {
            string sectionInfor = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR + "{2}", ModuleName, sectionId, controlPrefix);
            return sectionInfor;
        }

        /// <summary>
        /// copy trade name info to trade license
        /// </summary>
        /// <param name="newCap">a CapModel4WS</param>
        private void CopyTradeNameToTradeLicense(CapModel4WS newCap)
        {
            if (Request["FilterName"] == ACAConstant.REQUEST_PARMETER_TRADE_LICENSE)
            {
                CapModel4WS oldCap = AppSession.GetCapModelFromSession(ModuleName);
                newCap.licenseProfessionalModel = oldCap.licenseProfessionalModel;
                newCap.licenseProfessionalList = oldCap.licenseProfessionalList;
                newCap.licSeqNbr = oldCap.licSeqNbr;
                newCap.capType = oldCap.capType;
                CapUtil.FillCapModelTemplateValue(newCap);
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
            control.ID = key;
            controls.Add(control);
            _htUserControls.Add(key, control);
        }

        /// <summary>
        /// create section title label
        /// </summary>
        /// <param name="controls">a control collection that the label added to </param>
        /// <param name="labelKey">the key in UI DB that will load the text to the label</param>
        /// <param name="title">the text to the label</param>
        /// <param name="instruction">section instruction</param>
        /// <param name="labelType">label type</param>
        /// <returns>the created label</returns>
        private AccelaLabel CreateLabel(ControlCollection controls, string labelKey, string title, string instruction, LabelType labelType)
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

            label.LabelKey = labelKey;
            label.ID = "lbl" + CommonUtil.GetRandomUniqueID().Substring(0, 8).Replace("-", string.Empty);
            label.LabelType = labelType;
            label.SubLabel = instruction;

            controls.Add(label);

            return label;
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
        /// <param name="sectionName">section name</param>
        /// <param name="controlName">user control name</param>
        /// <param name="titleKey">the key of UI DB to load title for section,it is the default tittle if the parameter of <paramref name="title"/> is empty</param>
        /// <param name="title">the section title</param>
        /// <param name="sectionId">section id</param>
        /// <param name="sectionInstruction">section instructions</param>
        private void CreateSection(string sectionName, string controlName, string titleKey, string title, string sectionId, string sectionInstruction)
        {
            if (!AppSession.IsAdmin)
            {
                // the code is only for auto testing team
                CreateHiddenSectionName(sectionName + "Edit");
            }

            if (!sectionName.StartsWith(PageFlowConstant.SECTION_NAME_ASIT))
            {
                controlName += "Edit";
            }

            CreateLiteral(placeHolder.Controls, string.Format("<a name=\"{0}\" id=\"{0}\" class=\"SectionTextDecoration\">&nbsp;</a>", sectionName));

            CreateLiteral(placeHolder.Controls, "<div>");

            LabelType labelType = LabelType.SectionExText;

            AccelaLabel label = CreateLabel(placeHolder.Controls, titleKey, title, sectionInstruction, labelType);

            CreateUpdatePanel(placeHolder.Controls, sectionName, controlName, string.Format("UpdatePanel{0}", sectionName));

            CreateLiteral(placeHolder.Controls, "</div>");

            //Build section ID for Admin site to customize the form layout.
            label.SectionID = BuildSectionInfo(sectionId, ConstuctControlPrefix(sectionName));
        }

        /// <summary>
        /// Create hidden field for section name.
        /// </summary>
        /// <param name="sectionName">section name</param>
        private void CreateHiddenSectionName(string sectionName)
        {
            string htmlFormat = "<input type=\"hidden\" name=\"component\" value=\"{0}\"/>";
            placeHolder.Controls.Add(new Literal()
            {
                Text = string.Format(htmlFormat, sectionName)
            });
        }

        /// <summary>
        /// Construct the prefix of the control, only used in Admin side for form layout customization.
        /// </summary>
        /// <param name="controlName">user control name</param>
        /// <returns>the prefix</returns>
        private string ConstuctControlPrefix(string controlName)
        {
            string prefix = ((Control)_htUserControls[controlName + "Edit"]).ClientID + "_";

            if (controlName.StartsWith(PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX))
            {
                prefix += "contactEdit_";
            }
            else if (controlName.Equals(PageFlowConstant.SECTION_NAME_EDUCATION))
            {
                prefix += "EducationDetailEdit_";
            }
            else if (controlName.Equals(PageFlowConstant.SECTION_NAME_CONT_EDUCATION))
            {
                prefix += "ContEducationDetailEdit_";
            }
            else if (controlName.Equals(PageFlowConstant.SECTION_NAME_EXAMINATION))
            {
                prefix += "ucExaminationDetailEdit_";
            }
            else if (controlName.StartsWith(PageFlowConstant.SECTION_NAME_MULTI_LICENSE_PREFIX))
            {
                prefix += "licenseEdit_";
            }
            else if (controlName.StartsWith(PageFlowConstant.SECTION_NAME_ATTACHMENT_PREFIX))
            {
                //In Admin site, has hardcode one Document item to document edit list.
                //So, the "ctl00" in the prefix string is OK.
                prefix += "dlDocumentEdit_ctl00_documentEdit_";
            }

            return prefix;
        }

        /// <summary>
        /// create update panel
        /// </summary>
        /// <param name="controls">a control collection that the updatepanel added to </param>
        /// <param name="sectionName">section name</param>
        /// <param name="controlName">user control name</param>
        /// <param name="id">updatepanel id</param>
        private void CreateUpdatePanel(ControlCollection controls, string sectionName, string controlName, string id)
        {
            UpdatePanel panel = new UpdatePanel();
            panel.ID = id;
            panel.UpdateMode = UpdatePanelUpdateMode.Conditional;

            if (sectionName.StartsWith(PageFlowConstant.SECTION_NAME_ASI) && !sectionName.StartsWith(PageFlowConstant.SECTION_NAME_ASIT))
            {
                panel.UpdateMode = UpdatePanelUpdateMode.Always;
            }

            CreateControl(panel.ContentTemplateContainer.Controls, sectionName + "Edit", controlName);
            controls.Add(panel);
            _htUserControls.Add(id, panel);
        }

        /// <summary>
        /// create work location section and initial section's data.
        /// </summary>
        /// <param name="component">Component model</param>
        /// <param name="capModel">cap model information</param>
        private void InitAddress(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_address"));
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            CreateSection(PageFlowConstant.SECTION_NAME_ADDRESS, PageFlowConstant.CONTROL_NAME_ADDRESS, "per_permitReg_label_workLocation", customHeadingText, GviewID.AddressEdit, instruction);

            AddressEdit edit = (AddressEdit)_htUserControls["WorkLocationEdit"];

            edit.IsSectionRequired = CapUtil.Convert2BooleanValue(component.requiredFlag);
            edit.AddressEditCompleted += new CommonEventHandler(OnAddressEditCompleted);
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ValidateFlag = component.validateFlag;

            if (!IsPostBack)
            {
                if (StandardChoiceUtil.IsSuperAgency())
                {
                    edit.ExternalOwnerForSuperAgency = capModel.ownerModel;
                    edit.ExternalParcelForSuperAgency = capModel.parcelModel == null ? null : capModel.parcelModel.parcelModel;
                }

                if (Request.Form["GovXMLRequest"] != null)
                {
                    ACAGISModel model = SerializationUtil.XmlDeserialize(Request.Form["GovXMLRequest"], typeof(ACAGISModel)) as ACAGISModel;
                    if (string.Equals(model.CommandName, "send_address", StringComparison.CurrentCultureIgnoreCase))
                    {
                        capModel.addressModel = new AddressModel();

                        capModel.addressModel.zip = model.RefAddressModels[0].zip;
                    }
                }

                edit.DisplayAddress(capModel.addressModel, false, true);

                _isAPOLocked = edit.IsAPOLocked;

                // if one of APO condtion is locked, disabled continue button and save and resume.
                if (_isAPOLocked && StandardChoiceUtil.IsSuperAgency())
                {
                    actionBarBottom.DisableButtonByAPOLock();
                }
            }
        }

        /// <summary>
        /// Initialize the multiply ASI sections.
        /// </summary>
        /// <param name="component">The component model.</param>
        /// <param name="asiGroupModelList">The AppSpecificInfoGroupModel4 list.</param>
        /// <returns>
        /// if initialize success return true, else return false.
        /// </returns>
        private bool InitAppSpec(ComponentModel component, AppSpecificInfoGroupModel4WS[] asiGroupModelList)
        {
            if (asiGroupModelList == null || asiGroupModelList.Length == 0)
            {
                return false;
            }

            // if fee estimate, only get the fee fields
            if (_is4FeeEstimator)
            {
                asiGroupModelList = AppSpecInfoEdit.GetFeeCalculationFields(asiGroupModelList);
            }

            if (asiGroupModelList == null || asiGroupModelList.Length == 0 || !ASIBaseUC.ExistsASIFields(asiGroupModelList))
            {
                return false;
            }

            _isRedirectToFeePage = false;
            string groupInstruction = string.Empty;

            // display the ASI section.
            if (!CapUtil.IsSuperCAP(ModuleName))
            {
                groupInstruction = I18nStringUtil.GetCurrentLanguageString(asiGroupModelList[0].resGroupInstruction, asiGroupModelList[0].groupInstruction);
            }

            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_asi"));
            string sectionName = CapUtil.GetSectionName4ASI(component, asiGroupModelList);
            _dictASIGroupList.Add(sectionName, asiGroupModelList);

            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_ASI, "per_permitReg_label_appSpecificInfo", customHeadingText, null, groupInstruction);

            AppSpecInfoEdit edit = (AppSpecInfoEdit)_htUserControls[sectionName + "Edit"];
            edit.Is4FeeEstimator = _is4FeeEstimator;
            edit.IsConvertToApp = Request["IsConvertToApp"] == ACAConstant.COMMON_Y;
            edit.ASIGroupModelList = asiGroupModelList;
            edit.Display();

            return true;
        }

        /// <summary>
        /// Initialize the application specific info table.
        /// </summary>
        /// <param name="component">The component model.</param>
        /// <param name="asitGroupModelList">The AppSpecificTableGroupModel4WS list.</param>
        /// <returns>
        /// if initialize success return true, else return false.
        /// </returns>
        private bool InitAppSpecTable(ComponentModel component, AppSpecificTableGroupModel4WS[] asitGroupModelList)
        {
            if (asitGroupModelList == null || asitGroupModelList.Length == 0 || !ASIBaseUC.ExistsASITFields(asitGroupModelList))
            {
                return false;
            }

            _isRedirectToFeePage = false;
            string groupInstruction = string.Empty;

            // set ASI control and display
            if (!CapUtil.IsSuperCAP(ModuleName))
            {
                groupInstruction = I18nStringUtil.GetCurrentLanguageString(asitGroupModelList[0].resInstruction, asitGroupModelList[0].instruction);
            }

            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_asit"));
            string sectionName = CapUtil.GetSectionName4ASIT(component, asitGroupModelList);
            _dictASITGroupList.Add(sectionName, asitGroupModelList);

            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_ASIT + "List", "per_permitReg_label_appSpecInfoTable", customHeadingText, null, groupInstruction);

            AppSpecInfoTableList list = (AppSpecInfoTableList)_htUserControls[sectionName + "Edit"];
            list.SectionInfo = customHeadingText + ACAConstant.SPLIT_CHAR + component.displayOrder;
            list.ASITUIDataKey = sectionName;
            list.Display(asitGroupModelList);

            return true;
        }

        /// <summary>
        /// create applicant section and initial section's data
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel4WS">cap model including applicant</param>
        private void InitApplicant(ComponentModel component, CapModel4WS capModel4WS)
        {
            string customHeadingText = StandardChoiceUtil.GetContactTypeByKey(component.customHeading);
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX, component.componentSeqNbr);
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);

            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_CONTACT, "per_multi_contact_Label", customHeadingText, GviewID.ContactEdit, instruction);

            string key = string.Format("{0}Edit", sectionName);
            ContactEdit edit = (ContactEdit)_htUserControls[key];

            bool required = CapUtil.Convert2BooleanValue(component.requiredFlag);
            edit.ID = key;
            edit.ComponentName = sectionName;
            edit.SetSectionRequired("0", required);
            edit.ValidateFlag = component.validateFlag;
            edit.ContactExpressionType = ExpressionType.Applicant;
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ContactSectionPosition = ACAConstant.ContactSectionPosition.SpearForm;
            edit.ComponentID = (int)PageFlowComponent.APPLICANT;
            edit.ContactType = component.customHeading;
            AddParcelPKToContactAutoFill(capModel4WS.ownerModel, capModel4WS.parcelModel);

            if (!IsPostBack)
            {
                CapContactModel4WS applicant = ContactUtil.FindContactWithComponentName(capModel4WS.contactsGroup, sectionName);

                if (applicant == null && capModel4WS.IsContactsChecked4Record)
                {
                    applicant = ContactUtil.FindContactWithSectionNamePrefix(capModel4WS.contactsGroup, PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX);
                }

                if (applicant != null)
                {
                    applicant.validateFlag = component.validateFlag;
                    applicant.componentName = sectionName;
                }

                SetSingleContactType(applicant, edit);
                edit.DisplayView(applicant, component.customHeading, false);
            }

            if (!ContactUtil.IsContactTypeEnable(edit.ContactType, ContactTypeSource.Transaction, ModuleName, capModel4WS.capID.serviceProviderCode, false))
            {
                _isSingleContactValid = false;
                string errorMessage = GetTextByKey("aca_common_msg_dailycontacttype_notavailable");
                edit.ShowValidateErrorMessage(string.Format(errorMessage, edit.ContactType));
            }

            _dictComponentContactTypesList.Add(key, component.customHeading);
            _dictKeysAndNamesForMultipleApplicant.Add(key, sectionName);
            edit.ContactChanged += new CommonEventHandler(SingleContactSaved);
        }

        /// <summary>
        /// Creates the attachment section with the specified title and the specified content.
        /// </summary>
        /// <param name="component">The content of the section to create.</param>
        /// <param name="isForConditionDocument">Is for condition document or not</param>
        private void InitAttachment(ComponentModel component, bool isForConditionDocument)
        {
            string customHeadingText;
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);

            if (isForConditionDocument)
            {
                customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_conditiondocument"));
                CreateSection(PageFlowConstant.SECTION_NAME_CONDITIONDOCUMENT, PageFlowConstant.CONTROL_NAME_CONDITIONDOCUMENT, "aca_capedit_label_section_conditiondocumenttitle", customHeadingText, string.Empty, instruction);
            }
            else
            {
                customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_attachment"));
                string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_ATTACHMENT_PREFIX, component.componentSeqNbr);
                CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_ATTACHMENT, "per_attachment_Label_attachTitle", customHeadingText, GviewID.Attachment, instruction);

                string key = string.Format("{0}Edit", sectionName);
                AttachmentEdit edit = (AttachmentEdit)_htUserControls[key];
                edit.ComponentName = sectionName;
                edit.IsInSpearForm = true;
            }

            _isRedirectToFeePage = false;
        }

        /// <summary>
        /// Initializes the assets.
        /// </summary>
        /// <param name="component">The component.</param>
        private void InitAssets(ComponentModel component)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_assets"));
            string sectionName = PageFlowConstant.SECTION_NAME_ASSETS;
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_ASSETS, "aca_assetlist_label_sectiontitle", customHeadingText, null, instruction);

            string key = string.Format("{0}Edit", sectionName);
            AssetListEdit edit = _htUserControls[key] as AssetListEdit;

            if (edit != null)
            {
                edit.SetSectionRequired(ValidationUtil.IsYes(component.requiredFlag));
            }
        }

        /// <summary>
        /// create contact section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel4WS">the cap model</param>
        /// <param name="index">the index of contact</param>
        private void InitContractor(ComponentModel component, CapModel4WS capModel4WS, int index)
        {
            string sectionNamePrefix = string.Empty;
            string sectionName = string.Empty;
            string key = string.Empty;
            ExpressionType expressionType = ExpressionType.Contact_1;
            int componentType = 0;

            switch (index)
            {
                case 1:
                    expressionType = ExpressionType.Contact_1;
                    sectionNamePrefix = PageFlowConstant.SECTION_NAME_CONTACT1_PREFIX;
                    sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT1_PREFIX, component.componentSeqNbr);
                    componentType = (int)PageFlowComponent.CONTACT_1;

                    key = string.Format("{0}Edit", sectionName);
                    _dictKeysAndNamesForMultipleContact1.Add(key, sectionName);
                    break;
                case 2:
                    expressionType = ExpressionType.Contact_2;
                    sectionNamePrefix = PageFlowConstant.SECTION_NAME_CONTACT2_PREFIX;
                    sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT2_PREFIX, component.componentSeqNbr);
                    componentType = (int)PageFlowComponent.CONTACT_2;

                    key = string.Format("{0}Edit", sectionName);
                    _dictKeysAndNamesForMultipleContact2.Add(key, sectionName);
                    break;
                case 3:
                    expressionType = ExpressionType.Contact_3;
                    sectionNamePrefix = PageFlowConstant.SECTION_NAME_CONTACT3_PREFIX;
                    sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT3_PREFIX, component.componentSeqNbr);
                    componentType = (int)PageFlowComponent.CONTACT_3;

                    key = string.Format("{0}Edit", sectionName);
                    _dictKeysAndNamesForMultipleContact3.Add(key, sectionName);
                    break;
            }

            string customHeadingText = StandardChoiceUtil.GetContactTypeByKey(component.customHeading);
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_CONTACT, "per_multi_contact_Label", customHeadingText, GviewID.ContactEdit, instruction);
            bool required = CapUtil.Convert2BooleanValue(component.requiredFlag);

            ContactEdit edit = (ContactEdit)_htUserControls[key];
            edit.ID = key;
            edit.ContactExpressionType = expressionType;
            edit.ValidateFlag = component.validateFlag;
            edit.SetSectionRequired(index.ToString(), required);
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ContactSectionPosition = ACAConstant.ContactSectionPosition.SpearForm;
            edit.ComponentID = componentType;
            edit.ComponentName = sectionName;
            edit.ContactType = component.customHeading;

            AddParcelPKToContactAutoFill(capModel4WS.ownerModel, capModel4WS.parcelModel);

            if (!IsPostBack)
            {
                CapContactModel4WS contact = ContactUtil.FindContactWithComponentName(capModel4WS.contactsGroup, sectionName);

                if (contact == null && capModel4WS.IsContactsChecked4Record)
                {
                    contact = ContactUtil.FindContactWithSectionNamePrefix(capModel4WS.contactsGroup, sectionNamePrefix);
                }

                if (contact != null)
                {
                    contact.validateFlag = component.validateFlag;
                    contact.componentName = sectionName;
                }

                SetSingleContactType(contact, edit);
                edit.DisplayView(contact, component.customHeading, false);
            }

            if (!ContactUtil.IsContactTypeEnable(edit.ContactType, ContactTypeSource.Transaction, ModuleName, capModel4WS.capID.serviceProviderCode, false))
            {
                _isSingleContactValid = false;
                string errorMessage = GetTextByKey("aca_common_msg_dailycontacttype_notavailable");
                edit.ShowValidateErrorMessage(string.Format(errorMessage, edit.ContactType));
            }

            _dictComponentContactTypesList.Add(key, component.customHeading);
            edit.ContactChanged += new CommonEventHandler(SingleContactSaved);
        }

        /// <summary>
        /// create description section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model information</param>
        private void InitDescription(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_additionalinformation"));
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            CreateSection(PageFlowConstant.SECTION_NAME_ADDITIONAL_INFO, PageFlowConstant.CONTROL_NAME_ADDITIONAL_INFO, "per_permitReg_label_description", customHeadingText, GviewID.CAPDescriptionEdit, instruction);

            CapDescriptionEdit edit = (CapDescriptionEdit)_htUserControls["DescriptionEdit"];

            edit.IsOnlyShowFeeField = _is4FeeEstimator;
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);

            // convert BO to VO for presentation
            AddtionalInfo addtionalInfo = CapUtil.BuildAddtionalInfo(capModel);

            if (IsNeedShowDefaultJobValue(capModel))
            {
                //addtionalInfo.JobValue = StandardChoiceUtil.GetDefaultJobValue();
                edit.IsNewAddtionalInfor = true;
            }

            edit.DisplayAddtionalInfo(addtionalInfo);

            _isRedirectToFeePage = false;
        }

        /// <summary>
        /// create Detail information section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model information</param>
        private void InitDetailInfo(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_detailinformation"));
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            CreateSection(PageFlowConstant.SECTION_NAME_DETAIL_INFO, PageFlowConstant.CONTROL_NAME_DETAIL_INFO, "per_permitReg_label_detailinfo", customHeadingText, GviewID.DetailInformation, instruction);

            DetailInfoEdit edit = (DetailInfoEdit)_htUserControls["DetailInfoEdit"];
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);

            // convert BO to VO for presentation
            AddtionalInfo addtionalInfo = CapUtil.BuildAddtionalInfo(capModel);
            edit.DisplayAddtionalInfo(addtionalInfo);

            _isRedirectToFeePage = false;
        }

        /// <summary>
        /// create license professional section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model information</param>
        private void InitLicense(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_licenseprofessional"));
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);

            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_LICENSE_PREFIX, component.componentSeqNbr);
            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_LICENSE, "per_multi_license_Label", customHeadingText, GviewID.LicenseEdit, instruction);

            string key = string.Format("{0}Edit", sectionName);
            LicenseEdit edit = (LicenseEdit)_htUserControls[key];

            edit.IsSectionRequired = CapUtil.Convert2BooleanValue(component.requiredFlag);
            edit.ValidateFlag = component.validateFlag;
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.LicenseRemovedEvent += new CommonEventHandler(LicenseRemoved);
            edit.ComponentName = sectionName;

            _dictKeysAndNamesForMultipleLP.Add(key, sectionName);

            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(capModel.altID) && capModel.licenseProfessionalModel == null && !string.IsNullOrEmpty(capModel.licSeqNbr))
                {
                    ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
                    LicenseModel4WS lm = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, long.Parse(capModel.licSeqNbr), AppSession.User.PublicUserId);

                    if (lm != null)
                    {
                        edit.DisplayLicense(CapUtil.CreateLicenseProfessionalModel(lm));
                    }
                }
                else
                {
                    if (capModel.licenseProfessionalList == null)
                    {
                        edit.DisplayLicense(null);
                        return;
                    }

                    LicenseProfessionalModel4WS[] licenseProfessionalList = capModel.licenseProfessionalList;
                    string serviceProviderCode = capModel.capID != null ? capModel.capID.serviceProviderCode : string.Empty;
                    LicenseUtil.ResetLicenseeAgency(licenseProfessionalList, serviceProviderCode);

                    // Find the approprate license from the list of license professional list of capModel
                    LicenseProfessionalModel4WS licenseProfessional = LicenseUtil.FindLicenseProfessionalWithComponentName(capModel, sectionName);

                    // if it from clone record and not find LP, set the first LP whose component name is empty.
                    if (licenseProfessional == null && capModel.IsLicensesChecked4Record)
                    {
                        licenseProfessional = licenseProfessionalList.ToList().Find(lp => string.IsNullOrEmpty(lp.componentName));
                    }

                    if (licenseProfessional != null)
                    {
                        licenseProfessional.componentName = sectionName;
                    }

                    //initial temporary ID when control initial, because EMSE may clear the temporary ID
                    if (licenseProfessional != null && string.IsNullOrEmpty(licenseProfessional.TemporaryID))
                    {
                        licenseProfessional.TemporaryID = CommonUtil.GetRandomUniqueID();
                    }

                    edit.DisplayLicense(TempModelConvert.ConvertToLicenseProfessionalModel(licenseProfessional));
                }
            }
        }

        /// <summary>
        /// create owner section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="owner">owner model</param>
        private void InitOwner(ComponentModel component, RefOwnerModel owner)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_owner"));
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            CreateSection(PageFlowConstant.SECTION_NAME_OWNER, PageFlowConstant.CONTROL_NAME_OWNER, "per_permitReg_label_owner", customHeadingText, GviewID.OwnerEdit, instruction);

            OwnerEdit edit = (OwnerEdit)_htUserControls["OwnerEdit"];
            edit.IsSectionRequired = CapUtil.Convert2BooleanValue(component.requiredFlag);
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ValidateFlag = component.validateFlag;

            if (StandardChoiceUtil.IsSuperAgency() && owner != null && _isAPOLocked)
            {
                owner = null;
            }

            if (!IsPostBack)
            {
                edit.DisplayOwner(owner, false);
            }
        }

        /// <summary>
        /// create parcel section and initial section's data
        /// </summary>
        /// <param name="component">Component model</param>
        /// <param name="parcel">parcel model</param>
        private void InitParcel(ComponentModel component, CapParcelModel parcel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_parcel"));
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            IsParcelSectionExists = true;
            CreateSection(PageFlowConstant.SECTION_NAME_PARCEL, PageFlowConstant.CONTROL_NAME_PARCEL, "per_permitReg_label_parcel", customHeadingText, GviewID.ParcelEdit, instruction);

            ParcelEdit edit = (ParcelEdit)_htUserControls["ParcelEdit"];
            edit.IsSectionRequired = CapUtil.Convert2BooleanValue(component.requiredFlag);
            edit.ParceEditCompleted += new CommonEventHandler(OnParceEditCompleted);
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ValidateFlag = component.validateFlag;

            // if APO has locked condition,don't allow to fill APO info.
            if (StandardChoiceUtil.IsSuperAgency() && parcel != null && _isAPOLocked)
            {
                parcel = null;
            }

            if (!IsPostBack)
            {
                edit.DisplayParcel(parcel, false);
            }
        }

        /// <summary>
        /// create education section and initial section's data.
        /// </summary>
        /// <param name="component">Component model</param>
        /// <param name="capModel">education model list</param>
        private void InitEducation(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_education"));
            string sectionName = PageFlowConstant.SECTION_NAME_EDUCATION;
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);

            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_EDUCATION, "per_education_section_name", customHeadingText, null, instruction);

            string key = string.Format("{0}Edit", sectionName);
            EducationEdit edit = _htUserControls[key] as EducationEdit;
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.EducationSectionPosition = EducationOrExamSectionPosition.CapEdit;

            if (edit.IsEditable)
            {
                edit.SetSectionRequired(CapUtil.Convert2BooleanValue(component.requiredFlag));
            }

            if (capModel.educationList == null)
            {
                CapAssociateLicenseCertification4WS associateLicense = AppSession.GetCapTypeAssociateLicenseCertification();

                if (associateLicense == null || associateLicense.capAssociateEducation == null || associateLicense.capAssociateEducation.Length == 0)
                {
                    if (!IsPostBack)
                    {
                        edit.DisplayEducations(null);
                    }

                    return;
                }

                capModel.educationList = EducationUtil.ConvertToEducation4WSModelArray(associateLicense.capAssociateEducation);
            }
            else
            {
                if (capModel.educationList.Any(f => string.IsNullOrEmpty(f.entityType)))
                {
                    foreach (var edu in capModel.educationList)
                    {
                        edu.entityType = ACAConstant.CAP_EDUCATION_ENTITY_TYPE;
                    }
                }
            }

            edit.EducationsChanged += new CommonEventHandler(EducationSaved);

            if (!IsPostBack)
            {
                edit.DisplayEducations(capModel.educationList);
            }
        }

        /// <summary>
        /// create examination section and initial section's data.
        /// </summary>
        /// <param name="component">Component model</param>
        /// <param name="capModel">examination model list</param>
        private void InitExamination(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_examination"));
            string sectionName = PageFlowConstant.SECTION_NAME_EXAMINATION;
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);

            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_EXAMINATION, "examination_edit_section_name", customHeadingText, null, instruction);

            string key = string.Format("{0}Edit", sectionName);
            ExaminationEdit edit = _htUserControls[key] as ExaminationEdit;
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            edit.ExaminationSectionPosition = EducationOrExamSectionPosition.CapEdit;

            if (edit.IsEditable)
            {
                edit.IsRequired = CapUtil.Convert2BooleanValue(component.requiredFlag);
            }

            edit.DataChanged += new CommonEventHandler(ExaminationChanged);

            //DataSource Bind
            if (capModel.examinationList == null)
            {
                CapAssociateLicenseCertification4WS associateLicense = AppSession.GetCapTypeAssociateLicenseCertification();

                if (associateLicense == null || associateLicense.capAssociateExamination == null || associateLicense.capAssociateExamination.Length == 0)
                {
                    if (!IsPostBack)
                    {
                        edit.DisplayExamination(null);
                    }

                    return;
                }

                capModel.examinationList = ExaminationUtil.ConvertToExaminationModelArray(associateLicense.capAssociateExamination);
            }
            else
            {
                if (capModel.examinationList.Any(f => string.IsNullOrEmpty(f.entityType)))
                {
                    foreach (var exam in capModel.examinationList)
                    {
                        exam.entityType = ACAConstant.CAP_EXAMINATION_ENTITY_TYPE;
                    }
                }
            }

            if (!IsPostBack)
            {
                //edit.CAPOnThisExamination = capModel;
                edit.DisplayExamination(capModel.examinationList);
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
        /// Create continuing education section and initial section's data.
        /// </summary>
        /// <param name="component">component model</param>
        /// <param name="capModel">cap model contain continuing education models</param>
        private void InitContEducation(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_continuingeducation"));
            string sectionName = PageFlowConstant.SECTION_NAME_CONT_EDUCATION;
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);

            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_CONT_EDUCATION, "continuing_education_section_name", customHeadingText, null, instruction);

            string key = string.Format("{0}Edit", sectionName);
            ContinuingEducationEdit contEducationEdit = _htUserControls[key] as ContinuingEducationEdit;
            contEducationEdit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
            contEducationEdit.ContEducationSectionPosition = EducationOrExamSectionPosition.CapEdit;

            if (contEducationEdit.IsEditable)
            {
                contEducationEdit.SetContEducationSectionRequired(CapUtil.Convert2BooleanValue(component.requiredFlag));
            }

            if (capModel.contEducationList == null)
            {
                CapAssociateLicenseCertification4WS associateLicense = AppSession.GetCapTypeAssociateLicenseCertification();

                if (associateLicense == null || associateLicense.capAssociateContEducation == null || associateLicense.capAssociateContEducation.Length == 0)
                {
                    if (!IsPostBack)
                    {
                        contEducationEdit.DislayContEducations(null);
                    }

                    return;
                }

                capModel.contEducationList = EducationUtil.ConvertToContEdu4WSModelArray(associateLicense.capAssociateContEducation);
            }
            else
            {
                if (capModel.contEducationList.Any(f => string.IsNullOrEmpty(f.entityType)))
                {
                    foreach (var contEdu in capModel.examinationList)
                    {
                        contEdu.entityType = ACAConstant.CAP_CONT_EDUCATION_ENTITY_TYPE;
                    }
                }
            }

            contEducationEdit.ContEducationsChanged += new CommonEventHandler(SaveContEducation);

            if (!IsPostBack)
            {
                contEducationEdit.DislayContEducations(capModel.contEducationList);
            }
        }

        /// <summary>
        /// create calculator section and initial calculator's data.
        /// </summary>
        /// <param name="component">Component model</param>
        /// <param name="capModel">calculator model list</param>
        private void InitValuationCalculator(ComponentModel component, CapModel4WS capModel)
        {
            //Create Valuation Calculator Section
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            string customHeading = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_valuationcalculator"));
            string sectionName = PageFlowConstant.SECTION_NAME_VALUATION_CALCULATOR;
            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_VALUATION_CALCULATOR, "valuationcalculator_edit_section_name", customHeading, string.Empty, instruction);

            string key = string.Format("{0}Edit", sectionName);
            ValuationCalculatorEdit edit = (ValuationCalculatorEdit)_htUserControls[key];

            if (!AppSession.IsAdmin)
            {
                //Get reference valuation calculator and make data source setting            
                IValuationCalculatorBll valcalBLL = ObjectFactory.GetObject<IValuationCalculatorBll>();
                BCalcValuatnModel4WS[] calvals = valcalBLL.GetBCalcValuationListByCapType(capModel.capType, capModel.capID);
                edit.RefDataSource = ObjectConvertUtil.ConvertArrayToList(calvals);
                if (capModel.bCalcValuationListField == null)
                {
                    capModel.bCalcValuationListField = calvals;
                }
            }

            if (!IsPostBack)
            {
                edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);
                edit.DisplayValuationCalculator(capModel.bCalcValuationListField);
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
        /// Load Fee Form.
        /// </summary>
        /// <param name="stepList">StepModel array.</param>
        /// <param name="capModel">a CapModel4WS</param>
        private void LoadFeeForm(StepModel[] stepList, CapModel4WS capModel)
        {
            // get the ASI component list
            List<ComponentModel> asiComponentList = new List<ComponentModel>();
            List<ComponentModel> asitComponentList = new List<ComponentModel>();

            // get the ASI/ASIT component list
            foreach (StepModel step in stepList)
            {
                foreach (PageModel page in step.pageList)
                {
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
                }
            }

            PageFlowGroupModel pfGroupModel = PageFlowUtil.GetPageflowGroup(capModel);
            List<string> displayedASI = new List<string>();

            foreach (StepModel step in stepList)
            {
                foreach (PageModel page in step.pageList)
                {
                    foreach (ComponentModel component in page.componentList)
                    {
                        string asiKey = string.Format("{0}_{1}_{2}", component.componentID, component.portletRange1, component.customHeading);

                        switch (component.componentName.ToUpperInvariant())
                        {
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
                            case GViewConstant.SECTION_ADDITIONAL_INFO:
                                InitDescription(component, capModel);
                                break;
                            case GViewConstant.SECTION_VALUATION_CALCULATOR:
                                InitValuationCalculator(component, capModel);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// load sections in a page
        /// </summary>
        /// <param name="customPage"> a PageModel object</param>
        /// <param name="capModel">cap model information</param>
        private void LoadPage(PageModel customPage, CapModel4WS capModel)
        {
            CapUtil.SetLPTemporaryID(capModel);

            _currentPageModel = customPage;
            bool contactPageSkipable = false;

            if (!Page.IsPostBack)
            {
                PageFlowUtil.MarkPageAsUnLoaded(capModel, _currentStep, _currentPage);

                //page onload
                EMSEResultModel4WS emseResult = EmseUtil.ExecuteEMSE(ref capModel, customPage, EmseEventType.OnloadEvent);

                if (emseResult != null)
                {
                    if (!string.IsNullOrEmpty(emseResult.returnData))
                    {
                        string hidePage = EmseUtil.GetEMSEReturnData(emseResult, "PageFlow", "HidePage");

                        if (ValidationUtil.IsYes(hidePage))
                        {
                            SetBreadcrumbPageFlow();
                            AppSession.SetCapModelToSession(ModuleName, capModel);

                            ContinueToNextPage();
                            return;
                        }
                    }

                    _emseErrorMsg = emseResult.errorMessage;
                }

                if ((AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk) && capModel.contactsGroup != null && capModel.contactsGroup.Length != 0)
                {
                    string contactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];
                    var contactModel = capModel.contactsGroup.FirstOrDefault(o => o.refContactNumber == contactSeqNbr);

                    if (contactModel != null)
                    {
                        contactPageSkipable = string.IsNullOrEmpty(contactModel.componentName)
                                              || PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX.Equals(contactModel.componentName, StringComparison.InvariantCultureIgnoreCase);
                    }
                }

                //for trade license only
                if (!string.IsNullOrEmpty(customPage.onloadEventName))
                {
                    CopyTradeNameToTradeLicense(capModel);
                }

                if (!string.IsNullOrEmpty(_emseErrorMsg))
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, _emseErrorMsg);
                }

                AppSession.SetCapModelToSession(this.ModuleName, capModel);

                PageFlowUtil.MarkPageAsLoaded(capModel, _currentStep, _currentPage);

                //save the trace data, because if jump out to other page without click "Continue Application" button
                PageFlowUtil.UpdatePageTrace(capModel);
            }

            // get the ASI component list
            List<ComponentModel> asiComponentList = new List<ComponentModel>();
            List<ComponentModel> asitComponentList = new List<ComponentModel>();

            // get the ASI/ASIT component list
            foreach (ComponentModel component in customPage.componentList)
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
            bool isBlankPage = true;

            foreach (ComponentModel component in customPage.componentList)
            {
                string asiKey = string.Format("{0}_{1}_{2}", component.componentID, component.portletRange1, component.customHeading);

                // if only the ASI/ASIT component in the page and all initial failed, it is blank page. 
                if (!GViewConstant.SECTION_ASI.Equals(component.componentName, StringComparison.InvariantCultureIgnoreCase) &&
                   !GViewConstant.SECTION_ASIT.Equals(component.componentName, StringComparison.InvariantCultureIgnoreCase))
                {
                    isBlankPage = false;
                }

                switch (component.componentName.ToUpperInvariant())
                {
                    case GViewConstant.SECTION_ASI:
                        if (!displayedASI.Contains(asiKey))
                        {
                            AppSpecificInfoGroupModel4WS[] asiGroupModelList = CapUtil.GetASIGroupModelList(ModuleName, component, asiComponentList, pfGroupModel);
                            bool asiInitSuccess = InitAppSpec(component, asiGroupModelList);

                            // add the displayed ASI to list
                            displayedASI.Add(asiKey);

                            if (asiInitSuccess)
                            {
                                isBlankPage = false;
                            }
                        }

                        break;
                    case GViewConstant.SECTION_ASIT:
                        if (!displayedASI.Contains(asiKey))
                        {
                            AppSpecificTableGroupModel4WS[] asitGroupModelList = CapUtil.GetASITGroupModelList(ModuleName, component, asitComponentList, pfGroupModel);
                            bool asitInitSuccess = InitAppSpecTable(component, asitGroupModelList);

                            // add the displayed ASIT to list
                            displayedASI.Add(asiKey);

                            if (asitInitSuccess)
                            {
                                isBlankPage = false;
                            }
                        }

                        break;
                    case GViewConstant.SECTION_ADDITIONAL_INFO:
                        InitDescription(component, capModel);
                        break;
                    case GViewConstant.SECTION_DETAIL:
                        InitDetailInfo(component, capModel);
                        break;
                    case GViewConstant.SECTION_ADDRESS:
                        InitAddress(component, capModel);

                        if (capModel.APOConditionType == APOConditionType.Address)
                        {
                            DisplayCondition(ConditionType.Address);
                        }
                        
                        break;
                    case GViewConstant.SECTION_PARCEL:
                        InitParcel(component, capModel.parcelModel);

                        if (capModel.APOConditionType == APOConditionType.Parcel)
                        {
                            DisplayCondition(ConditionType.Parcel);
                        }
                        
                        break;
                    case GViewConstant.SECTION_OWNER:
                        InitOwner(component, capModel.ownerModel);
                        break;
                    case GViewConstant.SECTION_LICENSE:
                        InitLicense(component, capModel);
                        break;
                    case GViewConstant.SECTION_APPLICANT:
                        InitApplicant(component, capModel);
                        break;
                    case GViewConstant.SECTION_CONTACT1:
                        InitContractor(component, capModel, 1);
                        break;
                    case GViewConstant.SECTION_CONTACT2:
                        InitContractor(component, capModel, 2);
                        break;
                    case GViewConstant.SECTION_CONTACT3:
                        InitContractor(component, capModel, 3);
                        break;
                    case GViewConstant.SECTION_MULTIPLE_CONTACTS:
                        InitMultipleContacts(component, capModel);
                        break;
                    case GViewConstant.SECTION_MULTIPLE_LICENSES:
                        InitMultipleLicenses(component, capModel);
                        break;
                    case GViewConstant.SECTION_ATTACHMENT:
                        InitAttachment(component, false);
                        break;
                    case GViewConstant.SECTION_EDUCATOIN:
                        InitEducation(component, capModel);
                        break;
                    case GViewConstant.SECTION_CONTINUING_EDUCATION:
                        InitContEducation(component, capModel);
                        break;
                    case GViewConstant.SECTION_EXAMINATION:
                        InitExamination(component, capModel);
                        break;
                    case GViewConstant.SECTION_VALUATION_CALCULATOR:
                        InitValuationCalculator(component, capModel);
                        break;
                    case GViewConstant.SECTION_CUSTOM_COMPONENT:
                        InitCustomComponent(component);
                        break;
                    case GViewConstant.SECTION_CONDITION_DOCUMENT:
                        InitAttachment(component, true);
                        break;
                    case GViewConstant.SECTION_ASSETS:
                        InitAssets(component);
                        break;
                    default:
                        break;
                }
            }

            ComponentModel contactComponent = new ComponentModel();
            string isCloningRecord = Request.QueryString[ACAConstant.IS_CLONE_RECORD];
            bool isRenewal = ValidationUtil.IsYes(Request["isRenewal"]);

            // check whether need to skip this contact section page
            bool isSkipPage = (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk)
                                 && !IsPostBack
                                 && ContactUtil.IsContainContactSectionOnly(_currentStep, _currentPage, ref contactComponent)
                                 && ContactUtil.IsFirstShowContactSection(_currentStep, _currentPage)
                                 && StandardChoiceUtil.IsCustomerDetailEditable()
                                 && contactPageSkipable
                                 && !_isFromConfirmPage
                                 && !_isFromShoppingCart
                                 && !_isAmendment
                                 && !ACAConstant.COMMON_TRUE.Equals(isCloningRecord)
                                 && !isRenewal;

            if (isSkipPage)
            {
                //Validate Fields
                if (ContactUtil.ValidateRequiredFields4ContactSection(capModel, ModuleName, contactComponent))
                {
                    int stepIndex = int.Parse(Request.QueryString["stepNumber"]);

                    BreadCrumbParmsInfo hideApplicationSession = AppSession.BreadcrumbParams;

                    if (hideApplicationSession == null)
                    {
                        hideApplicationSession = new BreadCrumbParmsInfo();
                    }

                    if (hideApplicationSession.Urls.ContainsKey(stepIndex))
                    {
                        hideApplicationSession.Urls[stepIndex] = GetBackToStepPageUrl(stepIndex, 1);
                    }
                    else
                    {
                        hideApplicationSession.Urls.Add(stepIndex, GetBackToStepPageUrl(stepIndex, 1));
                    }

                    AppSession.BreadcrumbParams = hideApplicationSession;
                }
                else
                {
                    isSkipPage = false;
                }
            }

            // skip this blank page
            if (isBlankPage || isSkipPage)
            {
                ContinueToConfrim(null);
            }

            if (!IsPostBack)
            {
                AutoFillOwnerInfoForSuperAgency(capModel.parcelModel);
            }
        }

        /// <summary>
        /// redirect page to cap fee
        /// </summary>
        /// <param name="stepNumber">step number</param>
        /// <param name="pageNumber">page number.</param>
        /// <returns>the page url.</returns>
        private string GetBackToStepPageUrl(int stepNumber, int pageNumber)
        {
            string isRenewalFlag = ACAConstant.COMMON_Y == Request["isRenewal"] ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            string url = string.Format(
                                    "CapEdit.aspx?stepNumber={0}&pageNumber={1}&currentStep={2}&currentPage={3}&Module={4}{5}&isRenewal={6}&isFromShoppingCart={7}",
                                    stepNumber,
                                    pageNumber,
                                    stepNumber - 2,
                                    0,
                                    ModuleName,
                                    SourceSequenceNumber == 0 ? string.Empty : "&ssn=" + SourceSequenceNumber,
                                    isRenewalFlag,
                                    Request.QueryString[ACAConstant.FROMSHOPPINGCART]);

            return GetRedirectUrl(url);
        }

        /// <summary>
        /// Get Redirect Url
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>The redirect url.</returns>
        private string GetRedirectUrl(string url)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(url);

            if (!string.IsNullOrEmpty(Request.QueryString["FilterName"]))
            {
                sbUrl.Append("&FilterName=" + Request.QueryString["FilterName"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString[ACAConstant.IS_CLONE_RECORD]))
            {
                sbUrl.Append(ACAConstant.AMPERSAND + ACAConstant.IS_CLONE_RECORD + ACAConstant.EQUAL_MARK + Request.QueryString[ACAConstant.IS_CLONE_RECORD]);
            }

            if (ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
            {
                sbUrl.Append(ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["IsConvertToApp"]))
            {
                sbUrl.Append("&IsConvertToApp=" + Request.QueryString["IsConvertToApp"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["IsFromMap"]))
            {
                sbUrl.Append("&IsFromMap=" + Request.QueryString["IsFromMap"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.AgencyCode]))
            {
                sbUrl.Append("&" + UrlConstant.AgencyCode + "=" + Request.QueryString[UrlConstant.AgencyCode]);
            }

            // Use IsSuperAgencyAssoForm to indicates current request is come from Super Agency Associated Forms.
            if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_SUPERAGENCY_ASSOFORM]))
            {
                sbUrl.Append("&" + UrlConstant.IS_SUPERAGENCY_ASSOFORM + "=" + ACAConstant.COMMON_Y);
            }

            // For authorized agent or clerk, to auto-fill the contact info that inputed or selected in the customer detail page.
            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]))
            {
                sbUrl.Append("&" + UrlConstant.CONTACT_SEQ_NUMBER + "=" + Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]);
            }

            return sbUrl.ToString();
        }

        /// <summary>
        /// Initialize the customize component.
        /// </summary>
        /// <param name="component">The component.</param>
        private void InitCustomComponent(ComponentModel component)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_customizecomponent"));
            string virtualPath = CombineWebPath(FileUtil.CustomizeUserControlFolder, component.portletRange1);
            string physicalPath = Server.MapPath(virtualPath);

            if (AppSession.IsAdmin || !File.Exists(physicalPath))
            {
                return;
            }

            CreateLiteral(placeHolder.Controls, "<div>");

            // add the label for section title
            AccelaLabel label = new AccelaLabel();
            label.Text = customHeadingText;
            label.LabelType = LabelType.SectionExText;
            label.SubLabel = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);
            placeHolder.Controls.Add(label);

            // add the customize control
            UpdatePanel panel = new UpdatePanel();
            panel.UpdateMode = UpdatePanelUpdateMode.Conditional;
            CreateCustomizeComponentControl(panel.ContentTemplateContainer.Controls, virtualPath);
            placeHolder.Controls.Add(panel);

            CreateLiteral(placeHolder.Controls, "</div>");
        }

        /// <summary>
        /// Create the customize component control.
        /// </summary>
        /// <param name="controls">The control collection that contain the customize component control.</param>
        /// <param name="virtualPath">The customize component's virtual path.</param>
        private void CreateCustomizeComponentControl(ControlCollection controls, string virtualPath)
        {
            BaseCustomizeComponent customizeComponent = LoadControl(virtualPath) as BaseCustomizeComponent;

            if (customizeComponent != null)
            {
                controls.Add(customizeComponent);

                _htUserControls.Add(CUSTOMIZE_COMPONENT, customizeComponent);
            }
        }

        /// <summary>
        /// Is Section at current page or after.
        /// </summary>
        /// <param name="sectionName">section name</param>
        /// <returns>true or false.</returns>
        private bool IsSectionAfterCurrentPage(string sectionName)
        {
            int currentPageIndex = ASIBaseUC.GetCurrentPageIndex(_currentStep, _currentPage);
            int sectionPageIndex = ASIBaseUC.GetSectionPageIndex(sectionName);

            return currentPageIndex <= sectionPageIndex;
        }

        /// <summary>
        /// Auto Fill Owner Info into the DropdownList in Contact1,2,3 and Applicant Section for Super Agency
        /// </summary>
        /// <param name="capParcel">Cap Parcel Model</param>
        private void AutoFillOwnerInfoForSuperAgency(CapParcelModel capParcel)
        {
            // This Function is just for Super Agency
            if (!StandardChoiceUtil.IsSuperAgency() || capParcel == null || capParcel.parcelModel == null)
            {
                return;
            }

            IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();
            ParcelModel parcelPK = new ParcelModel();
            parcelPK.parcelNumber = capParcel.parcelModel.parcelNumber;
            parcelPK.UID = capParcel.parcelModel.UID;

            //For super agency parcel number has unique index, so we can get parcel model by parcel number or uid.
            ParcelModel parcel = parcelBll.GetParcelByPK(ConfigManager.AgencyCode, parcelPK);

            if (parcel == null || parcel.sourceSeqNumber == null || (string.IsNullOrEmpty(parcel.parcelNumber) && string.IsNullOrEmpty(parcel.UID)))
            {
                return;
            }

            SourceSequenceNumber = parcel.sourceSeqNumber;
        }

        /// <summary>
        /// Display Condition
        /// </summary>
        /// <param name="conditionType">Condition type.</param>
        private void DisplayCondition(ConditionType conditionType)
        {
            if (Session[SessionConstant.SESSION_APO_CONDITION] == null)
            {
                return;
            }

            ParcelModel condition = Session[SessionConstant.SESSION_APO_CONDITION] as ParcelModel;

            switch (conditionType)
            {
                case ConditionType.Address:
                    AddressEdit addressedit = (AddressEdit)_htUserControls["WorkLocationEdit"];
                    addressedit.DisplayCondition(condition.noticeConditions, condition.hightestCondition, conditionType);
                    break;
                case ConditionType.Parcel:
                    ParcelEdit parceledit = (ParcelEdit)_htUserControls["ParcelEdit"];
                    parceledit.DisplayCondition(condition.noticeConditions, condition.hightestCondition, conditionType);
                    break;
            }

            Session.Remove(SessionConstant.SESSION_APO_CONDITION);
        }

        /// <summary>
        /// response the address user control edit completed event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="arg">a hash table including address information.</param>
        private void OnAddressEditCompleted(object sender, CommonEventArgs arg)
        {
            Hashtable ht = (Hashtable)arg.ArgObject;
            string seqNum = (string)ht["SequenceNumber"];
            string parcelNumber = (string)ht["ParcelNumber"];
            string parcelUID = (string)ht["ParcelUID"];
            bool isFromMap = (bool)ht["IsFromMap"];
            string duplicateParcelKey = (string)ht["duplicateParcelKey"];
            string duplicateOwnerKey = (string)ht["duplicateOwnerKey"];

            if (!string.IsNullOrEmpty(seqNum) && (!string.IsNullOrEmpty(parcelNumber) || !string.IsNullOrEmpty(parcelUID)))
            {
                SourceSequenceNumber = long.Parse(seqNum);

                ParcelModel parcelPK = new ParcelModel();
                parcelPK.sourceSeqNumber = StringUtil.ToLong(seqNum);
                parcelPK.parcelNumber = parcelNumber;
                parcelPK.UID = parcelUID;
                IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();
                ParcelModel parcelModel = parcelBll.GetParcelByPK(ConfigManager.AgencyCode, parcelPK);

                if (!string.IsNullOrEmpty(duplicateParcelKey))
                {
                    parcelModel.duplicatedAPOKeys = JsonConvert.DeserializeObject<DuplicatedAPOKeyModel[]>(duplicateParcelKey);
                }

                if (_htUserControls.ContainsKey("ParcelEdit"))
                {
                    if (parcelModel != null)
                    {
                        ParcelEdit parcelEdit = (ParcelEdit)_htUserControls["ParcelEdit"];
                        parcelEdit.DisplayParcel(true, CapUtil.ConvertParcelModel2CapParcelModel(parcelModel), false);
                        RefreshUpdatePanel("Parcel");
                    }
                }
                else if (isFromMap && IsSectionAfterCurrentPage(GViewConstant.SECTION_PARCEL) && parcelModel != null)
                {
                    FillParcelModel(parcelModel);
                }

                UpdateOwnerSection(parcelPK, (string)ht["OwnerNumber"], (string)ht["OwnerUID"], isFromMap, seqNum, duplicateOwnerKey);
            }
        }

        /// <summary>
        /// Fill ParcelModel to CapModel4WS.
        /// </summary>
        /// <param name="parcelModel"> ParcelModel object</param>
        private void FillParcelModel(ParcelModel parcelModel)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            if (PageFlowUtil.IsComponentExist(GViewConstant.SECTION_PARCEL))
            {
                capModel.parcelModel = CapUtil.ConvertParcelModel2CapParcelModel(parcelModel);
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }
        }

        /// <summary>
        /// response the parcel user control edit completed event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="arg">a hash table including parcel information</param>
        private void OnParceEditCompleted(object sender, CommonEventArgs arg)
        {
            Hashtable ht = (Hashtable)arg.ArgObject;
            string seqNum = (string)ht["SequenceNumber"];
            bool isFromMap = (bool)ht["IsFromMap"];
            string duplicateAddressKey = (string)ht["duplicateAddressKey"];
            string duplicateOwnerKey = (string)ht["duplicateOwnerKey"];

            if (!string.IsNullOrEmpty(seqNum))
            {
                string ownerNum = (string)ht["OwnerNumber"];
                string ownerUID = (string)ht["OwnerUID"];
                SourceSequenceNumber = long.Parse(seqNum);
                OwnerModel ownerModel = null;
                IOwnerBll ownerBll = ObjectFactory.GetObject<IOwnerBll>();
                if (!string.IsNullOrEmpty(ownerNum) || !string.IsNullOrEmpty(ownerUID))
                {
                    OwnerModel ownerPK = new OwnerModel();
                    ownerPK.ownerNumber = StringUtil.ToLong(ownerNum);
                    ownerPK.sourceSeqNumber = (long?)SourceSequenceNumber;
                    ownerPK.UID = ownerUID;
                    ownerModel = ownerBll.GetOwnerByPK(ConfigManager.AgencyCode, ownerPK);
                    ownerModel.duplicatedAPOKeys = JsonConvert.DeserializeObject<DuplicatedAPOKeyModel[]>(duplicateOwnerKey);
                }

                if (_htUserControls.ContainsKey("OwnerEdit"))
                {
                    if (!string.IsNullOrEmpty(ownerNum) || !string.IsNullOrEmpty(ownerUID))
                    {
                        //if standard choice display owner is 'N' not aotu fill owner infomation by search parcel.
                        if (ownerModel != null && StandardChoiceUtil.IsDisplayOwnerSection())
                        {
                            OwnerEdit ownerEdit = (OwnerEdit)_htUserControls["OwnerEdit"];
                            ownerEdit.DisplayOwner(true, ownerModel.ToRefOwnerModel(), false);
                            RefreshUpdatePanel("Owner");
                        }
                    }

                    string parcelNumber = (string)ht["ParcelNumber"];
                    string parcelUID = (string)ht["ParcelUID"];

                    if (_currentPage == 0 && (!string.IsNullOrEmpty(parcelNumber) || !string.IsNullOrEmpty(parcelUID)))
                    {
                        ParcelModel parcelPK = new ParcelModel();
                        parcelPK.sourceSeqNumber = StringUtil.ToLong(seqNum);
                        parcelPK.parcelNumber = parcelNumber;
                        parcelPK.UID = parcelUID;

                        Session[SessionConstant.APO_SESSION_PARCELMODEL] = parcelPK;
                    }
                }
                else if (isFromMap && IsSectionAfterCurrentPage(GViewConstant.SECTION_OWNER) && ownerModel != null)
                {
                    FillOwnerModel(ownerModel);
                }

                string addressID = (string)ht["AddressID"];
                string addressUID = (string)ht["AddressUID"];

                if (!string.IsNullOrEmpty(addressID) || !string.IsNullOrEmpty(addressUID))
                {
                    RefAddressModel refAddressPK = new RefAddressModel();
                    refAddressPK.sourceNumber = StringUtil.ToInt(seqNum);
                    refAddressPK.refAddressId = StringUtil.ToLong(addressID);
                    refAddressPK.UID = addressUID;

                    IRefAddressBll addressBll = ObjectFactory.GetObject<IRefAddressBll>();
                    RefAddressModel refAddressModel = addressBll.GetAddressByPK(ConfigManager.AgencyCode, refAddressPK);
                    refAddressModel.duplicatedAPOKeys = JsonConvert.DeserializeObject<DuplicatedAPOKeyModel[]>(duplicateAddressKey);
                    if (_htUserControls.ContainsKey("WorkLocationEdit"))
                    {
                        if (refAddressModel != null)
                        {
                            AddressEdit addressEdit = (AddressEdit)_htUserControls["WorkLocationEdit"];
                            addressEdit.DisplayAddress(true, CapUtil.ConvertRefAddressModel2AddressModel(refAddressModel), false, false);
                            RefreshUpdatePanel("WorkLocation");
                        }
                    }
                    else if (isFromMap && IsSectionAfterCurrentPage(GViewConstant.SECTION_ADDRESS) && refAddressModel != null)
                    {
                        FillRefAddressModel(refAddressModel);
                    }
                }
            }
        }

        /// <summary>
        /// Parse next step and page
        /// </summary>
        /// <param name="stepNumber">step Number</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="nextStepNumber">next step number.</param>
        /// <param name="nextPageNumber">next page number.</param>
        /// <returns>If all dynamic pages have been created return true else false</returns>
        private bool ParseStepAndPage(ref int stepNumber, ref int pageNumber, int nextStepNumber, int nextPageNumber)
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

            bool isEMSEConditionExist = false;
            if (nextStepNumber > 0 && nextPageNumber > 0)
            {
                isEMSEConditionExist = true;
                _currentStep = nextStepNumber - 1;
                _currentPage = nextPageNumber - 1;

                stepNumber = nextStepNumber + BreadCrumpToolBar.GetOffsetIndex() + 1;
                pageNumber = nextPageNumber;
            }
            else
            {
                _currentPage++;
                pageNumber++;
            }

            /* if it from confirm page, and next step number and next page number are not be set or set its value larger 
             * than acture value in EMSE then go to confirm page directly.
             */
            if (_isFromConfirmPage
                && (!isEMSEConditionExist
                    || _currentStep >= pageflowGroup.stepList.Length
                    || _currentPage >= pageflowGroup.stepList[_currentStep].pageList.Length))
            {
                return true;
            }

            if (pageflowGroup.stepList[_currentStep].pageList.Length <= _currentPage)
            {
                _currentStep++;
                stepNumber++;

                // Go to next step
                _currentPage = 0;
                pageNumber = 1;

                //All dynamic page have been created.
                if (_isFromConfirmPage || pageflowGroup.stepList.Length <= _currentStep)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// parse url argument
        /// </summary>
        private void ParseUrlArg()
        {
            _is4FeeEstimator = Request["isFeeEstimator"] == ACAConstant.COMMON_Y;
            _isRedirectToFeePage = _is4FeeEstimator;

            if (!string.IsNullOrEmpty(Request[UrlConstant.IS_FROM_CONFIRMPAGE]))
            {
                _isFromConfirmPage = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_CONFIRMPAGE]);
            }

            if (!string.IsNullOrEmpty(Request["isFromShoppingCart"]))
            {
                _isFromShoppingCart = Request["isFromShoppingCart"] == ACAConstant.COMMON_Y;
            }

            if (!string.IsNullOrEmpty(Request["isAmendment"]))
            {
                _isAmendment = ValidationUtil.IsYes(Request["isAmendment"]);
            }

            _currentStep = string.IsNullOrEmpty(Request["currentStep"]) ? 0 : int.Parse(Request["currentStep"]);
            _currentPage = string.IsNullOrEmpty(Request["currentPage"]) ? 0 : int.Parse(Request["currentPage"]);
            _stepNumber = string.IsNullOrEmpty(Request.QueryString["stepNumber"]) ? 0 : int.Parse(Request.QueryString["stepNumber"]);
            _pageNumber = string.IsNullOrEmpty(Request.QueryString["pageNumber"]) ? 0 : int.Parse(Request.QueryString["pageNumber"]);

            string ssn = Request["ssn"];

            if (!string.IsNullOrEmpty(ssn))
            {
                SourceSequenceNumber = long.Parse(ssn);
            }
        }

        /// <summary>
        /// redirect page to cap fee
        /// </summary>
        /// <param name="convert2App">if it is a application converted from fee estimate</param>
        /// <param name="stepNumber">the step No</param>
        private void RedirectToCapFeePage(string convert2App, int stepNumber)
        {
            try
            {
                Response.Redirect(string.Format("CapFees.aspx?sourceFrom={0}&stepNumber={1}&Module={2}&isFeeEstimator={3}", convert2App, stepNumber, this.ModuleName, ACAConstant.COMMON_Y), true);
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// redirect to confirm page
        /// </summary>
        private void RedirectToConfirmPage()
        {
            // Before navigating to the confirm page, we need to remove the data of a component which resides in the hidden page.
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            CleanUpCapModel(capModel);

            //Clear the capModel.licenseProfessionalModel.
            capModel.licenseProfessionalModel = null;

            if (_is4FeeEstimator)
            {
                AppSession.SetPageflowGroupToSession(null);
                PageFlowUtil.GetPageflowGroup(capModel);
            }

            int stepNumber = 0;

            if (_isFromConfirmPage)
            {
                stepNumber = string.IsNullOrEmpty(Request["confirmStepNumber"]) ? 0 : int.Parse(Request["confirmStepNumber"]);
            }
            else
            {
                stepNumber = int.Parse(Request.QueryString["stepNumber"]) + 1;
            }

            string isRenewalFlag = ACAConstant.COMMON_Y == Request["isRenewal"] ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            string url = string.Format("CapConfirm.aspx?stepNumber={0}&pageNumber=1&Module={1}&isRenewal={2}&isFromShoppingCart={3}", stepNumber, ModuleName, isRenewalFlag, Request.QueryString[ACAConstant.FROMSHOPPINGCART]);

            // the parameter is used for back to CapEdit from CapConfirm when click 'Edit' button
            if (SourceSequenceNumber != 0)
            {
                url += "&ssn=" + SourceSequenceNumber;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["FilterName"]))
            {
                url = url + "&FilterName=" + Request.QueryString["FilterName"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.AgencyCode]))
            {
                url += "&" + UrlConstant.AgencyCode + "=" + Request.QueryString[UrlConstant.AgencyCode];
            }

            if (CloneRecordUtil.IsCloneRecord(Request))
            {
                url += ACAConstant.AMPERSAND + ACAConstant.IS_CLONE_RECORD + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_TRUE;
            }

            if (ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
            {
                url += ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
            }

            // Use IsSuperAgencyAssoForm to indicates current request is come from Super Agency Associated Forms.
            if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_SUPERAGENCY_ASSOFORM]))
            {
                url += "&" + UrlConstant.IS_SUPERAGENCY_ASSOFORM + "=" + ACAConstant.COMMON_Y;
            }

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]))
            {
                url += "&" + UrlConstant.CONTACT_SEQ_NUMBER + "=" + Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];
            }

            Response.Redirect(url);
        }

        /// <summary>
        /// redirect page to cap fee
        /// </summary>
        /// <param name="stepNumber">step number</param>
        /// <param name="pageNumber">page number.</param>
        private void RedirectToEditPage(int stepNumber, int pageNumber)
        {
            string isRenewalFlag = ACAConstant.COMMON_Y == Request["isRenewal"] ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            int confirmStepNumber = string.IsNullOrEmpty(Request["confirmStepNumber"]) ? 0 : int.Parse(Request["confirmStepNumber"]);
            string url = string.Format(
                "CapEdit.aspx?stepNumber={0}&pageNumber={1}&currentStep={2}&currentPage={3}&Module={4}{5}&isRenewal={6}&isFromShoppingCart={7}&{8}={9}&confirmStepNumber={10}&isFromConfirmPage=N",
                stepNumber,
                pageNumber,
                _currentStep,
                _currentPage,
                ModuleName,
                SourceSequenceNumber == 0 ? string.Empty : "&ssn=" + SourceSequenceNumber,
                isRenewalFlag,
                Request.QueryString[ACAConstant.FROMSHOPPINGCART],
                UrlConstant.IS_FROM_CONFIRMPAGE,
                Request[UrlConstant.IS_FROM_CONFIRMPAGE],
                confirmStepNumber);

            url = GetRedirectUrl(url);

            if (!string.IsNullOrEmpty(Request.QueryString["isAmendment"]))
            {
                url += "&isAmendment=" + Request.QueryString["isAmendment"];
            }

            Response.Redirect(url);
        }

        /// <summary>
        /// cause a updatepanel to refresh in client
        /// </summary>
        /// <param name="sectionName">the name of the section need be refreshed</param>
        private void RefreshUpdatePanel(string sectionName)
        {
            UpdatePanel up = (UpdatePanel)_htUserControls["UpdatePanel" + sectionName];
            up.Update();
        }

        /// <summary>
        /// save partial cap model and redirect to cap home page
        /// </summary>
        private void SaveAndResume()
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            bool isValidateForLicense = LicenseUtil.IsReferenceDataForLicense(_htUserControls, ModuleName, _dictKeysAndNamesForMultipleLP, _dictKeysAndNamesForLPList);
            bool isValidateForContact = ContactUtil.IsReferenceDataForContact(
                                                        _htUserControls,
                                                        capModel,
                                                        _dictKeysAndNamesForMultipleApplicant,
                                                        _dictKeysAndNamesForMultipleContact1,
                                                        _dictKeysAndNamesForMultipleContact2,
                                                        _dictKeysAndNamesForMultipleContact3,
                                                        _dictKeysAndNamesForMultipleContactList);

            if (!isValidateForLicense || !isValidateForContact)
            {
                return;
            }

            BaseCustomizeComponent customizeComponent = (BaseCustomizeComponent)_htUserControls[CUSTOMIZE_COMPONENT];

            // execute customize component's [SaveAndResumeBefore] action
            if (customizeComponent != null)
            {
                ResultMessage result = customizeComponent.SaveAndResumeBefore();

                if (!result.IsSuccess)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, result.Message);
                }
            }

            try
            {
                string errMsg = SetupConfirmData(false);

                // execute customize component's [SaveAndResume] action
                if (customizeComponent != null)
                {
                    ResultMessage result = customizeComponent.SaveAndResume();

                    if (!result.IsSuccess)
                    {
                        MessageUtil.ShowMessage(Page, MessageType.Error, result.Message);
                    }
                }

                if (!string.IsNullOrEmpty(errMsg))
                {
                    HandleSubmitException(errMsg);
                    return;
                }

                CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(this.ModuleName);
                capModel4WS = CapUtil.ConstructCapModel(capModel4WS, this.ModuleName, Request["isRenewal"]);
                AppSession.SetCapModelToSession(ModuleName, capModel4WS);

                if (capModel4WS.altID != null)
                {
                    capModel4WS.capClass = ACAConstant.INCOMPLETE_CAP;

                    capModel4WS = UpdatePartialCap(capModel4WS);

                    EmseUtil.TriggerEMSESaveAndResume(capModel4WS);
                }

                //remove ACAGISModel
                GISUtil.RemoveACAGISModelFromSession(ModuleName);

                // execute customize component's [SaveAndResumeAfter] action
                if (customizeComponent != null)
                {
                    ResultMessage result = customizeComponent.SaveAndResumeAfter();

                    if (!result.IsSuccess)
                    {
                        MessageUtil.ShowMessage(Page, MessageType.Error, result.Message);
                    }
                }

                CapUtil.SaveResumeRedirect(Response, ModuleName, capModel4WS.capID, Request["isRenewal"]);
            }
            catch (ACAException err)
            {
                HandleSubmitException(err.Message);
            }
        }

        /// <summary>
        /// update partial cap model
        /// </summary>
        private void SavePartialCapModel()
        {
            CapModel4WS feeEstimateModel = AppSession.GetCapModelFromSession(this.ModuleName);

            CapModel4WS capModel4WS = CapUtil.ConstructCapModel(feeEstimateModel, this.ModuleName, Request["isRenewal"]);

            if (capModel4WS.capClass != ACAConstant.INCOMPLETE_TEMP_CAP)
            {
                if (!_is4FeeEstimator)
                {
                    capModel4WS.capClass = ACAConstant.INCOMPLETE_CAP;
                }
                else
                {
                    capModel4WS.capClass = ACAConstant.INCOMPLETE;
                }
            }

            if (capModel4WS.capClass == ACAConstant.INCOMPLETE_TEMP_CAP)
            {
                //This partial cap will don't display in ACA, unless public user click save and resume button to save.
                capModel4WS.accessByACA = ACAConstant.COMMON_N;
            }
            else
            {
                capModel4WS.accessByACA = ACAConstant.COMMON_Y;
            }

            UpdatePartialCap(capModel4WS);

            AppSession.SetCapModelToSession(ModuleName, capModel4WS);
        }

        /// <summary>
        /// setup APO data
        /// </summary>
        /// <param name="capModel4WS">a cap model</param>
        /// <param name="needValidate">if need to check validate of APO and license info</param>
        /// <returns>if validate failed,return error message,otherwise return null</returns>
        private string SetupAPOData(CapModel4WS capModel4WS, bool needValidate)
        {
            if (_htUserControls.ContainsKey("ParcelEdit"))
            {
                ParcelEdit parcelEdit = (ParcelEdit)_htUserControls["ParcelEdit"];

                if (!needValidate || parcelEdit.ValidateParcel())
                {
                    capModel4WS.parcelModel = parcelEdit.GetParcel(capModel4WS.parcelModel);
                }
                else
                {
                    return GetTextByKey("per_parcel_error_searchClickedRequired");
                }
            }
            else if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_PARCEL))
            {
                capModel4WS.parcelModel = null;
            }

            if (_htUserControls.ContainsKey("OwnerEdit"))
            {
                OwnerEdit ownerEdit = (OwnerEdit)_htUserControls["OwnerEdit"];

                if (!needValidate || ownerEdit.ValidateOwner())
                {
                    capModel4WS.ownerModel = ownerEdit.GetOwner(capModel4WS.ownerModel);
                }
                else
                {
                    return GetTextByKey("per_owner_error_searchClickedRequired");
                }
            }
            else if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_OWNER))
            {
                capModel4WS.ownerModel = null;
            }

            if (_htUserControls.ContainsKey("WorkLocationEdit"))
            {
                AddressEdit addressEdit = (AddressEdit)_htUserControls["WorkLocationEdit"];

                if (!needValidate || addressEdit.ValidateAddress())
                {
                    capModel4WS.addressModel = addressEdit.GetAddressModel(capModel4WS.addressModel);
                }
                else
                {
                    return GetTextByKey("per_workLocation_error_searchClickedRequired");
                }
            }
            else if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ADDRESS))
            {
                capModel4WS.addressModel = null;
            }

            return null;
        }

        /// <summary>
        /// setup ASI data
        /// </summary>
        /// <param name="capModel4WS">a cap model.</param>
        /// <param name="needValidate">need validate or not.</param>
        /// <returns>The error message if occur error.</returns>
        private string SetupASIData(CapModel4WS capModel4WS, bool needValidate)
        {
            string errMsg = string.Empty;

            // save ASI data
            if (PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ASI))
            {
                foreach (string key in _dictASIGroupList.Keys)
                {
                    string controlId = key + "Edit";
                    if (_htUserControls.ContainsKey(controlId))
                    {
                        ((AppSpecInfoEdit)_htUserControls[controlId]).SaveAppSpecInfo(capModel4WS, _dictASIGroupList[key]);
                    }
                }
            }
            else
            {
                capModel4WS.appSpecificInfoGroups = null;
            }

            // save ASIT data
            if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ASIT))
            {
                capModel4WS.appSpecTableGroups = null;
            }
            else
            {
                errMsg = SetupASITData(capModel4WS, needValidate);
            }

            if (_htUserControls.ContainsKey("DescriptionEdit"))
            {
                CapDescriptionEdit capDescription = (CapDescriptionEdit)_htUserControls["DescriptionEdit"];
                AddtionalInfo addtionalInfo = capDescription.GetAdditionalInfo(capModel4WS.bvaluatnModel);
                addtionalInfo.JobValueModel.capID = capModel4WS.capID;
                capModel4WS.bvaluatnModel = addtionalInfo.JobValueModel;

                if (!_is4FeeEstimator)
                {
                    CapDetailModel capDetailModel = capModel4WS.capDetailModel;

                    if (capDetailModel == null)
                    {
                        capDetailModel = new CapDetailModel();
                    }

                    //capDetailModel.shortNotes = addtionalInfo.GeneralDesc;
                    if (string.IsNullOrEmpty(addtionalInfo.BuildingNumber))
                    {
                        capDetailModel.buildingCount = null;
                    }
                    else
                    {
                        capDetailModel.buildingCount = Convert.ToInt64(addtionalInfo.BuildingNumber);
                    }

                    if (string.IsNullOrEmpty(addtionalInfo.HousingUnit))
                    {
                        capDetailModel.houseCount = null;
                    }
                    else
                    {
                        capDetailModel.houseCount = Convert.ToInt64(addtionalInfo.HousingUnit);
                    }

                    capDetailModel.publicOwned = addtionalInfo.PublicOwner;
                    capDetailModel.constTypeCode = addtionalInfo.ConstructionType;
                    capDetailModel.auditUser = AppSession.User.PublicUserId;
                    capModel4WS.capDetailModel = capDetailModel;
                }
            }
            else if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ADDITIONAL_INFO))
            {
                //every cap must have one cap detail, don't allow set it null.
                //capModel4WS.capDetailModel = null; // error set
                CapDetailModel capDetailModel = capModel4WS.capDetailModel;

                if (capDetailModel == null)
                {
                    capDetailModel = new CapDetailModel();
                }

                //clear job value.
                capModel4WS.bvaluatnModel = null;

                //capDetailModel.shortNotes = null;   //general description
                capDetailModel.houseCount = null;
                capDetailModel.buildingCount = null;
                capDetailModel.publicOwned = null;
                capDetailModel.constTypeCode = null;
                capDetailModel.auditUser = AppSession.User.PublicUserId;
                capDetailModel.auditStatus = ACAConstant.VALID_STATUS;
                capModel4WS.capDetailModel = capDetailModel;
            }

            if (_htUserControls.ContainsKey("DetailInfoEdit"))
            {
                DetailInfoEdit detailInfo = (DetailInfoEdit)_htUserControls["DetailInfoEdit"];
                AddtionalInfo addtionalInfo = detailInfo.GetAddtionalInfo(capModel4WS.bvaluatnModel);

                CapDetailModel capDetailModel = capModel4WS.capDetailModel;

                if (capDetailModel == null)
                {
                    capDetailModel = new CapDetailModel();
                }

                capDetailModel.shortNotes = addtionalInfo.GeneralDesc;
                capDetailModel.auditUser = AppSession.User.PublicUserId;
                capModel4WS.capDetailModel = capDetailModel;

                CapWorkDesModel4WS capWorkModel = capModel4WS.capWorkDesModel;

                if (capWorkModel == null)
                {
                    capWorkModel = new CapWorkDesModel4WS();
                }

                capWorkModel.description = addtionalInfo.DetailedDesc;
                capWorkModel.auditID = AppSession.User.PublicUserId;
                capModel4WS.capWorkDesModel = capWorkModel;
                capModel4WS.specialText = addtionalInfo.ApplicationName;
            }
            else if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_DETAIL))
            {
                //every cap must have one cap detail, don't allow set it null.
                CapDetailModel capDetailModel = capModel4WS.capDetailModel;

                if (capDetailModel == null)
                {
                    capDetailModel = new CapDetailModel();
                }

                capDetailModel.auditUser = AppSession.User.PublicUserId;
                capDetailModel.auditStatus = ACAConstant.VALID_STATUS;
                capModel4WS.capDetailModel = capDetailModel;
                capModel4WS.capWorkDesModel = null; //detail description
            }

            return errMsg;
        }

        /// <summary>
        /// Fill the "rowIndex" and "tableFieldValues" properties for each ASI Table.
        /// </summary>
        /// <param name="capModel">Cap model.</param>
        /// <param name="needValidate">A flag to indicates if need to validate required fields.</param>
        /// <returns>The error message.</returns>
        [Obsolete("This method only provides forward compatibility for the EMSE.")]
        private string SetupASITData(CapModel4WS capModel, bool needValidate)
        {
            if (_dictASITGroupList == null || _dictASITGroupList.Count < 1)
            {
                return null;
            }

            List<AppSpecificTableGroupModel4WS> appSpecTableGroupList = new List<AppSpecificTableGroupModel4WS>();

            // Find current page ASIT group in the passed Cap Model.
            foreach (var dictAsitGroup in _dictASITGroupList)
            {
                foreach (var asitGroup in dictAsitGroup.Value)
                {
                    AppSpecificTableGroupModel4WS foundAsitGroup = capModel.appSpecTableGroups.SingleOrDefault(g =>
                        g.capIDModel != null && g.capIDModel.Equals(asitGroup.capIDModel)
                        && string.Equals(g.groupName, asitGroup.groupName, StringComparison.OrdinalIgnoreCase));

                    appSpecTableGroupList.Add(foundAsitGroup);
                }
            }

            var asiTables = CapUtil.GetAllVisibleASITables(ModuleName, appSpecTableGroupList.ToArray());

            if (asiTables != null && asiTables.Length > 0)
            {
                foreach (var table in asiTables)
                {
                    table.rowIndex = null;
                    table.tableFieldValues = null;

                    if (table.columns != null && table.columns.Length > 0 && table.tableField != null && table.tableField.Length > 0)
                    {
                        var fieldIndexes = new List<string>();
                        var fieldValues = new List<string>();
                        int columnSize = table.columns.Length;
                        int rowIdx = 0;
                        int colIdx = 0;

                        foreach (var field in table.tableField)
                        {
                            fieldIndexes.Add(rowIdx.ToString());
                            colIdx++;

                            if (colIdx == columnSize)
                            {
                                rowIdx++;
                                colIdx = 0;
                            }

                            fieldValues.Add(field != null ? field.inputValue : null);
                        }

                        table.rowIndex = fieldIndexes.ToArray();
                        table.tableFieldValues = fieldValues.ToArray();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// setup Education data.
        /// </summary>
        /// <param name="capModel">cap model.</param>
        private void SetupEducationData(CapModel4WS capModel)
        {
            string key = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EDUCATION);

            if (_htUserControls.ContainsKey(key))
            {
                EducationEdit educationEdit = (EducationEdit)_htUserControls[key];
                capModel.educationList = educationEdit.GetEducationModelList();
            }
        }

        /// <summary>
        /// Setup continuing education data to Cap model.
        /// </summary>
        /// <param name="capModel">cap model.</param>
        private void SetupContEducationData(CapModel4WS capModel)
        {
            string key = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_CONT_EDUCATION);

            if (_htUserControls.ContainsKey(key))
            {
                ContinuingEducationEdit contEducationEdit = (ContinuingEducationEdit)_htUserControls[key];
                capModel.contEducationList = contEducationEdit.GetContEducations();
            }
        }

        /// <summary>
        /// setup valuation calculators data
        /// </summary>
        /// <param name="capModel">cap model.</param>
        private void SetupValuationCalculatorData(CapModel4WS capModel)
        {
            string key = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_VALUATION_CALCULATOR);

            if (_htUserControls.ContainsKey(key))
            {
                ValuationCalculatorEdit edit = (ValuationCalculatorEdit)_htUserControls[key];
                capModel.bCalcValuationListField = edit.GetCalValuationModel();
            }
        }

        /// <summary>
        /// setup Examination data.
        /// </summary>
        /// <param name="capModel">cap model.</param>
        private void SetupExaminationData(CapModel4WS capModel)
        {
            string key = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EXAMINATION);

            if (_htUserControls.ContainsKey(key))
            {
                ExaminationEdit examinationEdit = (ExaminationEdit)_htUserControls[key];
                capModel.examinationList = (examinationEdit.DataSource as List<ExaminationModel>).ToArray();
            }
        }

        /// <summary>
        /// Setup clone CapModel
        /// </summary>
        /// <param name="cloneCapModel">clone CapModel</param>
        /// <param name="needValidate">Is need Validate</param>
        /// <returns>string for setup clone</returns>
        private string SetupCloneCapModel(CapModel4WS cloneCapModel, bool needValidate)
        {
            string errMsg = SetupAPOData(cloneCapModel, needValidate);

            if (!string.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            if (Request.QueryString["FilterName"] != ACAConstant.REQUEST_PARMETER_TRADE_LICENSE)
            {
                errMsg = SetupLicenseData(cloneCapModel);
            }

            if (!string.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            if (!string.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            errMsg = SetupASIData(cloneCapModel, needValidate);

            if (!string.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            if (needValidate && !AttachmentUtil.IsDisabledEDMS(ConfigManager.AgencyCode, ModuleName))
            {
                errMsg = ValidateRequiredDocument(cloneCapModel);

                if (!string.IsNullOrEmpty(errMsg))
                {
                    return errMsg;
                }
            }

            if (needValidate)
            {
                errMsg = ValidateContactType(cloneCapModel);

                if (!string.IsNullOrEmpty(errMsg))
                {
                    return errMsg;
                }
            }

            SetupEducationData(cloneCapModel);

            SetupContEducationData(cloneCapModel);

            SetupExaminationData(cloneCapModel);

            SetupValuationCalculatorData(cloneCapModel);

            return null;
        }

        /// <summary>
        /// construct the cap module to session
        /// </summary>
        /// <param name="needValidate">if need to check validate of APO and license info</param>
        /// <returns>if validate failed,return error message,otherwise return null</returns>
        private string SetupConfirmData(bool needValidate)
        {
            CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(this.ModuleName);
            CapModel4WS cloneCapModel = (CapModel4WS)capModel4WS.Clone();

            string errMsg = SetupCloneCapModel(cloneCapModel, needValidate);

            if (!string.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            AppSession.SetCapModelToSession(ModuleName, cloneCapModel);

            return null;
        }

        /// <summary>
        /// construct the cap module to session
        /// </summary>
        /// <returns>if validate failed,return error message,otherwise return null</returns>
        private string SetupConfirmData4EMSE()
        {
            CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(this.ModuleName);

            CapModel4WS cloneCapModel = (CapModel4WS)capModel4WS.Clone();

            string errorMsg = SetupCloneCapModel(cloneCapModel, true);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return errorMsg;
            }

            //Before Click EMSE Event
            EMSEResultModel4WS emseResult = EmseUtil.ExecuteEMSE(ref cloneCapModel, _currentPageModel, EmseEventType.BeforeButtonEvent);

            if (emseResult != null)
            {
                _emseErrorMsg = emseResult.errorMessage;
            }

            //EMSE script error
            if (string.IsNullOrEmpty(_emseErrorMsg) || _emseErrorMsg.StartsWith(ACAConstant.SSNORFEIN_ERRORCODE))
            {
                capModel4WS = cloneCapModel;
                AppSession.SetCapModelToSession(ModuleName, capModel4WS);
            }

            return null;
        }

        /// <summary>
        /// validate the contact type and show message.
        /// </summary>
        /// <param name="capModel">The CapModel</param>
        /// <returns>Return the error message.</returns>
        private string ValidateContactType(CapModel4WS capModel)
        {
            bool existsContactListComponent = false;

            if (_dictKeysAndNamesForMultipleContactList != null && _dictKeysAndNamesForMultipleContactList.Count > 0)
            {
                if (_dictKeysAndNamesForMultipleContactList.Keys.Any(key => _htUserControls.ContainsKey(key)))
                {
                    existsContactListComponent = true;
                }
            }

            if (!existsContactListComponent)
            {
                return null;
            }

            string errorMsg = string.Empty;
            PageFlowGroupModel pageFlow = PageFlowUtil.GetPageflowGroup(capModel);
            string agencyCode = pageFlow != null ? pageFlow.serviceProviderCode : ConfigManager.AgencyCode;
            string pageflowName = pageFlow != null ? pageFlow.pageFlowGrpCode : null;

            foreach (string componentName in _dictKeysAndNamesForMultipleContactList.Values)
            {
                XEntityPermissionModel xentity = new XEntityPermissionModel();
                xentity.servProvCode = agencyCode;
                xentity.entityType = XEntityPermissionConstant.CONTACT_DATA_VALIDATION;
                xentity.entityId = ModuleName;
                xentity.entityId2 = pageflowName;
                xentity.componentName = componentName;

                List<ContactTypeUIModel> contactTypeUIModels = DropDownListBindUtil.GetContactTypesByXEntity(xentity, true);

                if (contactTypeUIModels == null || contactTypeUIModels.Count == 0)
                {
                    continue;
                }

                // check the min contact type count
                foreach (ContactTypeUIModel contactTypeUIModel in contactTypeUIModels)
                {
                    if (string.IsNullOrEmpty(contactTypeUIModel.MinNum))
                    {
                        continue;
                    }

                    IEnumerable<CapContactModel4WS> capContactTypes = null;
                    CapContactModel4WS[] capContacts = capModel.contactsGroup;

                    if (capContacts != null && capContacts.Length > 0)
                    {
                        capContactTypes = capContacts.Where(c => c.people.contactType.Equals(contactTypeUIModel.Key, StringComparison.InvariantCultureIgnoreCase)
                                                                                 && c.componentName.Equals(componentName, StringComparison.InvariantCultureIgnoreCase));
                    }

                    if (capContactTypes == null || capContactTypes.Count() < int.Parse(contactTypeUIModel.MinNum))
                    {
                        errorMsg = GetTextByKey("aca_validate_contacttype_message");

                        return errorMsg;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Validate required document.
        /// </summary>
        /// <param name="capModel">a cap model.</param>
        /// <returns>The error message.</returns>
        private string ValidateRequiredDocument(CapModel4WS capModel)
        {
            StringBuilder errorMessage = new StringBuilder();

            try
            {
                if (_htUserControls.ContainsKey(PageFlowConstant.CONTROL_NAME_CONDITIONDOCUMENT + "Edit"))
                {
                    IEDMSDocumentBll edmsDocBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                    List<CapConditionModel4WS> capConditions = ConditionsUtil.GetFilteredCapConditions(capModel.capID);

                    if (capConditions == null || !capConditions.Any())
                    {
                        return string.Empty;
                    }

                    CapIDModel capId = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);
                    DocumentModel[] docList = edmsDocBll.GetRecordDocumentList(capId.serviceProviderCode, ModuleName, AppSession.User.PublicUserId, capId, true);
                    IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();

                    foreach (CapConditionModel4WS capCondition in capConditions)
                    {
                        string conditionGroup = bizDomainBll.GetConditionGroupFor18N(capId.serviceProviderCode, capCondition.conditionGroup);

                        if (docList == null
                            || docList.Length == 0
                            || docList.All(d => d.conditionNumber != capCondition.conditionNumber))
                        {
                            errorMessage.Append(conditionGroup + ACAConstant.SPLITLINE + capCondition.dispConditionDescription + ACAConstant.HTML_BR);
                        }
                    }
                }

                if (errorMessage.Length > 0)
                {
                    errorMessage.Insert(0, LabelUtil.GetTextByKey("aca_validate_requireddocument_message", ModuleName) + ACAConstant.HTML_BR);
                }
            }
            catch (ACAException e)
            {
                errorMessage.Append(e.Message);
            }

            return errorMessage.ToString();
        }

        /// <summary>
        /// setup license data
        /// </summary>
        /// <param name="capModel4WS">a cap model.</param>
        /// <returns>if validate failed,return error message,otherwise return null</returns>
        private string SetupLicenseData(CapModel4WS capModel4WS)
        {
            string errorMsg = string.Empty;

            if (!PageFlowUtil.IsComponentExist(GViewConstant.SECTION_LICENSE))
            {
                capModel4WS.licenseProfessionalModel = null;
                capModel4WS.licSeqNbr = null;
            }

            //When locate the LP section or LP List section on Spear Form at first time, the capModel4WS.licenseProfessionalList is null.
            if (capModel4WS.licenseProfessionalList == null || CapUtil.IsSuperCAP(ModuleName))
            {
                return string.Empty;
            }

            //Check the duplicate LP record entered in each LP/LP List section on Spear Form
            for (int i = 0; i < capModel4WS.licenseProfessionalList.Length - 1; i++)
            {
                LicenseProfessionalModel4WS licenseRecord = capModel4WS.licenseProfessionalList[i];

                for (int j = i + 1; j < capModel4WS.licenseProfessionalList.Length; j++)
                {
                    if (capModel4WS.licenseProfessionalList[j].licenseType == licenseRecord.licenseType
                        && capModel4WS.licenseProfessionalList[j].licenseNbr == licenseRecord.licenseNbr)
                    {
                        errorMsg = GetTextByKey("aca_capedit_msg_duplicatelicensedprofessional");
                        return errorMsg;
                    }
                }
            }

            return errorMsg;
        }

        /// <summary>
        /// update owner section
        /// </summary>
        /// <param name="parcelPK">ParcelModel model with key value.</param>
        /// <param name="ownerNumber">Owner reference number.</param>
        /// <param name="ownerUID">Owner unique ID for supporting XAPO.</param>
        /// <param name="isFromMap">is from map or not.</param>
        /// <param name="sourceSeqNum">Source sequence number.</param>
        /// <param name="duplicateOwnerKey">The duplicate owner key.</param>
        private void UpdateOwnerSection(ParcelModel parcelPK, string ownerNumber, string ownerUID, bool isFromMap, string sourceSeqNum, string duplicateOwnerKey)
        {
            if (string.IsNullOrEmpty(ownerNumber) && string.IsNullOrEmpty(ownerUID))
            {
                return;
            }

            OwnerModel ownerPK = new OwnerModel();
            ownerPK.ownerNumber = StringUtil.ToLong(ownerNumber);
            ownerPK.sourceSeqNumber = long.Parse(sourceSeqNum);
            ownerPK.UID = ownerUID;

            IOwnerBll ownerBll = ObjectFactory.GetObject<IOwnerBll>();
            OwnerModel ownerModel = ownerBll.GetOwnerByPK(ConfigManager.AgencyCode, ownerPK);

            if (ownerModel == null)
            {
                return;
            }

            ownerModel.duplicatedAPOKeys = JsonConvert.DeserializeObject<DuplicatedAPOKeyModel[]>(duplicateOwnerKey);

            Session[SessionConstant.APO_SESSION_PARCELMODEL] = parcelPK;

            string controlKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_OWNER);

            if (_htUserControls.ContainsKey(controlKey))
            {
                //if standard choice display owner is 'N' not aotu fill owner infomation by search address.
                if (StandardChoiceUtil.IsDisplayOwnerSection())
                {
                    OwnerEdit ownerEdit = (OwnerEdit)_htUserControls[controlKey];
                    ownerEdit.DisplayOwner(true, ownerModel.ToRefOwnerModel(), false);
                    RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_OWNER);
                }
            }
            else if (isFromMap && IsSectionAfterCurrentPage(GViewConstant.SECTION_OWNER))
            {
                FillOwnerModel(ownerModel);
            }
        }

        /// <summary>
        /// Fill OwnerModel to CapModel4WS
        /// </summary>
        /// <param name="ownerModel">OwnerModel object</param>
        private void FillOwnerModel(OwnerModel ownerModel)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            if (PageFlowUtil.IsComponentExist(GViewConstant.SECTION_OWNER))
            {
                capModel.ownerModel = CapUtil.ConvertOwnerModel2RefOwnerModel(ownerModel);
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }
        }

        /// <summary>
        /// Fill OwnerModel to CapModel4WS
        /// </summary>
        /// <param name="addressModel">RefAddressModel object</param>
        private void FillRefAddressModel(RefAddressModel addressModel)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            if (PageFlowUtil.IsComponentExist(GViewConstant.SECTION_ADDRESS))
            {
                capModel.addressModel = CapUtil.ConvertRefAddressModel2AddressModel(addressModel);
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }
        }

        /// <summary>
        /// create multiple contacts section and initial section's data
        /// </summary>
        /// <param name="component">the component</param>
        /// <param name="capModel">cap model.</param>
        private void InitMultipleContacts(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_contactlist"));
            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX, component.componentSeqNbr);
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);

            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_MULTI_CONTACTS, "per_multi_contact_Label", customHeadingText, null, instruction);

            string key = string.Format("{0}Edit", sectionName);
            MultiContactsEdit edit = (MultiContactsEdit)_htUserControls[key];

            edit.ID = key;
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);

            if (edit.IsEditable)
            {
                edit.SetSectionRequired(CapUtil.Convert2BooleanValue(component.requiredFlag));
            }

            edit.ValidateFlag = component.validateFlag;
            edit.ContactEdit.ContactExpressionType = ExpressionType.Contacts;
            edit.ComponentID = (int)PageFlowComponent.CONTACT_LIST;

            // property ContactSectionPosition need use ComponentName, so assign the ComponentName first.
            edit.ContactEdit.ComponentName = sectionName;
            edit.ContactEdit.ContactSectionPosition = ACAConstant.ContactSectionPosition.SpearForm;
            edit.ComponentName = sectionName;

            if (!IsPostBack)
            {
                List<CapContactModel4WS> contactsGroupList = new List<CapContactModel4WS>();

                /* When rendering the content of ContactList control, we have to remove those items which do not belong to the ContactList.
                 * It means an item which belongs to Applicant, Contact1, Contact2, or Contact3 will be removed.
                 */
                if (!AppSession.IsAdmin && capModel.contactsGroup != null && capModel.contactsGroup.Length != 0)
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

                    /* 
                     * If the current Contact List Component Name is not changed and exists some uncopied contact records(Its component name has no compoment sequence number), 
                     * the all contact records without valid component name will copy to current(first) contact list.
                     */
                    CapContactModel4WS[] tmpContacts = ContactUtil.FindAppropriatedContacts(capModel.contactsGroup);
                    contactsGroupList.AddRange(tmpContacts);

                    //Update the Component Name and initial RowIndex for contact records in Contact List section.
                    for (int index = 0; index < contactsGroupList.Count; index++)
                    {
                        contactsGroupList[index].people.RowIndex = index;
                        contactsGroupList[index].componentName = sectionName;
                    }
                }

                edit.DisplayContacts(contactsGroupList.ToArray());
            }

            AddParcelPKToContactAutoFill(capModel.ownerModel, capModel.parcelModel);
            edit.ContactsChanged += new CommonEventHandler(ContactListSaved);

            _dictKeysAndNamesForMultipleContactList.Add(key, sectionName);
        }

        /// <summary>
        /// create multiple licenses section and initial section's data
        /// </summary>
        /// <param name="component">Component Model</param>
        /// <param name="capModel">cap model.</param>
        private void InitMultipleLicenses(ComponentModel component, CapModel4WS capModel)
        {
            string customHeadingText = I18nStringUtil.GetString(component.resCustomHeading, LabelUtil.GetGlobalTextByKey("aca_pageflow_component_licenseprofessionallist"));
            string instruction = I18nStringUtil.GetCurrentLanguageString(component.resInstruction, component.instruction);

            string sectionName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_LICENSE_PREFIX, component.componentSeqNbr);
            CreateSection(sectionName, PageFlowConstant.CONTROL_NAME_MULTI_LICENSES, "per_multi_license_Label", customHeadingText, null, instruction);

            string key = string.Format("{0}Edit", sectionName);
            MultiLicensesEdit edit = (MultiLicensesEdit)_htUserControls[key];

            edit.ComponentName = sectionName;
            edit.IsEditable = CapUtil.IsSectionEditable(component.editableFlag);

            if (edit.IsEditable)
            {
                edit.SetSectionRequired(CapUtil.Convert2BooleanValue(component.requiredFlag));
            }

            edit.ValidateFlag = component.validateFlag;

            if (!IsPostBack)
            {
                /* When rendering the content of LicenseProfessionalList control, we have to remove those items which do not belong to it.
                 * It means an item which belongs to License Professional will be removed.
                 */
                LicenseProfessionalModel4WS[] lpBelongToLPList = LicenseUtil.FindLicenseProfessionalsWithComponentName(capModel, sectionName);
                List<LicenseProfessionalModel4WS> remainUnCopiedLP = new List<LicenseProfessionalModel4WS>();

                if (capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
                {
                    remainUnCopiedLP = capModel.licenseProfessionalList.Where(lp => string.IsNullOrEmpty(lp.componentName)).ToList();
                }

                // If not any LP belong to the LP List component, mean the page flow has changed, it will put all LP whose component name is empty to the LP List component.
                if ((lpBelongToLPList == null || lpBelongToLPList.Length == 0 || remainUnCopiedLP.Count > 0)
                    && capModel.IsLicensesChecked4Record
                    && capModel.licenseProfessionalList != null
                    && capModel.licenseProfessionalList.Length > 0)
                {
                    List<LicenseProfessionalModel4WS> tempLPList = remainUnCopiedLP;
                    tempLPList.ForEach(lp => lp.componentName = sectionName);

                    /*
                     * If the Component Name of the LP List component is not changed and still exists some LP records has no Component Name,
                     * all these LP records with no component name will be auto added to the current(first) LP list.
                     */
                    if (lpBelongToLPList != null && lpBelongToLPList.Length > 0)
                    {
                        tempLPList.AddRange(lpBelongToLPList);
                    }

                    lpBelongToLPList = tempLPList.ToArray();
                }

                //initial temporary ID when control initial, because EMSE may clear the temporary ID
                if (lpBelongToLPList != null)
                {
                    foreach (var tempLp in lpBelongToLPList.Where(tempLp => string.IsNullOrEmpty(tempLp.TemporaryID)))
                    {
                        tempLp.TemporaryID = CommonUtil.GetRandomUniqueID();
                    }
                }

                string serviceProviderCode = capModel.capID != null ? capModel.capID.serviceProviderCode : string.Empty;
                LicenseProfessionalModel[] licensees = LicenseUtil.ResetLicenseeAgency(lpBelongToLPList, serviceProviderCode);

                edit.DisplayLicenses(licensees);
            }

            edit.LicensesRemovedEvent += new CommonEventHandler(LicenseRemoved);

            _dictKeysAndNamesForLPList.Add(key, sectionName);
        }

        /// <summary>
        /// triggered after multiple contacts saved
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A System.EventArgs object containing the event data.</param>
        private void ContactListSaved(object sender, CommonEventArgs arg)
        {
            if (arg == null)
            {
                return;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel != null)
            {
                object[] objArray = (object[])arg.ArgObject;

                if (objArray.Length != 3)
                {
                    return;
                }

                CapContactModel4WS[] contactsBeingSaved = objArray[0] as CapContactModel4WS[];
                CapContactModel4WS currentCapContact = objArray[1] as CapContactModel4WS;
                string refContactSeqNbr = currentCapContact == null ? string.Empty : currentCapContact.refContactNumber;
                bool isClearRefContact = ValidationUtil.IsYes(Convert.ToString(objArray[2]));
                IEnumerable<CapContactModel4WS> currentContactList = new List<CapContactModel4WS>();
                bool isDeleteAction = false;
                bool isAddAction = false;

                if (contactsBeingSaved != null)
                {
                    if (capModel.contactsGroup != null)
                    {
                        currentContactList = capModel.contactsGroup.Where(f => f.componentName == contactsBeingSaved[0].componentName);
                    }

                    isAddAction = contactsBeingSaved.Length > currentContactList.Count();
                }

                isDeleteAction = contactsBeingSaved == null || contactsBeingSaved.Length < currentContactList.Count();

                if ((isDeleteAction && ContactUtil.IsPrimaryContact(currentCapContact)) || isClearRefContact)
                {
                    string primaryContactSeqNbr = refContactSeqNbr;

                    if (isClearRefContact)
                    {
                        CapContactModel4WS primaryCapContact = currentContactList.FirstOrDefault(f => f.people != null && f.people.flag == ACAConstant.COMMON_Y);

                        /* If current contact list have the primary contact before Clear then Save Contact, but the primary contact can not be found 
                         * in the remain contacts, indicating user have Cleared the primary contact, need to remove the related license certification data
                         * by setting primaryContactSeqNbr.
                         */
                        if (string.IsNullOrWhiteSpace(primaryContactSeqNbr) && primaryCapContact != null && contactsBeingSaved != null
                            && contactsBeingSaved.All(f => f.people != null && f.people.flag != ACAConstant.COMMON_Y))
                        {
                            primaryContactSeqNbr = primaryCapContact.refContactNumber;
                        }
                        else
                        {
                            primaryContactSeqNbr = string.Empty;
                        }
                    }

                    RemovePrimaryContactLCData(capModel, primaryContactSeqNbr);
                }

                if (contactsBeingSaved != null)
                {
                    capModel.contactsGroup = ContactUtil.AppendContactsListToGroup(capModel.contactsGroup, contactsBeingSaved);
                }
                else
                {
                    // delete all contact in ContactList, this ArgObject indicates as ContactList's ComponentName
                    capModel.contactsGroup = ContactUtil.RemoveContactWithComponentNameFromGroup(capModel.contactsGroup, (string)objArray[0]);
                }

                IEnumerable<CapContactModel4WS> refContacts =
                    capModel.contactsGroup.Where(f => !string.IsNullOrWhiteSpace(f.refContactNumber)
                    && EnumUtil<ContactType4License>.Parse(f.people.contactTypeFlag, ContactType4License.Individual) == ContactType4License.Individual);

                bool isFirstAddRefContact = isAddAction && refContacts.Count() == 1;

                // Change the 'Select from Contact' button status when all ref. contact was deleted or add the first ref. contact or clear the ref. contact..
                if ((!string.IsNullOrWhiteSpace(refContactSeqNbr) && (!refContacts.Any() || isFirstAddRefContact))
                    || (isClearRefContact && !refContacts.Any()))
                {
                    SwitchButtonStatus4SelectFromContact(capModel);
                }
            }
        }

        /// <summary>
        /// triggered after multiple licenses saved
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A System.EventArgs object containing the event data.</param>
        private void LicenseRemoved(object sender, CommonEventArgs arg)
        {
            if (arg == null)
            {
                return;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel != null)
            {
                LicenseProfessionalModel[] licenseProfessionals = arg.ArgObject as LicenseProfessionalModel[];

                if (licenseProfessionals != null)
                {
                    LicenseProfessionalModel4WS[] lpBeingSaved = TempModelConvert.ConvertToLicenseProfessionalModel4WSList(licenseProfessionals);
                    capModel.licenseProfessionalList = LicenseUtil.AppendCapLPToGroup(lpBeingSaved, capModel.licenseProfessionalList);
                }
                else
                {
                    capModel.licenseProfessionalList = LicenseUtil.RemoveLPWithComponentNameFromGroup(capModel.licenseProfessionalList, (string)arg.ArgObject);
                }
            }
        }

        /// <summary>
        /// triggered after single contact saved
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A System.EventArgs object containing the event data.</param>
        private void SingleContactSaved(object sender, CommonEventArgs arg)
        {
            if (arg == null)
            {
                return;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            object[] objArray = (object[])arg.ArgObject;

            if (objArray.Length != 3)
            {
                return;
            }

            bool isDeleteAction = Convert.ToBoolean(objArray[0]);
            CapContactModel4WS capContactModel = (CapContactModel4WS)objArray[1];
            bool isClearRefContact = ValidationUtil.IsYes(Convert.ToString(objArray[2]));
            bool isAddAction = !isDeleteAction
                && (capModel.contactsGroup == null || capModel.contactsGroup.Count(f => f.componentName == capContactModel.componentName) == 0);

            if (isDeleteAction)
            {
                // delete action
                if (ContactUtil.IsPrimaryContact(capContactModel))
                {
                    RemovePrimaryContactLCData(capModel, capContactModel.refContactNumber);
                }

                capModel.contactsGroup = ContactUtil.RemoveContactWithComponentNameFromGroup(capModel.contactsGroup, capContactModel.componentName);
            }
            else
            {
                if (isClearRefContact)
                {
                    CapContactModel4WS originCapContact = capModel.contactsGroup == null
                                                              ? null
                                                              : capModel.contactsGroup.FirstOrDefault(f => f.componentName == capContactModel.componentName);

                    if (ContactUtil.IsPrimaryContact(originCapContact))
                    {
                        RemovePrimaryContactLCData(capModel, originCapContact.refContactNumber);
                    }
                }

                // add/edit action
                CapUtil.SetCapContactToCap(capModel, capContactModel);
            }

            IEnumerable<CapContactModel4WS> refContacts = capModel.contactsGroup.Where(f => !string.IsNullOrWhiteSpace(f.refContactNumber)
                && EnumUtil<ContactType4License>.Parse(f.people.contactTypeFlag, ContactType4License.Individual) == ContactType4License.Individual);

            bool isFirstAddRefContact = isAddAction && refContacts.Count() == 1;

            // Change the 'Select from Contact' button status when all ref. contact was deleted or add the first ref. contact or clear the ref. contact.
            if ((!string.IsNullOrEmpty(capContactModel.refContactNumber) && (!refContacts.Any() || isFirstAddRefContact))
                || (isClearRefContact && !refContacts.Any()))
            {
                SwitchButtonStatus4SelectFromContact(capModel);
            }
        }

        /// <summary>
        /// Update partial cap 
        /// if for super agency,parent cap and children caps will be updated.
        /// </summary>
        /// <param name="capModel4WS">Cap model for ACA</param>
        /// <returns>a CapModel4WS</returns>
        private CapModel4WS UpdatePartialCap(CapModel4WS capModel4WS)
        {
            PeopleUtil.SetSearchAndSyncFlag4Contact(capModel4WS, ModuleName, true);

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

            if (CapUtil.IsSuperCAP(ModuleName))
            {
                CapUtil.FilterSameLicenseType(capModel4WS);
                capModel4WS = capBll.UpdatePartialCaps(capModel4WS);
            }
            else
            {
                capModel4WS = capBll.UpdatePartialCapModelWrapper(ConfigManager.AgencyCode, capModel4WS, AppSession.User.PublicUserId);
            }

            return capModel4WS;
        }

        /// <summary>
        /// register JS function.
        /// register "javascript" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnSubmit()
        {
            if (!Page.ClientScript.IsStartupScriptRegistered("OnSubmitExpression"))
            {
                const string JsFunctionBeforeExp = @"
                        if (typeof(ShowEducationMessage) != 'undefined') {
                            ShowEducationMessage();
                        }
                        if (DisabelSave) {
                            return false;
                        }
                        SetNotAsk(true);";

                string callJsFunction = GetRunningExpressionJS(true);
                string jsSubmitFunction = ExpressionUtil.GetExpressionScriptOnSubmit(callJsFunction, JsFunctionBeforeExp);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnSubmitExpression", jsSubmitFunction, true);
            }
        }

        /// <summary>
        /// register "javascript" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnLoad()
        {
            string callJsFunction = GetRunningExpressionJS(false);

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnLoadExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnLoadExpression", callJsFunction, true);
            }
        }

        /// <summary>
        /// Generate the expression scripts for OnLoad and OnSubmit events.
        /// </summary>
        /// <param name="isSubmit">true - for OnSubmit event. | false - for OnLoad event.</param>
        /// <returns>expression string for javascript</returns>
        private string GetRunningExpressionJS(bool isSubmit)
        {
            Dictionary<string, List<string>> expressionFieldsList = new Dictionary<string, List<string>>();
            Dictionary<string, ExpressionRuntimeArgumentsModel> argumentsModels = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
            Dictionary<string, WebControl> asiControls = new Dictionary<string, WebControl>();

            // Step 1 ASI Expression
            //Step 1.1 Collect ASI controls & sub cap models
            List<AppSpecInfoEdit> asiSections = new List<AppSpecInfoEdit>();
            IDictionary<string, CapModel4WS> asiSubCaps = new Dictionary<string, CapModel4WS>();

            foreach (string key in _dictASIGroupList.Keys)
            {
                AppSpecInfoEdit asiEdit = _htUserControls[key + "Edit"] as AppSpecInfoEdit;

                if (asiEdit == null)
                {
                    continue;
                }

                foreach (KeyValuePair<string, WebControl> isc in asiEdit.AllControls)
                {
                    if (!asiControls.ContainsKey(isc.Key))
                    {
                        asiControls.Add(isc.Key, isc.Value);
                    }
                }

                if (asiEdit.SubCapModels != null)
                {
                    asiSubCaps = asiSubCaps.Concat(asiEdit.SubCapModels).ToDictionary(v => v.Key, v => v.Value);
                }

                asiSections.Add(asiEdit);
            }

            // step 1.2 Attach expression for asi control
            if (asiSections.Count > 0)
            {
                ExpressionFactory asiExpInstance = new ExpressionFactory(ModuleName, ExpressionType.ASI, asiControls, asiSubCaps);
                string asiScripts = isSubmit ? asiExpInstance.GetRunExpFunctionOnSubmit() : asiExpInstance.GetRunExpFunctionOnLoad();
                ExpressionUtil.CombineArgumentsAndExpressionFieldsModule(argumentsModels, expressionFieldsList, asiScripts);

                if (isSubmit)
                {
                    asiExpInstance.AttachEventToControl();
                }
            }

            // Step 2 ASIT Expression
            // step 2.1 collect asit ui table and sub caps
            List<ASITUITable> asitUITables = new List<ASITUITable>();
            IDictionary<string, CapModel4WS> asitSubCaps = new Dictionary<string, CapModel4WS>();

            foreach (string key in _dictASITGroupList.Keys)
            {
                AppSpecInfoTableList asitList = _htUserControls[key + "Edit"] as AppSpecInfoTableList;

                if (asitList == null)
                {
                    continue;
                }

                if (asitList.UITables != null)
                {
                    asitUITables.AddRange(asitList.UITables);
                }

                if (asitList.SubCapModels == null)
                {
                    continue;
                }

                foreach (var item in asitList.SubCapModels)
                {
                    if (!asitSubCaps.ContainsKey(item.Key))
                    {
                        asitSubCaps.Add(item);
                    }
                }
            }

            // step 2.2 attach expression for asit
            foreach (string key in _dictASITGroupList.Keys)
            {
                AppSpecInfoTableList asitList = _htUserControls[key + "Edit"] as AppSpecInfoTableList;

                if (asitList == null)
                {
                    continue;
                }

                ExpressionFactory asitExpInstance = new ExpressionFactory(ModuleName, ExpressionType.ASI_Table, asiControls, null, asitUITables, asitSubCaps, asitList.ASITUIDataKey);
                string asitScripts = isSubmit ? asitExpInstance.GetClientExpScript4ASIT(ExpressionFactory.SUBMIT_EVENT_NAME) : asitExpInstance.GetClientExpScript4ASIT(ExpressionFactory.LOAD_EVENT_NAME);
                ExpressionUtil.CombineArgumentsAndExpressionFieldsModule(argumentsModels, expressionFieldsList, asitScripts);

                //Attach ASIT expression to ASI fields(use isSubmit to prevent the event be attach twice).
                if (isSubmit)
                {
                    foreach (KeyValuePair<string, WebControl> isc in asiControls)
                    {
                        if (isc.Value != null)
                        {
                            WebControl ctl = isc.Value as WebControl;
                            asitExpInstance.AttachEventToControl(isc.Key, ctl, Page);
                        }
                    }
                }
            }

            // Step 3 Address Expression
            foreach (Control htControl in _htUserControls.Values)
            {
                if (htControl is FormDesignerWithExpressionControl)
                {
                    FormDesignerWithExpressionControl expressionControl = htControl as FormDesignerWithExpressionControl;

                    string expScripts = string.Empty;

                    if (isSubmit)
                    {
                        expScripts = expressionControl.GetRunExpFunctionOnSubmit();
                    }
                    else
                    {
                        expressionControl.ExpressionControls = asiControls;
                        expScripts = expressionControl.GetRunExpFunctionOnLoad();
                    }

                    ExpressionUtil.CombineArgumentsAndExpressionFieldsModule(argumentsModels, expressionFieldsList, expScripts);
                }
            }

            //Step.4 - Generate the expression scripts and register to client.
            string scripts = ExpressionUtil.BuildRunExpressionScripts(argumentsModels, expressionFieldsList, isSubmit, false);
            return scripts;
        }

        /// <summary>
        /// Handle Exception/Error when the current page is submit
        /// </summary>
        /// <param name="message">the message.</param>
        private void HandleSubmitException(string message)
        {
            // 1. Show Error Message
            MessageUtil.ShowMessage(Page, MessageType.Error, message);
        }

        /// <summary>
        /// Disable or enable the Select from Contact button.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        private void SwitchButtonStatus4SelectFromContact(CapModel4WS capModel)
        {
            bool isDisable = !ContactUtil.IsRefContactExist(capModel);

            PageFlowGroupModel pageflowGroup = PageFlowUtil.GetPageflowGroup(capModel);
            PageModel pageModel = pageflowGroup.stepList[_currentStep].pageList[_currentPage];

            if (pageModel.componentList.Any(f => f.componentID == (long)PageFlowComponent.EDUCATION))
            {
                string eduEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EDUCATION);
                EducationEdit eduEdit = _htUserControls[eduEditKey] as EducationEdit;

                if (eduEdit != null)
                {
                    eduEdit.DisableSelectFromContact(isDisable);
                    RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_EDUCATION);
                }
            }

            if (pageModel.componentList.Any(f => f.componentID == (long)PageFlowComponent.EXAMINATION))
            {
                string eduEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EXAMINATION);
                ExaminationEdit examEdit = _htUserControls[eduEditKey] as ExaminationEdit;

                if (examEdit != null)
                {
                    examEdit.DisableSelectFromContact(isDisable);
                    RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_EXAMINATION);
                }
            }

            if (pageModel.componentList.Any(f => f.componentID == (long)PageFlowComponent.CONTINUING_EDUCATION))
            {
                string eduEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_CONT_EDUCATION);
                ContinuingEducationEdit contEduEdit = _htUserControls[eduEditKey] as ContinuingEducationEdit;

                if (contEduEdit != null)
                {
                    contEduEdit.DisableSelectFromContact(isDisable);
                    RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_CONT_EDUCATION);
                }
            }
        }

        /// <summary>
        /// Remove the primary contact related license certification data.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="primaryContactSeqNbr">The primary contact sequence number.</param>
        private void RemovePrimaryContactLCData(CapModel4WS capModel, string primaryContactSeqNbr)
        {
            if (capModel == null || string.IsNullOrEmpty(primaryContactSeqNbr) || !PageFlowUtil.IsEduExamComponentExist())
            {
                return;
            }

            CapAssociateLicenseCertification4WS capAssociateLicenseCertification = AppSession.GetCapTypeAssociateLicenseCertification();

            if (capModel.educationList != null && capModel.educationList.Any())
            {
                capModel.educationList = capAssociateLicenseCertification.capAssociateEducation == null
                                             ? null
                                             : EducationUtil.ConvertToEducation4WSModelArray(capAssociateLicenseCertification.capAssociateEducation);

                string eduEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EDUCATION);
                EducationEdit eduEdit = _htUserControls[eduEditKey] as EducationEdit;

                if (eduEdit != null)
                {
                    eduEdit.UpdateEduList(ObjectConvertUtil.ConvertArrayToList(capModel.educationList));
                    eduEdit.DisplayDelActionNotice(true);
                }
            }

            if (capModel.examinationList != null && capModel.examinationList.Any())
            {
                capModel.examinationList = capAssociateLicenseCertification.capAssociateExamination == null
                                               ? null
                                               : ExaminationUtil.ConvertToExaminationModelArray(capAssociateLicenseCertification.capAssociateExamination);

                string eduEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_EXAMINATION);
                ExaminationEdit examEdit = _htUserControls[eduEditKey] as ExaminationEdit;

                if (examEdit != null)
                {
                    examEdit.UpdateExamList(ObjectConvertUtil.ConvertArrayToList(capModel.examinationList));
                    examEdit.DisplayDelActionNotice(true);
                }
            }

            if (capModel.contEducationList != null && capModel.contEducationList.Any())
            {
                capModel.contEducationList = capAssociateLicenseCertification.capAssociateContEducation == null
                                                 ? null
                                                 : EducationUtil.ConvertToContEdu4WSModelArray(capAssociateLicenseCertification.capAssociateContEducation);

                string eduEditKey = string.Format("{0}Edit", PageFlowConstant.SECTION_NAME_CONT_EDUCATION);
                ContinuingEducationEdit contEduEdit = _htUserControls[eduEditKey] as ContinuingEducationEdit;

                if (contEduEdit != null)
                {
                    contEduEdit.DislayContEducations(capModel.contEducationList);
                    RefreshUpdatePanel(PageFlowConstant.SECTION_NAME_CONT_EDUCATION);
                    contEduEdit.DisplayDelActionNotice(true);
                }
            }
        }

        /// <summary>
        /// Sets the contact type of the single contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="edit">The edit.</param>
        private void SetSingleContactType(CapContactModel4WS contact, ContactEdit edit)
        {
            if (contact != null
                && contact.people != null
                && string.IsNullOrEmpty(contact.people.contactType))
            {
                PeopleModel4WS people = contact.people;
                string contactType = edit.ContactType;

                contact.people.contactType = contactType;
                ContactUtil.MergeContactTemplateModel(people, contactType, ModuleName);
            }
        }

        /// <summary>
        /// Build the script of the popup page opened from confirm page
        /// </summary>
        /// <returns>The script open the popup page</returns>
        private string BuildScript4PopupFromConfirm()
        {
            string rowIndex = Request.QueryString[UrlConstant.ROW_INDEX];
            string sectionName = Request.QueryString[UrlConstant.SECTION_NAME];
            string isFromConfirmPage = Request.QueryString[UrlConstant.IS_FROM_CONFIRMPAGE];

            if (string.IsNullOrEmpty(sectionName) || !ValidationUtil.IsYes(isFromConfirmPage) || !AppSession.IsEditFromConfirmFlag)
            {
                return string.Empty;
            }

            object editControl = _htUserControls[sectionName + "Edit"];
            string script = string.Empty;

            if (editControl is LicenseEdit)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null && capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
                {
                    LicenseProfessionalModel4WS licenseProfessional = capModel.licenseProfessionalList.FirstOrDefault(lp => sectionName.Equals(lp.componentName, StringComparison.InvariantCultureIgnoreCase));

                    if (licenseProfessional != null)
                    {
                        LicenseEdit licenseEdit = (LicenseEdit)editControl;
                        script = string.Format("{0}();", licenseEdit.EditLicenseFunction);
                    }
                }
            }
            else if (editControl is MultiLicensesEdit)
            {
                if (!string.IsNullOrEmpty(rowIndex))
                {
                    MultiLicensesEdit multiLicensesEdit = (MultiLicensesEdit)editControl;
                    DataRow drLicense = multiLicensesEdit.DataSource.Rows[int.Parse(rowIndex)];
                    LicenseProfessionalModel licenseProfessionalModel = (LicenseProfessionalModel)drLicense["LicenseProfessionalModel"];
                    script = string.Format(
                        "{0}('{1}','{2}','{3}','{4}');",
                        multiLicensesEdit.EditLicenseFunction,
                        licenseProfessionalModel.licSeqNbr,
                        ScriptFilter.AntiXssJavaScriptEncode(licenseProfessionalModel.licenseNbr),
                        ScriptFilter.AntiXssJavaScriptEncode(licenseProfessionalModel.licenseType),
                        ScriptFilter.AntiXssJavaScriptEncode(licenseProfessionalModel.TemporaryID));
                }
            }
            else if (editControl is ContactEdit)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null && capModel.contactsGroup != null && capModel.contactsGroup.Length > 0)
                {
                    CapContactModel4WS contactRecord = capModel.contactsGroup.FirstOrDefault(contact => sectionName.Equals(contact.componentName, StringComparison.InvariantCultureIgnoreCase));

                    if (contactRecord != null)
                    {
                        ContactEdit contactEdit = (ContactEdit)editControl;
                        script = string.Format(
                            "{0}({1},'','',{2});",
                            contactEdit.CreateContactSessionFunction,
                            ContactProcessType.Edit.ToString("D"),
                            contactEdit.EditContactFunction);
                    }
                }
            }
            else if (editControl is MultiContactsEdit)
            {
                if (!string.IsNullOrEmpty(rowIndex))
                {
                    MultiContactsEdit multiContactsEdit = (MultiContactsEdit)editControl;
                    DataRow drContact = multiContactsEdit.DataSource.Rows[int.Parse(rowIndex)];
                    CapContactModel4WS capContactModel = (CapContactModel4WS)drContact[ColumnConstant.Contact.CapContactModel.ToString()];

                    script = string.Format(
                        "{0}({1}, '{2}','',{3});",
                        multiContactsEdit.CreateContactSessionFunction,
                        ContactProcessType.Edit.ToString("D"),
                        capContactModel.people.RowIndex,
                        multiContactsEdit.EditContactFunction);
                }
            }

            if (!string.IsNullOrEmpty(script))
            {
                AppSession.IsEditFromConfirmFlag = false;
            }

            return script;
        }

        /// <summary>
        /// set the breadcrumb's page flow configuration
        /// </summary>
        private void SetBreadcrumbPageFlow()
        {
            bool chkFeeForm = _is4FeeEstimator && !_isRedirectToFeePage;

            if (AppSession.IsAdmin == false)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                bool isHideFee = CapUtil.IsFeeEstimateCapType(capModel.capType);
                BreadCrumpToolBar.PageFlow = BreadCrumpUtil.GetPageFlowConfig(ModuleName, chkFeeForm, isHideFee);
            }
        }

        #endregion Methods
    }
}
