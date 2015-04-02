#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IHandler.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  IPayment interface..
 *
 *  Notes:
 * $Id: IHandler.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// payment interface
    /// </summary>
    public interface IHandler
    {
        #region Methods

        /// <summary>
        /// Handle payment result and return the redirect url
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns>The redirect url after handle the payment result.</returns>
        string HandlePaymentResult(Page currentPage);

        /// <summary>
        /// Handle the data posted back from 3rd party web site
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        void HandlePostbackData(Page currentPage);

        #endregion Methods
    }
}