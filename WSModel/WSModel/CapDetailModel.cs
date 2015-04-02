/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapDetailModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapDetailModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapDetailModel
    {

        private string actionField;

        private System.Nullable<double> actualProdUnitsField;

        private string anonymousFlagField;

        private string appStatusField;

        private System.Nullable<System.DateTime> appStatusDateField;

        private System.Nullable<System.DateTime> appearanceDateField;

        private string appearanceDayOfWeekField;

        private System.Nullable<System.DateTime> appearanceEndDateField;

        private string applSubStatusField;

        private System.Nullable<System.DateTime> asgnDateField;

        private string asgnDeptField;

        private string asgnStaffField;

        private System.Nullable<double> assetCostField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditStatusField;

        private string auditUserField;

        private double balanceField;

        private System.Nullable<System.DateTime> balanceDateField;

        private string bookingFlagField;

        private System.Nullable<long> buildingCountField;

        private CapIDModel capIDField;

        private string closedByField;

        private System.Nullable<System.DateTime> closedDateField;

        private string closedDeptField;

        private System.Nullable<System.DateTime> completeDateField;

        private string completeDeptField;

        private string completeStaffField;

        private string constTypeCodeField;

        private System.Nullable<double> costPerUnitField;

        private string createByField;

        private string creatorDeptField;

        private string creatorDeptAliasField;

        private string dfndtSignatureFlagField;

        private string dispositionField;

        private System.Nullable<System.DateTime> dispositionDateField;

        private System.Nullable<double> endActualProdUnitsField;

        private System.Nullable<System.DateTime> endAsgnDateField;

        private System.Nullable<System.DateTime> endClosedDateField;

        private System.Nullable<System.DateTime> endCompleteDateField;

        private System.Nullable<double> endCostPerUnitField;

        private System.Nullable<double> endEstCostPerUnitField;

        private System.Nullable<double> endEstJobCostField;

        private System.Nullable<double> endEstProdUnitsField;

        private System.Nullable<System.DateTime> endFirstIssuedDateField;

        private System.Nullable<System.DateTime> endScheduledDateField;

        private System.Nullable<double> endTotalJobCostField;

        private System.Nullable<double> endUndistributedCostField;

        private string enforceDeptField;

        private string enforceOfficerIdField;

        private string enforceOfficerNameField;

        private System.Nullable<double> estCostPerUnitField;

        private System.Nullable<double> estJobCostField;

        private System.Nullable<double> estProdUnitsField;

        private System.Nullable<System.DateTime> estimatedDueDateField;

        private System.Nullable<System.DateTime> firstIssuedDateField;

        private string fullNameOfStaffField;

        private string gaAgencyCodeField;

        private string gaBureauCodeField;

        private string gaDivisionCodeField;

        private string gaFnameField;

        private string gaGroupCodeField;

        private string gaLnameField;

        private string gaMnameField;

        private string gaOfficeCodeField;

        private string gaSectionCodeField;

        private System.Nullable<long> houseCountField;

        private System.Nullable<double> inPossessionTimeField;

        private string infractionFlagField;

        private string initialFeeScheduleField;

        private string inspectorDeptField;

        private string inspectorIdField;

        private string inspectorNameField;

        private string misdemeanorFlagField;

        private string offnWitnessedFlagField;

        private System.Nullable<double> overallApplicationTimeField;

        private double percentCompleteField;

        private System.Nullable<long> pmScheduleSeqField;

        private string priorityField;

        private string prodUnitTypeField;

        private string publicOwnedField;

        private string referenceTypeField;

        private string reportedChannelField;

        private System.Nullable<System.DateTime> scheduledDateField;

        private string scheduledTimeField;

        private string serverityField;

        private string shortNotesField;

        private string statusReasonField;

        private double totalFeeField;

        private System.Nullable<double> totalJobCostField;

        private double totalPayField;

        private System.Nullable<System.DateTime> trackStartDateField;

        private System.Nullable<double> undistributedCostField;

        private string urlField;

        private System.Nullable<double> valuationExtraAmountField;

        private System.Nullable<double> valuationMultiplierField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> actualProdUnits
        {
            get
            {
                return this.actualProdUnitsField;
            }
            set
            {
                this.actualProdUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string anonymousFlag
        {
            get
            {
                return this.anonymousFlagField;
            }
            set
            {
                this.anonymousFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appStatus
        {
            get
            {
                return this.appStatusField;
            }
            set
            {
                this.appStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> appStatusDate
        {
            get
            {
                return this.appStatusDateField;
            }
            set
            {
                this.appStatusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> appearanceDate
        {
            get
            {
                return this.appearanceDateField;
            }
            set
            {
                this.appearanceDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appearanceDayOfWeek
        {
            get
            {
                return this.appearanceDayOfWeekField;
            }
            set
            {
                this.appearanceDayOfWeekField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> appearanceEndDate
        {
            get
            {
                return this.appearanceEndDateField;
            }
            set
            {
                this.appearanceEndDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string applSubStatus
        {
            get
            {
                return this.applSubStatusField;
            }
            set
            {
                this.applSubStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> asgnDate
        {
            get
            {
                return this.asgnDateField;
            }
            set
            {
                this.asgnDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string asgnDept
        {
            get
            {
                return this.asgnDeptField;
            }
            set
            {
                this.asgnDeptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string asgnStaff
        {
            get
            {
                return this.asgnStaffField;
            }
            set
            {
                this.asgnStaffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> assetCost
        {
            get
            {
                return this.assetCostField;
            }
            set
            {
                this.assetCostField = value;
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
        public string auditUser
        {
            get
            {
                return this.auditUserField;
            }
            set
            {
                this.auditUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double balance
        {
            get
            {
                return this.balanceField;
            }
            set
            {
                this.balanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> balanceDate
        {
            get
            {
                return this.balanceDateField;
            }
            set
            {
                this.balanceDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bookingFlag
        {
            get
            {
                return this.bookingFlagField;
            }
            set
            {
                this.bookingFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> buildingCount
        {
            get
            {
                return this.buildingCountField;
            }
            set
            {
                this.buildingCountField = value;
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
        public string closedBy
        {
            get
            {
                return this.closedByField;
            }
            set
            {
                this.closedByField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> closedDate
        {
            get
            {
                return this.closedDateField;
            }
            set
            {
                this.closedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string closedDept
        {
            get
            {
                return this.closedDeptField;
            }
            set
            {
                this.closedDeptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> completeDate
        {
            get
            {
                return this.completeDateField;
            }
            set
            {
                this.completeDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string completeDept
        {
            get
            {
                return this.completeDeptField;
            }
            set
            {
                this.completeDeptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string completeStaff
        {
            get
            {
                return this.completeStaffField;
            }
            set
            {
                this.completeStaffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string constTypeCode
        {
            get
            {
                return this.constTypeCodeField;
            }
            set
            {
                this.constTypeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> costPerUnit
        {
            get
            {
                return this.costPerUnitField;
            }
            set
            {
                this.costPerUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string createBy
        {
            get
            {
                return this.createByField;
            }
            set
            {
                this.createByField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string creatorDept
        {
            get
            {
                return this.creatorDeptField;
            }
            set
            {
                this.creatorDeptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string creatorDeptAlias
        {
            get
            {
                return this.creatorDeptAliasField;
            }
            set
            {
                this.creatorDeptAliasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dfndtSignatureFlag
        {
            get
            {
                return this.dfndtSignatureFlagField;
            }
            set
            {
                this.dfndtSignatureFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string disposition
        {
            get
            {
                return this.dispositionField;
            }
            set
            {
                this.dispositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> dispositionDate
        {
            get
            {
                return this.dispositionDateField;
            }
            set
            {
                this.dispositionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endActualProdUnits
        {
            get
            {
                return this.endActualProdUnitsField;
            }
            set
            {
                this.endActualProdUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endAsgnDate
        {
            get
            {
                return this.endAsgnDateField;
            }
            set
            {
                this.endAsgnDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endClosedDate
        {
            get
            {
                return this.endClosedDateField;
            }
            set
            {
                this.endClosedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endCompleteDate
        {
            get
            {
                return this.endCompleteDateField;
            }
            set
            {
                this.endCompleteDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endCostPerUnit
        {
            get
            {
                return this.endCostPerUnitField;
            }
            set
            {
                this.endCostPerUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endEstCostPerUnit
        {
            get
            {
                return this.endEstCostPerUnitField;
            }
            set
            {
                this.endEstCostPerUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endEstJobCost
        {
            get
            {
                return this.endEstJobCostField;
            }
            set
            {
                this.endEstJobCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endEstProdUnits
        {
            get
            {
                return this.endEstProdUnitsField;
            }
            set
            {
                this.endEstProdUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endFirstIssuedDate
        {
            get
            {
                return this.endFirstIssuedDateField;
            }
            set
            {
                this.endFirstIssuedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endScheduledDate
        {
            get
            {
                return this.endScheduledDateField;
            }
            set
            {
                this.endScheduledDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endTotalJobCost
        {
            get
            {
                return this.endTotalJobCostField;
            }
            set
            {
                this.endTotalJobCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endUndistributedCost
        {
            get
            {
                return this.endUndistributedCostField;
            }
            set
            {
                this.endUndistributedCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string enforceDept
        {
            get
            {
                return this.enforceDeptField;
            }
            set
            {
                this.enforceDeptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string enforceOfficerId
        {
            get
            {
                return this.enforceOfficerIdField;
            }
            set
            {
                this.enforceOfficerIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string enforceOfficerName
        {
            get
            {
                return this.enforceOfficerNameField;
            }
            set
            {
                this.enforceOfficerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> estCostPerUnit
        {
            get
            {
                return this.estCostPerUnitField;
            }
            set
            {
                this.estCostPerUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> estJobCost
        {
            get
            {
                return this.estJobCostField;
            }
            set
            {
                this.estJobCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> estProdUnits
        {
            get
            {
                return this.estProdUnitsField;
            }
            set
            {
                this.estProdUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> estimatedDueDate
        {
            get
            {
                return this.estimatedDueDateField;
            }
            set
            {
                this.estimatedDueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> firstIssuedDate
        {
            get
            {
                return this.firstIssuedDateField;
            }
            set
            {
                this.firstIssuedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fullNameOfStaff
        {
            get
            {
                return this.fullNameOfStaffField;
            }
            set
            {
                this.fullNameOfStaffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaAgencyCode
        {
            get
            {
                return this.gaAgencyCodeField;
            }
            set
            {
                this.gaAgencyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaBureauCode
        {
            get
            {
                return this.gaBureauCodeField;
            }
            set
            {
                this.gaBureauCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaDivisionCode
        {
            get
            {
                return this.gaDivisionCodeField;
            }
            set
            {
                this.gaDivisionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaFname
        {
            get
            {
                return this.gaFnameField;
            }
            set
            {
                this.gaFnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaGroupCode
        {
            get
            {
                return this.gaGroupCodeField;
            }
            set
            {
                this.gaGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaLname
        {
            get
            {
                return this.gaLnameField;
            }
            set
            {
                this.gaLnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaMname
        {
            get
            {
                return this.gaMnameField;
            }
            set
            {
                this.gaMnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaOfficeCode
        {
            get
            {
                return this.gaOfficeCodeField;
            }
            set
            {
                this.gaOfficeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gaSectionCode
        {
            get
            {
                return this.gaSectionCodeField;
            }
            set
            {
                this.gaSectionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> houseCount
        {
            get
            {
                return this.houseCountField;
            }
            set
            {
                this.houseCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> inPossessionTime
        {
            get
            {
                return this.inPossessionTimeField;
            }
            set
            {
                this.inPossessionTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string infractionFlag
        {
            get
            {
                return this.infractionFlagField;
            }
            set
            {
                this.infractionFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string initialFeeSchedule
        {
            get
            {
                return this.initialFeeScheduleField;
            }
            set
            {
                this.initialFeeScheduleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectorDept
        {
            get
            {
                return this.inspectorDeptField;
            }
            set
            {
                this.inspectorDeptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectorId
        {
            get
            {
                return this.inspectorIdField;
            }
            set
            {
                this.inspectorIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectorName
        {
            get
            {
                return this.inspectorNameField;
            }
            set
            {
                this.inspectorNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string misdemeanorFlag
        {
            get
            {
                return this.misdemeanorFlagField;
            }
            set
            {
                this.misdemeanorFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string offnWitnessedFlag
        {
            get
            {
                return this.offnWitnessedFlagField;
            }
            set
            {
                this.offnWitnessedFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> overallApplicationTime
        {
            get
            {
                return this.overallApplicationTimeField;
            }
            set
            {
                this.overallApplicationTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double percentComplete
        {
            get
            {
                return this.percentCompleteField;
            }
            set
            {
                this.percentCompleteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> pmScheduleSeq
        {
            get
            {
                return this.pmScheduleSeqField;
            }
            set
            {
                this.pmScheduleSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string priority
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
        public string prodUnitType
        {
            get
            {
                return this.prodUnitTypeField;
            }
            set
            {
                this.prodUnitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string publicOwned
        {
            get
            {
                return this.publicOwnedField;
            }
            set
            {
                this.publicOwnedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string referenceType
        {
            get
            {
                return this.referenceTypeField;
            }
            set
            {
                this.referenceTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reportedChannel
        {
            get
            {
                return this.reportedChannelField;
            }
            set
            {
                this.reportedChannelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> scheduledDate
        {
            get
            {
                return this.scheduledDateField;
            }
            set
            {
                this.scheduledDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string scheduledTime
        {
            get
            {
                return this.scheduledTimeField;
            }
            set
            {
                this.scheduledTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serverity
        {
            get
            {
                return this.serverityField;
            }
            set
            {
                this.serverityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string shortNotes
        {
            get
            {
                return this.shortNotesField;
            }
            set
            {
                this.shortNotesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string statusReason
        {
            get
            {
                return this.statusReasonField;
            }
            set
            {
                this.statusReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalFee
        {
            get
            {
                return this.totalFeeField;
            }
            set
            {
                this.totalFeeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> totalJobCost
        {
            get
            {
                return this.totalJobCostField;
            }
            set
            {
                this.totalJobCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalPay
        {
            get
            {
                return this.totalPayField;
            }
            set
            {
                this.totalPayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> trackStartDate
        {
            get
            {
                return this.trackStartDateField;
            }
            set
            {
                this.trackStartDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> undistributedCost
        {
            get
            {
                return this.undistributedCostField;
            }
            set
            {
                this.undistributedCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> valuationExtraAmount
        {
            get
            {
                return this.valuationExtraAmountField;
            }
            set
            {
                this.valuationExtraAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> valuationMultiplier
        {
            get
            {
                return this.valuationMultiplierField;
            }
            set
            {
                this.valuationMultiplierField = value;
            }
        }
    }
}
