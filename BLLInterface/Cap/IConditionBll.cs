#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IConditionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  IConditionBll Bll interface..
 *
 *  Notes:
 * $Id: IConditionBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// Defines methods for Condition function.
    /// </summary>
    public interface IConditionBll
    {
        #region Methods

        /// <summary>
        /// Get NoticeCondition info by cap id.
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <returns>Array of NoticeConditionModel</returns>
        NoticeConditionModel[] GetAllCondition4ACAFeeEstimate(CapIDModel4WS capID, string isConditionOfApproval);

        /// <summary>
        /// Get capCondition info by cap id.
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <returns>Array of CapConditionModel4WS</returns>
        CapConditionModel4WS[] GetCapConditions(CapIDModel4WS capID);

        /// <summary>
        /// Get contact condition
        /// </summary>
        /// <param name="contactSeqNumber">contact sequence number</param>
        /// <returns>condition notice model</returns>
        ConditionNoticeModel GetContactConditionNotices(string contactSeqNumber);

        /// <summary>
        /// Get all conditions of address
        /// </summary>
        /// <param name="servProvCode">Service Provider Code</param>
        /// <param name="refAddressPK">Reference Address Model with key information such as reference address id, source sequence number and UID</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>Array of NoticeConditionModel</returns>
        NoticeConditionModel[] GetAddressConditions(string servProvCode, RefAddressModel refAddressPK, string callerID);

        /// <summary>
        /// Get all conditions of parcel
        /// </summary>
        /// <param name="servProvCode">Service Provider Code</param>
        /// <param name="parcelPKModel">Parcel Model with key information such as parcel number, source sequence number and UID</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>Array of NoticeConditionModel</returns>
        NoticeConditionModel[] GetParcelConditions(string servProvCode, ParcelModel parcelPKModel, string callerID);

        /// <summary>
        /// Get highest condition from condition list
        /// </summary>
        /// <param name="noticeConditions">notice condition list</param>
        /// <returns>the highest condition</returns>
        NoticeConditionModel GetHighestCondition(NoticeConditionModel[] noticeConditions);

        /// <summary>
        /// Indicate record locked or not.
        /// </summary>
        /// <param name="capID">Cap ID Model</param>
        /// <returns>whether cap lock</returns>
        bool IsCapLocked(CapIDModel capID);

        /// <summary>
        /// Judges the parcel is locked or not.
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="parcelPKModel">parcel model</param>
        /// <param name="callerID">caller id</param>
        /// <returns>whether parcel lock</returns>
        bool IsParcelLocked(string servProvCode, ParcelModel parcelPKModel, string callerID);

        /// <summary>
        /// Judges the address is locked or not.
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="refAddressPK">address primary key mode</param>
        /// <param name="callerID">caller id</param>
        /// <returns>whether address lock</returns>
        bool IsAddressLocked(string servProvCode, RefAddressModel refAddressPK, string callerID);

        /// <summary>
        /// Judges the license is locked or not.
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="sequenceNbr">license sequence number.</param>
        /// <param name="callerID">caller id</param>
        /// <returns>whether license lock</returns>
        bool IsLicenseLocked(string servProvCode, long sequenceNbr, string callerID);

        /// <summary>
        /// Judges the contact is locked or not.
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="sequenceNbr">contact sequence number.</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>whether contact lock.</returns>
        bool IsContactLocked(string servProvCode, long sequenceNbr, string callerID);

        /// <summary>
        /// Judges the owner is locked or not.
        /// </summary>
        /// <param name="servProvCode">Agency code.</param>
        /// <param name="owner">Owner Model</param>
        /// <param name="callerID">Caller Id.</param>
        /// <returns>whether owner lock.</returns>
        bool IsOwnerLocked(string servProvCode, OwnerModel owner, string callerID);

        /// <summary>
        /// Get condition of cap by condition number
        /// </summary>
        /// <param name="capID">Cap ID Model</param>
        /// <param name="conditionNbr">Condition number</param>
        /// <returns>Notice Condition information</returns>
        NoticeConditionModel GetCapConditionApprovalByNbr(CapIDModel capID, string conditionNbr);

        #endregion Methods
    }
}