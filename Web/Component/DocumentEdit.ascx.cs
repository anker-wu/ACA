#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DocumentEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DocumentEdit.ascx.cs 123865 2009-03-16 09:40:24Z ACHIEVO\weiky.chen $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// DocumentEdit control
    /// </summary>
    public partial class DocumentEdit : FormDesignerBaseControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DocumentEdit class.
        /// </summary>
        public DocumentEdit()
            : base(GviewID.Attachment)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets DocumentModel
        /// </summary>
        public DocumentModel DataModel
        {
            get
            {
                if (ViewState["DocumentModel"] == null)
                {
                    ViewState["DocumentModel"] = new DocumentModel();
                }

                return ViewState["DocumentModel"] as DocumentModel;
            }

            private set
            {
                ViewState["DocumentModel"] = value;
            }
        }

        /// <summary>
        /// Gets the component name to indicate the correct Attachment Component because it can set multiple Attachment Components in the same page.
        /// </summary>
        public string ComponentName
        {
            get
            {
                return AttachmentUtil.ExtractComponentNameFromClientID(ClientID);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating which behavior used.
        /// </summary>
        public FileUploadBehavior CurrentFileUploadBehavior
        {
            get
            {
                if (ViewState["CurrentFileUploadBehavior"] == null)
                {
                    return FileUploadBehavior.Basic;
                }

                FileUploadBehavior behavior = FileUploadBehavior.Basic;
                FileUploadBehavior.TryParse(ViewState["CurrentFileUploadBehavior"].ToString(), true, out behavior);

                return behavior;
            }

            set
            {
                ViewState["CurrentFileUploadBehavior"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                string permissionValue = string.Empty;

                if (!string.IsNullOrEmpty(ddlDocType.SelectedValue))
                {
                    permissionValue = ddlDocType.SelectedValue;
                }

                base.Permission = ControlBuildHelper.GetPermissionWithGenericTemplate(ViewId, GViewConstant.SECTION_ATTACHMENT, permissionValue);

                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets Initial Parameters for Upload control.
        /// </summary>
        protected string InitParams
        {
            get
            {
                var paramString = new StringBuilder();
                paramString.Append("fileId=");
                paramString.Append(DataModel.FileId);
                paramString.Append(ACAConstant.COMMA);
                paramString.Append("fileState=");
                paramString.Append(DataModel.FileState);

                return paramString.ToString();
            }
        }

        /// <summary>
        /// Gets current language code for upload control.
        /// </summary>
        protected string LanguageCode
        {
            get
            {
                var languageCode = I18nCultureUtil.GetLanguageCodeForSoapHandler();
                var regionalCode = I18nCultureUtil.GetRegionalCodeForSoapHandler();
                languageCode = string.Format("{0}_{1}", languageCode, regionalCode);

                return languageCode;
            }
        }

        /// <summary>
        /// Gets a value indicating whether current page is account manager.
        /// </summary>
        private bool IsAccountManagerPage
        {
            get
            {
                return Request.Path.Contains("AccountManager.aspx");
            }
        }

        /// <summary>
        /// Gets a value indicating whether current page is cap detail page.
        /// </summary>
        private bool IsCapDetailPage
        {
            get
            {
                return Request.Path.Contains("CapDetail.aspx");
            }
        }

        /// <summary>
        /// Gets a value indicating whether current section id account is account manager page.
        /// </summary>
        private string DocumentEditViewID
        {
            get
            {
                return IsAccountManagerPage ? GviewID.PeopleAttachment : GviewID.Attachment;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Display DocumentModel
        /// </summary>
        /// <param name="document">Document Model</param>
        /// <param name="isLoadTemplate">Is need reload template</param>
        public void Display(DocumentModel document, bool isLoadTemplate)
        {
            DataModel = document;

            if (!string.IsNullOrEmpty(document.entityID)
                && !string.IsNullOrEmpty(document.entityType)
                && !string.IsNullOrEmpty(document.SpecificEntityType)
                && !string.IsNullOrEmpty(document.serviceProviderCode))
            {
                string selectValue = DataUtil.ConcatStringWithSplitChar(new string[] { document.entityType, document.SpecificEntityType, document.entityID, document.serviceProviderCode }, ACAConstant.SPLIT_CHAR4URL1.ToString());
                ddlAssociatedPeople.SetValue(selectValue);
                InitDocumentTypeByPeople(selectValue);
            }

            if (!string.IsNullOrEmpty(document.docDescription))
            {
                txtAFileDescription.Text = document.docDescription;
            }

            if (!string.IsNullOrEmpty(document.docCategory))
            {
                ddlDocType.SetValue(document.docGroup + ACAConstant.SPLIT_DOUBLE_COLON + document.docCategory);
            }

            if (!string.IsNullOrEmpty(document.fileName))
            {
                lblFileName.Value = document.fileName;
            }

            if (!string.IsNullOrEmpty(document.AlsoAttachTo))
            {
                ddlAlsoAttachTo.SetValue(document.AlsoAttachTo);
            }

            if (!string.IsNullOrEmpty(DataModel.FileState) && ACAConstant.FINISHED_STATUS.Equals(DataModel.FileState))
            {
                lblFileName.ProgressParams = CurrentFileUploadBehavior == FileUploadBehavior.Html5 ? "100%" : string.Format("fileId={0},fileState={1}", DataModel.FileId, DataModel.FileState);
            }

            if (!string.IsNullOrEmpty(document.virtualFolders))
            {
                ckbVirtualFolders.SetSelectValues(document.virtualFolders.Split(ACAConstant.SPLIT_CHAR_SEMICOLON));
            }

            if (isLoadTemplate)
            {
                //Display template fields
                DisplayTemplate();
            }
        }

        #region Event handlers

        /// <summary>
        /// <c>OnInit</c> method
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitDocumentTypeByRecord();
            InitVirtaulFolders();

            if (IsAccountManagerPage)
            {
                BindAssociatedPeople();              
            }
            else
            {
                BindAlsoAttachTo();
            }

            CurrentFileUploadBehavior = StandardChoiceUtil.GetFileUploadBehavior();
            lblFileName.FileUploadBehavior = CurrentFileUploadBehavior;
            lblFileName.IsNeedProgress = !AppSession.IsAdmin && CurrentFileUploadBehavior != FileUploadBehavior.Advanced;

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    ddlDocType.AutoPostBack = false;
                    ddlDocType.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
                    ddlDocType.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");
                }
            }
        }

        /// <summary>
        /// overwrite PreRender.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (IsPostBack && DataModel != null)
            {
                RefreshValues();
                lblFileName.Value = DataModel.fileName;
            }

            if (IsAccountManagerPage)
            {
                BindAssociatedPeople();
                Display(DataModel, false);
                documentEditContainer.Update();
            }
            else
            {
                ddlAlsoAttachTo.IsHidden = !IsShowAlsoAttachTo(DataModel);

                if (!ddlAlsoAttachTo.IsHidden)
                {
                    BindAlsoAttachTo();
                    Display(DataModel, false);
                    documentEditContainer.Update();
                }
            }

            base.OnPreRender(e);
            ViewId = DocumentEditViewID;
            ControlBuildHelper.AddValidationForStandardFields(DocumentEditViewID, ModuleName, Permission, Controls);

            if (ddlDocType.Items.Count == 0 ||
                (ddlDocType.Items.Count == 1 && DropDownListBindUtil.DefaultListItem.Equals(ddlDocType.Items[0])))
            {
                ddlDocType.Required = false;
            }

            InitFormDesignerPlaceHolder(phContent);

            if (CurrentFileUploadBehavior != FileUploadBehavior.Html5)
            {
                lblFileName.ProgressParams = InitParams;
            }
            else if (!string.IsNullOrEmpty(DataModel.FileState) && ACAConstant.FINISHED_STATUS.Equals(DataModel.FileState))
            {
                lblFileName.ProgressParams = "100%";
            }
        }

        /// <summary>
        /// page load.
        /// </summary>
        /// <param name="sender">page object</param>
        /// <param name="e">Event Arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                RefreshValues();
            }

            if (ddlAssociatedPeople != null && ddlAssociatedPeople.Items.Count > 1)
            {
                ViewState[ddlAssociatedPeople.UniqueID] = ddlAssociatedPeople.SelectedValue;
            }
        }

        /// <summary>
        /// Doc type change event handler.
        /// </summary>
        /// <param name="sender">button object</param>
        /// <param name="e">event arguments</param>
        protected void DocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDocType.IsHidden = false;
            DataModel.template = null;
            DisplayTemplate();
            Page.FocusElement(ddlDocType.ClientID);
        }

        /// <summary>
        /// Associated people dropdown list change event handler.
        /// </summary>
        /// <param name="sender">Dropdown List object</param>
        /// <param name="e">Event arguments</param>
        protected void AssociatedPeople_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlAssociatedPeople.SelectedValue))
            {
                // get entity type and entity id.
                string[] peopleInfo = ddlAssociatedPeople.SelectedValue.Split(new char[] { ACAConstant.SPLIT_CHAR4URL1 }, StringSplitOptions.None);
                DataModel.entityID = peopleInfo[2];
                DataModel.entityType = peopleInfo[0];
            }

            InitDocumentTypeByPeople(ddlAssociatedPeople.SelectedValue);
            DataModel.entityType = string.Empty;
            DataModel.SpecificEntityType = string.Empty;
            DataModel.entityID = string.Empty;
            DataModel.serviceProviderCode = string.Empty;
            DataModel.template = null;
            DisplayTemplate();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Display document template fields.
        /// </summary>
        private void DisplayTemplate()
        {
            genericTemplate.ResetControl();

            if (!ddlDocType.IsHidden && !string.IsNullOrEmpty(ddlDocType.SelectedValue))
            {
                if (DataModel.template == null)
                {
                    // get template fields by document type.
                    string[] docTypeInfo = Regex.Split(ddlDocType.SelectedValue, ACAConstant.SPLIT_DOUBLE_COLON);
                    ITemplateBll templateBLL = ObjectFactory.GetObject<ITemplateBll>();
                    TemplateModel targetTemplate = templateBLL.GetDocumentTemplates(ConfigManager.AgencyCode, docTypeInfo[0], docTypeInfo[1]);
                    TemplateModel sourceTemplate = DataModel.SelectFromAccountTemplate;
                    GenericTemplateUtil.MergeGenericTemplate(sourceTemplate, targetTemplate, ModuleName);
                    DataModel.template = targetTemplate;
                }
                
                genericTemplate.Display(DataModel.template);
            }
        }

        /// <summary>
        /// Initial virtual folders.
        /// </summary>
        private void InitVirtaulFolders()
        {
            if (AppSession.IsAdmin)
            {
                ckbVirtualFolders.IsHidden = false;
            }

            CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);

            if (cap != null
                && cap.capType != null
                && !string.IsNullOrEmpty(cap.capType.virtualFolderGroup))
            {
                ListItem[] virtualFolders = AttachmentUtil.GetVirtualFolders(cap.capType.virtualFolderGroup);

                if (virtualFolders.Length > 0)
                {
                    ckbVirtualFolders.Items.AddRange(virtualFolders);
                    ckbVirtualFolders.IsHidden = false;
                }
            }
        }

        /// <summary>
        /// Get value from UI.
        /// </summary>
        private void RefreshValues()
        {
            if (!ddlAssociatedPeople.IsHidden && !string.IsNullOrEmpty(ddlAssociatedPeople.SelectedValue))
            {
                string[] associatedPeopleInfo = ddlAssociatedPeople.SelectedValue.Split(new char[] { ACAConstant.SPLIT_CHAR4URL1 }, StringSplitOptions.None);

                if (associatedPeopleInfo.Length == 4)
                {
                    DataModel.entityType = associatedPeopleInfo[0];
                    DataModel.SpecificEntityType = associatedPeopleInfo[1];
                    DataModel.entityID = associatedPeopleInfo[2];
                    DataModel.serviceProviderCode = associatedPeopleInfo[3];
                }
            }

            //general upload
            if (DataModel.sourceDocNbr == null || DataModel.sourceDocNbr == 0)
            {
                string[] alsoAttachToInfo = ddlAlsoAttachTo.SelectedValue.Split(new char[] { ACAConstant.SPLIT_CHAR4URL1 }, StringSplitOptions.None);
                DataModel.AlsoAttachTo = ddlAlsoAttachTo.SelectedValue;

                if (alsoAttachToInfo.Length == 4)
                {
                    DataModel.sourceRecfulnam = AppSession.User.PublicUserId;
                    DataModel.sourceEntityType = alsoAttachToInfo[0];
                    DataModel.sourceEntityID = alsoAttachToInfo[2];
                    DataModel.sourceSpc = alsoAttachToInfo[3];
                    DataModel.serviceProviderCode = ConfigManager.AgencyCode;
                    DataModel.fileOwnerPermission = ACAConstant.DEFAULT_FILEOWNERPERMISSION;
                }
            }
            else
            {
                // select from account description is disable and pass requird validation.
                txtAFileDescription.DisableEdit();
            }

            if (!string.IsNullOrEmpty(ddlDocType.SelectedValue))
            {
                string[] docTypeInfo = Regex.Split(ddlDocType.SelectedValue, ACAConstant.SPLIT_DOUBLE_COLON);
                DataModel.docGroup = docTypeInfo[0];
                DataModel.docCategory = docTypeInfo[1];
                DataModel.resDocCategory = ddlDocType.SelectedItem.Text;
            }
            else
            {
                DataModel.docGroup = null;
                DataModel.docCategory = null;
                DataModel.resDocCategory = null;
            }

            DataModel.docDescription = txtAFileDescription.Text.Trim();

            if (!ckbVirtualFolders.IsHidden)
            {
                DataModel.virtualFolders = DataUtil.ConcatStringWithSplitChar(ckbVirtualFolders.GetSelectValues(), ACAConstant.SPLIT_CHAR_SEMICOLON.ToString());
            }
            else
            {
                DataModel.virtualFolders = null;
            }

            DataModel.template = genericTemplate.GetTemplateModel(true);

            //set default permission for ref document
            if (IsAccountManagerPage)
            {
                DataModel.fileOwnerPermission = ACAConstant.DEFAULT_FILEOWNERPERMISSION;
            }
        }

        /// <summary>
        /// Bind associated LPs and Contacts into dropdownlist.
        /// </summary>
        private void BindAssociatedPeople()
        {
            List<ListItem> items = new List<ListItem>();

            if (!AppSession.IsAdmin)
            {
                //Add associated License Professional with public user to Attach-To dropdown list.
                items = GetLPList();

                //Add associated Contact with public user to Attach-To dropdown list.
                items.AddRange(GetContactListFromUser());
            }

            DropDownListBindUtil.BindDDL(items, ddlAssociatedPeople, true, false);
        }

        /// <summary>
        /// Bind associated LPs and Contacts into dropdownlist.
        /// </summary>
        private void BindAlsoAttachTo()
        {
            List<ListItem> items = new List<ListItem>();

            if (!AppSession.IsAdmin)
            {
                //Add associated License Professional with public user to Attach-To dropdown list.
                items = GetLPList(false);

                //Add associated Contact with public user to Attach-To dropdown list.
                items.AddRange(GetContactListFromCurrentRecord());
            }

            DropDownListBindUtil.BindDDL(items, ddlAlsoAttachTo, true, false);
        }

        /// <summary>
        /// Build associated LP option for Also Attach To dropdown list. 
        /// </summary>
        /// <param name="isForAccount">Is for account. The default value is true.</param>
        /// <returns>all option in dropdown list (Attach To/Also Attach To)</returns>
        private List<ListItem> GetLPList(bool isForAccount = true)
        {
            List<LicenseModel4WS> tempResult = AttachmentUtil.GetAvaliableLicense4PeopleDocument(ConfigManager.SuperAgencyCode);
            List<LicenseProfessionalModel4WS> lp4PeopleDocument = LicenseUtil.ConvertLicenseModel2LicenseProfessionalModel4WS(tempResult, string.Empty);

            if (!isForAccount)
            {
                CapModel4WS capModel = null;
               
                if (!AppSession.IsAdmin)
                {
                    capModel = AppSession.GetCapModelFromSession(ModuleName);
                }

                if (capModel != null && capModel.licenseProfessionalList != null)
                {
                    /*
                     * if the liceSeqNbr is empty or -3 then means this is daily LP.
                     * LP must belong current public user and current agency.
                     */
                    var recordLPList = capModel.licenseProfessionalList.Where(lp => !string.IsNullOrEmpty(lp.licSeqNbr) && !ACAConstant.DAILY_LICENSE_NUMBER.Equals(lp.licSeqNbr, StringComparison.InvariantCulture));
                    lp4PeopleDocument = lp4PeopleDocument.Where(lp4Account => recordLPList.Any(lp4Record => lp4Record.licSeqNbr.Equals(lp4Account.licSeqNbr)
                                                                              && lp4Record.licenseType.Equals(lp4Account.licenseType))).ToList();
                }
                else
                {
                    lp4PeopleDocument = new List<LicenseProfessionalModel4WS>();
                }
            }

            List<ListItem> items = new List<ListItem>();
            List<ListItem> tempItems = new List<ListItem>();

            // current cap has license information
            foreach (LicenseProfessionalModel4WS license in lp4PeopleDocument)
            {
                ListItem item = new ListItem();
                string companyName = license.businessName;

                if (string.IsNullOrEmpty(companyName))
                {
                    string[] fullName = { license.contactFirstName, license.contactMiddleName, license.contactLastName };
                    companyName = DataUtil.ConcatStringWithSplitChar(fullName, ACAConstant.BLANK);
                }

                string[] licenseText = { companyName, license.licenseNbr };
                item.Text = DataUtil.ConcatStringWithSplitChar(licenseText, ACAConstant.SPLITLINE);
                item.Value = string.Format(
                                            "{1}{0}{2}{0}{3}{0}{4}",
                                            ACAConstant.SPLIT_CHAR4URL1,
                                            DocumentEntityType.LP,
                                            license.licenseType,
                                            license.licSeqNbr,
                                            license.agencyCode);
                tempItems.Add(item);
            }

            if (tempItems.Count > 0)
            {
                ListItem item = new ListItem();
                item.Text = GetTextByKey("aca_contactedit_autofillitems_associatedlicenseseparator");
                item.Value = ACAConstant.OPTION_GROUP;
                items.Add(item);
                tempItems.Sort(ListItemComparer.Instance);
            }

            items.AddRange(tempItems);

            return items;
        }

        /// <summary>
        /// Get contact list from current record.
        /// </summary>
        /// <returns>all options in dropdown list Also Attach To</returns>
        private List<ListItem> GetContactListFromCurrentRecord()
        {
            //get contact information from current record associations
            CapModel4WS capModel = null;
            List<ListItem> items = new List<ListItem>();
            List<ListItem> tempItems = new List<ListItem>();
            
            if (!AppSession.IsAdmin)
            {
                capModel = AppSession.GetCapModelFromSession(ModuleName);
            }

            // 'Also Attach To' contact's items only allow come from current Agency.
            var refPeoples = AttachmentUtil.GetAvaliableContact4PeopleDocument().Where(contact => contact.serviceProviderCode.Equals(ConfigManager.AgencyCode, StringComparison.InvariantCulture));

            List<CapContactModel4WS> contacts = new List<CapContactModel4WS>();

            if (capModel != null && capModel.contactsGroup != null)
            {
                foreach (var capContactModel in capModel.contactsGroup)
                {
                    if (contacts.All(o => o.refContactNumber != capContactModel.refContactNumber))
                    {
                        contacts.Add(capContactModel);
                    }
                }
            }

            if (capModel != null
                && capModel.capContactModel != null
                && capModel.capContactModel.refContactNumber != null
                && contacts.All(o => o.refContactNumber != capModel.capContactModel.refContactNumber))
            {
                contacts.Add(capModel.capContactModel);
            }

            List<PeopleModel4WS> peopleModels = new List<PeopleModel4WS>();
            
            foreach (CapContactModel4WS contact in contacts)
            {
                var refPeople = refPeoples.SingleOrDefault(o => o.contactSeqNumber.Equals(contact.refContactNumber));
                peopleModels.Add(refPeople);
            }

            List<ListItem> contactList = GetContactList(peopleModels);

            if (contactList != null && contactList.Count > 0)
            {
                tempItems.AddRange(contactList);    
            }

            if (tempItems.Count > 0)
            {
                ListItem item = new ListItem();
                item.Text = GetTextByKey("ACA_ContactEdit_AutoFillItems_AssociatedContactSeparator");
                item.Value = ACAConstant.OPTION_GROUP;
                items.Add(item);
                tempItems.Sort(ListItemComparer.Instance);
            }

            items.AddRange(tempItems);

            return items;
        }

        /// <summary>
        /// build associated contact option for attach to dropdown list. 
        /// </summary>
        /// <returns>all option in dropdown list.</returns>
        private List<ListItem> GetContactListFromUser()
        {
            List<PeopleModel4WS> refPeoples = new List<PeopleModel4WS>();
            List<ListItem> items = new List<ListItem>();
            List<ListItem> tempItems = new List<ListItem>();

            if (!AppSession.IsAdmin)
            {
                //get contact information from current user associations
                refPeoples = AttachmentUtil.GetAvaliableContact4PeopleDocument();
            }

            tempItems.AddRange(GetContactList(refPeoples));

            if (tempItems.Count > 0)
            {
                ListItem item = new ListItem();
                item.Text = GetTextByKey("ACA_ContactEdit_AutoFillItems_AssociatedContactSeparator");
                item.Value = ACAConstant.OPTION_GROUP;
                items.Add(item);
                tempItems.Sort(ListItemComparer.Instance);
            }

            items.AddRange(tempItems);

            return items;
        }

        /// <summary>
        /// Get contact list item
        /// </summary>
        /// <param name="refPeoples">People array</param>
        /// <returns>contact list item</returns>
        private List<ListItem> GetContactList(List<PeopleModel4WS> refPeoples)
        {
            if (refPeoples == null || !refPeoples.Any())
            {
                return null;
            }

            List<ListItem> tempItems = new List<ListItem>();

            // current user has contact information.
            foreach (PeopleModel4WS refPeople in refPeoples)
            {
                ListItem people = GetContact(refPeople);

                if (refPeople != null
                    && !string.IsNullOrEmpty(people.Text)
                    && !string.IsNullOrEmpty(people.Value)
                    && tempItems.All(o => o.Value != people.Value)
                    && ConfigManager.AgencyCode.Equals(refPeople.serviceProviderCode, StringComparison.InvariantCulture))
                {
                    tempItems.Add(GetContact(refPeople));
                }
            }

            return tempItems;
        }

        /// <summary>
        /// Get contact item
        /// </summary>
        /// <param name="refPeople">People contact</param>
        /// <returns>contact item</returns>
        private ListItem GetContact(PeopleModel4WS refPeople)
        {
            if (refPeople == null)
            {
                return null;
            }

            ListItem item = new ListItem();
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            string contactText = peopleBll.GetContactUserName(refPeople, true);

            if (!string.IsNullOrEmpty(contactText))
            {
                item.Text = contactText;
                item.Value = string.Format(
                                           "{1}{0}{2}{0}{3}{0}{4}",
                                           ACAConstant.SPLIT_CHAR4URL1,
                                           DocumentEntityType.RefContact,
                                           refPeople.contactType,
                                           refPeople.contactSeqNumber,
                                           refPeople.serviceProviderCode);
            }

            return item;
        }

        /// <summary>
        /// Bind document type dropdown list by record.
        /// </summary>
        private void InitDocumentTypeByRecord()
        {
            if (AppSession.IsAdmin)
            {
                ddlDocType.IsHidden = false;
                DropDownListBindUtil.BindAllDocumentTypes(ddlDocType, IsAccountManagerPage);
                return;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            CapIDModel4WS capID = capModel != null ? capModel.capID : null;

            if (capID == null)
            {
                return;
            }

            bool documentTypeConfiged;
            List<XEntityPermissionModel> availableDocumentTypes = AttachmentUtil.GetAvailableDocumentTypesInPageFlow(capModel, ModuleName, ComponentName, out documentTypeConfiged);
            IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            RefDocumentModel[] documentTypes = edmsBll.GetDocumentTypes(capID, AppSession.User.PublicUserId);
            List<ListItem> validTypes = new List<ListItem>();
            bool needDocumentTypeFilter = documentTypeConfiged;

            if (documentTypes != null && documentTypes.Length > 0)
            {
                foreach (RefDocumentModel item in documentTypes)
                {
                    if (AttachmentUtil.HasUploadPrivilegeInPageFlow(capModel, availableDocumentTypes, item, needDocumentTypeFilter))
                    {
                        string resType = I18nStringUtil.GetString(item.resDocumentType, item.documentType);
                        ListItem listItem = new ListItem(resType, item.documentCode + ACAConstant.SPLIT_DOUBLE_COLON + item.documentType);
                        validTypes.Add(listItem);
                    }
                }
            }

            if (validTypes.Count > 0)
            {
                if (needDocumentTypeFilter && !availableDocumentTypes.Any())
                {
                    ddlDocType.IsHidden = true;
                }
                else
                {
                    ddlDocType.IsHidden = false;
                    DropDownListBindUtil.BindDDL(validTypes, ddlDocType, true, true);
                    SetSelectedValue4DocType();
                }
            }
            else
            {
                ddlDocType.IsHidden = true;
            }
        }

        /// <summary>
        /// Get document types by <c>xentity</c>
        /// </summary>
        /// <param name="xEntity"><c>Xentity</c> Permission Model</param>
        /// <returns>return document types in aca admin configuration</returns>
        private List<XEntityPermissionModel> GetDocumentTypesByXEntity(XEntityPermissionModel xEntity)
        {
            //Account Manager page or Cap Detail page not need available document types.
            if (IsAccountManagerPage || IsCapDetailPage)
            {
                return null;
            }

            IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
            IEnumerable<XEntityPermissionModel> result = xEntityPermissionBll.GetXEntityPermissions(xEntity);

            return result != null ? result.ToList() : null;
        }

        /// <summary>
        /// Bind document type dropdown list by select license.
        /// </summary>
        /// <param name="selectValue">select license</param>
        private void InitDocumentTypeByPeople(string selectValue)
        {
            if (IsAccountManagerPage
                && !AppSession.IsAdmin
                && !string.IsNullOrEmpty(selectValue)
                && StandardChoiceUtil.IsEnableAccountAttachment())
            {
                var edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                RefDocumentModel[] documentTypes;
                var validTypes = new List<ListItem>();

                // get entity type by LP type
                string[] peopleInfo = selectValue.Split(new char[] { ACAConstant.SPLIT_CHAR4URL1 }, StringSplitOptions.None);

                string entityType = DocumentEntityType.LP.Equals(peopleInfo[0], StringComparison.InvariantCultureIgnoreCase) ? DocumentEntityType.RefLicenseProfessional : DocumentEntityType.RefContact;
                documentTypes = peopleInfo.Length > 2 ? edmsBll.GetDocTypeByPeopleType(ConfigManager.AgencyCode, entityType, peopleInfo[1]) : null;

                if (documentTypes != null && documentTypes.Length > 0)
                {
                    foreach (RefDocumentModel item in documentTypes)
                    {
                        string resType = I18nStringUtil.GetString(item.resDocumentType, item.documentType);
                        ListItem listItem = new ListItem(resType, item.documentCode + ACAConstant.SPLIT_DOUBLE_COLON + item.documentType);
                        validTypes.Add(listItem);
                    }
                }

                ddlDocType.IsHidden = validTypes.Count <= 0;

                DropDownListBindUtil.BindDDL(validTypes, ddlDocType, true, true);
                SetSelectedValue4DocType();
            }
        }

        /// <summary>
        /// Gets a value to indicating whether displaying the select from account button.
        /// </summary>
        /// <param name="documentModel">document model</param>
        /// <returns>Is show also attach to return true, otherwise return false.</returns>
        private bool IsShowAlsoAttachTo(DocumentModel documentModel)
        {
            if (AppSession.IsAdmin)
            {
                return true;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

            /*
             * in super agency create record and selected more than one servieces display also attachment to option.
             * if in confirm page or agency-defined disabled then hide the link.
             */
            if (IsAccountManagerPage
                || !FunctionTable.IsEnableUploadDocument()
                || !StandardChoiceUtil.IsEnableAccountAttachment()
                || documentModel.parentSeqNbr != null
                || (documentModel.sourceDocNbr != null && documentModel.sourceDocNbr > 0)
                || (StandardChoiceUtil.IsSuperAgency() && AppSession.GetSelectedServicesFromSession() != null && AppSession.GetSelectedServicesFromSession().Length > 1)
                || capBll.IsCreateByDelegateUser(capModel)
                || (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk))
            {
                return false;
            }

            CapIDModel4WS capID = capModel != null ? capModel.capID : null;
            EdmsPolicyModel4WS edmsPolicyModel = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, string.Empty, AppSession.User.PublicUserId, capID);

            // specific case: resume a application that is create in super agency and selected more than one servieces display also attachment to option.
            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            CapIDModel4WS[] childCapIDs = CapUtil.GetChildCapIDs(capModel, ModuleName, isAssoFormEnabled);

            if (childCapIDs != null && childCapIDs.Length >= 2)
            {
                return false;
            }

            if (edmsPolicyModel != null)
            {
                bool hasUploadPermission = AttachmentUtil.HasDocumentTypeUploadPermission(capModel, ModuleName, ComponentName, !IsAccountManagerPage && !IsCapDetailPage);
                return Convert.ToBoolean(edmsPolicyModel.uploadRight) && hasUploadPermission;
            }

            return false;
        }

        /// <summary>
        /// Set the Document Type's SelectedValue
        /// </summary>
        private void SetSelectedValue4DocType()
        {
            if (DropDownListBindUtil.IsExistOnlyOneItem(ddlDocType))
            {
                //In order to bind the template, you must set up the data selected
                ddlDocType.SelectedIndex = ddlDocType.Items.Count - 1;
            }
        }

        #endregion

        #endregion Methods
    }
}