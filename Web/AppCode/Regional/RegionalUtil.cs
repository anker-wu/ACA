#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RegionalUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: The regional util
 *
 *  Notes:
 *      $Id: RegionalUtil.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Regional
{
    /// <summary>
    /// common function of the Regional
    /// </summary>
    public static class RegionalUtil
    {
        /// <summary>
        /// Gets the default country code.
        /// </summary>
        /// <returns>default country code</returns>
        public static string GetDefaultCountryCode()
        {
            string countryCode = string.Empty;
            
            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();
            RegionalModel regionalModel = regionalBll.GetRegionalModelByCountry(string.Empty, out states);

            if (regionalModel != null)
            {
                countryCode = regionalModel.countryCode;
            }

            return countryCode;
        }
    }
}