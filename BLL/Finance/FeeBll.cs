#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FeeBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: FeeBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/10/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// This class provide the ability to operation fee.
    /// </summary>
    public class FeeBll : BaseBll, IFeeBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of FeeService.
        /// </summary>
        private FeeWebServiceService FeeService
        {
            get
            {
                return WSFactory.Instance.GetWebService<FeeWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds fee items
        /// </summary>
        /// <param name="feeItemArray">fee item array</param>
        /// <returns>whether update successfully</returns>
        public bool AddFeeItem(F4FeeItemModel4WS[] feeItemArray)
        {
            try
            {
                return FeeService.addFeeItem(feeItemArray);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Updates and Recalculates fee item by Cap ID
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callID">the public user id.</param>
        /// <param name="quantities">fee item quantity array</param>
        /// <param name="feeIDs">fee ID array</param>
        /// <param name="statuses">fee status array</param>
        /// <param name="feeFactor">fee factor, such as job value.</param>
        /// <returns>whether updated successfully</returns>
        public bool DoRecalculate(CapIDModel4WS capID, string callID, double[] quantities, long[] feeIDs, string[] statuses, double feeFactor)
        {
            try
            {
                string factory = "CONT," + feeFactor;
                return FeeService.doRecalculate(capID, callID, quantities, feeIDs, statuses, factory);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Updates and Recalculates fee item by Cap IDs
        /// </summary>
        /// <param name="calculateParameters">cap fee items with cap id  </param>
        /// <returns>whether updated successfully</returns>
        public bool DoRecalculates(RecalculateParameterModel4WS[] calculateParameters)
        {
            try
            {
                return FeeService.doRecalculates(calculateParameters);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets calculated valuation by cap ID
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>calculated valuation</returns>
        public double GetCalculatedValuation(CapIDModel4WS capID, string callID)
        {
            try
            {
                return FeeService.getCalculatedValuation(capID, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get total fee for a child agency.
        /// </summary>
        /// <param name="allFeeItems">all fee items of a super agency cap,include all child cap fee items</param>
        /// <param name="childAgency">child cap agency code</param>
        /// <returns>child agency total fee</returns>
        public double GetChildAgencyTotalFee(List<F4FeeItemModel4WS> allFeeItems, string childAgency)
        {
            double feeTotal = 0;

            foreach (F4FeeItemModel4WS feeItem in allFeeItems)
            {
                if (string.Equals(feeItem.capID.serviceProviderCode, childAgency))
                {
                    feeTotal += feeItem.fee;
                }
            }

            return feeTotal;
        }

        /// <summary>
        /// get child agencies from the fee items of all child caps in a parent cap .the child agencies have fee.
        /// </summary>
        /// <param name="allFeeItems">fee items of all Child caps in a parent cap</param>
        /// <returns>a fee item for a child agency in a parent cap, the child agencies have fee</returns>
        public ArrayList GetChildAgencysByFeeItems(List<F4FeeItemModel4WS> allFeeItems)
        {
            ArrayList agencys = new ArrayList();

            if (allFeeItems == null || allFeeItems.Count <= 0)
            {
                return agencys;
            }

            SortedList list = new SortedList();

            foreach (F4FeeItemModel4WS feeItem in allFeeItems)
            {
                string tempAgencyCode = feeItem.capID.serviceProviderCode;
                if (!list.ContainsKey(tempAgencyCode))
                {
                    list.Add(tempAgencyCode, feeItem);
                }
            }

            foreach (DictionaryEntry de in list)
            {
                agencys.Add(de.Value);
            }

            return agencys;
        }

        /// <summary>
        /// get fee items of child cap belong a parent cap.
        /// </summary>
        /// <param name="allFeeItems">fee items of all child caps in a parent cap</param>
        /// <param name="childCapID">child cap id model</param>
        /// <returns>fee items of a child cap</returns>
        public List<F4FeeItemModel4WS> GetChildCapFeeItems(List<F4FeeItemModel4WS> allFeeItems, CapIDModel4WS childCapID)
        {
            List<F4FeeItemModel4WS> feeItems = new List<F4FeeItemModel4WS>();

            if (allFeeItems == null || allFeeItems.Count <= 0)
            {
                return feeItems;
            }

            foreach (F4FeeItemModel4WS feeItem in allFeeItems)
            {
                if (feeItem.capID == null || (feeItem.feeSeqNbr <= 0 && string.IsNullOrEmpty(feeItem.formula)))
                {
                    continue;
                }

                if (IsSameCap(feeItem.capID, childCapID))
                {
                    feeItems.Add(feeItem);
                }
            }

            return feeItems;
        }

        /// <summary>
        /// get total fee for a child cap.
        /// </summary>
        /// <param name="allFeeItems">all fee items of a super agency cap,include all Child cap fee items</param>
        /// <param name="childCapID">child cap id model</param>
        /// <returns>child cap total fee</returns>
        public double GetChildCapTotalFee(List<F4FeeItemModel4WS> allFeeItems, CapIDModel4WS childCapID)
        {
            double feeTotal = 0;

            foreach (F4FeeItemModel4WS feeItem in allFeeItems)
            {
                if (IsSameCap(feeItem.capID, childCapID))
                {
                    feeTotal += feeItem.fee;
                }
            }

            return feeTotal;
        }

        /// <summary>
        /// get one child agency caps from the fee items of all child caps in a parent cap .the child caps have fee.
        /// </summary>
        /// <param name="allFeeItems">fee items of all child caps in a parent cap</param>
        /// <param name="childAgency">child cap agency code</param>
        /// <returns>a fee item for a child cap in a agency of a parent cap, the child caps have fee</returns>
        public List<F4FeeItemModel4WS> GetChildCapsByAgency(List<F4FeeItemModel4WS> allFeeItems, string childAgency)
        {
            List<F4FeeItemModel4WS> caps = new List<F4FeeItemModel4WS>();

            if (allFeeItems == null || allFeeItems.Count <= 0)
            {
                return caps;
            }

            foreach (F4FeeItemModel4WS feeItem in allFeeItems)
            {
                if (feeItem.capID == null)
                {
                    continue;
                }

                CapIDModel4WS tempCapModel = feeItem.capID;

                string tempAgencyCode = tempCapModel.serviceProviderCode;

                if (tempAgencyCode.CompareTo(childAgency) != 0)
                {
                    continue;
                }

                bool isExist = false;

                foreach (F4FeeItemModel4WS innerFeeItem in caps)
                {
                    CapIDModel4WS addedCapModel = innerFeeItem.capID;

                    if (IsSameCap(addedCapModel, tempCapModel))
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    caps.Add(feeItem);
                }
            }

            return caps;
        }

        /// <summary>
        /// Gets Contractor Supplied Valuation by cap ID
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>Contractor supplied valuations</returns>
        public BValuatnModel4WS[] GetContractorSuppliedValuation(CapIDModel4WS capID, string callID)
        {
            try
            {
                return FeeService.getContractorSuppliedValuation(capID, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get fee items to be associated with the plans
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <returns>The fee items array of the plan</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public F4FeeItemModel4WS[] GetFeeEstimate4PlanReview(long transactionID)
        {
            try
            {
                return FeeService.getFeeEstimate4PlanReview(AgencyCode, transactionID, null, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get fee items to be associated with the plans
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <param name="feeSchedule">Fee Schedule</param>
        /// <returns>The fee items array of the plan</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public F4FeeItemModel4WS[] GetFeeEstimate4PlanReview(long transactionID, string feeSchedule)
        {
            try
            {
                return FeeService.getFeeEstimate4PlanReview(AgencyCode, transactionID, feeSchedule, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets fee items by cap ID
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>F4FeeModel array</returns>
        public F4FeeModel4WS[] GetFeeItemsByCapID(CapIDModel4WS capID, string callID)
        {
            try
            {
                return FeeService.getFeeItemsByCapID(capID, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get fee items of child caps related to parent cap.
        /// </summary>
        /// <param name="capID">parent cap ID</param>
        /// <param name="isIncludeParent">Indicate whether include the parent's FeeItems</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>a F4FeeModel</returns>
        public F4FeeModel4WS[] GetFeeItemsByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID)
        {
            try
            {
                return FeeService.getFeeItemsByParentCapID(capID, isIncludeParent, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets no paid fee items by cap ID
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>F4FeeModel array</returns>
        public F4FeeModel4WS[] GetNoPaidFeeItemByCapID(CapIDModel4WS capID, string callID)
        {
            try
            {
                return FeeService.getNoPaidFeeItemByCapID(capID, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets paid fee items by cap ID
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>F4FeeModel array</returns>
        public F4FeeModel4WS[] GetPaidFeeItemByCapID(CapIDModel4WS capID, string callID)
        {
            try
            {
                return FeeService.getPaidFeeItemByCapID(capID, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
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
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RFeeItemModel4WS[] GetRefFeeItemList(string spCode, CapIDModel4WS capID, string feeSchedule, QueryFormat4WS qf, string callID)
        {
            try
            {
                return FeeService.getRefFeeItemList(spCode, capID, feeSchedule, qf, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
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
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string[] GetSelfPlanFeeTotal(string spCode, long planSeqNum, string callerId)
        {
            try
            {
                return FeeService.getSelfPlanFeeTotal(spCode, planSeqNum, callerId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get total balance fee for existing cap payment
        /// </summary>
        /// <param name="capID">CapIDModel for web service.</param>
        /// <param name="callID">Caller ID recode method invoker.</param>
        /// <returns>Total balance fee</returns>
        public double GetTotalBalanceFee(CapIDModel4WS capID, string callID)
        {
            try
            {
                return FeeService.getTotalBalanceFee(capID, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets total fee by cap ID
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>total fees.</returns>
        public double GetTotalFee(CapIDModel4WS capID, string callID)
        {
            try
            {
                return FeeService.getTotalFee(capID, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get total fee of child caps related to parent cap
        /// </summary>
        /// <param name="capID">parent cap id </param>
        /// <param name="isIncludeParent">Indicate whether include the parent's Fees</param>
        /// <param name="callID">the public user id</param>
        /// <returns> total fees. </returns>
        public double GetTotalFeeByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID)
        {
            try
            {
                return FeeService.getTotalFeeByParentCapID(capID, isIncludeParent, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get total fee of a plan review transaction
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <returns> total fees. </returns>
        public double GetTotalFeeByTransactionID(long transactionID)
        {
            try
            {
                return FeeService.getFeeTotalByPosTransaction(AgencyCode, transactionID, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// if some child cap related parent cap have fee item ,return true ,other is false.
        /// </summary>
        /// <param name="capID">parent cap id</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's FeeItems</param>
        /// <param name="callID">caller id number.</param>
        /// <returns><c>true</c> if [has fee item by parent cap unique identifier] [the specified cap unique identifier]; otherwise, <c>false</c>.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool HasFeeItemByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID)
        {
            try
            {
                return FeeService.hasFeeItemByParentCapID(capID, isIncludeParent, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// indicates job value is enable or not.
        /// </summary>
        /// <param name="capType">the cap type</param>
        /// <param name="callerId">the public use id</param>
        /// <returns>True if given cap has job value</returns>
        public bool IsJobValueEnable(CapTypeModel capType, string callerId)
        {
            IPageflowBll pageFlowBll = ObjectFactory.GetObject<IPageflowBll>();
            PageFlowGroupModel pfw = pageFlowBll.GetPageflowGroupByCapType(capType);

            if (pfw == null)
            {
                return false;
            }

            foreach (StepModel pStep in pfw.stepList)
            {
                if (pStep == null)
                {
                    break;
                }

                foreach (PageModel onePage in pStep.pageList)
                {
                    if (onePage == null)
                    {
                        break;
                    }

                    foreach (ComponentModel cws in onePage.componentList)
                    {
                        if (!string.IsNullOrEmpty(cws.componentName) && cws.componentName.ToUpperInvariant() == GViewConstant.SECTION_ADDITIONAL_INFO)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// if all fee item of child caps related to parent cap are read only ,then true ,other is false;
        /// </summary>
        /// <param name="capID">parent cap id</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's FeeItems</param>
        /// <param name="callID">caller id number.</param>
        /// <returns>Is read only</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool IsReadOnlyByParentCapID(CapIDModel4WS capID, bool isIncludeParent, string callID)
        {
            try
            {
                return FeeService.isReadOnlyByParentCapID(capID, isIncludeParent, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        ///  update fee item
        /// </summary>
        /// <param name="arrayF4FeeItemModel4WS">Fee Item Model Array</param>
        /// <param name="capID4WS">Cap ID Model</param>
        public void UpdateFeeItems(F4FeeItemModel4WS[] arrayF4FeeItemModel4WS, CapIDModel4WS capID4WS)
        {
            try
            {
                FeeService.updateFeeItems(arrayF4FeeItemModel4WS, capID4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Validate coupon code.
        /// </summary>
        /// <param name="coponCode">Coupon Code</param>
        /// <returns>Integer of validation code</returns>
        public int ValidateCouponCode(string coponCode)
        {
            try
            {
                return FeeService.validateCouponCode(AgencyCode, coponCode, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// if have fee change cap, return changed cap id list.
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        /// <returns>true or false</returns>
        public CapIDModel4WS[] GetFeeChangedCapIdList(CapIDModel4WS[] capIDs)
        {
            try
            {
                return FeeService.getFeeChangedCapIdList(capIDs, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update Fee Schedule Or Fee Items.
        /// </summary>
        /// <param name="capIDs">cap id list</param>
        public void UpdateFeeScheduleOrFeeItems(CapIDModel4WS[] capIDs)
        {
            try
            {
                FeeService.updateFeeScheduleOrFeeItems(capIDs, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// If the cap has fee, return true else return false. 
        /// </summary>
        /// <param name="capID">The cap id</param>
        /// <param name="isMultiCap">Whether has child caps</param>
        /// <param name="isIncludeParent">Indicate whether judge the parent's Fees</param>
        /// <returns>true or false</returns>
        public bool HasFee(CapIDModel4WS capID, bool isMultiCap, bool isIncludeParent)
        {
            try
            {
                return FeeService.hasFee(capID, isMultiCap, isIncludeParent, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets each examination's no paid exam fee items.
        /// </summary>
        /// <param name="exams">the examinations to get it's no paid fee item.</param>
        /// <returns>return the no paid examination fee items</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public F4FeeItemModel4WS[] GetNoPaidExamFeeItemsByExams(ExaminationModel[] exams)
        {
            try
            {
                return FeeService.getNoPaidExamFeeItemsByExams(exams);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// judge if the two caps are same.
        /// </summary>
        /// <param name="oneCap">cap id model.</param>
        /// <param name="anotherCap">cap id model</param>
        /// <returns>true or false.</returns>
        private bool IsSameCap(CapIDModel4WS oneCap, CapIDModel4WS anotherCap)
        {
            return string.Equals(oneCap.serviceProviderCode, anotherCap.serviceProviderCode) 
                && string.Equals(oneCap.id1, anotherCap.id1)
                && string.Equals(oneCap.id2, anotherCap.id2)
                && string.Equals(oneCap.id3, anotherCap.id3);
        }

        #endregion Methods
    }
}