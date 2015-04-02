using System;

namespace Accela.ACA.PaymentAdapter
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