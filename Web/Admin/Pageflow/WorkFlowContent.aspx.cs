#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WorkflowContent.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: WorkFlowContent.aspx.cs 279238 2014-10-15 09:35:57Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 09/05/2008           daly.zeng               initial version
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.EMSE;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Page to handler work flow
/// </summary>
[SuppressCsrfCheck]
public partial class Admin_Module_WorkFlowContent : Page
{
    #region Fields
    
    /// <summary>
    /// Action flag to get cap types
    /// </summary>
    private const string GET_CAPTYPES = "getCapTypes";

    /// <summary>
    /// Action flag to get supper agency setting.
    /// </summary>
    private const string IS_SUPPERAGENCY_ENABLED = "IsSupperAgencyEnabled";

    /// <summary>
    /// Action flag to get EMSE event names
    /// </summary>
    private const string GET_EMSE_EVENTS = "GetEmseEventNames"; //if this value has changed,must change the same variable 'GET_EMSE_EVENTS' in javascript

    /// <summary>
    /// Action flag to get page flow group
    /// </summary>
    private const string GET_PAGEFLOWGROUP = "GetPageFlowGroup";

    /// <summary>
    /// Action flag to get page flow name list
    /// </summary>
    private const string GET_PAGEFLOWGROUPNAME_LIST = "GetPageFlowGroupNameList";

    /// <summary>
    /// Action flag to get agency list
    /// </summary>
    private const string GET_AGENCYLIST = "GetAgencyList";

    /// <summary>
    /// Action flag to get agency code
    /// </summary>
    private const string SET_AGENCYCODE = "SetAgencyCode";

    /// <summary>
    /// Action flag to get related caps
    /// </summary>
    private const string GET_RELATED_CAPS = "GetRelatedCaps";

    /// <summary>
    /// Action flag to get related cap list
    /// </summary>
    private const string GET_RELATED_CAP_LIST = "GetRelatedCapList";

    /// <summary>
    /// Action flag to get standard choice
    /// </summary>
    private const string GET_STANDARD_CHOICE = "GetStandardChoice";

    /// <summary>
    /// Get ASI groups
    /// </summary>
    private const string GET_ASI_GROUPS = "GetASIGroups";

    /// <summary>
    /// Get ASI sub groups
    /// </summary>
    private const string GET_ASI_SUB_GROUPS = "GetASISubGroups";

    /// <summary>
    /// Get ASIT groups
    /// </summary>
    private const string GET_ASIT_GROUPS = "GetASITGroups";

    /// <summary>
    /// Get cap types groups
    /// </summary>
    private const string GET_CAP_TYPE_GROUPS = "GetCapTypeGroups";

    /// <summary>
    /// Get require document types 
    /// </summary>
    private const string GET_REQUIRE_DOCUMENT_TYPES = "GetRequireDocumentTypes";

    /// <summary>
    /// Get document types 
    /// </summary>
    private const string GET_DOCUMENT_TYPES = "GetDocumentTypes";

    /// <summary>
    /// An action type for gets settings for available document types for each Record Type and each Attachment section.
    /// </summary>
    private const string GET_DOCUMENT_TYPE_OPTION_CONFIG_HISTORY = "GetDocTypeOptConfigHistory";

    /// <summary>
    /// Action flag to get contact type list
    /// </summary>
    private const string GET_CONTACT_TYPE_LIST = "GetContactTypeList";

    /// <summary>
    /// Action flag to get language list
    /// </summary>
    private const string GET_CURRENTAGENCYCODE = "GetCurrentAgencyCode";

    /// <summary>
    /// Get ASIT sub groups
    /// </summary>
    private const string GET_ASIT_SUB_GROUPS = "GetASITSubGroups";

    /// <summary>
    /// Action flag to save page flow group
    /// </summary>
    private const string SAVE_PAGEFLOWGROUP = "SavePageFlowGroup";

    /// <summary>
    /// Action flag for script list
    /// </summary>
    private const string SCRIPTLIST = "--Scripts List--";

    /// <summary>
    /// Split line
    /// </summary>
    private const string SPLITLINE = " - ";

    /// <summary>
    /// Get the Contact Type associated ASI Group and ASI Sub Groups.
    /// </summary>
    private const string GET_CONTACT_GENERIC_TEMPLATE_GROUPS = "GetContactGenericTemplateGroups";

    /// <summary>
    /// The component key for old component sequence number.
    /// </summary>
    private const string COMPONENT_KEY_OLD_SEQ_NBR = "oldComponentSeqNbr";

    /// <summary>
    /// The component key for new component sequence number.
    /// </summary>
    private const string COMPONENT_KEY_NEW_SEQ_NBR = "newComponentSeqNbr";

    /// <summary>
    /// The component key json.
    /// </summary>
    private const string COMPONENT_KEY_JSON = "json";

