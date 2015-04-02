#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExpressionInputParamterResultModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExpressionInputParamterResultModel4WS.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ExpressionInputParamterResultModel4WS
    {

        private string[] executeFieldNameField;

        private string[] expressionNamesField;

        private ExpressionFieldModel[] inputParametersField;

        private string servProvCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("executeFieldName", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] executeFieldName
        {
            get
            {
                return this.executeFieldNameField;
            }
            set
            {
                this.executeFieldNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("expressionNames", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] expressionNames
        {
            get
            {
                return this.expressionNamesField;
            }
            set
            {
                this.expressionNamesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inputParameters", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ExpressionFieldModel[] inputParameters
        {
            get
            {
                return this.inputParametersField;
            }
            set
            {
                this.inputParametersField = value;
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
