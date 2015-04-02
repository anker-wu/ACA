/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: App.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Text;
using Accela.ACA.FormDesigner.GFilterViewService;

namespace Accela.ACA.FormDesigner
{
    /// <summary>
    /// the class for App
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initialize a new instance for App
        /// </summary>
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        /// <summary>
        /// method for Startup
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">StartupEventArgs object</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Application.Current.Host.Content.FullScreenOptions = System.Windows.Interop.FullScreenOptions.None;
            Application.Current.Host.Content.IsFullScreen = false;
            if (e.InitParams != null && e.InitParams.Count > 0)
            {
                BussinessParam param = new BussinessParam()
                {
                    ServProvCode = e.InitParams.ContainsKey("servProvCode") ? e.InitParams["servProvCode"] : string.Empty,
                    LevelType = e.InitParams.ContainsKey("levelType") ? e.InitParams["levelType"] : string.Empty,
                    LevelName = e.InitParams.ContainsKey("levelName") ? e.InitParams["levelName"] : string.Empty,
                    CallerId = e.InitParams.ContainsKey("callerId") ? e.InitParams["callerId"] : string.Empty,
                    ViewId = e.InitParams.ContainsKey("viewId") ? e.InitParams["viewId"] : string.Empty,
                    PermissionLevel = e.InitParams.ContainsKey("permissionLevel") ? e.InitParams["permissionLevel"] : string.Empty,
                    PermissionValue = e.InitParams.ContainsKey("permissionValue") ? e.InitParams["permissionValue"] : string.Empty,
                    CountryCode = e.InitParams.ContainsKey("countryCode") ? e.InitParams["countryCode"] : string.Empty,
                    LangCode = e.InitParams.ContainsKey("langCode") ? e.InitParams["langCode"] : string.Empty,
                    LoadingErrorMessge = e.InitParams.ContainsKey("loadingErrorMsg") ? e.InitParams["loadingErrorMsg"] : string.Empty,
                    SaveErrorMessge = e.InitParams.ContainsKey("saveErrorMsg") ? e.InitParams["saveErrorMsg"] : string.Empty
                };
                string hostUrl = GetUrl();
                //param.ServiceUrl = "http://henry-he/acaFd/WebService";
                param.ServiceUrl = hostUrl;
                this.RootVisual = new ACAFormDesigner(param);
            }
            else
            {
                this.RootVisual = new ACAFormDesigner();
            }
        }

        /// <summary>
        /// method for GetUrl
        /// </summary>
        /// <returns>the service url</returns>
        private string GetUrl()
        {
            string url = App.Current.Host.Source.ToString();
            if (url.IndexOf("?") > 0)
            {
                url = url.Substring(0, url.IndexOf("?"));
            }
            string urlhead = url.Substring(0, url.IndexOf(":") + 1);
            url = url.Substring(0, url.LastIndexOf("/"));
            url = url.Substring(0, url.LastIndexOf("/"));
            url += "/WebService";
            return url;
        }

        ///// <summary>
        ///// for test
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="sJson"></param>
        ///// <returns></returns>
        //private T Deserialize<T>(string sJson) where T : class
        //{
        //    System.Runtime.Serialization.DataContractSerializer ds = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
        //    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sJson));
        //    T obj = (T)ds.ReadObject(ms);
        //    ms.Close();
        //    return obj;
        //}

        /// <summary>
        /// method for Application exit
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">EventArgs object</param>
        private void Application_Exit(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// method for throw unhandle exception
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">ApplicationUnhandledExceptionEventArgs object</param>
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        /// <summary>
        /// method for report error to dom
        /// </summary>
        /// <param name="e">ApplicationUnhandledExceptionEventArgs object</param>
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
