#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DeepLinkAuditTrailModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DeepLinkAuditTrailModel.cs 196819 2013-10-10 15:23:22Z ACHIEVO\peter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

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
    public partial class DeepLinkAuditTrailModel
    {

        private SimpleAuditModel auditModelField;

        private string deepLinkIDField;

        private string gUIDField;

        private string serviceDataField;

        private string uRLField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SimpleAuditModel auditModel
        {
            get
            {
                return this.auditModelField;
            }
            set
            {
                this.auditModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string deepLinkID
        {
            get
            {
                return this.deepLinkIDField;
            }
            set
            {
                this.deepLinkIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string GUID
        {
            get
            {
                return this.gUIDField;
            }
            set
            {
                this.gUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceData
        {
            get
            {
                return this.serviceDataField;
            }
            set
            {
                this.serviceDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string URL
        {
            get
            {
                return this.uRLField;
            }
            set
            {
                this.uRLField = value;
            }
        }
    }
}
