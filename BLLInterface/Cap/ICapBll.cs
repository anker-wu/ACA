#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICapBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  ICapBll
 *
 *  Notes:
 * $Id: ICapBll.cs 278182 2014-08-28 10:13:16Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;

using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// Defines methods for CAP.
    /// </summary>
    public interface ICapBll
    {
        #region Methods

        /// <summary>
        /// Clone record
        /// </summary>
        /// <param name="srcCapID">source cap ID</param>
        /// <param name="sectionNamesList">clone sections' name</param>
        /// <param name="callerID">caller ID.</param>
        /// <returns>cap ID information list</returns>
        CapIDModel[] CloneRecords(CapIDModel srcCapID, string[] sectionNamesList, string callerID);

        /// <summary>
        /// Get CompletePaymentResultWithAddressModel model with completePaymentResultModels.
        /// </summary>
        /// <param name="capPaymentResultModels">array of capPaymentResultModels</param>
        /// <returns>Complete payment result with address model</returns>
        CapPaymentResultWithAddressModel[] ConstructCapPaymentResultWithAddressModel(CapPaymentResultModel[] capPaymentResultModels);

        /// <summary>
        /// Creates a regular CAP based on the Partial Cap 
        /// </summary>
        /// <param name="spCode">Agency Code</param>
        /// <param name="capModel4WS">Partial Cap</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order sequence</param>
        /// <returns>Cap information</returns>
        CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq);

        /// <summary>
        /// Creates a regular CAP based on the Partial Cap 
        /// </summary>
        /// <param name="spCode">Agency Code</param>
        /// <param name="capModel4WS">Partial Cap</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order sequence</param>
        /// <param name="isForDeferPayment">For Defer Payment</param>
        /// <returns>Cap information</returns>
        CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isForDeferPayment);

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
        CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isForDeferPayment, bool isAmendment);

        /// <summary>
        /// Creates regular sub Caps based on the Partial Caps 
        /// </summary>
        /// <param name="capModel4WS">The parent cap</param>
        /// <returns>Children Simple Cap  Models that has created successfully</returns>
        OnlinePaymentResultModel CreateCapModelsFromFeeEstimate(CapModel4WS capModel4WS);

        /// <summary>
        /// Method for renewable licenses and permit.
        /// First time access the method to Create renewal partial cap as child cap of parent licenses cap to renewal processing.
        /// Second time get the renewal partial cap to continue this renewal process.
        /// </summary>
        /// <param name="parentCapID">Cap ID Model</param>
        /// <returns>Cap information</returns>
        CapModel4WS CreateOrGetRenewalPartialCap(CapIDModel4WS parentCapID);

        /// <summary>
        /// Create partial caps for super agency
        /// </summary>
        /// <param name="capModel4WS">CapModel4WS entity</param>
        /// <param name="services">user selected services</param>
        /// <param name="isFeeEstimates">is fee estimates</param>
        /// <returns>The parent cap information.</returns>
        CapModel4WS CreatePartialCaps(CapModel4WS capModel4WS, ServiceModel[] services, bool isFeeEstimates);

        /// <summary>
        /// Method to crate a partial cap.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order ID</param>
        /// <param name="isFeeEstimates">whether fee estimate</param>
        /// <returns>Cap information</returns>
        CapModel4WS CreateWrapperForPartialCap(string serviceProviderCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isFeeEstimates);

        /// <summary>
        /// Method to get application status model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <returns>application status array</returns>
        AppStatusGroupModel4WS[] GetAppStatusGroupBySPC(string agencyCode, string moduleName);

        /// <summary>
        /// Get the simple Cap associate to a trade name
        /// </summary>
        /// <param name="capIDModule">CapIDModel4WS entity</param>
        /// <returns>cap information </returns>
        CapModel4WS GetCapByAltID(CapIDModel4WS capIDModule);

        /// <summary>
        /// get cap model(cap simple information - only b1permit data) by cap ID. if the cap doesn't exist, return null.
        /// </summary>
        /// <param name="capID4WS">capID Model identify a cap..</param>
        /// <returns>cap information</returns>
        CapModel4WS GetCapByPK(CapIDModel4WS capID4WS);

        /// <summary>
        /// Get Renew Cap's Child Cap.. 
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
        /// <param name="relationship">Renewal (ACAConstant.CAP_RENEWAL)</param>
        /// <param name="capStatus">Incomplete (ACAConstant.RENEWAL_INCOMPLETE)</param>
        /// <returns>Renew Child Cap.</returns>
        CapModel4WS GetCapByRelationshipPartialCap(CapIDModel4WS capID4ws, string relationship, string capStatus);

        /// <summary>
        /// Get CAPID Model by AltID.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="altID">Alt ID number.</param>
        /// <returns>cap information.</returns>
        CapIDModel4WS GetCapIDByAltID(string agencyCode, string altID);

        /// <summary>
        /// Gets cap List for ACA, it contains Cap search condition.
        /// </summary>
        /// <param name="capModel4WS">cap model object.</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="userSeqNum">public user sequence number</param>
        /// <param name="capInfo">CAP Info: COMPLETE, INCOMPLETE </param>
        /// <param name="isOnlyMyCAP">is only my CAP</param>
        /// <param name="moduleNames">search cross modules name.</param>        
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>array of cap information</returns>
        SearchResultModel GetCapList4ACA(CapModel4WS capModel4WS, QueryFormat queryFormat, string userSeqNum, string capInfo, bool isOnlyMyCAP, List<string> moduleNames, bool isSearchAllStartRow, string[] viewElementNames);

        /// <summary>
        /// Get a Cap associate to a trade name
        /// </summary>
        /// <param name="capID">Cap ID Model</param>
        /// <returns>cap information </returns>
        CapModel4WS GetCapViewByAltID(CapIDModel4WS capID);

        /// <summary>
        /// Gets cap view by single set.
        /// Gets cap contains CapDetailModel, CapWorkDesModel, AddressModel, CapContactModel, ContactsGroup,
        /// ParcelModel, OwnerModel, LicenseProfessionalModel, <c>BValuatnModel</c>. 
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <param name="isSuperAgency">
        /// There are difference logic between super agency and normal agency during get the cap data.
        /// In super agency, we need to go through the normal agency logic in sometimes. So expose the <see cref="isSuperAgency"/> parameter.
        /// Such as: User select single service to create cap, will follow the normal agency's logic.
        /// </param>
        /// <returns>Cap With Condition Model4WS</returns>
        CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID4ws, string userSeqNbr, string isConditionOfApproval, bool isSuperAgency);

        /// <summary>
        /// Gets the condition model by cap type.
        /// </summary>
        /// <param name="capType">The cap type model.</param>
        /// <param name="includeInactivity">Indicates whether include inactivity status or not.</param>
        /// <returns>return the NoticeConditionModel list.</returns>
        NoticeConditionModel[] GetStdConditionByRecordType(CapTypeModel capType, bool includeInactivity);

        /// <summary>
        /// Gets cap view by single set.
        /// Gets cap contains CapDetailModel, CapWorkDesModel, AddressModel, CapContactModel, ContactsGroup,
        /// ParcelModel, OwnerModel, LicenseProfessionalModel, <c>BValuatnModel</c>.
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <param name="isForView">is For View</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <param name="isSuperAgency">There are difference logic between super agency and normal agency during get the cap data.
        /// In super agency, we need to go through the normal agency logic in sometimes. So expose the <see cref="isSuperAgency" /> parameter.
        /// Such as: User select single service to create cap, will follow the normal agency's logic.</param>
        /// <returns>Cap information With Condition</returns>
        CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID4ws, string userSeqNbr, bool isForView, string isConditionOfApproval, bool isSuperAgency);

        /// <summary>
        /// Gets cap view by single set.
        /// Gets cap contains CapDetailModel, CapWorkDesModel, AddressModel, CapContactModel, ContactsGroup,
        /// ParcelModel, OwnerModel, LicenseProfessionalModel, <c>BValuatnModel</c>.
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
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
        CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID4ws, string userSeqNbr, bool isForView, string isConditionOfApproval, bool isSuperAgency, bool isForRenew);

        /// <summary>
        /// Gets Related Cap view Detail.
        /// </summary>
        /// <param name="capID4ws">cap ID number.</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <returns>Cap information</returns>
        CapModel4WS GetCapViewDetailByPK(CapIDModel4WS capID4ws, string userSeqNbr);

        /// <summary>
        /// Get the child agencies from cap id models.
        /// </summary>
        /// <param name="paymentResultModels">cap id models</param>
        /// <returns>agencies table</returns>
        DataTable GetChildAgencies(PaymentResultModel4WS[] paymentResultModels);

        /// <summary>
        /// Get child caps by mater cap id.
        /// </summary>
        /// <param name="parentCapID4WS">master cap id</param>
        /// <param name="relationship">cap relationship</param>
        /// <param name="status">the cap status</param>
        /// <returns>return the child Cap information.</returns>
        CapModel4WS[] GetChildCapDetailsByMasterID(CapIDModel4WS parentCapID4WS, string relationship, string status);

        /// <summary>
        /// Get child cap id by master cap id.
        /// </summary>
        /// <param name="capID4WS">master cap id</param>
        /// <param name="relationship">cap relationship, e.g.: renew, pay fee due...</param>
        /// <param name="status">the cap status, e.g.: incomplete, pending...</param>
        /// <param name="isAcrossAgencies">true:In ACA super agency get child cap. otherwise is false.</param>
        /// <returns>Cap id model for ACA.</returns>
        CapIDModel4WS GetChildCapIDByMasterID(CapIDModel4WS capID4WS, string relationship, string status, bool isAcrossAgencies);

        /// <summary>
        /// Get child cap id models by parent cap model
        /// </summary>
        /// <param name="parentCapModel">CapIDModel4WS entity</param>
        /// <returns>Array of CapIDModel4WS</returns>
        CapIDModel4WS[] GetChildCaps(CapIDModel4WS parentCapModel);

        /// <summary>
        /// get the child agency's cap id table.
        /// </summary>
        /// <param name="paymentResults">payment result models</param>
        /// <param name="childAgency">child agency code</param>
        /// <returns>child agency's cap id table</returns>
        DataTable GetChildCapsByAgency(PaymentResultModel4WS[] paymentResults, string childAgency);

        /// <summary>
        /// Get the related CAP ids which are not included in the incoming <see cref="capIds"/> based on the CAP <see cref="relationship"/>.
        /// </summary>
        /// <param name="capIds">Cap Id list</param>
        /// <param name="relationship">It may be "R", "EST", "Renewal", "Amendment" or "Ass Form".</param>
        /// <returns>missed Cap Id list</returns>
        IList<CapIDModel> GetMissingRecordIDs4ShoppingCart(CapIDModel[] capIds, string relationship);

        /// <summary>
        /// Get ModuleName array by capId list.
        /// </summary>
        /// <param name="capIdModels">CapID Model array</param>
        /// <returns>return string array</returns>
        StringArray[] GetModuleNamesByCapIDList(CapIDModel[] capIdModels);

        /// <summary>
        /// Gets the my cap List for ACA.
        /// 1) Get the license professional list base on registered user.
        /// 2) Loop the license professional to get the caps
        /// 3) Combine the caps into the original my permit list.
        /// 4) add the CAPs that were created by him
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="capModel4WS">cap model object</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="userSeqNum">public user sequence number</param>
        /// <param name="capInfo">CAP Info: COMPLETE, INCOMPLETE, ALL </param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>array of CapModel4WS</returns>
        SearchResultModel GetMyCapList4ACA(string serviceProviderCode, CapModel4WS capModel4WS, QueryFormat queryFormat, string userSeqNum, string capInfo, string[] viewElementNames);

        /// <summary>
        /// Get parent parent cap id by related child cap ID, relationship and status.
        /// </summary>
        /// <param name="childCapID">related child cap ID for web service</param>
        /// <param name="relationship">It may be "R", "EST", "Renewal", "Amendment" or "Ass Form".</param>
        /// <param name="status">1. It is for renewal CAP, it may be "Incomplete", "Review" or "Complete".
        ///                      2. It is for amendment CAP, it may be  "Incomplete" or "Complete".
        ///                      3. It is for Ass Form CAP, it may be null.</param>
        /// <returns>CapIDModel object</returns>
        CapIDModel GetParentCapIDByChildCapID(CapIDModel childCapID, string relationship, string status);

        /// <summary>
        /// Get parent parent cap id list by related child cap ID, relationship and status.
        /// </summary>
        /// <param name="childCapID">related child cap ID for web service</param>
        /// <param name="relationship">It may be "R", "Renewal", "Amendment" or "Ass Form", except "EST".</param>
        /// <param name="status">1. It is for renewal CAP, it may be "Incomplete", "Review" or "Complete".
        ///                      2. It is for amendment CAP, it may be  "Incomplete" or "Complete".
        ///                      3. It is for Ass Form CAP, it may be null.</param>
        /// <returns>CapIDModel object array</returns>
        CapIDModel[] GetParentCapIDListByChildCapID(CapIDModel childCapID, string relationship, string status);

        /// <summary>
        /// Get permit list data by address model.
        /// </summary>
        /// <param name="address">address value.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <param name="isOnlyMyPermit">is only my permit</param>
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
        SearchResultModel GetPermitsByAddress(AddressModel address, string agencyCode, string moduleName, string callerId, bool isOnlyMyPermit, List<string> moduleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames);

        /// <summary>
        /// Get permit list data by contact.
        /// </summary>
        /// <param name="capContactModel">cap contact model.</param>
        /// <param name="moduleName">module name.</param>
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
        SearchResultModel GetPermitsByContact(CapContactModel4WS capContactModel, string moduleName, bool isOnlyMyCAP, List<string> moduleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames);

        /// <summary>
        /// Get permit list data by license id.
        /// </summary>
        /// <param name="licenseType">the license type.</param>
        /// <param name="licenseStateNumber">the license state number.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <param name="isOnlyMyPermit">is only my permit</param>
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
        SearchResultModel GetPermitsByLicenseId(string licenseType, string licenseStateNumber, string agencyCode, string moduleName, string callerId, bool isOnlyMyPermit, List<string> moduleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames);

        /// <summary>
        /// Gets Related Cap List.
        /// </summary>
        /// <param name="rootCAPID4WS">root CAP ID</param>
        /// <param name="searchConstions4WS">cap model containing search conditions</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>Array of CapModel4WS</returns>
        CapModel4WS[] GetRelatedCapList(CapIDModel4WS rootCAPID4WS, CapModel4WS searchConstions4WS, QueryFormat queryFormat);

        /// <summary>
        ///  Get related Caps tree
        /// </summary>
        /// <param name="rootCAPID4WS">CapIDModel4WS entity</param>
        /// <param name="searchConstions4WS">CapModel4WS entity</param>
        /// <param name="queryFormat">queryFormat entity</param>
        /// <returns>ProjectTreeNodeModel4WS model </returns>
        ProjectTreeNodeModel4WS GetRelatedCapTree(CapIDModel4WS rootCAPID4WS, CapModel4WS searchConstions4WS, QueryFormat queryFormat);

        /// <summary>
        /// get licensee's related caps. and add the user role into them.
        /// </summary>
        /// <param name="licensee">License Model 4WS</param>
        /// <param name="moduleNames">active module names</param>
        /// <param name="queryFormat">the queryFormat model</param>
        /// <returns>a data table for cap list</returns>
        DataTable GetRelatedCapsByRefLP(LicenseModel4WS licensee, List<string> moduleNames, QueryFormat queryFormat);

        /// <summary>
        /// Get review child renewal cap ID by renewal parent Cap ID.
        /// </summary>
        /// <param name="capID4WS">Cap ID Model for web service</param>
        /// <returns>CapIDModel4WS object</returns>
        CapIDModel4WS GetReviewChildCapIDByMasterID(CapIDModel4WS capID4WS);

        /// <summary>
        /// Get a related records tree for current record ID.
        /// This tree only include the parent layer and the child layer of the current record ID(<see cref="currentRecordId"/>).
        /// </summary>
        /// <param name="currentRecordId">Current record ID.</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>Tree node model</returns>
        ProjectTreeNodeModel4WS GetSectionalRelatedCapTree(CapIDModel currentRecordId, QueryFormat queryFormat);

        /// <summary>
        /// Get Service list by Cap ID.
        /// </summary>
        /// <param name="capID4WS">CapID Model</param>
        /// <returns>return an array of Service Model</returns>
        ServiceModel[] GetServicesByRecordID(CapIDModel4WS capID4WS);

        /// <summary>
        /// Get Trade Name CapModel
        /// </summary>
        /// <param name="capIDModel">It like with capIDModel(customID,serviceProviderCode)</param>
        /// <returns>CapModel4WS of Trade Name</returns>
        CapModel4WS GetTradeNameCapModel(CapIDModel4WS capIDModel);

        /// <summary>
        /// Get auto issue flag by renewal child cap ID.
        /// </summary>
        /// <param name="childCapID4WS">renewal child cap ID for web service</param>
        /// <returns>CapIDModel4WS object</returns>
        bool IsAutoIssue4RenewalByChildCapID(CapIDModel4WS childCapID4WS);

        /// <summary>
        /// Is the cap created by initial user
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <returns>true or false.</returns>
        bool IsCreateByDelegateUser(CapModel4WS capModel);
             
        /// <summary>
        /// Gets a value indicating whether the <see cref="capClass"/> is partial CAP class.
        /// </summary>
        /// <param name="capClass">the CAP class</param>
        /// <returns>true or false.</returns>
        bool IsPartialCap(string capClass);

        /// <summary>
        /// check whether the select Cap can be renewed by the current public user.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="capIDModel4WS">Cap ID Model</param>
        /// <returns>boolean:whether is renewed</returns>
        bool IsRenewableForSingleCapID(string serviceProviderCode, CapIDModel4WS capIDModel4WS);

        /// <summary>
        /// Check that if configuration is valid for renewal license function.
        /// </summary>
        /// <param name="capID4WS">parent renewal cap ID.</param>
        /// <returns>configuration is wrong return false; configuration is right return true;</returns>
        bool IsValidConfig4Renewal(CapIDModel4WS capID4WS);

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
        SearchResultModel QueryPermitsGC(CapModel4WS capModel, List<string> moduleNames, string userSeqNum, bool isOnlyMyCAP, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames);

        /// <summary>
        /// save caps for sub agencies
        /// </summary>
        /// <param name="capModel4WS">The parent cap</param>
        /// <returns>The parent cap.</returns>
        CapModel4WS SavePartialCaps(CapModel4WS capModel4WS);

        /// <summary>
        /// create or update a partial cap.
        /// </summary>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <param name="wotModelSeq">work order ID</param>
        /// <param name="isFeeEstimates">whether fee estimate</param>
        /// <returns>CapModel4WS object</returns>
        CapModel4WS SaveWrapperForPartialCap(CapModel4WS capModel4WS, string wotModelSeq, bool isFeeEstimates);

        /// <summary>
        /// Generate a new alt id for fee estimate partial cap or partial cap, update cap alt ID and return it.
        /// </summary>
        /// <param name="capModel4WS">CapModel4WS entity</param>
        /// <param name="isFeeEstCap">Needs generate a new alt id for fee estimate partial cap, if this flag is true</param>
        /// <param name="isTemporaryCap">Needs generate a new alt id for fee estimate partial cap, if this flag is true.</param>
        /// <returns>alt ID number.</returns>
        string UpdateCapAltID(CapModel4WS capModel4WS, bool isFeeEstCap, bool isTemporaryCap);

        /// <summary>
        /// Method to update a cap's class.
        /// </summary>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <returns>CapModel4WS object</returns>
        CapModel4WS UpdateCapClass(CapModel4WS capModel4WS);

        /// <summary>
        /// Update cap class
        /// </summary>
        /// <param name="caps">the cap information which need to be updated</param>
        void UpdateCapStatus(CapModel4WS[] caps);

        /// <summary>
        /// Method to update a partial cap.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <returns>CapModel4WS object</returns>
        CapModel4WS UpdatePartialCapModelWrapper(string serviceProviderCode, CapModel4WS capModel4WS, string publicUserId);

        /// <summary>
        /// Update partial caps for super agency
        /// </summary>        
        /// <param name="capModel4WS">The parent cap</param>
        /// <returns>The parent cap.</returns>
        CapModel4WS UpdatePartialCaps(CapModel4WS capModel4WS);

        /// <summary>
        /// Remove child partial cap by cap id
        /// </summary>
        /// <param name="parentCapId">parent cap Id</param>
        /// <param name="childCapId">child cap Id</param>
        /// <param name="userId">user id</param>
        void RemoveChildPartialCap(CapIDModel4WS parentCapId, CapIDModel4WS childCapId, string userId);

        /// <summary>
        /// Populate the CapModel4WS list to a DataTable.
        /// </summary>
        /// <param name="capModel4WSList">a CapModel4WS list</param>
        /// <returns>
        /// The permit list data table. 
        /// </returns>
        DataTable BuildPermitDataTable(CapModel4WS[] capModel4WSList);

        /// <summary>
        /// Get licenses by customer sequence number
        /// </summary>
        /// <param name="contactSeqNum">the customer sequence number</param>
        /// <param name="modueName">the module name</param>
        /// <param name="filterName">the filter name</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="callerID">the caller id </param>
        /// <returns>SimpleCap model array</returns>
        SimpleCapModel[] GetLicensesByCustomer(string contactSeqNum, string modueName, string filterName, QueryFormat queryFormat, string callerID);

        /// <summary>
        /// Get the cap temporary data.
        /// </summary>
        /// <param name="capTemporaryData">The CapTemporaryDataModel</param>
        /// <returns>return the CapTemporaryDataModel</returns>
        CapTemporaryDataModel GetCapTemporaryData(CapTemporaryDataModel capTemporaryData);

        /// <summary>
        /// update the cap temporary data.
        /// </summary>
        /// <param name="capTemporaryData">The CapTemporaryDataModel</param>
        void UpdateCapTemporaryData(CapTemporaryDataModel capTemporaryData);

        /// <summary>
        /// Clear the component data in hidden page for the CapModel.
        /// </summary>
        /// <param name="capModel">The Current CapModel</param>
        /// <param name="pageFlowGroup">The PageFlow group</param>
        void ClearCapData(CapModel4WS capModel, PageFlowGroupModel pageFlowGroup);

        #endregion Methods
    }
}