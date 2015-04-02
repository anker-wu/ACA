#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CoBrandPlusHandler.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: CoBrandPlusHandler.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.UI;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// This class provide the CoBrandPlus Handler.
    /// </summary>
    public class CoBrandPlusHandler : IHandler
    {
        #region Fields
        
        /// <summary>
        /// DEFAULT TIME OUT
        /// </summary>
        private const int DEFAULT_TIME_OUT = 300;

        /// <summary>
        /// MIN TIME CHECK DATABASE CAP_STATUS
        /// </summary>
        private const int MIN_TIME_CHECK_DATABASE_CAP_STATUS = 5000; //millisecond

        /// <summary>
        /// Create an instance of ILog
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CoBrandPlusHandler));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Handle payment result and return the redirect url
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns>The redirect url after handle the payment result.</returns>
        public string HandlePaymentResult(Page currentPage)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Request.QueryString return " + HttpContext.Current.Request.QueryString);
            }

            if (HttpContext.Current.Session[SessionConstant.TRANSACTION_ID] == null)
            {
                Logger.Error("transaction ID is lost");                
                return ACAConstant.URL_WELCOME_PAGE;
            }

            string transactionID = HttpContext.Current.Session[SessionConstant.TRANSACTION_ID].ToString();
            Logger.Debug("transaction id is " + transactionID);

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            int timout = DEFAULT_TIME_OUT;
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            CoBrandPlusEntity entity = null;

            while ((end - start).TotalSeconds < timout)
            {
                entity = cacheManager.GetSingleCachedItem(transactionID) as CoBrandPlusEntity;

                if (entity == null || ACAConstant.PAYMENT_STATUS_CONVERT_CAP_FAILED.Equals(entity.PaymentStatus))
                {
                    break;
                }

                if (ACAConstant.PAYMENT_STATUS_CONVERT_CAP_SUCCESS.Equals(entity.PaymentStatus)
                    && entity.PaymentResult != null)
                {
                    bool isPaymentSuccessful = CheckPaymentStatus(entity);

                    if (isPaymentSuccessful)
                    {
                        Logger.Debug("the cap is paid successfully and the real cap is created successfully");
                        AppSession.SetOnlinePaymentResultModelToSession(entity.PaymentResult);

                        return GetRedirectURL(entity);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(MIN_TIME_CHECK_DATABASE_CAP_STATUS);
                    }
                }

                end = DateTime.Now;
            }

            HttpContext.Current.Session[SessionConstant.TRANSACTION_ID] = null;

            Logger.Error("time out for creating the real cap");
            return ACAConstant.URL_WELCOME_PAGE;
        }

        /// <summary>
        /// Handle the data posted back from 3rd party web site
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        public void HandlePostbackData(Page currentPage)
        {
            IPayment paymentProcessor = PaymentProcessorFactory.CreateProcessor();
            paymentProcessor.CompletePayment(currentPage);
        }

        /// <summary>
        /// check the payment status.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The payment status.</returns>
        private bool CheckPaymentStatus(CoBrandPlusEntity entity)
        {
            foreach (CapPaymentResultModel paymentModel in entity.PaymentResult.capPaymentResultModels)
            {
                if (paymentModel.paymentStatus)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// get the redirect url
        /// </summary>
        /// <param name="entity">a CoBrandPlusEntity model which contains payment result information</param>
        /// <returns>the url</returns>
        private string GetRedirectURL(CoBrandPlusEntity entity)
        {
            // If existed the stepNumber parameter, increase the stepNumer value for the breadcrumb
            string newQueryString = IncreaseStepNumber(entity.PaymentQueryString);
            bool enableShoppingCart = StandardChoiceUtil.IsEnableShoppingCart();
            bool isSuperAgency = StandardChoiceUtil.IsSuperAgency();
            string url = string.Empty;

            // When there is no parameters, it directs to default web to void some null reference exceptions.
            if (string.IsNullOrEmpty(newQueryString))
            {
                return ACAConstant.URL_WELCOME_PAGE;
            }

            if (enableShoppingCart || isSuperAgency)
            {
                url = string.Format("Cap/CapCompletions.aspx?{0}", newQueryString);
            }
            else
            {
                url = string.Format("Cap/CapCompletion.aspx?{0}", newQueryString);
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Redirect To " + url);
            }

            return url;
        }

        /// <summary>
        /// Find the stepNumber to increase 1, return the new query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>The new query string.</returns>
        private string IncreaseStepNumber(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                return string.Empty;
            }

            string retrieveString = "stepNumber=";
            int start = queryString.IndexOf(retrieveString, StringComparison.InvariantCulture);

            if (start > -1)
            {
                string oldValue = retrieveString;
                string newValue = retrieveString;
                string tempNumber = "0";
                int end = queryString.IndexOf("&", start, StringComparison.InvariantCulture);
                int startPositon = start + retrieveString.Length;

                if (end > start)
                {
                    tempNumber = queryString.Substring(startPositon, end - startPositon);
                }
                else
                {
                    tempNumber = queryString.Substring(startPositon);
                }

                oldValue += tempNumber;
                tempNumber = tempNumber.Trim();

                if (ValidationUtil.IsInt(tempNumber))
                {
                    tempNumber = (Convert.ToInt32(tempNumber) + 1).ToString();
                }

                newValue = retrieveString + tempNumber;
                return queryString.Replace(oldValue, newValue);
            }
            else
            {
                return queryString;
            }
        }

        #endregion Methods
    }
}