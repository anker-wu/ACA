#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminCapBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AdminCapBll.cs 278182 2014-08-28 10:13:16Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class provide the operate CAP in admin.
    /// </summary>
    public class AdminCapBll : BaseBll, ICapBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of AppStatusGroupService.
        /// </summary>
        /// <value>The application status group service.</value>
        private AppStatusGroupWebServiceService AppStatusGroupService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AppStatusGroupWebServiceService>();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a regular CAP based on the Partial Cap
        /// </summary>
        /// <param name="spCode">Agency Code</param>
        /// <param name="capModel4WS">Partial Cap</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order sequence</param>
        /// <returns>Cap information</returns>
        public CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Creates a regular CAP based on the Partial Cap
        /// </summary>
        /// <param name="spCode">Agency Code</param>
        /// <param name="capModel4WS">Partial Cap</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order sequence</param>
        /// <param name="isForDeferPayment">For Defer Payment</param>
        /// <returns>Cap information</returns>
        public CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isForDeferPayment)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Creates a regular CAP based on the Partial Cap
        /// </summary>
        /// <param name="spCode">Agency Code</param>
        /// <param name="capModel4WS">Partial Cap</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order template</param>
        /// <param name="isForDeferPayment">For Defer Payment</param>
        /// <param name="isAmendment">is amendment</param>
        /// <returns>Cap information</returns>
        public CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isForDeferPayment, bool isAmendment)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Create Cap Models From FeeEstimate.
        /// </summary>
        /// <param name="capModel4WS">cap model.</param>
        /// <returns>Online Payment Result Model.</returns>
        public OnlinePaymentResultModel CreateCapModelsFromFeeEstimate(CapModel4WS capModel4WS)
        {
            return new OnlinePaymentResultModel();
        }

        /// <summary>
        /// Create or get renewal partial Cap.
        /// </summary>
        /// <param name="parentCapID">parent Cap id.</param>
        /// <returns>Cap model for ACA.</returns>
        public CapModel4WS CreateOrGetRenewalPartialCap(CapIDModel4WS parentCapID)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Create partial Cap.
        /// </summary>
        /// <param name="capModel4WS">cap model content.</param>
        /// <param name="services">service model.</param>
        /// <param name="isFeeEstimates">is feeEstimates.</param>
        /// <returns>Cap model for ACA.</returns>
        public CapModel4WS CreatePartialCaps(CapModel4WS capModel4WS, ServiceModel[] services, bool isFeeEstimates)
        {
            return new CapModel4WS();
        }

        /// <summary>
        /// Method to crate a partial cap.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order ID</param>
        /// <param name="isFeeEstimates">whether fee estimate</param>
        /// <returns>Cap information</returns>
        public CapModel4WS CreateWrapperForPartialCap(string serviceProviderCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isFeeEstimates)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Method to get application status model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <returns>application status array</returns>
        /// <exception cref="DataValidateException">agency code is null</exception>
        /// <exception cref="ACAException">Exception from web service</exception>
        public AppStatusGroupModel4WS[] GetAppStatusGroupBySPC(string agencyCode, string moduleName)
        {
            if (string.IsNullOrEmpty(agencyCode))
            {
                throw new DataValidateException(new[] { agencyCode });
            }

            try
            {
                return AppStatusGroupService.getAppStatusGroupBySPC(agencyCode, moduleName);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get cap by AltID.
        /// </summary>
        /// <param name="capIDModule">Cap id model for ACA.</param>
        /// <returns>Cap model for ACA</returns>
        public CapModel4WS GetCapByAltID(CapIDModel4WS capIDModule)
        {
            return new CapModel4WS();
        }

        /// <summary>
        /// get cap model(cap simple information - only b1permit data) by cap ID. if the cap doesn't exist, return null.
        /// </summary>
        /// <param name="capID4WS">capID Model identify a cap..</param>
        /// <returns>cap information</returns>
        public CapModel4WS GetCapByPK(CapIDModel4WS capID4WS)
        {
            return new CapModel4WS();
        }

        /// <summary>
        /// Get Renew Cap's Child Cap..
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
        /// <param name="relationship">Renewal (ACAConstant.CAP_RENEWAL)</param>
        /// <param name="capStatus">Incomplete (ACAConstant.RENEWAL_INCOMPLETE)</param>
        /// <returns>Renew Child Cap.</returns>
        public CapModel4WS GetCapByRelationshipPartialCap(CapIDModel4WS capID4ws, string relationship, string capStatus)
        {
            return new CapModel4WS();
        }

        /// <summary>
        /// Get CAPID Model by AltID.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="altID">CAP's Alt ID.</param>
        /// <returns>CapIDModel4WS model.</returns>
        public CapIDModel4WS GetCapIDByAltID(string agencyCode, string altID)
        {
            return new CapIDModel4WS();
        }

        /// <summary>
        /// Gets cap List for ACA, it contains Cap search condition.
        /// </summary>
        /// <param name="capModel4WS">cap model object.</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="userSeqNum">public user sequence number</param>
        /// <param name="capInfo">CAP Info: COMPLETE, INCOMPLETE</param>
        /// <param name="isOnlyMyCAP">is only my CAP</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>array of cap information</returns>
        public SearchResultModel GetCapList4ACA(CapModel4WS capModel4WS, QueryFormat queryFormat, string userSeqNum, string capInfo, bool isOnlyMyCAP, List<string> moduleNames, bool isSearchAllStartRow, string[] viewElementNames)
        {
            return new SearchResultModel();
        }

        /// <summary>
        /// Get cap view by AltID
        /// </summary>
        /// <param name="capIDModel4WS">Cap id model for ACA.</param>
        /// <returns>Cap model for ACA</returns>
        public CapModel4WS GetCapViewByAltID(CapIDModel4WS capIDModel4WS)
        {
            return new CapModel4WS();
        }

        /// <summary>
        /// Gets cap view by single.
        /// </summary>
        /// <param name="capID4ws">Cap id model for ACA.</param>
        /// <param name="userSeqNbr">The user sequence number.</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <param name="isSuperAgency">is Super Agency.</param>
        /// <returns>CapWithCondition Model</returns>
        public CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID4ws, string userSeqNbr, string isConditionOfApproval, bool isSuperAgency)
        {
            return GetCapViewBySingle(capID4ws, userSeqNbr, false, isConditionOfApproval, isSuperAgency, false);
        }

        /// <summary>
        /// Get cap view by single cap.
        /// </summary>
        /// <param name="capID4ws">Cap id model for ACA.</param>
        /// <param name="userSeqNbr">The user sequence number.</param>
        /// <param name="isForView">Is for view.</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <param name="isSuperAgency">is Super Agency.</param>
        /// <returns>CapWithCondition Model</returns>
        public CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID4ws, string userSeqNbr, bool isForView, string isConditionOfApproval, bool isSuperAgency)
        {
            return GetCapViewBySingle(capID4ws, userSeqNbr, isConditionOfApproval, isSuperAgency);
        }

        /// <summary>
        /// Gets cap view by single set.
        /// Gets cap contains CapDetailModel, CapWorkDesModel, AddressModel, CapContactModel, ContactsGroup,
        /// ParcelModel, OwnerModel, LicenseProfessionalModel, <c>BValuatnModel</c>.
        /// </summary>
        /// <param name="capID">cap ID model</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <param name="isForView">is For View</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <param name="isSuperAgency">
        /// There are difference logic between super agency and normal agency during get the cap data.
        /// In super agency, we need to go through the normal agency logic in sometimes. So expose the <see cref="isSuperAgency"/> parameter.
        /// Such as: User select single service to create cap, will follow the normal agency's logic.
        /// </param>
        /// <param name="isForRenew">if it is true, get the child record for renewal.</param>
        /// <returns>Cap information With Condition</returns>
        public CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID, string userSeqNbr, bool isForView, string isConditionOfApproval, bool isSuperAgency, bool isForRenew)
        {
            CapWithConditionModel4WS model = new CapWithConditionModel4WS();
            model.capModel = (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];

            return model;
        }

        /// <summary>
        /// Gets Related Cap view Detail.
        /// </summary>
        /// <param name="capID4ws">Cap id model for ACA.</param>
        /// <param name="userSeqNbr">The user sequence number.</param>
        /// <returns>Cap model for ACA</returns>
        public CapModel4WS GetCapViewDetailByPK(CapIDModel4WS capID4ws, string userSeqNbr)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Gets the condition model by cap type.
        /// </summary>
        /// <param name="capType">The cap type model.</param>
        /// <param name="includeInactivity">Indicates whether include inactivity status or not.</param>
        /// <returns>return the NoticeConditionModel list.</returns>
        public NoticeConditionModel[] GetStdConditionByRecordType(CapTypeModel capType, bool includeInactivity)
        {
            return new NoticeConditionModel[0];
        }

        /// <summary>
        /// Get the child agencies from cap id models.
        /// </summary>
        /// <param name="paymentResultModels">payment result models</param>
        /// <returns>dataTable for agencies.</returns>
        public DataTable GetChildAgencies(PaymentResultModel4WS[] paymentResultModels)
        {
            return new DataTable();
        }

        /// <summary>
        /// Get child cap id by master cap id.
        /// </summary>
        /// <param name="capID4WS">master cap id</param>
        /// <param name="relationship">cap relationship, e.g.: renew, pay fee due...</param>
        /// <param name="status">the cap status, e.g.: incomplete, pending...</param>
        /// <param name="isAcrossAgencies">true:In ACA super agency get child cap. otherwise is false.</param>
        /// <returns>Cap id model for ACA.</returns>
        public CapIDModel4WS GetChildCapIDByMasterID(CapIDModel4WS capID4WS, string relationship, string status, bool isAcrossAgencies)
        {
            return new CapIDModel4WS();
        }

        /// <summary>
        /// Get child cap id models by parent cap model
        /// </summary>
        /// <param name="parentCapModel">CapIDModel4WS entity</param>
        /// <returns>Array of CapIDModel4WS</returns>
        public CapIDModel4WS[] GetChildCaps(CapIDModel4WS parentCapModel)
        {
            return null;
        }

        /// <summary>
        /// get the child agency's cap id table.
        /// </summary>
        /// <param name="paymentResults">payment results</param>
        /// <param name="childAgency">child agency.</param>
        /// <returns>data table for agency.</returns>
        public DataTable GetChildCapsByAgency(PaymentResultModel4WS[] paymentResults, string childAgency)
        {
            return new DataTable();
        }

        /// <summary>
        /// Get the related CAP ids which are not included in the incoming <see cref="capIds" /> based on the CAP <see cref="relationship" />.
        /// </summary>
        /// <param name="capIds">Cap Id list</param>
        /// <param name="relationship">It may be "R", "EST", "Renewal", "Amendment" or "Ass Form".</param>
        /// <returns>missed Cap Id list</returns>
        public IList<CapIDModel> GetMissingRecordIDs4ShoppingCart(CapIDModel[] capIds, string relationship)
        {
            return new List<CapIDModel>();
        }

        /// <summary>
        /// Get ModuleName array by capId list.
        /// </summary>
        /// <param name="capIdModels">CapID Model array</param>
        /// <returns>return string array</returns>
        public StringArray[] GetModuleNamesByCapIDList(CapIDModel[] capIdModels)
        {
            return null;
        }

        /// <summary>
        /// Gets the my cap List for ACA.
        /// 1) Get the license professional list base on registered user.
        /// 2) Loop the license professional to get the caps
        /// 3) Combine the caps into the original my permit list.
        /// 4) add the CAPs that were created by him
        /// </summary>
        /// <param name="serviceProviderCode">The agency code.</param>
        /// <param name="capModel4WS">Cap model for ACA.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <param name="userSeqNum">user sequence number.</param>
        /// <param name="capInfo">Cap information.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>Simple cap model array for ACA</returns>
        public SearchResultModel GetMyCapList4ACA(string serviceProviderCode, CapModel4WS capModel4WS, QueryFormat queryFormat, string userSeqNum, string capInfo, string[] viewElementNames)
        {
            return new SearchResultModel();
        }

        /// <summary>
        /// Get parent parent cap id by related child cap ID, relationship and status.
        /// </summary>
        /// <param name="childCapID">related child cap ID for web service</param>
        /// <param name="relationship">It may be "R", "EST" and "Renewal" or "Amendment".</param>
        /// <param name="status">1. It is for renewal CAP, it may be "Incomplete", "Review" or "Complete".
        /// 2. It is for amendment CAP, it may be  "Incomplete" or "Complete".</param>
        /// <returns>CapID Model</returns>
        public CapIDModel GetParentCapIDByChildCapID(CapIDModel childCapID, string relationship, string status)
        {
            return new CapIDModel();
        }

        /// <summary>
        /// Get parent parent cap id list by related child cap ID, relationship and status.
        /// </summary>
        /// <param name="childCapID">related child cap ID for web service</param>
        /// <param name="relationship">It may be "R", "Renewal", "Amendment" or "Ass Form", except "EST".</param>
        /// <param name="status">1. It is for renewal CAP, it may be "Incomplete", "Review" or "Complete".
        /// 2. It is for amendment CAP, it may be  "Incomplete" or "Complete".
        /// 3. It is for Ass Form CAP, it may be null.</param>
        /// <returns>CapIDModel object array</returns>
        CapIDModel[] ICapBll.GetParentCapIDListByChildCapID(CapIDModel childCapID, string relationship, string status)
        {
            return null;
        }

        /// <summary>
        /// Get permits by address.
        /// </summary>
        /// <param name="address">address info.</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="callerId">the caller id.</param>
        /// <param name="isOnlyMyCAP">Is only my Cap.</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="queryFormat">the query format.</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>Simple cap model array for ACA</returns>
        public SearchResultModel GetPermitsByAddress(AddressModel address, string agencyCode, string moduleName, string callerId, bool isOnlyMyCAP, List<string> moduleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames)
        {
            return new SearchResultModel();
        }

        /// <summary>
        /// Get permit list data by contact.
        /// </summary>
        /// <param name="capContactModel">cap contact model.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="isOnlyMyCAP">is only my permit</param>
        /// <param name="moudleNames">search cross modules name.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>Empty simple cap model array for ACA</returns>
        public SearchResultModel GetPermitsByContact(CapContactModel4WS capContactModel, string moduleName, bool isOnlyMyCAP, List<string> moudleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames)
        {
            return new SearchResultModel();
        }

        /// <summary>
        /// Get permit list data by license id.
        /// </summary>
        /// <param name="licenseType">the license type.</param>
        /// <param name="licenseStateNumber">the license state number.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <param name="isOnlyMyCAP">is only my permit</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>the permit list array. 
        /// It contains 5 column:
        /// Date
        /// PermitNumber
        /// PermitType
        /// Description
        /// Status
        /// </returns>
        public SearchResultModel GetPermitsByLicenseId(string licenseType, string licenseStateNumber, string agencyCode, string moduleName, string callerId, bool isOnlyMyCAP, List<string> moduleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames)
        {
            return new SearchResultModel();
        }

        /// <summary>
        /// Gets Related Cap List.
        /// </summary>
        /// <param name="rootCAPID4WS">root CAP ID</param>
        /// <param name="searchConstions4WS">cap model containing search conditions</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>Array of CapModel4WS</returns>
        public CapModel4WS[] GetRelatedCapList(CapIDModel4WS rootCAPID4WS, CapModel4WS searchConstions4WS, QueryFormat queryFormat)
        {
            return new CapModel4WS[0];
        }

        /// <summary>
        /// Gets Related Cap Tree.
        /// </summary>
        /// <param name="rootCAPID4WS">root CAP ID</param>
        /// <param name="searchConstions4WS">cap model containing search conditions</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>Array of CapModel4WS</returns>
        public ProjectTreeNodeModel4WS GetRelatedCapTree(CapIDModel4WS rootCAPID4WS, CapModel4WS searchConstions4WS, QueryFormat queryFormat)
        {
            return new ProjectTreeNodeModel4WS();
        }

        /// <summary>
        /// get licensee's related caps. and add the user role into them.
        /// </summary>
        /// <param name="licensee">License Model 4WS</param>
        /// <param name="moduleNames">active module names</param>
        /// <param name="queryFormat">the query format</param>
        /// <returns>a data table for cap list</returns>
        public DataTable GetRelatedCapsByRefLP(LicenseModel4WS licensee, List<string> moduleNames, QueryFormat queryFormat)
        {
            return new DataTable();
        }

        /// <summary>
        /// Get review child renewal cap ID by renewal parent Cap ID.
        /// </summary>
        /// <param name="capID4WS">Cap ID Model for web service</param>
        /// <returns>CapIDModel4WS object</returns>
        public CapIDModel4WS GetReviewChildCapIDByMasterID(CapIDModel4WS capID4WS)
        {
            return new CapIDModel4WS();
        }

        /// <summary>
        /// Get a related records tree for current record ID.
        /// This tree only include the parent layer and the child layer of the current record ID(<see cref="currentRecordId" />).
        /// </summary>
        /// <param name="currentRecordId">Current record ID.</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>Tree node model</returns>
        public ProjectTreeNodeModel4WS GetSectionalRelatedCapTree(CapIDModel currentRecordId, QueryFormat queryFormat)
        {
            return new ProjectTreeNodeModel4WS();
        }

        /// <summary>
        /// Get Services By Record ID
        /// </summary>
        /// <param name="capIDModel4WS">the capIDModel4WS model</param>
        /// <returns>the ServiceModel list.</returns>
        public ServiceModel[] GetServicesByRecordID(CapIDModel4WS capIDModel4WS)
        {
            return new ServiceModel[0];
        }

        /// <summary>
        /// Gets trade name cap model.
        /// </summary>
        /// <param name="capIDModel">Cap id model.</param>
        /// <returns>Cap model for ACA</returns>
        public CapModel4WS GetTradeNameCapModel(CapIDModel4WS capIDModel)
        {
            return new CapModel4WS();
        }

        /// <summary>
        /// Clone record
        /// </summary>
        /// <param name="srcCapID">source cap id</param>
        /// <param name="sectionNamesList">clone sections' name</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>cap id models</returns>
        CapIDModel[] ICapBll.CloneRecords(CapIDModel srcCapID, string[] sectionNamesList, string callerID)
        {
            return new CapIDModel[0];
        }

        /// <summary>
        /// get Cap PaymentResultWithAddress model list with CapPaymentResultModel list
        /// </summary>
        /// <param name="capPaymentResultModels">CapPaymentResult model</param>
        /// <returns>Cap PaymentResultWithAddress model list</returns>
        CapPaymentResultWithAddressModel[] ICapBll.ConstructCapPaymentResultWithAddressModel(CapPaymentResultModel[] capPaymentResultModels)
        {
            return new CapPaymentResultWithAddressModel[0];
        }

        /// <summary>
        /// Get child caps by mater cap id.
        /// </summary>
        /// <param name="parentCapID4WS">The master cap id</param>
        /// <param name="relationship">The cap relationship</param>
        /// <param name="status">The cap status</param>
        /// <returns>The cap detail</returns>
        CapModel4WS[] ICapBll.GetChildCapDetailsByMasterID(CapIDModel4WS parentCapID4WS, string relationship, string status)
        {
            return null;
        }

        /// <summary>
        /// Is valid config for renewal.
        /// </summary>
        /// <param name="capID4WS">Cap id model for ACA</param>
        /// <returns>true: valid; false: invalid</returns>
        bool ICapBll.IsValidConfig4Renewal(CapIDModel4WS capID4WS)
        {
            return false;
        }

        /// <summary>
        /// Generate a new alt id for fee estimate partial cap or partial cap, update cap alt ID and return it.
        /// </summary>
        /// <param name="capModel4WS">Cap Model for ACA</param>
        /// <param name="isFeeEstCap">is fee estimate Cap</param>
        /// <param name="isTemporaryCap">is temporary Cap</param>
        /// <returns>Update cap AltID.</returns>
        string ICapBll.UpdateCapAltID(CapModel4WS capModel4WS, bool isFeeEstCap, bool isTemporaryCap)
        {
            return string.Empty;
        }

        /// <summary>
        /// Is auto issue for renewal by child CapID.
        /// </summary>
        /// <param name="childCapID4WS">Child cap id for ACA.</param>
        /// <returns>true: auto issue false: not auto issue.</returns>
        public bool IsAutoIssue4RenewalByChildCapID(CapIDModel4WS childCapID4WS)
        {
            return false;
        }

        /// <summary>
        /// Is the cap created by initial user
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <returns>true or false.</returns>
        public bool IsCreateByDelegateUser(CapModel4WS capModel)
        {
            return false;
        }

        /// <summary>
        /// Determines whether [is locked original hold] [the specified cap ID].
        /// </summary>
        /// <param name="capId">The cap ID.</param>
        /// <param name="userSeqNumber">The user sequence number.</param>
        /// <returns><c>true</c> if [is locked original hold] [the specified cap ID]; otherwise, <c>false</c>.</returns>
        public bool IsLockedOrHold(CapIDModel4WS capId, string userSeqNumber)
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="capClass" /> is partial CAP class.
        /// </summary>
        /// <param name="capClass">the CAP class</param>
        /// <returns>true or false.</returns>
        public bool IsPartialCap(string capClass)
        {
            return false;
        }

        /// <summary>
        /// Is renew able for single CapID.
        /// </summary>
        /// <param name="serviceProviderCode">The agency code.</param>
        /// <param name="capIDModel4WS">Cap id model for ACA.</param>
        /// <returns>true: able renew single CAP; false: disable renew single CAP</returns>
        public bool IsRenewableForSingleCapID(string serviceProviderCode, CapIDModel4WS capIDModel4WS)
        {
            return false;
        }

        /// <summary>
        /// Query permits Data is CapModel4WS what is apply in general search
        /// if only query my permits,user sequence should be the current user sequence number.
        /// if query all of permits, user sequence should be null.
        /// </summary>
        /// <param name="capModel">CapModel4WS as condition.</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="userSeqNum">user sequence number.</param>
        /// <param name="isOnlyMyCAP">is only my cap</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>SimpleCapModel4WS array.</returns>
        public SearchResultModel QueryPermitsGC(CapModel4WS capModel, List<string> moduleNames, string userSeqNum, bool isOnlyMyCAP, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames)
        {
            return new SearchResultModel();
        }

        /// <summary>
        /// save partial caps.
        /// </summary>
        /// <param name="capModel4WS">Cap model for ACA.</param>
        /// <returns>Cap model for ACA</returns>
        public CapModel4WS SavePartialCaps(CapModel4WS capModel4WS)
        {
            return new CapModel4WS();
        }

        /// <summary>
        /// create or update a partial cap.
        /// </summary>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <param name="wotModelSeq">work order ID</param>
        /// <param name="isFeeEstimates">whether fee estimate</param>
        /// <returns>CapModel4WS object</returns>
        public CapModel4WS SaveWrapperForPartialCap(CapModel4WS capModel4WS, string wotModelSeq, bool isFeeEstimates)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Update cap class.
        /// </summary>
        /// <param name="capModel4WS">Cap model for ACA.</param>
        /// <returns>Cap model for ACA</returns>
        public CapModel4WS UpdateCapClass(CapModel4WS capModel4WS)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Update cap class
        /// </summary>
        /// <param name="caps">the id of the cap which need to be updated</param>
        public void UpdateCapStatus(CapModel4WS[] caps)
        {
        }

        /// <summary>
        /// Update partial Cap model wrapper
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="capModel4WS">cap model.</param>
        /// <param name="publicUserId">public user id.</param>
        /// <returns>Cap model for ACA.</returns>
        public CapModel4WS UpdatePartialCapModelWrapper(string serviceProviderCode, CapModel4WS capModel4WS, string publicUserId)
        {
            return (CapModel4WS)System.Web.HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        /// Update partial caps.
        /// </summary>
        /// <param name="capModel4WS">Cap model for ACA.</param>
        /// <returns>Cap model for ACA</returns>
        public CapModel4WS UpdatePartialCaps(CapModel4WS capModel4WS)
        {
            return new CapModel4WS();
        }

        /// <summary>
        /// Remove partial cap by cap id
        /// </summary>
        /// <param name="parentCapId">parent cap Id</param>
        /// <param name="childCapId">child cap Id</param>
        /// <param name="userId">user id</param>
        public void RemoveChildPartialCap(CapIDModel4WS parentCapId, CapIDModel4WS childCapId, string userId)
        {
        }

        /// <summary>
        /// Populate the CapModel4WS list to a DataTable.
        /// </summary>
        /// <param name="capModel4WSList">a CapModel4WS list</param>
        /// <returns>The permit list data table.</returns>
        public DataTable BuildPermitDataTable(CapModel4WS[] capModel4WSList)
        {
            return new DataTable();
        }

        /// <summary>
        /// Get licenses by customer sequence number
        /// </summary>
        /// <param name="contactSeqNum">the customer sequence number</param>
        /// <param name="modueName">the module name</param>
        /// <param name="filterName">the filter name</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="callerID">the caller id</param>
        /// <returns>SimpleCap model array</returns>
        public SimpleCapModel[] GetLicensesByCustomer(string contactSeqNum, string modueName, string filterName, QueryFormat queryFormat, string callerID)
        {
            return new SimpleCapModel[0];
        }

        /// <summary>
        /// Get the cap temporary data.
        /// </summary>
        /// <param name="capTemporaryData">The CapTemporaryDataModel</param>
        /// <returns>return the CapTemporaryDataModel</returns>
        public CapTemporaryDataModel GetCapTemporaryData(CapTemporaryDataModel capTemporaryData)
        {
            // not need implement
            return null;
        }

        /// <summary>
        /// update the cap temporary data.
        /// </summary>
        /// <param name="capTemporaryData">The CapTemporaryDataModel</param>
        public void UpdateCapTemporaryData(CapTemporaryDataModel capTemporaryData)
        {
            // not need implement
        }

        /// <summary>
        /// Clear the component data in hidden page for the CapModel.
        /// </summary>
        /// <param name="capModel">The Current CapModel</param>
        /// <param name="pageFlowGroup">The PageFlow group</param>
        public void ClearCapData(CapModel4WS capModel, PageFlowGroupModel pageFlowGroup)
        {
            // not need implement
        }

        #endregion Methods
    }
}