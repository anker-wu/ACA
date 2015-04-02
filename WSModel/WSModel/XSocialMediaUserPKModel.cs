#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: XSocialMediaUserPKModel.cs
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(XSocialMediaUserModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XSocialMediaUserPKModel
    {

        private System.Nullable<long> publicUserSeqField;

        private string serviceProviderCodeField;

        private string socialIDField;

        private string socialTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> publicUserSeq
        {
            get
            {
                return this.publicUserSeqField;
            }
            set
            {
                this.publicUserSeqField = value;
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
        public string socialID
        {
            get
            {
                return this.socialIDField;
            }
            set
            {
                this.socialIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string socialType
        {
            get
            {
                return this.socialTypeField;
            }
            set
            {
                this.socialTypeField = value;
            }
        }
    }
}
