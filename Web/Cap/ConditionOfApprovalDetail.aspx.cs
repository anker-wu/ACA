#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapConditions4CapDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ConditionsOfApprovalDetail.ascx.cs
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 09-25-2013           Ian Chen               Initial
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// condition of approval detail class
    /// </summary>
    public partial class ConditionOfApprovalDetail : PopupDialogBasePage
    {
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_recorddetail_conditionofapprovaldetail_label_title");
            SetDialogMaxHeight("500");

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    lblConditionsOfApprovalField.LabelKey = "aca_recorddetail_conditionofapprovaldetail_pattern";
                    divConditionOfApprovalDetail.Visible = true;
                }
            }

            if (!AppSession.IsAdmin)
            {
                string servProvCode = HttpUtility.UrlDecode(Request.QueryString["agencyCode"]);
                string capId1 = HttpUtility.UrlDecode(Request.QueryString["capId1"]);
                string capId2 = HttpUtility.UrlDecode(Request.QueryString["capId2"]);
                string capId3 = HttpUtility.UrlDecode(Request.QueryString["capId3"]);
                string conditionNbr = HttpUtility.UrlDecode(Request.QueryString["conditionNbr"]);
                CapIDModel capID = new CapIDModel();
                capID.serviceProviderCode = servProvCode;
                capID.ID1 = capId1;
                capID.ID2 = capId2;
                capID.ID3 = capId3;

                IConditionBll conditionBll = ObjectFactory.GetObject<IConditionBll>();
                NoticeConditionModel conditionModel = conditionBll.GetCapConditionApprovalByNbr(capID, conditionNbr);
                List<ConditionViewModel> list = ConditionsUtil.GetConditionViewList(new[] { conditionModel }, true);
                ConditionViewModel item = new ConditionViewModel();

                if (list != null)
                {
                    item = list[0];
                }

                ShowConditionDetail(item);
            }            
        }

        /// <summary>
        /// Show condition detail info
        /// </summary>
        /// <param name="model">The ConditionViewModel</param>
        private void ShowConditionDetail(ConditionViewModel model) 
        {
            string conditionsDetailPattern = LabelUtil.GetTextByKey("aca_recorddetail_conditionofapprovaldetail_pattern", ModuleName);
            lblConditionOfApprovalInfo.Text = ReplacePattern(conditionsDetailPattern, model);
        }

        /// <summary>
        /// Replace the pattern
        /// </summary>
        /// <param name="pattern">Condition detail pattern</param>
        /// <param name="itemData">The ConditionViewModel</param>
        /// <returns>return the display string</returns>
        private string ReplacePattern(string pattern, ConditionViewModel itemData)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return string.Empty;
            }

            string additionalInformation = Server.HtmlDecode(itemData.AdditionalInformation);
            string result = pattern.Replace(ConditionsUtil.ListItemVariables.ConditionName, itemData.ConditionName)
                                   .Replace(ConditionsUtil.ListItemVariables.Status, itemData.Status)
                                   .Replace(ConditionsUtil.ListItemVariables.Severity, itemData.Severity)
                                   .Replace(ConditionsUtil.ListItemVariables.Priority, itemData.PriorityText)
                                   .Replace(ConditionsUtil.ListItemVariables.ShortComments, itemData.ShortComments)
                                   .Replace(ConditionsUtil.ListItemVariables.StatusDate, itemData.StatusDateString)
                                   .Replace(ConditionsUtil.ListItemVariables.AppliedDate, itemData.AppliedDateString)
                                   .Replace(ConditionsUtil.ListItemVariables.EffectiveDate, itemData.EffectiveDateString)
                                   .Replace(ConditionsUtil.ListItemVariables.ExpirationDate, itemData.ExpirationDateString)
                                   .Replace(ConditionsUtil.ListItemVariables.LongComments, itemData.LongComments)
                                   .Replace(ConditionsUtil.ListItemVariables.AppliedByDept, itemData.AppliedByDept)
                                   .Replace(ConditionsUtil.ListItemVariables.AppliedByUser, itemData.AppliedByUser)
                                   .Replace(ConditionsUtil.ListItemVariables.ActionByDept, itemData.ActionByDept)
                                   .Replace(ConditionsUtil.ListItemVariables.ActionByUser, itemData.ActionByUser)
                                   .Replace(ConditionsUtil.ListItemVariables.AdditionalInformation, additionalInformation);
            return result;
        }
    }
}