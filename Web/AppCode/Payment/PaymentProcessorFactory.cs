#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  IPayment interface..
 *
 *  Notes:
 * $Id: PaymentProcessorFactory.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;

using log4net;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// This is payment processor factory.
    /// </summary>
    public class PaymentProcessorFactory
    {
        #region Fields

        /// <summary>
        /// Handler suffix.
        /// </summary>
        private const string HANDLER_SUFFIX = "Handler";

        /// <summary>
        /// Create an instance of ILog
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(PaymentProcessorFactory));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Get payment handler according to standard choice setting
        /// </summary>
        /// <returns>The payment handler.</returns>
        public static IHandler CreateHandler()
        {
            string tempAdapterName = StandardChoiceUtil.GetEPaymentAdapterType();
            string adapterName = tempAdapterName.Substring(0, tempAdapterName.IndexOf("_"));
            adapterName += HANDLER_SUFFIX;

            object o = ObjectFactory.GetObject(adapterName.ToUpper());

            if (o != null)
            {
                return o as IHandler;
            }

            return null;
        }

        /// <summary>
        /// Get payment processor according to standard choice setting
        /// </summary>
        /// <returns>The payment processor.</returns>
        public static IPayment CreateProcessor()
        {
            string adapterName = PaymentHelper.GetAdapterName();

            Logger.DebugFormat(string.Format("The payment adapter is {0}\n", adapterName));

            try
            {
                object o = ObjectFactory.GetObject(adapterName.ToUpper());

                if (o != null)
                {
                    return o as IPayment;
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }

            return null;
        }

        #endregion Methods
    }
}