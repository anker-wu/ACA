#region Header

/**
 *  Accela Citizen Access
 *  File: EnumConstant.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *   The Enum collections
 *
 *  Notes:
 * $Id: EnumConstant.cs 134054 2009-06-10 07:03:54Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Contact UI process type
    /// </summary>
    public enum ContactProcessType
    {
        /// <summary>
        /// The none process
        /// </summary>
        None = 0,

        /// <summary>
        /// The add process
        /// </summary>
        Add = 1,

        /// <summary>
        /// The edit process
        /// </summary>
        Edit = 2,

        /// <summary>
        /// The lookup
        /// </summary>
        Lookup = 3,

        /// <summary>
        /// The select contact from account
        /// </summary>
        SelectContactFromAccount = 4,

        /// <summary>
        /// The edit contact type
        /// </summary>
        EditContactType = 5,

        /// <summary>
        /// Select contact from other agencies
        /// </summary>
        SelectContactFromOtherAgencies = 6,

        /// <summary>
        /// Select close match people.
        /// </summary>
        SelectContactFromCloseMatch = 7
    }

    /// <summary>
    /// Contact Address Process Type
    /// </summary>
    public enum ContactAddressProcessType
    {
        /// <summary>
        /// The none process
        /// </summary>
        None = 0,

        /// <summary>
        /// The add
        /// </summary>
        Add = 1,

        /// <summary>
        /// The edit
        /// </summary>
        Edit = 2,

        /// <summary>
        /// The deactive
        /// </summary>
        Deactive = 3,

        /// <summary>
        /// The external validation
        /// </summary>
        ExternalValidation = 4,

        /// <summary>
        /// The delete
        /// </summary>
        Delete = 5
    }

    /// <summary>
    /// Contact look up page step enumeration
    /// </summary>
    public enum ContactLookUpStep
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The search form
        /// </summary>
        SearchForm = 1,

        /// <summary>
        /// The contact list
        /// </summary>
        ContactList = 2,

        /// <summary>
        /// The contact address look up
        /// </summary>
        CotactAddressLookUp = 3,
    }

    /// <summary>
    /// The action type
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Add successful
        /// </summary>
        AddSuccess = 0,

        /// <summary>
        /// Update successful
        /// </summary>
        UpdateSuccess = 1,

        /// <summary>
        /// Delete successful
        /// </summary>
        DeleteSuccess = 2,

        /// <summary>
        /// Deactivated action
        /// </summary>
        Deactivated = 3,

        /// <summary>
        /// The Duplicate
        /// </summary>
        Duplicate = 4,

        /// <summary>
        /// Duplicate to self
        /// </summary>
        DuplicateToSelf = 5,

        /// <summary>
        /// Add failed
        /// </summary>
        AddFailed = 6,

        /// <summary>
        /// Update failed
        /// </summary>
        UpdateFailed = 7,

        /// <summary>
        /// Delete failed
        /// </summary>
        DeleteFailed = 8,

        /// <summary>
        /// Set As Primary
        /// </summary>
        SetAsPrimary = 9
    }

    /// <summary>
    /// Shopping Cart Transaction Type.
    /// </summary>
    public enum ShoppingCartTransactionType
    {
        /// <summary>
        /// Transaction Per Cart
        /// </summary>
        TransactionPerCart = 0,

        /// <summary>
        /// Transaction Per Record
        /// </summary>
        TransactionPerRecord = 1,
    }

    /// <summary>
    /// All supported control type in ACA. 
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// text box
        /// </summary>
        Text,

        /// <summary>
        /// number box 
        /// </summary>
        Number,

        /// <summary>
        /// date control type
        /// </summary>
        Date,

        /// <summary>
        /// dropdown list
        /// </summary>
        DropdownList,

        /// <summary>
        /// radio button
        /// </summary>
        Radio,

        /// <summary>
        /// separator line
        /// </summary>
        Line,

        /// <summary>
        /// Generic Template Table
        /// </summary>
        Table
    }

    /// <summary>
    /// Parcel Genealogy Action Type.
    /// </summary>
    public enum ParcelGenealogyActionType
    {
        /// <summary>
        /// the merge type
        /// </summary>
        Merge,

        /// <summary>
        /// the split type
        /// </summary>
        Split
    }

    /// <summary>
    /// Issue license return code list.
    /// </summary>
    public enum IssueLicenseReturnCode
    {
        /// <summary>
        /// -2---The license is invalid.
        /// </summary>
        LIC_INVALID = -2,

        /// <summary>
        /// -1---Issue license fail.
        /// </summary>
        ISSUE_LIC_FAILED = -1,

        /// <summary>
        /// 0---License doesn't exist in DB.
        /// </summary>
        LIC_INEXISTENCE = 0,

        /// <summary>
        /// 1---The account already has the same license.  
        /// </summary>
        LICENSE_CONNECTED_THE_ACCOUNT = 1,

        /// <summary>
        /// 2---Issue license successfully, need to waiting for approval.
        /// </summary>
        ISSUE_SUCCESS = 2,

        /// <summary>
        /// 3---Issue license with auto approved successfully.
        /// </summary>
        ISSUE_SUCCESS_AUTO_APPROVED = 3
    }

    /// <summary>
    /// the Cap Detail section type.
    /// </summary>
    public enum CapDetailSectionType
    {
        /// <summary>
        /// the applicant section
        /// </summary>
        APPLICANT = 0,

        /// <summary>
        /// the work location section
        /// </summary>
        WORKLOCATION = 1,

        /// <summary>
        /// the license profession section
        /// </summary>
        LICENSED_PROFESSIONAL = 2,

        /// <summary>
        /// the project description section 
        /// </summary>
        PROJECT_DESCRIPTION = 3,

        /// <summary>
        /// the owner section
        /// </summary>
        OWNER = 4,

        /// <summary>
        /// the related contacts section
        /// </summary>
        RELATED_CONTACTS = 5,

        /// <summary>
        /// the additional information section
        /// </summary>
        ADDITIONAL_INFORMATION = 6,

        /// <summary>
        /// the application information section
        /// </summary>
        APPLICATION_INFORMATION = 7,

        /// <summary>
        /// the application information table section
        /// </summary>
        APPLICATION_INFORMATION_TABLE = 8,

        /// <summary>
        /// the parcel section
        /// </summary>
        PARCEL_INFORMATION = 9,

        /// <summary>
        /// the fee section
        /// </summary>
        FEE = 10,

        /// <summary>
        /// the inspections section
        /// </summary>
        INSPECTIONS = 11,

        /// <summary>
        /// the processing status section
        /// </summary>
        PROCESSING_STATUS = 12,

        /// <summary>
        /// the attachment section.
        /// </summary>
        ATTACHMENTS = 13,

        /// <summary>
        /// the related records section.
        /// </summary>
        RELATED_RECORDS = 14,

        /// <summary>
        /// the education section.
        /// </summary>
        EDUCATION = 15,

        /// <summary>
        /// the continue education section.
        /// </summary>
        CONTINUING_EDUCATION = 16,

        /// <summary>
        /// the examination section.
        /// </summary>
        EXAMINATION = 17,

        /// <summary>
        /// the Valuation Calculator section.
        /// </summary>
        VALUATION_CALCULATOR = 18,

        /// <summary>
        /// the trust account section.
        /// </summary>
        TRUST_ACCOUNT = 19,

        /// <summary>
        /// the assets section.
        /// </summary>
        ASSETS = 20
    }

    /// <summary>
    /// Uploading exam return code.
    /// </summary>
    public enum UploadExamReturnCode
    {
        /// <summary>
        /// Return code: Upload exam by CSV is successful only, if auto-update option is disabled.
        /// </summary>
        UPLOAD_SUCCESSFUL = 0,

        /// <summary>
        /// Return code: Upload and update exam automatically by CSV is successful both, if auto-update exam option is enabled.
        /// </summary>
        SUCCESSFUL = 1,

        /// <summary>
        /// Upload exam by CSV is failed, get empty list by CSV file. the file failed to update because Record IDs in the file cannot be identified.
        /// </summary>
        EMPTY_LIST = 2,

        /// <summary>
        /// Return code: Upload exam by CSV is failed, The configuration error. Such as EXAM_CSV_FORMAT configuration within standard choice.
        /// </summary>
        CONFIG_ERROR = 3,

        /// <summary>
        /// Return code: Upload exam by CSV is successful, but at least one record updating is failed.
        /// </summary>
        AUTO_UPDATE_EXAM_FAILED = 4
    }

    /// <summary>
    /// Field alignment.
    /// </summary>
    public enum FieldAlignment
    {
        /// <summary>
        /// from left to right.
        /// </summary>
        LTR = 0,

        /// <summary>
        /// from right to left.
        /// </summary>
        RTL = 1
    }

    /// <summary>
    /// Field types for ASI / ASIT /Generic Template fields.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// base text box.
        /// </summary>
        HTML_TEXTBOX = 1,

        /// <summary>
        /// text box for date control.
        /// </summary>
        HTML_TEXTBOX_OF_DATE,

        /// <summary>
        /// radio button control.
        /// </summary>
        HTML_RADIOBOX,

        /// <summary>
        ///  text box for number control.
        /// </summary>
        HTML_TEXTBOX_OF_NUMBER,

        /// <summary>
        ///  text box for select box.
        /// </summary>
        HTML_SELECTBOX,

        /// <summary>
        ///  text box for  text area.
        /// </summary>
        HTML_TEXTAREABOX,

        /// <summary>
        ///  text box for time control.
        /// </summary>
        HTML_TEXTBOX_OF_TIME,

        /// <summary>
        ///  text box for currency control.
        /// </summary>
        HTML_TEXTBOX_OF_CURRENCY,

        /// <summary>
        /// check box control.
        /// </summary>
        HTML_CHECKBOX
    }

    /// <summary>
    /// score grading style.
    /// </summary>
    public enum GradingStyle
    {
        /// <summary>
        /// pass or fail score style.
        /// </summary>
        Passfail,

        /// <summary>
        /// percentage score style.
        /// </summary>
        Percentage,

        /// <summary>
        /// score number style.
        /// </summary>
        Score,

        /// <summary>
        /// none score style.
        /// </summary>
        None,

        /// <summary>
        /// N/A score style.
        /// </summary>
        NA
    }

    /// <summary>
    /// Parameter type
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// ParameterType is Cap Type
        /// </summary>
        CapType,

        /// <summary>
        /// ParameterType is Department
        /// </summary>
        Department,

        /// <summary>
        /// ParameterType is Date
        /// </summary>
        Date,

        /// <summary>
        /// ParameterType is number
        /// </summary>
        Number,

        /// <summary>
        /// ParameterType is string
        /// </summary>
        String,

        /// <summary>
        /// ParameterType is session variable.
        /// </summary>
        SessionVariable,

        /// <summary>
        /// ParameterType is variable..
        /// </summary>
        Variable,

        /// <summary>
        /// ParameterType is Dropdown
        /// </summary>
        Dropdown
    }

    /// <summary>
    /// Search Types
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// Search type is address.
        /// </summary>
        Address = 0,

        /// <summary>
        /// Search type is parcel.
        /// </summary>
        Parcel = 1,

        /// <summary>
        /// Search type is owner.
        /// </summary>
        Owner = 2,

        /// <summary>
        /// Search type is permit.
        /// </summary>
        Permit = 3,

        /// <summary>
        /// Search type is license.
        /// </summary>
        License = 4
    }

    /// <summary>
    /// Session Value Keys, the first letters are lowercase to keep same as V360.
    /// Below keys are not supported by ACA, but supported by AA:
    /// invoiceID/setID/providerNumber/assetNumber/department/portletID.
    /// </summary>
    public enum SessionContextValue
    {
        /// <summary>
        /// the cap id1.
        /// </summary>
        capID1,

        /// <summary>
        /// the cap id2.
        /// </summary>
        capID2,

        /// <summary>
        /// the cap id3.
        /// </summary>
        capID3,

        /// <summary>
        /// first Name.
        /// </summary>
        firstName,

        /// <summary>
        /// middle Name.
        /// </summary>
        middleName,

        /// <summary>
        /// last Name.
        /// </summary>
        lastName,

        /// <summary>
        /// user id...
        /// </summary>
        userID,

        /// <summary>
        /// user Full Name.
        /// </summary>
        userFullName,

        /// <summary>
        /// module name.
        /// </summary>
        module,

        /// <summary>
        /// user Group.
        /// </summary>
        userGroup,

        /// <summary>
        /// Alt ID......
        /// </summary>
        altID,

        /// <summary>
        /// cap ID.....
        /// </summary>
        capID,

        /// <summary>
        /// The agency code.
        /// </summary>
        servProvCode,

        /// <summary>
        /// today date.
        /// </summary>
        today,

        /// <summary>
        /// parcel ID.
        /// </summary>
        parcelID,

        /// <summary>
        /// collection ID.
        /// </summary>
        collectionID,

        /// <summary>
        /// state License Number.
        /// </summary>
        stateLicNum,

        /// <summary>
        /// public user ID.
        /// </summary>
        publicUserID,

        /// <summary>
        /// language info.
        /// </summary>
        language,

        /// <summary>
        /// trust account receipt id
        /// </summary>
        trustAccountReceptID,

        /// <summary>
        /// the fee transaction id in Cashier, trust account.
        /// </summary>
        transactionID
    }

    /// <summary>
    /// Session Variable Type
    /// </summary>
    public enum SessionVaraibleType
    {
        /// <summary>
        /// Cap Model.
        /// </summary>
        CapModel,

        /// <summary>
        /// Module Name.
        /// </summary>
        ModuleName,

        /// <summary>
        /// Collection ID.
        /// </summary>
        CollectionID,

        /// <summary>
        /// Trust account receipt ID
        /// </summary>
        TrustAccountReceptID,

        /// <summary>
        /// payment transaction ID
        /// </summary>
        TransactionID
    }

    /// <summary>
    /// Template Type
    /// </summary>
    public enum TemplateType
    {
        /// <summary>
        /// cap  address.
        /// </summary>
        CAP_ADDRESS = 0,

        /// <summary>
        /// cap parcel.
        /// </summary>
        CAP_PARCEL = 1,

        /// <summary>
        /// cap owner.
        /// </summary>
        CAP_OWNER = 2
    }

    /// <summary>
    /// People Type
    /// </summary>
    public enum PeopleType
    {
        /// <summary>
        /// The Contact.
        /// </summary>
        Contact = 0,

        /// <summary>
        /// The Professional.
        /// </summary>
        Professional = 1
    }

    /// <summary>
    /// User role type
    /// </summary>
    public enum UserRoleType
    {
        /// <summary>
        ///  All ACA Users
        /// </summary>
        AllACAUsers = 1,

        /// <summary>
        /// CAP Creator
        /// </summary>
        CAPCreator = 2,

        /// <summary>
        /// CAP Creator and Associated Licensed Professionals.
        /// </summary>
        CAPCreatorAndAssociatedLicensedProfessionals = 3,
    }

    /// <summary>
    /// Auto Fill Type.
    /// </summary>
    public enum AutoFillType
    {
        /// <summary>
        /// Default Type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Auto Fill City.
        /// </summary>
        City = 1,

        /// <summary>
        /// Auto Fill State.
        /// </summary>
        State = 2,
    }

    /// <summary>
    /// type for search refEducation result.
    /// </summary>
    public enum RefResultType
    {
        /// <summary>
        /// the search refResult table type is provider.
        /// </summary>
        Provider,

        /// <summary>
        /// the search refResult table type is education.
        /// </summary>
        Education,

        /// <summary>
        /// the search refResult table type is continuing education.
        /// </summary>
        ContEdu,

        /// <summary>
        /// the search refResult table type is examination.
        /// </summary>
        Examination,

        /// <summary>
        /// the search result table type is licensee.
        /// </summary>
        Licensee,

        /// <summary>
        /// the search result table type is food facility.
        /// </summary>
        FoodFacility,

        /// <summary>
        /// the search result table type is certified business.
        /// </summary>
        CertifiedBusiness
    }

    /// <summary>
    /// type for Control Type.
    /// </summary>
    public enum ControllerType
    {
        /// <summary>
        /// The Controller type when create cap type.
        /// </summary>
        CAPTYPEFILTER,

        /// <summary>
        /// The Controller type when search cap type.
        /// </summary>
        CAPSEARCHFILTER,

        /// <summary>
        /// The Controller type when setting LicenseVerification Section.
        /// </summary>
        LICENSEVERIFICATION,

        /// <summary>
        /// The Controller type for amendment button.
        /// </summary>
        COMBINEBUTTIONSETTING
    }

    /// <summary>
    /// type for Control Type.
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// The Entity Type is License Type.
        /// </summary>
        LICENSETYPE,

        /// <summary>
        /// The General Roles.
        /// </summary>
        GENERAL,

        /// <summary>
        /// The Section Type.
        /// </summary>
        SECTIONTYPE,

        /// <summary>
        /// Create Amendment.
        /// </summary>
        CreateAmendment
    }

    /// <summary>
    /// Auto Fill Position.
    /// </summary>
    public enum AutoFillPosition
    {
        /// <summary>
        /// Default Position.
        /// </summary>
        None = 0,

        /// <summary>
        /// Address Section city control
        /// </summary>
        LookUpByAddressCity = 6001101,

        /// <summary>
        /// Address Section state control
        /// </summary>
        LookUpByAddressState = 6001102,

        /// <summary>
        /// Owner Section state control
        /// </summary>
        LookUpByOwnerState = 6001301,

        /// <summary>
        /// Owner Section city control
        /// </summary>
        LookUpByOwnerCity = 6001302,

        /// <summary>
        /// Registration Contact Information city control.
        /// </summary>
        RegistrationContactInfoCity = 6005501,

        /// <summary>
        /// Registration Contact Information state control.
        /// </summary>
        RegistrationContactInfoState = 6005502,

        /// <summary>
        /// Payment with Credit Cart city control.
        /// </summary>
        CreditCardPaymentCity = 6006601,

        /// <summary>
        /// Payment with Credit Cart state control.
        /// </summary>
        CreditCardPaymentState = 6006602,

        /// <summary>
        /// Payment with Credit Cart city control.
        /// </summary>
        LookUpEducationCity = 6007501,

        /// <summary>
        /// Payment with Credit Cart city control.
        /// </summary>
        LookUpEducationState = 6007502,

        /// <summary>
        /// Cap Home General Search city control.
        /// </summary>
        GeneralSearchCity = 6000701,

        /// <summary>
        /// Cap Home General Search state control.
        /// </summary>
        GeneralSearchState = 6000702,

        /// <summary>
        /// Search by address city control.
        /// </summary>
        SearchByAddressCity = 6001001,

        /// <summary>
        /// Search by address state control.
        /// </summary>
        SearchByAddressState = 6001002,

        /// <summary>
        /// Search by address city control.
        /// </summary>
        SearchByContactCity = 6006501,

        /// <summary>
        /// Search by address state control.
        /// </summary>
        SearchByContactState = 6006502,

        /// <summary>
        /// Search By Contact Driver License State control.
        /// </summary>
        SearchByContactDriverLicenseState = 6006503,

        /// <summary>
        /// Search By Contact Birth place State control.
        /// </summary>
        SearchByContactBirthplaceState = 6006504,

        /// <summary>
        /// Search By Contact Birth place City
        /// </summary>
        SearchByContactBirthplaceCity = 6006505,

        /// <summary>
        /// Spear Form Address city control.
        /// </summary>
        SpearFormAddressCity = 6000101,

        /// <summary>
        /// SpearForm Address state control.
        /// </summary>
        SpearFormAddressState = 6000102,

        /// <summary>
        /// SpearForm Owner city control.
        /// </summary>
        SpearFormOwnerCity = 6000301,

        /// <summary>
        /// SpearForm Owner state control.
        /// </summary>
        SpearFormOwnerState = 6000302,

        /// <summary>
        /// SpearForm License city control.
        /// </summary>
        SpearFormLicenseCity = 6000401,

        /// <summary>
        /// SpearForm License state control.
        /// </summary>
        SpearFormLicenseState = 6000402,

        /// <summary>
        /// SpearForm Contacts city control.
        /// </summary>
        SpearFormContactsCity = 6000501,

        /// <summary>
        /// SpearFrom Contacts state control.
        /// </summary>
        SpearFormContactsState = 6000502,

        /// <summary>
        /// SpearFrom Contacts Birth place state control.
        /// </summary>
        SpearFormContactBirthplaceState = 6000503,

        /// <summary>
        /// SpearFrom Contacts Birth place city control.
        /// </summary>
        SpearFormContactBirthplaceCity = 6000504,

        /// <summary>
        /// SpearFrom Contacts Driver License State control.
        /// </summary>
        SpearFormContactDriverLicenseState = 6000505,

        /// <summary>
        /// SpearForm Education city control.
        /// </summary>
        SpearFormEducationCity = 6001801,

        /// <summary>
        /// SpearForm Education State control.
        /// </summary>
        SpearFormEducationState = 6001802,

        /// <summary>
        /// SpearForm Continuing Education city control.
        /// </summary>
        SpearFormContEducationCity = 6008401,

        /// <summary>
        /// SpearForm continuing Education State control.
        /// </summary>
        SpearFormContEducationState = 6008402,

        /// <summary>
        /// Auto city in Examination edit form on Spear Form page.
        /// </summary>
        SpearFormExaminationCity = 6001601,

        /// <summary>
        /// Auto state in Examination edit form on Spear Form page.
        /// </summary>
        SpearFormExaminationState = 6001602,

        /// <summary>
        /// Payment with BankAccount city control.
        /// </summary>
        BankAccountPaymentCity = 6007901,

        /// <summary>
        /// Payment with BankAccount state control.
        /// </summary>
        BankAccountPaymentState = 6007902,

        /// <summary>
        /// Look up APO by Licensee
        /// </summary>
        LookUpAPOByLicenseeCity = 6009401,

        /// <summary>
        /// Look up APO by Licensee
        /// </summary>
        LookUpAPOByLicenseeState = 6009402,
    }

    /// <summary>
    /// enumeration for contact type for license.
    /// </summary>
    public enum ContactType4License
    {
        /// <summary>
        /// Individual type
        /// </summary>
        Individual,

        /// <summary>
        /// organization type
        /// </summary>
        Organization
    }

    /// <summary>
    /// enumeration for message type.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Error message
        /// </summary>
        Error = 1,

        /// <summary>
        /// Common Information
        /// </summary>
        Notice,

        /// <summary>
        /// Successful message
        /// </summary>
        Success
    }

    /// <summary>
    /// enumeration for message separation type.
    /// </summary>
    public enum MessageSeperationType
    {
        /// <summary>
        /// only show Top
        /// </summary>
        Top = 1,

        /// <summary>
        /// only show bottom
        /// </summary>
        Bottom,

        /// <summary>
        /// show both top and bottom 
        /// </summary>
        Both,

        /// <summary>
        /// show no separation.
        /// </summary>
        NoOne
    }

    /// <summary>
    /// Access key type
    /// </summary>
    public enum AccessKeyType
    {
        /// <summary>
        /// the type of help key
        /// </summary>
        Help,

        /// <summary>
        /// the type of "skip navigation" key
        /// </summary>
        SkipNavigation,

        /// <summary>
        /// the type of "skip toolbar" key
        /// </summary>
        SkipToolBar,

        /// <summary>
        /// the type of "home page" key
        /// </summary>
        HomePage,

        /// <summary>
        /// the type of "accessibility setup" key
        /// </summary>
        AccessibilitySetup,

        /// <summary>
        /// the type of "submit form" key
        /// </summary>
        SubmitForm,

        /// <summary>
        /// the type of "validation results" key
        /// </summary>
        ValidationResults
    }

    /// <summary>
    /// Represents the HTML scope attribute for classes that represent header cells in a table.
    /// </summary>
    public enum Scope
    {
        /// <summary>
        /// The object that represents a header cell of a table is rendered with the scope attribute set to "Row".
        /// </summary>
        row,

        /// <summary>
        /// The object that represents a header cell of a table is rendered with the scope attribute set to "Column".
        /// </summary>
        col
    }

    /// <summary>
    /// Auto-fill select all options.
    /// </summary>
    public enum SelectAllOptions
    {
        /// <summary>
        /// select all contacts
        /// </summary>
        SelectAllContacts,

        /// <summary>
        /// select all Licensees
        /// </summary>
        SelectAllLicensees
    }

    /// <summary>
    /// The Proxy User Page Type.
    /// </summary>
    public enum ProxyUserPageType
    {
        /// <summary>
        /// The Create
        /// </summary>
        Create,

        /// <summary>
        /// The Edit
        /// </summary>
        Edit,

        /// <summary>
        /// The View
        /// </summary>
        View
    }

    /// <summary>
    /// Proxy User Role Type.
    /// </summary>
    public enum ProxyPermissionType
    {
        /// <summary>
        /// The proxy permission type for view record.
        /// </summary>
        VIEW_RECORD,

        /// <summary>
        /// The proxy permission type for create application.
        /// </summary>
        CREATE_APPLICATION,

        /// <summary>
        /// The proxy permission type for renew record.
        /// </summary>
        RENEW_RECORD,

        /// <summary>
        /// The proxy permission type for manage inspection.
        /// </summary>
        MANAGE_INSPECTIONS,

        /// <summary>
        /// The proxy permission type for manage document.
        /// </summary>
        MANAGE_DOCUMENTS,

        /// <summary>
        /// The proxy permission type for make payment.
        /// </summary>
        MAKE_PAYMENTS,

        /// <summary>
        /// The proxy permission type for amendment.
        /// </summary>
        AMENDMENT
    }

    /// <summary>
    /// Document actions
    /// </summary>
    public enum DocumentActions
    {
        /// <summary>
        /// The action for view
        /// </summary>
        View,

        /// <summary>
        /// The action for download
        /// </summary>
        Download,

        /// <summary>
        /// The action for upload
        /// </summary>
        Upload,

        /// <summary>
        /// The action for resubmit
        /// </summary>
        Resubmit,

        /// <summary>
        /// The action for delete
        /// </summary>
        Delete
    }

    /// <summary>
    /// ACA user type enumeration, contain Anonymous, Registered, LicenseProfessional.
    /// </summary>
    public enum ACAUserType
    {
        /// <summary>
        /// The anonymous user
        /// </summary>
        Anonymous,

        /// <summary>
        /// The registered user
        /// </summary>
        Registered,

        /// <summary>
        /// The license professional user
        /// </summary>
        LicenseProfessional,

        /// <summary>
        /// All user
        /// </summary>
        AllUser,

        /// <summary>
        /// The authorized agent
        /// </summary>
        AuthorizedAgent,

        /// <summary>
        /// The authorized agent clerk
        /// </summary>
        AuthorizedAgentClerk
    }

    /// <summary>
    /// Generic Template Type.
    /// </summary>
    public enum GenericTemplateEntityType
    {
        /// <summary>
        /// The default entity.
        /// </summary>
        DefaultEntity = 0,

        /// <summary>
        /// The reference condition type definition.
        /// </summary>
        RefConditionTypeDefinition = 1,

        /// <summary>
        /// The reference standard condition.
        /// </summary>
        RefStandardCondition = 2,

        /// <summary>
        /// The reference address condition.
        /// </summary>
        RefAddressCondition = 3,

        /// <summary>
        /// The reference parcel condition.
        /// </summary>
        RefParcelCondition = 4,

        /// <summary>
        /// The reference owner condition.
        /// </summary>
        RefOwnerCondition = 5,

        /// <summary>
        /// The reference license professional condition.
        /// </summary>
        RefLicProCondition = 6,

        /// <summary>
        /// The reference contact condition.
        /// </summary>
        RefContactCondition = 7,

        /// <summary>
        /// The reference structure condition.
        /// </summary>
        RefStructureCondition = 8,

        /// <summary>
        /// The reference struct and establishment condition.
        /// </summary>
        RefSTRUCTAndEstablishmentCondition = 9,

        /// <summary>
        /// The asset condition.
        /// </summary>
        AssetCondition = 10,

        /// <summary>
        /// The record condition.
        /// </summary>
        RecordCondition = 11,

        /// <summary>
        /// The inspection condition.
        /// </summary>
        InspectionCondition = 12,

        /// <summary>
        /// The reference LP type definition.
        /// </summary>
        RefLIcProTypeDefinition = 13,

        /// <summary>
        /// The reference license professional.
        /// </summary>
        RefLicProfessional = 14,

        /// <summary>
        /// The daily license professional.
        /// </summary>
        DailyLicProfessional = 15,

        /// <summary>
        /// The reference document type definition.
        /// </summary>
        RefDocTypeDefinition = 16,

        /// <summary>
        /// The record document.
        /// </summary>
        RecordDocument = 17,

        /// <summary>
        /// The asset document.
        /// </summary>
        AssetDocument = 18,

        /// <summary>
        /// The asset document.
        /// </summary>
        AssetCaDocument = 19,

        /// <summary>
        /// The inspection document.
        /// </summary>
        InspectionDocument = 20,

        /// <summary>
        /// The reference parcel document.
        /// </summary>
        RefParcelDocument = 21,

        /// <summary>
        /// The part document.
        /// </summary>
        PartDocument = 22,

        /// <summary>
        /// The reference license professional document.
        /// </summary>
        RefLicProDocument = 23,

        /// <summary>
        /// The evidence document.
        /// </summary>
        EvidenceDocument = 24,

        /// <summary>
        /// The asset inspection document.
        /// </summary>
        AssetInspectionDocument = 25,

        /// <summary>
        /// The asset inspection detail document.
        /// </summary>
        AssetInspectionDetailDocument = 26,

        /// <summary>
        /// The reference contact type definition.
        /// </summary>
        RefContactTypeDefinition = 27,

        /// <summary>
        /// The reference set document.
        /// </summary>
        RefSetDocument = 28,

        /// <summary>
        /// The reference meeting type definition.
        /// </summary>
        RefMeetingTypeDefinition = 29,

        /// <summary>
        /// The meeting.
        /// </summary>
        MEETING = 30,

        /// <summary>
        /// The education definition.
        /// </summary>
        EducationDefinition = 31,

        /// <summary>
        /// The continuing education definition.
        /// </summary>
        ContinuingEducationDefinition = 32,

        /// <summary>
        /// The examination definition.
        /// </summary>
        ExaminationDefinition = 33,

        /// <summary>
        /// The education.
        /// </summary>
        Education = 34,

        /// <summary>
        /// The continuing education.
        /// </summary>
        ContinuingEducation = 35,

        /// <summary>
        /// The examination.
        /// </summary>
        Examination = 36,

        /// <summary>
        /// The reference contact.
        /// </summary>
        RefContact = 37,

        /// <summary>
        /// The daily contact.
        /// </summary>
        DailyContact = 38,

        /// <summary>
        /// The reference education.
        /// </summary>
        RefEducation = 39,

        /// <summary>
        /// The reference continuing education.
        /// </summary>
        RefContinuingEducation = 40,

        /// <summary>
        /// The reference examination.
        /// </summary>
        RefExamination = 41
    }

    /// <summary>
    /// contact identity fields key.
    /// </summary>
    public enum IdentityFields
    {
        /// <summary>
        /// The contact SSN field.
        /// </summary>
        SSN = 28,

        /// <summary>
        /// The contact SSN field.
        /// </summary>
        FEIN = 29,

        /// <summary>
        /// The contact Passport Number field.
        /// </summary>
        PassportNumber = 39,

        /// <summary>
        /// The contact Driver's License Number field.
        /// </summary>
        DriverLicenseNumber = 40,

        /// <summary>
        /// The contact Driver's License State field.
        /// </summary>
        DriverLicenseState = 41,

        /// <summary>
        /// The contact State ID Number field.
        /// </summary>
        StateIDNumber = 42,

        /// <summary>
        /// The contact E-mail field.
        /// </summary>
        Email = 5
    }

    #region License Certification

    /// <summary>
    /// the license verification section type.
    /// </summary>
    public enum LicenseVerificationSectionType
    {
        /// <summary>
        /// Related Records section
        /// </summary>
        RELATED_RECORDS = 0,

        /// <summary>
        /// Education section
        /// </summary>
        EDUCATION = 1,

        /// <summary>
        /// Continue Education section
        /// </summary>
        CONTINUE_EDUCATION = 2,

        /// <summary>
        /// Examination section
        /// </summary>
        EXAMINATION = 3,

        /// <summary>
        /// Public Documents section
        /// </summary>
        PUBLIC_DOCUMENTS = 4
    }

    /// <summary>
    /// enumeration for provider type.
    /// </summary>
    public enum ProviderType
    {
        /// <summary>
        /// Offer education
        /// </summary>
        OfferEducation,

        /// <summary>
        /// Offer continuing
        /// </summary>
        OfferContinuing,

        /// <summary>
        /// Offer examination
        /// </summary>
        OfferExamination
    }

    /// <summary>
    /// Education or examination section position.
    /// </summary>
    public enum EducationOrExamSectionPosition
    {
        /// <summary>
        /// The None.
        /// </summary>
        None,

        /// <summary>
        /// Cap edit page.
        /// </summary>
        CapEdit,

        /// <summary>
        /// Cap confirm page.
        /// </summary>
        CapConfirm,

        /// <summary>
        /// Account contact edit page.
        /// </summary>
        AccountContactEdit,

        /// <summary>
        /// Cap/License detail page.
        /// </summary>
        CapDetail
    }

    /// <summary>
    /// License certification type.
    /// </summary>
    public enum LicenseCertificationType
    {
        /// <summary>
        /// The None.
        /// </summary>
        None,

        /// <summary>
        /// The type for Examine.
        /// </summary>
        Examine,

        /// <summary>
        /// The type for Education.
        /// </summary>
        Education,

        /// <summary>
        /// The type for Continue Education.
        /// </summary>
        ContEducation
    }

    #endregion License Certification

    /// <summary>
    /// Customization Type Per Page Level
    /// </summary>
    [Flags]
    public enum CustomizationType
    {
        /// <summary>
        /// The None
        /// </summary>
        None = 0,

        /// <summary>
        /// Only CSS
        /// </summary>
        Css = 1,

        /// <summary>
        /// Only Javascript
        /// </summary>
        Javascript = 2,

        /// <summary>
        /// CSS and Javascript
        /// </summary>
        CssAndJavascript = 3,

        /// <summary>
        /// Custom Page
        /// </summary>
        CustomPage = 4
    }
    
    /// <summary>
    /// show type for Property Information 
    /// </summary>
    public enum APOShowType
    {
        /// <summary>
        /// The None
        /// </summary>
        None,

        /// <summary>
        /// The show type for Address
        /// </summary>
        ShowAddress,

        /// <summary>
        /// The show type for Parcel
        /// </summary>
        ShowParcel,

        /// <summary>
        /// The show type for Owner
        /// </summary>
        ShowOwner,

        /// <summary>
        /// The show type for LP
        /// </summary>
        ShowAddressByLp,

        /// <summary>
        /// The show type for Cap
        /// </summary>
        ShowAddressByCap
    }

    /// <summary>
    /// Status code that indicates an user's login result.
    /// </summary>
    public enum LoginStatusCode
    {
        /// <summary>
        /// The None
        /// </summary>
        None = 0,

        /// <summary>
        /// User status is active.
        /// </summary>
        ACTIVE,

        /// <summary>
        /// Failed to login.
        /// </summary>
        FAIL,

        /// <summary>
        /// User is inactive.
        /// </summary>
        INACTIVE,

        /// <summary>
        /// User is disabled.
        /// </summary>
        DISABLE,

        /// <summary>
        /// User is locked.
        /// </summary>
        LOCKED,

        /// <summary>
        /// User is not registered at this site.
        /// </summary>
        NOTREGISTERED
    }

    /// <summary>
    /// file owner permission
    /// </summary>
    [Flags]
    public enum FileOwnerPermission
    {
        /// <summary>
        /// No Permission for each action
        /// </summary>
        None = 0,

        /// <summary>
        /// The permission for TitleViewable
        /// </summary>
        TitleViewable = 512,

        /// <summary>
        /// The permission for Download
        /// </summary>
        Downloadable = 256,

        /// <summary>
        /// The permission for Upload
        /// </summary>
        Uploadable = 128,

        /// <summary>
        /// The permission for Delete
        /// </summary>
        Deleteable = 64
    }

    /// <summary>
    /// URL target for the a hyperlink.
    /// </summary>
    public enum URLTarget
    {
        /// <summary>
        /// Renders the content in the frame with focus.
        /// </summary>
        Self = 0,

        /// <summary>
        /// Renders the content in a new window without frames.
        /// </summary>
        Blank = 1,

        /// <summary>
        /// Renders the content in the immediate frameset parent.
        /// </summary>
        Parent = 2,

        /// <summary>
        /// Renders the content in the full window without frames.
        /// </summary>
        Top = 3
    }

    /// <summary>
    /// Row command type for owner list
    /// </summary>
    public enum OwnerListRowCommandType
    {
        /// <summary>
        /// Show owner
        /// </summary>
        ShowOwner,

        /// <summary>
        /// Select owner
        /// </summary>
        SelectOwner,
    }

    /// <summary>
    /// Address format type
    /// </summary>
    public enum AddressFormatType
    {
        /// <summary>
        /// Long address with format
        /// </summary>
        LONG_ADDRESS_WITH_FORMAT,

        /// <summary>
        /// long address no format
        /// </summary>
        LONG_ADDRESS_NO_FORMAT,

        /// <summary>
        /// short address with format
        /// </summary>
        SHORT_ADDRESS_WITH_FORMAT,

        /// <summary>
        /// short address no format.
        /// </summary>
        SHORT_ADDRESS_NO_FORMAT
    }

    /// <summary>
    /// http method.
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// post http method.
        /// </summary>
        POST,

        /// <summary>
        /// Get http method.
        /// </summary>
        GET
    }
    
    /// <summary>
    /// File Upload Behavior
    /// </summary>
    public enum FileUploadBehavior
    {
        /// <summary>
        /// upload in Basic way
        /// </summary>
        Basic,

        /// <summary>
        /// upload in Advanced way
        /// </summary>
        Advanced,

        /// <summary>
        /// upload in Html5
        /// </summary>
        Html5
    }
}
