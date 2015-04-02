/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AppSpecificTableModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AppSpecificTableModel4WS.cs 258169 2013-10-10 03:54:11Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class AppSpecificTableModel4WS : ICloneable
    {
        private AppSpecificTableColumnModel4WS[] columnsField;
        
        private AppSpecificTableField4WS[] defaultFieldField;
        
        private string groupNameField;
        
        private string resTableNameField;
        
        private string[] rowIndexField;
        
        private AppSpecificTableField4WS[] tableFieldField;
        
        private string[] tableFieldValuesField;
        
        private int tableIndexField;
        
        private string tableNameField;
        
        private TemplateLayoutConfigModel templateLayoutConfigField;
        
        private string vchDispFlagField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("columns", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public AppSpecificTableColumnModel4WS[] columns {
            get {
                return this.columnsField;
            }
            set {
                this.columnsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("defaultField", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public AppSpecificTableField4WS[] defaultField {
            get {
                return this.defaultFieldField;
            }
            set {
                this.defaultFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groupName {
            get {
                return this.groupNameField;
            }
            set {
                this.groupNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resTableName {
            get {
                return this.resTableNameField;
            }
            set {
                this.resTableNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("rowIndex", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string[] rowIndex {
            get {
                return this.rowIndexField;
            }
            set {
                this.rowIndexField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tableField", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public AppSpecificTableField4WS[] tableField {
            get {
                return this.tableFieldField;
            }
            set {
                this.tableFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tableFieldValues", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public string[] tableFieldValues {
            get {
                return this.tableFieldValuesField;
            }
            set {
                this.tableFieldValuesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int tableIndex {
            get {
                return this.tableIndexField;
            }
            set {
                this.tableIndexField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tableName {
            get {
                return this.tableNameField;
            }
            set {
                this.tableNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateLayoutConfigModel templateLayoutConfig {
            get {
                return this.templateLayoutConfigField;
            }
            set {
                this.templateLayoutConfigField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string vchDispFlag {
            get {
                return this.vchDispFlagField;
            }
            set {
                this.vchDispFlagField = value;
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Deep clone this AppSpecificTableModel4WS object.
        /// </summary>
        /// <returns>AppSpecificTableModel4WS object.</returns>
        public object Clone()
        {
            if (null == this)
            {
                return null;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        #endregion
    }
}
