#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: XEntityPermissionBll.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: XEntityPermission operation class in Admin.
*
*  Notes:
* $Id: XEntityPermissionBll.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Oct 28, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// XEntityPermission operation class in Admin.
    /// </summary>
    public class XEntityPermissionBll : BaseBll, IXEntityPermissionBll
    {
        /// <summary>
        /// Gets an instance of XEntityPermissionService.
        /// </summary>
        private EntityPermissionWebServiceService XEntityPermissionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<EntityPermissionWebServiceService>();
            }
        }

        /// <summary>
        /// Get XEntityPermissionModel collection by given XEntityPermissionModel object.
        /// </summary>
        /// <param name="xEntityPermisson">The XEntityPermissionModel object.</param>
        /// <returns>XEntityPermissionModel collection.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public IEnumerable<XEntityPermissionModel> GetXEntityPermissions(XEntityPermissionModel xEntityPermisson)
        {
            try
            {
                XEntityPermissionModel[] xentityPermissions = XEntityPermissionService.getXEntityPermissions(xEntityPermisson);
                return xentityPermissions;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update x entity permissions.
        /// </summary>
        /// <param name="deletePK">A XEntityPermissionModel object be used as the PK to delete existing data.</param>
        /// <param name="xEntityPermissons">XEntityPermissionModel collection.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void UpdateXEntityPermissions(XEntityPermissionModel deletePK, IEnumerable<XEntityPermissionModel> xEntityPermissons)
        {
            try
            {
                XEntityPermissionService.updateXEntityPermissions(deletePK, xEntityPermissons.ToArray());

                //Remove cache when has XEntityPermission updated.
                if (deletePK != null && !string.IsNullOrEmpty(deletePK.servProvCode))
                {
                    ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
                    string cacheKey = deletePK.servProvCode +
                        ACAConstant.SPLIT_CHAR +
                        I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_ALL_XENTITY_PEMISSION);
                    cacheManager.Remove(cacheKey);
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
    }
}