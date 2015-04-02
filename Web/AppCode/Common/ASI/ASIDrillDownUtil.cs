#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ASIDrillDownUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ASIDrillDownUtil.cs 278416 2014-09-03 08:48:18Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// ASI drill down utility
    /// </summary>
    [System.Serializable]
    public class ASIDrillDownUtil
    {
        /// <summary>
        /// Gets the cur ASI drill down.
        /// </summary>
        /// <param name="asiDrillDownModels">The asi drill down models.</param>
        /// <param name="specInfoGroup">The spec info group.</param>
        /// <param name="moduleName">module name</param>
        /// <returns>get drill down for ASI group</returns>
        public ASITableDrillDownModel4WS GetCurASIDrillDown(ASITableDrillDownModel4WS[] asiDrillDownModels, AppSpecificInfoGroupModel4WS specInfoGroup, string moduleName)
        {
            ASITableDrillDownModel4WS curASIDrillDown = null;

            //Get current drill down associated current ASI sub group from the drill down list associated current ASI group.
            if (asiDrillDownModels != null)
            {
                foreach (ASITableDrillDownModel4WS drillDownModel in asiDrillDownModels)
                {
                    if (specInfoGroup.groupCode.Equals(drillDownModel.entityKey1, StringComparison.InvariantCulture)
                        && specInfoGroup.groupName.Equals(drillDownModel.entityKey2, StringComparison.InvariantCulture))
                    {
                        curASIDrillDown = drillDownModel;
                        break;
                    }
                }
            }

            //if current drill down has a invisible ASI field, current drill down don't be applied to current ASI sub group,
            //so system sets it's value null and doesn't apply the drill down to current ASI sub group.
            if (curASIDrillDown != null && !IsValidDrillDown(specInfoGroup, curASIDrillDown, moduleName))
            {
                curASIDrillDown = null;
            }

            return curASIDrillDown;
        }

        /// <summary>
        /// Changes the ASI data source.
        /// </summary>
        /// <param name="specInfoGroup">The spec info group.</param>
        /// <param name="curASIDrillDown">The cur ASI drill down.</param>
        /// <param name="groupIndex">Index of the group.</param>
        /// <returns>return ASI sub group with changed data source by drill down.</returns>
        public AppSpecificInfoGroupModel4WS ChangeASIDataSource(AppSpecificInfoGroupModel4WS specInfoGroup, ASITableDrillDownModel4WS curASIDrillDown, int groupIndex)
        {
            //if a ASI field associated to a drill down, it is possible to change it's data source as below: 
            //1. The field is a head field in drill down.
            //2. The field has default value.
            //3. The field has user entered value.
            if (curASIDrillDown != null)
            {
                curASIDrillDown = SortSeriesInDrillDown(curASIDrillDown);

                //if user cancel the ASI editor in CAP confirm form, system can't change the ASI value with user input information.
                string eventTarget = Convert.ToString(HttpContext.Current.Request["__EVENTTARGET"]);

                if (eventTarget == null || eventTarget.IndexOf("btnAppSpecInfo") < 0)
                {
                    specInfoGroup = SetClientValueIntoASI(specInfoGroup, groupIndex, curASIDrillDown);
                }

                IAppSpecificInfoBll appSpecificInfoBll = (IAppSpecificInfoBll)ObjectFactory.GetObject(typeof(IAppSpecificInfoBll));
                specInfoGroup = appSpecificInfoBll.ChangeASIDataSourceByDrillD(specInfoGroup, curASIDrillDown);
            }

            return specInfoGroup;
        }

        /// <summary>
        /// Determines whether [is drill down field] [the specified drill down model].
        /// </summary>
        /// <param name="drillDownModel">The drill down model.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// <c>true</c> if [is drill down field] [the specified drill down model]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDrillDownField(ASITableDrillDownModel4WS drillDownModel, string fieldName)
        {
            if (drillDownModel == null || drillDownModel.seriesModelList == null || drillDownModel.seriesModelList.Length <= 0)
            {
                return false;
            }

            bool isDrillDField = false;
            ASITableDrillDSeriesModel4WS[] drillDSerieses = drillDownModel.seriesModelList;

            foreach (ASITableDrillDSeriesModel4WS drillDSeries in drillDSerieses)
            {
                string parentColName = Convert.ToString(drillDSeries.parentColName);
                string childColName = Convert.ToString(drillDSeries.childColName);

                if (parentColName.Equals(fieldName, StringComparison.InvariantCulture)
                    || childColName.Equals(fieldName, StringComparison.InvariantCulture))
                {
                    isDrillDField = true;
                    break;
                }
            }

            return isDrillDField;
        }

        /// <summary>
        /// Adds the attribute for drill down.
        /// </summary>
        /// <param name="dropDownList">The drop down list.</param>
        /// <param name="appSpecInfo">The app spec info.</param>
        /// <param name="specInfoGroupModels">The spec info group models.</param>
        /// <param name="drillDownModel">The drill down model.</param>
        /// <returns>return a web control added the drill down information.</returns>
        public WebControl AddAttributeForDrillDown(WebControl dropDownList, AppSpecificInfoModel4WS appSpecInfo, AppSpecificInfoGroupModel4WS[] specInfoGroupModels, ASITableDrillDownModel4WS drillDownModel)
        {
            if (drillDownModel == null || drillDownModel.seriesModelList == null || drillDownModel.seriesModelList.Length <= 0)
            {
                return dropDownList;
            }

            string nextColNames = string.Empty;

            ArrayList childSeriesIds = new ArrayList();
            ArrayList childFields = new ArrayList();

            //structures seriesIds. and gets all chield ASI fields' name.
            foreach (ASITableDrillDSeriesModel4WS seriesModel in drillDownModel.seriesModelList)
            {
                if (seriesModel == null)
                {
                    continue;
                }

                if (appSpecInfo.checkboxDesc.Equals(seriesModel.parentColName))
                {
                    if (!childSeriesIds.Contains(seriesModel.seriesId))
                    {
                        childSeriesIds.Add(seriesModel.seriesId);
                    }

                    if (!childFields.Contains(seriesModel.childColName))
                    {
                        childFields.Add(seriesModel.childColName);
                    }
                }
            }

            StringBuilder sbChildControlId = new StringBuilder();

            //Structures next columns' name.
            if (childFields.Count > 0)
            {
                for (int i = 0; i < childFields.Count; i++)
                {
                    ASIBaseUC asiBaseUC = new ASIBaseUC();
                    AppSpecificInfoModel4WS specInfoModel = GetSpecInfoModel(specInfoGroupModels, appSpecInfo, childFields[i].ToString());
                    int groupIndex = ASIBaseUC.GetSpecInfoGroupIndex(specInfoGroupModels, specInfoModel);
                    int itemIndex = ASIBaseUC.GetSpecInfoIndex(specInfoGroupModels, specInfoModel);
                    sbChildControlId.Append(asiBaseUC.StructureDDLControlID(groupIndex, itemIndex)).Append(ACAConstant.SPLIT_CHAR18);
                }
            }

            string nextControlIDs = sbChildControlId.ToString();

            if (nextControlIDs.Length > 0)
            {
                dropDownList.Attributes.Add("NextControlIDs", nextControlIDs.Substring(0, nextControlIDs.Length - 1));
            }

            string seriesIds = string.Empty;

            if (childSeriesIds.Count > 0)
            {
                StringBuilder sbSeriesIds = new StringBuilder();

                foreach (string curSeries in childSeriesIds)
                {
                    sbSeriesIds.Append(curSeries).Append(ACAConstant.SPLIT_CHAR18);
                }

                seriesIds = sbSeriesIds.ToString();
            }

            if (seriesIds.Length > 0)
            {
                dropDownList.Attributes.Add("SeriesIds", seriesIds.Substring(0, seriesIds.Length - 1));
            }

            if (nextControlIDs.Length > 0 && seriesIds.Length > 0)
            {
                string agencyCode =
                string.IsNullOrEmpty(appSpecInfo.serviceProviderCode) ? ConfigManager.AgencyCode : appSpecInfo.serviceProviderCode;
                dropDownList.Attributes.Add("AgencyCode", agencyCode);
                dropDownList.Attributes.Add("GroupName", drillDownModel.entityKey1);
                dropDownList.Attributes.Add("SubGroupName", drillDownModel.entityKey2);
                ASIDrillDownUtil asiDrillDownInstance = new ASIDrillDownUtil();
                string sortedControls = asiDrillDownInstance.GetSortedControlsFromDrillD(drillDownModel, specInfoGroupModels, appSpecInfo);
                dropDownList.Attributes.Add("SortedControls", sortedControls);
                dropDownList.Attributes.Add("onchange", "GetNextDrillDown(this);");
            }

            dropDownList.Attributes.Add("ValidateDisabledControl", ACAConstant.COMMON_Y);

            return dropDownList;
        }

        /// <summary>
        /// Disables the control for drill down.
        /// </summary>
        /// <param name="dropDownList">The drop down list.</param>
        /// <param name="appSpecInfo">The app spec info.</param>
        /// <param name="specInfoGroupModels">The spec info group models.</param>
        /// <param name="drillDownModel">The drill down model.</param>
        /// <returns>return a web control with the enable property from drill down.</returns>
        public WebControl DisableControlForDrillDown(WebControl dropDownList, AppSpecificInfoModel4WS appSpecInfo, AppSpecificInfoGroupModel4WS[] specInfoGroupModels, ASITableDrillDownModel4WS drillDownModel)
        {
            // the control's 'Enable' property's default value is true.
            if (drillDownModel == null || drillDownModel.seriesModelList == null || drillDownModel.seriesModelList.Length <= 0)
            {
                return dropDownList;
            }

            if (appSpecInfo.valueList == null || appSpecInfo.valueList.Length <= 0)
            {
                string fieldName = appSpecInfo.checkboxDesc;
                string parentFieldName = string.Empty;

                foreach (ASITableDrillDSeriesModel4WS seriesModel in drillDownModel.seriesModelList)
                {
                    if (fieldName.Equals(seriesModel.childColName, StringComparison.InvariantCulture))
                    {
                        parentFieldName = seriesModel.parentColName;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(parentFieldName))
                {
                    AppSpecificInfoGroupModel4WS curAppSpecInfoGroup =
                        GetSpecInfoGroupModel(specInfoGroupModels, appSpecInfo.actStatus, appSpecInfo.checkboxType);

                    foreach (AppSpecificInfoModel4WS specificInfo in curAppSpecInfoGroup.fields)
                    {
                        //if the field hasn't options and it's parent field hasn't options too, it must be set to disable.
                        //or parent selected value is null, it must be set to disable too.
                        if (parentFieldName.Equals(specificInfo.checkboxDesc, StringComparison.InvariantCulture)
                            && !HaveParentOptionSelected(specificInfo))
                        {
                            (dropDownList as AccelaDropDownList).DisableEdit();
                        }
                    }
                }
            }

            return dropDownList;
        }

        /// <summary>
        /// Gets the sorted controls from drill D.
        /// </summary>
        /// <param name="drillDownModel">The drill down model.</param>
        /// <param name="specInfoGroupModels">The spec info group models.</param>
        /// <param name="appSpecInfo">The app spec info.</param>
        /// <returns>get all controls sorted by drill down.</returns>
        public string GetSortedControlsFromDrillD(ASITableDrillDownModel4WS drillDownModel, AppSpecificInfoGroupModel4WS[] specInfoGroupModels, AppSpecificInfoModel4WS appSpecInfo)
        {
            string sortedControls = string.Empty;

            if (drillDownModel == null || drillDownModel.seriesModelList == null || drillDownModel.seriesModelList.Length <= 0)
            {
                return sortedControls;
            }

            List<string> headFields = GetHeadFields(drillDownModel.seriesModelList);
            StringBuilder sbSortControlID = new StringBuilder();
            ASIBaseUC asiBaseUC = new ASIBaseUC();

            foreach (ASITableDrillDSeriesModel4WS seriesModel in drillDownModel.seriesModelList)
            {
                if (seriesModel == null)
                {
                    continue;
                }

                AppSpecificInfoModel4WS parentSpecInfoModel = GetSpecInfoModel(specInfoGroupModels, appSpecInfo, seriesModel.parentColName);
                int groupIndex = ASIBaseUC.GetSpecInfoGroupIndex(specInfoGroupModels, parentSpecInfoModel);

                if (headFields.Contains(seriesModel.parentColName))
                {
                    if (!string.IsNullOrEmpty(sbSortControlID.ToString()))
                    {
                        sbSortControlID.Append(ACAConstant.SPLIT_CHAR);
                    }

                    int parentItemIndex = ASIBaseUC.GetSpecInfoIndex(specInfoGroupModels, parentSpecInfoModel);
                    sbSortControlID.Append(asiBaseUC.StructureDDLControlID(groupIndex, parentItemIndex)).Append(ACAConstant.SPLIT_CHAR18);
                }

                AppSpecificInfoModel4WS childSpecInfoModel = GetSpecInfoModel(specInfoGroupModels, appSpecInfo, seriesModel.childColName);
                int childItemIndex = ASIBaseUC.GetSpecInfoIndex(specInfoGroupModels, childSpecInfoModel);
                sbSortControlID.Append(asiBaseUC.StructureDDLControlID(groupIndex, childItemIndex)).Append(ACAConstant.SPLIT_CHAR18);
            }

            sortedControls = sbSortControlID.ToString();

            return sortedControls;
        }

        /// <summary>
        /// Sorts the series in drill down.
        /// </summary>
        /// <param name="drillDownModel">The drill down model.</param>
        /// <returns>
        /// return a drill down model with sorted series
        /// </returns>
        private ASITableDrillDownModel4WS SortSeriesInDrillDown(ASITableDrillDownModel4WS drillDownModel)
        {
            if (drillDownModel == null || drillDownModel.seriesModelList == null || drillDownModel.seriesModelList.Length <= 0)
            {
                return drillDownModel;
            }

            //orders series.
            List<ASITableDrillDSeriesModel4WS> orderedSeriesList = GetOrderedSeriesList(drillDownModel.seriesModelList);

            drillDownModel.seriesModelList = orderedSeriesList.ToArray();

            return drillDownModel;
        }

        /// <summary>
        /// Gets the ordered series list.
        /// </summary>
        /// <param name="asiSeriesModels">The ASI series models.</param>
        /// <returns>return a ordered series list</returns>
        private List<ASITableDrillDSeriesModel4WS> GetOrderedSeriesList(ASITableDrillDSeriesModel4WS[] asiSeriesModels)
        {
            Hashtable initHashTable = new Hashtable();
            List<string> keylist = new List<string>();

            List<string> headList = GetHeadFields(asiSeriesModels);

            StringBuilder sbKey = new StringBuilder();

            foreach (ASITableDrillDSeriesModel4WS seriesModel in asiSeriesModels)
            {
                if (seriesModel != null)
                {
                    sbKey.Remove(0, sbKey.Length);
                    sbKey.Append(seriesModel.parentColName);
                    sbKey.Append(ACAConstant.SPLIT_CHAR);
                    sbKey.Append(seriesModel.childColName);

                    initHashTable.Add(sbKey.ToString(), seriesModel);
                    keylist.Add(sbKey.ToString());
                }
            }

            Hashtable groupedSeries = GetGroupedSeries(keylist);

            List<string> fieldNameChains = GetAvailableFieldChain(headList, groupedSeries);

            // construct the ordered series list
            return LoadOrderedASIDrillDSeries(initHashTable, fieldNameChains);
        }

        /// <summary>
        /// Gets the group series hashtable.
        /// </summary>
        /// <param name="keylist">The key list.</param>
        /// <returns>
        /// Get the series hashtable which group by the header key
        /// </returns>
        private Hashtable GetGroupedSeries(List<string> keylist)
        {
            Hashtable groupHashTable = new Hashtable();
            List<string> childFieldNameList = new List<string>();

            foreach (string seriesKey in keylist)
            {
                childFieldNameList = new List<string>();
                string[] fieldNames = seriesKey.Split(ACAConstant.SPLIT_CHAR);
                string parentFieldName = fieldNames[0];

                if (!groupHashTable.ContainsKey(parentFieldName))
                {
                    foreach (string fieldName in keylist)
                    {
                        if (parentFieldName.Equals(fieldName.Split(ACAConstant.SPLIT_CHAR)[0]))
                        {
                            childFieldNameList.Add(fieldName.Split(ACAConstant.SPLIT_CHAR)[1]);
                        }
                    }

                    groupHashTable.Add(parentFieldName, childFieldNameList);
                }
            }

            return groupHashTable;
        }

        /// <summary>
        /// Loads the ordered ASI drill down series list.
        /// </summary>
        /// <param name="initHashTable">The initial hash table.</param>
        /// <param name="fieldNameChains">The field name chains.</param>
        /// <returns>
        /// load the ordered ASI Drill Down Series List
        /// </returns>
        private List<ASITableDrillDSeriesModel4WS> LoadOrderedASIDrillDSeries(Hashtable initHashTable, List<string> fieldNameChains)
        {
            List<ASITableDrillDSeriesModel4WS> orderSeriesList = new List<ASITableDrillDSeriesModel4WS>();

            if (fieldNameChains == null || fieldNameChains.Count <= 0)
            {
                return orderSeriesList;
            }

            foreach (string fieldNameChain in fieldNameChains)
            {
                if (string.IsNullOrEmpty(fieldNameChain))
                {
                    continue;
                }

                string[] fields = fieldNameChain.Split(ACAConstant.SPLIT_CHAR);

                for (int k = 0; k < fields.Length - 1; k++)
                {
                    StringBuilder sbKey = new StringBuilder();
                    sbKey.Append(fields[k]);
                    sbKey.Append(ACAConstant.SPLIT_CHAR);
                    sbKey.Append(fields[k + 1]);
                    orderSeriesList.Add((ASITableDrillDSeriesModel4WS)initHashTable[sbKey.ToString()]);
                }
            }

            return orderSeriesList;
        }

        /// <summary>
        /// Filters the available series path.
        /// </summary>
        /// <param name="headFieldNames">The head field names.</param>
        /// <param name="groupedSeries">The grouped series.</param>
        /// <returns>generate the available series paths</returns>
        private List<string> GetAvailableFieldChain(List<string> headFieldNames, Hashtable groupedSeries)
        {
            List<string> fieldChainList = new List<string>();
            Hashtable seriesRelationShips;

            foreach (string fieldName in headFieldNames)
            {
                seriesRelationShips = WrapHashTable(fieldName, groupedSeries);

                foreach (DictionaryEntry seriesRelation in seriesRelationShips)
                {
                    List<string> values = (List<string>)seriesRelation.Value;

                    if ((values == null || values.Count == 0) && !fieldChainList.Contains(seriesRelation.Key.ToString()))
                    {
                        fieldChainList.Add(seriesRelation.Key.ToString());
                    }
                }
            }

            return fieldChainList;
        }

        /// <summary>
        /// Wraps the hash table.
        /// </summary>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="groupedSeries">The grouped series.</param>
        /// <returns>load the all series path Hashtable</returns>
        private Hashtable WrapHashTable(string parentKey, Hashtable groupedSeries)
        {
            if (groupedSeries == null || groupedSeries.Count == 0)
            {
                return groupedSeries;
            }

            List<string> currentLevelList = (List<string>)groupedSeries[parentKey];

            if (currentLevelList == null || currentLevelList.Count == 0)
            {
                return groupedSeries;
            }

            Hashtable wrappedSeries = groupedSeries;
            List<string> valueList = new List<string>();

            bool isEndlessLoop = false;

            foreach (string key in currentLevelList)
            {
                // check endless loop
                string[] oldKeys = parentKey.Split(ACAConstant.SPLIT_CHAR);

                foreach (string oldKey in oldKeys)
                {
                    if (key.Equals(oldKey))
                    {
                        isEndlessLoop = true;
                    }
                }

                if (isEndlessLoop)
                {
                    return new Hashtable();
                }

                string newKey = parentKey + ACAConstant.SPLIT_CHAR + key;
                valueList = new List<string>();

                if (groupedSeries.ContainsKey(key) && !groupedSeries.ContainsKey(newKey))
                {
                    wrappedSeries.Add(newKey, groupedSeries[key]);
                    valueList = (List<string>)groupedSeries[key];
                }
                else if (!groupedSeries.ContainsKey(newKey))
                {
                    wrappedSeries.Add(newKey, null);
                    valueList = new List<string>();
                }

                if (valueList != null && valueList.Count != 0)
                {
                    WrapHashTable(newKey, wrappedSeries);
                }
            }

            return wrappedSeries;
        }

        /// <summary>
        /// Gets the head fields.
        /// </summary>
        /// <param name="seriesModels">The series models.</param>
        /// <returns>get all head fields in drill down</returns>
        private List<string> GetHeadFields(ASITableDrillDSeriesModel4WS[] seriesModels)
        {
            List<string> headFields = new List<string>();
            List<string> childFields = new List<string>();

            // generate series path link list
            foreach (ASITableDrillDSeriesModel4WS seriesModel in seriesModels)
            {
                if (seriesModel == null)
                {
                    continue;
                }

                if (!childFields.Contains(seriesModel.childColName))
                {
                    childFields.Add(seriesModel.childColName);
                }
            }

            foreach (ASITableDrillDSeriesModel4WS seriesModel in seriesModels)
            {
                if (seriesModel == null)
                {
                    continue;
                }

                if (seriesModel != null && !childFields.Contains(seriesModel.parentColName) && !headFields.Contains(seriesModel.parentColName))
                {
                    headFields.Add(seriesModel.parentColName);
                }
            }

            return headFields;
        }

        /// <summary>
        /// Gets the spec info group model.
        /// </summary>
        /// <param name="specInfoGroupModels">The spec info group models.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="subGroupName">Name of the sub group.</param>
        /// <returns>get the specific information group model which has the ASI field.</returns>
        private AppSpecificInfoGroupModel4WS GetSpecInfoGroupModel(AppSpecificInfoGroupModel4WS[] specInfoGroupModels, string groupName, string subGroupName)
        {
            AppSpecificInfoGroupModel4WS appSpecInfoGroup = new AppSpecificInfoGroupModel4WS();

            if (specInfoGroupModels == null || specInfoGroupModels.Length <= 0 || string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(subGroupName))
            {
                return appSpecInfoGroup;
            }

            foreach (AppSpecificInfoGroupModel4WS specInfoGroup in specInfoGroupModels)
            {
                if (groupName.Equals(specInfoGroup.groupCode, StringComparison.InvariantCulture)
                    && subGroupName.Equals(specInfoGroup.groupName, StringComparison.InvariantCulture))
                {
                    appSpecInfoGroup = specInfoGroup;
                    break;
                }
            }

            return appSpecInfoGroup;
        }

        /// <summary>
        /// Determines whether [is valid drill down] [the specified app spec info group model].
        /// </summary>
        /// <param name="appSpecInfoGroupModel">The app spec info group model.</param>
        /// <param name="drillDownModel">The drill down model.</param>
        /// <param name="moduleName">module name</param>
        /// <returns>
        /// <c>true</c> if [is valid drill down] [the specified app spec info group model]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidDrillDown(AppSpecificInfoGroupModel4WS appSpecInfoGroupModel, ASITableDrillDownModel4WS drillDownModel, string moduleName)
        {
            if (appSpecInfoGroupModel == null || appSpecInfoGroupModel.fields == null || appSpecInfoGroupModel.fields.Length <= 0
                || drillDownModel == null || drillDownModel.seriesModelList == null || drillDownModel.seriesModelList.Length <= 0)
            {
                return false;
            }

            bool isValidDrillD = true;
            ASITableDrillDSeriesModel4WS[] drillDSerieses = drillDownModel.seriesModelList;
            List<string> drillDownFields = new List<string>();

            // gets all ASI fields from drill down
            foreach (ASITableDrillDSeriesModel4WS seriesModel in drillDSerieses)
            {
                if (seriesModel == null)
                {
                    continue;
                }

                if (!drillDownFields.Contains(seriesModel.parentColName))
                {
                    drillDownFields.Add(seriesModel.parentColName);
                }

                if (!drillDownFields.Contains(seriesModel.childColName))
                {
                    drillDownFields.Add(seriesModel.childColName);
                }
            }

            for (int i = 0; i < drillDownFields.Count; i++)
            {
                string drillDownField = drillDownFields[i];
                bool isDisplayInACA = false;

                foreach (AppSpecificInfoModel4WS appSpecInfo in appSpecInfoGroupModel.fields)
                {
                    if (appSpecInfo == null)
                    {
                        continue;
                    }

                    string fieldName = Convert.ToString(appSpecInfo.checkboxDesc);

                    if (drillDownField.Equals(fieldName, StringComparison.InvariantCulture))
                    {
                        appSpecInfo.groupCode = string.IsNullOrEmpty(appSpecInfo.groupCode) ? appSpecInfoGroupModel.groupCode : appSpecInfo.groupCode;

                        string asiFieldSecurity = ASISecurityUtil.GetASISecurity(appSpecInfo, moduleName);

                        if (ACAConstant.ASISecurity.Read.Equals(asiFieldSecurity) || ACAConstant.ASISecurity.None.Equals(asiFieldSecurity))
                        {
                            isValidDrillD = false;
                            break;
                        }

                        //if the field in specific information model, it would display in ACA.
                        isDisplayInACA = true;

                        if (ValidationUtil.IsHidden(appSpecInfo.vchDispFlag) || (int)FieldType.HTML_SELECTBOX != int.Parse(appSpecInfo.fieldType))
                        {
                            isValidDrillD = false;
                            break;
                        }
                    }
                }

                //if the field in specific information model, it would display in ACA.
                if (!isDisplayInACA)
                {
                    isValidDrillD = false;
                    break;
                }
            }

            return isValidDrillD;
        }

        /// <summary>
        /// Sets the client value into ASI.
        /// </summary>
        /// <param name="specInfoGroup">The spec info group.</param>
        /// <param name="groupIndex">Index of the group.</param>
        /// <param name="drillDownModel">The drill down model.</param>
        /// <returns>specific information group model</returns>
        private AppSpecificInfoGroupModel4WS SetClientValueIntoASI(AppSpecificInfoGroupModel4WS specInfoGroup, int groupIndex, ASITableDrillDownModel4WS drillDownModel)
        {
            if (specInfoGroup == null || specInfoGroup.fields == null || specInfoGroup.fields.Length <= 0
                || drillDownModel == null || drillDownModel.seriesModelList == null || drillDownModel.seriesModelList.Length <= 0)
            {
                return specInfoGroup;
            }

            string[] controlClientIDs = System.Web.HttpContext.Current.Request.Form.AllKeys;

            for (int i = 0; i < specInfoGroup.fields.Length; i++)
            {
                if (specInfoGroup.fields[i] == null)
                {
                    continue;
                }

                ASIBaseUC asiBaseUC = new ASIBaseUC();
                string controlID = asiBaseUC.StructureDDLControlID(groupIndex, i);

                if (!IsDrillDownField(drillDownModel, specInfoGroup.fields[i].checkboxDesc))
                {
                    continue;
                }

                foreach (string controlClientID in controlClientIDs)
                {
                    if (controlClientID == null || controlClientID.Length < controlID.Length)
                    {
                        continue;
                    }

                    string tempID =
                        controlClientID.Substring(controlClientID.Length - controlID.Length, controlID.Length);

                    if (tempID.Equals(controlID, StringComparison.InvariantCulture))
                    {
                        specInfoGroup.fields[i].checklistComment =
                            System.Web.HttpContext.Current.Request.Form[controlClientID].ToString();
                        break;
                    }
                }
            }

            return specInfoGroup;
        }

        /// <summary>
        /// Gets the spec info model.
        /// </summary>
        /// <param name="specInfoGroupModels">The spec info group models.</param>
        /// <param name="specificInfoModel">The specific info model.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>an AppSpecificInfoModel4WS having the field(fieldName)</returns>
        private AppSpecificInfoModel4WS GetSpecInfoModel(AppSpecificInfoGroupModel4WS[] specInfoGroupModels, AppSpecificInfoModel4WS specificInfoModel, string fieldName)
        {
            AppSpecificInfoModel4WS specInfoModel = null;

            if (specInfoGroupModels == null || specificInfoModel == null || specificInfoModel.serviceProviderCode == null ||
                specificInfoModel.actStatus == null || specificInfoModel.checkboxType == null)
            {
                return specInfoModel;
            }

            foreach (AppSpecificInfoGroupModel4WS appSpecInfoGroup in specInfoGroupModels)
            {
                if (appSpecInfoGroup == null || appSpecInfoGroup.capID == null)
                {
                    continue;
                }

                if (!specificInfoModel.serviceProviderCode.Equals(appSpecInfoGroup.capID.serviceProviderCode, StringComparison.InvariantCulture)
                    || !specificInfoModel.actStatus.Equals(appSpecInfoGroup.groupCode, StringComparison.InvariantCulture)
                    || !specificInfoModel.checkboxType.Equals(appSpecInfoGroup.groupName, StringComparison.InvariantCulture))
                {
                    continue;
                }

                for (int i = 0; i < appSpecInfoGroup.fields.Length; i++)
                {
                    AppSpecificInfoModel4WS appSpecInfo = appSpecInfoGroup.fields[i];

                    if (appSpecInfo == null)
                    {
                        continue;
                    }

                    if (fieldName.Equals(appSpecInfo.checkboxDesc, StringComparison.InvariantCulture))
                    {
                        specInfoModel = appSpecInfo;
                        break;
                    }
                }
            }

            return specInfoModel;
        }

        /// <summary>
        /// check if parent field have a option that is selected by user or is default value.
        /// </summary>
        /// <param name="specificInfo">AppSpecificInfoModel4WS model.</param>
        /// <returns>true:select; false:no-select</returns>
        private bool HaveParentOptionSelected(AppSpecificInfoModel4WS specificInfo)
        {
            bool isSelected = false;

            if (specificInfo.valueList == null || specificInfo.valueList.Length <= 0 || string.IsNullOrEmpty(specificInfo.checklistComment))
            {
                return isSelected;
            }

            foreach (RefAppSpecInfoDropDownModel4WS option in specificInfo.valueList)
            {
                if (specificInfo.checklistComment.Equals(option.attrValue, StringComparison.InvariantCulture))
                {
                    isSelected = true;
                    break;
                }
            }

            return isSelected;
        }
    }
}