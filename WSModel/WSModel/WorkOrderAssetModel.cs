#region Header

/**
 *  Accela Citizen Access
 *  File: WorkOrderAssetModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: WorkOrderAssetModel.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class WorkOrderAssetModel : LanguageModel
    {
        private string assetCompleteField;

        private System.DateTime assetCompletionDateField;

        private bool assetCompletionDateFieldSpecified;

        private int assetOrderField;

        private bool assetOrderFieldSpecified;

        private AssetMasterPK assetPKField;

        private string assetShortNotesField;

        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private CapIDModel capIDField;

        private double endWorkLocationField;

        private bool endWorkLocationFieldSpecified;

        private double startWorkLocationField;

        private bool startWorkLocationFieldSpecified;

        private string workDirectionField;

        private double workLengthField;

        private bool workLengthFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetComplete
        {
            get
            {
                return this.assetCompleteField;
            }
            set
            {
                this.assetCompleteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime assetCompletionDate
        {
            get
            {
                return this.assetCompletionDateField;
            }
            set
            {
                this.assetCompletionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assetCompletionDateSpecified
        {
            get
            {
                return this.assetCompletionDateFieldSpecified;
            }
            set
            {
                this.assetCompletionDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int assetOrder
        {
            get
            {
                return this.assetOrderField;
            }
            set
            {
                this.assetOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assetOrderSpecified
        {
            get
            {
                return this.assetOrderFieldSpecified;
            }
            set
            {
                this.assetOrderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AssetMasterPK assetPK
        {
            get
            {
                return this.assetPKField;
            }
            set
            {
                this.assetPKField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetShortNotes
        {
            get
            {
                return this.assetShortNotesField;
            }
            set
            {
                this.assetShortNotesField = value;
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
        public double endWorkLocation
        {
            get
            {
                return this.endWorkLocationField;
            }
            set
            {
                this.endWorkLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endWorkLocationSpecified
        {
            get
            {
                return this.endWorkLocationFieldSpecified;
            }
            set
            {
                this.endWorkLocationFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double startWorkLocation
        {
            get
            {
                return this.startWorkLocationField;
            }
            set
            {
                this.startWorkLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startWorkLocationSpecified
        {
            get
            {
                return this.startWorkLocationFieldSpecified;
            }
            set
            {
                this.startWorkLocationFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workDirection
        {
            get
            {
                return this.workDirectionField;
            }
            set
            {
                this.workDirectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double workLength
        {
            get
            {
                return this.workLengthField;
            }
            set
            {
                this.workLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool workLengthSpecified
        {
            get
            {
                return this.workLengthFieldSpecified;
            }
            set
            {
                this.workLengthFieldSpecified = value;
            }
        }
    }
}
