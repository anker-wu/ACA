/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapTypeFilterModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapTypeFilterModel4WS.cs 178037 2010-07-30 06:25:12Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy.WSModel
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapTypeFilterModel4WS
    {

        private CapTypeModel[] availableCapTypeListField;

        private string filterNameField;

        private CapTypeModel[] filteredCapTypeListField;

        private string moduleNameField;

        private string servProvCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("availableCapTypeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public CapTypeModel[] availableCapTypeList
        {
            get
            {
                return this.availableCapTypeListField;
            }
            set
            {
                this.availableCapTypeListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string filterName
        {
            get
            {
                return this.filterNameField;
            }
            set
            {
                this.filterNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("filteredCapTypeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public CapTypeModel[] filteredCapTypeList
        {
            get
            {
                return this.filteredCapTypeListField;
            }
            set
            {
                this.filteredCapTypeListField = value;
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
    }
}
