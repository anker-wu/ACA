#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CoBrandPlusEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: CoBrandPlusEntity.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// The CoBrand Plus Entity
    /// </summary>
    public class CoBrandPlusEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cap id list.
        /// </summary>
        public CapIDModel[] CapIDs
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the config data.
        /// </summary>
        public Hashtable ConfigData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        public string ModuleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the payment query string.
        /// </summary>
        public string PaymentQueryString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the payment result.
        /// </summary>
        public OnlinePaymentResultModel PaymentResult
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the payment status.
        /// </summary>
        public string PaymentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post back data.
        /// </summary>
        public Hashtable PostbackData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the public user id.
        /// </summary>
        public string PublicUserID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the agency code.
        /// </summary>
        public string ServProvCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        public int StepNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the total fee.
        /// </summary>
        public double TotalFee
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        public PaymentMethod PaymentMethod
        {
            get;
            set;
        }

        #endregion Properties
    }
}