    /// <summary>
    /// Create an instance of ILog
    /// </summary>
    private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(Admin_Module_WorkFlowContent));

    /// <summary>
    /// Cap type dictionary
    /// Key: record type key
    /// </summary>
    private static Dictionary<string, CapTypeModel> _dictCapType = new Dictionary<string, CapTypeModel>();

    /// <summary>
    /// Require document type dictionary
    /// Key is record type, Value is a dictionary: {key: DocumentType, value: ResDocumentType}.
    /// </summary>
    private static Dictionary<string, Dictionary<string, string>> _dictRequireDocumentType = new Dictionary<string, Dictionary<string, string>>();

    #endregion Fields

    #region Methods

    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!AppSession.IsAdmin)
        {
            Response.Redirect("../login.aspx");
        }

        IPageflowBll pageflowBll = (IPageflowBll)ObjectFactory.GetObject(typeof(IPageflowBll));
        ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
        IEMSEBll emseBll = (IEMSEBll)ObjectFactory.GetObject(typeof(IEMSEBll));
        IAppSpecificInfoBll asiBll = (IAppSpecificInfoBll)ObjectFactory.GetObject(typeof(IAppSpecificInfoBll));
        string groupCode = Request.Params["pageFlowGrpCode"];
        string moduleName = Request.Params["moduleName"];
        string asiGroupCode = Request.Params["groupCode"];
        string recordTypeKey = Request.Params["capTypeKey"];
        string componentSeqNbr = Request.Params["componentSeqNbr"];

        switch (Request.Params["action"])
        {
            case GET_PAGEFLOWGROUPNAME_LIST:
                string[] pageFlowGroupNames = pageflowBll.GetPageFlowGroupNameList("PERMIT");
                string defaultSelectValue = Request.Params["defaultSelectValue"];

                if (!string.IsNullOrEmpty(defaultSelectValue))
                {
                    Response.Write(ConvertArrayToJson(pageFlowGroupNames, defaultSelectValue));
                }
                else
                {
                    Response.Write(ConvertArrayToJson(pageFlowGroupNames));
                }

                break;
            case GET_CAPTYPES:
                break;
            case SAVE_PAGEFLOWGROUP:
                string mode = Request.Params["mode"];
                PageFlowGroupModel pageFlowGroup = (PageFlowGroupModel)JsonConvert.DeserializeObject(Request.Params["json"], typeof(PageFlowGroupModel));

                ConverPageFlowGroup(pageFlowGroup, true);
                ArrayList filterCapTypeNames = new ArrayList();

                foreach (string capTypeName in pageFlowGroup.capTypeNameList)
                {
                    filterCapTypeNames.Add(ScriptFilter.DecodeJson(capTypeName));
                }

                foreach (var step in pageFlowGroup.stepList)
                {
                    step.stepName = ScriptFilter.DecodeJson(step.stepName);

                    foreach (var page in step.pageList)
                    {
                        page.instruction = ScriptFilter.DecodeJson(page.instruction);
                        page.pageName = ScriptFilter.DecodeJson(page.pageName);

                        foreach (var component in page.componentList)
                        {
                            component.customHeading = ScriptFilter.DecodeJson(component.customHeading);
                            component.instruction = ScriptFilter.DecodeJson(component.instruction).Replace("&amp;", "&");
                            component.resInstruction = ScriptFilter.DecodeJson(component.resInstruction).Replace("&amp;", "&");
                        }
                    }
                }

                filterCapTypeNames.ToArray().CopyTo(pageFlowGroup.capTypeNameList, 0);
                pageFlowGroup.pageFlowGrpCode = ScriptFilter.DecodeJson(pageFlowGroup.pageFlowGrpCode);

                if (!I18nCultureUtil.IsInMasterLanguage)
                {
                    UpdatePageFlowGroupModel(pageFlowGroup, false);
                }

                MakePageFlowComponentUnique(pageFlowGroup);

                if (mode.Equals("New", StringComparison.InvariantCulture))
                {
                    pageflowBll.CreatePageFlowGroup(pageFlowGroup, moduleName);
                }
                else if (mode.Equals("Edit", StringComparison.InvariantCulture))
                {
                    pageflowBll.EditePageFlowGroup(pageFlowGroup, moduleName);
                }

                // put it after the CreatePageFlowGroup/EditePageFlowGroup, because the DB table for save contact type options depend on it.
                SaveContactTypeList(moduleName, pageFlowGroup.pageFlowGrpCode);
                SaveDocumentTypeList(moduleName, pageFlowGroup.pageFlowGrpCode);

                break;
            case GET_PAGEFLOWGROUP:
                try
                {
                    PageFlowGroupModel pageflowGroup = pageflowBll.GetPageFlowGroup(moduleName, groupCode);

                    if (!I18nCultureUtil.IsInMasterLanguage)
                    {
                        UpdatePageFlowGroupModel(pageflowGroup, true);
                    }

                    if (pageflowGroup != null)
                    {
                        ConverPageFlowGroup(pageflowGroup, false);
                        string result = JsonConvert.SerializeObject(pageflowGroup);

                        if (Logger.IsDebugEnabled && pageflowGroup.stepList == null)
                        {
                            Logger.Debug(result);
                        }

                        Response.Write(result);
                    }
                    else
                    {
                        if (Logger.IsDebugEnabled)
                        {
                            Logger.DebugFormat("Page Flow model is null when loading. Group Code is {0}", groupCode);
                        }

                        Response.Write("{}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    Response.Write("-1");
                }

                break;
            case GET_RELATED_CAP_LIST:
                CapTypeModel[] relatedCaps = capTypeBll.GetCapTypeListByPageflowGroupCode(groupCode, moduleName);
                Response.Write(CreateCapListJson(relatedCaps));
                break;
            case GET_RELATED_CAPS:
                CapTypeModel[] caps = capTypeBll.GetCapTypeList4ACAByModule(moduleName, null); //"Request.Params["ModuleName"]);
                Response.Write(CreateTreeNodeListJson(caps, groupCode));
                break;
            case GET_STANDARD_CHOICE:
                string stdCatName = Request.Params["standardChoiceName"];

                IBizDomainBll _standChoiceBll1 = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;

                string[] contactTypeValueList = null;
                IList<ItemValue> contactItems = _standChoiceBll1.GetContactTypeList(ConfigManager.AgencyCode, false, ContactTypeSource.Transaction);
                StringBuilder sb = new StringBuilder();

                foreach (ItemValue contactItem in contactItems)
                {
                    sb.Append(contactItem.Value.ToString() + "|");
                }

                if (sb != null)
                {
                    sb.Length -= 1;
                    contactTypeValueList = sb.ToString().Split('|');
                }

                Response.Write(ConvertArrayToJson(contactTypeValueList));
                break;
            case GET_EMSE_EVENTS:
                object[] emseObj = emseBll.GetScriptNameList();
                string[] emseEvents = ConvertObjectToString(emseObj);
                Response.Write(ConvertArrayToJson(emseEvents, SCRIPTLIST));

                break;
            case IS_SUPPERAGENCY_ENABLED:
                IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
                string agencyCode = Request.Params["isGetParentAgency"] == null ? ConfigManager.AgencyCode : ConfigManager.SuperAgencyCode;
                string isSuperAgency = bizBll.GetValueForStandardChoice(agencyCode, BizDomainConstant.STD_CAT_MULTI_SERVICE_SETTINGS, BizDomainConstant.STD_ITEM_IS_SUPER_AGENCY);
                Response.Write(ValidationUtil.IsYes(isSuperAgency) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
                break;
            case GET_ASI_GROUPS:
                GActivitySpecInfoGroupCodeModel[] asiGroupList = asiBll.GetASIGroups(ConfigManager.AgencyCode);
                string asiGroups = GetASIGroups(asiGroupList);
                Response.Write(asiGroups);

                break;
            case GET_ASI_SUB_GROUPS:
                GActivitySpecInfoGroupCodeModel[] asiSubGroupList = asiBll.GetASIGroups(ConfigManager.AgencyCode);
                string asiSubGroups = GetASISubGroups(asiSubGroupList, asiGroupCode);
                Response.Write(asiSubGroups);

                break;
            case GET_ASIT_GROUPS:
                GActivitySpecInfoGroupCodeModel[] asitGroupList = asiBll.GetASITGroups(ConfigManager.AgencyCode);
                string asitGroups = GetASIGroups(asitGroupList);
                Response.Write(asitGroups);

                break;
            case GET_CAP_TYPE_GROUPS:
                CapTypeModel[] capTypes = capTypeBll.GetCapTypeListByPageflowGroupCode(groupCode, moduleName);

                if (capTypes != null)
                {
                    Response.Write(GetCapTypeGroups(capTypes));
                }

                break;
            case GET_DOCUMENT_TYPE_OPTION_CONFIG_HISTORY:
                var docTypeOptConfigs = GetDocTypeOptConfigHistory(groupCode, moduleName);

                if (docTypeOptConfigs == null)
                {
                    docTypeOptConfigs = new Dictionary<string, Dictionary<string, List<DocumentTypeOptionModel>>>();
                }

                Response.Write(JsonConvert.SerializeObject(docTypeOptConfigs));

                break;
            case GET_REQUIRE_DOCUMENT_TYPES:
                Response.Write(JsonConvert.SerializeObject(_dictRequireDocumentType));
                break;
            case GET_DOCUMENT_TYPES:
                string documentTypes = GetDocumentTypeListByRecordType(groupCode, moduleName, recordTypeKey, string.Format("Attachment_{0}", componentSeqNbr));

                Response.Write(documentTypes);
                break;
            case GET_ASIT_SUB_GROUPS:
                GActivitySpecInfoGroupCodeModel[] asitSubGroupList = asiBll.GetASITGroups(ConfigManager.AgencyCode);
                string asitSubGroups = GetASISubGroups(asitSubGroupList, asiGroupCode);
                Response.Write(asitSubGroups);
                break;
            case GET_CONTACT_TYPE_LIST:
                string contactTypeList = GetContactTypeListByPageFlow(groupCode, moduleName, componentSeqNbr);
                Response.Write(contactTypeList);
                break;
            case GET_AGENCYLIST:
                Dictionary<string, string> languageList = GetAgencyList();
                Response.Write(ConvertDictionaryToJson(languageList));
                break;
            case SET_AGENCYCODE:
                agencyCode = Request.Params[UrlConstant.AgencyCode];
                Session[SessionConstant.SESSION_USER_PREFERRED_AGENCYCODE] = agencyCode;
                break;
            case GET_CURRENTAGENCYCODE:
                string currentAgencyCode = Session[SessionConstant.SESSION_USER_PREFERRED_AGENCYCODE] == null ? ConfigManager.SuperAgencyCode : Session[SessionConstant.SESSION_USER_PREFERRED_AGENCYCODE].ToString();
                Response.Write(currentAgencyCode);
                break;
            case GET_CONTACT_GENERIC_TEMPLATE_GROUPS:
                string contactType = Request.Params["ContactType"];

                if (!I18nCultureUtil.IsInMasterLanguage)
                {
                    contactType = GetStandardChoiceKeyByValue(contactType);
                }

                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                TemplateModel model = templateBll.GetContactTemplates(ConfigManager.AgencyCode, contactType, false, AppSession.User.UserSeqNum);
                StringBuilder buf = new StringBuilder();

                if (model != null
                    && model.templateForms != null
                    && model.templateForms.Length > 0
                    && model.templateForms[0] != null
                    && !string.IsNullOrEmpty(model.templateForms[0].groupName)
                    && model.templateForms[0].subgroups != null
                    && model.templateForms[0].subgroups.Length > 0)
                {
                    TemplateGroup group = model.templateForms[0];

                    buf.AppendFormat("{{'ASIGroupName':'{0}','ASISubGroupName':[", group.groupName);

                    foreach (TemplateSubgroup item in group.subgroups)
                    {
                        buf.AppendFormat("'{0}',", item.subgroupName);
                    }

                    buf.Remove(buf.Length - 1, 1);
                    buf.Append("]}");
                }

                Response.Write(buf.ToString());
                break;
        }
    }
    
    /// <summary>
    /// Update the document type options.
    /// </summary>
    /// <param name="documentTypeOptionModels">The DocumentTypeOptionModel list.</param>
    /// <param name="entityModel">The entity model.</param>
    /// <param name="dictComponentInfo">The component information.</param>
    private static void UpdateDocumentTypeOptions(DocumentTypeOptionModel[] documentTypeOptionModels, XEntityPermissionModel entityModel, Dictionary<string, object> dictComponentInfo)
    {
        List<XEntityPermissionModel> xEntities = new List<XEntityPermissionModel>();

        // delete the entities that has modified per CAP type.
        XEntityPermissionModel deletePk = new XEntityPermissionModel();
        deletePk.servProvCode = ConfigManager.AgencyCode;
        deletePk.entityType = XEntityPermissionConstant.DOCUMENT_TYPE_OPTIONS;
        deletePk.entityId = entityModel.entityId;
        deletePk.entityId2 = entityModel.entityId2;
        deletePk.entityId3 = entityModel.entityId3;
        deletePk.componentName = string.Format("Attachment_{0}", dictComponentInfo[COMPONENT_KEY_OLD_SEQ_NBR]);

        // insert the new entities for that modified per CAP type.
        foreach (DocumentTypeOptionModel model in documentTypeOptionModels)
        {
            XEntityPermissionModel xEntity = new XEntityPermissionModel();

            xEntity.servProvCode = ConfigManager.AgencyCode;
            xEntity.entityType = XEntityPermissionConstant.DOCUMENT_TYPE_OPTIONS;
            xEntity.entityId = entityModel.entityId;
            xEntity.entityId2 = entityModel.entityId2;
            xEntity.entityId3 = entityModel.entityId3;
            xEntity.entityId4 = model.DocumentType;
            xEntity.componentName = string.Format("Attachment_{0}", dictComponentInfo[COMPONENT_KEY_NEW_SEQ_NBR]);
            xEntity.permissionValue = model.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            xEntity.recFulNam = ACAConstant.ADMIN_CALLER_ID;
            xEntity.recStatus = ACAConstant.VALID_STATUS;
            xEntities.Add(xEntity);
        }

        IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
        xEntityPermissionBll.UpdateXEntityPermissions(deletePk, xEntities);
    }

    /// <summary>
    /// Get the contact type list for page flow's ContactList Component.
    /// </summary>
    /// <param name="pageFlowGroupCode">The page flow group code.</param>
    /// <param name="moduleName">The module name.</param>
    /// <param name="componentSeqNbr">The component sequence number.</param>
    /// <returns>The contact type list.</returns>
    private static string GetContactTypeListByPageFlow(string pageFlowGroupCode, string moduleName, string componentSeqNbr)
    {
        XEntityPermissionModel xentity = new XEntityPermissionModel();
        xentity.servProvCode = ConfigManager.AgencyCode;
        xentity.entityType = XEntityPermissionConstant.CONTACT_DATA_VALIDATION;
        xentity.entityId = moduleName;
        xentity.entityId2 = pageFlowGroupCode;
        xentity.componentName = string.Format("MultiContacts_{0}", componentSeqNbr);

        List<ContactTypeUIModel> contactTypeList = DropDownListBindUtil.GetContactTypesByXEntity(xentity, false);

        string jasonChar = string.Empty;

        if (contactTypeList.Count > 0)
        {
            jasonChar = JsonConvert.SerializeObject(contactTypeList);
        }

        return jasonChar;
    }

    /// <summary>
    /// Get document type list by record type
    /// </summary>
    /// <param name="pageFlowGroupCode">page flow group code</param>
    /// <param name="moduleName">module name</param>
    /// <param name="recordTypeKey">record type</param>
    /// <param name="componentName">the component name</param>
    /// <returns>return document type list</returns>
    private static string GetDocumentTypeListByRecordType(string pageFlowGroupCode, string moduleName, string recordTypeKey, string componentName)
    {
        XEntityPermissionModel xentity = new XEntityPermissionModel();
        xentity.servProvCode = ConfigManager.AgencyCode;
        xentity.entityType = XEntityPermissionConstant.DOCUMENT_TYPE_OPTIONS;
        xentity.entityId = moduleName;
        xentity.entityId2 = pageFlowGroupCode;
        xentity.entityId3 = recordTypeKey;
        xentity.componentName = componentName;

        CapTypeModel capTypeModel = _dictCapType.ContainsKey(recordTypeKey) ? _dictCapType[recordTypeKey] : null;

        DocumentTypeUIModel documentTypeList = GetDocumentTypesByXEntity(xentity, recordTypeKey, capTypeModel);
        string jasonChar = string.Empty;

        if (documentTypeList != null)
        {
            jasonChar = JsonConvert.SerializeObject(documentTypeList);
        }

        return jasonChar;
    }

    /// <summary>
    /// Gets the settings for available document types for each Record Type and each Attachment section.
    /// </summary>
    /// <param name="pageFlowGroupCode">page flow group code</param>
    /// <param name="moduleName">module name</param>
    /// <returns>
    /// Return a dictionary
    /// Dictionary structure: {Key:ComponentName, Value:{Key: Record Type, Value: DocumentTypeOptionModel list}}.
    /// </returns>
    private static Dictionary<string, Dictionary<string, List<DocumentTypeOptionModel>>> GetDocTypeOptConfigHistory(string pageFlowGroupCode, string moduleName)
    {
        Dictionary<string, Dictionary<string, List<DocumentTypeOptionModel>>> _dictDocTypeOptConfigHistory
            = new Dictionary<string, Dictionary<string, List<DocumentTypeOptionModel>>>();

        XEntityPermissionModel xentity = new XEntityPermissionModel();
        xentity.servProvCode = ConfigManager.AgencyCode;
        xentity.entityType = XEntityPermissionConstant.DOCUMENT_TYPE_OPTIONS;
        xentity.entityId = moduleName;
        xentity.entityId2 = pageFlowGroupCode;

        IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
        IEnumerable<XEntityPermissionModel> documentTypeEntities = xEntityPermissionBll.GetXEntityPermissions(xentity);

        IEnumerable<string> componentNameList = GetComponentNameList(documentTypeEntities);

        if (componentNameList == null || !componentNameList.Any())
        {
            return null;
        }

        foreach (string componentName in componentNameList)
        {
            IEnumerable<string> recordTypeList = GetRecordTypeList(componentName, documentTypeEntities);

            if (recordTypeList == null || !recordTypeList.Any())
            {
                continue;
            }

            Dictionary<string, List<DocumentTypeOptionModel>> recordTypeOpts
                = new Dictionary<string, List<DocumentTypeOptionModel>>();

            foreach (string recordType in recordTypeList)
            {
                var docTypeOptions = documentTypeEntities.Where(item => item.componentName == componentName
                                                               && item.entityId3 == recordType);

                if (docTypeOptions == null || !docTypeOptions.Any())
                {
                    continue;
                }

                List<DocumentTypeOptionModel> docTypeOptionModels = new List<DocumentTypeOptionModel>();

                foreach (XEntityPermissionModel item in docTypeOptions)
                {
                    DocumentTypeOptionModel docTypeOpt = new DocumentTypeOptionModel();
                    docTypeOpt.DocumentType = item.entityId4;
                    docTypeOpt.Checked = ValidationUtil.IsYes(item.permissionValue);
                    docTypeOptionModels.Add(docTypeOpt);
                }

                recordTypeOpts.Add(recordType, docTypeOptionModels);
            }

            _dictDocTypeOptConfigHistory.Add(componentName, recordTypeOpts);
        }

        return _dictDocTypeOptConfigHistory;
    }

    /// <summary>
    /// Gets component name list from document type option settings.
    /// </summary>
    /// <param name="documentTypeEntities">Document type option settings list.</param>
    /// <returns>return component name list</returns>
    private static IEnumerable<string> GetComponentNameList(IEnumerable<XEntityPermissionModel> documentTypeEntities)
    {
        if (documentTypeEntities == null || !documentTypeEntities.Any())
        {
            return null;
        }

        List<string> componentNameList = new List<string>();

        foreach (XEntityPermissionModel xEntityPermissionModel in documentTypeEntities)
        {
            if (!string.IsNullOrEmpty(xEntityPermissionModel.componentName)
                && !componentNameList.Contains(xEntityPermissionModel.componentName))
            {
                componentNameList.Add(xEntityPermissionModel.componentName);
            }
        }

        return componentNameList;
    }

    /// <summary>
    /// Gets record type list from document type option settings.
    /// </summary>
    /// <param name="componentName">component name</param>
    /// <param name="documentTypeEntities">Document type option settings list.</param>
    /// <returns>return record type group list</returns>
    private static IEnumerable<string> GetRecordTypeList(string componentName, IEnumerable<XEntityPermissionModel> documentTypeEntities)
    {
        if (documentTypeEntities == null || !documentTypeEntities.Any())
        {
            return null;
        }

        IEnumerable<XEntityPermissionModel> filterdocumentTypeEntities =
            documentTypeEntities.Where(dte => dte.componentName == componentName);

        if (filterdocumentTypeEntities == null || !filterdocumentTypeEntities.Any())
        {
            return null;
        }

        List<string> recordTypeList = new List<string>();

        foreach (XEntityPermissionModel xEntityPermissionModel in filterdocumentTypeEntities)
        {
            if (!recordTypeList.Contains(xEntityPermissionModel.entityId3))
            {
                recordTypeList.Add(xEntityPermissionModel.entityId3);
            }
        }

        return recordTypeList;
    }

    /// <summary>
    /// Get document type by xEntity.
    /// </summary>
    /// <param name="xEntity">the xEntity model</param>
    /// <param name="recordTypeKey">The record type key.</param>
    /// <param name="capTypeModel">return specify cap type of document types. If cap type is null return all document types</param>
    /// <returns>document type UI model list.</returns>
    private static DocumentTypeUIModel GetDocumentTypesByXEntity(XEntityPermissionModel xEntity, string recordTypeKey, CapTypeModel capTypeModel = null)
    {
        IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
        RefDocumentModel[] documentTypes = edmsBll.GetAllDocumentTypes(ConfigManager.AgencyCode, capTypeModel);

        if (documentTypes == null || documentTypes.Length <= 0)
        {
            return null;
        }

        IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
        List<DocumentTypeOptionModel> documentTypeList = new List<DocumentTypeOptionModel>();

        IEnumerable<XEntityPermissionModel> documentTypeEntities = xEntityPermissionBll.GetXEntityPermissions(xEntity);
        documentTypes = documentTypes.Where(model => ValidationUtil.IsYes(model.isRestrictDocType4ACA)).ToArray();

        if (documentTypeEntities != null)
        {
            foreach (RefDocumentModel model in documentTypes)
            {
                XEntityPermissionModel xEntityPermissionModel =
                    documentTypeEntities.FirstOrDefault(dtEntity => dtEntity.entityId4.Equals(model.documentType, StringComparison.InvariantCultureIgnoreCase));

                DocumentTypeOptionModel documentTypeOptionModel = new DocumentTypeOptionModel();
                documentTypeOptionModel.Checked = xEntityPermissionModel != null && ValidationUtil.IsYes(xEntityPermissionModel.permissionValue);
                documentTypeOptionModel.DocumentType = model.documentType;
                documentTypeOptionModel.ResDocumentType = I18nStringUtil.GetString(model.resDocumentType, model.documentType);

                documentTypeList.Add(documentTypeOptionModel);
            }
        }
        else
        {
            foreach (RefDocumentModel model in documentTypes)
            {
                DocumentTypeOptionModel documentTypeOptionModel = new DocumentTypeOptionModel();
                documentTypeOptionModel.Checked = true;
                documentTypeOptionModel.DocumentType = model.documentType;
                documentTypeOptionModel.ResDocumentType = I18nStringUtil.GetString(model.resDocumentType, model.documentType);

                documentTypeList.Add(documentTypeOptionModel);
            }
        }

        DocumentTypeUIModel result = new DocumentTypeUIModel();
        result.CapTypeKey = recordTypeKey;
        result.DocumentTypes = JsonConvert.SerializeObject(documentTypeList);

        return result;
    }

    /// <summary>
    /// Convert object array to hash table
    /// </summary>
    /// <param name="obj">object array</param>
    /// <param name="isSavePageflow">true if save page flow; otherwise, false.</param>
    /// <returns>a hash table.</returns>
    private static Hashtable ConverObjectToHashtable(object[] obj, bool isSavePageflow)
    {
        if (obj == null || obj.Length != 2)
        {
            return null;
        }

        object[] valueList = ((StringArray)obj[0]).item;
        object[] titleList = ((StringArray)obj[1]).item;

        if (valueList == null || titleList == null || valueList.Length != titleList.Length)
        {
            return null;
        }

        Hashtable htEmseEvents = new Hashtable();

        for (int i = 0; i < valueList.Length; i++)
        {
            string value = valueList[i] == null ? string.Empty : valueList[i].ToString();
            string title = titleList[i] == null ? string.Empty : titleList[i].ToString();
            if (isSavePageflow)
            {
                htEmseEvents.Add(value + SPLITLINE + title, value);
            }
            else
            {
                htEmseEvents.Add(value, value + SPLITLINE + title);
            }
        }

        return htEmseEvents;
    }

    /// <summary>
    /// since we save EMSE event name to database with only 'CODE' and display with "CODE+LABEL"
    /// we need to convert values of the three properties (<c>onloadEvent,beforeButton,afterButton</c>) in page when loading or saving page flow
    /// </summary>
    /// <param name="pageflowGroupModel4WS">PageFlowGroupModel object</param>
    /// <param name="isSavePageflow">save or get</param>
    private static void ConverPageFlowGroup(PageFlowGroupModel pageflowGroupModel4WS, bool isSavePageflow)
    {
        if (pageflowGroupModel4WS == null)
        {
            return;
        }

        IEMSEBll emseBll = (IEMSEBll)ObjectFactory.GetObject(typeof(IEMSEBll));
        object[] emseObj = emseBll.GetScriptNameList();
        Hashtable htEmseEvents = new Hashtable();
        htEmseEvents = ConverObjectToHashtable(emseObj, isSavePageflow);

        if (pageflowGroupModel4WS.stepList == null)
        {
            return;
        }

        foreach (StepModel step in pageflowGroupModel4WS.stepList)
        {
            foreach (PageModel page in step.pageList)
            {
                GetEMSEScriptsCode(page, htEmseEvents);
            }
        }
    }

    /// <summary>
    /// Convert object array to string
    /// </summary>
    /// <param name="obj">object array</param>
    /// <returns>string array</returns>
    private static string[] ConvertObjectToString(object[] obj)
    {
        if (obj == null || obj.Length != 2)
        {
            return null;
        }

        object[] valueList = ((StringArray)obj[0]).item;
        object[] titleList = ((StringArray)obj[1]).item;

        if (valueList.Length != titleList.Length)
        {
            return null;
        }

        //char c18 = (char)18;
        string[] emseEvents = new string[valueList.Length];
        for (int i = 0; i < valueList.Length; i++)
        {
            string value = valueList[i] == null ? string.Empty : valueList[i].ToString();
            string title = titleList[i] == null ? string.Empty : titleList[i].ToString();
            emseEvents.SetValue(value + SPLITLINE + title, i);
        }

        return emseEvents;
    }

    /// <summary>
    /// Get EMSE script code
    /// </summary>
    /// <param name="page">Current page model</param>
    /// <param name="htEmseEvents">Hashtable object</param>
    private static void GetEMSEScriptsCode(PageModel page, Hashtable htEmseEvents)
    {
        string afterClickEventName = GetEmseEventCode(htEmseEvents, page.afterClickEventName);
        page.afterClickEventName = afterClickEventName;

        string beforeClickEventName = GetEmseEventCode(htEmseEvents, page.beforeClickEventName);
        page.beforeClickEventName = beforeClickEventName;

        string onloadEventName = GetEmseEventCode(htEmseEvents, page.onloadEventName);
        page.onloadEventName = onloadEventName;
    }

    /// <summary>
    /// Get EMSE event code
    /// </summary>
    /// <param name="htEmseEvents">Hashtable that contains EMSE events</param>
    /// <param name="key">EMSE event key</param>
    /// <returns>EMSE event code string</returns>
    private static string GetEmseEventCode(Hashtable htEmseEvents, string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        if (htEmseEvents != null && htEmseEvents.ContainsKey(key))
        {
            return htEmseEvents[key].ToString();
        }
        else
        {
            return string.Empty;
        }
    }
    
    /// <summary>
    /// Get the component information.
    /// </summary>
    /// <param name="pageflowGroup">The page flow group.</param>
    /// <param name="javaScriptObject">The javascript object, for example: Contact Type Options/Attachment Type Options etc.</param>
    /// <returns>Return the component information.</returns>
    private static Dictionary<string, object> GetComponentInformation(PageFlowGroupModel pageflowGroup, JObject javaScriptObject)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        string displayOrder = javaScriptObject["displayOrder"].ToString();
        result.Add(COMPONENT_KEY_OLD_SEQ_NBR, javaScriptObject["cptSeqNbr"]);
        result.Add(COMPONENT_KEY_JSON, javaScriptObject["json"]);

        /* Need obtain the component sequence number from DB, because:
         * 1. The component is new created
         * 2. The component have droped that make the component sequence number changed
         * The display order is format with "{step index}_{page index}_{component index}"
         */
        if (string.IsNullOrEmpty(displayOrder) || displayOrder.Split('_').Length != 3)
        {
            return null;
        }

        string[] displayOrders = displayOrder.Split('_');
        int stepIndex = int.Parse(displayOrders[0]);
        int pageIndex = int.Parse(displayOrders[1]);
        int cptIndex = int.Parse(displayOrders[2]);

        var componentModel = (from stepModel in pageflowGroup.stepList
                              where stepModel.stepOrder == stepIndex
                              from pageModel in stepModel.pageList
                              where pageModel.pageOrder == pageIndex
                              from cptModel in pageModel.componentList
                              where cptModel.displayOrder == cptIndex
                              select cptModel).FirstOrDefault();

        if (componentModel != null)
        {
            result.Add(COMPONENT_KEY_NEW_SEQ_NBR, componentModel.componentSeqNbr);
        }

        return result;
    }

    /// <summary>
    /// Get Agency List.
    /// </summary>
    /// <returns>Supported language list</returns>
    private Dictionary<string, string> GetAgencyList()
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        IServiceProviderBll serviceProviderBll = (IServiceProviderBll)ObjectFactory.GetObject(typeof(IServiceProviderBll));

        //return subagencies include agency code from web configurer.
        DelegateUserModel delegateUser = new DelegateUserModel();
        delegateUser.parentServProvCode = ConfigManager.SuperAgencyCode;
        delegateUser.auditStatus = ACAConstant.VALID_STATUS;
        delegateUser.parentUserName = Session[SessionConstant.SESSION_ADMIN_USERNAME].ToString();

        string[] subAgencies = serviceProviderBll.GetSubAgencies(delegateUser);

        if (subAgencies == null || subAgencies.Length == 0)
        {
            return null;
        }

        foreach (string subAgency in subAgencies)
        {
            result.Add(subAgency, subAgency);
        }

        return result.OrderBy(entity => entity.Key).ToDictionary(e => e.Key, e => e.Value);
    }

    /// <summary>
    /// save contact type list.
    /// </summary>
    /// <param name="moduleName">the module name.</param>
    /// <param name="pageFlowGroupCode">the page flow group code.</param>
    private void SaveContactTypeList(string moduleName, string pageFlowGroupCode)
    {
        string contactTypeList = Request.Params["contactTypeList"];

        if (string.IsNullOrEmpty(contactTypeList))
        {
            return;
        }

        IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
        JObject arrContactTypeList = JsonConvert.DeserializeObject(contactTypeList) as JObject;

        if (arrContactTypeList == null || arrContactTypeList.Count == 0)
        {
            return;
        }

        IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
        PageFlowGroupModel pageflowGroup = pageflowBll.GetPageFlowGroup(moduleName, pageFlowGroupCode);

        foreach (var jsContactType in arrContactTypeList)
        {
            JObject jsContactTypeValue = jsContactType.Value as JObject;

            if (jsContactTypeValue == null)
            {
                continue;
            }

            Dictionary<string, object> dictComponentInfo = GetComponentInformation(pageflowGroup, jsContactTypeValue);

            if (dictComponentInfo == null)
            {
                continue;
            }

            ContactTypeUIModel[] contactTypeUiModels = JsonConvert.DeserializeObject<ContactTypeUIModel[]>(dictComponentInfo[COMPONENT_KEY_JSON].ToString());

            if (contactTypeUiModels == null || contactTypeUiModels.Length == 0)
            {
                continue;
            }

            List<XEntityPermissionModel> xentitys = new List<XEntityPermissionModel>();

            XEntityPermissionModel deletePK = new XEntityPermissionModel();
            deletePK.servProvCode = ConfigManager.AgencyCode;
            deletePK.entityType = XEntityPermissionConstant.CONTACT_DATA_VALIDATION;
            deletePK.entityId = moduleName;
            deletePK.entityId2 = pageFlowGroupCode;
            deletePK.componentName = string.Format("MultiContacts_{0}", dictComponentInfo[COMPONENT_KEY_OLD_SEQ_NBR]);

            foreach (ContactTypeUIModel contactTypeUIModel in contactTypeUiModels)
            {
                XEntityPermissionModel xentity = new XEntityPermissionModel();

                xentity.servProvCode = ConfigManager.AgencyCode;
                xentity.entityType = XEntityPermissionConstant.CONTACT_DATA_VALIDATION;
                xentity.entityId = moduleName;
                xentity.entityId2 = pageFlowGroupCode;
                xentity.entityId3 = contactTypeUIModel.Key;
                xentity.permissionValue = contactTypeUIModel.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                xentity.data1 = contactTypeUIModel.MinNum;
                xentity.data2 = contactTypeUIModel.MaxNum;
                xentity.componentName = string.Format("MultiContacts_{0}", dictComponentInfo[COMPONENT_KEY_NEW_SEQ_NBR]);
                xentity.recFulNam = ACAConstant.ADMIN_CALLER_ID;
                xentity.recStatus = ACAConstant.VALID_STATUS;
                xentitys.Add(xentity);
            }

            xEntityPermissionBll.UpdateXEntityPermissions(deletePK, xentitys.ToArray());
        }
    }

    /// <summary>
    /// save document type list to XEntityPermission.
    /// </summary>
    /// <param name="moduleName">the module name.</param>
    /// <param name="pageFlowGroupCode">the page flow group code.</param>
    private void SaveDocumentTypeList(string moduleName, string pageFlowGroupCode)
    {
        string documentTypeListJson = Request.Params["documentTypeList"];

        if (string.IsNullOrEmpty(documentTypeListJson))
        {
            return;
        }

        JObject arrDocumentTypeList = JsonConvert.DeserializeObject(documentTypeListJson) as JObject;

        if (arrDocumentTypeList == null || arrDocumentTypeList.Count == 0)
        {
            return;
        }

        IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
        PageFlowGroupModel pageflowGroup = pageflowBll.GetPageFlowGroup(moduleName, pageFlowGroupCode);

        foreach (var jsDocumentType in arrDocumentTypeList)
        {
            JObject jsDocumentTypeValue = jsDocumentType.Value as JObject;

            if (jsDocumentTypeValue == null)
            {
                continue;
            }

            Dictionary<string, object> dictComponentInfo = GetComponentInformation(pageflowGroup, jsDocumentTypeValue);

            if (dictComponentInfo == null)
            {
                continue;
            }

            JArray documentTypeUIModels = dictComponentInfo[COMPONENT_KEY_JSON] as JArray;

            if (documentTypeUIModels == null || documentTypeUIModels.Count == 0)
            {
                continue;
            }

            foreach (JObject documentTypeUIModel in documentTypeUIModels)
            {
                if (documentTypeUIModel == null || documentTypeUIModel.Count == 0)
                {
                    continue;
                }

                DocumentTypeOptionModel[] documentTypeOptionModels = JsonConvert.DeserializeObject<DocumentTypeOptionModel[]>(documentTypeUIModel["DocumentTypes"].ToString());
                XEntityPermissionModel entityModel = new XEntityPermissionModel();
                entityModel.entityId = moduleName;
                entityModel.entityId2 = pageFlowGroupCode;
                entityModel.entityId3 = documentTypeUIModel["CapTypeKey"].ToString();

                UpdateDocumentTypeOptions(documentTypeOptionModels, entityModel, dictComponentInfo);
            }
        }
    }

    /// <summary>
    /// Convert string array to json format
    /// </summary>
    /// <param name="array">string array</param>
    /// <returns>Json format string</returns>
    private string ConvertArrayToJson(string[] array)
    {
        if (array == null || array.Length < 1)
        {
            return "[[]]";
        }

        StringBuilder jasonStr = new StringBuilder();
        jasonStr.Append("[");

        foreach (string str in array)
        {
            jasonStr.Append("['" + ScriptFilter.EncodeJson(str) + "'],");
        }

        jasonStr.Remove(jasonStr.Length - 1, 1);
        jasonStr.Append("]");

        return jasonStr.ToString();
    }

    /// <summary>
    /// Convert string array to json format
    /// </summary>
    /// <param name="array">string array</param>
    /// <param name="defalutValue">default value</param>
    /// <returns>Json format string</returns>
    private string ConvertArrayToJson(string[] array, string defalutValue)
    {
        if (array == null || array.Length < 1)
        {
            if (string.IsNullOrEmpty(defalutValue))
            {
                return "[[]]";
            }
            else
            {
                return "[['" + defalutValue + "']]";
            }
        }

        StringBuilder jasonStr = new StringBuilder();
        jasonStr.Append("[");

        if (!string.IsNullOrEmpty(defalutValue))
        {
            jasonStr.Append("['" + ScriptFilter.EncodeJson(defalutValue) + "'],");
        }

        foreach (string str in array)
        {
            jasonStr.Append("['" + ScriptFilter.EncodeJson(str) + "'],");
        }

        jasonStr.Remove(jasonStr.Length - 1, 1);
        jasonStr.Append("]");

        return jasonStr.ToString();
    }

    /// <summary>
    /// Create json string for cap list
    /// </summary>
    /// <param name="relatedCaps">related cap array</param>
    /// <returns>json format string</returns>
    private string CreateCapListJson(CapTypeModel[] relatedCaps)
    {
        if (relatedCaps == null || relatedCaps.Length < 1)
        {
            return "{}";
        }

        StringBuilder buf = new StringBuilder();
        buf.Append("{'CapTypes':[");

        foreach (CapTypeModel capTypeModel in relatedCaps)
        {
            buf.Append("{'CapName':'");
            buf.Append(ScriptFilter.EncodeJson(CAPHelper.GetCapTypeLabel(capTypeModel)));
            buf.Append("','Alias':'");
            buf.Append(ScriptFilter.EncodeJson(CAPHelper.GetAliasOrCapTypeLabel(capTypeModel)));
            buf.Append("'},");
        }

        buf.Remove(buf.Length - 1, 1);
        buf.Append("]}");

        return buf.ToString();
    }

    /// <summary>
    /// Create json string for tree node.
    /// </summary>
    /// <param name="capTypeModel">CapTypeModel object</param>
    /// <param name="index">Index of current cap type</param>
    /// <returns>Json format string</returns>
    private string CreateTreeNodeJson(CapTypeModel capTypeModel, int index)
    {
        StringBuilder buf = new StringBuilder();

        buf.Append("{");
        buf.Append("id:");
        buf.Append(index);
        buf.Append(",text:'");

        string alias = CAPHelper.GetAliasOrCapTypeLabel(capTypeModel); // string.IsNullOrEmpty(capTypeModel.resAlias) ? capTypeModel.alias : capTypeModel.resAlias;
        buf.Append(ScriptFilter.EncodeJson(alias));

        buf.Append("',name:'");
        buf.Append(ScriptFilter.EncodeJson(capTypeModel.alias));

        buf.Append("',masterLangCapTypeName:'");
        buf.Append(ScriptFilter.EncodeJson(CAPHelper.GetCapTypeLabel(capTypeModel, CAPHelper.GetCapTypeOption.OnlyGetMasterLanguageCapType)));

        buf.Append("',binded:");
        buf.Append(string.IsNullOrEmpty(capTypeModel.smartChoiceCode4ACA) ? "false" : "true");
        buf.Append(",leaf:true,draggable:true");
        buf.Append("},");

        return buf.ToString();
    }

    /// <summary>
    /// Create json string for tree node list.
    /// </summary>
    /// <param name="relatedCaps">CapTypeModel array</param>
    /// <param name="groupCode">group code.</param>
    /// <returns>Json format string for tree node list</returns>
    private string CreateTreeNodeListJson(CapTypeModel[] relatedCaps, string groupCode)
    {
        if (relatedCaps == null || relatedCaps.Length < 1)
        {
            return "{}";
        }

        int index = 0;
        StringBuilder bufAvailable = new StringBuilder();
        StringBuilder bufSelected = new StringBuilder();

        foreach (CapTypeModel capTypeModel in relatedCaps)
        {
            string nodeJson = CreateTreeNodeJson(capTypeModel, index);

            //if (!string.IsNullOrEmpty(capTypeModel.smartChoiceCode4ACA) && capTypeModel.smartChoiceCode4ACA.Equals(groupCode))
            if (!string.IsNullOrEmpty(capTypeModel.smartChoiceCode4ACA) && capTypeModel.smartChoiceCode4ACA.Equals(groupCode, StringComparison.InvariantCulture))
            {
                bufSelected.Append(nodeJson.ToString());
                index++;
            }
            else
            {
                bufAvailable.Append(nodeJson.ToString());
                index++;
            }
        }

        if (bufAvailable.Length > 0)
        {
            bufAvailable.Remove(bufAvailable.Length - 1, 1);
        }

        if (bufSelected.Length > 0)
        {
            bufSelected.Remove(bufSelected.Length - 1, 1);
        }

        bufAvailable.Insert(0, "{'availableCaps':[");
        bufSelected.Insert(0, "'selectedCaps':[");
        bufAvailable.Append("],");
        bufSelected.Append("]");
        bufAvailable.Append(bufSelected.ToString());
        bufAvailable.Append("}");

        return bufAvailable.ToString();
    }

    /// <summary>
    /// Get standard choice key by value.
    /// </summary>
    /// <param name="value">standard choice value</param>
    /// <returns>standard choice key</returns>
    private string GetStandardChoiceKeyByValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
        IList<ItemValue> contactItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_CONTACT_TYPE, false);

        foreach (ItemValue contactItem in contactItems)
        {
            if (contactItem.Value == null)
            {
                continue;
            }

            if (contactItem.Value.ToString().Equals(value, StringComparison.InvariantCultureIgnoreCase))
            {
                if (contactItem.Key == null)
                {
                    return string.Empty;
                }
                else
                {
                    return contactItem.Key.ToString();
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Get standard choice value by key.
    /// </summary>
    /// <param name="key">standard choice key</param>
    /// <returns>standard choice value</returns>
    private string GetStandardChoiceValueByKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
        IList<ItemValue> contactItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_CONTACT_TYPE, false);

        foreach (ItemValue contactItem in contactItems)
        {
            if (contactItem.Key == null)
            {
                continue;
            }

            if (contactItem.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
            {
                if (contactItem.Value == null)
                {
                    return string.Empty;
                }
                else
                {
                    return contactItem.Value.ToString();
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Update custom heading value.
    /// </summary>
    /// <param name="componentList">ComponentModel array</param>
    /// <param name="isKey">yes:Get contact type value; no:Save contact type</param>
    private void UpdateCustomHeadingValue(ComponentModel[] componentList, bool isKey)
    {
        string componentName = string.Empty;
        string customHeading = string.Empty;
        string[] spearSectionArray = { GViewConstant.SECTION_APPLICANT, GViewConstant.SECTION_CONTACT1, GViewConstant.SECTION_CONTACT2, GViewConstant.SECTION_CONTACT3 };

        foreach (ComponentModel componet in componentList)
        {
            componentName = componet.componentName;
            customHeading = componet.customHeading;

            for (int i = 0; i < spearSectionArray.Length; i++)
            {
                if (componentName.Equals(spearSectionArray[i].ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    if (isKey)
                    {
                        //get contact type.
                        componet.customHeading = GetStandardChoiceValueByKey(customHeading);
                    }
                    else
                    {
                        //save contact type.
                        componet.resCustomHeading = componet.customHeading;
                        componet.customHeading = GetStandardChoiceKeyByValue(customHeading);
                    }
                }
            }
        }
    }

    /// <summary>
    /// update PageFlowGroupModel.
    /// </summary>
    /// <param name="pageFlowGroup">PageFlowGroupModel object</param>
    /// <param name="isKey">yes:Get contact type value; no:Save contact type</param>
    private void UpdatePageFlowGroupModel(PageFlowGroupModel pageFlowGroup, bool isKey)
    {
        if (pageFlowGroup == null || pageFlowGroup.stepList == null || pageFlowGroup.stepList.Length == 0)
        {
            return;
        }

        foreach (StepModel stepModel in pageFlowGroup.stepList)
        {
            if (stepModel == null)
            {
                continue;
            }

            foreach (PageModel pageModel in stepModel.pageList)
            {
                if (pageModel == null)
                {
                    continue;
                }

                //Update custom heading value.
                UpdateCustomHeadingValue(pageModel.componentList, isKey);
            }
        }
    }

    /// <summary>
    /// Get ASI/ASIT groups
    /// </summary>
    /// <param name="modelList">the GActivitySpecInfoGroupCodeModel, contain group and sub group</param>
    /// <returns>return the ASI/ASIT groups</returns>
    private string GetASIGroups(GActivitySpecInfoGroupCodeModel[] modelList)
    {
        List<string> groupList = new List<string>();

        // add the default drop down item
        groupList.Add(WebConstant.DropDownDefaultText);

        // add the distinct group data to list);
        foreach (GActivitySpecInfoGroupCodeModel model in modelList)
        {
            if (!groupList.Contains(model.groupCode))
            {
                groupList.Add(model.groupCode);
            }
        }

        return ConvertArrayToJson(groupList.ToArray());
    }

    /// <summary>
    /// Get cap type groups
    /// </summary>
    /// <param name="capTypeList">The cap type list</param>
    /// <returns>return cap type groups</returns>
    private string GetCapTypeGroups(CapTypeModel[] capTypeList)
    {
        //clear the both dictionarys
        _dictCapType.Clear();
        _dictRequireDocumentType.Clear();

        Dictionary<string, string> dictCapType = new Dictionary<string, string>();

        // add the default drop down item
        dictCapType.Add(WebConstant.DropDownDefaultText, WebConstant.DropDownDefaultText);

        IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();

        foreach (CapTypeModel model in capTypeList)
        {
            RefDocumentModel[] documentModels = edmsBll.GetAllDocumentTypes(model.serviceProviderCode, model);

            if (documentModels != null &&
                documentModels.Length > 0 &&
                documentModels.Any(item => ValidationUtil.IsYes(item.isRestrictDocType4ACA)))
            {
                string key = CAPHelper.GetCapTypeValue(model);
                string value = ScriptFilter.EncodeJson(CAPHelper.GetAliasOrCapTypeLabel(model));

                if (!dictCapType.ContainsKey(key))
                {
                    dictCapType.Add(key, value);
                    _dictCapType.Add(key, model);
                    _dictRequireDocumentType.Add(key, CapUtil.GetRequiredDocumentTypeList(model));
                }
            }
        }

        return ConvertDictionaryToJson(dictCapType);
    }

    /// <summary>
    /// Get the ASI/ASIT sub group by group code
    /// </summary>
    /// <param name="modelList">the GActivitySpecInfoGroupCodeModel, contain group and sub group</param>
    /// <param name="groupCode">the ASI/ASIT group code</param>
    /// <returns>return the ASI/ASIT sub group</returns>
    private string GetASISubGroups(GActivitySpecInfoGroupCodeModel[] modelList, string groupCode)
    {
        // key: subgroup, value: resSubgroup
        Dictionary<string, string> dict = new Dictionary<string, string>();

        // add the default drop down item
        dict.Add(WebConstant.DropDownDefaultText, WebConstant.DropDownDefaultText);

        if (string.IsNullOrEmpty(groupCode))
        {
            return ConvertDictionaryToJson(dict);
        }

        foreach (GActivitySpecInfoGroupCodeModel model in modelList)
        {
            if (model.groupCode != groupCode)
            {
                continue;
            }

            if (I18nCultureUtil.IsInMasterLanguage)
            {
                if (!dict.ContainsKey(model.subGroup))
                {
                    dict.Add(model.subGroup, model.subGroup);
                }
            }
            else
            {
                bool isCurrentCulture = model.resLangId != null && model.resLangId.Replace("_", "-") == I18nCultureUtil.UserPreferredCulture;
                string resSubgroup = I18nStringUtil.GetString(model.resSubGroup, model.subGroup);

                if (!dict.ContainsKey(model.subGroup) && (model.resLangId == null || isCurrentCulture))
                {
                    dict.Add(model.subGroup, resSubgroup);
                }
                else if (isCurrentCulture)
                {
                    dict[model.subGroup] = resSubgroup;
                }
            }
        }

        return ConvertDictionaryToJson(dict);
    }

    /// <summary>
    /// Make the multiple components(except ASI/ASIT) store unique in page flow.
    /// </summary>
    /// <param name="pageFlowGroup">The page flow group.</param>
    private void MakePageFlowComponentUnique(PageFlowGroupModel pageFlowGroup)
    {
        long[] componentListAllowMultipleTimes =
                                                { 
                                                    (long)PageFlowComponent.APPLICANT, 
                                                    (long)PageFlowComponent.CONTACT_1, 
                                                    (long)PageFlowComponent.CONTACT_2,
                                                    (long)PageFlowComponent.CONTACT_3,
                                                    (long)PageFlowComponent.CONTACT_LIST,
                                                    (long)PageFlowComponent.LICENSED_PROFESSIONAL,
                                                    (long)PageFlowComponent.ATTACHMENT
                                                };

        // Use the CommonUtil.GetRandomUniqueID() to make the component unique in each page flow.
        foreach (StepModel step in pageFlowGroup.stepList)
        {
            foreach (PageModel page in step.pageList)
            {
                foreach (ComponentModel component in page.componentList.Where(component => componentListAllowMultipleTimes.Contains(component.componentID)))
                {
                    component.portletRange1 = string.IsNullOrEmpty(component.portletRange1) ? CommonUtil.GetRandomUniqueID() : component.portletRange1;
                }
            }
        }
    }

    /// <summary>
    /// Convert dictionary string to json format
    /// </summary>
    /// <param name="dict">The dictionary.</param>
    /// <returns>Json format string</returns>
    private string ConvertDictionaryToJson(Dictionary<string, string> dict)
    {
        if (dict == null || dict.Count < 1)
        {
            return "[[]]";
        }

        StringBuilder jsonStr = new StringBuilder();
        jsonStr.Append("[");

        foreach (KeyValuePair<string, string> pair in dict)
        {
            jsonStr.AppendFormat("['{0}','{1}'],", ScriptFilter.EncodeJson(pair.Value), ScriptFilter.EncodeJson(pair.Key));
        }

        jsonStr.Remove(jsonStr.Length - 1, 1);
        jsonStr.Append("]");

        return jsonStr.ToString();
    }

    #endregion Methods
}