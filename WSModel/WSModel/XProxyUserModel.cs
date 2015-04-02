/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XProxyUserModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XProxyUserModel.cs 182829 2010-10-20 08:59:58Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XProxyUserModel : XProxyUserPKModel
    {
        private System.Nullable<System.DateTime> createdDateField;

        private System.Nullable<System.DateTime> accessDateField;

        private SimpleAuditModel auditModelField;

        private string invitationMessageField;

        private string nickNameField;

        private XProxyUserPermissionModel[] xProxyUserPermissionModelsField;

        private System.Nullable<ProxyUserStatus> proxyStatusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> createdDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }     

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> accessDate
        {
            get
            {
                return this.accessDateField;
            }
            set
            {
                this.accessDateField = value;
            }
        }

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
        public string invitationMessage
        {
            get
            {
                return this.invitationMessageField;
            }
            set
            {
                this.invitationMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nickName
        {
            get
            {
                return this.nickNameField;
            }
            set
            {
                this.nickNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public XProxyUserPermissionModel[] XProxyUserPermissionModels
        {
            get
            {
                return this.xProxyUserPermissionModelsField;
            }
            set
            {
                this.xProxyUserPermissionModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<ProxyUserStatus> proxyStatus
        {
            get
            {
                return this.proxyStatusField;
            }
            set
            {
                this.proxyStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public enum ProxyUserStatus
    {

        /// <remarks/>
        P,

        /// <remarks/>
        A,

        /// <remarks/>
        R,

        /// <remarks/>
        E
    }
}
