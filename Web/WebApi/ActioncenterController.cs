#region

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ActioncenterController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:ActioncenterController.cs 77905 2014-06-11 12:49:28Z ACHIEVO\dennis.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Accela.ACA.BLL;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.Web.WebApi.Entity;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Action center Controller
    /// </summary>
    public class ActioncenterController : ApiController
    {
        /// <summary>
        /// A Map controller
        /// </summary>
        private MapController mapHelper = new MapController();

        #region LanchPad
        /// <summary>
        /// Get all modules by action
        /// </summary>
        /// <param name="actionKey">Apply ,ObtainFeeEstimate ,ScheduleAnInspection </param>
        /// <returns>all modules</returns>
        [ActionName("Modules")]
        public HttpResponseMessage GetModules(string actionKey)
        {
            string result = string.Empty;
            IList<TabItem> tabsList = GetTabsList();
            IList<ModuleEntity> modules = new List<ModuleEntity>();
            if (tabsList != null && tabsList.Count > 0)
            {
                foreach (var tabItem in tabsList)
                {
                    string module = tabItem.Title;
                    string linkUrl;
                    bool isShowModule = IsActionKeyModule(module, actionKey, out linkUrl);

                    if (isShowModule)
                    {
                        if ("APO".Equals(tabItem.Key, StringComparison.InvariantCultureIgnoreCase)  && tabItem.Children != null && tabItem.Children.Count > 0)
                        {
                            foreach (LinkItem subLinkItem in tabItem.Children)
                            {
                                ModuleEntity moduleEntity = new ModuleEntity();
                                moduleEntity.ModuleTitle = LabelUtil.GetTextByKey(subLinkItem.Label, string.Empty);
                                moduleEntity.Module = tabItem.Module;
                                moduleEntity.Url = subLinkItem.Url;
                                modules.Add(moduleEntity);
                            }
                        }
                        else
                        {
                            if (tabItem.Module != "ServiceRequest" || "Search".Equals(actionKey, StringComparison.InvariantCultureIgnoreCase))
                            {
                                ModuleEntity moduleEntity = new ModuleEntity();
                                moduleEntity.ModuleTitle = tabItem.Title;
                                moduleEntity.Module = tabItem.Module;
                                moduleEntity.Url = linkUrl;
                                modules.Add(moduleEntity);
                            }
                        }
                    }
                }
            }

            if (modules.Count > 0)
            {
                result = "{\"modules\":" + JsonConvert.SerializeObject(modules) + "}";
            }
            else
            {
                result = "{\"modules\":\"\"}";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// Get SUB Modules Info
        /// </summary>
        /// <param name="callerName">caller Name</param>
        /// <param name="actionKey">action Key</param>
        /// <param name="mapData">map Data</param>
        /// <returns>Sub Modules Information</returns>
        [ActionName("SubModules")]
        public HttpResponseMessage GetSubModulesInfo(string callerName, string actionKey, string mapData)
        {
            //if the url come form map then Perform the following method
            if (!string.IsNullOrEmpty(mapData))
            {
                dynamic map = JsonConvert.DeserializeObject(mapData);
                var action = map.command.ToString();

                if (ACAConstant.AGIS_COMMAND_CREATE_CAP.Equals(action, StringComparison.InvariantCultureIgnoreCase) ||
                    ACAConstant.AGIS_COMMAND_SERVICE_REQUEST.Equals(action, StringComparison.InvariantCultureIgnoreCase))
                {
                    mapHelper.SetGisSessionModuleName(callerName);
                }
                else
                {
                    HttpContext.Current.Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] = null;
                }
            }

            string result = string.Empty;
            IList<SubModuleEntity> subList = new List<SubModuleEntity>();
            IList<TabItem> tabsList = GetTabsList();
            try
            {
                TabItem module = tabsList.First(item => item.Module == callerName);
                IList<LinkItem> linkList = module.Children;
                if (linkList != null && linkList.Count > 0)
                {
                    foreach (var item in linkList)
                    {
                        string subModuleName = LabelUtil.GetSuperAgencyTextByKey(item.Label, item.Module);
                        string key = item.Key;

                        if (key.Contains(actionKey))
                        {
                            SubModuleEntity subModule = new SubModuleEntity();
                            subModule.SubModuleName = subModuleName;
                            subModule.Key = key;
                            subModule.Url = item.Url;
                            if (!string.IsNullOrEmpty(item.Url))
                            {  
                               string[] parameters = item.Url.Split('&');

                                foreach (var para in parameters)
                                {
                                   string[] fValue = para.Split('=');

                                    if (fValue.Length == 2)
                                    {
                                        if (fValue[0].Trim() == "FilterName")
                                        {
                                            subModule.FilterName = fValue[1]; 
                                            break;
                                        }
                                    }
                                }
                            }

                            subList.Add(subModule);
                        }
                    }
                }
            }
            catch (Exception) 
            {
            }

            if (subList.Count > 0)
            {
                result = "{\"subModules\":" + Newtonsoft.Json.JsonConvert.SerializeObject(subList) + "}";
            }
            else
            {
                result = "{\"subModules\":\"\"}";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// Get Cap Type Info
        /// </summary>
        /// <param name="callerName">caller Name</param>
        /// <param name="filterValue">filter Value</param>
        /// <param name="url">URL parameter</param>
        /// <returns>Cap Type Info</returns>
        [ActionName("CapTypes")]
        public HttpResponseMessage GetCapTypeInfo(string callerName, string filterValue, string url)
        {
            string result = string.Empty;
            bool isForceLogin = ForceLoginValidation(url, callerName);

            if (isForceLogin)
            {
                result = "{\"isForceLogin\":true}";

                return new HttpResponseMessage
                {
                    Content = new StringContent(result)
                };
            }
           
            IList<CapTypeEntity> capTypes = new List<CapTypeEntity>();

            CapTypeModel[] permitTypelist = null;
            string vchType = ACAConstant.VCH_TYPE_VHAPP;
            string filterName = filterValue ?? string.Empty;
            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            string userID = AppSession.User.PublicUserId;
            CapTypeModel[] capTypeModels = capTypeBll.GetGeneralCapTypeList(callerName, filterName, vchType, userID, false);

            // if cap type anonymousCreateAllowed field is N then remove the cap type from the cap type list
            permitTypelist = AppSession.User.IsAnonymous && capTypeModels != null
                ? capTypeModels.Where(w => !ValidationUtil.IsNo(w.anonymousCreateAllowed)).ToArray()
                : capTypeModels;
            if (permitTypelist != null && permitTypelist.Count() > 0)
            {
                foreach (CapTypeModel typemodel in permitTypelist)
                {
                    string capTypeText = CAPHelper.GetAliasOrCapTypeLabel(typemodel);
                    string capTypeValue = CAPHelper.GetCapTypeValue(typemodel);

                    CapTypeEntity capType = new CapTypeEntity();
                    capType.CapTypeValue = capTypeValue;
                    capType.CapTypeText = capTypeText;
                    capTypes.Add(capType);
                }
            }

            if (capTypes.Count > 0)
            {
                capTypes = capTypes.OrderBy(m => m.CapTypeText).ToList();
                result = "{\"capTypes\":" + Newtonsoft.Json.JsonConvert.SerializeObject(capTypes) + "}";
            }
            else
            {
                result = "{\"capTypes\":\"\"}";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// Get Cap Detail
        /// </summary>
        /// <param name="moduleTitle">module Title</param>
        /// <param name="capTypeValue">cap Type Value</param>
        /// <param name="capTypeText">cap Type Text</param>
        /// <param name="actionKey">action Key</param>
        /// <returns>Cap Detail</returns>
        [ActionName("cap-detail")]
        public HttpResponseMessage GetCapDetail(string moduleTitle, string capTypeValue, string capTypeText, string actionKey)
        {
            AppSession.BreadcrumbParams = null;

            string result = SkipToCapEditPage(capTypeValue, capTypeText, moduleTitle);

            if (actionKey.Contains("ObtainFeeEstimate"))
            {
                if (result.Contains("isFeeEstimator="))
                {
                    result = result.Replace("isFeeEstimator=", "isFeeEstimator=Y");
                }
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"capTypeDetail\":[{\"url\":\"" + result + "\"}]}")
            };
        }

        /// <summary>
        /// Gets the current url from session.
        /// </summary>
        /// <returns>The current url</returns>
        [ActionName("CurrentUrl")]
        public HttpResponseMessage GetCurrentUrl()
        {
            string url = string.Empty;

            if (HttpContext.Current.Session[ACAConstant.CURRENT_URL] != null)
            {
                url = HttpContext.Current.Session[ACAConstant.CURRENT_URL].ToString();

                // Remove the first "/"
                if (url.Length > 1 && url.IndexOf("/", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    url = url.Substring(1);
                }

                HttpContext.Current.Session[ACAConstant.CURRENT_URL] = null;
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(url)
            };
        }

        /// <summary>
        /// Gets the enable registration flag.
        /// </summary>
        /// <returns>enable flag</returns>
        [ActionName("registFlag")]
        public HttpResponseMessage GetEnabledRegister()
        {
            var isEnabledRegister = StandardChoiceUtil.IsRegistrationEnabled();

            return new HttpResponseMessage
            {
                Content = new StringContent(isEnabledRegister.ToString())
            };
        }

        /// <summary>
        /// Gets the enable login flag.
        /// </summary>
        /// <returns>login flag</returns>
        [ActionName("loginFlag")]
        public HttpResponseMessage GetEnabledLogin()
        {
            var isEnabledLogin = StandardChoiceUtil.IsLoginEnabled();

            return new HttpResponseMessage
            {
                Content = new StringContent(isEnabledLogin.ToString())
            };
        }

        /// <summary>
        /// Gets the enable account management.
        /// </summary>
        /// <returns>account management flag</returns>
        [ActionName("accountManagementFlag")]
        public HttpResponseMessage GetEnabledAccountManagement()
        {
            var isEnableAccountAttachment = StandardChoiceUtil.IsAccountManagementEnabled();

            return new HttpResponseMessage
            {
                Content = new StringContent(isEnableAccountAttachment.ToString())
            };
        }

        /// <summary>
        /// Build Tab Item
        /// </summary>
        /// <param name="tab">the instance of TabItem</param>
        /// <param name="setLabel">is set label</param>
        private static void BuildTabItem(TabItem tab, bool setLabel)
        {
            string label = LabelUtil.GetSuperAgencyTextByKey(tab.Label, tab.Module);

            if (label == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
            {
                label = DataUtil.AddBlankToString(tab.Module);
            }

            tab.Title = LabelUtil.RemoveHtmlFormat(label);

            if (setLabel)
            {
                tab.Label = label;
            }
        }

        /// <summary>
        /// Is Action Key Module
        /// </summary>
        /// <param name="moduleName">module Name</param>
        /// <param name="actionKey">action Key</param>
        /// <param name="linkUrl">link URL</param>
        /// <returns>boolean IS Action Key Module</returns>
        [NonAction]
        private bool IsActionKeyModule(string moduleName, string actionKey, out string linkUrl)
        {
            linkUrl = string.Empty;
            bool result = false;

            IList<TabItem> tabsList = GetTabsList();
            TabItem module = tabsList.First(item => item.Title == moduleName);
            IList<LinkItem> linkList = module.Children;
            if (linkList != null && linkList.Count > 0)
            {
                foreach (var item in linkList)
                {
                    string key = item.Key;

                    if (key == null)
                    {
                        continue;
                    }

                    if (key.IndexOf(actionKey, StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        linkUrl = item.Url;
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }
        #endregion

        /// <summary>
        /// whether jump to capdetial page
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="moduleName">module Name</param>
        /// <returns>bool</returns>
        [ActionName("CapDetail")]
        [HttpGet]
        public HttpResponseMessage GoToCapDetail(string url, string moduleName)
        {
            string result = "{\"isForceLogin\":false}";
            bool isForceLogin = ForceLoginValidation(url, moduleName);

            if (isForceLogin)
            {
                result = "{\"isForceLogin\":true}";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        #region LanchPad methods.

        /// <summary>
        /// Gets all of link blocks need to displayed to the current page.
        /// </summary>
        /// <returns>all link blocks.</returns>
        private IList<TabItem> GetTabsList()
        {
            bool isAdminModeRegistered = false;

            // get all defined tabs and links.
            IList<TabItem> tabsList = TabUtil.GetTabList(isAdminModeRegistered);

            IList<TabItem> linkTabs = new List<TabItem>();

            foreach (TabItem tab in tabsList)
            {
                // if the tab needn't to be showed in home page as link block
                if (!tab.BlockVisible || string.IsNullOrEmpty(tab.Label))
                {
                    continue;
                }

                BuildTabItem(tab, false);

                // found the block links in home page.
                if (tab.Children.Count > 0)
                {
                    ICapTypeFilterBll capTypFiltereBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
                    List<LinkItem> listItemList = new List<LinkItem>();

                    foreach (LinkItem subLink in tab.Children)
                    {
                        string filterName = capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, subLink.Module, subLink.Label);

                        // append tab name to url to ensure tab can be selected correctly.
                        subLink.Url = TabUtil.RebuildUrl(subLink.Url, tab.Key, filterName);
                        listItemList.Add(subLink);
                    }

                    tab.Children = listItemList;
                    linkTabs.Add(tab);
                }
            }

            // Report block is configurated in standard choice,which is diffrent with other block.
            TabItem reportBlock = GetReportLink();
            if (reportBlock != null)
            {
                linkTabs.Add(reportBlock);
            }

            return linkTabs;
        }

        /// <summary>
        /// Gets report link block.
        /// </summary>
        /// <returns>TabLinkItem object.</returns>
        private TabItem GetReportLink()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            // get the report name from standard choice.
            string reportName = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_PRINT_REPORT_NAME);

            // if the report name is configruated, add the link.otherwise no report link block.
            if (!string.IsNullOrEmpty(reportName))
            {
                //Home Page Report URL
                string homePageReportUrl = string.Format("/Report/ShowReport.aspx?reportType={0}&reportName={1}&reportID={2}", ACAConstant.PRINT_HOMEPAGE_REPORT, reportName, ACAConstant.NONASSIGN_NUMBER);

                LinkItem linkItem = new LinkItem();
                linkItem.Label = "com_welcome_label_print";
                linkItem.Url = homePageReportUrl;

                TabItem reportTab = new TabItem();
                reportTab.Label = "com_welcome_label_print_title";
                reportTab.Url = homePageReportUrl;
                reportTab.Order = 999; // set enough big order to ensure it is the last tab.
                reportTab.Children.Add(linkItem);

                return reportTab;
            }

            return null;
        }

        /// <summary>
        /// Skip to cap type page when select one cap type or only one cap type.
        /// </summary>
        /// <param name="capTypeValue">cap Type Value</param>
        /// <param name="capTypeText">cap Type Text</param>
        /// <param name="moduleTitle">module Title</param>
        /// <returns>string edit page</returns>
        private string SkipToCapEditPage(string capTypeValue, string capTypeText, string moduleTitle)
        {
            CapTypeModel capType = CapUtil.CreateNewCapType(capTypeValue, capTypeText, moduleTitle);
            PageFlowGroupModel pageflowGroup = CreatePageflowToSession(capType, moduleTitle);

            if (pageflowGroup == null || pageflowGroup.stepList == null || pageflowGroup.stepList.Length < 1)
            {
                // MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applyPermit_error_noRelatedPageflowGroup"));
            }
            else
            {
                return SetupCapType(capTypeValue, capTypeText, moduleTitle);
            }

            return string.Empty;
        }

        /// <summary>
        /// create page flow to session.
        /// </summary>
        /// <param name="capType">CapTypeModel OBJ</param>
        /// <param name="moduleTitle">module Title</param>
        /// <returns>page flow group model</returns>
        private PageFlowGroupModel CreatePageflowToSession(CapTypeModel capType, string moduleTitle)
        {
            string isCloningRecord = string.Empty; //Request.QueryString[ACAConstant.IS_CLONE_RECORD] == null ? string.Empty : Request.QueryString[ACAConstant.IS_CLONE_RECORD].ToString();

            IPageflowBll pageflowBll = (IPageflowBll)ObjectFactory.GetObject(typeof(IPageflowBll));
            CapModel4WS capModel = null; //AppSession.GetCapModelFromSession(moduleTitle);

            PageFlowGroupModel pageflowGroup = pageflowBll.GetPageflowGroupByCapType(capType);

            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(Accela.ACA.BLL.Cap.ICapBll));

            pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel, pageflowGroup);

            AppSession.SetPageflowGroupToSession(pageflowGroup);

            return pageflowGroup;
        }

        /// <summary>
        /// set up cap type
        /// </summary>
        /// <param name="capTypeValue">cap Type Value</param>
        /// <param name="capTypeText">cap Type Text</param>
        /// <param name="moduleTitle">module Title</param>
        /// <returns>cap type</returns>
        private string SetupCapType(string capTypeValue, string capTypeText, string moduleTitle)
        {
            //this step should always be 1.
            int nextStep = 2;

            string isCloningRecord = string.Empty;

            if (!CreateCapModeToSession(capTypeValue, capTypeText, moduleTitle))
            {
                return string.Empty;
            }

            string url = "CapEdit.aspx?Module=" + moduleTitle + "&stepNumber=" + nextStep + "&pageNumber=1&isFeeEstimator=" /*+ Request.QueryString["isFeeEstimator"]*/;

            //Set the parent info to session for the Associated Forms.
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleTitle);
            CapUtil.SetAssoFormParentToAppSession(capModel, capModel.capType, AppSession.GetPageflowGroupFromSession());

            return url;
        }

        /// <summary>
        /// Create cap mode to session.
        /// </summary>
        /// <param name="capTypeValue">cap Type Value</param>
        /// <param name="capTypeText">cap Type Text</param>
        /// <param name="moduleTitle">module Title</param>
        /// <returns>cap mode to session</returns>
        private bool CreateCapModeToSession(string capTypeValue, string capTypeText, string moduleTitle)
        {
            CapTypeModel capType = CapUtil.CreateNewCapType(
                capTypeValue,
                capTypeText,
                moduleTitle);

            LicenseModel4WS selectedLicenseModel = null;

            //if (!LicenseUtil.IsAvailableLicense(SelectedLicenseModel, capType))
            //{
            //    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applypermit_selecttype_error_unavailablelicense"));
            //    return false;
            ////}

            PageFlowGroupModel pageflowGroup = CreatePageflowToSession(capType, moduleTitle);

            //if (pageflowGroup == null || pageflowGroup.stepList == null || pageflowGroup.stepList.Length < 1)
            //{
            //    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applyPermit_error_noRelatedPageflowGroup"));
            //    return false;
            ////}

            //if (SelectedLicenseModel != null)
            //{
            //    bool hasLPComonent = PageFlowUtil.IsComponentExist(GViewConstant.SECTION_LICENSE) || PageFlowUtil.IsComponentExist(GViewConstant.SECTION_MULTIPLE_LICENSES);

            //    // if has no LP component in page flow,clear the selected license.
            //    if (!hasLPComonent)
            //    {
            //        SelectedLicenseModel = null;
            //    }
            ////}

            CapModel4WS capModel4WS = new CapModel4WS();
            capModel4WS.capType = capType;

            //if (Request.QueryString["parentCapModelID"] != null && Request.QueryString["parentCapModelID"].ToString() != String.Empty)
            //{
            //    CapIDModel4WS parentCapIdModel = new CapIDModel4WS();
            //    string customID = Request.QueryString["parentCapModelID"].ToString();
            //    parentCapIdModel.customID = customID;
            //    string[] idParameters = customID.Split('-');
            //    parentCapIdModel.id1 = idParameters[0];
            //    parentCapIdModel.id2 = idParameters[1];
            //    parentCapIdModel.id3 = idParameters[2];
            //    parentCapIdModel.serviceProviderCode = ConfigManager.AgencyCode;

            //    if (Request.QueryString["trackingID"] != null && Request.QueryString["trackingID"].ToString() != String.Empty)
            //    {
            //        parentCapIdModel.trackingID = long.Parse(Request.QueryString["trackingID"]);
            //    }

            //    capModel4WS.parentCapID = parentCapIdModel;
            //    AppSession.SetParentCapIDModelToSession(ACAConstant.CAP_RELATIONSHIP_AMENDMENT, parentCapIdModel);
            ////}

            capModel4WS.licSeqNbr = null;
            capModel4WS.licenseProfessionalModel = TempModelConvert.ConvertToLicenseProfessionalModel4WS(CapUtil.CreateLicenseProfessionalModel(selectedLicenseModel));

            capModel4WS.auditID = AppSession.User.PublicUserId;

            //This partial cap will don't display in ACA, unless public user click save and resume button to save.
            capModel4WS.accessByACA = ACAConstant.COMMON_N;

            //This is temporary cap flag. This cap be saved, but it can't by view in ACA.
            capModel4WS.capClass = ACAConstant.INCOMPLETE_TEMP_CAP; 
            capModel4WS.createdBy = AppSession.User.PublicUserId;

            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            ServiceModel[] services = AppSession.GetSelectedServicesFromSession();

            // if current agency is super agency,create parent cap and children caps.
            if (StandardChoiceUtil.IsSuperAgency())
            {
                //SetAPOFromWorkLocation(capModel4WS);
                //capModel4WS = capBll.CreatePartialCaps(capModel4WS, services, false);
            }
            else
            {
                //Super agency site select one service, which have page flow.
                //if (services != null && services.Length > 0 && ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
                //{
                //    SetAPOFromWorkLocation(capModel4WS);
                //    capModel4WS.service = services[0];

                //    // Because no license select page in super agency environment, so if the page flow contains License section, auto copy associated license to License section.
                //    if (PageFlowUtil.IsComponentExist(GViewConstant.SECTION_LICENSE) || PageFlowUtil.IsComponentExist(GViewConstant.SECTION_MULTIPLE_LICENSES))
                //    {
                //        capModel4WS.licenseProfessionalList = GetUserLicenseList(capModel4WS);
                //    }
                ////}

                capModel4WS = capBll.CreateWrapperForPartialCap(ConfigManager.AgencyCode, capModel4WS, AppSession.User.PublicUserId, string.Empty, false);
            }

            CapWithConditionModel4WS capWithConditionModel4WS = capBll.GetCapViewBySingle(capModel4WS.capID, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, StandardChoiceUtil.IsSuperAgency());
            capModel4WS = capWithConditionModel4WS.capModel;

            if (capModel4WS.licenseProfessionalList != null && capModel4WS.licenseProfessionalList.Length > 0)
            {
                foreach (LicenseProfessionalModel4WS item in capModel4WS.licenseProfessionalList)
                {
                    if (string.IsNullOrEmpty(item.agencyCode))
                    {
                        item.agencyCode = item.capID.serviceProviderCode;
                    }
                }

                capModel4WS.licenseProfessionalModel = capModel4WS.licenseProfessionalList[0];
            }

            capModel4WS.applicantModel = null;

            //if (peopleModel != null)
            //{
            //    peopleModel.contactType = string.Empty;
            //    SetReferenceContactToFirstContactComponent(pageflowGroup, peopleModel, capModel4WS);
            ////}

            if (capModel4WS.licenseProfessionalList != null && capModel4WS.licenseProfessionalList.Length != 0)
            {
                foreach (var licenseProfessionalModel in capModel4WS.licenseProfessionalList)
                {
                    licenseProfessionalModel.TemporaryID = CommonUtil.GetRandomUniqueID();
                }
            }

            AppSession.SetCapModelToSession(moduleTitle, capModel4WS);

            pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel4WS, pageflowGroup);
            AppSession.SetPageflowGroupToSession(pageflowGroup);

            return true;
        }

        /// <summary>
        /// Judge Access right and login statues of the feature.
        /// </summary>
        /// <param name="url">the url from request or special one.</param>
        /// <param name="moduleName">module Name</param>
        /// <returns>is force login or not</returns>
        private bool ForceLoginValidation(string url, string moduleName)
        {
            // if feature need to force login, and user have not login, redirect to login page else do nothing.
            bool isForceLogin = !AuthenticationUtil.IsAuthenticated && IsFeatureForceLogin(url, moduleName);

            return isForceLogin;
        }

        /// <summary>
        /// check if force login
        /// </summary>
        /// <param name="url">the URL from request or special one.</param>
        /// <param name="moduleName">module Name</param>
        /// <returns>true if force login</returns>
        private bool IsFeatureForceLogin(string url, string moduleName)
        {
            IBizDomainBll bizDomain = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            return bizDomain.IsForceLogin(moduleName, url, null);
        }
        #endregion
    }
}