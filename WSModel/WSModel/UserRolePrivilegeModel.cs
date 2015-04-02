#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: UserRolePrivilegeModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: UserRolePrivilegeModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class UserRolePrivilegeModel
    {

        private bool allAcaUserAllowedField;

        private bool capCreatorAllowedField;

        private bool contactAllowedField;

        private string[] licenseTypeRuleArrayField;

        private bool licensendProfessionalAllowedField;

        private bool onCapTypeLevelField;

        private bool ownerAllowedField;

        private bool registeredUserAllowedField;

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
        [System.Xml.Serialization.XmlElementAttribute("licenseTypeRuleArray", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] licenseTypeRuleArray
        {
            get
            {
                return this.licenseTypeRuleArrayField;
            }
            set
            {
                this.licenseTypeRuleArrayField = value;
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
        public bool onCapTypeLevel
        {
            get
            {
                return this.onCapTypeLevelField;
            }
            set
            {
                this.onCapTypeLevelField = value;
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
            get
            {
                return this.registeredUserAllowedField;
            }
            set
            {
                this.registeredUserAllowedField = value;
            }
        }
    }
}