/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExpressionRunResultModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExpressionRunResultModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class ExpressionRunResultModel4WS
    {

        private System.Nullable<long>[] exceptionsMapKeysField;

        private string[] exceptionsMapValuesField;

        private ExpressionFieldModel[] fieldsField;

        private System.Nullable<long>[] messagesMapKeysField;

        private string[] messagesMapValuesField;

        private string[] newRowsMapKeysField;

        private System.Nullable<int>[] newRowsMapValuesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("exceptionsMapKeys", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long>[] exceptionsMapKeys
        {
            get
            {
                return this.exceptionsMapKeysField;
            }
            set
            {
                this.exceptionsMapKeysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("exceptionsMapValues", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] exceptionsMapValues
        {
            get
            {
                return this.exceptionsMapValuesField;
            }
            set
            {
                this.exceptionsMapValuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("fields", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ExpressionFieldModel[] fields
        {
            get
            {
                return this.fieldsField;
            }
            set
            {
                this.fieldsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("messagesMapKeys", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long>[] messagesMapKeys
        {
            get
            {
                return this.messagesMapKeysField;
            }
            set
            {
                this.messagesMapKeysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("messagesMapValues", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] messagesMapValues
        {
            get
            {
                return this.messagesMapValuesField;
            }
            set
            {
                this.messagesMapValuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("newRowsMapKeys", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] newRowsMapKeys
        {
            get
            {
                return this.newRowsMapKeysField;
            }
            set
            {
                this.newRowsMapKeysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("newRowsMapValues", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int>[] newRowsMapValues
        {
            get
            {
                return this.newRowsMapValuesField;
            }
            set
            {
                this.newRowsMapValuesField = value;
            }
        }
    }
}
