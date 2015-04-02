#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionTypePermission.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: InspectionTypePermission.cs 178385 2010-08-06 07:47:06Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/09/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// This class provide the ability to operation inspection type permission.
    /// </summary>
    public class InspectionTypePermissionBll : BaseBll, IInspectionTypePermissionBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of InspectionTypeService.
        /// </summary>
        private InspectionTypePermissionWebServiceService InspectionTypePermissionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<InspectionTypePermissionWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Save action permission settings
        /// </summary>
        /// <param name="insActionPermission">array for inspection action permission.</param>
        /// <returns>save successful?</returns>
        public bool SaveActionPermissionSettings(InspectionActionPermissionModel[] insActionPermission)
        {
            return InspectionTypePermissionService.saveActionPermissionSettings(insActionPermission);
        }

        /// <summary>
        /// Get inspection action permissions.
        /// </summary>
        /// <param name="recordStatusGroupCode">inspection status group code</param>
        /// <param name="recordStatus">inspection status.</param>
        /// <param name="inspectionGroupCode">inspection group code</param>
        /// <returns>array for inspection action permission.</returns>
        public IList<InspectionActionPermissionModel> GetInspectionActionPermissions(string recordStatusGroupCode, string recordStatus, string inspectionGroupCode)
        {
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            Hashtable htInsActionPermission = cacheManager.GetCachedItem(AgencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_INSPECTION_ACTION_PERMISSION));
            string key = string.Format("{0}{1}{2}{3}{4}", recordStatusGroupCode, ACAConstant.SPLIT_CHAR5, recordStatus, ACAConstant.SPLIT_CHAR5, inspectionGroupCode);

            if (htInsActionPermission[key] != null)
            {
                return htInsActionPermission[key] as IList<InspectionActionPermissionModel>;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get inspection schedule type.
        /// </summary>
        /// <param name="inspectionTypeModel">the inspectionTypeModel</param>
        /// <returns>Return the inspection schedule type.</returns>
        public InspectionScheduleType GetScheduleType(InspectionTypeModel inspectionTypeModel)
        {            
            return InspectionScheduleTypeUtil.GetScheduleType(inspectionTypeModel);
        }

        #endregion Methods
    }
}
