#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdditionalInformationDetail.aspx
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AdditionalInformationDetail.aspx
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 02-12-2014        Canon Wu        Initial
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// Additional Information Detail Class
    /// </summary>
    public partial class ConditionAdditionalInfoDetail : PopupDialogBasePage
    {
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_condition_label_additionalinfo_title");
            SetDialogMaxHeight("500");

            if (!Page.IsPostBack)
            {
                Dictionary<string, string> additionalInfoList = AppSession.GetConditionAdditionalInfoFromSession();
                string servProvCode = HttpUtility.UrlDecode(Request.QueryString["agencyCode"]);
                string conditionNbr = HttpUtility.UrlDecode(Request.QueryString["conditionNbr"]);
                string conditionKey = string.Format("{0}_{1}", servProvCode, conditionNbr);

                if (additionalInfoList != null)
                {
                    lblAdditionalInformationDetail.Text = additionalInfoList[conditionKey];
                }
            }
        }
    }
}