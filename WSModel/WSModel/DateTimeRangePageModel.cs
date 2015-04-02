#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DateTimeRangePageModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DateTimeRangePageModel.cs 196819 2011-05-23 08:23:22Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class DateTimeRangePageModel : DateAndTimesModel
    {

        private string dateTimeRangeTypeField;

        private bool hideInspTimesInACAField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dateTimeRangeType
        {
            get
            {
                return this.dateTimeRangeTypeField;
            }
            set
            {
                this.dateTimeRangeTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool hideInspTimesInACA
        {
            get
            {
                return this.hideInspTimesInACAField;
            }
            set
            {
                this.hideInspTimesInACAField = value;
            }
        }
    }
}
