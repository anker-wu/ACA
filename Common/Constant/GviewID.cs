#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GviewID.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  Define all GView id
 *  Notes:
 *      $Id: GviewID.cs 133464 2009-06-05 05:06:34Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System.Collections.Generic;

namespace Accela.ACA.Common
{
    /// <summary>
    /// GVIEW ID 
    /// </summary>
    public class GviewID
    {
        #region Forms

        #region SPEAR FORMS

        /// <summary>
        /// address spear form:60001.
        /// </summary>
        public const string AddressEdit = "60001";

        /// <summary>
        /// parcel spear form:60002.
        /// </summary>
        public const string ParcelEdit = "60002";

        /// <summary>
        /// owner spear form:60003
        /// </summary>
        public const string OwnerEdit = "60003";

        /// <summary>
        /// license spear form:60004.
        /// </summary>
        public const string LicenseEdit = "60004";

        /// <summary>
        /// contact spear form:60005.
        /// </summary>
        public const string ContactEdit = "60005";

        /// <summary>
        /// cap description spear form:60006.
        /// </summary>
        public const string CAPDescriptionEdit = "60006";

        /// <summary>
        /// Detail Information:60015
        /// </summary>
        public const string DetailInformation = "60015";

        /// <summary>
        /// Attachment spear form:60016
        /// </summary>
        public const string Attachment = "60016";

        /// <summary>
        /// Registration Contact Form: 60140.
        /// </summary>
        public const string RegistrationContactForm = "60140";

        /// <summary>
        /// Add Reference Contact Form: 60141.
        /// </summary>
        public const string AddReferenceContactForm = "60141";

        /// <summary>
        /// Modify Reference Contact Form: 60142.
        /// </summary>
        public const string ModifyReferenceContactForm = "60142";

        /// <summary>
        /// Registration Contact Address Form: 60143.
        /// </summary>
        public const string RegistrationContactAddressForm = "60143";

        /// <summary>
        /// Registration Contact Address List: 60144.
        /// </summary>
        public const string RegistrationContactAddressList = "60144";

        /// <summary>
        /// External Address List: 60146.
        /// </summary>
        public const string ExternalAddressList = "60146";

        /// <summary>
        /// cap description section id 
        /// People Attachment form:60137.
        /// </summary>
        public const string PeopleAttachment = "60137";

        /// <summary>
        /// education spear form:60018
        /// </summary>
        public const string EducationEdit = "60018";

        /// <summary>
        /// examination spear form:60019.
        /// </summary>
        public const string ExaminationEdit = "60019";

        /// <summary>
        /// continuing education spear form:60084.
        /// </summary>
        public const string ContinuingEducationEdit = "60084";

        /// <summary>
        /// Contact Address:60135.
        /// </summary>
        public const string ContactAddress = "60135";

        /// <summary>
        /// User Registration:60129.
        /// </summary>
        public const string UserRegistration = "60129";

        /// <summary>
        /// User Account:60130.
        /// </summary>
        public const string UserAccount = "60130";

        /// <summary>
        /// Existing Account Registration: 60168
        /// </summary>
        public const string ExistingAccountRegisteration = "60168";

        /// <summary>
        /// Clerk login information add form:60147.
        /// </summary>
        public const string AuthAgentNewClerkAccountForm = "60147";

        /// <summary>
        /// Clerk contact form for new clerk:60148.
        /// </summary>
        public const string AuthAgentNewClerkContactForm = "60148";

        /// <summary>
        /// Clerk contact address list for new clerk: 60150.
        /// </summary>
        public const string AuthAgentNewClerkContactAddressList = "60150";

        /// <summary>
        /// Clerk list:60151.
        /// </summary>
        public const string AuthAgentClerkList = "60151";

        /// <summary>
        /// Clerk login information edit form:60152.
        /// </summary>
        public const string AuthAgentEditClerkAccountForm = "60152";

        /// <summary>
        /// Authorized agent customer detail contact address form:60155.
        /// </summary>
        public const string AuthAgentCustomerDetailCAForm = "60155";

