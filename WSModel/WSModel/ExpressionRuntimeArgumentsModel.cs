/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExpressionRuntimeArgumentsModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExpressionRuntimeArgumentsModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ExpressionRuntimeArgumentsModel
    {

        private System.Nullable<long> apoNumberField;

        private AppSpecificInfoModel[] appSpecificInfoModelListField;

        private ExpressionRuntimeArgumentPKModel[] argumentPKsField;

        private string asiGroupCodeField;

        private string callerIDField;

        private CapIDModel capIDField;

        private string executeFieldVariableKeyField;

        private string executeInField;

        private StringIntegerMapEntry[] maxRowIndexesField;

        private string servProvCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> apoNumber
        {
            get
            {
                return this.apoNumberField;
            }
            set
            {
                this.apoNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("appSpecificInfoModelList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AppSpecificInfoModel[] appSpecificInfoModelList
        {
            get
            {
                return this.appSpecificInfoModelListField;
            }
            set
            {
                this.appSpecificInfoModelListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("argumentPKs", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ExpressionRuntimeArgumentPKModel[] argumentPKs
        {
            get
            {
                return this.argumentPKsField;
            }
            set
            {
                this.argumentPKsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string asiGroupCode
        {
            get
            {
                return this.asiGroupCodeField;
            }
            set
            {
                this.asiGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string callerID
        {
            get
            {
                return this.callerIDField;
            }
            set
            {
                this.callerIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capID
        {
            get
            {
                return this.capIDField;
            }
            set
            {
                this.capIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string executeFieldVariableKey
        {
            get
            {
                return this.executeFieldVariableKeyField;
            }
            set
            {
                this.executeFieldVariableKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string executeIn
        {
            get
            {
                return this.executeInField;
            }
            set
            {
                this.executeInField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StringIntegerMapEntry[] maxRowIndexes
        {
            get
            {
                return this.maxRowIndexesField;
            }
            set
            {
                this.maxRowIndexesField = value;
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class StringIntegerMapEntry
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string key
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int? value
        {
            get;
            set;
        }
    }

}
