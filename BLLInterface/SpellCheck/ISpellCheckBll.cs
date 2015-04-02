#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ISpellCheck.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ISpellCheck.cs 143930 2009-08-19 10:40:51Z ACHIEVO\weiky chen $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   
 * </pre>
 */

#endregion Header

using System;
using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL
{
    /// <summary>
    /// Defines method signs for SpellCheck.
    /// </summary>
    public interface ISpellCheckBll
    {
        #region Methods

        /// <summary>
        /// check spell
        /// </summary>
        /// <param name="sentence">the sentence that need to check spell</param>
        /// <param name="serviceProviderCode">The agency code.</param>
        /// <returns>a array of check result</returns>
        SpellCheckerResultModel CheckSpelling(string sentence, string serviceProviderCode);

        #endregion Methods
    }
}