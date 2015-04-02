/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SimpleCapModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SimpleCapModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class SimpleCapModel : CapPage4ACAModel
    {
        private PersonModel personModelField;

        private AddressModel addressModelField;

        private AddressModel[] addressModelsField;

        private string delegateUserIdField;

        private string altIDField;

        private string appTypeAliasField;

        private string arabicTradeNameField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string capClassField;

        private CapDetailModel capDetailModelField;

        private CapIDModel capIDField;

        private string capStatusField;

        private CapTypeModel capTypeField;

        private string englishTradeNameField;

        private System.Nullable<System.DateTime> fileDateField;

        private GISObjectModel[] gisObjectsField;

        private string licenseTypeField;

        private string moduleNameField;

        private ParcelInfoModel[] parcelModelsField;

        private string paymentStatusField;      

        private string relatedTradeLicField;

        private string resCapStatusField;

        private string[] serviceNamesField;

        private string specialTextField;

        private string statusGroupCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PersonModel personModel
        {
            get
            {
                return this.personModelField;
            }
            set
            {
                this.personModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AddressModel addressModel
        {
            get
            {
                return this.addressModelField;
            }
            set
            {
                this.addressModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addressModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AddressModel[] addressModels
        {
            get
            {
                return this.addressModelsField;
            }
            set
            {
                this.addressModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string delegateUserId
        {
            get
            {
                return this.delegateUserIdField;
            }
            set
            {
                this.delegateUserIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string altID
        {
            get
            {
                return this.altIDField;
            }
            set
            {
                this.altIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appTypeAlias
        {
            get
            {
                return this.appTypeAliasField;
            }
            set
            {
                this.appTypeAliasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arabicTradeName
        {
            get
            {
                return this.arabicTradeNameField;
            }
            set
            {
                this.arabicTradeNameField = value;
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
        public string capClass
        {
            get
            {
                return this.capClassField;
            }
            set
            {
                this.capClassField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapDetailModel capDetailModel
        {
            get
            {
                return this.capDetailModelField;
            }
            set
            {
                this.capDetailModelField = value;
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
        public string capStatus
        {
            get
            {
                return this.capStatusField;
            }
            set
            {
                this.capStatusField = value;
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
        public string englishTradeName
        {
            get
            {
                return this.englishTradeNameField;
            }
            set
            {
                this.englishTradeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> fileDate
        {
            get
            {
                return this.fileDateField;
            }
            set
            {
                this.fileDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("gisObjects", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GISObjectModel[] gisObjects
        {
            get
            {
                return this.gisObjectsField;
            }
            set
            {
                this.gisObjectsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string licenseType
        {
            get
            {
                return this.licenseTypeField;
            }
            set
            {
                this.licenseTypeField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("parcelModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ParcelInfoModel[] parcelModels
        {
            get
            {
                return this.parcelModelsField;
            }
            set
            {
                this.parcelModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentStatus
        {
            get
            {
                return this.paymentStatusField;
            }
            set
            {
                this.paymentStatusField = value;
            }
        }       

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string relatedTradeLic
        {
            get
            {
                return this.relatedTradeLicField;
            }
            set
            {
                this.relatedTradeLicField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resCapStatus
        {
            get
            {
                return this.resCapStatusField;
            }
            set
            {
                this.resCapStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("serviceNames", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] serviceNames
        {
            get
            {
                return this.serviceNamesField;
            }
            set
            {
                this.serviceNamesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string specialText
        {
            get
            {
                return this.specialTextField;
            }
            set
            {
                this.specialTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string statusGroupCode
        {
            get
            {
                return this.statusGroupCodeField;
            }
            set
            {
                this.statusGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int relatedRecordsCount
        {
            get;

            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PersonModel delegatePersonModel
        {
            get;
            set;
        }
    }
}
