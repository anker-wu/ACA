/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TemplateLayoutConfigModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TemplateLayoutConfigModel.cs 130107 2010-08-13 12:23:56Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class TemplateLayoutConfigModel : TemplateLayoutConfigPKModel
    {

        private string alternativeLabelField;
        
        private SimpleAuditModel auditModelField;
        
        private string buttonAddDisplayField;
        
        private string buttonAddMoreLabelField;
        
        private string buttonAddRowLabelField;
        
        private string buttonDeleteDisplayField;
        
        private string buttonDeleteRowLabelField;
        
        private string buttonEditDisplayField;
        
        private string buttonEditRowLabelField;
        
        private ColumnArrangement columnArrangementField;
        
        private System.Nullable<int> columnLayoutField;
                
        private ConfigLevel configLevelField;
        
        private TemplateEntityType entityTypeField;
                
        private string fieldNameField;
        
        private TemplateLayoutConfigI18NModel i18NModelField;
        
        private string instructionField;
        
        private LableDisplay labelDisplayField;
        
        private string templateCodeField;
        
        private TemplateLayoutConfigI18NModel[] templateLayoutConfigI18NModelsField;
        
        private string templateTypeField;
        
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
        public string buttonAddDisplay {
            get {
                return this.buttonAddDisplayField;
            }
            set {
                this.buttonAddDisplayField = value;
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
        public string buttonDeleteDisplay {
            get {
                return this.buttonDeleteDisplayField;
            }
            set {
                this.buttonDeleteDisplayField = value;
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
        public string buttonEditDisplay {
            get {
                return this.buttonEditDisplayField;
            }
            set {
                this.buttonEditDisplayField = value;
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
        public ColumnArrangement columnArrangement {
            get {
                return this.columnArrangementField;
            }
            set {
                this.columnArrangementField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public System.Nullable<int> columnLayout {
            get {
                return this.columnLayoutField;
            }
            set {
                this.columnLayoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ConfigLevel configLevel {
            get {
                return this.configLevelField;
            }
            set {
                this.configLevelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateEntityType entityType {
            get {
                return this.entityTypeField;
            }
            set {
                this.entityTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fieldName {
            get {
                return this.fieldNameField;
            }
            set {
                this.fieldNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateLayoutConfigI18NModel i18NModel {
            get {
                return this.i18NModelField;
            }
            set {
                this.i18NModelField = value;
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
        public LableDisplay labelDisplay {
            get {
                return this.labelDisplayField;
            }
            set {
                this.labelDisplayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string templateCode {
            get {
                return this.templateCodeField;
            }
            set {
                this.templateCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("templateLayoutConfigI18N", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public TemplateLayoutConfigI18NModel[] templateLayoutConfigI18NModels {
            get {
                return this.templateLayoutConfigI18NModelsField;
            }
            set {
                this.templateLayoutConfigI18NModelsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string templateType {
            get {
                return this.templateTypeField;
            }
            set {
                this.templateTypeField = value;
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public enum ColumnArrangement
    {

        /// <remarks/>
        Horizontal,

        /// <remarks/>
        Vertical,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public enum ConfigLevel
    {

        /// <remarks/>
        TEMPLATECODE,

        /// <remarks/>
        TEMPLATENAME,

        /// <remarks/>
        FIELDNAME,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public enum TemplateEntityType
    {

        /// <remarks/>
        ASI,

        /// <remarks/>
        ASITABLE,

        /// <remarks/>
        APOTEMPLATE,

        /// <remarks/>
        PEOPLETEMPLATE,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public enum LableDisplay
    {

        /// <remarks/>
        TOP,

        /// <remarks/>
        LEFT,
    }
}
