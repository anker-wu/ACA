#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IFeeBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IFeeBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// Defines methods for Fee BLL.
    /// </summary>
    public interface IFeeBll
    {
        #region Methods

        /// <summary>
        /// Adds fee items
        /// </summary>
        /// <param name="feeItemArray">fee item array</param>
        /// <returns>whether update successfully</returns>
        bool AddFeeItem(F4FeeItemModel4WS[] feeItemArray);

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
        bool DoRecalculate(CapIDModel4WS capID, string callID, double[] quantities, long[] feeIDs, string[] statuses, double feeFactor);

        /// <summary>
        /// Updates and Recalculates fee item by Cap IDs
        /// </summary>
        /// <param name="calculateParameters">cap fee items with cap id  </param>
        /// <returns>whether updated successfully</returns>
        bool DoRecalculates(RecalculateParameterModel4WS[] calculateParameters);

        /// <summary>
        /// Gets calculated valuation by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>calculated valuation</returns>
        double GetCalculatedValuation(CapIDModel4WS capID, string callID);

        /// <summary>
        /// get total fee for a child agency.
        /// </summary>
        /// <param name="allFeeItems">all fee items of a super agency cap,include all child cap fee items</param>
        /// <param name="childAgency">child cap agency code</param>
        /// <returns>child agency total fee</returns>
        double GetChildAgencyTotalFee(List<F4FeeItemModel4WS> allFeeItems, string childAgency);

        /// <summary>
        /// get child agencies from the fee items of all child caps in a parent cap .the child agencies have fee.
        /// </summary>
        /// <param name="allFeeItems">fee items of all Child caps in a parent cap </param>
        /// <returns>a fee item for a child agency in a parent cap, the child agencies have fee</returns>
        ArrayList GetChildAgencysByFeeItems(List<F4FeeItemModel4WS> allFeeItems);

        /// <summary>
        /// get fee items of child cap belong a parent cap.
        /// </summary>
        /// <param name="allFeeItems">fee items of all child caps in a parent cap</param>
        /// <param name="childCapID">child cap id model</param>
        /// <returns>fee items of a child cap</returns>
        List<F4FeeItemModel4WS> GetChildCapFeeItems(List<F4FeeItemModel4WS> allFeeItems, CapIDModel4WS childCapID);

        /// <summary>
        /// get total fee for a child cap.
        /// </summary>
        /// <param name="allFeeItems">all fee items of a super agency cap,include all Child cap fee items</param>
        /// <param name="childCapID">child cap id model</param>
        /// <returns> child cap total fee </returns>
        double GetChildCapTotalFee(List<F4FeeItemModel4WS> allFeeItems, CapIDModel4WS childCapID);

        /// <summary>
        /// get one child agency caps from the fee items of all child caps in a parent cap .the child caps have fee.
        /// </summary>
        /// <param name="allFeeItems">fee items of all child caps in a parent cap</param>
        /// <param name="childAgency">child cap agency code</param>
        /// <returns>a fee item for a child cap in a agency of a parent cap, the child caps have fee</returns>
        List<F4FeeItemModel4WS> GetChildCapsByAgency(List<F4FeeItemModel4WS> allFeeItems, string childAgency);

        /// <summary>
        /// Gets Contractor Supplied Valuation by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>Contractor supplied valuations</returns>
        BValuatnModel4WS[] GetContractorSuppliedValuation(CapIDModel4WS capID, string callID);

        /// <summary>
        /// Get fee items to be associated with the plans
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <returns>The fee items array of the plan</returns>
        F4FeeItemModel4WS[] GetFeeEstimate4PlanReview(long transactionID);

        /// <summary>
        /// Get fee items to be associated with the plans
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <param name="feeSchedule">Fee Schedule</param>
        /// <returns>The fee items array of the plan</returns>
        F4FeeItemModel4WS[] GetFeeEstimate4PlanReview(long transactionID, string feeSchedule);

        /// <summary>
        /// Gets fee items by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>Array of F4FeeModel4WS</returns>
        F4FeeModel4WS[] GetFeeItemsByCapID(CapIDModel4WS capID, string callID);

        /// <summary>
        /// get fee items of child caps related to parent cap.
        /// </summary>
        /// <param name="capID">parent cap ID</param>
        /// <param name="isIncludeParent">Indicate whether include the parent's FeeItems</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>Array of F4FeeModel4WS</returns>
        F4FeeModel4WS[] GetFeeItemsByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID);

        /// <summary>
        /// Gets NoPaid fee items by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Array of F4FeeModel4WS</returns>
        F4FeeModel4WS[] GetNoPaidFeeItemByCapID(CapIDModel4WS capID, string callerID);

        /// <summary>
        /// Gets paid fee items by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Array of F4FeeModel4WS</returns>
        F4FeeModel4WS[] GetPaidFeeItemByCapID(CapIDModel4WS capID, string callerID);

        /// <summary>
        /// Gets reference fee item array
        /// </summary>
        /// <param name="spCode">agency code</param>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="feeSchedule">fee schedule</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>reference fee item array </returns>
        RFeeItemModel4WS[] GetRefFeeItemList(string spCode, CapIDModel4WS capID, string feeSchedule, QueryFormat4WS qf, string callID);

        /// <summary>
        ///  Self plan checking payment:
        /// Create fee items to be associated with special CAP at first time.
        /// 1. Create fee. 
        /// 2. Create Invoice 
        /// 3. Get total fee.
        /// </summary>
        /// <param name="spCode">SP Code values.</param>
        /// <param name="planSeqNum">plan Sequence Number</param>
        /// <param name="callerId">caller Id  number.</param>
        /// <returns>Self Plan Fee Total.</returns>
        string[] GetSelfPlanFeeTotal(string spCode, long planSeqNum, string callerId);

        /// <summary>
        /// Get total balance fee for existing cap payment
        /// </summary>
        /// <param name="capID">CapIDModel for web service.</param>
        /// <param name="callID">Caller ID recode method invoker.</param>
        /// <returns>Total balance fee</returns>
        double GetTotalBalanceFee(CapIDModel4WS capID, string callID);

        /// <summary>
        /// Gets total fee by cap ID
        /// </summary>
        /// <param name="capID">Cap ID number.</param>
        /// <param name="callID">caller ID number.</param>
        /// <returns>total fee value.</returns>
        double GetTotalFee(CapIDModel4WS capID, string callID);

        /// <summary>
        /// get total fee of child caps related to parent cap
        /// </summary>
        /// <param name="capID">parent cap id </param>
        /// <param name="isIncludeParent">Indicate whether include the parent's Fees</param>
        /// <param name="callID"> caller id number.</param>
        /// <returns> total fee value.</returns>
        double GetTotalFeeByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID);

        /// <summary>
        /// Get total fee of a plan review transaction
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <returns> total fee value.</returns>
        double GetTotalFeeByTransactionID(long transactionID);

        /// <summary>
        /// if some child cap related parent cap have fee item ,return true ,other is false.
        /// </summary>
        /// <param name="capID">parent cap id</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's FeeItems</param>
        /// <param name="callID">caller id number.</param>
        /// <returns><c>true</c> if [has fee item by parent cap unique identifier] [the specified cap unique identifier]; otherwise, <c>false</c>.</returns>
        bool HasFeeItemByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID);

        /// <summary>
        /// indicates job value is enable or not.
        /// </summary>
        /// <param name="capType">the cap type</param>
        /// <param name="callerId">the public use id</param>
        /// <returns>True if given cap has job value</returns>
        bool IsJobValueEnable(CapTypeModel capType, string callerId);

        /// <summary>
        /// if all fee item of child caps related to parent cap are read only ,then true ,other is false;
        /// </summary>
        /// <param name="capID">parent cap id</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's FeeItems</param>
        /// <param name="callID">caller id number.</param>
        /// <returns>Is read only</returns>
        bool IsReadOnlyByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID);

        /// <summary>
        ///  update fee item
        /// </summary>
        /// <param name="arrayF4FeeItemModel">Fee Item Model Array</param>
        /// <param name="capID4WS">Cap ID Model</param>
        void UpdateFeeItems(F4FeeItemModel4WS[] arrayF4FeeItemModel, CapIDModel4WS capID4WS);

        /// <summary>
        /// Validate coupon code.
        /// </summary>
        /// <param name="coponCode">Coupon Code</param>
        /// <returns>Integer of validation code</returns>
        int ValidateCouponCode(string coponCode);

        /// <summary>
        /// if have fee change cap, return fee changed cap id list. 
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        /// <returns>Fee Changed cap id list</returns>
        CapIDModel4WS[] GetFeeChangedCapIdList(CapIDModel4WS[] capIDs);

        /// <summary>
        /// Update Fee Schedule Or Fee Items.
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        void UpdateFeeScheduleOrFeeItems(CapIDModel4WS[] capIDs);

        /// <summary>
        /// If the cap has fee, return true else return false.
        /// </summary>
        /// <param name="capID">The cap id</param>
        /// <param name="isMultiCap">Whether has child caps</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's Fees</param>
        /// <returns><c>true</c> if the specified cap unique identifier has fee; otherwise, <c>false</c>.</returns>
        bool HasFee(CapIDModel4WS capID, bool isMultiCap, bool isIncludeParent);

        /// <summary>
        /// Gets each examination's no paid exam fee items.
        /// </summary>
        /// <param name="exams">the examinations to get it's no paid fee item.</param>
        /// <returns>return the no paid examination fee items</returns>
        F4FeeItemModel4WS[] GetNoPaidExamFeeItemsByExams(ExaminationModel[] exams);

        #endregion Methods
    }
}