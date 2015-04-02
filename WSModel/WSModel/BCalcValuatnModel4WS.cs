/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: BCalcValuatnModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: BCalcValuatnModel4WS.cs 
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class BCalcValuatnModel4WS
    {

        private string auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private long calcValueSeqNbrField;

        private CapIDModel4WS capIDField;

        /// <summary>
        /// Unit Type
        /// </summary>
        private string conTypField;        

        private string excludeRegionalModifierField;

        private string feeIndicatorField;

        /// <summary>
        /// Job Value
        /// </summary>
        private double totalValueField;

        private double unitCostField;

        /// <summary>
        /// Unit Type (i.e. SQFT)   
        /// </summary>
        private string unitTypField;

        /// <summary>
        /// Unit Amount
        /// </summary>
        private double unitValueField;

        /// <summary>
        /// Occupancy Type
        /// </summary>
        private string useTypField;        

        private string versionField;        

        private int rowindexField; 

        /// <summary>
        /// Fields For Internationa Lization
        /// </summary>
        private string disUnitTypeField;
        private string disVersionField;
        private string disUseTypeField;
        private string disConTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditDate
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
        public long calcValueSeqNbr
        {
            get
            {
                return this.calcValueSeqNbrField;
            }
            set
            {
                this.calcValueSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel4WS capID
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
        public string conTyp
        {
            get
            {
                return this.conTypField;
            }
            set
            {
                this.conTypField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string disConType
        {
            get
            {
                return this.disConTypeField;
            }
            set
            {
                this.disConTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string excludeRegionalModifier
        {
            get
            {
                return this.excludeRegionalModifierField;
            }
            set
            {
                this.excludeRegionalModifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string feeIndicator
        {
            get
            {
                return this.feeIndicatorField;
            }
            set
            {
                this.feeIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalValue
        {
            get
            {
                return this.totalValueField;
            }
            set
            {
                this.totalValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double unitCost
        {
            get
            {
                return this.unitCostField;
            }
            set
            {
                this.unitCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitTyp
        {
            get
            {
                return this.unitTypField;
            }
            set
            {
                this.unitTypField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string disUnitType
        {
            get
            {
                return this.disUnitTypeField;
            }
            set
            {
                this.disUnitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double unitValue
        {
            get
            {
                return this.unitValueField;
            }
            set
            {
                this.unitValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string useTyp
        {
            get
            {
                return this.useTypField;
            }
            set
            {
                this.useTypField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string disUseType
        {
            get
            {
                return this.disUseTypeField;
            }
            set
            {
                this.disUseTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string disVersion
        {
            get
            {
                return this.disVersionField;
            }
            set
            {
                this.disVersionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int RowIndex
        {
            get
            {
                return this.rowindexField;
            }
            set
            {
                this.rowindexField = value;
            }
        }
    }
}