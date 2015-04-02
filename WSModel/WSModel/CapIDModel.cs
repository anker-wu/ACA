/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapIDModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapIDModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    public partial class CapIDModel
    {

        private string customIDField;

        private string iD1Field;

        private string iD2Field;

        private string iD3Field;

        private string serviceProviderCodeField;

        private long trackingIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string customID
        {
            get
            {
                return this.customIDField;
            }
            set
            {
                this.customIDField = value;
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
        public long trackingID
        {
            get
            {
                return this.trackingIDField;
            }
            set
            {
                this.trackingIDField = value;
            }
        }

        /// <summary>
        /// overload equals method
        /// </summary>
        /// <param name="obj">CapIDModel object</param>
        /// <returns>Boolean value.</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            if (!(obj is CapIDModel))
            {
                return false;
            }

            CapIDModel capID = (CapIDModel)obj;
            if ((this.ID1 == null && capID.ID1 != null) || (this.ID1 != null && capID.ID1 == null) || !this.ID1.Equals(capID.ID1) ||
                (this.ID2 == null && capID.ID2 != null) || (this.ID2 != null && capID.ID2 == null) || !this.ID2.Equals(capID.ID2) ||
                (this.ID3 == null && capID.ID3 != null) || (this.ID3 != null && capID.ID3 == null) || !this.ID3.Equals(capID.ID3) ||
                (this.serviceProviderCode == null && capID.serviceProviderCode != null) ||
                (this.serviceProviderCode != null && capID.serviceProviderCode == null) ||
                !this.serviceProviderCode.Equals(capID.serviceProviderCode))
            {
                return false;
            }

            return true;
        }
    }
}
