/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AdminConfigureService.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 *  
 * 
 *  Notes:
 *      $Id:AdminConfigureService.cs 77905 2009-08-31 12:49:28Z ACHIEVO\Jackie.Yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

using Accela.ACA.AdminBLL;
using Accela.ACA.BLL;
using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.CustomComponent;
using Accela.ACA.BLL.EMSE;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.AppCode.Common;
using Accela.ACA.Web.AuthorizedAgent;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Common;
using Accela.ACA.WSProxy.WSModel;
using Accela.Web.Controls;
using Newtonsoft.Json;

namespace Accela.ACA.Web.WebService
{
    /// <summary>
    /// This is the Admin Configure Web Service.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class AdminConfigureService : System.Web.Services.WebService
    {
        #region Fields

        /// <summary>
        /// The admin user id
        /// </summary>
        private const string ADMIN_AUDIT_ID = "Admin";

        /// <summary>
        /// The default user role for inspection schedule.
        /// </summary>
        private const string DefaultUserRole4InspectionSchedule = "100000";

        /// <summary>
        /// The default user role for inspection input contact.
        /// </summary>
        private const string DefaultUserRole4InspectionInputContact = "100000";

        /// <summary>
        /// The default user role for inspection view contact.
        /// </summary>
        private const string DefaultUserRole4InspectionViewContact = "010000";

        /// <summary>
        /// min row height 
        /// </summary>
        private const int FORM_DESIGNER_MIN_ROW_HEIGHT = 64;

        /// <summary>
        /// Grid unit
        /// </summary>
        private const int FORM_DESIGNER_GRID_UNIT = 16;

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(AdminConfigureService));

        /// <summary>
        /// section fields model list.
        /// </summary>
        private List<SimpleViewModel4WS> _sectionFieldsModelList = new List<SimpleViewModel4WS>();

        /// <summary>
        /// the grid view head width list
        /// </summary>
        private Dictionary<string, SimpleViewModel4WS> _gridViewHeadWidthList;

        /// <summary>
        /// label model list
        /// </summary>
        private List<XUITextModel> _labelModelsList = new List<XUITextModel>();

        /// <summary>
        /// standard choice hard code list
        /// </summary>
        private List<BizDomainModel4WS> _standardChoiceHardCodeList = new List<BizDomainModel4WS>();

        /// <summary>
        /// standard choice list
        /// </summary>
        private List<BizDomainModel4WS> _standardChoiceArrayList = new List<BizDomainModel4WS>();

        /// <summary>
        /// policy standard choice list
        /// </summary>
        private List<XPolicyModel> _policyStandardChoiceList = new List<XPolicyModel>();

        /// <summary>
        /// cap type model list
        /// </summary>
        private List<CapTypeModel> _capTypeModelList = new List<CapTypeModel>();

        /// <summary>
        /// standard bizDomain model list
        /// </summary>
        private List<BizDomainModel4WS> _standardBizDomainModels = new List<BizDomainModel4WS>();

        /// <summary>
        /// XPolicy list
        /// </summary>
        private List<XPolicyModel> _xPolicyList = new List<XPolicyModel>();

        /// <summary>
        /// XPolicy I18N list
        /// </summary>
        private List<XPolicyModel> _xPolicyListI18N = new List<XPolicyModel>();

        /// <summary>
        /// Template fields config list
        /// </summary>
        private List<TemplateLayoutConfigModel> _templateFieldsConfigList = new List<TemplateLayoutConfigModel>();

        /// <summary>
        /// tab order
        /// </summary>
        private string[] _tabOrder;

        /// <summary>
        /// the configuration model
        /// </summary>
        private AdminConfigurationModel4WS _configurationModel;

        /// <summary>
        /// the agency code
        /// </summary>
        private string _servProvCode = ConfigManager.AgencyCode;

        /// <summary>
        /// the module name
        /// </summary>
        private string _moduleName = ConfigManager.AgencyCode;

        /// <summary>
        /// level type
        /// </summary>
        private string _levelType = ACAConstant.LEVEL_TYPE_AGENCY;

        /// <summary>
        /// temporary section id.
        /// </summary>
        private string _tempSectionID = string.Empty;

        /// <summary>
        /// in general search agency dropdownlist selected value.
        /// </summary>
        private string _selectedAgency = ConfigManager.AgencyCode;

        /// <summary>
        /// biz BLL interface
        /// </summary>
        private IBizDomainBll _bizBll = ObjectFactory.GetObject<IBizDomainBll>();

        /// <summary>
        /// admin BLL interface
        /// </summary>
        private IAdminBll _adminBll = ObjectFactory.GetObject<IAdminBll>();

        /// <summary>
        /// cap type BLL interface
        /// </summary>
        private ICapTypeBll _capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();

        /// <summary>
        /// cap type filter BLL interface
        /// </summary>
        private ICapTypeFilterBll _capTypeFilterBll = ObjectFactory.GetObject<ICapTypeFilterBll>();

        /// <summary>
        /// cache manager BLL interface
        /// </summary>
        private ICacheManager _cacheManager = ObjectFactory.GetObject<ICacheManager>();

        /// <summary>
        /// custom component BLL interface
        /// </summary>
        private ICustomComponentBll _customComponentBll = ObjectFactory.GetObject<ICustomComponentBll>();

        /// <summary>
        /// get IXPolicy BLL instance.
        /// </summary>
        private IXPolicyBll _xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();

        #endregion Fields

        #region Public Method

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminConfigureService"/> class.
        /// </summary>
        public AdminConfigureService()
        {
            if (!AppSession.IsAdmin)
            {
                throw new ACAException("unauthenticated web service invoking");
            }
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <returns>The enabled modules.</returns>
        [WebMethod(Description = "get module list", EnableSession = true)]
        public string GetModules()
        {
            Dictionary<string, string> modules = TabUtil.GetAllEnableModules(true);
            StringBuilder result = new StringBuilder("[");

            foreach (var module in modules)
            {
                result.Append("[\"" + module.Key + "\",\"" + module.Value + "\"],");
            }

            if (result.Length > 1)
            {
                result.Length -= 1;
                result.Append("]");
            }

            return result.ToString();
        }

        /// <summary>
        /// Save the authorized agent service setting.
        /// </summary>
        /// <param name="module">The module name.</param>
        /// <param name="capTypeFilter">The cap type filter.</param>
        /// <returns>The authorized agent service setting.</returns>
        [WebMethod(Description = "save the authorized agent service setting", EnableSession = true)]
        public bool SaveFishAndHuntingSetting(string module, string capTypeFilter)
        {
            bool result = true;

            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();

            BizDomainModel4WS[] bizDomains = new BizDomainModel4WS[2];
            bizDomains[0] = new BizDomainModel4WS()
            {
                serviceProviderCode = ConfigManager.AgencyCode,
                bizdomain = BizDomainConstant.STD_AUTHORIZED_SERVICE,
                bizdomainValue = BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_MODULE,
                description = module,
                auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now),
                auditID = ACAConstant.ADMIN_CALLER_ID,
                auditStatus = ACAConstant.VALID_STATUS
            };

            bizDomains[1] = new BizDomainModel4WS()
            {
                serviceProviderCode = ConfigManager.AgencyCode,
                bizdomain = BizDomainConstant.STD_AUTHORIZED_SERVICE,
                bizdomainValue = BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_CAPTYPEFILTER,
                description = capTypeFilter,
                auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now),
                auditID = ACAConstant.ADMIN_CALLER_ID,
                auditStatus = ACAConstant.VALID_STATUS
            };

            //"{\"Module\":\"" + module + "\", \"CapTypeFilter\":\"" + capTypeFilter + "\"}",
            result = bizDomainBll.CreateAndEditBizDomain(bizDomains);

