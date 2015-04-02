/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefOwnerModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefOwnerModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class RefOwnerModel : RefOwnerBaseModel
    {

        private System.Nullable<double> l1OwnerNumberField;

        private System.Nullable<double> ownerNumberField;

        private System.Nullable<long> ownerNumberLongField;

        private TemplateAttributeModel[] templatesField;

        private DuplicatedAPOKeyModel[] duplicatedAPOKeysField;

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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> l1OwnerNumber
        {
            get
            {
                return this.l1OwnerNumberField;
            }
            set
            {
                this.l1OwnerNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> ownerNumber
        {
            get
            {
                return this.ownerNumberField;
            }
            set
            {
                this.ownerNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> ownerNumberLong
        {
            get
            {
                return this.ownerNumberLongField;
            }
            set
            {
                this.ownerNumberLongField = value;
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
        public string resState
        {
            get;
            set;
        }
    }
}
