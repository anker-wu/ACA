/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PageFlowGroupModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PageFlowGroupModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
   public partial class PageFlowGroupModel {
        
        private System.Nullable<System.DateTime> auditDateField;
        
        private string auditIDField;
        
        private string auditStatusField;
        
        private string[] capTypeNameListField;
        
        private string displayReCertificationField;
        
        private string pageFlowGrpCodeField;
        
        private string pageFlowTypeField;
        
        private string serviceProviderCodeField;
        
        private StepModel[] stepListField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public System.Nullable<System.DateTime> auditDate {
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
        [System.Xml.Serialization.XmlElementAttribute("capTypeNameList", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string[] capTypeNameList {
            get {
                return this.capTypeNameListField;
            }
            set {
                this.capTypeNameListField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayReCertification {
            get {
                return this.displayReCertificationField;
            }
            set {
                this.displayReCertificationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pageFlowGrpCode {
            get {
                return this.pageFlowGrpCodeField;
            }
            set {
                this.pageFlowGrpCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pageFlowType {
            get {
                return this.pageFlowTypeField;
            }
            set {
                this.pageFlowTypeField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("stepList", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public StepModel[] stepList {
            get {
                return this.stepListField;
            }
            set {
                this.stepListField = value;
            }
        }
    }
}
