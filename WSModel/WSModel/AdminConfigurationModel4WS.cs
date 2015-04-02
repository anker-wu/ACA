/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ACAModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AdminConfigurationModel4WS.cs 209458 2011-12-12 06:03:07Z ACHIEVO\daly.zeng $.
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
    public partial class AdminConfigurationModel4WS
    {

        private string callerIdField;

        private CapTypeModel[] capTypeModel4WSField;

        private XUITextModel[] labelModelArrayField;

        private string levelTypeField;

        private string moduleNameField;

        private string servProvCodeField;

        private SimpleViewModel4WS[] simpleViewModelsField;

        private BizDomainModel4WS[] standardBizDomainField;

        private BizDomainModel4WS[] standardChoice4HardcodeArrayField;

        private BizDomainModel4WS[] standardChoiceArrayField;

        private XPolicyModel[] standardChoiceHardcodeModelField;

        private string[] tabsOrderField;

        private TemplateLayoutConfigModel[] templateLayoutConfigListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string callerId
        {
            get
            {
                return this.callerIdField;
            }
            set
            {
                this.callerIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("capTypeModel4WS", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public CapTypeModel[] capTypeModel4WS
        {
            get
            {
                return this.capTypeModel4WSField;
            }
            set
            {
                this.capTypeModel4WSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("labelModelArray", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XUITextModel[] labelModelArray
        {
            get
            {
                return this.labelModelArrayField;
            }
            set
            {
                this.labelModelArrayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelType
        {
            get
            {
                return this.levelTypeField;
            }
            set
            {
                this.levelTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string moduleName
        {
            get
            {
                return this.moduleNameField;
            }
            set
            {
                this.moduleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servProvCode
        {
            get
            {
                return this.servProvCodeField;
            }
            set
            {
                this.servProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("simpleViewModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public SimpleViewModel4WS[] simpleViewModels
        {
            get
            {
                return this.simpleViewModelsField;
            }
            set
            {
                this.simpleViewModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("standardBizDomain", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public BizDomainModel4WS[] standardBizDomain
        {
            get
            {
                return this.standardBizDomainField;
            }
            set
            {
                this.standardBizDomainField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("standardChoice4HardcodeArray", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public BizDomainModel4WS[] standardChoice4HardcodeArray
        {
            get
            {
                return this.standardChoice4HardcodeArrayField;
            }
            set
            {
                this.standardChoice4HardcodeArrayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("standardChoiceArray", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public BizDomainModel4WS[] standardChoiceArray
        {
            get
            {
                return this.standardChoiceArrayField;
            }
            set
            {
                this.standardChoiceArrayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("standardChoiceHardcodeModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XPolicyModel[] standardChoiceHardcodeModel
        {
            get
            {
                return this.standardChoiceHardcodeModelField;
            }
            set
            {
                this.standardChoiceHardcodeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tabsOrder", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] tabsOrder
        {
            get
            {
                return this.tabsOrderField;
            }
            set
            {
                this.tabsOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateLayoutConfigList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TemplateLayoutConfigModel[] templateLayoutConfigList
        {
            get
            {
                return this.templateLayoutConfigListField;
            }
            set
            {
                this.templateLayoutConfigListField = value;
            }
        }
    }
}
