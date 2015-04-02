#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SpellCheck.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: SpellCheck.cs 143930 2009-08-19 10:40:51Z ACHIEVO\weiky chen $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL
{
    /// <summary>
    /// Defines method signs for SpellCheck.
    /// </summary>
    public class SpellCheckBll : ISpellCheckBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of SpellCheckService.
        /// </summary>
        private SpellCheckerWebServiceService SpellCheckService
        {
            get
            {
                return WSFactory.Instance.GetWebService<SpellCheckerWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// check spell
        /// </summary>
        /// <param name="sentence">the sentence that need to check spell</param>
        /// <param name="serviceProviderCode">The agency code.</param>
        /// <returns>a array of check result</returns>
        public SpellCheckerResultModel CheckSpelling(string sentence, string serviceProviderCode)
        {
            return SpellCheckService.checkSpelling(sentence, I18nCultureUtil.UserPreferredCultureInfo.Name.Replace("-", "_"), serviceProviderCode);
        }

        #endregion Methods
    }
}