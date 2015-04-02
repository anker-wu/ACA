 #region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ExpressionUtil.cs
*
*  Accela, Inc.
*  Copyright (C): 2013-2014
*
*  Description: Expression utility.
*
*  Notes:
* $Id: UIUtil.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Jul 27, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.ExpressionBuild
{
    /// <summary>
    /// Expression utility class.
    /// </summary>
    public static class ExpressionUtil
    {
        #region Fields

        /// <summary>
        /// Event target ID to indicating whether has expression 'Insert Row' result needs to handle.
        /// </summary>
        public const string ASIT_EXP_INSERTROW_TARGET_ID = "$ASIT_INSERTROW";

        /// <summary>
        /// Hidden field ID for store the expression 'Insert Row' result.
        /// </summary>
        public const string ASIT_EXP_INSERTROW_FIELD_ID = "ASITInsertRowExpResult";

        /// <summary>
        /// Used to analyze RunExpression segment from scripts
        /// </summary>
        public const string RUN_EXPRESSION_PATTERN = @"RunExpression\((?<Argument>\{.*?\}),(?<InputParams>\[.*?\]),(true|false),this\)";

        /// <summary>
        /// ASIT field name pattern.
        /// </summary>
        private const string PatternASITFieldName = @"^ASIT_(?<TableKey>\w+)_(?<RowIndex>\d+)_(\d+|\w{8})$";

        /// <summary>
        /// expression type model
        /// </summary>
        private static List<ExpressionTypeModel> _expressionTypeModels = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression type models
        /// </summary>
        public static List<ExpressionTypeModel> ExpressionTypeModels
        {
            get
            {
                if (_expressionTypeModels == null)
                {
                    _expressionTypeModels = new List<ExpressionTypeModel>();
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.ASI, TemplateViewID = null, TPL_FormViewID = null, TPL_TableViewID = null });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.ASI_Table, TemplateViewID = null, TPL_FormViewID = null, TPL_TableViewID = null });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.Fee_Item, TemplateViewID = null, TPL_FormViewID = null, TPL_TableViewID = null });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.License_Professional, TemplateViewID = null, TPL_FormViewID = null, TPL_TableViewID = null });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.Applicant, TemplateViewID = ExpressionType.APPLICANT_PEOPLE_TEMPLATE, TPL_FormViewID = ExpressionType.Applicant_TPL_Form, TPL_TableViewID = ExpressionType.Applicant_TPL_Table });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.Contact_1, TemplateViewID = ExpressionType.CONTACT1_PEOPLE_TEMPLATE, TPL_FormViewID = ExpressionType.Contact1_TPL_Form, TPL_TableViewID = ExpressionType.Contact1_TPL_Table });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.Contact_2, TemplateViewID = ExpressionType.CONTACT2_PEOPLE_TEMPLATE, TPL_FormViewID = ExpressionType.Contact2_TPL_Form, TPL_TableViewID = ExpressionType.Contact2_TPL_Table });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.Contact_3, TemplateViewID = ExpressionType.CONTACT3_PEOPLE_TEMPLATE, TPL_FormViewID = ExpressionType.Contact3_TPL_Form, TPL_TableViewID = ExpressionType.Contact3_TPL_Table });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.Contacts, TemplateViewID = ExpressionType.CONTACT_PEOPLE_TEMPLATE, TPL_FormViewID = ExpressionType.Contact_TPL_Form, TPL_TableViewID = ExpressionType.Contact_TPL_Table });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.ReferenceContact, TemplateViewID = ExpressionType.REFPEOPLE_TEMPLATE, TPL_FormViewID = ExpressionType.RefContact_TPL_Form, TPL_TableViewID = ExpressionType.RefContact_TPL_Table });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.AuthAgent_Customer_Detail, TemplateViewID = ExpressionType.AuthAgent_People_Template, TPL_FormViewID = ExpressionType.AuthAgent_TPL_Form, TPL_TableViewID = ExpressionType.AuthAgent_TPL_Table });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.Address, TemplateViewID = ExpressionType.Address_Template, TPL_FormViewID = null, TPL_TableViewID = null });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.Contact_Address, TemplateViewID = null, TPL_FormViewID = null, TPL_TableViewID = null });
                    _expressionTypeModels.Add(new ExpressionTypeModel { PortletID = ExpressionType.AuthAgent_Address, TemplateViewID = null, TPL_FormViewID = null, TPL_TableViewID = null });
                }

                return _expressionTypeModels;
            }
        }

        #endregion Properties

        /// <summary>
        /// Build client scripts for RunExpression method.
        /// </summary>
        /// <param name="args">A dictionary of the expression runtime arguments. Currently the format of key is CapID1+CapID2+CapID3.</param>
        /// <param name="inputFieldIDList">A dictionary of the Client ID list of the input parameters for each expression runtime argument. Key format is same as <paramref name="args"/>.</param>
        /// <param name="isSubmit">Indicating whether is a submit expression.</param>
        /// <param name="isLockLicense">Indicating license lock status.</param>
        /// <returns>String of client scripts.</returns>
        public static string BuildRunExpressionScripts(Dictionary<string, ExpressionRuntimeArgumentsModel> args, Dictionary<string, List<string>> inputFieldIDList, bool isSubmit, bool? isLockLicense)
        {
            StringBuilder scriptsBuilder = new StringBuilder();

            if (inputFieldIDList != null && inputFieldIDList.Count > 0 && inputFieldIDList.Count.Equals(args.Count))
            {
                if (isLockLicense != null)
                {
                    scriptsBuilder.Append("IsLockLicense = " + isLockLicense.ToString().ToLowerInvariant() + ";");
                }

                foreach (KeyValuePair<string, ExpressionRuntimeArgumentsModel> arg in args)
                {
                    scriptsBuilder.Append(BuildRunExpressionScripts(arg.Value, inputFieldIDList[arg.Key], isSubmit));
                }
            }

            return scriptsBuilder.ToString();
        }

        /// <summary>
        /// Build client scripts for RunExpression method.
        /// </summary>
        /// <param name="arg">Expression runtime argument object.</param>
        /// <param name="inputFieldIDList">The Client ID list of input parameters.</param>
        /// <param name="isSubmit">Indicating whether is a submit expression.</param>
        /// <returns>String of client scripts.</returns>
        public static string BuildRunExpressionScripts(ExpressionRuntimeArgumentsModel arg, List<string> inputFieldIDList, bool isSubmit)
        {
            StringBuilder scriptsBuilder = new StringBuilder();
            scriptsBuilder.Append(ExpressionFactory.RUN_EXPRESSION_NAME);
            scriptsBuilder.Append("(");
            scriptsBuilder.Append(JsonConvert.SerializeObject(arg));
            scriptsBuilder.Append(ACAConstant.COMMA);
            scriptsBuilder.Append(JsonConvert.SerializeObject(inputFieldIDList));
            scriptsBuilder.Append(ACAConstant.COMMA);
            scriptsBuilder.Append(isSubmit.ToString().ToLower());
            scriptsBuilder.Append(ACAConstant.COMMA);
            scriptsBuilder.Append("this);");
            return scriptsBuilder.ToString();
        }

        /// <summary>
        /// Get latest status of ASIT fields from UI data.
        /// </summary>
        /// <param name="executeKey">Expression execute key.</param>
        /// <param name="expField">Expression field.</param>
        /// <returns>Updated expression field.</returns>
        public static ExpressionFieldModel GetLatestStatus4ASITField(string executeKey, ExpressionFieldModel expField)
        {
            ExpressionFieldModel result = expField;

            if (expField.blockSubmit == true
                || !IsASITOrGenericTempalteTableView(expField.viewID)
                || !Regex.IsMatch(expField.name, PatternASITFieldName, RegexOptions.IgnoreCase))
            {
                return result;
            }

            ASITUITable[] uiTables;

            if (executeKey.StartsWith(ExpressionFactory.ASIT_FIELD_PREFIX))
            {
                uiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITCopy, null) as ASITUITable[];
            }
            else
            {
                uiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, null) as ASITUITable[];
            }

            if (uiTables != null && uiTables.Length > 0)
            {
                Match matchASITFieldName = Regex.Match(expField.name, PatternASITFieldName, RegexOptions.IgnoreCase);
                string tableKey = matchASITFieldName.Result("${TableKey}");
                var uiTable = uiTables.SingleOrDefault(t => t.TableKey.Equals(tableKey));

                if (uiTable != null)
                {
                    //Search out UI field by field ID.
                    ASITUIField uiField = (from row in uiTable.Rows
                                           from field in row.Fields
                                           where field.FieldID.Equals(expField.name)
                                           select field as ASITUIField).SingleOrDefault();

                    if (uiField != null)
                    {
                        result.value = uiField.Value;
                        result.hidden = uiField.IsHiddenByExp;
                        result.readOnly = uiField.IsReadOnlyByExp;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Handle the Expression result and to process the result for ASIT.
        /// </summary>
        /// <param name="expArgument">Expression argument model.</param>
        /// <param name="result">Expression result model.</param>
        /// <param name="isInSpearForm">Indicating the expression whether is triggered from Spear form (or by Scripts mode).</param>
        public static void HandleASITExpressionResult(ExpressionRuntimeArgumentsModel expArgument, ExpressionRunResultModel4WS result, bool isInSpearForm)
        {
            if (result == null || result.fields == null || !result.fields.Any())
            {
                return;
            }

            //uidatakey ,viewID
            var uiDataKeyViewIDMapping = new Dictionary<string, long>();

            /*
            * 1. Combine the [Table Key] to "subGroup" attribute of each field for ASIT,
            *    client scripts(Expression.js) will use Table Key to handle the expression behaviors.
            * 2. The [Table Key] is search out from the UI tables by the Table Name and Cap ID.
            * 3. Dirll down page will to search out the Agency Code/Group Name/Table Name by the [Table Key]
            *    and to get the drill-down data.
            */
            foreach (ExpressionFieldModel expField in result.fields)
            {
                if (expField.viewID == null || !IsValidASITExpField(expField))
                {
                    continue;
                }

                IList<string> uiDataKeys = GetASITUIDataKey(expField);

                if ((uiDataKeys == null || uiDataKeys.Count == 0) || (ExpressionType)expField.viewID == ExpressionType.ASI_Table)
                {
                    var asitUIs = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT);

                    foreach (var uiTables in asitUIs)
                    {
                        if (uiTables.Value == null || uiTables.Value.Length == 0)
                        {
                            continue;
                        }

                        ASITUITable asitUiTable = uiTables.Value[0] as ASITUITable;

                        if (asitUiTable.IsTemplateTable)
                        {
                            continue;
                        }

                        uiDataKeys.Add(uiTables.Key);
                    }
                }

                if (uiDataKeys == null || uiDataKeys.Count == 0)
                {
                    continue;
                }

                ASITUITable[] asitTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, uiDataKeys.ToArray()) as ASITUITable[];

                if (asitTables == null)
                {
                    continue;
                }

                foreach (string dataKey in uiDataKeys.Where(dataKey => !uiDataKeyViewIDMapping.ContainsKey(dataKey)))
                {
                    uiDataKeyViewIDMapping.Add(dataKey, expField.viewID.Value);
                }

                //Search UI table by table name.
                string tableName = GetSubGroupName(expField.subGroup);
                ASITUITable asiTable = asitTables.SingleOrDefault(t => t.TableName == tableName
                                                                           && (t.CapID == null || TempModelConvert.Trim4WSOfCapIDModel(t.CapID).Equals(expArgument.capID)));

                if (asiTable != null)
                {
                    //Combine table key to "subGroup" attribute.
                    expField.subGroup = ExpressionFactory.ASIT_FIELD_PREFIX + asiTable.TableKey;
                }
            }

            foreach (var uiDataKey in uiDataKeyViewIDMapping)
            {
                //Collect updated fields.
                var nonInsertedFields = result.fields.Where(f => f.viewID == uiDataKey.Value &&
                    f.blockSubmit != true &&
                    !f.name.StartsWith(ExpressionFactory.INSERT_FIRST_ROW));

                //To remove the inserted fields for view page.
                //Because the template table don't support other page(e.g. capdetail,capconfirm),and in these page,it support asi_table only.
                if (!isInSpearForm && result.fields != null)
                {
                    result.fields = result.fields.Where(f => f.blockSubmit != true && !f.name.StartsWith(ExpressionFactory.INSERT_FIRST_ROW)).ToArray();
                }

                //Collect new inserted fields and related table keys.
                var insertedFields = result.fields.Where(f => f.viewID == uiDataKey.Value &&
                    f.blockSubmit != true &&
                    f.name.StartsWith(ExpressionFactory.INSERT_FIRST_ROW));

                var insertedTableKeys = insertedFields.Select(f => f.subGroup.Replace(ExpressionFactory.ASIT_FIELD_PREFIX, string.Empty)).Distinct();

                if (IsASITVariableKey(expArgument.executeFieldVariableKey))
                {
                    /*
                     * If expression is triggered by ASIT fields or by "onPopulate" event, 
                     * the expression result will be apply to ASIT Copy data and ASIT Edit data.
                     * Data format of execute key of the onpopulate event: ASIT::[table name]::onPopulate.
                     */

                    //Apply expression result to Copy data.
                    if (nonInsertedFields.Count() > 0)
                    {
                        ASITUITable[] uiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITCopy, new string[] { uiDataKey.Key }) as ASITUITable[];
                        ApplyExpressionToUITables(nonInsertedFields, uiTables);
                        UIModelUtil.SetDataToUIContainer(uiTables, UIDataType.ASITCopy, uiDataKey.Key);
                    }

                    //Combine the Copy data and Edit data.
                    var copyUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITCopy, new string[] { uiDataKey.Key }) as ASITUITable[];
                    var editUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { uiDataKey.Key }) as ASITUITable[];
                    var baseUIData = ObjectCloneUtil.DeepCopy(copyUIData);
                    ASITUIModelUtil.SyncASITUIRowData(editUIData, baseUIData);

                    if (insertedFields.Count() > 0)
                    {
                        var sortedTableKeys = SortASITTableKeysBasedUIData(insertedTableKeys, baseUIData);
                        List<ASITUITable> newTableList = CreateASITUITable4InsertRow(baseUIData, insertedFields, sortedTableKeys, uiDataKey.Key);

                        if (newTableList != null && newTableList.Count > 0)
                        {
                            //Sync new row to Edit data.
                            ASITUIModelUtil.SyncASITUIRowData(newTableList.ToArray(), editUIData);
                            UIModelUtil.SetDataToUIContainer(editUIData, UIDataType.ASITEdit, uiDataKey.Key);
                        }
                    }
                }
                else
                {
                    /*
                     * The expression is triggered by [ASI fields / onLoad / onSubmit] event,
                     * the expression result will be apply to ASIT data and ASIT Edit data.
                     */

                    //Apply expression result to ASIT data.
                    if (nonInsertedFields.Count() > 0)
                    {
                        ASITUITable[] uiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new string[] { uiDataKey.Key }) as ASITUITable[];
                        ApplyExpressionToUITables(nonInsertedFields, uiTables);
                        UIModelUtil.SetDataToUIContainer(uiTables, UIDataType.ASIT, uiDataKey.Key);
                    }

                    if (insertedFields.Count() > 0)
                    {
                        var baseUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new string[] { uiDataKey.Key }) as ASITUITable[];
                        var sortedTableKeys = SortASITTableKeysBasedUIData(insertedTableKeys, baseUIData);

                        List<ASITUITable> newTableList = CreateASITUITable4InsertRow(baseUIData, insertedFields, sortedTableKeys, uiDataKey.Key);

                        if (newTableList != null && newTableList.Count > 0)
                        {
                            /*
                             * In Super agency environment, if have multiple agency to Insert ASIT Row in onload event,
                             *     will generated multiple "RunExpression" scripts statement;
                             * In order to prevent the Edit form popped repeatedly; need to combine the edit data for each agency.
                             */
                            var editUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { uiDataKey.Key }) as ASITUITable[];
                            editUIData = editUIData != null ? ASITUIModelUtil.ConcatASITUITables(newTableList.ToArray(), editUIData) : newTableList.ToArray();

                            //Set new row to Edit data.
                            UIModelUtil.SetDataToUIContainer(editUIData, UIDataType.ASITEdit, uiDataKey.Key);
                        }
                    }
                }

                //Apply expression result to Edit data.
                var editUITables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { uiDataKey.Key }) as ASITUITable[];

                if (editUITables != null)
                {
                    ApplyExpressionToUITables(result.fields, editUITables);
                }
            }
        }

        /// <summary>
        /// Handle ASIT post back behaviors.
        /// 1. Update the update panels by data edit functionality.
        /// 2. Update the update panels by expression 'Update Row' behavior.
        /// 3. Pop out the edit form for 'Insert Row' behavior.
        /// </summary>
        /// <param name="page">Web page instance.</param>
        public static void HandleASITPostbackBehavior(Page page)
        {
            string eventTarget = page.Request.Form[Page.postEventSourceID];
            string eventArgument = page.Request.Form[Page.postEventArgumentID];

            if (!string.IsNullOrEmpty(eventTarget) && eventTarget.Contains(ASITUIModelUtil.ASIT_POSTEVENT_TARGET_ID))
            {
                //Handle expression [Update Row] behavior.

                /*
                 * The __EVENTARGUMENT is a string contains the Unique IDs of ASIT update panels, the IDs is joined with Comma char.
                 * Data example:
                 * ctl00$PlaceHolderMain$AppSpecTable455Edit$rptASITableList$tablePanel,ctl00$PlaceHolderMain$AppSpecTable456Edit$rptASITableList$tablePanel
                 */
                string[] uniqueIDs = eventArgument.Split(ACAConstant.COMMA_CHAR);

                foreach (var uniqueID in uniqueIDs)
                {
                    if (string.IsNullOrWhiteSpace(uniqueID))
                    {
                        continue;
                    }

                    /*
                     * Default value of IdSeparator is '$'.
                     * argItem data format example:
                     * ctl00$PlaceHolderMain$AppSpecTable455Edit$rptASITableList$tablePanel
                     */
                    string[] controlIDs = uniqueID.Split(page.IdSeparator);
                    Control control = page;

                    foreach (string ctlID in controlIDs)
                    {
                        //To find control by current level control ID.
                        control = control.FindControl(ctlID);

                        //If current level can not be found, break and continue next unique ID.
                        if (control == null)
                        {
                            break;
                        }
                    }

                    //If the control can not be found, continue next unique ID.
                    if (control == null)
                    {
                        continue;
                    }

                    //If the control is found and is UpdatePanel object, to do update.
                    var panel = control as UpdatePanel;

                    if (panel != null)
                    {
                        panel.Update();
                    }
                }

                //Handle expression [Insert Row] behavior.
                if (eventTarget.Contains(ASIT_EXP_INSERTROW_TARGET_ID))
                {
                    /*
                     * Get expression result and table info from hidden field.
                     * The hidden field is create by client in Expression.js.
                     */
                    string param = page.Request.Form[ASIT_EXP_INSERTROW_FIELD_ID];

                    if (!string.IsNullOrEmpty(param))
                    {
                        //Data format: Expression result|| Array of table keys|| Array of UI data keys
                        string[] insertParams = param.Split(new string[] { ACAConstant.SPLIT_DOUBLE_VERTICAL }, StringSplitOptions.None);
                        string scriptsBuilder = string.Format("ASIT_InsertRow({0}, {1}, {2})", insertParams);
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "ASITInsertExpression", scriptsBuilder, true);
                    }
                }
            }
        }

        /// <summary>
        /// Is have ASIT field.
        /// </summary>
        /// <param name="expressionType">the expression type.</param>
        /// <returns>true or false.</returns>
        public static bool IsIncludeASITFields(ExpressionType expressionType)
        {
            if (expressionType == ExpressionType.ASI_Table)
            {
                return true;
            }

            //Step-1 get express type model
            ExpressionTypeModel expressionTypeModel = ExpressionTypeModels.FirstOrDefault(f => f.PortletID == expressionType);

            return expressionTypeModel != null && expressionTypeModel.TPL_TableViewID != null;
        }

        /// <summary>
        /// Is generic template view.
        /// </summary>
        /// <param name="viewID">the view id.</param>
        /// <returns>Generic template table view.</returns>
        public static bool IsGenericTempalteTableView(long? viewID)
        {
            if (viewID == null)
            {
                return false;
            }

            ExpressionType expressionType = (ExpressionType)viewID;

            return ExpressionTypeModels.Any(w => w.TPL_TableViewID == expressionType);
        }

        /// <summary>
        /// IS ASIT for expression field.
        /// </summary>
        /// <param name="viewID">The view id.</param>
        /// <returns>true or false.</returns>
        public static bool IsASITOrGenericTempalteTableView(long? viewID)
        {
            if (viewID == null)
            {
                return false;
            }

            ExpressionType expressionType = (ExpressionType)viewID;

            if (expressionType == ExpressionType.ASI_Table)
            {
                return true;
            }

            //Check the view id.
            return ExpressionTypeModels.Any(w => w.TPL_TableViewID == expressionType);
        }

        /// <summary>
        /// collect expression information for each contact component.
        /// </summary>
        /// <param name="isSubmit">indicate review page or not.</param>
        /// <param name="editControl">Call the editControl's Expression instance to generate expression scripts.</param>
        /// <returns>The expression script</returns>
        public static string GetExpressionScript(bool isSubmit, FormDesignerBaseControl editControl)
        {
            string expressionScript = string.Empty;

            if (isSubmit && editControl is FormDesignerWithExpressionControl)
            {
                expressionScript = ((FormDesignerWithExpressionControl)editControl).GetRunExpFunctionOnSubmit();
            }
            else if (editControl is FormDesignerWithExpressionControl)
            {
                expressionScript = ((FormDesignerWithExpressionControl)editControl).GetRunExpFunctionOnLoad();
            }

            var expressionFieldsList = new Dictionary<string, List<string>>();
            var argumentsModels = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
            CombineArgumentsAndExpressionFieldsModule(argumentsModels, expressionFieldsList, expressionScript);
            string scripts = BuildRunExpressionScripts(argumentsModels, expressionFieldsList, isSubmit, false);
            return scripts;
        }

        /// <summary>
        /// Register "JavaScript" code into page to running expression for "on submit".
        /// </summary>
        /// <param name="expJsFunction">expression JavaScript.</param>
        /// <param name="clientJsFunctionBeforeExp">client script before running expression.</param>
        /// <param name="clientJsFuntionAfterExp">client script after running expression.</param>
        /// <returns>The expression on submit script</returns>
        public static string GetExpressionScriptOnSubmit(string expJsFunction, string clientJsFunctionBeforeExp = null, string clientJsFuntionAfterExp = null)
        {
            string jsSubmit = string.Format(
                        @" var btnSubmit;
                        function SubmitEP(fireObj) {{
                            if (typeof(fireObj) != 'undefined') {{
                                btnSubmit = fireObj;
                            }}
                            {0}
                            // Do not trigger expression if Submit button is disabled.
                            if ($(btnSubmit).attr('disabled')) {{
                                return false;
                            }}
                            initialSubmit4Exp('{1}');
                            {2}
                            {3}
                            return finishSubmit4Exp();
                        }}",
                        clientJsFunctionBeforeExp ?? string.Empty,
                        !string.IsNullOrEmpty(expJsFunction),
                        expJsFunction ?? string.Empty,
                        !string.IsNullOrEmpty(clientJsFuntionAfterExp) ? clientJsFuntionAfterExp : "SetCurrentValidationSectionID('');");

            return jsSubmit;
        }

        /// <summary>
        /// Get expression data from session
        /// </summary>
        /// <param name="asitUiDataKey">UI data type.</param>
        /// <returns>A dictionary contains the data keys and data tables.</returns>
        public static ExpressionSessionModel GetExpressionDataFromSession(string asitUiDataKey)
        {
            Hashtable expressionData = AppSession.GetExpressionDataFromSession();

            if (expressionData == null || !expressionData.ContainsKey(asitUiDataKey))
            {
                return null;
            }

            return expressionData[asitUiDataKey] as ExpressionSessionModel;
        }

        /// <summary>
        /// Set the expression data to session .
        /// </summary>
        /// <param name="expressionSessionModel">A dictionary contains the data keys and data tables.</param>
        /// <param name="asitUiDataKey">the asit UI data key</param>
        public static void SetExpressionDataToSession(ExpressionSessionModel expressionSessionModel, string asitUiDataKey)
        {
            Hashtable expressionData = AppSession.GetExpressionDataFromSession();

            if (expressionData == null)
            {
                expressionData = new Hashtable();
            }

            if (!expressionData.ContainsKey(asitUiDataKey))
            {
                expressionData.Add(asitUiDataKey, expressionSessionModel);
            }
            else
            {
                expressionData[asitUiDataKey] = expressionSessionModel;
            }

            AppSession.SetExpressionDataToSession(expressionData);
        }

        /// <summary>
        /// Clear the asit expression session
        /// </summary>
        /// <param name="asitUiDataKey">the asit UI data key</param>
        public static void ClearExpressionDataByKey(string asitUiDataKey)
        {
            Hashtable expressionData = AppSession.GetExpressionDataFromSession();

            if (expressionData != null && expressionData.ContainsKey(asitUiDataKey))
            {
                expressionData.Remove(asitUiDataKey);
                AppSession.SetExpressionDataToSession(expressionData);
            }
        }

        /// <summary>
        /// Get expression result from session.
        /// </summary>
        /// <returns>expression result</returns>
        public static Dictionary<string, KeyValuePair<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS>> GetExpressionResultFromSession()
        {
            var sessionData = HttpContext.Current.Session[SessionConstant.SESSION_EXPRESSION_RESULT_DATA] as Dictionary<string, KeyValuePair<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS>>;
            return sessionData;
        }

        /// <summary>
        /// add expression result to session
        /// </summary>
        /// <param name="expressionKey">expression result key.</param>
        /// <param name="expressionResult">expression result which need add to session.</param>
        public static void AddExpressionResultToSession(string expressionKey, KeyValuePair<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS> expressionResult)
        {
            var sessionData = HttpContext.Current.Session[SessionConstant.SESSION_EXPRESSION_RESULT_DATA] as Dictionary<string, KeyValuePair<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS>>;

            if (sessionData == null)
            {
                sessionData = new Dictionary<string, KeyValuePair<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS>>();
            }

            if (sessionData.ContainsKey(expressionKey))
            {
                sessionData[expressionKey] = expressionResult;
            }
            else
            {
                sessionData.Add(expressionKey, expressionResult);
            }

            SetExpressionResultToSession(sessionData);
        }

        /// <summary>
        /// Set expression result to session.
        /// </summary>
        /// <param name="expressionResult">expression result,if is null, will remove the session key.</param>
        public static void SetExpressionResultToSession(Dictionary<string, KeyValuePair<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS>> expressionResult)
        {
            if (expressionResult == null)
            {
                HttpContext.Current.Session.Remove(SessionConstant.SESSION_EXPRESSION_RESULT_DATA);
            }
            else
            {
                HttpContext.Current.Session[SessionConstant.SESSION_EXPRESSION_RESULT_DATA] = expressionResult;
            }
        }

        /// <summary>
        ///  Update the argumentsModels and expression field list
        /// </summary>
        /// <param name="collectArgumentsModule">collect argument model module</param>
        /// <param name="collectExpressionFieldsModule">collect expression field module</param>
        /// <param name="expressionScript">expression script</param>
        public static void CombineArgumentsAndExpressionFieldsModule(Dictionary<string, ExpressionRuntimeArgumentsModel> collectArgumentsModule, Dictionary<string, List<string>> collectExpressionFieldsModule, string expressionScript)
        {
            if (collectArgumentsModule == null)
            {
                collectArgumentsModule = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
            }

            if (collectExpressionFieldsModule == null)
            {
                collectExpressionFieldsModule = new Dictionary<string, List<string>>();
            }

            if (string.IsNullOrEmpty(expressionScript))
            {
                return;
            }

            Regex expReg = new Regex(RUN_EXPRESSION_PATTERN);

            if (!expReg.IsMatch(expressionScript))
            {
                return;
            }

            List<ExpressionRuntimeArgumentsModel> currentExpressionArguments = new List<ExpressionRuntimeArgumentsModel>();
            List<string> currentInputParams = new List<string>();
            var expMatchs = expReg.Matches(expressionScript);

            foreach (Match expMatch in expMatchs)
            {
                string argument = expMatch.Result("${Argument}");
                ExpressionRuntimeArgumentsModel expressionArgument = JsonConvert.DeserializeObject(argument, typeof(ExpressionRuntimeArgumentsModel)) as ExpressionRuntimeArgumentsModel;

                if (expressionArgument != null)
                {
                    currentExpressionArguments.Add(expressionArgument);
                }

                string inputparams = expMatch.Result("${InputParams}");
                string[] fieldNames = JsonConvert.DeserializeObject(inputparams, typeof(string[])) as string[];

                if (fieldNames != null)
                {
                    currentInputParams = currentInputParams.Union(fieldNames).ToList();
                }
            }

            Dictionary<string, ExpressionRuntimeArgumentsModel> asiExpArgs = GetEPArgumentsModels(currentExpressionArguments);
            CollectionInputParams(currentInputParams, collectExpressionFieldsModule, asiExpArgs);
            CombineArgumentsModule(collectArgumentsModule, asiExpArgs);
        }

        /// <summary>
        /// Use the '_' char to replace the special char.
        /// The rule is must same with the <c>RefExpressionUtil.dealSpecialChar</c> function in AA.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Replaced string</returns>
        public static string DealSpecialChar(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                Regex specChar = new Regex(@"[\.\s,\+""\^\\:\-']");
                return specChar.Replace(str, "_");
            }

            return string.Empty;
        }

        /// <summary>
        /// Generate full control field name
        /// </summary>
        /// <param name="capModel">CAP Model object</param>
        /// <param name="fieldName">field name</param>
        /// <returns>full variable key name</returns>
        public static string GetFullControlFieldName(CapModel4WS capModel, string fieldName)
        {
            StringBuilder fullExtFieldName = new StringBuilder();

            // In Reference contact,there is no cap model,so use field name and agencycode
            if (capModel != null)
            {
                fullExtFieldName.Append(fieldName);
                fullExtFieldName.Append(ACAConstant.SPLIT_CHAR);

                if (capModel.capType != null)
                {
                    fullExtFieldName.Append(capModel.capType.serviceProviderCode);
                    fullExtFieldName.Append(ACAConstant.SPLIT_CHAR);
                }

                string capTypeName = HttpUtility.UrlEncode(CAPHelper.GetAliasOrCapTypeLabel(capModel.capType));
                fullExtFieldName.Append(FilterSpciefCharForControlName(capTypeName));
            }
            else
            {
                fullExtFieldName.Append(fieldName);
                fullExtFieldName.Append(ACAConstant.SPLIT_CHAR);
                fullExtFieldName.Append(ConfigManager.AgencyCode);
            }

            return fullExtFieldName.ToString();
        }

        /// <summary>
        /// Collection Expression Input Variables from expression script(only for onLoad and onSubmit event)
        /// </summary>
        /// <param name="currentInputParams">current input parameters</param>
        /// <param name="collectExpressionFieldsModule">Field Name List of expression input fields</param>
        /// <param name="expressionArguments">Expression Argument Model</param>
        public static void CollectionInputParams(List<string> currentInputParams, Dictionary<string, List<string>> collectExpressionFieldsModule, Dictionary<string, ExpressionRuntimeArgumentsModel> expressionArguments)
        {
            if (collectExpressionFieldsModule == null || expressionArguments == null || expressionArguments.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<string, ExpressionRuntimeArgumentsModel> item in expressionArguments)
            {
                if (!collectExpressionFieldsModule.ContainsKey(item.Key))
                {
                    List<string> tmpFieldNameList = new List<string>();
                    collectExpressionFieldsModule.Add(item.Key, tmpFieldNameList);
                }

                collectExpressionFieldsModule[item.Key] = collectExpressionFieldsModule[item.Key].Union(currentInputParams).ToList();
            }
        }

        /// <summary>
        /// Get ExpressionRuntimeArgumentsModel object from "run expression" string.
        /// </summary>
        /// <param name="currentExpressionArguments">current expression arguments.</param>
        /// <returns>Expression Runtime Arguments Model4WS</returns>
        public static Dictionary<string, ExpressionRuntimeArgumentsModel> GetEPArgumentsModels(List<ExpressionRuntimeArgumentsModel> currentExpressionArguments)
        {
            var resultExpressionArguments = new Dictionary<string, ExpressionRuntimeArgumentsModel>();

            foreach (ExpressionRuntimeArgumentsModel expressionArgument in currentExpressionArguments)
            {
                var curArgs = GetEPArgumentsModels(expressionArgument);
                CombineArgumentsModule(resultExpressionArguments, curArgs);
            }

            return resultExpressionArguments;
        }

        /// <summary>
        /// Get ExpressionRuntimeArgumentsModel object from "run expression" string.
        /// </summary>
        /// <param name="expressionArgument">current expression arguments.</param>
        /// <returns>Expression Runtime Arguments Model4WS</returns>
        public static Dictionary<string, ExpressionRuntimeArgumentsModel> GetEPArgumentsModels(ExpressionRuntimeArgumentsModel expressionArgument)
        {
            var curArgs = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
            string key = string.Empty;

            if (expressionArgument.capID != null)
            {
                key = expressionArgument.capID.ID1 + expressionArgument.capID.ID2 + expressionArgument.capID.ID3;
            }
            else if (expressionArgument.argumentPKs != null && expressionArgument.argumentPKs.Any())
            {
                if (expressionArgument.argumentPKs[0].portletID == (long?)ExpressionType.ReferenceContact)
                {
                    key = ACAConstant.EXPRESSION_COMBINE_KEY_REFERENCE_CONTACT;
                }
                else if (expressionArgument.argumentPKs[0].portletID == (long?)ExpressionType.AuthAgent_Customer_Detail)
                {
                    key = ACAConstant.EXPRESSION_COMBINE_KEY_AUTHAGENT;
                }
                else if (expressionArgument.argumentPKs[0].portletID == (long?)ExpressionType.RefContact_Address)
                {
                    key = ACAConstant.EXPRESSION_COMBINE_KEY_REFERENCE_CONTACT_ADDRESS;
                }
                else if (expressionArgument.argumentPKs[0].portletID == (long?)ExpressionType.AuthAgent_Address)
                {
                    key = ACAConstant.EXPRESSION_COMBINE_KEY_AUTHAGENT_ADDRESS;
                }
            }

            curArgs.Add(key, expressionArgument);

            return curArgs;
        }

        /// <summary>
        /// filter control name for expression in ASI
        /// </summary>
        /// <param name="str">control name</param>
        /// <returns>final control name</returns>
        public static string FilterSpciefCharForControlName(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace('.', '_');
                str = str.Replace(' ', '_');
                str = str.Replace(',', '_');
                str = str.Replace('+', '_');
                str = str.Replace('"', '_');
                str = str.Replace('^', '_');
                str = str.Replace('\\', '_');
                str = str.Replace(':', '_');
                str = str.Replace('-', '_');
                str = str.Replace('\'', '_');
            }

            return str;
        }

        /// <summary>
        /// Clear Expression Variables in Session(Don't run in pop up page).
        /// </summary>
        public static void ClearExpressionVariables()
        {
            HttpContext.Current.Session[ACAConstant.Expression_InputVariables] = null;
            HttpContext.Current.Session[ACAConstant.Expression_SystemVariables] = null;
        }

        /// <summary>
        /// Register some script lib related to expression to current page
        /// </summary>
        /// <param name="currentPage">Web page object.</param>
        public static void RegisterScriptLibToCurrentPage(Page currentPage)
        {
            if (!currentPage.ClientScript.IsClientScriptIncludeRegistered("ExpressionJSSource"))
            {
                ServiceReference sr = new ServiceReference("~/WebService/ExpressionService.asmx");
                ScriptManager smg = ScriptManager.GetCurrent(currentPage);
                smg.EnablePageMethods = true;
                smg.Services.Add(sr);
                ScriptManager.RegisterClientScriptInclude(currentPage, typeof(Page), "ExpressionJSSource", currentPage.ResolveClientUrl("~/Scripts/Expression.js"));
            }
        }

        /// <summary>
        /// Reset JavaScript expression hidden value
        /// </summary>
        /// <param name="currentPage">Web page object.</param>
        public static void ResetJsExpression(Page currentPage)
        {
            if (!currentPage.ClientScript.IsStartupScriptRegistered("ResetExpressionSettings"))
            {
                string scriptStr = "if(typeof(ReSetControlParams)=='function')ReSetControlParams();";

                if (ScriptManager.GetCurrent(currentPage).IsInAsyncPostBack)
                {
                    scriptStr = " with (Sys.WebForms.PageRequestManager.getInstance()) { add_endRequest(ReSetControlParams);}";
                }

                ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), "ResetExpressionSettings", scriptStr, true);
            }
        }

        /// <summary>
        /// Update the <see cref="sourArgumentModels"/> into <see cref="destArgumentModels"/>.
        /// </summary>
        /// <param name="destArgumentModels">Destination arguments.</param>
        /// <param name="sourArgumentModels">Source arguments.</param>
        public static void CombineArgumentsModule(Dictionary<string, ExpressionRuntimeArgumentsModel> destArgumentModels, Dictionary<string, ExpressionRuntimeArgumentsModel> sourArgumentModels)
        {
            foreach (KeyValuePair<string, ExpressionRuntimeArgumentsModel> item in sourArgumentModels)
            {
                if (item.Value == null)
                {
                    continue;
                }

                string newKey = item.Key;

                if (!destArgumentModels.Values.Any(argment => argment.capID != null && item.Value != null &&
                    argment.capID.ID1 + argment.capID.ID2 + argment.capID.ID3 == newKey)
                    || item.Key == ACAConstant.EXPRESSION_COMBINE_KEY_REFERENCE_CONTACT
                    || item.Key == ACAConstant.EXPRESSION_COMBINE_KEY_REFERENCE_CONTACT_ADDRESS
                    || item.Key == ACAConstant.EXPRESSION_COMBINE_KEY_AUTHAGENT
                    || item.Key == ACAConstant.EXPRESSION_COMBINE_KEY_AUTHAGENT_ADDRESS)
                {
                    destArgumentModels.Add(newKey, item.Value);
                }
                else
                {
                    foreach (KeyValuePair<string, ExpressionRuntimeArgumentsModel> comItem in destArgumentModels)
                    {
                        if (comItem.Value == null || comItem.Value.capID == null)
                        {
                            continue;
                        }

                        if (newKey.Equals(comItem.Value.capID.ID1 + comItem.Value.capID.ID2 + comItem.Value.capID.ID3)
                            && item.Value.argumentPKs != null && item.Value.argumentPKs.Length > 0)
                        {
                            var keys = from key in item.Value.argumentPKs select key;
                            comItem.Value.argumentPKs = keys.Union(from key in comItem.Value.argumentPKs select key).ToArray();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove Input Variable from session. 
        /// The input variables are cancel on edit form page.
        /// only used for asit
        /// </summary>
        /// <param name="uiDataType">UI data type</param>
        public static void RemoveInputVariablesFromSession(UIDataType uiDataType)
        {
            if (uiDataType != UIDataType.ASITEdit)
            {
                return;
            }

            Dictionary<string, ExpressionFieldModel> inputVarsFromSession = HttpContext.Current.Session[ACAConstant.Expression_InputVariables] as Dictionary<string, ExpressionFieldModel>;

            if (inputVarsFromSession == null || inputVarsFromSession.Count == 0)
            {
                return;
            }

            Dictionary<string, UITable[]> uiTableList = UIModelUtil.GetDataFromUIContainer(uiDataType);

            if (uiTableList == null || uiTableList.Count == 0)
            {
                return;
            }

            // if delete current rows, it must remove from Expression_InputVariables session.
            List<string> removeKeys4InputVarsFromSession = new List<string>();

            foreach (var tables in uiTableList.Values)
            {
                foreach (ASITUITable uiTable in tables)
                {
                    if (uiTable == null)
                    {
                        continue;
                    }

                    foreach (var uiRow in uiTable.Rows)
                    {
                        string fieldPrefix = string.Format("ASIT_{0}_{1}_", uiTable.TableKey, uiRow.RowIndex);
                        removeKeys4InputVarsFromSession.Add(fieldPrefix);
                    }
                }
            }

            RemoveInputVariablesFromSession(removeKeys4InputVarsFromSession);
        }

        /// <summary>
        /// Remove Input Variable from session. 
        /// Delete ASIT row
        /// </summary>
        /// <param name="removeKeys">UI data type</param>
        public static void RemoveInputVariablesFromSession(List<string> removeKeys)
        {
            if (removeKeys == null || removeKeys.Count == 0)
            {
                return;
            }

            Dictionary<string, ExpressionFieldModel> inputVarsFromSession = HttpContext.Current.Session[ACAConstant.Expression_InputVariables] as Dictionary<string, ExpressionFieldModel>;

            if (inputVarsFromSession == null || inputVarsFromSession.Count == 0)
            {
                return;
            }

            foreach (string fieldPrefix in removeKeys)
            {
                List<string> fieldKeys = inputVarsFromSession.Keys.Where(w => w.StartsWith(fieldPrefix)).ToList();

                foreach (string fieldKey in fieldKeys)
                {
                    inputVarsFromSession.Remove(fieldKey);
                }
            }

            HttpContext.Current.Session[ACAConstant.Expression_InputVariables] = inputVarsFromSession;
        }

        /// <summary>
        /// Run expression for ASI/T onLoad event.
        /// </summary>
        /// <param name="moduleName">the module name</param>
        public static void RunExpressionForOnload(string moduleName)
        {
            SetExpressionResultToSession(null);

            Dictionary<string, Dictionary<string, ExpressionFieldModel>> collectExpressionFieldsModule = new Dictionary<string, Dictionary<string, ExpressionFieldModel>>();
            Dictionary<string, ExpressionRuntimeArgumentsModel> collectArgumentsModels = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            // asi
            IDictionary<string, CapModel4WS> asiSubCapModels = CapUtil.GetSubCapsByASIGroups(moduleName, capModel.appSpecificInfoGroups);
            ExpressionFactory asiExpInstance = new ExpressionFactory(moduleName, ExpressionType.ASI, null, asiSubCapModels);
            asiExpInstance.CombineArgumentsAndExpressionFieldsModuleForOnLoad(collectArgumentsModels, collectExpressionFieldsModule);

            // asit
            var allVisibleGroups = new List<AppSpecificTableGroupModel4WS>();
            AppSpecificTableModel4WS[] allVisibleTables = CapUtil.GetAllVisibleASITables(moduleName, capModel.appSpecTableGroups, allVisibleGroups);
            IDictionary<string, CapModel4WS> asitSubCapModels = CapUtil.GetSubCapsByASITGroups(moduleName, allVisibleGroups);
            ASITUITable[] asitUIData = ASITUIModelUtil.ConvertASITablesToUITables(allVisibleTables, moduleName, false, string.Empty);
            ExpressionFactory asitExpressionInstance = new ExpressionFactory(moduleName, ExpressionType.ASI_Table, null, null, asitUIData, asitSubCapModels, string.Empty);
            asitExpressionInstance.CombineArgumentsAndExpressionFieldsModuleForOnLoad(collectArgumentsModels, collectExpressionFieldsModule);

            // excute expression
            IExpressionBll expressionBll = ObjectFactory.GetObject<IExpressionBll>();

            foreach (var expressionRuntimeArgumentsModel in collectArgumentsModels)
            {
                ExpressionFieldModel[] inputFieldParams = collectExpressionFieldsModule[expressionRuntimeArgumentsModel.Key].Values.ToArray();

                foreach (var inputParam in inputFieldParams)
                {
                    inputParam.name = HttpUtility.UrlEncode(inputParam.name);
                }

                ExpressionRunResultModel4WS result = expressionBll.RunExpressions(expressionRuntimeArgumentsModel.Value, inputFieldParams);

                if (result != null && result.fields != null && result.fields.Any())
                {
                    AddExpressionResultToSession(expressionRuntimeArgumentsModel.Key, new KeyValuePair<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS>(expressionRuntimeArgumentsModel.Value, result));
                }
            }
        }

        /// <summary>
        /// Get sub group name.
        /// </summary>
        /// <param name="subGroupfield">the sub group field.</param>
        /// <returns>The sub group name.</returns>
        private static string GetSubGroupName(string subGroupfield)
        {
            string subGroupName = string.Empty;
            int subGroupPosition = subGroupfield.LastIndexOf(ACAConstant.SPLIT_DOUBLE_COLON, StringComparison.InvariantCultureIgnoreCase);

            if (subGroupPosition > 0)
            {
                subGroupName = subGroupfield.Substring(subGroupPosition + 2);
            }

            return subGroupName;
        }

        /// <summary>
        /// Get ASIT UI data key
        /// </summary>
        /// <param name="expField">the expression field.</param>
        /// <returns>The ASIT UI data key.</returns>
        private static IList<string> GetASITUIDataKey(ExpressionFieldModel expField)
        {
            IList<string> uiDataKeys = new List<string>();

            if (expField.viewID == null)
            {
                return uiDataKeys;
            }

            // step 1 get portlet model
            ExpressionType expressionType = (ExpressionType)expField.viewID;
            ExpressionTypeModel expressionPortletModel = ExpressionTypeModels.FirstOrDefault(f => f.TPL_TableViewID == expressionType);

            if (expressionType == ExpressionType.ASI_Table)
            {
                expressionPortletModel = ExpressionTypeModels.FirstOrDefault(f => f.PortletID == expressionType);
            }

            if (expressionPortletModel == null)
            {
                return uiDataKeys;
            }

            ExpressionType portlet = expressionPortletModel.PortletID;

            // step 2 Get session expression
            Hashtable expressionSessionData = AppSession.GetExpressionDataFromSession();

            if (expressionSessionData == null || expressionSessionData.Count == 0)
            {
                return uiDataKeys;
            }

            // step 3 get ui data key.
            foreach (DictionaryEntry dictionaryEntry in expressionSessionData)
            {
                ExpressionSessionModel expressionSessionModel = dictionaryEntry.Value as ExpressionSessionModel;

                if (expressionSessionModel != null && expressionSessionModel.ExpressionFactoryType == portlet && !string.IsNullOrEmpty(expressionSessionModel.AsitUiDataKey))
                {
                    uiDataKeys.Add(expressionSessionModel.AsitUiDataKey);
                }
            }

            return uiDataKeys;
        }

        /// <summary>
        /// Apply expression result fields to UI table.
        /// </summary>
        /// <param name="expFields">Expression field list.</param>
        /// <param name="uiTables">this asit UI table list.</param>
        private static void ApplyExpressionToUITables(IEnumerable<ExpressionFieldModel> expFields, ASITUITable[] uiTables)
        {
            if (expFields == null || uiTables == null)
            {
                return;
            }

            foreach (var expField in expFields)
            {
                if (!IsValidASITExpField(expField))
                {
                    continue;
                }

                //Get table key and field name from expression field.
                string tableKey = expField.subGroup.Replace(ExpressionFactory.ASIT_FIELD_PREFIX, string.Empty);
                ASITUITable table = uiTables.SingleOrDefault(t => t.TableKey == tableKey);

                if (table == null)
                {
                    continue;
                }

                string fieldName = expField.name;

                //For newly added rows by expression.
                if (fieldName.StartsWith(ExpressionFactory.INSERT_FIRST_ROW))
                {
                    string[] nameInfos = fieldName.Split(ACAConstant.SPLIT_CHAR);

                    if (nameInfos.Length > 1 && Regex.IsMatch(nameInfos[1], PatternASITFieldName, RegexOptions.IgnoreCase))
                    {
                        fieldName = nameInfos[1];
                    }
                }

                //Search out UI field by field ID.
                ASITUIField uiField = (from row in table.Rows
                                       from field in row.Fields
                                       where field.FieldID.Equals(fieldName)
                                       select field as ASITUIField).SingleOrDefault();

                //Apply value and hidden behavior to UI field.
                if (uiField != null)
                {
                    uiField.Value = expField.value.ToString();

                    if (expField.hidden != null)
                    {
                        uiField.IsHiddenByExp = expField.hidden.Value;
                    }

                    if (expField.readOnly != null)
                    {
                        uiField.IsReadOnlyByExp = expField.readOnly.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Create new UI table for expression inserted fields.
        /// </summary>
        /// <param name="baseUIData">ASIT UI table. Will based on the table to retrieve template row and current row index.</param>
        /// <param name="insertedFields">Inserted fields</param>
        /// <param name="insertedTableKeys">Table keys of inserted fields.</param>
        /// <param name="uiDataKey">asit UI data key.</param>
        /// <returns>ASIT UI table list.</returns>
        private static List<ASITUITable> CreateASITUITable4InsertRow(ASITUITable[] baseUIData, IEnumerable<ExpressionFieldModel> insertedFields, IEnumerable<string> insertedTableKeys, string uiDataKey)
        {
            if (baseUIData == null || insertedFields == null)
            {
                return null;
            }

            var result = new List<ASITUITable>();

            //Generate field ID based on the UI data for new inserted fields.
            foreach (ExpressionFieldModel expField in insertedFields)
            {
                string tableKey = expField.subGroup.Replace(ExpressionFactory.ASIT_FIELD_PREFIX, string.Empty);
                ASITUITable asiTable = baseUIData.SingleOrDefault(t => t.TableKey == tableKey);

                if (asiTable != null)
                {
                    var rowIndex = ASITUIModelUtil.GetNewRowIndex(asiTable);
                    var fieldName = string.IsNullOrEmpty(expField.variableKey) ? string.Empty : expField.variableKey.Substring(expField.variableKey.LastIndexOf(ACAConstant.SPLIT_DOUBLE_COLON, StringComparison.Ordinal) + ACAConstant.SPLIT_DOUBLE_COLON.Length).GetHashCode().ToString("X2");
                    var fieldID = ControlBuildHelper.GetASITFieldID(asiTable.TableKey, rowIndex, fieldName);

                    if (expField.viewID == (long)ExpressionType.ASI_Table)
                    {
                        fieldID = ControlBuildHelper.GetASITFieldID(asiTable.TableKey, rowIndex, expField.columnIndex.ToString());
                    }

                    expField.name = ExpressionFactory.INSERT_FIRST_ROW + ACAConstant.SPLIT_CHAR + fieldID;
                }
            }

            //Create new row based on the inserted fields.
            foreach (string tableKey in insertedTableKeys)
            {
                ASITUITable sTable = baseUIData.Where(t => t.TableKey == tableKey).SingleOrDefault();
                ASITUITable dTable = ASITUIModelUtil.CreateNewRow4ASITUITable(sTable, 1, uiDataKey);

                if (dTable != null)
                {
                    result.Add(dTable);
                }
            }

            return result;
        }

        /// <summary>
        /// Indicating the Expression field whether is a valid ASIT expression variable.
        /// 1. ViewID is -2;
        /// 2. Not a blockSubmit variable;
        /// 3. "subGroup" started with "ASIT::".
        /// </summary>
        /// <param name="expField">Expression field model.</param>
        /// <returns>boolean value.</returns>
        private static bool IsValidASITExpField(ExpressionFieldModel expField)
        {
            return IsASITOrGenericTempalteTableView(expField.viewID) &&
                   expField.blockSubmit != true &&
                   !string.IsNullOrEmpty(expField.subGroup);
        }

        /// <summary>
        /// Is ASIT variable for expression.
        /// </summary>
        /// <param name="variableKey">the variable Key</param>
        /// <returns>true or false.</returns>
        private static bool IsASITVariableKey(string variableKey)
        {
            if (!string.IsNullOrEmpty(variableKey) && (variableKey.StartsWith("ASIT::") || variableKey.StartsWith("CONTACT1TPLTABLE::")
                || variableKey.StartsWith("CONTACT2TPLTABLE::") || variableKey.StartsWith("CONTACT3TPLTABLE::")
                || variableKey.StartsWith("APPLICANTTPLTABLE::") || variableKey.StartsWith("REFCONTACTTPLTABLE::")
                || variableKey.StartsWith("CONTACTTPLTABLE::") || variableKey.StartsWith("CUSTOMERTPLTABLE::")))
            {
                return true;
            }

            // the popup ASIT row submit event name
            if (string.Equals(ExpressionFactory.ASITROW_SUBMIT_EVENT_NAME, variableKey, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sort the table keys based on the UI data.
        /// Because of the input parameters returned from AA is out-of-order,
        /// so need sort the ASI Table based the UI data.
        /// </summary>
        /// <param name="tableKeys">ASI Table keys.</param>
        /// <param name="baseUIData">Based UI data.</param>
        /// <returns>Ordered table keys.</returns>
        private static List<string> SortASITTableKeysBasedUIData(IEnumerable<string> tableKeys, IEnumerable<ASITUITable> baseUIData)
        {
            if (tableKeys == null || baseUIData == null)
            {
                return null;
            }

            var sortedTableKeys = new List<string>();

            foreach (var table in baseUIData)
            {
                if (tableKeys.Where(key => table.TableKey.Equals(key)).Count() > 0)
                {
                    sortedTableKeys.Add(table.TableKey);
                }
            }

            return sortedTableKeys;
        }
    }
}