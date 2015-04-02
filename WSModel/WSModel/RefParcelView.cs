#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefParcelView.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: RefParcelView.cs 130988 2009-9-8  9:30:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class RefParcelView
    {

        private string[] fullAddressField;

        private string parcelNumberField;

        private string[] refOwnerNameField;

        private string sourceSeqNbrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("fullAddress", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] fullAddress
        {
            get
            {
                return this.fullAddressField;
            }
            set
            {
                this.fullAddressField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("refOwnerName", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] refOwnerName
        {
            get
            {
                return this.refOwnerNameField;
            }
            set
            {
                this.refOwnerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sourceSeqNbr
        {
            get
            {
                return this.sourceSeqNbrField;
            }
            set
            {
                this.sourceSeqNbrField = value;
            }
        }
    }
}
