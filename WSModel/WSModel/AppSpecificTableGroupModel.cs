#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AppSpecificTableGroupModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AppSpecificTableGroupModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class AppSpecificTableGroupModel
    {
        private CapIDModel capIDModelField;

        private string groupNameField;

        private AppSpecificInfoModel[] searchInfoModelsField;

        private AppSpecificTableGroupModelEntry[] tablesMapField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capIDModel
        {
            get
            {
                return this.capIDModelField;
            }
            set
            {
                this.capIDModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groupName
        {
            get
            {
                return this.groupNameField;
            }
            set
            {
                this.groupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("searchInfoModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AppSpecificInfoModel[] searchInfoModels
        {
            get
            {
                return this.searchInfoModelsField;
            }
            set
            {
                this.searchInfoModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("entry", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public AppSpecificTableGroupModelEntry[] tablesMap
        {
            get
            {
                return this.tablesMapField;
            }
            set
            {
                this.tablesMapField = value;
            }
        }
    }
}
