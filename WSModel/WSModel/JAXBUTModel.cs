/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: JAXBUTModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: JAXBUTModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    public partial class JAXBUTModel
    {

        private System.Nullable<bool> bigBooleanField;

        private System.Nullable<double> bigDoubleField;

        private System.Nullable<float> bigFloatField;

        private System.Nullable<int> bigIntegerField;

        private System.Nullable<long> bigLongField;

        private bool smallBooleanField;

        private ushort smallCharField;

        private double smallDoubleField;

        private float smallFloadField;

        private int smallIntField;

        private long smallLongField;

        private System.Nullable<System.DateTime> testDateField;

        private string[] testListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<bool> bigBoolean
        {
            get
            {
                return this.bigBooleanField;
            }
            set
            {
                this.bigBooleanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> bigDouble
        {
            get
            {
                return this.bigDoubleField;
            }
            set
            {
                this.bigDoubleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<float> bigFloat
        {
            get
            {
                return this.bigFloatField;
            }
            set
            {
                this.bigFloatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> bigInteger
        {
            get
            {
                return this.bigIntegerField;
            }
            set
            {
                this.bigIntegerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> bigLong
        {
            get
            {
                return this.bigLongField;
            }
            set
            {
                this.bigLongField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool smallBoolean
        {
            get
            {
                return this.smallBooleanField;
            }
            set
            {
                this.smallBooleanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ushort smallChar
        {
            get
            {
                return this.smallCharField;
            }
            set
            {
                this.smallCharField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double smallDouble
        {
            get
            {
                return this.smallDoubleField;
            }
            set
            {
                this.smallDoubleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public float smallFload
        {
            get
            {
                return this.smallFloadField;
            }
            set
            {
                this.smallFloadField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int smallInt
        {
            get
            {
                return this.smallIntField;
            }
            set
            {
                this.smallIntField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long smallLong
        {
            get
            {
                return this.smallLongField;
            }
            set
            {
                this.smallLongField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> testDate
        {
            get
            {
                return this.testDateField;
            }
            set
            {
                this.testDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("testList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] testList
        {
            get
            {
                return this.testListField;
            }
            set
            {
                this.testListField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string noSetField
        {
            get;
            set;
        }
    }
}
