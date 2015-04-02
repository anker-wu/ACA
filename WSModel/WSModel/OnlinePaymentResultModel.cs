/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: OnlinePaymentResultModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: OnlinePaymentResultModel.cs 181105 2010-09-15 11:26:45Z ACHIEVO\hans.shi $.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class OnlinePaymentResultModel
    {

        private string batchNbrField;

        private CapPaymentResultModel[] capPaymentResultModelsField;

        private EntityPaymentResultModel[] entityPaymentResultModelsField;

        private string[] exceptionMsgField;

        private string paramStringField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string batchNbr
        {
            get
            {
                return this.batchNbrField;
            }
            set
            {
                this.batchNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("capPaymentResultModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public CapPaymentResultModel[] capPaymentResultModels
        {
            get
            {
                return this.capPaymentResultModelsField;
            }
            set
            {
                this.capPaymentResultModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("entityPaymentResultModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public EntityPaymentResultModel[] entityPaymentResultModels
        {
            get
            {
                return this.entityPaymentResultModelsField;
            }
            set
            {
                this.entityPaymentResultModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("exceptionMsg", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] exceptionMsg
        {
            get
            {
                return this.exceptionMsgField;
            }
            set
            {
                this.exceptionMsgField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paramString
        {
            get
            {
                return this.paramStringField;
            }
            set
            {
                this.paramStringField = value;
            }
        }
    }
}
