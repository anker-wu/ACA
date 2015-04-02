/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: UserRolePrivilegeModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: UserRolePrivilegeModel4WS.cs 169604 2010-03-30 09:59:38Z ACHIEVO\daly.zeng $.
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
    public partial class UserRolePrivilegeModel4WS
    {

        private bool allAcaUserAllowedField;

        private bool capCreatorAllowedField;

        private bool contactAllowedField;

        private bool licensendProfessionalAllowedField;

        private bool ownerAllowedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool allAcaUserAllowed
        {
            get
            {
                return this.allAcaUserAllowedField;
            }
            set
            {
                this.allAcaUserAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool capCreatorAllowed
        {
            get
            {
                return this.capCreatorAllowedField;
            }
            set
            {
                this.capCreatorAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool contactAllowed
        {
            get
            {
                return this.contactAllowedField;
            }
            set
            {
                this.contactAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool licensendProfessionalAllowed
        {
            get
            {
                return this.licensendProfessionalAllowedField;
            }
            set
            {
                this.licensendProfessionalAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool ownerAllowed
        {
            get
            {
                return this.ownerAllowedField;
            }
            set
            {
                this.ownerAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool registeredUserAllowed
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("licenseTypeRuleArray", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string[] licenseTypeRuleArray 
        {
            get;
            set;
        }
    }
}
