/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapContactModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapContactModel4WS.cs 245488 2013-03-06 07:36:24Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapContactModel4WS
    {

        private CapIDModel4WS capIDField;

        private string componentNameField;

        private string contactOnSRChangeField;

        private PeopleModel4WS peopleField;

        private string personTypeField;

        private string refContactNumberField;

        private string searchFlagField;

        private string syncFlagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel4WS capID
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
        public string componentName
        {
            get
            {
                return this.componentNameField;
            }
            set
            {
                this.componentNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactOnSRChange
        {
            get
            {
                return this.contactOnSRChangeField;
            }
            set
            {
                this.contactOnSRChangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PeopleModel4WS people
        {
            get
            {
                return this.peopleField;
            }
            set
            {
                this.peopleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string personType
        {
            get
            {
                return this.personTypeField;
            }
            set
            {
                this.personTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refContactNumber
        {
            get
            {
                return this.refContactNumberField;
            }
            set
            {
                this.refContactNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string searchFlag
        {
            get
            {
                return this.searchFlagField;
            }
            set
            {
                this.searchFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string syncFlag
        {
            get
            {
                return this.syncFlagField;
            }
            set
            {
                this.syncFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accessLevel
        {
            get;
            set;
        }

        /// <summary>
        /// The <c>validateFlag</c> property only used in ACA. 
        /// It's used to keep Contact section's DataSource setting to set Search and Sync flag for Contact data before save cap.
        /// </summary>
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string validateFlag
        {
            get;
            set;
        }
    }
}
