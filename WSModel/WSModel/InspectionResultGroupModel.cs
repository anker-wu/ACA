/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionResultGroupModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: InspectionResultGroupModel.cs 169604 2010-03-30 09:59:38Z ACHIEVO\daly.zeng $.
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
    public partial class InspectionResultGroupModel
    {

        private string groupNameField;

        private InspectionResultModel[] inspectionResultModelsField;

        private string sPCField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groupName
        {
            get
            {
                return this.groupNameField;
            }
            set
            {
                this.groupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("inspectionResultModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public InspectionResultModel[] inspectionResultModels
        {
            get
            {
                return this.inspectionResultModelsField;
            }
            set
            {
                this.inspectionResultModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SPC
        {
            get
            {
                return this.sPCField;
            }
            set
            {
                this.sPCField = value;
            }
        }
    }
}
