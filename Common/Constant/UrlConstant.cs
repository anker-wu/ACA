#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UrlConstant.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: A static class to define common url constant variables.
*
* </pre>
*/

#endregion

namespace Accela.ACA.Common
{
    /// <summary>
    /// A static class to define common url constant variables.
    /// </summary>
    public static class UrlConstant
    {
        /// <summary>
        /// Define the parameter name for Return Url parameter of the form authentication.
        /// </summary>
        public const string RETURN_URL = "ReturnUrl";

        /// <summary>
        /// Define the parameter name for cap type.
        /// </summary>
        public const string CAPTYPE = "CAPType";

        /// <summary>
        /// Define the parameter name for filter name.
        /// </summary>
        public const string FILTER_NAME = "FilterName";

        /// <summary>
        /// User sequence number.
        /// </summary>
        public const string USER_SEQ_NUM = "UserSeqNum";

        /// <summary>
        /// In order to prevent the ID conflict between multiple instances for these JavaScript functions in ACA components.
        /// We often use the ClientID_FunctionName to define the JavaScript function name.
        /// This Url parameter is defined to indicating the JavaScript function when a user control have multiple instances in a web page.
        /// </summary>
        public const string PARENT_INSTANCE_ID = "ParentInstanceID";

        /// <summary>
        /// Define the form field name to retrieve the service data from 3rd part system for multiple records creation.
        /// </summary>
        public const string SELECTED_SERVICE_LIST = "SelectedServiceList";

        /// <summary>
        /// Define the url parameter name to indicates the multiple records creation from deep link and pass the service data key.
        /// </summary>
        public const string DEEPLINK_MULTIPLE_SERVICES_DATAKEY = "DeepLinkServiceDataKey";

        /// <summary>
        /// URL parameter for pass the page flow group code.
        /// </summary>
        public const string PAGEFLOW_GROUP_CODE = "PageFlowGroupCode";

        /// <summary>
        /// URL parameter for is super agency associated form
        /// </summary>
        public const string IS_SUPERAGENCY_ASSOFORM = "IsSuperAgencyAssoForm";

        /// <summary>
        /// Indicate whether popup page or not.
        /// </summary>
        public const string IS_POPUP_PAGE = "isPopup";

        /// <summary>
        /// Agency code in current request Url.
        /// </summary>
        public const string AgencyCode = "agencyCode";

        /// <summary>
        /// Boolean value to show shopping cart link in current request Url.
        /// </summary>
        public const string IS_SHOW_SHOPPING_CART = "isShowShoppingCart";

        /// <summary>
        /// Url parameter for agent clerk action.
        /// </summary>
        public const string IS_FOR_CLERK = "isForClerk";

        /// <summary>
        /// Url parameter for Validate Flag.
        /// </summary>
        public const string ValidateFlag = "ValidateFlag";

        /// <summary>
        /// Contact sequence number in current request Url.
        /// </summary>
        public const string CONTACT_SEQ_NUMBER = "contactSeqNbr";

        /// <summary>
        /// Contact is from external in current request url.
        /// </summary>
        public const string CONTACT_IS_FROM_EXTERNAL = "contactIsFromExternal";

        /// <summary>
        /// Hidden the breadcrumb's navigation, only show the page title.
        /// </summary>
        public const string BREADCRUMB_HIDDEN_NAVIGATE = "BreadCrumbHiddenNavigate";

        /// <summary>
        /// Hide the action button.
        /// </summary>
        public const string HIDE_ACTION_BUTTON = "HideActionButton";

        /// <summary>
        /// Url parameter for asset detail page
        /// </summary>
        public const string ASSET_DETAIL_FROM_SEARCH = "fromSearch";

        /// <summary>
        /// Url parameter for asset detail page
        /// </summary>
        public const string ASSET_DETAIL_ARGS = "args";

        /// <summary>
        /// Url parameter for asset list in asset detail page indicate whether show action column
        /// </summary>
        public const string ASSET_ISSHOW_ACTION = "isShowAction";

        /// <summary>
        /// Url parameter for asset search page indicate whether show criteria.
        /// </summary>
        public const string ASSET_ISSHOW_CRITERIA = "isShowCriteria";

