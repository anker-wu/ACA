/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapPage4ACAModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapPage4ACAModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleCapModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapPage4ACAModel : LanguageModel
    {

        private string accessByACAField;

        private string createdByField;

        private string createdByACAField;

        private string createdByDisplayField;

        private System.Nullable<System.DateTime> expDateField;

        private bool hasPrivilegeToHandleCapField;

        private bool hasPrivilegeToReadCapField;

        private bool isTNExpiredField;

        private bool noPaidFeeFlagField;

        private string renewalStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accessByACA
        {
            get
            {
                return this.accessByACAField;
            }
            set
            {
                this.accessByACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string createdBy
        {
            get
            {
                return this.createdByField;
            }
            set
            {
                this.createdByField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string createdByACA
        {
            get
            {
                return this.createdByACAField;
            }
            set
            {
                this.createdByACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string createdByDisplay
        {
            get
            {
                return this.createdByDisplayField;
            }
            set
            {
                this.createdByDisplayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Nullable<System.DateTime> expDate
        {
            get
            {
                return this.expDateField;
            }
            set
            {
                this.expDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool hasPrivilegeToHandleCap
        {
            get
            {
                return this.hasPrivilegeToHandleCapField;
            }
            set
            {
                this.hasPrivilegeToHandleCapField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool hasPrivilegeToReadCap
        {
            get
            {
                return this.hasPrivilegeToReadCapField;
            }
            set
            {
                this.hasPrivilegeToReadCapField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isTNExpired
        {
            get
            {
                return this.isTNExpiredField;
            }
            set
            {
                this.isTNExpiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool noPaidFeeFlag
        {
            get
            {
                return this.noPaidFeeFlagField;
            }
            set
            {
                this.noPaidFeeFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string renewalStatus
        {
            get
            {
                return this.renewalStatusField;
            }
            set
            {
                this.renewalStatusField = value;
            }
        }
    }

}
