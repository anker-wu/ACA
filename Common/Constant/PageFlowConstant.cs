#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PageFlowConstant.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 *  I18N settings relevant contant define
 *  Notes:
 *      $Id: PageFlowConstant.cs 133464 2013-11-05 05:06:34Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.Common
{
    /// <summary>
    /// Constants to indicate what kind of a component is.
    /// </summary>
    public enum PageFlowComponent
    {
        /// <summary>
        /// An unknown component
        /// </summary>
        UNKNOWN = -9999,

        /// <summary>
        /// The Address component.
        /// </summary>
        ADDRESS = 1,

        /// <summary>
        /// The License Profession component.
        /// </summary>
        LICENSED_PROFESSIONAL = 4,

        /// <summary>
        /// The License Profession List component.
        /// </summary>
        LICENSED_PROFESSIONAL_LIST = 18,

        /// <summary>
        /// The Detail Information component.
        /// </summary>
        DETAIL_INFORMATION = 12,

        /// <summary>
        /// The Owner component.
        /// </summary>
        OWNER = 3,

        /// <summary>
        /// The Applicant component.
        /// </summary>
        APPLICANT = 5,

        /// <summary>
        /// The Contact1 component
        /// </summary>
        CONTACT_1 = 6,

        /// <summary>
        /// The Contact2 component.
        /// </summary>
        CONTACT_2 = 7,

        /// <summary>
        /// The Contact3 component.
        /// </summary>
        CONTACT_3 = 8,

        /// <summary>
        /// The Contact List component.
        /// </summary>
        CONTACT_LIST = 14,

        /// <summary>
        /// The Additional Information component.
        /// </summary>
        ADDITIONAL_INFORMATION = 9,

        /// <summary>
        /// The ASI component.
        /// </summary>
        ASI = 10,

        /// <summary>
        /// The ASIT component.
        /// </summary>
        ASI_TABLE = 11,

        /// <summary>
        /// The Parcel component.
        /// </summary>
        PARCEL = 2,

        /// <summary>
        /// The Education component.
        /// </summary>
        EDUCATION = 15,

        /// <summary>
        /// The Continuing Education component.
        /// </summary>
        CONTINUING_EDUCATION = 16,

        /// <summary>
        /// The Examination component.
        /// </summary>
        EXAMINATION = 17,

        /// <summary>
        /// The Valuation Calculator component.
        /// </summary>
        VALUATION_CALCULATOR = 19,

        /// <summary>
        /// The Attachment component.
        /// </summary>
        ATTACHMENT = 13,

        /// <summary>
        /// The Customize component
        /// </summary>
        CUSTOM_COMPONENT = 20,

        /// <summary>
        /// The Condition Attachment component.
        /// </summary>
        CONDITION_ATTACHMENT = 21,

        /// <summary>
        /// The Asset component.
        /// </summary>
        ASSETS = 22
    }

    /// <summary>
    /// the clone record components.
    /// </summary>
    public enum CloneRecordComponent
    {
        /// <summary>
        /// the work location section
        /// </summary>
        cloneAddressList = 1,

        /// <summary>
        /// the license profession section
        /// </summary>
        cloneProfessional = 2,

        /// <summary>
        /// the project description section 
        /// </summary>
        cloneDetailInformation = 3,

        /// <summary>
        /// the owner section
        /// </summary>
        cloneOwnerList = 4,

        /// <summary>
        /// the related contacts section
        /// </summary>
        cloneContacts = 5,

        /// <summary>
        /// the additional information section
        /// </summary>
        cloneAdditionInfo = 6,

        /// <summary>
        /// the application information section
        /// </summary>
        cloneAppSpecificInfo = 7,

        /// <summary>
        /// the application information table section
        /// </summary>
        cloneAppSpecificInfoTable = 8,

        /// <summary>
        /// the parcel section
        /// </summary>
        cloneParcelList = 9,

        /// <summary>
        /// the education section.
        /// </summary>
        cloneEducation = 15,

        /// <summary>
        /// the continue education section.
        /// </summary>
        cloneContEducation = 16,

        /// <summary>
        /// the examination section.
        /// </summary>
        cloneExamination = 17,

        /// <summary>
        /// the Valuation Calculator section.
        /// </summary>
        cloneValuationCalculator = 18,

        /// <summary>
        /// the Asset section
        /// </summary>
        cloneAssetList = 22
    }

    /// <summary>
    /// The constant define for page flow
    /// </summary>
    public static class PageFlowConstant
    {
        #region Section Name

        /// <summary>
        /// The section name prefix for applicant.
        /// </summary>
        public const string SECTION_NAME_APPLICANT_PREFIX = "Applicant";

        /// <summary>
        /// The section name prefix for contact1.
        /// </summary>
        public const string SECTION_NAME_CONTACT1_PREFIX = "Contact1";

        /// <summary>
        /// The section name prefix for contact2.
        /// </summary>
        public const string SECTION_NAME_CONTACT2_PREFIX = "Contact2";

        /// <summary>
        /// The section name prefix for contact3.
        /// </summary>
        public const string SECTION_NAME_CONTACT3_PREFIX = "Contact3";

        /// <summary>
        /// The section name prefix for multiple contact.
        /// </summary>
        public const string SECTION_NAME_MULTI_CONTACT_PREFIX = "MultiContacts";

        /// <summary>
        /// The section name prefix for license professional.
        /// </summary>
        public const string SECTION_NAME_LICENSE_PREFIX = "License";

        /// <summary>
        /// The section name prefix for multiple license professional.
        /// </summary>
        public const string SECTION_NAME_MULTI_LICENSE_PREFIX = "MultiLicenses";

        /// <summary>
        /// The section name prefix for attachment.
        /// </summary>
        public const string SECTION_NAME_ATTACHMENT_PREFIX = "Attachment";

        /// <summary>
        /// The section name for assets
        /// </summary>
        public const string SECTION_NAME_ADDRESS = "WorkLocation";

        /// <summary>
        /// The section name for Owner
        /// </summary>
        public const string SECTION_NAME_OWNER = "Owner";

        /// <summary>
        /// The section name for Parcel
        /// </summary>
        public const string SECTION_NAME_PARCEL = "Parcel";

        /// <summary>
        /// The section name for assets
        /// </summary>
        public const string SECTION_NAME_ASSETS = "Assets";

        /// <summary>
        /// The UI data key for ASI.
        /// </summary>
        public const string SECTION_NAME_ASI = "AppSpec";

        /// <summary>
        /// The UI data key for ASI table.
        /// </summary>
        public const string SECTION_NAME_ASIT = "AppSpecTable";

        /// <summary>
        /// The section name for Education
        /// </summary>
        public const string SECTION_NAME_EDUCATION = "Education";

        /// <summary>
        /// The section name for condition document
        /// </summary>
        public const string SECTION_NAME_CONDITIONDOCUMENT = "ConditionDocument";

        /// <summary>
        /// The section name for ContinuingEducation
        /// </summary>
        public const string SECTION_NAME_CONT_EDUCATION = "ContinuingEducation";

        /// <summary>
        /// The section name for Examination
        /// </summary>
        public const string SECTION_NAME_EXAMINATION = "Examination";

        /// <summary>
        /// The section name for DetailInfo
        /// </summary>
        public const string SECTION_NAME_DETAIL_INFO = "DetailInfo";

        /// <summary>
        /// The section name for AdditionalInfo
        /// </summary>
        public const string SECTION_NAME_ADDITIONAL_INFO = "Description";

        /// <summary>
        /// The section name for ValuationCalculator
        /// </summary>
        public const string SECTION_NAME_VALUATION_CALCULATOR = "ValuationCalculator";

        #endregion Section Name

        #region ASCX Control Name

        /// <summary>
        /// The ascx control name for Address
        /// </summary>
        public const string CONTROL_NAME_ADDRESS = "Address";

        /// <summary>
        /// The ascx control name for Parcel
        /// </summary>
        public const string CONTROL_NAME_PARCEL = "Parcel";

        /// <summary>
        /// The ascx control name for Owner
        /// </summary>
        public const string CONTROL_NAME_OWNER = "Owner";

        /// <summary>
        /// The ascx control name for Asset
        /// </summary>
        public const string CONTROL_NAME_ASSETS = "AssetList";

        /// <summary>
        /// The ascx control name for Applicant/Contact1/2/3 
        /// </summary>
        public const string CONTROL_NAME_CONTACT = "Contact";

        /// <summary>
        /// The ascx control name for MultiContacts
        /// </summary>
        public const string CONTROL_NAME_MULTI_CONTACTS = "MultiContacts";

        /// <summary>
        /// The ascx control name for License
        /// </summary>
        public const string CONTROL_NAME_LICENSE = "License";

        /// <summary>
        /// The ascx control name for MultiLicenses
        /// </summary>
        public const string CONTROL_NAME_MULTI_LICENSES = "MultiLicenses";

        /// <summary>
        /// The ascx control name for ASI
        /// </summary>
        public const string CONTROL_NAME_ASI = "AppSpecInfo";

        /// <summary>
        /// The ascx control name for ASIT
        /// </summary>
        public const string CONTROL_NAME_ASIT = "AppSpecInfoTable";

        /// <summary>
        /// The ascx control name for Education
        /// </summary>
        public const string CONTROL_NAME_EDUCATION = "Education";

        /// <summary>
        /// The ascx control name for ContinuingEducation
        /// </summary>
        public const string CONTROL_NAME_CONT_EDUCATION = "ContinuingEducation";

        /// <summary>
        /// The ascx control name for Examination
        /// </summary>
        public const string CONTROL_NAME_EXAMINATION = "Examination";

        /// <summary>
        /// The ascx control name for Attachment
        /// </summary>
        public const string CONTROL_NAME_ATTACHMENT = "Attachment";

        /// <summary>
        /// The ascx control name for Condition Document
        /// </summary>
        public const string CONTROL_NAME_CONDITIONDOCUMENT = "ConditionDocument";

        /// <summary>
        /// The ascx control name for DetailInfo
        /// </summary>
        public const string CONTROL_NAME_DETAIL_INFO = "DetailInfo";

        /// <summary>
        /// The ascx control name for CapDescription
        /// </summary>
        public const string CONTROL_NAME_ADDITIONAL_INFO = "CapDescription";

        /// <summary>
        /// The ascx control name for ValuationCalculator
        /// </summary>
        public const string CONTROL_NAME_VALUATION_CALCULATOR = "ValuationCalculator";

        #endregion ASCX Control Name
    }
}
