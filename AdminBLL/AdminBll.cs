#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminBLL.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AdminBll.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is provided to handle ACA admin
    /// </summary>
    public class AdminBll : BaseBll, IAdminBll
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(IAdminBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of AppStatusGroupService.
        /// </summary>
        /// <value>The application status group service.</value>
        private AppStatusGroupWebServiceService AppStatusGroupService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AppStatusGroupWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of AcaAdminTreeService.
        /// </summary>
        /// <value>The aca admin tree service.</value>
        private AcaAdminTreeWebServiceService AcaAdminTreeService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AcaAdminTreeWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of GFilterViewService.
        /// </summary>
        /// <value>The aggregate filter view service.</value>
        private GFilterViewWebServiceService GFilterViewService
        {
            get
            {
                return WSFactory.Instance.GetWebService<GFilterViewWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Method to save application status model.
        /// </summary>
        /// <param name="appStatusGroupModel4WS">AppStatusGroupModel4WS array</param>
        /// <exception cref="DataValidateException">new string[] { appStatusGroupModel4WS }</exception>
        /// <exception cref="ACAException"></exception>
        public void EditAppStatusGroup(AppStatusGroupModel4WS[] appStatusGroupModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.EditAppStatusGroup()");
            }

            if (appStatusGroupModel4WS == null)
            {
                throw new DataValidateException(new string[] { "appStatusGroupModel4WS" });
            }

            try
            {
                AppStatusGroupService.editAppStatusGroup(appStatusGroupModel4WS);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.EditAppStatusGroup()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to save sub tree node model
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="wsModelList">tree model</param>
        /// <exception cref="DataValidateException">
        /// <paramref name="wsModelList"/> is null
        /// </exception>
        /// <exception cref="ACAException">Exception from web service</exception>
        public void EditSubTreeNode(string servProvCode, AcaAdminTreeNodeModel4WS[] wsModelList)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.EditSubTreeNode()");
            }

            if (wsModelList == null)
            {
                throw new DataValidateException(new string[] { "wsModelList" });
            }

            if (string.IsNullOrEmpty(servProvCode))
            {
                throw new DataValidateException(new string[] { servProvCode });
            }

            try
            {
                AcaAdminTreeService.editSubTreeNode(servProvCode, wsModelList);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.EditSubTreeNode()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get all ACA page list which is predefined in aca admin tree.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>AcaAdminTreeNodeModel4WS array.</returns>
        /// <exception cref="ACAException">Exception from web service</exception>
        public AcaAdminTreeNodeModel4WS[] GetACAPageList(string agencyCode)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.GetACAPageList()");
            }

            try
            {
                AcaAdminTreeNodeModel4WS[] treeNodeModel = AcaAdminTreeService.getACAPageList(agencyCode, ACAConstant.ADMIN_CALLER_ID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.GetACAPageList()");
                }

                return treeNodeModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get application status data table.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <returns>Data Table</returns>
        /// <exception cref="DataValidateException">Agency code or module name is null</exception>
        /// <exception cref="ACAException">Exception from web service</exception>
        public DataTable GetAppStatusGroup(string agencyCode, string moduleName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.GetAppStatusGroup()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(moduleName))
            {
                throw new DataValidateException(new string[] { agencyCode, moduleName });
            }

            try
            {
                AppStatusGroupModel4WS[] treeNodeModel = AppStatusGroupService.getAppStatusGroupBySPC(agencyCode, moduleName);

                DataTable dt = BuildAppStatusGroup2DataTable(treeNodeModel);

                if (dt == null)
                {
                    return null;
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.GetAppStatusGroup()");
                }

                return dt;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get application status model.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="moduleName">module name</param>
        /// <returns>AppStatusGroupModel4WS array</returns>
        /// <exception cref="DataValidateException">agency code or module name null</exception>
        /// <exception cref="ACAException">Exception from web service</exception>
        public AppStatusGroupModel4WS[] GetAppStatusGroupBySPC(string servProvCode, string moduleName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.GetAppStatusGroupBySPC()");
            }

            if (string.IsNullOrEmpty(servProvCode) || string.IsNullOrEmpty(moduleName))
            {
                throw new DataValidateException(new string[] { servProvCode, moduleName });
            }

            try
            {
                AppStatusGroupModel4WS[] treeNodeModel = AppStatusGroupService.getAppStatusGroupBySPC(servProvCode, moduleName);

                if (treeNodeModel == null)
                {
                    return null;
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.GetSubTreeNode()");
                }

                return treeNodeModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get page id by url
        /// </summary>
        /// <param name="url">string page url</param>
        /// <returns>string page id</returns>
        public string GetPageIDbyUrl(string url)
        {
            string pageID = string.Empty;
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            Hashtable htPages = cacheManager.GetCachedItem(this.AgencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_ACAPAGES));

            // Find the corresponding page id by matching the request url.
            if (htPages != null && htPages.Count > 0 && !string.IsNullOrEmpty(url))
            {
                // value-url, key-page id
                foreach (DictionaryEntry entry in htPages)
                {
                    if (url.Equals(Convert.ToString(entry.Value), StringComparison.InvariantCultureIgnoreCase))
                    {
                        pageID = entry.Key.ToString();

                        break;
                    }
                }
            }

            return pageID;
        }

        /// <summary>
        /// Get SimpleViewMode
        /// </summary>
        /// <param name="servProvCode">The agency code</param>
        /// <param name="levelType">The level type</param>
        /// <param name="moduleName">The model name</param>
        /// <param name="viewID">The view ID</param>
        /// <param name="screenPermission">The GFilterScreenPermissionModel</param>
        /// <param name="callerid">The caller ID</param>
        /// <returns>The SimpleViewModel</returns>
        /// <exception cref="ACAException">Exception from web service</exception>
        public SimpleViewModel4WS GetSimpleViewMode(string servProvCode, string levelType, string moduleName, string viewID, GFilterScreenPermissionModel4WS screenPermission, string callerid)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.GetSimpleViewMode()");
            }

            try
            {
                SimpleViewModel4WS simpleViewModel = GFilterViewService.getFilterScreenView(servProvCode, levelType, moduleName, viewID, screenPermission, callerid);
                List<string> searchSections = GetSearchSections();
                if (searchSections.Contains(viewID))
                {
                    FilterRequiredFields(simpleViewModel);
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.GetSimpleViewMode()");
                }

                return simpleViewModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Save SimpleViewMode
        /// </summary>
        /// <param name="servProvCode">The agency code</param>
        /// <param name="levelType">The level type</param>
        /// <param name="moduleName">The model name</param>
        /// <param name="simpleViewMode">The SimpleViewModel</param>
        /// <param name="callerid">The caller ID</param>
        /// <exception cref="ACAException">Exception from web service</exception>
        public void SaveSimpleViewModel(string servProvCode, string levelType, string moduleName, SimpleViewModel4WS simpleViewMode, string callerid)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.GetSimpleViewMode()");
            }

            try
            {
                GFilterViewService.saveFilterScreenView(servProvCode, levelType, moduleName, simpleViewMode, callerid);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.GetSimpleViewMode()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get sub tree node data table.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="callerID">string caller id</param>
        /// <returns>Data Table</returns>
        /// <exception cref="DataValidateException">agency code or caller id null</exception>
        /// <exception cref="ACAException">Exception from web service</exception>
        public DataTable GetSubTreeNode(string agencyCode, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.GetSubTreeNode()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { agencyCode, callerID });
            }

            try
            {
                AcaAdminTreeNodeModel4WS[] treeNodeModel = AcaAdminTreeService.getSubTreeNode(agencyCode, callerID);

                DataTable dt = BuildFeature2DataTable(treeNodeModel);

                if (dt == null)
                {
                    return null;
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.GetSubTreeNode()");
                }

                return dt;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get sub tree nodes
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="callerID">string caller id</param>
        /// <returns>AcaAdminTreeNodeModel4WS array</returns>
        /// <exception cref="DataValidateException">agency code or caller id null</exception>
        /// <exception cref="ACAException">Exception from web service</exception>
        public AcaAdminTreeNodeModel4WS[] GetSubTreeNodeModel(string agencyCode, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBll.GetSubTreeNode()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { agencyCode, callerID });
            }

            try
            {
                AcaAdminTreeNodeModel4WS[] treeNodeModel = AcaAdminTreeService.getSubTreeNode(agencyCode, callerID);

                if (treeNodeModel == null)
                {
                    return null;
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBll.GetSubTreeNode()");
                }

                return treeNodeModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Admin Left Tree Nodes
        /// </summary>
        /// <returns>tree Data Table</returns>
        /// <exception cref="ACAException">Exception from web service</exception>
        public DataTable GetTreeNodes()
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin Admin.GetTreeNodes()");
            }

            try
            {
                string defaultLanguage = I18nCultureUtil.GetLanguageCode(I18nCultureUtil.DefaultCulture);
                string defaultCountry = I18nCultureUtil.GetRegionalCode(I18nCultureUtil.DefaultCulture);
                AcaAdminTreeNodeModel4WS[] tnWS = AcaAdminTreeService.getNodeList(AgencyCode, defaultLanguage, defaultCountry, "ACA Admin");

                DataTable returnValue = BuildTreeNodes2DataTable(tnWS);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End Admin.GetTreeNodes()");
                }

                return returnValue;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Search form.
        /// </summary>
        /// <returns>return search form array which need filter required flag.</returns>
        private List<string> GetSearchSections()
        {
            List<string> list = new List<string> { "60007", "60010", "60011", "60012", "60013" };
            return list;
        }

        /// <summary>
        /// Build data table for application status group
        /// </summary>
        /// <param name="treeNodes">AppStatusGroupModel4WS array</param>
        /// <returns>Data Table</returns>
        private DataTable BuildAppStatusGroup2DataTable(AppStatusGroupModel4WS[] treeNodes)
        {
            DataTable table = new DataTable();

            table.Columns.Add("GroupName");
            table.Columns.Add("ItemName");
            table.Columns.Add("resItemName");
            table.Columns.Add("Checked");
            table.Columns.Add("Disabled");
            table.Columns.Add("DisplayRequestTradeLicense");

            if (treeNodes == null || treeNodes.Length < 1)
            {
                return table;
            }

            List<AppStatusGroupModel4WS> treeModelArray = new List<AppStatusGroupModel4WS>();

            foreach (AppStatusGroupModel4WS treeNode in treeNodes)
            {
                treeModelArray.Add(treeNode);
            }

            treeModelArray.Sort(delegate(AppStatusGroupModel4WS item1, AppStatusGroupModel4WS item2) { return item1.appStatusGroupCode.CompareTo(item2.appStatusGroupCode); });

            string groupName = string.Empty;
            string oldGroupName = string.Empty;
            DataRow dr = null;

            foreach (AppStatusGroupModel4WS treeNode in treeModelArray.ToArray())
            {
                groupName = treeNode.appStatusGroupCode;
                if (groupName != oldGroupName)
                {
                    dr = table.NewRow();
                    dr[0] = string.Empty;
                    dr[1] = treeNode.appStatusGroupCode;
                    dr[2] = treeNode.appStatusGroupCode;
                    dr[3] = string.Empty;
                    dr[4] = string.Empty;
                    dr[5] = string.Empty;
                    table.Rows.Add(dr);

                    dr = table.NewRow();
                    dr[0] = treeNode.appStatusGroupCode;
                    dr[1] = treeNode.status;
                    dr[2] = I18nStringUtil.GetString(treeNode.resStatus, treeNode.status);
                    dr[3] = treeNode.acaStatus;
                    dr[4] = treeNode.auditStatus;
                    dr[5] = treeNode.displayRequestTradeLic;
                    table.Rows.Add(dr);

                    oldGroupName = groupName;
                }
                else
                {
                    dr = table.NewRow();
                    dr[0] = treeNode.appStatusGroupCode;
                    dr[1] = treeNode.status;
                    dr[2] = I18nStringUtil.GetString(treeNode.resStatus, treeNode.status);
                    dr[3] = treeNode.acaStatus;
                    dr[4] = treeNode.auditStatus;
                    dr[5] = treeNode.displayRequestTradeLic;
                    table.Rows.Add(dr);
                }
            }

            return table;
        }

        /// <summary>
        /// Build a data table for feature
        /// </summary>
        /// <param name="treeNodes">AcaAdminTreeNodeModel4WS array</param>
        /// <returns>Data Table</returns>
        private DataTable BuildFeature2DataTable(AcaAdminTreeNodeModel4WS[] treeNodes)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ActionURL");
            table.Columns.Add("DisplayFlag");
            table.Columns.Add("DisplayOrder");
            table.Columns.Add("ElementID");
            table.Columns.Add("ElementName");
            table.Columns.Add("IsSelected");
            table.Columns.Add("NodeDescribe");
            table.Columns.Add("ParentID");
            table.Columns.Add("RecDate", typeof(DateTime));
            table.Columns.Add("RecFul_Nam");
            table.Columns.Add("RecStatus");
            table.Columns.Add("ServProvCode");
            table.Columns.Add("isUsedDaily");
            table.Columns.Add("PageType");
            table.Columns.Add("RootNodeName");
            table.Columns.Add("LabelKey");
            table.Columns.Add("NewElementName");
            table.Columns.Add("ForceLogin");
            table.Columns.Add("SingleServiceOnly");

            if (treeNodes == null || treeNodes.Length < 1)
            {
                return table;
            }

            foreach (AcaAdminTreeNodeModel4WS treeNode in treeNodes)
            {
                DataRow dr = table.NewRow();

                dr[0] = treeNode.actionURL;
                dr[1] = treeNode.displayFlag;
                dr[2] = treeNode.displayOrder;
                dr[3] = treeNode.elementID;
                dr[4] = treeNode.elementName;
                dr[5] = treeNode.isSelected;
                dr[6] = treeNode.nodeDescribe;
                dr[7] = treeNode.parentID;
                dr[8] = I18nDateTimeUtil.ParseFromWebService4DataTable(treeNode.recDate);
                dr[9] = treeNode.recFul_Nam;
                dr[10] = treeNode.recStatus;
                dr[11] = treeNode.servProvCode;
                dr[12] = treeNode.isUsedDaily;
                dr[13] = treeNode.pageType;
                dr[14] = treeNode.rootNodeName;
                dr[15] = treeNode.labelKey;
                dr[16] = treeNode.elementName;
                dr[17] = treeNode.forceLogin;
                dr[18] = treeNode.singleServiceOnly;

                table.Rows.Add(dr);
            }

            return table;
        }

        /// <summary>
        /// Build tree nodes data table
        /// </summary>
        /// <param name="treeNodes">AcaAdminTreeNodeModel4WS array</param>
        /// <returns>Data Table</returns>
        private DataTable BuildTreeNodes2DataTable(AcaAdminTreeNodeModel4WS[] treeNodes)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ActionURL");
            table.Columns.Add("DisplayFlag");
            table.Columns.Add("DisplayOrder");
            table.Columns.Add("ElementID");
            table.Columns.Add("ElementName");
            table.Columns.Add("IsSelected");
            table.Columns.Add("NodeDescribe");
            table.Columns.Add("ParentID");
            table.Columns.Add("RecDate", typeof(DateTime));
            table.Columns.Add("RecFul_Nam");
            table.Columns.Add("RecStatus");
            table.Columns.Add("ServProvCode");
            table.Columns.Add("isUsedDaily");
            table.Columns.Add("PageType");
            table.Columns.Add("RootNodeName");

            if (treeNodes == null || treeNodes.Length < 1)
            {
                return table;
            }

            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string superAgency = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_MULTI_SERVICE_SETTINGS, BizDomainConstant.STD_ITEM_IS_SUPER_AGENCY);
            bool isSuperAgency = ValidationUtil.IsYes(superAgency);

            foreach (AcaAdminTreeNodeModel4WS treeNode in treeNodes)
            {
                if (treeNode.elementID == ACAConstant.ADMIN_TREENODE_HOSTEDAGENCYSETTINGS && !isSuperAgency)
                {
                    continue;
                }

                DataRow dr = table.NewRow();

                dr[0] = treeNode.actionURL;
                dr[1] = treeNode.displayFlag;
                dr[2] = treeNode.displayOrder;
                dr[3] = treeNode.elementID;
                dr[4] = ScriptFilter.RemoveHTMLTag(treeNode.elementName);
                dr[5] = treeNode.isSelected;
                dr[6] = treeNode.nodeDescribe;
                dr[7] = treeNode.parentID;
                dr[8] = I18nDateTimeUtil.ParseFromWebService4DataTable(treeNode.recDate);
                dr[9] = treeNode.recFul_Nam;
                dr[10] = treeNode.recStatus;
                dr[11] = treeNode.servProvCode;
                dr[12] = treeNode.isUsedDaily;
                dr[13] = treeNode.pageType;
                dr[14] = treeNode.rootNodeName;

                table.Rows.Add(dr);
            }

            return table;
        }

        /// <summary>
        /// Filter Required filed flag in search form.
        /// </summary>
        /// <param name="simpleViewModel">generic view object</param>
        private void FilterRequiredFields(SimpleViewModel4WS simpleViewModel)
        {
            foreach (SimpleViewElementModel4WS item in simpleViewModel.simpleViewElements)
            {
                if (ACAConstant.COMMON_N.Equals(item.standard))
                {
                    item.required = ACAConstant.COMMON_N;
                }
            }
        }

        #endregion Methods
    }
}