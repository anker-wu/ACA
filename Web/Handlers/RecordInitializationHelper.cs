#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: RecordInitializationHelper.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Record Initialization Helper for Url handler.
*
*  Notes:
* $Id$.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// URL routing handler help for Record.
    /// </summary>
    internal static class RecordInitializationHelper
    {
        /// <summary>
        /// Get SimpleCapModel by capId
        /// </summary>
        /// <param name="capID">the capId model.</param>
        /// <param name="capModel">out CapModel4WS</param>
        /// <returns>Cap object</returns>
        public static SimpleCapModel GetRecordById(CapIDModel4WS capID, out CapModel4WS capModel)
        {
            if (capID == null)
            {
                throw new ACAException(LabelUtil.GetGUITextByKey("aca_urlrouting_label_invalidcapid"));
            }

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            bool isSuperCAP = CapUtil.IsSuperCAP(capID);

            string userSeqNum = (AppSession.User == null || string.IsNullOrEmpty(AppSession.User.UserSeqNum)) ? ACAConstant.ANONYMOUS_FLAG : AppSession.User.UserSeqNum;
            CapWithConditionModel4WS capWithConditionModel = capBll.GetCapViewBySingle(capID, userSeqNum, ACAConstant.COMMON_N, isSuperCAP);
            capModel = capWithConditionModel == null ? null : capWithConditionModel.capModel;

            if (capModel == null)
            {
                throw new ACAException(LabelUtil.GetGUITextByKey("aca_urlrouting_label_invalidcapid"));
            }

            CapModel4WS capCondition = new CapModel4WS 
            { 
                capID = capID,
                altID = capModel.altID,
                moduleName = capModel.moduleName
            };

            SearchResultModel searchResult = capBll.QueryPermitsGC(capCondition, null, userSeqNum, false, null, false, null);

            if (searchResult == null || searchResult.resultList == null || searchResult.resultList.Length == 0 || (searchResult.resultList[0] as SimpleCapModel) == null)
            {
                throw new ACAException(LabelUtil.GetGUITextByKey("aca_urlrouting_label_nopermission_record"));
            }

            SimpleCapModel simpleCapModel = searchResult.resultList[0] as SimpleCapModel;

            return simpleCapModel;
        }

        /// <summary>
        /// initialize related session data to session so that the following steps can use necessary data from session.
        /// </summary>
        /// <param name="capModel">cap model.</param>
        public static void InitilizeRelatedDataToSession(CapModel4WS capModel)
        {
            string pageFlow = capModel.capType.smartChoiceCode4ACA;

            if (StandardChoiceUtil.IsSuperAgency())
            {
                ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                CapTypeModel capType = capTypeBll.GetCapTypeByCapIDForShoppingCart(capModel.capID);
                pageFlow = capType.smartChoiceCode;

                if (string.IsNullOrEmpty(pageFlow))
                {
                    throw new ACAException(LabelUtil.GetGUITextByKey("aca_urlrouting_label_invalidcapid"));
                }
            }

            // set template
            CapUtil.FillCapModelTemplateValue(capModel);

            // store cap to session
            CapUtil.SetCapInfoToAppSession(capModel, capModel.capType, capModel.moduleName);

            // Get pageflow group and store it to session
            IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
            var pageflowGroup = pageflowBll.GetPageflowGroupByCapType(capModel.capType);
            pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel, pageflowGroup);
            AppSession.SetPageflowGroupToSession(pageflowGroup);
        }
    }
}