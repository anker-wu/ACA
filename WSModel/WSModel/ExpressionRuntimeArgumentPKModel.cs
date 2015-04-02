#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: expressionRuntimeArgumentPKModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: expressionRuntimeArgumentPKModel.cs 181867 2011-01-03 08:06:18Z ACHIEVO\kale.huang $.
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
    public partial class ExpressionRuntimeArgumentPKModel
    {

        private System.Nullable<long> portletIDField;

        private string viewKey1Field;

        private string viewKey2Field;

        private string viewKey3Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> portletID
        {
            get
            {
                return this.portletIDField;
            }
            set
            {
                this.portletIDField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewKey2
        {
            get
            {
                return this.viewKey2Field;
            }
            set
            {
                this.viewKey2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewKey3
        {
            get
            {
                return this.viewKey3Field;
            }
            set
            {
                this.viewKey3Field = value;
            }
        }
    }
}