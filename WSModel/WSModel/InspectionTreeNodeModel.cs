#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionTreeNodeModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class InspectionTreeNodeModel
    {

        private InspectionTreeNodeModel[] childrenField;

        private bool firstChildField;

        private InspectionModel inspectionModelField;

        private bool lastChildField;

        private int levelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("children", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public InspectionTreeNodeModel[] children
        {
            get
            {
                return this.childrenField;
            }
            set
            {
                this.childrenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool firstChild
        {
            get
            {
                return this.firstChildField;
            }
            set
            {
                this.firstChildField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public InspectionModel inspectionModel
        {
            get
            {
                return this.inspectionModelField;
            }
            set
            {
                this.inspectionModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool lastChild
        {
            get
            {
                return this.lastChildField;
            }
            set
            {
                this.lastChildField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }
    }
}
