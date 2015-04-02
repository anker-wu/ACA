/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XRefContactAddressTypePKModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XRefContactAddressTypePKModel.cs 218456 2012-05-02 05:10:45Z ACHIEVO\alan.hu $.
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
    public partial class XRefContactAddressTypePKModel
    {

        private string serviceProviderCodeField;

        private long xRefIDField;

        private bool xRefIDFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long XRefID
        {
            get
            {
                return this.xRefIDField;
            }
            set
            {
                this.xRefIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool XRefIDSpecified
        {
            get
            {
                return this.xRefIDFieldSpecified;
            }
            set
            {
                this.xRefIDFieldSpecified = value;
            }
        }
    }
}
