/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ConditionModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ConditionModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NoticeConditionModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ConditionModel : LanguageModel
    {

        private string actionDepartmentNameField;

        private string additionalInformationField;

        private string appliedDepartmentNameField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string conditionCommentField;

        private string conditionDescriptionField;

        private string conditionGroupField;

        private long conditionNumberField;

        private string conditionStatusField;

        private string conditionStatusAndTypeValueField;

        private string conditionStatusTypeField;

        private string conditionTypeField;

        private string dispAdditionalInformationField;

        private string dispConditionCommentField;

        private string dispConditionDescriptionField;

        private string dispLongDescriptonField;

        private string dispPublicDisplayMessageField;

        private string dispResolutionActionField;

        private string displayConditionNoticeField;

        private string displayConditionStatusAndTypeField;

        private string displayNoticeOnACAField;

        private string displayNoticeOnACAFeeField;

        private System.Nullable<System.DateTime> effectDateField;

        private System.DateTime endStatusDateField;

        private string entityTypeField;

        private System.Nullable<System.DateTime> expireDateField;

        private string impactCodeField;

        private string includeInConditionNameField;

        private string includeInShortDescriptionField;

        private string inheritableField;

        private SysUserModel issuedByUserField;

        private System.Nullable<System.DateTime> issuedDateField;

        private string longDescriptonField;

        private string noticeActionTypeField;

        private int priorityField;

        private string publicDisplayMessageField;

        private string refNumber1Field;

        private string refNumber2Field;

        private string resAdditionalInformationField;

        private string resConditionCommentField;

        private string resConditionDescriptionField;

        private string resLongDescriptonField;

        private string resPublicDisplayMessageField;

        private string resResolutionActionField;

        private string resolutionActionField;

        private string rightGrantedField;

        private string serviceProviderCodeField;

        private long sourceNumberField;

        private System.Nullable<long> standardConditionNumberField;

        private SysUserModel statusByUserField;

        private System.Nullable<System.DateTime> statusDateField;

        private TemplateModel templateField;

        private int uIUIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string actionDepartmentName
        {
            get
            {
                return this.actionDepartmentNameField;
            }
            set
            {
                this.actionDepartmentNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string additionalInformation
        {
            get
            {
                return this.additionalInformationField;
            }
            set
            {
                this.additionalInformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appliedDepartmentName
        {
            get
            {
                return this.appliedDepartmentNameField;
            }
            set
            {
                this.appliedDepartmentNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> auditDate
        {
            get
            {
                return this.auditDateField;
            }
            set
            {
                this.auditDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditID
        {
            get
            {
                return this.auditIDField;
            }
            set
            {
                this.auditIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditStatus
        {
            get
            {
                return this.auditStatusField;
            }
            set
            {
                this.auditStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionComment
        {
            get
            {
                return this.conditionCommentField;
            }
            set
            {
                this.conditionCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionDescription
        {
            get
            {
                return this.conditionDescriptionField;
            }
            set
            {
                this.conditionDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionGroup
        {
            get
            {
                return this.conditionGroupField;
            }
            set
            {
                this.conditionGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long conditionNumber
        {
            get
            {
                return this.conditionNumberField;
            }
            set
            {
                this.conditionNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionStatus
        {
            get
            {
                return this.conditionStatusField;
            }
            set
            {
                this.conditionStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionStatusAndTypeValue
        {
            get
            {
                return this.conditionStatusAndTypeValueField;
            }
            set
            {
                this.conditionStatusAndTypeValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionStatusType
        {
            get
            {
                return this.conditionStatusTypeField;
            }
            set
            {
                this.conditionStatusTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string conditionType
        {
            get
            {
                return this.conditionTypeField;
            }
            set
            {
                this.conditionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispAdditionalInformation
        {
            get
            {
                return this.dispAdditionalInformationField;
            }
            set
            {
                this.dispAdditionalInformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispConditionComment
        {
            get
            {
                return this.dispConditionCommentField;
            }
            set
            {
                this.dispConditionCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispConditionDescription
        {
            get
            {
                return this.dispConditionDescriptionField;
            }
            set
            {
                this.dispConditionDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispLongDescripton
        {
            get
            {
                return this.dispLongDescriptonField;
            }
            set
            {
                this.dispLongDescriptonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispPublicDisplayMessage
        {
            get
            {
                return this.dispPublicDisplayMessageField;
            }
            set
            {
                this.dispPublicDisplayMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispResolutionAction
        {
            get
            {
                return this.dispResolutionActionField;
            }
            set
            {
                this.dispResolutionActionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayConditionNotice
        {
            get
            {
                return this.displayConditionNoticeField;
            }
            set
            {
                this.displayConditionNoticeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayConditionStatusAndType
        {
            get
            {
                return this.displayConditionStatusAndTypeField;
            }
            set
            {
                this.displayConditionStatusAndTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayNoticeOnACA
        {
            get
            {
                return this.displayNoticeOnACAField;
            }
            set
            {
                this.displayNoticeOnACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayNoticeOnACAFee
        {
            get
            {
                return this.displayNoticeOnACAFeeField;
            }
            set
            {
                this.displayNoticeOnACAFeeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> effectDate
        {
            get
            {
                return this.effectDateField;
            }
            set
            {
                this.effectDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endStatusDate
        {
            get
            {
                return this.endStatusDateField;
            }
            set
            {
                this.endStatusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityType
        {
            get
            {
                return this.entityTypeField;
            }
            set
            {
                this.entityTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> expireDate
        {
            get
            {
                return this.expireDateField;
            }
            set
            {
                this.expireDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string impactCode
        {
            get
            {
                return this.impactCodeField;
            }
            set
            {
                this.impactCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string includeInConditionName
        {
            get
            {
                return this.includeInConditionNameField;
            }
            set
            {
                this.includeInConditionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string includeInShortDescription
        {
            get
            {
                return this.includeInShortDescriptionField;
            }
            set
            {
                this.includeInShortDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inheritable
        {
            get
            {
                return this.inheritableField;
            }
            set
            {
                this.inheritableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SysUserModel issuedByUser
        {
            get
            {
                return this.issuedByUserField;
            }
            set
            {
                this.issuedByUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> issuedDate
        {
            get
            {
                return this.issuedDateField;
            }
            set
            {
                this.issuedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string longDescripton
        {
            get
            {
                return this.longDescriptonField;
            }
            set
            {
                this.longDescriptonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string noticeActionType
        {
            get
            {
                return this.noticeActionTypeField;
            }
            set
            {
                this.noticeActionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int priority
        {
            get
            {
                return this.priorityField;
            }
            set
            {
                this.priorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string publicDisplayMessage
        {
            get
            {
                return this.publicDisplayMessageField;
            }
            set
            {
                this.publicDisplayMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refNumber1
        {
            get
            {
                return this.refNumber1Field;
            }
            set
            {
                this.refNumber1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refNumber2
        {
            get
            {
                return this.refNumber2Field;
            }
            set
            {
                this.refNumber2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resAdditionalInformation
        {
            get
            {
                return this.resAdditionalInformationField;
            }
            set
            {
                this.resAdditionalInformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resConditionComment
        {
            get
            {
                return this.resConditionCommentField;
            }
            set
            {
                this.resConditionCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resConditionDescription
        {
            get
            {
                return this.resConditionDescriptionField;
            }
            set
            {
                this.resConditionDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resLongDescripton
        {
            get
            {
                return this.resLongDescriptonField;
            }
            set
            {
                this.resLongDescriptonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resPublicDisplayMessage
        {
            get
            {
                return this.resPublicDisplayMessageField;
            }
            set
            {
                this.resPublicDisplayMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resResolutionAction
        {
            get
            {
                return this.resResolutionActionField;
            }
            set
            {
                this.resResolutionActionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resolutionAction
        {
            get
            {
                return this.resolutionActionField;
            }
            set
            {
                this.resolutionActionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rightGranted
        {
            get
            {
                return this.rightGrantedField;
            }
            set
            {
                this.rightGrantedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long sourceNumber
        {
            get
            {
                return this.sourceNumberField;
            }
            set
            {
                this.sourceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> standardConditionNumber
        {
            get
            {
                return this.standardConditionNumberField;
            }
            set
            {
                this.standardConditionNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SysUserModel statusByUser
        {
            get
            {
                return this.statusByUserField;
            }
            set
            {
                this.statusByUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> statusDate
        {
            get
            {
                return this.statusDateField;
            }
            set
            {
                this.statusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateModel template
        {
            get
            {
                return this.templateField;
            }
            set
            {
                this.templateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int UIUID
        {
            get
            {
                return this.uIUIDField;
            }
            set
            {
                this.uIUIDField = value;
            }
        }
    }
}
