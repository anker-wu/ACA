/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XpolicyUserRolePrivilegeModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XpolicyUserRolePrivilegeModel.cs 169604 2010-03-30 09:59:38Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XpolicyUserRolePrivilegeModel : XPolicyModel
    {

        private UserRolePrivilegeModel userRolePrivilegeModelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel userRolePrivilegeModel
        {
            get
            {
                return this.userRolePrivilegeModelField;
            }
            set
            {
                this.userRolePrivilegeModelField = value;
            }
        }
    }   
    
}
