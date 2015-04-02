﻿/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XServiceGroupModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XServiceGroupModel.cs 171698 2013-09-17 09:39:39Z ACHIEVO\jone.lu $.
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
    public partial class XServiceGroupModel  {
        
        private System.DateTime auditDateField;
        
        private string auditIDField;
        
        private string auditStatusField;
        
        private ServiceGroupModel groupField;
        
        private ServiceModel serviceField;
        
        private string serviceProviderCodeField;

        private System.Nullable<int> sortOrderField;
        
        private long serviceGroupSeqNbrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime auditDate {
            get {
                return this.auditDateField;
            }
            set {
                this.auditDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditID {
            get {
                return this.auditIDField;
            }
            set {
                this.auditIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditStatus {
            get {
                return this.auditStatusField;
            }
            set {
                this.auditStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ServiceGroupModel group {
            get {
                return this.groupField;
            }
            set {
                this.groupField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ServiceModel service {
            get {
                return this.serviceField;
            }
            set {
                this.serviceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode {
            get {
                return this.serviceProviderCodeField;
            }
            set {
                this.serviceProviderCodeField = value;
            }
        }
        
         /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public System.Nullable<int> sortOrder {
            get {
                return this.sortOrderField;
            }
            set {
                this.sortOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long serviceGroupSeqNbr {
            get {
                return this.serviceGroupSeqNbrField;
            }
            set {
                this.serviceGroupSeqNbrField = value;
            }
        }
    }
}
