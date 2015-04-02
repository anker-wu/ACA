using System;

namespace PaymentProviderEmulator
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// application start event
        /// </summary>
        /// <param name="sender">target object</param>
        /// <param name="e">event arguments</param>
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
