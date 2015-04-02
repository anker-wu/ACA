#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EPaymentConfig.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: EPaymentConfig.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;

using log4net;

/// <summary>
/// EPayment Config Utility Class
/// </summary>
public static class EPaymentConfig
{
    #region Fields

    /// <summary>
    /// E PAYMENT ADAPTER
    /// </summary>
    private const string E_PAYMENT_ADAPTER = "Adapter";

    /// <summary>
    /// ePayment adapter method which is redirect
    /// </summary>
    private const string E_PAYMENT_METHOD_REDIRECT = "Redirect";
    
    /// <summary>
    /// ePayment adapter convenience fee formula
    /// </summary>
    private const string E_PAYMENT_CONV_FEE_FORMULA = "ConvenienceFeeFormula";

    /// <summary>
    /// Create an instance of ILog
    /// </summary>
    private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(EPaymentConfig));

    #endregion Fields

    #region Methods

    /// <summary>
    /// Get E-Payment Configuration
    /// </summary>
    /// <returns>The configuration.</returns>
    public static Hashtable GetConfig()
    {
        XPolicyModel policy = GetPolicyModel(ConfigManager.AgencyCode);
        if (policy == null)
        {
            return null;
        }

        string adapter = GetAdapterName(policy);
        Logger.InfoFormat("adapter:{0}.\n", adapter);

        if (!string.IsNullOrEmpty(adapter))
        {
            PaymentAdapterType adapterType = PaymentHelper.GetPaymentAdapterType();

            if (adapterType == PaymentAdapterType.ExternalAdapter)
            {
                /* If it is redirect payment mode, allow to append static URL parameters that redirect to ACA Adapter.
                 * It use XPOLICY table's DATA3 column.
                 * The parameters format: paramName1=paramValue1;paramName2=paramValue2;
                 */
                Hashtable items = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                DataUtil.ParseString(items, policy.data1, ';', '=');
                DataUtil.ParseString(items, policy.data2, ';', '=');
                DataUtil.ParseString(items, policy.data4, ';', '=');

                // append the customized url parameter to redirect to ACA adapter
                Hashtable htUrlParams = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                DataUtil.ParseString(htUrlParams, policy.data3, ';', '=');

                if (htUrlParams.Count > 0)
                {
                    StringBuilder strUrlParams = new StringBuilder();

                    foreach (var key in htUrlParams.Keys)
                    {
                        strUrlParams.AppendFormat("&{0}={1}", key, HttpUtility.UrlEncode(htUrlParams[key].ToString()));
                    }

                    strUrlParams.Remove(0, 1);

                    items.Add(PaymentConstant.CUSTOMIZED_URL_PARAMETER, strUrlParams);
                }

                return items;
            }
            else if (adapterType != PaymentAdapterType.Unknown)
            {
                IPayment paymentProcessor = (IPayment)ObjectFactory.GetObject(adapter.ToUpper());
                return paymentProcessor.GetEPaymentConfig(policy);
            }
            else
            {
                Hashtable items = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                DataUtil.ParseString(items, policy.data1, ';', '=');
                DataUtil.ParseString(items, policy.data2, ';', '=');
                DataUtil.ParseString(items, policy.data4, ';', '=');

                return items;
            }
        }

        return null;
    }

    /// <summary>
    /// If the payment method use redirect or not
    /// </summary>
    /// <returns>return true if the payment method is redirect</returns>
    public static bool IsPaymentMethodUseRedirect()
    {
        bool result = false;
        XPolicyModel policy = GetPolicyModel(ConfigManager.AgencyCode);

        if (policy != null && policy.data1 != null)
        {
            Hashtable items = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            DataUtil.ParseString(items, policy.data1, ';', '=');

            string item = string.Empty;

            if (items.ContainsKey(E_PAYMENT_ADAPTER))
            {
                item = Convert.ToString(items[E_PAYMENT_ADAPTER]);
            }

            if (string.Equals(item, E_PAYMENT_METHOD_REDIRECT, StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// if exist the convenience fee formula
    /// </summary>
    /// <param name="agencyCodes">The agency code list.</param>
    /// <returns>return true if the total amount formula exist</returns>
    public static bool ExistConvenienceFeeFormula(IEnumerable<string> agencyCodes)
    {
        if (agencyCodes == null)
        {
            return ExistConvenienceFeeFormula(ConfigManager.AgencyCode);
        }

        return agencyCodes.Any(ExistConvenienceFeeFormula);
    }

    /// <summary>
    /// If exist the convenience fee formula in one agency.
    /// </summary>
    /// <param name="agencyCode">The agency code.</param>
    /// <returns>return true if the total amount formula exist.</returns>
    private static bool ExistConvenienceFeeFormula(string agencyCode)
    {
        bool result = false;
        XPolicyModel policy = GetPolicyModel(agencyCode);

        if (policy != null && policy.data4 != null)
        {
            Hashtable items = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            DataUtil.ParseString(items, policy.data4, ';', '=');

            string item = string.Empty;

            if (items.ContainsKey(E_PAYMENT_CONV_FEE_FORMULA))
            {
                item = Convert.ToString(items[E_PAYMENT_CONV_FEE_FORMULA]);
            }

            if (item.Trim() != string.Empty)
            {
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Get the adapter name.
    /// </summary>
    /// <param name="policy">The XPolicy model.</param>
    /// <returns>The adapter name.</returns>
    private static string GetAdapterName(XPolicyModel policy)
    {
        if (policy != null &&
            !string.IsNullOrEmpty(policy.levelData))
        {
            return policy.levelData.Split('_')[0];
        }

        return null;
    }

    /// <summary>
    /// Get the policy model.
    /// </summary>
    /// <param name="agencyCode">The agency code.</param>
    /// <returns>The policy model.</returns>
    private static XPolicyModel GetPolicyModel(string agencyCode)
    {
        IXPolicyBll bizBll = ObjectFactory.GetObject<IXPolicyBll>();
        List<XPolicyModel> models = bizBll.GetPolicyListByPolicyName(XPolicyConstant.EPAYMENT_ADAPTER, agencyCode);

        if (models == null || models.Count == 0)
        {
            return null;
        }

        string adapterType = StandardChoiceUtil.GetEPaymentAdapterType();
        Logger.InfoFormat("adapterName:{0}.\n", adapterType);

        foreach (XPolicyModel model in models)
        {
            //_logger.DebugFormat("model.levelData:{0}.\n", model.levelData);
            if (!string.IsNullOrEmpty(model.levelData) && model.levelData.Equals(adapterType, StringComparison.InvariantCultureIgnoreCase))
            {
                return model;
            }
        }

        return null;
    }

    #endregion Methods
}