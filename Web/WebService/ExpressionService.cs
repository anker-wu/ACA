#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: ExpressionService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 * Service for JS function called in client end
 *  Notes:
 * $Id: ExpressionService.cs 145293 2009-08-31 04:45:27Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.WebService
{
    /// <summary>
    /// ExpressionService class
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class ExpressionService
    {
        #region Fields

        /// <summary>
        /// <c>IExpressionBll</c> instance. 
        /// </summary>
        private IExpressionBll _expressionBll;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ExpressionService class.
        /// </summary>
        public ExpressionService()
        {
            if (AppSession.User == null)
            {
                throw new ACAException("unauthenticated web service invoking");
            }

            _expressionBll = ObjectFactory.GetObject(typeof(IExpressionBll)) as IExpressionBll;
        }

        #endregion Constructors

        #region Methods
        
        /// <summary>
        /// Call AA expression engine to calculate out result.
        /// </summary>
        /// <param name="expressionArgument">Arguments model of run expression.</param>
        /// <param name="inputFieldNames">Array of input field name.</param>
        /// <param name="inputFieldProperties">Array of input field properties.</param>
        /// <returns>String of expression result.</returns>
        [WebMethod(Description = "Send all parameters to expression engine and return calculate result", EnableSession = true)]
        public string RunExpression(string expressionArgument, string[] inputFieldNames, string[] inputFieldProperties)
        {
            const string ErrorMessage = "Service Error: Expression engine meets some errors, please refresh page and try it again.";
            ExpressionRuntimeArgumentsModel expArgument = JsonConvert.DeserializeObject(expressionArgument, typeof(ExpressionRuntimeArgumentsModel)) as ExpressionRuntimeArgumentsModel;

            if (inputFieldNames == null || inputFieldProperties == null ||
                inputFieldNames.Length != inputFieldProperties.Length ||
                expArgument == null || string.IsNullOrEmpty(expArgument.servProvCode))
            {
                return ErrorMessage;
            }

            //Get input variables and system variables from session.
            Dictionary<string, ExpressionFieldModel> inputVarsFromSession = HttpContext.Current.Session[ACAConstant.Expression_InputVariables] as Dictionary<string, ExpressionFieldModel>;
            Dictionary<string, Dictionary<string, ExpressionFieldModel>> systemVarsFromSession = HttpContext.Current.Session[ACAConstant.Expression_SystemVariables] as Dictionary<string, Dictionary<string, ExpressionFieldModel>>;
            Dictionary<string, ExpressionFieldModel> curAgencySysVariables = null;

            if (systemVarsFromSession != null)
            {
                curAgencySysVariables = systemVarsFromSession[expArgument.servProvCode];
            }

            if (inputVarsFromSession == null || curAgencySysVariables == null)
            {
                return ErrorMessage;
            }

            //Combine the input variables with data from UI.
            List<ExpressionFieldModel> inputVariabls = new List<ExpressionFieldModel>();
            inputVariabls.AddRange(curAgencySysVariables.Values);

            /*
             * Scenario: Configure expression to use a Generic Template field(ASI) to update a Generic Template table(ASIT).
             *  After render the ASI field, it will be attached a onchange attribute and value is "RunExpression({argument}, {inputFieldList},...)",
             *  The {inputFieldList} is refer to all user added ASIT rows and data.
             *  But if user add a new ASIT row and then submit to ASIT list, only the ASIT owned update panel be updated.
             *  The ASI field will not be render again and the associated Expression scripts will not be updated,
             *      so the the expression will not know the new added ASIT rows and cannot to reference it.
             *  Below code is used to merge the new added ASIT fields into the Expression input field list.
             */
            List<string> mergerInputFieldNames = new List<string>();
            mergerInputFieldNames.AddRange(inputFieldNames);

            //add exists asit data
            foreach (string fieldName in inputFieldNames)
            {
                if (!fieldName.StartsWith(ExpressionFactory.INSERT_FIRST_ROW))
                {
                    continue;
                }

                ExpressionFieldModel variable = null;

                if (inputVarsFromSession.ContainsKey(fieldName))
                {
                    variable = inputVarsFromSession[fieldName];
                }
                else if (inputVarsFromSession.ContainsKey(HttpUtility.UrlEncode(fieldName)))
                {
                    variable = inputVarsFromSession[HttpUtility.UrlEncode(fieldName)];
                }
                else
                {
                    continue;
                }

                string variableKey = variable.variableKey;
                var relateInputs = inputVarsFromSession.Where(w => w.Value.variableKey.Equals(variableKey)).ToDictionary(v => v.Key, v => v.Value);

                foreach (var input in relateInputs)
                {
                    if (!mergerInputFieldNames.Contains(input.Key))
                    {
                        mergerInputFieldNames.Add(input.Key);
                    }
                }
            }

            for (int i = 0; i < mergerInputFieldNames.Count; i++)
            {
                string fieldName = mergerInputFieldNames[i];

                if (inputVarsFromSession.ContainsKey(fieldName) || inputVarsFromSession.ContainsKey(HttpUtility.UrlEncode(fieldName)))
                {
                    ExpressionFieldModel variable = inputVarsFromSession.ContainsKey(fieldName)
                                                            ? inputVarsFromSession[fieldName]
                                                            : inputVarsFromSession[HttpUtility.UrlEncode(fieldName)];

                    /*                 
                     * Scenario: Configure expression to use a field(Generic Template field/ASI/stard field/peope template field)to update a Generic Template table(ASIT).
                     *               and configure generic template table field B = generic template filed A
                     *  First : add one generic template table(ASIT) row,
                     *  Second: trigger the expression,
                     *  Third : update the generic template table(ASIT) row,
                     *  Forth : trigger the expression again,
                     *  result: find that the expression execution result is incorrect.
                     *  Below code is used to update latest value for ASIT and "Generic template table" fields.
                     *  Related bugs: #38079, #47963
                     */
                    if (ExpressionUtil.IsASITOrGenericTempalteTableView(variable.viewID))
                    {
                        variable = ExpressionUtil.GetLatestStatus4ASITField(expArgument.executeFieldVariableKey, variable);
                    }

                    // Null means that this control does not exist in page
                    if (inputFieldProperties.Length > i && inputFieldProperties[i] != null)
                    {
                        string tmpFieldProperty = inputFieldProperties[i];
                        string[] properties = tmpFieldProperty.Split(ACAConstant.SPLIT_CHAR);

                        if (properties.Length == 5)
                        {
                            bool needParseNumber = IsNeedParseNumber(variable);

                            if (variable.value != null && needParseNumber)
                            {
                                double number = 0;

                                if (string.IsNullOrEmpty(properties[0]))
                                {
                                    variable.value = null;
                                }
                                else if (I18nNumberUtil.TryParseNumberFromInput(properties[0], out number))
                                {
                                    variable.value = I18nNumberUtil.FormatNumberForWebService(number);
                                }
                            }
                            else
                            {
                                variable.value = properties[0];
                            }

                            if (string.IsNullOrEmpty(properties[1]))
                            {
                                variable.required = null;
                            }
                            else
                            {
                                variable.required = ACAConstant.COMMON_TRUE.Equals(properties[1]);
                            }

                            if (string.IsNullOrEmpty(properties[2]))
                            {
                                variable.readOnly = null;
                            }
                            else
                            {
                                variable.readOnly = ACAConstant.COMMON_TRUE.Equals(properties[2]);
                            }

                            if (string.IsNullOrEmpty(properties[3]))
                            {
                                variable.hidden = null;
                            }
                            else
                            {
                                variable.hidden = ACAConstant.COMMON_TRUE.Equals(properties[3]);
                            }

                            variable.message = properties[4];
                        }
                    }

                    variable.name = HttpUtility.UrlEncode(variable.name);
                    inputVariabls.Add(variable);
                }
            }

            ExpressionRunResultModel4WS result = _expressionBll.RunExpressions(expArgument, inputVariabls.ToArray());

            ExpressionUtil.HandleASITExpressionResult(expArgument, result, true);

            ResetValueToResult(result);

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Reset value of result before return to client side for specify logic.
        /// </summary>
        /// <param name="result">calculate result.</param>
        private void ResetValueToResult(ExpressionRunResultModel4WS result)
        {
            if (result != null && result.fields != null && result.fields.Length > 0)
            {
                //Quantity controls of fee item don't require validation.
                foreach (ExpressionFieldModel item in result.fields)
                {
                    if (item.viewID == (int)ExpressionType.Fee_Item)
                    {
                        item.required = false;
                    }

                    bool needParseNumber = IsNeedParseNumber(item);

                    if (item.value != null && needParseNumber)
                    {
                        double number = 0;

                        if (I18nNumberUtil.TryParseNumberFromWebService(item.value.ToString(), out number))
                        {
                            item.value = I18nNumberUtil.FormatNumberForUI(number);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Is need parse number.
        /// </summary>
        /// <param name="expressionFieldModel">the expression model</param>
        /// <returns>True or false.</returns>
        private bool IsNeedParseNumber(ExpressionFieldModel expressionFieldModel)
        {
            // V360 defined phone/zip/fax as number type, in fact they are string type, add the special to bypass the number parse logic.
            var isSpecialFields = @"^((CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*(phone\d*|zip|fax)|(REFCONTACT|CUSTOMER)::(phone\d*|zip|fax)|LP::professionalModel\*(phone\d*|zip|fax))$";

            Match addressMatch = Regex.Match(expressionFieldModel.variableKey, isSpecialFields);

            if (addressMatch.Success)
            {
                return false;
            }

            bool needParseNumber = false;
            
            switch (expressionFieldModel.viewID)
            {
                case (int)ExpressionType.Fee_Item:
                    needParseNumber = true;
                    break;

                case (int)ExpressionType.ASI:
                case (int)ExpressionType.ASI_Table:
                case (int)ExpressionType.Contact_TPL_Form:
                case (int)ExpressionType.RefContact_TPL_Form:
                case (int)ExpressionType.Applicant_TPL_Form:
                case (int)ExpressionType.Contact1_TPL_Form:
                case (int)ExpressionType.Contact2_TPL_Form:
                case (int)ExpressionType.Contact3_TPL_Form:
                case (int)ExpressionType.Contact_TPL_Table:
                case (int)ExpressionType.RefContact_TPL_Table:
                case (int)ExpressionType.Applicant_TPL_Table:
                case (int)ExpressionType.Contact1_TPL_Table:
                case (int)ExpressionType.Contact2_TPL_Table:
                case (int)ExpressionType.Contact3_TPL_Table:
                case (int)ExpressionType.AuthAgent_TPL_Form:
                case (int)ExpressionType.AuthAgent_TPL_Table:
                    if (expressionFieldModel.type != null && (expressionFieldModel.type == (int)FieldType.HTML_TEXTBOX_OF_NUMBER
                        || expressionFieldModel.type == (int)FieldType.HTML_TEXTBOX_OF_CURRENCY))
                    {
                        needParseNumber = true;
                    }

                    break;
                case (int)ExpressionType.License_Professional:
                case (int)ExpressionType.Applicant:
                case (int)ExpressionType.Contact_1:
                case (int)ExpressionType.Contact_2:
                case (int)ExpressionType.Contact_3:
                case (int)ExpressionType.Contacts:
                case (int)ExpressionType.ReferenceContact:
                case (int)ExpressionType.AuthAgent_Customer_Detail:
                case (int)ExpressionType.Address:
                case (int)ExpressionType.Contact_Address:
                case (int)ExpressionType.AuthAgent_Address:
                    if (expressionFieldModel.type != null && expressionFieldModel.type == (int)FieldType.HTML_TEXTBOX_OF_NUMBER)
                    {
                        needParseNumber = true;
                    }

                    break;
            }

            return needParseNumber;
        }

        #endregion Methods
    }
}