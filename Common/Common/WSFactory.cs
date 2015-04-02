/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: WSFactory.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 *  Web Service factory to generate web service instance
 *  Notes:
 * $Id: WSFactory.cs 171719 2010-04-29 10:28:45Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System.Collections.Generic;
using System.Web.Services.Protocols;
using Accela.ACA.Common.Config;
using Microsoft.Web.Services3;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Web Service Factory to ensure that all of web service instance is created by this factory.
    /// WSFactory is singleton class.
    /// </summary>
    public class WSFactory
    {
        #region Public Fields

        /// <summary>
        /// singleton pattern.
        /// </summary>
        public static readonly WSFactory Instance = new WSFactory();

        #endregion Public Fields

        #region Private Variables

        /// <summary>
        /// the web service instance that use SoapHttpClientProtocol
        /// </summary>
        private Dictionary<string, SoapHttpClientProtocol> _wsInstances = new Dictionary<string, SoapHttpClientProtocol>();
        
        /// <summary>
        /// the web service instance that use WebServicesClientProtocol
        /// </summary>
        private Dictionary<string, WebServicesClientProtocol> _ws3Instances = new Dictionary<string, WebServicesClientProtocol>();

        #endregion Private Variables

        /// <summary>
        /// Get Web Service instance according to the template T.
        /// </summary>
        /// <typeparam name="T">Web Service class which must inherit from SoapHttpClientProtocol.</typeparam>
        /// <returns>A instance of T.</returns>
        public T GetWebService<T>() where T : SoapHttpClientProtocol, new()
        {
            T t;
            string wsName = typeof(T).FullName;

            if (_wsInstances.ContainsKey(wsName))
            {
                t = _wsInstances[wsName] as T;
            }
            else
            {
                lock (typeof(T))
                {
                    if (!_wsInstances.ContainsKey(wsName))
                    {
                        t = new T();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(T));
                        t.Url = p.Url;
                        t.Timeout = p.Timeout;
                        _wsInstances.Add(wsName, t);
                    }
                    else
                    {
                        t = _wsInstances[wsName] as T;
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// Get Web Service instance according to the template T.
        /// </summary>
        /// <typeparam name="T">Web Service class which must inherit from WebServicesClientProtocol.</typeparam>
        /// <returns>A instance of T.</returns>
        public T GetWebService3<T>() where T : WebServicesClientProtocol, new()
        {
            T t;
            string wsName = typeof(T).FullName;

            if (_ws3Instances.ContainsKey(wsName))
            {
                t = _ws3Instances[wsName] as T;
            }
            else
            {
                lock (typeof(T))
                {
                    if (!_ws3Instances.ContainsKey(wsName))
                    {
                        t = new T();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(T));
                        t.Url = p.Url;
                        t.Timeout = p.Timeout;
                        _ws3Instances.Add(wsName, t);
                    }
                    else
                    {
                        t = _ws3Instances[wsName] as T;
                    }
                }
            }

            return t;
        }
    }
}