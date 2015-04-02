#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XInspectionTypeCategoryModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XInspectionTypeCategoryModel.cs 207115 2011-11-10 03:23:07Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class XInspectionTypeCategoryModel
    {

        private SimpleAuditModel auditModelField;

        private int externalIdField;

        private XInspectionTypeCategoryPKModel refInspectionTypeCategoryPKModelField;

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
        public int externalId
        {
            get
            {
                return this.externalIdField;
            }
            set
            {
                this.externalIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XInspectionTypeCategoryPKModel refInspectionTypeCategoryPKModel
        {
            get
            {
                return this.refInspectionTypeCategoryPKModelField;
            }
            set
            {
                this.refInspectionTypeCategoryPKModelField = value;
            }
        }
    }
}
