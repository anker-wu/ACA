/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TemplateLayoutConfigI18NModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TemplateLayoutConfigI18NModel.cs 130107 2010-08-13 12:23:56Z ACHIEVO\alan.hu $.
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
    public partial class TemplateLayoutConfigI18NModel : I18NPKModel
    {

        private string alternativeLabelField;
        
        private SimpleAuditModel auditModelField;
        
        private string buttonAddMoreLabelField;
        
        private string buttonAddRowLabelField;
        
        private string buttonDeleteRowLabelField;
        
        private string buttonEditRowLabelField;
        
        private string instructionField;
        
        private string waterMarkField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string alternativeLabel {
            get {
                return this.alternativeLabelField;
            }
            set {
                this.alternativeLabelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SimpleAuditModel auditModel {
            get {
                return this.auditModelField;
            }
            set {
                this.auditModelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string buttonAddMoreLabel {
            get {
                return this.buttonAddMoreLabelField;
            }
            set {
                this.buttonAddMoreLabelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string buttonAddRowLabel {
            get {
                return this.buttonAddRowLabelField;
            }
            set {
                this.buttonAddRowLabelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string buttonDeleteRowLabel {
            get {
                return this.buttonDeleteRowLabelField;
            }
            set {
                this.buttonDeleteRowLabelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string buttonEditRowLabel {
            get {
                return this.buttonEditRowLabelField;
            }
            set {
                this.buttonEditRowLabelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string instruction {
            get {
                return this.instructionField;
            }
            set {
                this.instructionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string waterMark {
            get {
                return this.waterMarkField;
            }
            set {
                this.waterMarkField = value;
            }
        }
    }
}
