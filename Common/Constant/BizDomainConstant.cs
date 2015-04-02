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
 *  BizDomain contant define
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
    /// The max length for standard choice items.
    /// </summary>
    public struct StandardChoiceMaxLength
    {
        /// <summary>
        /// max length for states in play pay(STATES)
        /// </summary>
        public const int MAX_LENGTH_PlANPAY_STATES = 2;

        /// <summary>
        /// max length for asset group
        /// </summary>
        public const int MAX_LENGTH_ASSET_GROUP = 100;

        /// <summary>
        /// max length for street direction(STREET DIRECTIONS)
        /// </summary>
        public const int MAX_LENGTH_STREET_DIRECTION = 2;

        /// <summary>
        /// max length for construction type(CENSUS_BUREAU_CONSTRUCTION_TYPE_CODE)
        /// </summary>
        public const int MAX_LENGTH_CONSTRUCTION_TYPE = 4;

        /// <summary>
        /// max length for unit types(UNIT TYPES)
        /// </summary>
        public const int MAX_LENGTH_UNIT_TYPES = 6;

        /// <summary>
        /// max length for contact methods(CONTACT_METHODS)
        /// </summary>
        public const int MAX_LENGTH_CONTACT_METHODS = 15;

        /// <summary>
        /// max length for LP type(LICENSED PROFESSIONAL TYPE)
        /// </summary>
        public const int MAX_LENGTH_LP_TYPE = 255;

        /// <summary>
        /// max length for education degree(EDUCATION_DEGREE)
        /// </summary>
        public const int MAX_LENGTH_EDUCATION_DEGREE = 30;

        /// <summary>
        /// max length for salutation(SALUTATION)
        /// </summary>
        public const int MAX_LENGTH_SALUTATION = 30;

        /// <summary>
        /// max length for contact type(CONTACT TYPE)
        /// </summary>
        public const int MAX_LENGTH_CONTACT_TYPE = 255;

        /// <summary>
        /// max length for street suffixes(STREET SUFFIXES)
        /// </summary>
        public const int MAX_LENGTH_STREET_SUFFIXES = 30;

        /// <summary>
        /// max length for states(STATES)
        /// </summary>
        public const int MAX_LENGTH_STATES = 30;

        /// <summary>
        /// max length for payment check type(PAYMENT_CHECK_TYPE)
        /// </summary>
        public const int MAX_LENGTH_PAYMENT_CHECK_TYPE = 30;

        /// <summary>
        /// max length for credit card type(PAYMENT_CREDITCARD_TYPE)
        /// </summary>
        public const int MAX_LENGTH_PAYMENT_CREDITCARD_TYPE = 30;

        /// <summary>
        /// max length for subdivisions(APO_SUBDIVISIONS)
        /// </summary>
        public const int MAX_LENGTH_SUBDIVISIONS = 240;

        /// <summary>
        /// max length for licensing board(LICENSING BOARD)
        /// </summary>
        public const int MAX_LENGTH_LICENSING_BOARD = 255;

        /// <summary>
        /// max length for security question(SECURITY_QUESTIONS)
        /// </summary>
        public const int MAX_LENGTH_SECURITY_QUESTIONS = 255;
    }
    
    /// <summary>
    /// The constant define for BizDomain
    /// </summary>
    public static class BizDomainConstant
    {
        /// <summary>
        /// standard choice item indicating expand all steps for customization
        /// </summary>
        public const string STD_ITEM_EXPAND_BREADCRUMB_BAR = "EXPAND_BREADCRUMB_BAR";

        /// <summary>
        /// Defined a high priority document permission for all public users.
        /// Note:
        /// This is one special standard choice and came from production case 11ACC-04992.
        /// 1. When "View" is config as "Yes", it indicates the document is always viewable for all ACA users.
        /// 2. When "Download" is config as "Yes", it indicates the document is always downloadable for all ACA users.
        /// 3. When "Delete" is config as "Yes", it indicates the document is always deletable for all ACA users.
        /// </summary>
        public const string STD_ALL_USER_DOCUMENT_PERMISSION = "ALL_USER_DOCUMENT_PERMISSION";

        /// <summary>
        /// standard choice for cap filter user type 
        /// </summary>
        public const string STD_CAP_FILTER_USERTYPE = "CAP_FILTER_USERTYPE";

        /// <summary>
        /// standard choice for ACA config list
        /// </summary>
        public const string STD_CAT_ACA_CONFIGS = "ACA_CONFIGS";

        /// <summary>
        /// standard choice item indicating display quick query
        /// </summary>
        public const string STD_ITEM_DISPLAY_QUICK_QUERY = "DISPLAY_QUICK_QUERY";

        /// <summary>
        /// standard choice for ACA config links 
        /// </summary>
        public const string STD_CAT_ACA_CONFIGS_LINKS = "ACA_CONFIGS_LINKS";

        /// <summary>
        /// standard choice for ACA config links for upload inspection result
        /// </summary>
        public const string STD_CAT_ACA_CONFIGS_LINKS_UPLOAD_INSPECTION = "APO_UploadInspectionResult";

        /// <summary>
        /// standard choice for ACA config tabs 
        /// </summary>
        public const string STD_CAT_ACA_CONFIGS_TABS = "ACA_CONFIGS_TABS";

        /// <summary>
        /// standard choice for for auto sync people
        /// </summary>
        public const string STD_AUTO_SYNC_PEOPLE = "AUTO_SYNC_PEOPLE";

        /// <summary>
        ///  standard choice for ACA filter cap by license 
        /// </summary>
        public const string STD_CAT_ACA_FILTER_CAP_BY_LICENSE = "ACA_FILTER_CAP_BY_LICENSE";

        /// <summary>
        /// standard choice for ACA page picker 
        /// </summary>
        public const string STD_CAT_ACA_PAGE_PICKER = "ACA_PAGE_PICKER";

        /// <summary>
        /// standard choice for APO lookup type 
        /// </summary>
        public const string STD_CAT_APO_LOOKUP_TYPE = "ACA_APO_LOOKUP";

        /// <summary>
        /// standard choice for APO subdivisions 
        /// </summary>
        public const string STD_CAT_APO_SUBDIVISIONS = "APO_SUBDIVISIONS";

        /// <summary>
        /// standard choice for applicant relation 
        /// </summary>
        public const string STD_CAT_APPLICANT_RELATION = "APPLICANT RELATION";

        /// <summary>
        /// standard choice for cap payment type
        /// </summary>
        public const string STD_CAT_CAP_PAYMENT_TYPE = "ACA_CAP_PAYMENT";

        /// <summary>
        /// standard choice for cap search type
        /// </summary>
        public const string STD_CAT_CAP_SEARCH_TYPE = "ACA_CAP_SEARCH";

        /// <summary>
        /// standard choice for condition status
        /// </summary>
        public const string STD_CAT_CONDITION_STATUS = "CONDITION STATUS";

        /// <summary>
        /// standard choice for conditions of approval status
        /// </summary>
        public const string STD_CAT_CONDITIONS_OF_APPROVAL_STATUS = "STATUS_OF_CONDITIONS_OF_APPROVAL";

        /// <summary>
        /// standard choice for conditions of condition group
        /// </summary>
        public const string STD_CAT_CONDITIONS_GROUP = "CONDITION GROUP";

        /// <summary>
        /// standard choice for construction type
        /// </summary>
        public const string STD_CAT_CONSTUCTION_TYPE = "CENSUS_BUREAU_CONSTRUCTION_TYPE_CODE";

        /// <summary>
        /// standard choice for contact address type.
        /// </summary>
        public const string STD_CAT_CONTACT_ADDRESS_TYPE = "CONTACT_ADDRESS_TYPE";

        /// <summary>
        /// standard choice for RACE
        /// </summary>
        public const string STD_CAT_RACE = "RACE";

        /// <summary>
        /// standard choice for auto invoice module.
        /// </summary>
        public const string STD_CAT_AUTO_INVOICE_MODULE = "AUTO_INVOICE_MODULE";

        /// <summary>
        /// standard choice for security setting for ACA.
        /// </summary>
        public const string STD_CAT_SECURITY_SETTING = "ACA_SECURITY_SETTING";

        /// <summary>
        /// standard choice item for enable url referrer check.
        /// </summary>
        public const string STD_ITEM_SECURITY_SETTING_ENABLE_URL_REFERER_CHECK = "ENABLE_URL_REFERER_CHECK";

        /// <summary>
        /// standard choice item for trusted sites
        /// </summary>
        public const string STD_ITEM_SECURITY_SETTING_TRUSTED_SITES = "TRUSTED_SITES";

        /// <summary>
        /// standard choice for contact methods 
        /// </summary>
        public const string STD_CAT_CONTACT_METHODS = "CONTACT_METHODS";

        /// <summary>
        /// standard choice for contact type
        /// </summary>
        public const string STD_CAT_CONTACT_TYPE = "CONTACT TYPE";

        /// <summary>
        /// standard choice for country 
        /// </summary>
        public const string STD_CAT_COUNTRY = "COUNTRY";

        /// <summary>
        /// standard choice for country IDD 
        /// </summary>
        public const string STD_CAT_COUNTRY_IDD = "PHONE_NUMBER_IDD";

        /// <summary>
        /// standard choice for credit card post back data
        /// </summary>
        public const string STD_CAT_CREDIT_CARD_POST_BACK_DATA = "ACA_CREDIT_CARD_POST_BACK_DATA";

        /// <summary>
        /// standard choice for credit card url parameter 
        /// </summary>
        public const string STD_CAT_CREDIT_CARD_URL_PARAMETER = "ACA_CREDIT_CARD_URL_PARAMETER";

        /// <summary>
        /// standard choice for default job value
        /// </summary>
        public const string STD_CAT_DEFAULT_JOB_VALUE = "DEFAULT_JOB_VALUE";

        /// <summary>
        /// standard choice for payment config 
        /// </summary>
        public const string STD_CAT_EPAYMENT_CONFIG = "EPaymentAdapter";

        /// <summary>
        /// standard choice for find app date range 
        /// </summary>
        public const string STD_CAT_FIND_APP_DATE_RANGE = "FIND APP DATE RANGE";

        /// <summary>
        /// standard choice for gender 
        /// </summary>
        public const string STD_CAT_GENDER = "GENDER";

        /// <summary>
        /// standard choice for i18n settings
        /// </summary>
        public const string STD_CAT_I18N_SETTINGS = "I18N_SETTINGS";

        /// <summary>
        /// standard choice for i18n address format
        /// </summary>
        public const string STD_CAT_I18N_ADDRESS_FORMAT = "I18N_ADDRESS_FORMAT";

        /// <summary>
        /// standard choice for inspection config 
        /// </summary>
        public const string STD_CAT_INSPECTION_CONFIGS = "INSPECTION_SETTING";

        /// <summary>
        /// standard choice for license type 
        /// </summary>
        public const string STD_CAT_LICENSE_TYPE = "LICENSED PROFESSIONAL TYPE";

        /// <summary>
        /// standard choice for degree type
        /// </summary>
        public const string STD_CAT_EDUCATION_DEGREE_TYPE = "EDUCATION_DEGREE";

        /// <summary>
        /// standard choice for multiple service settings 
        /// </summary>
        public const string STD_CAT_MULTI_SERVICE_SETTINGS = "MULTI_SERVICE_SETTINGS";

        /// <summary>
        /// standard choice for template EMSE drop down list
        /// </summary>
        public const string STD_CAT_TEMPLATE_EMSE_DROPDOWN = "TEMPLATE_EMSE_DROPDOWN";

        /// <summary>
        /// standard choice for online payment web service
        /// </summary>
        public const string STD_CAT_ONLINE_PAYMENT_WEBSERVICE = "ACA_ONLINE_PAYMENT_WEBSERVICE";

        /// <summary>
        /// standard choice for parcel mask 
        /// </summary>
        public const string STD_CAT_PARCEL_MASK = "MASKS";

        /// <summary>
        /// standard choice for payment check account type 
        /// </summary>
        public const string STD_CAT_PAYMENT_CHECK_ACCOUNT_TYPE = "PAYMENT_CHECK_ACCOUNT_TYPE";

        /// <summary>
        /// standard choice for payment check type 
        /// </summary>
        public const string STD_CAT_PAYMENT_CHECK_TYPE = "PAYMENT_CHECK_TYPE";

        /// <summary>
        /// standard choice for payment credit card type 
        /// </summary>
        public const string STD_CAT_PAYMENT_CREDITCARD_TYPE = "PAYMENT_CREDITCARD_TYPE";

        /// <summary>
        /// standard choice for payment max fee
        /// </summary>
        public const string STD_CAT_ONLINE_PAYMENT_MAX_AMOUNT = "ONLINE_PAYMENT_MAX_AMOUNT";

        /// <summary>
        /// standard choice for phone number IDD enable 
        /// </summary>
        public const string STD_CAT_PHONE_NUMBER_IDD_ENABLE = "PHONE_NUMBER_IDD_ENABLE";

        /// <summary>
        /// standard choice for plan review pay fee 
        /// </summary>
        public const string STD_CAT_PLANREVIEW_PAY_FEE = "PLANREVIEW_PAY_FEE";

        /// <summary>
        /// standard choice for reference address type
        /// </summary>
        public const string STD_CAT_REF_ADDRESS_TYPE = "REF_ADDRESS_TYPE";

        /// <summary>
        /// standard choice for role list
        /// </summary>
        public const string STD_CAT_ROLE_LIST = "ACA_RESTRICT_ROLE";

        /// <summary>
        /// standard choice for salutation
        /// </summary>
        public const string STD_CAT_SALUTATION = "SALUTATION";

        /// <summary>
        /// standard choice for self plan rule set 
        /// </summary>
        public const string STD_CAT_SELF_PLAN_RULESET = "SELF_PLAN_RULESET";

        /// <summary>
        /// standard choice for state type
        /// </summary>
        public const string STD_CAT_STATE_TYPE = "STATES";

        /// <summary>
        /// standard choice for licensing board
        /// </summary>
        public const string STD_CAT_LICENSING_BOARD = "LICENSING BOARD";

        /// <summary>
        /// standard choice for street directions 
        /// </summary>
        public const string STD_CAT_STREET_DIRECTIONS = "STREET DIRECTIONS";

        /// <summary>
        /// standard choice for asset group 
        /// </summary>
        public const string STD_CAT_ASSET_GROUP = "ASSET_GROUP";

        /// <summary>
        /// standard choice for street suffixes
        /// </summary>
        public const string STD_CAT_STREET_SUFFIXES = "STREET SUFFIXES";

        /// <summary>
        /// standard choice for unit types 
        /// </summary>
        public const string STD_CAT_UNIT_TYPES = "UNIT TYPES";

        /// <summary>
        /// standard choice for continuing education required hours.
        /// </summary>
        public const string STD_CONTINUING_EDUCATION_REQUIRED_HOURS = "CONTINUING_EDUCATION_REQUIRED_HOURS";

        /// <summary>
        /// standard choice for standard people attribute unit.
        /// </summary>
        public const string STD_PEOPLE_ATTRIBUTE_UNIT = "PEOPLE ATTRIBUTE UNIT";

        /// <summary>
        /// standard choice for variable config 
        /// </summary>
        public const string STD_CAT_VARIABLE_CONFIG = "ReportVariableSec";

        /// <summary>
        /// standard choice for enable contact type filtering by module
        /// </summary>
        public const string STD_ENABLE_CONTACT_TYPE_FILTERING_BY_MODULE = "ENABLE_CONTACT_TYPE_FILTERING_BY_MODULE";

        /// <summary>
        /// standard choice for display map for 'Use map to select work location'
        /// </summary>
        public const string STD_DISPLAY_MAP_FOR_SELECTOBJECT = "DISPLAY_MAP_FOR_SELECTOBJECT";

        /// <summary>
        /// standard choice for display map for 'Show on Map'
        /// </summary>
        public const string STD_DISPLAY_MAP_FOR_SHOWOBJECT = "DISPLAY_MAP_FOR_SHOWOBJECT";

        /// <summary>
        /// standard choice for display optional inspections
        /// </summary>
        public const string STD_DISPLAY_OPTIONAL_INSPECTIONS = "DISPLAY_OPTIONAL_INSPECTIONS";

        /// <summary>
        /// standard choice for enable anonymous report 
        /// </summary>
        public const string STD_ENABLE_ANONYMOUS_REPORT = "ENABLE_ANONYMOUS_REPORT";

        /// <summary>
        /// standard choice for enable contact address.
        /// </summary>
        public const string STD_ENABLE_CONTACT_ADDRESS = "ENABLE_CONTACT_ADDRESS";

        /// <summary>
        /// standard choice for enable contact address validation.
        /// </summary>
        public const string STD_ENABLE_CONTACT_ADDRESS_VALIDATION = "ENABLE_CONTACT_ADDRESS_VALIDATION";

        /// <summary>
        /// standard choice for enable browser detect.
        /// </summary>
        public const string STD_ITEM_ENABLE_BROWSER_DETECT = "ENABLE_BROWSER_DETECT";

        /// <summary>
        ///  standard choice for inspection calendar name 
        /// </summary>
        public const string STD_INSPECTION_CALENDAR_NAME = "INSPECTION_CALENDAR_NAME";

        /// <summary>
        /// standard choice for inspection display option 
        /// </summary>
        public const string STD_INSPECTION_DISPLAYOPTION = "INSPECTION_DISPLAYOPTION";

        /// <summary>
        /// standard choice item for ASI label width
        /// </summary>
        public const string STD_ITEM_ASI_LABEL_WIDTH = "ASI_LABEL_WIDTH";

        /// <summary>
        /// standard choice item to indicate which field in ASIT to be used remember the child Record ID
        /// </summary>
        public const string ASSOCIATED_FORMS_ASIT_RELATIONSHIP_FIELD = "ASSOCIATED_FORMS_ASIT_RELATIONSHIP_FIELD";

        /// <summary>
        /// standard choice item for ACA Default Country
        /// </summary>
        public const string STD_ITEM_DEFAULT_COUNTRY = "ACA_DEFAULT_COUNTRY";

        /// <summary>
        /// standard choice item for ACA adapter type 
        /// </summary>
        public const string STD_ITEM_ACA_ADAPTER_TYPE = "ACAAdapterType";

        /// <summary>
        /// standard choice item for enable/disable template emse drop down list 
        /// </summary>
        public const string STD_ITEM_TEMPLATE_EMSE_ENABLE = "ENABLE_EMSE";

        /// <summary>
        /// standard choice item for ACA log visible 
        /// </summary>
        public const string STD_ITEM_ACA_LOG_VISIBLE = "STD_ITEM_ACA_LOG_VISIBLE";

        /// <summary>
        /// standard choice item for allow exporting to CSV
        /// </summary>
        public const string STD_ITEM_ALLOW_EXPORTING_TO_CSV = "ALLOW_EXPORTING_TO_CSV";

        /// <summary>
        /// standard choice for disable expression alert
        /// </summary>
        public const string STD_DISABLE_EXPRESSION_ALERT = "DISABLE_EXPRESSION_ALERT";

        /// <summary>
        /// standard choice item for defer payment enabled 
        /// </summary>
        public const string STD_ITEM_DEFER_PAYMENT_ENABLED = "DEFER_PAYMENT_ENABLED";

        /// <summary>
        /// standard choice item for continue shopping enabled 
        /// </summary>
        public const string STD_ITEM_CONTINUE_SHOPPING_ENABLE = "CONTINUE_SHOPPING_ENABLE";

        /// <summary>
        /// standard choice item for disallowed file type 
        /// </summary>
        public const string STD_ITEM_DISALLOWED_FILE_TYPES = "ACA_EDMS_DISALLOWED_FILE_TYPES";

        /// <summary>
        /// standard choice item for cap detail section roles 
        /// </summary>
        public const string STD_ITEM_CAPDETAIL_SECTIONROLES = "CAPDETAIL_SECTIONROLES";

        /// <summary>
        /// standard choice item for inspection contact right 
        /// </summary>
        public const string STD_ITEM_INSPECTION_CONTACT_RIGHT = "INSPECTION_CONTACT_RIGHT";

        /// <summary>
        /// standard choice item for display currency symbol for ASI 
        /// </summary>
        public const string STD_ITEM_DISPLAY_CURRENCY_SYMBOL_FOR_ASI = "DISPLAY_CURRENCY_SYMBOL_FOR_ASI";

        /// <summary>
        /// standard choice item for display user initials 
        /// </summary>
        public const string STD_ITEM_DISPLAY_USER_INITIALS = "DISPLAY_USER_INITIALS";

        /// <summary>
        /// standard choice item for enable account verification email 
        /// </summary>
        public const string STD_ITEM_ENABLE_ACCOUNT_VERIFICATION_EMAIL = "ENABLE_ACCOUNT_VERIFICATION_EMAIL";

        /// <summary>
        /// standard choice item for enable filter by reference type
        /// </summary>
        public const string STD_ITEM_ENABLE_FILTER_BY_REFTYPE = "ENABLE_FILTER_BY_REFTYPE_";

        /// <summary>
        /// A standard choice item to indicates whether the LDAP authentication is enabled in ACA.
        /// </summary>
        public const string STD_ITEM_ENABLE_LDAP_AUTHENTICATION = "ENABLE_LDAP_AUTHENTICATION";

        /// <summary>
        /// standard choice item for enable unlimited comment length
        /// </summary>
        public const string STD_ITEM_ENABLE_UNLIMIT_COMMENT_LENGTH = "UNLIMITED_COMMENT_LENGTH";

        /// <summary>
        /// standard choice item for GIS portlet url 
        /// </summary>
        public const string STD_ITEM_GIS_PORLET_URL = "GIS_PORTLET_URL";

        /// <summary>
        /// standard choice item to indicate whether it is super agency 
        /// </summary>
        public const string STD_ITEM_IS_SUPER_AGENCY = "IS_SUPER_AGENCY";

        /// <summary>
        /// standard choice item for login enabled 
        /// </summary>
        public const string STD_ITEM_LOGON_ENABLED = "LOGIN_ENABLED";

        /// <summary>
        /// standard choice item for official website url 
        /// </summary>
        public const string STD_ITEM_OFFICIAL_WEBSITE_URL = "OFFICIAL_WEBSITE_URL";

        /// <summary>
        /// standard choice item for print report name 
        /// </summary>
        public const string STD_ITEM_PRINT_REPORT_NAME = "PRINT_REPORT_NAME";

        /// <summary>
        /// standard choice item for purge expired account interval
        /// </summary>
        public const string STD_ITEM_PURGE_EXPIRED_ACCOUNT_INTERVAL = "PURGE_EXPIRED_ACCOUNT_INTERVAL";

        /// <summary>
        /// standard choice item for registration enabled
        /// </summary>
        public const string STD_ITEM_REGISTRATION_ENABLED = "REGISTRATION_ENABLED";

        /// <summary>
        /// standard choice item for account management enabled.
        /// </summary>
        public const string STD_ITEM_ACCOUNT_MANAGEMENT_ENABLED = "ACCOUNT_MANAGEMENT_ENABLED";

        /// <summary>
        /// standard choice item for registration license enabled
        /// </summary>
        public const string STD_ITEM_REGISTRATION_LICENSE_ENABLED = "REGISTRATION_LICENSE_ENABLED";

        /// <summary>
        /// Gets a value indicating whether only show auto invoiced fee items.
        /// </summary>
        public const string STD_ITEM_ENABLE_AUTO_INVOICE = "ENABLE_AUTO_INVOICE";

        /// <summary>
        /// standard choice item for remove pay fee
        /// </summary>
        public const string STD_ITEM_REMOVE_PAY_FEE = "REMOVE_PAY_FEE";

        /// <summary>
        /// standard choice item for support decimal quantity
        /// </summary>
        public const string STD_ITEM_SUPPORT_DECIMAL_QUANTITY = "SUPPORT_DECIMAL_QUANTITY";

        /// <summary>
        /// standard choice for fee quantity accuracy
        /// </summary>
        public const string STD_FEE_QUANTITY_ACCURACY = "FEE_QUANTITY_ACCURACY";

        /// <summary>
        /// standard choice for conditions of approval
        /// </summary>
        public const string STD_CONDITIONS_OF_APPROVALS = "CONDITIONS_OF_APPROVALS";

        /// <summary>
        /// standard choice for condition priorities
        /// </summary>
        public const string STD_CONDITION_PRIORITIES = "CONDITION_PRIORITIES";

        /// <summary>
        /// standard choice item for report name
        /// </summary>
        public const string STD_ITEM_REPORT_NAME = "REPORT_NAME";

        /// <summary>
        /// standard choice item for session timeout 
        /// </summary>
        public const string STD_ITEM_SESSION_TIMEOUT = "SESSION_TIMEOUT";

        /// <summary>
        /// standard choice item for V360 web action password 
        /// </summary>
        public const string STD_ITEM_V360_WEB_ACTION_PASSWORD = "V360_WEB_ACTION_PASSWORD";

        /// <summary>
        /// standard choice item for V360 web action username
        /// </summary>
        public const string STD_ITEM_V360_WEB_ACTION_USERNAME = "V360_WEB_ACTION_USERNAME";

        /// <summary>
        /// standard choice for multiple inspections enabled.
        /// </summary>
        public const string STD_MULTIPLE_INSPECTIONS_ENABLED = "MULTIPLE_INSPECTIONS_ENABLED";

        /// <summary>
        /// standard choice for no save and resume later button module 
        /// </summary>
        public const string STD_NO_SAVEANDRESUMELATER_BUTTON_MODULE = "NO_SAVEANDRESUMELATER_BUTTON_MODULE";

        /// <summary>
        /// standard choice for spell checker enabled
        /// </summary>
        public const string STD_SPELL_CHECKER_ENABLED = "ACA_SPELL_CHECKER_ENABLED";

        /// <summary>
        /// standard choice for enable expired license.
        /// </summary>
        public const string STD_CAT_ENABLE_EXPIRED_LICENSE = "ENABLE_EXPIRED_LICENSE";

        /// <summary>
        /// standard choice item for enable expired license.
        /// </summary>
        public const string STD_ITEM_ENABLE_EXPIRED_LICENSE = "ENABLE_EXPIRED";

        /// <summary>
        /// standard choice item for enable notify expired license at login.
        /// </summary>
        public const string STD_ITEM_ENABLE_NOTIFY_EXPIRED_LICENSE_AT_LOGIN = "ENABLE_NOTIFY_LICENSE_EXPIRED_AT_LOGIN";

        /// <summary>
        /// standard choice item for enable notify expired insurance at login.
        /// </summary>
        public const string STD_ITEM_ENABLE_NOTIFY_EXPIRED_INSURANCE_AT_LOGIN = "ENABLE_NOTIFY_INSURANCE_EXPIRED_AT_LOGIN";

        /// <summary>
        /// standard choice item for enable notify expired business license at login.
        /// </summary>
        public const string STD_ITEM_ENABLE_NOTIFY_EXPIRED_BUSINESS_LICENSE_AT_LOGIN = "ENABLE_NOTIFY_BUSINESS_LICENSE_EXPIRED_AT_LOGIN";

        /// <summary>
        /// standard choice item for Display Request Trade License Filter in ACA Admin
        /// </summary>
        public const string STD_ITEM_DISPLAY_REQUEST_TRADE_LICENSE_FILTER = "DISPLAY_REQUEST_TRADE_LICENSE_FILTER";

        /// <summary>
        /// standard choice for exam CSV format
        /// </summary>
        public const string STD_EXAM_CSV_FORMAT = "EXAM_CSV_FORMAT";

        /// <summary>
        /// standard choice for valuation calculator regional modifier
        /// </summary>
        public const string STD_REGIONAL_MODIFIER = "REGIONAL MODIFIER";

        /// <summary>
        /// standard choice for exam CSV format
        /// </summary>
        public const string STD_LICENSE_AUTO_APPROVED = "ACA_CONNECT_LICENSE_AUTO_APPROVED";

        /// <summary>
        /// Gets a value indicating whether to enable customization per page.
        /// </summary>
        public const string STD_ITEM_ENABLE_CUSTOMIZATION_PER_PAGE = "ENABLE_CUSTOMIZATION_PER_PAGE";

        /// <summary>
        /// standard choice for password security settings for password security.
        /// </summary>
        public const string STD_CAT_PASSWORD_POLICY_SETTINGS = "PASSWORD_POLICY_SETTINGS";

        /// <summary>
        /// standard choice item for enable check password security in ACA.
        /// </summary>
        public const string STD_ITEM_ENABLE_ACA_PASSWORD_CHECK = "include_aca";

        /// <summary>
        /// standard choice item for enable check password expiration in ACA.
        /// </summary>
        public const string STD_ITEM_PASSWORD_EXPRIATION_CHECK = "PASSWORD_EXPRIATION_CHECK";

        /// <summary>
        /// standard choice item for enable check password failed attempts in ACA.
        /// </summary>
        public const string STD_ITEM_PASSWORD_FAILED_ATTEMPTS_CHECK = "PASSWORD_FAILED_ATTEMPTS_CHECK";

        /// <summary>
        /// standard choice item for GVIEW element.
        /// </summary>
        public const string STD_CACHE_GVIEW_ELEMENT = "GVIEW_ELEMENT";

        /// <summary>
        /// standard choice for trust account transaction type
        /// </summary>
        public const string STD_TRANSACTION_TYPE = "TRANSACTION_TYPE";

        /// <summary>
        /// standard choice for payment processing method
        /// </summary>
        public const string STD_PAYMENT_PROCESSING_METHOD = "PAYMENT_PROCESSING_METHOD";

        /// <summary>
        /// standard choice for food facility inspection
        /// </summary>
        public const string STD_FOOD_FACILITY_INSPECTION = "FOOD_FACILITY_INSPECTION";

        /// <summary>
        /// standard choice for certified business three-digit NIGP type
        /// </summary>
        public const string STD_CERT_BUSINESS_NIGP_TYPE = "NIGP TYPE";

        /// <summary>
        /// standard choice for certified business three-digit NIGP class
        /// </summary>
        public const string STD_CERT_BUSINESS_NIGP_CLASS = "NIGP_CODE_THREE_DIGIT"; //"DEFO-NIGP Class";

        /// <summary>
        /// standard choice for certified business five-digit NIGP sub class code
        /// </summary>
        public const string STD_CERT_BUSINESS_NIGP_SUBCLASS = "NIGP_CODE_FIVE_DIGIT"; // "DEFO-NIGP Sub Class";

        /// <summary>
        /// standard choice for certified business ethnicity
        /// </summary>
        public const string STD_CERT_BUSINESS_ETHNICITY = "NIGP-Ethnicity";

        /// <summary>
        /// standard choice for certified business certification category
        /// </summary>
        public const string STD_CERT_BUSINESS_CERTIFICATION_CATEGORY = "NIGP-Classification";

        /// <summary>
        /// standard choice for certified business largest contract experience
        /// </summary>
        public const string STD_CERT_BUSINESS_LARGEST_CONTRACT_EXPERIENCE = "NIGP-Contract Experience";

        /// <summary>
        /// standard choice for certified business largest contract value
        /// </summary>
        public const string STD_CERT_BUSINESS_LARGEST_CONTRACT_AMOUNT = "NIGP-Contract Experience Amount";

        /// <summary>
        /// standard choice certified business location
        /// </summary>
        public const string STD_CERT_BUSINESS_LOCATION = "NIGP-County";

        /// <summary>
        /// standard choice for certified business zip code
        /// </summary>
        public const string STD_CERT_BUSINESS_ZIP_CODE = "ZIP_CODE";

        /// <summary>
        /// Standard choice item for Preferred Channel.
        /// </summary>
        public const string STD_CONTACT_PREFERRED_CHANNEL = "CONTACT_PREFERRED_CHANNEL";

        /// <summary>
        /// General description: It defines what modules' records can be paid together as same payment group.
        /// Related Component Payment
        /// Related Product ACA
        /// Mandatory or not: It isn't mandatory; default Behavior is agency level group, if without the standard choice.
        /// Example: Value = Module Name; Value Description = product ID of OPC or group ID
        /// </summary>
        public const string STD_CAT_PAYMENT_GROUP = "PAYMENT_GROUP";

        /// <summary>
        /// standard choice for display show more/show less lines
        /// </summary>
        public const string STD_COLLAPSE_LINES = "COLLAPSE_LINES";

        /// <summary>
        /// standard choice for reschedule or cancel examination reason
        /// </summary>
        public const string BIZDOMAIN_RESCHED_CANCEL_REASON = "REASON_FOR_RESCHEDULING_CANCELLING_EXAMINATION";

        /// <summary>
        /// standard choice for country default value.
        /// </summary>
        public const string STD_COUNTRY_DEFAULT_VALUE = "COUNTRY_DEFAULT_VALUE";

        /// <summary>
        /// standard choice for primary contact address required.
        /// </summary>
        public const string STD_PRIMARY_CONTACT_ADDRESS_REQUIRED = "PRIMARY_CONTACT_ADDRESS_REQUIRED";

        /// <summary>
        /// The standard choice authorized agent service.
        /// </summary>
        public const string STD_AUTHORIZED_SERVICE = "AUTHORIZED_SERVICE";

        /// <summary>
        /// The standard choice item for authorized agent service' module.
        /// </summary>
        public const string STD_ITEM_AUTHORIZED_SERVICE_MODULE = "Module";

        /// <summary>
        /// The standard choice item for authorized agent service' cap type filter.
        /// </summary>
        public const string STD_ITEM_AUTHORIZED_SERVICE_CAPTYPEFILTER = "CapTypeFilter";

        /// <summary>
        /// The standard choice item for authorized agent service' printers.
        /// It split with ";", for example: "DATAMAX E-4205A;DATAMAX E-4203"
        /// </summary>
        public const string STD_ITEM_AUTHORIZED_SERVICE_PRINTERS = "Printers";

        /// <summary>
        /// The standard choice item for authorized agent service' FISHING_AND_HUNTING_LICENSE_SALES.
        /// </summary>
        public const string STD_ITEM_AUTHORIZED_SERVICE_FISHING_AND_HUNTING_LICENSE_SALES = "FISHING_AND_HUNTING_LICENSE_SALES";

        /// <summary>
        /// The standard choice item for hidden state form licensee detail page.
        /// </summary>
        public const string STD_ITEM_HIDE_STATE_FROM_LICENSEE_DETAIL = "HIDE_STATE_FROM_LICENSEE_DETAIL";

        /// <summary>
        /// The standard choice for reprint reason
        /// </summary>
        public const string STD_REPRINT_REASONS = "REPRINT_REASONS";

        /// <summary>
        /// The standard choice for inspection result's CSV file' format
        /// </summary>
        public const string STD_INSPECTION_RESULT_CSV_FORMAT = "INSPECTION_RESULT_CSV_FORMAT";

        /// <summary>
        /// Gets a value  indicating whether to save data in confirm page.
        /// </summary>
        public const string STD_ITEM_CREATE_APPLICATION_MODEL = "CREATE_APPLICATION_MODEL";

        /// <summary>
        /// standard choice item indicating whether to enable expand/collapse ability for review page
        /// </summary>
        public const string STD_ITEM_ENABLE_EXPAND_REVIEW = "ENABLE_EXPAND_IN_REVIEW_PAGE";

        /// <summary>
        /// standard choice item indicating whether to display grid view owner column.
        /// </summary>
        public const string STD_DISPLAY_OWNER_INFORMATION = "DISPLAY_OWNER_INFORMATION";

        /// <summary>
        /// The standard choices item name for the mask of Mask control
        /// </summary>
        public const string STD_MASKS = "MASKS";

        /// <summary>
        /// The standard choices value for the mask of SSNMask control
        /// </summary>
        public const string STD_MASKS_ITEM_SSN = "SSN";

        /// <summary>
        /// The standard choices value for the mask of FEINMask control
        /// </summary>
        public const string STD_MASKS_ITEM_FEIN = "FEIN";

        /// <summary>
        /// Gets a value indicate action item style
        /// </summary>
        public const string STD_POP_ACTION_ITEM_STYLE = "POP_ACTION_ITEM_STYLE";

        /// <summary>
        /// Gets a value indicate document condition type filter.
        /// </summary>
        public const string DOCUMENT_CONDITION_TYPE_FILTER = "DOCUMENT_CONDITION_TYPE_FILTER";

        /// <summary>
        /// The standard choice option for ACA_AUTO_ASSIGN_INSPECTOR
        /// </summary>
        public const string STD_ACA_AUTO_ASSIGN_INSPECTOR = "ACA_AUTO_ASSIGN_INSPECTOR";

        /// <summary>
        /// The standard choices item name for ACA_AUTO_ASSIGN_INSPECTOR
        /// </summary>
        public const string STD_ITEM_BLOCK_SCHEDULE_WHEN_NO_INSPECTOR_FOUND = "BLOCK_SCHEDULE_WHEN_NO_INSPECTOR_FOUND";

        /// <summary>
        /// The standard choice item for default address type
        /// </summary>
        public const string STD_CONTACT_ADDRESS_TYPE = "DEFAULT_CONTACT_ADDRESS_TYPE";

        /// <summary>
        /// The generic template is visible for registration
        /// </summary>
        public const string STD_HIDE_CONTACT_GENERIC_TEMPLATE_FOR_CITIZEN_REGISTRATION = "HIDE_CONTACT_GENERIC_TEMPLATE_FOR_CITIZEN_REGISTRATION";

        /// <summary>
        /// The generic template is visible for clerk registration
        /// </summary>
        public const string STD_HIDE_CONTACT_GENERIC_TEMPLATE_FOR_CLERK_REGISTRATION = "HIDE_CONTACT_GENERIC_TEMPLATE_FOR_CLERK_REGISTRATION";

        /// <summary>
        /// A standard choice item to define the attachment upload behavior:
        /// Basic -- Use the new simple upload style. Not configure the standard choice will use the Basic style.
        /// Advanced -- Use the popped up upload dialog.
        /// </summary>
        public const string STD_FILE_UPLOAD_BEHAVIOR = "FILE_UPLOAD_BEHAVIOR";

        /// <summary>
        /// A standard choice item to define the attachment upload behavior:
        /// Basic -- Use the new simple upload style. Not configure the standard choice will use the Basic style.
        /// </summary>
        public const string STD_FILE_UPLOAD_BEHAVIOR_ITEM_BASIC = "Basic";

        /// <summary>
        /// A standard choice item to define the attachment upload behavior:
        /// Advanced -- Use the popped up upload dialog.
        /// </summary>
        public const string STD_FILE_UPLOAD_BEHAVIOR_ITEM_ADVANCED = "Advanced";

        /// <summary>
        /// A standard choice item to define the attachment upload behavior:
        /// Html5 -- Use the html5 upload style.
        /// </summary>
        public const string STD_FILE_UPLOAD_BEHAVIOR_ITEM_HTML5 = "Html5";

        /// <summary>
        /// The standard choice authentication by security question
        /// </summary>
        public const string STD_AUTHENTICATION_BY_SECURITY_QUESTION = "AUTHENTICATION_BY_SECURITY_QUESTION";

        /// <summary>
        /// The standard choice item enable authentication by security question
        /// </summary>
        public const string STD_ITEM_ENABLE_AUTHENTICATION_BY_SECURITY_QUESTION = "ENABLE_AUTHENTICATION_BY_SECURITY_QUESTION";

        /// <summary>
        /// The standard choice item the quantity of compulsory security question
        /// </summary>
        public const string STD_ITEM_COMPULSORY_SECURITY_QUESTION_QUANTITY = "COMPULSORY_SECURITY_QUESTION_QUANTITY";

        /// <summary>
        /// When standard choice value is “Yes”, the following data picker or date format will use Islamic calendar or Islamic format. 
        /// </summary>
        public const string STD_ENABLE_DISPLAY_ISLAMIC_CALENDAR = "ENABLE_DISPLAY_ISLAMIC_CALENDAR";

        /// <summary>
        /// The standard choice item indicating whether the address source is external.
        /// </summary>
        public const string STD_EXTERNAL_ADDRESS_SOURCE = "EXTERNAL_ADDRESS_SOURCE";

        /// <summary>
        /// The standard choice item indicating whether the parcel source is external.
        /// </summary>
        public const string STD_EXTERNAL_PARCEL_SOURCE = "EXTERNAL_PARCEL_SOURCE";

        /// <summary>
        /// The standard choice item indicating whether the owner source is external.
        /// </summary>
        public const string STD_EXTERNAL_OWNER_SOURCE = "EXTERNAL_OWNER_SOURCE";

        /// <summary>
        /// The standard choice item indicating the external source location.
        /// </summary>
        public const string STD_EXTERNAL_SOURCE_LOCATION = "SOURCE_LOCATION";

        /// <summary>
        /// The standard choice item indicating the value of the external source location.
        /// </summary>
        public const string STD_EXTERNAL_SOURCE_LOCATION_VALUE = "External";

        /// <summary>
        /// standard choice item for new GIS portlet url 
        /// </summary>
        public const string STD_ITEM_NEW_GIS_PORLET_URL = "NEW_GIS_PORTLET_URL";

        /// <summary>
        /// multiple trade name forbidden
        /// </summary>
        public const string MULTIPLE_TRADE_NAME_FORBIDDEN = "MULTIPLE_TRADE_NAME_FORBIDDEN";

        /// <summary>
        /// pa cap status after apply 
        /// </summary>
        public const string PA_CAP_STATUS_AFTER_APPLY = "PA_CAP_STATUS_AFTER_APPLY";

        /// <summary>
        /// only schedule one business day 
        /// </summary>
        public const string ONLY_SCHEDULE_ONE_BUSINESS_DAY = "ONLY_SCHEDULE_ONE_BUSINESS_DAY";

        #region Methods

        /// <summary>
        /// Gets XPolicy category names.
        /// </summary>
        /// <returns>XPolicy category names.</returns>
        public static string[] GetXPolicyCategoryNames()
        {
            return new string[] { STD_CAT_ACA_CONFIGS, STD_CAT_VARIABLE_CONFIG, XPolicyConstant.CONTACT_TYPE_RESTRICTION_BY_MODULE, XPolicyConstant.EPAYMENT_ADAPTER };
        }

        /// <summary>
        /// Gets ACA hard code for DropDownList category names.
        /// </summary>
        /// <returns>hard code list.</returns>
        public static string[] GetACAHardCodeDropDownListCategoryNames()
        {
            return new string[] { STD_CAT_APO_LOOKUP_TYPE, STD_CAT_CAP_SEARCH_TYPE, STD_CAT_CAP_PAYMENT_TYPE };
        }

        /// <summary>
        /// Gets ACA Standard Choice for CategoryNames
        /// </summary>
        /// <returns>Category Names</returns>
        public static string[] GetACAStandardChoiceCategoryNames()
        {
            return new string[]
                       {
                           STD_CAT_STREET_DIRECTIONS, STD_CAT_STREET_SUFFIXES, STD_CAT_UNIT_TYPES, STD_PEOPLE_ATTRIBUTE_UNIT, STD_CAT_CONTACT_METHODS, STD_CAT_STATE_TYPE, STD_CAT_CAP_SEARCH_TYPE, STD_CAT_CAP_PAYMENT_TYPE,
                           STD_CAT_APO_LOOKUP_TYPE, STD_CAT_EPAYMENT_CONFIG, STD_CAT_LICENSE_TYPE, STD_CAT_ACA_CONFIGS, STD_CAT_INSPECTION_CONFIGS, STD_CAT_PAYMENT_CREDITCARD_TYPE, STD_SPELL_CHECKER_ENABLED,
                           STD_CAT_PAYMENT_CHECK_TYPE, STD_CAT_CONSTUCTION_TYPE, STD_CAT_PARCEL_MASK, STD_CAT_SELF_PLAN_RULESET, STD_CAT_ACA_CONFIGS_TABS, STD_CAT_ACA_CONFIGS_LINKS, STD_CAT_APO_SUBDIVISIONS,
                           STD_CAT_FIND_APP_DATE_RANGE, STD_CAT_ACA_FILTER_CAP_BY_LICENSE, STD_CAT_DEFAULT_JOB_VALUE, STD_CAT_ONLINE_PAYMENT_WEBSERVICE, STD_CAT_PAYMENT_CHECK_ACCOUNT_TYPE, STD_CAT_CREDIT_CARD_POST_BACK_DATA,
                           STD_CAT_CREDIT_CARD_URL_PARAMETER, STD_CAT_APPLICANT_RELATION, STD_CAT_CONTACT_TYPE, STD_CAT_I18N_SETTINGS, STD_CAT_I18N_ADDRESS_FORMAT, STD_CAT_COUNTRY, STD_CAT_COUNTRY_IDD, STD_CAT_SALUTATION, STD_CAT_GENDER, STD_CAT_LICENSING_BOARD,
                           STD_CAT_REF_ADDRESS_TYPE, STD_CAT_PHONE_NUMBER_IDD_ENABLE, STD_CAT_MULTI_SERVICE_SETTINGS, STD_CAT_CONDITION_STATUS, STD_CAT_CONDITIONS_OF_APPROVAL_STATUS, STD_CAT_ACA_PAGE_PICKER, STD_CAT_EDUCATION_DEGREE_TYPE, STD_CAT_TEMPLATE_EMSE_DROPDOWN, STD_CONTINUING_EDUCATION_REQUIRED_HOURS,
                           STD_EXAM_CSV_FORMAT, STD_CAT_ENABLE_EXPIRED_LICENSE, STD_LICENSE_AUTO_APPROVED, STD_ITEM_DEFAULT_COUNTRY, STD_CAT_PASSWORD_POLICY_SETTINGS, STD_REGIONAL_MODIFIER, STD_TRANSACTION_TYPE, STD_PAYMENT_PROCESSING_METHOD, STD_FOOD_FACILITY_INSPECTION,
                           STD_CERT_BUSINESS_NIGP_TYPE, STD_CERT_BUSINESS_NIGP_CLASS, STD_CERT_BUSINESS_NIGP_SUBCLASS, STD_CERT_BUSINESS_LARGEST_CONTRACT_EXPERIENCE, STD_CERT_BUSINESS_LARGEST_CONTRACT_AMOUNT, STD_CERT_BUSINESS_CERTIFICATION_CATEGORY, STD_CERT_BUSINESS_ETHNICITY, 
                           STD_CERT_BUSINESS_LOCATION, STD_CERT_BUSINESS_ZIP_CODE, STD_CAT_PAYMENT_GROUP, STD_FEE_QUANTITY_ACCURACY, STD_ENABLE_CONTACT_ADDRESS, STD_CAT_CONTACT_ADDRESS_TYPE, STD_ENABLE_CONTACT_TYPE_FILTERING_BY_MODULE, STD_CAT_ONLINE_PAYMENT_MAX_AMOUNT,
                           STD_CONDITIONS_OF_APPROVALS, STD_CONDITION_PRIORITIES, STD_AUTO_SYNC_PEOPLE, STD_CAT_RACE, STD_CONTACT_PREFERRED_CHANNEL, STD_ENABLE_CONTACT_ADDRESS_VALIDATION, STD_COUNTRY_DEFAULT_VALUE, STD_ALL_USER_DOCUMENT_PERMISSION, STD_ITEM_DISPLAY_REQUEST_TRADE_LICENSE_FILTER,
                           STD_PRIMARY_CONTACT_ADDRESS_REQUIRED, STD_AUTHORIZED_SERVICE, STD_CAT_AUTO_INVOICE_MODULE, STD_REPRINT_REASONS, STD_FILE_UPLOAD_BEHAVIOR, STD_CAT_ASSET_GROUP, STD_MASKS, STD_CAT_CONDITIONS_GROUP, DOCUMENT_CONDITION_TYPE_FILTER, STD_ACA_AUTO_ASSIGN_INSPECTOR, 
                           STD_AUTHENTICATION_BY_SECURITY_QUESTION, STD_CAT_SECURITY_SETTING, STD_ENABLE_DISPLAY_ISLAMIC_CALENDAR, STD_EXTERNAL_ADDRESS_SOURCE, STD_EXTERNAL_PARCEL_SOURCE, STD_DISABLE_EXPRESSION_ALERT
                       };
        }

        /// <summary>
        /// 1: Normal, 2: Save record in confirm page.
        /// </summary>
        public struct Create_Application_Model
        {
            /// <summary>
            /// The Normal model.
            /// </summary>
            public const string NormalModel = "1";

            /// <summary>
            /// Save record in confirm page.
            /// </summary>
            public const string SaveDataInConfirmPageModel = "2";
        }

        #endregion
    }
}
