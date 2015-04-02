#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AppSpecificInfoBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AppSpecificInfoBll.cs 277225 2014-08-12 10:47:00Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// This class provide the ability to operation appSpecificInfo .
    /// </summary>
    public sealed class AppSpecificInfoBll : BaseBll, IAppSpecificInfoBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AppSpecificInfoBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of AppSpecificInfoService.
        /// </summary>
        private AppSpecificInfoWebServiceService AppSpecificInfoService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AppSpecificInfoWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// create app spec info table group
        /// </summary>
        /// <param name="appSpecificTableGroup">app specific table group</param>
        /// <param name="callerID">caller ID number</param>
        /// <param name="capID">cap ID number</param>
        /// <exception cref="DataValidateException">{ <c>appSpecificTableGroup or capID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void CreateAppSpecificTableGroup(AppSpecificTableGroupModel4WS appSpecificTableGroup, string callerID, CapIDModel4WS capID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AppSpecificInfoBll.CreateAppSpecificTableGroup()");
            }

            if (appSpecificTableGroup == null)
            {
                throw new DataValidateException(new string[] { "appSpecificTableGroup" });
            }

            if (capID == null)
            {
                throw new DataValidateException(new string[] { "capID" });
            }

            try
            {
                if (callerID != null)
                {
                    AppSpecificInfoService.createAppSpecificTableGroup(appSpecificTableGroup, callerID, capID);
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AppSpecificInfoBll.CreateAppSpecificTableGroup()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get App Spec Info table group by Cape ID.
        /// </summary>
        /// <param name="capID">cap ID number</param>
        /// <param name="callerID">caller ID number</param>
        /// <returns>ASIT Group information</returns>
        /// <exception cref="DataValidateException">{ <c>capID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public AppSpecificTableGroupModel4WS GetAppSpecificTableGroupModelByCapID(CapIDModel4WS capID, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AppSpecificInfoBll.getAppSpecificTableGroupModelByCapID()");
            }

            if (capID == null)
            {
                throw new DataValidateException(new string[] { "capID" });
            }

            try
            {
                AppSpecificTableGroupModel4WS response = AppSpecificInfoService.getAppSpecificTableGroupModelByCapID(capID, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AppSpecificInfoBll.getAppSpecificTableGroupModelByCapID()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get reference application specific information group with application specific field by CAP Type.
        /// </summary>
        /// <param name="capType4WS">cap type object.</param>
        /// <param name="callerID">caller id number.</param>
        /// <returns>Array of Reference ASI group information</returns>
        /// <exception cref="DataValidateException">{ <c>capType4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public Array GetRefAppSpecInfoListWithFiledsByCapType(CapTypeModel capType4WS, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AppSpecificInfoBll.getRefAppSpecInfoListWithFiledsByCapType()");
            }

            if (capType4WS == null)
            {
                throw new DataValidateException(new string[] { "capType4WS" });
            }

            try
            {
                Array response = AppSpecificInfoService.getRefAppSpecInfoListWithFiledsByCapType(capType4WS, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AppSpecificInfoBll.getRefAppSpecInfoListWithFiledsByCapType()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Returns relational searchable application specific info for ACA by Group/Type/SubType/Category of cap type.
        /// Returns all searchable application specific info for ACA if cap type hasn't any value in
        /// </summary>
        /// <param name="capType4WS">capType CapTypeModel Group/Type/SubType/Category can be null, but serviceProviderCode must has value.</param>
        /// <returns>Array of Reference ASI group information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        RefAppSpecInfoGroupModel4WS[] IAppSpecificInfoBll.GetRefSearchableAppSpecInfoFieldList(CapTypeModel capType4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AppSpecificInfoBll.GetRefAppSpecificInfoGroupList4ACASearch()");
            }

            try
            {
                RefAppSpecInfoGroupModel4WS[] response = AppSpecificInfoService.getRefAppSpecificInfoGroupList4ACASearch(capType4WS, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AppSpecificInfoBll.GetRefAppSpecificInfoGroupList4ACASearch()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        ///  Returns searchable application specific table group for ACA by Group/Type/SubType/Category of cap type.
        ///  Returns all searchable application specific info for ACA if cap type hasn't any value in
        /// </summary>
        /// <param name="capType">capType CapTypeModel Group/Type/SubType/Category can be null, but serviceProviderCode must has value.</param>
        /// <returns>Array of RefAppSpecInfoGroupModel4WS</returns>
        AppSpecificTableGroupModel4WS[] IAppSpecificInfoBll.GetSearchableAppSpecTableFieldList(CapTypeModel capType)
        {
            try
            {
                AppSpecificTableGroupModel4WS[] response = AppSpecificInfoService.getRefAppSpecificTableInfoList4ACASearch(capType, User.PublicUserId);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get the ASI group and sub group by agency code.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>ASI group and sub group</returns>
        GActivitySpecInfoGroupCodeModel[] IAppSpecificInfoBll.GetASIGroups(string agencyCode)
        {
            if (string.IsNullOrEmpty(agencyCode))
            {
                return null;
            }

            return AppSpecificInfoService.getASIGroups(agencyCode);
        }

        /// <summary>
        /// get the ASIT group and sub group by agency code.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>ASIT group and sub group</returns>
        GActivitySpecInfoGroupCodeModel[] IAppSpecificInfoBll.GetASITGroups(string agencyCode)
        {
            if (string.IsNullOrEmpty(agencyCode))
            {
                return null;
            }

            return AppSpecificInfoService.getASITGroups(agencyCode);
        }

        #region ASIDrillDown Members

        /// <summary>
        /// Get initial drill down information for a ASI group.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="groupName">ASI group name</param>
        /// <returns>drill down information list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        ASITableDrillDownModel4WS[] IAppSpecificInfoBll.GetASIDrillDown(string agencyCode, string groupName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AppSpecificInfoBll.GetASIDrillDown()");
            }

            try
            {
                ASITableDrillDownModel4WS[] asiTableDrillDownList = AppSpecificInfoService.getASIDrillDown(agencyCode, groupName, ACAConstant.GRANTED_RIGHT);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AppSpecificInfoBll.GetASIDrillDown()");
                }

                return asiTableDrillDownList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get next drill down info by user selected ASI value.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="groupName">ASI group name</param>
        /// <param name="subGroupName">ASI sub group name</param>
        /// <param name="seriesIds">series ids which have the next ASI fields</param>
        /// <param name="selectedValue">user selected value</param>
        /// <returns>drill down information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        ASITableDrillDownModel4WS IAppSpecificInfoBll.GetNextASIDrillDownData(string agencyCode, string groupName, string subGroupName, string[] seriesIds, string selectedValue)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AppSpecificInfoBll.GetNextASIDrillDownData()");
            }

            try
            {
                ASITableDrillDownModel4WS asiTableDrillDown = AppSpecificInfoService.getNextASIDrillDownData(agencyCode, groupName, subGroupName, seriesIds, selectedValue);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AppSpecificInfoBll.GetNextASIDrillDownData()");
                }

                return asiTableDrillDown;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// change ASI data source with drill down information.
        /// </summary>
        /// <param name="subASIGroupModel">model for sub ASI</param>
        /// <param name="drillDownModel">drill down model</param>
        /// <returns>return information for sub ASI</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        AppSpecificInfoGroupModel4WS IAppSpecificInfoBll.ChangeASIDataSourceByDrillD(AppSpecificInfoGroupModel4WS subASIGroupModel, ASITableDrillDownModel4WS drillDownModel)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AppSpecificInfoBll.ChangeASIDataSourceByDrillD()");
            }

            try
            {
                AppSpecificInfoGroupModel4WS specificInfoGroupModel = subASIGroupModel;

                if (specificInfoGroupModel == null || specificInfoGroupModel.fields == null || specificInfoGroupModel.fields.Length <= 0 || 
                    drillDownModel == null || drillDownModel.seriesModelList == null || drillDownModel.seriesModelList.Length <= 0)
                {
                    return specificInfoGroupModel;
                }

                //gets all head fields' name in drill down.
                List<string> headFields = GetHeadFields(drillDownModel.seriesModelList);

                //judges the parent field has default value/user selected value or not.
                bool hasSelectedValue = false;

                foreach (ASITableDrillDSeriesModel4WS seriesModel in drillDownModel.seriesModelList)
                {
                    if (seriesModel == null)
                    {
                        continue;
                    }

                    string parentColName = seriesModel.parentColName;

                    if (headFields.Contains(parentColName))
                    {
                        // handle the parent field
                        hasSelectedValue = true;
                        hasSelectedValue = ChangeASIDataSource(seriesModel, specificInfoGroupModel, hasSelectedValue, true);
                    }

                    // handle the child field 
                    hasSelectedValue = HandleChildField(seriesModel, drillDownModel, specificInfoGroupModel, hasSelectedValue);
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AppSpecificInfoBll.ChangeASIDataSourceByDrillD()");
                }

                return specificInfoGroupModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
        
        /// <summary>
        /// Handle the child field in a drill down series.
        /// </summary>
        /// <param name="seriesModel">series model</param>
        /// <param name="drillDownModel">drill down model</param>
        /// <param name="subASIGroupModel">model for sub ASI</param>
        /// <param name="hasSelectedValue">parent ASI field has default value/user selected value or not</param>
        /// <returns>current ASI field has default value/user selected value or not</returns>
        private bool HandleChildField(ASITableDrillDSeriesModel4WS seriesModel, ASITableDrillDownModel4WS drillDownModel, AppSpecificInfoGroupModel4WS subASIGroupModel, bool hasSelectedValue)
        {
            string parentColumnName = string.Empty;
            AppSpecificInfoModel4WS parentASI = null;

            //get the parent ASI model, if it has default value or user selected value,
            //we need get child ASI options by the value.
            ASITableDrillDSeriesModel4WS childSeries = new ASITableDrillDSeriesModel4WS();

            //the parent model need keep initial value, so clone a object.
            childSeries = (ASITableDrillDSeriesModel4WS)ObjectCloneUtil.CloneSimpleObject(seriesModel, childSeries);
            childSeries.asiTableDrillDValMapModel4WSList = null;

            if (hasSelectedValue)
            {
                //get parent ASI.
                foreach (AppSpecificInfoModel4WS specificInfoModel in subASIGroupModel.fields)
                {
                    parentColumnName = specificInfoModel != null ? specificInfoModel.checkboxDesc : string.Empty;

                    if (parentColumnName.Equals(seriesModel.parentColName))
                    {
                        parentASI = specificInfoModel;
                        break;
                    }
                }

                //get drill down by parent ASI.
                if (parentASI != null && !string.IsNullOrEmpty(parentASI.checklistComment))
                {
                    ASITableDrillDownModel4WS childDrillDownModel =
                    AppSpecificInfoService.getNextASIDrillDownData(
                    drillDownModel.servProvCode, 
                    drillDownModel.entityKey1, 
                    drillDownModel.entityKey2, 
                    new string[] { seriesModel.seriesId }, 
                    parentASI.checklistComment);

                    if (childDrillDownModel != null && childDrillDownModel.seriesModelList != null
                        && childDrillDownModel.seriesModelList.Length > 0 && childDrillDownModel.seriesModelList[0] != null)
                    {
                        childSeries = childDrillDownModel.seriesModelList[0];
                    }
                }
            }

            return ChangeASIDataSource(childSeries, subASIGroupModel, hasSelectedValue, false);
        }

        /// <summary>
        /// Changes the ASI data source.
        /// </summary>
        /// <param name="seriesModel">The series model.</param>
        /// <param name="subASIGroupModel">The sub ASI group model.</param>
        /// <param name="hasSelectedValue">if set to <c>true</c> [has selected value].</param>
        /// <param name="isParentField">if set to <c>true</c> [is parent field].</param>
        /// <returns>current ASI field has default value/user selected value or not</returns>
        private bool ChangeASIDataSource(ASITableDrillDSeriesModel4WS seriesModel, AppSpecificInfoGroupModel4WS subASIGroupModel, bool hasSelectedValue, bool isParentField)
        {
            //it is the ASI field name from drill down.
            string drillDColumnName = isParentField ? seriesModel.parentColName : seriesModel.childColName;

            //the ASI has a default value or user selected value ,it is true, else is false;
            bool valueIsSelect = hasSelectedValue;

            foreach (AppSpecificInfoModel4WS specificInfoModel in subASIGroupModel.fields)
            {
                //It is ASI field name.
                string columnName = specificInfoModel.checkboxDesc;

                if (drillDColumnName.Equals(columnName))
                {
                    if (!valueIsSelect)
                    {
                        specificInfoModel.valueList = null;
                    }
                    else
                    {
                        specificInfoModel.valueList = CreateASIValuesByDrillD(seriesModel.asiTableDrillDValMapModel4WSList, specificInfoModel);

                        if (!HaveParentOptionSelected(specificInfoModel))
                        {
                            valueIsSelect = false;
                        }
                    }

                    break;
                }
            }

            return valueIsSelect;
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

        /// <summary>
        /// Creates the ASI values by drill Down information.
        /// </summary>
        /// <param name="drillDValMapModel">The drill Down value map model.</param>
        /// <param name="specificInfoModel">The specific info model.</param>
        /// <returns>Application specification information drop down model list</returns>
        private RefAppSpecInfoDropDownModel4WS[] CreateASIValuesByDrillD(ASITableDrillDValMapModel4WS[] drillDValMapModel, AppSpecificInfoModel4WS specificInfoModel)
        {
            if (drillDValMapModel == null || drillDValMapModel.Length <= 0)
            {
                return null;
            }

            ArrayList specInfoValueList = new ArrayList();

            for (int i = 0; i < drillDValMapModel.Length; i++)
            {
                if (drillDValMapModel[i] == null)
                {
                    continue;
                }

                RefAppSpecInfoDropDownModel4WS newDropDownValueModel = new RefAppSpecInfoDropDownModel4WS();
                newDropDownValueModel.attrValue = drillDValMapModel[i].childValueName;
                newDropDownValueModel.auditID = drillDValMapModel[i].recFulNam;
                newDropDownValueModel.auditStatus = drillDValMapModel[i].recStatus;
                newDropDownValueModel.checkboxGroup = ACAConstant.ASI_CHECKBOX_GROUP;
                newDropDownValueModel.fieldLabel = specificInfoModel.fieldLabel;
                newDropDownValueModel.groupCode = specificInfoModel.groupCode;
                newDropDownValueModel.groupName = specificInfoModel.checkboxType;
                string resValue = string.IsNullOrEmpty(drillDValMapModel[i].resChildValueName) ? drillDValMapModel[i].childValueName : drillDValMapModel[i].resChildValueName;
                newDropDownValueModel.resAttrValue = resValue;
                newDropDownValueModel.serviceProviderCode = drillDValMapModel[i].servProvCode;
                specInfoValueList.Add(newDropDownValueModel);
            }

            return (RefAppSpecInfoDropDownModel4WS[])specInfoValueList.ToArray(typeof(RefAppSpecInfoDropDownModel4WS));
        }

        /// <summary>
        /// Gets the drill down head fields.
        /// </summary>
        /// <param name="seriesModels">The series models.</param>
        /// <returns>a string list for drill down head fields</returns>
        private List<string> GetHeadFields(ASITableDrillDSeriesModel4WS[] seriesModels)
        {
            List<string> headFields = new List<string>();
            List<string> childFields = new List<string>();

            // get all child fields 
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

            //if the field doesn't belong child fields, it is head field.
            foreach (ASITableDrillDSeriesModel4WS seriesModel in seriesModels)
            {
                if (seriesModel == null)
                {
                    continue;
                }

                if (!childFields.Contains(seriesModel.parentColName) && !headFields.Contains(seriesModel.parentColName))
                {
                    headFields.Add(seriesModel.parentColName);
                }
            }

            return headFields;
        }
 
        #endregion 

        #endregion Methods
    }
}