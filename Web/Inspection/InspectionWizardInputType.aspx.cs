#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionWizardInputType.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Select the inspection type from available list.
 *
 *  Notes:
 *      $Id: InspectionWizardInputType.aspx.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// This class provide the ability to select the inspection type from available list.
    /// </summary>
    public partial class InspectionWizardInputType : InspectionWizardBasePage
    {
        #region Properties

        /// <summary>
        /// Category ID
        /// </summary>
        private const string CATEGORY_ID = "ID";

        /// <summary>
        /// Category Text
        /// </summary>
        private const string CATEGORY_TEXT = "Category";

        /// <summary>
        /// Gets or sets data source.
        /// </summary>
        private DataTable GridViewDataSource
        {
            get
            {
                string moduleName = string.IsNullOrEmpty(ModuleName) ? ACAConstant.DEFAULT_MODULE_NAME : ModuleName;

                if (ViewState[moduleName] != null)
                {
                    return (DataTable)ViewState[moduleName];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                string moduleName = string.IsNullOrEmpty(ModuleName) ? ACAConstant.DEFAULT_MODULE_NAME : ModuleName;

                ViewState[moduleName] = value;
            }
        }

        #endregion Properties

        #region Protected Methods

        /// <summary>
        /// Raises the page load event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_inspection_title_scheduleorrequestinspection");
            this.SetDialogMaxHeight("600");

            if (!Page.IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    MarkCurrentPageTrace(InspectionWizardPage.SelectTypes, false);

                    // step 1
                    InitShowingOptionalInspectionSwitch();

                    // step 2
                    InitCategoriesAndInspectionTypes(false);

                    // set the button style
                    if (string.IsNullOrEmpty(InspectionWizardParameter.TypeID))
                    {
                        SetWizardButtonDisable(lnkContinue.ClientID, true);
                    }

                    MarkCurrentPageTrace(InspectionWizardPage.SelectTypes, true);
                }

                // set the admin UI
                SetAdminUI();
            }
        }

        /// <summary>
        /// Raises the Continue button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdInspectionType.Value))
            {
                SkipToNextPage();
            }
            else
            {
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, "You have not selected a inspection type.");
            }
        }

        /// <summary>
        /// Raises the page index changing event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.GridViewPageEventArgs object containing the event data.</param>
        protected void InspectionTypeGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int selectedRowIndex = e.NewPageIndex * gvInspectionType.PageSize;
            BindDataSource(GridViewDataSource, selectedRowIndex);

            SetContinueButtonStatus();
        }

        /// <summary>
        /// Raises ShowOptional checkbox checked changed event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ShowOptionalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            InitCategoriesAndInspectionTypes(true);

            SetContinueButtonStatus();
        }

        /// <summary>
        /// Raises Category dropdownlist selected index changed event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void CategoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitInspectionTypes();

            SetContinueButtonStatus();
        }

        /// <summary>
        /// Handles the RowDataBound event of the <c>gvInspectionType</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void InspectionTypeGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var rdInspectionType = e.Row.FindControl("rdInspectionType") as AccelaRadioButton;
                var dataRowView = e.Row.DataItem as DataRowView;
                long selectedInspectionTypeID = string.IsNullOrEmpty(hdInspectionType.Value) ? -1 : long.Parse(hdInspectionType.Value);
                long currentInspectionTypeID = dataRowView != null && dataRowView[InspectionParameter.Keys.TypeID] != null && dataRowView[InspectionParameter.Keys.TypeID] is long ? (long)dataRowView[InspectionParameter.Keys.TypeID] : -2;

                if (currentInspectionTypeID == selectedInspectionTypeID)
                {
                    rdInspectionType.Checked = true;
                }
                else
                {
                    rdInspectionType.Checked = false;
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Initialize the showing optional inspection switch.
        /// </summary>
        private void InitShowingOptionalInspectionSwitch()
        {
            chkShowOptional.Visible = IsACAShowInspectionOptional(ModuleName);

            if (chkShowOptional.Visible)
            {
                // set the default value, checked.
                if (InspectionWizardParameter.ShowOptionalType == null)
                {
                    chkShowOptional.Checked = true;
                }
                else
                {
                    chkShowOptional.Checked = InspectionWizardParameter.ShowOptionalType.Value;
                }
            }
        }

        /// <summary>
        /// Determines whether [is to show optional inspections].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [is to show optional inspections]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsToShowOptionalInspections()
        {
            return chkShowOptional.Visible && chkShowOptional.Checked;
        }

        /// <summary>
        /// Initialize the categories and inspection types.
        /// </summary>
        /// <param name="refreshCurrentCategoryOnly">if set to <c>true</c> [refresh current category only].</param>
        private void InitCategoriesAndInspectionTypes(bool refreshCurrentCategoryOnly)
        {
            //begin get data sources for categories and inspection types
            var recordModel = GetCapModel();
            var recordIDModel = TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID);
            var needToGetOptionalInspections = chkShowOptional.Visible;
            var inspectionTypeViewModels = InspectionTypeViewUtil.GetInspectionTypeViewModels(ModuleName, recordModel, needToGetOptionalInspections, AppSession.User);
            var inspectionCategoryDataModels = InspectionTypeViewUtil.GetInspectionCategories(inspectionTypeViewModels, recordIDModel);
            var optionalInspectionCount = InspectionTypeViewUtil.GetOptionalInspectionCount(inspectionTypeViewModels);

            //bind category drop down list
            if (!refreshCurrentCategoryOnly)
            {
                string selectedCategoryID = ddlCategory != null && ddlCategory.Visible && ddlCategory.Items.Count > 0 ? ddlCategory.SelectedValue : InspectionWizardParameter.Catagory;
                BindCategories(inspectionCategoryDataModels, selectedCategoryID);
            }

            inspectionTypeViewModels = string.IsNullOrEmpty(ddlCategory.SelectedValue) ? inspectionTypeViewModels : InspectionTypeViewUtil.FilterInspectionTypeViewModels(inspectionTypeViewModels, ddlCategory.SelectedValue, IsToShowOptionalInspections());

            //bind inspection types
            string selectedTypeID = hdInspectionType != null && !string.IsNullOrEmpty(hdInspectionType.Value) ? hdInspectionType.Value : InspectionWizardParameter.TypeID;
            BindInspectionTypes(inspectionTypeViewModels, selectedTypeID);

            //set no inspection type notice
            lblNoInspectionTypesFound.Visible = inspectionTypeViewModels.Count == 0;

            //set show optional checkbox
            if (!Page.IsPostBack && (inspectionTypeViewModels.Count == 0 || optionalInspectionCount == 0))
            {
                chkShowOptional.Visible = false;
            }
        }

        /// <summary>
        /// Gets the available inspection types count.
        /// </summary>
        /// <returns>The available inspection types count.</returns>
        private int GetAvailableInspectionTypesCount()
        {
            CapModel4WS recordModel = GetCapModel();
            bool needToGetOptionalInspections = IsACAShowInspectionOptional(ModuleName);

            List<InspectionTypeViewModel> inspectionTypeViewModels = InspectionTypeViewUtil.GetInspectionTypeViewModels(ModuleName, recordModel, needToGetOptionalInspections, AppSession.User);
            
            if (inspectionTypeViewModels != null && inspectionTypeViewModels.Count > 0)
            {
                return inspectionTypeViewModels.Count;
            }

            return 0;
        }

        /// <summary>
        /// Initialize the categories and inspection types.
        /// </summary>
        private void InitInspectionTypes()
        {
            //begin get data sources for inspection types
            var recordModel = GetCapModel();
            var inspectionTypeViewModels = InspectionTypeViewUtil.GetInspectionTypeViewModels(ModuleName, recordModel, IsToShowOptionalInspections(), AppSession.User);

            inspectionTypeViewModels = InspectionTypeViewUtil.FilterInspectionTypeViewModels(inspectionTypeViewModels, ddlCategory.SelectedValue, IsToShowOptionalInspections());
            BindInspectionTypes(inspectionTypeViewModels, InspectionWizardParameter.TypeID);

            //set no inspection type notice
            lblNoInspectionTypesFound.Visible = inspectionTypeViewModels.Count == 0;
        }

        /// <summary>
        /// Bind the categories.
        /// </summary>
        /// <param name="categoryDataModels">The category data models.</param>
        /// <param name="selectedCategoryID">The selected category ID.</param>
        private void BindCategories(List<InspectionCategoryDataModel> categoryDataModels, string selectedCategoryID)
        {
            bool categoryVisible = categoryDataModels != null && categoryDataModels.Count > 0;
            ddlCategory.Visible = categoryVisible;

            if (categoryVisible)
            {
                ddlCategory.DataSource = categoryDataModels;
                ddlCategory.DataValueField = CATEGORY_ID;
                ddlCategory.DataTextField = CATEGORY_TEXT;
                ddlCategory.DataBind();

                if (!string.IsNullOrEmpty(selectedCategoryID))
                {
                    ListItem itemFound = ddlCategory.Items.FindByValue(selectedCategoryID);

                    if (itemFound != null)
                    {
                        ddlCategory.SelectedValue = selectedCategoryID;
                    }
                }
                else
                {
                    ddlCategory.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Bind the inspection types.
        /// </summary>
        /// <param name="inspectionTypeViewModels">The inspection type view models.</param>
        /// <param name="selectedTypeID">The selected type ID.</param>
        private void BindInspectionTypes(List<InspectionTypeViewModel> inspectionTypeViewModels, string selectedTypeID)
        {
            // clear the inspection type value
            hdInspectionType.Value = string.Empty;

            if (inspectionTypeViewModels != null)
            {
                // bind the data source
                DataTable dataTable = CreateDataTable();
                int selectedRowIndex = 0;
                int currentRowIndex = -1;

                foreach (InspectionTypeViewModel model in inspectionTypeViewModels)
                {
                    if (IsToShowOptionalInspections() || model.InspectionTypeDataModel.Required)
                    {
                        currentRowIndex++;
                        DataRow row = dataTable.NewRow();
                        row[InspectionParameter.Keys.Action] = model.InspectionTypeDataModel.AvailableOperations[0];
                        row[InspectionParameter.Keys.Group] = model.InspectionTypeDataModel.Group;
                        row[InspectionParameter.Keys.Units] = model.InspectionTypeDataModel.Units;
                        row[InspectionParameter.Keys.TypeID] = model.TypeID;
                        row[InspectionParameter.Keys.Type] = model.InspectionTypeDataModel.Type;
                        row[InspectionParameter.Keys.TypeText] = model.TypeText;
                        row[InspectionParameter.Keys.ScheduledType] = model.InspectionTypeDataModel.ScheduleType;
                        row[InspectionParameter.Keys.InAdvance] = model.InspectionTypeDataModel.InAdvance;
                        row[InspectionParameter.Keys.Required] = model.InspectionTypeDataModel.Required;
                        row[InspectionParameter.Keys.RescheduleRestrictionSettings] = model.InspectionTypeDataModel.RestrictionSettings4Reschedule;
                        row[InspectionParameter.Keys.CancelRestrictionSettings] = model.InspectionTypeDataModel.RestrictionSettings4Cancel;
                        row[InspectionParameter.Keys.ReadyTimeEnabled] = model.InspectionTypeDataModel.ReadyTimeEnabled;
                        row["Enabled"] = model.Enabled;
                        row["InspTypeLabel"] = string.Format(
                                                        " {0}<span class=\"InspectionTypeStatus\"> ({1})</span>",
                                                        model.TypeText,
                                                        model.RequiredOrOptional.ToLower());

                        bool selected = false;

                        if (!string.IsNullOrEmpty(selectedTypeID) && long.Parse(selectedTypeID) == model.TypeID)
                        {
                            selected = true;
                            selectedRowIndex = currentRowIndex;
                            hdInspectionType.Value = model.TypeID.ToString();
                        }

                        row["Selected"] = selected;

                        dataTable.Rows.Add(row);
                    }
                }

                GridViewDataSource = dataTable;

                BindDataSource(dataTable, selectedRowIndex);

                // bind the data count
                lblAvailableInspections.Text = string.Format(GetTextByKey("aca_inspection_available_lable"), dataTable.Rows.Count);
            }
        }

        /// <summary>
        /// Bind the data source to the inspection type list.
        /// </summary>
        /// <param name="dataTable">The data source whose type is DataTable.</param>
        /// <param name="selectedRowIndex">Index of the selected row.</param>
        private void BindDataSource(DataTable dataTable, int selectedRowIndex)
        {
            int pageIndex = 0;
            if (selectedRowIndex > 0)
            {
                pageIndex = selectedRowIndex / gvInspectionType.PageSize;
            }

            gvInspectionType.DataSource = dataTable;
            gvInspectionType.PageIndex = pageIndex;
            gvInspectionType.DataBind();
            gvInspectionType.Attributes.Add("PageCount", gvInspectionType.PageCount.ToString());                        
            
            /*
             *  when has only one inspection type,
             *    and not postback,
             *    it will skip the "Available Inspection Type" page.
             */
            int inspectionTypesCount = GetAvailableInspectionTypesCount();

            if (!IsPostBack && inspectionTypesCount == 1)
            {
                hdInspectionType.Value = dataTable.Rows[0][InspectionParameter.Keys.TypeID].ToString();
                SkipToNextPage(true);
            }
        }

        /// <summary>
        /// Create the data table structure.
        /// </summary>
        /// <returns>Return the data table structure.</returns>
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.Group, typeof(string)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.TypeID, typeof(long)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.Type, typeof(string)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.TypeText, typeof(string)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.Units, typeof(double)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.Action, typeof(InspectionAction)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.ScheduledType, typeof(InspectionScheduleType)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.InAdvance, typeof(bool)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.Required, typeof(bool)));
            dt.Columns.Add(new DataColumn("InspTypeLabel", typeof(string)));
            dt.Columns.Add(new DataColumn("Enabled", typeof(bool)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.RescheduleRestrictionSettings, typeof(string)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.CancelRestrictionSettings, typeof(string)));
            dt.Columns.Add(new DataColumn(InspectionParameter.Keys.ReadyTimeEnabled, typeof(bool)));
            dt.Columns.Add(new DataColumn("Selected", typeof(bool)));

            return dt;
        }

        /// <summary>
        /// Set the admin side UI
        /// </summary>
        private void SetAdminUI()
        {
            if (AppSession.IsAdmin)
            {
                ddlCategory.Visible = false;

                chkShowOptional.Visible = true;
                chkShowOptional.Checked = true;
                chkShowOptional.AutoPostBack = false;

                lblNoInspectionTypesFound.Visible = true;
            }
        }

        /// <summary>
        /// Set the Continue button status.
        /// </summary>
        private void SetContinueButtonStatus()
        {
            bool wizardButtonDisabled = string.IsNullOrEmpty(hdInspectionType.Value);
            SetWizardButtonDisable(lnkContinue.ClientID, wizardButtonDisabled);
        }

        /// <summary>
        /// Skip to next page.
        /// </summary>
        /// <param name="isSingleType">is single type or not.</param>
        private void SkipToNextPage(bool isSingleType = false)
        {
            InspectionParameter inspectionParameter = InspectionWizardParameter;
            inspectionParameter.TypeID = hdInspectionType.Value;
            inspectionParameter.Catagory = ddlCategory.SelectedValue;
            inspectionParameter.ShowOptionalType = IsToShowOptionalInspections();
            inspectionParameter.IsFromWizardPage = true;
            inspectionParameter.ScheduledDateTime = null;

            foreach (DataRow row in GridViewDataSource.Rows)
            {
                if (row[InspectionParameter.Keys.TypeID].ToString() == hdInspectionType.Value)
                {
                    inspectionParameter.Group = row[InspectionParameter.Keys.Group].ToString();
                    inspectionParameter.Units = (double)row[InspectionParameter.Keys.Units];
                    inspectionParameter.Type = row[InspectionParameter.Keys.Type].ToString();
                    inspectionParameter.TypeText = row[InspectionParameter.Keys.TypeText].ToString();
                    inspectionParameter.Action = (InspectionAction)row[InspectionParameter.Keys.Action];
                    inspectionParameter.ScheduleType = (InspectionScheduleType)row[InspectionParameter.Keys.ScheduledType];
                    inspectionParameter.InAdvance = (bool)row[InspectionParameter.Keys.InAdvance];
                    inspectionParameter.Required = (bool)row[InspectionParameter.Keys.Required];
                    inspectionParameter.RescheduleRestrictionSettings = row[InspectionParameter.Keys.RescheduleRestrictionSettings].ToString();
                    inspectionParameter.CancelRestrictionSettings = row[InspectionParameter.Keys.CancelRestrictionSettings].ToString();
                    inspectionParameter.ReadyTimeEnabled = (bool)row[InspectionParameter.Keys.ReadyTimeEnabled];

                    break;
                }
            }

            string url = "InspectionWizardInputDateTime.aspx";
            var isReadyTimeEnabled = inspectionParameter.ReadyTimeEnabled != null && inspectionParameter.ReadyTimeEnabled.Value == true;

            if (inspectionParameter.ScheduleType == InspectionScheduleType.RequestOnlyPending && !isReadyTimeEnabled)
            {
                url = "InspectionWizardInputLocation.aspx";
            }

            url = string.Format("{0}?{1}", url, Request.QueryString.ToString());
            url = InspectionParameterUtil.UpdateURLAndSaveParameters(url, inspectionParameter);
            url += string.Format("&{0}={1}", UrlConstant.IS_SINGLE_TYPE, isSingleType ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);

            Response.Redirect(url);
        }

        #endregion Private Methods
    }
}
