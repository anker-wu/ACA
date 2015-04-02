/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XPolicyModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XPolicyModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(XpolicyUserRolePrivilegeModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XPolicyModel : LanguageModel
    {

        private string data1Field;

        private string data2Field;

        private string data3Field;

        private string data4Field;

        private string data5Field;

        private string dispData2Field;

        private string dispLevelDataField;

        private EdmsUserModel edmsUserField;

        private string fIDField;

        private string levelField;

        private string levelDataField;

        private string menuLevelField;

        private string policyNameField;

        private long policySeqField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFullNameField;

        private string recStatusField;

        private string resData2Field;

        private string rightGrantedField;

        private string serviceProviderCodeField;

        private string statusField;

        private string templateNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data1
        {
            get
            {
                return this.data1Field;
            }
            set
            {
                this.data1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data2
        {
            get
            {
                return this.data2Field;
            }
            set
            {
                this.data2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data3
        {
            get
            {
                return this.data3Field;
            }
            set
            {
                this.data3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data4
        {
            get
            {
                return this.data4Field;
            }
            set
            {
                this.data4Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data5
        {
            get
            {
                return this.data5Field;
            }
            set
            {
                this.data5Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispData2
        {
            get
            {
                return this.dispData2Field;
            }
            set
            {
                this.dispData2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispLevelData
        {
            get
            {
                return this.dispLevelDataField;
            }
            set
            {
                this.dispLevelDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public EdmsUserModel edmsUser
        {
            get
            {
                return this.edmsUserField;
            }
            set
            {
                this.edmsUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FID
        {
            get
            {
                return this.fIDField;
            }
            set
            {
                this.fIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelData
        {
            get
            {
                return this.levelDataField;
            }
            set
            {
                this.levelDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string menuLevel
        {
            get
            {
                return this.menuLevelField;
            }
            set
            {
                this.menuLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string policyName
        {
            get
            {
                return this.policyNameField;
            }
            set
            {
                this.policyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long policySeq
        {
            get
            {
                return this.policySeqField;
            }
            set
            {
                this.policySeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> recDate
        {
            get
            {
                return this.recDateField;
            }
            set
            {
                this.recDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFullName
        {
            get
            {
                return this.recFullNameField;
            }
            set
            {
                this.recFullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recStatus
        {
            get
            {
                return this.recStatusField;
            }
            set
            {
                this.recStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resData2
        {
            get
            {
                return this.resData2Field;
            }
            set
            {
                this.resData2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rightGranted
        {
            get
            {
                return this.rightGrantedField;
            }
            set
            {
                this.rightGrantedField = value;
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
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string templateName
        {
            get
            {
                return this.templateNameField;
            }
            set
            {
                this.templateNameField = value;
            }
        }
    }

 
}
