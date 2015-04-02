#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: XSocialMediaUserModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012
 *
 *  Description:
 *
 * </pre>
 */
#endregion

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XSocialMediaUserModel : XSocialMediaUserPKModel
    {

        private SimpleAuditModel auditModelField;

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
    }
}
