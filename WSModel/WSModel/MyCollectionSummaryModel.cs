/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MyCollectionSummaryModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: MyCollectionSummaryModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class MyCollectionSummaryModel
    {

        private double feeDueField;

        private double feePaidField;

        private int inspectionApprovedField;

        private int inspectionCanceledField;

        private int inspectionDeniedField;

        private int inspectionPendingField;

        private int inspectionRescheduledField;

        private int inspectionScheduledField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double feeDue
        {
            get
            {
                return this.feeDueField;
            }
            set
            {
                this.feeDueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double feePaid
        {
            get
            {
                return this.feePaidField;
            }
            set
            {
                this.feePaidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int inspectionApproved
        {
            get
            {
                return this.inspectionApprovedField;
            }
            set
            {
                this.inspectionApprovedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int inspectionCanceled
        {
            get
            {
                return this.inspectionCanceledField;
            }
            set
            {
                this.inspectionCanceledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int inspectionDenied
        {
            get
            {
                return this.inspectionDeniedField;
            }
            set
            {
                this.inspectionDeniedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int inspectionPending
        {
            get
            {
                return this.inspectionPendingField;
            }
            set
            {
                this.inspectionPendingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int inspectionRescheduled
        {
            get
            {
                return this.inspectionRescheduledField;
            }
            set
            {
                this.inspectionRescheduledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int inspectionScheduled
        {
            get
            {
                return this.inspectionScheduledField;
            }
            set
            {
                this.inspectionScheduledField = value;
            }
        }
    }
}
