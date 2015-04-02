#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccountContactEdit.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccountContactEdit.cs 151830 2010-04-26 13:39:43Z ACHIEVO\grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;

using Accela.ACA.BLL;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.People;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// the class of AccountContact 
    /// </summary>
    public partial class AccountContactEdit : BasePage
    {
        #region Properties

        /// <summary>
        /// Gets the reload page after save.
        /// </summary>
        protected string ReloadPage
        {
            get
            {
                return Page.ResolveUrl("~/Account/AccountManager.aspx");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the contact type editable settings.
        /// </summary>
        private bool ContactTypePermission
        {
            get
            {
                if (!AppSession.IsAdmin)
                {
                    return ContactUtil.GetContactTypeEditablePermissionByContactSeqNbr(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]);
                }

                return true;
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
            ContactSessionParameter sessionParameter = AppSession.GetContactSessionParameter();

            if (!string.IsNullOrEmpty(parameterString))
            {
                ContactSessionParameter newSessionParameter = new JavaScriptSerializer().Deserialize<ContactSessionParameter>(parameterString);
                sessionParameter.ContactSectionPosition = newSessionParameter.ContactSectionPosition;
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
        /// Edits the contact by type disabled.
        /// </summary>
        [WebMethod(Description = "Edit Contact By Contact Type Disabled", EnableSession = true)]
        public static void EditContactByTypeDisabled()
        {
            ContactSessionParameter sessionParameter = AppSession.GetContactSessionParameter();

            if (sessionParameter != null && sessionParameter.Process != null)
            {
                sessionParameter.Process.ContactProcessType = ContactProcessType.EditContactType;
            }
        }

        /// <summary>
        /// The initial event.
        /// </summary>
        /// <param name="e">the event handle.</param>
        protected override void OnInit(EventArgs e)
        {
            bool isFirstLoad = !IsPostBack
                               && Request.UrlReferrer != null
                               && (Request.UrlReferrer.LocalPath.EndsWith("account/AccountManager.aspx", StringComparison.InvariantCultureIgnoreCase)
                                    || IsRefreshOnCurrentpage());

            if (isFirstLoad)
            {
                AppSession.SetContactSessionParameter(null);
            }

            if (!IsPostBack)
            {
                UIModelUtil.ClearUIData();
            }

            ucContactInfo.ContactSectionPosition = ACAConstant.ContactSectionPosition.ModifyReferenceContact;

            lnkCreateAmendment.Visible = AppSession.IsAdmin;
            ucContactInfo.InitTitleBar("aca_manage_label_contactinfo_title");

            ucContactInfo.ParentID = ucContactInfo.ClientID;
            ucContactInfo.ContactExpressionType = ExpressionType.ReferenceContact;
            ucContactInfo.InitCountry();

            base.OnInit(e);
        }

        /// <summary>
        /// overwrite on load method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isContactApproved = IsContactApproved();
            bool isContactTypeEnable = true;
            string contactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];
            ContactSessionParameter sessionParameter = AppSession.GetContactSessionParameter();
            string contactType = sessionParameter == null ? string.Empty : sessionParameter.ContactType;

            bool sessionEditable = sessionParameter != null
                                   && sessionParameter.Process.ContactProcessType == ContactProcessType.EditContactType
                                   && (sessionParameter.Data.DataObject as PeopleModel4WS).contactSeqNumber == contactSeqNbr
                                   && sessionParameter.PageFlowComponent.IsEditable;

            PeopleModel4WS contact = PeopleUtil.GetPeopleByContactSeqNbr(contactSeqNbr);

            bool isExternal = contact != null
                              && (string.IsNullOrEmpty(contact.serviceProviderCode) || !contact.serviceProviderCode.Equals(ConfigManager.AgencyCode, StringComparison.OrdinalIgnoreCase));

            if (sessionParameter != null && !string.IsNullOrEmpty(sessionParameter.ContactType))
            {
                ucContactInfo.ContactType = sessionParameter.ContactType;
            }

            if (isContactApproved)
            {
                //when contact not approved, not need get the indication [is contact type enabled].
                isContactTypeEnable = ContactUtil.IsContactTypeEnable4AcountContactEdit(contactSeqNbr, contactType);
            }

            //all pepople template field should disabled if the contact is external.
            lnkCreateAmendment.Enabled = !isExternal;
            btnSave.Enabled = !isExternal;
            ucContactInfo.SupportAlwaysEditable = !isExternal;

            ucContactInfo.IsEditable = (sessionEditable || (isContactApproved && isContactTypeEnable)) && ContactTypePermission && !isExternal;
            ucContactInfo.SetSectionRequired("0");       
            educationEdit.EducationsChanged += new CommonEventHandler(EducationListChanged);
            examinationEdit.DataChanged += new CommonEventHandler(ExaminationListChanged);
            continuingEducationEdit.ContEducationsChanged += new CommonEventHandler(ContEducationEdit_ContEducationChanged);

            if (!IsPostBack)
            {
                PeopleModel4WS people = null;

                lblSubTitle.Visible = true;
                acc_manage_text_accountInfo.Visible = true;

                if (!AppSession.IsAdmin)
                {
                    ICapTypeFilterBll captypeFilterBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
                    XButtonFilterModel4WS xbFilter = captypeFilterBll.GetFilter4ButtonModel(ConfigManager.AgencyCode, ModuleName, "aca_account_management_contact_amendment", AppSession.User.PublicUserId);

                    if (xbFilter != null && !string.IsNullOrEmpty(xbFilter.moduleName) && !string.IsNullOrEmpty(xbFilter.filterName))
                    {
                        lnkCreateAmendment.Visible = true;
                    }

                    people = LoadContactInfo();
                }

                bool isIndividual = people != null && EnumUtil<ContactType4License>.Parse(people.contactTypeFlag, ContactType4License.Individual) == ContactType4License.Individual;
                bool isEduExamCESectionVisible = AppSession.IsAdmin || (StandardChoiceUtil.IsEnableAccountEduExamCEInput() && isContactApproved && isIndividual);

                if (isEduExamCESectionVisible)
                {
                    divEduExamCEInfo.Visible = true;
                    educationEdit.ContactSeqNbr = contactSeqNbr;
                    examinationEdit.ContactSeqNbr = contactSeqNbr;
                    continuingEducationEdit.ContactSeqNbr = contactSeqNbr;

                    if ((!ContactTypePermission && !AppSession.IsAdmin) || isExternal)
                    {
                        educationEdit.IsEditable = false;
                        examinationEdit.IsEditable = false;
                        continuingEducationEdit.IsEditable = false;
                        educationEdit.ContactIsFromExternal = isExternal;
                        examinationEdit.ContactIsFromExternal = isExternal;
                        continuingEducationEdit.ContactIsFromExternal = isExternal;
                    }

                    BindRefContactEducationExamList(contactSeqNbr);
                    divEduExamCEInfo.Visible = !isExternal;
                }
            }
            
            if (!ContactTypePermission)
            {
                btnSave.SetButtonStatus(true);
            }

            if ((!isContactTypeEnable && !sessionEditable) && ContactTypePermission && !isExternal)
            {
                btnSave.SetButtonStatus(true);
                MessageBar.IsShowLinkButton = true;
                MessageBar.OnClientClick = "changeContactType();return false;";
                MessageBar.ShowWithText(MessageType.Notice, GetTextByKey("aca_account_contact_label_type_disabled"), MessageSeperationType.Bottom);
            }
        }

        /// <summary>
        /// Amendment button event handler.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event hander.</param>
        protected void CreateAmendmentButton_Click(object sender, EventArgs e)
        {
            ICapTypeFilterBll captypeFilterBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
            XButtonFilterModel4WS xbFilter = captypeFilterBll.GetFilter4ButtonModel(ConfigManager.AgencyCode, ModuleName, "aca_account_management_contact_amendment", AppSession.User.PublicUserId);

            if (xbFilter != null && !string.IsNullOrEmpty(xbFilter.moduleName) && !string.IsNullOrEmpty(xbFilter.filterName))
            {
                string contactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];

                //Use the "isSubAgencyCap" to bypass worklocation.aspx page in super agency when doing Contact Amendment at account manager page.
                string isSubAgencyCap = StandardChoiceUtil.IsSuperAgency() ? "&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y : string.Empty;
                string url = string.Format("~/Cap/CapType.aspx?Module={0}&filterName={1}&" + UrlConstant.CONTACT_SEQ_NUMBER + "={2}{3}&isAmendment=Y&{4}={5}", xbFilter.moduleName, HttpUtility.UrlEncode(xbFilter.filterName), contactSeqNbr, isSubAgencyCap, UrlConstant.IS_FROM_ACCOUNT_MANANGEMENT, ACAConstant.COMMON_Y);
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Click Save button event handler
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                PeopleModel4WS peopleModel4Ws = ucContactInfo.GetPeopleModel();
                string contactSeqNumber = peopleModel4Ws.contactSeqNumber;
                peopleModel4Ws.contactAddressList = ucContactInfo.GetContactAddressList();
                string caErrorMessage = string.Empty;

                if (StandardChoiceUtil.IsEnableContactAddress())
                {
                    caErrorMessage = ucContactInfo.ValidateContactAddress(peopleModel4Ws.contactAddressList);
                }

                if (string.IsNullOrEmpty(caErrorMessage))
                {
                    IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                    
                    string identityKeyMessage = PeopleUtil.GetIdentityKeyMessage(peopleModel4Ws, PeopleUtil.IdentityFieldLabels, "aca_accountcontactedit_msg_duplicate_contact_forupdate");

                    if (!string.IsNullOrEmpty(identityKeyMessage))
                    {
                        MessageBar.ShowWithText(MessageType.Error, identityKeyMessage, MessageSeperationType.Bottom);
                        return;
                    }

                    peopleBll.EditRefContact(TempModelConvert.ConvertToPeopleModel(peopleModel4Ws));

                    // Clear the contact in the session
                    PeopleUtil.RemoveTempContact(contactSeqNumber);

                    AppSession.ReloadPublicUserSession();
                    Response.Redirect(ResolveUrl("~/Account/AccountManager.aspx"));
                }
            }
            catch (Exception exception)
            {
                /*
                 * RefContactEditBefore and RefContactEditAfter event will be triggered 
                 *  in PeopleService.addContact4PublicUser and PeopleService.editRefContact.
                 */
                MessageBar.ShowWithText(MessageType.Error, exception.Message, MessageSeperationType.Bottom);
            }
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">The event argument</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Don't run expression in administration model.
            if (!AppSession.IsAdmin)
            {
                ExpressionUtil.RegisterScriptLibToCurrentPage(this);

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
        /// triggered after education saved or deleted.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the education models.</param>
        protected void EducationListChanged(object sender, CommonEventArgs arg)
        {
            if (arg != null)
            {
                AppSession.SetContactEducationListToSession(Request.QueryString["contactSeqNbr"], (EducationModel4WS[])arg.ArgObject);
            }
        }

        /// <summary>
        /// triggered after examination saved or deleted.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the examination models.</param>
        protected void ExaminationListChanged(object sender, CommonEventArgs arg)
        {
            if (arg != null)
            {
                AppSession.SetContactExaminationListToSession(Request.QueryString["contactSeqNbr"], (ExaminationModel[])arg.ArgObject);
            }
        }

        /// <summary>
        /// triggered after continuing education saved or deleted.
        /// </summary>
        /// <param name="sender">An object that continuing contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the continuing education models.</param>
        protected void ContEducationEdit_ContEducationChanged(object sender, CommonEventArgs arg)
        {
            if (arg != null)
            {
                AppSession.SetContactContEducationListToSession(Request.QueryString["contactSeqNbr"], (ContinuingEducationModel4WS[])arg.ArgObject);
            }
        }

        /// <summary>
        /// Handles the Click event of the UpdateContactType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arg">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void UpdateContactType_Click(object sender, EventArgs arg)
        {
            string contactType = string.Empty;
            string contactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];
            ContactSessionParameter sessionParameter = AppSession.GetContactSessionParameter();
            
            if (sessionParameter != null)
            {
                contactType = sessionParameter.ContactType;
            }
            else
            {
                contactType = arg.ToString();
            }

            if (string.IsNullOrEmpty(contactType) || string.IsNullOrEmpty(contactSeqNbr))
            {
                return;
            }

            PeopleModel tempPeopleModel = AppSession.GetPeopleModelFromSession(contactSeqNbr);
            PeopleModel peopleModel = ObjectCloneUtil.DeepCopy(tempPeopleModel);

            peopleModel.contactType = contactType;
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
            TemplateModel model = templateBll.GetContactTemplates(ConfigManager.AgencyCode, contactType, false, AppSession.User.UserSeqNum);
            GenericTemplateUtil.MergeGenericTemplate(peopleModel.template, model, string.Empty);
            sessionParameter.Process.ContactProcessType = ContactProcessType.Edit;

            peopleModel.template = model;
            peopleModel.attributes = templateBll.GetPeopleTemplateAttributes(contactType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);

            MessageBar.Hide();
            ucContactInfo.ContactType = contactType;
            ucContactInfo.ContactTypeFlag = peopleModel.contactTypeFlag;
            ucContactInfo.ContactSeqNumber = contactSeqNbr;
            ucContactInfo.IsEditable = true;
            ucContactInfo.ResetTemplate();
            ucContactInfo.DisplayPeople(TempModelConvert.ConvertToPeopleModel4WS(peopleModel), contactType, peopleModel.contactSeqNumber, string.Empty);
        }

        /// <summary>
        /// Bind the ref contact education, examination and continuing education list.
        /// </summary>
        /// <param name="contactSeqNbr">The reference contact sequence number.</param>
        private void BindRefContactEducationExamList(string contactSeqNbr)
        {
            if (!string.IsNullOrEmpty(contactSeqNbr))
            {
                ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
                IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();

                // Education List
                EducationModel4WS[] educationModels = TempModelConvert.ConvertToEducationModel4WS(licenseCertificationBll.GetRefPeopleEduList(contactSeqNbr));
                educationEdit.DisplayEducations(educationModels);

                // Examination List
                ExaminationModel[] examinationModels = examinationBll.GetRefPeopleExamList(contactSeqNbr);
                examinationEdit.DisplayExamination(examinationModels);

                // Continuing Education List
                ContinuingEducationModel4WS[] continuingEducationModels = TempModelConvert.ConvertToContEducationModel4WS(licenseCertificationBll.GetRefPeopleContEduList(contactSeqNbr));
                continuingEducationEdit.DislayContEducations(continuingEducationModels);
            }
            else
            {
                educationEdit.DisplayEducations(null);
                examinationEdit.DisplayExamination(null);
                continuingEducationEdit.DislayContEducations(null);
            }
        }

        /// <summary>
        /// register JS function.
        /// register "javascript" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnSubmit()
        {
            var callJsFunction = ExpressionUtil.GetExpressionScript(true, ucContactInfo);
            var strSubmitFuction = ExpressionUtil.GetExpressionScriptOnSubmit(callJsFunction);

            if (!Page.ClientScript.IsStartupScriptRegistered("OnSubmitExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnSubmitExpression", strSubmitFuction, true);
            }
        }

        /// <summary>
        /// register "javascript" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnLoad()
        {
            var callJsFunction = ExpressionUtil.GetExpressionScript(false, ucContactInfo);

            if (!Page.ClientScript.IsStartupScriptRegistered("OnLoadExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnLoadExpression", callJsFunction, true);
            }
        }

        /// <summary>
        /// To determine whether the current contact is approved.
        /// </summary>
        /// <returns>true or false.</returns>
        private bool IsContactApproved()
        {
            bool isContactApproved = false;
            string contactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];

            if (!string.IsNullOrEmpty(contactSeqNbr))
            {
                PeopleModel4WS[] approvedContacts = AppSession.User.ApprovedContacts;

                if (approvedContacts != null && approvedContacts.Any(p => contactSeqNbr.Equals(p.contactSeqNumber)))
                {
                    isContactApproved = true;
                }
            }

            return isContactApproved;
        }

        /// <summary>
        /// Load contact information.
        /// </summary>
        /// <returns>The people model.</returns>
        private PeopleModel4WS LoadContactInfo()
        {
            string contactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];

            if (string.IsNullOrEmpty(contactSeqNbr))
            {
                ucContactInfo.LoadContactProperties();
                ucContactInfo.ClearAddressList();
                return null;
            }

            PeopleModel4WS people = PeopleUtil.GetPeopleByContactSeqNbr(contactSeqNbr);

            if (people == null)
            {
                throw new ACAException("People not found.");
            }

            // isExternal contact, should merger people template and generic template data.
            if (string.IsNullOrEmpty(people.serviceProviderCode) || !people.serviceProviderCode.Equals(ConfigManager.AgencyCode, StringComparison.OrdinalIgnoreCase))
            {
                ContactUtil.MergeContactTemplateModel(people, people.contactType, ModuleName);
            }

            CreateContactParametersSession(people);
            ucContactInfo.ContactTypeFlag = people.contactTypeFlag;
            ucContactInfo.DisplayPeople(people, people.contactType, people.contactSeqNumber, string.Empty);

            // if contact is pending or reject, cannot modify contact information.
            if (ContractorPeopleStatus.Pending.Equals(people.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase) || ContractorPeopleStatus.Rejected.Equals(people.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase))
            {
                lnkCreateAmendment.Visible = false;
                btnSave.SetButtonStatus(true);
                string[] filterIds = TemplateUtil.GetAlwaysEditableControlIDs(people.attributes, ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX);
                ucContactInfo.DisableContactForm(false, filterIds);
            }

            ucContactInfo.ContactSeqNumber = contactSeqNbr;
            return people;
        }

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="people">people model</param>
        private void CreateContactParametersSession(PeopleModel4WS people)
        {
            ContactSessionParameter sessionParameter = new ContactSessionParameter();
            sessionParameter.ContactExpressionType = ExpressionType.ReferenceContact;
            sessionParameter.ContactSectionPosition = ucContactInfo.ContactSectionPosition;
            sessionParameter.ContactType = people.contactType;
            sessionParameter.Process.CallbackFunctionName = ucContactInfo.ClientID;
            sessionParameter.Process.ContactProcessType = ContactProcessType.Edit;
            sessionParameter.PageFlowComponent.IsEditable = ucContactInfo.IsEditable;

            if (people.contactAddressList != null)
            {
                int addressIndex = 0;

                foreach (ContactAddressModel address in people.contactAddressList)
                {
                    address.RowIndex = addressIndex;
                    addressIndex++;
                }
            }

            sessionParameter.Data.DataObject = people;
            AppSession.SetContactSessionParameter(sessionParameter);
        }

        #endregion Methods
    }
}