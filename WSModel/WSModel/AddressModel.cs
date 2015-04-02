/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AddressModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AddressModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class AddressModel : AddressBaseModel
    {

        private System.Nullable<long> addressIdField;

        private string addressLine1Field;

        private string addressLine2Field;

        private string addressTypeField;

        private string displayAddressField;

        private System.Nullable<bool> displayParcelLinkField;

        private System.Nullable<double> distanceField;

        private DuplicatedAPOKeyModel[] duplicatedAPOKeysField;

        private string houseNumberAlphaEndField;

        private string houseNumberAlphaStartField;

        private System.Nullable<int> houseNumberEndField;

        private System.Nullable<int> houseNumberRangeEndField;

        private System.Nullable<int> houseNumberRangeStartField;

        private System.Nullable<int> houseNumberStartField;

        private string levelNumberEndField;

        private string levelNumberStartField;

        private string levelPrefixField;

        private System.Nullable<long> refAddressIdField;

        private string refAddressTypeField;

        private System.Nullable<int> secondaryRoadNumberField;

        private TemplateAttributeModel[] templatesField;

        private string unitRangeEndField;

        private string unitRangeStartField;

        private System.Nullable<double> xCoordinatorField;

        private System.Nullable<double> xCoordinatorEndField;

        private System.Nullable<double> yCoordinatorField;

        private System.Nullable<double> yCoordinatorEndField;

        private System.Nullable<int> houseNumberStartFromField;

        private System.Nullable<int> houseNumberStartToField;

        private System.Nullable<int> houseNumberEndFromField;

        private System.Nullable<int> houseNumberEndToField;

            /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> addressId
        {
            get
            {
                return this.addressIdField;
            }
            set
            {
                this.addressIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressType
        {
            get
            {
                return this.addressTypeField;
            }
            set
            {
                this.addressTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayAddress
        {
            get
            {
                return this.displayAddressField;
            }
            set
            {
                this.displayAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<bool> displayParcelLink
        {
            get
            {
                return this.displayParcelLinkField;
            }
            set
            {
                this.displayParcelLinkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> distance
        {
            get
            {
                return this.distanceField;
            }
            set
            {
                this.distanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("duplicatedAPOKey", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public DuplicatedAPOKeyModel[] duplicatedAPOKeys
        {
            get
            {
                return this.duplicatedAPOKeysField;
            }
            set
            {
                this.duplicatedAPOKeysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string houseNumberAlphaEnd
        {
            get
            {
                return this.houseNumberAlphaEndField;
            }
            set
            {
                this.houseNumberAlphaEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string houseNumberAlphaStart
        {
            get
            {
                return this.houseNumberAlphaStartField;
            }
            set
            {
                this.houseNumberAlphaStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberEnd
        {
            get
            {
                return this.houseNumberEndField;
            }
            set
            {
                this.houseNumberEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberRangeEnd
        {
            get
            {
                return this.houseNumberRangeEndField;
            }
            set
            {
                this.houseNumberRangeEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberRangeStart
        {
            get
            {
                return this.houseNumberRangeStartField;
            }
            set
            {
                this.houseNumberRangeStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberStart
        {
            get
            {
                return this.houseNumberStartField;
            }
            set
            {
                this.houseNumberStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelNumberEnd
        {
            get
            {
                return this.levelNumberEndField;
            }
            set
            {
                this.levelNumberEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelNumberStart
        {
            get
            {
                return this.levelNumberStartField;
            }
            set
            {
                this.levelNumberStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelPrefix
        {
            get
            {
                return this.levelPrefixField;
            }
            set
            {
                this.levelPrefixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> refAddressId
        {
            get
            {
                return this.refAddressIdField;
            }
            set
            {
                this.refAddressIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refAddressType
        {
            get
            {
                return this.refAddressTypeField;
            }
            set
            {
                this.refAddressTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> secondaryRoadNumber
        {
            get
            {
                return this.secondaryRoadNumberField;
            }
            set
            {
                this.secondaryRoadNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("template", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TemplateAttributeModel[] templates
        {
            get
            {
                return this.templatesField;
            }
            set
            {
                this.templatesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitRangeEnd
        {
            get
            {
                return this.unitRangeEndField;
            }
            set
            {
                this.unitRangeEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitRangeStart
        {
            get
            {
                return this.unitRangeStartField;
            }
            set
            {
                this.unitRangeStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> XCoordinator
        {
            get
            {
                return this.xCoordinatorField;
            }
            set
            {
                this.xCoordinatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> XCoordinatorEnd
        {
            get
            {
                return this.xCoordinatorEndField;
            }
            set
            {
                this.xCoordinatorEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> YCoordinator
        {
            get
            {
                return this.yCoordinatorField;
            }
            set
            {
                this.yCoordinatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> YCoordinatorEnd
        {
            get
            {
                return this.yCoordinatorEndField;
            }
            set
            {
                this.yCoordinatorEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberStartFrom
        {
            get
            {
                return this.houseNumberStartFromField;
            }

            set 
            { 
                this.houseNumberStartFromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberStartTo
        {
            get
            {
                return this.houseNumberStartToField;
            }

            set
            {
                this.houseNumberStartToField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberEndFrom
        {
            get
            {
                return this.houseNumberEndFromField;
            }

            set
            {
                this.houseNumberEndFromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberEndTo
        {
            get
            {
                return this.houseNumberEndToField;
            }

            set
            {
                this.houseNumberEndToField = value;
            }
        }
    }
}
