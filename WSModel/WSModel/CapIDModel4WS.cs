/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapIDModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapIDModel4WS.cs 184822 2010-11-18 09:35:14Z ACHIEVO\daly.zeng $.
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
    public partial class CapIDModel4WS
    {

        private string customIDField;

        private string id1Field;

        private string id2Field;

        private string id3Field;

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
        public string id1
        {
            get
            {
                return this.id1Field;
            }
            set
            {
                this.id1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string id2
        {
            get
            {
                return this.id2Field;
            }
            set
            {
                this.id2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string id3
        {
            get
            {
                return this.id3Field;
            }
            set
            {
                this.id3Field = value;
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
        /// <param name="obj">CapIDModel4WS object</param>
        /// <returns>Boolean value.</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            if (!(obj is CapIDModel4WS))
            {
                return false;
            }

            CapIDModel4WS capID = (CapIDModel4WS)obj;
            if ((this.id1 == null && capID.id1 != null) || (this.id1 != null && capID.id1 == null) || !this.id1.Equals(capID.id1) ||
                (this.id2 == null && capID.id2 != null) || (this.id2 != null && capID.id2 == null) || !this.id2.Equals(capID.id2) ||
                (this.id3 == null && capID.id3 != null) || (this.id3 != null && capID.id3 == null) || !this.id3.Equals(capID.id3) ||
                (this.serviceProviderCode == null && capID.serviceProviderCode != null) ||
                (this.serviceProviderCode != null && capID.serviceProviderCode == null) ||
                !this.serviceProviderCode.Equals(capID.serviceProviderCode))
            {
                return false;
            }

            return true;	
	    }

        /// <summary>
        /// Get Cap ID Model Key String
        /// </summary>
        /// <returns>key string</returns>
        public string toKey()
        {
            if (this.id1 == null)
            {
                return string.Empty;
            }
            else
            {
                return this.serviceProviderCode + "-" + this.id1 + "-" + this.id2 + "-" + this.id3;
            }
        }
    }
   
}
