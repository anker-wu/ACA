/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefExpressionExecuteFieldModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefExpressionExecuteFieldModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class RefExpressionExecuteFieldModel4WS
    {

        private string m_executeFieldField;

        private string m_expressionNameField;

        private string m_fireEventField;

        private string m_servProvCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string m_executeField
        {
            get
            {
                return this.m_executeFieldField;
            }
            set
            {
                this.m_executeFieldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string m_expressionName
        {
            get
            {
                return this.m_expressionNameField;
            }
            set
            {
                this.m_expressionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string m_fireEvent
        {
            get
            {
                return this.m_fireEventField;
            }
            set
            {
                this.m_fireEventField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string m_servProvCode
        {
            get
            {
                return this.m_servProvCodeField;
            }
            set
            {
                this.m_servProvCodeField = value;
            }
        }
    }
}
