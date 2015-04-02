#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: StrucEstaMasterModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: StrucEstaMasterModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureEstablishmentModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class StrucEstaMasterModel : LanguageModel
    {
        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private double bathsField;

        private bool bathsFieldSpecified;

        private double bedsField;

        private bool bedsFieldSpecified;

        private double coordinator_XField;

        private bool coordinator_XFieldSpecified;

        private double coordinator_YField;

        private bool coordinator_YFieldSpecified;

        private System.DateTime dateBuiltField;

        private bool dateBuiltFieldSpecified;

        private string descriptionField;

        private string dispDescriptionField;

        private string dispGroupField;

        private string dispNameField;

        private string dispTypeField;

        private System.DateTime endDateBuiltField;

        private bool endDateBuiltFieldSpecified;

        private System.DateTime endStatusDateField;

        private bool endStatusDateFieldSpecified;

        private double floorField;

        private bool floorFieldSpecified;

        private double floorArea_1stField;

        private bool floorArea_1stFieldSpecified;

        private double frontDimensionField;

        private bool frontDimensionFieldSpecified;

        private string garageField;

        private string groupField;

        private double heightField;

        private bool heightFieldSpecified;

        private string idField;

        private string landUseValueField;

        private string nameField;

        private double numberRoomsField;

        private bool numberRoomsFieldSpecified;

        private double parcelsAreasField;

        private bool parcelsAreasFieldSpecified;

        private double percentEmployeesField;

        private bool percentEmployeesFieldSpecified;

        private double percentResidentialUnitsField;

        private bool percentResidentialUnitsFieldSpecified;

        private int percentStructureField;

        private bool percentStructureFieldSpecified;

        private int percentUsedField;

        private bool percentUsedFieldSpecified;

        private string poolField;

        private double rearDimensionField;

        private bool rearDimensionFieldSpecified;

        private string recordStatusField;

        private string resDescriptionField;

        private string resNameField;

        private string servProvCodeField;

        private double sideDimension1Field;

        private bool sideDimension1FieldSpecified;

        private double sideDimension2Field;

        private bool sideDimension2FieldSpecified;

        private long sourceSeqNumberField;

        private bool sourceSeqNumberFieldSpecified;

        private string statusField;

        private System.DateTime statusDateField;

        private bool statusDateFieldSpecified;

        private long strucEstaSeqField;

        private bool strucEstaSeqFieldSpecified;

        private double totalAreaField;

        private bool totalAreaFieldSpecified;

        private double totalFloorAreaField;

        private bool totalFloorAreaFieldSpecified;

        private double totalFloorsField;

        private bool totalFloorsFieldSpecified;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime auditDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool auditDateSpecified
        {
            get
            {
                return this.auditDateFieldSpecified;
            }
            set
            {
                this.auditDateFieldSpecified = value;
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
        public double baths
        {
            get
            {
                return this.bathsField;
            }
            set
            {
                this.bathsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool bathsSpecified
        {
            get
            {
                return this.bathsFieldSpecified;
            }
            set
            {
                this.bathsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double beds
        {
            get
            {
                return this.bedsField;
            }
            set
            {
                this.bedsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool bedsSpecified
        {
            get
            {
                return this.bedsFieldSpecified;
            }
            set
            {
                this.bedsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double coordinator_X
        {
            get
            {
                return this.coordinator_XField;
            }
            set
            {
                this.coordinator_XField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool coordinator_XSpecified
        {
            get
            {
                return this.coordinator_XFieldSpecified;
            }
            set
            {
                this.coordinator_XFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double coordinator_Y
        {
            get
            {
                return this.coordinator_YField;
            }
            set
            {
                this.coordinator_YField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool coordinator_YSpecified
        {
            get
            {
                return this.coordinator_YFieldSpecified;
            }
            set
            {
                this.coordinator_YFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dateBuilt
        {
            get
            {
                return this.dateBuiltField;
            }
            set
            {
                this.dateBuiltField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateBuiltSpecified
        {
            get
            {
                return this.dateBuiltFieldSpecified;
            }
            set
            {
                this.dateBuiltFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispDescription
        {
            get
            {
                return this.dispDescriptionField;
            }
            set
            {
                this.dispDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispGroup
        {
            get
            {
                return this.dispGroupField;
            }
            set
            {
                this.dispGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispName
        {
            get
            {
                return this.dispNameField;
            }
            set
            {
                this.dispNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispType
        {
            get
            {
                return this.dispTypeField;
            }
            set
            {
                this.dispTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endDateBuilt
        {
            get
            {
                return this.endDateBuiltField;
            }
            set
            {
                this.endDateBuiltField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endDateBuiltSpecified
        {
            get
            {
                return this.endDateBuiltFieldSpecified;
            }
            set
            {
                this.endDateBuiltFieldSpecified = value;
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endStatusDateSpecified
        {
            get
            {
                return this.endStatusDateFieldSpecified;
            }
            set
            {
                this.endStatusDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double floor
        {
            get
            {
                return this.floorField;
            }
            set
            {
                this.floorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool floorSpecified
        {
            get
            {
                return this.floorFieldSpecified;
            }
            set
            {
                this.floorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double floorArea_1st
        {
            get
            {
                return this.floorArea_1stField;
            }
            set
            {
                this.floorArea_1stField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool floorArea_1stSpecified
        {
            get
            {
                return this.floorArea_1stFieldSpecified;
            }
            set
            {
                this.floorArea_1stFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double frontDimension
        {
            get
            {
                return this.frontDimensionField;
            }
            set
            {
                this.frontDimensionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool frontDimensionSpecified
        {
            get
            {
                return this.frontDimensionFieldSpecified;
            }
            set
            {
                this.frontDimensionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string garage
        {
            get
            {
                return this.garageField;
            }
            set
            {
                this.garageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string landUseValue
        {
            get
            {
                return this.landUseValueField;
            }
            set
            {
                this.landUseValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double numberRooms
        {
            get
            {
                return this.numberRoomsField;
            }
            set
            {
                this.numberRoomsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool numberRoomsSpecified
        {
            get
            {
                return this.numberRoomsFieldSpecified;
            }
            set
            {
                this.numberRoomsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double parcelsAreas
        {
            get
            {
                return this.parcelsAreasField;
            }
            set
            {
                this.parcelsAreasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool parcelsAreasSpecified
        {
            get
            {
                return this.parcelsAreasFieldSpecified;
            }
            set
            {
                this.parcelsAreasFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double percentEmployees
        {
            get
            {
                return this.percentEmployeesField;
            }
            set
            {
                this.percentEmployeesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool percentEmployeesSpecified
        {
            get
            {
                return this.percentEmployeesFieldSpecified;
            }
            set
            {
                this.percentEmployeesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double percentResidentialUnits
        {
            get
            {
                return this.percentResidentialUnitsField;
            }
            set
            {
                this.percentResidentialUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool percentResidentialUnitsSpecified
        {
            get
            {
                return this.percentResidentialUnitsFieldSpecified;
            }
            set
            {
                this.percentResidentialUnitsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int percentStructure
        {
            get
            {
                return this.percentStructureField;
            }
            set
            {
                this.percentStructureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool percentStructureSpecified
        {
            get
            {
                return this.percentStructureFieldSpecified;
            }
            set
            {
                this.percentStructureFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int percentUsed
        {
            get
            {
                return this.percentUsedField;
            }
            set
            {
                this.percentUsedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool percentUsedSpecified
        {
            get
            {
                return this.percentUsedFieldSpecified;
            }
            set
            {
                this.percentUsedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pool
        {
            get
            {
                return this.poolField;
            }
            set
            {
                this.poolField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double rearDimension
        {
            get
            {
                return this.rearDimensionField;
            }
            set
            {
                this.rearDimensionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool rearDimensionSpecified
        {
            get
            {
                return this.rearDimensionFieldSpecified;
            }
            set
            {
                this.rearDimensionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recordStatus
        {
            get
            {
                return this.recordStatusField;
            }
            set
            {
                this.recordStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resDescription
        {
            get
            {
                return this.resDescriptionField;
            }
            set
            {
                this.resDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resName
        {
            get
            {
                return this.resNameField;
            }
            set
            {
                this.resNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servProvCode
        {
            get
            {
                return this.servProvCodeField;
            }
            set
            {
                this.servProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double sideDimension1
        {
            get
            {
                return this.sideDimension1Field;
            }
            set
            {
                this.sideDimension1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sideDimension1Specified
        {
            get
            {
                return this.sideDimension1FieldSpecified;
            }
            set
            {
                this.sideDimension1FieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double sideDimension2
        {
            get
            {
                return this.sideDimension2Field;
            }
            set
            {
                this.sideDimension2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sideDimension2Specified
        {
            get
            {
                return this.sideDimension2FieldSpecified;
            }
            set
            {
                this.sideDimension2FieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long sourceSeqNumber
        {
            get
            {
                return this.sourceSeqNumberField;
            }
            set
            {
                this.sourceSeqNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sourceSeqNumberSpecified
        {
            get
            {
                return this.sourceSeqNumberFieldSpecified;
            }
            set
            {
                this.sourceSeqNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime statusDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusDateSpecified
        {
            get
            {
                return this.statusDateFieldSpecified;
            }
            set
            {
                this.statusDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long strucEstaSeq
        {
            get
            {
                return this.strucEstaSeqField;
            }
            set
            {
                this.strucEstaSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool strucEstaSeqSpecified
        {
            get
            {
                return this.strucEstaSeqFieldSpecified;
            }
            set
            {
                this.strucEstaSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalArea
        {
            get
            {
                return this.totalAreaField;
            }
            set
            {
                this.totalAreaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalAreaSpecified
        {
            get
            {
                return this.totalAreaFieldSpecified;
            }
            set
            {
                this.totalAreaFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalFloorArea
        {
            get
            {
                return this.totalFloorAreaField;
            }
            set
            {
                this.totalFloorAreaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalFloorAreaSpecified
        {
            get
            {
                return this.totalFloorAreaFieldSpecified;
            }
            set
            {
                this.totalFloorAreaFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalFloors
        {
            get
            {
                return this.totalFloorsField;
            }
            set
            {
                this.totalFloorsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalFloorsSpecified
        {
            get
            {
                return this.totalFloorsFieldSpecified;
            }
            set
            {
                this.totalFloorsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }
}
