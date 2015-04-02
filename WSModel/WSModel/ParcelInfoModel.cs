/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ParcelInfoModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ParcelInfoModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class ParcelInfoModel
    {

        private RefAddressModel[] addressListsField;

        private string addressStringField;

        private string ownerFullNameField;

        private OwnerModel[] ownerListsField;

        private OwnerModel ownerModelField;

        private XParOwnerModel parOwnerModelField;

        private ParcelModel parcelModelField;

        private RefAddressModel rAddressModelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("addressList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RefAddressModel[] addressLists
        {
            get
            {
                return this.addressListsField;
            }
            set
            {
                this.addressListsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressString
        {
            get
            {
                return this.addressStringField;
            }
            set
            {
                this.addressStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ownerFullName
        {
            get
            {
                return this.ownerFullNameField;
            }
            set
            {
                this.ownerFullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ownerList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public OwnerModel[] ownerLists
        {
            get
            {
                return this.ownerListsField;
            }
            set
            {
                this.ownerListsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OwnerModel ownerModel
        {
            get
            {
                return this.ownerModelField;
            }
            set
            {
                this.ownerModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XParOwnerModel parOwnerModel
        {
            get
            {
                return this.parOwnerModelField;
            }
            set
            {
                this.parOwnerModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ParcelModel parcelModel
        {
            get
            {
                return this.parcelModelField;
            }
            set
            {
                this.parcelModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefAddressModel RAddressModel
        {
            get
            {
                return this.rAddressModelField;
            }
            set
            {
                this.rAddressModelField = value;
            }
        }

        /// <summary>
        /// Row Index
        /// </summary>
        public int RowIndex
        {
            get;
            set;
        }
    }
}
