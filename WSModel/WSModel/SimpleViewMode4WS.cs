/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: simpleViewModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: simpleViewModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\grady.lu $.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class SimpleViewModel4WS
    {

        private GFilterScreenPermissionModel4WS permissionModelField;

        private int screenHeightField;

        private int screenWidthField;

        private string sectionIDField;

        private SimpleViewElementModel4WS[] simpleViewElementsField;

        private int sizeUnitField;

        private string labelLayoutTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public GFilterScreenPermissionModel4WS permissionModel
        {
            get
            {
                return this.permissionModelField;
            }
            set
            {
                this.permissionModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int screenHeight
        {
            get
            {
                return this.screenHeightField;
            }
            set
            {
                this.screenHeightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int screenWidth
        {
            get
            {
                return this.screenWidthField;
            }
            set
            {
                this.screenWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sectionID
        {
            get
            {
                return this.sectionIDField;
            }
            set
            {
                this.sectionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("simpleViewElements", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public SimpleViewElementModel4WS[] simpleViewElements
        {
            get
            {
                return this.simpleViewElementsField;
            }
            set
            {
                this.simpleViewElementsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int sizeUnit
        {
            get
            {
                return this.sizeUnitField;
            }
            set
            {
                this.sizeUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string labelLayoutType
        {
            get
            {
                return this.labelLayoutTypeField;
            }

            set
            {
                this.labelLayoutTypeField = value;
            }
        }
        

    }
}
