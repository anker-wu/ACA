#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AppSession.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: AppSession.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;
using System.Web;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.ACA.Web.Examination;
using Accela.ACA.Web.Inspection;
using Accela.ACA.Web.People;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.UI;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provide a class to manage global session objects.
    /// All of global session objects should be defined in this class.
    /// </summary>
    public static class AppSession
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Breadcrumb information, which is used on apply process.
        /// </summary>
        public static BreadCrumbParmsInfo BreadcrumbParams
        {
            get
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                return session.BreadCrumbParmsInfo;
            }

            set
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                session.BreadCrumbParmsInfo = value;
            }
        }

        /// <summary>
        /// Gets or sets examination information
        /// </summary>
        public static List<ExaminationListItemViewModel> ExaminationData
        {
            get
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                return session.ExaminationData;
            }

            set
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                session.ExaminationData = value;
            }
        }

        /// <summary>
        /// Gets or sets inspection information
        /// </summary>
        public static List<InspectionListItemViewModel> InspectionData
        {
            get
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                return session.InspectionData;
            }

            set
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                session.InspectionData = value;
            }
        }

        /// <summary>
        /// Gets or sets the inspection parameters.
        /// </summary>
        /// <value>
        /// The inspection parameters.
        /// </value>
        public static Hashtable InspectionParameters
        {
            get
            {
                var iACASession = ObjectFactory.GetObject<IACASession>();
                return iACASession.InspectionParameters;
            }

            set
            {
                var iACASession = ObjectFactory.GetObject<IACASession>();
                iACASession.InspectionParameters = value;
            }
        }

        /// <summary>
        /// Gets or sets the examination parameters.
        /// </summary>
        /// <value>The examination parameters.</value>
        public static Hashtable ExaminationParameters
        {
            get
            {
                var iACASession = ObjectFactory.GetObject<IACASession>();
                return iACASession.ExaminationParameters;
            }

            set
            {
                var iACASession = ObjectFactory.GetObject<IACASession>();
                iACASession.ExaminationParameters = value;
            }
        }

        /// <summary>
        /// Gets the Current URL
        /// </summary>
        public static string CurrentURL
        {
            get
            {
                /*
                 * For multiple records creation by deep link, the first Http request(POST request) and second Http request(GET response URL) may be in different session.
                 * It's caused that we canont got the correct session item CURRENT_URL after the secord Http request back to ACA.
                 * So store the response URL to Application state, and then the second request will get the correct URL by the Key.
                 */
                string deeplinkUrlKey = HttpContext.Current.Request[UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY];

                if (!string.IsNullOrEmpty(deeplinkUrlKey))
                {
                    IDeepLinkBLL deepLinkBLL = ObjectFactory.GetObject<IDeepLinkBLL>();
                    DeepLinkAuditTrailModel deepLinkAuditTrail = deepLinkBLL.GetDeepLinkAuditTrail(deeplinkUrlKey, User == null ? "PUBLICUSER0" : User.PublicUserId);

                    string currentUrl = string.Empty;

                    // The valid record indicates that the service has not been used to create an application
                    if (deepLinkAuditTrail != null && !string.IsNullOrEmpty(deepLinkAuditTrail.URL))
                    {
                        currentUrl = string.Format(
                            "{0}://{1}{2}",
                            ConfigManager.Protocol,
                            HttpContext.Current.Request.Url.Authority,
                            FileUtil.CombineWebPath(HttpContext.Current.Request.ApplicationPath, deepLinkAuditTrail.URL));
                    }

                    if (!string.IsNullOrEmpty(currentUrl))
                    {
                        HttpContext.Current.Session[ACAConstant.CURRENT_URL] = currentUrl;

                        return currentUrl;
                    }
                }

                if (HttpContext.Current.Session[ACAConstant.CURRENT_URL] != null)
                {
                    return HttpContext.Current.Session[ACAConstant.CURRENT_URL].ToString();
                }
                else
                {
                    string url = ConfigManager.HomePage;

                    if (string.IsNullOrEmpty(url))
                    {
                        // if the default home page doesn't be defined in web.config, get from constant.
                        url = ACAConstant.URL_WELCOME_PAGE;
                    }

                    return FileUtil.AppendApplicationRoot(url);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current runtime mode is working admin mode or not.
        /// </summary>
        public static bool IsAdmin
        {
            get
            {
                object isAdmin = HttpContext.Current.Session[SessionConstant.SESSION_ADMIN_MODE];

                if (isAdmin != null &&
                    isAdmin.ToString() == ACAConstant.COMMON_Y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                if (value)
                {
                    HttpContext.Current.Session[SessionConstant.SESSION_ADMIN_MODE] = ACAConstant.COMMON_Y;
                }
                else
                {
                    HttpContext.Current.Session[SessionConstant.SESSION_ADMIN_MODE] = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected address, which is used on apply process to fill the address section.
        /// </summary>
        public static ParcelInfoModel SelectedParcelInfo
        {
            get
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                return session.SelectedParcelInfo;
            }

            set
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                session.SelectedParcelInfo = value;
            }
        }

        /// <summary>
        /// Gets or sets the Breadcrumb information, which is used on shopping cart.
        /// </summary>
        public static BreadCrumbParmsInfo ShoppingCartBreadcrumbParams
        {
            get
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                return session.ShoppingCartBreadCrumbParmsInfo;
            }

            set
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                session.ShoppingCartBreadCrumbParmsInfo = value;
            }
        }

        /// <summary>
        /// Gets or sets the current user session.
        /// </summary>
        public static User User
        {
            get
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                return session.User;
            }

            set
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                session.User = value;
            }
        }

        /// <summary>
        /// Gets or sets the HistoryAction information.
        /// </summary>
        public static HistoryAction GlobalSearchHistoryAction
        {
            get
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                return session.HistoryAction;
            }

            set
            {
                IACASession session = ObjectFactory.GetObject<IACASession>();
                session.HistoryAction = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the record's flag is edited from Confirm Page.
        /// </summary>
        public static bool IsEditFromConfirmFlag 
        {
            get 
            {
                object obj = HttpContext.Current.Session[SessionConstant.SESSION_IS_EDIT_FROM_CONFIRM];
                
                if (obj != null)
                {
                    return (bool)obj;
                }

                return false;
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_IS_EDIT_FROM_CONFIRM] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enter the register page from clerk list.
        /// </summary>
        public static bool IsEditFromClerkFlag
        {
            get
            {
                object obj = HttpContext.Current.Session["IsEditFromClerkFlag"];

                if (obj != null)
                {
                    return (bool)obj;
                }

                return false;
            }

            set
            {
                HttpContext.Current.Session["IsEditFromClerkFlag"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enter the register page from login.
        /// </summary>
        public static bool IsEditFromLoginFlag
        {
            get
            {
                object obj = HttpContext.Current.Session["IsEditFromLoginFlag"];

                if (obj != null)
                {
                    return (bool)obj;
                }

                return false;
            }

            set
            {
                HttpContext.Current.Session["IsEditFromLoginFlag"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the Authorized Agent Reprint Limit
        /// </summary>
        public static object AuthorizedAgentReprintLimit
        {
            get
            {
                return HttpContext.Current.Session[ACAConstant.SESSION_AUTHORIZED_AGENT_REPRINT_LIMIT];
            }

            set
            {
                HttpContext.Current.Session[ACAConstant.SESSION_AUTHORIZED_AGENT_REPRINT_LIMIT] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the announcements model from session.
        /// </summary>
        /// <returns>announcement list</returns>
        public static List<AnnouncementModel> GetAnnouncementsFromSession()
        {
            IACASession session = (IACASession)ObjectFactory.GetObject(typeof(IACASession));
            return session.GetAnnouncementsFromSession();
        }

        /// <summary>
        /// Gets the announcement flag from session.
        /// </summary>
        /// <returns>The announcement flag.</returns>
        public static string GetAnnouncementFlagFromSession()
        {
            IACASession session = (IACASession)ObjectFactory.GetObject(typeof(IACASession));
            return session.GetAnnouncementFlagFromSession();
        }

        /// <summary>
        /// Set announcements to session
        /// </summary>
        /// <param name="announcements">The announcement list.</param>
        public static void SetAnnouncementsToSession(List<AnnouncementModel> announcements)
        {
            IACASession session = (IACASession)ObjectFactory.GetObject(typeof(IACASession));
            session.SetAnnouncementsToSession(announcements);
        }

        /// <summary>
        /// Set announcement flag to session
        /// </summary>
        /// <param name="flag">The announcement flag.</param>
        public static void SetAnnouncementFlagToSession(string flag)
        {
            IACASession session = (IACASession)ObjectFactory.GetObject(typeof(IACASession));
            session.SetAnnouncementFlagToSession(flag);
        }

        /// <summary>
        /// Gets the caps id model from session.
        /// </summary>
        /// <returns>CapIDModels object.</returns>
        public static CapIDModel4WS[] GetCapIDModelsFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetCapIDModelsFromSession();
        }

        /// <summary>
        /// Get the parent Cap ID model from Session.
        /// </summary>
        /// <param name="relationship">It may be <c>"R", "EST", "Renewal", "Amendment" or "AssoForm"</c>.</param>
        /// <returns>parent Cap ID model</returns>
        public static CapIDModel4WS GetParentCapIDModelFromSession(string relationship)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetParentCapIDModelFromSession(relationship);
        }

        /// <summary>
        /// Gets the cap model from session.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>the capModel instance for web service</returns>
        public static CapModel4WS GetCapModelFromSession(string moduleName)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetCapModelFromSession(moduleName);
        }

        /// <summary>
        /// Get the Shopping Cart Item Number from Session
        /// </summary>
        /// <returns>ServiceManagementModel4WS list.</returns>
        public static string GetCartItemNumberFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetCartItemNumberFromSession();
        }

        /// <summary>
        /// Get my collection models from session.
        /// </summary>
        /// <returns>the instance of MyCollectionModel4WS</returns>
        public static MyCollectionModel[] GetMyCollectionsFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetMyCollectionsFromSession();
        }

        /// <summary>
        /// gets PaymentResultModel4WSs from session. Them were returned from creating real caps .
        /// </summary> 
        /// <returns>the instance of OnlinePaymentResultModel</returns>
        public static OnlinePaymentResultModel GetOnlinePaymentResultModelFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetOnlinePaymentResultModelFromSession();
        }

        /// <summary>
        /// Gets the page flow group from session.
        /// </summary>
        /// <returns>PageFlowGroupModel object.</returns>
        public static PageFlowGroupModel GetPageflowGroupFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetPageflowGroupFromSession();
        }

        /// <summary>
        /// Get the parent cap type from session.
        /// </summary>
        /// <returns>CapTypeModel object.</returns>
        public static CapTypeModel GetParentCapTypeFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetParentCapTypeFromSession();
        }

        /// <summary>
        /// Get the parent page flow group from session.
        /// </summary>
        /// <returns>PageFlowGroupModel object.</returns>
        public static PageFlowGroupModel GetParentPageflowGroupFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetParentPageflowGroupFromSession();
        }

        /// <summary>
        /// gets PaymentResultModel4WSs from session. Them were returned from creating real caps .
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>the instance of PaymentResultModel4WS list</returns>
        public static PaymentResultModel4WS[] GetPaymentResultModelsFromSession(string moduleName)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetPaymentResultModelsFromSession(moduleName);
        }

        /// <summary>
        /// Get report parameters from session.
        /// </summary>
        /// <returns>the instance of ParameterModel4WS array</returns>
        public static ParameterModel4WS[] GetReportParameterFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetReportParameterFromSession();
        }

        /// <summary>
        /// Gets the selected services from session.
        /// </summary>
        /// <returns>ServiceManagementModel4WS list.</returns>
        public static ServiceModel[] GetSelectedServicesFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetSelectedServicesFromSession();
        }

        /// <summary>
        /// Gets the upload file info from session.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>Upload file information.</returns>
        public static Dictionary<string, List<FileUploadInfo>> GetUploadFileInfoFromSession(string moduleName)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetUploadFileInfoFromSession(moduleName);
        }

        /// <summary>
        /// Get the flag which current user whether has Provider License from session.
        /// </summary>
        /// <returns>Indicate the current user whether has Provider License.</returns>
        public static int GetHasProviderLicenseFlagFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetHasProviderLicenseFlagFromSession();
        }

        /// <summary>
        /// Get the UI data from session.
        /// </summary>
        /// <returns>A hashtable contains the all UI data.</returns>
        public static Hashtable GetUIDataFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetUIDataFromSession();
        }

        /// <summary>
        /// Gets the asset search model from session.
        /// </summary>
        /// <returns>Asset search model</returns>
        public static AssetMasterModel GetAssetSearchModelFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetAssetSearchModelFromSession();
        }

        /// <summary>
        /// Sets the cap id models to session. 
        /// </summary>
        /// <param name="capIDs">the CapIDModel to be set to session.</param>
        public static void SetCapIDModelsToSession(CapIDModel4WS[] capIDs)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetCapIDModelsToSession(capIDs);
        }

        /// <summary>
        /// Set the parent Cap ID model to Session.
        /// </summary>
        /// <param name="relationship">It may be <c>"R", "EST", "Renewal", "Amendment" or "AssoForm"</c>.</param>
        /// <param name="parentCapID">parent Cap ID model</param>
        public static void SetParentCapIDModelToSession(string relationship, CapIDModel4WS parentCapID)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetParentCapIDModelToSession(relationship, parentCapID);
        }

        /// <summary>
        /// Sets the cap model to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capModel">the CapModel to be set to session.</param>
        public static void SetCapModelToSession(string moduleName, CapModel4WS capModel)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetCapModelToSession(moduleName, capModel);
        }

        /// <summary>
        /// Sets the upload file info to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="htFileUploadInfos">the upload file info to be set to session.</param>
        public static void SetUploadFileInfoToSession(string moduleName, Dictionary<string, List<FileUploadInfo>> htFileUploadInfos)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetUploadFileInfoToSession(moduleName, htFileUploadInfos);
        }

        /// <summary>
        /// Sets the Shopping Cart Item Number To Session
        /// </summary>
        /// <param name="cartItemsNumber">cart Items Number</param>
        public static void SetCartItemNumberToSession(string cartItemsNumber)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetCartItemNumberToSession(cartItemsNumber);
        }

        /// <summary>
        /// Set my collection models to session.
        /// </summary>
        /// <param name="myCollectionModelList">my collection model list</param>
        public static void SetMyCollectionsToSession(MyCollectionModel[] myCollectionModelList)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetMyCollectionsToSession(myCollectionModelList);
        }

        /// <summary>
        /// sets OnlinePaymentResultModel to session,them were returned from creating real caps.
        /// </summary>
        /// <param name="onlinePaymentResultModel">the onlinePaymentResultModel</param>
        public static void SetOnlinePaymentResultModelToSession(OnlinePaymentResultModel onlinePaymentResultModel)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetOnlinePaymentResultModelToSession(onlinePaymentResultModel);
        }

        /// <summary>
        /// Sets the cap model to session. 
        /// </summary>
        /// <param name="pageflow">the PageFlowGroupModel to be set to session.</param>
        public static void SetPageflowGroupToSession(PageFlowGroupModel pageflow)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetPageflowGroupToSession(pageflow);
        }

        /// <summary>
        /// Set the parent cap type model to session. 
        /// </summary>
        /// <param name="capType">the CapTypeModel to be set to session.</param>
        public static void SetParentCapTypeToSession(CapTypeModel capType)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetParentCapTypeToSession(capType);
        }

        /// <summary>
        /// Set the parent page flow group model to session. 
        /// </summary>
        /// <param name="parentPageflow">the PageFlowGroupModel to be set to session.</param>
        public static void SetParentPageflowGroupToSession(PageFlowGroupModel parentPageflow)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetParentPageflowGroupToSession(parentPageflow);
        }

        /// <summary>
        /// sets PaymentResultModel4WSs to session,them were returned from creating real caps.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="paymentResultModels">the PaymentResultModel4WS list</param>
        public static void SetPaymentResultModelsToSession(string moduleName, PaymentResultModel4WS[] paymentResultModels)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetPaymentResultModelsToSession(moduleName, paymentResultModels);
        }

        /// <summary>
        /// Set report parameters to session.
        /// </summary>
        /// <param name="parameters">the ParameterModel4WS list</param>
        public static void SetReportParameterToSession(ParameterModel4WS[] parameters)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetReportParameterToSession(parameters);
        }

        /// <summary>
        /// Sets the selected services to session
        /// </summary>
        /// <param name="services">the ServiceModel list</param>
        public static void SetSelectedServicesToSession(ServiceModel[] services)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetSelectedServicesToSession(services);
        }

        /// <summary>
        /// Sets the flag which current user whether has Provider License to session.
        /// </summary>
        /// <param name="hasProviderLicenseFlag">indicate current user whether has Provider License.</param>
        public static void SetHasProviderLicenseFlagToSession(int hasProviderLicenseFlag)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetHasProviderLicenseFlagToSession(hasProviderLicenseFlag);
        }

        /// <summary>
        /// Set UI data to session.
        /// </summary>
        /// <param name="uiData">A hashtable contains the all UI data.</param>
        public static void SetUIDataToSession(Hashtable uiData)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetUIDataToSession(uiData);
        }

        /// <summary>
        /// Update session.
        /// </summary>
        public static void ReloadPublicUserSession()
        {
            IAccountBll accountBll = ObjectFactory.GetObject(typeof(IAccountBll)) as IAccountBll;
            AppSession.User.UserModel4WS = accountBll.GetPublicUser(User.UserSeqNum);
        }

        /// <summary>
        /// Gets the people model from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the people object.</returns>
        public static PeopleModel GetPeopleModelFromSession(string contactSeqNbr)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetPeopleModelFromSession(contactSeqNbr);
        }

        /// <summary>
        /// Sets the people model to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="peopleModel">The people model.</param>
        public static void SetPeopleModelToSession(string contactSeqNbr, PeopleModel peopleModel)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetPeopleModelToSession(contactSeqNbr, peopleModel);
        }

        /// <summary>
        /// Sets the asset search model to session.
        /// </summary>
        /// <param name="assetMasterModel">The asset master model.</param>
        public static void SetAssetSearchModelToSession(AssetMasterModel assetMasterModel)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetAssetSearchModelToSession(assetMasterModel);
        }

        /// <summary>
        /// Get the expression info from session.
        /// </summary>
        /// <returns>A hashtable contains the all UI data.</returns>
        public static Hashtable GetExpressionDataFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetExpressionDataFromSession();
        }

        /// <summary>
        /// Set the expression info to session.
        /// </summary>
        /// <param name="uiData">A hashtable contains the all UI data.</param>
        public static void SetExpressionDataToSession(Hashtable uiData)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetExpressionDataToSession(uiData);
        }

        /// <summary>
        /// Gets the contact education list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference education list object.</returns>
        public static EducationModel4WS[] GetContactEducationListFromSession(string contactSeqNbr)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetContactEducationListFromSession(contactSeqNbr);
        }

        /// <summary>
        /// Gets the contact continuing education list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference continuing education list object.</returns>
        public static ContinuingEducationModel4WS[] GetContactContEducationListFromSession(string contactSeqNbr)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetContactContEducationListFromSession(contactSeqNbr);
        }

        /// <summary>
        /// Sets the contact education list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="educationList">The education list.</param>
        public static void SetContactEducationListToSession(string contactSeqNbr, EducationModel4WS[] educationList)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetContactEducationListToSession(contactSeqNbr, educationList);
        }

        /// <summary>
        /// Gets the contact examination list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference examination list object.</returns>
        public static ExaminationModel[] GetContactExaminationListFromSession(string contactSeqNbr)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetContactExaminationListFromSession(contactSeqNbr);
        }

        /// <summary>
        /// Sets the contact examination list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="examinationList">The examination list.</param>
        public static void SetContactExaminationListToSession(string contactSeqNbr, ExaminationModel[] examinationList)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetContactExaminationListToSession(contactSeqNbr, examinationList);
        }

        /// <summary>
        /// Sets the contact education list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="contEducationList">The continuing education list.</param>
        public static void SetContactContEducationListToSession(string contactSeqNbr, ContinuingEducationModel4WS[] contEducationList)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetContactContEducationListToSession(contactSeqNbr, contEducationList);
        }

        /// <summary>
        /// Sets the contact UI process session.
        /// </summary>
        /// <param name="contactParametersModel">The contact parameters model.</param>
        public static void SetContactSessionParameter(ContactSessionParameter contactParametersModel)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetContactSessionParameter(contactParametersModel);
        }

        /// <summary>
        /// Gets the contact UI process session.
        /// </summary>
        /// <returns>contact parameter model</returns>
        public static ContactSessionParameter GetContactSessionParameter()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetContactSessionParameter();
        }

        /// <summary>
        /// Sets the register contact UI process session.
        /// </summary>
        /// <param name="contactParametersModel">The contact parameters model.</param>
        public static void SetRegisterContactSessionParameter(ContactSessionParameter contactParametersModel)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetRegisterContactSessionParameter(contactParametersModel);
        }

        /// <summary>
        /// Gets the register contact UI process session.
        /// </summary>
        /// <returns>contact parameter model</returns>
        public static ContactSessionParameter GetRegisterContactSessionParameter()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetRegisterContactSessionParameter();
        }

        /// <summary>
        /// Sets the cap type associate license certification.
        /// </summary>
        /// <param name="capAssociateLicenseCertification">The cap associate license certification.</param>
        public static void SetCapTypeAssociateLicenseCertification(CapAssociateLicenseCertification4WS capAssociateLicenseCertification)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetCapTypeAssociateLicenseCertification(capAssociateLicenseCertification);
        }

        /// <summary>
        /// Gets the cap type associate license certification.
        /// </summary>
        /// <returns>The cap type associate license certification.</returns>
        public static CapAssociateLicenseCertification4WS GetCapTypeAssociateLicenseCertification()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetCapTypeAssociateLicenseCertification();
        }

        /// <summary>
        /// Sets selected contact associated examinations/educations/continuing educations to session.
        /// </summary>
        /// <param name="selectedLicenseCertification">The selected license certification.</param>
        public static void SetSelectedContactLicenseCertification(SelectedContactLicenseCertificationModel selectedLicenseCertification)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetSelectedContactLicenseCertification(selectedLicenseCertification);
        }

        /// <summary>
        /// Gets selected contact associated examinations/educations/continuing educations from session.
        /// </summary>
        /// <returns>The selected contact license certification.</returns>
        public static SelectedContactLicenseCertificationModel GetSelectedContactLicenseCertification()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetSelectedContactLicenseCertification();
        }
        
        /// <summary>
        /// Get the Associated form parent CapModel from Session
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>The associated parent cap.</returns>
        public static CapModel4WS GetAssociatedParentCapFromSession(string moduleName)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetAssociatedParentCapFromSession(moduleName);
        }

        /// <summary>
        /// Sets the Associated form parent CapModel to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capModel">the Associated form parent CapModel to be set to session.</param>
        public static void SetAssociatedParentCapToSession(string moduleName, CapModel4WS capModel)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetAssociatedParentCapToSession(moduleName, capModel);
        }

        /// <summary>
        /// Sets the Condition Additional Information to Session.
        /// </summary>
        /// <param name="additionalInfo">The Condition Additional Info List</param>
        public static void SetConditionAdditionalInfoToSession(Dictionary<string, string> additionalInfo)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetConditionAdditionalInfoToSession(additionalInfo);
        }

        /// <summary>
        /// Gets the Condition Additional Information from Session.
        /// </summary>
        /// <returns>The condition additional information.</returns>
        public static Dictionary<string, string> GetConditionAdditionalInfoFromSession()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetConditionAdditionalInfoFromSession();
        }

        /// <summary>
        /// Sets the APO session.
        /// </summary>
        /// <param name="apoSessionParameter">The APO session parameters model.</param>
        public static void SetAPOSessionParameter(APOSessionParameterModel apoSessionParameter)
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            session.SetAPOSessionParameter(apoSessionParameter);
        }

        /// <summary>
        /// Gets the APO session.
        /// </summary>
        /// <returns>APO session parameters model</returns>
        public static APOSessionParameterModel GetAPOSessionParameter()
        {
            IACASession session = ObjectFactory.GetObject<IACASession>();
            return session.GetAPOSessionParameter();
        }

        #endregion Methods
    }
}