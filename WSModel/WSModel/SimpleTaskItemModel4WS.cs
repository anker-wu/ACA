/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ACAModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SimpleTaskItemModel4WS.cs 209458 2011-12-12 06:03:07Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.WSProxy
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class SimpleTaskItemModel4WS
    {

        private string pRelationIDField;

        private string pTaskNameField;

        private string g6StatDdField;

        private string ga_fnameField;

        private string ga_lnameField;

        private string ga_mnameField;

        private TaskItemModel4WS[] historyTaskItemsField;

        private string isMasterProcessField;

        private string isRestrictView4ACAField;

        private string levelIDField;

        private string proDesField;

        private string r1ProcessCodeField;

        private string resPTaskNameField;

        private string resProDesField;

        private string resSdAppDesField;

        private string restrictRoleField;

        private string sdAppDesField;

        private string sdChk1Field;

        private string sdChk2Field;

        private StandardCommentModel4WS[] standardCommentsField;

        private string stpNumField;

        private TaskSpecificInfoModel4WS[] taskSpecItemsField;

        private UserRolePrivilegeModel userRolePrivilegeModelField;

        private string refActStatusFlagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PRelationID
        {
            get
            {
                return this.pRelationIDField;
            }
            set
            {
                this.pRelationIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PTaskName
        {
            get
            {
                return this.pTaskNameField;
            }
            set
            {
                this.pTaskNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g6StatDd
        {
            get
            {
                return this.g6StatDdField;
            }
            set
            {
                this.g6StatDdField = value;
            }
        }

        /// <remarks/>
       [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ga_fname
        {
            get
            {
                return this.ga_fnameField;
            }
            set
            {
                this.ga_fnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ga_lname
        {
            get
            {
                return this.ga_lnameField;
            }
            set
            {
                this.ga_lnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ga_mname
        {
            get
            {
                return this.ga_mnameField;
            }
            set
            {
                this.ga_mnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("historyTaskItems", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TaskItemModel4WS[] historyTaskItems
        {
            get
            {
                return this.historyTaskItemsField;
            }
            set
            {
                this.historyTaskItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isMasterProcess
        {
            get
            {
                return this.isMasterProcessField;
            }
            set
            {
                this.isMasterProcessField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isRestrictView4ACA
        {
            get
            {
                return this.isRestrictView4ACAField;
            }
            set
            {
                this.isRestrictView4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelID
        {
            get
            {
                return this.levelIDField;
            }
            set
            {
                this.levelIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string proDes
        {
            get
            {
                return this.proDesField;
            }
            set
            {
                this.proDesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1ProcessCode
        {
            get
            {
                return this.r1ProcessCodeField;
            }
            set
            {
                this.r1ProcessCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resPTaskName
        {
            get
            {
                return this.resPTaskNameField;
            }
            set
            {
                this.resPTaskNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resProDes
        {
            get
            {
                return this.resProDesField;
            }
            set
            {
                this.resProDesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resSdAppDes
        {
            get
            {
                return this.resSdAppDesField;
            }
            set
            {
                this.resSdAppDesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string restrictRole
        {
            get
            {
                return this.restrictRoleField;
            }
            set
            {
                this.restrictRoleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sdAppDes
        {
            get
            {
                return this.sdAppDesField;
            }
            set
            {
                this.sdAppDesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sdChk1
        {
            get
            {
                return this.sdChk1Field;
            }
            set
            {
                this.sdChk1Field = value;
            }
        }

        /// <remarks/>
       [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sdChk2
        {
            get
            {
                return this.sdChk2Field;
            }
            set
            {
                this.sdChk2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("standardComments", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public StandardCommentModel4WS[] standardComments
        {
            get
            {
                return this.standardCommentsField;
            }
            set
            {
                this.standardCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string stpNum
        {
            get
            {
                return this.stpNumField;
            }
            set
            {
                this.stpNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taskSpecItems", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TaskSpecificInfoModel4WS[] taskSpecItems
        {
            get
            {
                return this.taskSpecItemsField;
            }
            set
            {
                this.taskSpecItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel userRolePrivilegeModel
        {
            get
            {
                return this.userRolePrivilegeModelField;
            }
            set
            {
                this.userRolePrivilegeModelField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refActStatusFlag
        {
            get
            {
                return this.refActStatusFlagField;
            }
            set
            {
                this.refActStatusFlagField = value;
            }
        }
    }

}
