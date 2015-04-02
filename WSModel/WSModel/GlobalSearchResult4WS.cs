#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchResult4WS.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: GlobalSearchResult4WS.cs 130988 2009-8-21  10:23:01Z ACHIEVO\jackie.yu $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GlobalSearchResult4WS
    {

        private CapView[] capViewsField;

        private RefAddressView[] refAddressViewsField;

        private RefLPView[] refLPViewsField;

        private RefParcelView[] refParcelViewsField;

        private int startNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("capView", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public CapView[] capViews
        {
            get
            {
                return this.capViewsField;
            }
            set
            {
                this.capViewsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refAddressView", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RefAddressView[] refAddressViews
        {
            get
            {
                return this.refAddressViewsField;
            }
            set
            {
                this.refAddressViewsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refLPView", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RefLPView[] refLPViews
        {
            get
            {
                return this.refLPViewsField;
            }
            set
            {
                this.refLPViewsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refParcelView", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RefParcelView[] refParcelViews
        {
            get
            {
                return this.refParcelViewsField;
            }
            set
            {
                this.refParcelViewsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int startNumber
        {
            get
            {
                return this.startNumberField;
            }
            set
            {
                this.startNumberField = value;
            }
        }
    }
}
