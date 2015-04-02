/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RecordTypeLicTypePermissionModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RecordTypeLicTypePermissionModel.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class RecordTypeLicTypePermissionModel
    {

        private string aliasField;

        private SimpleAuditModel auditModelField;

        private bool bizLicExpEnabled4AAField;

        private bool bizLicExpEnabled4ACAField;

        private string categoryField;

        private string controllerTypeField;

        private string entityKey1Field;

        private string entityKey2Field;

        private string entityKey3Field;

        private string entityPermissionField;

        private System.Nullable<long> entityPermissionSeqField;

        private string entityTypeField;

        private string groupField;

        private bool insExpEnabled4AAField;

        private bool insExpEnabled4ACAField;

        private CapTypePermissionModel interModelField;

        private bool licExpEnabled4AAField;

        private bool licExpEnabled4ACAField;

        private string licTypeField;

        private string moduleNameField;

        private string serviceProviderCodeField;

        private string subTypeField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string alias
        {
            get
            {
                return this.aliasField;
            }
            set
            {
                this.aliasField = value;
            }
        }

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
        public bool bizLicExpEnabled4AA
        {
            get
            {
                return this.bizLicExpEnabled4AAField;
            }
            set
            {
                this.bizLicExpEnabled4AAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool bizLicExpEnabled4ACA
        {
            get
            {
                return this.bizLicExpEnabled4ACAField;
            }
            set
            {
                this.bizLicExpEnabled4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string controllerType
        {
            get
            {
                return this.controllerTypeField;
            }
            set
            {
                this.controllerTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityKey1
        {
            get
            {
                return this.entityKey1Field;
            }
            set
            {
                this.entityKey1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityKey2
        {
            get
            {
                return this.entityKey2Field;
            }
            set
            {
                this.entityKey2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityKey3
        {
            get
            {
                return this.entityKey3Field;
            }
            set
            {
                this.entityKey3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityPermission
        {
            get
            {
                return this.entityPermissionField;
            }
            set
            {
                this.entityPermissionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> entityPermissionSeq
        {
            get
            {
                return this.entityPermissionSeqField;
            }
            set
            {
                this.entityPermissionSeqField = value;
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
        public string group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool insExpEnabled4AA
        {
            get
            {
                return this.insExpEnabled4AAField;
            }
            set
            {
                this.insExpEnabled4AAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool insExpEnabled4ACA
        {
            get
            {
                return this.insExpEnabled4ACAField;
            }
            set
            {
                this.insExpEnabled4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapTypePermissionModel interModel
        {
            get
            {
                return this.interModelField;
            }
            set
            {
                this.interModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool licExpEnabled4AA
        {
            get
            {
                return this.licExpEnabled4AAField;
            }
            set
            {
                this.licExpEnabled4AAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool licExpEnabled4ACA
        {
            get
            {
                return this.licExpEnabled4ACAField;
            }
            set
            {
                this.licExpEnabled4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string licType
        {
            get
            {
                return this.licTypeField;
            }
            set
            {
                this.licTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string moduleName
        {
            get
            {
                return this.moduleNameField;
            }
            set
            {
                this.moduleNameField = value;
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
        public string subType
        {
            get
            {
                return this.subTypeField;
            }
            set
            {
                this.subTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }
}
