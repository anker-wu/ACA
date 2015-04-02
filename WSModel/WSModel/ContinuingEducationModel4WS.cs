/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ContinuingEducationModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ContinuingEducationModel4WS.cs 136284 2009-06-25 10:10:38Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ContinuingEducationModel4WS
    {
        private string approvedFlagField;

        private AuditModel4WS auditModelField;

        private string b1PerId1Field;

        private string b1PerId2Field;

        private string b1PerId3Field;

        private string classNameField;

        private string commentsField;

        private string contEduNameField;

        private long? contactSeqNumberField;

        private ContinuingEducationPKModel4WS continuingEducationPKModelField;

        private string dateOfClassField;

        private string entityIDField;

        private string entityTypeField;

        private string finalScoreField;

        private string gradingStyleField;

        private double hoursCompletedField;

        private string passingScoreField;

        private ProviderDetailModel4WS providerDetailModelField;

        private string providerNameField;

        private string providerNoField;

        private string requiredFlagField;

        private string syncFlagField;

        private TemplateModel templateField;

        private int associatedContEduCountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string approvedFlag
        {
            get
            {
                return this.approvedFlagField;
            }
            set
            {
                this.approvedFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AuditModel4WS auditModel
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
        public string b1PerId1
        {
            get
            {
                return this.b1PerId1Field;
            }
            set
            {
                this.b1PerId1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1PerId2
        {
            get
            {
                return this.b1PerId2Field;
            }
            set
            {
                this.b1PerId2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1PerId3
        {
            get
            {
                return this.b1PerId3Field;
            }
            set
            {
                this.b1PerId3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string className
        {
            get
            {
                return this.classNameField;
            }
            set
            {
                this.classNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string comments
        {
            get
            {
                return this.commentsField;
            }
            set
            {
                this.commentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contEduName
        {
            get
            {
                return this.contEduNameField;
            }
            set
            {
                this.contEduNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long? contactSeqNumber
        {
            get
            {
                return this.contactSeqNumberField;
            }
            set
            {
                this.contactSeqNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ContinuingEducationPKModel4WS continuingEducationPKModel
        {
            get
            {
                return this.continuingEducationPKModelField;
            }
            set
            {
                this.continuingEducationPKModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dateOfClass
        {
            get
            {
                return this.dateOfClassField;
            }
            set
            {
                this.dateOfClassField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityID
        {
            get
            {
                return this.entityIDField;
            }
            set
            {
                this.entityIDField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string finalScore
        {
            get
            {
                return this.finalScoreField;
            }
            set
            {
                this.finalScoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gradingStyle
        {
            get
            {
                return this.gradingStyleField;
            }
            set
            {
                this.gradingStyleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double hoursCompleted
        {
            get
            {
                return this.hoursCompletedField;
            }
            set
            {
                this.hoursCompletedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passingScore
        {
            get
            {
                return this.passingScoreField;
            }
            set
            {
                this.passingScoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ProviderDetailModel4WS providerDetailModel
        {
            get
            {
                return this.providerDetailModelField;
            }
            set
            {
                this.providerDetailModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string providerName
        {
            get
            {
                return this.providerNameField;
            }
            set
            {
                this.providerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string providerNo
        {
            get
            {
                return this.providerNoField;
            }
            set
            {
                this.providerNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requiredFlag
        {
            get
            {
                return this.requiredFlagField;
            }
            set
            {
                this.requiredFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string syncFlag
        {
            get
            {
                return this.syncFlagField;
            }
            set
            {
                this.syncFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateModel template
        {
            get
            {
                return this.templateField;
            }
            set
            {
                this.templateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int associatedContEduCount
        {
            get
            {
                return this.associatedContEduCountField;
            }
            set
            {
                this.associatedContEduCountField = value;
            }
        }
    }
}