        /// <summary>
        /// Point out the page if it was from confirm page.
        /// </summary>
        public const string IS_FROM_CONFIRMPAGE = "isFromConfirmPage";

        /// <summary>
        /// Url parameter for popup UploadPage
        /// </summary>
        public const string IFRAME_ID = "iframeid";

        /// <summary>
        /// Url parameter if it is for condition document.
        /// </summary>
        public const string IS_FOR_CONDITION_DOCUMENT = "isForConditionDocument";

        /// <summary>
        /// The parameter of ContactSectionPosition in the url.
        /// </summary>
        public const string CONTACT_SECTION_POSITION = "secPos";

        /// <summary>
        /// The parameter indicating whether country is hidden in the url.
        /// </summary>
        public const string IS_COUNTRY_HIDDEN = "isCountryHidden";

        /// <summary>
        /// The parameter of row index in the url.
        /// </summary>
        public const string ROW_INDEX = "rowIndex";

        /// <summary>
        /// The parameter of isBack in the url.
        /// </summary>
        public const string IS_BACK = "isBack";

        /// <summary>
        /// The url parameter indicating whether a page is opened from Contact Add New page.
        /// </summary>
        public const string IS_OPEN_FROM_CONTACT_ADD_NEW = "openFromContactAddNew";

        /// <summary>
        /// The url parameter indicating whether the Multiple contact be used.
        /// </summary>
        public const string IS_MULTIPLE_CONTACT = "isMultiple";

        /// <summary>
        /// The url parameter indicating the parent ID.
        /// </summary>
        public const string PARENT_ID = "parentid";

        /// <summary>
        /// The url parameter indicating the upload whether from new UI.
        /// </summary>
        public const string IS_UPLOAD_FROM_NEW_UI = "isUpLoadFromNewUI";

        /// <summary>
        /// The url parameter indicating whether the data is new.
        /// </summary>
        public const string NEW = "opt";

        /// <summary>
        /// The value of the url parameter indicating the data is new.
        /// </summary>
        public const string NEW_FLAG = "new";

        /// <summary>
        /// The url parameter indicating whether to show the Contact Detail form after Add New.
        /// </summary>
        public const string IS_SHOW_CONTACT_DETAIL = "detail";

        /// <summary>
        /// The url parameter indicating whether a page is editable.
        /// </summary>
        public const string IS_EDITABLE = "editable";

        /// <summary>
        /// Contact search type
        /// </summary>
        public const string CONTACT_SEARCH_TYPE = "SearchType";

        /// <summary>
        /// The url parameter indicating reference contact sequence number.
        /// </summary>
        public const string REF_CONTACT_SEQ_NBR = "refContactSeqNbr";

        /// <summary>
        /// The url parameter contact component name
        /// </summary>
        public const string CONTACT_COMPONENT_NAME = "ComponentName";

        /// <summary>
        /// The url parameter indicating ValidateFlag
        /// </summary>
        public const string VALIDATE_FLAG = "valFlag";

        /// <summary>
        /// The component type
        /// </summary>
        public const string COMPONENT_TYPE = "ComponentType";

        /// <summary>
        /// The url parameter license number
        /// </summary>
        public const string LICENSE_NBR = "licenseNbr";

        /// <summary>
        /// The url parameter licenseType
        /// </summary>
        public const string LICENSE_TYPE = "licenseType";

        /// <summary>
        /// The url parameter indicating whether to need identify check.
        /// </summary>
        public const string NEED_IDENTIFY_CHECK = "needIdentifyCheck";

        /// <summary>
        /// The url parameter action Type
        /// </summary>
        public const string ACTION_TYPE = "action";

        /// <summary>
        /// The section name
        /// </summary>
        public const string SECTION_NAME = "sectionName";

        /// <summary>
        /// The url parameter for clerk sequence number.
        /// </summary>
        public const string CLERK_SEQ_NBR = "clerkSeqNbr";

        /// <summary>
        /// The url parameter: is from shopping cart.
        /// </summary>
        public const string IS_FROM_SHOPPING_CART = "isFromShoppingCart";

