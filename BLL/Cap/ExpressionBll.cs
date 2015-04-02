#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExpressionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ExpressionBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// This class provide the ability to operation expression.
    /// </summary>
    public class ExpressionBll : BaseBll, IExpressionBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of ExpressionService.
        /// </summary>
        private ExpressionWebServiceService ExpressionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ExpressionWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get expression execute fields.
        /// </summary>
        /// <param name="argumentsModel">the arguments for expression runtime</param>
        /// <returns>the expression fields</returns>
        public ExpressionFieldModel[] GetExecuteFields(ExpressionRuntimeArgumentsModel argumentsModel)
        {
            return ExpressionService.getExecuteFields(argumentsModel);
        }

        /// <summary>
        /// Get input parameters.
        /// </summary>
        /// <param name="argumentsModel">the arguments for expression runtime</param>
        /// <returns>the expression fields</returns>
        public ExpressionFieldModel[] GetInputParameters(ExpressionRuntimeArgumentsModel argumentsModel)
        {
            return ExpressionService.getInputParameters(argumentsModel);
        }

        /// <summary>
        /// Get the Input Expression Parameter for multiple execute fields
        /// </summary>
        /// <param name="arguments">The expression runtime argument model list</param>
        /// <param name="appSpecificInfoModels">The application information field model list</param>
        /// <returns>The collection of input parameters</returns>
        public ExpressionInputParamterResultModel4WS[] GetInputParametersForMultiExecuteFields(ExpressionRuntimeArgumentsModel[] arguments, AppSpecificInfoModel[] appSpecificInfoModels)
        {
            return ExpressionService.getInputParametersForMultiExecuteFields(AgencyCode, arguments, appSpecificInfoModels); 
        }

        /// <summary>
        /// Run expressions
        /// </summary>
        /// <param name="expressionRuntimeArgumentsModel">the arguments for expression runtime</param>
        /// <param name="expressionFieldModels">the model for expression fields</param>
        /// <returns>the result of the expression</returns>
        public ExpressionRunResultModel4WS RunExpressions(ExpressionRuntimeArgumentsModel expressionRuntimeArgumentsModel, ExpressionFieldModel[] expressionFieldModels)
        {
            return ExpressionService.runExpressions(expressionRuntimeArgumentsModel, expressionFieldModels);
        }

        /// <summary>
        /// Build an ExpressionRuntimeArgumentsModel object.
        /// </summary>
        /// <param name="capModel">current cap model</param>
        /// <param name="expressionType">Expression Type</param>
        /// <param name="executeFieldVariableKey">execute Field VariableKey</param>
        /// <param name="argumentPKModels">argumentPKModel.view1Keys: For ASIT - it's sub group name list | For Contact - it's contact type list
        /// argumentPKModel.view2Keys: Only for contact - it's generic template group code.
        /// argumentPKModel.view3Keys: Only for contact - it's generic template table group code and sub group code.</param>
        /// <returns>an ExpressionRuntimeArgumentsModel data</returns>
        public ExpressionRuntimeArgumentsModel BuildERArgumentsModel(CapModel4WS capModel, ExpressionType expressionType, string executeFieldVariableKey, List<ExpressionRuntimeArgumentPKModel> argumentPKModels)
        {
            ExpressionRuntimeArgumentsModel argumentsModel = new ExpressionRuntimeArgumentsModel();
            argumentsModel.apoNumber = null;

            // Get default ASI Group Code from CAP Type.
            argumentsModel.callerID = User.PublicUserId;

            if (capModel != null)
            {
                argumentsModel.capID = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);

                if (capModel.capType != null && !string.IsNullOrEmpty(capModel.capType.specInfoCode))
                {
                    argumentsModel.asiGroupCode = capModel.capType.specInfoCode;
                }
            }

            if (argumentPKModels == null)
            {
                argumentPKModels = new List<ExpressionRuntimeArgumentPKModel>();
            }

            if (!argumentPKModels.Any())
            {
                var argumentPKModel = new ExpressionRuntimeArgumentPKModel();
                argumentPKModel.portletID = (long?)expressionType;
                argumentPKModels.Add(argumentPKModel);
            }

            argumentsModel.argumentPKs = argumentPKModels.ToArray();
            argumentsModel.executeFieldVariableKey = executeFieldVariableKey;

            if (capModel != null && capModel.capID != null)
            {
                argumentsModel.servProvCode = capModel.capID.serviceProviderCode;
            }
            else
            {
                argumentsModel.servProvCode = AgencyCode;
            }

            return argumentsModel;
        }

        #endregion Methods
    }
}
