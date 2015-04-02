#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IRegionalBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IRegionalBll.cs 197931 2011-06-22 05:56:38Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Class regional business.
    /// </summary>
    public class RegionalBll : IRegionalBll
    {
        /// <summary>
        /// Gets an instance of RegionalWebService.
        /// </summary>
        private RegionalWebServiceService RegionalService
        {
            get
            {
                return WSFactory.Instance.GetWebService<RegionalWebServiceService>();
            }
        }

        /// <summary>
        /// Gets all regional.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <returns>regional setting object array</returns>
        public RegionalModel[] GetAllRegional(string servProvCode)
        {
            return RegionalService.getAllRegional(servProvCode);
        }

        /// <summary>
        /// Gets the states config.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="stateSTDList">The state STD list.</param>
        /// <returns>standard choices</returns>
        public BizDomainModel[] GetStatesConfig(string servProvCode, List<string> stateSTDList)
        {
            return RegionalService.getStatesConfig(servProvCode, stateSTDList.ToArray());
        }

        /// <summary>
        /// Gets the regional model by country.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="states">The states.</param>
        /// <returns>regional setting object</returns>
        public RegionalModel GetRegionalModelByCountry(string countryCode, out Dictionary<string, string> states)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            return cacheManager.GetRegionalModelByCountry(countryCode, out states);
        }
    }
}
