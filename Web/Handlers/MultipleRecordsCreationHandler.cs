#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: MultipleRecordsCreationHandler.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: A http request handler to handle multiple records creation from deep link.
*  
* </pre>
*/

#endregion

using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;
using Accela.ACA.WSProxy;
using log4net;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// A http request handler to handle multiple records creation from deep link.
    /// </summary>
    [SuppressCsrfCheck]
    public class MultipleRecordsCreationHandler : IHttpHandler, IHttpHandlerCommon, IRequiresSessionState
    {
        #region Fields

        /// <summary>
        /// Log object.
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(MultipleRecordsCreationHandler));

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the handler can be reused.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the config section.
        /// </summary>
        /// <value>The config section.</value>
        public HttpHandlerConfigObject ConfigObject { get; set; }  
        
        #endregion

        #region Methodes

        /// <summary>
        /// Validate the http request parameters.
        /// It's useless for this handler, always return true.
        /// </summary>
        /// <param name="type">The handler type.</param>
        /// <returns>true means validation passed.</returns>
        public bool ValidateParams(ServiceType type)
        {
            return true;
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the System.Web.IHttpHandler interface.
        /// </summary>
        /// <param name="context">An System.Web.HttpContext object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;

            try
            {
                //Check service data list.
                ServiceDataFromDeepLink serviceData = CheckServiceList(request);
                                
                //Generate the data key as the identifier of deep link transaction data.
                string dataKey = CommonUtil.GetRandomUniqueID("N");

                //Build url and set CURRENT_URL session for CapApplyDisclaimer page.                
                string urlCapDisclaimer = "/cap/CapApplyDisclaimer.aspx";
                urlCapDisclaimer += "?" + ACAConstant.MODULE_NAME + "=" + serviceData.Module;
                urlCapDisclaimer += "&" + UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY + "=" + dataKey;
                urlCapDisclaimer += "&createRecordByService=" + ACAConstant.COMMON_YES;

                if (ValidationUtil.IsYes(context.Request.QueryString[UrlConstant.IS_SKIP_DISCLAIMER]))
                {
                    urlCapDisclaimer += "&" + UrlConstant.IS_SKIP_DISCLAIMER + "=" + ACAConstant.COMMON_Y;
                }
                
                context.Session[ACAConstant.CURRENT_URL] = urlCapDisclaimer;
                
                DeepLinkAuditTrailModel deepLinkAuditTrail = new DeepLinkAuditTrailModel();
                deepLinkAuditTrail.GUID = dataKey;

                // hard code 1003 for deep link code
                deepLinkAuditTrail.deepLinkID = "1003";
                deepLinkAuditTrail.URL = urlCapDisclaimer;
                deepLinkAuditTrail.serviceData = JsonConvert.SerializeObject(serviceData);

                IDeepLinkBLL deepLinkBll = ObjectFactory.GetObject<IDeepLinkBLL>();
                deepLinkBll.CreateDeepLinkAuditTrail(deepLinkAuditTrail, AppSession.User == null ? "PUBLICUSER0" : AppSession.User.PublicUserId);

                //Build and return url for Wrapper page.
                string urlWrapperPage = string.Format(
                    "{0}://{1}{2}",
                    request.Url.Scheme,
                    request.Url.Authority,
                    FileUtil.CombineWebPath(request.ApplicationPath, ConfigurationManager.AppSettings["DefaultPageFile"]));

                context.Response.ContentType = "text/plain";
                context.Response.Clear();
                context.Response.Write(urlWrapperPage + "?" + UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY + "=" + dataKey);
                context.Response.Flush();
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);

                //Throw the standard http exception "400 Bad Request" if there have any exceptions thrown.
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
            }

            context.Response.End();
        }

        /// <summary>
        /// Check service data list, throw the <see cref="InvalidDataException"/> if there any invalid data.
        /// </summary>
        /// <param name="request">Current http request.</param>
        /// <returns>Return a Service data model if validation passed</returns>
        private ServiceDataFromDeepLink CheckServiceList(HttpRequest request)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            string serviceJsonData = request.Form[UrlConstant.SELECTED_SERVICE_LIST];

            if (!string.IsNullOrWhiteSpace(serviceJsonData))
            {
                var serviceData = javaScriptSerializer.Deserialize<ServiceDataFromDeepLink>(serviceJsonData);

                //The service data must have one Service at least with the valid Service Name and Agency Code.
                if (serviceData != null 
                    && serviceData.ServiceList != null 
                    && serviceData.ServiceList.Count > 0
                    && serviceData.ServiceList.Count(p => string.IsNullOrWhiteSpace(p.Name) || string.IsNullOrWhiteSpace(p.Agency)) == 0)
                {
                    /*
                     * If third party only provides one service, the master record type is optional,
                     *  will get module from sub-agency record type, the assumption is the module must exists in super agency.
                     */
                    if (serviceData.ServiceList.Count == 1)
                    {
                        serviceData.Module = GetModuleByService(serviceData.ServiceList[0]);
                    }
                    else
                    {
                        //if the services more than one, the master record type is required, the module will get from master record type.
                        if (string.IsNullOrEmpty(serviceData.MasterRecordType))
                        {
                            throw new ACAException("Master record type is required!");
                        }

                        serviceData.Module = GetModuleByRecordType(serviceData.MasterRecordType);
                    }

                    if (string.IsNullOrEmpty(serviceData.Module))
                    {
                        throw new InvalidDataException("Cannot get the correct module, maybe the associated record type is disabled.");
                    }

                    return serviceData;
                }
            }

            throw new InvalidDataException();
        }

        /// <summary>
        /// Gets module by record type.
        /// </summary>
        /// <param name="recordType">Record type string.</param>
        /// <returns>return the module name</returns>
        private string GetModuleByRecordType(string recordType)
        {
            string module = string.Empty;
            string[] capTypeLevels = recordType.Split('/');

            if (capTypeLevels != null && capTypeLevels.Length == 4)
            {
                module = capTypeLevels[0];
            }
            else
            {
                //Third party passed Cap type info may be a cap type alias.
                ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                CapTypeModel capType = new CapTypeModel();
                capType.alias = recordType;
                CapTypeDetailModel capTypeDetail = capTypeBll.GetCapTypeByPK(capType);

                if (capTypeDetail != null)
                {
                    module = capTypeDetail.moduleName;
                }
            }

            return module;
        }

        /// <summary>
        /// Gets module by service
        /// </summary>
        /// <param name="serviceItem">service data</param>
        /// <returns>return the module name</returns>
        private string GetModuleByService(ServiceItemFromDeepLink serviceItem)
        {
            string module = string.Empty;
            ServiceModel service = null;

            IServiceManagementBll serviceManagementBll = ObjectFactory.GetObject<IServiceManagementBll>();

            // Create a anonymous user context if current user context is null.
            if (AppSession.User == null)
            {
                AccountUtil.CreateUserContext(AccountUtil.MakeAnonymousUser());
            }

            ServiceModel searchModel = new ServiceModel();
            searchModel.servPorvCode = serviceItem.Agency;
            searchModel.serviceName = serviceItem.Name;

            ServiceModel[] availableServices = serviceManagementBll.GetServiceByServiceName(searchModel);

            if (availableServices != null && availableServices.Length > 0)
            {
                service = availableServices.FirstOrDefault(s =>
                                                s.servPorvCode.Equals(serviceItem.Agency, StringComparison.OrdinalIgnoreCase)
                                                && s.serviceName.Equals(serviceItem.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (service != null && service.capType != null)
            {
                module = service.capType.moduleName;

                // if cannot get module, use the record type's group in "group/type/sub-type/category"
                if (string.IsNullOrEmpty(module))
                {
                    module = service.capType.group;
                }
            }

            return module;
        }

        #endregion
    }
}