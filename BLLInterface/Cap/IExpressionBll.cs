#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IExpressionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IExpressionBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header
using System.Collections.Generic;

using Accela.ACA.ExpressionBuild;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// Defines methods for expression.
    /// </summary>
    public interface IExpressionBll
    {
        #region Methods

        /// <summary>
        /// Get expression execute fields.
        /// </summary>
        /// <param name="argumentsModel">the arguments for expression runtime</param>
        /// <returns>the expression execute fields array</returns>
        ExpressionFieldModel[] GetExecuteFields(ExpressionRuntimeArgumentsModel argumentsModel);

        /// <summary>
        /// Get input parameters.
        /// </summary>
        /// <param name="argumentsModel">the arguments for expression runtime</param>
        /// <returns>the expression fields</returns>
        ExpressionFieldModel[] GetInputParameters(ExpressionRuntimeArgumentsModel argumentsModel);

        /// <summary>
        /// Get the Input Expression Parameter for multiple execute fields
        /// </summary>
        /// <param name="arguments">The expression runtime argument model list</param>
        /// <param name="appSpecificInfoModels">The application information field model list</param>
        /// <returns>The collection of input parameters</returns>
        ExpressionInputParamterResultModel4WS[] GetInputParametersForMultiExecuteFields(ExpressionRuntimeArgumentsModel[] arguments, AppSpecificInfoModel[] appSpecificInfoModels);

        /// <summary>
        /// Run expressions
        /// </summary>
        /// <param name="expressionRuntimeArgumentsModel">the arguments for expression runtime</param>
        /// <param name="expressionFieldModels">the model for expression fields</param>
        /// <returns>the result of the expression</returns>
        ExpressionRunResultModel4WS RunExpressions(ExpressionRuntimeArgumentsModel expressionRuntimeArgumentsModel, ExpressionFieldModel[] expressionFieldModels);

        /// <summary>
        /// Build an ExpressionRuntimeArgumentsModel object.
        /// </summary>
        /// <param name="capModel">current cap model</param>
        /// <param name="expressionType">Expression Type</param>
        /// <param name="executeFieldVariableKey">execute Field VariableKey</param>
        /// <param name="argumentPKModels">
        /// argumentPKModel.view1Keys: For ASIT - it's sub group name list | For Contact - it's contact type list
        /// argumentPKModel.view2Keys: Only for contact - it's generic template group code.
        /// argumentPKModel.view3Keys: Only for contact - it's generic template table group code and sub group code.
        /// </param>
        /// <returns>an ExpressionRuntimeArgumentsModel data</returns>
        ExpressionRuntimeArgumentsModel BuildERArgumentsModel(CapModel4WS capModel, ExpressionType expressionType, string executeFieldVariableKey, List<ExpressionRuntimeArgumentPKModel> argumentPKModels);

        #endregion Methods
    }
}