#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ValuationCalculatorBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ValuationCalculatorBll.cs 
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// This class provide the ability to operation daily side valuation calculator.
    /// </summary>
    public class ValuationCalculatorBll : BaseBll, IValuationCalculatorBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of FeeValuationService.
        /// </summary>
        private static FeeValuationWebServiceService FeeValuationService
        {
            get
            {
                return WSFactory.Instance.GetWebService<FeeValuationWebServiceService>();
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Convert RefEducation model array to valuation calculator model array.
        /// </summary>
        /// <param name="gValuationCalculators">The aggregate valuation calculators.</param>
        /// <param name="capId">cap id</param>
        /// <returns>valuation calculator model array</returns>
        BCalcValuatnModel4WS[] IValuationCalculatorBll.ConvertRefValCalcul2ValCalculs(GValuationModel4WS[] gValuationCalculators, CapIDModel4WS capId)
        {
            IList<BCalcValuatnModel4WS> valuationcalculatorList = new List<BCalcValuatnModel4WS>();

            if (gValuationCalculators != null && gValuationCalculators.Length > 0)
            {
                foreach (GValuationModel4WS refValuationCalculator in gValuationCalculators)
                {
                    BCalcValuatnModel4WS valcalculator = ConvertRefValCalulator2ValCalulator(refValuationCalculator, capId);

                    valuationcalculatorList.Add(valcalculator);
                }
            }

            BCalcValuatnModel4WS[] valuationcaculators = new BCalcValuatnModel4WS[valuationcalculatorList.Count];
            valuationcalculatorList.CopyTo(valuationcaculators, 0);
            return valuationcaculators;
        }

        /// <summary>
        /// Get Valuation Calculator Define
        /// </summary>
        /// <returns>valuation calculator array.</returns>
        BCalcValuatnModel4WS[] IValuationCalculatorBll.GetNullValuationCalculator()
        {
            BCalcValuatnModel4WS[] valcals = new BCalcValuatnModel4WS[1];
            valcals[0] = new BCalcValuatnModel4WS();
            valcals[0].auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);            
            valcals[0].auditStatus = ACAConstant.VALID_STATUS;
            valcals[0].auditID = string.Empty;
            valcals[0].calcValueSeqNbr = 0;      
            valcals[0].totalValue = 0;
            valcals[0].unitCost = 0;
            valcals[0].unitTyp = string.Empty;
            valcals[0].unitValue = 0;
            valcals[0].useTyp = string.Empty;
            valcals[0].version = string.Empty;           
            return valcals;
        }

        /// <summary>
        /// Get daily valuation calculator model list by cap type.
        /// </summary>
        /// <param name="capType">cap type</param>
        /// <param name="capId">cap id</param>
        /// <returns>valuation calculator Model array</returns>
        BCalcValuatnModel4WS[] IValuationCalculatorBll.GetBCalcValuationListByCapType(CapTypeModel capType, CapIDModel4WS capId)
        {
            if (capType == null)
            {
                return null;
            }

            return FeeValuationService.getBCalcValuationListByAppType(capType, capId, User.PublicUserId);
        }

        /// <summary>
        /// Get Ref valuation calculator models by cap type.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>ref education model array</returns>
        GValuationModel4WS[] IValuationCalculatorBll.GetRefValuationCalculatorModelsByCapType(CapTypeModel capType)
        {            
            if (capType == null)
            {
                return null;
            }

            return FeeValuationService.getGValuationByAppType(capType);
        }

        /// <summary>
        /// Get valuations calculator table by valuations calculator objects
        /// </summary>
        /// <param name="calculatorValuations">calculator Valuations</param>
        /// <returns>calculator Valuations data table</returns>
        DataTable IValuationCalculatorBll.GetValationCalculatorTable(BCalcValuatnModel4WS[] calculatorValuations)
        {
            Dictionary<string, BCalcValuatnModel4WS[]> dicValCal = new Dictionary<string, BCalcValuatnModel4WS[]>();

            DataTable dtGroup = new DataTable();
            dtGroup.Columns.Add(new DataColumn("valuationCalculatorGroup", typeof(string)));
            dtGroup.Columns.Add(new DataColumn("valuationCalculatorList", typeof(object)));
            dtGroup.Columns.Add(new DataColumn("valuationMultiplier", typeof(double)));
            dtGroup.Columns.Add(new DataColumn("valuationExtraAmount", typeof(double)));
            dtGroup.Columns.Add(new DataColumn("capTypeDisplay", typeof(string)));

            if (calculatorValuations != null && calculatorValuations.Length > 0)
            {
                foreach (BCalcValuatnModel4WS calval in calculatorValuations)
                {
                    if (calval == null 
                        || (calval.capID != null && calval.capID.serviceProviderCode == null) 
                        || dicValCal.ContainsKey(calval.capID.id1 + calval.capID.id2 + calval.capID.id3 + calval.capID.serviceProviderCode))
                    {
                        continue;
                    }

                    string groupName = calval.capID.id1 + calval.capID.id2 + calval.capID.id3 + calval.capID.serviceProviderCode;                     
                    BCalcValuatnModel4WS[] valcals = GetGroupValuationCalculators(calculatorValuations, groupName);
                    dicValCal.Add(groupName, valcals);
                    DataRow rowGrp = dtGroup.NewRow();
                    rowGrp["valuationCalculatorGroup"] = calval.capID.serviceProviderCode;
                    
                    ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                    CapModel4WS capModel = capBll.GetCapViewDetailByPK(calval.capID, User.UserSeqNum);

                    rowGrp["valuationMultiplier"] = capModel.capDetailModel.valuationMultiplier;
                    rowGrp["valuationExtraAmount"] = capModel.capDetailModel.valuationExtraAmount;
                    rowGrp["capTypeDisplay"] = string.IsNullOrEmpty(capModel.capType.resAlias) ? capModel.capType.alias : capModel.capType.resAlias;
                    rowGrp["valuationCalculatorList"] = ObjectConvertUtil.ConvertArrayToList(valcals);
                    dtGroup.Rows.Add(rowGrp);
                }
            }

            return dtGroup;
        }

        /// <summary>
        /// Get Group Valuation Calculators
        /// </summary>
        /// <param name="valationCalculators">valuation calculators</param>
        /// <param name="groupName">group name</param>
        /// <returns>valuation calculator data table</returns>
        private BCalcValuatnModel4WS[] GetGroupValuationCalculators(BCalcValuatnModel4WS[] valationCalculators, string groupName)
        {
            ArrayList alValCals = new ArrayList();

            if (valationCalculators != null && valationCalculators.Length > 0)
            {
                foreach (BCalcValuatnModel4WS sameValCal in valationCalculators)
                {
                    if (groupName.Equals(sameValCal.capID.id1 + sameValCal.capID.id2 + sameValCal.capID.id3 + sameValCal.capID.serviceProviderCode))
                    {
                        alValCals.Add(sameValCal);
                    }
                }
            }

            return (BCalcValuatnModel4WS[])alValCals.ToArray(typeof(BCalcValuatnModel4WS));
        }

        /// <summary>
        /// Convert Ref valuation calculator model to valuation calculator model.
        /// </summary>
        /// <param name="refValCalculator">The preference value calculator.</param>
        /// <param name="capId">The cap unique identifier.</param>
        /// <returns>valuation calculator model</returns>
        private BCalcValuatnModel4WS ConvertRefValCalulator2ValCalulator(GValuationModel4WS refValCalculator, CapIDModel4WS capId)
        {
            BCalcValuatnModel4WS valuacalculator = new BCalcValuatnModel4WS();
            valuacalculator.auditDate = refValCalculator.auditDate;
            valuacalculator.auditID = refValCalculator.auditID;
            valuacalculator.auditStatus = refValCalculator.auditStatus;
            valuacalculator.calcValueSeqNbr = refValCalculator.calcValueSeqNbr;
            valuacalculator.conTyp = refValCalculator.conTyp;
            valuacalculator.excludeRegionalModifier = refValCalculator.excludeRegionalModifier;
            valuacalculator.feeIndicator = refValCalculator.feeIndicator;
            valuacalculator.totalValue = refValCalculator.totalValue;
            valuacalculator.unitCost = refValCalculator.unitAmount;
            valuacalculator.unitTyp = refValCalculator.unitTyp;
            valuacalculator.unitValue = refValCalculator.unitValue;
            valuacalculator.useTyp = refValCalculator.useTyp;
            valuacalculator.version = refValCalculator.version;
            valuacalculator.capID = new CapIDModel4WS();
            valuacalculator.capID.serviceProviderCode = refValCalculator.servProvCode;
            valuacalculator.capID.id1 = capId.id1;
            valuacalculator.capID.id2 = capId.id2;
            valuacalculator.capID.id3 = capId.id3;
            return valuacalculator;
        }

        #endregion Method
    }
}
