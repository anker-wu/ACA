/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GenericViewElementPKModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GenericViewElementPKModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericViewElementModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class GenericViewElementPKModel
    {

        private long viewElementIDField;

        private bool viewElementIDFieldSpecified;

        private long viewIDField;

        private bool viewIDFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long viewElementID
        {
            get
            {
                return this.viewElementIDField;
            }
            set
            {
                this.viewElementIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool viewElementIDSpecified
        {
            get
            {
                return this.viewElementIDFieldSpecified;
            }
            set
            {
                this.viewElementIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long viewID
        {
            get
            {
                return this.viewIDField;
            }
            set
            {
                this.viewIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool viewIDSpecified
        {
            get
            {
                return this.viewIDFieldSpecified;
            }
            set
            {
                this.viewIDFieldSpecified = value;
            }
        }
    }
}