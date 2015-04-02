/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ServiceModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ServiceModel.cs 171698 2013-09-17 09:39:39Z ACHIEVO\jone.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ServiceModel : LanguageModel {
        
        private string aSIGroupCodeField;
        
        private string aSISubGroupCodeField;
        
        private CapTypeModel capTypeField;
        
        private string[] licProTypeField;
        
        private RecordTypeLicTypePermissionModel[] licTypePermissionsField;
        
        private System.DateTime recDateField;
        
        private string recFullNameField;
        
        private string recStatusField;
        
        private string resServiceNameField;
        
        private string servPorvCodeField;
        
        private string serviceNameField;
        
        private long sourceNumberField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ASIGroupCode {
            get {
                return this.aSIGroupCodeField;
            }
            set {
                this.aSIGroupCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ASISubGroupCode {
            get {
                return this.aSISubGroupCodeField;
            }
            set {
                this.aSISubGroupCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapTypeModel capType {
            get {
                return this.capTypeField;
            }
            set {
                this.capTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("licProType", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string[] licProType {
            get {
                return this.licProTypeField;
            }
            set {
                this.licProTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("licTypePermissions", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public RecordTypeLicTypePermissionModel[] licTypePermissions {
            get {
                return this.licTypePermissionsField;
            }
            set {
                this.licTypePermissionsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime recDate {
            get {
                return this.recDateField;
            }
            set {
                this.recDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFullName {
            get {
                return this.recFullNameField;
            }
            set {
                this.recFullNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recStatus {
            get {
                return this.recStatusField;
            }
            set {
                this.recStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resServiceName {
            get {
                return this.resServiceNameField;
            }
            set {
                this.resServiceNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servPorvCode {
            get {
                return this.servPorvCodeField;
            }
            set {
                this.servPorvCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceName {
            get {
                return this.serviceNameField;
            }
            set {
                this.serviceNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long sourceNumber {
            get {
                return this.sourceNumberField;
            }
            set {
                this.sourceNumberField = value;
            }
        }
    }
}
