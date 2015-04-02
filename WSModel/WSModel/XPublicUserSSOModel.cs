/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XPublicUserSSOModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XPublicUserSSOModel.cs 269106 2014-07-09 07:54:20Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XPublicUserSSOModel : XPublicUserSSOPKModel
    {

        private string accountIDField;

        private string accountTypeField;

        private SimpleAuditModel auditModelField;

        private System.DateTime? effectiveTimeField;

        private string publicUserUUIDField;

        private long? userSeqNbrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accountID
        {
            get
            {
                return this.accountIDField;
            }
            set
            {
                this.accountIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accountType
        {
            get
            {
                return this.accountTypeField;
            }
            set
            {
                this.accountTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SimpleAuditModel auditModel
        {
            get
            {
                return this.auditModelField;
            }
            set
            {
                this.auditModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime? effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string publicUserUUID
        {
            get
            {
                return this.publicUserUUIDField;
            }
            set
            {
                this.publicUserUUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long? userSeqNbr
        {
            get
            {
                return this.userSeqNbrField;
            }
            set
            {
                this.userSeqNbrField = value;
            }
        }
    }
}
