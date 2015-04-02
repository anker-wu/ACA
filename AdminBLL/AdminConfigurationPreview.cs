#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminConfigurationPreview.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: AdminConfigurationPreview.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web;

using Accela.ACA.BLL.Admin;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is configuration preview in ACA admin.
    /// </summary>
    public class AdminConfigurationPreview : BaseBll, IAdminConfigurationPreview
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly string ServerPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath);

        #endregion Fields

        #region Methods

        /// <summary>
        /// Set CapModel dummy data for admin page configuration preview
        /// </summary>
        public void SetPreviewCapModelDummyData()
        {
            CapModel4WS tempObject = ObjectXMLSerializer<CapModel4WS>.Load(ServerPath + ACAConstant.ADMIN_PREVIEW_CAPMODEL_DUMMY_DATA);
            tempObject.capID.serviceProviderCode = AgencyCode;
            HttpContext.Current.Session.Add(SessionConstant.SESSION_CAP_MODEL_ADMIN, tempObject);
        }

        /// <summary>
        /// Set admin configuration temp data into session for preview
        /// </summary>
        public void SetPreviewConfigurationTempData()
        {
        }

        #endregion Methods
    }
}