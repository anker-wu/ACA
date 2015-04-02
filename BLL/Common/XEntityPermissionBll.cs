#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: XEntityPermissionBll.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: XEntityPermission operation class.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// XEntityPermission operation class.
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
        public IEnumerable<XEntityPermissionModel> GetXEntityPermissions(XEntityPermissionModel xEntityPermisson)
        {
            try
            {
                string agencyCode = xEntityPermisson == null ? null : xEntityPermisson.servProvCode;

                if (string.IsNullOrEmpty(agencyCode))
                {
                    agencyCode = AgencyCode;
                }

                ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
                string cacheKey = agencyCode +
                    ACAConstant.SPLIT_CHAR +
                    I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_ALL_XENTITY_PEMISSION);

                Hashtable htXEntities = cacheManager.GetCachedItem(agencyCode, cacheKey);
                IEnumerable<XEntityPermissionModel> xEntities = htXEntities[string.Empty] as XEntityPermissionModel[];

                if (xEntities != null && xEntityPermisson != null)
                {
                    var query = xEntities.AsQueryable();
                    var properties = xEntityPermisson.GetType().GetProperties();

                    //Create dynamic lambda expression IQueryable.Where(e => e.PropX == ValueX).
                    ParameterExpression paramExp = Expression.Parameter(typeof(XEntityPermissionModel), "e");

                    foreach (var pt in properties)
                    {
                        object value = pt.GetValue(xEntityPermisson, null);
                        object defValue = pt.PropertyType.IsValueType ? Activator.CreateInstance(pt.PropertyType) : null;

                        if (value != null && !value.Equals(defValue))
                        {
                            var expLeft = Expression.Property(paramExp, pt.Name);
                            var expRight = Expression.Constant(value);
                            var expBody = Expression.Equal(expLeft, expRight);
                            var lambdaExp = Expression.Lambda(expBody, paramExp);

                            var callExp = Expression.Call(typeof(Queryable), "Where", new[] { typeof(XEntityPermissionModel) }, Expression.Constant(query), lambdaExp);
                            query = query.Provider.CreateQuery<XEntityPermissionModel>(callExp);
                        }
                    }

                    xEntities = query;
                }

                return xEntities;
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