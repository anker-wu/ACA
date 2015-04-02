#region Header

/**
 *  Accela Citizen Access
 *  File: GisRelatedTableModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: GisRelatedTableModel.cs 134054 2013-10-29 10:10:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class GisRelatedTableModel
    {

        private string foreignKeyFieldField;

        private string gisLayerField;

        private string gisRelatedTableField;

        private System.Nullable<long> gisRelatedTableIDField;

        private System.Nullable<long> gisRelationshipIDField;

        private string gisServiceField;

        private string referenceKeyFieldField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string foreignKeyField
        {
            get
            {
                return this.foreignKeyFieldField;
            }
            set
            {
                this.foreignKeyFieldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisLayer
        {
            get
            {
                return this.gisLayerField;
            }
            set
            {
                this.gisLayerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisRelatedTable
        {
            get
            {
                return this.gisRelatedTableField;
            }
            set
            {
                this.gisRelatedTableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> gisRelatedTableID
        {
            get
            {
                return this.gisRelatedTableIDField;
            }
            set
            {
                this.gisRelatedTableIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> gisRelationshipID
        {
            get
            {
                return this.gisRelationshipIDField;
            }
            set
            {
                this.gisRelationshipIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisService
        {
            get
            {
                return this.gisServiceField;
            }
            set
            {
                this.gisServiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string referenceKeyField
        {
            get
            {
                return this.referenceKeyFieldField;
            }
            set
            {
                this.referenceKeyFieldField = value;
            }
        }
    }
}