        /// <summary>
        /// Authorized agent customer detail contact address list: 60156.
        /// </summary>
        public const string AuthAgentCustomerDetailCAList = "60156";

        /// <summary>
        /// Authorized agent customer detail associated license list:60158;
        /// </summary>
        public const string AuthAgentCustomerAssociatedLicenseList = "60158";

        /// <summary>
        /// Clerk contact form for edit clerk:60159.
        /// </summary>
        public const string AuthAgentEditClerkContactForm = "60159";

        /// <summary>
        /// Clerk contact address form for edit clerk:60160.
        /// </summary>
        public const string AuthAgentEditClerkContactAddressForm = "60160";

        /// <summary>
        /// Clerk contact address list for edit clerk: 60161.
        /// </summary>
        public const string AuthAgentEditClerkContactAddressList = "60161";

        /// <summary>
        /// Asset search result list view id:60171.
        /// </summary>
        public const string AssetResultList = "60171";

        /// <summary>
        /// Asset page flow list view id:60173.
        /// </summary>
        public const string AssetListEdit = "60173";

        /// <summary>
        /// Asset address list view id:60172.
        /// </summary>
        public const string AssetAddressList = "60172";

        /// <summary>
        /// Asset list view id in cap detail:60169
        /// </summary>
        public const string AssetCapDetail = "60169";

        /// <summary>
        /// Reference contact education spear form:60174
        /// </summary>
        public const string RefContactEducationEdit = "60174";

        /// <summary>
        /// Reference contact examination spear form:60175.
        /// </summary>
        public const string RefContactExaminationEdit = "60175";

        /// <summary>
        /// Reference contact continuing education spear form:60176.
        /// </summary>
        public const string RefContactContinuingEducationEdit = "60176";

        #endregion SPEAR FORMS

        #region Search forms

        /// <summary>
        /// general search form:60007.
        /// </summary>
        public const string GeneralSearch = "60007";

        /// <summary>
        /// Search by Permit:60008.
        /// </summary>
        public const string SearchByPermit = "60008";

        /// <summary>
        /// search by license:60009.
        /// </summary>
        public const string SearchByLicense = "60009";

        /// <summary>
        /// Search by Address:60010.
        /// </summary>
        public const string SearchByAddress = "60010";

        /// <summary>
        /// Search for asset:60017.
        /// </summary>
        public const string SearchForAsset = "60017";

        /// <summary>
        /// Search by trade name:60057.
        /// </summary>
        public const string SearchByTradeName = "60057";

        /// <summary>
        /// Search by Contact:60065.
        /// </summary>
        public const string SearchByContact = "60065";

        /// <summary>
        /// Search for Provider:60075
        /// </summary>
        public const string SearchForProvider = "60075";

        /// <summary>
        /// Search for Education/Exam:60076.
        /// </summary>
        public const string SearchForEducationAndExam = "60076";

        /// <summary>
        /// search by reference license:60086.
        /// </summary>
        public const string SearchForLicensee = "60086";

        /// <summary>
        /// search by food facility:60115. 
        /// </summary>
        public const string SearchForFoodFacility = "60115";

        /// <summary>
        /// search by certified business:60119.
        /// </summary>
        public const string SearchForCertifiedBusiness = "60119";

        /// <summary>
        /// Look up by Address form:60011.
        /// </summary>
        public const string LookUpByAddress = "60011";

        /// <summary>
        /// Look up by Parcel:60012
        /// </summary>
        public const string LookUpByParcel = "60012";

        /// <summary>
        /// Look up by owner:60013.
        /// </summary>
        public const string LookUpByOwner = "60013";

        /// <summary>
        /// Look up by permit:60014.
        /// </summary>
        public const string LookUpByPermit = "60014";

        /// <summary>
        /// Look up by License:60094.
        /// </summary>
        public const string LookUpByLicensee = "60094";

