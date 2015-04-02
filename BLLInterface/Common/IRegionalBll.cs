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

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Regional Business 
    /// </summary>
    public interface IRegionalBll
    {
        /// <summary>
        /// Gets all regional.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <returns>regional setting object array</returns>
        RegionalModel[] GetAllRegional(string servProvCode);

        /// <summary>
        /// Gets the states config.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="stateSTDList">The state STD list.</param>
        /// <returns>standard choices</returns>
        BizDomainModel[] GetStatesConfig(string servProvCode, List<string> stateSTDList);

        /// <summary>
        /// Gets the regional model by country.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="states">The states.</param>
        /// <returns>regional setting object</returns>
        RegionalModel GetRegionalModelByCountry(string countryCode, out Dictionary<string, string> states);
    }
}
