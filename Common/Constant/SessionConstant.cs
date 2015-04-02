#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SessionConstant.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 *  Session Contant Define
 *  Notes:
 *      $Id: SessionConstant.cs 133464 2009-06-05 05:06:34Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.Common
{
    /// <summary>
    /// The constant define for Session
    /// </summary>
    public static class SessionConstant
    {
        /// <summary>
        /// session admin mode
        /// </summary>
        public const string SESSION_ADMIN_MODE = "SESSION_IS_ADMIN";

        /// <summary>
        /// session USER_PREFERRED_AGENCYCODE
        /// </summary>
        public const string SESSION_USER_PREFERRED_AGENCYCODE = "SESSION_USER_PREFERRED_AGENCYCODE";

        /// <summary>
        /// session Admin User Name
        /// </summary>
        public const string SESSION_ADMIN_USERNAME = "AdminUserName";

        /// <summary>
        /// session apo query 
        /// </summary>
        public const string SESSION_APO_QUERY = "APOQueryInfo";

        /// <summary>
        /// session asit temp rows
        /// </summary>
        public const string SESSION_ASIT_TEMP_ROWS = "SESSION_ASIT_TEMP_ROWS";

        /// <summary>
        /// session breadcrumb
        /// </summary>
        public const string SESSION_BREADCRUMB = "SESSION_BREADCRUMB";

        /// <summary>
        /// The session asset search model
        /// </summary>
        public const string SESSION_ASSET_SEARCH_MODEL = "SESSION_ASSET_SEARCH_MODEL";

        /// <summary>
        /// session HistoryAction
        /// </summary>
        public const string SESSION_HISTORYACTION = "SESSION_HISTORYACTION";

        /// <summary>
        /// session for inspection data
        /// </summary>
        public const string SESSION_INSPECTION_DATA = "SESSION_INSPECTION_DATA";

        /// <summary> 
        /// session for examination data
        /// </summary>
        public const string SESSION_EXAMINATION_DATA = "SESSION_EXAMINATION_DATA";

        /// <summary> 
        /// session for announcement list
        /// </summary>
        public const string SESSION_ANNOUNCEMENT_LIST = "SESSION_ANNOUNCEMENT_LIST";

        /// <summary>
        /// session for announcement list flag which identify whether aca got list from server
        /// </summary>
        public const string SESSION_ANNOUNCEMENT_LIST_FLAG = "SESSION_ANNOUNCEMENT_LIST_FLAG";

        /// <summary>
        /// session for inspection parameters
        /// </summary>
        public const string SESSION_INSPECTION_PARAMETERS = "SESSION_INSPECTION_PARAMETERS";

        /// <summary>
        /// session for examination parameters
        /// </summary>
        public const string SESSION_EXAMINATION_PARAMETERS = "SESSION_EXAMINATION_PARAMETERS";

        /// <summary>
        /// session cap id models
        /// </summary>
        public const string SESSION_CAPID_MODELS = "SESSION_CAPID_MODELS";

        /// <summary>
        /// session parent CapIDModel
        /// </summary>
        public const string SESSION_PARENT_CAPID_MODEL = "SESSION_PARENT_CAPID_MODEL";

        /// <summary>
        /// session cap model 
        /// </summary>
        public const string SESSION_CAP_MODEL = "SESSION_CAP_MODEL";

        /// <summary>
        /// cap model session for admin dummy data
        /// </summary>
        public const string SESSION_CAP_MODEL_ADMIN = "SESSION_CAP_MODEL_ADMIN";

        /// <summary>
        /// session drill down list 
        /// </summary>
        public const string SESSION_DRILL_DOWN_LIST = "SESSION_DRILL_DOWN_LIST";

        /// <summary>
        ///  session educationQueryInfo 
        /// </summary>
        public const string SESSION_EDUCATION_QUERYINFO = "SESSION_EDUCATION_QUERYINFO";

        /// <summary>
        /// session load payment completion 
        /// </summary>
        public const string SESSION_LOAD_PAYMENT_COMPLETION = "LOAD_PAYMENT_COMPLETION";

        /// <summary>
        /// session my collections 
        /// </summary>
        public const string SESSION_MYCOLLECTIONS = "SESSION_MYCOLLECTIONS";

        /// <summary>
        /// session for store flag which indicate current user whether has Provider License.
        /// </summary>
        public const string SESSION_HAS_PROVIDER_LICENSE = "HAS_PROVIDER_LICENSE";

        /// <summary>
        /// session online payment result
        /// </summary>
        public const string SESSION_ONLINE_PAYMENT_RESULT = "ONLINE_PAYMENT_RESULT";

        /// <summary>
        /// session page flow group 
        /// </summary>
        public const string SESSION_PAGEFLOW_GROUP = "SESSION_PAGEFLOW_GROUP";

        /// <summary>
        /// session parent cap type
        /// </summary>
        public const string SESSION_PARENT_CAPTYPE = "SESSION_PARENT_CAPTYPE";

        /// <summary>
        /// session parent page flow group 
        /// </summary>
        public const string SESSION_PARENT_PAGEFLOW_GROUP = "SESSION_PARENT_PAGEFLOW_GROUP";

        /// <summary>
        /// session payment result models
        /// </summary>
        public const string SESSION_PAYMENTRESULT_MODELS = "SESSION_PAYMENTRESULT_MODELS";

        /// <summary>
        ///  session permit query info 
        /// </summary>
        public const string SESSION_PERMITQUERYINFO = "PermitQueryInfo";

        /// <summary>
        /// session preview cap model 
        /// </summary>
        public const string SESSION_PREVIEW_CAP_MODEL = "SESSION_PREVIEW_CAP_MODEL";

        /// <summary>
        /// session preview smart groups
        /// </summary>
        public const string SESSION_PREVIEW_SMART_GROUPS = "SESSION_PREVIEW_SMART_GROUPS";

        /// <summary>
        /// session register licenses
        /// </summary>
        public const string SESSION_REGISTER_LICENSES = "REGISTER_LICENSES";

        /// <summary>
        /// session register user model 
        /// </summary>
        public const string SESSION_REGISTER_USER_MODEL = "REGISTER_USER_MODEL";

        /// <summary>
        /// session register People model 
        /// </summary>
        public const string SESSION_REGISTER_PEPOLE_MODEL = "REGISTER_PEOPLE_MODEL";

        /// <summary>
        /// report session parameter.
        /// </summary>
        public const string SESSION_REPORT_PARAMETER = "SESSION_REPORT_PARAMETER";

        /// <summary>
        /// session selected parcel info
        /// </summary>
        public const string SESSION_SELECTED_PARCEL_INFO = "SESSION_SELECTED_PARCEL_INFO";

        /// <summary>
        /// session Default Module for payment log
        /// </summary>
        public const string SESSION_DEFAULT_MODULE = "SESSION_DEFAULT_MODULE";

        /// <summary>
        /// session attachment edit for upload file info
        /// </summary>
        public const string SESSION_ATTACHMENT_UPLOADFILEINFO = "SESSION_ATTACHMENT_UPLOADFILEINFO";

        /// <summary>
        /// session selected services
        /// </summary>
        public const string SESSION_SELECTED_SERVICES = "SESSION_SELECTED_SERVICES";

        /// <summary>
        /// session shopping cart item number
        /// </summary>
        public const string SESSION_SHOPPINGCART_ITEMNUMBER = "ShoppingCartItemNumber";

        /// <summary>
        /// session shopping cart breadcrumb 
        /// </summary>
        public const string SESSION_SHOPPING_CART_BREADCRUMB = "SESSION_SHOPPING_CART_BREADCRUMB";

        /// <summary>
        /// session supper agency 
        /// </summary>
        public const string SESSION_SUPPER_AGENCY = "SESSION_SUPPER_AGENCY";

        /// <summary>
        /// session user
        /// </summary>
        public const string SESSION_USER = "SESSION_USER"; // user session key

        /// <summary>
        /// session variable prefix
        /// </summary>
        public const string SESSION_VARIABLE_PREFIX = "SessionVariable.";

        /// <summary>
        /// transaction id 
        /// </summary>
        public const string TRANSACTION_ID = "transaction_id";

        /// <summary>
        /// session active modules
        /// </summary>
        public const string SESSION_ACTIVE_MODULES_FOR_ADMIN = "SESSION_ACTIVE_MODULES_FOR_ADMIN";

        /// <summary>
        /// create cap session.
        /// </summary>
        public const string SESSION_CREATE_RECORD_BY_MAP = "SESSION_CREATE_RECORD_BY_MAP";

        /// <summary>
        /// session for parcel information condition by gis.
        /// </summary>
        public const string SESSION_APO_CONDITION = "SESSION_APO_CONDITION";

        /// <summary>
        /// Declare a session variable for the UI data storing.
        /// </summary>
        public const string SESSION_UI_DATA = "SESSION_UI_DATA";

        /// <summary>
        /// The social media access token
        /// </summary>
        public const string SESSION_SOCIAL_MEDIA_ACCESS_TOKEN = "SESSION_SOCIAL_MEDIA_ACCESS_TOKEN";

        /// <summary>
        /// session for validate contact address
        /// </summary>
        public const string SESSION_VALIDATE_CONTACT_ADDRESS = "SESSION_VALIDATE_CONTACT_ADDRESS";

        /// <summary>
        /// Facebook user entity session object.
        /// </summary>
        public const string FACEBOOK_USER = "FACEBOOK_USER";

        /// <summary>
        /// A session key to store the reference entity data to prevent lose the data which fields hidden by form designer.
        /// </summary>
        public const string SESSION_REFERENCE_ENTITY_PREFIX = "SESSION_REFERENCE_ENTITY_PREFIX_";

        /// <summary>
        /// A session key to store the people model that get from reference contact sequence number.
        /// </summary>
        public const string SESSION_PEOPLE = "SESSION_PEOPLE";

        /// <summary>
        /// A session key to store the reference contact educations that get from reference contact sequence number.
        /// </summary>
        public const string SESSION_REF_CONTACT_EDUCATIONS = "SESSION_REF_CONTACT_EDUCATIONS";

        /// <summary>
        /// A session key to store the reference contact examinations that get from reference contact sequence number.
        /// </summary>
        public const string SESSION_REF_CONTACT_EXAMINATIONS = "SESSION_REF_CONTACT_EXAMINATIONS";

        /// <summary>
        /// A session key to store the reference contact continuing educations that get from reference contact sequence number.
        /// </summary>
        public const string SESSION_REF_CONTACT_CONT_EDUCATIONS = "SESSION_REF_CONTACT_CONTINUING_EDUCATIONS";

        /// <summary>
        /// A session key to store the expression 
        /// </summary>
        public const string SESSION_EXPRESSION_DATA = "SESSION_EXPRESSION_DATA";

        /// <summary>
        /// A session key to store the expression result
        /// </summary>
        public const string SESSION_EXPRESSION_RESULT_DATA = "SESSION_EXPRESSION_RESULT_DATA";

        /// <summary>
        /// The contact session parameter
        /// </summary>
        public const string CONTACT_SESSION_PARAMETER = "ContactSessionParameter";

        /// <summary>
        /// The cap associate license certification
        /// </summary>
        public const string CAP_ASSOCIATE_LICENSE_CERTIFICATION = "CapAssociateLicenseCertification";

        /// <summary>
        /// The selected contact license certification
        /// </summary>
        public const string SELECTED_CONTACT_LICENSE_CERTIFICATION = "SELECTED_CONTACT_LICENSE_CERTIFICATION";

        /// <summary>
        /// The Record's Flag if Edit from Confirm Page.
        /// </summary>
        public const string SESSION_IS_EDIT_FROM_CONFIRM = "IsEditFromConfirm";

        /// <summary>
        /// The session key to store the global report property model
        /// </summary>
        public const string SESSION_GLOBAL_REPORT = "SESSION_GLOBAL_REPORT";

        /// <summary>
        /// The session key to store the associated parent CAP model
        /// </summary>
        public const string SESSION_ASSOCIATED_PARENT_CAP_MODEL = "SESSION_ASSOCIATED_PARENT_CAP_MODEL";

        /// <summary>
        /// This access is from deep link.
        /// </summary>
        public const string FROM_DEEP_LINK = "FROM_DEEP_LINK";

        /// <summary>
        /// session register user model expire time
        /// </summary>
        public const string SESSION_REGISTRE_USER_MODEL_EXPIRETIME = "SESSION_REGISTRE_USER_MODEL_EXPIRETIME";

        /// <summary>
        /// The session key to store the condition Additional Information value.
        /// </summary>
        public const string SESSION_CONDITION_ADDITIONAL_INFORMATION = "SESSION_CONDITION_ADDITIONAL_INFORMATION";

        /// <summary>
        /// The session key to store login user for the login security question.
        /// </summary>
        public const string SESSION_USER_FOR_LOGIN_SECURITY = "SESSION_USER_FOR_LOGIN_SECURITY";

        /// <summary>
        /// The session key to store input people for spear form close match.
        /// </summary>
        public const string SESSION_INPUTPEOPLE_FOR_SPEARFORM_CLOSEMATCH = "SESSION_INPUTPEOPLE_FOR_SPEARFORM_CLOSEMATCH";

        /// <summary>
        /// The APO session parameter
        /// </summary>
        public const string APO_SESSION_PARAMETER = "APOSessionParameter";

        /// <summary>
        /// The parcel PK model session parameter. It is used to search parcel associated owner record.
        /// </summary>
        public const string APO_SESSION_PARCELMODEL = "APO_SESSION_PARCELMODEL";

        /// <summary>
        /// The document status list tree.Its is used to show the document status list on cap detail
        /// </summary>
        public const string DOCUMENT_STATUS_LIST = "DocumentStatusList";
    }
}