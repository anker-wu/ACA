#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ObjectFactory.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ObjectFactory.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;
using Accela.ACA.Common.Advise;
using Accela.ACA.Common.Log;
using log4net;
using Spring.Aop.Framework;
using Spring.Context;
using Spring.Context.Support;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Object Factory class to produce the object by configuration.
    /// </summary>
    public static class ObjectFactory
    {
        #region Fields

        /// <summary>
        /// admin suffix
        /// </summary>
        private const string ADMIN_SUFFIX = "_Admin";

        /// <summary>
        /// logger info.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger("ObjectFactory");

        /// <summary>
        /// context information.
        /// </summary>
        private static IApplicationContext _context;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets object context.
        /// </summary>
        private static IApplicationContext ObjectContext
        {
            get
            {
                if (_context == null)
                {
                    _context = ContextRegistry.GetContext();

                    if (_context == null)
                    {
                        Logger.Fatal("The object context can't be found, please check the configuration for spring.net whether is configurated correctly.");
                    }
                }

                return _context;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets an object implement by the interface type.
        /// </summary>
        /// <param name="interfaceType">interface type, which name is used as object id to retrieve the according object.</param>
        /// <returns>an object.</returns>
        public static object GetObject(Type interfaceType)
        {
            return GetObject(interfaceType, IsAdminMode());
        }

        /// <summary>
        /// Gets the object implement by the interface type.
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <returns>the object with target type</returns>
        public static T GetObject<T>() where T : class
        {
            return (T)GetObject(typeof(T), IsAdminMode());
        }

        /// <summary>
        /// Gets an object implement by the interface type.
        /// </summary>
        /// <param name="interfaceType">interface type, which name is used as object id to retrieve the according object.</param>
        /// <param name="isAdmin">indicates whether need to get an admin implement object</param>
        /// <returns>an object.</returns>
        public static object GetObject(Type interfaceType, bool isAdmin)
        {
            string objectId = interfaceType.Name;

            // all of admin mode, the object id is required to end with "_Admin", it is contract in ACA.
            if (isAdmin)
            {
                // if there have been configurated the admin implement,It will get admin implement.
                // if not, system will use the daily implement.
                if (ObjectContext.ContainsObject(objectId + ADMIN_SUFFIX))
                {
                    objectId += ADMIN_SUFFIX;
                }
            }

            return GetObject(objectId);
        }

        /// <summary>
        /// Gets an object by unique object id 
        /// </summary>
        /// <param name="objectId">unique object id.</param>
        /// <returns>an object.</returns>
        public static object GetObject(string objectId)
        {
            try
            {
                object obj = ObjectContext.GetObject(objectId);

                if (obj == null)
                {
                    throw new ACAException(string.Format("The object {0} can't be found from object container.", objectId));
                }

                if (Logger.IsDebugEnabled)
                {
                    if ((objectId != "IBizDomainBll" && objectId != "IGviewBll" && objectId != "IViewBll" && objectId != "II18nSettingsBll" && objectId != "IXPolicyBll" && objectId != "ICapTypeFilterBll") && (objectId.EndsWith("Bll") || objectId.EndsWith("Bll_Admin")))
                    {
                        ProxyFactory factory = new ProxyFactory(obj);

                        factory.AddAdvice(new AroundAdvise());
                        ////factory.AddAdvice(new BeforeAdvise());
                        ////factory.AddAdvice(new AfterReturningAdvise());
                        factory.AddAdvice(new ThrowsAdvise());

                        obj = factory.GetProxy();
                    }
                }

                return obj;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
        
        /// <summary>
        /// Gets the object by configuration.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="configPath">The config file path.</param>
        /// <param name="interfaceKey">The interface key specified in the config file's node.</param>
        /// <returns>Return the generic type's object.</returns>
        public static T GetObjectByConfiguration<T>(string configPath, string interfaceKey) where T : class
        {
            if (!File.Exists(configPath))
            {
                return null;
            }

            T result = null;
            XmlDocument doc = new XmlDocument();

            // load the xml config file
            try
            {
                doc.Load(configPath);
            }
            catch (Exception ex)
            {
                // NOT throw exception
                Logger.Error(ex.Message);
                return null;
            }

            if (doc.DocumentElement == null || doc.DocumentElement.ChildNodes == null)
            {
                return null;
            }

            // parse the xml config file to search the <object id="IGrantPermission"> node
            int allNodeNum = 0;
            int correctNodeNum = 0;

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    allNodeNum++;
                }

                // search the <object id="IGrantPermission"> node
                if (node.NodeType == XmlNodeType.Element &&
                    string.Equals(node.Name, "object", StringComparison.InvariantCultureIgnoreCase) &&
                    node.Attributes != null &&
                    string.Equals(node.Attributes["id"].Value, interfaceKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    string typeDescrption = node.Attributes["type"].Value;
                    result = GetObjectByTypeDesc<T>(typeDescrption, configPath);
                    correctNodeNum++;
                    break;
                }
            }

            if (correctNodeNum != allNodeNum && allNodeNum > 0)
            {
                Logger.ErrorFormat("The configuration file \"{0}\" is incorrect.", configPath);
            }

            return result;
        }

        /// <summary>
        /// Gets the object by the type description string that format as "Namespace, AssemblyName".
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="typeDescription">The type description.</param>
        /// <param name="configPath">The config path.</param>
        /// <returns>Return the generic type's object.</returns>
        private static T GetObjectByTypeDesc<T>(string typeDescription, string configPath) where T : class
        {
            if (string.IsNullOrEmpty(typeDescription))
            {
                Logger.ErrorFormat("The configuration file \"{0}\" is incorrect.", configPath);
                return null;
            }

            // parse the type string that format as "Namespace, AssemblyName"
            string[] typeArray = typeDescription.Split(',');

            if (typeArray.Length != 2 || string.IsNullOrEmpty(typeArray[0].Trim()) || string.IsNullOrEmpty(typeArray[1].Trim()))
            {
                Logger.ErrorFormat("The type \"{0}\" format is incorrect in the file \"{1}\", please use the format type=\"Namespace, AssemblyName\"", typeDescription, configPath);
                return null;
            }

            T result = null;
            string custNamespace = typeArray[0].Trim();
            string custAssemblyName = typeArray[1].Trim();

            try
            {
                Assembly assembly = Assembly.Load(custAssemblyName);
                Type type = assembly.GetType(custNamespace);

                result = Activator.CreateInstance(type) as T;
            }
            catch (Exception ex)
            {
                // NOT throw exception
                Logger.Error(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Indicates the current page whether need to be presented as admin.
        /// </summary>
        /// <returns>true - admin mode,false-daily mode.</returns>
        private static bool IsAdminMode()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return false;
            }

            object isAdmin = HttpContext.Current.Session[SessionConstant.SESSION_ADMIN_MODE];

            if (isAdmin != null && isAdmin.ToString() == ACAConstant.COMMON_Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Methods
    }
}