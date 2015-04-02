#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DocumentResultModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DocumentResultModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\zehon.cai $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System.Collections.Generic;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class DocumentResultModel
    {
        private string errorMessageField;

        private DocumentModel[] documentListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string errorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("documentList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DocumentModel[] documentList
        {
            get
            {
                return this.documentListField;
            }
            set
            {
                this.documentListField = value;
            }
        }
    }
}