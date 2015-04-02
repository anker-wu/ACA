#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XPolicyConstant.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 *  XPolicy constant define
 * </pre>
 */
#endregion Header

namespace Accela.ACA.Common
{
    /// <summary>
    /// The constant for XPolicy
    /// </summary>
    public static class XPolicyConstant
    {
        #region ACA_CONFIGS

        /// <summary>
        /// aca announcement interval.
        /// </summary>
        public const string ANNOUNCEMENT_INTERVAL = "ANNOUNCEMENT_INTERVAL";

        /// <summary>
        /// aca enable Enable Announcement 
        /// </summary>
        public const string ACA_ENABLE_ANNOUNCEMENT = "ENABLE_ANNOUNCEMENT";

        /// <summary>
        /// Auto-update examination by CSV
        /// </summary>
        public const string ENABLE_AUTO_UPDATE_EXAM_BY_CSV = "ENABLE_AUTO_UPDATE_EXAM_BY_CSV";

        /// <summary>
        /// auto-update the uploaded inspection result
        /// </summary>
        public const string ENABLE_AUTO_UPDATE_INSPECTION_RESULT = "ENABLE_AUTO_UPDATE_INSPECTION_RESULT";

        /// <summary>
        /// Indicating whether the reference contact search is enabled.
        /// </summary>
        public const string ENABLE_REFERENCE_CONTACT_SEARCH = "ENABLE_REFERENCE_CONTACT_SEARCH";

        /// <summary>
        /// Indicating whether the reference licensed professional search is enabled.
        /// </summary>
        public const string ENABLE_REFERENCE_LP_SEARCH = "ENABLE_REFERENCE_LP_SEARCH";

        /// <summary>
        /// Indicating whether the registration captcha is enabled.
        /// </summary>
        public const string ENABLE_CAPTCHA_FOR_REGISTRATION = "ENABLE_CAPTCHA_FOR_REGISTRATION";

        /// <summary>
        /// Indicating whether the login captcha is enabled.
        /// </summary>
        public const string ENABLE_CAPTCHA_FOR_LOGIN = "ENABLE_CAPTCHA_FOR_LOGIN";

        /// <summary>
        /// ACA enable account attachment
        /// </summary>
        public const string ENABLE_ACCOUNT_ATTACHMENT = "ENABLE_ACCOUNT_ATTACHMENT";

        /// <summary>
        /// ACA edit contact address
        /// </summary>
        public const string ENABLE_CONTACT_ADDRESS_EDIT = "ENABLE_CONTACT_ADDRESS_EDIT";

        /// <summary>
        /// ACA enable manual contact association
        /// </summary>
        public const string ENABEL_MANUAL_CONTACT_ASSOCIATION = "ENABEL_MANUAL_CONTACT_ASSOCIATION";

        /// <summary>
        /// ACA automatically activate new association
        /// </summary>
        public const string AUTO_ACTIVATE_NEW_ASSOCIATED_CONTACT = "AUTO_ACTIVATE_NEW_ASSOCIATED_CONTACT";

        /// <summary>
        /// enable contact address maintenance
        /// </summary>
        public const string ENABLE_CONTACT_ADDRESS_MAINTENANCE = "ENABLE_CONTACT_ADDRESS_MAINTENANCE";

        /// <summary>
        /// Permission control for social media buttons.
        /// </summary>
        public const string SOCIAL_MEDIA_SHARE_BUTTON_PERMISSION = "SOCIAL_MEDIA_SHARE_BUTTON_PERMISSION";

        /// <summary>
        /// Determine who can schedule inspections
        /// </summary>
        public const string INSPECTION_PERMISSION_USER_ROLES = "INSPECTION_PERMISSION_USER_ROLES";

        /// <summary>
        /// the policy name for payment adapter
        /// </summary>
        public const string EPAYMENT_ADAPTER = "PaymentAdapterSec";

        /// <summary>
        /// Indicating whether the authorized agent/clerk can edit the customer information or not.
        /// </summary>
        public const string AUTH_AGENT_CUSTOMER_EDITABLE = "AUTH_AGENT_CUSTOMER_EDITABLE";

        /// <summary>
        /// A XPolicy Key to indicate whether enable the contact address deactivate.
        /// </summary>
        public const string ENABLE_CONTACT_ADDRESS_DEACTIVATE = "ENABLE_CONTACT_ADDRESS_DEACTIVATE"; 

        /// <summary>
        /// Indicating whether enable account Education, Examination and Continuing Education Input.
        /// </summary>
        public const string ENABLE_ACCOUNT_EDU_EXAM_CE_INPUT = "ENABLE_ACCOUNT_EDU_EXAM_CE_INPUT";

        /// <summary>
        /// The clerk registration contact type filter
        /// </summary>
        public const string CONTACT_TYPE_REGISTERATION_CLERK = "CONTACT_TYPE_REGISTERATION_CLERK";

        /// <summary>
        /// The registration contact type filter
        /// </summary>
        public const string CONTACT_TYPE_REGISTERATION = "CONTACT_TYPE_REGISTERATION";

        /// <summary>
        /// The association contact type filter
        /// </summary>
        public const string CONTACT_TYPE_ASSOCICATION = "CONTACT_TYPE_ASSOCICATION";

