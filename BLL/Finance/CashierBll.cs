#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CashierBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CashierBll.cs 277355 2014-08-14 07:28:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/10/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// This class provide the ability to operation cashier.
    /// </summary>
    public class CashierBll : BaseBll, ICashierBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of CashierService.
        /// </summary>
        private CashierWebServiceService CashierService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CashierWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// get customized receipt by receiptNo ,if customized receipt is null.return receiptNO.
        /// </summary>
        /// <param name="capID4ws">capID object.</param>
        /// <param name="receiptNbr">receipt sequence number</param>
        /// <param name="format4ws">Model of QueryFormat4WS</param>
        /// <returns>Customized Receipt</returns>
        /// <exception cref="DataValidateException">{ <c>capID4ws</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string GetCustomizedReceiptByReceiptNo(CapIDModel4WS capID4ws, long receiptNbr, QueryFormat4WS format4ws)
        {
            if (capID4ws == null)
            {
                throw new DataValidateException(new string[] { "capID4ws" });
            }

            try
            {
                return CashierService.getCustomizedReceiptByReceiptNo(capID4ws, receiptNbr, format4ws);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Returns the receipt model based on the primary key..
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="receiptSeqNbr">receipt Sequence Number</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Receipt object.</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ReceiptModel4WS GetReceiptByPK(string serviceProviderCode, long receiptSeqNbr, string callerID)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                return CashierService.getReceiptByPK(serviceProviderCode, receiptSeqNbr, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}