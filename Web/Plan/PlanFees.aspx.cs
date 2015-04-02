#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapFees.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PlanFees.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.Plan;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Plan
{
    /// <summary>
    /// Cap fee display and recalculation.
    /// </summary>
    /// <remark>
    /// this page comes from below:
    /// 1.Apply a permit
    /// 2.Obtain a Fee Estimate
    /// 3.Search permits.
    /// input parameter(required): the paramters are used to create CapIdModel.
    /// 1.capId1
    /// 2.capId2
    /// 3.capId3
    /// input parameter(optional): 
    /// 1.IsResume. this parameter indicats the current cap whether is a resume cap or not.
    /// </remark>
    public partial class PlanFees : BasePage
    {
        #region Fields

        /// <summary>
        /// the tran ID.
        /// </summary>
        private long tranID;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Get Fee Description.
        /// </summary>
        /// <param name="objResFeeDescription">object for result fee description</param>
        /// <param name="objFeeDescription">object for fee description</param>
        /// <returns>Fee Description</returns>
        protected string GetFeeDescription(object objResFeeDescription, object objFeeDescription)
        {
            string resFeeDescription = (string)objResFeeDescription;
            string feeDescription = (string)objFeeDescription;

            return I18nStringUtil.GetString(resFeeDescription, feeDescription);
        }

        /// <summary>
        /// To load and initial page data.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetTransactionID();

            if (tranID == -1)
            {
                return;
            }

            if (!IsPostBack)
            {
                ShowFeeInformation();
            }

            txtCouponCode.ToolTip = GetTextByKey("planreview_planfees_label_couponcode");
        }

        /// <summary>
        /// Recalculate the fee amount when user applied a coupon.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ApplyCouponButton_Click(object sender, EventArgs e)
        {
            // Applying coupon, need to show fee items to UI again.
            txtCouponCode.Text = txtCouponCode.Text.Trim();

            string couponCode = txtCouponCode.Text.ToUpperInvariant();

            if (string.IsNullOrEmpty(couponCode))
            {
                return;
            }

            // get fee total
            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();

            int validationCode = feeBll.ValidateCouponCode(couponCode);

            if (validationCode <= 0)
            {
                ShowExpiredCouponMessage();

                return;
            }

            ShowFeeInformation(couponCode);

            txtCouponCode.Text = string.Empty;
        }

        /// <summary>
        /// Continue Application to payment page. 
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContinueApplicationButton_Click(object sender, EventArgs e)
        {
            // get fee total
            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();

            double feeAmount = 0.0;

            feeAmount = feeBll.GetTotalFeeByTransactionID(tranID);

            // if there is fee to be paid, go to payment page.
            if (feeAmount > 0)
            {
                HttpContext.Current.Response.Redirect(string.Format("~/plan/PlanPay.aspx?module={0}&{1}={2}", ModuleName, ACAConstant.REQUEST_PARMETER_TRAN_ID, tranID), true);
            }
            else
            {
                // if no fee to be paid, redirect it to successful page. update the plan to the paid status since payments are turned off
                IPlanBll planBll = ObjectFactory.GetObject<IPlanBll>();
                planBll.UpdatePublicUserPlanStatus(tranID, "Paid");
                HttpContext.Current.Response.Redirect(string.Format("~/plan/PlanPaySuccess.aspx?module={0}&receiptNbr=0", ModuleName), true);
            }
        }

        /// <summary>
        /// PlanList Item Data Bound
        /// </summary>
        /// <param name="sender">object sender,</param>
        /// <param name="e">DataListItemEventArgs e</param>
        protected void PlanList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DictionaryEntry de = (DictionaryEntry)e.Item.DataItem;

            string planName = ((PublicUserPlanModel4WS)de.Key).planName;
            ArrayList feeItems = (ArrayList)de.Value;

            PlanFeeItem pfi = (PlanFeeItem)e.Item.FindControl("PlanFeeItem");
            pfi.BindDataSource(planName, feeItems);
        }

        /// <summary>
        /// Bind plan fees
        /// </summary>
        private void BindPlanFees()
        {
            planList.DataSource = GetFeeEstimate();
            planList.DataBind();
        }

        /// <summary>
        /// Get fee estimate of plan check.
        /// </summary>
        /// <returns>Hashtable with key plan, value fee item list</returns>
        private Hashtable GetFeeEstimate()
        {
            return GetFeeEstimate(null);
        }

        /// <summary>
        /// Get fee estimate of plan check.
        /// </summary>
        /// <param name="feeSchedule">Fee Schedule</param>
        /// <returns>Hashtable with key plan, value fee item list</returns>
        private Hashtable GetFeeEstimate(string feeSchedule)
        {
            IPlanBll planBll = ObjectFactory.GetObject<IPlanBll>();
            Array plans = planBll.GetPublicUserPlansByTransactionID(tranID);

            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();
            F4FeeItemModel4WS[] feeItems = feeBll.GetFeeEstimate4PlanReview(tranID, feeSchedule);

            Hashtable ht = GroupFeeItems(plans, feeItems);

            return ht;
        }

        /// <summary>
        /// Group fee items by plan.
        /// </summary>
        /// <param name="plans">Plan array</param>
        /// <param name="feeItems">Fee items array</param>
        /// <returns>Hashtable with key plan, value fee item list</returns>
        private Hashtable GroupFeeItems(Array plans, F4FeeItemModel4WS[] feeItems)
        {
            Hashtable ht = new Hashtable();

            if (plans == null || plans.Length == 0)
            {
                return ht;
            }

            ArrayList list;

            foreach (PublicUserPlanModel4WS plan in plans)
            {
                list = new ArrayList();

                if (feeItems != null)
                {
                    foreach (F4FeeItemModel4WS feeItem in feeItems)
                    {
                        if (feeItem.udf2 == plan.planSeqNbr.ToString())
                        {
                            list.Add(feeItem);
                        }
                    }
                }

                ht.Add(plan, list);
            }

            return ht;
        }

        /// <summary>
        /// Set Transaction ID.
        /// </summary>
        private void SetTransactionID()
        {
            tranID = 0;

            string tmpTran = Request.QueryString[ACAConstant.REQUEST_PARMETER_TRAN_ID];

            if (!string.IsNullOrEmpty(tmpTran))
            {
                try
                {
                    tranID = long.Parse(tmpTran);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Show the invalid coupon message to UI.
        /// </summary>
        private void ShowExpiredCouponMessage()
        {
            string msg = GetTextByKey("planreview_planfees_expiredcouponcode_msg");
            MessageUtil.ShowMessage(Page, MessageType.Error, msg);
        }

        /// <summary>
        /// Show all of fee information to UI
        /// </summary>
        private void ShowFeeInformation()
        {
            BindPlanFees(); //BindFeeItems();
            ShowFeeTotal();
        }

        /// <summary>
        /// Show all of fee information to UI
        /// </summary>
        /// <param name="couponCode">coupon Code.</param>
        private void ShowFeeInformation(string couponCode)
        {
            planList.DataSource = GetFeeEstimate(couponCode);
            planList.DataBind();

            ShowFeeTotal();
        }

        /// <summary>
        /// Gets fee total and show it to UI.
        /// </summary>
        private void ShowFeeTotal()
        {
            // show fee total
            double feeAmount = 0;
            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();

            feeAmount = feeBll.GetTotalFeeByTransactionID(tranID);

            //lblFeeAmount.Text = String.Format("{0:C}", feeAmount);
            lblFeeAmount.Text = I18nNumberUtil.FormatMoneyForUI(feeAmount);
        }

        #endregion Methods
    }
}
