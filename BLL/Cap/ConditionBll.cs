#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ConditionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  FeeConditionBll..
 *
 *  Notes:
 * $Id: ConditionBll.cs 277355 2014-08-14 07:28:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// This class provide the ability to operation condition.
    /// </summary>
    public class ConditionBll : BaseBll, IConditionBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of ConditionService.
        /// </summary>
        private ConditionWebServiceService ConditionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ConditionWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get NoticeCondition info by cap id.
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <returns>Array of NoticeConditionModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public NoticeConditionModel[] GetAllCondition4ACAFeeEstimate(CapIDModel4WS capID, string isConditionOfApproval)
        {
            try
            {
                NoticeConditionModel[] response = ConditionService.getAllCondition4ACAFeeEstimate(capID, User.PublicUserId, isConditionOfApproval);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get capCondition info by cap id.
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <returns>Array of CapConditionModel4WS</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapConditionModel4WS[] GetCapConditions(CapIDModel4WS capID)
        {
            try
            {
                return ConditionService.getCapConditions(capID, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get contact condition
        /// </summary>
        /// <param name="contactSeqNumber">contact sequence number</param>
        /// <returns>condition notice model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ConditionNoticeModel GetContactConditionNotices(string contactSeqNumber)
        {
            try
            {
                NoticeConditionModel[] notices = ConditionService.getContactConditions(AgencyCode, contactSeqNumber);

                NoticeConditionModel highestCondition = ((IConditionBll)this).GetHighestCondition(notices);

                // Combine highest condition and list to one model
                ConditionNoticeModel conditionModel = new ConditionNoticeModel();
                conditionModel.HighestCondition = highestCondition;
                conditionModel.NoticeConditions = notices;

                return conditionModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Indicate record locked or not.
        /// </summary>
        /// <param name="capID">Cap ID Model</param>
        /// <returns>whether cap lock</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        bool IConditionBll.IsCapLocked(CapIDModel capID)
        {
            try
            {
                bool recordLocked = ConditionService.isCapLocked(capID);
               
                return recordLocked;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Judges the parcel is locked or not.
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="parcelPKModel">parcel model</param>
        /// <param name="callerID">caller id</param>
        /// <returns>whether parcel lock</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        bool IConditionBll.IsParcelLocked(string servProvCode, ParcelModel parcelPKModel, string callerID)
        {
            try
            {
                bool parcelLocked = ConditionService.isParcelLocked(servProvCode, parcelPKModel, callerID);

                return parcelLocked;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Judges the address is locked or not.
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="refAddressPK">address primary key mode</param>
        /// <param name="callerID">caller id</param>
        /// <returns>whether address lock</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        bool IConditionBll.IsAddressLocked(string servProvCode, RefAddressModel refAddressPK, string callerID)
        {
            try
            {
                bool addressLocked = ConditionService.isAddressLocked(servProvCode, refAddressPK, callerID);

                return addressLocked;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Judges the license is locked or not.
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="sequenceNbr">license sequence number.</param>
        /// <param name="callerID">caller id</param>
        /// <returns>whether license lock</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        bool IConditionBll.IsLicenseLocked(string servProvCode, long sequenceNbr, string callerID)
        {
            try
            {
                bool licenseLocked = ConditionService.isLicenseLocked(servProvCode, sequenceNbr, callerID);

                return licenseLocked;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Judges the owner is locked or not.
        /// </summary>
        /// <param name="servProvCode">Agency code.</param>
        /// <param name="owner">Owner Model</param>
        /// <param name="callerID">Caller Id.</param>
        /// <returns>whether owner lock.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        bool IConditionBll.IsOwnerLocked(string servProvCode, OwnerModel owner, string callerID)
        {
            try
            {
                bool ownerLocked = ConditionService.isOwnerLocked(servProvCode, owner, callerID);

                return ownerLocked;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Judges the contact is locked or not.
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="sequenceNbr">contact sequence number.</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>whether contact lock.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        bool IConditionBll.IsContactLocked(string servProvCode, long sequenceNbr, string callerID)
        {
            try
            {
                bool contactLocked = ConditionService.isContactLocked(servProvCode, sequenceNbr, callerID);

                return contactLocked;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get highest condition
        /// </summary>
        /// <param name="noticeConditions">notice conditions</param>
        /// <returns>the highest condition</returns>
        NoticeConditionModel IConditionBll.GetHighestCondition(NoticeConditionModel[] noticeConditions)
        {
            NoticeConditionModel highestCondition = null;
            Hashtable conditionList = new Hashtable();
            string lockStatus = "LOCK";
            string hodeStatus = "HODE";
            string noticeStatus = "NOTICE";

            if (noticeConditions != null)
            {
                foreach (NoticeConditionModel noticeCondition in noticeConditions)
                {
                    if (noticeCondition == null)
                    {
                        continue;
                    }

                    if (ACAConstant.LOCK_CONDITION.Equals(noticeCondition.impactCode, StringComparison.OrdinalIgnoreCase)
                        && !conditionList.ContainsKey(lockStatus))
                    {
                        conditionList.Add(lockStatus, noticeCondition);
                    }

                    if (ACAConstant.HOLD_CONDITION.Equals(noticeCondition.impactCode, StringComparison.OrdinalIgnoreCase)
                        && !conditionList.ContainsKey(hodeStatus))
                    {
                        conditionList.Add(hodeStatus, noticeCondition);
                    }

                    if (ACAConstant.NOTICE_CONDITION.Equals(noticeCondition.impactCode, StringComparison.OrdinalIgnoreCase)
                        && !conditionList.ContainsKey(noticeStatus))
                    {
                        conditionList.Add(noticeStatus, noticeCondition);
                    }
                }
            }

            if (conditionList.ContainsKey(lockStatus))
            {
                highestCondition = conditionList[lockStatus] as NoticeConditionModel;
            }
            else if (conditionList.ContainsKey(hodeStatus))
            {
                highestCondition = conditionList[hodeStatus] as NoticeConditionModel;
            }
            else if (conditionList.ContainsKey(noticeStatus))
            {
                highestCondition = conditionList[noticeStatus] as NoticeConditionModel;
            }

            return highestCondition;
        }

        /// <summary>
        /// Get all conditions of address
        /// </summary>
        /// <param name="servProvCode">Service Provider Code</param>
        /// <param name="refAddressPK">Reference Address Model with key information such as reference address id, source sequence number and UID</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>Array of NoticeConditionModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        NoticeConditionModel[] IConditionBll.GetAddressConditions(string servProvCode, RefAddressModel refAddressPK, string callerID)
        {
            try
            {
                return ConditionService.getAddressConditions(servProvCode, refAddressPK, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get all conditions of parcel
        /// </summary>
        /// <param name="servProvCode">Service Provider Code</param>
        /// <param name="parcelPKModel">Parcel Model with key information such as parcel number, source sequence number and UID</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>Array of NoticeConditionModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        NoticeConditionModel[] IConditionBll.GetParcelConditions(string servProvCode, ParcelModel parcelPKModel, string callerID)
        {
            try
            {
                return ConditionService.getParcelConditions(servProvCode, parcelPKModel, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get condition of cap by condition number
        /// </summary>
        /// <param name="capID">Cap ID Model</param>
        /// <param name="conditionNbr">Condition number</param>
        /// <returns>Notice Condition information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        NoticeConditionModel IConditionBll.GetCapConditionApprovalByNbr(CapIDModel capID, string conditionNbr)
        {
            try
            {
                return ConditionService.getCapConditionApprovalByNbr(capID, conditionNbr);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}