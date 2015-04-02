#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ContactAddressPKModel.cs
*
*  Accela, Inc.
*  Copyright (C): 2011
*
*  Description: Contact Address PK model.
*
*  Notes:
* $Id: ContactAddressPKModel.cs 210786 2011-12-27 09:54:22Z ACHIEVO\daly.zeng $.
*  Revision History
*  Date,            Who,        What
*  Dec 5, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ContactAddressPKModel
    {
        private System.Nullable<long> addressIDField;

        private string serviceProviderCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> addressID
        {
            get
            {
                return this.addressIDField;
            }
            set
            {
                this.addressIDField = value;
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
    }
}
