#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ASITDrillDownService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ASITDrillDownService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ComponentService.Model;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// Custom service of the ASIT Drill Down
    /// </summary>
    public class ASITDrillDownService : BaseService
    {
        /// <summary>
        /// DrillDown Business Class
        /// </summary>
        private IASITableDrillDownBLL asitDrill = ObjectFactory.GetObject<IASITableDrillDownBLL>();

        /// <summary>
        /// Initializes a new instance of the ASITDrillDownService class.
        /// </summary>
        /// <param name="context">User info in the context</param>
        public ASITDrillDownService(UserContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Get the ASITUITable from ACA Session
        /// </summary>
        /// <param name="tableKey">The session key</param>
        /// <param name="moduleName">The module of ACA</param>
        /// <returns>the Wrapper Model of the ASIT</returns>
        public static ASITWrapperModel GetASITUITableFromCurSession(string tableKey, string moduleName)
        {
            var allAsiTables = GetDataFromUIContainer(UIDataType.ASIT, null) as ASITUITable[];

            if (allAsiTables == null)
            {
                return null;
            }

            var curUiTable = allAsiTables.SingleOrDefault(t => t.TableKey == tableKey);

            if (curUiTable == null)
            {
                return null;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            AppSpecificTableGroupModel4WS[] asitGroupModels = capModel.appSpecTableGroups;

            AppSpecificTableModel4WS specifiedAsitTableInfo = null;
            AppSpecificTableColumnModel4WS[] specifiedAsitCols = null;

            AppSpecificTableGroupModel4WS specifiedAsitGroup = asitGroupModels.FirstOrDefault(t => t.groupName.ToLower().Equals(curUiTable.GroupName.ToLower()));

            if (specifiedAsitGroup != null)
            {
                AppSpecificTableModel4WS[] asitTableInfos = specifiedAsitGroup.tablesMapValues;

                foreach (AppSpecificTableModel4WS asit in asitTableInfos)
                {
                    if (asit.tableName.ToLower().Equals(curUiTable.TableName.ToLower()))
                    {
                        specifiedAsitTableInfo = asit;
                        break;
                    }
                }
            }

            if (specifiedAsitTableInfo != null)
            {
                specifiedAsitCols = specifiedAsitTableInfo.columns;
            }

            ASITWrapperModel asitDesc = new ASITWrapperModel(curUiTable, specifiedAsitCols);

            return asitDesc;
        }

        /// <summary>
        /// Get first drill down map list of ASIT
        /// </summary>
        /// <param name="agencyCode">Agency code of ASIT</param>
        /// <param name="asitGroupName">Group Name of ASIT</param>
        /// <param name="asitSubGroupName">Sub Group Name of ASIT</param>
        /// <returns>the Wrapper Model of the Drill Down Series</returns>
        public DrillDownSeriesWrapperModel GetFirstDrillDownListOfASIT(string agencyCode, string asitGroupName, string asitSubGroupName)
        {
            DrillDownSeriesWrapperModel seriesModel = null;
            ASITableDrillDSeriesModel4WS[] asiTableDrillDSeries = asitDrill.GetFirstASITableDrillDownDatas(agencyCode, asitGroupName, asitSubGroupName);

            if (asiTableDrillDSeries != null && asiTableDrillDSeries.Any())
            {
                seriesModel = new DrillDownSeriesWrapperModel(asiTableDrillDSeries[0]);
            }

            return seriesModel;
        }

        /// <summary>
        /// Get Next Drill down map list of ASIT .
        /// </summary>
        /// <param name="agencyCode">Agency code of ASIT</param>
        /// <param name="selectedIdsOfLastDrillDown">The selected drill down item list of series</param>
        /// <param name="seriesId">the series id of ASIT</param>
        /// <returns>the Wrapper Model of Drill Down Series</returns>
        public DrillDownSeriesWrapperModel GetNextDrillDownListOfASIT(string agencyCode, string[] selectedIdsOfLastDrillDown, string seriesId)
        {
            DrillDownSeriesWrapperModel seriesModel = null;
            ASITableDrillDSeriesModel4WS[] asitDrillDSeriesModel4Ws = asitDrill.GetNextASITableDrillDownDatas(agencyCode, selectedIdsOfLastDrillDown, long.Parse(seriesId));
            
            if (asitDrillDSeriesModel4Ws != null && asitDrillDSeriesModel4Ws.Any())
            {
                seriesModel = new DrillDownSeriesWrapperModel(asitDrillDSeriesModel4Ws[0]);
            }

            return seriesModel;
        }

        /// <summary>
        /// Search the last item of ASIT Drill Down
        /// </summary>
        /// <param name="sAgencyCode">Agency code of ASIT</param>
        /// <param name="asitGroupName">Group Name of ASIT</param>
        /// <param name="asitSubGroupName">Sub Group Name of ASIT</param>
        /// <param name="searchValue">the string of the search input text</param>
        /// <returns>the list of the Wrapper model of the Drill Down item</returns>
        public List<DrillDownItemWrapperModel> SearchDrillDownItem(string sAgencyCode, string asitGroupName, string asitSubGroupName, string searchValue)
        {
            string[][] searchResult = asitDrill.LoadSearchResultBySearchValue(sAgencyCode, asitGroupName, asitSubGroupName, searchValue);
            List<DrillDownItemWrapperModel> list = DrillDownItemWrapperModel.ConvertFromSearchResult(searchResult);
            return list;
        }

        /// <summary>
        /// Update the select ASIT drill down item back to ACA
        /// </summary>
        /// <param name="finalColumnNames">the ASIT Columns names set</param>
        /// <param name="selectIdList">the select drill down item id list</param>
        /// <param name="groupName">Group Name of ASIT</param>
        /// <param name="subGroupName">Sub Group Name of ASIT</param>
        /// <param name="asitUiKey">The UI Key of the ASIT</param>
        /// <returns>if Success the result is true .</returns>
        public bool UpdateSelectedDrillDown(Dictionary<string, int> finalColumnNames, List<string[]> selectIdList, string groupName, string subGroupName, string[] asitUiKey)
        {
            var uiDataKey = asitUiKey != null ? asitUiKey[0] : string.Empty;
            ASITUITable[] uiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new string[] { uiDataKey }) as ASITUITable[];
            
            if (uiTables == null)
            {
                return false;
            }

            ASITUITable currentTable = uiTables.SingleOrDefault(t => t.AgencyCode == AgencyCode && t.GroupName == groupName && t.TableName == subGroupName);

            if (currentTable == null)
            {
                return false;
            }

            var dTable = ASITUIModelUtil.CreateNewRow4ASITUITable(currentTable, selectIdList.Count, uiDataKey);

            for (var i = 0; i < selectIdList.Count; i++)
            {
                var languageValueIDs = selectIdList[i];
                UIRow currentRow = dTable.Rows[i];
                string[] languageValue = asitDrill.GetMainLanguageValues(AgencyCode, languageValueIDs);

                foreach (UIField field in currentRow.Fields)
                {
                    ASITUIField currentField = field as ASITUIField;

                    if (currentField != null && finalColumnNames.ContainsKey(currentField.Label))
                    {
                        int columnIndexID = finalColumnNames[currentField.Label];
                        currentField.IsDrillDown = true;
                        currentField.Value = languageValue[columnIndexID];
                    }
                }
            }

            UIModelUtil.SetDataToUIContainer(new ASITUITable[] { dTable }, UIDataType.ASITEdit, null);

            Dictionary<string, UITable[]> allAsitUiDataList = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT);
            Dictionary<string, UITable[]> curAsitUiDataList = allAsitUiDataList.Where(v => asitUiKey.Contains(v.Key)).ToDictionary(v => v.Key, v => v.Value);
            Dictionary<string, UITable[]> asitCopyUiData = ObjectCloneUtil.DeepCopy(curAsitUiDataList);
            UIModelUtil.SetDataToUIContainer(asitCopyUiData, UIDataType.ASITCopy);

            var asitEditUiData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, null) as ASITUITable[];
            var asitCopyUiDataArr = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITCopy, asitUiKey) as ASITUITable[];

            if (asitEditUiData == null || asitCopyUiData == null)
            {
                return false;
            }

            ASITUIModelUtil.SyncASITUIRowData(asitEditUiData, asitCopyUiDataArr);

            return true;
        }

        /// <summary>
        /// Build the successfully add ASIT row script lin.
        /// </summary>
        /// <returns>the script content</returns>
        public StringBuilder BuildSuccessfullyAddASITRowScript()
        {
            var asitEditUiData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, null) as ASITUITable[];

            var submitScripts = new StringBuilder("ReloadListAndCloseDialog([");
            var tableKeys = from t in asitEditUiData
                            select "'" + t.TableKey + "'";
            submitScripts.AppendFormat(string.Join(",", tableKeys));
            submitScripts.Append("]);");

            return submitScripts;
        }

        /// <summary>
        /// Get the constant of ASIT_POSTEVENT_SYNCUICOPYDATA from ACA
        /// </summary>
        /// <returns>the value of the constant</returns>
        public string GetASIT_POSTEVENT_SYNCUICOPYDATA()
        {
            return ASITUIModelUtil.ASIT_POSTEVENT_SYNCUICOPYDATA;
        }

        /// <summary>
        /// Get UI data from UI container.
        /// </summary>
        /// <param name="dataType">UI data type.</param>
        /// <param name="dataKey">UI data key.</param>
        /// <returns>Array of UI table.</returns>
        private static UITable[] GetDataFromUIContainer(UIDataType dataType, string[] dataKey)
        {
            return UIModelUtil.GetDataFromUIContainer(dataType, dataKey);
        }
    }
}