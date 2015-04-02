/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: xRefEducationProviderModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 *  xRefEducationProviderModel4WS model..
 * 
 *  Notes:
 * $Id: xRefEducationProviderModel4WS.cs 142354 2009-08-07 02:19:45Z ACHIEVO\jackie.yu $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class XRefEducationProviderModel4WS : ProviderPKModel4WS
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ProviderModel4WS providerModel
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefEducationModel4WS refEducationModel
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long refEducationNbr
        { 
            get;
            set;
        }
    }
}