        /// <summary>
        /// Search for Authorized agent customer
        /// </summary>
        public const string AuthAgentCustomerSearchForm = "60153";

        /// <summary>
        /// Authorized agent customer detail
        /// </summary>
        public const string AuthAgentCustomerDetail = "60154";

        /// <summary>
        /// Contact look up form: 60180.
        /// </summary>
        public const string ContactLookUp = "60180";

        /// <summary>
        /// License look up form: 60181.
        /// </summary>
        public const string LicenseLookUp = "60181";

        /// <summary>
        /// Inspection specify contact information
        /// </summary>
        public const string InspectionSpecifyContact = "60020";

        #endregion Search Forms.

        #endregion Forms

        #region List

        /// <summary>
        /// Permit list:60033.
        /// </summary>
        public const string PermitList = "60033";

        /// <summary>
        /// Search Form Contact List
        /// </summary>
        public const string SearchFormContactList = "60066";

        /// <summary>
        /// provider list:60069.
        /// </summary>
        public const string ProviderList = "60069";

        /// <summary>
        /// education list:60074.
        /// </summary>
        public const string EducationList = "60074";

        /// <summary>
        /// continuing education list:60079.
        /// </summary>
        public const string ContinuingEducationList = "60079";

        /// <summary>
        /// examination list:60085.
        /// </summary>
        public const string ExaminationList = "60085";

        /// <summary>
        /// licensee list:60087.
        /// </summary>
        public const string LicenseeList = "60087";

        /// <summary>
        /// Multiple contact list:60067.
        /// </summary>
        public const string ContactList = "60067";

        /// <summary>
        /// Reference contact list:60133.
        /// </summary>
        public const string ReferenceContactList = "60133";

        /// <summary>
        /// Reference contact list gathered from other agencies: 60182
        /// </summary>
        public const string OtherAgenciesReferenceContactList = "60182";

        /// <summary>
        /// food facility list:60116.
        /// </summary>
        public const string FoodFacilityList = "60116";

        /// <summary>
        /// certified business list:60120.
        /// </summary>
        public const string CertifiedBusinessList = "60120";

        /// <summary>
        /// certified business experience list:60123.
        /// </summary>
        public const string CertifiedBusinessExperienceList = "60123";

        /// <summary>
        /// Address Condition list:60036.
        /// </summary>
        public const string AddressConditionList = "60036";

        /// <summary>
        /// Contact Condition list:60096.
        /// </summary>
        public const string ContactConditionList = "60096";

        /// <summary>
        /// License Condition list:60043.
        /// </summary>
        public const string LicenseConditionList = "60041";

        /// <summary>
        /// Owner Condition list:60043.
        /// </summary>
        public const string OwnerConditionList = "60043";

        /// <summary>
        /// Parcel Condition list:60045.
        /// </summary>
        public const string ParcelConditionList = "60045";

        /// <summary>
        /// Contact Address List.
        /// </summary>
        public const string ContactAddressList = "60134";

        /// <summary>
        /// Contact select from account list
        /// </summary>
        public const string ContactSelectFromAccountList = "60178";

        /// <summary>
        /// Contact select from professionals for registration.
        /// </summary>
        public const string ContactSelectFromProfessionals = "60195";

        /// <summary>
        /// Cap edit attachment list:60061.
        /// </summary>
        public const string CapEditAttachmentList = "60061";

        /// <summary>
        /// Cap detail attachment list:60062.
        /// </summary>
        public const string CapDetailAttachmentList = "60062";

        /// <summary>
        /// Account manager attachment list:60136.
        /// </summary>
        public const string AccountManagerAttachmentList = "60136";

        /// <summary>
        /// My shared List:60138.
        /// </summary>
        public const string FacebookMySharedList = "60138";

        /// <summary>
        /// Account management reference contact list:60145.
        /// </summary>
        public const string AccountReferenceContactList = "60145";

        /// <summary>
        /// Look up by address list result:60047;
        /// </summary>
        public const string LookUpByAddressList = "60047";

