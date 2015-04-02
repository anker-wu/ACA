#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminFeeBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AdminFeeBll.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/10/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;
using Accela.ACA.BLL.Finance;
using Accela.ACA.WSProxy;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is operation fee in ACA admin. 
    /// </summary>
    public class AdminFeeBll : BaseBll, IFeeBll
    {
        #region Methods

        /// <summary>
        /// Adds fee items
        /// </summary>
        /// <param name="feeItemArray">fee item array</param>
        /// <returns>whether update successfully</returns>
        public bool AddFeeItem(F4FeeItemModel4WS[] feeItemArray)
        {
            return false;
        }

        /// <summary>
        /// Updates and Recalculates fee item by Cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <param name="quantities">fee item quantity array</param>
        /// <param name="feeIDs">fee ID array</param>
        /// <param name="statuses">fee status array</param>
        /// <param name="feeFactor">fee factor, such as job value.</param>
        /// <returns>whether updated successfully</returns>
        public bool DoRecalculate(CapIDModel4WS capID, string callID, double[] quantities, long[] feeIDs, string[] statuses, double feeFactor)
        {
            return false;
        }

        /// <summary>
        /// Updates and Recalculates fee item by Cap IDs
        /// </summary>
        /// <param name="calculateParameters">cap fee items with cap id</param>
        /// <returns>whether updated successfully</returns>
        public bool DoRecalculates(RecalculateParameterModel4WS[] calculateParameters)
        {
            return false;
        }

        /// <summary>
        /// Gets calculated valuation by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>calculated valuation</returns>
        public double GetCalculatedValuation(CapIDModel4WS capID, string callID)
        {
            return 0;
        }

        /// <summary>
        /// get total fee for a child agency.
        /// </summary>
        /// <param name="allFeeItems">all fee items of a super agency cap,include all child cap fee items</param>
        /// <param name="childAgency">child cap agency code</param>
        /// <returns>child agency total fee</returns>
        public double GetChildAgencyTotalFee(List<F4FeeItemModel4WS> allFeeItems, string childAgency)
        {
            return 0;
        }

        /// <summary>
        /// get child agencies from the fee items of all child caps in a parent cap .the child agencies have fee.
        /// </summary>
        /// <param name="allFeeItems">fee items of all Child caps in a parent cap</param>
        /// <returns>a fee item for a child agency in a parent cap, the child agencies have fee</returns>
        public ArrayList GetChildAgencysByFeeItems(List<F4FeeItemModel4WS> allFeeItems)
        {
            return new ArrayList();
        }

        /// <summary>
        /// get fee items of child cap belong a parent cap.
        /// </summary>
        /// <param name="allFeeItems">fee items of all child caps in a parent cap</param>
        /// <param name="childCapID">child cap id model</param>
        /// <returns>fee items of a child cap</returns>
        public List<F4FeeItemModel4WS> GetChildCapFeeItems(List<F4FeeItemModel4WS> allFeeItems, CapIDModel4WS childCapID)
        {
            return new List<F4FeeItemModel4WS>();
        }

        /// <summary>
        /// get total fee for a child cap.
        /// </summary>
        /// <param name="allFeeItems">all fee items of a super agency cap,include all Child cap fee items</param>
        /// <param name="childCapID">child cap id model</param>
        /// <returns> child cap total fee </returns>
        public double GetChildCapTotalFee(List<F4FeeItemModel4WS> allFeeItems, CapIDModel4WS childCapID)
        {
            return 0;
        }

        /// <summary>
        /// get one child agency caps from the fee items of all child caps in a parent cap .the child caps have fee.
        /// </summary>
        /// <param name="allFeeItems">fee items of all child caps in a parent cap</param>
        /// <param name="childAgency">child cap agency code</param>
        /// <returns>a fee item for a child cap in a agency of a parent cap, the child caps have fee</returns>
        public List<F4FeeItemModel4WS> GetChildCapsByAgency(List<F4FeeItemModel4WS> allFeeItems, string childAgency)
        {
            return new List<F4FeeItemModel4WS>();
        }

        /// <summary>
        /// Gets Contractor Supplied Valuation by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>Contractor supplied valuations</returns>
        public BValuatnModel4WS[] GetContractorSuppliedValuation(CapIDModel4WS capID, string callID)
        {
            return new BValuatnModel4WS[0];
        }

        /// <summary>
        /// Get fee items to be associated with the plans
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <returns>The fee items array of the plan</returns>
        public F4FeeItemModel4WS[] GetFeeEstimate4PlanReview(long transactionID)
        {
            return new F4FeeItemModel4WS[0];
        }

        /// <summary>
        /// Get fee items to be associated with the plans
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <param name="feeSchedule">Fee Schedule</param>
        /// <returns>The fee items array of the plan</returns>
        public F4FeeItemModel4WS[] GetFeeEstimate4PlanReview(long transactionID, string feeSchedule)
        {
            return new F4FeeItemModel4WS[0];
        }

        /// <summary>
        /// Gets fee items by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>Array of F4FeeModel4WS</returns>
        public F4FeeModel4WS[] GetFeeItemsByCapID(CapIDModel4WS capID, string callID)
        {
            return new F4FeeModel4WS[0];
        }

        /// <summary>
        /// get fee items of child caps related to parent cap.
        /// </summary>
        /// <param name="capID">parent cap ID</param>
        /// <param name="isIncludeParent">Indicate whether include the parent's FeeItems</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>Array of F4FeeModel4WS</returns>
        public F4FeeModel4WS[] GetFeeItemsByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID)
        {
            return new F4FeeModel4WS[0];
        }

        /// <summary>
        /// Gets NoPaid fee items by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Array of F4FeeModel4WS</returns>
        public F4FeeModel4WS[] GetNoPaidFeeItemByCapID(CapIDModel4WS capID, string callerID)
        {
            return new F4FeeModel4WS[0];
        }

        /// <summary>
        /// Gets paid fee items by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Array of F4FeeModel4WS</returns>
        public F4FeeModel4WS[] GetPaidFeeItemByCapID(CapIDModel4WS capID, string callerID)
        {
            return new F4FeeModel4WS[0];
        }

        /// <summary>
        /// Gets reference fee item array
        /// </summary>
        /// <param name="spCode">agency code</param>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="feeSchedule">fee schedule</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>reference fee item array</returns>
        public RFeeItemModel4WS[] GetRefFeeItemList(string spCode, CapIDModel4WS capID, string feeSchedule, QueryFormat4WS qf, string callID)
        {
            return new RFeeItemModel4WS[0];
        }

        /// <summary>
        /// Self plan checking payment:
        /// Create fee items to be associated with special CAP at first time.
        /// 1. Create fee.
        /// 2. Create Invoice
        /// 3. Get total fee.
        /// </summary>
        /// <param name="spCode">SP Code values.</param>
        /// <param name="planSeqNum">plan Sequence Number</param>
        /// <param name="callerId">caller Id  number.</param>
        /// <returns>Self Plan Fee Total.</returns>
        public string[] GetSelfPlanFeeTotal(string spCode, long planSeqNum, string callerId)
        {
            return new string[0];
        }

        /// <summary>
        /// Get total balance fee for existing cap payment
        /// </summary>
        /// <param name="capID">CapIDModel for web service.</param>
        /// <param name="callID">Caller ID recode method invoker.</param>
        /// <returns>Total balance fee</returns>
        public double GetTotalBalanceFee(CapIDModel4WS capID, string callID)
        {
            return 0;
        }

        /// <summary>
        /// Gets total fee by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>total fee value.</returns>
        public double GetTotalFee(CapIDModel4WS capID, string callID)
        {
            return 0;
        }

        /// <summary>
        /// get total fee of child caps related to parent cap
        /// </summary>
        /// <param name="capID">parent cap id</param>
        /// <param name="isIncludeParent">Indicate whether include the parent's Fees</param>
        /// <param name="callID">caller id number.</param>
        /// <returns>total fee value.</returns>
        public double GetTotalFeeByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID)
        {
            return 0;
        }

        /// <summary>
        /// Get total fee of a plan review transaction
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <returns>total fee value.</returns>
        public double GetTotalFeeByTransactionID(long transactionID)
        {
            return 0;
        }

        /// <summary>
        /// if some child cap related parent cap have fee item ,return true ,other is false.
        /// </summary>
        /// <param name="capID">parent cap id</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's FeeItems</param>
        /// <param name="callID">caller id number.</param>
        /// <returns><c>true</c> if [has fee item by parent cap unique identifier] [the specified cap unique identifier]; otherwise, <c>false</c>.</returns>
        public bool HasFeeItemByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID)
        {
            return false;
        }

        /// <summary>
        /// indicates job value is enable or not.
        /// </summary>
        /// <param name="capType">the cap type</param>
        /// <param name="callerId">the public use id</param>
        /// <returns>True if given cap has job value</returns>
        public bool IsJobValueEnable(CapTypeModel capType, string callerId)
        {
            return false;
        }

        /// <summary>
        /// if all fee item of child caps related to parent cap are read only ,then true ,other is false;
        /// </summary>
        /// <param name="capID">parent cap id</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's FeeItems</param>
        /// <param name="callID">caller id number.</param>
        /// <returns>Is read only</returns>
        public bool IsReadOnlyByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID)
        {
            return false;
        }

        /// <summary>
        /// update fee item
        /// </summary>
        /// <param name="arrayF4FeeItemModel">Fee Item Model Array</param>
        /// <param name="capID4WS">Cap ID Model</param>
        public void UpdateFeeItems(F4FeeItemModel4WS[] arrayF4FeeItemModel, CapIDModel4WS capID4WS)
        {
        }

        /// <summary>
        /// Validate coupon code.
        /// </summary>
        /// <param name="coponCode">Coupon Code</param>
        /// <returns>Integer of validation code</returns>
        public int ValidateCouponCode(string coponCode)
        {
            return -1;
        }

        /// <summary>
        /// if have fee change cap, return fee changed cap id list.
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        /// <returns>Fee Changed cap id list</returns>
        public CapIDModel4WS[] GetFeeChangedCapIdList(CapIDModel4WS[] capIDs)
        {
            return null;
        }

        /// <summary>
        /// Update Fee Schedule Or Fee Items.
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        public void UpdateFeeScheduleOrFeeItems(CapIDModel4WS[] capIDs)
        {
        }

        /// <summary>
        /// If the cap has fee, return true else return false.
        /// </summary>
        /// <param name="capID">The cap id</param>
        /// <param name="isMultiCap">Whether has child caps</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's Fees</param>
        /// <returns><c>true</c> if the specified cap unique identifier has fee; otherwise, <c>false</c>.</returns>
        public bool HasFee(CapIDModel4WS capID, bool isMultiCap, bool isIncludeParent)
        {
            return false;
        }

        /// <summary>
        /// Gets each examination's no paid exam fee items.
        /// </summary>
        /// <param name="exams">the examinations to get it's no paid fee item.</param>
        /// <returns>return the no paid examination fee items</returns>
        public F4FeeItemModel4WS[] GetNoPaidExamFeeItemsByExams(ExaminationModel[] exams)
        {
            return null;
        }

        #endregion Methods
    }
}