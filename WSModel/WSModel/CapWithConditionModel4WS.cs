/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapWithConditionModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapWithConditionModel4WS.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapWithConditionModel4WS
    {

        private CapModel4WS capModelField;

        private NoticeConditionModel conditionModelField;

        private NoticeConditionModel[] conditionModelArrayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapModel4WS capModel
        {
            get
            {
                return this.capModelField;
            }
            set
            {
                this.capModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NoticeConditionModel conditionModel
        {
            get
            {
                return this.conditionModelField;
            }
            set
            {
                this.conditionModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("conditionModelArray", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public NoticeConditionModel[] conditionModelArray
        {
            get
            {
                return this.conditionModelArrayField;
            }
            set
            {
                this.conditionModelArrayField = value;
            }
        }
    }
}
