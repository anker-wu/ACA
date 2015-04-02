#region Header

/**
 *  Accela Citizen Access
 *  File: AttrTableAttributeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AttrTableAttributeModel.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header


namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class AttrTableAttributeModel : templateAttributeModel
    {

        private System.Nullable<long> r1AttributeTableIDField;

        private string r1AttributeTableNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> r1AttributeTableID
        {
            get
            {
                return this.r1AttributeTableIDField;
            }
            set
            {
                this.r1AttributeTableIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeTableName
        {
            get
            {
                return this.r1AttributeTableNameField;
            }
            set
            {
                this.r1AttributeTableNameField = value;
            }
        }
    }
}