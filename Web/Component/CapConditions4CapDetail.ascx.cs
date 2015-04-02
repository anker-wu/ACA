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
 *      $Id: CapConditions4CapDetail.ascx.cs
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 09-25-2013           Ian Chen               Initial
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
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation Condition.
    /// </summary>
    public partial class CapConditions4CapDetail : ConditionBaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the label key of the pending status conditions of approval's pattern.
        /// </summary>
        public string PendingConditionsOfApprovalPatternLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key of the met status conditions of approval's pattern.
        /// </summary>
        public string MetConditionsOfApprovalPatternLabelKey
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

        #endregion Properties

        #region Event

        /// <summary>
        /// Initial event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            shConditionOfApproval.SectionBodyClientID = divConditionsOfApproval.ClientID;
            shConditionOfApproval.SearchEvent += SearchConditionOfApprovalButton_OnClick;

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            shGeneralConditionsInfo.TitleLabelKey = GeneralConditionsTitleLabelKey;
            shConditionOfApproval.TitleLabelKey = ConditionsOfApprovalTitleLabelKey;

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
                        btnHideOrShowMet.CommandName = BUTTON_COMMAND_HIDE_MET;
                    }
                }

                if (AppSession.IsAdmin)
                {
                    lblGeneralConditionsField.LabelKey = GeneralConditionsPatternLabelKey;
                    lblPendingConditionsOfApprovalField.LabelKey = PendingConditionsOfApprovalPatternLabelKey;
                    lblMetConditionsOfApprovalField.LabelKey = MetConditionsOfApprovalPatternLabelKey;

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
            else
            {
                // Display contents and expand the section if it is post back, such as Prev/Next was clicked in paging case.
                shConditionOfApproval.Collapsed = false;
                shConditionOfApproval.ShowInstruction = true;
                divConditionsOfApproval.Attributes["class"] = "ACA_Page";
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

            string[] labelKey = { PendingConditionsOfApprovalPatternLabelKey, MetConditionsOfApprovalPatternLabelKey };
            if (conditionsOfApprovalDataSource != null && conditionsOfApprovalDataSource.Count > 0)
            {
                conditionsOfApprovalDataSource = FilterDownloadDataSource(PendingConditionsOfApprovalPatternLabelKey, MetConditionsOfApprovalPatternLabelKey, conditionsOfApprovalDataSource);
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
                    AccelaLabel lblPendingConditionsOfApprovalInfo = (AccelaLabel)e.Row.FindControl("lblConditionsOfApprovalInfo");
                    string pendingConditionsOfApprovalPattern = LabelUtil.GetTextByKey(PendingConditionsOfApprovalPatternLabelKey, ModuleName);
                    string metConditionsOfApprovalPattern = LabelUtil.GetTextByKey(MetConditionsOfApprovalPatternLabelKey, ModuleName);

                    if (!itemData.IsApplied)
                    {
                        lblPendingConditionsOfApprovalInfo.Text = ReplacePattern(metConditionsOfApprovalPattern, itemData, false, true);
                    }
                    else
                    {
                        lblPendingConditionsOfApprovalInfo.Text = ReplacePattern(pendingConditionsOfApprovalPattern, itemData, false, true);
                    }
                }
            }
        }   

        /// <summary>
        /// Search condition of approval
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ExpandEventArgs e</param>
        protected void SearchConditionOfApprovalButton_OnClick(object sender, CommonEventArgs e)
        {
            gdvConditionsOfApprovalList.PageIndex = 0;
            gdvConditionsOfApprovalList.DataSource = null;
            gdvConditionsOfApprovalList.DataBind();
            string searchStr = e.ArgObject as string;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capModel.capID, AppSession.User.UserSeqNum, true, null);
            List<ConditionViewModel> result = new List<ConditionViewModel>();
            List<ConditionViewModel> conditions = ConditionsUtil.GetConditionViewList(capWithConditionModel.conditionModelArray, true);
            string searchLabel = GetTextByKey("aca_sectionsearch_label_search");
            searchLabel = LabelUtil.RemoveHtmlFormat(searchLabel).Replace("'", "&#39;").Replace("\"", "&quot;");

            if (string.IsNullOrEmpty(searchStr) || string.Equals(searchStr, searchLabel, StringComparison.InvariantCultureIgnoreCase))
            {
                result = conditions;
            }
            else
            {
                foreach (ConditionViewModel con in conditions)
                {
                    string compareText = string.Format(
                        "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}",
                        ACAConstant.SPLIT_CHAR, 
                        con.GroupName,
                        con.RecordType,
                        con.ConditionType,
                        con.ConditionName,
                        con.Status,
                        con.Severity,
                        con.StatusDateString,
                        con.AppliedDateString,
                        con.PriorityText,
                        con.ActionByUser,
                        con.ActionByDept,
                        con.AppliedByUser,
                        con.AppliedByDept,
                        con.LongComments,
                        con.ShortComments,
                        con.AdditionalInformation);

                    if (compareText.IndexOf(searchStr, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        result.Add(con);
                    }
                }
            }

            if (result.Count > 0)
            {
                NoRecordFound.Visible = false;
                divHideOrShowMetDaily.Visible = true;
            }
            else
            {
                NoRecordFound.Visible = true;
                divHideOrShowMetDaily.Visible = false;
            }

            divConditionsOfApproval.Attributes["class"] = "ACA_Page";
            btnHideOrShowMet.CommandName = BUTTON_COMMAND_HIDE_MET;
            btnHideOrShowMet.LabelKey = HideMetCondition ? "aca_capcondition_label_showmet" : "aca_capcondition_label_hidemet";
            Page.FocusElement(btnHideOrShowMet.ClientID);

            ConditionsOfApprovalDataSource = result;
            gdvConditionsOfApprovalList.DataSource = ConditionsOfApprovalDataSource;
            gdvConditionsOfApprovalList.DataBind();
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

            divConditionsOfApproval.Attributes["class"] = "ACA_Page";
            Page.FocusElement(btnHideOrShowMet.ClientID);
        }

        #endregion

        #region Override Method

        /// <summary>
        /// Bind the condition list
        /// </summary>
        protected override void BindConditionList()
        {
            //Do not display the conditions if all of them are not applied
            bool isAllNotApplied = true;
            foreach (ConditionViewModel condition in GeneralConditionsDataSource)
            {
                if (condition.IsApplied && !string.IsNullOrEmpty(condition.Severity)) 
                {
                    isAllNotApplied = false;
                    break;
                }
            }

            if (GeneralConditionsDataSource != null && GeneralConditionsDataSource.Count > 0 && !isAllNotApplied)
            {
                divGeneralConditionsInfo.Visible = true;
                gdvGeneralConditionsList.DataSource = GeneralConditionsDataSource;
                gdvGeneralConditionsList.DataBind();
            }

            if (IsDisplayConditionsOfApproval && ConditionsOfApprovalDataSource != null && ConditionsOfApprovalDataSource.Count > 0)
            {
                divConditionsOfApprovalInfo.Visible = true;
                bool hasPendingCondition = false;

                foreach (ConditionViewModel condition in ConditionsOfApprovalDataSource)
                {
                    if (condition.IsApplied && !string.IsNullOrEmpty(condition.Severity))
                    {
                        hasPendingCondition = true;
                        break;
                    }
                }

                if (!hasPendingCondition && !IsPostBack)
                {
                    shConditionOfApproval.Collapsed = true;
                    shConditionOfApproval.ShowInstruction = false;
                    divConditionsOfApproval.Attributes["class"] = "ACA_Hide";
                }

                btnHideOrShowMet.CommandName = BUTTON_COMMAND_HIDE_MET;
                gdvConditionsOfApprovalList.DataSource = ConditionsOfApprovalDataSource;
                gdvConditionsOfApprovalList.DataBind();
            }
        }

        /// <summary>
        /// Is the condition section display or not;
        /// </summary>
        /// <returns>return true or false</returns>
        protected override bool IsDisplayConditionSection()
        {
            return divGeneralConditionsInfo.Visible || divConditionsOfApprovalInfo.Visible;
        }

        /// <summary>
        /// Set the control label display with the label dictionary
        /// </summary>
        /// <param name="dictControlLabel">Label dictionary to be set display</param>
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

        #region private Method     

        /// <summary>
        /// Filter the download data of condition source
        /// </summary>
        /// <param name="pendingConditionLabelPattern">1.filter by the pending status label pattern</param>
        /// <param name="metConditionLabelPattern">2.filter by the met status label pattern</param>
        /// <param name="conditions">All conditions ready to be filter</param>
        /// <returns>return the filtered results</returns>
        private List<ConditionViewModel> FilterDownloadDataSource(string pendingConditionLabelPattern, string metConditionLabelPattern, List<ConditionViewModel> conditions)
        {
            string pendingPattern = LabelUtil.GetTextByKey(pendingConditionLabelPattern, ModuleName);
            string metPattern = LabelUtil.GetTextByKey(metConditionLabelPattern, ModuleName);
            List<ConditionViewModel> results = new List<ConditionViewModel>();

            foreach (ConditionViewModel condition in conditions)
            {
                if (!condition.IsApplied)
                {
                    results.Add(FilterConditionData(metPattern, condition));
                }
                else
                {
                    results.Add(FilterConditionData(pendingPattern, condition));
                }
            }

            return results;
        }

        /// <summary>
        /// Filter the condition data to download. If no field configured in label pattern then can not be download the data.
        /// So clear it.
        /// </summary>
        /// <param name="pattern">The filter label pattern</param>
        /// <param name="condition">The filter condition</param>
        /// <returns>Return the filtered condition</returns>
        private ConditionViewModel FilterConditionData(string pattern, ConditionViewModel condition)
        {
            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.ConditionName, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.ConditionName = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.Status, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.Status = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.Severity, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.Severity = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.Priority, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.PriorityText = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.ShortComments, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.ShortComments = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.StatusDate, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.StatusDateString = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.AppliedDate, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.AppliedDateString = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.EffectiveDate, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.EffectiveDateString = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.ExpirationDate, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.ExpirationDateString = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.LongComments, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.LongComments = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.AppliedByDept, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.AppliedByDept = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.AppliedByUser, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.AppliedByUser = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.ActionByDept, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.ActionByDept = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.ActionByUser, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.ActionByUser = string.Empty;
            }

            if (pattern.IndexOf(ConditionsUtil.ListItemVariables.AdditionalInformation, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                condition.AdditionalInformation = string.Empty;
            }

            return condition;
        }
        #endregion private Method
    }
}
