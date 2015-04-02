/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: EDMSEntityIdModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: EDMSEntityIdModel4WS.cs 130439 2009-05-13 10:02:20Z ACHIEVO\jackie.yu $.
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
    public partial class EDMSEntityIdModel4WS
    {

        private string idField;

        private string altIdField;

        private string capIdField;

        private string inspectionIdField;

        private string parcelIdField;

        private string workflowIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID
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
        public string altId
        {
            get
            {
                return this.altIdField;
            }
            set
            {
                this.altIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string capId
        {
            get
            {
                return this.capIdField;
            }
            set
            {
                this.capIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectionId
        {
            get
            {
                return this.inspectionIdField;
            }
            set
            {
                this.inspectionIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parcelId
        {
            get
            {
                return this.parcelIdField;
            }
            set
            {
                this.parcelIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workflowId
        {
            get
            {
                return this.workflowIdField;
            }
            set
            {
                this.workflowIdField = value;
            }
        }
    }
}
