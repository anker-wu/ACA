﻿#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: B1ExpirationModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: B1ExpirationModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class B1ExpirationModel
    {
        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private CapIDModel capIDModelField;

        private string expCodeField;

        private System.DateTime expDateField;

        private bool expDateFieldSpecified;

        private long expIntervalField;

        private string expStatusField;

        private string expUnitField;

        private long graceIntervalField;

        private string graceUnitField;

        private string payPeriodGroupField;

        private string penaltyCodeField;

        private string penaltyFunctionField;

        private long penaltyIntervalField;

        private long penaltyNumField;

        private long penaltyPeriodField;

        private string penaltyUnitField;

        private string renewalCodeField;

        private string renewalFunctionField;

        private string udf1Field;

        private string udf2Field;

        private string udf3Field;

        private string udf4Field;

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
        public CapIDModel capIDModel
        {
            get
            {
                return this.capIDModelField;
            }
            set
            {
                this.capIDModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string expCode
        {
            get
            {
                return this.expCodeField;
            }
            set
            {
                this.expCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime expDate
        {
            get
            {
                return this.expDateField;
            }
            set
            {
                this.expDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expDateSpecified
        {
            get
            {
                return this.expDateFieldSpecified;
            }
            set
            {
                this.expDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long expInterval
        {
            get
            {
                return this.expIntervalField;
            }
            set
            {
                this.expIntervalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string expStatus
        {
            get
            {
                return this.expStatusField;
            }
            set
            {
                this.expStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string expUnit
        {
            get
            {
                return this.expUnitField;
            }
            set
            {
                this.expUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long graceInterval
        {
            get
            {
                return this.graceIntervalField;
            }
            set
            {
                this.graceIntervalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string graceUnit
        {
            get
            {
                return this.graceUnitField;
            }
            set
            {
                this.graceUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payPeriodGroup
        {
            get
            {
                return this.payPeriodGroupField;
            }
            set
            {
                this.payPeriodGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string penaltyCode
        {
            get
            {
                return this.penaltyCodeField;
            }
            set
            {
                this.penaltyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string penaltyFunction
        {
            get
            {
                return this.penaltyFunctionField;
            }
            set
            {
                this.penaltyFunctionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long penaltyInterval
        {
            get
            {
                return this.penaltyIntervalField;
            }
            set
            {
                this.penaltyIntervalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long penaltyNum
        {
            get
            {
                return this.penaltyNumField;
            }
            set
            {
                this.penaltyNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long penaltyPeriod
        {
            get
            {
                return this.penaltyPeriodField;
            }
            set
            {
                this.penaltyPeriodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string penaltyUnit
        {
            get
            {
                return this.penaltyUnitField;
            }
            set
            {
                this.penaltyUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string renewalCode
        {
            get
            {
                return this.renewalCodeField;
            }
            set
            {
                this.renewalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string renewalFunction
        {
            get
            {
                return this.renewalFunctionField;
            }
            set
            {
                this.renewalFunctionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf1
        {
            get
            {
                return this.udf1Field;
            }
            set
            {
                this.udf1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf2
        {
            get
            {
                return this.udf2Field;
            }
            set
            {
                this.udf2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf3
        {
            get
            {
                return this.udf3Field;
            }
            set
            {
                this.udf3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf4
        {
            get
            {
                return this.udf4Field;
            }
            set
            {
                this.udf4Field = value;
            }
        }
    }
}
