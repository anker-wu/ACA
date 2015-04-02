/**
 *  Accela Citizen Access
 *  File: Global.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 *   
 * 
 *  Notes:
 * $Id: Global.cs 130107 2010-07-30 15:23:56Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;

namespace Accela.ACA.PaymentAdapter.TPEAdapterWeb
{
    /// <summary>
    /// Application entrance
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// application start event
        /// </summary>
        /// <param name="sender">target object</param>
        /// <param name="e">event arguments</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            log4net.Config.XmlConfigurator.Configure();     
        }
    }
}