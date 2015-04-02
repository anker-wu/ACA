/**
 *  Accela Citizen Access
 *  File: WSFactory.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: WSFactory.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using Accela.ACA.PaymentAdapter;

namespace Accela.ACA.PaymentAdapter.Service
{
    /// <summary>
    /// web service factory
    /// </summary>
    public sealed class WSFactory
    {
        /// <summary>
        /// singleton pattern.
        /// </summary>
        public static readonly WSFactory Instance = new WSFactory();

        /// <summary>
        /// payment gateway web service
        /// </summary>
        private PaymentGatewayWebServiceService _paymentGatewayWebService = null;

        /// <summary>
        /// Prevents a default instance of the WSFactory class from being created.
        /// private constructor avoid to be instance by caller.
        /// </summary>
        private WSFactory()
        {
            // Set the maximum idle time of a ServicePoint instance to 15 seconds.
            // After the idle time expires, the ServicePoint object is eligible for
            // garbage collection and cannot be used by the ServicePointManager object.
            // Notice that MaxServicePointIdleTime value must less than the tomcat connectionTimeout value 
            // to make sure that client have released the connection resource before Tomcat release it.
            // Tomcat connectionTimeout value can be configed in jbossweb-tomcat55.sar\server.xml, and the default value is 20 seconds. 
            // Make sure the idle timeout(MaxServicePointIdleTime) on the client side is less than that on the server side(in jbossweb-tomcat55.sar\server.xml)    
            ServicePointManager.MaxServicePointIdleTime = 15000;
        }

        /// <summary>
        /// Get the PaymentGateway web service
        /// </summary>
        /// <returns>The instance of PaymentGatewayWebService</returns>
        public PaymentGatewayWebServiceService GetPaymentGatewayWebService()
        {
            if (_paymentGatewayWebService == null)
            {
                lock (typeof(PaymentGatewayWebServiceService))
                {
                    if (_paymentGatewayWebService == null)
                    {
                        _paymentGatewayWebService = new PaymentGatewayWebServiceService();
                    }

                    NameValueCollection urlRoot = ConfigurationManager.GetSection(@"paymentAdapter") as NameValueCollection;

                    _paymentGatewayWebService.Url = String.Concat(urlRoot["WebServiceURLRoot"], "PaymentGatewayWebService");
                    _paymentGatewayWebService.Timeout = 300;
                }
            }

            return _paymentGatewayWebService;
        }
    }
}
