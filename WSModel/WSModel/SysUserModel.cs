#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SysUserModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SysUserModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    public partial class SysUserModel : PersonModel
    {
        private string agencyCodeField;

        private string allowUserChangePasswordField;

        private string bureauCodeField;

        private string cashierIDField;

        private System.Nullable<double> dailyInspUnitsField;

        private string deptOfUserField;

        private System.Nullable<bool> disciplineField;

        private string dispDeptOfUserField;

        private bool displayInitialField;

        private string distinguishedNameField;

        private System.Nullable<bool> districtField;

        private string divisionCodeField;

        private string emailField;

        private string gaUserIDField;

        private string groupCodeField;

        private string initialField;

        private string integratedFlagField;

        private string isInspectorField;

        private string officeCodeField;

        private string phoneNumberField;

        private string resFullNameField;

        private string resInitialField;

        private string sectionCodeField;

        private string serviceProviderCodeField;

        private string userIDField;

        private string userStatusField;

        private System.Nullable<double> workloadField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agencyCode
        {
            get
            {
                return this.agencyCodeField;
            }
            set
            {
                this.agencyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string allowUserChangePassword
        {
            get
            {
                return this.allowUserChangePasswordField;
            }
            set
            {
                this.allowUserChangePasswordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bureauCode
        {
            get
            {
                return this.bureauCodeField;
            }
            set
            {
                this.bureauCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cashierID
        {
            get
            {
                return this.cashierIDField;
            }
            set
            {
                this.cashierIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> dailyInspUnits
        {
            get
            {
                return this.dailyInspUnitsField;
            }
            set
            {
                this.dailyInspUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string deptOfUser
        {
            get
            {
                return this.deptOfUserField;
            }
            set
            {
                this.deptOfUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<bool> discipline
        {
            get
            {
                return this.disciplineField;
            }
            set
            {
                this.disciplineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispDeptOfUser
        {
            get
            {
                return this.dispDeptOfUserField;
            }
            set
            {
                this.dispDeptOfUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool displayInitial
        {
            get
            {
                return this.displayInitialField;
            }
            set
            {
                this.displayInitialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string distinguishedName
        {
            get
            {
                return this.distinguishedNameField;
            }
            set
            {
                this.distinguishedNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<bool> district
        {
            get
            {
                return this.districtField;
            }
            set
            {
                this.districtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string divisionCode
        {
            get
            {
                return this.divisionCodeField;
            }
            set
            {
                this.divisionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaUserID
        {
            get
            {
                return this.gaUserIDField;
            }
            set
            {
                this.gaUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groupCode
        {
            get
            {
                return this.groupCodeField;
            }
            set
            {
                this.groupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string initial
        {
            get
            {
                return this.initialField;
            }
            set
            {
                this.initialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string integratedFlag
        {
            get
            {
                return this.integratedFlagField;
            }
            set
            {
                this.integratedFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isInspector
        {
            get
            {
                return this.isInspectorField;
            }
            set
            {
                this.isInspectorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string officeCode
        {
            get
            {
                return this.officeCodeField;
            }
            set
            {
                this.officeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phoneNumber
        {
            get
            {
                return this.phoneNumberField;
            }
            set
            {
                this.phoneNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resFullName
        {
            get
            {
                return this.resFullNameField;
            }
            set
            {
                this.resFullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resInitial
        {
            get
            {
                return this.resInitialField;
            }
            set
            {
                this.resInitialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sectionCode
        {
            get
            {
                return this.sectionCodeField;
            }
            set
            {
                this.sectionCodeField = value;
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
        public string userID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string userStatus
        {
            get
            {
                return this.userStatusField;
            }
            set
            {
                this.userStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> workload
        {
            get
            {
                return this.workloadField;
            }
            set
            {
                this.workloadField = value;
            }
        }
    }
}