        /// <summary>
        /// look up by record information:60034
        /// </summary>
        public const string LookUpByRecordList = "60034";

        /// <summary>
        /// global search cap result:60088
        /// </summary>
        public const string GlobalSearchCapList = "60088";

        /// <summary>
        /// show record for cap list:60121.
        /// </summary>
        public const string GISCapList = "60121";

        /// <summary>
        /// show record for apo list:60122.
        /// </summary>
        public const string GISAPOList = "60122";

        /// <summary>
        /// valuation calculator list:60100.
        /// </summary>
        public const string ValuationCalculatorList = "60100";

        /// <summary>
        /// licensee detail cap list:60103.
        /// </summary>
        public const string LicenseDetailCapList = "60103";

        /// <summary>
        /// The authorized agent search list
        /// </summary>
        public const string AuthAgentSearchList = "60157";

        /// <summary>
        /// The new inspection list.
        /// </summary>
        public const string NewInspectionList = "60162";

        /// <summary>
        /// The resulted inspection list.
        /// </summary>
        public const string ResultedInspectionList = "60163";

        /// <summary>
        /// The reference contact associate education list:60165.
        /// </summary>
        public const string RefContactEducationList = "60165";

        /// <summary>
        /// The reference contact associate examination list:60166.
        /// </summary>
        public const string RefContactExaminationList = "60166";

        /// <summary>
        /// The reference contact associate continuing education list:60167.
        /// </summary>
        public const string RefContactContinuingEducationList = "60167";

        /// <summary>
        /// The spear form education list:60070.
        /// </summary>
        public const string SpearFormEducationList = "60070";

        /// <summary>
        /// The spear form continuing education list:60083.
        /// </summary>
        public const string SpearFormContinuingEducationList = "60083";

        /// <summary>
        /// The spear form examination list:60085.
        /// </summary>
        public const string SpearFormExaminationList = "60085";

        /// <summary>
        /// The license detail examination list:60126.
        /// </summary>
        public const string LicenseDetailExaminationList = "60126";

        /// <summary>
        /// Contact address search list.
        /// </summary>
        public const string ContactAddressSearchList = "60179";

        /// <summary>
        /// Address list in Work Location.
        /// </summary>
        public const string AddressListInWorkLocation = "60108";

        /// <summary>
        /// The grid view id.
        /// </summary>
        public const string AssociatedParcelList = "60191";

        #endregion List

        #region Others

        /// <summary>
        /// Examination Schedule:60131.
        /// </summary>
        public const string ExaminationSchedule = "60131";

        /// <summary>
        /// Check Examination Schedule available:60132.
        /// </summary>
        public const string CheckexaminationSchedule = "60132";

        /// <summary>
        /// trust account report button view:50007
        /// </summary>
        public const string TrustAccoutReportButton = "50007";

        /// <summary>
        /// GView element id for CAP Detail.
        /// </summary>
        public const string CapDetail = "60099";

        /// <summary>
        /// food facility detail 
        /// </summary>
        public const string FoodFacilityDetail = "60118";

        /// <summary>
        /// Licensee Detail:60127
        /// </summary>
        public const string LicenseeDetail = "60127";

        #endregion Others

        /// <summary>
        /// Gets the view Id list that exist the generic template.
        /// </summary>
        /// <value>The need generic template view Id list.</value>
        public static List<string> ExistGenericTemplateViewIds
        {
            get
            {
                return new List<string>()
                {
                    AddReferenceContactForm,
                    AuthAgentCustomerDetail,
                    AuthAgentEditClerkContactForm,
                    AuthAgentNewClerkContactForm,
                    Attachment,
                    ContactEdit,
                    ContinuingEducationEdit,
                    EducationEdit,
                    ExaminationEdit,
                    PeopleAttachment,
                    ModifyReferenceContactForm,
                    RegistrationContactForm,
                    RefContactEducationEdit,
                    RefContactExaminationEdit,
                    RefContactContinuingEducationEdit
                };
            }
        }
    }
}
