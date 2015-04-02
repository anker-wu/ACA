#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: DrillDownService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 * Service for JS function called in client end
 *  Notes:
 * $Id: DrillDownService.cs 145293 2009-08-31 04:45:27Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.Services;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebService
{
    /// <summary>
    /// Drill Down Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class DrillDownService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrillDownService"/> class.
        /// </summary>
        public DrillDownService()
        {
            if (AppSession.User == null)
            {
                throw new ACAException("unauthenticated web service invoking");
            }
        }

        /// <summary>
        /// Get next ASI drill down
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="subGroupName">Name of the sub group.</param>
        /// <param name="seriesIds">The series ids.</param>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>return a string has all drop down options</returns>
        [WebMethod(Description = "GetNextDrillDown", EnableSession = true)]
        public string GetNextDrillDown(string agencyCode, string groupName, string subGroupName, string seriesIds, string selectedValue)
        {
            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(groupName) ||
                string.IsNullOrEmpty(subGroupName) || string.IsNullOrEmpty(seriesIds) || string.IsNullOrEmpty(selectedValue))
            {
                return string.Empty;
            }

            string[] seriesIDList = seriesIds.Split(ACAConstant.SPLIT_CHAR18);

            IAppSpecificInfoBll appSpecificInfoBll = (IAppSpecificInfoBll)ObjectFactory.GetObject(typeof(IAppSpecificInfoBll));
            ASITableDrillDownModel4WS asiDrillDownModel =
                appSpecificInfoBll.GetNextASIDrillDownData(agencyCode, groupName, subGroupName, seriesIDList, selectedValue);

            StringBuilder optionsBuilder = new StringBuilder();
            optionsBuilder.Append("[");

            for (int i = 0; i < seriesIDList.Length; i++)
            {
                string seriesID = seriesIDList[i];
                optionsBuilder.Append(StructureOneOption(seriesID, string.Empty, WebConstant.DropDownDefaultText)).Append(",");
                ASITableDrillDValMapModel4WS[] drillDownValueMapList = GetDrillDValueMapList(seriesID, asiDrillDownModel);

                if (drillDownValueMapList != null && drillDownValueMapList.Length > 0)
                {
                    foreach (ASITableDrillDValMapModel4WS valueMapModel in drillDownValueMapList)
                    {
                        string resValue = string.IsNullOrEmpty(valueMapModel.resChildValueName) ? valueMapModel.childValueName : valueMapModel.resChildValueName;
                        optionsBuilder.Append(StructureOneOption(seriesID, valueMapModel.childValueName, resValue)).Append(",");
                    }
                }
            }

            string optionString = optionsBuilder.ToString();
            optionString = optionString.Substring(0, optionString.Length - 1) + "]";
            return optionString;
        }

        /// <summary>
        /// Structures the one option.
        /// </summary>
        /// <param name="seriesID">The series ID.</param>
        /// <param name="optionValue">The option value.</param>
        /// <param name="optionText">The option text.</param>
        /// <returns>return an option string.</returns>
        private string StructureOneOption(string seriesID, string optionValue, string optionText)
        {
            StringBuilder sbOption = new StringBuilder();
            sbOption.Append("{");
            sbOption.Append("'seriesID'").Append(":").Append("'").Append(ScriptFilter.EncodeJson(seriesID)).Append("'");
            sbOption.Append(",");
            sbOption.Append("'ListValue'").Append(":").Append("'").Append(ScriptFilter.EncodeJson(optionValue)).Append("'");
            sbOption.Append(",");
            sbOption.Append("'ListText'").Append(":").Append("'").Append(ScriptFilter.EncodeJson(optionText)).Append("'");
            sbOption.Append("}");

            return sbOption.ToString();
        }

        /// <summary>
        /// Gets the drill down value map list.using System.Web.Security;
        /// </summary>
        /// <param name="seriesID">The series ID.</param>
        /// <param name="asiDrillDownModel">The ASI drill down model.</param>
        /// <returns>return drill down value map list</returns>
        private ASITableDrillDValMapModel4WS[] GetDrillDValueMapList(string seriesID, ASITableDrillDownModel4WS asiDrillDownModel)
        {
            ASITableDrillDValMapModel4WS[] drillDownValueMapList = null;

            if (asiDrillDownModel == null || asiDrillDownModel.seriesModelList == null || asiDrillDownModel.seriesModelList.Length <= 0)
            {
                return drillDownValueMapList;
            }

            foreach (ASITableDrillDSeriesModel4WS seriesModel in asiDrillDownModel.seriesModelList)
            {
                if (seriesModel == null)
                {
                    continue;
                }

                if (seriesID.Equals(seriesModel.seriesId, StringComparison.InvariantCulture))
                {
                    drillDownValueMapList = seriesModel.asiTableDrillDValMapModel4WSList;
                    break;
                }
            }

            return drillDownValueMapList;
        }
    }
}