        /// <summary>
        /// The clerk registration contact type filter
        /// </summary>
        public const string CONTACT_TYPE_CUSTOMERDETAIL = "CONTACT_TYPE_CUSTOMERDETAIL";

        /// <summary>
        /// Indicating whether the email address page and security question page into one page is enabled.
        /// </summary>
        public const string ENABLE_RESETPASSWORD_ON_COMBINE = "ENABLE_RESETPASSWORD_ON_COMBINE";

        /// <summary>
        /// Indicating whether the registration login is enabled.
        /// </summary>
        public const string ENABLE_LOGIN_ON_REGISTRATION = "ENABLE_LOGIN_ON_REGISTRATION";

        /// <summary>
        /// The switch display a default contact when scheduling inspection
        /// </summary>
        public const string INSPECITON_DISPLAY_DEFAULT_CONTACT = "INSPECTION_DISPLAY_DEFAULT_CONTACT";

        /// <summary>
        /// This key is using to define the timeout warning time.
        /// </summary>
        public const string WARNING_TIMEOUT = "WARNING_TIMEOUT";

        /// <summary>
        /// The Search for Trade Name Cap Status
        /// </summary>
        public const string RECORD_STATUS_SEARCH_FOR_TRADENAME = "RECORD_STATUS_SEARCH_FOR_TRADENAME";

        /// <summary>
        /// Indicating use new template or classic template.
        /// </summary>
        public const string ENABLE_NEW_TEMPLATE = "ENABLE_NEW_TEMPLATE";

        /// <summary>
        /// The enable get contact cross agency
        /// </summary>
        public const string ENABLE_GET_CONTACT_FROM_OTHER_AGENCY = "ENABLE_GET_CONTACT_FROM_OTHER_AGENCY";

        /// <summary>
        /// Color theme
        /// </summary>
        public const string COLOR_THEME = "COLOR_THEME";

        /// <summary>
        /// ACA enable to display license state.
        /// </summary>
        public const string ACA_ENABLE_LICENSESTATE = "ENABLE_LICENSE_STATE";

        /// <summary>
        /// aca enable shopping cart
        /// </summary>
        public const string ACA_ENABLE_SHOPPING_CART = "ENABLE_SHOPPING_CART";

        /// <summary>
        /// enable document type filter.
        /// </summary>
        public const string ACA_ENABLE_DOCUMENT_TYPE_FILTER = "ENABLE_DOCUMENT_TYPE_FILTER";

        /// <summary>
        /// aca enable Enable Proxy user
        /// </summary>
        public const string ACA_ENABLE_PROXYUSER = "ENABLE_PROXYUSER";

        /// <summary>
        /// ACA enable registration remove license.
        /// </summary>
        public const string ACA_DISABLE_REGISTRATION_REMOVE_LICENSE = "DISABLE_REGISTRATION_REMOVE_LICENSE";

        /// <summary>
        /// standard choice for display pay fee link to all aca user.
        /// </summary>
        public const string PAY_FEE_LINK_DISP = "PAY_FEE_LINK_DISP";

        /// <summary>
        /// Enable clone function to all aca user.
        /// </summary>
        public const string ENABLE_CLONE_RECORD = "ENABLE_CLONE_RECORD";

        /// <summary>
        /// enable LicenseBoard required.
        /// </summary>
        public const string ENABLE_LICENSINGBOARD_REQUIRED = "Enable_LicensingBoard_Required";

        /// <summary>
        /// standard choice for search by asi additional criteria.
        /// </summary>
        public const string ENABLE_SEARCHASI_ADDITIONALCRITERIA = "Enable_SearchASI_AdditionalCriteria";

        /// <summary>
        /// standard choice for search by contact template additional criteria.
        /// </summary>
        public const string ENABLE_SEARCHCONTACT_ADDITIONALCRITERIA = "Enable_SearchContact_AdditionalCriteria";

        /// <summary>
        /// standard choice item for support Fein Mask
        /// </summary>
        public const string ITEM_ENABLE_FEIN_MASKING = "ENABLE_FEIN_MASKING";

        /// <summary>
        /// (Feature:09ACC-08040_Board_Type_Selection) the flag indicating whether Board Type Selection mode
        /// is enabled for the specified module
        /// </summary>
        public const string ENABLE_BOARD_TYPE_SELECTION = "ENABLE_BOARD_TYPE_SELECTION";

        /// <summary>
        /// ACA enable accessibility
        /// </summary>
        public const string ACA_ENABLE_ACCESSIBILITY = "ENABLE_ACCESSIBILITY";

        #endregion

        #region DATA2 Key items

        /// <summary>
        /// A XPolicy key to indicates the "Look Up by Owner" item in the APO look type list.
        /// This item initialized in STANDARDDATA by XPolicy.CSV
        /// </summary>
        public const string APO_LOOKUP_ITEM_DATA2_LOOK_UP_BY_OWNER = "Look Up by Owner";

        #endregion

        /// <summary>
        /// Determine the section is expanded or not.
        /// </summary>
        public const string IS_EXPANDED_SECTION = "EXPANDED_SECTION_TITLE";

        /// <summary>
        /// standard choice for contact type restriction.
        /// </summary>
        public const string CONTACT_TYPE_RESTRICTION_BY_MODULE = "Contact_Type_By_Module";
    }
}
