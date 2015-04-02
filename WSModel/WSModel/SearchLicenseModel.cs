/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SearchLicenseModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SearchLicenseModel.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class SearchLicenseModel : LicenseModel
    {
        private string[] certicationsField;

        private string[] ethnicitiesField;

        private string[] locationsField;

        private string maximumContractAmountField;

        private string maximumContractAmountOperatorField;

        private string[] nigpCodesField;

        private string nigpKeywordField;

        private string nigpTypeField;

        private string peopleTypeField;

        private string recordStatusField;

        private string[] searchZipsField;

        private System.Nullable<long> userSeqNbrField;
      
        private System.Nullable<DateTime> certificationDateFromField;

        private System.Nullable<DateTime> certificationDateToField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("certications", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] certications
        {
            get
            {
                return this.certicationsField;
            }
            set
            {
                this.certicationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ethnicities", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] ethnicities
        {
            get
            {
                return this.ethnicitiesField;
            }
            set
            {
                this.ethnicitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("locations", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] locations
        {
            get
            {
                return this.locationsField;
            }
            set
            {
                this.locationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string maximumContractAmount
        {
            get
            {
                return this.maximumContractAmountField;
            }
            set
            {
                this.maximumContractAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string maximumContractAmountOperator
        {
            get
            {
                return this.maximumContractAmountOperatorField;
            }
            set
            {
                this.maximumContractAmountOperatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("nigpCodes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] nigpCodes
        {
            get
            {
                return this.nigpCodesField;
            }
            set
            {
                this.nigpCodesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nigpKeyword
        {
            get
            {
                return this.nigpKeywordField;
            }
            set
            {
                this.nigpKeywordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nigpType
        {
            get
            {
                return this.nigpTypeField;
            }
            set
            {
                this.nigpTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string peopleType
        {
            get
            {
                return this.peopleTypeField;
            }
            set
            {
                this.peopleTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recordStatus
        {
            get
            {
                return this.recordStatusField;
            }
            set
            {
                this.recordStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("searchZips", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] searchZips
        {
            get
            {
                return this.searchZipsField;
            }
            set
            {
                this.searchZipsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> userSeqNbr
        {
            get
            {
                return this.userSeqNbrField;
            }
            set
            {
                this.userSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<DateTime> certificationDateFrom
        {
            get
            {
                return this.certificationDateFromField;
            }

            set
            {
                this.certificationDateFromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<DateTime> certificationDateTo
        {
            get
            {
                return this.certificationDateToField;
            }

            set
            {
                this.certificationDateToField = value;
            }
        }
    }
}
