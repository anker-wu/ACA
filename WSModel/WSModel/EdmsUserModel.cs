/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: EdmsUserModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: EdmsUserModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class EdmsUserModel
    {

        private string authIDField;

        private string authTypeField;

        private string edmsNameField;

        private string edmsPasswordField;

        private string edmsUserNameField;

        private long policySeqField;

        private bool policySeqFieldSpecified;

        private System.Nullable<System.DateTime> recDateField;

        private string recFulNameField;

        private string recStatusField;

        private string serviceProviderCodeField;

        private string updatePwdFlagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string authID
        {
            get
            {
                return this.authIDField;
            }
            set
            {
                this.authIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string authType
        {
            get
            {
                return this.authTypeField;
            }
            set
            {
                this.authTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string edmsName
        {
            get
            {
                return this.edmsNameField;
            }
            set
            {
                this.edmsNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string edmsPassword
        {
            get
            {
                return this.edmsPasswordField;
            }
            set
            {
                this.edmsPasswordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string edmsUserName
        {
            get
            {
                return this.edmsUserNameField;
            }
            set
            {
                this.edmsUserNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long policySeq
        {
            get
            {
                return this.policySeqField;
            }
            set
            {
                this.policySeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool policySeqSpecified
        {
            get
            {
                return this.policySeqFieldSpecified;
            }
            set
            {
                this.policySeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> recDate
        {
            get
            {
                return this.recDateField;
            }
            set
            {
                this.recDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFulName
        {
            get
            {
                return this.recFulNameField;
            }
            set
            {
                this.recFulNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recStatus
        {
            get
            {
                return this.recStatusField;
            }
            set
            {
                this.recStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string updatePwdFlag
        {
            get
            {
                return this.updatePwdFlagField;
            }
            set
            {
                this.updatePwdFlagField = value;
            }
        }
    }



}
