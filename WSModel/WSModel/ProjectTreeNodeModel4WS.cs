/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ProjectTreeNodeModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ProjectTreeNodeModel4WS.cs 193571 2011-03-23 12:19:12Z ACHIEVO\hans.shi $.
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
    public partial class ProjectTreeNodeModel4WS
    {

        private CapModel4WS cAPField;

        private ProjectTreeNodeModel4WS[] childrenField;

        private bool firstChildField;

        private bool lastChildField;

        private int levelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapModel4WS CAP
        {
            get
            {
                return this.cAPField;
            }
            set
            {
                this.cAPField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("children", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ProjectTreeNodeModel4WS[] children
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