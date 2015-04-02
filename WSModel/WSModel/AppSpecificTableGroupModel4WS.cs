/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AppSpecificTableGroupModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AppSpecificTableGroupModel4WS.cs 277225 2014-08-12 10:47:00Z ACHIEVO\james.shi $.
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
    public partial class AppSpecificTableGroupModel4WS
    {

        private CapIDModel4WS capIDModelField;

        private string groupNameField;

        private string instructionField;

        private string resInstructionField;

        private string[] tablesMapField;

        private AppSpecificTableModel4WS[] tablesMapValuesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel4WS capIDModel
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
        public string groupName
        {
            get
            {
                return this.groupNameField;
            }
            set
            {
                this.groupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string instruction
        {
            get
            {
                return this.instructionField;
            }
            set
            {
                this.instructionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resInstruction
        {
            get
            {
                return this.resInstructionField;
            }
            set
            {
                this.resInstructionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tablesMap", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] tablesMap
        {
            get
            {
                return this.tablesMapField;
            }
            set
            {
                this.tablesMapField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tablesMapValues", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AppSpecificTableModel4WS[] tablesMapValues
        {
            get
            {
                return this.tablesMapValuesField;
            }
            set
            {
                this.tablesMapValuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("searchInfoModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AppSpecificInfoModel4WS[] searchInfoModels
        {
            get;
            set;
        }
    }
}
