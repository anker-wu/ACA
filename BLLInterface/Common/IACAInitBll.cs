#region Header

/*
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IACAInitBll.cs
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

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// ACA Initial interface
    /// </summary>
    public interface IACAInitBll
    {
        #region Methods

        /// <summary>
        /// Initial the ACA.
        /// </summary>
        void InitACA();

        #endregion
    }
}
