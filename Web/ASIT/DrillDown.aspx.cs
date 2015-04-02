#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DrillDown.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DrillDown.ascx.cs 195301 2011-06-29 09:35:35Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using Newtonsoft.Json;

namespace Accela.ACA.Web.ASIT
{
    /// <summary>
    /// Drill down pop-up page
    /// </summary>
    public partial class DrillDown : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// page rows count
        /// </summary>
        private const int PAGE_ROWS_COUNT = 10;

        /// <summary>
        /// whether or not need hidden the field in Single Section model.
        /// </summary>
        private bool needHiddenSingleSectionField = false;

        /// <summary>
        /// Array of the ASIT UI data keys.
        /// </summary>
        private string[] asitUIDataKeys;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is single section.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is single section; otherwise, <c>false</c>.
        /// </value>
        protected bool IsSingleSection
        {
            get
            {
                if (ViewState["isSingleSection"] == null)
                {
                    ViewState["isSingleSection"] = true;
                }

                return (bool)ViewState["isSingleSection"];
            }

            set
            {
                ViewState["isSingleSection"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the asit field I18n name and I18n label
        /// </summary>
        private Dictionary<string, string> AsitFieldsLabel
        {
            get
            {
                return ViewState["AsitFieldsLabel"] as Dictionary<string, string>;
            }

            set
            {
                ViewState["AsitFieldsLabel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is single section on last step.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is single section on last step; otherwise, <c>false</c>.
        /// </value>
        private bool IsSingleSectionOnLastStep
        {
            get
            {
                if (ViewState["IsSingleSectionOnLastStep"] == null)
                {
                    ViewState["IsSingleSectionOnLastStep"] = false;
                }

                return (bool)ViewState["IsSingleSectionOnLastStep"];
            }

            set
            {
                ViewState["IsSingleSectionOnLastStep"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is search table running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is search table running; otherwise, <c>false</c>.
        /// </value>
        private bool IsSearchTableRuning
        {
            get
            {
                if (ViewState["IsSearchTableRuning"] == null)
                {
                    ViewState["IsSearchTableRuning"] = false;
                }

                return (bool)ViewState["IsSearchTableRuning"];
            }

            set
            {
                ViewState["IsSearchTableRuning"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the agency code.
        /// </summary>
        /// <value>
        /// The agency code.
        /// </value>
        private string AgencyCode
        {
            get
            {
                return ViewState["agencyCode"].ToString();
            }

            set
            {
                ViewState["agencyCode"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        private string GroupName
        {
            get
            {
                return ViewState["groupName"].ToString();
            }

            set
            {
                ViewState["groupName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the sub group.
        /// </summary>
        /// <value>
        /// The name of the sub group.
        /// </value>
        private string TableName
        {
            get
            {
                return ViewState["tableName"].ToString();
            }

            set
            {
                ViewState["tableName"] = value;
            }
        }

        /// <summary>
        /// Gets the current selected item.
        /// </summary>
        private Dictionary<int, int> CurrentSelectedItem
        {
            get
            {
                if (ViewState["CurrentSelectedItem"] == null)
                {
                    ViewState["CurrentSelectedItem"] = new Dictionary<int, int>();
                }

                return ViewState["CurrentSelectedItem"] as Dictionary<int, int>;
            }
        }

        /// <summary>
        /// Gets or sets the current level. (current level for current step)
        /// </summary>
        /// <value>
        /// The current level.
        /// </value>
        private int CurrentLevel
        {
            get
            {
                if (ViewState["CurrentLevel"] == null)
                {
                    ViewState["CurrentLevel"] = 0;
                }

                return (int)ViewState["CurrentLevel"];
            }

            set
            {
                ViewState["CurrentLevel"] = value;
            }
        }

        /// <summary>
        /// Gets the index of the selected single selection row.
        /// </summary>
        /// <value>
        /// The index of the selected single selection row.
        /// </value>
        private int SelectedSingleSelectionRowIndex
        {
            get
            {
                int result = -1;
                bool conversionResult = int.TryParse(Request.Form["SuppliersGroup"], out result);
                result = conversionResult ? result : -1;
                result = result % PAGE_ROWS_COUNT;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the selected indexes for each drill down level.
        /// </summary>
        /// <value>
        /// The selected indexes for each drill down level.
        /// </value>
        private Dictionary<int, List<int>> SelectedIndexes
        {
            get
            {
                if (ViewState["SelectedIndexes"] == null)
                {
                    ViewState["SelectedIndexes"] = new Dictionary<int, List<int>>();
                }

                return ViewState["SelectedIndexes"] as Dictionary<int, List<int>>;
            }

            set
            {
                ViewState["SelectedIndexes"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected page indexes for each level.
        /// </summary>
        /// <value>
        /// The selected page indexes.
        /// </value>
        private Dictionary<int, int> SelectedPageIndexes
        {
            get
            {
                if (ViewState["SelectedPageIndexes"] == null)
                {
                    ViewState["SelectedPageIndexes"] = new Dictionary<int, int>();
                }

                return ViewState["SelectedPageIndexes"] as Dictionary<int, int>;
            }

            set
            {
                ViewState["SelectedPageIndexes"] = value;
            }
        }

        /// <summary>
        /// Gets the name of the selected column.(Collect all selected Column names.The format is (columnID,ColumnName))
        /// </summary>
        /// <value>
        /// The name of the selected column.
        /// </value>
        private Dictionary<string, string> SelectedColumnName
        {
            get
            {
                if (ViewState["SelectedColumnName"] == null)
                {
                    ViewState["SelectedColumnName"] = new Dictionary<string, string>();
                }

                return ViewState["SelectedColumnName"] as Dictionary<string, string>;
            }
        }

        /// <summary>
        /// Gets or sets the drill down column ID paths.(Collect all selected Column IDs for every step.)
        /// </summary>
        /// <value>
        /// The drill down column ID paths.
        /// </value>
        private Dictionary<string, ArrayList> DrillDownColmunIDPaths
        {
            get
            {
                if (ViewState["DrillDownColmunIDPaths"] == null)
                {
                    ViewState["DrillDownColmunIDPaths"] = new Dictionary<string, ArrayList>();
                }

                return ViewState["DrillDownColmunIDPaths"] as Dictionary<string, ArrayList>;
            }

            set
            {
                ViewState["DrillDownColmunIDPaths"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the dynamic column collection.
        /// </summary>
        /// <value>
        /// The dynamic column collection.
        /// </value>
        private Dictionary<string, ASITableDrillDSeriesModel4WS> DynamicColumnCollection
        {
            get
            {
                if (ViewState["DynamicColumnCollection"] == null)
                {
                    ViewState["DynamicColumnCollection"] = new Dictionary<string, ASITableDrillDSeriesModel4WS>();
                }

                return ViewState["DynamicColumnCollection"] as Dictionary<string, ASITableDrillDSeriesModel4WS>;
            }

            set
            {
                ViewState["DynamicColumnCollection"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the ASIT drill down column count.
        /// </summary>
        /// <value>
        /// The ASIT drill down column count.
        /// </value>
        private int ASITDrillDownColumnCount
        {
            get
            {
                if (ViewState["ASITDrillDownColumnCount"] == null)
                {
                    ViewState["ASITDrillDownColumnCount"] = 0;
                }

                return (int)ViewState["ASITDrillDownColumnCount"];
            }

            set
            {
                ViewState["ASITDrillDownColumnCount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether need auto select edit data.
        /// </summary>
        private bool NeedAutoSelectEditData
        {
            get
            {
                if (ViewState["NeedAutoSelectEditData"] == null)
                {
                    return true;
                }

                return (bool)ViewState["NeedAutoSelectEditData"];
            }

            set
            {
                ViewState["NeedAutoSelectEditData"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// On Initial event method
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            string[] uiDataKeys = JsonConvert.DeserializeObject(Request.QueryString["uikey"], typeof(string[])) as string[];

            if (uiDataKeys != null && uiDataKeys.Length > 0)
            {
                asitUIDataKeys = uiDataKeys.Where(w => w.StartsWith(PageFlowConstant.SECTION_NAME_ASIT)).ToArray();
            }

            //If there is not exist ASIT, then will redirect to edit form page.
            if (asitUIDataKeys == null || asitUIDataKeys.Length == 0)
            {
                Response.Redirect("EditForm.aspx?" + Request.QueryString.ToString());
            }

            GridViewBuildHelper.SetSimpleViewElements(drillDownList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("600");
            SetPageTitleKey("aca_drilldown_label_title|tip");
            SetPageTitleVisible(false);

            if (!Page.IsPostBack)
            {
                bool initilizingResult = InitParameters();

                if (initilizingResult)
                {
                    ShowFirstDrillDownList();
                    lblSearchName.Text = LabelUtil.GetTextByKey("aca_sys_drill_down_search_lbl", ModuleName);
                    string searchContentTitle = LabelUtil.RemoveHtmlFormat(lblSearchName.Text + " " + LabelUtil.GetTextByKey("aca_required_field", ModuleName));
                    txtSearchContent.Attributes.Add("title", searchContentTitle);
                    RegisterHotKey();
                }

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    reqSearch.ForeColor = Color.FromName("#B60000");
                }
            }
        }

        /// <summary>
        /// Search asi table fields.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle</param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            IsSearchTableRuning = true;
            this.IsSingleSection = this.IsSingleSectionOnLastStep;
            string searchContent = txtSearchContent.Text.Trim();
            ASITDrillDownUtil asitDrillDownInstance = new ASITDrillDownUtil();
            asitDrillDownInstance.AsitFieldsLabel = AsitFieldsLabel;
            var searchResults = asitDrillDownInstance.SearchDrillDown(AgencyCode, GroupName, TableName, searchContent);
            Session[SessionConstant.SESSION_DRILL_DOWN_LIST] = searchResults;
            drillDownList.DataSource = searchResults;
            drillDownList.DataBind();
            SetBreadcrumb();
            AccessibilityUtil.FocusElement(txtSearchContent.ClientID);
        }

        /// <summary>
        /// Handles the RowDataBound event of the drillDownList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void DrillDownList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (string.IsNullOrEmpty(e.Row.Cells[1].Text) || e.Row.Cells[1].Text == "&nbsp;")
                {
                    e.Row.Visible = false;
                    return;
                }

                StringBuilder title = new StringBuilder();
                for (int i = 6; i < e.Row.Cells.Count; i++)
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[i].Text))
                    {
                        title.AppendFormat("{0} ", e.Row.Cells[i].Text);
                    }
                }

                int realRowIndex = GetRealRowIndex(e.Row.RowIndex);
                bool isCurrentRowSelected = SelectedIndexes.ContainsKey(CurrentLevel) && SelectedIndexes[CurrentLevel].Contains(realRowIndex);

                if (IsSingleSection)
                {
                    Literal output = e.Row.FindControl("RadioButtonMarkup") as Literal;
                    output.Text = string.Format(@"<input type=""radio"" name=""SuppliersGroup"" id=""RowSelector{0}"" value=""{0}"" rowIndex=""{1}"" title=""{2}"" ", realRowIndex, "_" + realRowIndex + "_", title);

                    if (isCurrentRowSelected)
                    {
                        output.Text += @" checked=""checked""";
                    }

                    output.Text += "onclick='EnableOperationButton(this)' />";
                }
                else
                {
                    CheckBox cb = e.Row.FindControl("chkSelectDrill") as CheckBox;
                    cb.Attributes.Add("onclick", "EnableOperationButton(this)");
                    cb.Attributes.Add("rowIndex", "_" + realRowIndex.ToString() + "_");
                    cb.InputAttributes.Add("title", title.ToString());

                    if (isCurrentRowSelected)
                    {
                        cb.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the drillDownList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void DrillDownList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            UpdateSelectedData();
            ASITDrillDownUtil asitDrillDownInstance = new ASITDrillDownUtil();
            asitDrillDownInstance.CurrentFinallyDrillDownList = Session[SessionConstant.SESSION_DRILL_DOWN_LIST] as DataTable;
            drillDownList.PageIndex = e.NewPageIndex;
            drillDownList.DataSource = asitDrillDownInstance.GetSeemlyRowsTable(e.NewPageIndex);
            drillDownList.DataBind();
            SetBreadcrumb();
            AccessibilityUtil.FocusElement(txtSearchContent.ClientID);
        }

        /// <summary>
        /// Handles the OnPreRender event of the ASITable control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void DrillDownList_PreRender(object sender, EventArgs e)
        {
            var currentGridView = sender as GridView;

            if (currentGridView == null)
            {
                return;
            }

            if (currentGridView.Controls.Count > 0
                && currentGridView.Controls[0].Controls.Count > 0
                && currentGridView.Controls[0].Controls[0] is GridViewRow)
            {
                var headerRow = currentGridView.Controls[0].Controls[0] as GridViewRow;
                HiddenAndSetSomeColumns(headerRow);
            }

            foreach (GridViewRow currentRow in currentGridView.Rows)
            {
                HiddenAndSetSomeColumns(currentRow);
            }
        }

        /// <summary>
        /// Go to next step to set another process
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnNext_Click(object sender, EventArgs e)
        {
            UpdateSelectedData();
            int seriseID = this.GetCurrentLevelSeriseID();
            ASITDrillDownUtil asitDrillDownInstance = new ASITDrillDownUtil();
            asitDrillDownInstance.DynamicColumnCollection = this.DynamicColumnCollection;
            asitDrillDownInstance.SelectedColumnName = SelectedColumnName;
            asitDrillDownInstance.AsitFieldsLabel = AsitFieldsLabel;

            DataTable dt = asitDrillDownInstance.GetNextDrillDownDataList(AgencyCode, GetLevelDrillDownColumnIDPaths(CurrentLevel), seriseID, CurrentLevel);
            IsSingleSection = asitDrillDownInstance.IsSingleSection;
            SelectedPageIndexes[CurrentLevel] = drillDownList.PageIndex;

            CurrentLevel += 1;
            drillDownList.PageIndex = 0;
            bool hasNoData = dt.Rows.Count == 0;

            AutoSelectEditData(dt);
            drillDownList.DataSource = dt;
            drillDownList.DataBind();
            Session[SessionConstant.SESSION_DRILL_DOWN_LIST] = asitDrillDownInstance.CurrentFinallyDrillDownList;
            SetLabels(dt.Columns, hasNoData);
            AccessibilityUtil.FocusElement(txtSearchContent.ClientID);
        }

        /// <summary>
        /// Back to previous step that shall display previous step
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnBack_Click(object sender, EventArgs e)
        {
            ClearCurrentLevelData();

            if (CurrentLevel < 2)
            {
                CurrentLevel--;
                ShowFirstDrillDownList();
            }
            else
            {
                var asitDrillDownInstance = new ASITDrillDownUtil();

                if (!CurrentSelectedItem.ContainsKey(CurrentLevel))
                {
                    CurrentSelectedItem.Add(CurrentLevel, GetCurrentLevelSeriseID());
                }

                this.DynamicColumnCollection.Remove(CurrentSelectedItem[CurrentLevel].ToString());
                asitDrillDownInstance.DynamicColumnCollection = DynamicColumnCollection;
                asitDrillDownInstance.SelectedColumnName = SelectedColumnName;
                asitDrillDownInstance.AsitFieldsLabel = AsitFieldsLabel;
                int previous2Level = CurrentLevel - 2;
                long seriseID = long.Parse(CurrentSelectedItem[previous2Level].ToString());
                var dt = asitDrillDownInstance.GetNextDrillDownDataList(AgencyCode, GetLevelDrillDownColumnIDPaths(previous2Level), seriseID, previous2Level);
                IsSingleSection = asitDrillDownInstance.IsSingleSection;
                CurrentLevel--;
                drillDownList.PageIndex = SelectedPageIndexes.ContainsKey(CurrentLevel) ? SelectedPageIndexes[CurrentLevel] : 0;
                Session[SessionConstant.SESSION_DRILL_DOWN_LIST] = dt;
                drillDownList.DataSource = dt;
                drillDownList.DataBind();
                SetLabels(dt.Columns, false);
            }

            string currentSelectedItems = string.Empty;

            foreach (int rowIndex in SelectedIndexes[CurrentLevel])
            {
                currentSelectedItems += string.Format("{0}{1}{2}", ACAConstant.SPLIT_CHAR5, rowIndex.ToString(), ACAConstant.SPLIT_CHAR5);
            }

            hdnSelectedItems.Value = currentSelectedItems;

            IsSearchTableRuning = false;

            ScriptManager.RegisterStartupScript(this.Page, GetType(), "SetCurrentSelectedItems", "SetCurrentSelectedItems();", true);
            AccessibilityUtil.FocusElement(txtSearchContent.ClientID);
        }

        /// <summary>
        /// Finish ASI table drill down processes and generate data to ASI table rows
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnFinish_Click(object sender, EventArgs e)
        {
            FinishDrillDown();
        }

        /// <summary>
        /// OnPreRender event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetActionButtons();
        }

        /// <summary>
        /// Initialize the parameters.
        /// </summary>
        /// <returns>the result for initializing parameters</returns>
        private bool InitParameters()
        {
            bool result = false;
            string tableInfo = Request.QueryString["param"];

            if (!string.IsNullOrEmpty(tableInfo))
            {
                string[] asiTableKeys = JsonConvert.DeserializeObject(tableInfo, typeof(string[])) as string[];

                if (asiTableKeys != null && asiTableKeys.Length > 0)
                {
                    var allASITables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new string[] { asitUIDataKeys[0] }) as ASITUITable[];
                    var curUITable = allASITables.Where(t => asiTableKeys.Contains(t.TableKey)).FirstOrDefault();

                    if (curUITable != null)
                    {
                        AgencyCode = curUITable.AgencyCode;
                        GroupName = curUITable.GroupName;
                        TableName = curUITable.TableName;
                        result = true;

                        //collect the asit fields I18n column name and I18n Label
                        Dictionary<string, string> asitFields = new Dictionary<string, string>();

                        if (curUITable.TemplateRow != null && curUITable.TemplateRow.Fields != null)
                        {
                            foreach (var field in curUITable.TemplateRow.Fields)
                            {
                                string colName = I18nStringUtil.GetString(field.ResName, field.Name);

                                if (!asitFields.ContainsKey(colName))
                                {
                                    asitFields.Add(colName, field.Label);
                                }
                            }
                        }

                        AsitFieldsLabel = asitFields;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the action buttons.
        /// </summary>
        private void SetActionButtons()
        {
            bool hasCurrentLevelSelectedItems = SelectedIndexes.ContainsKey(CurrentLevel) && SelectedIndexes[CurrentLevel].Count > 0;
            var finishButtonVisible = CurrentLevel >= ASITDrillDownColumnCount || IsSearchTableRuning;
            var nextButtonVisible = !finishButtonVisible;

            btnNext.Visible = nextButtonVisible;
            btnFinish.Visible = finishButtonVisible;

            AccelaButton currentActiveButton = finishButtonVisible ? btnFinish : btnNext;

            if (!hasCurrentLevelSelectedItems)
            {
                currentActiveButton.Attributes.Add("disabled", "disabled");
                currentActiveButton.DivEnableCss = "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize";
            }
            else
            {
                currentActiveButton.Attributes.Remove("disabled");
                currentActiveButton.DivEnableCss = "ACA_LgButton ACA_LgButton_FontSize";
            }

            btnBack.Visible = CurrentLevel == 0 || IsSearchTableRuning ? false : true;
        }

        /// <summary>
        /// Set some labels value of ASIT drill down form by label key
        /// </summary>
        /// <param name="dataColumnCollection">The data column collection.</param>
        /// <param name="hasNoData">if set to <c>true</c> [has no data].</param>
        private void SetLabels(DataColumnCollection dataColumnCollection, bool hasNoData)
        {
            if (hasNoData)
            {
                lblColumnName.Visible = false;
                lblDDTip.Visible = false;
                lblSingleOrMulti.Text = LabelUtil.GetTextByKey("aca_asi_table_drill_down_tip_four", ModuleName);
            }
            else
            {
                //hidden 5 columns
                int currentASIFileldIndex = 5 + CurrentSelectedItem.Count;
                currentASIFileldIndex = currentASIFileldIndex >= dataColumnCollection.Count ? dataColumnCollection.Count - 1 : currentASIFileldIndex;

                lblColumnName.Visible = true;
                lblColumnName.Text = LabelUtil.GetTextByKey("aca_asi_table_drill_down_select_label", ModuleName) + " " + dataColumnCollection[currentASIFileldIndex].ColumnName;

                lblSingleOrMulti.Text = "(";
                lblSingleOrMulti.Text += IsSingleSection
                                             ? LabelUtil.GetTextByKey("aca_asi_table_drill_down_select_single", ModuleName) : LabelUtil.GetTextByKey("aca_asi_table_drill_down_select_multiple", ModuleName);
                lblSingleOrMulti.Text += ")";

                lblDDTip.Visible = true;
                lblDDTip.Text = CurrentLevel != 0 ? string.Empty : @"<ul>" + string.Format(LabelUtil.GetTextByKey("aca_asi_table_drill_down_tip_one", ModuleName), dataColumnCollection[currentASIFileldIndex].ColumnName) + @"</ul>";
            }

            SetBreadcrumb();
        }

        /// <summary>
        /// Set the bread crumb
        /// </summary>
        private void SetBreadcrumb()
        {
            string selectTitle = string.Empty;

            if (CurrentLevel > 0 && !IsSearchTableRuning)
            {
                int loopLevel = 0;

                while (loopLevel < CurrentLevel)
                {
                    string keyEnd = ACAConstant.SPLIT_CHAR + Convert.ToString(loopLevel);
                    var loopSelecteds = SelectedColumnName.Where(w => w.Key.EndsWith(keyEnd)).ToDictionary(v => v.Key, v => v.Value);

                    if (loopSelecteds.Count() == 1)
                    {
                        selectTitle += loopSelecteds.ElementAt(0).Value + " > ";
                    }
                    else
                    {
                        selectTitle = string.Empty;
                        break;
                    }

                    loopLevel++;
                }
            }

            lblSelectPath.Text = selectTitle.TrimEnd().TrimEnd('>');
            needHiddenSingleSectionField = !string.IsNullOrEmpty(selectTitle);
        }

        /// <summary>
        ///   Register hot key to cancel button
        /// </summary>
        private void RegisterHotKey()
        {
            const string JS_PATTERN = "OverrideTabKey(event, {0}, '{1}')";
            lnkCancel.Attributes.Add("onkeydown", string.Format(JS_PATTERN, "false", txtSearchContent.ClientID));
            txtSearchContent.Attributes.Add("onkeydown", string.Format(JS_PATTERN, "true", lnkCancel.ClientID));
        }

        /// <summary>
        /// show first step drill down data list if current sub group requires to generate data by ASIT drill down process.
        /// </summary>
        private void ShowFirstDrillDownList()
        {
            IsSearchTableRuning = false;

            ASITDrillDownUtil asitDrillDownInstance = new ASITDrillDownUtil();
            asitDrillDownInstance.AsitFieldsLabel = AsitFieldsLabel;

            DataTable dt = asitDrillDownInstance.GetFirstDrillDownDataList(AgencyCode, GroupName, TableName);
            bool hasDrillDownData = dt.Rows.Count > 0 && asitDrillDownInstance.IsRequireDrillDownData();

            if (hasDrillDownData)
            {
                this.DynamicColumnCollection = asitDrillDownInstance.DynamicColumnCollection;
                this.IsSingleSectionOnLastStep = asitDrillDownInstance.IsSingleSectionOnLastStep;
                IsSingleSection = asitDrillDownInstance.IsSingleSection;
                ASITDrillDownColumnCount = asitDrillDownInstance.ASITDrillDownTableColumnCount;
                Session[SessionConstant.SESSION_DRILL_DOWN_LIST] = asitDrillDownInstance.CurrentFinallyDrillDownList;
                CurrentLevel = 0;
                AutoSelectEditData(dt);
                drillDownList.PageIndex = SelectedPageIndexes.ContainsKey(CurrentLevel) ? SelectedPageIndexes[CurrentLevel] : 0;
                drillDownList.DataSource = dt;
                drillDownList.DataBind();
                SetLabels(dt.Columns, false);
            }
            else
            {
                Response.Redirect("EditForm.aspx?" + Request.QueryString.ToString());
            }
        }

        /// <summary>
        /// Finishes the drill down.
        /// </summary>
        private void FinishDrillDown()
        {
            UpdateSelectedRowIndexes();

            var finalColumnNames = GetFinalColumnNames();
            var languageValueIDList = new List<string[]>();
            var selectedIndexes4Level = SelectedIndexes[CurrentLevel];
            int rowIndex = 0;

            foreach (DataRow gvr in (Session[SessionConstant.SESSION_DRILL_DOWN_LIST] as DataTable).Rows)
            {
                if (selectedIndexes4Level.Contains(rowIndex))
                {
                    string[] languageValueIDs = gvr[4].ToString().Split('_');
                    languageValueIDList.Add(languageValueIDs);
                }

                rowIndex++;
            }

            var uiDataKey = asitUIDataKeys[0];
            ASITUITable dTable = null;
            string action = Request.QueryString["action"];
            int editRowCount = 0;

            if ("edit".Equals(action, StringComparison.OrdinalIgnoreCase))
            {
                var editTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { uiDataKey }) as ASITUITable[];
                ASITUITable editTable = editTables.Where(t => t.AgencyCode == AgencyCode && t.GroupName == GroupName && t.TableName == TableName).FirstOrDefault();

                if (editTable != null)
                {
                    editRowCount = editTable.Rows.Count;
                    dTable = editTable;
                }
            }

            ASITUITable[] uiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new string[] { uiDataKey }) as ASITUITable[];
            ASITUITable currentTable = uiTables.Where(t => t.AgencyCode == AgencyCode && t.GroupName == GroupName && t.TableName == TableName).SingleOrDefault();
            var newRowCount = languageValueIDList.Count - editRowCount;
            var newTable = ASITUIModelUtil.CreateNewRow4ASITUITable(currentTable, newRowCount, uiDataKey);

            if (dTable != null && newTable != null)
            {
                ASITUIModelUtil.SyncASITUIRowData(new ASITUITable[] { newTable }, new ASITUITable[] { dTable });
            }
            else if (newTable != null)
            {
                dTable = newTable;
            }

            var asitDrillDownBll = ObjectFactory.GetObject<IASITableDrillDownBLL>();

            for (var i = 0; i < languageValueIDList.Count; i++)
            {
                var languageValueIDs = languageValueIDList[i];
                UIRow currentRow = dTable.Rows[i];
                string[] languageValue = asitDrillDownBll.GetMainLanguageValues(AgencyCode, languageValueIDs);

                for (int columnIndex = 0; columnIndex < currentRow.Fields.Count; columnIndex++)
                {
                    ASITUIField currentField = currentRow.Fields[columnIndex] as ASITUIField;

                    if (finalColumnNames.ContainsKey(currentField.Label))
                    {
                        int columnIndexID = finalColumnNames[currentField.Label];
                        currentField.IsDrillDown = true;
                        currentField.Value = languageValue[columnIndexID];
                    }
                }
            }

            var asitEditUiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { uiDataKey }) as ASITUITable[];

            if (asitEditUiTables != null)
            {
                ASITUIModelUtil.SyncASITUIRowData(new ASITUITable[] { dTable }, asitEditUiTables);
            }
            else
            {
                asitEditUiTables = new ASITUITable[] { dTable };
            }

            UIModelUtil.SetDataToUIContainer(asitEditUiTables, UIDataType.ASITEdit, uiDataKey);

            Response.Redirect("EditForm.aspx?isFromDrillDown=Yes&isPopup=Y&action=edit&module=" + ModuleName + "&uikey=" + Request.QueryString["uikey"]);
        }

        /// <summary>
        /// Gets the final column names.
        /// </summary>
        /// <returns>the final column names.</returns>
        private Dictionary<string, int> GetFinalColumnNames()
        {
            var results = new Dictionary<string, int>();

            for (int i = 6; i < drillDownList.HeaderRow.Cells.Count; i++)
            {
                results.Add(Server.HtmlDecode(drillDownList.HeaderRow.Cells[i].Text), i - 6);
            }

            return results;
        }

        /// <summary>
        /// Clear current level data.
        /// </summary>
        private void ClearCurrentLevelData()
        {
            string keyEnd = ACAConstant.SPLIT_CHAR + CurrentLevel.ToString();
            ClearSelectedColumnNames(SelectedColumnName, keyEnd);
            ClearSelectedDrillDownIDPaths(DrillDownColmunIDPaths, keyEnd);

            CurrentSelectedItem.Remove(CurrentLevel);
            SelectedIndexes.Remove(CurrentLevel);
        }

        /// <summary>
        /// Clears the selected column names.
        /// </summary>
        /// <param name="selectedColumnName">Name of the selected column.</param>
        /// <param name="keyEnd">The key end.</param>
        private void ClearSelectedColumnNames(Dictionary<string, string> selectedColumnName, string keyEnd)
        {
            if (selectedColumnName != null && selectedColumnName.Count > 0)
            {
                string[] columnKeys = new string[selectedColumnName.Keys.Count];
                selectedColumnName.Keys.CopyTo(columnKeys, 0);

                foreach (string columnKey in columnKeys)
                {
                    if (columnKey.EndsWith(keyEnd))
                    {
                        selectedColumnName.Remove(columnKey);
                    }
                }
            }
        }

        /// <summary>
        /// Clears the selected drill down ID paths.
        /// </summary>
        /// <param name="drillDownColmunIDPaths">The drill down column ID paths.</param>
        /// <param name="keyEnd">The key end.</param>
        private void ClearSelectedDrillDownIDPaths(Dictionary<string, ArrayList> drillDownColmunIDPaths, string keyEnd)
        {
            if (drillDownColmunIDPaths != null && drillDownColmunIDPaths.Count > 0)
            {
                string[] columnKeys = new string[drillDownColmunIDPaths.Keys.Count];
                drillDownColmunIDPaths.Keys.CopyTo(columnKeys, 0);

                foreach (string columnKey in columnKeys)
                {
                    if (columnKey.EndsWith(keyEnd))
                    {
                        drillDownColmunIDPaths.Remove(columnKey);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the selected data.
        /// </summary>
        private void UpdateSelectedData()
        {
            UpdateSelectedItem();
            UpdateSelectedColumnNames();
            UpdateSelectedColumnIDPaths();
            UpdateSelectedRowIndexes();
        }

        /// <summary>
        /// auto select edit data
        /// </summary>
        /// <param name="dt">the data table</param>
        private void AutoSelectEditData(DataTable dt)
        {
            if (!NeedAutoSelectEditData)
            {
                return;
            }

            string action = Request.QueryString["action"];

            if (!"edit".Equals(action, StringComparison.OrdinalIgnoreCase))
            {
                NeedAutoSelectEditData = false;
                return;
            }

            var uiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { asitUIDataKeys[0] }) as ASITUITable[];

            if (uiTables == null || uiTables.Length == 0
                || uiTables[0].Rows == null || uiTables[0].Rows.Count == 0
                || uiTables[0].Rows[0].Fields == null || uiTables[0].Rows[0].Fields.Count == 0)
            {
                // Did not found any data.
                NeedAutoSelectEditData = false;
                return;
            }

            IList<UIField> editDrillDownFields = uiTables[0].Rows[0].Fields;

            /*
             * the key is the dt table's column index.
             * the value is the asit edit field value.
             * the count equal the CurrentLevel
             */
            var tempEditFields = new Dictionary<int, string>();

            for (int i = 5; i < dt.Columns.Count; i++)
            {
                DataColumn dataColumn = dt.Columns[i];
                var edit = editDrillDownFields.Where(w => w.Label.Equals(dataColumn.ColumnName)).FirstOrDefault();

                if (edit == null)
                {
                    // Did not found the column.
                    NeedAutoSelectEditData = false;
                    return;
                }

                tempEditFields.Add(i, edit.Value);
            }

            if (tempEditFields.Count == 0)
            {
                // Did not found any column.
                NeedAutoSelectEditData = false;
                return;
            }

            // Get I18N value
            var editFields = new Dictionary<int, string>();
            var asitDrillDownBLL = ObjectFactory.GetObject<IASITableDrillDownBLL>();
            string[] resValues = asitDrillDownBLL.GetFieldTexts(uiTables[0].AgencyCode, tempEditFields.Values.ToArray());

            if (resValues != null && resValues.Length == tempEditFields.Count)
            {
                for (int i = 0; i < resValues.Length; i++)
                {
                    editFields.Add(tempEditFields.ElementAt(i).Key, resValues[i]);
                }
            }

            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                bool isEditRowFound = true;
                DataRow dataRow = dt.Rows[rowIndex];

                /*
                 * if data cell value not equal the editField.value
                 * it means that the data row record not match the edit field.
                 */
                foreach (var editField in editFields)
                {
                    if (dataRow[editField.Key].Equals(editField.Value))
                    {
                        continue;
                    }

                    isEditRowFound = false;
                    break;
                }

                if (isEditRowFound)
                {
                    var selectedIndexes4Level = SelectedIndexes.ContainsKey(CurrentLevel) ? SelectedIndexes[CurrentLevel] : new List<int>() { };

                    if (!selectedIndexes4Level.Contains(rowIndex))
                    {
                        selectedIndexes4Level.Add(rowIndex);
                    }

                    SelectedIndexes[CurrentLevel] = selectedIndexes4Level;
                    return;
                }
            }

            NeedAutoSelectEditData = false;
        }

        /// <summary>
        /// Updates the selected item.
        /// </summary>
        private void UpdateSelectedItem()
        {
            int seriseID = GetCurrentLevelSeriseID();

            if (seriseID != 0)
            {
                CurrentSelectedItem[CurrentLevel] = seriseID;
            }
        }

        /// <summary>
        /// Updates the selected column ID paths.
        /// </summary>
        private void UpdateSelectedColumnIDPaths()
        {
            int pageIndex = drillDownList.PageIndex > -1 ? drillDownList.PageIndex : 0;
            string pageKey = GetPageKey(CurrentLevel, pageIndex);

            if (IsSingleSection)
            {
                if (SelectedSingleSelectionRowIndex != -1)
                {
                    GridViewRow currentRow = drillDownList.Rows[SelectedSingleSelectionRowIndex];
                    string currentColumnIDPath = currentRow.Cells[5].Text;

                    string keyEnd = ACAConstant.SPLIT_CHAR + CurrentLevel.ToString();
                    ClearSelectedDrillDownIDPaths(DrillDownColmunIDPaths, keyEnd);
                    DrillDownColmunIDPaths[pageKey] = new ArrayList() { currentColumnIDPath };
                }
            }
            else
            {
                string keyEnd = GetPageKey(CurrentLevel, pageIndex);
                ClearSelectedDrillDownIDPaths(DrillDownColmunIDPaths, keyEnd);

                if (!DrillDownColmunIDPaths.ContainsKey(pageKey) || DrillDownColmunIDPaths[pageKey] == null)
                {
                    DrillDownColmunIDPaths[pageKey] = new ArrayList() { };
                }

                var columnIDPathList = DrillDownColmunIDPaths[pageKey];

                foreach (GridViewRow currentRow in drillDownList.Rows)
                {
                    string currentColumnIDPath = currentRow.Cells[5].Text;
                    CheckBox cb = currentRow.Cells[0].Controls[1] as CheckBox;

                    if (cb.Checked && !columnIDPathList.Contains(currentColumnIDPath))
                    {
                        columnIDPathList.Add(currentColumnIDPath);
                    }
                    else if (!cb.Checked && columnIDPathList.Contains(currentColumnIDPath))
                    {
                        columnIDPathList.Remove(currentColumnIDPath);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the selected column names.
        /// </summary>
        private void UpdateSelectedColumnNames()
        {
            int pageIndex = drillDownList.PageIndex > -1 ? drillDownList.PageIndex : 0;

            if (IsSingleSection)
            {
                if (SelectedSingleSelectionRowIndex != -1)
                {
                    GridViewRow currentRow = drillDownList.Rows[SelectedSingleSelectionRowIndex];
                    string currentSupplierID = currentRow.Cells[5].Text;
                    string selectedColumnKey = currentSupplierID + ACAConstant.SPLIT_CHAR + GetPageKey(CurrentLevel, pageIndex);
                    string currentColumnName = Server.HtmlDecode(currentRow.Cells[currentRow.Cells.Count - 1].Text);

                    string keyEnd = ACAConstant.SPLIT_CHAR + CurrentLevel.ToString();
                    ClearSelectedColumnNames(SelectedColumnName, keyEnd);

                    SelectedColumnName[selectedColumnKey] = currentColumnName;
                }
            }
            else
            {
                string keyEnd = ACAConstant.SPLIT_CHAR + GetPageKey(CurrentLevel, pageIndex);
                ClearSelectedColumnNames(SelectedColumnName, keyEnd);

                foreach (GridViewRow currentRow in drillDownList.Rows)
                {
                    CheckBox cb = currentRow.Cells[0].Controls[1] as CheckBox;

                    /*
                    * Pre level is multi, you select A and B, and A,B both match the C
                    * In current level,if you select the A+C Row,the A's selectedColumnKey equal the B's selectedColumnKey
                    * It will be remove from the SelectedColumnName,
                    * Next level,it will not find the C value.
                    * so use path id. 
                    */
                    string currentSupplierID = currentRow.Cells[5].Text;
                    string selectedColumnKey = currentSupplierID + ACAConstant.SPLIT_CHAR + GetPageKey(CurrentLevel, pageIndex);
                    string currentColumnName = Server.HtmlDecode(currentRow.Cells[currentRow.Cells.Count - 1].Text);

                    if (cb.Checked && !SelectedColumnName.ContainsKey(selectedColumnKey))
                    {
                        SelectedColumnName.Add(selectedColumnKey, currentColumnName);
                    }
                    else if (!cb.Checked && SelectedColumnName.ContainsKey(selectedColumnKey))
                    {
                        SelectedColumnName.Remove(selectedColumnKey);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the selected row indexes.
        /// </summary>
        private void UpdateSelectedRowIndexes()
        {
            if (IsSingleSection)
            {
                if (SelectedSingleSelectionRowIndex != -1)
                {
                    var actualSelectedRowIndex = GetRealRowIndex(SelectedSingleSelectionRowIndex);
                    SelectedIndexes[CurrentLevel] = new List<int>() { actualSelectedRowIndex };
                }
            }
            else
            {
                var selectedIndexes4Level = SelectedIndexes.ContainsKey(CurrentLevel) ? SelectedIndexes[CurrentLevel] : new List<int>() { };

                foreach (GridViewRow currentRow in drillDownList.Rows)
                {
                    CheckBox cb = currentRow.Cells[0].Controls[1] as CheckBox;

                    int realRowIndex = GetRealRowIndex(currentRow.RowIndex);

                    if (cb.Checked && !selectedIndexes4Level.Contains(realRowIndex))
                    {
                        selectedIndexes4Level.Add(realRowIndex);
                    }
                    else if (!cb.Checked && selectedIndexes4Level.Contains(realRowIndex))
                    {
                        selectedIndexes4Level.Remove(realRowIndex);
                    }
                }

                SelectedIndexes[CurrentLevel] = selectedIndexes4Level;
            }
        }

        /// <summary>
        /// Gets the current level series ID.
        /// </summary>
        /// <returns>the current level series ID.</returns>
        private int GetCurrentLevelSeriseID()
        {
            int result = drillDownList.Rows.Count > 0 ? int.Parse(drillDownList.Rows[0].Cells[3].Text) : 0;
            return result;
        }

        /// <summary>
        /// get current level drill down column id paths.
        /// </summary>
        /// <param name="level">it is a level.</param>
        /// <returns>
        /// level drill down column id paths
        /// </returns>
        private ArrayList GetLevelDrillDownColumnIDPaths(int level)
        {
            ArrayList curLevelDrillDownColumnIDPaths = new ArrayList();

            if (DrillDownColmunIDPaths != null && DrillDownColmunIDPaths.Count > 0)
            {
                string keyEnds = level.ToString();

                string[] columnKeys = new string[DrillDownColmunIDPaths.Keys.Count];
                DrillDownColmunIDPaths.Keys.CopyTo(columnKeys, 0);

                foreach (string columnKey in columnKeys)
                {
                    if (columnKey.EndsWith(keyEnds))
                    {
                        curLevelDrillDownColumnIDPaths.AddRange(DrillDownColmunIDPaths[columnKey].ToArray());
                    }
                }
            }

            return curLevelDrillDownColumnIDPaths;
        }

        /// <summary>
        /// The real row index equal current sum of current page* current row index.
        /// </summary>
        /// <param name="relativeRowIndex">Index of the relative row.</param>
        /// <returns>
        /// current page* current row index
        /// </returns>
        private int GetRealRowIndex(int relativeRowIndex)
        {
            int pageIndex = drillDownList.PageIndex > -1 ? drillDownList.PageIndex : 0;
            int realRowIndex = relativeRowIndex + (pageIndex * PAGE_ROWS_COUNT);
            return realRowIndex;
        }

        /// <summary>
        /// generate part of a key string which  Associated with page index.
        /// </summary>
        /// <param name="level">it is a level.</param>
        /// <param name="pageIndex">it is a page index.</param>
        /// <returns>
        /// part of key string
        /// </returns>
        private string GetPageKey(int level, int pageIndex)
        {
            return pageIndex.ToString() + ACAConstant.SPLIT_CHAR + level.ToString();
        }

        /// <summary>
        /// hidden and set the grid view columns
        /// </summary>
        /// <param name="currentRow">the grid view row</param>
        private void HiddenAndSetSomeColumns(GridViewRow currentRow)
        {
            if ((currentRow.RowType != DataControlRowType.Header && currentRow.RowType != DataControlRowType.DataRow) || currentRow.Cells.Count <= 5)
            {
                return;
            }

            /*
             * The previous six coulumn is hidden and not showed.
             * The seventh column is first display column.
             * The first display column must set width when the culture is arabic.
             */

            int notSetWidthCellsCount = 6;

            if (needHiddenSingleSectionField)
            {
                notSetWidthCellsCount = notSetWidthCellsCount + CurrentLevel;
            }

            for (int i = notSetWidthCellsCount; i < currentRow.Cells.Count; i++)
            {
                double width = (1.00 / (currentRow.Cells.Count - notSetWidthCellsCount)) * 748;
                currentRow.Cells[i].Style[HtmlTextWriterStyle.Width] = width.ToString() + "px";
                currentRow.Cells[i].Style[HtmlTextWriterStyle.VerticalAlign] = HtmlTextWriterStyle.Top.ToString();
            }

            /*
             * Below fields is the primary key, and does not display to UI.
             * Cell 0 is the Radio/CheckBox control filed.
             * Cell 1 is "ID" field.
             * Cell 2 is "ParentID" field.
             * Cell 3 is "SeriesID" field.
             * Cell 4 is "SeriesParentID" field.
             * Cell 5 is "ColumnIDPath" field.
             */
            if (currentRow.RowType == DataControlRowType.Header)
            {
                currentRow.Cells[0].Text = string.Empty;
            }

            currentRow.Cells[1].Visible = false;
            currentRow.Cells[2].Visible = false;
            currentRow.Cells[3].Visible = false;
            currentRow.Cells[4].Visible = false;
            currentRow.Cells[5].Visible = false;

            if (needHiddenSingleSectionField)
            {
                int loopLevel = 0;

                while (loopLevel < CurrentLevel)
                {
                    currentRow.Cells[6 + loopLevel].Visible = false;
                    loopLevel++;
                }
            }
        }

        #endregion Methods
    }
}
