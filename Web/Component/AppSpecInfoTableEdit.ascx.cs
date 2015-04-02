#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AppSpecInfoTableEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AppSpecInfoTableEdit.ascx.cs 278548 2014-09-05 08:26:21Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using AjaxControlToolkit;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// ASIT Edit control.
    /// </summary>
    public partial class AppSpecInfoTableEdit : ASIBaseUC
    {
        #region Fields

        /// <summary>
        /// Defines control quantity for each row.
        /// </summary>
        private const int CONTROL_COUNT_PER_ROW = 3;

        /// <summary>
        /// Collect update panel ID for all table.
        /// </summary>
        private string _asitUpdatePanelIDs = string.Empty;

        /// <summary>
        /// UI table list.
        /// </summary>
        private IList<ASITUITable> _uiTableList;

        /// <summary>
        /// the control is template table control or not.
        /// </summary>
        private bool _isTemplateTable;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Expression session model
        /// </summary>
        public ExpressionSessionModel ExpressionSessionModel
        {
            get
            {
                if (ViewState["ExpressionSessionModel"] == null)
                {
                    ViewState["ExpressionSessionModel"] = ExpressionUtil.GetExpressionDataFromSession(UIDataKey);
                }

                return ViewState["ExpressionSessionModel"] as ExpressionSessionModel;
            }
        }

        /// <summary>
        /// Gets or sets UI data key
        /// </summary>
        public string UIDataKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether need show Hijri Calendar
        /// </summary>
        protected override bool NeedShowHijriCalendar
        {
            get { return !_isTemplateTable; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Display the inputted UI tables.
        /// </summary>
        /// <param name="uiDataKey">ASIT UI data key.</param>
        /// <param name="isShowSectionTitle">is show section title.</param>
        public void Display(string uiDataKey, bool isShowSectionTitle)
        {
            UIDataKey = uiDataKey;
            var uiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { UIDataKey }) as ASITUITable[];
            _uiTableList = uiTables != null ? uiTables.ToList() : null;

            if (uiTables != null && uiTables.Length > 0)
            {
                _isTemplateTable = uiTables[0].IsTemplateTable;

                if (isShowSectionTitle && !string.IsNullOrEmpty(uiTables[0].SectionTitle))
                {
                    labSectionName.Text = uiTables[0].SectionTitle;
                    labSectionName.Visible = true;
                }
            }

            if (IsPostBack)
            {
                //If the postback is triggered by expression-insert-row by onchange event of ASIT field,
                //need to get field's value from UI and sync to UI data.
                RefreshValues(_uiTableList);
            }

            if (!_isTemplateTable)
            {
                InitSubCapModels();
            }

            dlAppInfoTable.DataSource = _uiTableList;
            dlAppInfoTable.DataBind();
        }

        /// <summary>
        /// Gets the on populate running expression JS.
        /// </summary>
        /// <returns>the on populate running expression JS.</returns>
        public string GetOnPopulateRunningExpressionJS()
        {
            var expressionFieldsList = new Dictionary<string, List<string>>();
            var argumentsModels = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
            string asitScripts = ExpressionInstance.GetClientExpScript4ASIT("onPopulate");
            ExpressionUtil.CombineArgumentsAndExpressionFieldsModule(argumentsModels, expressionFieldsList, asitScripts);

            return ExpressionUtil.BuildRunExpressionScripts(argumentsModels, expressionFieldsList, false, null);
        }

        /// <summary>
        /// Use the control value to refresh the UI table.
        /// </summary>
        /// <param name="tables">ASIT UI Tables need to be refreshed.</param>
        public void RefreshValues(IEnumerable<ASITUITable> tables)
        {
            if (tables == null)
            {
                return;
            }

            foreach (ASITUITable asiTable in tables)
            {
                foreach (UIRow row in asiTable.Rows)
                {
                    foreach (ASITUIField field in row.Fields)
                    {
                        //Drill-down fields are filled out in backend by drill-down data. so , need not to get value from UI.
                        if (!field.IsDrillDown)
                        {
                            field.Value = ControlBuildHelper.GetControlValue(Request, int.Parse(field.Type), field.FieldID, field.Value);
                        }
                    }
                }
            }
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
            if (!_isTemplateTable && ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                //Handle ASIT expression behaviors.
                ExpressionUtil.HandleASITPostbackBehavior(Page);
            }
        }

        /// <summary>
        /// Handle PreRender event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AppSession.IsAdmin)
            {
                if (ExpressionSessionModel != null)
                {
                    InitExpressionInstance();
                }

                //Register upload-panel ID to client for partial update.
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "asitUpdatePanelIDs", "\r\nvar asitUpdatePanelIDs = new Object();\r\n", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), this.UniqueID, _asitUpdatePanelIDs + "\r\n", true);
            }
        }

        /// <summary>
        /// Handle ItemDataBound event for table list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void TableList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ASITUITable currentTable = (ASITUITable)e.Item.DataItem;
                UpdatePanel tableEditPanel = e.Item.FindControl("tableEditPanel") as UpdatePanel;
                PlaceHolder phCurrentTable = (PlaceHolder)e.Item.FindControl("phAppInfoTable");

                if (currentTable != null && currentTable.Rows != null && currentTable.Rows.Count > 0)
                {
                    ShowAppSpecInfoGroup(currentTable, phCurrentTable);
                }

                if (_uiTableList.IndexOf(currentTable) < _uiTableList.Count - 1)
                {
                    Literal separatedLine = new Literal();
                    separatedLine.Text = "<div class=\"ACA_TabRow ACA_Line_Content\">&nbsp;</div>";
                    e.Item.Controls.Add(separatedLine);
                }

                //Collect upload-panel ID for partial update.
                _asitUpdatePanelIDs += "asitUpdatePanelIDs['" + currentTable.TableKey + "'] = '" + tableEditPanel.UniqueID + "';\r\n";
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize the expression instance for ASIT edit form.
        /// </summary>
        private void InitExpressionInstance()
        {
            var copyUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITCopy, new string[] { UIDataKey }) as ASITUITable[];
            var editUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { UIDataKey }) as ASITUITable[];

            List<ASITUITable> asitUITables = new List<ASITUITable>();

            /*
             * When the Expression prepare the PK argument for expression, it will get ASIT sub group from its _uiTables variables.
             *   If use ASITCopy UI data to collect asitUITables, it will make redundant expression script.
             */
            if (editUIData != null)
            {
                foreach (ASITUITable copyTable in copyUIData)
                {
                    ASITUITable editTable = editUIData.SingleOrDefault(dt =>
                                                                       dt.AgencyCode == copyTable.AgencyCode &&
                                                                       dt.GroupName == copyTable.GroupName &&
                                                                       dt.TableName == copyTable.TableName);

                    if (editTable != null)
                    {
                        ASITUITable cloneTable = ObjectCloneUtil.DeepCopy(copyTable);
                        asitUITables.Add(cloneTable);
                    }
                }
            }

            ASITUIModelUtil.SyncASITUIRowData(editUIData, asitUITables.ToArray());

            //Ui data expression type(default: ExpressionType.ASI_Table
            ExpressionType uiExpressionType = ExpressionType.ASI_Table;
            List<ExpressionRuntimeArgumentPKModel> argumentPKModels = null;

            if (_isTemplateTable && ExpressionSessionModel != null)
            {
                argumentPKModels = ExpressionSessionModel.ExpressionArgumentPKModels;
                uiExpressionType = ExpressionSessionModel.ExpressionFactoryType;
            }

            ExpressionInstance = new ExpressionFactory(ModuleName, uiExpressionType, AllControls, argumentPKModels, asitUITables, SubCapModels, UIDataKey);
        }

        /// <summary>
        /// Fill the sub cap models.
        /// </summary>
        private void InitSubCapModels()
        {
            CapModel4WS currentCap = AppSession.GetCapModelFromSession(ModuleName);

            if (currentCap == null || currentCap.appSpecTableGroups == null || _uiTableList == null)
            {
                return;
            }

            var capIDs = from asitGroup in currentCap.appSpecTableGroups
                         from uiTable in _uiTableList
                         where asitGroup != null && uiTable != null
                               && asitGroup.capIDModel != null
                               && asitGroup.capIDModel.serviceProviderCode == uiTable.AgencyCode
                               && asitGroup.groupName == uiTable.GroupName
                               && asitGroup.tablesMap.Contains(uiTable.TableName)
                         select asitGroup.capIDModel;

            if (CapUtil.IsSuperCAP(currentCap) && capIDs.Count() > 0)
            {
                var capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                SubCapModels.Clear();

                foreach (CapIDModel4WS capId in capIDs)
                {
                    string key = capId.toKey();

                    if (!SubCapModels.ContainsKey(key))
                    {
                        var capTypeModel = capTypeBll.GetCapTypeByCapID(capId);
                        var tempCapModel = new CapModel4WS();
                        tempCapModel.capID = capId;
                        tempCapModel.capType = capTypeModel;
                        tempCapModel.moduleName = capTypeModel.group;
                        tempCapModel.auditID = currentCap.auditID;

                        SubCapModels.Add(key, tempCapModel);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the control value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="control">The control.</param>
        /// <param name="asitFieldsWithI18NValues">The asit fields with i18N values.</param>
        private void SetControlValue(ASITUIField field, Control control, List<ASITUIField> asitFieldsWithI18NValues)
        {
            int controlType = int.Parse(field.Type);

            switch (controlType)
            {
                case (int)FieldType.HTML_RADIOBOX:
                    RadioButtonList rdo = (RadioButtonList)control;
                    string val = field.Value;

                     /*
                     * ASIT Y/N field use 'Yes' or 'No' as value.
                     * Generic template Y/N field use 'Y' or 'N' as value.
                     */
                    if (ValidationUtil.IsYes(val))
                    {
                        val = _isTemplateTable ? ACAConstant.COMMON_Y : ACAConstant.COMMON_Yes;
                    }
                    else if (!string.IsNullOrEmpty(val))
                    {
                        val = _isTemplateTable ? ACAConstant.COMMON_N : ACAConstant.COMMON_No;
                    }

                    rdo.SelectedValue = string.IsNullOrEmpty(val) ? string.Empty : val;

                    if (field.IsReadOnly)
                    {
                        SetReadOnly(control, FieldType.HTML_RADIOBOX.ToString());
                    }

                    break;

                case (int)FieldType.HTML_SELECTBOX:
                    AccelaDropDownList ddl = (AccelaDropDownList)control;
                    DropDownListBindUtil.SetSelectedValue(ddl, field.Value);

                    if (field.IsReadOnly)
                    {
                        SetReadOnly(control, FieldType.HTML_SELECTBOX.ToString());
                    }

                    break;

                case (int)FieldType.HTML_CHECKBOX:
                    CheckBox chk = (CheckBox)control;

                    if (ControlBuildHelper.CHK_SELECT.Equals(field.Value, StringComparison.OrdinalIgnoreCase)
                        || ACAConstant.COMMON_CHECKED.Equals(field.Value, StringComparison.OrdinalIgnoreCase)
                        || ValidationUtil.IsYes(field.Value))
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        chk.Checked = false;
                    }

                    if (field.IsReadOnly)
                    {
                        SetReadOnly(control, FieldType.HTML_CHECKBOX.ToString());
                    }

                    break;
                default:

                    string fieldResValue = string.IsNullOrEmpty(field.Value) ? string.Empty : field.Value;

                    if (controlType == (int)FieldType.HTML_TEXTBOX_OF_DATE)
                    {
                        if (!string.IsNullOrEmpty(field.Value))
                        {
                            fieldResValue = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(field.Value);
                        }
                    }
                    else if (controlType == (int)FieldType.HTML_TEXTBOX_OF_CURRENCY)
                    {
                        if (!string.IsNullOrEmpty(field.Value))
                        {
                            fieldResValue = I18nNumberUtil.ConvertMoneyFromWebServiceToInput(field.Value);
                        }
                    }
                    else if (controlType == (int)FieldType.HTML_TEXTBOX_OF_NUMBER)
                    {
                        if (!string.IsNullOrEmpty(field.Value))
                        {
                            fieldResValue = I18nNumberUtil.ConvertNumberFromWebServiceToInput(field.Value);
                        }
                    }

                    TextBox txt = (TextBox)control;
                    txt.Text = fieldResValue;

                    if (field.IsReadOnly || field.IsDrillDown)
                    {
                        txt.ReadOnly = true;
                        txt.CssClass = ACAConstant.CSS_CLASS_READONLY;
                    }
                    else
                    {
                        txt.ReadOnly = false;
                        txt.CssClass = String.Empty;
                    }

                    break;
            }

            if (field.IsDrillDown && control is AccelaTextBox)
            {
                ASITUIField currentResField = asitFieldsWithI18NValues.Single(p => p.FieldID == field.FieldID);
                string resFieldValue = currentResField == null ? string.Empty : currentResField.Value;
                var currentTextBox = (AccelaTextBox)control;
                currentTextBox.Text = I18nStringUtil.GetString(resFieldValue, currentTextBox.Text);
            }
        }
        
        /// <summary>
        /// Set control to readonly for SelectBox/ CheckBox/ RadioBox by expression scripts.
        /// </summary>
        /// <param name="control">Field Control</param>
        /// <param name="controlType">Field Control Type</param>
        private void SetReadOnly(Control control, string controlType)
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("var ctrl_{0}=$get('{0}');", control.ClientID);
            script.AppendFormat("SetReadOnlyProperty(ctrl_{0}, ExpressionControlType.{1}, true);", control.ClientID, controlType);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), control.ID + "_readonly", script.ToString(), true);
        }

        /// <summary>
        /// Render UI table to client.
        /// </summary>
        /// <param name="appInfoTable">ASIT UI table.</param>
        /// <param name="placeHolder">Please holder for control rendering.</param>
        private void ShowAppSpecInfoGroup(ASITUITable appInfoTable, PlaceHolder placeHolder)
        {
            placeHolder.Controls.Clear();

            if ((appInfoTable == null) || (appInfoTable.Rows.Count() <= 0))
            {
                return;
            }

            bool isSuperCAP = false;

            if (!string.IsNullOrEmpty(ModuleName))
            {
                isSuperCAP = CapUtil.IsSuperCAP(ModuleName);
            }

            string subGroupName = StringUtil.GetString(appInfoTable.AlternativeLabel, appInfoTable.TableTitle);

            StringBuilder groupHeader = new StringBuilder();

            groupHeader.Append("<div class=\"ACA_TabRow\"><h1><i>");

            if (!_isTemplateTable && isSuperCAP)
            {
                //Dispaly Logo for super cap.
                string logoDescription = string.Format("{0} ({1})", subGroupName, appInfoTable.AgencyCode);
                string agencyLogo = CapUtil.GetAgencyLogoHtml(appInfoTable.AgencyCode, ModuleName);

                groupHeader.Append(agencyLogo);
                groupHeader.Append("<div class=\"ACA_ValCal_Title\">");
                groupHeader.Append(logoDescription);
                groupHeader.Append("</div>");
                groupHeader.Append("</i></h1></div>");
            }
            else
            {
                groupHeader.Append(subGroupName);
                groupHeader.Append("</i></h1></div>");
            }

            placeHolder.Controls.Add(new LiteralControl(groupHeader.ToString()));

            // display instruction for ASIT
            string instruction = appInfoTable.Instruction;
            string instructionCls = string.IsNullOrEmpty(instruction) ? "ACA_Hide" : "ACA_Section_Instruction ACA_Section_Instruction_FontSize";
            string instructionDiv = string.Format("<div class=\"{0}\">{1}</div>", instructionCls, instruction);
            placeHolder.Controls.Add(new LiteralControl(instructionDiv));

            //Get the I18N values for drill-down fields.
            var asitFieldsWithI18NValues = ASITUIModelUtil.GetASITFieldsWithResValues(appInfoTable);

            for (int j = 0; j < appInfoTable.Rows.Count; j++)
            {
                Table tb = new Table();
                string widthdescription = "100%";
                tb.Width = new Unit(widthdescription);
                tb.CellSpacing = 0;
                tb.CellPadding = 5;

                //ASIT doesn't have configure to set columnArrangement and columnLayout.
                //default value columnArrangement = Horizontal / columnLayout=3
                tb.Attributes.Add("columnArrangement", ControlLayoutType.Horizontal.ToString());
                tb.Attributes.Add("columnLayout", CONTROL_COUNT_PER_ROW.ToString());
                tb.Attributes.Add("role", "presentation");
                bool hasHiddenByExp = false;
                TableRow tr = new TableRow();

                //Collect hidden fields count by ASIT security or Properties settings
                int hiddenItemsCount = 0;
                int cellCountPerRow = 0;

                //Warning:Do not change this variable's value.
                int fieldIndex = 0;

                //The tableCellIndex is for ASI table re-layout by expression scripts.
                int tableCellIndex = 0;

                // loop for columns
                foreach (ASITUIField field in appInfoTable.Rows[j].Fields)
                {
                    string controlIDSuffix = j + "_" + fieldIndex + " " + appInfoTable.Rows[0].Fields.Count;

                    if (_isTemplateTable)
                    {
                        controlIDSuffix = j + "_" + field.Name.GetHashCode().ToString("X2");
                    }

                    cellCountPerRow++;

                    TableCell tc = new TableCell();

                    //index attribute to sort visible fields
                    tc.Attributes.Add("index", tableCellIndex.ToString());
                    tc.VerticalAlign = VerticalAlign.Top;

                    //To display in a line when hidden some fields by expression
                    tc.Style.Add("width", "33%");
                    WebControl control = ControlBuildHelper.ASIT.CreateWebControl(field, _isTemplateTable);

                    if (field.IsHidden || field.IsHiddenByExp)
                    {
                        if (field.IsHidden)
                        {
                            //Hidden by ASIT security or Properties settings.
                            hiddenItemsCount++;
                        }

                        if (field.IsHiddenByExp)
                        {
                            hasHiddenByExp = true;
                        }

                        // if it hidden by expression, not add the ishidden attribute
                        if (field.IsHidden)
                        {
                            //Hidden by ASIT security or Properties settings.
                            (control as IAccelaControl).IsHidden = true;
                            control.Attributes.Add(ACAConstant.IS_HIDDEN, ACAConstant.COMMON_TRUE);

                            if (control is CheckBox)
                            {
                                CheckBox cb = control as CheckBox;
                                cb.InputAttributes.Add(ACAConstant.IS_HIDDEN, ACAConstant.COMMON_TRUE);
                            }
                        }

                        tc.CssClass = "ACA_Hide";
                    }

                    if (control is CheckBox)
                    {
                        var ctrl = control as CheckBox;
                        ctrl.InputAttributes.Add("SecurityType", field.SecurityValue);
                        ctrl.InputAttributes.Add("IsASITControl", bool.TrueString.ToLower());
                    }
                    else
                    {
                        var calendar = control as AccelaCalendarText;

                        if (calendar != null)
                        {
                            calendar.IsHijriDate = IsDisplayHijriCalendar;
                        }

                        control.Attributes.Add("SecurityType", field.SecurityValue);
                        control.Attributes.Add("IsASITControl", bool.TrueString.ToLower());
                    }

                    SetControlValue(field, control, asitFieldsWithI18NValues);
                    placeHolder.Controls.Add(control);
                    tc.HorizontalAlign = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? HorizontalAlign.Right : HorizontalAlign.Left;

                    if (ExpressionSessionModel != null)
                    {
                        //Prepare the expression variables.
                        string finalID = HttpUtility.UrlEncode(appInfoTable.TableName) + "_" + controlIDSuffix;
                        CapModel4WS capModel = CapModel;

                        if (SubCapModels.Count > 0 && SubCapModels.ContainsKey(appInfoTable.CapID.toKey()))
                        {
                            capModel = SubCapModels[appInfoTable.CapID.toKey()];
                        }

                        finalID = ExpressionUtil.GetFullControlFieldName(capModel, finalID);

                        if (!AllControls.ContainsKey(finalID))
                        {
                            AllControls.Add(finalID, control);
                        }
                    }

                    tc.Controls.Add(control);

                    if (int.Parse(field.Type) == (int)FieldType.HTML_RADIOBOX && field.IsRequired && !field.IsHidden)
                    {
                        RadioButtonListRequiredFieldValidator reqValidator = new RadioButtonListRequiredFieldValidator();
                        reqValidator.ID = control.ID + "_req";
                        reqValidator.Display = ValidatorDisplay.None;
                        reqValidator.ControlToValidate = control.ID;
                        reqValidator.SetFocusOnError = true;

                        ValidatorCallbackExtender reqValidatorExt = new ValidatorCallbackExtender();
                        reqValidatorExt.ID = control.ID + "_req_ext";
                        reqValidatorExt.TargetControlID = reqValidator.ID;
                        reqValidatorExt.CallbackFailFunction = "doErrorCallbackFun";
                        reqValidatorExt.CallbackControlID = control.ID;
                        reqValidatorExt.HighlightCssClass = "HighlightCssClass";

                        tc.Controls.Add(reqValidator);
                        tc.Controls.Add(reqValidatorExt);
                    }

                    tr.Cells.Add(tc);
                    tc = null;

                    if ((cellCountPerRow - hiddenItemsCount) % CONTROL_COUNT_PER_ROW == 0)
                    {
                        tb.Rows.Add(tr);
                        tr = new TableRow();
                        cellCountPerRow = 0;
                        hiddenItemsCount = 0;
                    }

                    fieldIndex++;
                    tableCellIndex++;
                }

                //fill empty row evry row has 3 TD
                if (cellCountPerRow < CONTROL_COUNT_PER_ROW)
                {
                    for (int initColumn = 0; initColumn < CONTROL_COUNT_PER_ROW - cellCountPerRow; initColumn++)
                    {
                        TableCell tc1 = new TableCell();
                        tc1.Attributes.Add("index", tableCellIndex.ToString());
                        tr.Cells.Add(tc1);
                        tableCellIndex++;
                    }
                }

                if (cellCountPerRow > 0)
                {
                    tb.Rows.Add(tr);
                }

                tb.Rows.Add(tr);
                placeHolder.Controls.Add(tb);

                //Add separate line between the ASIT rows.
                if (j < appInfoTable.Rows.Count - 1)
                {
                    Table tbHR = new Table();
                    TableRow trHR = new TableRow();
                    TableCell tcHR = new TableCell();

                    tbHR.Width = tb.Width;
                    tbHR.CellSpacing = tb.CellSpacing;
                    tbHR.CellPadding = tb.CellPadding;
                    tbHR.Attributes.Add("role", "presentation");
                    Literal literal = new Literal();
                    literal.Text = "<div class=\"ACA_TabRow ACA_Line_Content\">&nbsp;</div>";

                    tcHR.Controls.Add(literal);
                    trHR.Cells.Add(tcHR);
                    tbHR.Rows.Add(trHR);
                    placeHolder.Controls.Add(tbHR);
                }

                if (hasHiddenByExp)
                {
                    //Need to re-layout asit by "Insert a row" to trigger expression.
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ReSortASIT" + tb.ClientID, "LayoutASI($get('" + tb.ClientID + "'));", true);
                }
            }
        }

        #endregion
    }
}