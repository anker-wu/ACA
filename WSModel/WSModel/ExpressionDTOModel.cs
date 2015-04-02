/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExpressionDTOModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExpressionDTOModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class ExpressionDTOModel
    {

        private string behaviorField;

        private ExpressionFieldModel[] inputParamsField;

        private ExpressionFieldModel[] runResultField;

        private string viewKey1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string behavior
        {
            get
            {
                return this.behaviorField;
            }
            set
            {
                this.behaviorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inputParams", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ExpressionFieldModel[] inputParams
        {
            get
            {
                return this.inputParamsField;
            }
            set
            {
                this.inputParamsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("runResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ExpressionFieldModel[] runResult
        {
            get
            {
                return this.runResultField;
            }
            set
            {
                this.runResultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewKey1
        {
            get
            {
                return this.viewKey1Field;
            }
            set
            {
                this.viewKey1Field = value;
            }
        }
    }
}
