#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapTypePermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CapTypePermissionBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Linq;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL
{
    /// <summary>
    /// This class provide the ability to operation CAP type Permission.
    /// </summary>
    public sealed class CapTypePermissionBll : BaseBll, ICapTypePermissionBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of CapTypePermissionService.
        /// </summary>
        private CapTypePermissionWebServiceService CapTypePermissionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapTypePermissionWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Cap Type Permission List. Filter Cache record by Cap Type information
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="capTypePermission">the capType Permission Model</param>
        /// <returns>the CapTypePermission list.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypePermissionModel[] GetCapTypePermissions(string agencyCode, CapTypePermissionModel capTypePermission)
        {
            try
            {
                ArrayList entityPermissionList = new ArrayList();
                CapTypePermissionModel[] capTypePermissions = GetPermissionByControllerType(agencyCode, capTypePermission);

                if (capTypePermissions != null && capTypePermissions.Length > 0)
                {
                    foreach (CapTypePermissionModel model in capTypePermissions)
                    {
                        if (model.group == capTypePermission.group && model.type == capTypePermission.type
                            && model.subType == capTypePermission.subType && model.category == capTypePermission.category
                            && model.entityType == capTypePermission.entityType)
                        {
                            entityPermissionList.Add(model);
                        }
                    }
                }

                return (CapTypePermissionModel[])entityPermissionList.ToArray(typeof(CapTypePermissionModel));
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Cap Type Permission List. It is used for cap home dropdown list.
        /// </summary>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="capTypePermission">the capType Permission model.</param>
        /// <returns>the Cap Type Permission model list.</returns>
        public CapTypePermissionModel[] GetPermissionByControllerType(string agencyCode, CapTypePermissionModel capTypePermission)
        {
            try
            {
                ArrayList entityPermissionList = new ArrayList();
                UserRolePrivilegeModel roles = new UserRolePrivilegeModel();
                ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
                Hashtable htCapTypeEntities = cacheManager.GetCachedItem(agencyCode, agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_ALL_CAPTYPE_ENTITIES));

                if (htCapTypeEntities.ContainsKey(capTypePermission.controllerType) && htCapTypeEntities[capTypePermission.controllerType] != null)
                {
                    CapTypePermissionModel[] capTypePermissions = (CapTypePermissionModel[])htCapTypeEntities[capTypePermission.controllerType];
                    foreach (CapTypePermissionModel model in capTypePermissions)
                    {
                        if (model.moduleName == capTypePermission.moduleName && model.controllerType == capTypePermission.controllerType)
                        {
                            if (string.IsNullOrEmpty(model.entityType) || model.entityType == capTypePermission.entityType)
                            {
                                entityPermissionList.Add(model);
                            }
                        }
                    }
                }

                return (CapTypePermissionModel[])entityPermissionList.ToArray(typeof(CapTypePermissionModel));
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Is document deletable by Application status
        /// </summary>
        /// <param name="capStatus">CAP status</param>
        /// <param name="buttonName">button name</param>
        /// <param name="capType">CapTypeModel object</param>
        /// <returns>true if the document is deletable;otherwise,false</returns>
        public bool IsDeletableDocumentByAppStatus(string capStatus, string buttonName, CapTypeModel capType)
        {
            bool isDeletale = CapTypePermissionService.isDeletableDocumentByAppStatus(AgencyCode, capStatus, buttonName, capType);
            
            return isDeletale;
        }

        /// <summary>
        /// Get Cap type filter name by app type.
        /// </summary>
        /// <param name="agencyCode">The agency code, pass sub-agency code if it is sub-agency.</param>
        /// <param name="capTypePermission">the cap type permission model</param>
        /// <returns>cap type filter name.</returns>
        public string GetCapTypeFilterByAppStatus(string agencyCode, CapTypePermissionModel capTypePermission)
        {
            ICapTypeFilterBll capTypFilterBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
            string filterName = capTypFilterBll.GetCapTypeFilterByLabelKey(agencyCode, capTypePermission.moduleName, "per_permitDetail_label_createAmendment");

            if (string.IsNullOrEmpty(filterName))
            {
                CapTypePermissionModel[] capTypePermissions = GetPermissionByControllerType(agencyCode, capTypePermission);

                if (capTypePermissions != null && capTypePermissions.Length > 0)
                {
                    foreach (CapTypePermissionModel model in capTypePermissions)
                    {
                        if (model.group == capTypePermission.group && model.type == capTypePermission.type
                            && model.subType == capTypePermission.subType && model.category == capTypePermission.category
                            && model.entityType == capTypePermission.entityType && model.entityKey2 == capTypePermission.entityKey2)
                        {
                            filterName = model.entityKey4;
                            break;
                        }
                    }
                }
            }

            //if the filter name is removed. should setting filter name to blank.
            if (!string.IsNullOrEmpty(filterName))
            {
                ICapTypeFilterBll capTypFiltereBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
                string[] filterNameList = capTypFiltereBll.GetCapTypeFilterListByModule(agencyCode, capTypePermission.moduleName, User.PublicUserId);

                if (filterNameList == null || filterNameList.Length < 1 || !filterNameList.Contains(filterName))
                {
                    filterName = string.Empty;
                }
            }

            return filterName;
        }

        #endregion Methods
    }
}
