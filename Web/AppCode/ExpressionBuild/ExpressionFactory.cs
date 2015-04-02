#region Header
/*
 * <pre>
 *  Accela Citizen Access
 *  File: ExpressionFactory.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 * class define all functions of expression.
 *  Notes:
 * $Id: ExpressionFactory.cs 131474 2009-06-08 02:34:33Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.ExpressionBuild
{
    /// <summary>
    /// Defined methods that is referenced Expression.
    /// </summary>
    public class ExpressionFactory
    {
        #region Fields

        /// <summary>
        /// Expression field prefix for ASIT fields.
        /// </summary>
        public const string ASIT_FIELD_PREFIX = "ASIT::";

        /// <summary>
        /// ASIT field name for insert new row.
        /// </summary>
        public const string INSERT_FIRST_ROW = "InsertFirstRow";

        /// <summary>
        /// Fee items' quantity field label who is switched fee item's expression model name property.
        /// </summary>
        public const string EXPRESSION_QUANTITY_LABEL = "Quantity";

        /// <summary>
        /// split char.
        /// </summary>
        public const char SPLIT_CHAR = '_';

        /// <summary>
        /// Control's submit event name.
        /// </summary>
        public const string SUBMIT_EVENT_NAME = "onsubmit";

        /// <summary>
        /// Loading event name of page.
        /// </summary>
        public const string LOAD_EVENT_NAME = "onload";

        /// <summary>
        /// On Populate Event
        /// </summary>
        public const string ON_POPULATE_NAME = "onPopulate";

        /// <summary>
        /// the popup ASIT Row Submit event name.
        /// </summary>
        public const string ASITROW_SUBMIT_EVENT_NAME = "OnASITRowSubmit";

        /// <summary>
        /// Run expression function  name on client side.
        /// </summary>
        public const string RUN_EXPRESSION_NAME = "RunExpression";

        /// <summary>
        /// ASI Table template row index
        /// </summary>
        private const int ASIT_TEMPLATE_ROW_INDEX = -1;

        /// <summary>
        /// View ID for Expression System Variables
        /// </summary>
        private const string EXPRESSION_SYS_FIELD_VIEWID = "-99999";

        /// <summary>
        /// View ID for Record Detail Variables
        /// </summary>
        private const string EXPRESSION_RECORD_DETAIL_FIELD_VIEWID = "112";

        /// <summary>
        /// ASIT Expression script name list
        /// The argument PK models.
        /// </summary>
        private List<ExpressionRuntimeArgumentPKModel> _argumentPKModels = new List<ExpressionRuntimeArgumentPKModel>();

        /// <summary>
        /// sub cap model. It apply in super agency.
        /// </summary>
        private IDictionary<string, CapModel4WS> _subCapModels = new Dictionary<string, CapModel4WS>();

        /// <summary>
        /// current cap model.
        /// </summary>
        private CapModel4WS _capModel;

        /// <summary>
        /// expression type.
        /// </summary>
        private ExpressionType _expressionType;

        /// <summary>
        /// enabled calculate input dictionary. Key name is execute field name.
        /// </summary>
        private Dictionary<string, ExpressionFieldModel[]> _enabledInputParams = new Dictionary<string, ExpressionFieldModel[]>();

        /// <summary>
        /// execute field list.
        /// </summary>
        private Dictionary<string, ExpressionFieldModel> _executeFieldList;

        /// <summary>
        /// Require to calculate 
        /// </summary>
        private Dictionary<string, WebControl> _expressionControls = new Dictionary<string, WebControl>();

        /// <summary>
        /// Controls count for Fee expression.
        /// </summary>
        private int _feeControlsCount;

        /// <summary>
        /// Max row count for every ASIT sub-group.
        /// </summary>
        private Dictionary<string, CapModel4WS> _capModels = new Dictionary<string, CapModel4WS>();

        /// <summary>
        /// fee item list.
        /// </summary>
        private Dictionary<string, List<F4FeeItemModel4WS>> _feeItems = new Dictionary<string, List<F4FeeItemModel4WS>>();

        /// <summary>
        /// UI table list.
        /// </summary>
        private IEnumerable<UITable> _uiTables;

        /// <summary>
        /// AppSpecific Field List from CAP Model
        /// </summary>
        private Dictionary<string, AppSpecificInfoModel4WS> _appSpecficInfoFieldList = new Dictionary<string, AppSpecificInfoModel4WS>();

        /// <summary>
        /// the expression key for asit UI data key.
        /// Because the asit onChange event is in pop up page, 
        ///     when it need the parent of the pop up page control value, 
        ///          it easily get the control info from the session by the asit UI data key.
        /// </summary>
        private string _asitUiDataKey = string.Empty;

        /// <summary>
        /// The flag indicates the expression builder is already attached to control
        /// </summary>
        private bool _isExpressionAttachedToControl = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ExpressionFactory class.
        /// Initializes field variable to run expression in license(which not contain generic template form/table)
        /// Be used in License, Address, Contact Address, Reference Contact Address.(which expression type not need consider generic template form/table and not need consider sub cap model)
        /// </summary>
        /// <param name="moduleName">current module name</param>
        /// <param name="expressionType">expression type</param>
        /// <param name="expressionControls">reference expression control list.</param>
        public ExpressionFactory(string moduleName, ExpressionType expressionType, Dictionary<string, WebControl> expressionControls)
        {
            Constructors4ExpressionFactory(moduleName, expressionType, expressionControls, null, null, null, string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the ExpressionFactory class.
        /// Be used in ASI Section, Fee item. (which expression type need consider sub cap model, but not need consider generic template form/table)
        /// </summary>
        /// <param name="moduleName">current module name</param>
        /// <param name="expressionType">expression type</param>
        /// <param name="expressionControls">reference expression control list.</param>
        /// <param name="subCapModels">CAP model list.</param>
        public ExpressionFactory(string moduleName, ExpressionType expressionType, Dictionary<string, WebControl> expressionControls, IDictionary<string, CapModel4WS> subCapModels)
        {
            if (expressionType == ExpressionType.Fee_Item && expressionControls != null && expressionControls.Any())
            {
                _feeControlsCount = expressionControls.Count;
            }

            Constructors4ExpressionFactory(moduleName, expressionType, expressionControls, null, null, subCapModels, string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the ExpressionFactory class.
        /// Be used in Contact.(which expression type need consider generic template form/table but not need consider sub cap model)
        /// </summary>
        /// <param name="moduleName">Module name.</param>
        /// <param name="expressionType">Expression type.</param>
        /// <param name="expressionControls">Expression controls.</param>
        /// <param name="argumentPkModels">the expression runtime argument PK model list for asit expression.</param>
        /// <param name="asitUiTables">ASIT UI tables.</param>
        /// <param name="asitUiDataKey">asit UI data key</param>
        public ExpressionFactory(string moduleName, ExpressionType expressionType, Dictionary<string, WebControl> expressionControls, List<ExpressionRuntimeArgumentPKModel> argumentPkModels, IEnumerable<ASITUITable> asitUiTables, string asitUiDataKey)
        {
            Constructors4ExpressionFactory(moduleName, expressionType, expressionControls, argumentPkModels, asitUiTables, null, asitUiDataKey);
        }

        /// <summary>
        /// Initializes a new instance of the ExpressionFactory class.
        /// Initializes field variable to run expression in ASIT , generic template table. 
        /// </summary>
        /// <param name="moduleName">Module name.</param>
        /// <param name="expressionType">Expression type.</param>
        /// <param name="expressionControls">Expression controls</param>
        /// <param name="argumentPkModels">the expression runtime argument PK model list for asit expression.</param>
        /// <param name="asitUiTables">ASIT UI tables.</param>
        /// <param name="subCapModels">Sub cap models for super cap.</param>
        /// <param name="asitUiDataKey">asit UI data key</param>
        public ExpressionFactory(string moduleName, ExpressionType expressionType, Dictionary<string, WebControl> expressionControls, List<ExpressionRuntimeArgumentPKModel> argumentPkModels, IEnumerable<ASITUITable> asitUiTables, IDictionary<string, CapModel4WS> subCapModels, string asitUiDataKey)
        {
            Constructors4ExpressionFactory(moduleName, expressionType, expressionControls, argumentPkModels, asitUiTables, subCapModels, asitUiDataKey);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets sub-CAP models.
        /// </summary>
        public IDictionary<string, CapModel4WS> SubCapModels
        {
            get
            {
                return _subCapModels;
            }

            set
            {
                _subCapModels = value;
            }
        }

        /// <summary>
        /// Gets execute field list.
        /// </summary>
        public List<ExpressionFieldModel> ExecuteFieldList
        {
            get
            {
                return _executeFieldList != null ? _executeFieldList.Values.ToList() : null;
            }
        }

        /// <summary>
        /// Gets or sets fee item list for expression of cap fee.
        /// </summary>
        public Dictionary<string, List<F4FeeItemModel4WS>> FeeItems
        {
            get
            {
                return _feeItems;
            }

            set
            {
                _feeItems = value;
            }
        }

        /// <summary>
        /// Gets cap model congregation. It works in super agency. 
        /// </summary>
        private Dictionary<string, CapModel4WS> CapModels
        {
            get
            {
                return _capModels;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attach 'JavaScript' function into trigger event of specify control list.
        /// </summary>
        /// <param name="keyName">Control collection's key name</param>
        /// <param name="ctl">specify control</param>
        /// <param name="currentPage">current activity page</param>
        public void AttachEventToControl(string keyName, WebControl ctl, Page currentPage)
        {
            foreach (ExpressionFieldModel ews in _executeFieldList.Values)
            {
                bool isAttachTheControl = false;

                if (ExpressionUtil.IsASITOrGenericTempalteTableView(ews.viewID))
                {
                    //This [keyName] is genreated by "GetFullControlFieldName" or "ASIBaseUC.CreateWebControl"(SuperAgency)
                    //Format is: {FieldName} + ACAConstant.SPLIT_CHAR + {Agency Code} + ACAConstant.SPLIT_CHAR + {Cap Type Name}
                    string[] keyItems = keyName.Split(ACAConstant.SPLIT_CHAR);
                    string agencyCode = keyItems[1];
                    string[] tpName = RemoveAgencyCaptypeText(keyName, agencyCode).Split(SPLIT_CHAR);
                    string asitSubGroupName = GetSubGroupName(keyName, agencyCode);
                    string subGroupName = string.Empty;

                    if (ews.variableKey != null)
                    {
                        subGroupName = GetASITableNameByVariableKey(ews.variableKey);
                    }

                    if (!string.IsNullOrEmpty(subGroupName))
                    {
                        //the variableKey combined with three parts, such as "ASIT::My_Group_Name::Field_Name"
                        var fieldName = RemoveAgencyCaptypeText(ews.name, agencyCode);

                        if (ExpressionUtil.IsGenericTempalteTableView(ews.viewID))
                        {
                            fieldName = fieldName.GetHashCode().ToString("X2");
                        }

                        isAttachTheControl = HttpUtility.UrlDecode(tpName[tpName.Length - 1]) == HttpUtility.UrlDecode(fieldName)
                                            && HttpUtility.UrlDecode(subGroupName) == HttpUtility.UrlDecode(asitSubGroupName);
                    }
                }
                else
                {
                    string controlName = HttpUtility.UrlEncode(HttpUtility.UrlDecode(ews.name));
                    Dictionary<string, WebControl> controls = EncodeExpressionControlKey();

                    if (controls.ContainsKey(controlName))
                    {
                        isAttachTheControl = controls[controlName] == ctl;
                    }
                }

                if (isAttachTheControl)
                {
                    StringBuilder expScriptsBuilder = new StringBuilder();
                    CapModel4WS capModel = CapModels != null && CapModels.Count > 0 ? CapModels[ews.name] : null;
                    ExpressionFieldModel[] inputParams = GetInputParamList(capModel, ews);

                    if (ExpressionUtil.IsIncludeASITFields(this._expressionType))
                    {
                        ExpressionRuntimeArgumentsModel expRuntimeArgModel = this.GetERArgumentsModel(capModel, ews.variableKey);
                        List<ExpressionFieldModel> expInParamList = this.AssembleExpressionInputParamters(capModel, inputParams);
                        expScriptsBuilder.Append(this.AssembleExpressionRuntimeScript(expRuntimeArgModel, expInParamList.ToArray(), false));
                    }
                    else
                    {
                        expScriptsBuilder.Append(GetClientExperssionScripts(capModel, ews, inputParams, false));
                    }

                    expScriptsBuilder.Append(";");

                    BindingClientMethod(ctl, ews.fireEvent.ToLower(), expScriptsBuilder.ToString());
                }
            }
        }

        /// <summary>
        /// Attach 'javascript' function into trigger event of specify control list.
        /// </summary>        
        public void AttachEventToControl()
        {
            // Step 1,  Return if no execute fields or the execute field is attached already.

            /* In order to improve the performance of page load, below code would stop to attach expression builder to each ASI field dulicatly, 
             * and the duplicated times is the number of ASI Section in one step/page.
             * The best solution is seperate the instance of expression factory for each ASI component in CapEdit.aspx
             */ 
            if (_isExpressionAttachedToControl || _executeFieldList == null || _executeFieldList.Count == 0)
            {
                return;
            }

            _isExpressionAttachedToControl = true;

            Dictionary<string/*Execute Field Name*/, ExpressionRuntimeArgumentsModel/*Expression RuntimeArgument Model*/> dicExpRuntimeArgModels = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
            Dictionary<string/*Cap ID*/, CapModel4WS> dicCapIDAndCapModels = new Dictionary<string, CapModel4WS>();

            // Dictionary<Execute Field Name, Input paramter>
            Dictionary<string, ExpressionFieldModel[]> dicExpressionInputFields = new Dictionary<string, ExpressionFieldModel[]>();

            CapModel4WS capModel = null;
            Dictionary<string, WebControl> controls = EncodeExpressionControlKey();

            // Step 2, Collect Expression Argument Models and Cap Models
            foreach (ExpressionFieldModel ews in _executeFieldList.Values)
            {
                string controlName = HttpUtility.UrlEncode(HttpUtility.UrlDecode(ews.name));

                // Only when the execute field exists in the collected controls
                if (controls.ContainsKey(controlName) && controls[controlName] != null)
                {
                    capModel = CapModels != null && CapModels.Count > 0 ? CapModels[ews.name] : null;

                    // Assimble one expression runtime argument model
                    ExpressionRuntimeArgumentsModel expRuntimeArgModel = GetERArgumentsModel(capModel, ews.variableKey);

                    string excuteFieldKey = ews.variableKey + ACAConstant.SPLIT_CHAR + ews.name;

                    if (!dicExpRuntimeArgModels.ContainsKey(excuteFieldKey))
                    {
                        // Collect the expression runtime argument model to get the input parameters
                        dicExpRuntimeArgModels.Add(excuteFieldKey, expRuntimeArgModel);
                    }

                    // Collect the cap model. Note: There may be many child cap model on the super agency model
                    if (capModel != null && capModel.capID != null)
                    {
                        if (!dicCapIDAndCapModels.ContainsKey(capModel.capID.toKey()))
                        {
                            dicCapIDAndCapModels.Add(capModel.capID.toKey(), capModel);
                        }
                    }
                }
            }

            // If no expression argument model, return directly
            if (dicExpRuntimeArgModels.Count == 0)
            {
                return;
            }

            // Step 3, Get the input expression parameters for the collected execute fields
            dicExpressionInputFields = GetInputParamListForMultiExecuteFields(dicExpRuntimeArgModels, GetASIFieldsFromCapModels(dicCapIDAndCapModels));

            // Step 4, Attach expression builder to control
            AttachExpressionBuilderToControls(controls, dicExpRuntimeArgModels, dicExpressionInputFields);
        }
        
        /// <summary>
        /// Get Input Expression Parameters for multiple execute fields
        /// </summary>
        /// <param name="expRuntimeArgModels">Dictionary of Execute Field Name and Expression Argument Model</param>
        /// <param name="appSpecificInfoModels">Application Information Field List</param>
        /// <returns>The dictionary of Execute Field Name and Input Expression Parameter List</returns>
        public Dictionary<string, ExpressionFieldModel[]> GetInputParamListForMultiExecuteFields(Dictionary<string, ExpressionRuntimeArgumentsModel> expRuntimeArgModels, AppSpecificInfoModel[] appSpecificInfoModels)
        {
            Dictionary<string, ExpressionFieldModel[]> results = new Dictionary<string, ExpressionFieldModel[]>();

            // Step 1, Validate the input parameter
            if (expRuntimeArgModels == null || expRuntimeArgModels.Keys.Count == 0)
            {
                return results;
            }

            // Step 2, Get the expression input parameters for multiple execute fields
            IExpressionBll expressionBll = ObjectFactory.GetObject<IExpressionBll>();
            ExpressionInputParamterResultModel4WS[] expInputParamterResults = expressionBll.GetInputParametersForMultiExecuteFields(expRuntimeArgModels.Values.ToArray(), appSpecificInfoModels);

            // Step 3, Build one dictionary <control name, imput paramter list>
            foreach (string name in expRuntimeArgModels.Keys)
            {
                ExpressionRuntimeArgumentsModel argModel = expRuntimeArgModels[name];

                if (argModel == null)
                {
                    continue;
                }

                // Step 3.1 Find the matched expression input paramters result model with same agency code and execute field key
                ExpressionInputParamterResultModel4WS expInputParametersResultModel = null;

                foreach (ExpressionInputParamterResultModel4WS expInputParameterResultModel in expInputParamterResults)
                {
                    if (expInputParameterResultModel != null
                        && expInputParameterResultModel.servProvCode.Equals(argModel.servProvCode) // Under the multiple agency enviroment, the variable key may be same
                        && expInputParameterResultModel.executeFieldName.Contains<string>(argModel.executeFieldVariableKey))
                    {
                        expInputParametersResultModel = expInputParameterResultModel;
                        break;
                    }
                }

                // Step 3.2 Update the input expression parameters and store them into one local dictionary
                if (expInputParametersResultModel != null)
                {
                    UpdateASIExpName(expInputParametersResultModel.inputParameters);

                    _enabledInputParams.Add(name, expInputParametersResultModel.inputParameters);

                    results.Add(name, CloneExpressionParam(expInputParametersResultModel.inputParameters));
                }
            }

            return results;
        }

        /// <summary>
        /// Binding "JavaScript" function into control's event.
        /// </summary>
        /// <param name="ctl">It is the bind control</param>
        /// <param name="eventName">event name</param>
        /// <param name="functionName">called function script</param>
        public void BindingClientMethod(WebControl ctl, string eventName, string functionName)
        {
            if (ctl is TextBox)
            {
                //The RunExpression function must be executed after the Postback.
                if (((TextBox)ctl).AutoPostBack && eventName.Equals("onchange", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ctl.Page != null && ctl.Page.IsPostBack && string.Equals(ctl.UniqueID, ctl.Page.Request[Page.postEventSourceID], StringComparison.OrdinalIgnoreCase))
                    {
                        ScriptManager.RegisterStartupScript(ctl, typeof(TextBox), "RunExpressionAfterPostBack", functionName, true);
                    }
                }
                else
                {
                    string script = ctl.Attributes[eventName] + functionName;
                    ctl.Attributes[eventName] = MergeExpressionScripts(script);
                }
            }
            else if (ctl is DropDownList)
            {
                //The RunExpression function must be executed after the Postback.
                if (((DropDownList)ctl).AutoPostBack && eventName.Equals("onchange", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ctl.Page != null && ctl.Page.IsPostBack && string.Equals(ctl.UniqueID, ctl.Page.Request[Page.postEventSourceID], StringComparison.OrdinalIgnoreCase))
                    {
                        ScriptManager.RegisterStartupScript(ctl, typeof(DropDownList), "RunExpressionAfterPostBack", functionName, true);
                    }
                }
                else
                {
                    string script = ctl.Attributes[eventName] + functionName;
                    ctl.Attributes[eventName] = MergeExpressionScripts(script);
                }
            }
            else if (ctl is RadioButtonList)
            {
                RadioButtonList radios = ctl as RadioButtonList;

                if (radios != null)
                {
                    foreach (ListItem radio in radios.Items)
                    {
                        string script = radio.Attributes["onclick"] + functionName;
                        radio.Attributes["onclick"] = MergeExpressionScripts(script);
                    }
                }
            }
            else if (ctl is AccelaCheckBox)
            {
                AccelaCheckBox accb = ctl as AccelaCheckBox;

                if (accb != null)
                {
                    string script = accb.Attributes["onclick"] + functionName;
                    accb.Attributes["onclick"] = MergeExpressionScripts(script);
                }
            }
            else if (ctl is CheckBox)
            {
                CheckBox accb = ctl as CheckBox;

                if (accb != null)
                {
                    string script = accb.Attributes["onclick"] + functionName;
                    accb.Attributes["onclick"] = MergeExpressionScripts(script);
                }
            }
            else if (ctl is AccelaStateControl)
            {
                AccelaStateControl stateCtl = ctl as AccelaStateControl;
                stateCtl.AddAttribute(eventName, functionName);
            }
        }

        /// <summary>
        /// Attach running expression function into page's "onLoad" event handle.
        /// </summary>
        /// <param name="currentPage">Current activity page.</param>
        public void AttachOnLoadEvent(Page currentPage)
        {
            string functionName = string.Empty;
            string jsBlockName = string.Empty;
            StringBuilder functionNameBuilder = new StringBuilder();

            foreach (ExpressionFieldModel ews in _executeFieldList.Values)
            {
                if (ews != null && !string.IsNullOrEmpty(ews.fireEvent))
                {
                    if (ews.fireEvent.ToLower() == LOAD_EVENT_NAME)
                    {
                        CapModel4WS capModel = CapModels != null && CapModels.Count > 0 ? CapModels[ews.name] : null;
                        ExpressionFieldModel[] inputParams = GetInputParamList(capModel, ews);
                        functionNameBuilder.Append(GetClientExperssionScripts(capModel, ews, inputParams, false));
                        functionNameBuilder.Append(";");
                        functionName = functionNameBuilder.ToString();
                        jsBlockName = ews.name + this._expressionType;
                    }
                }
            }

            if (!currentPage.ClientScript.IsClientScriptBlockRegistered(jsBlockName))
            {
                ScriptManager.RegisterStartupScript(currentPage, typeof(Page), jsBlockName, functionName, true);
            }
        }

        /// <summary>
        /// Get run expression function on "onLoad" event is triggered;
        /// </summary>
        /// <returns>The expression</returns>
        public string GetRunExpFunctionOnLoad()
        {
            StringBuilder functionName = new StringBuilder();

            foreach (ExpressionFieldModel ews in _executeFieldList.Values)
            {
                if (ews == null || string.IsNullOrEmpty(ews.fireEvent) || !ews.fireEvent.Equals(LOAD_EVENT_NAME, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                CapModel4WS capModel = CapModels != null && CapModels.Count > 0 ? CapModels[ews.name] : null;
                ExpressionFieldModel[] inputParams = GetInputParamList(capModel, ews);
                List<ExpressionFieldModel> resultExpInParamList = AssembleExpressionInputParamters(capModel, inputParams);

                functionName.Append(GetClientExperssionScripts(capModel, ews, resultExpInParamList.ToArray(), false));
                functionName.Append(";");
            }

            return functionName.ToString();
        }

        /// <summary>
        /// get run expression function for submit action
        /// </summary>
        /// <returns>complete run expression for JS function name</returns>
        public string GetRunExpFunctionOnSubmit()
        {
            StringBuilder functionName = new StringBuilder();

            foreach (ExpressionFieldModel ews in _executeFieldList.Values)
            {
                if (ews == null || string.IsNullOrEmpty(ews.fireEvent) || !ews.fireEvent.Equals(SUBMIT_EVENT_NAME, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                CapModel4WS capModel = CapModels != null && CapModels.Count > 0 ? CapModels[ews.name] : null;
                ExpressionFieldModel[] inputParams = GetInputParamList(capModel, ews);
                List<ExpressionFieldModel> resultExpInParamList = AssembleExpressionInputParamters(capModel, inputParams);

                functionName.Append(GetClientExperssionScripts(capModel, ews, resultExpInParamList.ToArray(), true));
                functionName.Append(";");
            }

            return functionName.ToString();
        }

        /// <summary>
        /// Run ASIT Expression Function for onLoad event.
        /// </summary>
        /// <returns>Return the expression result model.</returns>
        public Dictionary<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS> RunExpressionForOnLoad()
        {
            var dicExpressionResult = new Dictionary<ExpressionRuntimeArgumentsModel, ExpressionRunResultModel4WS>();

            foreach (ExpressionFieldModel ews in _executeFieldList.Values)
            {
                if (ews == null || string.IsNullOrEmpty(ews.fireEvent) || !LOAD_EVENT_NAME.Equals(ews.fireEvent, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                // Step-1: Get Cap Model
                CapModel4WS capModel = CapModels != null && CapModels.Count > 0 ? CapModels[ews.name] : null;

                // Step-2: Collect Expression Input Paramters
                ExpressionFieldModel[] inputParams = GetInputParamList(capModel, ews);

                // Step-3: Assemble Expression Input Paramters
                List<ExpressionFieldModel> expInParamList = AssembleExpressionInputParamters(capModel, inputParams);

                // Step-4: Get Expression Runtime Argument Model
                ExpressionRuntimeArgumentsModel expRuntimeArgModel = this.GetERArgumentsModel(capModel, ews.variableKey);

                //ExpressionFieldModel.name might contain special char which will cause web service running failed, and the value is not used in the web service invocation, so set the value to string.Empty.
                var asiInputParameters = expInParamList.Where(e => e != null && !string.IsNullOrEmpty(e.variableKey) && e.viewID == (int)ExpressionType.ASI);

                if (asiInputParameters.Count() > 0)
                {
                    ExpressionFieldModel[] asiInputVariables = asiInputParameters.ToArray();

                    foreach (ExpressionFieldModel variable in asiInputVariables)
                    {
                        variable.name = variable.name.Replace(ACAConstant.SPLIT_CHAR, '_');
                    }
                }

                dicExpressionResult.Add(
                    expRuntimeArgModel,
                    RunExpression(expRuntimeArgModel, expInParamList.ToArray()));
            }

            return dicExpressionResult;
        }

        /// <summary>
        /// Collection Expression Argument and input parameters
        /// </summary>
        /// <param name="collectArgumentsModule">the expression argument array.<!--Dictionary<(expressionArgument key,see ExpressionUtil.GetEPArgumentsModels)), ExpressionRuntimeArgumentsModel>--></param>
        /// <param name="collectExpressionFieldsModule">the expression input field array.<!--Dictionary<expressionArgument.key, Dictionary<Expression field key, see GetExpressionFieldKey, ExpressionFieldModel>>--></param>
        public void CombineArgumentsAndExpressionFieldsModuleForOnLoad(Dictionary<string, ExpressionRuntimeArgumentsModel> collectArgumentsModule, Dictionary<string, Dictionary<string, ExpressionFieldModel>> collectExpressionFieldsModule)
        {
            foreach (ExpressionFieldModel ews in _executeFieldList.Values)
            {
                if (ews == null || string.IsNullOrEmpty(ews.fireEvent) || !LOAD_EVENT_NAME.Equals(ews.fireEvent, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                // Step-1: Get Cap Model
                CapModel4WS capModel = CapModels != null && CapModels.Count > 0 ? CapModels[ews.name] : null;

                // Step-2: Collect Expression Input Paramters
                ExpressionFieldModel[] inputParams = GetInputParamList(capModel, ews);

                // Step-3: Assemble Expression Input Paramters
                List<ExpressionFieldModel> expInParamList = AssembleExpressionInputParamters(capModel, inputParams);

                // Step-4: Get Expression Runtime Argument Model
                ExpressionRuntimeArgumentsModel expRuntimeArgModel = GetERArgumentsModel(capModel, ews.variableKey);
                var curArgs = ExpressionUtil.GetEPArgumentsModels(expRuntimeArgModel);

                ExpressionUtil.CombineArgumentsModule(collectArgumentsModule, curArgs);

                Dictionary<string, ExpressionFieldModel> currentInputParams = GetExpressionFieldListFromExpressionVariables(expInParamList);
                CollectionInputParams(currentInputParams, collectExpressionFieldsModule, curArgs);
            }
        }

        /// <summary>
        /// Get ASIT Expression Function Script(Only for ASIT)
        /// </summary>
        /// <param name="eventName">Event Name: onLoad, onSubmit, onPopulate</param>
        /// <returns>Return expression scripts.</returns>
        public string GetClientExpScript4ASIT(string eventName)
        {
            StringBuilder functionName = new StringBuilder();

            foreach (ExpressionFieldModel ews in _executeFieldList.Values)
            {
                if (ews != null && !string.IsNullOrEmpty(ews.fireEvent))
                {
                    if (eventName.Equals(ews.fireEvent, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Step-1: Get Cap Model
                        CapModel4WS capModel = CapModels != null && CapModels.Count > 0 ? CapModels[ews.name] : null;

                        // Step-2: Collect Expression Input Paramters
                        ExpressionFieldModel[] inputParams = GetInputParamList(capModel, ews);

                        // Step-3: Get Expression Runtime Argument Model
                        ExpressionRuntimeArgumentsModel expRuntimeArgModel = this.GetERArgumentsModel(capModel, ews.variableKey);

                        // Step-4: Assemble Expression Input Paramters
                        List<ExpressionFieldModel> expInParamList = this.AssembleExpressionInputParamters(capModel, inputParams);

                        // Step-5: Generate Expression Function Script for Client End
                        if (!SUBMIT_EVENT_NAME.Equals(eventName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            functionName.Append(this.AssembleExpressionRuntimeScript(expRuntimeArgModel, expInParamList.ToArray(), false));
                        }
                        else
                        {
                            functionName.Append(this.AssembleExpressionRuntimeScript(expRuntimeArgModel, expInParamList.ToArray(), true));
                        }

                        functionName.Append(";");
                    }
                }
            }

            return functionName.ToString();
        }

        /// <summary>
        /// Get Input parameters for specify execute field. 
        /// </summary>
        /// <param name="capModel">CapModel4WS object</param>
        /// <param name="exprssion">ExpressionFieldModel object</param>        
        /// <returns>expression's input parameters.</returns>
        public ExpressionFieldModel[] GetInputParamList(CapModel4WS capModel, ExpressionFieldModel exprssion)
        {
            ExpressionFieldModel[] expressionFields = GetExpressionParameterList(exprssion, capModel);

            if (expressionFields != null)
            {
                foreach (ExpressionFieldModel expressionModel in expressionFields)
                {
                    HandleExpress(expressionModel, capModel);
                }
            }

            return expressionFields;
        }

        /// <summary>
        /// Get "JavaScript" code to run expression.
        /// </summary>
        /// <param name="capModel">capModel instance</param>
        /// <param name="epm">execute field model</param>
        /// <param name="inputParams">expression field</param>
        /// <param name="isSubmit">current action is submit</param>
        /// <returns>"JavaScript" code that call expression to calculate result.</returns>
        public string GetClientExperssionScripts(CapModel4WS capModel, ExpressionFieldModel epm, ExpressionFieldModel[] inputParams, bool isSubmit)
        {
            if (epm == null || inputParams == null)
            {
                return string.Empty;
            }

            if (this._expressionType == ExpressionType.ASI_Table)
            {
                //jone del
                //throw new ACAException("This function is not supported for ASIT Expression");
            }

            // Step 1. Remove System Variables from input variables.
            List<ExpressionFieldModel> systemVariables = this.GetExpressionSystemVariables(inputParams);
            List<ExpressionFieldModel> inputVariables = this.GetExpressionInputVariables(inputParams);

            if (inputVariables == null || !inputVariables.Any())
            {
                return string.Empty;
            }

            // Update Expression Field Value for ASI Field
            foreach (ExpressionFieldModel inputVariable in inputVariables)
            {
                string key = inputVariable.servProvCode + "::" + inputVariable.variableKey;
                if (inputVariable.viewID == (int)ExpressionType.ASI && _appSpecficInfoFieldList.ContainsKey(key))
                {
                    AppSpecificInfoModel4WS asiField = _appSpecficInfoFieldList[key];
                    inputVariable.value = asiField.checklistComment;
                }
            }

            // Step 2. Update input parameters' based on page control list
            inputParams = inputVariables.ToArray();

            if (this._expressionType == ExpressionType.Fee_Item)
            {
                inputParams = ConvertToFeeInputParams(inputParams, epm);
            }
            else
            {
                foreach (ExpressionFieldModel inputParam in inputParams)
                {
                    if (_expressionControls.ContainsKey(inputParam.name))
                    {
                        inputParam.name = _expressionControls[inputParam.name].ClientID;
                    }
                }
            }

            // Step 3. Update some meta data about expression in session
            StoreSystemVariablesToSession(epm.servProvCode, systemVariables.ToArray());
            StoreInputVariablesToSession(inputParams);

            // Step 4. Get Expression Runtime Argument Model
            ExpressionRuntimeArgumentsModel argumentsModel = GetERArgumentsModel(capModel, epm.variableKey);

            // Step 5. Get Expression Input Expression Field List.
            List<string> fieldNameList = GetFieldNameListFromExpressionVariables(inputParams);

            // Step 6. Build script for expression
            string expressionScript = ExpressionUtil.BuildRunExpressionScripts(argumentsModel, fieldNameList, isSubmit);

            return expressionScript;
        }

        /// <summary>
        /// Assemble Expression Input Parameters(Only for ASIT)
        /// </summary>
        /// <param name="capModel">Cap model.</param>
        /// <param name="inputParams">Expression field model of original input parameters.</param>
        /// <returns>Expression field model of all input parameters.</returns>
        public List<ExpressionFieldModel> AssembleExpressionInputParamters(CapModel4WS capModel, ExpressionFieldModel[] inputParams)
        {
            if (inputParams == null)
            {
                throw new ArgumentException("The function arguments inputParams is null.");
            }

            //Step-2 Assemble ASIT Input Params
            List<ExpressionFieldModel> resultExpInParamList = new List<ExpressionFieldModel>();

            // Step 2.1 Sperate Input Parameters
            var inputParameters = inputParams.Where(e => e != null && !string.IsNullOrEmpty(e.variableKey) && e.viewID != int.Parse(EXPRESSION_SYS_FIELD_VIEWID));
            ExpressionFieldModel[] inputVariables = inputParameters.ToArray();

            // Step 2.2 Update ASI Input Paramters based on the latest value
            foreach (ExpressionFieldModel inputVariable in inputVariables)
            {
                string key = inputVariable.servProvCode + "::" + inputVariable.variableKey;

                if (inputVariable.viewID == (int)ExpressionType.ASI && _appSpecficInfoFieldList.ContainsKey(key))
                {
                    AppSpecificInfoModel4WS asiField = _appSpecficInfoFieldList[key];
                    inputVariable.value = asiField.checklistComment;
                }
            }
            
            //Step-1 Check Input Paramters
            /*
             * if expression type may have ASIT,and _uiTables exists             
             * then Assemble ASIT expression
             * */
            if (!ExpressionUtil.IsIncludeASITFields(this._expressionType) || this._uiTables == null || this._uiTables.Count() == 0)
            {
                return inputParams.ToList();
            }

            // Step 2.3 Assemble ASI Table Input Parameters based ASIT UITable Structure on current page
            resultExpInParamList = AssembleASITInputParams(capModel, inputVariables);

            //Step-3 Append Session Paramters
            var sessionParamters = inputParams.Where(e => e != null && !string.IsNullOrEmpty(e.variableKey) && e.viewID == int.Parse(EXPRESSION_SYS_FIELD_VIEWID));
            resultExpInParamList.AddRange(sessionParamters.ToArray());

            return resultExpInParamList;
        }

        /// <summary>
        /// Run Expression At Server
        /// </summary>
        /// <param name="argumentsModel">Expression Runtime Argument Model</param>
        /// <param name="inputParams">Expression input field</param>
        /// <returns>Expression Result Model</returns>
        public ExpressionRunResultModel4WS RunExpression(ExpressionRuntimeArgumentsModel argumentsModel, ExpressionFieldModel[] inputParams)
        {
            IExpressionBll expressionBll = ObjectFactory.GetObject<IExpressionBll>();
            return expressionBll.RunExpressions(argumentsModel, inputParams);
        }

        /// <summary>
        /// Initializes a new instance of the ExpressionFactory class.
        /// </summary>
        /// <param name="moduleName">Module name.</param>
        /// <param name="expressionType">Expression type.</param>
        /// <param name="expressionControls">Expression controls</param>
        /// <param name="argumentPkModels">the expression runtime argument PK model list for asit expression.</param>
        /// <param name="asitUiTables">ASIT UI tables.</param>
        /// <param name="subCapModels">Sub cap models for super cap.</param>
        /// <param name="asitUiDataKey">asit UI data key</param>
        private void Constructors4ExpressionFactory(string moduleName, ExpressionType expressionType, Dictionary<string, WebControl> expressionControls, List<ExpressionRuntimeArgumentPKModel> argumentPkModels, IEnumerable<ASITUITable> asitUiTables, IDictionary<string, CapModel4WS> subCapModels, string asitUiDataKey)
        {
            _capModel = AppSession.GetCapModelFromSession(moduleName);
            _expressionType = expressionType;
            _subCapModels = subCapModels;
            _expressionControls = expressionControls;
            _argumentPKModels = argumentPkModels;
            _uiTables = asitUiTables;
            _asitUiDataKey = asitUiDataKey;

            /*
             * Use ExpressionUtil.GetExpressionDataFromSession(asitUiDataKey) == null judge.
             * Reason : the asit/generic template table change event in pop up page is not allowed to reset the session value.            
             * With reference to the summary of "_asitUiDataKey" . 
             */
            if (!string.IsNullOrEmpty(asitUiDataKey) && ExpressionUtil.GetExpressionDataFromSession(asitUiDataKey) == null)
            {
                ExpressionSessionModel sessionModel = new ExpressionSessionModel();
                sessionModel.ExpressionArgumentPKModels = argumentPkModels;
                sessionModel.ExpressionFactoryType = expressionType;
                sessionModel.AsitUiDataKey = _asitUiDataKey;

                if (expressionControls != null)
                {
                    Dictionary<string, string> mapControlIDs = expressionControls.ToDictionary(expControl => expControl.Key, expControl => expControl.Value.ClientID);
                    sessionModel.ExpressionBaseControls = mapControlIDs;
                }

                ExpressionUtil.SetExpressionDataToSession(sessionModel, asitUiDataKey);
            }

            InitExecuteFields();
        }

        /// <summary>
        /// Assemble Expression Script for ASIT Table
        /// </summary>
        /// <param name="argumentsModel">Expression Runtime Argument Model</param>
        /// <param name="inputParams">Expression Input Parameters</param>
        /// <param name="isSubmit">For Submit Event or not</param>
        /// <returns>Client Script f</returns>
        private string AssembleExpressionRuntimeScript(ExpressionRuntimeArgumentsModel argumentsModel, ExpressionFieldModel[] inputParams, bool isSubmit)
        {
            // Step-1: Separate expression input paramter from session or ui
            var systemInputParamters = inputParams.Where(e => e != null && !string.IsNullOrEmpty(e.variableKey) && e.viewID == int.Parse(EXPRESSION_SYS_FIELD_VIEWID));
            var nonSystemInputParameters = inputParams.Where(e => e != null && !string.IsNullOrEmpty(e.variableKey) && e.viewID != int.Parse(EXPRESSION_SYS_FIELD_VIEWID));

            ExpressionFieldModel[] systemVariables = systemInputParamters.ToArray();
            ExpressionFieldModel[] inputVariables = nonSystemInputParameters.ToArray();

            SetControlID4TemplateTableExpression(inputVariables);

            // Step-2: Store template expression input paramter in session
            StoreSystemVariablesToSession(argumentsModel.servProvCode, systemVariables);
            StoreInputVariablesToSession(inputVariables);

            // Step-3: Get unique input paramter names
            List<string> fieldNameList = GetFieldNameListFromExpressionVariables(inputVariables);

            // STep-4: Assemble client script for expression
            string expressionScript = ExpressionUtil.BuildRunExpressionScripts(argumentsModel, fieldNameList, isSubmit);

            return expressionScript;
        }

        /// <summary>
        /// Set the control id for input variables for template table expression.
        /// Because the template table(ASIT) edit form is a pop up page,
        ///     and Expression of template table may be refer to Standard fields and template fields and people template fields.
        /// The method to set the correct control id for the referenced fields.
        /// </summary>
        /// <param name="inputVariables">input field</param>
        private void SetControlID4TemplateTableExpression(ExpressionFieldModel[] inputVariables)
        {
            if (string.IsNullOrEmpty(_asitUiDataKey) || _expressionType == ExpressionType.ASI_Table
                || _uiTables == null || !_uiTables.Any() || !inputVariables.Any()
                || !ExpressionUtil.IsIncludeASITFields(this._expressionType))
            {
                return;
            }

            ExpressionSessionModel expressionSession = ExpressionUtil.GetExpressionDataFromSession(_asitUiDataKey);

            if (expressionSession == null)
            {
                return;
            }

            Dictionary<string, string> mapControlIDs = expressionSession.ExpressionBaseControls;

            if (mapControlIDs == null || mapControlIDs.Count == 0)
            {
                return;
            }

            foreach (ExpressionFieldModel inputVariable in inputVariables)
            {
                if (mapControlIDs.ContainsKey(inputVariable.name))
                {
                    inputVariable.name = mapControlIDs[inputVariable.name];
                }
            }
        }

        /// <summary>
        /// Get Field Name List From Expression Variables
        /// </summary>
        /// <param name="expressionFields">Input Expression Variables</param>
        /// <returns>Field Name List</returns>
        private List<string> GetFieldNameListFromExpressionVariables(ExpressionFieldModel[] expressionFields)
        {
            List<string> fieldNameList = new List<string>();

            foreach (ExpressionFieldModel field in expressionFields)
            {
                string key = GetExpressionFieldKey(field);

                if (!string.IsNullOrEmpty(key) && !fieldNameList.Contains(key))
                {
                    fieldNameList.Add(key);
                }
            }

            return fieldNameList;
        }

        /// <summary>
        /// Collection Expression Input Variables from expression script(only for onLoad and onSubmit event)
        /// </summary>
        /// <param name="currentInputParams">current input parameters</param>
        /// <param name="collectExpressionFieldsModule">the expression input field array.<!--Dictionary<expressionArgument.key, Dictionary<Expression field key, see GetExpressionFieldKey, ExpressionFieldModel>>--></param>
        /// <param name="expressionArguments">the expression argument array.<!--Dictionary<(expressionArgument key,see ExpressionUtil.GetEPArgumentsModels)), ExpressionRuntimeArgumentsModel>--></param>
        private void CollectionInputParams(Dictionary<string, ExpressionFieldModel> currentInputParams, Dictionary<string, Dictionary<string, ExpressionFieldModel>> collectExpressionFieldsModule, Dictionary<string, ExpressionRuntimeArgumentsModel> expressionArguments)
        {
            if (collectExpressionFieldsModule == null || expressionArguments == null || expressionArguments.Count == 0)
            {
                return;
            }

            foreach (var item in expressionArguments)
            {
                if (!collectExpressionFieldsModule.ContainsKey(item.Key))
                {
                    collectExpressionFieldsModule.Add(item.Key, new Dictionary<string, ExpressionFieldModel>());
                }

                Dictionary<string, ExpressionFieldModel> collectExpressionFields = collectExpressionFieldsModule[item.Key];

                foreach (var expressionField in currentInputParams)
                {
                    if (collectExpressionFields.ContainsKey(expressionField.Key))
                    {
                        collectExpressionFields[expressionField.Key] = expressionField.Value;
                    }
                    else
                    {
                        collectExpressionFields.Add(expressionField.Key, expressionField.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Get expression field key value list from expression variables.
        /// </summary>
        /// <param name="expressionFields">Input Expression Variables</param>
        /// <returns>Key: expression field key; Value: expression field.</returns>
        private Dictionary<string, ExpressionFieldModel> GetExpressionFieldListFromExpressionVariables(List<ExpressionFieldModel> expressionFields)
        {
            var dictExpFields = new Dictionary<string, ExpressionFieldModel>();

            foreach (ExpressionFieldModel field in expressionFields)
            {
                string key = GetExpressionFieldKey(field);

                if (!string.IsNullOrEmpty(key) && !dictExpFields.ContainsKey(key))
                {
                    dictExpFields.Add(key, field);
                }
            }

            return dictExpFields;
        }

        /// <summary>
        /// Get expression field key.
        /// </summary>
        /// <param name="field">expression field</param>
        /// <returns>Expression field key.</returns>
        private string GetExpressionFieldKey(ExpressionFieldModel field)
        {
            string key = string.Empty;

            // CASE 1: ASIT expression variables(with index -1) value from UI.
            if (field.viewID != null
                && ASIT_TEMPLATE_ROW_INDEX.Equals(field.rowIndex)
                && ExpressionUtil.IsASITOrGenericTempalteTableView(field.viewID))
            {
                if (INSERT_FIRST_ROW.Equals(field.name))
                {
                    // Step 3.1 Insert ASIT. All fields' name is InsertFirstRow
                    key = field.name + ACAConstant.SPLIT_CHAR + field.variableKey;
                }
            }
            else
            {
                // CASE 2: FEE expression variables value not from UI
                if (field.viewID != null && field.viewID == (int)ExpressionType.Fee_Item && !"FEE::QUANTITY".Equals(field.variableKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Step 3.3 Fee expression input variables
                    key = field.name + ACAConstant.SPLIT_CHAR + field.value;
                }
                else
                {
                    key = field.name;
                }
            }

            return key;
        }

        /// <summary>
        /// Initializes AppSpecificInfo Field List from CAP Model
        /// </summary>
        private void InitAppSpecficInfoFieldList()
        {
            // Step 1: Check if ASI exists
            if (_capModel == null || _capModel.appSpecificInfoGroups == null)
            {
                return;
            }

            // Step 2: Init AppSpecficInfoFieldList
            // Expression VariableKey is unique
            string prefix = "{0}::ASI::{1}::{2}", servProvCode, variableKey, subGroupName, fieldName;

            _appSpecficInfoFieldList.Clear();

            foreach (AppSpecificInfoGroupModel4WS appSpecicInfoGroup in _capModel.appSpecificInfoGroups)
            {
                foreach (AppSpecificInfoModel4WS appSpecificInfo in appSpecicInfoGroup.fields)
                {
                    servProvCode = appSpecificInfo.serviceProviderCode;
                    subGroupName = appSpecificInfo.checkboxType;
                    fieldName = appSpecificInfo.fieldLabel;

                    variableKey = string.Format(prefix, servProvCode, subGroupName, fieldName);

                    if (!_appSpecficInfoFieldList.ContainsKey(variableKey))
                    {
                        _appSpecficInfoFieldList.Add(variableKey, appSpecificInfo);
                    }
                }
            }
        }

        /// <summary>
        /// Get Execute Fields.
        /// If for super agency,should return all the sub agency s' ASI execute fields.
        /// for ASI Table, only need the super agency's
        /// </summary>        
        private void InitExecuteFields()
        {
            InitAppSpecficInfoFieldList();

            if (_executeFieldList != null)
            {
                return;
            }

            _executeFieldList = new Dictionary<string, ExpressionFieldModel>();

            // if super agency, should get the execute fields by each sub agency.
            if (StandardChoiceUtil.IsSuperAgency() && SubCapModels != null && SubCapModels.Count > 0)
            {
                string key = _capModel.capID.toKey();

                if (!SubCapModels.ContainsKey(key))
                {
                    SubCapModels.Add(key, _capModel);
                }

                foreach (CapModel4WS subCap in SubCapModels.Values)
                {
                    CollectExecuteFieldAndCapModels(subCap);
                }
            }
            else
            {
                CollectExecuteFieldAndCapModels(_capModel);
            }
        }

        /// <summary>
        /// Collect execute fields into _executeFieldList and capModel into CapModels.
        /// </summary>
        /// <param name="subCap">a cap model.</param>
        private void CollectExecuteFieldAndCapModels(CapModel4WS subCap)
        {
            IExpressionBll expressionBll = ObjectFactory.GetObject<IExpressionBll>();
            ExpressionFieldModel[] subExecuteFields = expressionBll.GetExecuteFields(GetERArgumentsModel(subCap, string.Empty));

            if (subExecuteFields == null)
            {
                return;
            }

            UpdateASIExpName(subExecuteFields);

            foreach (ExpressionFieldModel expressionField in subExecuteFields)
            {
                expressionField.name = ExpressionUtil.GetFullControlFieldName(subCap, HttpUtility.UrlEncode(HttpUtility.UrlDecode(expressionField.name)));

                if (!ExpressionType.AuthAgent_Customer_Detail.Equals(this._expressionType)
                    && !ExpressionType.ReferenceContact.Equals(this._expressionType)
                    && !CapModels.ContainsKey(expressionField.name))
                {
                    CapModels.Add(expressionField.name, subCap);
                }

                /*
                 * Related bugs bug #49040 , #35900
                 * It is improper to use alone variableKey or name as a key for the following reasons.
                 *  1. Use variableKey alone. In super agency, it's exist the same variableKey, but they are in different agency.  e.g.(onload, onsubmit, ASI:ASISubgroupName:ASIField)
                 *  2. Use name alone. In the same agency , the asit name is "0 2". but they may be different field.
                 *  So use variableKey + name to ensure uniqueness.
                 */
                string excuteFieldKey = expressionField.variableKey + ACAConstant.SPLIT_CHAR + expressionField.name;

                if (!_executeFieldList.ContainsKey(excuteFieldKey))
                {
                    _executeFieldList.Add(excuteFieldKey, expressionField);
                }
            }
        }

        /// <summary>
        /// Get "ExpressionRuntimeArgumentsModel" model to prepare the PK argument for expression.
        /// </summary>
        /// <param name="capModel">Current Cap Model</param>
        /// <param name="executeFieldName">execute field name value</param>
        /// <returns>ExpressionRuntimeArgumentsModel model.</returns>
        private ExpressionRuntimeArgumentsModel GetERArgumentsModel(CapModel4WS capModel, string executeFieldName)
        {
            var expressionBll = ObjectFactory.GetObject<IExpressionBll>();

            if (ExpressionType.ASI_Table.Equals(_expressionType) && this._uiTables != null)
            {
                //Collect All ASI Table Name for specific CAP from the current page.
                var varTableNames = from uiTable in this._uiTables
                                    let asitUITable = uiTable as ASITUITable
                                    where asitUITable.CapID.Equals(capModel.capID)
                                    select asitUITable.TableName;

                if (varTableNames.Any())
                {
                    List<ExpressionRuntimeArgumentPKModel> asitArgPkModel = new List<ExpressionRuntimeArgumentPKModel>();

                    foreach (var viewkey1 in varTableNames)
                    {
                        var argumentPKModel = new ExpressionRuntimeArgumentPKModel();
                        argumentPKModel.portletID = (long?)_expressionType;
                        argumentPKModel.viewKey1 = viewkey1;
                        asitArgPkModel.Add(argumentPKModel);
                    }

                    _argumentPKModels = asitArgPkModel;
                }
            }

            return expressionBll.BuildERArgumentsModel(capModel, _expressionType, executeFieldName, _argumentPKModels);
        }

        /// <summary>
        /// Get sub-group name for ASI/ASIT.
        /// </summary>
        /// <param name="variableKey">variable key.It include sub-group name.</param>
        /// <param name="agencyCode">Agency code</param>
        /// <returns>sub-group name</returns>
        private string GetSubGroupName(string variableKey, string agencyCode)
        {
            variableKey = RemoveAgencyCaptypeText(variableKey, agencyCode);
            string subGroupName = variableKey.Substring(0, variableKey.LastIndexOf(ExpressionFactory.SPLIT_CHAR));
            subGroupName = variableKey.Substring(0, subGroupName.LastIndexOf(ExpressionFactory.SPLIT_CHAR));

            return subGroupName;
        }

        /// <summary>
        /// remove agency code and cap type text from "variableKey"
        /// </summary>
        /// <param name="variableKey">variable key name.It's format like "label name_agency name_CapType"</param>
        /// <param name="agencyCode">Agency code</param>
        /// <returns>It hasn't agency code and cap type.</returns>
        private string RemoveAgencyCaptypeText(string variableKey, string agencyCode)
        {
            string returnResult = variableKey;

            if (variableKey.IndexOf(ACAConstant.SPLIT_CHAR + agencyCode) > 0)
            {
                returnResult = variableKey.Substring(0, variableKey.IndexOf(ACAConstant.SPLIT_CHAR + agencyCode));
            }

            return returnResult;
        }

        /// <summary>
        /// Combine the variable key and store them into session.
        /// </summary>
        /// <param name="expressionFields">Expression input variable</param>
        private void StoreInputVariablesToSession(ExpressionFieldModel[] expressionFields)
        {
            Dictionary<string, ExpressionFieldModel> inputVariables =
                HttpContext.Current.Session[ACAConstant.Expression_InputVariables] as Dictionary<string, ExpressionFieldModel>;

            if (inputVariables == null)
            {
                inputVariables = new Dictionary<string, ExpressionFieldModel>();
            }

            foreach (ExpressionFieldModel field in expressionFields)
            {
                if (field.viewID != null && ExpressionUtil.IsASITOrGenericTempalteTableView(field.viewID)
                    && ASIT_TEMPLATE_ROW_INDEX.Equals(field.rowIndex))
                {
                    // Case 1: ASIT Expression variables(with index -1) value NOT from UI.

                    //Replace field's name with "InsertFirstRow" for ASIT template fields.
                    if (!INSERT_FIRST_ROW.Equals(field.name))
                    {
                        field.name = INSERT_FIRST_ROW;
                    }

                    //Build the key and add to varibles dictionary.
                    string key = field.name + ACAConstant.SPLIT_CHAR + field.variableKey;

                    if (!inputVariables.ContainsKey(key))
                    {
                        inputVariables.Add(key, field);
                    }
                }
                else if (field.viewID != null && field.viewID == (int)ExpressionType.Fee_Item
                        && !"FEE::QUANTITY".Equals(field.variableKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Case 2: Fee Expression variables value NOT from UI

                    //Build the key and add to varibles dictionary.
                    string key = field.name + ACAConstant.SPLIT_CHAR + field.value;

                    if (!inputVariables.ContainsKey(key))
                    {
                        inputVariables.Add(key, field);
                    }
                }
                else
                {
                    // Other normal Expression input variables from UI.
                    if (!inputVariables.ContainsKey(field.name))
                    {
                        inputVariables.Add(field.name, field);
                    }
                    else
                    {
                        /*
                        * In 7.3.1, Contact edit form of all contact section use the same control ID since they use the same page to Edit contact in a popup dialog.
                        * So need to update the ExpressionField model since the same control mapping to different variable key in different expression portlet.
                        * Such as:
                        * In Applicant component, the variable key of the First Name field is "APPLICANT::applicant*firstName".
                        * But in Contact2 component, the vailable key "CONTACT2::contactsModel1*firstName".
                        */
                        inputVariables[field.name] = field;
                    }
                }
            }

            HttpContext.Current.Session[ACAConstant.Expression_InputVariables] = inputVariables;
        }

        /// <summary>
        /// Store Expression System Variables in session
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="systemVariables">Expression System Variables</param>
        private void StoreSystemVariablesToSession(string servProvCode, ExpressionFieldModel[] systemVariables)
        {
            // Step 1. Get All Expression System Variables in Session
            Dictionary<string, Dictionary<string, ExpressionFieldModel>> expressionSystemVariables = HttpContext.Current.Session[ACAConstant.Expression_SystemVariables] as Dictionary<string, Dictionary<string, ExpressionFieldModel>>;

            if (expressionSystemVariables == null)
            {
                expressionSystemVariables = new Dictionary<string, Dictionary<string, ExpressionFieldModel>>();
            }

            // Step 2. Get Expression System Variables by Agency Code
            Dictionary<string, ExpressionFieldModel> tmpSystemVariables = new Dictionary<string, ExpressionFieldModel>();

            if (!expressionSystemVariables.ContainsKey(servProvCode))
            {
                expressionSystemVariables.Add(servProvCode, tmpSystemVariables);
            }

            tmpSystemVariables = expressionSystemVariables[servProvCode];

            // Step 3. Store Expression System Variables
            string key = string.Empty;
            foreach (ExpressionFieldModel variable in systemVariables)
            {
                key = variable.name;

                if (!tmpSystemVariables.ContainsKey(key))
                {
                    tmpSystemVariables.Add(key, variable);
                }
            }

            // Step 4. Store Expression System Variables with Agency Code
            expressionSystemVariables[servProvCode] = tmpSystemVariables;

            // Step 5. Store Expression System Variables into Session
            HttpContext.Current.Session[ACAConstant.Expression_SystemVariables] = expressionSystemVariables;
        }

        /// <summary>
        /// Clone expression field
        /// </summary>
        /// <param name="expressionParams">original expression field</param>
        /// <returns>cloned expression field</returns>
        private ExpressionFieldModel[] CloneExpressionParam(ExpressionFieldModel[] expressionParams)
        {
            ExpressionFieldModel[] clone = null;

            if (expressionParams != null &&
                expressionParams.Length > 0)
            {
                clone = new ExpressionFieldModel[expressionParams.Length];
                for (int i = 0; i < expressionParams.Length; i++)
                {
                    clone[i] = CloneExpressionProperties(expressionParams[i]);
                }
            }

            return clone;
        }

        /// <summary>
        /// Get the expression properties for template
        /// </summary>
        /// <param name="originalInputParams">The original input parameters</param>
        /// <param name="targetExpression">The target expression field</param>
        /// <returns>Expression field model</returns>
        private ExpressionFieldModel CloneInitialExpressionProperties(ExpressionFieldModel[] originalInputParams, ExpressionFieldModel targetExpression)
        {
            ExpressionFieldModel clone = null;

            if (originalInputParams != null && originalInputParams.Length > 0 && targetExpression != null)
            {
                foreach (ExpressionFieldModel orginal in originalInputParams)
                {
                    if (orginal.variableKey.Equals(targetExpression.variableKey, StringComparison.InvariantCulture))
                    {
                        clone = CloneExpressionProperties(orginal, targetExpression.name);
                        break;
                    }
                }
            }

            return clone;
        }

        /// <summary>
        /// Close expression properties
        /// </summary>
        /// <param name="orginal">The original input parameter field</param>
        /// <returns>Expression field model</returns>
        private ExpressionFieldModel CloneExpressionProperties(ExpressionFieldModel orginal)
        {
            return CloneExpressionProperties(orginal, string.Empty);
        }

        /// <summary>
        /// Clone expression properties
        /// </summary>
        /// <param name="orginal">original expression properties</param>
        /// <param name="targetControlName">target Control Name</param>
        /// <returns>cloned expression properties</returns>
        private ExpressionFieldModel CloneExpressionProperties(ExpressionFieldModel orginal, string targetControlName)
        {
            ExpressionFieldModel expressionParam = ObjectCloneUtil.DeepCopy(orginal);

            if (!string.IsNullOrEmpty(targetControlName) && ExpressionUtil.IsASITOrGenericTempalteTableView(orginal.viewID))
            {
                //the template of asi table need to maintian the target name
                expressionParam.name = targetControlName;
            }
            else
            {
                expressionParam.name = orginal.name;
            }

            return expressionParam;
        }

        /// <summary>
        /// Get expression parameters
        /// </summary>
        /// <param name="executeField">ExpressionFieldModel object instance of execute field.</param>
        /// <param name="capModel">CapModel4WS object</param>
        /// <returns>ExpressionDTOModel4WS object</returns>
        private ExpressionFieldModel[] GetExpressionParameterList(ExpressionFieldModel executeField, CapModel4WS capModel)
        {
            if (executeField == null)
            {
                return null;
            }

            if (!_enabledInputParams.ContainsKey(executeField.name))
            {
                IExpressionBll iEP = ObjectFactory.GetObject(typeof(IExpressionBll)) as IExpressionBll;

                ExpressionRuntimeArgumentsModel expRuntimeArgumentModel = GetERArgumentsModel(capModel, executeField.variableKey);

                // In order to improve the performance of initializing expression buider for onLoad and onSubmit, the expression runtime 
                // argument model should contains application information fields so that expression builder engin does not need retrieve application information fields from database.
                //if (LOAD_EVENT_NAME.Equals(executeField.fireEvent, StringComparison.InvariantCultureIgnoreCase)
                //   || SUBMIT_EVENT_NAME.Equals(executeField.fireEvent, StringComparison.InvariantCultureIgnoreCase))
                if (capModel != null && capModel.appSpecificInfoGroups != null)
                {
                    List<AppSpecificInfoModel> appSpecifiInfoList = new List<AppSpecificInfoModel>();

                    foreach (AppSpecificInfoGroupModel4WS appSpecificInfoGroup in capModel.appSpecificInfoGroups)
                    {
                        if (appSpecificInfoGroup.fields == null)
                        {
                            continue;
                        }
                        
                        foreach (AppSpecificInfoModel4WS appSpecificInfo in appSpecificInfoGroup.fields)
                        {
                            // Convert ASIModel4WS to ASIModel
                            appSpecifiInfoList.Add(TempModelConvert.ConvertToAppSpecificInfoModel(appSpecificInfo));
                        }
                    }

                    expRuntimeArgumentModel.appSpecificInfoModelList = appSpecifiInfoList.ToArray();
                }

                ExpressionFieldModel[] expInputParams = iEP.GetInputParameters(expRuntimeArgumentModel);
                UpdateASIExpName(expInputParams);

                _enabledInputParams.Add(executeField.name, expInputParams);
            }

            return CloneExpressionParam(_enabledInputParams[executeField.name]);
        }

        /// <summary>
        /// update asi expression field name
        /// </summary>
        /// <param name="expInputParams">the expression input parameter</param>
        private void UpdateASIExpName(ExpressionFieldModel[] expInputParams)
        {
            if (expInputParams == null)
            {
                return;
            }

            /*
             * The generic template field name is start with "app_spec_info_" in DB.
             * The ASI field name is also start with "app_spec_info_" in DB.
             * So for generic template expression, there need to change original ASI field name to specific field name.
             * change 'app_spec_info_' to ExoressionType.
             * */
            foreach (ExpressionFieldModel field in expInputParams)
            {
                if (field == null || field.viewID == null)
                {
                    continue;
                }

                ExpressionType fieldExpressionType = (ExpressionType)field.viewID;

                if (ExpressionUtil.ExpressionTypeModels.Exists(w => w.TPL_FormViewID == fieldExpressionType))
                {
                    var regex = new Regex(ACAConstant.EXP_ASI_FIELD_NAME_PREFIX);
                    field.name = regex.Replace(field.name, _expressionType.ToString(), 1);
                }
            }
        }

        /// <summary>
        /// Handle expression
        /// </summary>
        /// <param name="expression">ExpressionFieldModel object</param>
        /// <param name="capModel">CapModel4WS object</param>
        private void HandleExpress(ExpressionFieldModel expression, CapModel4WS capModel)
        {
            if (ExpressionUtil.IsASITOrGenericTempalteTableView(expression.viewID))
            {
                string[] temp = expression.name.Split(' ');

                if (temp.Length > 1)
                {
                    int columnIndex = -1;

                    if (!ExpressionUtil.IsGenericTempalteTableView(expression.viewID) && int.TryParse(temp[0], out columnIndex))
                    {
                        expression.columnIndex = columnIndex;
                    }
                }
            }

            SetInputParamValueForExpression(expression, capModel);
            expression.name = HttpUtility.UrlEncode(HttpUtility.UrlDecode(expression.name));

            // append the sub agency code and cap type to match the expression name to the control name. (Does not support Record detail fields)
            if (expression.viewID != int.Parse(EXPRESSION_SYS_FIELD_VIEWID) && expression.viewID != int.Parse(EXPRESSION_RECORD_DETAIL_FIELD_VIEWID))
            {
                if (ExpressionUtil.IsASITOrGenericTempalteTableView(expression.viewID))
                {
                    // Get ASIT Sub Group Name from Variable Key Data
                    // Also: The Format of the Expression Variable Key: ASIT::[ASIT Table Name]::[Field Label]
                    // Contact Template table Format: CONTACT(1|2|3)TPLTABLE|APPLICANTTPLTABLE|REFCONTACTTPLTABLE::[ASIT Group]::[ASIT Table Name]::[Field Label]
                    string asitSubGroupName = GetASITableNameByVariableKey(expression.variableKey);

                    if (!string.IsNullOrEmpty(asitSubGroupName))
                    {
                        asitSubGroupName = asitSubGroupName + ACAConstant.SPLIT_CHAR5;
                    }

                    expression.name = ExpressionUtil.GetFullControlFieldName(capModel, asitSubGroupName + expression.name);
                }
                else
                {
                    string expressionName = expression.name;

                    if ("FORM".Equals(expressionName, StringComparison.InvariantCultureIgnoreCase)
                        && Regex.IsMatch(expression.variableKey, @"::FORM$"))
                    {
                        expressionName = expression.variableKey;
                    }

                    expression.name = ExpressionUtil.GetFullControlFieldName(capModel, expressionName);
                }
            }
        }

        /// <summary>
        /// Fill some data s into some fields of input field for expression
        /// </summary>
        /// <param name="fws">input field for expression</param>
        /// <param name="capModel">current cap model</param>
        private void SetInputParamValueForExpression(ExpressionFieldModel fws, CapModel4WS capModel)
        {
            switch (fws.name)
            {
                case ExpressionSysInputParams.CapID1:
                    fws.value = (capModel != null && capModel.capID != null) ? _capModel.capID.id1 : null;
                    break;
                case ExpressionSysInputParams.CapID2:
                    fws.value = (capModel != null && capModel.capID != null) ? _capModel.capID.id2 : null;
                    break;
                case ExpressionSysInputParams.CapID3:
                    fws.value = (capModel != null && capModel.capID != null) ? _capModel.capID.id3 : null;
                    break;
                case ExpressionSysInputParams.ServProvCode:
                    fws.value = (capModel != null && capModel.capType != null) ? capModel.capType.serviceProviderCode : ConfigManager.AgencyCode;
                    break;
                case ExpressionSysInputParams.UserID:
                    fws.value = AppSession.User.UserID;
                    break;
                case ExpressionSysInputParams.Publicuser_email:
                    fws.value = AppSession.User.Email;
                    break;
                case ExpressionSysInputParams.FirstName:
                    fws.value = AppSession.User.FirstName;
                    break;
                case ExpressionSysInputParams.MiddleName:
                    fws.value = AppSession.User.MiddleName;
                    break;
                case ExpressionSysInputParams.LastName:
                    fws.value = AppSession.User.LastName;
                    break;
                case ExpressionSysInputParams.Userfullname:
                    fws.value = AppSession.User.FullName;
                    break;
                case ExpressionSysInputParams.Today:
                    fws.value = I18nDateTimeUtil.FormatToDateStringForUI(DateTime.Today);
                    break;
                case ExpressionSysInputParams.Module:
                    fws.value = capModel != null ? capModel.moduleName : string.Empty;
                    break;
            }
        }

        /// <summary>
        /// Assemble ASIT Expression's Input Parameter based on UI Structure
        /// </summary>
        /// <param name="capModel">Cap model.</param>
        /// <param name="originalInputParams">Input Parameters(for asit and template table)</param>
        /// <returns>Input Parameters for ASIT Expression</returns>
        private Dictionary<string, List<List<ExpressionFieldModel>>> AssembleASITInputParamterBasedOnUITables(CapModel4WS capModel, ExpressionFieldModel[] originalInputParams)
        {
            Dictionary<string, List<List<ExpressionFieldModel>>> finalASITExpInputParams = new Dictionary<string, List<List<ExpressionFieldModel>>>();

            //Step 1 - Get All Available ASIT Fields
            Dictionary<string, ExpressionFieldModel> templateInputParameters = new Dictionary<string, ExpressionFieldModel>();

            List<ExpressionFieldModel> firstRowExpFields = new List<ExpressionFieldModel>();
            List<string> tables = new List<string>();
            string tableName = string.Empty;

            foreach (ExpressionFieldModel param in originalInputParams)
            {
                if (param == null || string.IsNullOrEmpty(param.variableKey))
                {
                    continue;
                }

                // Step - 1.1
                ExpressionFieldModel cloneParam = CloneExpressionProperties(param, INSERT_FIRST_ROW);
                cloneParam.name = INSERT_FIRST_ROW;

                // Step - 1.2
                param.rowIndex = 0;
                templateInputParameters.Add(param.name, param);
                tableName = GetASITableNameByVariableKey(param.variableKey);

                if (!tables.Contains(tableName))
                {
                    tables.Add(tableName);
                }

                // Step - 1.3
                firstRowExpFields.Add(cloneParam);
            }

            //Step 2 - Collect Non ASIT Expression Input Field
            bool isTableExist = _uiTables == null ? false : (from uiTable in _uiTables
                                                             let asitUITable = uiTable as ASITUITable
                                                             where asitUITable != null &&
                                                             tables.Contains(asitUITable.TableName)
                                                             select uiTable).Count() > 0;

            if (isTableExist)
            {
                Dictionary<string, List<ExpressionFieldModel>> tableGroup = new Dictionary<string, List<ExpressionFieldModel>>();

                foreach (ExpressionFieldModel expField in firstRowExpFields)
                {
                    tableName = GetASITableNameByVariableKey(expField.variableKey);

                    if (!tableGroup.ContainsKey(tableName))
                    {
                        tableGroup.Add(tableName, new List<ExpressionFieldModel>());
                    }

                    tableGroup[tableName].Add(expField);
                }

                foreach (string tmpTableName in tableGroup.Keys)
                {
                    if (tableGroup[tmpTableName].Count <= 0)
                    {
                        continue;
                    }

                    if (!finalASITExpInputParams.ContainsKey(tmpTableName))
                    {
                        finalASITExpInputParams.Add(tmpTableName, new List<List<ExpressionFieldModel>>());
                    }

                    finalASITExpInputParams[tmpTableName].Add(tableGroup[tmpTableName]);
                }
            }

            //Step 3 - Assemble Expression Input Paramter for each ASI Table
            int fieldCount = 0;

            if (_uiTables != null)
            {
                foreach (ASITUITable asitUITable in this._uiTables)
                {
                    if (asitUITable == null
                        || (asitUITable.CapID != null && !asitUITable.CapID.Equals(capModel.capID))
                        || asitUITable.Rows == null
                        || asitUITable.Rows.Count <= 0)
                    {
                        continue;
                    }

                    tableName = asitUITable.TableName;

                    int rowIndex = 0;

                    foreach (UIRow row in asitUITable.Rows)
                    {
                        if (row == null
                            || row.Fields == null
                            || row.Fields.Count <= 0)
                        {
                            continue;
                        }

                        fieldCount = row.Fields.Count;
                        List<ExpressionFieldModel> tableFields = new List<ExpressionFieldModel>();

                        int columnIndex = 0;
                        foreach (UIField field in row.Fields)
                        {
                            string fieldName = columnIndex + " " + fieldCount;

                            if (originalInputParams.Length > 0 && ExpressionUtil.IsGenericTempalteTableView(originalInputParams[0].viewID))
                            {
                                fieldName = field.Name;
                            }

                            fieldName = HttpUtility.UrlEncode(fieldName);
                            string expControlKey = tableName + "_" + fieldName;
                            expControlKey = ExpressionUtil.GetFullControlFieldName(capModel, expControlKey);

                            if (templateInputParameters.ContainsKey(expControlKey)
                                && templateInputParameters[expControlKey].variableKey.Contains(tableName))
                            {
                                ExpressionFieldModel expInputField = GetExpressioField(templateInputParameters[expControlKey] as ExpressionFieldModel, field);
                                expInputField.rowIndex = rowIndex;
                                tableFields.Add(expInputField);
                            }

                            columnIndex = columnIndex + 1;
                        }

                        if (tableFields.Count > 0)
                        {
                            if (!finalASITExpInputParams.ContainsKey(tableName))
                            {
                                finalASITExpInputParams.Add(tableName, new List<List<ExpressionFieldModel>>());
                            }

                            finalASITExpInputParams[tableName].Add(tableFields);
                        }

                        rowIndex = rowIndex + 1;
                    }
                }
            }

            return finalASITExpInputParams;
        }

        /// <summary>
        /// Get the ASI Table name from variable key.
        /// </summary>
        /// <param name="variableKey">Variable key.</param>
        /// <returns>ASI Table name.</returns>
        private string GetASITableNameByVariableKey(string variableKey)
        {
            if (variableKey == null)
            {
                return string.Empty;
            }

            string subGroupName = string.Empty;
            string[] arr = Regex.Split(variableKey, ACAConstant.SPLIT_DOUBLE_COLON);

            if (arr != null && arr.Length > 1)
            {
                subGroupName = arr[arr.Length - 2];
            }

            return subGroupName;
        }

        /// <summary>
        /// Split Expression System Variables
        /// </summary>
        /// <param name="inputParams">All Expression Variables</param>
        /// <returns>System Variables</returns>
        private List<ExpressionFieldModel> GetExpressionSystemVariables(ExpressionFieldModel[] inputParams)
        {
            List<ExpressionFieldModel> systemVariables = new List<ExpressionFieldModel>();

            foreach (ExpressionFieldModel paramter in inputParams)
            {
                if (paramter != null
                    && !string.IsNullOrEmpty(paramter.variableKey)
                    && paramter.viewID == int.Parse(EXPRESSION_SYS_FIELD_VIEWID))
                {
                    systemVariables.Add(paramter);
                }
            }

            return systemVariables;
        }

        /// <summary>
        /// Split Expression Input Variables
        /// </summary>
        /// <param name="inputParams">All Expression Variables</param>
        /// <returns>System Variables</returns>
        private List<ExpressionFieldModel> GetExpressionInputVariables(ExpressionFieldModel[] inputParams)
        {
            List<ExpressionFieldModel> expressionVariables = new List<ExpressionFieldModel>();

            foreach (ExpressionFieldModel paramter in inputParams)
            {
                if (paramter != null
                    && !string.IsNullOrEmpty(paramter.variableKey)
                    && paramter.viewID == int.Parse(EXPRESSION_SYS_FIELD_VIEWID))
                {
                    continue;
                }

                expressionVariables.Add(paramter);
            }

            return expressionVariables;
        }

        /// <summary>
        /// Assemble Input Parameters For ASIT Expression
        /// </summary>
        /// <param name="capModel">Cap Model.</param>
        /// <param name="originalInputParams">Input Parameters</param>
        /// <returns>Assembled Input Parameters</returns>
        private List<ExpressionFieldModel> AssembleASITInputParams(CapModel4WS capModel, ExpressionFieldModel[] originalInputParams)
        {
            List<ExpressionFieldModel> inputParams = new List<ExpressionFieldModel>();

            //Step-1 ASI Input Paramters
            // Step-1.1 Seperate ASI Input Parameters
            var nonAsitInputParamters = originalInputParams.Where(e => e != null && !ExpressionUtil.IsASITOrGenericTempalteTableView(e.viewID));

            // Step-1.2 Assemble template ASI input parameters
            ExpressionFieldModel[] nonAsitInputParamterArray = nonAsitInputParamters.ToArray();
            foreach (ExpressionFieldModel expField in nonAsitInputParamterArray)
            {
                expField.rowIndex = null;
                inputParams.Add(expField);
            }

            //ASIT Input Paramters
            // Step 2.1 Seperate ASIT Input Parameters
            var asitInputParamters = originalInputParams.Where(e => e != null && ExpressionUtil.IsASITOrGenericTempalteTableView(e.viewID));
            ExpressionFieldModel[] asitInputParamterArray = asitInputParamters.ToArray();

            // Step 2.2 Assemble ASIT Input Parameters based ASIT UITable Structure
            Dictionary<string, List<List<ExpressionFieldModel>>> asitInputParameters = AssembleASITInputParamterBasedOnUITables(capModel, asitInputParamterArray);

            // Step 2.3 Assemble template ASIT input paramters
            foreach (string tableName in asitInputParameters.Keys)
            {
                List<List<ExpressionFieldModel>> rows = asitInputParameters[tableName];

                List<ExpressionFieldModel> templateExpressionModel = rows[0];
                for (int k = 0; k < templateExpressionModel.Count; k++)
                {
                    ExpressionFieldModel templateField = CloneInitialExpressionProperties(originalInputParams, templateExpressionModel[k]);
                    templateField.rowIndex = ASIT_TEMPLATE_ROW_INDEX;
                    inputParams.Add(templateField);
                }

                foreach (List<ExpressionFieldModel> row in rows)
                {
                    for (int i = 0; i < row.Count; i++)
                    {
                        if (row[i].rowIndex != ASIT_TEMPLATE_ROW_INDEX)
                        {
                            inputParams.Add(row[i]);
                        }
                    }
                }
            }

            return inputParams;
        }

        /// <summary>
        /// convert old Fee items input parameters to deal with new parameter format.
        /// </summary>
        /// <param name="originalInputParams">original input parameters</param>
        /// <param name="expressionField">"expression field" includes service code and full key name</param>
        /// <returns>new format parameters</returns>
        private ExpressionFieldModel[] ConvertToFeeInputParams(ExpressionFieldModel[] originalInputParams, ExpressionFieldModel expressionField)
        {
            if (expressionField == null)
            {
                return null;
            }

            List<ExpressionFieldModel> inputParams = new List<ExpressionFieldModel>();

            if (originalInputParams == null || originalInputParams.Length == 0 || _expressionControls.Count == 0)
            {
                return inputParams.ToArray();
            }

            int rowIndex = 0;
            int inputParamsIndex = 0;
            int rowCount = _feeControlsCount;
            List<WebControl> expressionControls = new List<WebControl>();

            foreach (string keyName in _expressionControls.Keys)
            {
                int startIndex = expressionField.name.IndexOf(expressionField.servProvCode);
                string captype = expressionField.name.Length > startIndex ? expressionField.name.Substring(startIndex) : string.Empty;

                //"keyName" value is "Quantity+SPLIT_CHAR+rowindex+SPLIT_CHAR+angency+SPLIT_CHAR+captype";
                //filter can't match agency and ASI of control.
                if (keyName.IndexOf(captype) == -1)
                {
                    rowCount--;
                }
                else
                {
                    expressionControls.Add(_expressionControls[keyName]);
                }
            }

            for (int i = 0; i < originalInputParams.Length; i++)
            {
                if (originalInputParams[i] == null)
                {
                    continue;
                }

                if (originalInputParams[i].viewID.Equals((long)ExpressionType.Fee_Item))
                {
                    rowIndex = 0;

                    for (int j = 0; j < rowCount; j++)
                    {
                        ExpressionFieldModel finalEWS = CloneExpressionProperties(originalInputParams[i]);

                        if (j < expressionControls.Count && finalEWS.name != null
                            && finalEWS.name.IndexOf(EXPRESSION_QUANTITY_LABEL) == 0)
                        {
                            finalEWS.name = expressionControls[j].ClientID;
                        }

                        finalEWS.rowIndex = rowIndex;
                        inputParams.Add(finalEWS);
                        inputParamsIndex++;
                        rowIndex++;

                        if (finalEWS.name != null
                            && finalEWS.name.IndexOf(EXPRESSION_QUANTITY_LABEL) != 0)
                        {
                            SetValueToInputParamsForFeeItem(finalEWS, expressionField.name);
                        }
                    }
                }
                else
                {
                    inputParams.Add(originalInputParams[i]);
                }
            }

            return inputParams.ToArray();
        }

        /// <summary>
        /// Sets value of "FeeItem"'s property to input parameters.
        /// </summary>
        /// <param name="inputParam">input parameter of expression.</param>
        /// <param name="key">key name of execute property.</param>
        private void SetValueToInputParamsForFeeItem(ExpressionFieldModel inputParam, string key)
        {
            int startIndex = key.IndexOf(ACAConstant.SPLIT_CHAR);

            //if star index = -1 , key.Substring(starIndex) cause yellow page error.
            if (startIndex > -1)
            {
                string finallyKey = key.Substring(startIndex);
                int updateIndex = inputParam.rowIndex.HasValue ? (int)inputParam.rowIndex : -1;

                //Key of "FeeItem"'s format is "SPLIT_CHAR+agency+SPLIT_CHAR+captype";
                if (FeeItems.ContainsKey(finallyKey) && FeeItems[finallyKey] != null &&
                    updateIndex < FeeItems[finallyKey].Count && updateIndex != -1)
                {
                    F4FeeItemModel4WS feeItem = FeeItems[finallyKey][updateIndex];
                    inputParam.value = GetPropertyValue(feeItem, inputParam.variableKey);
                }
            }
        }

        /// <summary>
        /// Gets specify value of property of F4FeeItemModel4WS object instance.
        /// </summary>
        /// <param name="feeItem">a F4FeeItemModel4WS object instance</param>
        /// <param name="propertyName">property name</param>
        /// <returns>value of  specify property</returns>
        private string GetPropertyValue(F4FeeItemModel4WS feeItem, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return string.Empty;
            }

            Hashtable propertyNames = GetKeyValuesForFeeItem();
            string finallyPropertyName = string.Empty;

            if (propertyNames[propertyName] != null)
            {
                finallyPropertyName = propertyNames[propertyName].ToString();
            }

            string propertyValue = string.Empty;
            Type valueType = feeItem.GetType();
            PropertyInfo[] propertyValues = valueType.GetProperties();

            for (int i = 0; i < propertyValues.Length; i++)
            {
                string name = propertyValues[i].Name;

                if (name.Equals(finallyPropertyName))
                {
                    object value = propertyValues[i].GetValue(feeItem, null);
                    propertyValue = value == null ? string.Empty : value.ToString();
                    break;
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get expression fields
        /// </summary>
        /// <param name="expressionFieldModel">ExpressionFieldModel object</param>
        /// <param name="field">field control</param>
        /// <returns>Expression Field</returns>
        private ExpressionFieldModel GetExpressioField(ExpressionFieldModel expressionFieldModel, UIField field)
        {
            ExpressionFieldModel finalExpressionFieldModel = new ExpressionFieldModel();
            finalExpressionFieldModel.name = field.FieldID;
            finalExpressionFieldModel.expressionName = expressionFieldModel.expressionName;
            finalExpressionFieldModel.value = field.Value;
            finalExpressionFieldModel.servProvCode = expressionFieldModel.servProvCode;
            finalExpressionFieldModel.variableKey = expressionFieldModel.variableKey;
            finalExpressionFieldModel.viewID = expressionFieldModel.viewID;
            finalExpressionFieldModel.dbRequired = expressionFieldModel.dbRequired;
            finalExpressionFieldModel.required = expressionFieldModel.required;
            finalExpressionFieldModel.message = expressionFieldModel.message;
            finalExpressionFieldModel.readOnly = expressionFieldModel.readOnly;
            finalExpressionFieldModel.columnIndex = expressionFieldModel.columnIndex;
            finalExpressionFieldModel.type = expressionFieldModel.type;
            finalExpressionFieldModel.variableKey = expressionFieldModel.variableKey;
            finalExpressionFieldModel.hidden = expressionFieldModel.hidden;

            return finalExpressionFieldModel;
        }

        /// <summary>
        /// Encode expression control key
        /// </summary>
        /// <returns>the dictionary with the key encoded</returns>
        private Dictionary<string, WebControl> EncodeExpressionControlKey()
        {
            Dictionary<string, WebControl> controls = new Dictionary<string, WebControl>();

            foreach (KeyValuePair<string, WebControl> isc in _expressionControls)
            {
                string key = HttpUtility.UrlEncode(HttpUtility.UrlDecode(isc.Key));
                controls.Add(key, isc.Value);
            }

            return controls;
        }

        /// <summary>
        /// generate key value collections for fee item property names.
        /// </summary>
        /// <returns>Key and value collection.</returns>
        private Hashtable GetKeyValuesForFeeItem()
        {
            Hashtable propertyNames = new Hashtable();
            propertyNames.Add("FEE::Period", "paymentPeriod");
            propertyNames.Add("FEE::Subgroup", "subGroup");
            propertyNames.Add("FEE::feeVsn", "version");
            propertyNames.Add("FEE::FeeItem", "resFeeDescription");
            propertyNames.Add("FEE::Notes", "feeNotes");
            propertyNames.Add("FEE::FeeCode", "feeCod");
            propertyNames.Add("FEE::Quantity", "feeUnit");
            propertyNames.Add("FEE::FeeSched", "feeSchudle");
            propertyNames.Add("FEE::Priority", string.Empty);
            propertyNames.Add("FEE::Unit", "feeUnit");

            return propertyNames;
        }

        /// <summary>
        /// Merge multiple RunExpression segment into one RunExpression if any.
        /// </summary>
        /// <param name="expressionFieldsScript">this expression script</param>
        /// <returns>merger script</returns>
        private string MergeExpressionScripts(string expressionFieldsScript)
        {
            string result = expressionFieldsScript;

            Regex expReg = new Regex(ExpressionUtil.RUN_EXPRESSION_PATTERN);

            if (!expReg.IsMatch(expressionFieldsScript))
            {
                return result;
            }

            var expMatchs = expReg.Matches(expressionFieldsScript);

            if (expMatchs.Count == 1)
            {
                return result;
            }

            foreach (Match expMatch in expMatchs)
            {
                result = result.Replace(expMatch.Value + ACAConstant.SPLIT_CHAR_SEMICOLON, string.Empty);
            }

            Dictionary<string, List<string>> inputParams = new Dictionary<string, List<string>>();
            Dictionary<string, ExpressionRuntimeArgumentsModel> asitExpArgs = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
            ExpressionUtil.CombineArgumentsAndExpressionFieldsModule(asitExpArgs, inputParams, expressionFieldsScript);
            string expScript = ExpressionUtil.BuildRunExpressionScripts(asitExpArgs, inputParams, false, null);
            result += expScript;

            return result;
        }

        /// <summary>
        /// Attach expression builder to control
        /// </summary>
        /// <param name="controls">The Controls</param>
        /// <param name="dicExpRuntimeArgModels">The dictionary of the execute field name and the expression runtime argument model</param>
        /// <param name="dicExpressionInputFields">The dictionary of the execute field name and the input expression parameters</param>
        private void AttachExpressionBuilderToControls(
                        Dictionary<string, WebControl> controls,
                        Dictionary<string, ExpressionRuntimeArgumentsModel> dicExpRuntimeArgModels,
                        Dictionary<string, ExpressionFieldModel[]> dicExpressionInputFields)
        {
            if (controls == null || controls.Count == 0
                || dicExpRuntimeArgModels == null || dicExpRuntimeArgModels.Count == 0
                || dicExpressionInputFields == null || dicExpressionInputFields.Count == 0)
            {
                throw new ACAException("Failed to attach expression builder to control.");
            }

            WebControl ctl = null;
            CapModel4WS capModel = null;

            foreach (string dicKey in dicExpRuntimeArgModels.Keys)
            {
                int firstSplitChar = dicKey.IndexOf(ACAConstant.SPLIT_CHAR);
                string name = dicKey.Substring(firstSplitChar + 1);

                ctl = controls[HttpUtility.UrlEncode(HttpUtility.UrlDecode(name))];
                capModel = CapModels != null && CapModels.Count > 0 ? CapModels[name] : null;

                ExpressionFieldModel[] inputParams = dicExpressionInputFields[dicKey];

                if (inputParams != null)
                {
                    foreach (ExpressionFieldModel expressionModel in inputParams)
                    {
                        HandleExpress(expressionModel, capModel);
                    }
                }

                // Step 2.3 Assemble ASI Table Input Parameters based ASIT UITable Structure on current page
                List<ExpressionFieldModel> resultExpInParamList = AssembleExpressionInputParamters(capModel, inputParams);

                if (resultExpInParamList != null)
                {
                    StringBuilder functionName = new StringBuilder();
                    functionName.Append(GetClientExperssionScripts(capModel, _executeFieldList[dicKey], resultExpInParamList.ToArray(), false));
                    functionName.Append(";");
                    BindingClientMethod(ctl, _executeFieldList[dicKey].fireEvent.ToLower(), functionName.ToString());
                }
            }
        }

        /// <summary>
        /// Collect all application information fields
        /// </summary>
        /// <param name="dicCapIDAndCapModels">The dictionary of cap id and cap model</param>
        /// <returns>All ASI Fields</returns>
        private AppSpecificInfoModel[] GetASIFieldsFromCapModels(Dictionary<string, CapModel4WS> dicCapIDAndCapModels)
        {
            // Step 1, Validate the input paramters
            if (dicCapIDAndCapModels == null || dicCapIDAndCapModels.Count == 0)
            {
                return null;
            }

            // Step 2, Collect All Application Information Fields 
            List<AppSpecificInfoModel> appSpecifiInfoList = new List<AppSpecificInfoModel>();

            foreach (CapModel4WS cap in dicCapIDAndCapModels.Values)
            {
                if (cap == null || cap.appSpecificInfoGroups == null)
                {
                    continue;
                }

                foreach (AppSpecificInfoGroupModel4WS appSpecificInfoGroup in cap.appSpecificInfoGroups)
                {
                    if (appSpecificInfoGroup.fields == null)
                    {
                        continue;
                    }

                    foreach (AppSpecificInfoModel4WS appSpecificInfo in appSpecificInfoGroup.fields)
                    {
                        // Convert ASIModel4WS to ASIModel
                        AppSpecificInfoModel tmpASIModel = TempModelConvert.ConvertToAppSpecificInfoModel(appSpecificInfo);
                        if (tmpASIModel != null)
                        {
                            appSpecifiInfoList.Add(tmpASIModel);
                        }
                    }
                }
            }

            return appSpecifiInfoList.ToArray();
        }

        #endregion

        #region Nested Types

        /// <summary>
        /// Define some parameters name for expression parameter model.
        /// </summary>
        private static class ExpressionSysInputParams
        {
            #region Fields
            /// <summary>
            /// CapID1 property name
            /// </summary>
            public const string CapID1 = "capID1";

            /// <summary>
            /// property name
            /// </summary>
            public const string CapID2 = "capID2";

            /// <summary>
            /// property name
            /// </summary>
            public const string CapID3 = "capID3";

            /// <summary>
            /// property name
            /// </summary>
            public const string Department = "department";

            /// <summary>
            /// property name
            /// </summary>
            public const string FirstName = "firstName";

            /// <summary>
            /// property name
            /// </summary>
            public const string GaUserID = "gaUserID";

            /// <summary>
            /// property name
            /// </summary>
            public const string LastName = "lastName";

            /// <summary>
            /// property name
            /// </summary>
            public const string MiddleName = "middleName";

            /// <summary>
            /// property name
            /// </summary>
            public const string Module = "module";

            /// <summary>
            /// property name
            /// </summary>
            public const string Publicuser_email = "publicuser_email";

            /// <summary>
            /// property name
            /// </summary>
            public const string ServProvCode = "servProvCode";

            /// <summary>
            /// property name
            /// </summary>
            public const string Today = "today";

            /// <summary>
            /// property name
            /// </summary>
            public const string UserGroup = "userGroup";

            /// <summary>
            /// property name
            /// </summary>
            public const string UserID = "userID";

            /// <summary>
            /// property name
            /// </summary>
            public const string Userfullname = "userfullname";

            #endregion Fields
        }

        #endregion Nested Types
    }
}