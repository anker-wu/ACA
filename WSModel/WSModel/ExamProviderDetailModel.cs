#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExamProviderDetailModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamProviderDetailModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public class ExamProviderDetailModel : ProviderDetailModel
    {
        private string drivingDirectionsField;

        private string handicapAccessibleDesField;

        private string isHandicapAccessibleField;

        private string supportedLanguagesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string drivingDirections
        {
            get
            {
                return this.drivingDirectionsField;
            }
            set
            {
                this.drivingDirectionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string handicapAccessibleDes
        {
            get
            {
                return this.handicapAccessibleDesField;
            }
            set
            {
                this.handicapAccessibleDesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isHandicapAccessible
        {
            get
            {
                return this.isHandicapAccessibleField;
            }
            set
            {
                this.isHandicapAccessibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string supportedLanguages
        {
            get
            {
                return this.supportedLanguagesField;
            }
            set
            {
                this.supportedLanguagesField = value;
            }
        }
    }
}
