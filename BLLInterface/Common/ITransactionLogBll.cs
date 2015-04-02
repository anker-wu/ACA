/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ITransactionLogBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ITransactionLogBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Defines methods for transaction log function.
    /// </summary>
    public interface ITransactionLogBll
    {
        #region Methods

        /// <summary>
        /// Add the log information to queue for logging to DB.
        /// </summary>
        /// <param name="location">file name.</param>
        /// <param name="opertation">method name.</param>
        /// <param name="message">the message need to be logged.</param>
        void AddTransactionLog(string location, string opertation, string message);

        #endregion Methods
    }
}