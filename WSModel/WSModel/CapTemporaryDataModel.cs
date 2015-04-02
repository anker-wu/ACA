/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapTemporaryDataModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapTemporaryDataModel.cs 239452 2012-12-12 05:56:43Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1009")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapTemporaryDataModel
    {
        private SimpleAuditModel auditModelField;

        private CapTemporaryDataPKModel capTemporaryDataPKModelField;

        private string data1Field;

        private string entityTypeField;

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
        public CapTemporaryDataPKModel capTemporaryDataPKModel
        {
            get
            {
                return this.capTemporaryDataPKModelField;
            }
            set
            {
                this.capTemporaryDataPKModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data1
        {
            get
            {
                return this.data1Field;
            }
            set
            {
                this.data1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityType
        {
            get
            {
                return this.entityTypeField;
            }
            set
            {
                this.entityTypeField = value;
            }
        }
    }
}
