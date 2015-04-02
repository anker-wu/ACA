#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapConditions.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapConditions.ascx.cs 209458 2011-3-29 17:06:07Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 04-23-2012           Daly zeng               Initial
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation Condition.
    /// </summary>
    public partial class CapConditions : ConditionBaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the label key of the conditions of approval's pattern.
        /// </summary>
        public string ConditionsOfApprovalPatternLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether hiding or displaying condition of approval records which meet requirements
        /// </summary>
        private bool HideMetCondition
        {
            get
            {
                return string.IsNullOrEmpty(btnHideOrShowMet.CommandName) || btnHideOrShowMet.CommandName == BUTTON_COMMAND_SHOW_MET;
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblGeneralConditionsTitle.LabelKey = GeneralConditionsTitleLabelKey;
            lblConditionsOfApprovalTitle.LabelKey = ConditionsOfApprovalTitleLabelKey;

            if (!IsPostBack)
            {
                InitialExport(gdvConditionsOfApprovalList);

                divTitleLocked.Attributes["title"] = GetTextByKey("ACA_CapCondition_Severity_Locked");
                divTitleOnHold.Attributes["title"] = GetTextByKey("ACA_CapCondition_Severity_OnHold");
                divTitleNotice.Attributes["title"] = GetTextByKey("ACA_CapCondition_Severity_Notice");
                divTitleRequired.Attributes["title"] = GetTextByKey("ACA_CapCondition_Severity_Required");

                if (!HideLink4ViewMet)
                {
                    divHideOrShowMet.Visible = true;

                    if (AppSession.IsAdmin)
                    {
                        divHideOrShowMetAdmin.Visible = true;
                    }
                    else
                    {
                        divHideOrShowMetDaily.Visible = true;
                        btnHideOrShowMet.CommandName = BUTTON_COMMAND_SHOW_MET;
                    }
                }

                if (AppSession.IsAdmin)
                {
                    lblGeneralConditionsField.LabelKey = GeneralConditionsPatternLabelKey;
                    lblConditionsOfApprovalField.LabelKey = ConditionsOfApprovalPatternLabelKey;

                    divGeneralConditionsField.Visible = true;
                    divConditionsOfApprovalField.Visible = true;

                    if (ConditionDateUnconfigurable)
                    {
                        phGeneralCondition.Visible = false;
                        phConditionsOfApproval.Visible = false;
                    }

                    gdvGeneralConditionsList.Visible = false;
                    gdvConditionsOfApprovalList.Visible = false;

                    divGeneralConditionsInfo.Visible = true;

                    if (IsDisplayConditionsOfApproval)
                    {
                        divConditionsOfApprovalInfo.Visible = true;
                    }
                }
            }

            if (!HideLink4ViewMet && !AppSession.IsAdmin)
            {
                btnHideOrShowMet.LabelKey = HideMetCondition ? "aca_capcondition_label_showmet" : "aca_capcondition_label_hidemet";
            }
        }

        /// <summary>
        /// Raises the export event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ConditionsOfApprovalList_GridViewDownload(object sender, EventArgs e)
        {
            List<ConditionViewModel> conditionsOfApprovalDataSource = ConditionsOfApprovalDataSource;

            // Only export applied data if the link of [Hide/View Those Met] is show and the those met data is hide.
            if (!HideLink4ViewMet && HideMetCondition)
            {
                conditionsOfApprovalDataSource = conditionsOfApprovalDataSource.Where(c => c.IsApplied).ToList();
            }

            if (conditionsOfApprovalDataSource != null && conditionsOfApprovalDataSource.Count > 0)
            {
                string[] labelKey = { ConditionsOfApprovalPatternLabelKey };
                gdvConditionsOfApprovalList.ExportedContent = BuildExportContent(labelKey, conditionsOfApprovalDataSource);
            }
        }

        /// <summary>
        /// GridView GeneralConditionsList row data bound event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void GeneralConditionsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GeneralConditionsListRowDataBound(e.Row, gdvGeneralConditionsList.PageSize);            
        }

        /// <summary>
        /// GridView ConditionsOfApprovalList row data bound event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void ConditionsOfApprovalList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ConditionsOfApprovalListRowDataBound(e.Row, gdvGeneralConditionsList.PageSize, HideMetCondition);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ConditionViewModel itemData = e.Row.DataItem as ConditionViewModel;

                if (itemData != null)
                {
                    AccelaLabel lblConditionsOfApprovalInfo = (AccelaLabel)e.Row.FindControl("lblConditionsOfApprovalInfo");
                    string conditionsOfApprovalPattern = LabelUtil.GetTextByKey(ConditionsOfApprovalPatternLabelKey, ModuleName);

                    lblConditionsOfApprovalInfo.Text = ReplacePattern(conditionsOfApprovalPattern, itemData, false);
                }
            }
        }

        /// <summary>
        /// Raise the event for hide or show those met.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void HideOrShowMetButton_Click(object sender, EventArgs e)
        {
            if (ConditionsOfApprovalDataSource != null)
            {
                List<ConditionViewModel> approvalDataSource = null;

                if (HideMetCondition)
                {
                    approvalDataSource = ConditionsOfApprovalDataSource;
                    btnHideOrShowMet.LabelKey = "aca_capcondition_label_hidemet";
                    btnHideOrShowMet.CommandName = BUTTON_COMMAND_HIDE_MET;
                }
                else
                {
                    approvalDataSource = ConditionsOfApprovalDataSource.Where(w => w.IsApplied).ToList();

                    btnHideOrShowMet.LabelKey = "aca_capcondition_label_showmet";
                    btnHideOrShowMet.CommandName = BUTTON_COMMAND_SHOW_MET;
                }

                gdvConditionsOfApprovalList.DataSource = approvalDataSource;
                gdvConditionsOfApprovalList.DataBind();
            }

            Page.FocusElement(btnHideOrShowMet.ClientID);
        }

        #endregion

        #region Override Method

        /// <summary>
        /// Bind the condition list
        /// </summary>
        protected override void BindConditionList()
        {
            if (GeneralConditionsDataSource != null && GeneralConditionsDataSource.Count > 0)
            {
                divGeneralConditionsInfo.Visible = true;
                gdvGeneralConditionsList.DataSource = GeneralConditionsDataSource;
                gdvGeneralConditionsList.DataBind();
            }

            // bind the condition of approval, it only shows the applied info by default.
            if (IsDisplayConditionsOfApproval && ConditionsOfApprovalDataSource != null && ConditionsOfApprovalDataSource.Count > 0)
            {
                divConditionsOfApprovalInfo.Visible = true;
                gdvConditionsOfApprovalList.DataSource = ConditionsOfApprovalDataSource.Where(c => c.IsApplied).ToList();
                gdvConditionsOfApprovalList.DataBind();
            }
        }

        /// <summary>
        /// Check whether the condition section display or not.
        /// </summary>
        /// <returns>Is display condition section flag</returns>
        protected override bool IsDisplayConditionSection()
        {
            return divGeneralConditionsInfo.Visible || divConditionsOfApprovalInfo.Visible;
        }

        /// <summary>
        /// Set Condition Bar Label
        /// </summary>
        /// <param name="dictControlLabel">Control Label Dictionary</param>
        protected override void SetConditionBarLabel(Dictionary<string, string> dictControlLabel)
        {
            if (dictControlLabel == null || dictControlLabel.Count == 0)
            {
                return;
            }

            foreach (var item in dictControlLabel)
            {
                if (item.Key.Equals(ACAConstant.LOCK_CONDITION, StringComparison.InvariantCulture))
                {
                    pnlLocked.Visible = true;
                    lblLocked.Text = item.Value;
                }
                else if (item.Key.Equals(ACAConstant.HOLD_CONDITION, StringComparison.InvariantCulture))
                {
                    pnlHold.Visible = true;
                    lblHold.Text = item.Value;
                }
                else if (item.Key.Equals(ACAConstant.NOTICE_CONDITION, StringComparison.InvariantCulture))
                {
                    pnlNotice.Visible = true;
                    lblNotice.Text = item.Value;
                }
                else if (item.Key.Equals(ACAConstant.REQUIRED_CONDITION, StringComparison.InvariantCulture))
                {
                    pnlRequired.Visible = true;
                    lblRequired.Text = item.Value;
                }
            }
        }

        #endregion Override Method
    }
}
