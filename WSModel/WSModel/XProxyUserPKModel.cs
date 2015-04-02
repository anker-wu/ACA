/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XProxyUserPKModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XProxyUserPKModel.cs 182829 2010-10-20 08:59:58Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(XProxyUserModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XProxyUserPKModel
    {

        private System.Nullable<long> proxyUserSeqNbrField;

        private string serviceProviderCodeField;

        private System.Nullable<long> userSeqNbrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> proxyUserSeqNbr
        {
            get
            {
                return this.proxyUserSeqNbrField;
            }
            set
            {
                this.proxyUserSeqNbrField = value;
            }
        }

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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> userSeqNbr
        {
            get
            {
                return this.userSeqNbrField;
            }
            set
            {
                this.userSeqNbrField = value;
            }
        }
    }
    
}
