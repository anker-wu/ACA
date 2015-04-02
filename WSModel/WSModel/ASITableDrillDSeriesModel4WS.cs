/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ASITableDrillDownModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ASITableDrillDSeriesModel4WS.cs 136284 2009-06-25 10:10:38Z ACHIEVO\jackie.yu $.
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
    public partial class ASITableDrillDSeriesModel4WS
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("asiTableDrillDValMapModel4WSList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ASITableDrillDValMapModel4WS[] asiTableDrillDValMapModel4WSList
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string childColName
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parentColName
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parentSeriesId
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string selectType
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("selectTypeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] selectTypeList
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string seriesId
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int seriesSize
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string uniqueLabel
        {
            get;
            set;
        }
    }
}
