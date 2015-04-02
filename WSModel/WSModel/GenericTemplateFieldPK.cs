/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GenericTemplateFieldPK.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GenericTemplateFieldPK.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTemplateTableValuePK))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTemplateTableValue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTemplateAttribute))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GenericTemplateFieldPK
    {

        private string fieldNameField;

        private string groupNameField;

        private string subgroupNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fieldName
        {
            get
            {
                return this.fieldNameField;
            }
            set
            {
                this.fieldNameField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string subgroupName
        {
            get
            {
                return this.subgroupNameField;
            }
            set
            {
                this.subgroupNameField = value;
            }
        }
    }
}
