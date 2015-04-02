#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PlanStart.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PlanStart.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Plan;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Plan
{
    /// <summary>
    /// the class for PlanStart.
    /// </summary>
    public partial class PlanStart : BasePage
    {
        #region Methods

        /// <summary>
        /// Continue Button Click.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            if (PlanList.PlansSeq == null || PlanList.PlansSeq.Length <= 0)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("planreview_start_noplanuploaded_msg"));
                return;
            }

            IPlanBll planBll = ObjectFactory.GetObject<IPlanBll>();
            long tranSeqNbr = planBll.UpdatePublicUserPlanTransaction(PlanList.PlansSeq);

            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string chargeFees = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_PLANREVIEW_PAY_FEE, "No");

            if (chargeFees.Equals("YES", StringComparison.InvariantCultureIgnoreCase))
            {
                HttpContext.Current.Response.Redirect(string.Format("~/plan/PlanFees.aspx?module={0}&{1}={2}", ModuleName, ACAConstant.REQUEST_PARMETER_TRAN_ID, tranSeqNbr), true);
            }
            else
            {
                // update the plan to the paid status since payments are turned off
                planBll.UpdatePublicUserPlanStatus(tranSeqNbr, "Paid");
                HttpContext.Current.Response.Redirect(string.Format("~/plan/PlanPaySuccess.aspx?module={0}&receiptNbr=0", ModuleName), true);
            }
        }

        #endregion Methods
    }
}
