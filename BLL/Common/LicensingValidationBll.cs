#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicensingValidationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  This class deal with time zone conversion
 *
 *  Notes:
 * $Id: LicensingValidationBll.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to operation licensing validation.
    /// </summary>
    public class LicensingValidationBll : BaseBll, ILicensingValidationBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of LicensingValidationService.
        /// </summary>
        private LicensingValidationWebServiceService LicensingValidationService
        {
            get
            {
                return WSFactory.Instance.GetWebService<LicensingValidationWebServiceService>();
            }
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Check User License
        /// if return 0, means expired
        /// if return -1,means the add-on is not purchased
        /// if return positive number, means the purchased license count
        /// </summary>
        /// <returns>product license</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public int CheckProductLicense()
        {
            try
            {
                return LicensingValidationService.getLicensingUser(AgencyCode, "ACA");
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}