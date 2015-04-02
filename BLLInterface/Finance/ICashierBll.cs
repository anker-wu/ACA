#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICashierBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ICashierBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// Defines methods for cashier BLL.
    /// </summary>
    public interface ICashierBll
    {
        #region Methods

        /// <summary>
        /// get customized receipt by receiptNo ,if customized receipt is null.return receiptNO.
        /// </summary>
        /// <param name="capID4ws">capID object.</param>
        /// <param name="receiptNbr">receipt sequence number</param>
        /// <param name="format4ws">Model of QueryFormat4WS</param>
        /// <returns>Customized Receipt</returns>
        string GetCustomizedReceiptByReceiptNo(CapIDModel4WS capID4ws, long receiptNbr, QueryFormat4WS format4ws);

        /// <summary>
        /// Returns the receipt model based on the primary key..
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="receiptSeqNbr">receipt Sequence Number</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Receipt object.</returns>
        ReceiptModel4WS GetReceiptByPK(string serviceProviderCode, long receiptSeqNbr, string callerID);

        #endregion Methods
    }
}