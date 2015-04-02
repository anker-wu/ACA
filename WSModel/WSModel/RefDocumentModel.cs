/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefDocumentModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefDocumentModel.cs 136291 2009-06-25 10:46:10Z ACHIEVO\jackie.yu $.
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
    public partial class RefDocumentModel : LanguageModel
    {

        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private string commentField;

        private UserRolePrivilegeModel deleteRolePrivilegeModelField;

        private string docSeqNumberField;

        private string documentCodeField;

        private string documentTypeField;

        private string isRestrictDocType4ACAField;

        private string resDocumentTypeField;

        private string restrictRole4ACAField;

        private string servProvCodeField;

        private UserRolePrivilegeModel uploadRolePrivilegeModelField;

        private UserRolePrivilegeModel viewRolePrivilegeModelField;

        private UserRolePrivilegeModel viewTitleRolePrivilegeModelField;

        private string reviewStatusGroupField;

        private string docStatusGroupField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reviewStatusGroup
        {
            get
            {
                return this.reviewStatusGroupField;
            }
            set
            {
                this.reviewStatusGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docStatusGroup
        {
            get
            {
                return this.docStatusGroupField;
            }
            set
            {
                this.docStatusGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime auditDate
        {
            get
            {
                return this.auditDateField;
            }
            set
            {
                this.auditDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool auditDateSpecified
        {
            get
            {
                return this.auditDateFieldSpecified;
            }
            set
            {
                this.auditDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditID
        {
            get
            {
                return this.auditIDField;
            }
            set
            {
                this.auditIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditStatus
        {
            get
            {
                return this.auditStatusField;
            }
            set
            {
                this.auditStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel deleteRolePrivilegeModel
        {
            get
            {
                return this.deleteRolePrivilegeModelField;
            }
            set
            {
                this.deleteRolePrivilegeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docSeqNumber
        {
            get
            {
                return this.docSeqNumberField;
            }
            set
            {
                this.docSeqNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string documentCode
        {
            get
            {
                return this.documentCodeField;
            }
            set
            {
                this.documentCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string documentType
        {
            get
            {
                return this.documentTypeField;
            }
            set
            {
                this.documentTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isRestrictDocType4ACA
        {
            get
            {
                return this.isRestrictDocType4ACAField;
            }
            set
            {
                this.isRestrictDocType4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resDocumentType
        {
            get
            {
                return this.resDocumentTypeField;
            }
            set
            {
                this.resDocumentTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string restrictRole4ACA
        {
            get
            {
                return this.restrictRole4ACAField;
            }
            set
            {
                this.restrictRole4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servProvCode
        {
            get
            {
                return this.servProvCodeField;
            }
            set
            {
                this.servProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel uploadRolePrivilegeModel
        {
            get
            {
                return this.uploadRolePrivilegeModelField;
            }
            set
            {
                this.uploadRolePrivilegeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel viewRolePrivilegeModel
        {
            get
            {
                return this.viewRolePrivilegeModelField;
            }
            set
            {
                this.viewRolePrivilegeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel viewTitleRolePrivilegeModel
        {
            get
            {
                return this.viewTitleRolePrivilegeModelField;
            }
            set
            {
                this.viewTitleRolePrivilegeModelField = value;
            }
        }
    }
}
