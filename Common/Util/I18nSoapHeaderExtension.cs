#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nSoapHeaderExtension.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  I18nSoapHeaderExtension for getting I18n soap header information.
 *
 *  Notes:
 * $Id: I18nSoapHeaderExtension.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;

using Accela.ACA.Common.Log;

using log4net;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// provides I18n soap header extension to serve the framework. 
    /// </summary>
    public class I18nSoapHeaderExtension : SoapExtension
    {
        #region Fields

        /// <summary>
        /// culture info insensitive methods, format List([web service full name].[method name]), 
        /// if ended with *, then all methods under that web service are culture info insensitive.
        /// tips: list item is case sensitive.
        /// </summary>
        private static readonly List<string> CultureInfoInsensitiveMethods = new List<string>()
                {
                    "Accela.ACA.WSProxy.I18nSettingsWebServiceService.*",
                    "Accela.ACA.WSProxy.CashierWebServiceService.postTransactionLog",
                    "Accela.ACA.WSProxy.GenericViewWebServiceService.getXUIGUITextList",
                    "Accela.ACA.WSProxy.BizDomainWebServiceService.getBizDomainValue",
                    "Accela.ACA.WSProxy.PolicyWebServiceService.getXPolicyList",
                    "Accela.ACA.WSProxy.PolicyWebServiceService.getXpolicyUserRoleList"
                };

        /// <summary>
        /// the logger info.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger("I18nSoapHeaderExtension");

        /// <summary>
        /// indication for is MTOM.
        /// </summary>
        private bool isMTOM;

        /// <summary>
        /// new stream.
        /// </summary>
        private Stream newStream;

        /// <summary>
        /// the old stream.
        /// </summary>
        private Stream oldStream;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets current user property
        /// </summary>
        public static string CurrentUser
        {
            get
            {
                if (null == HttpContext.Current || null == HttpContext.Current.Session)
                {
                    return string.Empty;
                }

                string tempName = HttpContext.Current.Session["I18nSoapHeaderExtension_CurrentUser"] as string;

                if (null == tempName)
                {
                    tempName = string.Empty;
                }

                return tempName;
            }

            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                {
                    HttpContext.Current.Session["I18nSoapHeaderExtension_CurrentUser"] = value;
                }
            }
        }

        /// <summary>
        /// Gets the name of the module from current request URL queryString.
        /// </summary>
        /// <value>The name of the module.</value>
        private static string ModuleName
        {
            get
            {
                var request = HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request : null;
                var moduleName = request != null ? request.QueryString[ACAConstant.MODULE_NAME] : string.Empty;
                moduleName = string.IsNullOrEmpty(moduleName) ? string.Empty : moduleName;
                return moduleName;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Save the Stream representing the SOAP request or SOAP response into a local memory buffer.
        /// </summary>
        /// <param name="stream">stream information.</param>
        /// <returns>Chain stream</returns>
        public override Stream ChainStream(Stream stream)
        {
            if (isMTOM)
            {
                return stream;
            }

            oldStream = stream;
            newStream = new MemoryStream();

            return newStream;
        }

        /// <summary>
        /// override method
        /// </summary>
        /// <param name="serviceType">service type.</param>
        /// <returns>service type full name</returns>
        public override object GetInitializer(Type serviceType)
        {
            return serviceType.FullName;
        }

        /// <summary>
        /// override method
        /// </summary>
        /// <param name="methodInfo">method Info</param>
        /// <param name="attribute">attribute information</param>
        /// <returns>Initializer information</returns>
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        /// <summary>
        /// override method
        /// </summary>
        /// <param name="initializer">initializer information</param>
        public override void Initialize(object initializer)
        {
            if ("Accela.ACA.BLL.Plan.UploadAttachmentWS".Equals(initializer) || "Accela.ACA.WSProxy.EDMSDocumentUploadWebServiceService".Equals(initializer) || "Accela.ACA.Web.WebService.GFilterViewService".Equals(initializer))
            {
                isMTOM = true;
            }
            else
            {
                isMTOM = false;
            }

            return;
        }

        /// <summary>
        /// override method
        /// process message
        /// </summary>
        /// <param name="message">the message information.</param>
        public override void ProcessMessage(SoapMessage message)
        {
            if (isMTOM)
            {
                return;
            }

            try
            {
                switch (message.Stage)
                {
                    case SoapMessageStage.BeforeSerialize:
                        if (Logger.IsDebugEnabled)
                        {
                            Logger.DebugFormat("BeforeSerialize   - {0} : {1}", message.MethodInfo, I18nDateTimeUtil.GetMilliSecond());
                        }

                        break;
                    case SoapMessageStage.AfterSerialize:
                        if (Logger.IsDebugEnabled)
                        {
                            Logger.DebugFormat("AfterSerialize    - {0} : {1}", message.MethodInfo, I18nDateTimeUtil.GetMilliSecond());
                        }

                        SetupOldStream(message);
                        break;
                    case SoapMessageStage.BeforeDeserialize:
                        if (Logger.IsDebugEnabled)
                        {
                            Logger.DebugFormat("BeforeDeserialize - {0} : {1}", message.MethodInfo, I18nDateTimeUtil.GetMilliSecond());
                        }

                        SetupNewStream(message);
                        break;
                    case SoapMessageStage.AfterDeserialize:
                        if (Logger.IsDebugEnabled)
                        {
                            Logger.DebugFormat("AfterDeserialize  - {0} : {1}", message.MethodInfo, I18nDateTimeUtil.GetMilliSecond());
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred in method ProcessMessage, status={0}, Method:{1}, Exception: {2}", message == null ? string.Empty : message.Stage.ToString(), message.MethodInfo, ex);

                throw;
            }
        }

        /// <summary>
        /// Get key values of custom node
        /// </summary>
        /// <param name="isMethodCultureInfoSensitive">parameter isMethodCultureInfoSensitive</param>
        /// <returns>key values of custom node</returns>
        private Dictionary<string, string> GetKeyValuesOfCustomNode(bool isMethodCultureInfoSensitive)
        {
            string languageCode = string.Empty;
            string regionalCode = string.Empty;

            if (isMethodCultureInfoSensitive)
            {
                languageCode = I18nCultureUtil.GetLanguageCodeForSoapHandler();
                regionalCode = I18nCultureUtil.GetRegionalCodeForSoapHandler();
                languageCode = string.Format("{0}_{1}", languageCode, regionalCode);
            }

            string agencyCode = ACAConstant.AgencyCode;
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            keyValues.Add("countryCode", regionalCode);
            keyValues.Add("langCode", languageCode);
            keyValues.Add("serviceProviderCode", agencyCode);
            keyValues.Add("currentUser", CurrentUser);
            keyValues.Add("module", ModuleName);

            return keyValues;
        }

        /// <summary>
        /// Determines whether method of web service in soap message is culture info sensitive.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// <c>true</c> if method of web service in soap message is culture info sensitive; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMethodCultureInfoSensitive(SoapMessage message)
        {
            bool result = true;

            if (message != null && message.MethodInfo != null && message.MethodInfo.DeclaringType != null)
            {
                string webServiceFullName = message.MethodInfo.DeclaringType.FullName;
                webServiceFullName = string.IsNullOrEmpty(webServiceFullName) ? string.Empty : webServiceFullName;

                string methodName = message.MethodInfo.Name;
                methodName = string.IsNullOrEmpty(methodName) ? string.Empty : methodName;

                string key4All = string.Format("{0}.*", webServiceFullName);
                string key4Single = string.Format("{0}.{1}", webServiceFullName, methodName);

                if (CultureInfoInsensitiveMethods.Contains(key4All) || CultureInfoInsensitiveMethods.Contains(key4Single))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Get xml with appended header
        /// </summary>
        /// <param name="soapXml">soap Xml content.</param>
        /// <param name="keyValues">key values.</param>
        /// <returns>Xml with appended header</returns>
        private string GetXmlWithAppendedHeader(string soapXml, Dictionary<string, string> keyValues)
        {
            bool hasHeader = false;

            //build xmlDocument
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapXml);

            //get rootNodeList, headerNodeList, rootNode
            XmlNodeList rootNodeList = xmlDocument.GetElementsByTagName("soap:Envelope");
            XmlNode rootNode = rootNodeList[0];
            XmlNodeList headerNodeList = xmlDocument.GetElementsByTagName("soap:Header");
            hasHeader = headerNodeList.Count > 0;

            //get headerNode
            XmlNode headerNode;

            if (hasHeader)
            {
                headerNode = headerNodeList[0];
            }
            else
            {
                headerNode = xmlDocument.CreateElement("soap", "Header", rootNode.NamespaceURI);
                XmlAttribute headerNodeAttribute = xmlDocument.CreateAttribute("encodingStyle");
                headerNodeAttribute.Value = "http://schemas.xmlsoap.org/soap/encoding/";
                headerNode.Attributes.Append(headerNodeAttribute);
            }

            if (null != keyValues)
            {
                foreach (string key in keyValues.Keys)
                {
                    XmlNode tempNode = headerNode.SelectSingleNode(key);
                    string tempNodeValue = keyValues[key];

                    if (null == tempNode)
                    {
                        //tempNode = xmlDocument.CreateElement(key);
                        tempNode = xmlDocument.CreateElement(key, "http://service.webservice.accela.com");
                        tempNode.InnerXml = tempNodeValue;
                        headerNode.AppendChild(tempNode);
                    }
                    else
                    {
                        tempNode.InnerXml = tempNodeValue;
                    }
                }
            }

            //insert headerNode to the first place of rootNode
            if (!hasHeader)
            {
                rootNode.InsertBefore(headerNode, rootNode.FirstChild);
            }

            //return xml with appended header
            return xmlDocument.InnerXml;
        }

        /// <summary>
        /// setup new stream
        /// </summary>
        /// <param name="message">Soap Message.</param>
        private void SetupNewStream(SoapMessage message)
        {
            if (message.Stage == SoapMessageStage.BeforeDeserialize)
            {
                StreamReader streamReader = new StreamReader(oldStream);
                StreamWriter streamWriter = new StreamWriter(newStream);
                string soapXml = streamReader.ReadToEnd();
                streamWriter.Write(soapXml);
                streamWriter.Flush();
                newStream.Position = 0;
            }
        }

        /// <summary>
        /// setup old stream
        /// </summary>
        /// <param name="message">the message info</param>
        private void SetupOldStream(SoapMessage message)
        {
            if (message.Stage == SoapMessageStage.AfterSerialize && message is System.Web.Services.Protocols.SoapClientMessage)
            {
                newStream.Position = 0;
                StreamReader streamReader = new StreamReader(newStream);
                StreamWriter streamWriter = new StreamWriter(oldStream);
                string soapXml = streamReader.ReadToEnd();

                bool isMethodCultureInfoSensitive = IsMethodCultureInfoSensitive(message);

                //get xml with appended header
                soapXml = GetXmlWithAppendedHeader(soapXml, GetKeyValuesOfCustomNode(isMethodCultureInfoSensitive));

                streamWriter.Write(soapXml);
                streamWriter.Flush();
            }
        }

        #endregion Methods
    }
}