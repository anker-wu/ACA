/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ReportButtonInfoModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ReportButtonInfoModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class ReportButtonInfoModel4WS
    {

        private string buttonViewIDField;

        private string callerIDField;

        private CapIDModel4WS capIDField;

        private string currentModuleField;

        private bool multipleCapsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string buttonViewID
        {
            get
            {
                return this.buttonViewIDField;
            }
            set
            {
                this.buttonViewIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string callerID
        {
            get
            {
                return this.callerIDField;
            }
            set
            {
                this.callerIDField = value;
            }
        }

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
        public string currentModule
        {
            get
            {
                return this.currentModuleField;
            }
            set
            {
                this.currentModuleField = value;
            }
        }

        /// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool multipleCaps
        {
            get
            {
                return this.multipleCapsField;
            }
            set
            {
                this.multipleCapsField = value;
            }
        }
    }
}
