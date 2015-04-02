/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DataModelManager.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Accela.ACA.FormDesigner.GFilterViewService;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Accela.ACA.FormDesigner
{
    /// <summary>
    /// the class for DataModelManager
    /// </summary>
    public class DataModelManager
    {
        /// <summary>
        /// for store a GFilterViewWebServiceClient object.
        /// </summary>
        private GFilterViewServiceSoapClient  gfilterViewWebService;

        /// <summary>
        /// for store a DataModelManager instance.
        /// </summary>
        private static DataModelManager instance;

        /// <summary>
        /// gets a GFilterViewWebServiceClient object.
        /// </summary>
        public GFilterViewServiceSoapClient BusinessWebServiceObject
        {
            get { return gfilterViewWebService; }
        }

        /// <summary>
        /// gets or sets RelationModel4WS
        /// </summary>
        public GFilterScreenPermissionModel4WS ScreenPermissionModel4WS
        {
            get;
            set;
        }

        /// <summary>
        /// gets or sets ServProvCode
        /// </summary>
        public string ServProvCode
        {
            get;
            set;
        }

        /// <summary>
        /// gets or sets LevelType
        /// </summary>
        public string LevelType
        {
            get;
            set;
        }

        /// <summary>
        /// gets or sets LevelName
        /// </summary>
        public string LevelName
        {
            get;
            set;
        }

        /// <summary>
        /// gets or sets CallerId
        /// </summary>
        public string CallerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets CountryCode
        /// </summary>
        public string CountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets LangCode
        /// </summary>
        public string LangCode
        {
            get;
            set;
        }

        /// <summary>
        /// Initialize a new instance for private.
        /// </summary>
        private DataModelManager()
        {
        }

        /// <summary>
        /// method for get a DataModelManager instance .
        /// </summary>
        /// <param name="relationModel4WS">GFilterScreenRelationModel4WS object</param>
        /// <returns>DataModelManager instance</returns>
        public static DataModelManager GetDataModelManager(BussinessParam param)
        {
            if (instance == null)
            {
                instance = new DataModelManager();
            }

            GFilterScreenPermissionModel4WS permissionModel = new GFilterScreenPermissionModel4WS()
            {
                servProvCode = param.ServProvCode,
                permissionLevel = param.PermissionLevel == "CONTACT" || param.PermissionLevel == "LICENSE" ? "PEOPLE" : param.PermissionLevel,
                permissionValue = param.PermissionValue,
                recFulName = param.CallerId
            };

            instance.LevelName = param.LevelName;
            instance.LevelType = param.LevelType;
            instance.ServProvCode = param.ServProvCode;
            instance.CallerId = param.CallerId;
            instance.CountryCode = param.CountryCode;
            instance.LangCode = param.LangCode;

            instance.gfilterViewWebService = GetWebService(param.ServiceUrl);
            instance.ScreenPermissionModel4WS = permissionModel;
            return instance;
        }

        /// <summary>
        /// method for initialize webservice
        /// </summary>
        /// <param name="serviceUrl">service url</param>
        /// <returns>GFilterViewWebServiceClient object</returns>
        private static GFilterViewServiceSoapClient GetWebService(string serviceUrl)
        {
            GFilterViewServiceSoapClient gfilterViewWebService = null;
            if (!string.IsNullOrEmpty(serviceUrl))
            {
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
                if ("https".Equals(serviceUrl.Substring(0, 5), StringComparison.InvariantCultureIgnoreCase))
                {
                    basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                }

                basicHttpBinding.ReceiveTimeout=new TimeSpan(30000);
                basicHttpBinding.MaxReceivedMessageSize = 2147483647;

                //CustomBinding binding = new CustomBinding(basicHttpBinding);
                //binding.ReceiveTimeout = 
                //BindingElement binaryElement = new BinaryMessageEncodingBindingElement();
                //binding.Elements.Remove(binding.Elements[0]);
                //binding.Elements.Insert(0, binaryElement);

                EndpointAddress endPoint = new EndpointAddress(new Uri(serviceUrl + "/GFilterViewService.asmx", UriKind.RelativeOrAbsolute));

                gfilterViewWebService = (GFilterViewServiceSoapClient)Activator.CreateInstance(typeof(GFilterViewServiceSoapClient), basicHttpBinding, endPoint);
            }

            return gfilterViewWebService;
        }

        /// <summary>
        /// method for get a DataModelManager instance .
        /// </summary>
        /// <returns>DataModelManager instance</returns>
        public static DataModelManager GetDataModelManager(string serviceUrl)
        {
            if (instance == null)
            {
                instance = new DataModelManager();
            }

            instance.gfilterViewWebService = GetWebService(serviceUrl);
            return instance;
        }

        /// <summary>
        /// Get a AddessHearder array
        /// </summary>
        /// <returns>AddressHeader array</returns>
        public AddressHeader[] GetAddressHearderArray()
        {
            AddressHeader countryCode = AddressHeader.CreateAddressHeader("countryCode", FDConstant.SERVICE_NS, CountryCode);
            AddressHeader langCode = AddressHeader.CreateAddressHeader("langCode", FDConstant.SERVICE_NS, LangCode);
            AddressHeader serviceProviderCode = AddressHeader.CreateAddressHeader("serviceProviderCode", FDConstant.SERVICE_NS, ServProvCode);
            AddressHeader currentUser = AddressHeader.CreateAddressHeader("currentUser", FDConstant.SERVICE_NS, CallerId);
            AddressHeader[] headers = new AddressHeader[] { countryCode, langCode, serviceProviderCode, currentUser };

            return headers;
        }

        /// <summary>
        /// method for load SimpleViewModel4WS object by view id
        /// </summary>
        /// <param name="viewId">view id string</param>
        public void Load(String viewId)
        {
            gfilterViewWebService.GetFilterScreenViewAsync(this.ServProvCode, this.LevelType, this.LevelName, viewId, this.ScreenPermissionModel4WS, this.CallerId);
        }

        /// <summary>
        /// method for save SimpleViewModel4WS 
        /// </summary>
        /// <param name="simpleViewMode">SimpleViewModel4WS object</param>
        public void Save(SimpleViewModel4WS simpleViewMode)
        {
            gfilterViewWebService.SaveFilterScreenViewAsync(this.ServProvCode, this.LevelType, this.LevelName, simpleViewMode, this.CallerId);
        }
    }
}