            return result;
        }

        /// <summary>
        /// Gets the authorized agent service setting.
        /// </summary>
        /// <returns>The authorized agent service setting</returns>
        [WebMethod(Description = "get the authorized agent service setting", EnableSession = true)]
        public string GetFishAndHuntingSetting()
        {
            AuthorizedServiceSettingModel model = AuthorizedAgentServiceUtil.GetAuthorizedServiceSetting();

            return new JavaScriptSerializer().Serialize(model);
        }

        /// <summary>
        /// Gets the authentication by security question setting.
        /// </summary>
        /// <returns>authentication by security question setting info.</returns>
        [WebMethod(Description = "get the authentication by security question setting", EnableSession = true)]
        public string GetAuthBySecurityQuestionSetting()
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            AuthBySecurityQuestionModel authBySecurityQuestionModel = SecurityQuestionUtil.GetAuthBySecurityQuestionSetting();

            return javaScriptSerializer.Serialize(authBySecurityQuestionModel);
        }

        /// <summary>
        /// Save admin configuration data.
        /// </summary>
        /// <param name="dataCollection">The data collection.</param>
        /// <returns>Indicating save success or not.</returns>
        [WebMethod(Description = "Per session Hit Counter", EnableSession = true)]
        public bool SaveAdminConfigurationData(object dataCollection)
        {
            bool result = false;
            if (AnalysisConfigurationDataColletionSuccessfully(dataCollection))
            {
                SynchSectionModels();
                UpdateSimpleViewElementsTop();
                _configurationModel = new AdminConfigurationModel4WS();
                _configurationModel.callerId = ACAConstant.ADMIN_CALLER_ID;
                _configurationModel.levelType = _levelType;
                _configurationModel.moduleName = _moduleName;
                _configurationModel.servProvCode = _servProvCode;
                _configurationModel.labelModelArray = _labelModelsList.ToArray();
                _configurationModel.simpleViewModels = _sectionFieldsModelList.ToArray();
                /*
                 * AdminConfigurationModel4WS.standardChoiceArray used to Add or Delete StandardChoice item.
                 * From Contact Management feature in 7.2.0 FP4, Add or Delete Contact Type in ACA is not be supported,
                 *  so remove Contact Type item from the "standardChoiceArray" property.
                 */
                var standardChoiceItems = _standardChoiceArrayList.Where(f =>
                    !BizDomainConstant.STD_CAT_CONTACT_TYPE.Equals(f.bizdomain, StringComparison.OrdinalIgnoreCase)).ToArray();
                _configurationModel.standardChoiceArray = standardChoiceItems; //for ddl item from std choice
                _configurationModel.standardChoice4HardcodeArray = _standardChoiceHardCodeList.ToArray(); //for ddl item form hardcode
                _configurationModel.standardChoiceHardcodeModel = _policyStandardChoiceList.ToArray(); //for ddl items which are module level.
                _configurationModel.tabsOrder = _tabOrder;
                _configurationModel.standardBizDomain = _standardBizDomainModels.ToArray(); //for other standard choice
                _configurationModel.capTypeModel4WS = _capTypeModelList.ToArray();
                _configurationModel.templateLayoutConfigList = _templateFieldsConfigList.ToArray(); //Template fields properties

                IAdminConfigurationSave saver = (IAdminConfigurationSave)ObjectFactory.GetObject(typeof(IAdminConfigurationSave));
                result = saver.SaveAdminConfigurationData(_configurationModel);

                if (_xPolicyList != null && _xPolicyList.Count > 0)
                {
                    if (_xPolicyListI18N != null && _xPolicyListI18N.Count > 0)
                    {
                        _xPolicyBll.CreateOrUpdatePolicy(_xPolicyListI18N.ToArray(), _xPolicyList.ToArray());  // xpolicy support multi language.
                    }
                    else
                    {
                        _xPolicyBll.CreateOrUpdatePolicy(null, _xPolicyList.ToArray());  // Contact type xpolicy ignore multi language.
                    }

                    XPolicyModel[] contactPolicyModels = _xPolicyList.Where(p => XPolicyConstant.CONTACT_TYPE_RESTRICTION_BY_MODULE.Equals(p.policyName, StringComparison.OrdinalIgnoreCase) && ACAConstant.COMMON_N.Equals(p.rightGranted, StringComparison.OrdinalIgnoreCase)).ToArray();

                    if (contactPolicyModels != null && contactPolicyModels.Count() != 0)
                    {
                        _xPolicyBll.RemoveXPoliciesForContactTypeSecurity(contactPolicyModels.ToArray());
                    }
                }

                if (result)
                {
                    bool removeChoice = _configurationModel.standardChoiceArray.Length > 0 || _configurationModel.standardBizDomain.Length > 0 || _configurationModel.standardChoice4HardcodeArray.Length > 0;
                    UpdateLabelCache(_labelModelsList, _servProvCode, _levelType, _moduleName, removeChoice);

                    if (_sectionFieldsModelList.Count > 0)
                    {
                        string cacheKey = ConfigManager.AgencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_GVIEW_ELEMENT;
                        _cacheManager.Remove(cacheKey);
                    }

                    if (_templateFieldsConfigList.Count > 0)
                    {
                        string cacheKey = ConfigManager.AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_TEMPLATE);
                        _cacheManager.Remove(cacheKey);
                        _templateFieldsConfigList.Clear();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Saves the customize component configuration.
        /// </summary>
        /// <param name="dataCustomComponent">The customize component data.</param>
        /// <returns>Return [true] if save successful.</returns>
        [WebMethod(Description = "Save customize component for page", EnableSession = true)]
        public bool SaveCustomComponentConfig(object dataCustomComponent)
        {
            object[] arrCustomComponent = (object[])dataCustomComponent;

            if (arrCustomComponent == null || arrCustomComponent.Length == 0)
            {
                return false;
            }

            // convert the customize component data to CustomComponentModel list.
            List<CustomComponentModel> list = new List<CustomComponentModel>();

            foreach (object element in arrCustomComponent)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)element;

                CustomComponentModel model = new CustomComponentModel();
                model.serviceProviderCode = ConfigManager.AgencyCode;
                model.displayOrder = 0;
                model.auditModel = new SimpleAuditModel();
                model.auditModel.auditStatus = ACAConstant.VALID_STATUS;
                model.auditModel.auditID = ACAConstant.ADMIN_CALLER_ID;

                // set the value to CustomComponentModel
                foreach (KeyValuePair<string, object> kvp in dict)
                {
                    switch (kvp.Key)
                    {
                        case "ResID":
                            if (kvp.Value != null && long.Parse(kvp.Value.ToString()) != 0)
                            {
                                model.resID = long.Parse(kvp.Value.ToString());
                            }

                            break;
                        case "ElementID":
                            if (kvp.Value != null && long.Parse(kvp.Value.ToString()) != 0)
                            {
                                model.elementID = long.Parse(kvp.Value.ToString());
                            }

                            break;
                        case "ComponentName":
                            model.componentName = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "Visible":
                            model.visible = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "Path":
                            model.path = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        default:
                            break;
                    }
                }

                list.Add(model);
            }

            // do save operation
            if (list.Count > 0)
            {
                _customComponentBll.SaveComponentConfig(list.ToArray());
            }

            return true;
        }

        /// <summary>
        /// Get a json string that contains available CAP types and selected CAP types.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="buttonName">button name - CreateAmendment, DeleteDocument. etc...</param>
        /// <returns>json string for CAP type list</returns>
        [WebMethod(Description = "GetCapTypeListByModule", EnableSession = true)]
        public string GetCapTypeListByModule(string moduleName, string buttonName)
        {
            ButtonSettingModel4WS buttonSettingModel = new ButtonSettingModel4WS();
            buttonSettingModel.buttonName = buttonName;
            buttonSettingModel.moduleName = moduleName;
            buttonSettingModel.serviceProviderCode = ConfigManager.AgencyCode;

            IAdminCapTypePermissionBll capTypePermissionBll = (IAdminCapTypePermissionBll)ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll));
            ButtonSettingModel4WS returnButtonSettingModule = capTypePermissionBll.GetButtonSetting4CapType(buttonSettingModel, ADMIN_AUDIT_ID);

            return CreateAmendmentTreeNodeListJson(returnButtonSettingModule);
        }

        #region New page save

        /// <summary>
        /// Registration data.
        /// </summary>
        /// <returns>The registration data info.</returns>
        [WebMethod(Description = "Get Registration Data Info", EnableSession = true)]
        public string GetRegistrationDataInfo()
        {
            StringBuilder registrationSettings = new StringBuilder();

            registrationSettings.Append("{");
            registrationSettings.Append(GetRecaptChaDataInfo());
            registrationSettings.Append(GetIntervalDataInfo());
            registrationSettings.Append(GetPasswordDataInfo());
            registrationSettings.Append(GetConfigureLicenseDataInfo());
            registrationSettings.Append(GetAccountVerificationMailSetting());
            registrationSettings.Append(GetEnableResetPasswordOnCombineDataInfo());
            registrationSettings.Append(GetEnableLoginOnRegistrationDataInfo());
            registrationSettings.Append("}");

            return registrationSettings.ToString();
        }

        /// <summary>
        /// This method is to save global setting page data.
        /// </summary>
        /// <param name="dict">The global setting.</param>
        /// <returns>Indicating save success or not.</returns>
        [WebMethod(Description = "Save Global Setting", EnableSession = true)]
        public bool SaveGlobalSettingInfo(object dict)
        {
            try
            {
                Dictionary<string, object> dictList = (Dictionary<string, object>)dict;
                IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

                if (dictList != null)
                {
                    foreach (KeyValuePair<string, object> kvp in dictList)
                    {
                        object[] value = (object[])kvp.Value;

                        if (value == null ||
                            value.Length <= 0)
                        {
                            continue;
                        }

                        //update emse script info.
                        UpdateEventScriptCode(value);
                        UpdateCssContent(value);

                        List<BizDomainModel4WS> bizDomainParams = new List<BizDomainModel4WS>();
                        bizDomainParams.Add(GetGisModel(value));
                        bizDomainParams.Add(GetOfficialWebSiteModel(value));
                        bizDomainParams.Add(GetCountryCodeModel(value));
                        bizDomainParams.Add(GetExportModel(value));
                        bizDomainParams.Add(GetUserInitialDisplayModel(value));
                        bizDomainParams.Add(GetPayFeeLinkStatusModel(value));

                        // check if support decimal for fee item
                        bizDomainParams.Add(GetBizDomainModel4WSModel(value, "chkDecimalFeeItem", BizDomainConstant.STD_ITEM_SUPPORT_DECIMAL_QUANTITY));

                        _bizBll.CreateAndEditBizDomain(bizDomainParams.ToArray(), true);

                        //save or update xpolicy table.
                        policyBll.CreateOrUpdatePolicy(null, BuildXPolicyModelList(value));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                return false;
            }
        }

        /// <summary>
        /// This method is to save registration setting page data.
        /// </summary>
        /// <param name="dict">The registration setting.</param>
        /// <returns>Indicating save success or not.</returns>
        [WebMethod(Description = "Save Registration Setting", EnableSession = true)]
        public bool SaveRegistrationSettingInfo(object dict)
        {
            try
            {
                Dictionary<string, object> dictList = (Dictionary<string, object>)dict;

                if (dictList != null)
                {
                    foreach (KeyValuePair<string, object> kvp in dictList)
                    {
                        object[] value = (object[])kvp.Value;

                        if (value == null ||
                            value.Length <= 0)
                        {
                            continue;
                        }

                        List<BizDomainModel4WS> bizDomainList = new List<BizDomainModel4WS>();
                        bizDomainList.Add(GetRequireLicenseBizModel(value));
                        bizDomainList.Add(GetIntervalModel(value));
                        bizDomainList.Add(GetAccountVerificationMailModel(value));
                        bizDomainList.AddRange(GetAuthBySecurityQuestionSettingModels(value));

                        _bizBll.CreateAndEditBizDomain(bizDomainList.ToArray(), true);

                        //save or update xpolicy table.
                        _xPolicyBll.CreateOrUpdatePolicy(null, GetRegistrationSettingXPolicys(value));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                return false;
            }
        }

        /// <summary>
        /// This method is to get GIS information.
        /// </summary>
        /// <returns>The GIS data info.</returns>
        [WebMethod(Description = "Get Gis Data", EnableSession = true)]
        public ArrayList GetGisDataInfo()
        {
            ArrayList array = new ArrayList();

            // item 1: tempalte type.
            var isEnableNewTemplate = ValidationUtil.IsTrue(_xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_NEW_TEMPLATE));
            array.Add(isEnableNewTemplate);

            // item 2&3: GIS activate and GIS server URL of classic.
            CreateBizModel4GisData(BizDomainConstant.STD_ITEM_GIS_PORLET_URL, array);

            // item 4&5: GIS activate and GIS server URL of new.
            CreateBizModel4GisData(BizDomainConstant.STD_ITEM_NEW_GIS_PORLET_URL, array);

            // item 6: global features's header.
            StringBuilder globalFeaturesHead = new StringBuilder();
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("acaadmin_globalsetting_label_expose_contact_from_other_agencies_head"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("aca_label_manual_contact_association_head"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("aca_label_auto_activate_new_association_head"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("aca_label_contact_address_maintenance_head"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("acaadmin_global_setting_label_account_attachment_description"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("acaadmin_global_setting_label_enable_account_edu_exam_ce_desc"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append("<b>").Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_label_gis_head")).Append("</b> ");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_label_gis_content"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append("<b>").Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_label_doctypefilter_head")).Append("</b> ");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_label_doctypefilter_content"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_label_countrycode_content"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_label_fein_content"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("admin_global_accessibility_content"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("acaadmin_licensestate_msg_content"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append("<b>" + LabelUtil.GetAdminUITextByKey("admin_global_setting_label_announcementsettiong_head") + "</b> " + LabelUtil.GetAdminUITextByKey("admin_global_setting_label_announcementsettiong_msg"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("acaadmin_global_setting_label_enable_contactaddress_edit_description"));
            globalFeaturesHead.Append("<br/>");
            globalFeaturesHead.Append(LabelUtil.GetAdminUITextByKey("acaadmin_globalsetting_label_enablecontactaddressdeactivatedescription"));
            array.Add(globalFeaturesHead.ToString());
            
            // item 7: enable document type filter.
            IXPolicyBll xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string enableDocType = xPolicyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_DOCUMENT_TYPE_FILTER);

            if (string.IsNullOrEmpty(enableDocType))
            {
                enableDocType = ACAConstant.COMMON_N;
            }

            array.Add(enableDocType);

            return array;
        }

        /// <summary>
        /// This method is to get country code information.
        /// get bizDomain value from web service not from cache,
        /// because when you disable the bizDomain the method GetBizDomainListByModel can also get the value. 
        /// </summary>
        /// <returns>The country code data info.</returns>
        [WebMethod(Description = "Get Country Code", EnableSession = true)]
        public ArrayList GetCountryCodeDataInfo()
        {
            string[] bizModelValue;
            ArrayList array = new ArrayList();
            BizDomainModel4WS bizModel;

            bizModelValue = _bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_PHONE_NUMBER_IDD_ENABLE);
            bizModel = GetCountryCodeBizDomainModel();
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            if (bizModelValue != null
                && bizModel != null
                && bizModel.auditStatus == ACAConstant.VALID_STATUS)
            {
                array.Add(ValidationUtil.IsYes(bizModel.bizdomainValue) ? ACAConstant.COMMON_YES : ACAConstant.COMMON_NO);
            }
            else
            {
                array.Add(string.Empty);
            }

            return array;
        }

        /// <summary>
        /// This method is to get Fein Masking setting.
        /// get bizDomain value from web service not from cache,
        /// because when you disable the bizDomain the method GetBizDomainListByModel can also get the value. 
        /// </summary>
        /// <returns>The fein masking data info.</returns>
        [WebMethod(Description = "Get Fein Masking", EnableSession = true)]
        public string GetFeinMaskingDataInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string feinMaskingInfo = xPolicyBll.GetValueByKey(XPolicyConstant.ITEM_ENABLE_FEIN_MASKING);

            if (string.IsNullOrEmpty(feinMaskingInfo))
            {
                feinMaskingInfo = ACAConstant.COMMON_N;
            }

            return feinMaskingInfo;
        }

        /// <summary>
        /// Get accessibility Information
        /// </summary>
        /// <returns>The accessibility information.</returns>
        [WebMethod(Description = "Get accessibility Information", EnableSession = true)]
        public string GetAccessibilityDataInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string accessibility = string.Empty;

            //0.enable range search.
            accessibility = xPolicyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_ACCESSIBILITY);

            if (string.IsNullOrEmpty(accessibility))
            {
                accessibility = ACAConstant.COMMON_N;
            }

            return accessibility;
        }

        /// <summary>
        /// Get attachment Information
        /// </summary>
        /// <returns>The account attachment information.</returns>
        [WebMethod(Description = "Get account attachment Information", EnableSession = true)]
        public string GetAccountAttachmentDataInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            //0.enable range search.
            string accountAttachmentActive = xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_ACCOUNT_ATTACHMENT);

            if (string.IsNullOrEmpty(accountAttachmentActive))
            {
                accountAttachmentActive = ACAConstant.COMMON_N;
            }

            return accountAttachmentActive;
        }

        /// <summary>
        /// Get edit contact address Information.
        /// </summary>
        /// <returns>The edit contact address information.</returns>
        [WebMethod(Description = "Get edit contact address Information", EnableSession = true)]
        public string GetEnableContactAddressEditDataInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            //0.enable range search.
            string isEnableContactAddressEdit = xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_CONTACT_ADDRESS_EDIT);

            if (string.IsNullOrEmpty(isEnableContactAddressEdit))
            {
                isEnableContactAddressEdit = ACAConstant.COMMON_Y;
            }

            return isEnableContactAddressEdit;
        }

        /// <summary>
        /// Get XPolicy value by name
        /// </summary>
        /// <param name="policyName">The policy name</param>
        /// <returns>The XPolicy information.</returns>
        [WebMethod(Description = "Get XPolicy Information", EnableSession = true)]
        public string GetXPolicyValueByName(string policyName)
        {
            IXPolicyBll xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string xPolicyValue = xPolicyBll.GetValueByKey(policyName);

            return !string.IsNullOrEmpty(xPolicyValue) ? xPolicyValue : string.Empty;
        }

        /// <summary>
        /// Get license state information.
        /// </summary>
        /// <returns>The license state information.</returns>
        [WebMethod(Description = "Get License State Information", EnableSession = true)]
        public string GetLicenseStateInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string licenseState = string.Empty;

            licenseState = xPolicyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_LICENSESTATE);

            if (string.IsNullOrEmpty(licenseState))
            {
                licenseState = ACAConstant.COMMON_N;
            }

            return licenseState;
        }

        /// <summary>
        /// This method is to get official web site information.
        /// </summary>
        /// <returns>The official web site.</returns>
        [WebMethod(Description = "Get Official Web Site", EnableSession = true)]
        public string GetOfficialWebSiteInfo()
        {
            BizDomainModel4WS bizModel;

            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_OFFICIAL_WEBSITE_URL;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            if (bizModel != null &&
                bizModel.auditStatus == ACAConstant.VALID_STATUS)
            {
                // show the value when only the biz domain status is active.
                if (!string.IsNullOrEmpty(bizModel.description))
                {
                    return bizModel.description;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// This method is to get the users' initials display information.
        /// </summary>
        /// <returns>The users' initials display information.</returns>
        [WebMethod(Description = "Get Users' Initials Display Info", EnableSession = true)]
        public string GetUserInitialDisplayInfo()
        {
            BizDomainModel4WS bizModel;

            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_DISPLAY_USER_INITIALS;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            if (bizModel != null &&
                bizModel.auditStatus == ACAConstant.VALID_STATUS)
            {
                if (ValidationUtil.IsYes(bizModel.description))
                {
                    return ACAConstant.COMMON_Y;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// This method is to get the CheckBox Option in CAP list for Anonymous Users.
        /// get bizDomain value from web service not from cache,
        /// because when you disable the bizDomain the method GetBizDomainListByModel can also get the value. 
        /// </summary>
        /// <returns>The CheckBox option in cap list for anonymous users.</returns>
        [WebMethod(Description = "Get the CheckBox Option in CAP List for Anonymous Users", EnableSession = true)]
        public string GetCheckBoxOptionInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = string.Empty;

            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_CHECKBOX_ANONYMOUSUSER_VISIBLE);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                xPolicyValue = ACAConstant.COMMON_N;
            }

            return xPolicyValue;
        }

        /// <summary>
        /// This method is to get the CheckBox Option in CAP for "Cross Module Search"        
        /// </summary>
        /// <returns>The  CheckBox Option in CAP for "Cross Module Search".</returns>
        [WebMethod(Description = "This method is to get the CheckBox Option in CAP for 'Cross Module Search'", EnableSession = true)]
        public string GetSwitchValueOfCMSearch()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = string.Empty;

            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_ENABLE_CROSS_MODULE_SEARCH);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                xPolicyValue = ACAConstant.COMMON_N;
            }

            return xPolicyValue;
        }

        /// <summary>
        /// This method is to get the CheckBox Option in CAP for "Only Search My License"        
        /// </summary>
        /// <returns>The CheckBox Option in CAP for "Only Search My License".</returns>
        [WebMethod(EnableSession = true)]
        public string GetSearchMyLicense()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_ENABLE_ONLY_SEARCH_MY_LICENSE);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                xPolicyValue = ACAConstant.COMMON_N;
            }

            return xPolicyValue;
        }

        /// <summary>
        /// This method is to get Export information from standard choice ACA_Config.
        /// </summary>
        /// <returns>The export information from standard choice.</returns>
        [WebMethod(Description = "Get Export Data", EnableSession = true)]
        public ArrayList GetExportDataInfo()
        {
            ArrayList array = new ArrayList();
            StringBuilder content = new StringBuilder();

            BizDomainModel4WS bizModel;

            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_ALLOW_EXPORTING_TO_CSV;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);
            if (bizModel != null)
            {
                array.Add(bizModel.auditStatus == null ? "I" : bizModel.auditStatus);
                array.Add(bizModel.description);
            }
            else
            {
                array.Add("I");
                array.Add(string.Empty);
            }

            content.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_globalsearch_settingsdescription"));
            content.Append("<br/>");
            content.Append(LabelUtil.GetAdminUITextByKey("acaadmin_globalsetting_label_peoplesearch_desc"));
            content.Append("<br/>");
            content.Append(LabelUtil.GetAdminUITextByKey("aca_enable_cross_module_search_prompt_message"));
            content.Append("<br/>");
            content.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_search_license_tip"));
            content.Append("<br/>");
            content.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_label_export_content"));
            array.Add(content.ToString());

            return array;
        }

        /// <summary>
        /// This method is to get disable/enable pay fee link indication from standard choice ACA_Config.
        /// </summary>
        /// <returns>return Array List to client</returns>
        [WebMethod(Description = "Get disable/enable pay fee link indication", EnableSession = true)]
        public ArrayList GetPayFeeLinkStatus()
        {
            ArrayList array = new ArrayList();

            BizDomainModel4WS bizModel;

            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_REMOVE_PAY_FEE;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            if (bizModel != null)
            {
                array.Add(bizModel.auditStatus ?? ACAConstant.INVALID_STATUS);
                array.Add(ValidationUtil.IsYes(bizModel.description));
            }
            else
            {
                //Check the checkbox by default.
                array.Add(ACAConstant.VALID_STATUS);
                array.Add(ACAConstant.COMMON_NO);
            }

            return array;
        }

        /// <summary>
        /// Gets the reference people search settings.
        /// </summary>
        /// <param name="peopleType">'Contact' or 'LP'</param>
        /// <returns>JSON string.</returns>
        [WebMethod(EnableSession = true)]
        public string GetPeopleSearchSettings(string peopleType)
        {
            IXEntityPermissionBll xEntityPermissionBll = (IXEntityPermissionBll)ObjectFactory.GetObject(typeof(IXEntityPermissionBll));
            IBizDomainBll bizDomainBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            var xEntity = new XEntityPermissionModel();
            xEntity.servProvCode = ConfigManager.AgencyCode;
            string bizName = string.Empty;
            bool defaultStatus = false;

            switch (peopleType)
            {
                case "Contact":
                    //Gets contact types.
                    bizName = BizDomainConstant.STD_CAT_CONTACT_TYPE;
                    xEntity.entityType = XEntityPermissionConstant.REFERENCE_CONTACT_SEARCH;

                    //Reference searchable of all Contact types is disabled by default.
                    defaultStatus = false;
                    break;
                case "LP":
                    //Gets licensed professional types.
                    bizName = BizDomainConstant.STD_CAT_LICENSE_TYPE;
                    xEntity.entityType = XEntityPermissionConstant.REFERENCE_LICENSED_PROFESSIONAL_SEARCH;

                    //Reference searchable of all LP types is enabled by default.
                    defaultStatus = true;
                    break;
            }

            //Gets people search settings by XEntityPermission.
            var xEntities = xEntityPermissionBll.GetXEntityPermissions(xEntity);
            var dictXEntities = xEntities == null ? new Dictionary<string, string>() : xEntities.ToDictionary(e => e.entityId3, e => e.permissionValue);
            var peopleTypes = peopleType == "Contact" ? bizDomainBll.GetContactTypeList(ConfigManager.AgencyCode, false, ContactTypeSource.Reference) : bizDomainBll.GetBizDomainList(ConfigManager.AgencyCode, bizName, false);
            StringBuilder result = new StringBuilder("[");

            //Build json string and return to browser.
            foreach (var itm in peopleTypes)
            {
                //Permission settings.
                bool isChecked = false;

                if (!dictXEntities.ContainsKey(itm.Key))
                {
                    isChecked = defaultStatus;
                }
                else
                {
                    if (defaultStatus)
                    {
                        //If default status is true, Not 'N' means it's enabled.
                        isChecked = !ValidationUtil.IsNo(dictXEntities[itm.Key]);
                    }
                    else
                    {
                        //If default status is false, Is 'Y' means it's enabled.
                        isChecked = ValidationUtil.IsYes(dictXEntities[itm.Key]);
                    }
                }

                result.Append("{");
                result.Append("Key:'").Append(ScriptFilter.EncodeJson(itm.Key));
                result.Append("',Text:'").Append(ScriptFilter.EncodeJson(itm.Value.ToString()));
                result.Append("',Checked:").Append(isChecked ? "true" : "false");
                result.Append("},");
            }

            if (result.Length > 1)
            {
                //Remove last comma char.
                result.Remove(result.Length - 1, 1);
            }

            result.Append("]");

            return result.ToString();
        }

        /// <summary>
        /// Get Decimal Flag for fee item
        /// </summary>
        /// <returns>true: Y or false: N</returns>
        [WebMethod(Description = "Get yes/no value for decimal fee item", EnableSession = true)]
        public string GetDecimalFeeItemResult()
        {
            BizDomainModel4WS bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_SUPPORT_DECIMAL_QUANTITY;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            // set default value to Y
            string result = "Y";

            if (bizModel != null)
            {
                string auditStatus = bizModel.auditStatus == null ? ACAConstant.INVALID_STATUS : bizModel.auditStatus;

                if (ACAConstant.VALID_STATUS == auditStatus)
                {
                    result = ValidationUtil.IsYes(bizModel.description) ? "Y" : "N";
                }
            }

            return result;
        }

        /// <summary>
        /// This method is to get module setup label.
        /// </summary>
        /// <returns>The module setup label.</returns>
        [WebMethod(Description = "Get Gis Data", EnableSession = true)]
        public ArrayList GetModuleSetupLabelKeyInfo()
        {
            string head = string.Empty;
            string content = string.Empty;
            ArrayList array = new ArrayList();

            head = LabelUtil.GetAdminUITextByKey("admin_module_setup_label_module_setup_head");
            content = LabelUtil.GetAdminUITextByKey("admin_module_setup_label_module_setup_content");
            array.Add("<b>" + head + "</b> " + content);

            return array;
        }

        /// <summary>
        /// This method is to save module setup page data.
        /// </summary>
        /// <param name="dict">module setting.</param>
        /// <returns>Indicating save success or not.</returns>
        [WebMethod(Description = "Save Module Setting", EnableSession = true)]
        public bool SaveModuleSetupInfo(object dict)
        {
            try
            {
                Dictionary<string, object> dictList = (Dictionary<string, object>)dict;

                if (dictList != null)
                {
                    foreach (KeyValuePair<string, object> kvp in dictList)
                    {
                        if (kvp.Value is object[])
                        {
                            _adminBll.EditSubTreeNode(ConfigManager.AgencyCode, GetModuleArray((object[])kvp.Value));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                return false;
            }
        }

        /// <summary>
        /// This method is to get inspection SD.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>array list</returns>
        [WebMethod(Description = "Get Inspection SD", EnableSession = true)]
        public Dictionary<string, object> GetInspectionSDInfo(string moduleName)
        {
            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
            Dictionary<string, object> values = new Dictionary<string, object>();

            BizDomainModel4WS bizModel;

            // Display map for show object
            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = moduleName + "_" + BizDomainConstant.STD_DISPLAY_MAP_FOR_SHOWOBJECT;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, bizModel.bizdomainValue, ADMIN_AUDIT_ID);

            if (bizModel != null)
            {
                values.Add(BizDomainConstant.STD_DISPLAY_MAP_FOR_SHOWOBJECT, bizModel.auditStatus == ACAConstant.VALID_STATUS && !ValidationUtil.IsNo(bizModel.description));
            }

            // Display map for select object
            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = moduleName + "_" + BizDomainConstant.STD_DISPLAY_MAP_FOR_SELECTOBJECT;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, bizModel.bizdomainValue, ADMIN_AUDIT_ID);

            if (bizModel != null)
            {
                values.Add(BizDomainConstant.STD_DISPLAY_MAP_FOR_SELECTOBJECT, bizModel.auditStatus == ACAConstant.VALID_STATUS && !ValidationUtil.IsNo(bizModel.description));
            }

            //Add user type role into arraylist.
            IXPolicyWrapper policyWrapper = (IXPolicyWrapper)ObjectFactory.GetObject(typeof(IXPolicyWrapper));
            XpolicyUserRolePrivilegeModel policy = policyWrapper.GetPolicy(ConfigManager.AgencyCode, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_SCHEDULE, ACAConstant.LEVEL_TYPE_MODULE, moduleName);
            if (policy != null)
            {
                if (policy.userRolePrivilegeModel != null)
                {
                    values.Add(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_SCHEDULE, userRoleBll.ConvertToUserRoleString(policy.userRolePrivilegeModel));
                }
                else
                {
                    values.Add(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_SCHEDULE, DefaultUserRole4InspectionSchedule);
                }
            }
            else
            {
                values.Add(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_SCHEDULE, DefaultUserRole4InspectionSchedule);
            }

            //Add inspection input contact user type role into arraylist.
            string userRoleInputContact = GetInspectionContactUserRolePermission(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT, moduleName);
            if (!string.IsNullOrEmpty(userRoleInputContact))
            {
                values.Add(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT, userRoleInputContact);
            }
            else
            {
                values.Add(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT, DefaultUserRole4InspectionInputContact);
            }

            //Add inspection view contact user type role into arraylist.
            string userRoleViewContact = GetInspectionContactUserRolePermission(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT, moduleName);
            if (!string.IsNullOrEmpty(userRoleViewContact))
            {
                values.Add(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT, userRoleViewContact);
            }
            else
            {
                values.Add(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT, DefaultUserRole4InspectionViewContact);
            }

            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = moduleName + "_" + BizDomainConstant.STD_INSPECTION_DISPLAYOPTION;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, moduleName + "_" + BizDomainConstant.STD_INSPECTION_DISPLAYOPTION, ADMIN_AUDIT_ID);
            if (bizModel != null)
            {
                values.Add(BizDomainConstant.STD_INSPECTION_DISPLAYOPTION, ValidationUtil.IsYes(bizModel.description));
            }
            else
            {
                values.Add(BizDomainConstant.STD_INSPECTION_DISPLAYOPTION, true);
            }

            XPolicyModel[] xPolicies = _xPolicyBll.GetXPolicyByLevel(BizDomainConstant.STD_CAT_ACA_CONFIGS, ACAConstant.LEVEL_TYPE_MODULE, moduleName, ACAConstant.ADMIN_CALLER_ID);
            StringBuilder selectedModuleName = new StringBuilder();
            if (xPolicies != null && xPolicies.Length > 0)
            {
                foreach (XPolicyModel item in xPolicies)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    switch (item.data1)
                    {
                        case ACAConstant.ACA_ENABLE_SEARCH_CROSS_MODULE:
                            //Fill cross module search.
                            selectedModuleName.Append(item.data4);
                            selectedModuleName.Append(ACAConstant.SPLIT_CHAR2);
                            selectedModuleName.Append(item.data2);
                            selectedModuleName.Append(ACAConstant.SPLIT_CHAR);
                            break;
                        case BizDomainConstant.STD_MULTIPLE_INSPECTIONS_ENABLED:
                            //Add allow multiple inspection indication into arraylist.
                            values.Add(BizDomainConstant.STD_MULTIPLE_INSPECTIONS_ENABLED, ValidationUtil.IsYes(item.data2));
                            break;
                        case XPolicyConstant.PAY_FEE_LINK_DISP:
                            // Allow display pay fee link in module level
                            values.Add(XPolicyConstant.PAY_FEE_LINK_DISP, ValidationUtil.IsTrue(item.data2));
                            break;
                        case XPolicyConstant.ENABLE_CLONE_RECORD:
                            // Allow clone record in module level
                            values.Add(XPolicyConstant.ENABLE_CLONE_RECORD, ValidationUtil.IsTrue(item.data2));
                            break;
                        case XPolicyConstant.ENABLE_SEARCHASI_ADDITIONALCRITERIA:
                            // Enable Search by ASI addition information in module level
                            values.Add(XPolicyConstant.ENABLE_SEARCHASI_ADDITIONALCRITERIA, string.IsNullOrEmpty(item.data2) || ValidationUtil.IsTrue(item.data2));
                            break;
                        case XPolicyConstant.ENABLE_SEARCHCONTACT_ADDITIONALCRITERIA:
                            // Enable Search by contact template addition information in module level
                            values.Add(XPolicyConstant.ENABLE_SEARCHCONTACT_ADDITIONALCRITERIA, string.IsNullOrEmpty(item.data2) || ValidationUtil.IsTrue(item.data2));
                            break;
                        case XPolicyConstant.ENABLE_BOARD_TYPE_SELECTION:
                            // (Feature:09ACC-08040_Board_Type_Selection) the flag indicating  whether Board Type Selection mode
                            // is enabled for the specified module
                            values.Add(XPolicyConstant.ENABLE_BOARD_TYPE_SELECTION, ValidationUtil.IsYes(item.data2));
                            break;
                        case XPolicyConstant.INSPECITON_DISPLAY_DEFAULT_CONTACT:
                            values.Add(XPolicyConstant.INSPECITON_DISPLAY_DEFAULT_CONTACT, ValidationUtil.IsYes(item.data2));
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(selectedModuleName.ToString()))
            {
                values.Add(ACAConstant.ACA_ENABLE_SEARCH_CROSS_MODULE, selectedModuleName.ToString().Substring(0, selectedModuleName.Length - 1));
            }
            else
            {
                values.Add(ACAConstant.ACA_ENABLE_SEARCH_CROSS_MODULE, string.Empty);
            }

            return values;
        }

        /// <summary>
        /// This method is to get inspection label key.
        /// </summary>
        /// <returns>array list</returns>
        [WebMethod(Description = "Get Inspection LabelKey", EnableSession = true)]
        public ArrayList GetInspectionLabelKeyInfo()
        {
            ArrayList array = new ArrayList();
            string head = string.Empty;
            string content = string.Empty;

            content = LabelUtil.GetAdminUITextByKey("admin_module_setting_label_map_content");
            array.Add(content);

            head = LabelUtil.GetAdminUITextByKey("admin_inspection_setting_label_schedule_content_head");
            content = LabelUtil.GetAdminUITextByKey("admin_global_setting_label_cap_filter_content");
            array.Add("<b>" + head + "</b> " + content);

            head = LabelUtil.GetAdminUITextByKey("admin_inspection_setting_label_display_content_head");
            content = LabelUtil.GetAdminUITextByKey("admin_inspection_setting_label_display_content");
            array.Add("<b>" + head + "</b> " + content);

            head = LabelUtil.GetAdminUITextByKey("admin_inspection_setting_label_application_content_head");
            content = LabelUtil.GetAdminUITextByKey("admin_inspection_setting_label_application_content");
            array.Add("<b>" + head + "</b> " + content);

            content = LabelUtil.GetAdminUITextByKey("aca_admin_inspection_allow_multiple_content");
            array.Add(content);

            head = LabelUtil.GetAdminUITextByKey("education_section_setting_instruction_label");
            content = LabelUtil.GetAdminUITextByKey("education_section_setting_instruction");
            array.Add("<b>" + head + "</b> " + content);

            content = LabelUtil.GetAdminUITextByKey("admin_inspection_setting_label_instruct_inputcontact");
            array.Add(content);

            content = LabelUtil.GetAdminUITextByKey("admin_inspection_setting_label_instruct_viewcontact");
            array.Add(content);

            //social media settings header:
            content = "<b>" + LabelUtil.GetAdminUITextByKey("acaadmin_modulesettings_label_sharebutton") + "</b> "
                + LabelUtil.GetAdminUITextByKey("acaadmin_modulesettings_label_sharebuttoninstruction")
                + "<br/><b>" + LabelUtil.GetAdminUITextByKey("acaadmin_modulesettings_label_sharedcomments") + "</b> "
                + LabelUtil.GetAdminUITextByKey("acaadmin_modulesettings_label_sharedcommentsinstruction");
            array.Add(content);

            content = LabelUtil.GetAdminUITextByKey("acaadmin_modulesetting_inspection_label_displaydefaultcontact");
            array.Add(content);

            return array;
        }

        /// <summary>
        /// Get display email information.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The display email information.</returns>
        [WebMethod(Description = "Get display email adddress option", EnableSession = true)]
        public string GetDisplayEmailDataInfo(string moduleName)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            return policyBll.GetValueByKey(ACAConstant.ACA_ENABLE_WF_DISP_EMAIL, moduleName);
        }

        /// <summary>
        /// Get the shopping cart information.
        /// </summary>
        /// <returns>The shopping cart information.</returns>
        [WebMethod(Description = "Get shopping Cart Information", EnableSession = true)]
        public ArrayList GetShoppingCartDataInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = string.Empty;
            ArrayList shoppingCartArray = new ArrayList();
            StringBuilder content = new StringBuilder();

            //0.Description about Fees and Checkout.
            content.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_enable_payfee_content"));
            content.Append("<br/>");
            content.Append(LabelUtil.GetAdminUITextByKey("admin_inspection_setting_label_EMSE_disclaimer"));
            content.Append("<br/>");
            content.Append(LabelUtil.GetAdminUITextByKey("admin_global_setting_shoppingcart_content"));
            shoppingCartArray.Add(content.ToString());

            //1.enable shopping cat.
            xPolicyValue = xPolicyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_SHOPPING_CART);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                shoppingCartArray.Add(ACAConstant.COMMON_N);
            }
            else
            {
                shoppingCartArray.Add(xPolicyValue);
            }

            //2.transaction type.
            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_SHOPPING_CART_PAYMENT_TRANSACTION_SETTING);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                shoppingCartArray.Add(ACAConstant.SHOPPINGCART_TRANSACTION_DEFAULT_VALUE);
            }
            else
            {
                shoppingCartArray.Add(xPolicyValue);
            }

            //3.Set expiration days for Selected Items
            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_SHOPPING_CART_EXPIRATION_DAY_OF_SELECTED_ITEMS);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                shoppingCartArray.Add(ACAConstant.SHOPPINGCART_EXPIRATION_DEFAULT_DAY);
            }
            else
            {
                shoppingCartArray.Add(xPolicyValue);
            }

            //4.Set expiration days for Saved Items
            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_SHOPPING_CART_EXPIRATION_DAY_OF_SAVED_ITEMS);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                shoppingCartArray.Add(ACAConstant.SHOPPINGCART_EXPIRATION_DEFAULT_DAY);
            }
            else
            {
                shoppingCartArray.Add(xPolicyValue);
            }

            return shoppingCartArray;
        }

        /// <summary>
        /// Get the proxy user information.
        /// </summary>
        /// <returns>The proxy user information.</returns>
        [WebMethod(Description = "Get ProxyUser Data Info", EnableSession = true)]
        public ArrayList GetProxyUserDataInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = string.Empty;
            ArrayList proxyUserArray = new ArrayList();
            xPolicyValue = xPolicyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_PROXYUSER);

            StringBuilder content = new StringBuilder();

            //0.Description about Proxy User settings.
            content.Append(LabelUtil.GetAdminUITextByKey("aca_delegates_checkbox_description"));
            content.Append("<br/>");
            content.Append(LabelUtil.GetAdminUITextByKey("aca_delegates_expiration_day"));
            content.Append("<br/>");
            content.Append(LabelUtil.GetAdminUITextByKey("aca_delegates_puge_day"));
            proxyUserArray.Add(content.ToString());

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                proxyUserArray.Add(ACAConstant.COMMON_N);
            }
            else
            {
                proxyUserArray.Add(xPolicyValue);
            }

            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.PROXY_INVITATION_EXPIRATION_DAYS);

            if (!string.IsNullOrEmpty(xPolicyValue))
            {
                proxyUserArray.Add(xPolicyValue);
            }

            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.PROXY_INVITATION_PURGE_DAYS);

            if (!string.IsNullOrEmpty(xPolicyValue))
            {
                proxyUserArray.Add(xPolicyValue);
            }

            return proxyUserArray;
        }

        /// <summary>
        /// Get the announcement information.
        /// </summary>
        /// <returns>The announcement information.</returns>
        [WebMethod(Description = "Get Announcement Data Info", EnableSession = true)]
        public ArrayList GetAnnouncementDataInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = string.Empty;
            ArrayList announcementArray = new ArrayList();
            xPolicyValue = xPolicyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_ANNOUNCEMENT);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                announcementArray.Add(ACAConstant.COMMON_N);
            }
            else
            {
                announcementArray.Add(xPolicyValue);
            }

            xPolicyValue = xPolicyBll.GetValueByKey(XPolicyConstant.ANNOUNCEMENT_INTERVAL);
            if (!string.IsNullOrEmpty(xPolicyValue))
            {
                announcementArray.Add(xPolicyValue);
            }

            return announcementArray;
        }

        /// <summary>
        /// Get the parcel genealogy information.
        /// </summary>
        /// <returns>The parcel genealogy information.</returns>
        [WebMethod(Description = "Get Parcel Genealogy Data Info", EnableSession = true)]
        public ArrayList GetParcelGenealogyDataInfo()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = string.Empty;
            ArrayList parcelGenealogyArray = new ArrayList();
            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_ENABLE_PARCEL_GENEALOGY);

            StringBuilder content = new StringBuilder();

            content.Append(LabelUtil.GetAdminUITextByKey("aca_parcelgen_checkbox_description"));
            parcelGenealogyArray.Add(content.ToString());

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                parcelGenealogyArray.Add(ACAConstant.COMMON_N);
            }
            else
            {
                parcelGenealogyArray.Add(xPolicyValue);
            }

            return parcelGenealogyArray;
        }

        /// <summary>
        /// Gets the examination data info.
        /// </summary>
        /// <returns>The examination information.</returns>
        [WebMethod(Description = "Get Examination Data Info", EnableSession = true)]
        public ArrayList GetExaminationDataInfo()
        {
            ArrayList examinationArray = new ArrayList();

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = string.Empty;

            xPolicyValue = xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_AUTO_UPDATE_EXAM_BY_CSV);

            StringBuilder content = new StringBuilder();

            content.Append(LabelUtil.GetAdminUITextByKey("aca_exam_setting_autoupdate_message"));
            examinationArray.Add(content.ToString());

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                examinationArray.Add(ACAConstant.COMMON_N);
            }
            else
            {
                examinationArray.Add(xPolicyValue);
            }

            return examinationArray;
        }

        /// <summary>
        /// Gets the global search data info.
        /// </summary>
        /// <returns>global search data info</returns>
        [WebMethod(Description = "Get Global Search Information", EnableSession = true)]
        public ArrayList GetGlobalSearchDataInfo()
        {
            string xPolicyValue = string.Empty;
            ArrayList results = new ArrayList();

            //0.enable global search 
            xPolicyValue = _xPolicyBll.GetValueByKey(ACAConstant.ENABLE_GLOBAL_SEARCH);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                results.Add(ACAConstant.COMMON_N);
            }
            else
            {
                results.Add(xPolicyValue);
            }

            //1.enable records(CAP) list
            xPolicyValue = _xPolicyBll.GetValueByKey(ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_RECORDS);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                results.Add(ACAConstant.COMMON_N);
            }
            else
            {
                results.Add(xPolicyValue);
            }

            //2.enable LP(licensed professional) list
            xPolicyValue = _xPolicyBll.GetValueByKey(ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_LP);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                results.Add(ACAConstant.COMMON_N);
            }
            else
            {
                results.Add(xPolicyValue);
            }

            //3.enable property information(APO) list
            xPolicyValue = _xPolicyBll.GetValueByKey(ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_APO);

            if (string.IsNullOrEmpty(xPolicyValue))
            {
                results.Add(ACAConstant.COMMON_N);
            }
            else
            {
                results.Add(xPolicyValue);
            }

            return results;
        }

        /// <summary>
        /// This method is to get EMSE Script data source.
        /// </summary>
        /// <returns>The EMSE script.</returns>
        [WebMethod(Description = "Get EMSE Script list", EnableSession = true)]
        public object GetScriptNameInfo()
        {
            //1. Get scritp name list.
            ArrayList itemsList = new ArrayList();

            string[] defaultItem = { string.Empty, WebConstant.DropDownDefaultText };
            itemsList.Add(defaultItem);

            IEMSEBll emseBll = (IEMSEBll)ObjectFactory.GetObject(typeof(IEMSEBll));
            object[] emseObj = emseBll.GetScriptNameList();

            if (emseObj == null ||
                emseObj.Length != 2)
            {
                return null;
            }

            object[] valueList = ((StringArray)emseObj[0]).item;
            object[] titleList = ((StringArray)emseObj[1]).item;

            if (valueList == null || titleList == null || valueList.Length != titleList.Length)
            {
                return null;
            }

            for (int i = 0; i < valueList.Length; i++)
            {
                string value = valueList[i] == null ? string.Empty : valueList[i].ToString();
                string title = titleList[i] == null ? string.Empty : titleList[i].ToString();
                string text = value + ACAConstant.SPLITLINE + title;

                string[] item = new string[2];
                item[0] = value;
                item[1] = text;

                itemsList.Add(item);
            }

            //2. Get script name selected value.
            string selectedValue = string.Empty;
            selectedValue = emseBll.GetEventScriptByPK(ACAConstant.FEE_ESTIMATE_AFTER4ACA, ADMIN_AUDIT_ID);

            //3. Build a object array to return.
            object[] objArray = new object[2];
            objArray[0] = itemsList;
            objArray[1] = selectedValue;

            return objArray;
        }

        /// <summary>
        /// This method is to save inspection setting page data.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="insdict">inspection data from inspection section in aca admin</param>
        /// <param name="appdict">inspection data from application section in aca admin</param>
        /// <returns>Indication save successful or not</returns>
        [WebMethod(Description = "Save Inspection Setting", EnableSession = true)]
        public bool SaveInspectionSettingInfo(string moduleName, object insdict, object appdict)
        {
            try
            {
                Dictionary<string, object> insdictList = (Dictionary<string, object>)insdict;
                Dictionary<string, object> appdictList = (Dictionary<string, object>)appdict;
                IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

                if (insdictList != null)
                {
                    foreach (KeyValuePair<string, object> kvp in insdictList)
                    {
                        _bizBll.CreateAndEditBizDomain(GetInspectionArray((object[])kvp.Value, moduleName), true);
                        policyBll.CreateOrUpdatePolicy(null, GetXPolicyArray((object[])kvp.Value, moduleName));
                        SaveSharedComments((object[])kvp.Value, moduleName);
                    }
                }

                if (appdictList != null)
                {
                    foreach (KeyValuePair<string, object> kvp in appdictList)
                    {
                        _adminBll.EditAppStatusGroup(GetAppStatusArray((object[])kvp.Value, moduleName));
                    }

                    // Clear App Status Setting from cache
                    HttpRuntime.Cache.Remove(ConfigManager.AgencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_APPSTATUS);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                return false;
            }
        }

        /// <summary>
        /// Update button setting models
        /// </summary>
        /// <param name="selectedCapTypeValues">ArrayList of cap type values</param>
        /// <param name="capTypePermissionList">ArrayList of Cap type permission</param>
        /// <param name="buttonName">button name</param>
        /// <param name="moduleName">module name</param>
        [WebMethod(Description = "UpdateButtonSetting4CapType", EnableSession = true)]
        public void UpdateButtonSetting4CapType(ArrayList selectedCapTypeValues, ArrayList capTypePermissionList, string buttonName, string moduleName)
        {
            CapTypeModel[] selectedCapTypes = selectedCapTypeValues.Count == 0 ?
                    null : ConstructCapTypeModelArray(selectedCapTypeValues);

            ButtonSettingModel4WS buttonSettingModel = new ButtonSettingModel4WS();
            buttonSettingModel.buttonName = buttonName;
            buttonSettingModel.moduleName = moduleName;
            buttonSettingModel.serviceProviderCode = ConfigManager.AgencyCode;
            buttonSettingModel.selectedCapTypeList = selectedCapTypes;

            CapTypePermissionModel[] capTypePermissions = new CapTypePermissionModel[capTypePermissionList.Count];

            for (int i = 0; i < capTypePermissionList.Count; i++)
            {
                CapTypePermissionModel capTypePermission = capTypePermissionList[i] as CapTypePermissionModel;

                SimpleAuditModel auditModel = new SimpleAuditModel();
                auditModel.auditID = ACAConstant.ADMIN_CALLER_ID;
                auditModel.auditDate = DateTime.Now;
                auditModel.auditStatus = "A";
                capTypePermission.auditModel = auditModel;
                capTypePermissions[i] = capTypePermission;
            }

            IAdminCapTypePermissionBll capTypePermissionBll = ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll)) as IAdminCapTypePermissionBll;
            capTypePermissionBll.UpdateButtonSetting4CapType(buttonSettingModel, capTypePermissions);
        }

        #endregion

        #region Maintain Filters

        /// <summary>
        /// create a cap type filter
        /// </summary>
        /// <param name="filterItem">A object which contains a filter information</param>
        [WebMethod(Description = "Create a new cap type filter", EnableSession = true)]
        public void CreateCapTypeFilter(object filterItem)
        {
            CapTypeFilterModel4WS filterModel = ContructFilterModel(filterItem);
            _capTypeFilterBll.CreateCapTypeFilter(filterModel, ADMIN_AUDIT_ID);
        }

        /// <summary>
        /// Save License types associated with a specified Cap Type
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <param name="capTypeName">the cap type name</param>
        /// <param name="controllerType">the controller type</param>
        /// <param name="selectedItems">the selected license type list.</param>
        /// <returns>true or false.</returns>
        [WebMethod(Description = "Save LicenseTypeList By CapType", EnableSession = true)]
        public bool SaveLicenseTypeListByCapType(string moduleName, string capTypeName, string controllerType, object selectedItems)
        {
            controllerType = string.Equals("0", controllerType) ? ControllerType.CAPTYPEFILTER.ToString() : ControllerType.CAPSEARCHFILTER.ToString();
            string entityType = EntityType.LICENSETYPE.ToString();
            SavePermissionByCapType(moduleName, capTypeName, selectedItems, controllerType, entityType);

            return true;
        }

        /// <summary>
        /// Save selected License types to XPolicy table by the specific XPolicy key.
        /// Policy Name is "ACA_CONFIGS", XPolicy key mapping to data1 column.
        /// </summary>
        /// <param name="xpolicyKey">XPolicy key, it's mapping to data1 column of XPolicy table.</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="rightName">The sub right name, it's mapping to data4 column of XPolicy table.</param>
        /// <param name="selectedLPTypes">the selected license type list</param>
        /// <returns>true or false.</returns>
        [WebMethod(Description = "Save selected License types to Xpolicy table by the specific xpolicyKey", EnableSession = true)]
        public bool SaveLicenseTypeListByXpolicyKey(string xpolicyKey, string moduleName, string rightName, string[] selectedLPTypes)
        {
            return SaveLicenseTypeListByXpolicy(xpolicyKey, moduleName, rightName, selectedLPTypes);
        }

        /// <summary>
        /// Save Section List associated with a specified Cap Type
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <param name="capTypeName">the cap type name</param>
        /// <param name="selectedItems">the selected license type list.</param>
        /// <returns>true or false.</returns>
        [WebMethod(Description = "Save LicenseVerification Sections By CapType", EnableSession = true)]
        public bool SaveSectionListByCapType(string moduleName, string capTypeName, string[] selectedItems)
        {
            int length = selectedItems.Length;

            if (length < 1)
            {
                return true;
            }

            try
            {
                IAdminCapTypePermissionBll capTypePermissionBll = (IAdminCapTypePermissionBll)ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll));

                string[] capTypeArray = capTypeName.Split(ACAConstant.SPLIT_CHAR);

                List<CapTypePermissionModel> capTypePermissionModels = new List<CapTypePermissionModel>();

                for (int i = 0; i < length; i++)
                {
                    if (string.IsNullOrEmpty(selectedItems[i]) || selectedItems[i].Split(ACAConstant.SPLIT_CHAR).Length < 2)
                    {
                        continue;
                    }

                    string[] sectionPermission = selectedItems[i].Split(ACAConstant.SPLIT_CHAR);
                    string permission = sectionPermission[1] == ACAConstant.COMMON_TRUE ? ACAConstant.ROLE_HASPERMISSION : ACAConstant.ROLE_NOPERMISSION;
                    string sectionName = sectionPermission[0];
                    CapTypePermissionModel capTypePermissionModel = new CapTypePermissionModel();
                    capTypePermissionModel.group = capTypeArray[0];
                    capTypePermissionModel.type = capTypeArray[1];
                    capTypePermissionModel.subType = capTypeArray[2];
                    capTypePermissionModel.category = capTypeArray[3];
                    capTypePermissionModel.serviceProviderCode = ConfigManager.AgencyCode;
                    capTypePermissionModel.moduleName = moduleName;
                    capTypePermissionModel.controllerType = ControllerType.LICENSEVERIFICATION.ToString();
                    capTypePermissionModel.entityType = EntityType.SECTIONTYPE.ToString();
                    capTypePermissionModel.entityPermission = permission;
                    capTypePermissionModel.entityKey1 = Server.HtmlDecode(sectionName);
                    SimpleAuditModel auditModel = new SimpleAuditModel();
                    auditModel.auditID = ACAConstant.ADMIN_CALLER_ID;
                    auditModel.auditStatus = ACAConstant.VALID_STATUS;
                    capTypePermissionModel.auditModel = auditModel;

                    capTypePermissionModels.Add(capTypePermissionModel);
                }

                capTypePermissionBll.SaveCapTypePermissions(capTypePermissionModels.ToArray());
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update x entity permissions.
        /// </summary>
        /// <param name="serializedDeletePK">A XEntityPermissionModel object be used as the PK to delete existing data.</param>
        /// <param name="serializedEntities">Serialized XEntityPermissionModel collection.</param>
        [WebMethod(EnableSession = true)]
        public void UpdateXEntityPermissions(string serializedDeletePK, string serializedEntities)
        {
            IXEntityPermissionBll entityBll = (IXEntityPermissionBll)ObjectFactory.GetObject(typeof(IXEntityPermissionBll));
            XEntityPermissionModel deletePK = null;
            XEntityPermissionModel[] xEntities = null;

            try
            {
                deletePK = JsonConvert.DeserializeObject<XEntityPermissionModel>(serializedDeletePK);
                xEntities = JsonConvert.DeserializeObject<XEntityPermissionModel[]>(serializedEntities);
            }
            finally
            {
                entityBll.UpdateXEntityPermissions(deletePK, xEntities);
            }
        }

        /// <summary>
        /// update the cap types associated with a specified filter
        /// </summary>
        /// <param name="filterItem">A object which contains a filter information</param>
        [WebMethod(Description = "Update Relation", EnableSession = true)]
        public void UpdatControlFilter(object filterItem)
        {
            CapTypeFilterModel4WS filterModel = ContructFilterModel(filterItem);
            _capTypeFilterBll.EditCapTypeFilter(filterModel, ADMIN_AUDIT_ID);
        }

        /// <summary>
        /// update the relationship between a button and a filter
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="labelKey">the label key of a control</param>
        /// <param name="filterName">the filter name</param>
        /// <returns>Indicating create or edit success or not.</returns>
        [WebMethod(Description = "Update Relation", EnableSession = true)]
        public bool CreateOrEditFilter4Button(string moduleName, string labelKey, string filterName)
        {
            XButtonFilterModel4WS xButtonFilter = new XButtonFilterModel4WS();
            xButtonFilter.servProvCode = ConfigManager.AgencyCode;
            xButtonFilter.controlLabelKey = labelKey;
            xButtonFilter.filterName = filterName;
            xButtonFilter.moduleName = moduleName;

            if (labelKey == "aca_account_management_contact_amendment")
            {
                _capTypeFilterBll.EditFilter4ButtonAgencyLevel(xButtonFilter, ADMIN_AUDIT_ID);
            }
            else
            {
                _capTypeFilterBll.CreateOrEditFilter4Button(xButtonFilter, ADMIN_AUDIT_ID);
            }

            return true;
        }

        /// <summary>
        /// update Button URL
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="labelKey">the label key of a control</param>
        /// <param name="configureButtonUrlId">the configure button url id</param>
        /// <returns>Indicating save success or not.</returns>
        [WebMethod(Description = "Save Configure Button URL", EnableSession = true)]
        public bool SaveConfigureButtonUrl(string moduleName, string labelKey, string configureButtonUrlId)
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XPolicyModel[] policyArray = new XPolicyModel[1];
            XPolicyModel policy = new XPolicyModel();
            policy.serviceProviderCode = ConfigManager.AgencyCode;
            policy.policyName = ACAConstant.ACA_SHOPPING_CART_REDIRECT_PAGE;
            policy.data1 = ACAConstant.ACA_SHOPPING_CART_REDIRECT_PAGE;
            policy.data4 = ACAConstant.ACA_SHOPPING_CART_REDIRECT_PAGE;
            policy.rightGranted = ACAConstant.GRANTED_RIGHT;
            policy.level = ACAConstant.LEVEL_TYPE_AGENCY;
            policy.levelData = ConfigManager.AgencyCode;
            policy.data2 = configureButtonUrlId;
            policy.status = ACAConstant.VALID_STATUS;
            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN_CALLER_ID;
            policyArray[0] = policy;

            xPolicyBll.CreateOrUpdatePolicy(null, policyArray);
            return true;
        }

        /// <summary>
        /// Save Auto Fill City/State value.
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="data2">the auto Fill Value</param>
        /// <param name="policyName">the policyName</param>
        /// <param name="data4">the position ID</param>
        /// <returns>true or false.</returns>
        [WebMethod(Description = "Save Policy Value For Data4 As Key", EnableSession = true)]
        public bool SavePolicyValueForData4AsKey(string moduleName, string data2, string policyName, string data4)
        {
            string levelType = ACAConstant.LEVEL_TYPE_MODULE;
            string levelData = moduleName;

            if (string.IsNullOrEmpty(moduleName))
            {
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
                levelData = ConfigManager.AgencyCode;
            }

            data2 = string.Equals(data2, ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XPolicyModel[] policyArray = new XPolicyModel[1];
            XPolicyModel policy = new XPolicyModel();
            policy.serviceProviderCode = ConfigManager.AgencyCode;
            policy.policyName = policyName;
            policy.data1 = policyName;
            policy.data4 = data4;
            policy.rightGranted = ACAConstant.GRANTED_RIGHT;
            policy.level = levelType;
            policy.levelData = levelData;
            policy.data2 = data2;
            policy.status = ACAConstant.VALID_STATUS;
            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN_CALLER_ID;
            policyArray[0] = policy;

            xPolicyBll.CreateOrUpdatePolicy(null, policyArray);

            return true;
        }

        /// <summary>
        /// Delete a cap type filter
        /// </summary>
        /// <param name="filterName">the filter name</param>
        /// <param name="moduleName">the module name</param>
        /// <returns>The cap type filter.</returns>
        [WebMethod(Description = "Update Relation", EnableSession = true)]
        public string DeleteCapTypeFilter(string filterName, string moduleName)
        {
            _capTypeFilterBll.DeleteCapTypeFilter(ConfigManager.AgencyCode, moduleName, filterName, ADMIN_AUDIT_ID);

            return GetCapTypeFilterListByModule(moduleName);
        }

        /// <summary>
        /// Check if a filter is associated with a control
        /// </summary>
        /// <param name="filterName">the filter name</param>
        /// <param name="moduleName">the module name</param>
        /// <returns>Indicating whether existing relation or not.</returns>
        [WebMethod(Description = "check existing relation", EnableSession = true)]
        public bool CheckFilterRelation(string filterName, string moduleName)
        {
            //check if this filter is associated with a control
            XButtonFilterModel4WS[] buttons = _capTypeFilterBll.GetFilter4ButtonListByFilterName(ConfigManager.AgencyCode, moduleName, filterName, ADMIN_AUDIT_ID);

            if (buttons == null || buttons.Length < 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the filter names of a specific button. 
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="labelKey">the label key of the clicked control</param>
        /// <returns>the filter name</returns>
        [WebMethod(Description = "Get filter name of a control", EnableSession = true)]
        public string GetCapFilterName(string moduleName, string labelKey)
        {
            return _capTypeFilterBll.GetFilter4Button(ConfigManager.AgencyCode, moduleName, labelKey, ADMIN_AUDIT_ID);
        }

        /// <summary>
        /// Get all cap type filter names. 
        /// </summary>
        /// <param name="labelKey">the label key of the clicked control</param>
        /// <param name="moduleName">the module name</param>
        /// <returns>The all cap type filter names.</returns>
        [WebMethod(Description = "Get all filter names", EnableSession = true)]
        public ArrayList GetCapTypeFilterList(string labelKey, string moduleName)
        {
            ArrayList filter = new ArrayList();

            ArrayList filterNameList = _capTypeFilterBll.GetCapTypeFilterList(ConfigManager.AgencyCode, ADMIN_AUDIT_ID);
            string selectedFilterName = _capTypeFilterBll.GetFilter4Button(ConfigManager.AgencyCode, moduleName, labelKey, ADMIN_AUDIT_ID);

            filter.Add(FormatFilterNameList(filterNameList));
            filter.Add(selectedFilterName);

            return filter;
        }

        /// <summary>
        /// Get ConfigureUrl Of LinkButton
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <returns>The configure url of LinkButton.</returns>
        [WebMethod(Description = "Get ConfigureUrl Of LinkButton", EnableSession = true)]
        public ArrayList GetConfigureUrlOfLinkButton(string moduleName)
        {
            ArrayList urlArray = new ArrayList();

            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            IList<ItemValue> buttonUrlbizDomainList = bizBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_ACA_PAGE_PICKER, false);

            string selectedUrlId = GetConfiguredUrlIdFormXPolicy();
            string selectedUrlName = GetUrlNameByUrlId(buttonUrlbizDomainList, selectedUrlId);

            string allUrl = FormatConfigureUrlList(buttonUrlbizDomainList);
            urlArray.Add(allUrl);
            urlArray.Add(selectedUrlName);
            return urlArray;
        }

        /// <summary>
        /// Get AutoFill city or state Policy value, Get section title expand property.
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="policyName">the policy Name</param>
        /// <param name="data4">data 4.</param>
        /// <returns>The policy value for data4 as key.</returns>
        [WebMethod(Description = "Get Policy Value For Data4 As Key", EnableSession = true)]
        public string GetPolicyValueForData4AsKey(string moduleName, string policyName, string data4)
        {
            string levelType = ACAConstant.LEVEL_TYPE_MODULE;
            string levelData = moduleName;

            if (string.IsNullOrEmpty(moduleName))
            {
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
                levelData = ConfigManager.AgencyCode;
            }

            IXPolicyBll xpolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            string value = xpolicyBll.GetPolicyValueForData4AsKey(policyName, levelType, levelData, data4);

            return value;
        }

        /// <summary>
        /// Get License Board Required.
        /// </summary>
        /// <returns>true or false.</returns>
        [WebMethod(Description = "Get License Board Required", EnableSession = true)]
        public bool GetLicensingBoardRequired()
        {
            return StandardChoiceUtil.IsLicensingBoardRequired();
        }

        /// <summary>
        /// Get LicenseType List.
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <param name="capTypeName">the cap type name.</param>
        /// <param name="controllerType">the controller type.</param>
        /// <returns>the license type list.</returns>
        [WebMethod(Description = "Get LicenseType List", EnableSession = true)]
        public string GetLicenseTypeList(string moduleName, string capTypeName, string controllerType)
        {
            if (string.IsNullOrEmpty(capTypeName))
            {
                return string.Empty;
            }

            int isChecked = 0;
            StringBuilder jasonLicenseType = new StringBuilder();
            jasonLicenseType.Append("{'LisenTypeList':[");
            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            SortedList licenseTypeList = licenseBll.GetLicenseTypes(ConfigManager.AgencyCode);
            ArrayList selectedLTList = new ArrayList();

            string lpControllerType = string.Equals("0", controllerType) ? ControllerType.CAPTYPEFILTER.ToString() : ControllerType.CAPSEARCHFILTER.ToString();

            selectedLTList = GetCapTypePermissionEntityKey1List(moduleName, capTypeName, lpControllerType, EntityType.LICENSETYPE.ToString());

            IDictionaryEnumerator enumerator = licenseTypeList.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (selectedLTList.Contains(enumerator.Key))
                {
                    isChecked = 1; //1:checked,0:no checked.
                }
                else
                {
                    isChecked = 0;
                }

                jasonLicenseType.Append("{");
                jasonLicenseType.Append("Key:'").Append(ScriptFilter.EncodeJson(enumerator.Key.ToString()));
                jasonLicenseType.Append("',Text:'").Append(ScriptFilter.EncodeJson(enumerator.Value.ToString()));
                jasonLicenseType.Append("',Checked:").Append(isChecked);
                jasonLicenseType.Append("},");
            }

            jasonLicenseType.Remove(jasonLicenseType.Length - 1, 1);
            jasonLicenseType.Append("]}");

            return jasonLicenseType.ToString();
        }

        /// <summary>
        /// Gets the license type list from XPolicy table by the specific information.
        /// Policy Name is "ACA_CONFIGS" and XPolicy Key mapping to data1 column.
        /// </summary>
        /// <param name="xpolicyKey">XPolicy key, mapping to data1 column of XPolicy table.</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="rightName">the sub right, mapping to data4 column of XPolicy table.</param>
        /// <returns>Return the license type list.</returns>
        [WebMethod(Description = "Gets the license type list from xpolicy table by the specific information", EnableSession = true)]
        public string GetLicenseTypeListByXpolicyKey(string xpolicyKey, string moduleName, string rightName)
        {
            return GetLicenseTypeListByXpolicy(xpolicyKey, moduleName, rightName);
        }

        /// <summary>
        /// Get LicenseVerification Section List.
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <param name="capTypeName">the cap type name.</param>
        /// <returns>the license type list.</returns>
        [WebMethod(Description = "Get LicenseVerification Section List", EnableSession = true)]
        public string GetLicenseVerificationSectionList(string moduleName, string capTypeName)
        {
            if (string.IsNullOrEmpty(capTypeName))
            {
                return string.Empty;
            }

            int isChecked = 0;
            StringBuilder jasonSection = new StringBuilder();
            jasonSection.Append("{'SectionList':[");

            Dictionary<string, string> allSections = CapUtil.GetLicenseVerificationSectionLabels(moduleName);

            foreach (KeyValuePair<string, string> kvp in allSections)
            {
                if (IsSectionHasPermission(moduleName, capTypeName, kvp.Key))
                {
                    isChecked = 1; //1:checked,0:no checked.
                }
                else
                {
                    isChecked = 0;
                }

                jasonSection.Append("{");
                jasonSection.Append("Key:'").Append(kvp.Key);
                jasonSection.Append("',Text:'").Append(LabelUtil.RemoveHtmlFormat(allSections[kvp.Key].ToString()));
                jasonSection.Append("',Checked:").Append(isChecked);
                jasonSection.Append("},");
            }

            jasonSection.Remove(jasonSection.Length - 1, 1);
            jasonSection.Append("]}");

            return jasonSection.ToString();
        }

        /// <summary>
        /// Get all cap type filter names for a specified module. 
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <returns>Json string for CAP type filter list.</returns>
        [WebMethod(Description = "Get all filter names", EnableSession = true)]
        public string GetCapTypeFilterListByModule(string moduleName)
        {
            string[] filterNameList = _capTypeFilterBll.GetCapTypeFilterListByModule(ConfigManager.AgencyCode, moduleName, ADMIN_AUDIT_ID);

            if (filterNameList == null ||
                filterNameList.Length < 1)
            {
                return "[[]]";
            }

            return ConvertArrayToJson(filterNameList);
        }

        /// <summary>
        /// Get all cap types
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="filterName">the filter name.For the amendment cap this is isAmendable field. for the Cap type filter, this is the CAP type filter name</param>
        /// <returns>A json string of all returned cap types</returns>
        [WebMethod(Description = "Get all cap types", EnableSession = true)]
        public string GetCapFilterCapTypeList(string moduleName, string filterName)
        {
            CapTypeFilterModel4WS filter = _capTypeFilterBll.GetCapTypeFilterModel(ConfigManager.AgencyCode, moduleName, filterName, ADMIN_AUDIT_ID); //.getCapTypeList4ACAByModule(moduleName);

            return CreateCapListJson(filter); // capTypeList;
        }

        /// <summary>
        /// Get all cap type list.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The all cap type list.</returns>
        [WebMethod(Description = "Get all cap types", EnableSession = true)]
        public string GetAllCapTypeList(string moduleName)
        {
            CapTypeFilterModel4WS filter = new CapTypeFilterModel4WS();

            filter.availableCapTypeList = _capTypeBll.GetCapTypeList4ACAByModule(moduleName, null);

            return CreateCapListJson(filter); // capTypeList;
        }

        #endregion

        /// <summary>
        /// Save role data of contact type.The role data is from grid of contact display control section.
        /// </summary>
        /// <param name="saveDatas">Data collection it is mark up of all aca user,cap creator,license professional.
        /// contact,owner and none</param>
        /// <returns>if save successful then return true, if save failed then return false.</returns>
        [WebMethod(Description = "Save roles of contact type", EnableSession = true)]
        public bool SaveRolesForContactType(string[][] saveDatas)
        {
            try
            {
                IXPolicyWrapper xpolicyWrapper = ObjectFactory.GetObject(typeof(IXPolicyWrapper)) as IXPolicyWrapper;
                XpolicyUserRolePrivilegeModel[] policys = new XpolicyUserRolePrivilegeModel[saveDatas.Length];
                HttpRequest newRequest = new HttpRequest(string.Empty, Context.Request.UrlReferrer.OriginalString, Context.Request.UrlReferrer.Query.Substring(1));
                string moduleName = newRequest.QueryString["moduleName"];
                for (int i = 0; i < saveDatas.Length; i++)
                {
                    XpolicyUserRolePrivilegeModel rolePrivilege = new XpolicyUserRolePrivilegeModel();
                    UserRolePrivilegeModel role = new UserRolePrivilegeModel();
                    role.allAcaUserAllowed = bool.Parse(saveDatas[i][2]);
                    role.capCreatorAllowed = bool.Parse(saveDatas[i][3]);
                    role.licensendProfessionalAllowed = bool.Parse(saveDatas[i][4]);
                    role.contactAllowed = bool.Parse(saveDatas[i][5]);
                    role.ownerAllowed = bool.Parse(saveDatas[i][6]);

                    rolePrivilege.userRolePrivilegeModel = role;
                    rolePrivilege.data1 = saveDatas[i][0];
                    rolePrivilege.serviceProviderCode = this._servProvCode;
                    rolePrivilege.level = ACAConstant.LEVEL_TYPE_MODULE;
                    rolePrivilege.levelData = moduleName;
                    rolePrivilege.policyName = ACAConstant.ACA_CONTACT_TYPE_USER_ROLES;
                    rolePrivilege.rightGranted = ACAConstant.GRANTED_RIGHT;
                    rolePrivilege.status = ACAConstant.COMMON_Y;

                    policys[i] = rolePrivilege;
                }

                xpolicyWrapper.CreateOrUpdatePolicy(this._servProvCode, policys, ACAConstant.ADMIN_CALLER_ID);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save user privilege for reports
        /// </summary>
        /// <param name="reportRoles">report role privilege</param>
        /// <param name="isModule">true means it's module level</param>
        /// <returns>true means success.</returns>
        [WebMethod(Description = "Save roles for reports", EnableSession = true)]
        public bool SaveRolesForReports(string[][] reportRoles, bool isModule)
        {
            //reportRoles[i][0]-Report ID,
            //reportRoles[i][1]-Report Name,
            //reportRoles[i][2-5]-Privilege bit
            HttpRequest newRequest = new HttpRequest(string.Empty, Context.Request.UrlReferrer.OriginalString, Context.Request.UrlReferrer.Query.Substring(1));
            string moduleName = isModule ? newRequest.QueryString["moduleName"].ToString() : string.Empty;

            if (reportRoles == null ||
                reportRoles.Length == 0)
            {
                return false;
            }

            int length = reportRoles.Length;
            XPolicyModel[] policyModels = new XPolicyModel[length];

            for (int i = 0; i < length; i++)
            {
                // data1-category, data2-user privilege bit, data4-report id
                policyModels[i] = GetCommonPolicy(moduleName);
                policyModels[i].data1 = ACAConstant.ACA_REPORT_ROLE_SETTING; // ACA_REPORT_ROLE_SETTING
                policyModels[i].data4 = reportRoles[i][0]; // report id

                StringBuilder roles = new StringBuilder();
                for (int j = 2; j < reportRoles[i].Length; j++)
                {
                    // combine role data
                    roles.Append(reportRoles[i][j]);
                }

                // store 10 bit for expanding
                policyModels[i].data2 = roles.ToString().PadRight(10, '0');
            }

            bool isSuccess = true;

            try
            {
                IXPolicyBll xpolicyBll = ObjectFactory.GetObject(typeof(IXPolicyBll)) as IXPolicyBll;
                xpolicyBll.CreateOrUpdatePolicy(null, policyModels);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Save user privilege for reports
        /// </summary>
        /// <param name="capTypeRoles">report role privilege</param>
        /// <param name="moduleName">the module name.</param>
        /// <param name="isModuleLevel">true means it's module level</param>
        /// <returns>true means success.</returns>
        [WebMethod(Description = "Save roles for Cap Types", EnableSession = true)]
        public bool SaveRolesForCapTypes(string[][] capTypeRoles, string moduleName, bool isModuleLevel)
        {
            bool isSuccess = true;

            if (capTypeRoles == null || capTypeRoles.Length == 0)
            {
                return false;
            }

            IAdminCapTypePermissionBll capTypePermissionBll = (IAdminCapTypePermissionBll)ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll));

            if (isModuleLevel)
            {
                string data4 = isModuleLevel ? "0" : "1";

                XpolicyUserRolePrivilegeModel xpolicy = new XpolicyUserRolePrivilegeModel();
                StringBuilder moduleRoles = new StringBuilder();

                for (int i = 2; i < capTypeRoles[0].Length; i++)
                {
                    // combine role data
                    moduleRoles.Append(capTypeRoles[0][i]);
                }

                string moduleRoleList = moduleRoles.ToString().PadRight(10, '0');

                SaveModuleUserRolePermission(xpolicy, ACAConstant.USER_ROLE_POLICY_FOR_CAP_SEARCH, moduleName, moduleRoleList, data4);
            }
            else
            {
                int length = capTypeRoles.Length;

                CapTypePermissionModel[] capTypePermissionModels = new CapTypePermissionModel[length];

                for (int i = 0; i < length; i++)
                {
                    string[] capTypeArray = ScriptFilter.DecodeJson(capTypeRoles[i][0]).Split(ACAConstant.SPLIT_CHAR);

                    CapTypePermissionModel capTypePermissionModel = new CapTypePermissionModel();
                    capTypePermissionModel.group = capTypeArray[0];
                    capTypePermissionModel.type = capTypeArray[1];
                    capTypePermissionModel.subType = capTypeArray[2];
                    capTypePermissionModel.category = capTypeArray[3];
                    capTypePermissionModel.serviceProviderCode = ConfigManager.AgencyCode;
                    capTypePermissionModel.moduleName = moduleName;
                    capTypePermissionModel.controllerType = ControllerType.CAPSEARCHFILTER.ToString();
                    capTypePermissionModel.entityType = EntityType.GENERAL.ToString();
                    capTypePermissionModel.entityKey1 = ACAConstant.NULL_VALUE;

                    SimpleAuditModel auditModel = new SimpleAuditModel();
                    auditModel.auditID = ACAConstant.ADMIN_CALLER_ID;
                    auditModel.auditStatus = ACAConstant.VALID_STATUS;
                    capTypePermissionModel.auditModel = auditModel;

                    StringBuilder roles = new StringBuilder();
                    for (int j = 2; j < capTypeRoles[i].Length; j++)
                    {
                        // combine role data
                        roles.Append(capTypeRoles[i][j]);
                    }

                    // store 10 bit for expanding
                    capTypePermissionModel.entityPermission = roles.ToString().PadRight(10, '0');

                    capTypePermissionModels[i] = capTypePermissionModel;
                }

                try
                {
                    capTypePermissionBll.SaveCapTypePermissions(capTypePermissionModels);
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                    isSuccess = false;
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// Save user privilege for reports
        /// </summary>
        /// <param name="sectionRoles">section roles</param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>true means success.</returns>
        [WebMethod(Description = "Save RecordDetail Section Roles", EnableSession = true)]
        public bool SaveRecordDetailSectionRoles(string[][] sectionRoles, string moduleName)
        {
            List<XPolicyModel> xpolicies = new List<XPolicyModel>();

            bool isSuccess = true;

            if (sectionRoles == null || sectionRoles.Length == 0)
            {
                return false;
            }

            int length = sectionRoles.Length;

            for (int i = 0; i < length; i++)
            {
                XPolicyModel xpolicy = new XPolicyModel();

                StringBuilder roles = new StringBuilder();
                for (int j = 2; j < sectionRoles[i].Length; j++)
                {
                    // combine role data
                    roles.Append(sectionRoles[i][j]);
                }

                xpolicy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, moduleName, BizDomainConstant.STD_ITEM_CAPDETAIL_SECTIONROLES, roles.ToString().PadRight(10, '0'), EntityType.GENERAL.ToString(), sectionRoles[i][0]);

                xpolicies.Add(xpolicy);
            }

            try
            {
                IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
                xPolicyBll.CreateOrUpdateXPolicyForData3AsKey(null, xpolicies.ToArray());
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// save page setting for reports
        /// </summary>
        /// <param name="moduleLevelCapTypeRoles">the module level cap type roles.</param> 
        /// <param name="isModuleLevel">is use module level settings.</param> 
        /// <returns>true means success.</returns>
        [WebMethod(Description = "Save Search Level", EnableSession = true)]
        public bool SaveSearchLevel(string[][] moduleLevelCapTypeRoles, bool isModuleLevel)
        {
            HttpRequest newRequest = new HttpRequest(string.Empty, Context.Request.UrlReferrer.OriginalString, Context.Request.UrlReferrer.Query.Substring(1));
            string moduleName = newRequest.QueryString["moduleName"].ToString();

            bool isSuccess = true;
            string searchLevel = isModuleLevel ? "0" : "1";

            try
            {
                string data4 = isModuleLevel ? "0" : "1";

                XpolicyUserRolePrivilegeModel xpolicy = new XpolicyUserRolePrivilegeModel();
                StringBuilder moduleRoles = new StringBuilder();

                for (int i = 2; i < moduleLevelCapTypeRoles[0].Length; i++)
                {
                    // combine role data
                    moduleRoles.Append(moduleLevelCapTypeRoles[0][i]);
                }

                string moduleRoleList = moduleRoles.ToString().PadRight(10, '0');

                SaveModuleUserRolePermission(xpolicy, ACAConstant.USER_ROLE_POLICY_FOR_CAP_SEARCH, moduleName, moduleRoleList, data4);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// save page setting for reports
        /// </summary>
        /// <param name="pageID">page id that set the reports</param>
        /// <param name="moduleName">module name</param>
        /// <param name="reports">reports object</param>
        /// <returns>true means success.</returns>
        [WebMethod(Description = "Save reports setting for page", EnableSession = true)]
        public bool SaveReportsForPage(string pageID, string moduleName, object[] reports)
        {
            if (reports == null ||
                reports.Length == 0)
            {
                return false;
            }

            IList<XPolicyModel> policyModels = new List<XPolicyModel>();

            foreach (object report in reports)
            {
                XPolicyModel policyModel = GetCommonPolicy(moduleName);

                // data1-category, data3-pageID, data2-visible(Y/N), data4-reportID
                policyModel.data1 = ACAConstant.ACA_REPORT_PAGE_SETTING; //"ACA_REPORT_PAGE_SETTING"
                policyModel.data3 = pageID;

                Dictionary<string, object> reportDC = (Dictionary<string, object>)report;
                foreach (KeyValuePair<string, object> kvp in reportDC)
                {
                    switch (kvp.Key)
                    {
                        case "visible":
                            policyModel.data2 = Convert.ToBoolean(kvp.Value) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                            break;
                        case "reportID":
                            policyModel.data4 = Convert.ToString(kvp.Value);
                            break;
                    }
                }

                policyModels.Add(policyModel);
            }

            XPolicyModel[] policies = new XPolicyModel[policyModels.Count];
            policyModels.CopyTo(policies, 0);

            bool isSuccess = true;

            try
            {
                IXPolicyBll xpolicyBll = ObjectFactory.GetObject(typeof(IXPolicyBll)) as IXPolicyBll;
                xpolicyBll.CreateOrUpdateXPolicyForData3AsKey(null, policies);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get Application status by CAP type
        /// </summary>
        /// <param name="buttonName">button name - Create Amendment, Create Document Delete</param>
        /// <param name="capTypeValues">CAP type keys</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>CapTypePermissionModel array</returns>
        [WebMethod(Description = "Get Application Status", EnableSession = true)]
        public CapTypePermissionModel[] GetApplicationStatus(string buttonName, string[] capTypeValues, string moduleName)
        {
            if (string.IsNullOrEmpty(buttonName) || capTypeValues == null || capTypeValues.Length == 0)
            {
                return null;
            }

            CapTypeModel[] capTypes = new CapTypeModel[capTypeValues.Length];

            for (int i = 0; i < capTypes.Length; i++)
            {
                capTypes[i] = new CapTypeModel();
                capTypes[i].serviceProviderCode = ConfigManager.AgencyCode;
                string[] capTypeKeys = ScriptFilter.DecodeJson(capTypeValues[i].ToString()).Split('/');
                capTypes[i].group = capTypeKeys[0];
                capTypes[i].type = capTypeKeys[1];
                capTypes[i].subType = capTypeKeys[2];
                capTypes[i].category = capTypeKeys[3];
                capTypes[i].moduleName = moduleName;
            }

            IAdminCapTypePermissionBll capTypePermissionBll = ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll)) as IAdminCapTypePermissionBll;
            CapTypePermissionModel[] permissions = capTypePermissionBll.GetCapTypePermission4ButtonSetting(buttonName, capTypes);

            return permissions;
        }

        /// <summary>
        /// Judge whether the biz domain is existed or not.
        /// </summary>
        /// <param name="bizDomain">biz domain name</param>
        /// <param name="bizDomainValues">biz domain values</param>
        /// <returns>message for exist biz domain value</returns>
        [WebMethod(Description = "Judge whether the biz domain is existed or not.", EnableSession = true)]
        public string IsExistBizDomainValue(string bizDomain, string[] bizDomainValues)
        {
            if (string.IsNullOrEmpty(bizDomain) || bizDomainValues == null || bizDomainValues.Length == 0)
            {
                return string.Empty;
            }

            int duplicateIndex = ValidationUtil.DuplicateIndexOf(bizDomainValues, true);
            
            if (duplicateIndex > -1)
            {
                return string.Format(LabelUtil.GetTextByKey("aca_admin_dropdownlist_conflicteditem_errormes", string.Empty), bizDomainValues[duplicateIndex], bizDomainValues[duplicateIndex], bizDomain);   
            }

            string isExistMessage = _bizBll.IsExistBizDomainValue(ConfigManager.AgencyCode, bizDomain, bizDomainValues);

            return isExistMessage;
        }

        /// <summary>
        /// Save action permission settings
        /// </summary>
        /// <param name="saveDatas">string[][] format for save data</param>
        /// <returns>true or false.</returns>
        [WebMethod(Description = "save inspection action permission settings", EnableSession = true)]
        public bool SaveActionPermissionSettings(string[][] saveDatas)
        {
            //1.Save saveDatas to DB.
            IInspectionTypePermissionBll insTyepBll = (IInspectionTypePermissionBll)ObjectFactory.GetObject(typeof(IInspectionTypePermissionBll));
            InspectionActionPermissionModel[] insActionPermissions = BuildInsActionPermissionBySaveDatas(saveDatas);
            bool result = insTyepBll.SaveActionPermissionSettings(insActionPermissions);

            //2. clear inspectin action permission settings in Cache.
            if (result)
            {
                _cacheManager.Remove(I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_INSPECTION_ACTION_PERMISSION));
            }

            return result;
        }

        /// <summary>
        /// Get inspection action permission array.
        /// </summary>
        /// <param name="appGroupCode">application status group code</param>
        /// <param name="appStatus">application status</param>
        /// <param name="insGroupCode">inspection group code</param>
        /// <returns>array list format for inspection action permission array.</returns>
        [WebMethod(Description = "Get Inspection action permissions", EnableSession = true)]
        public ArrayList GetInsActionPermissionSettings(string appGroupCode, string appStatus, string insGroupCode)
        {
            //1. Get inspection type model list by nspectionGroupCode.
            IInspectionTypeBll insTyepBll = (IInspectionTypeBll)ObjectFactory.GetObject(typeof(IInspectionTypeBll));
            InspectionTypeModel[] insTypes = insTyepBll.GetInspectionTypesByGroupCode(insGroupCode);

            //2. Build inspection type data row array by inspection types. the data row key is inspection type sequence number.
            InspectionTypeDataRowInfo[] insTypeDataRows = BuildInsTypeDataRowsByInsTypes(insTypes);

            //3. Get inspection action permission setting records by appGroupCode, appStatus and inspectionGroupCode.
            IInspectionTypePermissionBll insTypePermissionBll = (IInspectionTypePermissionBll)ObjectFactory.GetObject(typeof(IInspectionTypePermissionBll));
            IList<InspectionActionPermissionModel> insActionPermissionRecords = insTypePermissionBll.GetInspectionActionPermissions(appGroupCode, appStatus, insGroupCode);

            //4. Set inspection type data row action column enabled by insepction action permission setting records in DB.
            SetInsActionColumnEnable(ref insTypeDataRows, insActionPermissionRecords);

            //5. Conert date format as string.
            return ConvertInsActionPermissionsToArray(insTypeDataRows);
        }

        /// <summary>
        /// Gets the customize component by element id.
        /// </summary>
        /// <param name="elementId">The element id.</param>
        /// <returns>Return the customize component</returns>
        [WebMethod(Description = "Get customize component names in a page", EnableSession = true)]
        public string GetCustomComponentByElementId(int elementId)
        {
            StringBuilder result = new StringBuilder();
            result.Append("[");

            CustomComponentModel searchModel = new CustomComponentModel();
            searchModel.serviceProviderCode = ConfigManager.AgencyCode;
            searchModel.elementID = elementId;

            CustomComponentModel[] models = _customComponentBll.GetComponentConfig(searchModel);
            if (models != null)
            {
                foreach (CustomComponentModel model in models)
                {
                    result.Append("{");
                    result.AppendFormat("ResID:'{0}',", model.resID);
                    result.AppendFormat("ComponentName:'{0}'", ScriptFilter.EncodeJson(model.componentName));
                    result.Append("},");
                }
            }

            // remove last comma
            if (result.Length > 1)
            {
                result.Remove(result.Length - 1, 1);
            }

            result.Append("]");

            return result.ToString();
        }

        #region service group public

        /// <summary>
        /// Save service group setting
        /// </summary>
        /// <param name="filterItem">A object which contains a filter information</param>
        /// <returns>the service group res id</returns>
        [WebMethod(Description = "Save the service group setting ", EnableSession = true)]
        public string SaveServiceGroupSetting(object filterItem)
        {
            XServiceGroupModel[] xServiceGroupList = ContructServiceFilterModel(filterItem);
            IServiceManagementBll serviceManagementBll = ObjectFactory.GetObject<IServiceManagementBll>();
            IList<XServiceGroupModel> serviceGroups = serviceManagementBll.SaveServiceGroupSetting(ConfigManager.AgencyCode, xServiceGroupList, ADMIN_AUDIT_ID);
            string result = string.Empty;

            if (serviceGroups != null && serviceGroups.Count > 0
                && serviceGroups[0].group != null && serviceGroups[0].group.serviceGroupSeqNbr != null)
            {
                result = serviceGroups[0].group.serviceGroupSeqNbr.ToString();
            }

            return result;
        }

        /// <summary>
        /// Delete service group setting
        /// </summary>
        /// <param name="serviceGroupSeqNbr">the service group sequence number and can for I18N.</param>
        [WebMethod(Description = "Delete service group setting ", EnableSession = true)]
        public void DeleteServiceGroupSetting(string serviceGroupSeqNbr)
        {
            IServiceManagementBll serviceManagementBll = ObjectFactory.GetObject<IServiceManagementBll>();
            serviceManagementBll.DeleteServiceGroupSetting(ConfigManager.AgencyCode, serviceGroupSeqNbr);
        }

        /// <summary>
        /// Get all available service groups
        /// </summary>
        /// <returns>return the available service groups</returns>
        [WebMethod(Description = "Get all available service groups ", EnableSession = true)]
        public string GetAllServiceGroup()
        {
            IServiceManagementBll serviceManagementBll = ObjectFactory.GetObject<IServiceManagementBll>();
            IList<ServiceGroupModel> groups = serviceManagementBll.GetAllServiceGroup(ConfigManager.AgencyCode);
            StringBuilder buf = new StringBuilder();
            buf.Append("[");

            if (groups != null && groups.Count > 0)
            {
                IList<ServiceGroupModel> sortGroups = groups.OrderBy(o => o.resGroupCode).ThenBy(o => o.groupCode).ToList();

                foreach (ServiceGroupModel group in sortGroups)
                {
                    buf.Append("{");
                    buf.AppendFormat("'serviceGroupSeqNbr':'{0}',", group.serviceGroupSeqNbr);
                    buf.AppendFormat("'groupName':'{0}',", ScriptFilter.EncodeJson(group.groupCode));

                    if (I18nCultureUtil.IsInMasterLanguage)
                    {
                        group.resGroupCode = string.Empty;
                    }

                    buf.AppendFormat("'resGroupName':'{0}',", ScriptFilter.EncodeJson(group.resGroupCode));
                    buf.AppendFormat("'sortOrder':'{0}'", group.sortOrder);
                    buf.Append("},");
                }

                // remove last comma
                buf.Remove(buf.Length - 1, 1);
            }

            buf.Append("]");
            return buf.ToString();
        }

        /// <summary>
        /// Get All Available and Selected Services
        /// </summary>
        /// <param name="serviceGroupSeqNbr">the service group sequence number and can for I18N.</param>
        /// <returns>All available and selected service json format</returns>
        [WebMethod(Description = "Get all available and selected Service", EnableSession = true)]
        public string GetServicesByGroupSeqNbr(string serviceGroupSeqNbr)
        {
            IServiceManagementBll serviceManagementBll = ObjectFactory.GetObject<IServiceManagementBll>();
            IList<ServiceModel> availableServiceList = serviceManagementBll.GetAllService(ConfigManager.AgencyCode, ADMIN_AUDIT_ID);
            StringBuilder buf = new StringBuilder();

            buf.Append("{'AvailableServiceList':[");
            int sortOrder = 1;

            if (availableServiceList != null && availableServiceList.Count > 0)
            {
                IList<ServiceModel> sortAvailableServiceList = availableServiceList.OrderBy(o => o.servPorvCode).ThenBy(o => o.serviceName).ToList();

                foreach (ServiceModel serviceModel in sortAvailableServiceList)
                {
                    //Need replace the CreateTreeNodeJson for ServiceModel.
                    buf.Append(CreateServiceNodeJson(serviceModel, sortOrder));
                    sortOrder++;
                }

                buf.Remove(buf.Length - 1, 1);
            }

            buf.Append("],");
            buf.Append("'SelectedServiceList':[");

            if (!string.IsNullOrEmpty(serviceGroupSeqNbr))
            {
                IList<XServiceGroupModel> selectedServiceList = serviceManagementBll.GetXServiceGroup(ConfigManager.AgencyCode, serviceGroupSeqNbr);

                if (selectedServiceList != null && selectedServiceList.Count > 0)
                {
                    IList<XServiceGroupModel> sortSelectedServiceList = selectedServiceList.OrderBy(o => o.sortOrder).ToList();

                    foreach (XServiceGroupModel xServiceGroupModel in sortSelectedServiceList)
                    {
                        buf.Append(CreateServiceNodeJson(xServiceGroupModel.service, sortOrder));
                        sortOrder++;
                    }

                    buf.Remove(buf.Length - 1, 1);
                }
            }

            buf.Append("]}");

            return buf.ToString();
        }

        #endregion service group public

        /// <summary>
        /// Get license type by licensing board.
        /// </summary>
        /// <param name="licensingBoard">the licensing board.</param>
        /// <param name="entityID1">module name or agency code.</param>
        /// <returns>License Type</returns>
        [WebMethod(Description = "Get license type by licensing board.", EnableSession = true)]
        public string GetLicenseTypesByLicensingBoard(string licensingBoard, string entityID1)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_LICENSE_TYPE, false, (int)DropDownListShowType.ShowValue);

            XEntityPermissionModel xentity = new XEntityPermissionModel();
            xentity.servProvCode = ConfigManager.AgencyCode;
            xentity.entityType = XEntityPermissionConstant.LICENSING_BOARD;
            xentity.entityId = entityID1;
            xentity.entityId2 = licensingBoard;

            IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
            IEnumerable<XEntityPermissionModel> licensingBoardEntities = xEntityPermissionBll.GetXEntityPermissions(xentity);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{{licensingBoard:'{0}',licenseTypesMapping:[", ScriptFilter.EncodeJson(licensingBoard));

            if (stdItems.Any())
            {
                foreach (ItemValue stdItem in stdItems)
                {
                    string isChecked = ACAConstant.COMMON_N;

                    if (licensingBoardEntities == null || licensingBoardEntities.Count() == 0)
                    {
                        isChecked = ACAConstant.COMMON_Y;
                    }
                    else if (licensingBoardEntities.Any(f => string.Equals(f.entityId3, stdItem.Key, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        XEntityPermissionModel entityModel = licensingBoardEntities.FirstOrDefault(f => string.Equals(f.entityId3, stdItem.Key, StringComparison.InvariantCultureIgnoreCase));
                        isChecked = entityModel.permissionValue;
                    }

                    sb.Append("{");
                    sb.AppendFormat(
                                "entityId3:'{0}',resEntityId3:'{1}',value:'{2}'",
                                ScriptFilter.EncodeJson(stdItem.Key),
                                ScriptFilter.EncodeJson(stdItem.Value.ToString()),
                                isChecked);
                    sb.Append("},");
                }

                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]}");

            return sb.ToString();
        }

        /// <summary>
        /// Get the available contact types with contact type data source.
        /// </summary>
        /// <param name="contactTypeSource">The contact type source.</param>
        /// <returns>The available contact types with contact type data source.</returns>
        [WebMethod(Description = "Get the available contact types with contact type data source.", EnableSession = true)]
        public string GetAvailableContactTypes(string contactTypeSource)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, contactTypeSource);
            StringBuilder contactTypes = new StringBuilder();

            contactTypes.Append("[");

            if (stdItems.Any())
            {                
                foreach (ItemValue itemValue in stdItems)
                {
                    //if xpolicy models is null. it will get item from stander choice.
                    if (itemValue == null)
                    {
                        continue;
                    }

                    contactTypes.AppendFormat("{{Key:'{0}', Value:'{1}'}},", ScriptFilter.EncodeJson(itemValue.Key), ScriptFilter.EncodeJson(itemValue.Value == null ? string.Empty : itemValue.Value.ToString()));
                }

                if (contactTypes.ToString().Length > 1)
                {
                    contactTypes.Remove(contactTypes.Length - 1, 1);   
                }           
            }

            contactTypes.Append("]");

            return contactTypes.ToString();
        }

        /// <summary>
        /// Get the editable setting for all contact types.
        /// </summary>
        /// <returns>The editable setting for all contact types</returns>
        [WebMethod(Description = " Get the editable setting for all contact types.", EnableSession = true)]
        public string GetContactTypeEditableSettings()
        {
            XEntityPermissionModel xentity = new XEntityPermissionModel();
            xentity.servProvCode = ConfigManager.AgencyCode;
            xentity.entityType = XEntityPermissionConstant.REFERENCE_CONTACT_EDITABLE_BY_CONTACT_TYPE;

            IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
            IEnumerable<XEntityPermissionModel> contactTypeEditableEntities = xEntityPermissionBll.GetXEntityPermissions(xentity);

            StringBuilder sb = new StringBuilder();

            if (contactTypeEditableEntities != null && contactTypeEditableEntities.Count() > 0)
            {
                sb.Append("[");

                foreach (var entityModel in contactTypeEditableEntities)
                {
                    sb.AppendFormat("{{contactType: '{0}', editable: '{1}'}},", ScriptFilter.EncodeJson(entityModel.entityId2), entityModel.permissionValue); 
                }

                sb.Append("]");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Save the customized theme in aca admin.
        /// </summary>
        /// <param name="theme">theme's string</param>
        /// <returns>true is save successfully</returns>
        [WebMethod(Description = "save custom theme", EnableSession = true)]
        public bool SaveCustomThemeInfo(string theme)
        {
            try
            {
                IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();

                var policy = GetXPolicyModel(
                    ACAConstant.LEVEL_TYPE_AGENCY,
                    ConfigManager.AgencyCode,
                    XPolicyConstant.COLOR_THEME,
                    theme ?? string.Empty);
                var policies = new[] {policy};
                policyBll.CreateOrUpdatePolicy(null, policies);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion  Public Method

        #region Prviate Method
        /// <summary>
        /// Save Label
        /// </summary>
        /// <param name="labelKey">label key</param>
        /// <param name="value">label value</param>
        /// <param name="textContent">text content</param>
        /// <param name="servProvCode">server provider code</param>
        /// <param name="levelType">level type</param>
        /// <param name="levelName">level name.</param>
        private void SaveLabel(string labelKey, string value, string textContent, string servProvCode, string levelType, string levelName)
        {
            string languageCode = string.Empty;
            string regionalCode = string.Empty;
            I18nCultureUtil.SplitCulture(I18nCultureUtil.UserPreferredCulture, ref languageCode, ref regionalCode);

            XUITextModel xuiText = new XUITextModel();
            xuiText.stringKey = labelKey;
            xuiText.servProvCode = servProvCode;
            xuiText.countryCode = regionalCode;
            xuiText.langCode = languageCode;
            xuiText.textLevelName = levelName;
            xuiText.textLevelType = levelType;
            xuiText.stringValue = value;
            xuiText.textContent = textContent;
            List<XUITextModel> xuiList = new List<XUITextModel>();
            xuiList.Add(xuiText);

            AdminConfigurationModel4WS adminConfiguration = new AdminConfigurationModel4WS();
            adminConfiguration.callerId = ACAConstant.ADMIN_CALLER_ID;
            adminConfiguration.servProvCode = servProvCode;
            adminConfiguration.labelModelArray = xuiList.ToArray();
            adminConfiguration.levelType = levelType;
            adminConfiguration.moduleName = levelName;
            IAdminConfigurationSave saver = (IAdminConfigurationSave)ObjectFactory.GetObject(typeof(IAdminConfigurationSave));
            bool isSuccessful = saver.SaveAdminConfigurationData(adminConfiguration);
            if (isSuccessful)
            {
                UpdateLabelCache(xuiList, servProvCode, levelType, levelName, false);
            }
        }

        /// <summary>
        /// Update SimpleViewElement's Top.
        /// </summary>
        private void UpdateSimpleViewElementsTop()
        {
            if (_sectionFieldsModelList == null || _sectionFieldsModelList.Count == 0)
            {
                return;
            }

            foreach (SimpleViewModel4WS simpleView in _sectionFieldsModelList)
            {
                if (simpleView.permissionModel != null && simpleView.permissionModel.permissionLevel != null)
                {
                    int rowHeight = 0;
                    int maxtop = GetMaxTop(simpleView.simpleViewElements, ref rowHeight);

                    for (int i = 0; i < simpleView.simpleViewElements.Length; i++)
                    {
                        if (simpleView.simpleViewElements[i].OldStatus != simpleView.simpleViewElements[i].recStatus)
                        {
                            // if element is not visible.
                            if (simpleView.simpleViewElements[i].recStatus == "I")
                            {
                                simpleView.simpleViewElements[i].elementTop = 10001 + i;
                                simpleView.simpleViewElements[i].elementLeft = 0;
                            }

                            //if element change to visible
                            if (simpleView.simpleViewElements[i].recStatus == "A")
                            {
                                maxtop += rowHeight;
                                simpleView.simpleViewElements[i].elementTop = maxtop;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get maximum top.
        /// </summary>
        /// <param name="simpleElements">The simple view element model list.</param>
        /// <param name="maxElementHeight">Max element height.</param>
        /// <returns>The maximum top.</returns>
        private int GetMaxTop(SimpleViewElementModel4WS[] simpleElements, ref int maxElementHeight)
        {
            int lastRowElementTop = 0;

            foreach (SimpleViewElementModel4WS item in simpleElements)
            {
                if (item.elementTop > lastRowElementTop && ACAConstant.VALID_STATUS.Equals(item.OldStatus))
                {
                    lastRowElementTop = item.elementTop;
                }
            }

            IEnumerable<SimpleViewElementModel4WS> textareaElements = simpleElements.Where(f => string.Equals(f.elementType, "textarea", StringComparison.InvariantCultureIgnoreCase) && f.elementTop == lastRowElementTop && ACAConstant.VALID_STATUS.Equals(f.OldStatus));

            if (textareaElements != null && textareaElements.Any())
            {
                foreach (var item in textareaElements)
                {
                    if (maxElementHeight < item.elementHeight)
                    {
                        maxElementHeight = item.elementHeight;
                    }
                }
            }

            maxElementHeight = maxElementHeight < FORM_DESIGNER_MIN_ROW_HEIGHT ? FORM_DESIGNER_MIN_ROW_HEIGHT : maxElementHeight + FORM_DESIGNER_GRID_UNIT;

            return lastRowElementTop;
        }

        /// <summary>
        /// Synchronize section model.
        /// </summary>
        private void SynchSectionModels()
        {
            if (_gridViewHeadWidthList != null && _gridViewHeadWidthList.Count > 0)
            {
                foreach (string sectionId in _gridViewHeadWidthList.Keys)
                {
                    SimpleViewModel4WS sectionModel = null;

                    foreach (SimpleViewModel4WS section in _sectionFieldsModelList)
                    {
                        if (section.sectionID == sectionId)
                        {
                            sectionModel = section;
                            break;
                        }
                    }

                    if (sectionModel == null)
                    {
                        IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
                        GFilterScreenPermissionModel4WS permission = _gridViewHeadWidthList[sectionId].permissionModel;

                        sectionModel = new SimpleViewModel4WS();
                        sectionModel.sectionID = sectionId;

                        if (permission != null && !string.IsNullOrEmpty(permission.permissionLevel) && !string.IsNullOrEmpty(permission.permissionValue))
                        {
                            sectionModel.permissionModel = permission;
                            sectionModel.simpleViewElements = gviewBll.GetSimpleViewElementModel(_moduleName, permission, sectionId, ACAConstant.ADMIN_CALLER_ID);
                        }
                        else
                        {
                            sectionModel.simpleViewElements = gviewBll.GetSimpleViewElementModel(_moduleName, sectionId);
                        }

                        _sectionFieldsModelList.Add(sectionModel);
                    }

                    SynchSectionModelFields(_gridViewHeadWidthList[sectionId].simpleViewElements, sectionModel.simpleViewElements);
                }
            }
        }

        /// <summary>
        /// Synchronize section model.
        /// </summary>
        /// <param name="list">The first SimpleViewElementModel4WS list.</param>
        /// <param name="simpleViewElementModel4WS">The second SimpleViewElementModel4WS list.</param>
        private void SynchSectionModelFields(SimpleViewElementModel4WS[] list, SimpleViewElementModel4WS[] simpleViewElementModel4WS)
        {
            foreach (SimpleViewElementModel4WS width in list)
            {
                foreach (SimpleViewElementModel4WS model in simpleViewElementModel4WS)
                {
                    if (width.viewElementName == model.viewElementName)
                    {
                        model.elementWidth = width.elementWidth;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// update label cache after saved
        /// </summary>
        /// <param name="xUITextModels">The XUITextModel.</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="levelType">The level type.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="removeStandardChoiceCache">is remove standard choice cache or not.</param>
        private void UpdateLabelCache(List<XUITextModel> xUITextModels, string agencyCode, string levelType, string moduleName, bool removeStandardChoiceCache)
        {
            if (xUITextModels != null && xUITextModels.Count > 0)
            {
                Hashtable labels = _cacheManager.GetCachedItem(agencyCode, agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_LABEL));

                if (labels != null)
                {
                    foreach (XUITextModel xUITextModel in xUITextModels)
                    {
                        xUITextModel.servProvCode = agencyCode;

                        if (string.IsNullOrEmpty(xUITextModel.textLevelName))
                        {
                            xUITextModel.textLevelType = levelType;
                            xUITextModel.textLevelName = moduleName;
                        }

                        string cacheKey = LabelUtil.BuildLabelKey(xUITextModel);
                        labels[cacheKey] = xUITextModel.stringValue;
                        labels[cacheKey + ACAConstant.LABEL_CONTENT_SUFFIX] = xUITextModel.textContent;
                    }

                    _cacheManager.RefreshLabels(agencyCode, labels);
                }
            }

            if (removeStandardChoiceCache)
            {
                string cacheKey = ConfigManager.AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_STANDARDCHOICE);
                _cacheManager.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Analysis configuration data collection
        /// </summary>
        /// <param name="dataCollection">The data collection.</param>
        /// <returns>Indicating analysis success or not.</returns>
        private bool AnalysisConfigurationDataColletionSuccessfully(object dataCollection)
        {
            Dictionary<string, object> collectionDC;
            bool result = false;
            if (!dataCollection.Equals(null))
            {
                collectionDC = (Dictionary<string, object>)dataCollection;
                if ((collectionDC["ModuleName"] != null) &&
                    (collectionDC["ModuleName"].ToString() != string.Empty))
                {
                    _moduleName = collectionDC["ModuleName"].ToString();
                    _levelType = ACAConstant.LEVEL_TYPE_MODULE;
                }

                collectionDC.Remove("ModuleName");

                //get GS home page selected agency.
                if (collectionDC["AgencyCode"] != null && collectionDC["AgencyCode"].ToString() != string.Empty)
                {
                    _selectedAgency = collectionDC["AgencyCode"].ToString();
                }

                collectionDC.Remove("AgencyCode");

                Dictionary<string, object> dictItems = (Dictionary<string, object>)collectionDC["Items"];
                if (dictItems == null)
                {
                    return false;
                }

                foreach (KeyValuePair<string, object> kvp in dictItems)
                {
                    try
                    {
                        switch (kvp.Key)
                        {
                            case "9": // "arrHeadWith"
                                MappingCollection2HeadWith((object[])kvp.Value);
                                break;
                            case "10": // "arrGridViewPageSize"
                                SaveGridViewPageSize((object[])kvp.Value);
                                break;
                            case "11": // "arrCustomComponent"
                                // do nothing
                                break;
                            default:
                                MappingCollection2Views((object[])kvp.Value);
                                break;
                        }

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This method is to get Interval information from standard choice.
        /// </summary>
        /// <returns>The interval information.</returns>
        private string GetIntervalDataInfo()
        {
            StringBuilder intervalData = new StringBuilder();
            string content = string.Empty;
            string labelkeyDay = string.Empty;

            BizDomainModel4WS bizModel;

            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_PURGE_EXPIRED_ACCOUNT_INTERVAL;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);
            if (bizModel != null)
            {
                intervalData.AppendFormat(",\"EnablePurgeExpiredAccountInterval\":\"{0}\"", bizModel.auditStatus == null ? "I" : bizModel.auditStatus);
                intervalData.AppendFormat(",\"PurgeExpiredAccountInterval\":\"{0}\"", I18nStringUtil.GetString(bizModel.resDescription, bizModel.description));
            }
            else
            {
                intervalData.AppendFormat(",\"EnablePurgeExpiredAccountInterval\":\"{0}\"", "I");
                intervalData.AppendFormat(",\"PurgeExpiredAccountInterval\":\"{0}\"", string.Empty);
            }

            content = LabelUtil.GetAdminUITextByKey("admin_registration_setting_label_interval_head_content");
            labelkeyDay = LabelUtil.GetAdminUITextByKey("admin_registration_setting_label_interval_day");

            intervalData.AppendFormat(",\"IntervalDataLableContent\":\"{0}\"", content);
            intervalData.AppendFormat(",\"IntervalDataLableDay\":\"{0}\"", labelkeyDay);

            return intervalData.ToString();
        }

        /// <summary>
        /// Get Password Settings from XPolicy.
        /// </summary>
        /// <returns>The password setting.</returns>
        private string GetPasswordDataInfo()
        {
            StringBuilder passWordSettings = new StringBuilder();
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            // Password expiration setting
            XPolicyModel[] xPolicyList = xPolicyBll.GetPolicyListByCategory(BizDomainConstant.STD_ITEM_PASSWORD_EXPRIATION_CHECK, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode);

            if (xPolicyList == null || xPolicyList.Length == 0)
            {
                passWordSettings.AppendFormat(",\"EnablePasswordExpires\":\"{0}\"", ACAConstant.COMMON_N);
                passWordSettings.AppendFormat(",\"PasswordExpires\":\"{0}\"", string.Empty);
            }
            else
            {
                XPolicyModel xpolicy = xPolicyList[0];

                // checkbox for password expiration
                passWordSettings.AppendFormat(",\"EnablePasswordExpires\":\"{0}\"", string.IsNullOrEmpty(xpolicy.data2) ? ACAConstant.COMMON_N : xpolicy.data2);

                // failed attempts times
                passWordSettings.AppendFormat(",\"PasswordExpires\":\"{0}\"", string.IsNullOrEmpty(xpolicy.data3) ? string.Empty : xpolicy.data3);
            }

            // Password failed attempts setting
            xPolicyList = xPolicyBll.GetPolicyListByCategory(BizDomainConstant.STD_ITEM_PASSWORD_FAILED_ATTEMPTS_CHECK, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode);

            if (xPolicyList == null || xPolicyList.Length == 0)
            {
                passWordSettings.AppendFormat(",\"EnableLockAccountAfter\":\"{0}\"", ACAConstant.COMMON_N);
                passWordSettings.AppendFormat(",\"LockAccountAfter\":\"{0}\"", string.Empty);
                passWordSettings.AppendFormat(",\"LockAccountDurationHours\":\"{0}\"", string.Empty);
            }
            else
            {
                XPolicyModel xpolicy = xPolicyList[0];

                passWordSettings.AppendFormat(",\"EnableLockAccountAfter\":\"{0}\"", string.IsNullOrEmpty(xpolicy.data2) ? ACAConstant.COMMON_N : xpolicy.data2);
                passWordSettings.AppendFormat(",\"LockAccountAfter\":\"{0}\"", string.IsNullOrEmpty(xpolicy.data3) ? string.Empty : xpolicy.data3);
                passWordSettings.AppendFormat(",\"LockAccountDurationHours\":\"{0}\"", string.IsNullOrEmpty(xpolicy.data5) ? string.Empty : xpolicy.data5);
            }

            return passWordSettings.ToString();
        }

        /// <summary>
        /// Mapping collection to view.
        /// </summary>
        /// <param name="elementCollection">The element collection.</param>
        private void MappingCollection2Views(object[] elementCollection)
        {
            Dictionary<string, object> elementDC;

            bool isSub;

            string languageCode = string.Empty;
            string regionalCode = string.Empty;
            I18nCultureUtil.SplitCulture(I18nCultureUtil.UserPreferredCulture, ref languageCode, ref regionalCode);

            foreach (object element in elementCollection)
            {
                elementDC = (Dictionary<string, object>)element;

                if (elementDC.ContainsKey("IsTemplateField") && elementDC.ContainsKey("TemplateAttribute") && null != elementDC["IsTemplateField"] && (bool)elementDC["IsTemplateField"] && null != elementDC["TemplateAttribute"])
                {
                    // deal with Template fields
                    SimpleAuditModel auditModel = new SimpleAuditModel();
                    auditModel.auditID = ACAConstant.ADMIN_CALLER_ID;
                    auditModel.auditStatus = ACAConstant.VALID_STATUS;

                    TemplateLayoutConfigModel templateConfigModel = new TemplateLayoutConfigModel();
                    templateConfigModel.auditModel = auditModel;
                    templateConfigModel.configLevel = ConfigLevel.FIELDNAME;
                    templateConfigModel.serviceProviderCode = _servProvCode;

                    TemplateAttribute templateAttribute = (TemplateAttribute)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(elementDC["TemplateAttribute"]), typeof(TemplateAttribute));

                    templateConfigModel.entityType = templateAttribute.EntityType;
                    templateConfigModel.templateCode = templateAttribute.TemplateCode;
                    templateConfigModel.templateType = templateAttribute.TemplateType;
                    templateConfigModel.fieldName = templateAttribute.AttributeName;

                    string instruction = elementDC["SubLabel"].ToString();
                    string watermark = elementDC.ContainsKey("WatermarkText") ? elementDC["WatermarkText"].ToString() : null;

                    if (!I18nCultureUtil.IsMultiLanguageEnabled || (I18nCultureUtil.IsMultiLanguageEnabled && I18nCultureUtil.IsInMasterLanguage))
                    {
                        templateConfigModel.instruction = instruction;
                        templateConfigModel.waterMark = watermark;
                    }

                    if (I18nCultureUtil.IsMultiLanguageEnabled)
                    {
                        TemplateLayoutConfigI18NModel templateConfigI18NModel = new TemplateLayoutConfigI18NModel();
                        templateConfigI18NModel.auditModel = auditModel;
                        templateConfigI18NModel.langId = I18nCultureUtil.ChangeCulture4WS(I18nCultureUtil.UserPreferredCulture);
                        templateConfigI18NModel.serviceProviderCode = _servProvCode;
                        templateConfigI18NModel.instruction = instruction;
                        templateConfigI18NModel.waterMark = watermark;
                        templateConfigModel.i18NModel = templateConfigI18NModel;
                    }

                    _templateFieldsConfigList.Add(templateConfigModel);
                }
                else
                {
                    // deal with Standard fields
                    XUITextModel xUITextModel = new XUITextModel();
                    xUITextModel.countryCode = regionalCode;
                    xUITextModel.langCode = languageCode;
                    xUITextModel.servProvCode = _servProvCode;
                    xUITextModel.textLevelType = _levelType;
                    xUITextModel.textLevelName = _moduleName;

                    XUITextModel xUITextModelForSub = new XUITextModel();
                    xUITextModelForSub.countryCode = regionalCode;
                    xUITextModelForSub.langCode = languageCode;
                    xUITextModelForSub.textLevelType = _levelType;
                    xUITextModelForSub.textLevelName = _moduleName;
                    xUITextModelForSub.servProvCode = _servProvCode;

                    XUITextModel xUITextModelForWatermark = new XUITextModel();
                    xUITextModelForWatermark.countryCode = regionalCode;
                    xUITextModelForWatermark.langCode = languageCode;
                    xUITextModelForWatermark.textLevelType = _levelType;
                    xUITextModelForWatermark.textLevelName = _moduleName;
                    xUITextModelForWatermark.servProvCode = _servProvCode;
                    
                    // Add two watermarks setting
                    XUITextModel xUITextModelForWatermark1 = new XUITextModel();
                    xUITextModelForWatermark1.countryCode = regionalCode;
                    xUITextModelForWatermark1.langCode = languageCode;
                    xUITextModelForWatermark1.textLevelType = _levelType;
                    xUITextModelForWatermark1.textLevelName = _moduleName;
                    xUITextModelForWatermark1.servProvCode = _servProvCode;

                    XUITextModel xUITextModelForWatermark2 = new XUITextModel();
                    xUITextModelForWatermark2.countryCode = regionalCode;
                    xUITextModelForWatermark2.langCode = languageCode;
                    xUITextModelForWatermark2.textLevelType = _levelType;
                    xUITextModelForWatermark2.textLevelName = _moduleName;
                    xUITextModelForWatermark2.servProvCode = _servProvCode;

                    BizDomainModel4WS tempBizDomainModel = new BizDomainModel4WS();

                    isSub = false;

                    foreach (KeyValuePair<string, object> kvp in elementDC)
                    {
                        switch (kvp.Key)
                        {
                            case "LabelKey":
                                xUITextModel.stringKey = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "Label":
                                xUITextModel.stringValue = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "ModuleName":
                                if (kvp.Value != null &&
                                    kvp.Value.ToString() != string.Empty)
                                {
                                    xUITextModel.textLevelName = kvp.Value.ToString();
                                    xUITextModel.textLevelType = ACAConstant.LEVEL_TYPE_MODULE;
                                }

                                break;
                            case "HTML":
                                xUITextModel.stringValue = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "Restrict":
                                tempBizDomainModel.description = (kvp.Value == null) ? "1" : kvp.Value.ToString();
                                tempBizDomainModel.bizdomain = ACAConstant.ACA_APPLICANT_DISPLAY_RULE;
                                tempBizDomainModel.bizdomainValue = _moduleName;
                                tempBizDomainModel.serviceProviderCode = _servProvCode;
                                break;
                            case "LogoVisible":
                                tempBizDomainModel.description = (kvp.Value.ToString() == "True") ? ACAConstant.COMMON_YES : ACAConstant.COMMON_NO;
                                tempBizDomainModel.bizdomain = BizDomainConstant.STD_CAT_ACA_CONFIGS;
                                tempBizDomainModel.bizdomainValue = BizDomainConstant.STD_ITEM_ACA_LOG_VISIBLE;
                                tempBizDomainModel.serviceProviderCode = _servProvCode;
                                break;
                            case "SubLabelKey":
                                isSub = true;
                                xUITextModelForSub.stringKey = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "SubLabel":
                                xUITextModelForSub.stringValue = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "WatermarkKey":
                                xUITextModelForWatermark.stringKey = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "WatermarkText":
                                xUITextModelForWatermark.stringValue = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "WatermarkKey1":
                                xUITextModelForWatermark1.stringKey = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "WatermarkText1":
                                xUITextModelForWatermark1.stringValue = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "WatermarkKey2":
                                xUITextModelForWatermark2.stringKey = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "WatermarkText2":
                                xUITextModelForWatermark2.stringValue = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                            case "SectionItems":
                                _tempSectionID = elementDC["ControlId"].ToString();
                                GFilterScreenPermissionModel4WS permission = null;
                                if (elementDC.ContainsKey("Permission"))
                                {
                                    Dictionary<string, object> dictPermission = (Dictionary<string, object>)elementDC["Permission"];
                                    permission = new GFilterScreenPermissionModel4WS();
                                    permission.permissionLevel = dictPermission["permissionLevel"].ToString();
                                    permission.permissionValue = dictPermission["permissionValue"].ToString();
                                }

                                MappingCollection2SimpleView((object[])kvp.Value, permission);
                                break;

                            case "TabNames":
                                if ((elementDC["TabNames"] != null) &&
                                    (elementDC["TabNames"].ToString() != string.Empty))
                                {
                                    _tabOrder = elementDC["TabNames"].ToString().Split(new char[] { '|' });
                                }

                                break;

                            case "Type":
                                if ((elementDC["CategoryValue"] != null) && (elementDC["CategoryValue"].ToString() != string.Empty)
                                    && elementDC.ContainsKey("Items") && elementDC["Items"] != null)
                                {
                                    MappingCollection2BizDomainChoices(elementDC["CategoryValue"].ToString(), kvp.Value.ToString(), (object[])elementDC["Items"]);
                                }

                                break;
                            case "ExtenseObjects":
                                UpdateXEntityPermissionModels(elementDC["ExtenseObjects"] as object[]);

                                break;

                            case "EnableSearchRequired":
                                string xpolicyValue = (kvp.Value.ToString() == "True") ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                                HandleXpolicyforEnableSearchRequired(xpolicyValue);

                                break;

                            case "PageFlow":
                                if (kvp.Value != null &&
                                    kvp.Value.ToString() != string.Empty)
                                {
                                    xUITextModel.textLevelName = kvp.Value.ToString();
                                    xUITextModel.textLevelType = ACAConstant.LEVEL_TYPE_PAGEFLOW;
                                }

                                break;

                            case "CollapseLines":
                                string[] pagearray = elementDC["PageId"].ToString().Split(ACAConstant.SPLIT_CHAR);
                                string pageinfo = pagearray[0];
                                XPolicyModel policyModel = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, BizDomainConstant.STD_COLLAPSE_LINES, kvp.Value.ToString(), null, pageinfo);
                                _xPolicyList.Add(policyModel);
                                break;

                            case "Editable":
                                string val = ValidationUtil.IsFalse(kvp.Value.ToString()) ? ACAConstant.COMMON_N : ACAConstant.COMMON_Y;
                                XPolicyModel xpolicyModel = GetXPolicyModel(
                                                                    ACAConstant.LEVEL_TYPE_AGENCY,
                                                                    ConfigManager.AgencyCode,
                                                                    XPolicyConstant.AUTH_AGENT_CUSTOMER_EDITABLE,
                                                                    val,
                                                                    null,
                                                                    XPolicyConstant.AUTH_AGENT_CUSTOMER_EDITABLE);

                                _xPolicyList.Add(xpolicyModel);
                                break;
                        }
                    }

                    if (tempBizDomainModel.description != null)
                    {
                        _standardBizDomainModels.Add(tempBizDomainModel);
                    }

                    if ((!string.IsNullOrEmpty(xUITextModel.stringKey)) &&
                        (xUITextModel.stringValue != null))
                    {
                        _labelModelsList.Add(xUITextModel);
                    }

                    if (isSub && (!string.IsNullOrEmpty(xUITextModelForSub.stringKey)) &&
                        (xUITextModelForSub.stringValue != null))
                    {
                        _labelModelsList.Add(xUITextModelForSub);
                    }

                    if ((!string.IsNullOrEmpty(xUITextModelForWatermark.stringKey)) &&
                        (xUITextModelForWatermark.stringValue != null))
                    {
                        _labelModelsList.Add(xUITextModelForWatermark);
                    }

                    if ((!string.IsNullOrEmpty(xUITextModelForWatermark1.stringKey)) && (xUITextModelForWatermark1.stringValue != null))
                    {
                        _labelModelsList.Add(xUITextModelForWatermark1);
                    }

                    if ((!string.IsNullOrEmpty(xUITextModelForWatermark2.stringKey)) && (xUITextModelForWatermark2.stringValue != null))
                    {
                        _labelModelsList.Add(xUITextModelForWatermark2);
                    }
                }
            }
        }

        /// <summary>
        /// Handle XPolicy for License Broad
        /// </summary>
        /// <param name="xpolicyValue">data for element</param>
        private void HandleXpolicyforEnableSearchRequired(string xpolicyValue)
        {
            XPolicyModel policyModel = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_LICENSINGBOARD_REQUIRED, xpolicyValue);

            _xPolicyList.Add(policyModel);
        }

        /// <summary>
        /// Mapping collection to head with.
        /// </summary>
        /// <param name="elementCollection">The element collection.</param>
        private void MappingCollection2HeadWith(object[] elementCollection)
        {
            if (elementCollection == null)
            {
                return;
            }

            Dictionary<string, GFilterScreenPermissionModel4WS> dictPermission = new Dictionary<string, GFilterScreenPermissionModel4WS>();
            Dictionary<string, List<SimpleViewElementModel4WS>> dictViewElementModel = new Dictionary<string, List<SimpleViewElementModel4WS>>();

            foreach (object element in elementCollection)
            {
                SimpleViewElementModel4WS simpleViewElementModel = new SimpleViewElementModel4WS();
                simpleViewElementModel.servProvCode = _servProvCode;
                string sectionId = string.Empty;

                foreach (KeyValuePair<string, object> kvp in (Dictionary<string, object>)element)
                {
                    switch (kvp.Key)
                    {
                        case "Width":
                            string strWidth = (string)kvp.Value;
                            if (!string.IsNullOrEmpty(strWidth))
                            {
                                strWidth = strWidth.Replace("px", string.Empty);
                                int width;
                                bool isInt = int.TryParse(strWidth, out width);
                                if (isInt)
                                {
                                    simpleViewElementModel.elementWidth = width;
                                }
                            }

                            break;
                        case "GridViewId":
                            sectionId = kvp.Value.ToString();
                            break;
                        case "ViewElementName":
                            simpleViewElementModel.viewElementName = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                    }
                }

                // get the permission list
                Dictionary<string, object> elementDC = (Dictionary<string, object>)element;
                if (elementDC.ContainsKey("Permission"))
                {
                    Dictionary<string, object> tempPermission = (Dictionary<string, object>)elementDC["Permission"];
                    GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS();
                    permission.permissionLevel = tempPermission["permissionLevel"].ToString();
                    permission.permissionValue = tempPermission["permissionValue"].ToString();

                    if (!dictPermission.ContainsKey(sectionId))
                    {
                        dictPermission.Add(sectionId, permission);
                    }
                }

                // get the simple view element model list
                if (dictViewElementModel.ContainsKey(sectionId))
                {
                    dictViewElementModel[sectionId].Add(simpleViewElementModel);
                }
                else
                {
                    List<SimpleViewElementModel4WS> models = new List<SimpleViewElementModel4WS> { simpleViewElementModel };
                    dictViewElementModel.Add(sectionId, models);
                }

                // get the simple view model that union with permission and simple view element model.
                _gridViewHeadWidthList = GenerateSimpleViewModel(dictViewElementModel, dictPermission);
            }
        }

        /// <summary>
        /// Generates the simple view model.
        /// </summary>
        /// <param name="dictViewElementModel">The dictionary view element model.</param>
        /// <param name="dictPermission">The dictionary permission.</param>
        /// <returns>Return the SimpleViewModel.</returns>
        private Dictionary<string, SimpleViewModel4WS> GenerateSimpleViewModel(Dictionary<string, List<SimpleViewElementModel4WS>> dictViewElementModel, Dictionary<string, GFilterScreenPermissionModel4WS> dictPermission)
        {
            Dictionary<string, SimpleViewModel4WS> dictViewModel = new Dictionary<string, SimpleViewModel4WS>();

            foreach (string sectionId in dictViewElementModel.Keys)
            {
                SimpleViewModel4WS viewModel = new SimpleViewModel4WS();
                viewModel.sectionID = sectionId;
                viewModel.simpleViewElements = dictViewElementModel[sectionId].ToArray();

                if (dictPermission.ContainsKey(sectionId))
                {
                    viewModel.permissionModel = dictPermission[sectionId];
                }

                dictViewModel.Add(sectionId, viewModel);
            }

            return dictViewModel;
        }

        /// <summary>
        /// Mapping collection to simple view.
        /// </summary>
        /// <param name="elementCollection">The element collection.</param>
        /// <param name="permission">The permission.</param>
        private void MappingCollection2SimpleView(object[] elementCollection, GFilterScreenPermissionModel4WS permission)
        {
            if (elementCollection == null)
            {
                return;
            }

            Dictionary<string, object> simpleViewElementDC;

            int i = 0;
            foreach (object element in elementCollection)
            {
                SimpleViewElementModel4WS elementModel = new SimpleViewElementModel4WS();
                string controlPrefix = string.Empty;
                simpleViewElementDC = (Dictionary<string, object>)element;
                foreach (KeyValuePair<string, object> kvp in simpleViewElementDC)
                {
                    switch (kvp.Key)
                    {
                        case "Label":
                            elementModel.labelValue = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "Visible":
                            elementModel.recStatus = (kvp.Value.ToString() == "True") ? "A" : "I";
                            break;
                        case "Required":
                            elementModel.required = (kvp.Value.ToString() == "True") ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                            break;
                        case "Editable":
                            elementModel.isEditable = (kvp.Value.ToString() == "True") ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                            break;
                        case "servProvCode":
                            elementModel.servProvCode = _servProvCode;
                            break;
                        case "m_viewElementNameField":
                            elementModel.viewElementId = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "OriginalElementName":
                            elementModel.viewElementName = (kvp.Value == null) ? null : ScriptFilter.DecodeJson(kvp.Value.ToString());
                            break;
                        case "Order":
                            elementModel.displayOrder = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "Left":
                            elementModel.elementLeft = int.Parse(kvp.Value.ToString());
                            break;
                        case "Top":
                            elementModel.elementTop = int.Parse(kvp.Value.ToString());
                            break;
                        case "Width":
                            elementModel.elementWidth = int.Parse(kvp.Value.ToString());
                            break;
                        case "LabelWidth":
                            elementModel.labelWidth = int.Parse(kvp.Value.ToString());
                            break;
                        case "InputWidth":
                            elementModel.inputWidth = int.Parse(kvp.Value.ToString());
                            break;
                        case "UnitWidth":
                            elementModel.unitWidth = int.Parse(kvp.Value.ToString());
                            break;
                        case "Height":
                            elementModel.elementHeight = int.Parse(kvp.Value.ToString());
                            break;
                        case "ViewElementId":
                            elementModel.viewElementId = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "Standard":
                            elementModel.standard = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "ElementType":
                            elementModel.elementType = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "LabelId":
                            elementModel.labelId = (kvp.Value == null) ? null : kvp.Value.ToString();
                            break;
                        case "ControlPrefix":
                            controlPrefix = kvp.Value.ToString();
                            break;
                        case "OldStatus":
                            elementModel.OldStatus = (kvp.Value.ToString() == "A") ? "A" : "I";
                            break;
                    }
                }

                // If it is template field, NOT save the labelId to database, 
                // because the labelId is the default language value, the string length may be great than the defined in database.
                if (ValidationUtil.IsNo(elementModel.standard))
                {
                    elementModel.labelId = string.Empty;
                }

                if (!string.IsNullOrEmpty(controlPrefix))
                {
                    elementModel.viewElementName = elementModel.viewElementName.Replace(controlPrefix, string.Empty);
                }

                if ((!string.IsNullOrEmpty(elementModel.viewElementName)) &&
                    (!string.IsNullOrEmpty(elementModel.recStatus)))
                {
                    UpdateSectionFieldsModelList(elementModel, elementCollection.Length, i++, permission);
                }
            }
        }

        /// <summary>
        /// Mapping the collection to biz domain choices.
        /// </summary>
        /// <param name="categoryValue">The category.</param>
        /// <param name="bizdomainType">The bizDomain type.</param>
        /// <param name="elementCollection">The element collection</param>
        private void MappingCollection2BizDomainChoices(string categoryValue, string bizdomainType, object[] elementCollection)
        {
            // deal with Cap Type configuration
            if (categoryValue == "CapTypeConfiguration")
            {
                Dictionary<string, object> capTypeDC;
                foreach (object element in elementCollection)
                {
                    CapTypeModel tempCapTypeModel = new CapTypeModel();
                    capTypeDC = (Dictionary<string, object>)element;

                    foreach (KeyValuePair<string, object> kvp in capTypeDC)
                    {
                        switch (kvp.Key)
                        {
                            case "Label":
                                tempCapTypeModel.alias = (kvp.Value == null) ? null : kvp.Value.ToString();
                                break;
                        }
                    }

                    tempCapTypeModel.serviceProviderCode = _selectedAgency;
                    _capTypeModelList.Add(tempCapTypeModel);
                }
            }
            else
            {
                // deal with BizDomain
                if (elementCollection.Length < 1)
                {
                    BizDomainModel4WS tempBizDomainModel = new BizDomainModel4WS();
                    tempBizDomainModel.bizdomain = categoryValue;
                    _standardChoiceArrayList.Add(tempBizDomainModel);
                    return;
                }

                foreach (object element in elementCollection)
                {
                    // for module level items
                    if (bizdomainType == "HardCode")
                    {
                        if (categoryValue == ACAConstant.EDUCATION_LOOKUP)
                        {
                            HandleXpolicyforEduSearchType(element);
                        }
                        else
                        {
                            HandleModuleBizDomain(categoryValue, element);
                        }
                    }
                    else if (bizdomainType == "PaymentHardCode")
                    {
                        HandleModuleBizDomain(categoryValue, element);
                    }
                    else if (bizdomainType == "STDandXPolicy")
                    {
                        /* only search by contact in CAP home page.
                         * The check box value add to xpolicy array.
                         */ 
                        HandleXpolicyforContactType(element);

                        //The drop down list text value add to standard choice.
                        HandleGlobalBizDomain(categoryValue, element, bizdomainType);
                    }
                    else
                    {
                        HandleGlobalBizDomain(categoryValue, element, bizdomainType);
                    }
                }
            }
        }

        /// <summary>
        /// This method is get module level bizDomain model.
        /// </summary>
        /// <param name="element">The element.</param>
        private void HandleXpolicyforEduSearchType(object element)
        {
            XPolicyModel policyModel = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.EDUCATION_LOOKUP, string.Empty);
            XPolicyModel policyModelI18N = ObjectCloneUtil.DeepCopy(policyModel);

            Dictionary<string, object> bizdomainElementDC = (Dictionary<string, object>)element;
            foreach (KeyValuePair<string, object> kvp in bizdomainElementDC)
            {
                switch (kvp.Key)
                {
                    case "Visible":
                        policyModel.status = policyModelI18N.status = (kvp.Value.ToString() == "True") ? "A" : "I";
                        break;
                    case "Label":
                        string item = (kvp.Value == null) ? string.Empty : kvp.Value.ToString();
                        string[] subStr = System.Text.RegularExpressions.Regex.Split(item, "\\|\\|");

                        if (subStr != null && subStr.Length == 3)
                        {
                            //I18n evvironment.
                            policyModel.data2 = I18nCultureUtil.IsInMasterLanguage ? subStr[1] : subStr[0];
                            policyModelI18N.data2 = subStr[1];
                            policyModel.data4 = policyModelI18N.data4 = subStr[2];
                        }
                        else if (subStr != null && subStr.Length == 2)
                        {
                            policyModel.data2 = policyModelI18N.data2 = subStr[0];
                            policyModel.data4 = policyModelI18N.data4 = subStr[1];
                        }
                        else
                        {
                            policyModel.data2 = policyModelI18N.data2 = string.Empty;
                        }

                        policyModel.rightGranted = policyModelI18N.rightGranted = ACAConstant.GRANTED_RIGHT;
                        break;
                }
            }

            _xPolicyListI18N.Add(policyModelI18N);
            _xPolicyList.Add(policyModel);
        }

        /// <summary>
        /// This method is get module level bizDomain model.
        /// </summary>
        /// <param name="categoryValue">The category value.</param>
        /// <param name="element">The element object.</param>
        private void HandleModuleBizDomain(string categoryValue, object element)
        {
            XPolicyModel policyModel = new XPolicyModel();
            policyModel.policyName = categoryValue; // label key

            Dictionary<string, object> bizdomainElementDC = (Dictionary<string, object>)element;
            foreach (KeyValuePair<string, object> kvp in bizdomainElementDC)
            {
                switch (kvp.Key)
                {
                    case "Visible":
                        policyModel.recStatus = (kvp.Value.ToString() == "True") ? ACAConstant.VALID_STATUS : ACAConstant.INVALID_STATUS;
                        break;
                    case "Label":
                        string item = (kvp.Value == null) ? string.Empty : kvp.Value.ToString();
                        int index = item.IndexOf("||");
                        if (index != -1)
                        {
                            // support I18N (item= bizDomainValue+"||"+resBizDomainValue)
                            policyModel.data2 = item.Substring(0, index);
                            policyModel.resData2 = item.Substring(index + 2);
                        }
                        else
                        {
                            policyModel.data2 = (kvp.Value == null) ? null : item;
                        }

                        policyModel.level = _levelType;
                        policyModel.levelData = _moduleName;
                        policyModel.rightGranted = ACAConstant.GRANTED_RIGHT;
                        break;
                    case "OldLabel":
                        policyModel.data1 = (kvp.Value == null) ? null : kvp.Value.ToString(); // data1 is description
                        break;
                }
            }

            _policyStandardChoiceList.Add(policyModel);
        }

        /// <summary>
        /// Handle XPolicy for Contact Type
        /// </summary>
        /// <param name="element">data for element</param>
        private void HandleXpolicyforContactType(object element)
        {
            XPolicyModel policyModel = null;
            Dictionary<string, object> bizdomainElementDC = (Dictionary<string, object>)element;

            if (bizdomainElementDC.Where(o => o.Value.ToString().EndsWith(XPolicyConstant.CONTACT_TYPE_REGISTERATION_CLERK + "||")).Any())
            {
                policyModel = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.CONTACT_TYPE_REGISTERATION_CLERK, string.Empty, string.Empty, string.Empty);
            }
            else if (bizdomainElementDC.Where(o => o.Value.ToString().EndsWith(XPolicyConstant.CONTACT_TYPE_REGISTERATION + "||")).Any())
            {
                policyModel = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.CONTACT_TYPE_REGISTERATION, string.Empty, string.Empty, string.Empty);
            }
            else if (bizdomainElementDC.Where(o => o.Value.ToString().EndsWith(XPolicyConstant.CONTACT_TYPE_ASSOCICATION + "||")).Any())
            {
                policyModel = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.CONTACT_TYPE_ASSOCICATION, string.Empty, string.Empty, string.Empty);
            }
            else if (bizdomainElementDC.Where(o => o.Value.ToString().EndsWith(XPolicyConstant.CONTACT_TYPE_CUSTOMERDETAIL + "||")).Any())
            {
                policyModel = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.CONTACT_TYPE_CUSTOMERDETAIL, string.Empty, string.Empty, string.Empty);
            }
            else if (bizdomainElementDC.Where(o => o.Value.ToString().EndsWith(XPolicyConstant.RECORD_STATUS_SEARCH_FOR_TRADENAME + "||")).Any())
            {
                policyModel = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, _moduleName, XPolicyConstant.RECORD_STATUS_SEARCH_FOR_TRADENAME, string.Empty, string.Empty, string.Empty);
            }
            else
            {
                policyModel = ConstructContactTypeXPolicyModel(_moduleName);
            }

            foreach (KeyValuePair<string, object> kvp in bizdomainElementDC)
            {
                switch (kvp.Key)
                {
                    case "Visible":
                        string value = ACAConstant.COMMON_TRUE.Equals(kvp.Value.ToString(), StringComparison.OrdinalIgnoreCase) ? ACAConstant.COMMON_F : ACAConstant.COMMON_N;

                        if (XPolicyConstant.CONTACT_TYPE_REGISTERATION_CLERK.Equals(policyModel.data1)
                            || XPolicyConstant.CONTACT_TYPE_REGISTERATION.Equals(policyModel.data1)
                            || XPolicyConstant.CONTACT_TYPE_CUSTOMERDETAIL.Equals(policyModel.data1)
                            || XPolicyConstant.CONTACT_TYPE_ASSOCICATION.Equals(policyModel.data1)
                            || XPolicyConstant.RECORD_STATUS_SEARCH_FOR_TRADENAME.Equals(policyModel.data1))
                        {
                            policyModel.data2 = value;
                        }
                        else
                        {
                            policyModel.rightGranted = value;
                        }

                        break;
                    case "Label":
                        string item = (kvp.Value == null) ? string.Empty : kvp.Value.ToString();

                        if (item.IndexOf("||") == -1)
                        {
                            break;
                        }

                        string[] subLabel = System.Text.RegularExpressions.Regex.Split(item, "\\|\\|");
                        string contactType = subLabel.Length > 2 && subLabel[1] != null ? subLabel[1] : string.Empty;

                        if (XPolicyConstant.CONTACT_TYPE_REGISTERATION_CLERK.Equals(policyModel.data1)
                            || XPolicyConstant.CONTACT_TYPE_REGISTERATION.Equals(policyModel.data1)
                            || XPolicyConstant.CONTACT_TYPE_CUSTOMERDETAIL.Equals(policyModel.data1)
                            || XPolicyConstant.CONTACT_TYPE_ASSOCICATION.Equals(policyModel.data1)
                            || XPolicyConstant.RECORD_STATUS_SEARCH_FOR_TRADENAME.Equals(policyModel.data1))
                        {
                            policyModel.data4 = contactType;
                        }
                        else
                        {
                            policyModel.data1 = contactType;
                        }

                        break;
                }
            }

            _xPolicyList.Add(policyModel);
        }

        /// <summary>
        /// This method is get global level bizDomain model.
        /// </summary>
        /// <param name="categoryValue">category Value</param>
        /// <param name="element">the element value</param>
        /// <param name="bizdomainType">the bizDomain value.</param>
        private void HandleGlobalBizDomain(string categoryValue, object element, string bizdomainType)
        {
            BizDomainModel4WS tempBizDomainModel = new BizDomainModel4WS();
            tempBizDomainModel.bizdomain = categoryValue;

            Dictionary<string, object> bizdomainElementDC = (Dictionary<string, object>)element;
            foreach (KeyValuePair<string, object> kvp in bizdomainElementDC)
            {
                switch (kvp.Key)
                {
                    case "Visible":
                        tempBizDomainModel.auditStatus = (kvp.Value.ToString() == "True" || DropDownListDataSourceType.STDandXPolicy.ToString().Equals(bizdomainType)) ? "A" : "I";
                        break;
                    case "Label":
                        if (kvp.Value != null)
                        {
                            SetBizDomainValue(kvp.Value.ToString(), categoryValue, tempBizDomainModel);
                        }

                        break;
                    case "OldLabel":
                        tempBizDomainModel.description = (kvp.Value == null) ? null : kvp.Value.ToString();
                        break;
                }
            }

            if ((!string.IsNullOrEmpty(tempBizDomainModel.auditStatus)) &&
                (!string.IsNullOrEmpty(tempBizDomainModel.bizdomainValue)))
            {
                _standardChoiceArrayList.Add(tempBizDomainModel);
            }
        }

        /// <summary>
        /// update bizDomainValue
        /// </summary>
        /// <param name="bizDomainValue">The biz domain value.</param>
        /// <param name="categoryValue">The category value.</param>
        /// <param name="tempBizDomainModel">The temp biz domain model.</param>
        private void SetBizDomainValue(string bizDomainValue, string categoryValue, BizDomainModel4WS tempBizDomainModel)
        {
            if (string.IsNullOrEmpty(bizDomainValue))
            {
                return;
            }

            string[] values = System.Text.RegularExpressions.Regex.Split(bizDomainValue, "\\|\\|");

            if (I18nCultureUtil.IsMultiLanguageEnabled)
            {
                // support multiple language
                tempBizDomainModel.bizdomainValue = values[1].Trim();
                tempBizDomainModel.resBizdomainValue = values[2].Trim();

                if (values[0] == "1" ||
                    values[0] == "2")
                {
                    // values[0] is show type. 1: show description; 2: show value and description
                    tempBizDomainModel.description = values[3].Trim();
                    tempBizDomainModel.resDescription = values[4].Trim();
                }
            }
            else
            {
                tempBizDomainModel.bizdomainValue = values[1].Trim();

                if (values[0] == "1" ||
                    values[0] == "2")
                {
                    tempBizDomainModel.description = values[2].Trim();
                }
            }
        }

        /// <summary>
        /// Update section fields model list.
        /// </summary>
        /// <param name="viewElement">The SimpleViewElementModel4WS.</param>
        /// <param name="count">The count.</param>
        /// <param name="index">The index.</param>
        /// <param name="permission">The permission.</param>
        private void UpdateSectionFieldsModelList(SimpleViewElementModel4WS viewElement, int count, int index, GFilterScreenPermissionModel4WS permission)
        {
            viewElement.sectionID = _tempSectionID;
            int currentSectionModelIndex = -1;
            if (permission == null)
            {
                currentSectionModelIndex = _sectionFieldsModelList.FindIndex(FindSectionFieldsModelBySectionID);
            }
            else
            {
                currentSectionModelIndex = _sectionFieldsModelList.FindIndex(f => f.sectionID == _tempSectionID && f.permissionModel.permissionValue == permission.permissionValue);
            }

            if (currentSectionModelIndex >= 0)
            {
                _sectionFieldsModelList[currentSectionModelIndex].simpleViewElements[index] = viewElement;
            }
            else
            {
                SimpleViewModel4WS newSectionFieldsModel = new SimpleViewModel4WS();
                if (permission != null && !string.IsNullOrEmpty(permission.permissionLevel))
                {
                    GFilterScreenPermissionModel4WS clonePermissionModel = ControlBuildHelper.GetPermissionWithGenericTemplate(_tempSectionID, permission.permissionLevel, permission.permissionValue);
                    IGviewBll gviewBll = ObjectFactory.GetObject(typeof(IGviewBll)) as IGviewBll;
                    SimpleViewModel4WS viewModel = gviewBll.GetSimpleViewModel(
                        ConfigManager.AgencyCode,
                        _moduleName,
                        _tempSectionID,
                        clonePermissionModel.permissionLevel,
                        clonePermissionModel.permissionValue,
                        ACAConstant.ADMIN_CALLER_ID);
                    newSectionFieldsModel.labelLayoutType = viewModel.labelLayoutType;
                }

                newSectionFieldsModel.simpleViewElements = new SimpleViewElementModel4WS[count];
                newSectionFieldsModel.simpleViewElements[index] = viewElement;
                newSectionFieldsModel.sectionID = _tempSectionID;
                if (permission != null)
                {
                    newSectionFieldsModel.permissionModel = permission;
                }

                _sectionFieldsModelList.Add(newSectionFieldsModel);
            }
        }

        /// <summary>
        /// Find section fields model by section id.
        /// </summary>
        /// <param name="sectionModel">The section model.</param>
        /// <returns>The section fields.</returns>
        private bool FindSectionFieldsModelBySectionID(SimpleViewModel4WS sectionModel)
        {
            if (sectionModel.sectionID == _tempSectionID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create the amendment tree node list that format as JSON.
        /// </summary>
        /// <param name="amendmentSettingModule">The amendment setting model.</param>
        /// <returns>The amendment tree node list that format as JSON.</returns>
        private string CreateAmendmentTreeNodeListJson(ButtonSettingModel4WS amendmentSettingModule)
        {
            CapTypeModel[] availableCapTypeList = amendmentSettingModule.availableCapTypeList;
            CapTypeModel[] selectedCapTypeList = amendmentSettingModule.selectedCapTypeList;

            int index = 0;
            StringBuilder bufAll = new StringBuilder();
            StringBuilder bufAvailable = new StringBuilder();
            StringBuilder bufSelected = new StringBuilder();

            if (availableCapTypeList != null)
            {
                foreach (CapTypeModel capTypeModel in availableCapTypeList)
                {
                    string nodeJson = CreateAmendmentTreeNodeJson(capTypeModel, index);
                    bufAvailable.Append(nodeJson);
                    index++;
                }
            }

            if (selectedCapTypeList != null)
            {
                foreach (CapTypeModel capTypeModel in selectedCapTypeList)
                {
                    string nodeJson = CreateAmendmentTreeNodeJson(capTypeModel, index);
                    bufSelected.Append(nodeJson.ToString());
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

            bufAll.Append("{'availableCaps':[");
            bufAll.Append(bufAvailable.ToString());
            bufAll.Append("],");
            bufAll.Append("'selectedCaps':[");
            bufAll.Append(bufSelected.ToString());
            bufAll.Append("]");
            bufAll.Append("}");

            return bufAll.ToString();
        }

        /// <summary>
        /// get BizDomainModel4WS[] for register license.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <returns>The BizDomainModel4WS.</returns>
        private BizDomainModel4WS GetRequireLicenseBizModel(object[] elementsCollection)
        {
            if (elementsCollection == null ||
                elementsCollection.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsCollection[0];

            BizDomainModel4WS bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_REGISTRATION_LICENSE_ENABLED;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            if (bizModel == null)
            {
                bizModel = GetConfigsBizDomainModel();
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_REGISTRATION_LICENSE_ENABLED;
            }

            bool isRequireLicense = dictList["chkRequireLisOption"] != null && ACAConstant.COMMON_TRUE.Equals(dictList["chkRequireLisOption"].ToString(), StringComparison.InvariantCultureIgnoreCase);

            bizModel.description = isRequireLicense ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            bizModel.auditStatus = isRequireLicense ? ACAConstant.VALID_STATUS : ACAConstant.INVALID_STATUS;

            return bizModel;
        }

        /// <summary>
        /// update EMSE event and script association.
        /// </summary>
        /// <param name="elementsCollection">The element collection.</param>
        private void UpdateEventScriptCode(object[] elementsCollection)
        {
            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsCollection[0];
            string scriptName = dictList["ddlScriptName"].ToString();

            IEMSEBll emseBll = (IEMSEBll)ObjectFactory.GetObject(typeof(IEMSEBll));
            emseBll.CreateOrUpdateEventScriptCode(ACAConstant.FEE_ESTIMATE_AFTER4ACA, scriptName, ADMIN_AUDIT_ID);
        }

        /// <summary>
        /// update CSS style content
        /// </summary>
        /// <param name="elementCollection">The element collection.</param>
        private void UpdateCssContent(object[] elementCollection)
        {
            Dictionary<string, object> dictList = (Dictionary<string, object>)elementCollection[0];

            if (dictList["customizedCss"] != null)
            {
                string cssContent = dictList["customizedCss"].ToString();
                string labelKey = "aca_css_customizedstyle";
                string levelType = ACAConstant.LEVEL_TYPE_AGENCY;
                string levelName = _servProvCode;
                string stringvalue = "nbsp;";
                SaveLabel(labelKey, stringvalue, cssContent, _servProvCode, levelType, levelName);
            }
        }

        /// <summary>
        /// Get GIS model from standard choice
        /// </summary>
        /// <param name="elementsColletion">newest value about GIS</param>
        /// <returns>The GIS model.</returns>
        private BizDomainModel4WS GetGisModel(object[] elementsColletion)
        {
            if (elementsColletion == null ||
                elementsColletion.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];
            string template = dictList["rdlTemplateType"].ToString();
            string bizdomainValue = BizDomainConstant.STD_ITEM_GIS_PORLET_URL;
            string description = dictList["txtGIS"].ToString();

            if (ValidationUtil.IsTrue(template))
            {
                bizdomainValue = BizDomainConstant.STD_ITEM_NEW_GIS_PORLET_URL;
                description = dictList["txtNewGIS"].ToString();
            }

            BizDomainModel4WS tempbizModel = GetConfigsBizDomainModel();
            tempbizModel.bizdomainValue = bizdomainValue;

            BizDomainModel4WS bizModel = _bizBll.GetBizDomainListByModel(tempbizModel, ADMIN_AUDIT_ID, true);
            if (bizModel == null)
            {
                bizModel = tempbizModel;
                bizModel.description = description;
                bizModel.auditStatus = ACAConstant.INVALID_STATUS;
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.bizdomainValue = bizdomainValue;
            }
            else
            {
                bizModel.description = description;
                if (ValidationUtil.IsTrue(dictList["chkGIS"].ToString()))
                {
                    bizModel.auditStatus = ACAConstant.VALID_STATUS;
                }
                else
                {
                    bizModel.auditStatus = ACAConstant.INVALID_STATUS;
                }
            }

            return bizModel;
        }

        /// <summary>
        /// This method is get Country Code bizDomain model.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <returns>The country code bizDomain model.</returns>
        private BizDomainModel4WS GetCountryCodeModel(object[] elementsCollection)
        {
            if (elementsCollection == null || elementsCollection.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsCollection[0];

            BizDomainModel4WS bizModelTemp;
            BizDomainModel4WS bizModel;
            string[] bizModelValue;

            //get bizdomain model from Rbizdomain_value table
            bizModelValue = _bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_PHONE_NUMBER_IDD_ENABLE);
            bizModelTemp = GetCountryCodeBizDomainModel();

            if (bizModelValue == null)
            {
                bizModel = _bizBll.GetBizDomainListByModel(bizModelTemp, ACAConstant.ADMIN_CALLER_ID);
                if (bizModel == null)
                {
                    bizModel = bizModelTemp;
                }

                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.auditStatus = ACAConstant.VALID_STATUS;
                bizModel.bizdomainValue = ACAConstant.COMMON_No;
            }
            else
            {
                bizModel = _bizBll.GetBizDomainListByModel(bizModelTemp, bizModelValue[0], ACAConstant.ADMIN_CALLER_ID);
                if (bizModel == null || bizModel.bizdomain == null ||
                    bizModel.bizdomain == string.Empty)
                {
                    bizModel = _bizBll.GetBizDomainListByModel(bizModelTemp, ACAConstant.ADMIN_CALLER_ID);
                    if (bizModel == null)
                    {
                        bizModel = bizModelTemp;
                    }

                    bizModel.auditID = ADMIN_AUDIT_ID;
                    bizModel.auditStatus = ACAConstant.VALID_STATUS;
                    bizModel.bizdomainValue = ACAConstant.COMMON_No;
                }
            }

            if (dictList["chkCountryCode"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                bizModel.bizdomainValue = ACAConstant.COMMON_Yes;
            }
            else
            {
                bizModel.bizdomainValue = ACAConstant.COMMON_No;
            }

            return bizModel;
        }

        /// <summary>
        /// This method is get official web site bizDomain model.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <returns>The official web site bizDomain model.</returns>
        private BizDomainModel4WS GetOfficialWebSiteModel(object[] elementsCollection)
        {
            if (elementsCollection == null ||
                elementsCollection.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsCollection[0];
            BizDomainModel4WS tempbizModel = GetConfigsBizDomainModel();
            tempbizModel.bizdomainValue = BizDomainConstant.STD_ITEM_OFFICIAL_WEBSITE_URL;

            BizDomainModel4WS bizModel = _bizBll.GetBizDomainListByModel(tempbizModel, BizDomainConstant.STD_ITEM_OFFICIAL_WEBSITE_URL, ADMIN_AUDIT_ID);

            if (bizModel == null)
            {
                //create the biz domain for official web site
                bizModel = tempbizModel;
                bizModel.description = dictList["txtOfficialWebSite"].ToString();
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.auditStatus = ACAConstant.VALID_STATUS;
                bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_OFFICIAL_WEBSITE_URL;
            }
            else
            {
                //update the official web site url
                bizModel.description = dictList["txtOfficialWebSite"].ToString();
                bizModel.auditStatus = ACAConstant.VALID_STATUS;
            }

            return bizModel;
        }

        /// <summary>
        /// Get authentication by security question setting models.
        /// </summary>
        /// <param name="elementsColletion">The elements collection.</param>
        /// <returns>The BizDomain model list.</returns>
        private IList<BizDomainModel4WS> GetAuthBySecurityQuestionSettingModels(object[] elementsColletion)
        {
            if (elementsColletion == null || elementsColletion.Length <= 0)
            {
                return null;
            }

            IList<BizDomainModel4WS> result = new List<BizDomainModel4WS>();
            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];
            dictList = (Dictionary<string, object>)elementsColletion[0];
            string isEnableSecurityQuestion = ValidationUtil.IsTrue(dictList["chkSecurityQuestion"].ToString())
                                              ? ACAConstant.COMMON_YES
                                              : ACAConstant.COMMON_NO;

            result.Add(new BizDomainModel4WS()
            {
                serviceProviderCode = ConfigManager.AgencyCode,
                bizdomain = BizDomainConstant.STD_AUTHENTICATION_BY_SECURITY_QUESTION,
                bizdomainValue = BizDomainConstant.STD_ITEM_ENABLE_AUTHENTICATION_BY_SECURITY_QUESTION,
                description = isEnableSecurityQuestion,
                auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now),
                auditID = ACAConstant.ADMIN_CALLER_ID,
                auditStatus = ACAConstant.VALID_STATUS
            });

            result.Add(new BizDomainModel4WS()
            {
                serviceProviderCode = ConfigManager.AgencyCode,
                bizdomain = BizDomainConstant.STD_AUTHENTICATION_BY_SECURITY_QUESTION,
                bizdomainValue = BizDomainConstant.STD_ITEM_COMPULSORY_SECURITY_QUESTION_QUANTITY,
                description = dictList["txtSecurityQuestionQuantity"].ToString(),
                auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now),
                auditID = ACAConstant.ADMIN_CALLER_ID,
                auditStatus = ACAConstant.VALID_STATUS
            });
            
            return result;
        }

        /// <summary>
        /// This method is get display user initial bizDomain model.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <returns>The user initial bizDomain model.</returns>
        private BizDomainModel4WS GetUserInitialDisplayModel(object[] elementsCollection)
        {
            if (elementsCollection == null ||
                elementsCollection.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsCollection[0];
            BizDomainModel4WS tempbizModel = GetConfigsBizDomainModel();
            tempbizModel.bizdomainValue = BizDomainConstant.STD_ITEM_DISPLAY_USER_INITIALS;

            BizDomainModel4WS bizModel = _bizBll.GetBizDomainListByModel(tempbizModel, BizDomainConstant.STD_ITEM_DISPLAY_USER_INITIALS, ADMIN_AUDIT_ID);

            if (bizModel == null)
            {
                //create the biz domain for the user initial display info
                bizModel = tempbizModel;
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_DISPLAY_USER_INITIALS;
            }

            if (dictList["isDisplayUserInitial"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                bizModel.description = ACAConstant.COMMON_YES;
            }
            else
            {
                bizModel.description = ACAConstant.COMMON_NO;
            }

            bizModel.auditStatus = ACAConstant.VALID_STATUS;

            return bizModel;
        }

        /// <summary>
        /// This method is get Interval bizDomain model.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <returns>The interval bizDomain model.</returns>
        private BizDomainModel4WS GetIntervalModel(object[] elementsCollection)
        {
            if (elementsCollection == null ||
                elementsCollection.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsCollection[0];
            BizDomainModel4WS tempbizModel = GetConfigsBizDomainModel();
            tempbizModel.bizdomainValue = BizDomainConstant.STD_ITEM_PURGE_EXPIRED_ACCOUNT_INTERVAL;

            BizDomainModel4WS bizModel = _bizBll.GetBizDomainListByModel(tempbizModel, ADMIN_AUDIT_ID, true);

            if (bizModel == null)
            {
                bizModel = GetConfigsBizDomainModel();
                bizModel.description = dictList["txtInterval"].ToString();
                bizModel.auditStatus = ACAConstant.INVALID_STATUS;
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_PURGE_EXPIRED_ACCOUNT_INTERVAL;
            }
            else
            {
                bizModel.description = dictList["txtInterval"].ToString();
                if (dictList["chkInterval"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    bizModel.auditStatus = ACAConstant.VALID_STATUS;
                }
                else
                {
                    bizModel.auditStatus = ACAConstant.INVALID_STATUS;
                }
            }

            return bizModel;
        }

        /// <summary>
        /// Get data model for account verification e-mail setting.
        /// </summary>
        /// <param name="elementsColletion">Javascript data object from client.</param>
        /// <returns>BizDomain model</returns>
        private BizDomainModel4WS GetAccountVerificationMailModel(object[] elementsColletion)
        {
            if (elementsColletion == null ||
                elementsColletion.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];
            BizDomainModel4WS tempbizModel = GetConfigsBizDomainModel();
            tempbizModel.bizdomainValue = BizDomainConstant.STD_ITEM_ENABLE_ACCOUNT_VERIFICATION_EMAIL;

            BizDomainModel4WS bizModel = _bizBll.GetBizDomainListByModel(tempbizModel, ADMIN_AUDIT_ID, true);

            if (bizModel == null)
            {
                bizModel = GetConfigsBizDomainModel();
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_ENABLE_ACCOUNT_VERIFICATION_EMAIL;
            }

            if (dictList["chkEnableAutoActivation"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                bizModel.description = ACAConstant.COMMON_No;
            }
            else
            {
                bizModel.description = ACAConstant.COMMON_Yes;
            }

            bizModel.auditStatus = ACAConstant.VALID_STATUS;

            return bizModel;
        }

        /// <summary>
        /// This method is get Export bizDomain model.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <returns>The export bizDomain model.</returns>
        private BizDomainModel4WS GetExportModel(object[] elementsCollection)
        {
            if (elementsCollection == null ||
                elementsCollection.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsCollection[0];
            BizDomainModel4WS tempbizModel = GetConfigsBizDomainModel();
            tempbizModel.bizdomainValue = BizDomainConstant.STD_ITEM_ALLOW_EXPORTING_TO_CSV;

            BizDomainModel4WS bizModel = _bizBll.GetBizDomainListByModel(tempbizModel, ADMIN_AUDIT_ID, true);

            if (bizModel == null)
            {
                bizModel = GetConfigsBizDomainModel();
                if (dictList["chkExport"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    bizModel.description = ACAConstant.COMMON_Y;
                    bizModel.auditStatus = ACAConstant.VALID_STATUS;
                }
                else
                {
                    bizModel.description = ACAConstant.COMMON_N;
                    bizModel.auditStatus = ACAConstant.INVALID_STATUS;
                }

                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_ALLOW_EXPORTING_TO_CSV;
            }
            else
            {
                if (dictList["chkExport"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    bizModel.auditStatus = ACAConstant.VALID_STATUS;
                    bizModel.description = ACAConstant.COMMON_Y;
                }
                else
                {
                    bizModel.auditStatus = ACAConstant.INVALID_STATUS;
                    bizModel.description = ACAConstant.COMMON_N;
                }
            }

            return bizModel;
        }

        /// <summary>
        /// Gets BizDomain model.
        /// </summary>
        /// <param name="elementsColletion">data collected from client</param>
        /// <param name="key">the key in data collected</param>
        /// <param name="stdName">the standard choice name</param>
        /// <returns>the  BizDomainModel4WS instance</returns>
        private BizDomainModel4WS GetBizDomainModel4WSModel(object[] elementsColletion, string key, string stdName)
        {
            if (elementsColletion == null || elementsColletion.Length <= 0 ||
                string.IsNullOrEmpty(key) || string.IsNullOrEmpty(stdName))
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];
            string controlValue = Convert.ToString(dictList[key]);
            BizDomainModel4WS tempbizModel = GetConfigsBizDomainModel();
            tempbizModel.bizdomainValue = stdName;

            BizDomainModel4WS bizModel = _bizBll.GetBizDomainListByModel(tempbizModel, ADMIN_AUDIT_ID, true);

            // initial default value
            if (bizModel == null)
            {
                bizModel = GetConfigsBizDomainModel();
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.bizdomainValue = stdName;
            }

            bizModel.auditStatus = ACAConstant.VALID_STATUS;

            if (controlValue.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                bizModel.description = ACAConstant.COMMON_Yes;
            }
            else
            {
                bizModel.description = ACAConstant.COMMON_No;
            }

            return bizModel;
        }

        /// <summary>
        /// This method is to get configure data information for register license.
        /// </summary>
        /// <returns>The configure data information for register license.</returns>
        private string GetConfigureLicenseDataInfo()
        {
            StringBuilder configureLicense = new StringBuilder();

            //1. Get require license option info.
            BizDomainModel4WS bizModel;

            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_REGISTRATION_LICENSE_ENABLED;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            //if standard choice value is null or not-exist, return default value 'No',
            if (bizModel == null ||
                bizModel.auditStatus != ACAConstant.VALID_STATUS)
            {
                configureLicense.AppendFormat(",\"EnableRequireLicense\":\"{0}\"", ACAConstant.COMMON_No);
            }
            else
            {
                configureLicense.AppendFormat(",\"EnableRequireLicense\":\"{0}\"", bizModel.description);
            }

            //2. Get add license option info.
            configureLicense.AppendFormat(",\"DisableAddLicense\":\"{0}\"", _xPolicyBll.GetValueByKey(ACAConstant.ACA_DISABLE_REGISTRATION_ADD_LICENSE));

            //3. Get remove license option info.
            configureLicense.AppendFormat(",\"DisableRemoveLicense\":\"{0}\"", _xPolicyBll.GetValueByKey(XPolicyConstant.ACA_DISABLE_REGISTRATION_REMOVE_LICENSE));

            return configureLicense.ToString();
        }

        /// <summary>
        /// Get verification e-mail setting for public user registration.
        /// </summary>
        /// <returns>The verification e-mail setting for public user registration.</returns>
        private string GetAccountVerificationMailSetting()
        {
            string configureAutoActivation = string.Empty;

            BizDomainModel4WS bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_ENABLE_ACCOUNT_VERIFICATION_EMAIL;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            if (bizModel == null || bizModel.auditStatus != ACAConstant.VALID_STATUS)
            {
                configureAutoActivation = string.Format(",\"EnableAutoActivation\":\"{0}\"", ACAConstant.COMMON_No);
            }
            else
            {
                configureAutoActivation = string.Format(",\"EnableAutoActivation\":\"{0}\"", bizModel.description);
            }

            return configureAutoActivation;
        }

        /// <summary>
        /// Get ReCaptcha settings.
        /// </summary>
        /// <returns>ReCaptcha data info</returns>
        private string GetRecaptChaDataInfo()
        {
            StringBuilder recaptCha = new StringBuilder();
            recaptCha.AppendFormat("\"RecaptChaForRegistration\":\"{0}\"", _xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_CAPTCHA_FOR_REGISTRATION));
            recaptCha.AppendFormat(",\"RecaptChaForLogin\":\"{0}\"", _xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_CAPTCHA_FOR_LOGIN));

            return recaptCha.ToString();
        }

        /// <summary>
        /// This method is to get biz domain model.
        /// </summary>
        /// <returns>The BizDomainModel4WS</returns>
        private BizDomainModel4WS GetConfigsBizDomainModel()
        {
            BizDomainModel4WS bizDomainModel = new BizDomainModel4WS();
            bizDomainModel.bizdomain = BizDomainConstant.STD_CAT_ACA_CONFIGS;
            bizDomainModel.serviceProviderCode = ConfigManager.AgencyCode;
            bizDomainModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
            bizDomainModel.dispositionID = 0;
            return bizDomainModel;
        }

        /// <summary>
        /// This method is to get Country Code biz domain model.
        /// </summary>
        /// <returns>The country code biz domain model.</returns>
        private BizDomainModel4WS GetCountryCodeBizDomainModel()
        {
            BizDomainModel4WS bizDomainModel = new BizDomainModel4WS();
            bizDomainModel.bizdomain = BizDomainConstant.STD_CAT_PHONE_NUMBER_IDD_ENABLE;
            bizDomainModel.serviceProviderCode = ConfigManager.AgencyCode;
            bizDomainModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
            bizDomainModel.dispositionID = 0;
            return bizDomainModel;
        }

        /// <summary>
        /// This method is get PayFeeLinkStatus bizDomain model.
        /// </summary>
        /// <param name="elementsColletion">data collected from client</param>
        /// <returns>get PayFee link status BizDomainModel4WS for save</returns>
        private BizDomainModel4WS GetPayFeeLinkStatusModel(object[] elementsColletion)
        {
            if (elementsColletion == null ||
                elementsColletion.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];
            string chkPayFeeLink = Convert.ToString(dictList["chkPayFeeLink"]);
            BizDomainModel4WS tempbizModel = GetConfigsBizDomainModel();
            tempbizModel.bizdomainValue = BizDomainConstant.STD_ITEM_REMOVE_PAY_FEE;

            BizDomainModel4WS bizModel = _bizBll.GetBizDomainListByModel(tempbizModel, ADMIN_AUDIT_ID, true);

            if (bizModel == null)
            {
                bizModel = GetConfigsBizDomainModel();
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.description = ACAConstant.COMMON_No;
                bizModel.auditStatus = ACAConstant.VALID_STATUS;
                bizModel.bizdomainValue = BizDomainConstant.STD_ITEM_REMOVE_PAY_FEE;

                if (!chkPayFeeLink.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    bizModel.description = ACAConstant.COMMON_Yes;
                }
            }
            else
            {
                bizModel.auditStatus = ACAConstant.VALID_STATUS;
                bizModel.description = ACAConstant.COMMON_No;

                if (!chkPayFeeLink.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    bizModel.description = ACAConstant.COMMON_Yes;
                }
            }

            return bizModel;
        }

        /// <summary>
        /// This method is get module array.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <returns>The module array.</returns>
        private AcaAdminTreeNodeModel4WS[] GetModuleArray(object[] elementsCollection)
        {
            AcaAdminTreeNodeModel4WS treeModel;
            List<AcaAdminTreeNodeModel4WS> treeModelArray = new List<AcaAdminTreeNodeModel4WS>();

            Dictionary<string, object> subelements = null;
            ArrayList arrayList = new ArrayList();
            ArrayList arrList = null;

            foreach (object elements in elementsCollection)
            {
                arrList = new ArrayList();
                subelements = (Dictionary<string, object>)elements;
                foreach (KeyValuePair<string, object> kvp in subelements)
                {
                    arrList.Add(kvp.Value);
                }

                arrayList.Add(arrList);
            }

            for (int i = 0; i < arrayList.Count; i++)
            {
                arrList = (ArrayList)arrayList[i];
                string check = arrList[3].ToString();
                string pageType = arrList[4].ToString();

                string elementId = arrList[5].ToString();
                string elementName = arrList[6].ToString();
                string parentId = arrList[7].ToString();
                string labelKey = arrList[8].ToString();
                string forceLogin = string.Empty;
                string singleSelectionOnly = string.Empty;

                if (arrList.Count >= 10)
                {
                    forceLogin = arrList[9].ToString();
                }

                if (StandardChoiceUtil.IsSuperAgency(true) && labelKey.Equals("aca_sys_feature_apply_a_permit_by_service"))
                {
                    if (arrList.Count >= 11)
                    {
                        singleSelectionOnly = arrList[10].ToString();
                    }
                }

                treeModel = new AcaAdminTreeNodeModel4WS();

                treeModel.elementID = elementId;
                treeModel.elementName = elementName;
                treeModel.parentID = parentId;

                if (pageType == "LOGIN_ENABLED" ||
                    pageType == "REGISTRATION_ENABLED" ||
                    BizDomainConstant.STD_ITEM_ACCOUNT_MANAGEMENT_ENABLED.Equals(pageType))
                {
                    treeModel.nodeDescribe = check.Equals("true", StringComparison.InvariantCultureIgnoreCase) ? "YES" : "NO";
                }
                else
                {
                    treeModel.recStatus = check.Equals("true", StringComparison.InvariantCultureIgnoreCase) ? "A" : "I";
                }

                treeModel.pageType = pageType;
                treeModel.labelKey = labelKey;
                treeModel.forceLogin = forceLogin;
                treeModel.singleServiceOnly = singleSelectionOnly;

                treeModelArray.Add(treeModel);
            }

            return treeModelArray.ToArray();
        }

        /// <summary>
        /// Saves the shared comments.
        /// </summary>
        /// <param name="elementsColletion">The elements collection.</param>
        /// <param name="moduleName">Name of the module.</param>
        private void SaveSharedComments(object[] elementsColletion, string moduleName)
        {
            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];
            if (dictList["SharedComments"].ToString() != string.Empty)
            {
                string labelKey = "aca_socialmedia_label_comments_pattern";
                string stringvalue = dictList["SharedComments"].ToString();
                SaveLabel(labelKey, stringvalue, null, _servProvCode, ACAConstant.LEVEL_TYPE_MODULE, moduleName);
            }
        }

        /// <summary>
        /// save user role privilege in policy table.    
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="policyName">The policy name.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="userRole">The user role.</param>
        private void SavePolicyUserRolePrivilege(XpolicyUserRolePrivilegeModel policy, string policyName, string moduleName, string userRole)
        {
            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();

            policy.serviceProviderCode = ConfigManager.AgencyCode;
            policy.policyName = policyName;

            //data1 is not empty filed.
            policy.data1 = policyName;
            policy.rightGranted = ACAConstant.GRANTED_RIGHT;
            policy.level = ACAConstant.LEVEL_TYPE_MODULE;
            policy.levelData = moduleName;
            policy.userRolePrivilegeModel = userRoleBll.ConvertToUserRolePrivilegeModel(userRole);
            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.status = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN_CALLER_ID;
            IXPolicyWrapper policyWrapper = (IXPolicyWrapper)ObjectFactory.GetObject(typeof(IXPolicyWrapper), true);

            List<XpolicyUserRolePrivilegeModel> policies = new List<XpolicyUserRolePrivilegeModel>();
            policies.Add(policy);
            policyWrapper.CreateOrUpdatePolicy(ConfigManager.AgencyCode, policies.ToArray(), ACAConstant.ADMIN_CALLER_ID);
        }
        
        /// <summary>
        /// save user role privilege in policy table for contact.    
        /// </summary>
        /// <param name="policyName">the policy name</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="userRole">the user role</param>
        private void SavePolicyUserRolePrivilege4Contact(string policyName, string moduleName, string userRole)
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XPolicyModel[] policyArray = new XPolicyModel[1];
            XPolicyModel policy = new XPolicyModel();

            policy.serviceProviderCode = ConfigManager.AgencyCode;
            policy.policyName = policyName;
            policy.data1 = policyName;
            policy.data4 = policyName;
            policy.rightGranted = ACAConstant.GRANTED_RIGHT;
            policy.level = ACAConstant.LEVEL_TYPE_MODULE;
            policy.levelData = moduleName;
            policy.data2 = userRole;
            policy.status = ACAConstant.VALID_STATUS;
            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN_CALLER_ID;

            policyArray[0] = policy;
            xPolicyBll.CreateOrUpdatePolicy(null, policyArray);
        }

        /// <summary>
        /// save user role privilege in policy table.    
        /// </summary>
        /// <param name="policy">the policy model</param>
        /// <param name="policyName">the policy name.</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="userRole">the user role.</param>
        /// <param name="data4">the data4.</param>
        private void SaveModuleUserRolePermission(XpolicyUserRolePrivilegeModel policy, string policyName, string moduleName, string userRole, string data4)
        {
            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();

            policy.serviceProviderCode = ConfigManager.AgencyCode;
            policy.policyName = policyName;

            //data1 is not empty filed.
            policy.data1 = policyName;
            policy.data4 = data4;
            policy.rightGranted = ACAConstant.GRANTED_RIGHT;
            policy.level = ACAConstant.LEVEL_TYPE_MODULE;
            policy.levelData = moduleName;
            policy.userRolePrivilegeModel = userRoleBll.ConvertToUserRolePrivilegeModel(userRole);
            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.status = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN_CALLER_ID;
            IXPolicyWrapper policyWrapper = (IXPolicyWrapper)ObjectFactory.GetObject(typeof(IXPolicyWrapper), true);

            List<XpolicyUserRolePrivilegeModel> policies = new List<XpolicyUserRolePrivilegeModel>();
            policies.Add(policy);
            policyWrapper.CreateOrUpdatePolicy(ConfigManager.AgencyCode, policies.ToArray(), ACAConstant.ADMIN_CALLER_ID);
        }

        /// <summary>
        /// Get XPolicy model array in inspection section.
        /// </summary>
        /// <param name="elementsColletion">data in ACA admin inspection section.</param>
        /// <param name="moduleName">module level</param>
        /// <returns>XPolicyModel[] array</returns>
        private XPolicyModel[] GetXPolicyArray(object[] elementsColletion, string moduleName)
        {
            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];

            List<XPolicyModel> policyArray = new List<XPolicyModel>();
            XPolicyModel policy;

            if (dictList.ContainsKey("DisplayEmail") && !string.IsNullOrEmpty(Convert.ToString(dictList["DisplayEmail"])))
            {
                policy = GetDisplayEmailXPolicy(dictList, moduleName);
                policyArray.Add(policy);
            }

            //Get Multiple Enabled indication.
            if (dictList.ContainsKey("IsMultipleEnabled") && !string.IsNullOrEmpty(Convert.ToString(dictList["IsMultipleEnabled"])))
            {
                policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, moduleName, BizDomainConstant.STD_MULTIPLE_INSPECTIONS_ENABLED, dictList["IsMultipleEnabled"].ToString());
                policyArray.Add(policy);
            }

            if (dictList.ContainsKey("DisplayDefaultContact4Inspection") && !string.IsNullOrEmpty(Convert.ToString(dictList["DisplayDefaultContact4Inspection"])))
            {
                policy = GetXPolicyModel(
                                    ACAConstant.LEVEL_TYPE_MODULE,
                                    moduleName,
                                    XPolicyConstant.INSPECITON_DISPLAY_DEFAULT_CONTACT,
                                    dictList["DisplayDefaultContact4Inspection"].ToString());
                policyArray.Add(policy);
            }

            //Get search cross modules
            if (dictList.ContainsKey("chkSearchCrossModule") && !string.IsNullOrEmpty(Convert.ToString(dictList["chkSearchCrossModule"])))
            {
                string[] checkedValues = dictList["chkSearchCrossModule"].ToString().Split(ACAConstant.SPLIT_CHAR);

                foreach (string item in checkedValues)
                {
                    string[] keyValue = item.Split(ACAConstant.SPLIT_CHAR2);
                    XPolicyModel tempPolicy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, moduleName, ACAConstant.ACA_ENABLE_SEARCH_CROSS_MODULE, keyValue[1]);
                    tempPolicy.data4 = keyValue[0];
                    policyArray.Add(tempPolicy);
                }
            }

            //Get display pay fee link indication.
            if (dictList.ContainsKey("DisplayPayFeeLink") && !string.IsNullOrEmpty(Convert.ToString(dictList["DisplayPayFeeLink"])))
            {
                policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, moduleName, XPolicyConstant.PAY_FEE_LINK_DISP, dictList["DisplayPayFeeLink"].ToString());
                policyArray.Add(policy);
            }

            //Get clone switch.
            if (dictList.ContainsKey("EnableCloneRecord") && !string.IsNullOrEmpty(Convert.ToString(dictList["EnableCloneRecord"])))
            {
                policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, moduleName, XPolicyConstant.ENABLE_CLONE_RECORD, dictList["EnableCloneRecord"].ToString());
                policyArray.Add(policy);
            }

            //Get Search by ASI addition information.
            if (dictList.ContainsKey("EnabelSearchASIAdditionCriteria") && !string.IsNullOrEmpty(Convert.ToString(dictList["EnabelSearchASIAdditionCriteria"])))
            {
                policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, moduleName, XPolicyConstant.ENABLE_SEARCHASI_ADDITIONALCRITERIA, dictList["EnabelSearchASIAdditionCriteria"].ToString());
                policyArray.Add(policy);
            }

            //Get Search by contact template addition information.
            if (dictList.ContainsKey("EnabelSearchContactTemplateAdditionCriteria") && !string.IsNullOrEmpty(Convert.ToString(dictList["EnabelSearchContactTemplateAdditionCriteria"])))
            {
                policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, moduleName, XPolicyConstant.ENABLE_SEARCHCONTACT_ADDITIONALCRITERIA, dictList["EnabelSearchContactTemplateAdditionCriteria"].ToString());
                policyArray.Add(policy);
            }

            // (Feature:09ACC-08040_Board_Type_Selection) the flag indicating 
            // whether Board Type Selection mode is enabled for the specified module
            if (dictList.ContainsKey("EnableBoardTypeSelection") &&
                !string.IsNullOrEmpty(Convert.ToString(dictList["EnableBoardTypeSelection"])))
            {
                policy = GetXPolicyModel(
                    ACAConstant.LEVEL_TYPE_MODULE,
                    moduleName,
                    XPolicyConstant.ENABLE_BOARD_TYPE_SELECTION,
                    dictList["EnableBoardTypeSelection"].ToString());
                policyArray.Add(policy);
            }

            if (dictList.ContainsKey("SocialMediaShareButtonPermission") && !string.IsNullOrEmpty(Convert.ToString(dictList["SocialMediaShareButtonPermission"])))
            {
                policy = GetXPolicyModel(
                    ACAConstant.LEVEL_TYPE_MODULE,
                    moduleName,
                    XPolicyConstant.SOCIAL_MEDIA_SHARE_BUTTON_PERMISSION,
                    dictList["SocialMediaShareButtonPermission"].ToString());
                policyArray.Add(policy);
            }

            return policyArray.ToArray();
        }

        /// <summary>
        /// Build XPolicy model list
        /// </summary>
        /// <param name="elementsColletion">data source</param>
        /// <returns>XPolicyModel model list.</returns>
        private XPolicyModel[] BuildXPolicyModelList(object[] elementsColletion)
        {
            if (elementsColletion == null ||
                elementsColletion.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];
            List<XPolicyModel> policyArray = new List<XPolicyModel>();
            XPolicyModel policy;
            string isEnabledCrossModeSearch = dictList["chkCrossModuleEnabled"] != null ? dictList["chkCrossModuleEnabled"].ToString() : string.Empty;
            string isEnableAccessibility = dictList.ContainsKey("chkAccessibility") ? Convert.ToString(dictList["chkAccessibility"]) : string.Empty;
            string isDisplayLicenseState = dictList.ContainsKey("chkLicenseState") ? Convert.ToString(dictList["chkLicenseState"]) : string.Empty;

            if (dictList != null && !string.IsNullOrEmpty(dictList.ToString()))
            {
                if (dictList.ContainsKey("chkShoppingCart") && !string.IsNullOrEmpty(dictList["chkShoppingCart"].ToString()))
                {
                    string isEnableShoppingCart = dictList["chkShoppingCart"].ToString();
                    if (isEnableShoppingCart.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isEnableShoppingCart = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        isEnableShoppingCart = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ACA_ENABLE_SHOPPING_CART, isEnableShoppingCart);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("cbxEnableProxyUser") && !string.IsNullOrEmpty(dictList["cbxEnableProxyUser"].ToString()))
                {
                    string isEnableProxyUser = dictList["cbxEnableProxyUser"].ToString();
                    if (isEnableProxyUser.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isEnableProxyUser = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        isEnableProxyUser = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ACA_ENABLE_PROXYUSER, isEnableProxyUser);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkAnnouncementActivate") && !string.IsNullOrEmpty(dictList["chkAnnouncementActivate"].ToString()))
                {
                    string isEnableAnnouncement = dictList["chkAnnouncementActivate"].ToString();
                    if (isEnableAnnouncement.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isEnableAnnouncement = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        isEnableAnnouncement = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ACA_ENABLE_ANNOUNCEMENT, isEnableAnnouncement);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("cbxEnableParcelGen") && !string.IsNullOrEmpty(dictList["cbxEnableParcelGen"].ToString()))
                {
                    string isEnableParcelGen = dictList["cbxEnableParcelGen"].ToString();

                    if (isEnableParcelGen.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isEnableParcelGen = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        isEnableParcelGen = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ACA_ENABLE_PARCEL_GENEALOGY, isEnableParcelGen);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkDocType") && !string.IsNullOrEmpty(dictList["chkDocType"].ToString()))
                {
                    string isEnableDocType = dictList["chkDocType"].ToString();

                    if (isEnableDocType.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isEnableDocType = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        isEnableDocType = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ACA_ENABLE_DOCUMENT_TYPE_FILTER, isEnableDocType);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("txtProxyUserExpiredDate") && !string.IsNullOrEmpty(dictList["txtProxyUserExpiredDate"].ToString()))
                {
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.PROXY_INVITATION_EXPIRATION_DAYS, dictList["txtProxyUserExpiredDate"].ToString());
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("txtProxyUserExpiredRemoveDate") && !string.IsNullOrEmpty(dictList["txtProxyUserExpiredRemoveDate"].ToString()))
                {
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.PROXY_INVITATION_PURGE_DAYS, dictList["txtProxyUserExpiredRemoveDate"].ToString());
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("txtAnnouncementInterval") && !string.IsNullOrEmpty(dictList["txtAnnouncementInterval"].ToString()))
                {
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ANNOUNCEMENT_INTERVAL, dictList["txtAnnouncementInterval"].ToString());
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("ddlTransaction") && !string.IsNullOrEmpty(dictList["ddlTransaction"].ToString()))
                {
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ACA_SHOPPING_CART_PAYMENT_TRANSACTION_SETTING, dictList["ddlTransaction"].ToString());
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("txtSelectExpiration") && !string.IsNullOrEmpty(dictList["txtSelectExpiration"].ToString()))
                {
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ACA_SHOPPING_CART_EXPIRATION_DAY_OF_SELECTED_ITEMS, dictList["txtSelectExpiration"].ToString());
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("txtSaveExpiration") && !string.IsNullOrEmpty(dictList["txtSaveExpiration"].ToString()))
                {
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ACA_SHOPPING_CART_EXPIRATION_DAY_OF_SAVED_ITEMS, dictList["txtSaveExpiration"].ToString());
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkCheckBoxOption") && !string.IsNullOrEmpty(dictList["chkCheckBoxOption"].ToString()))
                {
                    string isCheckBoxVisible = dictList["chkCheckBoxOption"].ToString();

                    if (isCheckBoxVisible.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isCheckBoxVisible = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        isCheckBoxVisible = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ACA_CHECKBOX_ANONYMOUSUSER_VISIBLE, isCheckBoxVisible);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkSearchMyLicense") && !string.IsNullOrEmpty(dictList["chkSearchMyLicense"].ToString()))
                {
                    string chkSearchMyLicense = dictList["chkSearchMyLicense"].ToString();

                    if (chkSearchMyLicense.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkSearchMyLicense = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        chkSearchMyLicense = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ACA_ENABLE_ONLY_SEARCH_MY_LICENSE, chkSearchMyLicense);
                    policyArray.Add(policy);
                }

                if (!string.IsNullOrEmpty(isEnabledCrossModeSearch))
                {
                    if (isEnabledCrossModeSearch.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isEnabledCrossModeSearch = ACAConstant.COMMON_Y;
                        InitSearchModules();
                    }
                    else
                    {
                        isEnabledCrossModeSearch = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ACA_ENABLE_CROSS_MODULE_SEARCH, isEnabledCrossModeSearch);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkGlobalSearchSwitch") && !string.IsNullOrEmpty(dictList["chkGlobalSearchSwitch"].ToString()))
                {
                    string flag4GlobalSearchEnabled = dictList["chkGlobalSearchSwitch"].ToString();

                    if (flag4GlobalSearchEnabled.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        flag4GlobalSearchEnabled = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        flag4GlobalSearchEnabled = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ENABLE_GLOBAL_SEARCH, flag4GlobalSearchEnabled);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkGlobalSearchCAPResultGroup") && !string.IsNullOrEmpty(dictList["chkGlobalSearchCAPResultGroup"].ToString()))
                {
                    string flag4GlobalSearchCAPResultGroupEnabled = dictList["chkGlobalSearchCAPResultGroup"].ToString();

                    if (flag4GlobalSearchCAPResultGroupEnabled.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        flag4GlobalSearchCAPResultGroupEnabled = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        flag4GlobalSearchCAPResultGroupEnabled = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_RECORDS, flag4GlobalSearchCAPResultGroupEnabled);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkGlobalSearchLPResultGroup") && !string.IsNullOrEmpty(dictList["chkGlobalSearchLPResultGroup"].ToString()))
                {
                    string flag4GlobalSearchLPResultGroupEnabled = dictList["chkGlobalSearchLPResultGroup"].ToString();

                    if (flag4GlobalSearchLPResultGroupEnabled.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        flag4GlobalSearchLPResultGroupEnabled = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        flag4GlobalSearchLPResultGroupEnabled = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_LP, flag4GlobalSearchLPResultGroupEnabled);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkGlobalSearchAPOResultGroup") && !string.IsNullOrEmpty(dictList["chkGlobalSearchAPOResultGroup"].ToString()))
                {
                    string flag4GlobalSearchAPOResultGroupEnabled = dictList["chkGlobalSearchAPOResultGroup"].ToString();

                    if (flag4GlobalSearchAPOResultGroupEnabled.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        flag4GlobalSearchAPOResultGroupEnabled = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        flag4GlobalSearchAPOResultGroupEnabled = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_APO, flag4GlobalSearchAPOResultGroupEnabled);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkFeinMasking") && !string.IsNullOrEmpty(dictList["chkFeinMasking"].ToString()))
                {
                    string flag4FeinMaskingEnabled = dictList["chkFeinMasking"].ToString();

                    if (flag4FeinMaskingEnabled.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        flag4FeinMaskingEnabled = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        flag4FeinMaskingEnabled = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ITEM_ENABLE_FEIN_MASKING, flag4FeinMaskingEnabled);
                    policyArray.Add(policy);
                }

                if (!string.IsNullOrEmpty(isEnableAccessibility))
                {
                    if (isEnableAccessibility.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isEnableAccessibility = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        isEnableAccessibility = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ACA_ENABLE_ACCESSIBILITY, isEnableAccessibility);
                    policyArray.Add(policy);
                }

                if (!string.IsNullOrEmpty(isDisplayLicenseState))
                {
                    if (ACAConstant.COMMON_TRUE.Equals(isDisplayLicenseState, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isDisplayLicenseState = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        isDisplayLicenseState = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ACA_ENABLE_LICENSESTATE, isDisplayLicenseState);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkEnableAutoUpdate") && !string.IsNullOrEmpty(dictList["chkEnableAutoUpdate"].ToString()))
                {
                    string chkEnableAutoUpdateByCSV = dictList["chkEnableAutoUpdate"].ToString();

                    if (chkEnableAutoUpdateByCSV.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkEnableAutoUpdateByCSV = ACAConstant.COMMON_Y;
                    }
                    else
                    {
                        chkEnableAutoUpdateByCSV = ACAConstant.COMMON_N;
                    }

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_AUTO_UPDATE_EXAM_BY_CSV, chkEnableAutoUpdateByCSV);
                    policyArray.Add(policy);
                }

                AddPolicyItem(policyArray, dictList, "chkInspectionResultEnableAutoUpdate", XPolicyConstant.ENABLE_AUTO_UPDATE_INSPECTION_RESULT);

                //Reference contact search settings.
                if (dictList.ContainsKey("RefContactSearchEnabled") && !string.IsNullOrEmpty(dictList["RefContactSearchEnabled"].ToString()))
                {
                    string refContactSearchEnabled = ValidationUtil.IsTrue(dictList["RefContactSearchEnabled"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_REFERENCE_CONTACT_SEARCH, refContactSearchEnabled);
                    policyArray.Add(policy);
                }

                //Reference licensed professional search settings.
                if (dictList.ContainsKey("RefLPSearchEnabled") && !string.IsNullOrEmpty(dictList["RefLPSearchEnabled"].ToString()))
                {
                    string refLPSearchEnabled = ValidationUtil.IsTrue(dictList["RefLPSearchEnabled"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_REFERENCE_LP_SEARCH, refLPSearchEnabled);
                    policyArray.Add(policy);
                }

                //Enable people document settings.
                if (dictList.ContainsKey("chkEnableAccountAttachment") && !string.IsNullOrEmpty(dictList["chkEnableAccountAttachment"].ToString()))
                {
                    string chkEnableAccountAttachment = ValidationUtil.IsTrue(dictList["chkEnableAccountAttachment"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_ACCOUNT_ATTACHMENT, chkEnableAccountAttachment);
                    policyArray.Add(policy);
                }

                //Edit contact address settings.
                if (dictList.ContainsKey("chkEnableContactAddressEdit") && !string.IsNullOrEmpty(dictList["chkEnableContactAddressEdit"].ToString()))
                {
                    string chkEnableContactAddressEdit = ValidationUtil.IsTrue(dictList["chkEnableContactAddressEdit"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_CONTACT_ADDRESS_EDIT, chkEnableContactAddressEdit);
                    policyArray.Add(policy);
                }

                //Enable Contact Address "Deactivate" action settings.
                if (dictList.ContainsKey("chkEnableContactAddressDeactivate") && !string.IsNullOrEmpty(dictList["chkEnableContactAddressDeactivate"].ToString()))
                {
                    string chkEnableContactAddressDeactivate = ValidationUtil.IsTrue(dictList["chkEnableContactAddressDeactivate"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_CONTACT_ADDRESS_DEACTIVATE, chkEnableContactAddressDeactivate);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkEnableManualContactAssociation") && !string.IsNullOrEmpty(dictList["chkEnableManualContactAssociation"].ToString()))
                {
                    string chkEnableManualContactAssociation = ValidationUtil.IsTrue(dictList["chkEnableManualContactAssociation"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABEL_MANUAL_CONTACT_ASSOCIATION, chkEnableManualContactAssociation);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkAutoActiveNewAssociatedContact") && !string.IsNullOrEmpty(dictList["chkAutoActiveNewAssociatedContact"].ToString()))
                {
                    string chkAutoActiveNewAssociatedContact = ValidationUtil.IsTrue(dictList["chkAutoActiveNewAssociatedContact"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.AUTO_ACTIVATE_NEW_ASSOCIATED_CONTACT, chkAutoActiveNewAssociatedContact);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkEnableContactAddressMaintenance") && !string.IsNullOrEmpty(dictList["chkEnableContactAddressMaintenance"].ToString()))
                {
                    string chkEnableContactAddressMaintenance = ValidationUtil.IsTrue(dictList["chkEnableContactAddressMaintenance"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_CONTACT_ADDRESS_MAINTENANCE, chkEnableContactAddressMaintenance);
                    policyArray.Add(policy);
                }

                if (dictList.ContainsKey("chkEnableContactCrossAgency") && !string.IsNullOrEmpty(dictList["chkEnableContactCrossAgency"].ToString()))
                {
                    string chkEnableContactAddressMaintenance = ValidationUtil.IsTrue(dictList["chkEnableContactCrossAgency"].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_GET_CONTACT_FROM_OTHER_AGENCY, chkEnableContactAddressMaintenance);
                    policyArray.Add(policy);
                }

                //Enable account education, examination and continuing education input settings.
                AddPolicyItem(policyArray, dictList, "chkEnableAccountEduExamCEInput", XPolicyConstant.ENABLE_ACCOUNT_EDU_EXAM_CE_INPUT);

                // Template
                if (dictList.ContainsKey("rdlTemplateType") && dictList["rdlTemplateType"].ToString().Length > 0)
                {
                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_NEW_TEMPLATE, dictList["rdlTemplateType"].ToString());
                    policyArray.Add(policy);
                }
            }

            return policyArray.ToArray();
        }

        /// <summary>
        /// Add the policy item.
        /// </summary>
        /// <param name="policyArray">The policy array.</param>
        /// <param name="dictList">The collection of policy's key and value pair.</param>
        /// <param name="itemKey">The item key.</param>
        /// <param name="policyName">The policy name.</param>
        private void AddPolicyItem(List<XPolicyModel> policyArray, Dictionary<string, object> dictList, string itemKey, string policyName)
        {
            if (dictList != null && dictList.ContainsKey(itemKey) && !string.IsNullOrEmpty(dictList[itemKey].ToString()))
            {
                string itemValue = ValidationUtil.IsTrue(dictList[itemKey].ToString()) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                XPolicyModel policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, policyName, itemValue);
                policyArray.Add(policy);
            }
        }

        /// <summary>
        /// Get Registration Setting XPolicy Model List.
        /// </summary>
        /// <param name="elementsColletion">elements Collection</param>
        /// <returns>XPolicy model array.</returns>
        private XPolicyModel[] GetRegistrationSettingXPolicys(object[] elementsColletion)
        {
            if (elementsColletion == null ||
                elementsColletion.Length <= 0)
            {
                return null;
            }

            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsColletion[0];
            List<XPolicyModel> policyArray = new List<XPolicyModel>();
            XPolicyModel policy;

            if (dictList != null && !string.IsNullOrEmpty(dictList.ToString()))
            {
                if (dictList["chkAddLisOption"] != null && !string.IsNullOrEmpty(dictList["chkAddLisOption"].ToString()))
                {
                    string isEnableAddLicense = dictList["chkAddLisOption"].ToString();
                    isEnableAddLicense = isEnableAddLicense.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, ACAConstant.ACA_DISABLE_REGISTRATION_ADD_LICENSE, isEnableAddLicense);
                    policyArray.Add(policy);
                }

                if (dictList["chkRemoveLisOption"] != null && !string.IsNullOrEmpty(dictList["chkRemoveLisOption"].ToString()))
                {
                    string isEnableRemoveLicense = dictList["chkRemoveLisOption"].ToString();
                    isEnableRemoveLicense = isEnableRemoveLicense.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ACA_DISABLE_REGISTRATION_REMOVE_LICENSE, isEnableRemoveLicense);
                    policyArray.Add(policy);
                }

                // Password settings -> password expiration
                string isEnablePasswordExpiration = Convert.ToString(dictList["chkPasswordExpiration"]);

                if (!string.IsNullOrEmpty(isEnablePasswordExpiration))
                {
                    isEnablePasswordExpiration = ValidationUtil.IsTrue(isEnablePasswordExpiration) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_PASSWORD_EXPRIATION_CHECK, isEnablePasswordExpiration);
                    string passwordDay = Convert.ToString(dictList["txtPasswordExpirationDays"]);
                    policy.data3 = passwordDay == null ? string.Empty : passwordDay;

                    policyArray.Add(policy);
                }

                // Password settings -> failed attempts
                string isEnableFailedAttempts = Convert.ToString(dictList["chkPasswordFailedAttempts"]);

                if (!string.IsNullOrEmpty(isEnableFailedAttempts))
                {
                    isEnableFailedAttempts = ValidationUtil.IsTrue(isEnableFailedAttempts) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_PASSWORD_FAILED_ATTEMPTS_CHECK, isEnableFailedAttempts);
                    string passwordTimes = Convert.ToString(dictList["txtPasswordFailedTimes"]);
                    policy.data3 = passwordTimes == null ? string.Empty : passwordTimes;
                    string passwordDurations = Convert.ToString(dictList["txtPasswordFailedDurations"]);
                    policy.data5 = passwordDurations == null ? string.Empty : passwordDurations;

                    policyArray.Add(policy);
                }

                if (dictList["chkEnableRecaptchaForRegistration"] != null && !string.IsNullOrEmpty(dictList["chkEnableRecaptchaForRegistration"].ToString()))
                {
                    string isEnableReCaptchaForRegistration = dictList["chkEnableRecaptchaForRegistration"].ToString();
                    isEnableReCaptchaForRegistration = isEnableReCaptchaForRegistration.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_CAPTCHA_FOR_REGISTRATION, isEnableReCaptchaForRegistration);
                    policyArray.Add(policy);
                }

                if (dictList["chkEnableRecaptchaForLogin"] != null && !string.IsNullOrEmpty(dictList["chkEnableRecaptchaForLogin"].ToString()))
                {
                    string chkEnableRecaptchaForLogin = dictList["chkEnableRecaptchaForLogin"].ToString();
                    chkEnableRecaptchaForLogin = chkEnableRecaptchaForLogin.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

                    policy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, XPolicyConstant.ENABLE_CAPTCHA_FOR_LOGIN, chkEnableRecaptchaForLogin);
                    policyArray.Add(policy);
                }

                //add reset password setting save
                AddPolicyItem(policyArray, dictList, "chkEnableResetPassordOnCombine", XPolicyConstant.ENABLE_RESETPASSWORD_ON_COMBINE);

                //add login setting save
                AddPolicyItem(policyArray, dictList, "chkEnableLoginOnRegistration", XPolicyConstant.ENABLE_LOGIN_ON_REGISTRATION);
                XPolicyModel xPolicyModel = policyArray[policyArray.Count - 1];

                if (xPolicyModel != null && xPolicyModel.data1.Equals(XPolicyConstant.ENABLE_LOGIN_ON_REGISTRATION, StringComparison.InvariantCultureIgnoreCase))
                {
                    string loginExpireTimes = Convert.ToString(dictList["txtLoginExpireTime"]);
                    xPolicyModel.data3 = loginExpireTimes;
                }
            }

            return policyArray.ToArray();
        }

        /// <summary>
        /// Get XPolicy model.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="levelData">The level data.</param>
        /// <param name="policyKey">The policy key.</param>
        /// <param name="dictlistString">The data2 value</param>
        /// <returns>The XPolicyModel.</returns>
        private XPolicyModel GetXPolicyModel(string level, string levelData, string policyKey, string dictlistString)
        {
            XPolicyModel policy = new XPolicyModel();

            policy.rightGranted = ACAConstant.GRANTED_RIGHT;
            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.status = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN_CALLER_ID;
            policy.serviceProviderCode = ConfigManager.AgencyCode;
            policy.policyName = BizDomainConstant.STD_CAT_ACA_CONFIGS;

            policy.level = level;
            policy.levelData = levelData;
            policy.data1 = policyKey;
            policy.data2 = dictlistString;
            policy.data4 = policyKey;

            return policy;
        }

        /// <summary>
        /// Get XPolicy Model
        /// </summary>
        /// <param name="level">the level.</param>
        /// <param name="levelData">the level data</param>
        /// <param name="policyKey">the policy key</param>
        /// <param name="dictlistString">the data2 value</param>
        /// <param name="data3">the data3 value</param>
        /// <param name="data4">the data4 value</param>
        /// <returns>the XPolicy model.</returns>
        private XPolicyModel GetXPolicyModel(string level, string levelData, string policyKey, string dictlistString, string data3, string data4)
        {
            XPolicyModel policy = new XPolicyModel();

            policy.rightGranted = ACAConstant.GRANTED_RIGHT;
            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.status = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN_CALLER_ID;
            policy.serviceProviderCode = ConfigManager.AgencyCode;
            policy.policyName = BizDomainConstant.STD_CAT_ACA_CONFIGS;

            policy.level = level;
            policy.levelData = levelData;
            policy.data1 = policyKey;
            policy.data2 = dictlistString;
            policy.data3 = data3;
            policy.data4 = data4;

            return policy;
        }

        /// <summary>
        /// Get XPolicy Model
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns>the XPolicy model.</returns>
        private XPolicyModel ConstructContactTypeXPolicyModel(string moduleName)
        {
            XPolicyModel policy = new XPolicyModel();

            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN;
            policy.status = ACAConstant.ENABLED_STATUS;
            policy.serviceProviderCode = ConfigManager.AgencyCode;
            policy.policyName = XPolicyConstant.CONTACT_TYPE_RESTRICTION_BY_MODULE;

            policy.level = ACAConstant.MODULE_NAME;
            policy.levelData = moduleName;
            policy.data2 = ACAConstant.RECORD_CONTACT_TYPE;

            return policy;
        }

        /// <summary>
        /// Initialize search module data when enable cross module search in the first time.
        /// </summary>
        private void InitSearchModules()
        {
            ACAUserType userType = ACAUserType.Registered;
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            IList<TabItem> tabList = bizBll.GetTabsList(ConfigManager.AgencyCode, userType, false);
            List<XPolicyModel> policyArray = new List<XPolicyModel>();
            List<string> moduleNames = new List<string>();

            foreach (TabItem item in tabList)
            {
                if (item != null && !string.IsNullOrEmpty(item.Module)
                    && ((item.TabVisible && !string.IsNullOrEmpty(item.Label) && item.Label != "aca_sys_default_home") || "aca_sys_apo_search".Equals(item.Label)))
                {
                    moduleNames.Add(item.Module);
                    XPolicyModel[] modules = policyBll.GetPolicyListByCategory(ACAConstant.ACA_ENABLE_SEARCH_CROSS_MODULE, item.Module);

                    if (modules != null && modules.Length > 0)
                    {
                        return;
                    }
                }
            }

            foreach (string name in moduleNames)
            {
                foreach (string item in moduleNames)
                {
                    if (item != null && !item.Equals(name))
                    {
                        XPolicyModel tempPolicy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, name, ACAConstant.ACA_ENABLE_SEARCH_CROSS_MODULE, ACAConstant.COMMON_Y);
                        tempPolicy.data4 = item;
                        policyArray.Add(tempPolicy);
                    }
                }
            }

            if (policyArray == null || policyArray.Count == 0)
            {
                return;
            }

            policyBll.CreateOrUpdatePolicy(null, policyArray.ToArray());
        }

        /// <summary>
        /// Get display email's XPolicy.
        /// </summary>
        /// <param name="dictList">The object that contain DisplayEmail value.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The XPolicy model.</returns>
        private XPolicyModel GetDisplayEmailXPolicy(Dictionary<string, object> dictList, string moduleName)
        {
            XPolicyModel policy = new XPolicyModel();

            policy.serviceProviderCode = ConfigManager.AgencyCode;

            policy.level = ACAConstant.LEVEL_TYPE_MODULE;
            policy.levelData = moduleName;
            policy.data2 = dictList["DisplayEmail"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            policy.data1 = ACAConstant.ACA_ENABLE_WF_DISP_EMAIL;
            policy.data4 = ACAConstant.ACA_ENABLE_WF_DISP_EMAIL;
            policy.rightGranted = ACAConstant.GRANTED_RIGHT;
            policy.recStatus = ACAConstant.VALID_STATUS;
            policy.status = ACAConstant.VALID_STATUS;
            policy.recFullName = ACAConstant.ADMIN_CALLER_ID;

            return policy;
        }

        /// <summary>
        /// This method is get bizDomain model by inspection.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The inspection bizDomain model.</returns>
        private BizDomainModel4WS[] GetInspectionArray(object[] elementsCollection, string moduleName)
        {
            Dictionary<string, object> dictList = (Dictionary<string, object>)elementsCollection[0];

            List<BizDomainModel4WS> bizModelArray = new List<BizDomainModel4WS>();
            BizDomainModel4WS bizModel = null;

            XpolicyUserRolePrivilegeModel policy = new XpolicyUserRolePrivilegeModel();

            // Display map for show object
            if (dictList["DisplayMap4ShowObject"].ToString() != string.Empty)
            {
                bizModel = GetBizModel4DisplayMap(
                    dictList,
                    "DisplayMap4ShowObject",
                    moduleName + "_" + BizDomainConstant.STD_DISPLAY_MAP_FOR_SHOWOBJECT);
                bizModelArray.Add(bizModel);
            }

            // Display map for select object
            if (dictList["DisplayMap4SelectObject"].ToString() != string.Empty)
            {
                bizModel = GetBizModel4DisplayMap(
                    dictList,
                    "DisplayMap4SelectObject",
                    moduleName + "_" + BizDomainConstant.STD_DISPLAY_MAP_FOR_SELECTOBJECT);
                bizModelArray.Add(bizModel);
            }

            if (dictList["UserType"].ToString() !=
                string.Empty)
            {
                SavePolicyUserRolePrivilege(policy, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_SCHEDULE, moduleName, dictList["UserType"].ToString());
            }

            if (dictList["UserTypeInputContact"].ToString() != string.Empty)
            {
                SavePolicyUserRolePrivilege4Contact(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT, moduleName, dictList["UserTypeInputContact"].ToString());
            }

            if (dictList["UserTypeViewContact"].ToString() != string.Empty)
            {
                SavePolicyUserRolePrivilege4Contact(ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT, moduleName, dictList["UserTypeViewContact"].ToString());
            }

            if (dictList["DisplayOption"].ToString() != string.Empty)
            {
                bizModel = GetDisplayOptionModel(dictList, moduleName);
                bizModelArray.Add(bizModel);
            }

            return bizModelArray.ToArray();
        }

        /// <summary>
        /// Build BizDomainModel for display map functionality.
        /// </summary>
        /// <param name="dictList">Key-value pair list for all setting items.</param>
        /// <param name="key">String Key used to get the map setting item from the list.</param>
        /// <param name="bizdomainValue">Standard choice item value.</param>
        /// <returns>Instance of BizDomainModel.</returns>
        private BizDomainModel4WS GetBizModel4DisplayMap(Dictionary<string, object> dictList, string key, string bizdomainValue)
        {
            BizDomainModel4WS searchModel = GetConfigsBizDomainModel();
            searchModel.bizdomainValue = bizdomainValue;
            BizDomainModel4WS resultModel = _bizBll.GetBizDomainListByModel(searchModel, bizdomainValue, ADMIN_AUDIT_ID);

            if (resultModel == null)
            {
                resultModel = searchModel;
                resultModel.auditID = ADMIN_AUDIT_ID;
            }

            resultModel.description = dictList[key].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Yes : ACAConstant.COMMON_No;
            resultModel.auditStatus = dictList[key].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? "A" : "I";

            return resultModel;
        }

        /// <summary>
        /// This method is get bizDomain model by display option.
        /// </summary>
        /// <param name="dictList">The dictionary that contain DisplayOption value.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The display option.</returns>
        private BizDomainModel4WS GetDisplayOptionModel(Dictionary<string, object> dictList, string moduleName)
        {
            BizDomainModel4WS bizModel;

            bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = moduleName + "_" + BizDomainConstant.STD_INSPECTION_DISPLAYOPTION;

            bizModel = _bizBll.GetBizDomainListByModel(bizModel, moduleName + "_" + BizDomainConstant.STD_INSPECTION_DISPLAYOPTION, ADMIN_AUDIT_ID);

            if (bizModel == null)
            {
                bizModel = GetConfigsBizDomainModel();
                bizModel.description = dictList["DisplayOption"].ToString().Equals("yes", StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Yes : ACAConstant.COMMON_No;
                bizModel.auditStatus = ACAConstant.VALID_STATUS;
                bizModel.auditID = ADMIN_AUDIT_ID;
                bizModel.bizdomainValue = moduleName + "_" + BizDomainConstant.STD_INSPECTION_DISPLAYOPTION;
                return bizModel;
            }
            else
            {
                //bizModel.description = (dictList["DisplayOption"].ToString().ToLower() == "yes") ? ACAConstant.COMMON_Yes : ACAConstant.COMMON_No;
                bizModel.description = dictList["DisplayOption"].ToString().Equals("yes", StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Yes : ACAConstant.COMMON_No;
                return bizModel;
            }
        }

        /// <summary>
        /// This method is get dictionary array.
        /// </summary>
        /// <param name="elementsCollection">The elements collection.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The application status array.</returns>
        private AppStatusGroupModel4WS[] GetAppStatusArray(object[] elementsCollection, string moduleName)
        {
            AppStatusGroupModel4WS treeModel;
            List<AppStatusGroupModel4WS> treeModelArray = new List<AppStatusGroupModel4WS>();

            Dictionary<string, object> subelements = null;
            ArrayList arrayList = new ArrayList();
            ArrayList arrList = null;

            foreach (object elements in elementsCollection)
            {
                arrList = new ArrayList();
                subelements = (Dictionary<string, object>)elements;
                foreach (KeyValuePair<string, object> kvp in subelements)
                {
                    arrList.Add(kvp.Value);
                }

                arrayList.Add(arrList);
            }

            for (int i = 0; i < arrayList.Count; i++)
            {
                arrList = (ArrayList)arrayList[i];
                string check = arrList[3].ToString();
                string groupName = arrList[4].ToString();
                string itemName = ScriptFilter.DecodeJson(arrList[5].ToString());
                string displayRequestTradeLicense = arrList[6].ToString();

                treeModel = new AppStatusGroupModel4WS();

                treeModel.moduleName = moduleName;
                treeModel.appStatusGroupCode = groupName;
                treeModel.status = itemName;
                treeModel.auditID = ADMIN_AUDIT_ID.ToUpperInvariant();
                treeModel.auditStatus = "A";
                treeModel.acaStatus = check.Equals("true", StringComparison.InvariantCultureIgnoreCase) ? "A" : "I";
                treeModel.servProvCode = ConfigManager.AgencyCode;
                treeModel.displayRequestTradeLic = displayRequestTradeLicense;

                treeModelArray.Add(treeModel);
            }

            return treeModelArray.ToArray();
        }

        /// <summary>
        /// Construct Cap Type models
        /// </summary>
        /// <param name="capTypeValues">ArrayList of Cap Type values</param>
        /// <returns>Array of CapTypeModel</returns>
        private CapTypeModel[] ConstructCapTypeModelArray(ArrayList capTypeValues)
        {
            if (capTypeValues == null || capTypeValues.Count == 0)
            {
                return null;
            }

            List<CapTypeModel> capTypes = new List<CapTypeModel>();

            for (int i = 0; i < capTypeValues.Count; i++)
            {
                CapTypeModel capType = new CapTypeModel();
                string[] capTypeKeys = ScriptFilter.DecodeJson(capTypeValues[i].ToString()).Split('/');

                capType.group = capTypeKeys[0];
                capType.type = capTypeKeys[1];
                capType.subType = capTypeKeys[2];
                capType.category = capTypeKeys[3];
                capType.serviceProviderCode = ConfigManager.AgencyCode;

                capTypes.Add(capType);
            }

            return capTypes.ToArray();
        }

        /// <summary>
        /// Save Permission By Cap Type.
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="capTypeName">the cap type name</param>
        /// <param name="selectedItems">the selected items.</param>
        /// <param name="controllerType">the controllerType.</param>
        /// <param name="entityType">the entity type.</param>
        private void SavePermissionByCapType(string moduleName, string capTypeName, object selectedItems, string controllerType, string entityType)
        {
            IAdminCapTypePermissionBll capTypePermissionBll = (IAdminCapTypePermissionBll)ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll));

            string[] capTypeArray = capTypeName.Split(ACAConstant.SPLIT_CHAR);
            int length = ((object[])selectedItems).Length;
            CapTypePermissionModel[] capTypePermissionModels = null;

            if (length > 0)
            {
                capTypePermissionModels = new CapTypePermissionModel[length];

                for (int i = 0; i < length; i++)
                {
                    CapTypePermissionModel capTypePermissionModel = new CapTypePermissionModel();
                    capTypePermissionModel.group = capTypeArray[0];
                    capTypePermissionModel.type = capTypeArray[1];
                    capTypePermissionModel.subType = capTypeArray[2];
                    capTypePermissionModel.category = capTypeArray[3];
                    capTypePermissionModel.serviceProviderCode = ConfigManager.AgencyCode;
                    capTypePermissionModel.moduleName = moduleName;
                    capTypePermissionModel.controllerType = controllerType;
                    capTypePermissionModel.entityType = entityType;
                    capTypePermissionModel.entityPermission = ACAConstant.ROLE_HASPERMISSION;
                    capTypePermissionModel.entityKey1 = Server.HtmlDecode(((object[])selectedItems)[i].ToString());
                    SimpleAuditModel auditModel = new SimpleAuditModel();
                    auditModel.auditID = ACAConstant.ADMIN_CALLER_ID;
                    auditModel.auditStatus = ACAConstant.VALID_STATUS;
                    capTypePermissionModel.auditModel = auditModel;

                    capTypePermissionModels[i] = capTypePermissionModel;
                }

                capTypePermissionBll.SaveCapTypePermissions(capTypePermissionModels);
            }
            else
            {
                CapTypePermissionModel capTypePermissionModel = new CapTypePermissionModel();
                capTypePermissionModel.group = capTypeArray[0];
                capTypePermissionModel.type = capTypeArray[1];
                capTypePermissionModel.subType = capTypeArray[2];
                capTypePermissionModel.category = capTypeArray[3];
                capTypePermissionModel.serviceProviderCode = ConfigManager.AgencyCode;
                capTypePermissionModel.moduleName = moduleName;
                capTypePermissionModel.controllerType = controllerType;
                capTypePermissionModel.entityType = entityType;
                capTypePermissionModel.entityPermission = ACAConstant.ROLE_HASPERMISSION;
                SimpleAuditModel auditModel = new SimpleAuditModel();
                auditModel.auditID = ACAConstant.ADMIN_CALLER_ID;
                auditModel.auditStatus = ACAConstant.VALID_STATUS;
                capTypePermissionModel.auditModel = auditModel;

                capTypePermissionBll.DeleteCapTypePermissions(capTypePermissionModel);
            }
        }

        /// <summary>
        /// Get EntityKey1 List.
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="capTypeName">the cap type name</param>
        /// <param name="controllerType">the controller type.</param>
        /// <param name="entityType">the entity type.</param>
        /// <returns>EntityKey1 list.</returns>
        private ArrayList GetCapTypePermissionEntityKey1List(string moduleName, string capTypeName, string controllerType, string entityType)
        {
            ArrayList array = new ArrayList();
            IAdminCapTypePermissionBll capTypePermissionBll = (IAdminCapTypePermissionBll)ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll));

            string[] capTypeArray = capTypeName.Split(ACAConstant.SPLIT_CHAR);

            CapTypePermissionModel capTypePermissionModel = new CapTypePermissionModel();
            capTypePermissionModel.group = capTypeArray[0];
            capTypePermissionModel.type = capTypeArray[1];
            capTypePermissionModel.subType = capTypeArray[2];
            capTypePermissionModel.category = capTypeArray[3];
            capTypePermissionModel.serviceProviderCode = ConfigManager.AgencyCode;
            capTypePermissionModel.moduleName = moduleName;
            capTypePermissionModel.controllerType = controllerType;
            capTypePermissionModel.entityType = entityType;

            CapTypePermissionModel[] capTypePermissionModels = capTypePermissionBll.GetCapTypePermissions(capTypePermissionModel);

            if (capTypePermissionModels != null && capTypePermissionModels.Length > 0)
            {
                foreach (CapTypePermissionModel model in capTypePermissionModels)
                {
                    array.Add(model.entityKey1);
                }
            }

            return array;
        }

        /// <summary>
        /// Is Section has permission to access
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="capTypeName">the cap type name</param>
        /// <param name="sectionName">the section name.</param>
        /// <returns>true or false</returns>
        private bool IsSectionHasPermission(string moduleName, string capTypeName, string sectionName)
        {
            bool hasPermission = true;

            IAdminCapTypePermissionBll capTypePermissionBll = (IAdminCapTypePermissionBll)ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll));

            string[] capTypeArray = capTypeName.Split(ACAConstant.SPLIT_CHAR);

            CapTypePermissionModel capTypePermissionModel = new CapTypePermissionModel();
            capTypePermissionModel.group = capTypeArray[0];
            capTypePermissionModel.type = capTypeArray[1];
            capTypePermissionModel.subType = capTypeArray[2];
            capTypePermissionModel.category = capTypeArray[3];
            capTypePermissionModel.serviceProviderCode = ConfigManager.AgencyCode;
            capTypePermissionModel.moduleName = moduleName;
            capTypePermissionModel.controllerType = ControllerType.LICENSEVERIFICATION.ToString();
            capTypePermissionModel.entityType = EntityType.SECTIONTYPE.ToString();
            capTypePermissionModel.entityKey1 = sectionName;

            CapTypePermissionModel[] capTypePermissionModels = capTypePermissionBll.GetCapTypePermissions(capTypePermissionModel);

            if (capTypePermissionModels != null && capTypePermissionModels.Length > 0)
            {
                if (ACAConstant.ROLE_NOPERMISSION.Equals(capTypePermissionModels[0].entityPermission))
                {
                    hasPermission = false;
                }
            }
            else
            {
                if (sectionName.Equals(LicenseVerificationSectionType.EDUCATION.ToString())
                    || sectionName.Equals(LicenseVerificationSectionType.EXAMINATION.ToString())
                    || sectionName.Equals(LicenseVerificationSectionType.CONTINUE_EDUCATION.ToString()))
                {
                    hasPermission = false;
                }
            }

            return hasPermission;
        }

        /// <summary>
        /// Get configured url id from XPolicy.
        /// </summary>
        /// <returns>The configured url id.</returns>
        private string GetConfiguredUrlIdFormXPolicy()
        {
            IXPolicyBll xPolicy = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            string policyValue = xPolicy.GetValueByKey(ACAConstant.ACA_SHOPPING_CART_REDIRECT_PAGE, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode);

            return policyValue;
        }

        /// <summary>
        /// Get the url name by url id.
        /// </summary>
        /// <param name="buttonUrlbizDomainList">The bizDomain list.</param>
        /// <param name="selectedUrlId">The selected url id.</param>
        /// <returns>The url name.</returns>
        private string GetUrlNameByUrlId(IList<ItemValue> buttonUrlbizDomainList, string selectedUrlId)
        {
            if (string.IsNullOrEmpty(selectedUrlId))
            {
                return LabelUtil.GetAdminUITextByKey("admin_fieldproperty_buttonurl_select");
            }

            string selectedUrlName = string.Empty;
            foreach (ItemValue buttonUrlbizDomain in buttonUrlbizDomainList)
            {
                if (buttonUrlbizDomain.Key == selectedUrlId)
                {
                    string buttonUrlbizDomainValue = buttonUrlbizDomain.Value.ToString();
                    string[] nameAndUrl = buttonUrlbizDomainValue.Split(',');
                    selectedUrlName = nameAndUrl[0].Split('=')[1];
                }
            }

            return selectedUrlName;
        }

        /// <summary>
        /// Format the configure url list.
        /// </summary>
        /// <param name="buttonUrlbizDomainList">The bizDomain list.</param>
        /// <returns>The formatted configure url list.</returns>
        private string FormatConfigureUrlList(IList<ItemValue> buttonUrlbizDomainList)
        {
            if (buttonUrlbizDomainList == null ||
                buttonUrlbizDomainList.Count < 1)
            {
                return "[[]]";
            }

            StringBuilder jasonStr = new StringBuilder();
            jasonStr.Append("[").Append("['','").Append(LabelUtil.GetAdminUITextByKey("admin_fieldproperty_buttonurl_select")).Append("'],");

            foreach (ItemValue buttonUrlbizDomain in buttonUrlbizDomainList)
            {
                jasonStr.Append("['" + buttonUrlbizDomain.Key + "',");

                string buttonUrlbizDomainValue = buttonUrlbizDomain.Value.ToString();
                string[] nameAndUrl = buttonUrlbizDomainValue.Split(',');
                string name = nameAndUrl[0].Split('=')[1];

                jasonStr.Append("'" + name + "'],");
            }

            jasonStr.Remove(jasonStr.Length - 1, 1);
            jasonStr.Append("]");

            return jasonStr.ToString();
        }

        /// <summary>
        /// Create cap type list to JSON.
        /// </summary>
        /// <param name="filter">The cap type filter.</param>
        /// <returns>The JSON format of the cap type list.</returns>
        private string CreateCapListJson(CapTypeFilterModel4WS filter)
        {
            int index = 0;
            StringBuilder buf = new StringBuilder();

            buf.Append("{'AvailableCapTypes':[");
            if (filter.availableCapTypeList != null &&
                filter.availableCapTypeList.Length > 0)
            {
                foreach (CapTypeModel capTypeModel in filter.availableCapTypeList)
                {
                    buf.Append(CreateTreeNodeJson(capTypeModel, index));
                    index++;
                }

                buf.Remove(buf.Length - 1, 1);
            }

            buf.Append("],");

            buf.Append("'SelectedCapTypes':[");
            if (filter.filteredCapTypeList != null &&
                filter.filteredCapTypeList.Length > 0)
            {
                foreach (CapTypeModel capTypeModel in filter.filteredCapTypeList)
                {
                    buf.Append(CreateTreeNodeJson(capTypeModel, index));
                    index++;
                }
            }

            buf.Append("]}");

            return buf.ToString();
        }

        /// <summary>
        /// Create tree node to JSON.
        /// </summary>
        /// <param name="capTypeModel">The cap type model.</param>
        /// <param name="index">The index.</param>
        /// <returns>The tree node's JSON.</returns>
        private string CreateTreeNodeJson(CapTypeModel capTypeModel, int index)
        {
            StringBuilder buf = new StringBuilder();

            string displayText = CAPHelper.GetAliasOrCapTypeLabel(capTypeModel); // !string.IsNullOrEmpty(capTypeModel.resAlias) ? capTypeModel.resAlias : capTypeModel.alias;
            string capTypeValue = CAPHelper.FormatCapTypeValue(capTypeModel);

            if (displayText == null)
            {
                displayText = string.Empty;
            }

            buf.Append("{");
            buf.Append("id:");
            buf.Append(index);
            buf.Append(",text:'");
            buf.Append(ScriptFilter.EncodeJson(displayText));
            buf.Append("',key:'");
            buf.Append(ScriptFilter.EncodeJson(capTypeValue));
            buf.Append("',aliasName:'");
            buf.Append(ScriptFilter.EncodeJson(capTypeModel.alias));
            buf.Append("',anon:'");
            buf.Append(capTypeModel.anonymousAllowed);
            buf.Append("',registered:'");
            buf.Append(capTypeModel.registeredAllowed);
            buf.Append("',LP:'");
            buf.Append(capTypeModel.licensedProfessionalAllowed);
            buf.Append("',leaf:true,draggable:true");
            buf.Append("},");

            return buf.ToString();
        }

        /// <summary>
        /// Create tree json string for CAP types
        /// </summary>
        /// <param name="capTypeModel">CapTypeModel object</param>
        /// <param name="index">CAP type index</param>
        /// <returns>json string for tree nodes</returns>
        private string CreateAmendmentTreeNodeJson(CapTypeModel capTypeModel, int index)
        {
            string displayText = CAPHelper.GetAliasOrCapTypeLabel(capTypeModel); //!string.IsNullOrEmpty(capTypeModel.resAlias) ? capTypeModel.resAlias : capTypeModel.alias;
            string capTypeValue = CAPHelper.GetCapTypeValue(capTypeModel);

            if (displayText == null)
            {
                displayText = string.Empty;
            }

            StringBuilder buf = new StringBuilder();

            buf.Append("{");
            buf.Append("id:");
            buf.Append(index);
            buf.Append(",text:'");
            buf.Append(ScriptFilter.EncodeJson(displayText));
            buf.Append("',key:'");
            buf.Append(ScriptFilter.EncodeJson(capTypeValue));

            buf.Append("',binded:");
            buf.Append(capTypeModel == null && string.IsNullOrEmpty(capTypeModel.smartChoiceCode4ACA) ? "false" : "true");
            buf.Append(",leaf:true,draggable:true");
            buf.Append("},");

            return buf.ToString();
        }

        /// <summary>
        /// Format filter name.
        /// </summary>
        /// <param name="filterModels">The filter model list.</param>
        /// <returns>The formatted filter name.</returns>
        private string FormatFilterNameList(ArrayList filterModels)
        {
            if (filterModels == null ||
                filterModels.Count < 1)
            {
                return "[[]]";
            }

            StringBuilder jasonStr = new StringBuilder();
            jasonStr.Append("[");

            Dictionary<string, string> moduleList = TabUtil.GetAllEnableModules(true);

            string moduleName = ((CapTypeFilterModel4WS)filterModels[0]).moduleName;

            if (moduleList.ContainsKey(moduleName))
            {
                jasonStr.Append("['ModuleName','-----");
                jasonStr.Append(moduleList[moduleName]);
                jasonStr.Append("-----'],");
            }

            foreach (CapTypeFilterModel4WS filterModel in filterModels)
            {
                if (!moduleList.ContainsKey(filterModel.moduleName))
                {
                    continue;
                }

                if (moduleName != filterModel.moduleName)
                {
                    moduleName = filterModel.moduleName;

                    jasonStr.Append("['ModuleName','-----");
                    jasonStr.Append(moduleList[filterModel.moduleName]);
                    jasonStr.Append("-----'],");
                }

                if (filterModel.moduleName.Equals(moduleName))
                {
                    jasonStr.Append("['" + moduleName + "',");
                    jasonStr.Append("'" + ScriptFilter.EncodeJson(filterModel.filterName) + "'],");
                }
            }

            jasonStr.Remove(jasonStr.Length - 1, 1);
            jasonStr.Append("]");

            return jasonStr.ToString();
        }

        /// <summary>
        /// Covert array to JSON.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>The converted JSON.</returns>
        private string ConvertArrayToJson(string[] array)
        {
            if (array == null ||
                array.Length < 1)
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
        /// construct filter model.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        /// <returns>The CapTypeFilterModel4WS.</returns>
        private CapTypeFilterModel4WS ContructFilterModel(object filterItem)
        {
            Dictionary<string, object> collectionDC = (Dictionary<string, object>)filterItem;
            CapTypeFilterModel4WS filterModel = new CapTypeFilterModel4WS();

            string filterName = collectionDC["FilterName"].ToString();
            string moduleName = collectionDC["ModuleName"].ToString();
            object[] capTypeList = (object[])collectionDC["CapTypeList"];
            List<CapTypeModel> filterCapTypeList = new List<CapTypeModel>();

            foreach (object capType in capTypeList)
            {
                CapTypeModel capTypeModel = MappingCollection2ViewsForCapType(capType);

                filterCapTypeList.Add(capTypeModel);
            }

            filterModel.filteredCapTypeList = (CapTypeModel[])filterCapTypeList.ToArray();
            filterModel.filterName = filterName;
            filterModel.servProvCode = ConfigManager.AgencyCode;
            filterModel.moduleName = moduleName;

            return filterModel;
        }

        /// <summary>
        /// Mapping collection to view for cap type.
        /// </summary>
        /// <param name="capType">The cap type.</param>
        /// <returns>The CapTypeModel.</returns>
        private CapTypeModel MappingCollection2ViewsForCapType(object capType)
        {
            Dictionary<string, object> elementDC = (Dictionary<string, object>)capType;
            CapTypeModel capTypeModel = new CapTypeModel();

            foreach (KeyValuePair<string, object> skvp in elementDC)
            {
                switch (skvp.Key)
                {
                    case "aliasName":
                        capTypeModel.alias = ScriptFilter.DecodeJson(skvp.Value.ToString());
                        break;
                    case "anon":
                        capTypeModel.anonymousAllowed = skvp.Value.ToString();
                        break;
                    case "registered":
                        capTypeModel.registeredAllowed = skvp.Value.ToString();
                        break;
                    case "LP":
                        capTypeModel.licensedProfessionalAllowed = skvp.Value.ToString();
                        break;
                }
            }

            capTypeModel.serviceProviderCode = ConfigManager.AgencyCode;
            capTypeModel.moduleName = _moduleName;

            return capTypeModel;
        }

        /// <summary>
        /// Get inspection action permissions by saveData
        /// </summary>
        /// <param name="saveDatas">array for saveData</param>
        /// <returns>array for InspectionActionPermissionModel</returns>
        private InspectionActionPermissionModel[] BuildInsActionPermissionBySaveDatas(string[][] saveDatas)
        {
            ArrayList arrayList = new ArrayList();

            if (saveDatas != null && saveDatas.Length > 0)
            {
                for (int i = 0; i < saveDatas.Length; i++)
                {
                    InspectionActionPermissionModel insActionPermission = new InspectionActionPermissionModel();

                    InspectionTypeModel inspectionType = new InspectionTypeModel();
                    inspectionType.sequenceNumber = long.Parse(saveDatas[i][0]);
                    inspectionType.type = saveDatas[i][1];
                    insActionPermission.inspectionTypeModel = inspectionType;
                    insActionPermission.auditID = ADMIN_AUDIT_ID;
                    insActionPermission.auditStatus = "A";

                    insActionPermission.serviceProvideCode = saveDatas[i][2];
                    insActionPermission.sequenceNumber = long.Parse(saveDatas[i][3]);
                    insActionPermission.actionCode = saveDatas[i][4];
                    insActionPermission.enabled = saveDatas[i][5] == ACAConstant.COMMON_ZERO ? false : true;

                    AppStatusGroupModel appStatusGroup = new AppStatusGroupModel();
                    appStatusGroup.appStatusGroupCode = saveDatas[i][6];
                    appStatusGroup.status = saveDatas[i][7];
                    insActionPermission.appStatusGroupModel = appStatusGroup;

                    arrayList.Add(insActionPermission);
                }
            }

            return (InspectionActionPermissionModel[])arrayList.ToArray(typeof(InspectionActionPermissionModel));
        }

        /// <summary>
        /// Get Inspection action permission list
        /// </summary>
        /// <param name="inspectionTypeDataRowInfos">a inspection template info</param>
        /// <returns>array list for inspection action permission</returns>
        private ArrayList ConvertInsActionPermissionsToArray(InspectionTypeDataRowInfo[] inspectionTypeDataRowInfos)
        {
            ArrayList array = new ArrayList();

            if (inspectionTypeDataRowInfos != null && inspectionTypeDataRowInfos.Length > 0)
            {
                foreach (InspectionTypeDataRowInfo insActionPermissionInfo in inspectionTypeDataRowInfos)
                {
                    if (insActionPermissionInfo != null)
                    {
                        //format: InsTypeSeqNbr, InsType, InsActionPermissionAgency/fInsActionPermissionSeqNbr/fRequest/f enabled, InsActionPermissionAgency/fInsActionPermissionSeqNbr/fSchedule/f enabled, InsActionPermissionAgency/fInsActionPermissionSeqNbr/fReSchedule/fenabled, InsActionPermissionAgency/fInsActionPermissionSeqNbr/fCannel/f Enabled
                        StringBuilder sb = new StringBuilder();
                        sb.Append(ScriptFilter.EncodeJson(insActionPermissionInfo.InsTypeSeqNbr.ToString()));
                        sb.Append(ACAConstant.COMMA);
                        sb.Append(ScriptFilter.EncodeJson(insActionPermissionInfo.InsType));
                        sb.Append(ACAConstant.COMMA);
                        sb.Append(FormatInsActionColumn(insActionPermissionInfo.Schedule, InspectionAction.Schedule.ToString()));
                        sb.Append(ACAConstant.COMMA);
                        sb.Append(FormatInsActionColumn(insActionPermissionInfo.ReSchedule, InspectionAction.Reschedule.ToString()));
                        sb.Append(ACAConstant.COMMA);
                        sb.Append(FormatInsActionColumn(insActionPermissionInfo.Cancel, InspectionAction.Cancel.ToString()));

                        array.Add(sb.ToString());
                    }
                }
            }

            return array;
        }

        /// <summary>
        /// Format inspection action column
        /// </summary>
        /// <param name="inspectionActionColumn">inspection type data row action column</param>
        /// <param name="actionType">action code,one of request/schedule/reschedule/cancel</param>
        /// <returns>format string action column</returns>
        private string FormatInsActionColumn(InspectionActionColumn inspectionActionColumn, string actionType)
        {
            string flagEnabled = inspectionActionColumn.Enabled ? ACAConstant.COMMON_ONE : ACAConstant.COMMON_ZERO;
            return string.Format("{0}{1}{2}{3}{4}{5}{6}", ScriptFilter.EncodeJson(inspectionActionColumn.InsActionPermissionAgency), ACAConstant.SPLIT_CHAR, ScriptFilter.EncodeJson(inspectionActionColumn.InsActionPermissionSeqNbr.ToString()), ACAConstant.SPLIT_CHAR, actionType, ACAConstant.SPLIT_CHAR, flagEnabled);
        }

        /// <summary>
        /// Set inspection type data row action column enabled by inspection action permission setting records in DB.
        /// </summary>
        /// <param name="insTypeDataRows">inspection type data rows</param>
        /// <param name="insActionPermissionRecords">inspection action permission records in DB.</param>
        private void SetInsActionColumnEnable(ref InspectionTypeDataRowInfo[] insTypeDataRows, IList<InspectionActionPermissionModel> insActionPermissionRecords)
        {
            if (insTypeDataRows == null || insActionPermissionRecords == null || insTypeDataRows.Length == 0 || insActionPermissionRecords.Count == 0)
            {
                return;
            }

            //foreach for inspection type data rows.
            foreach (InspectionTypeDataRowInfo dataRow in insTypeDataRows)
            {
                if (dataRow == null)
                {
                    continue;
                }

                //foreach for inspection action permission record.
                foreach (InspectionActionPermissionModel insActionPermissionRecord in insActionPermissionRecords)
                {
                    if (insActionPermissionRecord == null)
                    {
                        continue;
                    }

                    //inspection type seqNbr been assoicated in inspection action permisson.
                    long recordInsTypeSeqNbr = 0;

                    if (insActionPermissionRecord.inspectionTypeModel != null)
                    {
                        recordInsTypeSeqNbr = insActionPermissionRecord.inspectionTypeModel.sequenceNumber;
                    }

                    //current data row  inspection type seqNbr match with Inspection type seqNbr in inspection action permission record 
                    if (dataRow.InsTypeSeqNbr == recordInsTypeSeqNbr)
                    {
                        //action code in inspection action permission record.
                        string actionCodeInRecord = insActionPermissionRecord.actionCode;

                        //BuildIns action column by agency, seqNbr and enabled in inspection action permission record
                        InspectionActionColumn insActionColumn = BuildInsActionColumn(insActionPermissionRecord.serviceProvideCode, insActionPermissionRecord.sequenceNumber, insActionPermissionRecord.enabled);

                        //match with action code in inspection action permission record, and set action column agency, seqNbr and enabled to actio column in data row.  
                        if (InspectionAction.Schedule.ToString().Equals(actionCodeInRecord))
                        {
                            dataRow.Schedule = insActionColumn;
                        }
                        else if (InspectionAction.Reschedule.ToString().Equals(actionCodeInRecord))
                        {
                            dataRow.ReSchedule = insActionColumn;
                        }
                        else if (InspectionAction.Cancel.ToString().Equals(actionCodeInRecord))
                        {
                            dataRow.Cancel = insActionColumn;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Build inspection type data row array by inspection types. the data row key is inspection type sequence.
        /// </summary>
        /// <param name="insTypes">inspection type models</param>
        /// <returns>array for InspectionTypeDataRowInfo</returns>
        private InspectionTypeDataRowInfo[] BuildInsTypeDataRowsByInsTypes(InspectionTypeModel[] insTypes)
        {
            ArrayList arrayList = new ArrayList();

            if (insTypes != null && insTypes.Length > 0)
            {
                foreach (InspectionTypeModel insType in insTypes)
                {
                    InspectionTypeDataRowInfo dataRow = new InspectionTypeDataRowInfo();

                    dataRow.InsTypeSeqNbr = insType.sequenceNumber;
                    dataRow.InsType = I18nStringUtil.GetString(insType.resType, insType.type);

                    //defalut all action is enabled.
                    dataRow.ReSchedule = BuildInsActionColumn(_servProvCode, 0, true);
                    dataRow.Schedule = BuildInsActionColumn(_servProvCode, 0, true);
                    dataRow.Cancel = BuildInsActionColumn(_servProvCode, 0, true);
                    dataRow.RequestOrSchedule = InspectionAction.Schedule;

                    arrayList.Add(dataRow);
                }
            }

            return (InspectionTypeDataRowInfo[])arrayList.ToArray(typeof(InspectionTypeDataRowInfo));
        }

        /// <summary>
        /// Build inspection action column. it include action permission agency, permission sequence number and enabled.
        /// </summary>
        /// <param name="insActionPermissionAgency">inspection action permission agency</param>
        /// <param name="insActionPermissionSeqNbr">inspection action permission sequence number</param>
        /// <param name="enabled">the enabled property.</param>
        /// <returns>a InspectionActionColumn, it includes action permission agency, permission sequence number and enabled.</returns>
        private InspectionActionColumn BuildInsActionColumn(string insActionPermissionAgency, long insActionPermissionSeqNbr, bool enabled)
        {
            InspectionActionColumn actionColumn = new InspectionActionColumn();

            actionColumn.InsActionPermissionAgency = insActionPermissionAgency;
            actionColumn.InsActionPermissionSeqNbr = insActionPermissionSeqNbr;
            actionColumn.Enabled = enabled;

            return actionColumn;
        }

        /// <summary>
        /// Set some common value for policy model
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>XPolicyModel object</returns>
        private XPolicyModel GetCommonPolicy(string moduleName)
        {
            XPolicyModel policyModel = new XPolicyModel();
            policyModel.policyName = BizDomainConstant.STD_CAT_ACA_CONFIGS; //"ACA_CONFIGS"
            policyModel.serviceProviderCode = this._servProvCode;
            policyModel.rightGranted = ACAConstant.GRANTED_RIGHT; //"ACA"
            policyModel.status = ACAConstant.VALID_STATUS;  // "A"
            policyModel.recStatus = ACAConstant.VALID_STATUS;
            policyModel.recFullName = ACAConstant.ADMIN_CALLER_ID;

            if (string.IsNullOrEmpty(moduleName))
            {
                // global level
                policyModel.level = ACAConstant.LEVEL_TYPE_AGENCY;
                policyModel.levelData = this._servProvCode;
            }
            else
            {
                policyModel.level = ACAConstant.LEVEL_TYPE_MODULE;
                policyModel.levelData = moduleName;
            }

            return policyModel;
        }

        /// <summary>
        /// Get license type list by the XPolicy data.
        /// </summary>
        /// <param name="policyCategory">the XPolicy category</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="policyData4">the XPolicy data4</param>
        /// <returns>Return the license type list.</returns>
        private string GetLicenseTypeListByXpolicy(string policyCategory, string moduleName, string policyData4)
        {
            ArrayList selectedLTList = new ArrayList();
            int isChecked = 0;

            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            SortedList licenseTypeList = licenseBll.GetLicenseTypes(ConfigManager.AgencyCode);

            XPolicyModel[] xpolicys = _xPolicyBll.GetPolicyListByCategory(policyCategory, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            if (xpolicys != null && xpolicys.Length > 0)
            {
                foreach (XPolicyModel xpolicy in xpolicys)
                {
                    if (policyData4.Equals(xpolicy.data4) && EntityType.LICENSETYPE.ToString().Equals(xpolicy.data3))
                    {
                        if (string.IsNullOrEmpty(xpolicy.data2))
                        {
                            continue;
                        }

                        selectedLTList = ArrayList.Adapter(xpolicy.data2.Split(new string[] { ACAConstant.SPLIT_DOUBLE_VERTICAL }, StringSplitOptions.None));
                    }
                }
            }

            StringBuilder jasonLicenseType = new StringBuilder();
            jasonLicenseType.Append("{'LisenTypeList':[");
            IDictionaryEnumerator enumerator = licenseTypeList.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (selectedLTList.Contains(enumerator.Key))
                {
                    isChecked = 1; //1:checked,0:no checked.
                }
                else
                {
                    isChecked = 0;
                }

                jasonLicenseType.Append("{");
                jasonLicenseType.Append("Key:'").Append(ScriptFilter.EncodeJson(enumerator.Key.ToString()));
                jasonLicenseType.Append("',Text:'").Append(ScriptFilter.EncodeJson(enumerator.Value.ToString()));
                jasonLicenseType.Append("',Checked:").Append(isChecked);
                jasonLicenseType.Append("},");
            }

            jasonLicenseType.Remove(jasonLicenseType.Length - 1, 1);
            jasonLicenseType.Append("]}");

            return jasonLicenseType.ToString();
        }

        /// <summary>
        /// Save license type list by XPolicy.
        /// </summary>
        /// <param name="policyKey">the policy key</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="policyData4">the policy data4</param>
        /// <param name="selectedLPTypes">the selected license type from UI</param>
        /// <returns>Return true if save successful, or return false.</returns>
        private bool SaveLicenseTypeListByXpolicy(string policyKey, string moduleName, string policyData4, string[] selectedLPTypes)
        {
            string entityType = EntityType.LICENSETYPE.ToString();
            StringBuilder licenseType = new StringBuilder();

            foreach (string selectedLPType in selectedLPTypes)
            {
                licenseType.Append(selectedLPType);
                licenseType.Append(ACAConstant.SPLIT_DOUBLE_VERTICAL);
            }

            if (licenseType.Length > 2)
            {
                licenseType.Remove(licenseType.Length - 2, 2);
            }

            XPolicyModel xpolicy = GetXPolicyModel(ACAConstant.LEVEL_TYPE_MODULE, moduleName, policyKey, licenseType.ToString(), entityType, policyData4);

            XPolicyModel[] xpolicies = new XPolicyModel[1] { xpolicy };

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            xPolicyBll.CreateOrUpdateXPolicyForData3AsKey(null, xpolicies);

            return true;
        }

        /// <summary>
        /// Get inspection contact's user role permission.
        /// </summary>
        /// <param name="policyName">The policy name.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The inspection contact's user role permission.</returns>
        private string GetInspectionContactUserRolePermission(string policyName, string moduleName)
        {
            string userRolePermission = string.Empty;

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XPolicyModel[] xpolicys = xPolicyBll.GetPolicyListByCategory(policyName, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            if (xpolicys != null && xpolicys.Length > 0)
            {
                foreach (XPolicyModel xpolicy in xpolicys)
                {
                    if (xpolicy.serviceProviderCode == _servProvCode)
                    {
                        userRolePermission = xpolicy.data2;
                        break;
                    }
                }
            }

            return userRolePermission;
        }

        /// <summary>
        /// Save grid view page size.
        /// </summary>
        /// <param name="elementCollect">The element collection.</param>
        private void SaveGridViewPageSize(object[] elementCollect)
        {
            foreach (object element in elementCollect)
            {
                Dictionary<string, object> elementDC = (Dictionary<string, object>)element;
                string levelData = string.Empty;
                string gridViewID = string.Empty;
                string pageSize = string.Empty;
                string pageSizeKey = string.Empty;

                if (elementDC != null)
                {
                    levelData = Convert.ToString(elementDC["LevelData"]);
                    levelData = string.IsNullOrEmpty(levelData) ? _selectedAgency : levelData;
                    pageSize = Convert.ToString(elementDC["PageSize"]);
                    gridViewID = Convert.ToString(elementDC["GridViewId"]);
                    pageSizeKey = string.Format("{0}_{1}", ACAConstant.ACA_PAGE_SIZE, gridViewID);
                }

                XPolicyModel gridViewPageSize = GetXPolicyModel(_levelType, levelData, pageSizeKey, pageSize);
                _xPolicyList.Add(gridViewPageSize);
            }
        }

        #region service group private

        /// <summary>
        /// Construct XService Group Model
        /// </summary>
        /// <param name="filterItem">service and group json info</param>
        /// <returns>return the XServiceGroupModel</returns>
        private XServiceGroupModel[] ContructServiceFilterModel(object filterItem)
        {
            Dictionary<string, object> collectionDC = (Dictionary<string, object>)filterItem;
            List<XServiceGroupModel> result = new List<XServiceGroupModel>();

            //Group Info
            Dictionary<string, object> groupDC = (Dictionary<string, object>)collectionDC["GroupInfo"];
            ServiceGroupModel group = new ServiceGroupModel();
            group.serviceProviderCode = ConfigManager.AgencyCode;
            group.groupCode = ScriptFilter.DecodeJson(groupDC["GroupName"].ToString());

            int sortOrder = 0;
            long serviceGroupSeqNbr = 0;

            if (int.TryParse(groupDC["sortOrder"].ToString(), out sortOrder))
            {
                group.sortOrder = sortOrder;
            }
            else
            {
                group.sortOrder = null;
            }

            if (long.TryParse(groupDC["serviceGroupSeqNbr"].ToString(), out serviceGroupSeqNbr))
            {
                group.serviceGroupSeqNbr = serviceGroupSeqNbr;
            }
            else
            {
                group.serviceGroupSeqNbr = 0;
            }

            //service info
            object[] serviceList = (object[])collectionDC["ServiceList"];

            if (serviceList != null && serviceList.Length > 0)
            {
                foreach (object service in serviceList)
                {
                    Dictionary<string, object> elementDC = (Dictionary<string, object>)service;
                    ServiceModel serviceModel = new ServiceModel();
                    serviceModel.servPorvCode = elementDC["agencyCode"].ToString();
                    serviceModel.sourceNumber = int.Parse(elementDC["sourceNumber"].ToString());

                    XServiceGroupModel item = new XServiceGroupModel();
                    item.group = group;
                    item.service = serviceModel;
                    item.serviceProviderCode = ConfigManager.AgencyCode;
                    item.sortOrder = int.Parse(elementDC["sortOrder"].ToString());

                    result.Add(item);
                }
            }
            else
            {
                // if the group was not associated with service, need save group info
                XServiceGroupModel item = new XServiceGroupModel();
                item.group = group;
                item.serviceProviderCode = ConfigManager.AgencyCode;
                result.Add(item);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get the services in ToString.
        /// </summary>
        /// <param name="serviceModel">the serviceModel</param>
        /// <param name="sortOrder">the sort sortOrder</param>
        /// <returns>the service json format content</returns>
        private string CreateServiceNodeJson(ServiceModel serviceModel, int sortOrder)
        {
            StringBuilder buf = new StringBuilder();

            string displayServiceName = string.Format(
                                                    "{0}({1})",
                                                    I18nStringUtil.GetString(serviceModel.resServiceName, serviceModel.serviceName),
                                                    serviceModel.servPorvCode);

            buf.Append("{");
            buf.AppendFormat("sortOrder:{0},", sortOrder);
            buf.AppendFormat("agencyCode:'{0}',", serviceModel.servPorvCode);
            buf.AppendFormat("text:'{0}',", ScriptFilter.EncodeJson(displayServiceName));
            buf.AppendFormat("key:{0},", serviceModel.sourceNumber);
            buf.Append("leaf:true,");
            buf.Append("draggable:true");
            buf.Append("},");

            return buf.ToString();
        }

        #endregion service group private        

        /// <summary>
        /// Get reset password settings.
        /// </summary>
        /// <returns>reset info</returns>
        private string GetEnableResetPasswordOnCombineDataInfo()
        {
            StringBuilder registration = new StringBuilder();
            string enableResetPasswordOnCombine = _xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_RESETPASSWORD_ON_COMBINE);

            if (string.IsNullOrEmpty(enableResetPasswordOnCombine))
            {
                enableResetPasswordOnCombine = ACAConstant.COMMON_N;
            }

            registration.AppendFormat(",\"EnableResetPasswordOnCombine\":\"{0}\"", enableResetPasswordOnCombine);
            return registration.ToString();
        }

        /// <summary>
        /// Get Login settings.
        /// </summary>
        /// <returns>login data info</returns>
        private string GetEnableLoginOnRegistrationDataInfo()
        {
            StringBuilder registration = new StringBuilder();

            // Password expiration setting
            XPolicyModel[] xPolicyList = _xPolicyBll.GetPolicyListByCategory(XPolicyConstant.ENABLE_LOGIN_ON_REGISTRATION, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode);

            if (xPolicyList == null || xPolicyList.Length == 0)
            {
                registration.AppendFormat(",\"EnableLoginOnRegistration\":\"{0}\"", ACAConstant.COMMON_N);
                registration.AppendFormat(",\"LoginExpireTime\":\"{0}\"", string.Empty);
            }
            else
            {
                XPolicyModel xpolicy = xPolicyList[0];

                registration.AppendFormat(",\"EnableLoginOnRegistration\":\"{0}\"", !string.IsNullOrEmpty(xpolicy.data2) ? xpolicy.data2 : ACAConstant.COMMON_N);
                registration.AppendFormat(",\"LoginExpireTime\":\"{0}\"", !string.IsNullOrEmpty(xpolicy.data3) ? xpolicy.data3 : string.Empty);
            }

            return registration.ToString();
        }

        /// <summary>
        /// create biz model
        /// </summary>
        /// <param name="bizDomainValue">biz domain value.</param>
        /// <param name="array">an array, the result set.</param>
        private void CreateBizModel4GisData(string bizDomainValue, ArrayList array)
        {
            BizDomainModel4WS bizModel = GetConfigsBizDomainModel();
            bizModel.bizdomainValue = bizDomainValue;
            bizModel = _bizBll.GetBizDomainListByModel(bizModel, ADMIN_AUDIT_ID, true);

            if (bizModel != null)
            {
                array.Add(bizModel.auditStatus ?? "I");
                array.Add(bizModel.description ?? string.Empty);
            }
            else
            {
                array.Add("I");
                array.Add(string.Empty);
            }
        }

        #region Update XEntityPerssion

        /// <summary>
        /// Mapping the collection to XEntityPermissionModel and save.
        /// </summary>
        /// <param name="extenseObjects">extense Object array. Format[{extenseType,extenseItems}]</param>
        private void UpdateXEntityPermissionModels(object[] extenseObjects)
        {
            if (extenseObjects == null || extenseObjects.Length == 0)
            {
                return;
            }

            foreach (object extenseObject in extenseObjects)
            {
                Dictionary<string, object> extenseObjectDC = (Dictionary<string, object>)extenseObject;
                string extenseType = extenseObjectDC["extenseType"].ToString();

                object[] extenseItems = extenseObjectDC["extenseItems"] as object[];
                UpdateXEntityPermissionModels(extenseType, extenseItems);
            }
        }

        /// <summary>
        /// Mapping the collection to XEntityPermissionModel and save
        /// </summary>
        /// <param name="extenseType">extense type</param>
        /// <param name="extenseItems">extense item array. Format :[{extenseKey,deleteOption,updateOptions}]</param>
        private void UpdateXEntityPermissionModels(string extenseType, object[] extenseItems)
        {
            if (extenseItems == null || extenseItems.Length == 0)
            {
                return;
            }

            foreach (object extenseItem in extenseItems)
            {
                Dictionary<string, object> extenseItemDC = (Dictionary<string, object>)extenseItem;
                object deleteOption = extenseItemDC["deleteOption"];
                object[] updateOptions = extenseItemDC["updateOptions"] as object[];
                UpdateXEntityPermissionModels(extenseType, deleteOption, updateOptions);
            }
        }

        /// <summary>
        /// Mapping the collection to XEntityPermissionModel and save
        /// </summary>
        /// <param name="extenseType">extense type</param>
        /// <param name="deleteOption">delete extense options. Format:{entityId,entityId2,entityId3,entityId4}</param>
        /// <param name="updateOptions">update extense options. Format:[{entityId,entityId2,entityId3,entityId4,value}]</param>
        private void UpdateXEntityPermissionModels(string extenseType, object deleteOption, object[] updateOptions)
        {
            Dictionary<string, object> deleteOptionDC = (Dictionary<string, object>)deleteOption;

            XEntityPermissionModel deleteEntityPermissionModel = new XEntityPermissionModel();
            deleteEntityPermissionModel.servProvCode = ConfigManager.AgencyCode;
            deleteEntityPermissionModel.entityType = extenseType;

            if (deleteOptionDC.ContainsKey("entityId4"))
            {
                deleteEntityPermissionModel.entityId4 = ScriptFilter.DecodeJson(deleteOptionDC["entityId4"].ToString());
            }

            if (deleteOptionDC.ContainsKey("entityId3"))
            {
                deleteEntityPermissionModel.entityId3 = ScriptFilter.DecodeJson(deleteOptionDC["entityId3"].ToString());
            }

            if (deleteOptionDC.ContainsKey("entityId2"))
            {
                deleteEntityPermissionModel.entityId2 = ScriptFilter.DecodeJson(deleteOptionDC["entityId2"].ToString());
            }

            if (deleteOptionDC.ContainsKey("entityId"))
            {
                deleteEntityPermissionModel.entityId = ScriptFilter.DecodeJson(deleteOptionDC["entityId"].ToString());
            }

            if (deleteOptionDC.ContainsKey("entityType"))
            {
                deleteEntityPermissionModel.entityType = ScriptFilter.DecodeJson(deleteOptionDC["entityType"].ToString());
            }

            List<XEntityPermissionModel> updateEntityPermissionModels = new List<XEntityPermissionModel>();

            if (updateOptions != null && updateOptions.Length > 0)
            {
                foreach (object extenseItem in updateOptions)
                {
                    Dictionary<string, object> elementDC = (Dictionary<string, object>)extenseItem;
                    XEntityPermissionModel xEntity = new XEntityPermissionModel();
                    xEntity.servProvCode = ConfigManager.AgencyCode;
                    xEntity.entityType = extenseType;

                    if (elementDC.ContainsKey("entityId4"))
                    {
                        xEntity.entityId4 = ScriptFilter.DecodeJson(elementDC["entityId4"].ToString());
                    }

                    if (elementDC.ContainsKey("entityId3"))
                    {
                        xEntity.entityId3 = ScriptFilter.DecodeJson(elementDC["entityId3"].ToString());
                    }

                    if (elementDC.ContainsKey("entityId2"))
                    {
                        xEntity.entityId2 = ScriptFilter.DecodeJson(elementDC["entityId2"].ToString());
                    }

                    if (elementDC.ContainsKey("entityId"))
                    {
                        xEntity.entityId = ScriptFilter.DecodeJson(elementDC["entityId"].ToString());
                    }

                    if (elementDC.ContainsKey("value"))
                    {
                        xEntity.permissionValue = elementDC["value"].ToString();
                    }

                    xEntity.recFulNam = ACAConstant.ADMIN_CALLER_ID;
                    xEntity.recStatus = ACAConstant.VALID_STATUS;
                    updateEntityPermissionModels.Add(xEntity);
                }

                IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
                xEntityPermissionBll.UpdateXEntityPermissions(deleteEntityPermissionModel, updateEntityPermissionModels);
            }
        }

        #endregion Update XEntityPerssion

        #endregion private Method
    }
}
