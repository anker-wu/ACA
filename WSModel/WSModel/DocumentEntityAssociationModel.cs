#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DocumentEntityAssociationModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DocumentEntityAssociationModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class DocumentEntityAssociationModel
    {

        private SimpleAuditModel auditModelField;

        private string displayImageFlagField;

        private long documentIDField;

        private bool documentIDFieldSpecified;

        private string entityIDField;

        private string entityID1Field;

        private System.Nullable<int> entityID2Field;

        private bool entityID2FieldSpecified;

        private System.Nullable<int> entityID3Field;

        private bool entityID3FieldSpecified;

        private string entityTypeField;

        private string iD1Field;

        private string iD2Field;

        private string iD3Field;

        private string operationField;

        private long resIDField;

        private bool resIDFieldSpecified;

        private string serviceProviderCodeField;

        private string statusField;

        private string taskNameField;

        private string taskReviewCommentsField;

        private string taskReviewPagesField;

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
        public string displayImageFlag
        {
            get
            {
                return this.displayImageFlagField;
            }
            set
            {
                this.displayImageFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long documentID
        {
            get
            {
                return this.documentIDField;
            }
            set
            {
                this.documentIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool documentIDSpecified
        {
            get
            {
                return this.documentIDFieldSpecified;
            }
            set
            {
                this.documentIDFieldSpecified = value;
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
        public string entityID1
        {
            get
            {
                return this.entityID1Field;
            }
            set
            {
                this.entityID1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> entityID2
        {
            get
            {
                return this.entityID2Field;
            }
            set
            {
                this.entityID2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entityID2Specified
        {
            get
            {
                return this.entityID2FieldSpecified;
            }
            set
            {
                this.entityID2FieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> entityID3
        {
            get
            {
                return this.entityID3Field;
            }
            set
            {
                this.entityID3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entityID3Specified
        {
            get
            {
                return this.entityID3FieldSpecified;
            }
            set
            {
                this.entityID3FieldSpecified = value;
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
        public string ID1
        {
            get
            {
                return this.iD1Field;
            }
            set
            {
                this.iD1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID2
        {
            get
            {
                return this.iD2Field;
            }
            set
            {
                this.iD2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID3
        {
            get
            {
                return this.iD3Field;
            }
            set
            {
                this.iD3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string operation
        {
            get
            {
                return this.operationField;
            }
            set
            {
                this.operationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long resID
        {
            get
            {
                return this.resIDField;
            }
            set
            {
                this.resIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool resIDSpecified
        {
            get
            {
                return this.resIDFieldSpecified;
            }
            set
            {
                this.resIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string taskName
        {
            get
            {
                return this.taskNameField;
            }
            set
            {
                this.taskNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string taskReviewComments
        {
            get
            {
                return this.taskReviewCommentsField;
            }
            set
            {
                this.taskReviewCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string taskReviewPages
        {
            get
            {
                return this.taskReviewPagesField;
            }
            set
            {
                this.taskReviewPagesField = value;
            }
        }
    }
}
