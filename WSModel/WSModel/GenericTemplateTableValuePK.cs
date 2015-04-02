/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GenericTemplateTableValuePK.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GenericTemplateTableValuePK.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTemplateTableValue))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GenericTemplateTableValuePK : GenericTemplateFieldPK
    {

        private long rowIndexField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long rowIndex
        {
            get
            {
                return this.rowIndexField;
            }
            set
            {
                this.rowIndexField = value;
            }
        }
    }
}
