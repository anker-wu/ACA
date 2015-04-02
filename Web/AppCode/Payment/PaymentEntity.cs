#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: PaymentEntity.cs 135124 2009-06-17 06:01:11Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// The Payment entity.
    /// </summary>
    public class PaymentEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the PaymentEntity.EntityType
        /// </summary>
        public ACAConstant.PaymentEntityType EntityType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the PaymentEntity.EntityID
        /// </summary>
        public string EntityID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the PaymentEntity.TotalFee
        /// </summary>
        public double TotalFee
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cap id model list.
        /// </summary>
        [JsonIgnore]
        public CapIDModel4WS[] CapIDs
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [JsonIgnore]
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
        /// Gets or sets the OnlinePaymentResultModel .
        /// </summary>
         [JsonIgnore]
        public OnlinePaymentResultModel PaymentResult
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
        /// Gets or sets renew cap flag.
        /// </summary>
        public string RenewalFlag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets pay fee due cap flag.
        /// </summary>
        public string Pay4ExistingCapFlag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the payment method
        /// </summary>
        public PaymentMethod PaymentMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the merchant account id
        /// </summary>
        public long MerchantAccountID
        {
            get;
            set;
        }

        #endregion Properties
    }
}