#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapAssociateLicenseCertification4WS.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapAssociateLicenseCertification4WS.cs 261829 2013-11-29 05:46:42Z ACHIEVO\james.shi $.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapAssociateLicenseCertification4WS
    {

        private RefContinuingEducationModel[] capAssociateContEducationField;

        private RefEducationModel[] capAssociateEducationField;

        private RefExaminationModel[] capAssociateExaminationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("capAssociateContEducation", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefContinuingEducationModel[] capAssociateContEducation
        {
            get
            {
                return this.capAssociateContEducationField;
            }
            set
            {
                this.capAssociateContEducationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("capAssociateEducation", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefEducationModel[] capAssociateEducation
        {
            get
            {
                return this.capAssociateEducationField;
            }
            set
            {
                this.capAssociateEducationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("capAssociateExamination", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefExaminationModel[] capAssociateExamination
        {
            get
            {
                return this.capAssociateExaminationField;
            }
            set
            {
                this.capAssociateExaminationField = value;
            }
        }
    }
}
