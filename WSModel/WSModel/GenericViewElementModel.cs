/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GenericViewElementModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GenericViewElementModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class GenericViewElementModel : GenericViewElementPKModel
    {

        private SimpleAuditModel auditModelField;

        private int displayWidthField;

        private bool displayWidthFieldSpecified;

        private int heightField;

        private bool heightFieldSpecified;

        private string hotKeyField;

        private int inputWidthField;

        private bool inputWidthFieldSpecified;

        private string isDisplayDefaultWithField;

        private string isHiddenField;

        private string isLinkableField;

        private string isReadOnlyField;

        private string isRequiredField;

        private string labelIDField;

        private int labelWidthField;

        private bool labelWidthFieldSpecified;

        private int leftField;

        private bool leftFieldSpecified;

        private string mapTableField;

        private string mapTableColumnField;

        private string maskField;

        private string menuItemCodeField;

        private string refColumnNameField;

        private string refColumnTypeField;

        private string searchRangeField;

        private int sortOrderField;

        private bool sortOrderFieldSpecified;

        private string sortableField;

        private int tabIdxField;

        private bool tabIdxFieldSpecified;

        private string toolTipIDField;

        private int topField;

        private bool topFieldSpecified;

        private string typeField;

        private int unitWidthField;

        private bool unitWidthFieldSpecified;

        private string viewElementDataTypeField;

        private string viewElementDescField;

        private string viewElementNameField;

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
        public int displayWidth
        {
            get
            {
                return this.displayWidthField;
            }
            set
            {
                this.displayWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool displayWidthSpecified
        {
            get
            {
                return this.displayWidthFieldSpecified;
            }
            set
            {
                this.displayWidthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hotKey
        {
            get
            {
                return this.hotKeyField;
            }
            set
            {
                this.hotKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int inputWidth
        {
            get
            {
                return this.inputWidthField;
            }
            set
            {
                this.inputWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool inputWidthSpecified
        {
            get
            {
                return this.inputWidthFieldSpecified;
            }
            set
            {
                this.inputWidthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isDisplayDefaultWith
        {
            get
            {
                return this.isDisplayDefaultWithField;
            }
            set
            {
                this.isDisplayDefaultWithField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isHidden
        {
            get
            {
                return this.isHiddenField;
            }
            set
            {
                this.isHiddenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isLinkable
        {
            get
            {
                return this.isLinkableField;
            }
            set
            {
                this.isLinkableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isReadOnly
        {
            get
            {
                return this.isReadOnlyField;
            }
            set
            {
                this.isReadOnlyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isRequired
        {
            get
            {
                return this.isRequiredField;
            }
            set
            {
                this.isRequiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string labelID
        {
            get
            {
                return this.labelIDField;
            }
            set
            {
                this.labelIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int labelWidth
        {
            get
            {
                return this.labelWidthField;
            }
            set
            {
                this.labelWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool labelWidthSpecified
        {
            get
            {
                return this.labelWidthFieldSpecified;
            }
            set
            {
                this.labelWidthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int left
        {
            get
            {
                return this.leftField;
            }
            set
            {
                this.leftField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool leftSpecified
        {
            get
            {
                return this.leftFieldSpecified;
            }
            set
            {
                this.leftFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mapTable
        {
            get
            {
                return this.mapTableField;
            }
            set
            {
                this.mapTableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mapTableColumn
        {
            get
            {
                return this.mapTableColumnField;
            }
            set
            {
                this.mapTableColumnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mask
        {
            get
            {
                return this.maskField;
            }
            set
            {
                this.maskField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string menuItemCode
        {
            get
            {
                return this.menuItemCodeField;
            }
            set
            {
                this.menuItemCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refColumnName
        {
            get
            {
                return this.refColumnNameField;
            }
            set
            {
                this.refColumnNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refColumnType
        {
            get
            {
                return this.refColumnTypeField;
            }
            set
            {
                this.refColumnTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string searchRange
        {
            get
            {
                return this.searchRangeField;
            }
            set
            {
                this.searchRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int sortOrder
        {
            get
            {
                return this.sortOrderField;
            }
            set
            {
                this.sortOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sortOrderSpecified
        {
            get
            {
                return this.sortOrderFieldSpecified;
            }
            set
            {
                this.sortOrderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sortable
        {
            get
            {
                return this.sortableField;
            }
            set
            {
                this.sortableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int tabIdx
        {
            get
            {
                return this.tabIdxField;
            }
            set
            {
                this.tabIdxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool tabIdxSpecified
        {
            get
            {
                return this.tabIdxFieldSpecified;
            }
            set
            {
                this.tabIdxFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string toolTipID
        {
            get
            {
                return this.toolTipIDField;
            }
            set
            {
                this.toolTipIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int top
        {
            get
            {
                return this.topField;
            }
            set
            {
                this.topField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool topSpecified
        {
            get
            {
                return this.topFieldSpecified;
            }
            set
            {
                this.topFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int unitWidth
        {
            get
            {
                return this.unitWidthField;
            }
            set
            {
                this.unitWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool unitWidthSpecified
        {
            get
            {
                return this.unitWidthFieldSpecified;
            }
            set
            {
                this.unitWidthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewElementDataType
        {
            get
            {
                return this.viewElementDataTypeField;
            }
            set
            {
                this.viewElementDataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewElementDesc
        {
            get
            {
                return this.viewElementDescField;
            }
            set
            {
                this.viewElementDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewElementName
        {
            get
            {
                return this.viewElementNameField;
            }
            set
            {
                this.viewElementNameField = value;
            }
        }
    }
}
