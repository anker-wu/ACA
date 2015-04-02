/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ReportInfoModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ReportInfoModel4WS.cs 252276 2013-06-20 03:16:32Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ReportInfoModel4WS
    {

        private string callerIdField;

        private EDMSEntityIdModel4WS edMSEntityIdModelField;

        private string emailAddressField;

        private string emailReportNameField;

        private string isFromField;

        private bool isPreviewField;

        private string moduleField;

        private bool notSaveToEDMSField;

        private long reportIdField;

        private string reportIdFromOthersField;

        private string[] reportParameterMapKeysField;

        private string[] reportParametersMapValuesField;

        private string reprintReasonField;

        private string servProvCodeField;

        private string ssOAuthIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string callerId
        {
            get
            {
                return this.callerIdField;
            }
            set
            {
                this.callerIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public EDMSEntityIdModel4WS edMSEntityIdModel
        {
            get
            {
                return this.edMSEntityIdModelField;
            }
            set
            {
                this.edMSEntityIdModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string emailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string emailReportName
        {
            get
            {
                return this.emailReportNameField;
            }
            set
            {
                this.emailReportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isFrom
        {
            get
            {
                return this.isFromField;
            }
            set
            {
                this.isFromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isPreview
        {
            get
            {
                return this.isPreviewField;
            }
            set
            {
                this.isPreviewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string module
        {
            get
            {
                return this.moduleField;
            }
            set
            {
                this.moduleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool notSaveToEDMS
        {
            get
            {
                return this.notSaveToEDMSField;
            }
            set
            {
                this.notSaveToEDMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long reportId
        {
            get
            {
                return this.reportIdField;
            }
            set
            {
                this.reportIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reportIdFromOthers
        {
            get
            {
                return this.reportIdFromOthersField;
            }
            set
            {
                this.reportIdFromOthersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reportParameterMapKeys", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] reportParameterMapKeys
        {
            get
            {
                return this.reportParameterMapKeysField;
            }
            set
            {
                this.reportParameterMapKeysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reportParametersMapValues", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] reportParametersMapValues
        {
            get
            {
                return this.reportParametersMapValuesField;
            }
            set
            {
                this.reportParametersMapValuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reprintReason
        {
            get
            {
                return this.reprintReasonField;
            }
            set
            {
                this.reprintReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servProvCode
        {
            get
            {
                return this.servProvCodeField;
            }
            set
            {
                this.servProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ssOAuthId
        {
            get
            {
                return this.ssOAuthIdField;
            }
            set
            {
                this.ssOAuthIdField = value;
            }
        }
    }

}
