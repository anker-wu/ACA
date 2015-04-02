#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XRefExaminationProviderModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XRefExaminationProviderModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://model.webservice.accela.com")]
    public partial class XRefExaminationProviderModel : RefExaminationProviderPKModel {
        
        private SimpleAuditModel auditModelField;
        
        private string commentsField;
        
        private string externalExamIdField;
        
        private string externalExamNameField;
        
        private string gradingStyleField;
        
        private string isAllowDeleteField;
        
        private string isSchedulingInACAField;
        
        private string passingPercentageField;
        
        private System.Nullable<double> passingScoreField;
        
        private string passingScore2StringField;
        
        private ProviderModel providerModelField;
        
        private System.Nullable<double> reScheduleDeadlineField;
        
        private RefExaminationModel refExaminationModelField;
        
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
        public string comments {
            get {
                return this.commentsField;
            }
            set {
                this.commentsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string externalExamId {
            get {
                return this.externalExamIdField;
            }
            set {
                this.externalExamIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string externalExamName {
            get {
                return this.externalExamNameField;
            }
            set {
                this.externalExamNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gradingStyle {
            get {
                return this.gradingStyleField;
            }
            set {
                this.gradingStyleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isAllowDelete {
            get {
                return this.isAllowDeleteField;
            }
            set {
                this.isAllowDeleteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isSchedulingInACA {
            get {
                return this.isSchedulingInACAField;
            }
            set {
                this.isSchedulingInACAField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passingPercentage {
            get {
                return this.passingPercentageField;
            }
            set {
                this.passingPercentageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public System.Nullable<double> passingScore {
            get {
                return this.passingScoreField;
            }
            set {
                this.passingScoreField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passingScore2String {
            get {
                return this.passingScore2StringField;
            }
            set {
                this.passingScore2StringField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ProviderModel providerModel {
            get {
                return this.providerModelField;
            }
            set {
                this.providerModelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public System.Nullable<double> reScheduleDeadline {
            get {
                return this.reScheduleDeadlineField;
            }
            set {
                this.reScheduleDeadlineField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefExaminationModel refExaminationModel {
            get {
                return this.refExaminationModelField;
            }
            set {
                this.refExaminationModelField = value;
            }
        }    
    }
}