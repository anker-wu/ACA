/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: NoticeConditionModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: NoticeConditionModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class NoticeConditionModel : ConditionModel
    {

        private long actionNumberField;

        private long addressNumberField;

        private long assetNumberField;

        private CapIDModel capIDField;

        private string conditionOfApprovalField;

        private string conditionSourceField;

        private long contactSeqNumberField;

        private System.Nullable<long> displayOrderField;

        private string entityIDField;

        private string objectConditionNameField;

        private string objectConditionValueField;

        private long ownerNumberField;

        private string parcelNumberField;

        private long referenceConditionNumberField;

        private string resConditionGroupField;

        private string resConditionStatusField;

        private string resConditionTypeField;

        private long structureNumberField;

        private string uIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long actionNumber
        {
            get
            {
                return this.actionNumberField;
            }
            set
            {
                this.actionNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long addressNumber
        {
            get
            {
                return this.addressNumberField;
            }
            set
            {
                this.addressNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long assetNumber
        {
            get
            {
                return this.assetNumberField;
            }
            set
            {
                this.assetNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capID
        {
            get
            {
                return this.capIDField;
            }
            set
            {
                this.capIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionOfApproval
        {
            get
            {
                return this.conditionOfApprovalField;
            }
            set
            {
                this.conditionOfApprovalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionSource
        {
            get
            {
                return this.conditionSourceField;
            }
            set
            {
                this.conditionSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long contactSeqNumber
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> displayOrder
        {
            get
            {
                return this.displayOrderField;
            }
            set
            {
                this.displayOrderField = value;
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
        public string objectConditionName
        {
            get
            {
                return this.objectConditionNameField;
            }
            set
            {
                this.objectConditionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string objectConditionValue
        {
            get
            {
                return this.objectConditionValueField;
            }
            set
            {
                this.objectConditionValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long ownerNumber
        {
            get
            {
                return this.ownerNumberField;
            }
            set
            {
                this.ownerNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parcelNumber
        {
            get
            {
                return this.parcelNumberField;
            }
            set
            {
                this.parcelNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long referenceConditionNumber
        {
            get
            {
                return this.referenceConditionNumberField;
            }
            set
            {
                this.referenceConditionNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resConditionGroup
        {
            get
            {
                return this.resConditionGroupField;
            }
            set
            {
                this.resConditionGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resConditionStatus
        {
            get
            {
                return this.resConditionStatusField;
            }
            set
            {
                this.resConditionStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resConditionType
        {
            get
            {
                return this.resConditionTypeField;
            }
            set
            {
                this.resConditionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long structureNumber
        {
            get
            {
                return this.structureNumberField;
            }
            set
            {
                this.structureNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UID
        {
            get
            {
                return this.uIDField;
            }
            set
            {
                this.uIDField = value;
            }
        }
    }
}