        /// <summary>
        /// The search type in the URL Accessing the Record Home Page
        /// </summary>
        public const string CAPHOME_SEARCH_TYPE = "SearchType";

        /// <summary>
        /// The flag to show my permit list
        /// </summary>
        public const string SHOW_MY_PERMIT_LIST = "ShowMyPermitList";

        /// <summary>
        /// The url parameter: is from aca.
        /// </summary>
        public const string FROM_ACA = "FromACA";

        /// <summary>
        /// The url parameter: is single type.
        /// </summary>
        public const string IS_SINGLE_TYPE = "isSingleType";

        /// <summary>
        /// The url parameter: remembered user name.
        /// </summary>
        public const string Remembered_User_Name = "RememberedUserName";

        /// <summary>
        /// the url parameter: document number.
        /// </summary>
        public const string DOCUMENT_NO = "documentNo";

        /// <summary>
        /// the url parameter: message type.
        /// </summary>
        public const string MESSAGE_TYPE = "MessageType";

        /// <summary>
        /// Indicate whether need to skip disclaimer page for Single/Multiple Record(s) Creation Handler Deep link
        /// </summary>
        public const string IS_SKIP_DISCLAIMER = "IsSkipDisclaimer";

        /// <summary>
        /// The url parameter: HideHeader
        /// </summary>
        public const string HIDERHEADER = "HideHeader";

        /// <summary>
        /// The url parameter: the flag for show timeout's message
        /// </summary>
        public const string SHOW_TIMEOUT_MESSAGE_FLAG = "showTimeoutMsgFlag";

        /// <summary>
        /// The url parameter is from account management flag.
        /// </summary>
        public const string IS_FROM_ACCOUNT_MANANGEMENT = "IsFromAccount";

        /// <summary>
        /// An url parameter that indicates if it is from another agency
        /// </summary>
        public const string IS_LOGIN_USE_EXISTING_ACCOUNT = "IsLoginUseExistingAccount";

        /// <summary>
        /// An url parameter that indicates a user's id or email
        /// </summary>
        public const string USER_ID_OR_EMAIL = "UserIdOrEmail";

        /// <summary>
        /// An url parameter that indicates if it is register LP account
        /// </summary>
        public const string IS_REGISTER_LP_ACCOUNT = "isRegisterLPAccount";

        /// <summary>
        /// The url parameter: recoverSource.
        /// </summary>
        public const string RECOVER_SOURCE = "recoverSource";

        /// <summary>
        /// The url parameter that indicates if it is come from examine schedule.
        /// </summary>
        public const string IS_FROM_EXAM_SCHEDULE = "isFromExamSchedule";

        /// <summary>
        /// Is to renewal detail
        /// </summary>
        public const string IS_FOR_RENEW = "IsForRenew";

        /// <summary>
        /// Whether it is from account manager page.
        /// </summary>
        public const string IS_FROM_ACCOUNT_MANAGER = "IsFromAccountManager";

        /// <summary>
        /// The url parameter indicating a message to be display in welcome page.
        /// </summary>
        public const string RETURN_MESSAGE = "ReturnMessage";

        /// <summary>
        /// The url parameter indicating a message key to be display in welcome page.
        /// </summary>
        public const string RETURN_MESSAGE_KEY = "ReturnMessageKey";

        /// <summary>
        /// The url parameter indicating whether external account associated public user.
        /// </summary>
        public const string IS_EXTERNAL_ACCOUNT_ASSOCIATED_PUBLICUSER = "IsExternalAccountAssociatedPublicUser";

        /// <summary>
        /// confirmPassword for forgot password page type
        /// </summary>
        public const string FORGOTPASSWORD_PAGE_CONFIRMPASSWORD = "confirmPassword";

        /// <summary>
        /// securityQuestion for forgot password page type
        /// </summary>
        public const string FORGOTPASSWORD_PAGE_SECURITYQUESTION = "securityQuestion";

        /// <summary>
        /// forgot password page type
        /// </summary>
        public const string FORGOTPASSWORD_PAGE_TYPE = "pageType";

        /// <summary>
        /// Whether it is from New UI page.
        /// </summary>
        public const string IS_FROM_NEWUI = "isFromNewUi";
    }
}
