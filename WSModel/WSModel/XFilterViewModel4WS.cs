/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XFilterViewModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XFilterViewModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XFilterViewModel4WS
    {

        private string[] attributesField;

        private string[] attributesValuesField;

        private string contractValueField;

        private string displayColumnsField;

        private string expandValueField;

        private string filterDescField;

        private string filterLevelIdField;

        private GFilterLevelModel4WS[] filterLevelsField;

        private string filterNameField;

        private string layoutField;

        private string recDateField;

        private string recFulNameField;

        private string recStatusField;

        private string refreshIntervalField;

        private string selectTypeField;

        private string servProvCodeField;

        private string sortOrderField;

        private string viewDescField;

        private XFilterViewElementModel4WS[] viewElementsField;

        private string viewIdField;

        private string viewNameField;

        private string viewTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attributes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attributesValues", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] attributesValues
        {
            get
            {
                return this.attributesValuesField;
            }
            set
            {
                this.attributesValuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contractValue
        {
            get
            {
                return this.contractValueField;
            }
            set
            {
                this.contractValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayColumns
        {
            get
            {
                return this.displayColumnsField;
            }
            set
            {
                this.displayColumnsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string expandValue
        {
            get
            {
                return this.expandValueField;
            }
            set
            {
                this.expandValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string filterDesc
        {
            get
            {
                return this.filterDescField;
            }
            set
            {
                this.filterDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string filterLevelId
        {
            get
            {
                return this.filterLevelIdField;
            }
            set
            {
                this.filterLevelIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("filterLevels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GFilterLevelModel4WS[] filterLevels
        {
            get
            {
                return this.filterLevelsField;
            }
            set
            {
                this.filterLevelsField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recDate
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
        public string recFulName
        {
            get
            {
                return this.recFulNameField;
            }
            set
            {
                this.recFulNameField = value;
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
        public string refreshInterval
        {
            get
            {
                return this.refreshIntervalField;
            }
            set
            {
                this.refreshIntervalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string selectType
        {
            get
            {
                return this.selectTypeField;
            }
            set
            {
                this.selectTypeField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sortOrder
        {
            get
            {
                return this.sortOrderField;
            }
            set
            {
                this.sortOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewDesc
        {
            get
            {
                return this.viewDescField;
            }
            set
            {
                this.viewDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("viewElements", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XFilterViewElementModel4WS[] viewElements
        {
            get
            {
                return this.viewElementsField;
            }
            set
            {
                this.viewElementsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewId
        {
            get
            {
                return this.viewIdField;
            }
            set
            {
                this.viewIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewName
        {
            get
            {
                return this.viewNameField;
            }
            set
            {
                this.viewNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewType
        {
            get
            {
                return this.viewTypeField;
            }
            set
            {
                this.viewTypeField = value;
            }
        }
    }
}
