#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IValuationCalculatorBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IValuationCalculatorBll.cs  
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// This interface provide the ability to operation daily side Valuation Calculator.
    /// </summary>
    public interface IValuationCalculatorBll
    {
        #region Methods 

        /// <summary>
        /// Get Ref valuation calculator models by cap type.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>ref valuation calculator array</returns>
        GValuationModel4WS[] GetRefValuationCalculatorModelsByCapType(CapTypeModel capType);

        /// <summary>
        /// Get daily valuation calculator model list by cap type.
        /// </summary>
        /// <param name="capType">cap type</param>
        /// <param name="capId">cap id</param>
        /// <returns>valuation calculator Model array</returns>
        BCalcValuatnModel4WS[] GetBCalcValuationListByCapType(CapTypeModel capType, CapIDModel4WS capId);

        /// <summary>
        /// Convert RefEducation model array to valuation calculator model array.
        /// </summary>
        /// <param name="gValuationCalculators">The aggregate valuation calculators.</param>
        /// <param name="capId">cap id</param>
        /// <returns>valuation calculator model array</returns>
        BCalcValuatnModel4WS[] ConvertRefValCalcul2ValCalculs(GValuationModel4WS[] gValuationCalculators, CapIDModel4WS capId);

        /// <summary>
        /// Get Valuation Calculator Define
        /// </summary>
        /// <returns>valuation calculator array.</returns>
        BCalcValuatnModel4WS[] GetNullValuationCalculator(); 

        /// <summary>
        /// Get valuations calculator table by valuations calculator objects
        /// </summary>
        /// <param name="calculatorValuations">calculator Valuations</param>
        /// <returns>calculator Valuations data table</returns>
        DataTable GetValationCalculatorTable(BCalcValuatnModel4WS[] calculatorValuations);

        #endregion Methos
    }
}