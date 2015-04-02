/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapParcelModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapParcelModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    public partial class CapParcelModel
    {

        private CapIDModel capIDModelField;

        private string l1ParcelNoField;

        private ParcelModel parcelModelField;

        private string parcelNoField;

        private string parcelNumberField;

        private ParcelModel specialParcelModelField;

        private string uIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capIDModel
        {
            get
            {
                return this.capIDModelField;
            }
            set
            {
                this.capIDModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string l1ParcelNo
        {
            get
            {
                return this.l1ParcelNoField;
            }
            set
            {
                this.l1ParcelNoField = value;
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
        public string parcelNo
        {
            get
            {
                return this.parcelNoField;
            }
            set
            {
                this.parcelNoField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ParcelModel specialParcelModel
        {
            get
            {
                return this.specialParcelModelField;
            }
            set
            {
                this.specialParcelModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UID
        {
            get
            {
                return this.uIDField;
            }
            set
            {
                this.uIDField = value;
            }
        }
    }
}
