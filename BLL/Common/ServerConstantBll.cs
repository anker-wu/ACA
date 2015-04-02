#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ServerConstantBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ServerConstantBll.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to operation server constant.
    /// </summary>
    public class ServerConstantBll : BaseBll, IServerConstantBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ServerConstantBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of ServerConstantService.
        /// </summary>
        private ServerConstantWebServiceService ServerConstantService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ServerConstantWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the constant value from R1SERVER_CONSTANT table by constant name.
        /// If the constant name can't be found, it will return empty string.
        /// </summary>
        /// <param name="constantName">constant name be retrieved.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <returns>the constant value, return empty string if can't find the value.</returns>
        public string GetServerConstantValue(string constantName, string agencyCode, string callerId)
        {
            //Change the code to get constant from the cache
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();

            Hashtable htStdCates = cacheManager.GetCachedItem(agencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_SERVERCONSTANT));

            if (htStdCates == null || htStdCates.Count == 0)
            {
                return string.Empty;
            }

            if (htStdCates.Contains(constantName))
            {
                return htStdCates[constantName].ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets public user group
        /// </summary>
        /// <param name="moduleName">current module name</param>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="callerId">current caller id</param>
        /// <returns>public user group string</returns>
        public string GetPublicUserGroup(string moduleName, string agencyCode, string callerId)
        {
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));

            string cacheKey = string.Format("{0}{1}{2}", agencyCode, ACAConstant.SPLIT_CHAR, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_PUBLICUSERGROUP));

            Hashtable htUserGroups = cacheManager.GetCachedItem(agencyCode, cacheKey);

            string userGroup = string.Empty;
            
            if (htUserGroups == null || htUserGroups.Count == 0)
            {
                return userGroup;
            }
                       
            if (htUserGroups.Contains(moduleName))
            {
                userGroup = htUserGroups[moduleName].ToString();
            }

            return userGroup;
        }

        /// <summary>
        /// Gets the constant value from R1SERVER_CONSTANT table by constant name.
        /// If the constant name can't be found, it will return defaultValue.
        /// </summary>
        /// <param name="constantName">constant name be retrieved.</param>
        /// <param name="defaultValue">If the constant name can't be found,the defaultValue will be return.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <returns>the constant value, return defaultValue if can't find the value.</returns>
        public string GetServerConstantValue(string constantName, string defaultValue, string agencyCode, string callerId)
        {
            string logMsg = "The server constant name[{0}] isn't exist. please check whether [{0}] is configed into table R1SERVER_CONSTANT.";

            ServerConstantModel4WS[] constants = GetServerConstant(agencyCode, constantName, callerId);

            if (constants == null || constants.Length == 0)
            {
                Logger.ErrorFormat(logMsg, constantName);
                return string.Empty;
            }

            bool isExist = false;
            string constantVlaue = string.Empty;

            // loop to find the matched constant name
            foreach (ServerConstantModel4WS constant in constants)
            {
                //if (constant.serverConstant.ToUpper() == constantName.ToUpper())
                if (constant.serverConstant.Equals(constantName, StringComparison.InvariantCultureIgnoreCase))
                {
                    constantVlaue = constant.serverConstantValue;
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                Logger.ErrorFormat(logMsg, constantName);
            }

            return constantVlaue;
        }

        /// <summary>
        /// Get the server constant list
        /// </summary>
        /// <param name="serviceProviderCode"> The service provider code.</param>
        /// <param name="constantName">The server constant name.</param>
        /// <param name="callerID">The caller ID.</param>
        /// <returns>Array of ServerConstantModel4WS</returns>
        private ServerConstantModel4WS[] GetServerConstant(string serviceProviderCode, string constantName, string callerID)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(constantName))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "constantName" });
            }

            try
            {
                return ServerConstantService.getServerConstant(serviceProviderCode, constantName, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}