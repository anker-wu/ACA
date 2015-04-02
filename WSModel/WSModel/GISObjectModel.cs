/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GISObjectModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GISObjectModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GISObjectModel
    {

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIdField;

        private string auditStatusField;

        private CapIDModel capIDField;

        private string entIdField;

        private string gisIdField;

        private string gisLabelFieldField;

        private string gisObjectIDField;

        private string gisThemeGroupField;

        private string gisThemeNameField;

        private string idField;

        private string latitudeCoordinateField;

        private string layerIdField;

        private string longitudeCoordinateField;

        private System.Nullable<long> objectIdField;

        private string primaryGISFlagField;

        private string serviceIDField;

        private string serviceProviderCodeField;

        private System.Nullable<long> serviceSourceNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> auditDate
        {
            get
            {
                return this.auditDateField;
            }
            set
            {
                this.auditDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditId
        {
            get
            {
                return this.auditIdField;
            }
            set
            {
                this.auditIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditStatus
        {
            get
            {
                return this.auditStatusField;
            }
            set
            {
                this.auditStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capID
        {
            get
            {
                return this.capIDField;
            }
            set
            {
                this.capIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entId
        {
            get
            {
                return this.entIdField;
            }
            set
            {
                this.entIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisId
        {
            get
            {
                return this.gisIdField;
            }
            set
            {
                this.gisIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisLabelField
        {
            get
            {
                return this.gisLabelFieldField;
            }
            set
            {
                this.gisLabelFieldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisObjectID
        {
            get
            {
                return this.gisObjectIDField;
            }
            set
            {
                this.gisObjectIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisThemeGroup
        {
            get
            {
                return this.gisThemeGroupField;
            }
            set
            {
                this.gisThemeGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisThemeName
        {
            get
            {
                return this.gisThemeNameField;
            }
            set
            {
                this.gisThemeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string latitudeCoordinate
        {
            get
            {
                return this.latitudeCoordinateField;
            }
            set
            {
                this.latitudeCoordinateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string layerId
        {
            get
            {
                return this.layerIdField;
            }
            set
            {
                this.layerIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string longitudeCoordinate
        {
            get
            {
                return this.longitudeCoordinateField;
            }
            set
            {
                this.longitudeCoordinateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> objectId
        {
            get
            {
                return this.objectIdField;
            }
            set
            {
                this.objectIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string primaryGISFlag
        {
            get
            {
                return this.primaryGISFlagField;
            }
            set
            {
                this.primaryGISFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceID
        {
            get
            {
                return this.serviceIDField;
            }
            set
            {
                this.serviceIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> serviceSourceNumber
        {
            get
            {
                return this.serviceSourceNumberField;
            }
            set
            {
                this.serviceSourceNumberField = value;
            }
        }
    }
}
