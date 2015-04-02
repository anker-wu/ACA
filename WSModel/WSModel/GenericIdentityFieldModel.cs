/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GenericIdentityFieldModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GenericIdentityFieldModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GenericIdentityFieldModel : GenericIdentityFieldPKModel
    {

        private SimpleAuditModel auditModelField;

        private string identNameField;

        private GenericViewElementModel[] viewElementsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SimpleAuditModel auditModel
        {
            get
            {
                return this.auditModelField;
            }
            set
            {
                this.auditModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string identName
        {
            get
            {
                return this.identNameField;
            }
            set
            {
                this.identNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("viewElements", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GenericViewElementModel[] viewElements
        {
            get
            {
                return this.viewElementsField;
            }
            set
            {
                this.viewElementsField = value;
            }
        }
    }
}