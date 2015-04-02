#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DocumentModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DocumentModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class DocumentModel
    {

        private string acaPermissionsField;

        private System.Nullable<long> actionNoField;

        private string allowActionsField;

        private string altIdField;

        private string autoDownloadStatusField;

        private string b1Udf1Field;

        private string b1Udf2Field;

        private string b1Udf3Field;

        private string b1Udf4Field;

        private CapIDModel capIDField;

        private string capTypeAliasField;

        private string categoryByActionField;

        private string componentNameField;

        private long conditionNumberField;

        private string deleteRoleField;

        private UserRolePrivilegeModel deleteRoleModelField;

        private bool deleteableField;

        private string docCategoryField;

        private System.Nullable<System.DateTime> docDateField;

        private string docDepartmentField;

        private string docDescriptionField;

        private string docGroupField;

        private string docNameField;

        private System.Nullable<double> docNumOfSetsField;

        private string docStatusField;

        private System.Nullable<System.DateTime> docStatusDateField;

        private bool docStatusDateFieldSpecified;

        private string docSybTypeField;

        private string docTypeField;

        private string docVersionField;

        private DocumentContentModel documentContentField;

        private System.Nullable<long> documentNoField;

        private string editURLField;

        private string eleDateTimeField;

        private string eleLocationField;

        private string entityField;

        private string entityIDField;

        private string entityTypeField;

        private string fileDbSchemaField;

        private string fileDbServerField;

        private string fileDbTypeField;

        private string fileKeyField;

        private string fileNameField;

        private string fileOwnerPermissionField;

        private string fileSignedField;

        private string fileSignedByField;

        private System.Nullable<System.DateTime> fileSignedDateField;

        private System.Nullable<double> fileSizeField;

        private string fileUpLoadByField;

        private System.Nullable<System.DateTime> fileUpLoadDateField;

        private bool historyField;

        private string identifierDisplayField;

        private System.Nullable<long> inspectionIDField;

        private string inspectionTypeField;

        private string moduleNameField;

        private string parcelNumberField;

        private System.Nullable<long> parentSeqNbrField;

        private string passwordField;

        private System.Nullable<System.DateTime> phyDateTimeField;

        private string phyLocationField;

        private EdmsPolicyModel policyField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFulNamField;

        private string recLockField;

        private string recSecurityField;

        private string recStatusField;

        private System.Nullable<long> refDocumetntNoField;

        private string refServProvCodeField;

        private System.Nullable<long> relatedIDField;

        private DocumentRelationModel relationModelField;

        private string resDocCategoryField;

        private string scoreFileFlagField;

        private string serviceProviderCodeField;

        private string socCommentField;

        private string sourceField;

        private System.Nullable<long> sourceDocNbrField;

        private string sourceEntityIDField;

        private string sourceEntityTypeField;

        private string sourceNameField;

        private string sourceRecfulnamField;

        private string sourceSpcField;

        private TemplateModel templateField;

        private string uRLField;

        private UserRolePrivilegeModel uploadRoleModelField;

        private string userNameField;

        private string viewRoleField;

        private UserRolePrivilegeModel viewRoleModelField;

        private string viewTitleRoleField;

        private UserRolePrivilegeModel viewTitleRoleModelField;

        private bool viewTitleableField;

        private bool viewableField;

        private string virtualFoldersField;

        private bool viewStatusabledField;

        private string[] reviewStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool viewStatusabled
        {
            get
            {
                return this.viewStatusabledField;
            }
            set
            {
                this.viewStatusabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reviewStatus", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] reviewStatus
        {
            get
            {
                return this.reviewStatusField;
            }
            set
            {
                this.reviewStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string acaPermissions
        {
            get
            {
                return this.acaPermissionsField;
            }
            set
            {
                this.acaPermissionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> actionNo
        {
            get
            {
                return this.actionNoField;
            }
            set
            {
                this.actionNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string allowActions
        {
            get
            {
                return this.allowActionsField;
            }
            set
            {
                this.allowActionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string altId
        {
            get
            {
                return this.altIdField;
            }
            set
            {
                this.altIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string autoDownloadStatus
        {
            get
            {
                return this.autoDownloadStatusField;
            }
            set
            {
                this.autoDownloadStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1Udf1
        {
            get
            {
                return this.b1Udf1Field;
            }
            set
            {
                this.b1Udf1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1Udf2
        {
            get
            {
                return this.b1Udf2Field;
            }
            set
            {
                this.b1Udf2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1Udf3
        {
            get
            {
                return this.b1Udf3Field;
            }
            set
            {
                this.b1Udf3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1Udf4
        {
            get
            {
                return this.b1Udf4Field;
            }
            set
            {
                this.b1Udf4Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capID
        {
            get
            {
                return this.capIDField;
            }
            set
            {
                this.capIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string capTypeAlias
        {
            get
            {
                return this.capTypeAliasField;
            }
            set
            {
                this.capTypeAliasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string categoryByAction
        {
            get
            {
                return this.categoryByActionField;
            }
            set
            {
                this.categoryByActionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string componentName
        {
            get
            {
                return this.componentNameField;
            }
            set
            {
                this.componentNameField = value;
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
        public string deleteRole
        {
            get
            {
                return this.deleteRoleField;
            }
            set
            {
                this.deleteRoleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel deleteRoleModel
        {
            get
            {
                return this.deleteRoleModelField;
            }
            set
            {
                this.deleteRoleModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool deleteable
        {
            get
            {
                return this.deleteableField;
            }
            set
            {
                this.deleteableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docCategory
        {
            get
            {
                return this.docCategoryField;
            }
            set
            {
                this.docCategoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> docDate
        {
            get
            {
                return this.docDateField;
            }
            set
            {
                this.docDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docDepartment
        {
            get
            {
                return this.docDepartmentField;
            }
            set
            {
                this.docDepartmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docDescription
        {
            get
            {
                return this.docDescriptionField;
            }
            set
            {
                this.docDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docGroup
        {
            get
            {
                return this.docGroupField;
            }
            set
            {
                this.docGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docName
        {
            get
            {
                return this.docNameField;
            }
            set
            {
                this.docNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> docNumOfSets
        {
            get
            {
                return this.docNumOfSetsField;
            }
            set
            {
                this.docNumOfSetsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docStatus
        {
            get
            {
                return this.docStatusField;
            }
            set
            {
                this.docStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> docStatusDate
        {
            get
            {
                return this.docStatusDateField;
            }
            set
            {
                this.docStatusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool docStatusDateSpecified
        {
            get
            {
                return this.docStatusDateFieldSpecified;
            }
            set
            {
                this.docStatusDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docSybType
        {
            get
            {
                return this.docSybTypeField;
            }
            set
            {
                this.docSybTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docType
        {
            get
            {
                return this.docTypeField;
            }
            set
            {
                this.docTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docVersion
        {
            get
            {
                return this.docVersionField;
            }
            set
            {
                this.docVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DocumentContentModel documentContent
        {
            get
            {
                return this.documentContentField;
            }
            set
            {
                this.documentContentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> documentNo
        {
            get
            {
                return this.documentNoField;
            }
            set
            {
                this.documentNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string editURL
        {
            get
            {
                return this.editURLField;
            }
            set
            {
                this.editURLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string eleDateTime
        {
            get
            {
                return this.eleDateTimeField;
            }
            set
            {
                this.eleDateTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string eleLocation
        {
            get
            {
                return this.eleLocationField;
            }
            set
            {
                this.eleLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entity
        {
            get
            {
                return this.entityField;
            }
            set
            {
                this.entityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityID
        {
            get
            {
                return this.entityIDField;
            }
            set
            {
                this.entityIDField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileDbSchema
        {
            get
            {
                return this.fileDbSchemaField;
            }
            set
            {
                this.fileDbSchemaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileDbServer
        {
            get
            {
                return this.fileDbServerField;
            }
            set
            {
                this.fileDbServerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileDbType
        {
            get
            {
                return this.fileDbTypeField;
            }
            set
            {
                this.fileDbTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileKey
        {
            get
            {
                return this.fileKeyField;
            }
            set
            {
                this.fileKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileOwnerPermission
        {
            get
            {
                return this.fileOwnerPermissionField;
            }
            set
            {
                this.fileOwnerPermissionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileSigned
        {
            get
            {
                return this.fileSignedField;
            }
            set
            {
                this.fileSignedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileSignedBy
        {
            get
            {
                return this.fileSignedByField;
            }
            set
            {
                this.fileSignedByField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> fileSignedDate
        {
            get
            {
                return this.fileSignedDateField;
            }
            set
            {
                this.fileSignedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> fileSize
        {
            get
            {
                return this.fileSizeField;
            }
            set
            {
                this.fileSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileUpLoadBy
        {
            get
            {
                return this.fileUpLoadByField;
            }
            set
            {
                this.fileUpLoadByField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> fileUpLoadDate
        {
            get
            {
                return this.fileUpLoadDateField;
            }
            set
            {
                this.fileUpLoadDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool history
        {
            get
            {
                return this.historyField;
            }
            set
            {
                this.historyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string identifierDisplay
        {
            get
            {
                return this.identifierDisplayField;
            }
            set
            {
                this.identifierDisplayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> inspectionID
        {
            get
            {
                return this.inspectionIDField;
            }
            set
            {
                this.inspectionIDField = value;
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
        public string moduleName
        {
            get
            {
                return this.moduleNameField;
            }
            set
            {
                this.moduleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parcelNumber
        {
            get
            {
                return this.parcelNumberField;
            }
            set
            {
                this.parcelNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> parentSeqNbr
        {
            get
            {
                return this.parentSeqNbrField;
            }
            set
            {
                this.parentSeqNbrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> phyDateTime
        {
            get
            {
                return this.phyDateTimeField;
            }
            set
            {
                this.phyDateTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phyLocation
        {
            get
            {
                return this.phyLocationField;
            }
            set
            {
                this.phyLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public EdmsPolicyModel policy
        {
            get
            {
                return this.policyField;
            }
            set
            {
                this.policyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> recDate
        {
            get
            {
                return this.recDateField;
            }
            set
            {
                this.recDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFulNam
        {
            get
            {
                return this.recFulNamField;
            }
            set
            {
                this.recFulNamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recLock
        {
            get
            {
                return this.recLockField;
            }
            set
            {
                this.recLockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recSecurity
        {
            get
            {
                return this.recSecurityField;
            }
            set
            {
                this.recSecurityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recStatus
        {
            get
            {
                return this.recStatusField;
            }
            set
            {
                this.recStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> refDocumetntNo
        {
            get
            {
                return this.refDocumetntNoField;
            }
            set
            {
                this.refDocumetntNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refServProvCode
        {
            get
            {
                return this.refServProvCodeField;
            }
            set
            {
                this.refServProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> relatedID
        {
            get
            {
                return this.relatedIDField;
            }
            set
            {
                this.relatedIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DocumentRelationModel relationModel
        {
            get
            {
                return this.relationModelField;
            }
            set
            {
                this.relationModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resDocCategory
        {
            get
            {
                return this.resDocCategoryField;
            }
            set
            {
                this.resDocCategoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string scoreFileFlag
        {
            get
            {
                return this.scoreFileFlagField;
            }
            set
            {
                this.scoreFileFlagField = value;
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
        public string socComment
        {
            get
            {
                return this.socCommentField;
            }
            set
            {
                this.socCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> sourceDocNbr
        {
            get
            {
                return this.sourceDocNbrField;
            }
            set
            {
                this.sourceDocNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sourceEntityID
        {
            get
            {
                return this.sourceEntityIDField;
            }
            set
            {
                this.sourceEntityIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sourceEntityType
        {
            get
            {
                return this.sourceEntityTypeField;
            }
            set
            {
                this.sourceEntityTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sourceName
        {
            get
            {
                return this.sourceNameField;
            }
            set
            {
                this.sourceNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sourceRecfulnam
        {
            get
            {
                return this.sourceRecfulnamField;
            }
            set
            {
                this.sourceRecfulnamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sourceSpc
        {
            get
            {
                return this.sourceSpcField;
            }
            set
            {
                this.sourceSpcField = value;
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
        public string URL
        {
            get
            {
                return this.uRLField;
            }
            set
            {
                this.uRLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel uploadRoleModel
        {
            get
            {
                return this.uploadRoleModelField;
            }
            set
            {
                this.uploadRoleModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string userName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewRole
        {
            get
            {
                return this.viewRoleField;
            }
            set
            {
                this.viewRoleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel viewRoleModel
        {
            get
            {
                return this.viewRoleModelField;
            }
            set
            {
                this.viewRoleModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewTitleRole
        {
            get
            {
                return this.viewTitleRoleField;
            }
            set
            {
                this.viewTitleRoleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel viewTitleRoleModel
        {
            get
            {
                return this.viewTitleRoleModelField;
            }
            set
            {
                this.viewTitleRoleModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool viewTitleable
        {
            get
            {
                return this.viewTitleableField;
            }
            set
            {
                this.viewTitleableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool viewable
        {
            get
            {
                return this.viewableField;
            }
            set
            {
                this.viewableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string virtualFolders
        {
            get
            {
                return this.virtualFoldersField;
            }
            set
            {
                this.virtualFoldersField = value;
            }
        }
    }
}