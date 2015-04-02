#region

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AssetDataModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AssetDataModel4WS.cs 239452 2012-12-12 05:56:43Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */


#endregion

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class AssetDataModel4WS
    {

        private AssetAttrTableValueModel[] assetAttrTableValueListField;

        private AssetMasterModel assetMasterField;

        private AssetTypeModel assetTypeField;

        private DataAttributeModel[] dataAttributeListField;

        private AttrTableAttributeModel[] refAttrTableAttriListField;

        private AttributeModel[] refDataAttributeListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("assetAttrTableValueList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AssetAttrTableValueModel[] assetAttrTableValueList
        {
            get
            {
                return this.assetAttrTableValueListField;
            }
            set
            {
                this.assetAttrTableValueListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AssetMasterModel assetMaster
        {
            get
            {
                return this.assetMasterField;
            }
            set
            {
                this.assetMasterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AssetTypeModel assetType
        {
            get
            {
                return this.assetTypeField;
            }
            set
            {
                this.assetTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dataAttributeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public DataAttributeModel[] dataAttributeList
        {
            get
            {
                return this.dataAttributeListField;
            }
            set
            {
                this.dataAttributeListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("refAttrTableAttriList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AttrTableAttributeModel[] refAttrTableAttriList
        {
            get
            {
                return this.refAttrTableAttriListField;
            }
            set
            {
                this.refAttrTableAttriListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("refDataAttributeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AttributeModel[] refDataAttributeList
        {
            get
            {
                return this.refDataAttributeListField;
            }
            set
            {
                this.refDataAttributeListField = value;
            }
        }
    }
}