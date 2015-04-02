/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: OwnerCompModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: OwnerCompModel.cs 130107 2014-07-15 14:00:56Z ACHIEVO\canon.wu $.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class OwnerCompModel : OwnerModel
    {
        private string parcelNbrField;

        private string structureNameField;

        private string structureTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parcelNbr
        {
            get
            {
                return this.parcelNbrField;
            }
            set
            {
                this.parcelNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string structureName
        {
            get
            {
                return this.structureNameField;
            }
            set
            {
                this.structureNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string structureType
        {
            get
            {
                return this.structureTypeField;
            }
            set
            {
                this.structureTypeField = value;
            }
        }
    }
}
