#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PlanFeeItem.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PlanFeeItem.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation PlanFeeItem.
    /// </summary>
    public partial class PlanFeeItem : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets fee items.
        /// </summary>
        public ArrayList FeeItems
        {
            get
            {
                return ViewState["FEE_ITEMS"] as ArrayList;
            }

            set
            {
                ViewState["FEE_ITEMS"] = value;
            }
        }

        /// <summary>
        /// Gets or sets plan name.
        /// </summary>
        public string PlanName
        {
            get
            {
                return ViewState["PLAN_NAME"] as string;
            }

            set
            {
                ViewState["PLAN_NAME"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind Data Source
        /// </summary>
        public void BindDataSource()
        {
            lblPlanName.Text = PlanName;
            lblPlanTotalFee.Text = I18nNumberUtil.FormatMoneyForUI(GetTotalFee());
            feeItemList.DataSource = FeeItems;
            feeItemList.DataBind();
        }

        /// <summary>
        /// Bind Data Source
        /// </summary>
        /// <param name="planName">the plan name.</param>
        /// <param name="feeItems">the feel items.</param>
        public void BindDataSource(string planName, ArrayList feeItems)
        {
            PlanName = planName;
            FeeItems = feeItems;
            BindDataSource();
        }

        /// <summary>
        /// Get fee description
        /// </summary>
        /// <param name="objResFeeDescription">object result fee description</param>
        /// <param name="objFeeDescription">object fee description</param>
        /// <returns>fee description</returns>
        protected string GetFeeDescription(object objResFeeDescription, object objFeeDescription)
        {
            string resFeeDescription = (string)objResFeeDescription;
            string feeDescription = (string)objFeeDescription;
            return I18nStringUtil.GetString(resFeeDescription, feeDescription);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDataSource();
        }

        /// <summary>
        /// feeList itemDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DataListItemEventArgs e</param>
        protected void FeeList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            int index = e.Item.ItemIndex;
            string cssClass = string.Empty;

            if (StandardChoiceUtil.IsSuperAgency())
            {
                cssClass = index % 2 == 0 ? "ACA_TabRow_SmallOdd ACA_TabRow_SmallOdd_FontSize" : "ACA_TabRow_SmallEven2 font11px";
            }
            else
            {
                cssClass = index % 2 == 0 ? "ACA_TabRow_SmallEven ACA_TabRow_SmallEven_FontSize" : "ACA_TabRow_SmallOdd ACA_TabRow_SmallOdd_FontSize";
            }

            e.Item.CssClass = cssClass;

            //control the indentation of fee item list.
            HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("flexibleTD");

            if (StandardChoiceUtil.IsSuperAgency())
            {
                td.Visible = true;
            }
            else
            {
                td.Visible = false;
            }

            Label lblQty = (Label)e.Item.FindControl("lblQuantity");

            lblQty.Visible = true;
        }

        /// <summary>
        /// Get total fee
        /// </summary>
        /// <returns>double for total fee.</returns>
        private double GetTotalFee()
        {
            double total = 0;

            if (FeeItems != null)
            {
                foreach (F4FeeItemModel4WS feeItem in FeeItems)
                {
                    total += feeItem.fee;
                }
            }

            return total;
        }

        #endregion Methods
    }
}