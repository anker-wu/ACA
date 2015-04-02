#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: InspectionModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class InspectionModel : LanguageModel
    {

        private bool activeField;

        private ActivityModel activityField;

        private CalendarInspectionTypeModel calendarInspectionTypeField;

        private CapModel capField;

        private CapContactModel capContactModelField;

        private CapTypeModel capTypeField;

        private string capTypeStrField;

        private CommentModel commentField;

        private bool completedField;

        private bool configuredInInspFlowField;

        private TimeLogModel[] displayTimeModelsField;

        private string enabledCheckField;

        private TimeLogModel[] existTimeModelsField;

        private int guideSheetCountField;

        private GGuideSheetModel[] gGuideSheetModelsField;

        private int indexField;

        private string inspectionDepartmentNameField;

        private string inspectionTypeField;

        private AddressModel primaryAddressField;

        private LicenseProfessionalModel primaryLicenseProfessionalField;

        private CommentModel requestCommentField;

        private string resultCommentField;

        private int schedOrderField;

        private bool schedOrderFieldSpecified;

        private object[] specialInfoField;

        private StandardCommentModel[] standardCommentsField;

        private object[] timeAccountUpdateModelListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool active
        {
            get
            {
                return this.activeField;
            }
            set
            {
                this.activeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ActivityModel activity
        {
            get
            {
                return this.activityField;
            }
            set
            {
                this.activityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CalendarInspectionTypeModel calendarInspectionType
        {
            get
            {
                return this.calendarInspectionTypeField;
            }
            set
            {
                this.calendarInspectionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapModel cap
        {
            get
            {
                return this.capField;
            }
            set
            {
                this.capField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapContactModel capContactModel
        {
            get
            {
                return this.capContactModelField;
            }
            set
            {
                this.capContactModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapTypeModel capType
        {
            get
            {
                return this.capTypeField;
            }
            set
            {
                this.capTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string capTypeStr
        {
            get
            {
                return this.capTypeStrField;
            }
            set
            {
                this.capTypeStrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CommentModel comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool completed
        {
            get
            {
                return this.completedField;
            }
            set
            {
                this.completedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool configuredInInspFlow
        {
            get
            {
                return this.configuredInInspFlowField;
            }
            set
            {
                this.configuredInInspFlowField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("displayTimeModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TimeLogModel[] displayTimeModels
        {
            get
            {
                return this.displayTimeModelsField;
            }
            set
            {
                this.displayTimeModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string enabledCheck
        {
            get
            {
                return this.enabledCheckField;
            }
            set
            {
                this.enabledCheckField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("existTimeModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TimeLogModel[] existTimeModels
        {
            get
            {
                return this.existTimeModelsField;
            }
            set
            {
                this.existTimeModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int guideSheetCount
        {
            get
            {
                return this.guideSheetCountField;
            }
            set
            {
                this.guideSheetCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("gGuideSheetModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public GGuideSheetModel[] gGuideSheetModels
        {
            get
            {
                return this.gGuideSheetModelsField;
            }
            set
            {
                this.gGuideSheetModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int index
        {
            get
            {
                return this.indexField;
            }
            set
            {
                this.indexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectionDepartmentName
        {
            get
            {
                return this.inspectionDepartmentNameField;
            }
            set
            {
                this.inspectionDepartmentNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectionType
        {
            get
            {
                return this.inspectionTypeField;
            }
            set
            {
                this.inspectionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AddressModel primaryAddress
        {
            get
            {
                return this.primaryAddressField;
            }
            set
            {
                this.primaryAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LicenseProfessionalModel primaryLicenseProfessional
        {
            get
            {
                return this.primaryLicenseProfessionalField;
            }
            set
            {
                this.primaryLicenseProfessionalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CommentModel requestComment
        {
            get
            {
                return this.requestCommentField;
            }
            set
            {
                this.requestCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resultComment
        {
            get
            {
                return this.resultCommentField;
            }
            set
            {
                this.resultCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int schedOrder
        {
            get
            {
                return this.schedOrderField;
            }
            set
            {
                this.schedOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool schedOrderSpecified
        {
            get
            {
                return this.schedOrderFieldSpecified;
            }
            set
            {
                this.schedOrderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specialInfo", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] specialInfo
        {
            get
            {
                return this.specialInfoField;
            }
            set
            {
                this.specialInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("standardComments", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public StandardCommentModel[] standardComments
        {
            get
            {
                return this.standardCommentsField;
            }
            set
            {
                this.standardCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("timeAccountUpdateModelList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] timeAccountUpdateModelList
        {
            get
            {
                return this.timeAccountUpdateModelListField;
            }
            set
            {
                this.timeAccountUpdateModelListField = value;
            }
        }
    }
}
