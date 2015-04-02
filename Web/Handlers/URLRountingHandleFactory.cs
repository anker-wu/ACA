#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UrlRountingHandleFactory.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: Http handler factory.
*
*  Notes:
* $Id: FileUploadHandler.ashx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Accela.ACA.Common;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// the http handler factory.
    /// </summary>
    public class UrlRountingHandleFactory : IHttpHandlerFactory
    {
        #region Fields

        /// <summary>
        /// The error MSG not null
        /// </summary>
        private const string ErrorMsgNotNull = "Field id and type can not be empty in the configuration file.";

        /// <summary>
        /// error message for argument error.
        /// </summary>
        private const string ErrorMsgArgument = "Invaild Argument.";

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
        /// Gets the request.
        /// </summary>
        /// <value>The http request.</value>
        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        /// <summary>
        /// Gets the Handler Type.
        /// </summary>
        /// <value>The request string type.</value>
        private ServiceType HandlerType
        {
            get
            {
                string handlertype = Request.QueryString["Type"];
                ServiceType serviceType;
                Enum.TryParse(handlertype, out serviceType);
                return serviceType;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enables a factory to reuse an existing handler instance.
        /// </summary>
        /// <param name="handler">The System.Web.IHttpHandler object to reuse.</param>
        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        /// <summary>
        /// Returns an instance of a class that implements the System.Web.IHttpHandler interface.
        /// </summary>
        /// <param name="context">An instance of the System.Web.HttpContext class that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <param name="requestType">The HTTP data transfer method (GET or POST) that the client uses.</param>
        /// <param name="url">The System.Web.HttpRequest.RawUrl of the requested resource.</param>
        /// <param name="pathTranslated">The System.Web.HttpRequest.PhysicalApplicationPath to the requested resource.</param>
        /// <returns>A new System.Web.IHttpHandler object that processes the request.</returns>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            IHttpHandler handlerToReturn = null;
            var configList = HttpContext.Current.Application[ApplicationConstant.URLROUTING_HANDLER_CONFIG] as List<HttpHandlerConfigObject>;

            if (configList == null)
            {
                configList = CacheAndGetConfigList();
            }

            var section = GetTrueUrlRoutingConfig(configList);
            Type type = Type.GetType(section.ReflectionType);
            handlerToReturn = GetHandler(section, type);

            return handlerToReturn;
        }

        /// <summary>
        /// Gets the handler to return.
        /// </summary>
        /// <param name="section">The config section.</param>
        /// <param name="type">The Reflection type.</param>
        /// <returns>return IHttpHandler object</returns>
        private IHttpHandler GetHandler(HttpHandlerConfigObject section, Type type)
        {
            IHttpHandler handlerToReturn = new DefaultHttpHandler();

            if (type != null)
            {
                object objHttpHandler = Activator.CreateInstance(type);
                var validation = objHttpHandler as IHttpHandlerCommon;

                if (validation != null)
                {
                    validation.ConfigObject = section;

                    if (validation.ValidateParams(section.Id))
                    {
                        handlerToReturn = objHttpHandler as IHttpHandler;
                    }
                    else
                    {
                        throw new ArgumentException(ErrorMsgArgument);
                    }
                }
            }

            return handlerToReturn;
        }

        /// <summary>
        /// Caches and Get the config list.
        /// </summary>
        /// <returns>the config element list</returns>
        private List<HttpHandlerConfigObject> CacheAndGetConfigList()
        {
            var configList = new List<HttpHandlerConfigObject>();
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Config/UrlRoutingHandler.Config"));
            var node = xmlDoc.SelectSingleNode("/HttpHandlerConfigSection");

            if (node != null)
            {
                foreach (XmlNode n in node.ChildNodes)
                {
                    if (n.Attributes != null)
                    {
                        var routingElement = new HttpHandlerConfigObject();

                        if (n.Attributes["id"] != null)
                        {
                            ServiceType serviceType;

                            if (Enum.TryParse(n.Attributes["id"].Value, out serviceType))
                            {
                                routingElement.Id = serviceType;
                            }
                        }

                        if (n.Attributes["type"] != null)
                        {
                            routingElement.ReflectionType = n.Attributes["type"].Value;
                        }

                        if (n.Attributes["url"] != null)
                        {
                            routingElement.Url = n.Attributes["url"].Value;
                        }

                        if (n.Attributes["params"] != null)
                        {
                            routingElement.Params = n.Attributes["params"].Value;
                        }

                        if (routingElement.Id == ServiceType.Default 
                            || string.IsNullOrEmpty(routingElement.ReflectionType))
                        {
                            throw new ArgumentException(ErrorMsgNotNull);
                        }

                        if (configList.FirstOrDefault(o => o.Id == routingElement.Id) == null)
                        {
                            configList.Add(routingElement);
                        }
                    }
                }

                HttpContext.Current.Application[ApplicationConstant.URLROUTING_HANDLER_CONFIG] = configList;
            }

            return configList;
        }

        /// <summary>
        /// Gets the true URL routing config.
        /// </summary>
        /// <param name="configs">The config.</param>
        /// <returns>get the this request's config element.</returns>
        private HttpHandlerConfigObject GetTrueUrlRoutingConfig(List<HttpHandlerConfigObject> configs)
        {
            HttpHandlerConfigObject config = configs.FirstOrDefault(o => o.Id == HandlerType);

            if (config == null)
            {
                throw new ArgumentException(ErrorMsgArgument);
            }

            return config;
        }

        #endregion
    }
}