#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TransactionLogBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  This class deal with TransactionLogBll
 *
 *  Notes:
 * $Id: TransactionLogBll.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Timers;
using System.Web;

using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to operation transaction log.
    /// </summary>
    public class TransactionLogBll : BaseBll, ITransactionLogBll
    {
        #region Methods

        /// <summary>
        /// Add the log information to queue for logging to DB.
        /// </summary>
        /// <param name="location">file name.</param>
        /// <param name="opertation">method name.</param>
        /// <param name="message">the message need to be logged.</param>
        public void AddTransactionLog(string location, string opertation, string message)
        {
        }

        #endregion Methods
    }
}
