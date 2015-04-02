#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ColumnConstant.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: ColumnConstant.cs 133464 2009-06-05 05:06:34Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.Common
{
    /// <summary>
    /// the class for column constant.
    /// </summary>
    public static class ColumnConstant
    {
        #region Fields

        /// <summary>
        /// The template attribute column
        /// </summary>
        public const string TEAMPLATE_ATTRIBUTE = "TemplateAttributes";

        #endregion Fields

        #region Enums

        /// <summary>
        /// enumeration for contact column name.
        /// </summary>
        public enum Contact
        {
            /// <summary>
            /// Column name for ContactModel.Index
            /// </summary>
            RowIndex,

            /// <summary>
            /// Column name for ContactModel.ContactType
            /// </summary>
            ContactType,

            /// <summary>
            /// Column name for ContactModel.ContactType.Flag
            /// </summary>
            ContactTypeFlag,

            /// <summary>
            /// Column name for ContactModel.Salutation
            /// </summary>
            Salutation,

            /// <summary>
            /// Column name for ContactModel.Title
            /// </summary>
            Title,

            /// <summary>
            /// Combination of first name,middle name and last name
            /// </summary>
            FullName,

            /// <summary>
            /// Column name for ContactModel.FirstName
            /// </summary>
            FirstName,

            /// <summary>
            /// Column name for ContactModel.MiddleName
            /// </summary>
            MiddleName,

            /// <summary>
            /// Column name for ContactModel.LastName
            /// </summary>
            LastName,

            /// <summary>
            /// Column name for ContactModel.SuffixName
            /// </summary>
            SuffixName,

            /// <summary>
            /// Column name for ContactModel.BirthDate
            /// </summary>
            BirthDate,

            /// <summary>
            /// Column name for ContactModel.Gender
            /// </summary>
            Gender,

            /// <summary>
            /// Column name for ContactModel.Business
            /// </summary>
            Business,

            /// <summary>
            /// Column name for ContactModel.AddressLine1
            /// </summary>
            AddressLine1,

            /// <summary>
            /// Column name for ContactModel.AddressLine2
            /// </summary>
            AddressLine2,

            /// <summary>
            /// Column name for ContactModel.AddressLine3
            /// </summary>
            AddressLine3,

            /// <summary>
            /// Column name for ContactModel.Fein
            /// </summary>
            Fein,

            /// <summary>
            /// Column name for ContactModel.SSN
            /// </summary>
            SocialSecurityNumber,

            /// <summary>
            /// Column name for ContactModel.TradeName
            /// </summary>
            TradeName,

            /// <summary>
            /// Column name for ContactModel.City
            /// </summary>
            City,

            /// <summary>
            /// Column name for ContactModel.State
            /// </summary>
            State,

            /// <summary>
            /// Column name for ContactModel.Zip
            /// </summary>
            Zip,

            /// <summary>
            /// Column name for ContactModel.POBox
            /// </summary>
            POBox,

            /// <summary>
            /// Column name for ContactModel.Country
            /// </summary>
            Country,

            /// <summary>
            /// Column name for ContactModel.HomePhone
            /// </summary>
            HomePhone,

            /// <summary>
            /// Column name for ContactModel.HomePhoneCode
            /// </summary>
            HomePhoneCode,

            /// <summary>
            /// Column name for ContactModel.WorkPhone
            /// </summary>
            WorkPhone,

            /// <summary>
            /// Column name for ContactModel.WorkPhoneCode
            /// </summary>
            WorkPhoneCode,

            /// <summary>
            /// Column name for ContactModel.MobilePhone
            /// </summary>
            MobilePhone,

            /// <summary>
            /// Column name for ContactModel.MobilePhoneCode
            /// </summary>
            MobilePhoneCode,

            /// <summary>
            /// Column name for ContactModel.Fax
            /// </summary>
            Fax,

            /// <summary>
            /// Column name for ContactModel.FaxCode
            /// </summary>
            FaxCode,

            /// <summary>
            /// Column name for ContactModel.Email.
            /// </summary>
            Email,

            /// <summary>
            /// Column name for contact sequence number in ContactModel.people
            /// </summary>
            ContactSequence,

            /// <summary>
            /// Column Name for ContactModel.RefSequenceNumber
            /// </summary>
            RefSequence,

            /// <summary>
            /// Column name for CapContactModel.
            /// </summary>
            CapContactModel,

            /// <summary>
            /// Column name for CapContactModel.permission.
            /// </summary>
            ContactPermission,

            /// <summary>
            /// Column name for ContactModel.Business2
            /// </summary>
            BusinessName2,

            /// <summary>
            /// Birthplace City
            /// </summary>
            BirthplaceCity,

            /// <summary>
            /// Birthplace State
            /// </summary>
            BirthplaceState,

            /// <summary>
            /// Birthplace Country/Region
            /// </summary>
            BirthplaceCountry,

            /// <summary>
            /// Column name for Race
            /// </summary>
            Race,

            /// <summary>
            /// Deceased Date
            /// </summary>
            DeceasedDate,

            /// <summary>
            /// Passport Number
            /// </summary>
            PassportNumber,

            /// <summary>
            /// Driver License Number
            /// </summary>
            DriverLicenseNumber,

            /// <summary>
            /// Driver License State
            /// </summary>
            DriverLicenseState,

            /// <summary>
            /// State Id Number
            /// </summary>
            StateIdNumber,

            /// <summary>
            /// Required reference contact
            /// </summary>
            Required,

            /// <summary>
            /// Preferred Channel
            /// </summary>
            PreferredChannel,

            /// <summary>
            /// The column name for Comment
            /// </summary>
            Comment,

            /// <summary>
            /// Component Name
            /// </summary>
            ComponentName,

            /// <summary>
            /// Column name for ContactModel.AdditionalAddresses
            /// </summary>
            AdditionalAddresses
        }
        
        /// <summary>
        /// enumeration for education column name.
        /// </summary>
        public enum Education
        {
            /// <summary>
            /// Row index for each education record.
            /// </summary>
            RowIndex,

            /// <summary>
            /// Education number.
            /// </summary>
            educationNbr,

            /// <summary>
            /// Provider name.
            /// </summary>
            providerName,

            /// <summary>
            /// Provider number.
            /// </summary>
            providerNo,

            /// <summary>
            /// Ref education name.
            /// </summary>
            educationName,

            /// <summary>
            /// Education Degree.
            /// </summary>
            degree,

            /// <summary>
            /// Year attended.
            /// </summary>
            yearAttended,

            /// <summary>
            /// Year graduated.
            /// </summary>
            yearGraduated,

            /// <summary>
            /// Provider address1.
            /// </summary>
            address1,

            /// <summary>
            /// Provider address2.
            /// </summary>
            address2,

            /// <summary>
            /// Provider address3.
            /// </summary>
            address3,

            /// <summary>
            /// Provider city.
            /// </summary>
            city,

            /// <summary>
            /// Provider state.
            /// </summary>
            state,

            /// <summary>
            /// Provider zip code.
            /// </summary>
            zip,

            /// <summary>
            /// Provider phone number1.
            /// </summary>
            phone1,

            /// <summary>
            /// Provider phone1 country code.
            /// </summary>
            phone1CountryCode,

            /// <summary>
            /// Provider phone number2.
            /// </summary>
            phone2,

            /// <summary>
            /// Provider phone2 country code.
            /// </summary>
            phone2CountryCode,

            /// <summary>
            /// Provider phone number3.
            /// </summary>
            fax,

            /// <summary>
            /// Provider phone3 country code.
            /// </summary>
            faxCountryCode,

            /// <summary>
            /// Required for education.
            /// </summary>
            requiredFlag,

            /// <summary>
            /// Mail address.
            /// </summary>
            email,

            /// <summary>
            /// Comments for education.
            /// </summary>
            comments,

            /// <summary>
            /// The reference education number.
            /// </summary>
            RefEduNbr,

            /// <summary>
            /// The template model
            /// </summary>
            TemplateModel,

            /// <summary>
            /// The country code
            /// </summary>
            countryCode,

            /// <summary>
            /// The approved flag.
            /// </summary>
            ApprovedFlag,

            /// <summary>
            /// contact sequence number.
            /// </summary>
            ContactSeqNumber
        }

        /// <summary>
        /// enumeration for provider column name.
        /// </summary>
        public enum Provider
        {
            /// <summary>
            /// Provider number.
            /// </summary>
            ProviderNumber,

            /// <summary>
            /// Provider name.
            /// </summary>
            ProviderName
        }

        /// <summary>
        /// enumeration for refContinuing education column name.
        /// </summary>
        public enum RefContinuingEducation
        {
            /// <summary>
            /// Column name for RefContinuingEducation.CourseName.
            /// </summary>
            CourseName,

            /// <summary>
            /// Column name for RefContinuingEducation.GradingStyle.
            /// </summary>
            GradingStyle,

            /// <summary>
            /// Column name for RefContinuingEducation.PassingScore.
            /// </summary>
            PassingScore,

            /// <summary>
            /// Column name for RefContinuingEducation.ContinuingEducationNumber.
            /// </summary>
            ContEduNbr
        }

        /// <summary>
        /// enumeration for provider colum name.
        /// </summary>
        public enum RefProvider
        {
            /// <summary>
            /// Column name for RefProviderModel.ProviderName
            /// </summary>
            Name,

            /// <summary>
            /// Column name for RefProviderModel.ProviderNumber
            /// </summary>
            Number,

            /// <summary>
            /// Column name for RefProviderModel.Address
            /// </summary>
            Address,

            /// <summary>
            /// Column name for RefProviderModel.PhoneNumber
            /// </summary>
            PhoneNumber,

            /// <summary>
            /// Column name for RefProviderModel.PhoneNumber
            /// </summary>
            PhoneNumberCode,

            /// <summary>
            /// Column name for RefProviderModel's Provider number
            /// </summary>
            ProviderPKNbr
        }

        /// <summary>
        /// enumeration for education colum name.
        /// </summary>
        public enum RefEducation
        {
            /// <summary>
            /// Column name for RefEducationModel.RefEducationName
            /// </summary>
            Name,

            /// <summary>
            /// Column name for RefEducationModel.Degree
            /// </summary>
            Degree,

            /// <summary>
            /// Column name for RefEducationModel's Reference Education Number
            /// </summary>
            Number
        }

        /// <summary>
        /// enumeration for reference education detail column name.
        /// </summary>
        public enum RefEducationDetail
        {
            /// <summary>
            /// Reference education number.
            /// </summary>
            RefEducationNumber,

            /// <summary>
            /// Reference education name.
            /// </summary>
            RefEducationName,

            /// <summary>
            /// Degree for education.
            /// </summary>
            Degree,

            /// <summary>
            /// Required for education.
            /// </summary>
            Required,

            /// <summary>
            /// Comment for education.
            /// </summary>
            Comments
        }

        /// <summary>
        /// enumeration for examinations colum name.
        /// </summary>
        public enum RefExaminations
        {
            /// <summary>
            /// Column name for RefExaminations.ExaminationsName
            /// </summary>
            Name,
            
            /// <summary>
            /// Column name for RefExaminations.GradingStyle
            /// </summary>
            GradingStyle,

            /// <summary>
            /// Column name for RefExaminations.PassingScore
            /// </summary>
            PassingScore,

            /// <summary>
            /// Column name for RefExaminations.Number
            /// </summary>
            refExamNbr
        }

        /// <summary>
        /// enumeration for ref License Profession column name.
        /// </summary>
        public enum RefLicenseProfessional
        {
            /// <summary>
            /// The state license
            /// </summary>
            StateLicense,

            /// <summary>
            /// The license type
            /// </summary>
            LicenseType,

            /// <summary>
            /// The type flag
            /// </summary>
            TypeFlag,

            /// <summary>
            /// The masked SSN
            /// </summary>
            MaskedSSN,

            /// <summary>
            /// The fein
            /// </summary>
            Fein,

            /// <summary>
            /// The business name
            /// </summary>
            BusinessName,

            /// <summary>
            /// The business license
            /// </summary>
            BusinessLicense,

            /// <summary>
            /// The contact first name
            /// </summary>
            ContactFirstName,

            /// <summary>
            /// The contact middle name
            /// </summary>
            ContactMiddleName,

            /// <summary>
            /// The contact last name
            /// </summary>
            ContactLastName,

            /// <summary>
            /// combine the address1, address2, address3, city, state, zip.
            /// </summary>
            FullAddress,

            /// <summary>
            /// The license expiration date
            /// </summary>
            LicenseExpirationDate,

            /// <summary>
            /// The insurance expiration date
            /// </summary>
            InsuranceExpDate,

            /// <summary>
            /// The license board
            /// </summary>
            LicenseBoard,

            /// <summary>
            /// The license issue date
            /// </summary>
            LicenseIssueDate,

            /// <summary>
            /// The license last renewal date
            /// </summary>
            LicenseLastRenewalDate,

            /// <summary>
            /// The city
            /// </summary>
            City,

            /// <summary>
            /// The state
            /// </summary>
            State,

            /// <summary>
            /// The zip
            /// </summary>
            Zip,

            /// <summary>
            /// The address 1
            /// </summary>
            Address1,

            /// <summary>
            /// The address 2
            /// </summary>
            Address2,

            /// <summary>
            /// The address 3
            /// </summary>
            Address3,

            /// <summary>
            /// The business name 2
            /// </summary>
            BusName2,

            /// <summary>
            /// The title
            /// </summary>
            Title,

            /// <summary>
            /// The policy
            /// </summary>
            Policy,

            /// <summary>
            /// The insurance
            /// </summary>
            InsuranceCo,

            /// <summary>
            /// The res state
            /// </summary>
            ResState,

            /// <summary>
            /// The country code
            /// </summary>
            CountryCode
        }

        /// <summary>
        /// enumeration for attachment column name.
        /// </summary>
        public enum Attachment
        {
            /// <summary>
            /// Column name for document number
            /// </summary>
            DocNumber,

            /// <summary>
            /// Column name for document name
            /// </summary>
            Name,

            /// <summary>
            /// Column name for document description
            /// </summary>
            Description,

            /// <summary>
            /// Column name for document size
            /// </summary>
            Size,

            /// <summary>
            /// Column name for document date
            /// </summary>
            Date,

            /// <summary>
            /// Column name for document type
            /// </summary>
            Type,

            /// <summary>
            /// Column name for row index
            /// </summary>
            RowIndex,

            /// <summary>
            /// Column name for document res-type
            /// </summary>
            ResType,

            /// <summary>
            /// Column name for document view role privilege of partial cap
            /// </summary>
            ViewRole,

            /// <summary>
            /// Column name for document view role privilege of real cap
            /// </summary>
            ViewRole4RealCAP,

            /// <summary>
            /// Column name for document delete role privilege
            /// </summary>
            DeleteRole,

            /// <summary>
            /// column name for document upload role privilege
            /// </summary>
            uploadRole,

            /// <summary>
            /// Entity Type column
            /// </summary>
            EntityType,

            /// <summary>
            /// Record Number
            /// </summary>
            RecordNumber,

            /// <summary>
            /// Record Type
            /// </summary>
            RecordType,

            /// <summary>
            /// entity information
            /// </summary>
            EntityInfo,

            /// <summary>
            /// Document status
            /// </summary>
            DocumentStatus,

            /// <summary>
            /// Upload date
            /// </summary>
            UploadDate,

            /// <summary>
            /// Virtual folders
            /// </summary>
            VirtualFolders,

            /// <summary>
            /// Agency code.
            /// </summary>
            AgencyCode,

            /// <summary>
            /// Parent agency code.
            /// </summary>
            RefAgencyCode,

            /// <summary>
            /// Date status changed
            /// </summary>
            StatusDate,

            /// <summary>
            /// Resubmit link
            /// </summary>
            AllowActions,

            /// <summary>
            /// Document model for transform.
            /// </summary>
            DocumentModel,
            
            /// <summary>
            /// Entity include specific entity type - entity id.
            /// </summary>
            Entity,

            /// <summary>
            /// File owner permission
            /// </summary>
            FileOwnerPermission,

            /// <summary>
            /// Document Review Status.
            /// </summary>
            ReviewStatus
        }

        /// <summary>
        /// enumeration for trust account table column name.
        /// </summary>
        public enum TrustAccount
        {
            /// <summary>
            /// Column name for Account ID
            /// </summary>
            AccountID,

            /// <summary>
            /// Column name for Primary
            /// </summary>
            Primary,

            /// <summary>
            /// Column name for Balance
            /// </summary>
            Balance,

            /// <summary>
            /// Column name for Description
            /// </summary>
            Description,

            /// <summary>
            /// Column name for Status
            /// </summary>
            Status,

            /// <summary>
            /// Column name for Ledger Account
            /// </summary>
            LedgerAccount,

            /// <summary>
            /// Column name for Deposit
            /// </summary>
            Deposit,

            /// <summary>
            /// Column name for Service Provider Code
            /// </summary>
            ServProvCode,
        }

        /// <summary>
        /// enumeration for transaction table column name.
        /// </summary>
        public enum Transaction
        {
            /// <summary>
            /// Column name for TransID
            /// </summary>
            TransID,

            /// <summary>
            /// Column name for AccountID
            /// </summary>
            AccountID,

            /// <summary>
            /// Column name for TransType
            /// </summary>
            TransType,

            /// <summary>
            /// Column name for TransAmount
            /// </summary>
            TransAmount,

            /// <summary>
            /// Column name for TargetAccountID
            /// </summary>
            TargetAccountID,

            /// <summary>
            /// Column name for RecordID
            /// </summary>
            RecordID,

            /// <summary>
            /// Column name for ALTID
            /// </summary>
            ALTID,

            /// <summary>
            /// Column name for ClientTransNumber
            /// </summary>
            ClientTransNumber,

            /// <summary>
            /// Column name for ClientReceiptNumber
            /// </summary>
            ClientReceiptNumber,

            /// <summary>
            /// Column name for OfficeCode
            /// </summary>
            OfficeCode,

            /// <summary>
            /// Column name for TransDate
            /// </summary>
            TransDate,

            /// <summary>
            /// Column name for DepositMethod
            /// </summary>
            DepositMethod,

            /// <summary>
            /// Column name for TransactionCode
            /// </summary>
            TransactionCode,

            /// <summary>
            /// Column name for CashDrawerID
            /// </summary>
            CashDrawerID,

            /// <summary>
            /// Column name for Comments
            /// </summary>
            Comments,

            /// <summary>
            /// Column name for CustomizedReceiptNumber
            /// </summary>
            CustomizedReceiptNumber,

            /// <summary>
            /// Column name for ReferenceNumber
            /// </summary>
            ReferenceNumber,

            /// <summary>
            /// Column name for Credit Card Authorized Code
            /// </summary>
            CCAuthCode,

            /// <summary>
            /// Column name for Payer
            /// </summary>
            Payor,

            /// <summary>
            /// Column name for Received
            /// </summary>
            Received
        }

        /// <summary>
        /// enumeration for ref parcel list column name.
        /// </summary>
        public enum RefParcelList
        {
            /// <summary>
            /// Column name for Parcel Number.
            /// </summary>
            ParcelNumber,

            /// <summary>
            /// Column name for Lot.
            /// </summary>
            Lot,

            /// <summary>
            /// Column name for Block.
            /// </summary>
            Block,

            /// <summary>
            /// Column name for subdivision.
            /// </summary>
            Subdivision,

            /// <summary>
            /// Column name for owner full name.
            /// </summary>
            OwnerFullName,

            /// <summary>
            /// Column name for address.
            /// </summary>
            FullAddress
        }

        /// <summary>
        /// enumeration for address list column name.
        /// </summary>
        public enum RefAddressList
        {
            /// <summary>
            /// Column name for address.
            /// </summary>
            Address
        }

        /// <summary>
        /// enumeration for Ref Genealogy list column name.
        /// </summary>
        public enum RefGenealogy
        {
            /// <summary>
            /// Date Column for Ref Genealogy.
            /// </summary>
            Date,

            /// <summary>
            /// Date Column for Ref Genealogy.
            /// </summary>
            Description,

            /// <summary>
            /// Date Column for Ref Genealogy.
            /// </summary>
            Children,

            /// <summary>
            /// Date Column for Ref Genealogy.
            /// </summary>
            Action,

            /// <summary>
            /// Date Column for Ref Genealogy.
            /// </summary>
            Parents
        }

        /// <summary>
        /// enumeration for ref people list column name.
        /// </summary>
        public enum PeopleList
        {
            /// <summary>
            /// Column name for Type.
            /// </summary>
            Type,

            /// <summary>
            /// Column name for FirstName.
            /// </summary>
            FirstName,

            /// <summary>
            /// Column name for MiddleName.
            /// </summary>
            MiddleName,

            /// <summary>
            /// Column name for LastName.
            /// </summary>
            LastName,

            /// <summary>
            /// Column name for Address1.
            /// </summary>
            Address1,

            /// <summary>
            /// Column name for FirstName.
            /// </summary>
            Address2,

            /// <summary>
            /// Column name for Address3.
            /// </summary>
            Address3,

            /// <summary>
            /// Column name for Phone1.
            /// </summary>
            Phone1,

            /// <summary>
            /// Column name for Phone2.
            /// </summary>
            Phone2,

            /// <summary>
            /// Column name for Fax.
            /// </summary>
            FaxCode,

            /// <summary>
            /// Column name for Phone1.
            /// </summary>
            Phone1Code,

            /// <summary>
            /// Column name for Phone2.
            /// </summary>
            Phone2Code,

            /// <summary>
            /// Column name for Fax.
            /// </summary>
            Fax,

            /// <summary>
            /// Column name for Email.
            /// </summary>
            Email,

            /// <summary>
            /// Column name for LicenseNumber.
            /// </summary>
            LicenseNumber,

            /// <summary>
            /// Column name for LicenseExpirationDate.
            /// </summary>
            LicenseExpirationDate,

            /// <summary>
            /// Column name for CountryOrRegion.
            /// </summary>
            CountryOrRegion,

            /// <summary>
            /// Column name for FullName
            /// </summary>
            FullName
        }

        /// <summary>
        /// enumeration for reference examination schedule detail.
        /// </summary>
        public enum RefExaminationScheduleDetail
        {
            /// <summary>
            /// Column name for Examination Schedule ID 
            /// </summary>
            ExaminationScheduleID,

            /// <summary>
            /// Column name for Provider 
            /// </summary>
            Provider,

            /// <summary>
            /// Column name for Fee 
            /// </summary>
            Fee,

            /// <summary>
            /// Column name for Date 
            /// </summary>
            Date,

            /// <summary>
            /// Column name for Week Day 
            /// </summary>
            WeekyDay,

            /// <summary>
            /// Column name for StartTime 
            /// </summary>
            StartTime,

            /// <summary>
            /// Column name for EndTime 
            /// </summary>
            EndTime,

            /// <summary>
            /// Column name for Examination Site 
            /// </summary>
            ExaminationSite,

            /// <summary>
            /// Column name for Available Seats 
            /// </summary>
            AvailableSeats,

            /// <summary>
            /// Column name for Handicap Accessible 
            /// </summary>
            HandicapAccessible,

            /// <summary>
            /// Column name for Location Id 
            /// </summary>
            LocationId,

            /// <summary>
            /// Column name for Calendar Id
            /// </summary>
            CalendarId,

            /// <summary>
            /// Column name for Schedule Id
            /// </summary>
            ScheduleId,

            /// <summary>
            /// Column name for Provider number
            /// </summary>
            ProviderNbr,

            /// <summary>
            /// Column name for Is External
            /// </summary>
            IsExternal,

            /// <summary>
            /// Accessibility Descriptions
            /// </summary>
            AccessiblityDesc,

            /// <summary>
            /// Driving Direction Descriptions
            /// </summary>
            DrivingDesc,

            /// <summary>
            /// ref Exam Instructions
            /// </summary>
            ExamInstructions,

            /// <summary>
            /// provider exam is Online
            /// </summary>
            IsOnline
        }

        #endregion Enums
    }
}
