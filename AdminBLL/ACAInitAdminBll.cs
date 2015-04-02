#region Header

/*
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ACAInitBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  Standard Choice manager interface.
 *
 *  Notes:
 * $Id$.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// ACA Initial interface
    /// </summary>
    public class ACAInitAdminBll : BaseBll, IACAInitBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ACAInitAdminBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of ACA Initial Service.
        /// </summary>
        private ACAInitWebServiceService ACAInitService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ACAInitWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initials the ACA.
        /// </summary>
        void IACAInitBll.InitACA()
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin ACAInitAdminBll.InitACA()");
            }

            try
            {
                ACAInitService.initACA(AgencyCode);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End ACAInitAdminBll.InitACA()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion
    }
}
