#region Header

/**
 *  Accela Citizen Access
 *  File: ACAConstant.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *   It provide the logging related utility to serve the framework.
 *
 *  Notes:
 * $Id: ACAConstant.cs 134054 2009-06-10 07:03:54Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Configuration;

using Accela.ACA.Common.Util;

namespace Accela.ACA.Common
{
    #region Structs

    /// <summary>
    /// Delete attachment result
    /// </summary>
    public enum DeleteAttachmentResult
    {
        /// <summary>
        /// The successful
        /// </summary>
        Successfull = 1,

        /// <summary>
        /// The Failed
        /// </summary>
        Failed = 2,

        /// <summary>
        /// The Related
        /// </summary>
        Related = 3
    }

    /// <summary>
    /// construct component data source where it comes from.
    /// </summary>
    public struct ComponentDataSource
    {
        /// <summary>
        /// user must get information by search.
        /// </summary>
        public const string Reference = "R";

        /// <summary>
        /// user only get information by input data.
        /// </summary>
        public const string Transactional = "T";

        /// <summary>
        /// user can get information by search and input.
        /// </summary>
        public const string NoLimitation = "N";
    }

    /// <summary>
    /// struct for contact permission.
    /// </summary>
    public struct ContactPermission
    {
        /// <summary>
        /// Full Access
        /// </summary>
        public const string FullAccess = "F";

        /// <summary>
        /// Schedule Inspection Only
        /// </summary>
        public const string ScheduleInspectionOnly = "S";

        /// <summary>
        /// Read Only
        /// </summary>
        public const string ReadOnly = "R";

        /// <summary>
        /// No Access
        /// </summary>
        public const string NoAccess = "N";

        /// <summary>
        /// The Custom
        /// </summary>
        public const string Custom = "C";

        /// <summary>
        /// Renew And Amend
        /// </summary>
        public const string RenewAndAmend = "A";

        /// <summary>
        /// Make Payments
        /// </summary>
        public const string MakePayments = "P";

        /// <summary>
        /// Manage Documents
        /// </summary>
        public const string ManageDocuments = "D";
    }

    /// <summary>
    /// Public user contact methods.
    /// </summary>
    public struct ContactMethod
    {
        /// <summary>
        /// Contact by e-mail
        /// </summary>
        public const string Email = "E-mail";

        /// <summary>
        /// Contact by fax
        /// </summary>
        public const string Fax = "Fax";

        /// <summary>
        /// Contact by home phone
        /// </summary>
        public const string HomePhone = "Home Phone";

        /// <summary>
        /// Contact by mobile phone 
        /// </summary>
        public const string MobilePhone = "Mobile Phone";

        /// <summary>
        /// Contact by work phone
        /// </summary>
        public const string WorkPhone = "Work Phone";
    }

    /// <summary>
    /// The contact type source
    /// </summary>
    public struct ContactTypeSource
    {
        /// <summary>
        /// The Both
        /// </summary>
        public const string Both = "Both";

        /// <summary>
        /// The Reference
        /// </summary>
        public const string Reference = "Reference";

        /// <summary>
        /// The Transaction
        /// </summary>
        public const string Transaction = "Transaction";

        /// <summary>
        /// The All
        /// </summary>
        public const string All = "All";
    }

    /// <summary>
    /// The status of public user associated contact.
    /// </summary>
    public struct ContractorPeopleStatus
    {
        /// <summary>
        /// The association is approved.
        /// </summary>
        public const string Approved = "A";

        /// <summary>
        /// The association is pending.
        /// </summary>
        public const string Pending = "P";

        /// <summary>
        /// The association is rejected.
        /// </summary>
        public const string Rejected = "R";
    }

    /// <summary>
    /// The status of public user associated license.
    /// </summary>
    public struct ContractorLicenseStatus
    {
        /// <summary>
        /// approved status
        /// </summary>
        public const string Approved = "approved";

        /// <summary>
        /// pending status
        /// </summary>
        public const string Pending = "pending";

        /// <summary>
        /// rejected status
        /// </summary>
        public const string Rejected = "rejected";
    }

    /// <summary>
    /// ACA Document Entity type.
    /// </summary>
    public struct DocumentEntityType
    {
        /// <summary>
        /// entity type for cap.
        /// </summary>
        public const string Cap = "CAP";

        /// <summary>
        /// entity type for temple cap.
        /// </summary>
        public const string TMP_CAP = "TMP_CAP";

        /// <summary>
        /// entity type for cap renewal.
        /// </summary>
        public const string Renewal = "Renewal";

        /// <summary>
        /// entity type for cap related.
        /// </summary>
        public const string Related = "Related";

        /// <summary>
        /// entity type for parcel.
        /// </summary>
        public const string Parcel = "PARCEL";

        /// <summary>
        /// entity type for examination.
        /// </summary>
        public const string Examination = "EXAM";

        /// <summary>
        /// entity type for LP.
        /// </summary>
        public const string LP = "LICENSEPROFESSIONAL";

        /// <summary>
        /// entity type for Inspection
        /// </summary>
        public const string Inspection = "INSPECTION";

        /// <summary>
        /// entity type for reference contact
        /// </summary>
        public const string RefContact = "REFCONTACT";

        /// <summary>
        /// entity type for reference license professional
        /// </summary>
        public const string RefLicenseProfessional = "PROFESSIONAL";
    }

    /// <summary>
    /// List section component
    /// </summary>
    public struct ListSection
    {
        /// <summary>
        /// MultiContacts component
        /// </summary>
        public const string MULTICONTACTSEDIT = "MultiContactsEdit";

        /// <summary>
        /// MultiLicense component
        /// </summary>
        public const string MULTILICENSESEDIT = "MultiLicensesEdit";
    }

    /// <summary>
    /// Mask chars Status
    /// * --- Letter and Numeric
    /// # --- Numeric
    /// ? --- Letter
    /// </summary>
    public struct MaskChars
    {
        /// <summary>
        /// The optional match expression
        /// </summary>
        public const string OptionalMatchExpression = "\\(.+\\)";

        /// <summary>
        /// All people can see the button
        /// </summary>
        public static char All = '*';

        /// <summary>
        /// record creator can see the button
        /// </summary>
        public static char NumericChar = '#';

        /// <summary>
        /// disable button
        /// </summary>
        public static char LetterChar = 'A';

        /// <summary>
        /// left Bracket
        /// </summary>
        public static char LeftBracket = '(';

        /// <summary>
        /// The right bracket
        /// </summary>
        public static char RightBracket = ')';
    }

    /// <summary>
    /// struct for work flow task status.
    /// </summary>
    public struct WFActStatus
    {
        /// <summary>
        /// No Change 
        /// </summary>
        public static string NoChange = "U";

        /// <summary>
        /// Go to Next Task 
        /// </summary>
        public static string GoToNextTask = "Y";

        /// <summary>
        /// Go to Loop Task 
        /// </summary>
        public static string GoToLoopTask = "L";

        /// <summary>
        /// Go to Branch Task
        /// </summary>
        public static string GoToBranchTask = "B";
    }

    /// <summary>
    /// struct for Add data way.
    /// </summary>
    public struct AddDataWay
    {
        /// <summary>
        /// select LP from public user associated. 
        /// </summary>
        public const string SelectFromAccount = "S";

        /// <summary>
        /// add LP by search reference license
        /// </summary>
        public const string LookUp = "L";

        /// <summary>
        /// add LP by manual.
        /// </summary>
        public const string AddNew = "A";

        /// <summary>
        /// edit current LP.
        /// </summary>
        public const string Edit = "E";
    }

    /// <summary>
    /// SSO action type.
    /// </summary>
    public struct SSOActionType
    {
        /// <summary>
        /// To expire SSO link.
        /// </summary>
        public const string Expire = "EXPIRE";

        /// <summary>
        /// To update external account info.
        /// </summary>
        public const string Registration = "REGISTRATION";
    }

    #endregion

    /// <summary>
    /// All of constants should be put into this class so that the constant can be shared by other class.
    /// Which includes UI constant, BLL constant and so on.
    /// </summary>
    public static class ACAConstant
    {
        #region Fields

        /// <summary>
        /// file failed date.
        /// </summary>
        public const string FILE_FAILED_DATE = "01/01/1901";

        /// <summary>
        /// File pending date
        /// </summary>
        public const string FILE_PENDING_DATE = "01/01/1900";

        /// <summary>
        /// The days that the file can be deleted.
        /// </summary>
        public const int FILE_DELETE_DAYS = 3;

        /// <summary>
        /// ACA_RESUBMIT flag for attachment.
        /// </summary>
        public const string ACA_RESUBMIT = "ACA_RESUBMIT";

        /// <summary>
        /// RESUBMIT flag for v360 attachment,
        /// we use it to indicate this document's allowAction need delete "ACA_RESUBMIT" from it.
        /// </summary>
        public const string RESUBMIT = "RESUBMIT";

        /// <summary>
        /// abbreviated month year format
        /// </summary>
        public const string ABBREVIATED_MONTH_YEAR_FORMAT = "MMM yyyy";

        /// <summary>
        /// aca applicant display rule
        /// </summary>
        public const string ACA_APPLICANT_DISPLAY_RULE = "ACA_APPLICANT_DISPLAY_RULE";

        /// <summary>
        /// ACA checkbox enabled cross module search.
        /// </summary>
        public const string ACA_ENABLE_CROSS_MODULE_SEARCH = "ENABLE_CROSS_MODULE_SEARCH";

        /// <summary>
        /// ACA checkbox enabled only search my license.
        /// </summary>
        public const string ACA_ENABLE_ONLY_SEARCH_MY_LICENSE = "ENABLE_ONLY_SEARCH_MY_LICENSE";

        /// <summary>
        /// ACA search cross module label name.
        /// </summary>
        public const string ACA_ENABLE_SEARCH_CROSS_MODULE = "SEARCH_CROSS_MODULE";

        /// <summary>
        /// aca checkbox anonymous user visible 
        /// </summary>
        public const string ACA_CHECKBOX_ANONYMOUSUSER_VISIBLE = "CHECKBOX_ANONYMOUSUSER_VISIBLE";

        /// <summary>
        /// aca config prefix
        /// </summary>
        public const string ACA_CONFIG_PREFIX = "ACA_CONFIG_";

        /// <summary>
        /// aca contact type user roles
        /// </summary>
        public const string ACA_CONTACT_TYPE_USER_ROLES = "ACA_CONTACT_TYPE_USER_ROLES";

        /// <summary>
        /// aca enable Enable Parcel Genealogy 
        /// </summary>
        public const string ACA_ENABLE_PARCEL_GENEALOGY = "ENABLE_PARCEL_GENEALOGY";

        /// <summary>
        /// ACA enable registration add license.
        /// </summary>
        public const string ACA_DISABLE_REGISTRATION_ADD_LICENSE = "DISABLE_REGISTRATION_ADD_LICENSE";

        /// <summary>
        /// ACA enable range search
        /// </summary>
        public const string ACA_ENABLE_RANGE_SEARCH = "ENABLE_RANGE_SEARCH";

        /// <summary>
        /// Auto fill city.
        /// </summary>
        public const string STD_AUTOFILL_CITY_ENABLED = "AUTOFILL_CITY_ENABLED";

        /// <summary>
        /// Auto fill state.
        /// </summary>
        public const string STD_AUTOFILL_STATE_ENABLED = "AUTOFILL_STATE_ENABLED";

        /// <summary>
        /// aca enable display email
        /// </summary>
        public const string ACA_ENABLE_WF_DISP_EMAIL = "ACA_ENABLE_WF_DISP_EMAIL";

        /// <summary>
        /// aca report page setting
        /// </summary>
        public const string ACA_REPORT_PAGE_SETTING = "ACA_REPORT_PAGE_SETTING";

        /// <summary>
        /// aca report role setting 
        /// </summary>
        public const string ACA_REPORT_ROLE_SETTING = "ACA_REPORT_ROLE_SETTING";

        /// <summary>
        /// aca shopping cart expiration day of saved items
        /// </summary>
        public const string ACA_SHOPPING_CART_EXPIRATION_DAY_OF_SAVED_ITEMS = "EXPIRATION_DAY_OF_SAVED_ITEMS";

        /// <summary>
        /// aca proxy user expiration day.
        /// </summary>
        public const string PROXY_INVITATION_EXPIRATION_DAYS = "PROXY_INVITATION_EXPIRATION_DAYS";

        /// <summary>
        /// aca proxy user expiration remove day.
        /// </summary>
        public const string PROXY_INVITATION_PURGE_DAYS = "PROXY_INVITATION_PURGE_DAYS";

        /// <summary>
        /// aca shopping cart expiration day of selected items
        /// </summary>
        public const string ACA_SHOPPING_CART_EXPIRATION_DAY_OF_SELECTED_ITEMS = "EXPIRATION_DAY_OF_SELECTED_ITEMS";

        /// <summary>
        /// aca shopping cart payment transaction setting
        /// </summary>
        public const string ACA_SHOPPING_CART_PAYMENT_TRANSACTION_SETTING = "PAYMENT_TRANSACTION_SETTING";

        /// <summary>
        /// aca shopping cart redirect page
        /// </summary>
        public const string ACA_SHOPPING_CART_REDIRECT_PAGE = "ACA_SHOPPING_CART_REDIRECT_PAGE";

        /// <summary>
        /// aca enable global search
        /// </summary>
        public const string ENABLE_GLOBAL_SEARCH = "ENABLE_GLOBAL_SEARCH";

        /// <summary>
        /// aca enable global search result
        /// </summary>
        public const string ENABLE_GLOBAL_SEARCH_FOR_RECORDS = "ENABLE_GLOBAL_SEARCH_FOR_RECORDS";

        /// <summary>
        /// aca enable global search LP
        /// </summary>
        public const string ENABLE_GLOBAL_SEARCH_FOR_LP = "ENABLE_GLOBAL_SEARCH_FOR_LP";

        /// <summary>
        /// aca enable global search APO
        /// </summary>
        public const string ENABLE_GLOBAL_SEARCH_FOR_APO = "ENABLE_GLOBAL_SEARCH_FOR_APO";

        /// <summary>
        /// action report print
        /// </summary>
        public const int ACTION_REPORT_PRINT = 3;

        /// <summary>
        /// action report run
        /// </summary>
        public const int ACTION_REPORT_RUN = 1;

        /// <summary>
        /// action report save to EDMS
        /// </summary>
        public const int ACTION_REPORT_SAVETOEDMS = 2;

        /// <summary>
        /// active tab index
        /// </summary>
        public const string ACTIVE_TAB_INDEX = "ActiveTabIndex";

        /// <summary>
        /// active tab name
        /// </summary>
        public const string ACTIVE_TAB_NAME = "ACTIVE_TAB_NAME";

        /// <summary>
        /// Quick Query
        /// </summary>
        public const string QUICK_QUERY = "QuickQuery";

        /// <summary>
        /// Admin caller id
        /// </summary>
        public const string ADMIN_CALLER_ID = "ACA Admin";

        /// <summary>
        /// Admin caller id
        /// </summary>
        public const string ADMIN = "ADMIN";

        /// <summary>
        /// admin page flow dummy data 
        /// </summary>
        public const string ADMIN_PAGE_FLOW_DUMMY_DATA = @"\Admin\PreviewDummyData\PageFlow.xml";

        /// <summary>
        /// admin preview cap model dummy data
        /// </summary>
        public const string ADMIN_PREVIEW_CAPMODEL_DUMMY_DATA = @"\Admin\PreviewDummyData\capModel.xml";

        /// <summary>
        /// Admin root rode flag
        /// </summary>
        public const string ADMIN_ROOT_NODE = "#";

        /// <summary>
        /// alt id content..
        /// </summary>
        public const string ALT_ID = "altId";

        /// <summary>
        /// Amendment CAP is completed, it is can be show in related cap list for ACA user.
        /// </summary>
        public const string AMENDMENT_COMPLETE = "Complete";

        /// <summary>
        /// Partial amendment CAP
        /// </summary>
        public const string AMENDMENT_IMCOMPLETE = "Incomplete";

        /// <summary>
        /// time separator
        /// </summary>
        public const string TIME_SEPARATOR = ":";

        /// <summary>
        /// anonymous flag
        /// </summary>
        public const string ANONYMOUS_FLAG = "0";

        /// <summary>
        /// anonymous user 
        /// </summary>
        public const string ANONYMOUS_USER = PUBLIC_USER_NAME + ANONYMOUS_FLAG;

        /// <summary>
        /// auto populate state
        /// </summary>
        public const string AUTO_POPULATE_STATE = "AUTO_POPULATE_STATE";

        /// <summary>
        /// blank schedule date
        /// </summary>
        public const string BLANK_SCHEDULE_DATE = "TBD";

        /// <summary>
        /// bracket space.
        /// </summary>
        public const string BRACKET = ")";

        /// <summary>
        /// bracket space for left.
        /// </summary>
        public const string BRACKETLEFT = "(";

        /// <summary>
        /// blank space.
        /// </summary>
        public const string BLANK = " ";

        /// <summary>
        /// It used for order ASC.
        /// </summary>
        public const string ORDER_BY_ASC = "ASC";

        /// <summary>
        /// It used for order DESC
        /// </summary>
        public const string ORDER_BY_DESC = "DESC";

        /// <summary>
        /// button label for create amendment.
        /// </summary>
        public const string BUTTON_CREATE_AMENDMENT = "CreateAmendment";

        /// <summary>
        /// button label for create document deletion.
        /// </summary>
        public const string BUTTON_CREATE_DOCUMENT_DELETE = "DeleteDocument";

        /// <summary>
        /// calendar type
        /// </summary>
        public const string CALENDAR_TYPE = "AGENCY_MASTER";

        /// <summary>
        /// cap active..
        /// </summary>
        public const string CAP_ACTIVE = "Active";

        /// <summary>
        /// Relationship in XAPP2REF table for Renewal CAP.
        /// </summary>
        public const string CAP_RENEWAL = "Renewal";

        /// <summary>
        /// Relationship in XAPP2REF table for cap amendment
        /// </summary>
        public const string CAP_RELATIONSHIP_AMENDMENT = "Amendment";

        /// <summary>
        /// Relationship in XAPP2REF table for Related Cap.
        /// </summary>
        public const string CAP_RELATIONSHIP_RELATED = "R";

        /// <summary>
        /// Relationship in XAPP2REF table for Associated Forms
        /// </summary>
        public const string CAP_RELATIONSHIP_ASSOFORM = "AssoForm";

        /// <summary>
        /// CAP ID1 constant.
        /// </summary>
        public const string CAP_ID_1 = "capId1";

        /// <summary>
        /// the cap id 1 constant.
        /// </summary>
        public const string CAP_ID_2 = "capId2";

        /// <summary>
        /// the cap id2 constant.
        /// </summary>
        public const string CAP_ID_3 = "capId3";

        /// <summary>
        /// For pay fee due.
        /// </summary>
        public const string CAP_PAYFEEDUE = "PayFees";

        /// <summary>
        /// the action name for complete paid record.
        /// </summary>
        public const string ACTION_COMPLETE_PAID = "CompletePaid";

        /// <summary>
        /// The Amendment
        /// </summary>
        public const string AMENDMENT = "Amend";

        /// <summary>
        /// This is a pending cap for online payment
        /// </summary>
        public const string CAP_STATUS_PENDING = "PAYING";

        /// <summary>
        /// code error.
        /// </summary>
        public const int CODE_ERROR = -1;

        /// <summary>
        /// code ok information.
        /// </summary>
        public const int CODE_OK = 0;

        /// <summary>
        /// common checked 
        /// </summary>
        public const string COMMON_CHECKED = "CHECKED";

        /// <summary>
        /// common n code.
        /// </summary>
        public const string COMMON_N = "N";

        /// <summary>
        /// The common string "F"
        /// </summary>
        public const string COMMON_F = "F";

        /// <summary>
        /// common NO code. 
        /// </summary>
        public const string COMMON_NO = "NO";

        /// <summary>
        /// common No code.
        /// </summary>
        public const string COMMON_No = "No";

        /// <summary>
        /// common unchecked 
        /// </summary>
        public const string COMMON_UNCHECKED = "UNCHECKED";

        /// <summary>
        /// common Y constant.
        /// </summary>
        public const string COMMON_Y = "Y";

        /// <summary>
        /// common YES constant.
        /// </summary>
        public const string COMMON_YES = "YES";

        /// <summary>
        /// common Yes constant.
        /// </summary>
        public const string COMMON_Yes = "Yes";

        /// <summary>
        /// common Hidden constant.
        /// </summary>
        public const string COMMON_H = "H";

        /// <summary>
        /// common read only constant.
        /// </summary>
        public const string COMMON_READONLY = "R";

        /// <summary>
        /// This is a real/full cap
        /// </summary>
        public const string COMPLETED = "COMPLETE";

        /// <summary>
        /// if set hidden=true, the fields on ASI and ASIT shouldn't add the required flag  
        /// </summary>
        public const string IS_HIDDEN = "ishidden";

        /// <summary>
        /// Attribute Name for ASI control - groupName
        /// </summary>
        public const string ASI_GROUP_NAME = "groupName";

        /// <summary>
        /// Attribute Name for ASI control - subGroupName
        /// </summary>
        public const string ASI_SUB_GROUP_NAME = "subGroupName";

        /// <summary>
        /// Attribute Name for ASI control - fieldName
        /// </summary>
        public const string ASI_FIELD_NAME = "fieldName";

        /// <summary>
        /// condition status
        /// </summary>
        public const string CONDITION_STATUS_APPLIED = "applied";

        /// <summary>
        /// condition status met 
        /// </summary>
        public const string CONDITION_STATUS_MET = "met";

        /// <summary>
        /// condition status not applied
        /// </summary>
        public const string CONDITION_STATUS_NOTAPPLIED = "not applied";

        /// <summary>
        /// Module level contact type data2's value.
        /// </summary>
        public const string RECORD_CONTACT_TYPE = "RecordContactType";

        /// <summary>
        /// copy to constant.
        /// </summary>
        public const string COPY_TO = "COPY";

        /// <summary>
        /// common for "1"
        /// </summary>
        public const string COMMON_ONE = "1";

        /// <summary>
        /// common for "0"
        /// </summary>
        public const string COMMON_ZERO = "0";

        /// <summary>
        /// culture name for web service
        /// </summary>
        public const string CULTURE_FOR_WEB_SERVICE = "en-US";

        /// <summary>
        /// "CURRENT_URL"; need to be backward compatible, the existing agency may be using previous hard code session name 'currentUrl '.
        /// </summary>
        public const string CURRENT_URL = "currentUrl ";

        /// <summary>
        /// default find app date range
        /// </summary>
        public const int DEFAULT_FIND_APP_DATE_RANGE = 30;

        /// <summary>
        /// the module name from url, each page should pass this parameter.
        /// </summary>
        public const string DEFAULT_MODULE_NAME = "Module Name";

        /// <summary>
        /// the global search parameter key
        /// </summary>
        public const string GLOBAL_SEARCH_QUERY_TEXT = "QueryText";

        /// <summary>
        /// default temp directory
        /// </summary>
        public const string DEFAULT_TEMP_DIRECTORY = "c:\\ACA\\temp";

        /// <summary>
        /// default customization directory
        /// </summary>
        public const string DEFAULT_CUSTOMIZATION_DIRECTORY = "Customization";

        /// <summary>
        /// the default VEMAP URI.
        /// </summary>
        public const string DEFAULT_VEMAP_URI = "http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2";

        /// <summary>
        /// description required flag 
        /// </summary>
        public const string DESCRIPTION_REQUIRED_FLAG = "DESCRIPTION_REQUIRED_FLAG";

        /// <summary>
        ///  EDMS policy model 
        /// </summary>
        public const string EDMS_POLICY_MODEL = "EDMSPolicyModel";

        /// <summary>
        ///  education look up. 
        /// </summary>
        public const string EDUCATION_LOOKUP = "EDUCATION_LOOKUP";

        /// <summary>
        /// emse add license validation 
        /// </summary>
        public const string EMSE_ADD_LICENSE_VALIDATION = "AddLicenseValidation4ACA";

        /// <summary>
        /// emse after login
        /// </summary>
        public const string EMSE_AFTER_LOGON = "OnLoginEventAfter4ACA";

        /// <summary>
        /// emse application submit after 
        /// </summary>
        public const string EMSE_APPLICATION_SUBMIT_AFTER = "ApplicationSubmitAfter";

        /// <summary>
        /// emse before login
        /// </summary>
        public const string EMSE_BEFORE_LOGON = "OnLoginEventBefore4ACA";

        /// <summary>
        /// emse select license validation 
        /// </summary>
        public const string EMSE_SELECT_LICENSE_VALIDATION = "SelectLicenseValidation4ACA";

        /// <summary>
        /// emse shopping cart checkout before
        /// </summary>
        public const string EMSE_SHOPPINGCART_CHECKOUT_BEFORE = "ShoppingCartCheckOutBefore";

        /// <summary>
        /// enable app name constant 
        /// </summary>
        public const string ENABLE_APP_NAME_CONSTANT = "ENABLE_APP_NAME";

        /// <summary>
        ///  enable asynchronous upload 
        /// </summary>
        public const string ENABLE_ASYNCHRONOUS_UPLOAD = "ENABLE_ASYNCHRONOUS_UPLOAD";

        /// <summary>
        /// enable synchronous upload 
        /// </summary>
        public const string ENABLE_SYNCHRONOUS_UPLOAD = "ENABLE_SYNCHRONOUS_UPLOAD";

        /// <summary>
        /// fee_estimate_after for aca 
        /// </summary>
        public const string FEE_ESTIMATE_AFTER4ACA = "FeeEstimateAfter4ACA";

        /// <summary>
        /// fee estimator 
        /// </summary>
        public const string FEE_ESTIMATOR = "FeeEstimator";

        /// <summary>
        /// from shopping cart 
        /// </summary>
        public const string FROMSHOPPINGCART = "isFromShoppingCart";

        /// <summary>
        /// Request parameter is renewal flag.
        /// </summary>
        public const string REQUEST_PARMETER_ISRENEWAL = "isRenewal";

        /// <summary>
        /// Request parameter is pay fee due flag.
        /// </summary>
        public const string REQUEST_PARMETER_ISPAYFEEDUE = "isPay4ExistingCap";

        /// <summary>
        /// full date format 
        /// </summary>
        public const string LONG_DATE_FORMAT = "dddd, MMM dd yyyy";

        /// <summary>
        /// short date format
        /// </summary>
        public const string SHORT_DATE_FORMAT = "MMM dd, yyyy";

        /// <summary>
        /// char code 18 is used to separate string.
        /// </summary>
        public const int CHAR_CODE_EIGHTEEN = 18;

        /// <summary>
        /// the right to access the policy data
        /// </summary>
        public const string GRANTED_RIGHT = "ACA";

        /// <summary>
        /// the Global.
        /// </summary>
        public const string GLOBAL = "Global";

        /// <summary>
        /// hold condition
        /// </summary>
        public const string HOLD_CONDITION = "HOLD";

        /// <summary>
        /// hour12 minute time format
        /// </summary>
        public const string HOUR12_MINTUTE_TIME_FORMAT = "hh:mm";

        /// <summary>
        /// html br code.
        /// </summary>
        public const string HTML_BR = "<br/>                "; // keep the space to let the content which contains English & Arabic works in IE8

        /// <summary>
        /// html empty tag.
        /// </summary>
        public const string HTML_NBSP = "&nbsp;";

        /// <summary>
        /// html NOBR tag.
        /// </summary>
        public const string HTML_NOBR = "<nobr>";

        /// <summary>
        /// html NOBR end tag.
        /// </summary>
        public const string HTML_SLASH_NOBR = "</nobr>";

        /// <summary>
        /// I18N settings currency locale 
        /// </summary>
        public const string I18N_SETTINGS_CURRENCY_LOCALE = "CURRENCY_LOCALE";

        /// <summary>
        /// I18N settings address locale 
        /// </summary>
        public const string I18N_SETTINGS_ADDRESS_LOCALE = "ADDRESS_LOCALE";

        /// <summary>
        /// I18N settings currency symbol 
        /// </summary>
        public const string I18N_SETTINGS_CURRENCY_SYMBOL = "CURRENCY_SYMBOL";

        /// <summary>
        /// I18N settings date format 
        /// </summary>
        public const string I18N_SETTINGS_DATE_FORMAT = "DATE_FORMAT";

        /// <summary>
        /// I18N settings long date format 
        /// </summary>
        public const string I18N_SETTINGS_LONG_DATE_FORMAT = "LONG_DATE_FORMAT";

        /// <summary>
        /// I18N settings date time format 
        /// </summary>
        public const string I18N_SETTINGS_DATE_TIME_FORMAT = "DATE_TIME_FORMAT";

        /// <summary>
        /// I18N settings hide language options
        /// </summary>
        public const string I18N_SETTINGS_HIDE_LANGUAGE_OPTIONS = "HIDE_LANGUAGE_OPTIONS";

        /// <summary>
        /// I18N settings default language 
        /// </summary>
        public const string I18N_SETTINGS_DEFAULT_LANGUAGE = "I18N_DEFAULT_LANGUAGE";

        /// <summary>
        /// I18N settings multi language support enable 
        /// </summary>
        public const string I18N_SETTINGS_MULTI_LANGUAGE_SUPPORT_ENABLE = "MULTI-LANGUAGE SUPPORT ENABLE";

        /// <summary>
        /// cap ID constant.
        /// </summary>
        public const string ID = "ID";

        /// <summary>
        /// incomplete status for cap be created for fee estimate.
        /// </summary>
        public const string INCOMPLETE = "INCOMPLETE";

        /// <summary>
        /// This cap be saved when click save and resume button.
        /// </summary>
        public const string INCOMPLETE_CAP = "INCOMPLETE CAP";

        /// <summary>
        /// incomplete EST
        /// EMSE(ApplicationSubmitBefore/After) will don't be fired when this kind of cap be saved.
        /// </summary>
        public const string INCOMPLETE_EST = "INCOMPLETE EST";

        /// <summary>
        /// incomplete temporary cap 
        /// This is a temporary cap when click continue with selected cap type in cap type page.it is created after cap type be selected.
        /// </summary>
        public const string INCOMPLETE_TEMP_CAP = "INCOMPLETE TMP";

        /// <summary>
        /// the inspection's success flag
        /// </summary>
        public const string INSPECTION_FLAG_SUCCESS = "S";

        /// <summary>
        /// the key of "is to show inspection"
        /// </summary>
        public const string IS_TO_SHOW_INSPECTION = "IsToShowInspection";

        /// <summary>
        /// inspection action cancel 
        /// </summary>
        public const string INSPECTION_ACTION_CANCEL = "Cancel";

        /// <summary>
        /// inspection action cancelled 
        /// </summary>
        public const string INSPECTION_ACTION_CANCELED = "Cancelled";

        /// <summary>
        /// inspection action reschedule 
        /// </summary>
        public const string INSPECTION_ACTION_RESCHEDULE = "Reschedule";

        /// <summary>
        /// inspection action schedule 
        /// </summary>
        public const string INSPECTION_ACTION_SCHEDULE = "Schedule";

        /// <summary>
        /// inspection activity date 
        /// </summary>
        public const string INSPECTION_ACTIVITY_DATE = "date";

        /// <summary>
        ///  switch for cancel operation
        /// </summary>
        public const string INSPECTION_SWITCH_FOR_CANCEL_OPERATION = "switch4CancelOperation";

        /// <summary>
        /// reschedule restriction settings
        /// </summary>
        public const string INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS = "rescheduleRestrictionSettings";

        /// <summary>
        /// cancellation restriction settings
        /// </summary>
        public const string INSPECTION_CANCELLATION_RESTRICTION_SETTINGS = "cancellationRestrictionSettings";

        /// <summary>
        /// is ready time enabled
        /// </summary>
        public const string INSPECTION_IS_READY_TIME_ENABLED = "isReadyTimeEnabled";

        /// <summary>
        /// The property "requiredInspection" in inspectionType model, value is "Y" or "N"
        /// </summary>
        public const string INSPECTION_ISREQUIREDINSPECTION = "IsRequiredInspection";

        /// <summary>
        /// inspection group 
        /// </summary>
        public const string INSPECTION_GROUP = "group";

        /// <summary>
        /// inspection id number 
        /// </summary>
        public const string INSPECTION_ID_NUMBER = "typeId";

        /// <summary>
        /// inspection in advance 
        /// </summary>
        public const string INSPECTION_IN_ADVANCE = "inAdvance";

        /// <summary>
        /// inspection action
        /// </summary>
        public const string INSPECTION_OPERATION = "action";

        /// <summary>
        /// inspection result group 
        /// </summary>
        public const string INSPECTION_RESULT_GROUP = "resultGroup";

        /// <summary>
        /// inspection result pending 
        /// </summary>
        public const string INSPECTION_RESULT_PENDING = "PENDING";

        /// <summary>
        /// inspection result scheduled 
        /// </summary>
        public const string INSPECTION_RESULT_SCHEDULED = "Scheduled";

        /// <summary>
        /// inspection schedule type
        /// </summary>
        public const string INSPECTION_SCHEDULING_MANNER = "shecdulingManner";

        /// <summary>
        /// inspection schedule time range 
        /// </summary>
        public const string INSPECTION_SCHEDULE_TIME_RANGE = "timeRange";

        /// <summary>
        /// inspection sequence number
        /// </summary>
        public const string INSPECTION_SEQUENCE_NUBMER = "inspeSeqNbr";

        /// <summary>
        /// inspection status key cancelled 
        /// </summary>
        public const string INSPECTION_STATUS_KEY_CANCELED = "Cancelled";

        /// <summary>
        /// inspection status key pending 
        /// </summary>
        public const string INSPECTION_STATUS_KEY_PENDING = "Pending";

        /// <summary>
        /// inspection status key rescheduled 
        /// </summary>
        public const string INSPECTION_STATUS_KEY_RESCHEDULED = "Rescheduled";

        /// <summary>
        /// inspection status key scheduled 
        /// </summary>
        public const string INSPECTION_STATUS_KEY_SCHEDULED = "Scheduled";

        /// <summary>
        /// inspection type 
        /// </summary>
        public const string INSPECTION_TYPE = "type";

        /// <summary>
        /// inspection type label
        /// </summary>
        public const string INSPECTION_TYPE_LABEL = "TypeLabel";

        /// <summary>
        ///  inspection unit 
        /// </summary>
        public const string INSPECTION_UNIT = "InspectUnit";

        /// <summary>
        ///  invalid status 
        /// </summary>
        public const string INVALID_STATUS = "I";

        /// <summary>
        ///  disabled status 
        /// </summary>
        public const string DISABLED_STATUS = "D";

        /// <summary>
        ///  Enabled status 
        /// </summary>
        public const string ENABLED_STATUS = "E";

        /// <summary>
        /// level type agency 
        /// </summary>
        public const string LEVEL_TYPE_AGENCY = "AGENCY";

        /// <summary>
        /// level type module 
        /// </summary>
        public const string LEVEL_TYPE_MODULE = "MODULE";

        /// <summary>
        /// level type page flow 
        /// </summary>
        public const string LEVEL_TYPE_PAGEFLOW = "PAGE FLOW";

        /// <summary>
        /// suffix flag for label content
        /// </summary>
        public const string LABEL_CONTENT_SUFFIX = "_CONTENT";

        /// <summary>
        /// Cap condition constant four status.
        /// </summary>
        public const string LOCK_CONDITION = "LOCK";

        /// <summary>
        /// all module
        /// </summary>
        public const string MODULE_ALL = "ALL";

        /// <summary>
        /// module enabled suffix 
        /// </summary>
        public const string MODULE_ENABLED_SUFFIX = "_MODULE_ENABLED";

        /// <summary>
        /// the module name from url, each page should pass this parameter.
        /// </summary>
        public const string MODULE_NAME = "Module";

        /// <summary>
        /// the page flow name from url, each page should pass this parameter.
        /// </summary>
        public const string PAGE_FLOW_NAME = "PageFlow";

        /// <summary>
        /// move to constant.
        /// </summary>
        public const string MOVE_TO = "MOVE";

        /// <summary>
        /// non assign number 
        /// </summary>
        public const string NONASSIGN_NUMBER = "0";

        /// <summary>
        /// notice condition
        /// </summary>
        public const string NOTICE_CONDITION = "NOTICE";

        /// <summary>
        ///  no anonymous user
        /// </summary>
        public const string NO_ANONYMOUS_USER = "NO_ANONYMOUS_USER";

        /// <summary>
        /// no report assign error 
        /// </summary>
        public const string NO_REPORT_ASSIGN_ERROR = "NoReportAssignError:";

        /// <summary>
        /// online payment country code 
        /// </summary>
        public const string ONLINE_PAYMENT_COUNTRY_CODE = "CountryCode";

        /// <summary>
        /// online payment product id pre 
        /// </summary>
        public const string ONLINE_PAYMENT_PRODUCT_ID_PRE = "PRODUCT_";

        /// <summary>
        /// online payment web service API 
        /// </summary>
        public const string ONLINE_PAYMENT_WEBSERVICE_API = "API";

        /// <summary>
        /// online payment web service credit card url 
        /// </summary>
        public const string ONLINE_PAYMENT_WEBSERVICE_CREDIT_CARD_URL = "PAYMENT_CREDIT_CARD_URL";

        /// <summary>
        /// online payment web service ETISALAT
        /// </summary>
        public const string ONLINE_PAYMENT_WEBSERVICE_ETISALAT = "Etisalat";

        /// <summary>
        /// online payment web service online 
        /// </summary>
        public const string ONLINE_PAYMENT_WEBSERVICE_ONLINE = "ONLINE";

        /// <summary>
        /// online payment web service payment mode 
        /// </summary>
        public const string ONLINE_PAYMENT_WEBSERVICE_PAYMENT_MODE = "ACA_PAYMENT_MODE";

        /// <summary>
        /// online payment web service PayPal 
        /// </summary>
        public const string ONLINE_PAYMENT_WEBSERVICE_PAYPAL = "PAYPAL";

        /// <summary>
        /// online payment web service STP 
        /// </summary>
        public const string ONLINE_PAYMENT_WEBSERVICE_STP = "STP";

        /// <summary>
        /// online payment web service type 
        /// </summary>
        public const string ONLINE_PAYMENT_WEBSERVICE_TYPE = "Type";

        /// <summary>
        /// account verification page for expired
        /// </summary>
        public const string PAGE_ACCOUNT_VERIFICATION_EXPIRED = "../Account/AccountVerification.aspx?isExpiredPage=Y";

        /// <summary>
        /// register license page for viewing
        /// </summary>
        public const string PAGE_REGISTER_LICENSE_VIEW = "../Account/RegisterLicense.aspx?isLicenseView=Y";

        /// <summary>
        /// welcome page after registered
        /// </summary>
        public const string PAGE_WELCOME_REGISTERED = "../Welcome.aspx?registered=y";

        /// <summary>
        /// payment method CoBrand plus 
        /// </summary>
        public const string PAYMENT_METHOD_COBRANDPLUS = "CoBrandPlus";

        /// <summary>
        /// payment method GOVOLUTION online 
        /// </summary>
        public const string PAYMENT_METHOD_GOVOLUTION_ONLINE = "GovolutionOnline";

        /// <summary>
        /// payment method trust account 
        /// </summary>
        public const string PAYMENT_METHOD_TRUST_ACCOUNT = "Trust Account";

        /// <summary>
        /// payment processor 
        /// </summary>
        public const string PAYMENT_PROCESSOR = "Payment Processor";

        /// <summary>
        /// payment status
        /// </summary>
        public const string PAYMENT_STATUS = "PAYMENT_STATUS";

        /// <summary>
        /// payment status convert cap failed 
        /// </summary>
        public const string PAYMENT_STATUS_CONVERT_CAP_FAILED = "Convert CAP Failed";

        /// <summary>
        /// payment status convert cap success
        /// </summary>
        public const string PAYMENT_STATUS_CONVERT_CAP_SUCCESS = "Convert CAP Success";

        /// <summary>
        /// payment status creating cap 
        /// </summary>
        public const string PAYMENT_STATUS_CREATING_CAP = "Creating CAP";

        /// <summary>
        /// payment status paid failed.
        /// </summary>
        public const string PAYMENT_STATUS_PAID_FAILED = "Paid Failed";

        /// <summary>
        /// payment status start paid 
        /// </summary>
        public const string PAYMENT_STATUS_START_PAID = "Start Paid";

        /// <summary>
        /// payment status that have paid
        /// </summary>
        public const string PAYMENT_STATUS_PAID = "Paid";

        /// <summary>
        /// the expire time(60 minutes = 1 hour) of payment status in cache
        /// </summary>
        public const int PAYMENT_STAUTS_CACHE_EXPIRE_TIME = 60;

        /// <summary>
        /// payment total fee
        /// </summary>
        public const string PAYMENT_TOTALFEE = "TOTAL_FEE";

        /// <summary>
        /// pay method check 
        /// </summary>
        public const string PAY_METHOD_CHECK = "Check";

        /// <summary>
        /// pay method credit card 
        /// </summary>
        public const string PAY_METHOD_CREDIT_CARD = "Credit Card";

        /// <summary>
        /// pa workflow task after apply 
        /// </summary>
        public const string PA_WORKFLOW_TASK_AFTER_APPLY = "PA_WORKFLOW_TASK_AFTER_APPLY";

        /// <summary>
        /// pa workflow task apply comment
        /// </summary>
        public const string PA_WORKFLOW_TASK_APPLY_COMMENT = "PA_WORKFLOW_TASK_APPLY_COMMENT";

        /// <summary>
        /// print homepage report 
        /// </summary>
        public const string PRINT_HOMEPAGE_REPORT = "PRINT_HOMEPAGE_REPORT";

        /// <summary>
        /// print license report
        /// </summary>
        public const string PRINT_LICENSE_REPORT = "PRINT_LICENSE_REPORT";

        /// <summary>
        /// print payment receipt report
        /// </summary>
        public const string PRINT_PAYMENT_RECEIPT_REPORT = "PRINT_PAYMENT_RECEIPT_REPORT";

        /// <summary>
        /// print trust account receipt report.
        /// </summary>
        public const string PRINT_TRUST_ACCOUNT_RECEIPT_REPORT = "PRINT_TRUST_ACCOUNT_RECEIPT_REPORT";

        /// <summary>
        /// print permit report
        /// </summary>
        public const string PRINT_PERMIT_REPORT = "PRINT_PERMIT_REPORT";

        /// <summary>
        /// print requirements report.
        /// </summary>
        public const string PRINT_REQUIREMENTS_REPORT = "PRINT_REQUIREMENTS_REPORT";

        /// <summary>
        /// print permit summary report 
        /// </summary>
        public const string PRINT_PERMIT_SUMMARY_REPORT = "PRINT_PERMIT_SUMMARY_REPORT";

        /// <summary>
        /// print report list 
        /// </summary>
        public const string PRINT_REPORT_LIST = "LINK_REPORT_LIST";

        /// <summary>
        /// The action for Authorized Agent Label print.
        /// </summary>
        public const string PRINT_LABEL_VIEWID = "PRINT_LABEL_VIEWID";

        /// <summary>
        /// cache key for checking product license message info 
        /// </summary>
        public const string PRODUCT_LICENSE_KEY = "Product_license";

        /// <summary>
        /// product name. 
        /// </summary>
        public const string PRODUCT_NAME = "ACCELA_ACA";

        /// <summary>
        /// ACA's user id is required to add the prefix.
        /// </summary>
        public const string PUBLIC_USER_NAME = "PUBLICUSER";

        /// <summary>
        /// renewal complete
        /// </summary>
        public const string RENEWAL_COMPLETE = "Complete";

        /// <summary>
        /// renewal incomplete 
        /// </summary>
        public const string RENEWAL_INCOMPLETE = "Incomplete";

        /// <summary>
        /// agency id parameter for report.
        /// </summary>
        public const string REPORT_AGENCY_ID = "agencyid";

        /// <summary>
        /// batch transaction number parameter for report.
        /// </summary>
        public const string REPORT_BATCHTRANSACTION_NBR = "batchtransactionnbr";

        /// <summary>
        /// cap id parameter fro report.
        /// </summary>
        public const string REPORT_CAP_ID = "capid";

        /// <summary>
        /// Report parameter page root.
        /// </summary>
        public const string REPORT_PARAMETER_PAGE = "../Report/ReportParameter.aspx";

        /// <summary>
        /// Receipt number parameter for report.
        /// </summary>
        public const string REPORT_RECEIPT_NUMBER = "receiptnbr";

        /// <summary>
        /// RTF report output type.
        /// </summary>
        public const string REPORT_RTF_OUTPUT_TYPE = "RTF";

        /// <summary>
        /// URL report output type.
        /// </summary>
        public const string REPORT_URL_OUTPUT_TYPE = "URL_Report";

        /// <summary>
        /// request parameter address sequence 
        /// </summary>
        public const string REQUEST_PARMETER_ADDRESS_SEQUENCE = "AddressSeq";

        /// <summary>
        /// request parameter parcel reference number. It is for internal APO.
        /// </summary>
        public const string REQUEST_PARMETER_PARCEL_NUMBER = "ParcelNum";

        /// <summary>
        /// request parameter parcel source sequence number.
        /// </summary>
        public const string REQUEST_PARMETER_PARCEL_SEQUENCE = "ParcelSeq";

        /// <summary>
        /// request parameter address or parcel duplicate source number.
        /// </summary>
        public const string REQUEST_PARMETER_APO_SOURCE_NUMBERS = "sourceNumbs";

        /// <summary>
        /// request parameter external parcel unique id. It is for external APO.
        /// </summary>
        public const string REQUEST_PARMETER_PARCEL_UID = "ParcelUID";

        /// <summary>
        /// request parameter owner reference number. It is for internal APO.
        /// </summary>
        public const string REQUEST_PARMETER_OWNER_NUMBER = "OwnerNum";

        /// <summary>
        /// request parameter owner source sequence number.
        /// </summary>
        public const string REQUEST_PARMETER_OWNER_SEQUENCE = "OwnerSeq";

        /// <summary>
        /// request parameter external owner unique id. It is for external APO.
        /// </summary>
        public const string REQUEST_PARMETER_OWNER_UID = "OwnerUID";

        /// <summary>
        /// request parameter plan id 
        /// </summary>
        public const string REQUEST_PARMETER_PLAN_ID = "planid";

        /// <summary>
        /// request parameter reference address id. It is for internal APO.
        /// </summary>
        public const string REQUEST_PARMETER_REFADDRESS_ID = "AddressID";

        /// <summary>
        /// request parameter external address unique id. It is for external APO.
        /// </summary>
        public const string REQUEST_PARMETER_REFADDRESS_UID = "AddressUID";

        /// <summary>
        /// request parameter super agency 
        /// </summary>
        public const string REQUEST_PARMETER_SUPER_AGENCY = "SUPERAGENCY";

        /// <summary>
        /// request parameter trade license 
        /// </summary>
        public const string REQUEST_PARMETER_TRADE_LICENSE = "TRADELICENSE";

        /// <summary>
        /// request parameter trade name 
        /// </summary>
        public const string REQUEST_PARMETER_TRADE_NAME = "TRADENAME";

        /// <summary>
        /// request parameter tran id 
        /// </summary>
        public const string REQUEST_PARMETER_TRAN_ID = "tranid";

        /// <summary>
        /// required condition
        /// </summary>
        public const string REQUIRED_CONDITION = "REQUIRED";

        /// <summary>
        /// role type citizen 
        /// </summary>
        public const string ROLE_TYPE_CITIZEN = "0";

        /// <summary>
        /// selected page of page picker
        /// </summary>
        public const string SELECTED_PAGE_OF_PAGE_PICKER = "SELECTED_PAGE_OF_PAGE_PICKER";

        /// <summary>
        /// service lock condition
        /// </summary>
        public const string SERVICE_LOCK_CONDITION = "SRVLOCK";

        /// <summary>
        /// shopping cart expiration default day
        /// </summary>
        public const string SHOPPINGCART_EXPIRATION_DEFAULT_DAY = "30";

        /// <summary>
        /// shopping cart transaction default value
        /// </summary>
        public const string SHOPPINGCART_TRANSACTION_DEFAULT_VALUE = "1";

        /// <summary>
        /// shopping cart items
        /// </summary>
        public const string SHOPPING_CART_ITEMS = "Shopping_Cart_Items";

        /// <summary>
        /// slash code.
        /// </summary>
        public const string SLASH = "/";

        /// <summary>
        /// split line code.
        /// </summary>
        public const string SPLITLINE = " - ";

        /// <summary>
        /// split char for URL 1.
        /// </summary>
        public const char SPLIT_CHAR4URL1 = '|';

        /// <summary>
        /// split char.
        /// </summary>
        public const char SPLIT_CHAR = '\f';

        /// <summary>
        /// split char1.
        /// </summary>
        public const char SPLIT_CHAR1 = '\a';

        /// <summary>
        /// split char2.
        /// </summary>
        public const char SPLIT_CHAR2 = '\b';

        /// <summary>
        /// split char3.
        /// </summary>
        public const string SPLIT_CHAR3 = "|_|";

        /// <summary>
        /// split char4.
        /// </summary>
        public const string SPLIT_CHAR4 = "-";

        /// <summary>
        /// split with "||"
        /// </summary>
        public const string SPLIT_DOUBLE_VERTICAL = "||";

        /// <summary>
        /// split with "::";
        /// </summary>
        public const string SPLIT_DOUBLE_COLON = "::";

        /// <summary>
        /// split char5.
        /// </summary>
        public const string SPLIT_CHAR5 = "_";

        /// <summary>
        /// split char6.
        /// </summary>
        public const char SPLIT_CHAR6 = '$';

        /// <summary>
        /// split with "|,"
        /// </summary>
        public const string SPLIT_CHAR7 = "|,";

        /// <summary>
        /// spot char.
        /// </summary>
        public const string SPOT_CHAR = ".";

        /// <summary>
        /// delimiter - STAR
        /// </summary>
        public const string DELIMITER_STAR = "*";

        /// <summary>
        /// colon code
        /// </summary>
        public const string COLON_CHAR = ": ";

        /// <summary>
        /// comma code.
        /// </summary>
        public const string COMMA = ",";

        /// <summary>
        /// comma blank code.
        /// </summary>
        public const string COMMA_BLANK = ", ";

        /// <summary>
        /// comma char
        /// </summary>
        public const char COMMA_CHAR = ',';

        /// <summary>
        /// comma char '%'
        /// </summary>
        public const char COMMA_PERCENT = '%';

        /// <summary>
        /// Semicolon char
        /// </summary>
        public const char SPLIT_CHAR_SEMICOLON = ';';

        /// <summary>
        /// semicolon blank.
        /// </summary>
        public const string SEMICOLON_BLANK = "; ";

        /// <summary>
        /// Equal mark
        /// </summary>
        public const string EQUAL_MARK = "=";

        /// <summary>
        /// Question mark
        /// </summary>
        public const string QUESTION_MARK = "?";

        /// <summary>
        /// Ampersand mark
        /// </summary>
        public const string AMPERSAND = "&";

        /// <summary>
        /// culture separator for .Net
        /// </summary>
        public const string CULTURE_SEPARATOR_4_DOTNET = "-";

        /// <summary>
        /// culture separator for Java
        /// </summary>
        public const string CULTURE_SEPARATOR_4_JAVA = "_";

        /// <summary>
        /// Replace single quote by double quote.
        /// </summary>
        public const string DOUBLE_QUOTE = "''";

        /// <summary>
        /// Single quote.
        /// </summary>
        public const string SINGLE_QUOTE = "'";

        /// <summary>
        /// standard data.
        /// </summary>
        public const string STANDARDDATA = "STANDARDDATA";

        /// <summary>
        /// State from Standard data.
        /// </summary>
        public const string STD_STANDARDDATA_STATES = "STATES";

        /// <summary>
        /// suffix links anonymous 
        /// </summary>
        public const string SUFFIX_LINKS_ANONYMOUS = "_LINKS_ANONYMOUS";

        /// <summary>
        /// suffix links register user.
        /// </summary>
        public const string SUFFIX_LINKS_REGUSER = "_LINKS_REGUSER";

        /// <summary>
        /// key of service provide code setting in web.config file
        /// </summary>
        public const string ServProvCode_Key = "ServProvCode";

        /// <summary>
        /// key of payment extra parameters
        /// </summary>
        public const string CREDIT_CARD_NBR_KEY = "CreditCardNbr";

        /// <summary>
        /// time format, only for web service
        /// </summary>
        public const string TIME_FORMAT = "HH:mm";

        /// <summary>
        /// upload file detail information
        /// </summary>
        public const string UPLOAD_FILE_DETAILINFO = "_DetailInfo.xml";

        /// <summary>
        /// url account verification 
        /// </summary>
        public const string URL_ACCOUNT_VERIFICATION = "Account/AccountVerification.aspx";

        /// <summary>
        /// Header Navigation Constant
        /// </summary>
        public const string URL_DEFAULT = "~/Welcome.aspx";

        /// <summary>
        /// url default page
        /// </summary>
        public const string URL_DEFAULT_PAGE = "~/Default.aspx";

        /// <summary>
        /// url welcome page
        /// </summary>
        public const string URL_WELCOME_PAGE = "Welcome.aspx";

        /// <summary>
        /// url aca new UI
        /// </summary>
        public const string URL_NEW_UI = "/NewUI/Home.aspx";

        /// <summary>
        /// user role policy for cap search 
        /// </summary>
        public const string USER_ROLE_POLICY_FOR_CAP_SEARCH = "ACA_CAP_SEARCH_USER_ROLES";

        /// <summary>
        /// user role policy for inspection schedule
        /// </summary>
        public const string USER_ROLE_POLICY_FOR_INSPECTION_SCHEDULE = "ACA_INSPECTION_USER_ROLES";

        /// <summary>
        /// user role policy for inspection input contact
        /// </summary>
        public const string USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT = "ACA_INSPECTION_USER_ROLES_INPUT_CONTACT";

        /// <summary>
        /// user role policy for inspection view contact
        /// </summary>
        public const string USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT = "ACA_INSPECTION_USER_ROLES_VIEW_CONTACT";

        /// <summary>
        /// use to distinguish the ASI and ASI Table.
        /// </summary>
        public const string ASI_CHECKBOX_GROUP = "APPLICATION";

        /// <summary>
        /// default width value for ASI Label(chars)
        /// </summary>
        public const int ASI_LABEL_WIDTH_DEFAULT = 25;

        /// <summary>
        /// valid status 
        /// </summary>
        public const string VALID_STATUS = "A";

        /// <summary>
        /// VCH type EST.
        /// </summary>
        public const string VCH_TYPE_EST = "EST";

        /// <summary>
        /// Account constant
        /// </summary>
        public const string VCH_TYPE_VHAPP = "VHAPP";

        /// <summary>
        /// Renewal CAP is ready for review.
        /// </summary>
        public const string RENEWAL_REVIEW = "Review";

        /// <summary>
        /// The SSN or FEIN error code.
        /// </summary>
        public const string SSNORFEIN_ERRORCODE = "2";

        /// <summary>
        /// Renewal for pay fee due.
        /// </summary>
        public const string PAYFEEDUE_RENEWAL = "PayFeeDue4Renewal";

        /// <summary>
        /// Request trade license
        /// </summary>
        public const string REQUEST_TRADE_LICENSE = "RequestTradeLic";

        /// <summary>
        /// Renewal for deferred payment.
        /// </summary>
        public const string DEFERPAY_RENEWAL = "DeferredPayment";

        /// <summary>
        /// constant for common true
        /// </summary>
        public const string COMMON_TRUE = "true";

        /// <summary>
        /// constant for common false
        /// </summary>
        public const string COMMON_FALSE = "false";

        /// <summary>
        /// drop down control id prefix.
        /// </summary>
        public const string DROPDOWNLIST_CONTROLID_PREFIXION = "ddl";

        /// <summary>
        /// Template EMSE drop down list attribute name for saving template field name.
        /// </summary>
        public const string TEMPLATE_FIELD_NAME_ATTRIBUTE_FOR_EMSEDDL = "EMSETemplate";

        /// <summary>
        /// Null value.
        /// </summary>
        public const string NULL_VALUE = "N/A";

        /// <summary>
        /// NULL string.
        /// </summary>
        public const string NULL_STRING = "null";

        /// <summary>
        /// Examination score pass for pass/fail grading style.
        /// </summary>
        public const string EXAM_SCORE_PASS = "Pass";

        /// <summary>
        /// Examination score fail for pass/fail grading style.
        /// </summary>
        public const string EXAM_SCORE_FAIL = "Fail";

        /// <summary>
        /// The Currency Max length.
        /// </summary>
        public const int CURRENCY_MAX_LENGTH = 20;

        /// <summary>
        /// The Currency Max length.
        /// </summary>
        public const int CURRENCY_MAX_DIGITS_LENGTH = 2;

        /// <summary>
        /// a constant for always editable template fields
        /// </summary>
        public const string TEMPLATE_FIIELD_STATUS_ALWAYSEDITABLE = "E";

        /// <summary>
        /// display license verification model
        /// </summary>
        public const string TEMPLATE_FIIELD_STATUS_LIC_VERIFICATION_MODEL = "V";

        /// <summary>
        /// display license verification value
        /// </summary>
        public const string TEMPLATE_FIIELD_STATUS_LIC_VERIFICATION_VALUE = "A";

        /// <summary>
        /// Dropdownlist option group.
        /// </summary>
        public const string OPTION_GROUP = "optgroup";

        /// <summary>
        /// a constant for template field prefix
        /// </summary>
        public const string TEMPLATE_FIIELD_PREFIX = "Template";

        /// <summary>
        /// Has permission role constant.
        /// </summary>
        public const string ROLE_HASPERMISSION = "1000000000";

        /// <summary>
        /// No permission role constant.
        /// </summary>
        public const string ROLE_NOPERMISSION = "0000000000";

        /// <summary>
        /// No permission role constant.
        /// </summary>
        public const string ROLE_PROXYUSER_NOPERMISSION = "000000000000000";

        /// <summary>
        /// default permission,"1" means allowed.all aca user,register user,cap creator,owner,contact,LP.
        /// </summary>
        public const string DEFAULT_PERMISSION = "1111110000";

        /// <summary>
        /// Default file owner permission, "1" means allowed.
        /// Follow left to right is: TitleViewable, Downloadable, Uploadable, Deletable
        /// </summary>
        public const string DEFAULT_FILEOWNERPERMISSION = "1111000000";
        
        /// <summary>
        /// text default width on grid 960 system.
        /// </summary>
        public const int GRID960_TEXT_DEFAULT_WIDTH = 10;

        /// <summary>
        /// field default width on grid 960 system.
        /// </summary>
        public const int GRID960_FIELD_DEFAULT_WIDTH = 12;

        /// <summary>
        /// width default unit on grid 960 system.
        /// </summary>
        public const int GRID960_WIDTH_DEFAULT_UNIT = 10;

        /// <summary>
        /// add summary to data table
        /// </summary>
        public const string SUMMARY = "summary";

        /// <summary>
        /// field type: 1 - Text, 2 - Date, 3 - Y/N, 4 - Number, 5 - Dropdown List, 6 - Text area, 7 - Time 8 - Money 9 -* CheckBox                     
        /// </summary>
        public const string CONTROL_CHECKBOX_ID = "9";

        /// <summary>
        /// The radio box id.
        /// </summary>
        public const string CONTROL_RADIOBOX_ID = "3";

        /// <summary>
        /// the select box id.
        /// </summary>
        public const string CONTROL_SELECTBOX_ID = "5";

        /// <summary>
        /// The asset security address tab
        /// </summary>
        public const string ASSET_SECURITY_ADDRESS_TAB = "Address Tab";

        /// <summary>
        /// control string name for radio 
        /// </summary>
        public const string CONTROL_RADIO_TYPE = "RADIO";

        /// <summary>
        /// In super agency,the first row of cap fee list is cap type row.
        /// </summary>
        public const string CAP_TYPE_ROW = "capTypeRow";

        /// <summary>
        /// Gets direct trust accounts.
        /// </summary>
        public const string GET_DIRECT_TRUST_ACCOUNT = "Direct";

        /// <summary>
        /// Gets indirect trust accounts
        /// </summary>
        public const string GET_INDIRECT_TRUST_ACCOUNT = "Indirect";

        /// <summary>
        /// Get parent's trust accounts
        /// </summary>
        public const string GET_PARENT_TRUST_ACCOUNT = "Parent";

        #region Some Constants For Expression

        /// <summary>
        /// This key is used to flag expression system variables
        /// </summary>
        public const string Expression_SystemVariables = "EXPRESSION_SYSTEM_VARIABLES";

        /// <summary>
        /// This key is used to flag expression input variables
        /// </summary>
        public const string Expression_InputVariables = "EXPRESSION_INPUT_VARIABLES";

        #endregion

        /// <summary>
        /// Display Send_Email Button for Report or not.
        /// </summary>
        public const string Display_Send_Email = "isDisplaySendEmail";

        /// <summary>
        /// Attach all Set Member CAP contacts
        /// </summary>
        public const string ATTACH_ALL_CAP_SET_MEMBERS_CONTACTS = "ATTACH_ALL_SET";

        /// <summary>
        /// Attach all Set Member CAP primary contacts
        /// </summary>
        public const string ATTACH_PRIMARY_CAP_SET_MEMBERS_CONTACTS = "ATTACH_PRI_SET";

        /// <summary>
        /// Attach all CAP contacts.
        /// </summary>
        public const string ATTACH_ALL_CAP_CONTACTS = "ATTACH_ALL";

        /// <summary>
        /// Attach primary CAP contact.
        /// </summary>
        public const string ATTACH_PRIMARY_CAP_CONTACTS = "ATTACH_PRI";

        /// <summary>
        /// Attach contact type.
        /// </summary>
        public const string ATTACH_CONTACT_TYPE = "attachContacttype";

        /// <summary>
        /// Email report name.
        /// </summary>
        public const string EMAIL_REPORT_NAME = "emailReportName";

        /// <summary>
        /// AGIS command name:schedule inspection
        /// </summary>
        public const string AGIS_COMMAND_SCHEDULE_INSPECTION = "CREATE_INSPECTION";

        /// <summary>
        /// AGIS command :send address
        /// </summary>
        public const string AGIS_COMMAND_SEND_ADDRESS = "SEND_ADDRESS";

        /// <summary>
        /// AGIS command:send feature
        /// </summary>
        public const string AGIS_COMMAND_SEND_FEATURES = "SEND_FEATURES";

        /// <summary>
        /// AGIS command:Create Cap.
        /// </summary>
        public const string AGIS_COMMAND_CREATE_CAP = "CREATE_CAP";

        /// <summary>
        /// AGIS command:SERVICE REQUEST.
        /// </summary>
        public const string AGIS_COMMAND_SERVICE_REQUEST = "SERVICE_REQUEST";

        /// <summary>
        /// AGIS COMMAND :resume application
        /// </summary>
        public const string AGIS_COMMAND_RESUME = "RESUME";

        /// <summary>
        /// AGIS command:SHOW_ACCELA_RECORD;
        /// </summary>
        public const string AGIS_COMMAND_SHOW_ACCELA_RECORD = "SHOW_ACCELA_RECORD";

        /// <summary>
        /// AGIS command: show geo document
        /// </summary>
        public const string AGIS_COMMAND_SHOW_GEODOCUMENT = "SHOW_DOCUMENTS";

        /// <summary>
        /// AGIS command: send address to search asset
        /// </summary>
        public const string AGIS_COMMAND_SEND_ASSET = "SEND_ASSET";

        /// <summary>
        /// XPolicy key for page size.
        /// </summary>
        public const string ACA_PAGE_SIZE = "ACA_PAGE_SIZE";

        /// <summary>
        /// indicate whether cloning a record.
        /// </summary>
        public const string IS_CLONE_RECORD = "isCloneRecord";

        /// <summary>
        /// indicate whether is sub agency cap.
        /// </summary>
        public const string IS_SUBAGENCY_CAP = "isSubAgencyCap";

        /// <summary>
        /// ASI Name value.
        /// </summary>
        public const string EXP_ASI_FIELD_NAME_PREFIX = "app_spec_info";

        /// <summary>
        /// ASI Table Name
        /// </summary>
        public const string EXP_ASIT_FIELD_NAME_PREFIX = "app_spec_info_table";

        /// <summary>
        /// Finished status
        /// </summary>
        public const string FINISHED_STATUS = "Finished";

        /// <summary>
        /// Finished status
        /// </summary>
        public const string UPLOADING_STATUS = "Uploading";

        /// <summary>
        /// Asset List Layout in Cap Confirm page
        /// </summary>
        public const string ASSETLIST_LAYOUT_CAP_CONFIRM = "CapConfirm";

        /// <summary>
        /// Asset List Layout in Cap Detail page
        /// </summary>
        public const string ASSETLIST_LAYOUT_CAP_DETAIL = "CapDetail";

        /// <summary>
        /// if daily user input custom data, it's licenseNumber will be saved as -3
        /// </summary>
        public const string DAILY_LICENSE_NUMBER = "-3";

        #region For NIGP group

        /// <summary>
        /// NIGP hard code to define NIGP sub group.
        /// </summary>
        public const string NIGP_SUBGROUP_NIGPCODE = "NIGP Code";

        /// <summary>
        /// NIGP hard code to define NIGP field for NIGP code class.
        /// </summary>
        public const string NIGP_FIELD_NIGPCODE_CLASS = "NIGP Class";

        /// <summary>
        /// NIGP hard code to define NIGP field for NIGP code sub class.
        /// </summary>
        public const string NIGP_FIELD_NIGPCODE_SUBCLASS = "NIGP Sub Class";

        /// <summary>
        /// NIGP hard code to define NIGP field for maximum contract value.
        /// </summary>
        public const string NIGP_FIELD_MAX_CONTRACT = "Maximum Contract Value";

        /// <summary>
        /// NIGP hard code to define field for certification type.
        /// </summary>
        public const string NIGP_FIELD_CERTIFICATION_TYPE = "Certification Type";

        /// <summary>
        /// NIGP hard code to define field for expiration date.
        /// </summary>
        public const string NIGP_FIELD_CERTIFICATION_EXPIRATION_DATE = "Expiration Date";

        /// <summary>
        /// NIGP hard code to define region field.
        /// </summary>
        public const string NIGP_FIELD_REGION = "Region";

        /// <summary>
        /// NIGP hard code to define ethnicity field.
        /// </summary>
        public const string NIGP_FIELD_ETHNICITY = "Ethnicity";

        /// <summary>
        /// NIGP hard code to define field for location.
        /// </summary>
        public const string NIGP_FIELD_LOCATION = "Location";

        /// <summary>
        /// NIGP hard code to define field for contract experience of client name.
        /// </summary>
        public const string NIGP_FIELD_CONTRACT_EXPERIENCE_CLIENT = "Name of Client";

        /// <summary>
        /// NIGP hard code to define field for contract experience description.
        /// </summary>
        public const string NIGP_FIELD_CONTRACT_EXPERIENCE_DESCRIPTION = "Description of Work";

        /// <summary>
        /// NIGP hard code to define field for date of work.
        /// </summary>
        public const string NIGP_FIELD_CONTRACT_EXPERIENCE_WORKDATE = "Date of Work";

        /// <summary>
        /// NIGP hard code to define field for job value.
        /// </summary>
        public const string NIGP_FIELD_CONTRACT_EXPERIENCE_JOBVALUE = "Value of Job";

        /// <summary>
        /// NIGP hard code to define field for web site.
        /// </summary>
        public const string NIGP_FIELD_WEBSITE = "Website";

        #endregion For NIGP group

        #region prefix for template control id (it is same as aa)

        /// <summary>
        /// Control id prefix for LP template.
        /// </summary>
        public const string CAP_PROFESSIONAL_TEMPLATE_FIELD_PREFIX = "template_B3PROFESSIONAL_";

        /// <summary>
        /// Control id prefix for address template.
        /// </summary>
        public const string CAP_ADDRESS_TEMPLATE_FIELD_PREFIX = "template_B3ADDRESS_";

        /// <summary>
        /// Control id prefix for parcel template.
        /// </summary>
        public const string CAP_PARCEL_TEMPLATE_FIELD_PREFIX = "template_B3PARCEL_";

        /// <summary>
        /// Control id prefix for owner template.
        /// </summary>
        public const string CAP_OWNER_TEMPLATE_FIELD_PREFIX = "template_B3OWNER_";

        /// <summary>
        /// control id prefix for contact list template.
        /// </summary>
        public const string CAP_CONTACTS_TEMPLATE_FIELD_PREFIX = "template_B3CONTACT_";

        /// <summary>
        /// Control id prefix for applicant template.
        /// </summary>
        public const string CAP_APPLICANT_TEMPLATE_FIELD_PREFIX = "template_CAP_APPLICANT_";

        /// <summary>
        /// Control id prefix for contact 1 template.
        /// </summary>
        public const string CAP_CONTACT1_TEMPLATE_FIELD_PREFIX = "template_CAP_CONTACT1_";

        /// <summary>
        /// Control id prefix for contact 2 template.
        /// </summary>
        public const string CAP_CONTACT2_TEMPLATE_FIELD_PREFIX = "template_CAP_CONTACT2_";

        /// <summary>
        /// Control id prefix for contact 3 template.
        /// </summary>
        public const string CAP_CONTACT3_TEMPLATE_FIELD_PREFIX = "template_CAP_CONTACT3_";

        /// <summary>
        /// Control id prefix for reference contact template.
        /// </summary>
        public const string REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX = "template_G3CONTACT_";

        /// <summary>
        /// control id prefix for cap general search.
        /// </summary>
        public const string CAP_GENERAL_SEARCH_TEMPLATE_FIELD_FREFIX = "template_CAP_SEARCH_";

        /// <summary>
        /// control id prefix for PublicUser Register or Account Manager.
        /// </summary>
        public const string PUBLIC_USER_TEMPLATE_FIELD_FREFIX = "template_PUBLIC_USER_";

        #endregion

        /// <summary>
        /// Default page count for displayed.
        /// </summary>
        public const int DEFAULT_PAGECOUNT = 10;

        /// <summary>
        /// Default page size for displayed.
        /// </summary>
        public const int DEFAULT_PAGESIZE = 10;

        /// <summary>
        /// Additional search out record count to avoid display empty data list and "+" after clicking "..." in grid view page link. 
        /// </summary>
        public const int ADDITIONAL_RECORDS_COUNT = 1;

        /// <summary>
        /// The tail string of key about creating application in standard choices 'ACA_CONFIGS_LINKS'
        /// </summary>
        public const string MODULE_CREATION_KEY_TAIL = "Apply";

        /// <summary>
        /// he tail string of key about creating application by service in standard choices 'ACA_CONFIGS_LINKS'
        /// </summary>
        public const string MODULE_CREATION_BY_SERVICE_KEY_TAIL = "ApplyByService";

        /// <summary>
        /// The tail string of key about obtaining fee estimate in standard choices 'ACA_CONFIGS_LINKS'
        /// </summary>
        public const string MODULE_FEE_ESTIMATE_KEY_TAIL = "ObtainFeeEstimate";

        /// <summary>
        /// The tail string of key about scheduling in standard choices 'ACA_CONFIGS_LINKS'
        /// </summary>
        public const string MODULE_SCHEDULE_KEY_TAIL = "ScheduleAnInspection";

        /// <summary>
        /// The People
        /// </summary>
        public const string PEOPLE = "People";

        /// <summary>
        /// Public User
        /// </summary>
        public const string PUBLIC_USER = "Public User";

        /// <summary>
        /// examination status pending
        /// </summary>
        public const string EXAMINATION_STATUS_PENDING = "PENDING";

        /// <summary>
        /// examination status schedule
        /// </summary>
        public const string EXAMINATION_STATUS_SCHEDULE = "SCHEDULED";

        /// <summary>
        /// examination status completed for pending
        /// </summary>
        public const string EXAMINATION_STATUS_COMPLETED_PENDING = "PCOMPLETED";

        /// <summary>
        /// examination status completed for schedule
        /// </summary>
        public const string EXAMINATION_STATUS_COMPLETED_SCHEDULE = "SCOMPLETED";

        /// <summary>
        /// examination status completed for v360.
        /// </summary>
        public const string EXAMINATION_STATUS_COMPLETED = "COMPLETED";

        /// <summary>
        /// The EXAMINATION STATUS READY To SCHEDULE UNPAID
        /// </summary>
        public const string EXAMINATION_STATUS_READY_TO_SCHEDULE_UNPAID = "READYTOSCHEDULEUNPAID";

        /// <summary>
        /// The EXAMINATION STATUS READY To SCHEDULE PAID
        /// </summary>
        public const string EXAMINATION_STATUS_READY_TO_SCHEDULE_PAID = "READYTOSCHEDULEPAID";

        /// <summary>
        /// Label for element type
        /// </summary>
        public const string ELEMENT_TYPE_LABEL = "Label";

        /// <summary>
        /// The inspection result category for grade
        /// </summary>
        public const string INSPECTION_RESULT_CATEGORY_GRADE = "GRADE";

        /// <summary>
        /// The inspection result category for result
        /// </summary>
        public const string INSPECTION_RESULT_CATEGORY_RESULT = "RESULT";

        /// <summary>
        /// schedule operation type
        /// </summary>
        public const string EXAMINATION_SCHEDULE_TYPE_SCHEDULE = "schedule";

        /// <summary>
        /// reschedule operation type
        /// </summary>
        public const string EXAMINATION_SCHEDULE_TYPE_RESCHEDULE = "reschedule";

        /// <summary>
        /// The template genus level type, it indicates the type that show template fields in grid view.
        /// </summary>
        public const string TEMPLATE_GENUS_LEVEL_TYPE = "ListTemplate";

        /// <summary>
        /// Max records count by each request for the grid view export
        /// </summary>
        public const int GRIDVIEW_FULL_DOWNLOAD_EACH_NUM = 1000;

        /// <summary>
        /// The comments pattern of social media.
        /// </summary>
        public const string ACA_SOCIALMEDIA_LABEL_COMMENTS_PATTERN = "aca_socialmedia_label_comments_pattern";

        /// <summary>
        /// country jason title
        /// </summary>
        public const string ACA_COUNTRY_AUTOFILL_FLAG = "$$CountryAutoFill$$";

        /// <summary>
        /// A key for reference contact, just to make difference between Daily side expression and reference expression.
        /// No special meaning about the key value, just used to combine the Expression Argument Model.
        /// Daily side expression use CapId1+CapId2+CapId3 as the key.
        /// </summary>
        public const string EXPRESSION_COMBINE_KEY_REFERENCE_CONTACT = "ReferenceContact";

        /// <summary>
        /// A key for reference contact address, just to make difference between Daily side expression and reference expression.
        /// No special meaning about the key value, just used to combine the Expression Argument Model.
        /// Daily side expression use CapId1+CapId2+CapId3 as the key.
        /// </summary>
        public const string EXPRESSION_COMBINE_KEY_REFERENCE_CONTACT_ADDRESS = "ReferenceContactAddress";

        /// <summary>
        /// The Authorized Agent expression key,  No special meaning about the key value,
        /// Just used to combine the Expression Argument Model.
        /// </summary>
        public const string EXPRESSION_COMBINE_KEY_AUTHAGENT = "AUTHAGENT";

        /// <summary>
        /// The Authorized Agent Address expression key, No special meaning about the key value,
        /// Just used to combine the Expression Argument Model.
        /// </summary>
        public const string EXPRESSION_COMBINE_KEY_AUTHAGENT_ADDRESS = "AUTHAGENTADDRESS";

        /// <summary>
        /// One of the account type, represent the authorized agent.
        /// </summary>
        public const string PUBLICUSER_TYPE_AUTH_AGENT = "AUTH_AGENT";

        /// <summary>
        /// One of the account type, represent the authorized agent clerk.
        /// </summary>
        public const string PUBLICUSER_TYPE_AUTH_AGENT_CLERK = "AUTH_AGENT_CLERK";

        /// <summary>
        /// One of the account type, represent the self-certified inspector
        /// </summary>
        public const string PUBLICUSER_TYPE_SELF_CERTIFIED_INSPECTOR = "SELF_CERTIFIED_INSPECTOR";

        /// <summary>
        /// One of the account type, represent the contract inspector
        /// </summary>
        public const string PUBLICUSER_TYPE_CONTRACT_INSPECTOR = "CONTRACT_INSPECTOR";

        /// <summary>
        /// The session_ authorized_ agent_ reprint_ limit
        /// </summary>
        public const string SESSION_AUTHORIZED_AGENT_REPRINT_LIMIT = "ReprintLimit";

        /// <summary>
        /// The Authorized Agent payment result, win Lottery
        /// </summary>
        public const string AUTHORIZED_AGENT_PAYMENT_RESULT_WIN_LOTTERY = "10001::";

        /// <summary>
        /// The Authorized Agent payment result, Lose Lottery 
        /// </summary>
        public const string AUTHORIZED_AGENT_PAYMENT_RESULT_LOSE_LOTTERY = "10002::";

        /// <summary>
        /// Enable Expand.
        /// </summary>
        public const string ENABLE_EXPAND = "EnableExpand";

        /// <summary>
        /// Tree Node : Host agency setting node elementID for admin
        /// </summary>
        public const string ADMIN_TREENODE_HOSTEDAGENCYSETTINGS = "1144";

        /// <summary>
        /// The ref contact education entity type.
        /// </summary>
        public const string REF_CONTACT_EDUCATION_ENTITY_TYPE = "EDU";

        /// <summary>
        /// The ref contact examination entity type.
        /// </summary>
        public const string REF_CONTACT_EXAMINATION_ENTITY_TYPE = "EXAM";

        /// <summary>
        /// The ref contact continuing education entity type.
        /// </summary>
        public const string REF_CONTACT_CONT_EDUCATION_ENTITY_TYPE = "CONTEDU";

        /// <summary>
        /// The cap education entity type.
        /// </summary>
        public const string CAP_EDUCATION_ENTITY_TYPE = "CAP_EDU";

        /// <summary>
        /// The cap examination entity type.
        /// </summary>
        public const string CAP_EXAMINATION_ENTITY_TYPE = "CAP_EXAM";

        /// <summary>
        /// The cap continuing education entity type.
        /// </summary>
        public const string CAP_CONT_EDUCATION_ENTITY_TYPE = "CAP_CONTEDU";

        /// <summary>
        /// Represent the spear form side.
        /// </summary>
        public const string SPEAR_FORM = "SPEAR";

        /// <summary>
        /// cap home page
        /// </summary>
        public const string CAP_HOME_PAGE = "CAP_HOME_PAGE";

        /// <summary>
        /// Control Value Validation Function
        /// </summary>
        public const string CONTROL_VALUE_VALIDATION_FUNCTION = "AACA_CheckControlValueValidate";

        /// <summary>
        /// Template Control Value Validation Function
        /// </summary>
        public const string TEMPLETE_CONTROL_VALUE_VALIDATION_FUNCTION = "ATemplete_ACA_CheckControlValueValidate";

        /// <summary>
        /// readonly CSS name
        /// </summary>
        public const string CSS_CLASS_READONLY = "ACA_ReadOnly";

        /// <summary>
        /// Pop Action style for icon
        /// </summary>
        public const string POP_ACTION_ICO = "ICO";

        /// <summary>
        /// Logo Type category for new UI
        /// </summary>
        public const string LOGO_TYPE_CATEGORY_FOR_NEWUI = "ACA_NewUI_LOGO";

        /// <summary>
        /// split char
        /// </summary>
        public const char SPLIT_CHAR18 = (char)18;

        /// <summary>
        /// The citizen account type.
        /// </summary>
        public const string CITIZEN_ACCOUNT_TYPE = "CITIZEN";

        /// <summary>
        /// The islamic calendar
        /// </summary>
        public const string ISLAMIC_CALENDAR = "Islamic";

        /// <summary>
        /// The client state
        /// </summary>
        public const string CLIENT_STATE = "_Client_State";

        /// <summary>
        /// The SSO account type for RealMe.
        /// </summary>
        public const string SSO_ACCOUNT_TYPE_REALME = "RealMe";

        /// <summary>
        /// datetime format 
        /// </summary>
        internal const string DATATIME_FORMAT = "MM/dd/yyyy HH:mm:ss";

        /// <summary>
        /// date format string.
        /// </summary>
        internal const string DATE_FORMAT = "MM/dd/yyyy";

        /// <summary>
        /// the split char of the user preferred culture info
        /// </summary>
        private static string _cultureInfoSplitChar = string.Empty;

        #endregion Fields

        #region enum

        /// <summary>
        /// ASI Type
        /// </summary>
        public enum ASIType
        {
            /// <summary>
            /// The ASI
            /// </summary>
            ASI,

            /// <summary>
            /// The ASIT
            /// </summary>
            ASITable
        }

        /// <summary>
        /// Payment Associated Type for Trust Account.
        /// </summary>
        public enum PaymentAssociatedType
        {
            /// <summary>
            /// Associated Record Trust Account.
            /// </summary>
            Record = 0,

            /// <summary>
            /// Associated Licenses' Trust Account.
            /// </summary>
            Licenses = 1,

            /// <summary>
            /// Associated Contacts' Trust Account.
            /// </summary>
            Contacts = 2,

            /// <summary>
            /// Associated Addresses' Trust Account.
            /// </summary>
            Addresses = 3,

            /// <summary>
            /// Associated Parcels' Trust Account.
            /// </summary>
            Parcels = 4
        }

        /// <summary>
        /// 0: Record Payment;1: Trust Account Deposit .....
        /// </summary>
        public enum PaymentEntityType
        {
            /// <summary>
            /// Payment Entity - Record.
            /// </summary>
            CAP = 0,

            /// <summary>
            /// Payment Entity - Trust Account.
            /// </summary>
            TrustAccount = 1
        }

        /// <summary>
        /// 0: Permit;1: TrustAccount; 2: PROCESSING_FEE
        /// </summary>
        public enum FeeType
        {
            /// <summary>
            /// Fee Type - Permit for Record.
            /// </summary>
            Permit = 0,

            /// <summary>
            /// Fee Type - Trust Account.
            /// </summary>
            TrustAccount = 1,

            /// <summary>
            /// Fee Type - Processing fee for convenience fee.
            /// </summary>
            PROCESSING_FEE = 3
        }

        /// <summary>
        /// Contact section position
        /// </summary>
        public enum ContactSectionPosition
        {
            /// <summary>
            /// No position of contact
            /// </summary>
            None = 0,

            /// <summary>
            /// Add reference contact
            /// </summary>
            AddReferenceContact = 1,

            /// <summary>
            /// Modify reference contact
            /// </summary>
            ModifyReferenceContact = 2,

            /// <summary>
            /// Register account
            /// </summary>
            RegisterAccount = 3,

            /// <summary>
            /// Register account confirm
            /// </summary>
            RegisterAccountConfirm = 4,

            /// <summary>
            /// Register account complete
            /// </summary>
            RegisterAccountComplete = 5,

            /// <summary>
            /// Spear form
            /// </summary>
            SpearForm = 6,

            /// <summary>
            /// Validated contact address
            /// </summary>
            ValidatedContactAddress = 7,

            /// <summary>
            /// Register clerk page
            /// </summary>
            RegisterClerk = 8,

            /// <summary>
            /// If use the close match function, goto the RegisterAccountConfirm page.
            /// </summary>
            RegisterClerkConfirm = 9,

            /// <summary>
            /// the RegisterConfirm page, the last review page.
            /// </summary>
            RegisterClerkComplete = 10,

            /// <summary>
            /// Edit clerk page
            /// </summary>
            EditClerk = 11,

            /// <summary>
            /// customer detail page
            /// </summary>
            AuthAgentCustomerDetail = 12,

            /// <summary>
            /// Register existing account from another agency
            /// </summary>
            RegisterExistingAccount = 13,

            /// <summary>
            /// Spear from close match confirm page.
            /// </summary>
            SpearFormCloseMatchConfirm = 14
        }

        /// <summary>
        /// Define the associated form type
        /// </summary>
        public enum AssoFormType
        {
            /// <summary>
            /// It is not an associated form.
            /// </summary>
            NotAssoForm,

            /// <summary>
            /// Normal Associated Form: 
            /// 1. in normal agency, enable associated form.
            /// 2. in super agency, enable associated form and select single service to create.(IsSubAgency)
            /// </summary>
            Normal,

            /// <summary>
            /// In super agency, enable associated form and select multiply services to create.
            /// </summary>
            SuperAgency
        }

        /// <summary>
        /// The authorized agent's section position.
        /// </summary>
        public enum AuthAgentCustomerSectionPosition
        {
            /// <summary>
            /// The None.
            /// </summary>
            None,

            /// <summary>
            /// The detail form.
            /// </summary>
            DetailForm,

            /// <summary>
            /// The search form.
            /// </summary>
            SearchForm
        }

        /// <summary>
        /// Auto fill type in spear form.
        /// </summary>
        public enum AutoFillType4SpearForm
        {
            /// <summary>
            /// Not set auto-fill type
            /// </summary>
            None = 0,

            /// <summary>
            /// Auto-fill type for Address
            /// </summary>
            Address,

            /// <summary>
            /// Auto-fill type for Parcel
            /// </summary>
            Parcel,

            /// <summary>
            /// Auto-fill type for Owner
            /// </summary>
            Owner,

            /// <summary>
            /// Auto-fill type for License
            /// </summary>
            License,

            /// <summary>
            /// Auto-fill type for Contact
            /// </summary>
            Contact,

            /// <summary>
            /// Auto-fill type for Owner in ContactEdit
            /// </summary>
            ContactOwner
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets agency code from web config.
        /// </summary>
        public static string AgencyCode
        {
            get
            {
                return ConfigurationManager.AppSettings[ServProvCode_Key];
            }
        }

        /// <summary>
        /// Gets the split char of the user preferred culture info.
        /// </summary>
        public static string CultureInfoSplitChar
        {
            get
            {
                if (string.IsNullOrEmpty(_cultureInfoSplitChar))
                {
                    _cultureInfoSplitChar = I18nCultureUtil.GetLanguageCode(I18nCultureUtil.UserPreferredCultureInfo) == "ar" ? ";" : ",";
                }

                return _cultureInfoSplitChar;
            }
        }

        /// <summary>
        /// Gets forgot password page for confirm password
        /// </summary>
        public static string PageForgotPasswordConfirmUrl
        {
            get
            {
                return string.Format("../Account/ForgotPassword.aspx?{0}={1}", UrlConstant.FORGOTPASSWORD_PAGE_TYPE, UrlConstant.FORGOTPASSWORD_PAGE_CONFIRMPASSWORD);
            }
        }

        /// <summary>
        /// Gets forgot password page for security question
        /// </summary>
        public static string PageForgotPasswordSecurityQuestionUrl
        {
            get
            {
                return string.Format("../Account/ForgotPassword.aspx?{0}={1}", UrlConstant.FORGOTPASSWORD_PAGE_TYPE, UrlConstant.FORGOTPASSWORD_PAGE_SECURITYQUESTION);
            }
        }

        #endregion Properties
        
        #region Methods

        /// <summary>
        /// Gets ACA Server Constants.
        /// </summary>
        /// <returns>enable APP name</returns>
        public static string[] GetACAServerConstants()
        {
            return new string[] { ENABLE_APP_NAME_CONSTANT };
        }

        /// <summary>
        /// Gets the pages that been repeated used
        /// </summary>
        /// <returns>repeated use pages</returns>
        public static string[] GetRepeatedUsePages()
        {
            return new string[] { PAGE_ACCOUNT_VERIFICATION_EXPIRED, PageForgotPasswordConfirmUrl, PageForgotPasswordSecurityQuestionUrl, PAGE_REGISTER_LICENSE_VIEW, PAGE_WELCOME_REGISTERED };
        }

        #endregion Methods

        #region Structs

        /// <summary>
        /// Permission types of ASI/ASIT security.
        /// </summary>
        public struct ASISecurity
        {
            /// <summary>
            /// Full access.
            /// </summary>
            public const string Full = "F";

            /// <summary>
            /// Read only.
            /// </summary>
            public const string Read = "R";

            /// <summary>
            /// None permission.
            /// </summary>
            public const string None = "N";
        }

        /// <summary>
        /// Permission types of security.
        /// </summary>
        public struct Security
        {
            /// <summary>
            /// Full access.
            /// </summary>
            public const string Full = "F";

            /// <summary>
            /// Read only.
            /// </summary>
            public const string Read = "R";

            /// <summary>
            /// None permission.
            /// </summary>
            public const string None = "N";
        }

        /// <summary>
        /// asset template field type
        /// </summary>
        public struct AssetTemplateFieldType
        {
            /// <summary>
            /// The field type for date
            /// </summary>
            public const string Date = "Date";

            /// <summary>
            /// The field type for time
            /// </summary>
            public const string Time = "Time";

            /// <summary>
            /// The field type for number
            /// </summary>
            public const string Number = "Number";

            /// <summary>
            /// The field type for radio
            /// </summary>
            public const string Radio = "Y/N";

            /// <summary>
            /// The field type for text area
            /// </summary>
            public const string TextArea = "Textarea";

            /// <summary>
            /// The field type for text
            /// </summary>
            public const string Text = "Text";
        }

        #endregion
    }
}
