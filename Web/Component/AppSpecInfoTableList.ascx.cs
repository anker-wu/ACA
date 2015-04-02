#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: AppSpecInfoTableList.ascx.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: List control for ASI Table.
*
*  Notes:
* $Id: AppSpecInfoTableList.ascx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Aug 9, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Application Specific Information List
    /// </summary>
    public partial class AppSpecInfoTableList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Attribute key for keeping the table-key info.
        /// </summary>
        private const string TABLE_KEY = "tableKey";

        /// <summary>
        /// Attribute key for indicating the table whether is readonly.
        /// </summary>
        private const string IS_ASIT_READONLY_KEY = "isASITReadonly";

        /// <summary>
        /// Attribute key for indicating the edit button whether is display or not.
        /// </summary>
        private const string BUTTON_EDIT_DISPLAY = "buttonEditDisplay";

        /// <summary>
        /// Attribute key for indicating the delete button whether is display or not.
        /// </summary>
        private const string BUTTON_DELETE_DISPLAY = "buttonDeleteDisplay";

        /// <summary>
        /// Show the error message for table rows validate failed.
        /// </summary>
        private const string IS_SHOW_VALIDATION_MSG = "showValidationMsg";

        /// <summary>
        /// Collect update panel ID for all table.
        /// </summary>
        private string _asitUpdatePanelIDs = string.Empty;

        /// <summary>
        /// Collect UI data table key for all table.
        /// </summary>
        private string _asitUIDataTableKeys = string.Empty;

        /// <summary>
        /// UI data key
        /// </summary>
        private string _uiDataKey;

        /// <summary>
        /// UI data values
        /// </summary>
        private ASITUITable[] _uiData;

        /// <summary>
        /// ASI tables
        /// </summary>
        private AppSpecificTableModel4WS[] _asiTables;

        /// <summary>
        /// The table key collection used to indicates which tables need to update.
        /// </summary>
        private IEnumerable<string> _tableKeysNeedToUpdate;

        /// <summary>
        /// Is ASIT row fields value need to validate
        /// </summary>
        private bool _isNeedValidate = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether need to validate all required fields for ASIT and generic template table.
        /// Default is true, sometimes the validation is not need, such as when an examination in pending status.
        /// </summary>
        public bool NeedValidate
        {
            get
            {
                return _isNeedValidate;
            }
            
            set
            {
                _isNeedValidate = value;
            }
        }

        /// <summary>
        /// Gets or sets ASIT UI Data key
        /// </summary>
        public string ASITUIDataKey
        {
            get
            {
                return _uiDataKey;
            }

            set
            {
                _uiDataKey = value;
            }
        }

        /// <summary>
        /// Gets UI tables.
        /// </summary>
        public ASITUITable[] UITables
        {
            get
            {
                return _uiData;
            }
        }

        /// <summary>
        /// Gets the list of sub cap models
        /// </summary>
        public IDictionary<string, CapModel4WS> SubCapModels
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is template table control.
        /// </summary>
        public bool IsTemplateTable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the template table is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the asit edit pop page section info(Format:title/index )
        /// </summary>
        public string SectionInfo
        {
            private get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the table list is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                foreach (RepeaterItem repeater in rptASITableList.Items)
                {
                    AccelaGridView currentGridView = repeater.FindControl("gdvASITable") as AccelaGridView;

                    if (currentGridView != null && currentGridView.Rows.Count > 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether display Hijri Calendar 
        /// </summary>
        private bool IsDisplayHijriCalendar
        {
            get
            {
                return !IsTemplateTable && StandardChoiceUtil.IsEnableDisplayISLAMICCalendar();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays the specified groups.
        /// </summary>
        /// <param name="groups">The groups.</param>
        public void Display(AppSpecificTableGroupModel4WS[] groups)
        {
            IList<AppSpecificTableGroupModel4WS> allVisibleGroups = new List<AppSpecificTableGroupModel4WS>();
            _asiTables = CapUtil.GetAllVisibleASITables(ModuleName, groups, allVisibleGroups);

            SubCapModels = CapUtil.GetSubCapsByASITGroups(ModuleName, allVisibleGroups);

            if (!IsPostBack)
            {
                _uiData = ASITUIModelUtil.ConvertASITablesToUITables(_asiTables, ModuleName, true, SectionInfo);
                UIModelUtil.SetDataToUIContainer(_uiData, UIDataType.ASIT, _uiDataKey);
            }
            else
            {
                SyncUIData(out _uiData);

                ASITUIModelUtil.SyncInputValueToASITBizModel(_uiData, _asiTables);
            }
        }

        /// <summary>
        /// Displays the specified groups.
        /// </summary>
        /// <param name="templateModel">The groups.</param>
        /// <param name="isLoadViewState">indicating whether is load view state event</param>
        /// <param name="tableKeysNeedToUpdate">Table key collection used to indicates which tables need to update.</param>
        public void Display(TemplateModel templateModel, bool isLoadViewState, IEnumerable<string> tableKeysNeedToUpdate)
        {
            TemplateGroup[] groups = templateModel.templateTables;
            TemplateSubgroup[] templateSubgroups = CapUtil.GetGenericTemplateTables(ModuleName, groups);
            _tableKeysNeedToUpdate = tableKeysNeedToUpdate;

            if (!isLoadViewState)
            {
                _uiData = ASITUIModelUtil.ConvertTemplateTablesToUITables(templateSubgroups, ModuleName, _uiDataKey, SectionInfo);
                UIModelUtil.SetDataToUIContainer(_uiData, UIDataType.ASIT, _uiDataKey);
            }
            else
            {
                SyncUIData(out _uiData);
                ASITUIModelUtil.SyncInputValueToTemplateTableModel(_uiData, templateSubgroups);
            }

            // Bind table data for Generic Template Table, Generic Template Table data is initialized after Load View state.
            BindTableList();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Page load event handler.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DialogUtil.RegisterScriptForDialog(Page);

            /*
             * Bind table data for ASIT, since ASIT data is initialized in CapEdit.OnInit method.
             * Need bind data after the ViewState loaded in order to get the correct page index.
             */
            if (!IsTemplateTable)
            {
                BindTableList();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AppSession.IsAdmin)
            {
                //Register upload-panel ID to client for partial update.
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "asitUpdatePanelIDs", "\r\nvar asitUpdatePanelIDs = new Object();\r\n", true);

                if (!Page.ClientScript.IsStartupScriptRegistered("asitUIDataKeys"))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "asitUIDataKeys", "var asitUIDataKeys = new Object();\r\n", true);
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), this.UniqueID, _asitUpdatePanelIDs + _asitUIDataTableKeys + "\r\n", true);
                RegisterScripts();
            }
        }

        /// <summary>
        /// Handles the ItemCommand event of the ASITableList control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterCommandEventArgs"/> instance containing the event data.</param>
        protected void ASITableList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            AccelaGridView gdvASITable = e.Item.FindControl("gdvASITable") as AccelaGridView;
            AccelaHideLink lnkFocusAnchor = e.Item.FindControl("lnkFocusAnchor") as AccelaHideLink;

            string[] args = e.CommandArgument.ToString().Split(ACAConstant.SPLIT_CHAR);
            string tableKey = args[0];
            bool isForSingleEdit = args.Length > 1;
            int selectedDataRowIndex = isForSingleEdit ? int.Parse(args[1]) : -1;
            long[] selectedIndexes = null;

            //Control IDs for set focus for Section 508 functionality.
            string editTargetControlID = string.Empty;
            string deleteTargetControlID = string.Empty;

            if (isForSingleEdit)
            {
                selectedIndexes = new long[] { selectedDataRowIndex };

                //Focus on Actions menu after single item updated.
                GridViewRow currentRow = gdvASITable.Rows[int.Parse(args[2])];
                PopupActions actionMenu = currentRow.FindControl("paActionMenu") as PopupActions;

                editTargetControlID = actionMenu.ActionsLinkClientID;

                //Focus on hidden anchor before the ASIT Table after single item deleted.
                deleteTargetControlID = lnkFocusAnchor.ClientID;
            }
            else
            {
                DataTable selectedData = gdvASITable.GetSelectedData(gdvASITable.DataSource as DataTable);
                selectedIndexes = GetDataSelectedIndex(selectedData);

                /*
                 * Section 508:
                 * 1. After click the "Edit Selected" button will pop up the Edit form and transfer the ClientID of "Edit Selected" button,
                 *    after the Edit form submitted, will focus on the "Edit Selected" button based on the ClientID.
                 * 2. After deleted selected rows - focus on "Delete Selected" button.
                 */
                editTargetControlID = deleteTargetControlID = ((Control)e.CommandSource).ClientID;
            }

            switch (e.CommandName)
            {
                case "Edit":
                case "EditCurrentRow":
                    if (selectedIndexes.Length > 0)
                    {
                        CreateEditUIData(tableKey, selectedIndexes);

                        string scripts = "ASIT_EditRow(['" + tableKey + "'],['" + _uiDataKey + "'],'" + editTargetControlID + "');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "EditASITRow", scripts, true);
                    }

                    break;
                case "Delete":
                case "DeleteCurrentRow":
                    if (selectedIndexes.Length > 0)
                    {
                        DeleteSelectedRows(tableKey, selectedIndexes);

                        //Sec focus after delete some items.
                        Page.FocusElement(deleteTargetControlID);
                    }

                    break;
            }
        }

        /// <summary>
        /// Row data bound event handler
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected void ASITableList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ASITUITable asiTable = e.Item.DataItem as ASITUITable;

                if (asiTable == null)
                {
                    return;
                }

                //Render table name and sub group instructions
                Literal litTableInfo = e.Item.FindControl("litTableInfo") as Literal;
                string drillDownMsgID = string.Empty;

                if (litTableInfo != null)
                {
                    bool isSuperCAP = false;

                    if (!string.IsNullOrEmpty(ModuleName))
                    {
                        isSuperCAP = CapUtil.IsSuperCAP(ModuleName);
                    }

                    string tableName = StringUtil.GetString(asiTable.AlternativeLabel, asiTable.TableTitle);

                    StringBuilder tableHeader = new StringBuilder();

                    tableHeader.Append("<div class='ACA_TabRow'><h1><i>");

                    if (!IsTemplateTable && isSuperCAP)
                    {
                        //Dispaly Logo
                        string logoDesc = string.Format("{0} ({1})", tableName, asiTable.AgencyCode);
                        string agencyLogo = CapUtil.GetAgencyLogoHtml(asiTable.AgencyCode, ModuleName);

                        tableHeader.Append(agencyLogo);
                        tableHeader.Append("<div class='ACA_ValCal_Title'>");
                        tableHeader.Append(logoDesc);
                        tableHeader.Append("</div>");
                        tableHeader.Append("</i></h1></div>");
                    }
                    else
                    {
                        tableHeader.Append(tableName);
                        tableHeader.Append("</i></h1></div>");
                    }

                    //Display instruction for ASIT
                    string instruction = asiTable.Instruction;
                    instruction = string.IsNullOrEmpty(instruction) ?
                        string.Empty :
                        string.Format("<div class=\"ACA_Section_Instruction ACA_Section_Instruction_FontSize\">{0}</div>", instruction);

                    //Display Error Message for Drill Down
                    string drillDownMsg = string.Empty;

                    if (asiTable.HasDrillDownData)
                    {
                        drillDownMsgID = litTableInfo.ClientID + "_msg";
                        drillDownMsg = string.Format("<div class=\"ACA_TabRow ACA_Hide\"><span id=\"{0}\" name=\"{0}\"></span></div>", drillDownMsgID);
                    }

                    litTableInfo.Text = tableHeader + instruction + drillDownMsg;
                }

                //Bind Data to ASI Table
                AccelaGridView gdvASITable = e.Item.FindControl("gdvASITable") as AccelaGridView;
                bool buttonAddDisplay = !ValidationUtil.IsNo(asiTable.ButtonAddDisplay);
                bool buttonEditDisplay = !ValidationUtil.IsNo(asiTable.ButtonEditDisplay);
                bool buttonDeleteDisplay = !ValidationUtil.IsNo(asiTable.ButtonDeleteDisplay);
                gdvASITable.AutoGenerateColumns = true;

                /*
                 * Add a virtual Gview ID to force the AccelaGridView control to call the IAccelaControlRender.OnPreRender method to generate the
                 *  Validation extender control, the logic is located in the OnPrender Method in the AccelaGridView.cs
                 */
                gdvASITable.GridViewNumber = this.ClientID;

                if (gdvASITable != null)
                {
                    if (IsTemplateTable)
                    {
                        gdvASITable.IsAutoWidth = true;
                    }

                    gdvASITable.Attributes[BUTTON_EDIT_DISPLAY] = Convert.ToString(buttonEditDisplay);
                    gdvASITable.Attributes[BUTTON_DELETE_DISPLAY] = Convert.ToString(buttonDeleteDisplay);
                    gdvASITable.Attributes[TABLE_KEY] = asiTable.TableKey;
                    gdvASITable.Attributes[IS_ASIT_READONLY_KEY] = Convert.ToString(asiTable.IsReadOnly);
                    gdvASITable.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
                    gdvASITable.DataSource = ASITUIModelUtil.ConvertASITableToDataTable(asiTable, IsDisplayHijriCalendar);
                    gdvASITable.DataBind();
                }

                //Bind arguments to action buttons
                var divActionRowHolder = e.Item.FindControl("divActionRowHolder") as HtmlGenericControl;
                AccelaSplitButton btnAdd = e.Item.FindControl("btnAdd") as AccelaSplitButton;
                AccelaButton btnEdit = e.Item.FindControl("btnEdit") as AccelaButton;
                AccelaButton btnDelete = e.Item.FindControl("btnDelete") as AccelaButton;
                UpdatePanel tablePanel = e.Item.FindControl("tablePanel") as UpdatePanel;

                if (_tableKeysNeedToUpdate != null && _tableKeysNeedToUpdate.Contains(asiTable.TableKey))
                {
                    tablePanel.Update();
                }

                divActionRowHolder.Visible = !asiTable.IsReadOnly && !IsReadOnly;

                if (btnAdd != null && buttonAddDisplay)
                {
                    //Generate add button attributes.
                    string scriptsPattern = "ASIT_AddRow(['{0}'],{1},{2},['{3}'],'{4}');return false;";
                    btnAdd.OnClientClick = string.Format(scriptsPattern, asiTable.TableKey, 1, "false", _uiDataKey, btnAdd.ClientID);

                    if (!string.IsNullOrEmpty(asiTable.ButtonAddLabel))
                    {
                        btnAdd.Text = asiTable.ButtonAddLabel;
                    }
                    else
                    {
                        btnAdd.LabelKey = "aca_asit_label_addarow";
                    }

                    if (!asiTable.HasDrillDownData)
                    {
                        ActionViewModel[] actions = new ActionViewModel[9];

                        string buttonAddMoreLabel = (!string.IsNullOrEmpty(asiTable.ButtonAddMoreLabel) && asiTable.ButtonAddMoreLabel.IndexOf("{X}", StringComparison.OrdinalIgnoreCase) > -1)
                                                            ? asiTable.ButtonAddMoreLabel
                                                            : GetTextByKey("aca_asit_label_addrowspattern");

                        buttonAddMoreLabel = buttonAddMoreLabel.Replace("{x}", "{0}").Replace("{X}", "{0}");

                        for (int i = 0; i < actions.Length; i++)
                        {
                            actions[i] = new ActionViewModel();
                            actions[i].ActionLabel = string.Format(buttonAddMoreLabel, i + 2);
                            actions[i].ClientEvent = string.Format(scriptsPattern, asiTable.TableKey, i + 2, "false", _uiDataKey, btnAdd.ClientID);
                        }

                        btnAdd.MenuItems = actions;
                    }

                    btnAdd.Visible = true;
                }

                if (btnEdit != null && gdvASITable != null && buttonEditDisplay)
                {
                    if (!string.IsNullOrEmpty(asiTable.ButtonEditLabel))
                    {
                        btnEdit.Text = asiTable.ButtonEditLabel;
                    }
                    else
                    {
                        btnEdit.LabelKey = "aca_asit_label_editselected";
                    }

                    btnEdit.OnClientClick = "return ASIT_EditSelectedClientClick('" + gdvASITable.GetSelectedItemsFieldClientID() + "','" + drillDownMsgID + "'," + asiTable.HasDrillDownData.ToString().ToLower() + ");";
                    btnEdit.CommandName = "Edit";
                    btnEdit.CommandArgument = asiTable.TableKey;
                    btnEdit.Visible = true;
                }

                if (btnDelete != null && gdvASITable != null && buttonDeleteDisplay)
                {
                    if (!string.IsNullOrEmpty(asiTable.ButtonDeleteLabel))
                    {
                        btnDelete.Text = asiTable.ButtonDeleteLabel;
                    }
                    else
                    {
                        btnDelete.LabelKey = "aca_asit_label_deleteselected";
                    }

                    btnDelete.OnClientClick = "return ASIT_DeleteSelectedClientClick('" + gdvASITable.GetSelectedItemsFieldClientID() + "');";
                    btnDelete.CommandName = "Delete";
                    btnDelete.CommandArgument = asiTable.TableKey;
                    btnDelete.Visible = true;
                }

                //Collect upload-panel ID for partial update.
                _asitUpdatePanelIDs += "asitUpdatePanelIDs['" + asiTable.TableKey + "'] = '" + tablePanel.UniqueID + "';\r\n";
                _asitUIDataTableKeys += "asitUIDataKeys['" + asiTable.TableKey + "'] = '" + _uiDataKey + "';\r\n";
            }
        }

        /// <summary>
        /// Handles the OnPreRender event of the ASITable list control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ASITableList_PreRender(object sender, EventArgs e)
        {
            var currentRepeater = sender as Repeater;

            if (currentRepeater == null || currentRepeater.Items.Count <= 0)
            {
                return;
            }

            foreach (RepeaterItem currentRepeaterItem in currentRepeater.Items)
            {
                AccelaGridView currentGridView = currentRepeaterItem.FindControl("gdvASITable") as AccelaGridView;
                HtmlGenericControl divActionRowHolder = currentRepeaterItem.FindControl("divActionRowHolder") as HtmlGenericControl;
                HtmlGenericControl divIncompleteMark = currentRepeaterItem.FindControl("divIncompleteMark") as HtmlGenericControl;

                //Show/Hide the action bar when table is readonly.
                if (currentGridView != null && divActionRowHolder != null)
                {
                    var isASITReadonly = IsASITReadonly(currentGridView) || IsReadOnly;
                    divActionRowHolder.Visible = !isASITReadonly;
                }

                if (ValidationUtil.IsYes(currentGridView.Attributes[IS_SHOW_VALIDATION_MSG]))
                {                    
                    divIncompleteMark.Visible = true;
                }
                else
                {
                    divIncompleteMark.Visible = false;
                }

                currentGridView.Attributes.Remove(IS_SHOW_VALIDATION_MSG);
            }
        }

        /// <summary>
        /// Row data bound event handler
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected void ASITable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var currentGridView = sender as AccelaGridView;
            var currentRow = e.Row;

            if (currentRow.RowType == DataControlRowType.DataRow && currentGridView != null)
            {
                var tableKey = currentGridView.Attributes[TABLE_KEY];
                var keyColumnIndex = (currentGridView.DataSource as DataTable).Columns[ASITUITable.RowIdentityColumnName].Ordinal;
                var dataRowKeyIndex = Convert.ToInt32((currentRow.DataItem as DataRowView)[keyColumnIndex]);

                BuildActionMenu(currentRow, dataRowKeyIndex, currentGridView);
               
                ASITUITable currentUITable = _uiData.Where(t => string.Equals(t.TableKey, tableKey)).SingleOrDefault();
                UIRow row = currentUITable.Rows.Where(r => r.RowIndex == dataRowKeyIndex).SingleOrDefault();
                bool hasEmptyRequiredField = false;
                HtmlGenericControl divImg = (HtmlGenericControl)e.Row.FindControl("divImg");

                if (NeedValidate)
                {
                    foreach (ASITUIField field in row.Fields)
                    {
                        if (field.IsRequired && !field.IsHidden && !field.IsHiddenByExp && !field.IsReadOnly && !field.IsReadOnlyByExp && string.IsNullOrEmpty(field.Value))
                        {
                            hasEmptyRequiredField = true;
                            break;
                        }
                    }
                }

                if (hasEmptyRequiredField)
                {
                    divImg.Visible = true;
                    currentGridView.Attributes[IS_SHOW_VALIDATION_MSG] = ACAConstant.COMMON_Y;
                    currentGridView.IsForceValidation = true;
                    currentGridView.ErrorMessage = GetTextByKey("aca_asit_msg_validate_required_error");
                }
                else
                {
                    divImg.Visible = false;
                }
            }
        }

        /// <summary>
        /// Handles the OnPreRender event of the ASITable control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ASITable_PreRender(object sender, EventArgs e)
        {
            var currentGridView = sender as GridView;

            MoveActionMenuColumn(currentGridView);

            HideSomeColumns(currentGridView);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Update UI data.
        /// </summary>
        /// <param name="_uiData">ASIT UI table</param>
        private void SyncUIData(out ASITUITable[] _uiData)
        {
            _uiData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new string[] { _uiDataKey }) as ASITUITable[];
            var asitCopyUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITCopy, new string[] { _uiDataKey }) as ASITUITable[];

            if (asitCopyUIData != null &&
                Request.Form[Page.postEventSourceID] != null &&
                Request.Form[Page.postEventSourceID].Contains(ASITUIModelUtil.ASIT_POSTEVENT_SYNCUICOPYDATA))
            {
                ASITUIModelUtil.SyncASITUIRowData(asitCopyUIData, _uiData);
                UIModelUtil.SetDataToUIContainer(null, UIDataType.ASITCopy, _uiDataKey);
                UIModelUtil.SetDataToUIContainer(_uiData, UIDataType.ASIT, _uiDataKey);
            }

            //Focus on the "Add a Row" or "Edit Selected" button after the Edit Form submitted.
            Page.FocusElement(Request.Form["__LASTFOCUS_ID"]);
        }

        /// <summary>
        /// Bind ASI Table list to list control.
        /// </summary>
        private void BindTableList()
        {
            rptASITableList.DataSource = _uiData;
            rptASITableList.DataBind();
        }

        /// <summary>
        /// Builds the action menu.
        /// </summary>
        /// <param name="currentRow">Grid view row.</param>
        /// <param name="dataRowKeyIndex">Index of the data row key.</param>
        /// <param name="currentGridView">the grid view</param>
        private void BuildActionMenu(GridViewRow currentRow, int dataRowKeyIndex, AccelaGridView currentGridView)
        {
            PopupActions actionMenu = currentRow.FindControl("paActionMenu") as PopupActions;
            var btnEditCurrentRow = currentRow.FindControl("btnEditCurrentRow") as AccelaButton;
            var btnDeleteCurrentRow = currentRow.FindControl("btnDeleteCurrentRow") as AccelaButton;

            if (dataRowKeyIndex <= -1 || actionMenu == null)
            {
                return;
            }

            var tableKey = currentGridView.Attributes[TABLE_KEY];
            var buttonEditDisplay = currentGridView.Attributes[BUTTON_EDIT_DISPLAY];
            var buttonDeleteDisplay = currentGridView.Attributes[BUTTON_DELETE_DISPLAY];

            var actionViewModelList = new List<ActionViewModel>();
            string commandArgument = tableKey + ACAConstant.SPLIT_CHAR + dataRowKeyIndex + ACAConstant.SPLIT_CHAR + currentRow.RowIndex;

            if (btnEditCurrentRow != null && bool.Parse(buttonEditDisplay))
            {
                btnEditCurrentRow.CommandArgument = commandArgument;
                string scriptsPattern = "invokeClick(document.getElementById('{0}'),true)";

                var actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_asit_label_action_edit");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_edit.png");
                actionView.ClientEvent = string.Format(scriptsPattern, btnEditCurrentRow.ClientID);
                actionViewModelList.Add(actionView);
            }

            if (btnDeleteCurrentRow != null && bool.Parse(buttonDeleteDisplay))
            {
                btnDeleteCurrentRow.CommandArgument = commandArgument;
                string confirmMessage = GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'");
                string deleteScriptsPattern = "if(confirmMsg('{0}')) invokeClick(document.getElementById('{1}'),true);";

                var actionView = new ActionViewModel();
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_asit_label_action_delete");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
                actionView.ClientEvent = string.Format(deleteScriptsPattern, confirmMessage, btnDeleteCurrentRow.ClientID);
                actionViewModelList.Add(actionView);
            }

            if (actionViewModelList.Count > 0)
            {
                actionMenu.ActionLableKey = "aca_asit_label_action";
                actionMenu.AvailableActions = actionViewModelList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Creates the edit UI data.
        /// </summary>
        /// <param name="tableKey">Key of the table.</param>
        /// <param name="selectedRowIndexes">The selected row indexes.</param>
        private void CreateEditUIData(string tableKey, long[] selectedRowIndexes)
        {
            ASITUITable asitUITable = _uiData.Where(t => t.TableKey == tableKey).FirstOrDefault();
            ASITUITable editTable = null;

            if (asitUITable != null)
            {
                editTable = ObjectCloneUtil.DeepCopy(asitUITable);
                var selectedRows = editTable.Rows.Where(r => selectedRowIndexes.Contains(r.RowIndex));
                editTable.Rows = selectedRows.ToList();
            }
            
            UIModelUtil.SetDataToUIContainer(new[] { editTable }, UIDataType.ASITEdit, _uiDataKey);
        }

        /// <summary>
        /// Deletes the selected rows.
        /// </summary>
        /// <param name="tableKey">Key of the table.</param>
        /// <param name="selectedRowIndexes">The selected row indexes.</param>
        private void DeleteSelectedRows(string tableKey, long[] selectedRowIndexes)
        {
            ASITUITable asitUITable = _uiData.Where(t => t.TableKey == tableKey).FirstOrDefault();

            if (asitUITable != null)
            {
                // if delete current rows, it must remove expression fields from Expression_InputVariables session.
                List<string> removeKeys4InputVarsFromSession = new List<string>();

                foreach (int idx in selectedRowIndexes)
                {
                    for (int i = 0; i < asitUITable.Rows.Count; i++)
                    {
                        if (asitUITable.Rows[i].RowIndex == idx)
                        {
                            string fieldPrefix = string.Format("ASIT_{0}_{1}_", tableKey, asitUITable.Rows[i].RowIndex);
                            removeKeys4InputVarsFromSession.Add(fieldPrefix);
                            asitUITable.Rows.Remove(asitUITable.Rows[i]);
                        }
                    }
                }

                ExpressionUtil.RemoveInputVariablesFromSession(removeKeys4InputVarsFromSession);

                ASITUIModelUtil.SyncASITUIRowData(new ASITUITable[] { asitUITable }, _uiData);

                if (!IsTemplateTable)
                {
                    ASITUIModelUtil.SyncInputValueToASITBizModel(_uiData, _asiTables);
                }

                UIModelUtil.SetDataToUIContainer(_uiData, UIDataType.ASIT, _uiDataKey);

                BindTableList();
            }
        }

        /// <summary>
        /// Gets the index of the data selected.
        /// </summary>
        /// <param name="selectedRows">The selected rows.</param>
        /// <returns>
        /// the index of the data selected.
        /// </returns>
        private long[] GetDataSelectedIndex(DataTable selectedRows)
        {
            var idxes = from row in selectedRows.AsEnumerable()
                        let rowIdx = Convert.ToInt64(row.Field<int>(ASITUITable.RowIdentityColumnName))
                        orderby rowIdx
                        select rowIdx;
            return idxes.ToArray();
        }

        /// <summary>
        /// Hides some columns.
        /// </summary>
        /// <param name="currentGridView">The current grid view.</param>
        private void HideSomeColumns(GridView currentGridView)
        {
            // currentRow.Cells.Count - lastPosition1 is the GridRowID field position.
            int lastPosition1 = 2;

            // currentRow.Cells.Count - lastPosition2 is the RowIndex field position.  
            int lastPosition2 = 3;
            int checkboxIndex = 0;

            if (currentGridView.Controls.Count > 0 && currentGridView.Controls[0].Controls.Count > 0 && currentGridView.Controls[0].Controls[0] is GridViewRow)
            {
                var currentRow = currentGridView.Controls[0].Controls[0] as GridViewRow;

                if (currentRow != null && currentRow.RowType == DataControlRowType.Header && currentRow.Cells.Count >= 2)
                {
                    currentRow.Cells[currentRow.Cells.Count - lastPosition1].Visible = false;
                    currentRow.Cells[currentRow.Cells.Count - lastPosition2].Visible = false;
                }
            }

            if (currentGridView.Rows.Count > 0)
            {
                var buttonEditDisplay = currentGridView.Attributes[BUTTON_EDIT_DISPLAY];
                var buttonDeleteDisplay = currentGridView.Attributes[BUTTON_DELETE_DISPLAY];
                bool actionMenuVisible = !IsASITReadonly(currentGridView) && !IsReadOnly && (bool.Parse(buttonEditDisplay) || bool.Parse(buttonDeleteDisplay));

                currentGridView.HeaderRow.Cells[checkboxIndex].Visible = actionMenuVisible;

                foreach (GridViewRow currentRow in currentGridView.Rows)
                {
                    if (currentRow.RowType == DataControlRowType.DataRow && currentRow.Cells.Count >= 2)
                    {
                        currentRow.Cells[currentRow.Cells.Count - lastPosition1].Visible = false;
                        currentRow.Cells[currentRow.Cells.Count - lastPosition2].Visible = false;
                    }

                    currentRow.Cells[checkboxIndex].Visible = actionMenuVisible;

                    PopupActions actionMenu = currentRow.FindControl("paActionMenu") as PopupActions;

                    if (actionMenu != null)
                    {
                        actionMenu.Visible = actionMenuVisible;
                    }
                }
            }
        }

        /// <summary>
        /// Checks the and apply readonly property for all action elements except GridView checkbox which is handled before data bind.
        /// </summary>
        /// <param name="currentGridView">The current grid view.</param>
        /// <returns>
        /// <c>true</c> if [is ASIT readonly] [the specified current grid view]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsASITReadonly(GridView currentGridView)
        {
            var isASITReadonlyString = currentGridView.Attributes[IS_ASIT_READONLY_KEY];
            var result = string.IsNullOrEmpty(isASITReadonlyString) ? false : bool.Parse(isASITReadonlyString);

            return result;
        }

        /// <summary>
        /// Moves the action menu column.
        /// </summary>
        /// <param name="currentGridView">The current row.</param>
        private void MoveActionMenuColumn(GridView currentGridView)
        {
            int actionPosition = currentGridView.Rows.Count == 0 ? 0 : 2;

            if (currentGridView.Controls.Count > 0 && currentGridView.Controls[0].Controls.Count > 0 && currentGridView.Controls[0].Controls[0] is GridViewRow)
            {
                var currentRow = currentGridView.Controls[0].Controls[0] as GridViewRow;

                if (currentRow.RowType == DataControlRowType.Header && currentRow.Cells.Count > 1)
                {
                    TableCell tempCell = currentRow.Cells[actionPosition];

                    // remove the column and append to the end
                    currentRow.Cells.Remove(tempCell);
                    currentRow.Cells.Add(tempCell);
                }
            }

            foreach (GridViewRow currentRow in currentGridView.Rows)
            {
                if (currentRow.RowType == DataControlRowType.DataRow && currentRow.Cells.Count > 1)
                {
                    TableCell tempCell = currentRow.Cells[actionPosition];
                    currentRow.Cells.Remove(tempCell);
                    currentRow.Cells.Add(tempCell);
                }
            }
        }

        /// <summary>
        /// Register the support scripts for ASIT and generic template table.
        /// </summary>
        private void RegisterScripts()
        {
            if (!Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), "ASITScripts"))
            {
                StringBuilder sbScript = new StringBuilder();

                //ASIT_AddRow
                sbScript.Append("\r\nfunction ASIT_AddRow(param, rowQty, isExpression, uiDataKey, senderId) {\r\n");
                sbScript.Append("var url = '';\r\n");
                sbScript.AppendFormat("url += '{0}';\r\n", Page.ResolveUrl("~/ASIT/DrillDown.aspx"));
                sbScript.AppendFormat("url += '?module={0}&action=add';\r\n", ModuleName);
                sbScript.Append("url += '&param=' + escape(Sys.Serialization.JavaScriptSerializer.serialize(param));\r\n");
                sbScript.Append("url += '&qty=' + rowQty;\r\n");
                sbScript.AppendFormat("url += '&{0}={1}';\r\n", ACAConstant.IS_SUBAGENCY_CAP, Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]);
                sbScript.Append("if (isExpression) {url += '&isExp=y';}\r\n");
                sbScript.Append("if (uiDataKey) {\r\n");
                sbScript.Append("url += '&uikey=' + Sys.Serialization.JavaScriptSerializer.serialize(uiDataKey);\r\n");
                sbScript.Append("}\r\n");
                sbScript.Append("ACADialog.popup({ url: url, width: 730, objectTarget: senderId });\r\n");
                sbScript.Append("}\r\n");

                //ASIT_EditRow
                sbScript.Append("function ASIT_EditRow(param, uiDataKey, senderId) {\r\n");
                sbScript.Append("var url = '';\r\n");
                sbScript.AppendFormat("url += '{0}';\r\n", Page.ResolveUrl("~/ASIT/DrillDown.aspx"));
                sbScript.AppendFormat("url += '?module={0}&action=edit';\r\n", ModuleName);
                sbScript.Append("url += '&param=' + escape(Sys.Serialization.JavaScriptSerializer.serialize(param));\r\n");
                sbScript.Append("url += '&uikey=' + Sys.Serialization.JavaScriptSerializer.serialize(uiDataKey);\r\n");
                sbScript.AppendFormat("url += '&{0}={1}';\r\n", ACAConstant.IS_SUBAGENCY_CAP, Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]);
                sbScript.Append("ACADialog.popup({ url: url, width: 730, objectTarget: senderId });\r\n");
                sbScript.Append("}\r\n");

                //ASIT_EditSelectedClientClick
                sbScript.Append("function ASIT_EditSelectedClientClick(gridSelectedFieldID, msgID, hasDrillDownData) {\r\n");
                sbScript.Append("var gridSelectedHiddenField = $get(gridSelectedFieldID);\r\n");
                sbScript.Append("var valWithoutComma = gridSelectedHiddenField.value.replace(/\\,/g, '');\r\n");
                sbScript.Append("if (/^\\s*$/.test(valWithoutComma)) {\r\n");
                sbScript.Append("return false;\r\n");
                sbScript.Append("}\r\n");
                sbScript.Append("if (hasDrillDownData && gridSelectedHiddenField.value.split(',').length>3) {\r\n");
                sbScript.Append("showMessage(msgID, '" + GetTextByKey("aca_drilldown_msg_disallowmultiedit").Replace("'", "\\'") + "', \"" + MessageType.Notice.ToString() + "\", true, 1, true)\r\n");
                sbScript.Append("return false;\r\n");
                sbScript.Append("}\r\n");

                sbScript.Append("SetNotAsk(true);\r\n");
                sbScript.Append("}\r\n");

                //Constant definition for ASIT expression and data edit functionality.
                sbScript.AppendFormat("var ASIT_POSTEVENT_TARGET_ID = '{0}';\r\n", ASITUIModelUtil.ASIT_POSTEVENT_TARGET_ID);
                sbScript.AppendFormat("var ASIT_EXP_INSERTROW_TARGET_ID = '{0}';\r\n", ExpressionUtil.ASIT_EXP_INSERTROW_TARGET_ID);
                sbScript.AppendFormat("var ASIT_EXP_INSERTROW_FIELD_ID = '{0}';\r\n", ExpressionUtil.ASIT_EXP_INSERTROW_FIELD_ID);

                //ASIT_DeleteSelectedClientClick
                sbScript.Append("function ASIT_DeleteSelectedClientClick(gridSelectedFieldID) {\r\n");
                sbScript.Append("var gridSelectedHiddenField = $get(gridSelectedFieldID);\r\n");
                sbScript.Append("var valWithoutComma = gridSelectedHiddenField.value.replace(/\\,/g, '');\r\n");
                sbScript.Append("if (/^\\s*$/.test(valWithoutComma)) {\r\n");
                sbScript.Append("return false;\r\n");
                sbScript.Append("}\r\n");
                sbScript.Append("SetNotAsk(true);\r\n");
                sbScript.AppendFormat("return confirmMsg('{0}');\r\n", GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'"));
                sbScript.Append("}\r\n");

                //ASIT_BuildPostBackArgument
                sbScript.Append("function ASIT_BuildPostBackArgument(tableKeys, currentWindow) {\r\n");
                sbScript.Append("var postBackID;\r\n");
                sbScript.Append("var updatePanelIDs = '';\r\n");
                sbScript.Append("if(!currentWindow) {\r\n");
                sbScript.Append("currentWindow = window;\r\n");
                sbScript.Append("}\r\n");
                sbScript.Append("if (tableKeys && tableKeys.length > 0)\r\n");
                sbScript.Append("{\r\n");
                sbScript.Append("for (var i = 0; i < tableKeys.length; i++) {\r\n");
                sbScript.Append("var key = tableKeys[i];\r\n");
                sbScript.Append("var panelID = currentWindow.asitUpdatePanelIDs[key];\r\n");
                sbScript.Append("if (i == 0) {\r\n");
                sbScript.Append("if(panelID) {\r\n");
                sbScript.Append("postBackID = panelID + ASIT_POSTEVENT_TARGET_ID;\r\n");
                sbScript.Append("} else {\r\n");
                sbScript.Append("postBackID = ASIT_POSTEVENT_TARGET_ID;\r\n");
                sbScript.Append("break;\r\n");
                sbScript.Append("}\r\n");
                sbScript.Append("} else {\r\n");
                sbScript.Append("updatePanelIDs += panelID;\r\n");
                sbScript.Append("if (i != tableKeys.length - 1) {\r\n");
                sbScript.AppendFormat("updatePanelIDs += '{0}';\r\n", ACAConstant.COMMA_CHAR);
                sbScript.Append("}}}\r\n");
                sbScript.Append("}\r\n");
                sbScript.Append("return { EventTarget: postBackID, EventArgument: updatePanelIDs};\r\n");
                sbScript.Append("}\r\n");

                //ASIT_ClearUIData
                sbScript.Append("function ASIT_ClearUIData() {\r\n");
                sbScript.Append("PageMethods.ClearUIData(\"['ASITCopy', 'ASITEdit']\");\r\n");
                sbScript.Append("}\r\n");

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ASITScripts", sbScript.ToString(), true);
            }
        }

        #endregion
    }
}