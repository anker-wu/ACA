/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapTypeDetailModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapTypeDetailModel.cs 171719 2010-04-29 10:28:45Z ACHIEVO\hans.shi $.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapTypeDetailModel : CapTypeModel
    {

        private string altMask1Field;

        private string altMask2Field;

        private System.Nullable<long> altSeq1Field;

        private System.Nullable<long> altSeq2Field;

        private string altSeqReset1Field;

        private string altSeqReset2Field;

        private string appSpecificInfoCodeField;

        private string appStatusGroupCodeField;

        private int categoryDispOrderField;

        private string copyAllAssociatedAPOField;

        private string defaultCapTypeStatusField;

        private string docCodeField;

        private string docCode4ACAField;

        private System.Nullable<double> estCostPerUnitField;

        private System.Nullable<double> estProdUnitsField;

        private string expirationCodeField;

        private string gISServiceIDField;

        private string gISTypeIDField;

        private string hrEmailField;

        private string inspectionCodeField;

        private string partialAltIdMaskField;

        private string perUdcode1Field;

        private string prodUnitTypeField;

        private string smartchoiceCodeField;

        private string smartchoiceCode4ACAField;

        private int subTypeDispOrderField;

        private string temporaryAltIdMaskField;

        private int typeDispOrderField;

        private string udcode3Field;

        private string valueRequiredField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string altMask1
        {
            get
            {
                return this.altMask1Field;
            }
            set
            {
                this.altMask1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string altMask2
        {
            get
            {
                return this.altMask2Field;
            }
            set
            {
                this.altMask2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> altSeq1
        {
            get
            {
                return this.altSeq1Field;
            }
            set
            {
                this.altSeq1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> altSeq2
        {
            get
            {
                return this.altSeq2Field;
            }
            set
            {
                this.altSeq2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string altSeqReset1
        {
            get
            {
                return this.altSeqReset1Field;
            }
            set
            {
                this.altSeqReset1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string altSeqReset2
        {
            get
            {
                return this.altSeqReset2Field;
            }
            set
            {
                this.altSeqReset2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appSpecificInfoCode
        {
            get
            {
                return this.appSpecificInfoCodeField;
            }
            set
            {
                this.appSpecificInfoCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appStatusGroupCode
        {
            get
            {
                return this.appStatusGroupCodeField;
            }
            set
            {
                this.appStatusGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> auditDate
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int categoryDispOrder
        {
            get
            {
                return this.categoryDispOrderField;
            }
            set
            {
                this.categoryDispOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string copyAllAssociatedAPO
        {
            get
            {
                return this.copyAllAssociatedAPOField;
            }
            set
            {
                this.copyAllAssociatedAPOField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string defaultCapTypeStatus
        {
            get
            {
                return this.defaultCapTypeStatusField;
            }
            set
            {
                this.defaultCapTypeStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docCode
        {
            get
            {
                return this.docCodeField;
            }
            set
            {
                this.docCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docCode4ACA
        {
            get
            {
                return this.docCode4ACAField;
            }
            set
            {
                this.docCode4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> estCostPerUnit
        {
            get
            {
                return this.estCostPerUnitField;
            }
            set
            {
                this.estCostPerUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> estProdUnits
        {
            get
            {
                return this.estProdUnitsField;
            }
            set
            {
                this.estProdUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string expirationCode
        {
            get
            {
                return this.expirationCodeField;
            }
            set
            {
                this.expirationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string GISServiceID
        {
            get
            {
                return this.gISServiceIDField;
            }
            set
            {
                this.gISServiceIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string GISTypeID
        {
            get
            {
                return this.gISTypeIDField;
            }
            set
            {
                this.gISTypeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hrEmail
        {
            get
            {
                return this.hrEmailField;
            }
            set
            {
                this.hrEmailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectionCode
        {
            get
            {
                return this.inspectionCodeField;
            }
            set
            {
                this.inspectionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string partialAltIdMask
        {
            get
            {
                return this.partialAltIdMaskField;
            }
            set
            {
                this.partialAltIdMaskField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string perUdcode1
        {
            get
            {
                return this.perUdcode1Field;
            }
            set
            {
                this.perUdcode1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string prodUnitType
        {
            get
            {
                return this.prodUnitTypeField;
            }
            set
            {
                this.prodUnitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string smartchoiceCode
        {
            get
            {
                return this.smartchoiceCodeField;
            }
            set
            {
                this.smartchoiceCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string smartchoiceCode4ACA
        {
            get
            {
                return this.smartchoiceCode4ACAField;
            }
            set
            {
                this.smartchoiceCode4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int subTypeDispOrder
        {
            get
            {
                return this.subTypeDispOrderField;
            }
            set
            {
                this.subTypeDispOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string temporaryAltIdMask
        {
            get
            {
                return this.temporaryAltIdMaskField;
            }
            set
            {
                this.temporaryAltIdMaskField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int typeDispOrder
        {
            get
            {
                return this.typeDispOrderField;
            }
            set
            {
                this.typeDispOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udcode3
        {
            get
            {
                return this.udcode3Field;
            }
            set
            {
                this.udcode3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string valueRequired
        {
            get
            {
                return this.valueRequiredField;
            }
            set
            {
                this.valueRequiredField = value;
            }
        }
    }
}
