#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ObrarHttpHandler.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: A http handler to intercepts the http request of the url "/obrar.cgi" and to imitates the WebGate's functions.
*
* </pre>
*/

#endregion

using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Accela.ACA.OAMAccessGate
{
    /// <summary>
    /// <para>
    /// A http handler to intercepts the http request of the url "/obrar.cgi" and to imitates the WebGate's functions.
    /// After user just signed in, the OAM server will navigate user to the url "/obrar.cgi" under the requested web server.
    /// The url contains the authentication token and the destination url.
    /// This http handler is compatible to the Webgate 10g, it will create the ObSSOCookie used the authentication token as value
    /// and then navigate user to the destination url.
    /// </para>
    /// Configuration:
    /// <para>
    /// [IIS 6]
    /// 1. ACA Web Site -> Properties -> "Home Directory" tab -> Configuration -> "Mapping" tab -> Add an application extensions:
    ///     Extension: .cgi
    ///     Executable: %SystemRoot%\Microsoft.NET\Framework\{.net version}\aspnet_isapi.dll
    ///     Verbs: All verbs
    ///     Clear the check-box: Verity that file exists
    /// 2. If your ACA application is not in root path, copy "Accela.ACA.OAMAccessGate.dll" to "bin" folder under the ACA Web Site root folder.
    /// 3. Add below content to the "configuration/system.web/httpHandlers" section in the "web.config" file under the ACA Web Site root folder.
    ///     <add path="/obrar.cgi" verb="*" type="Accela.ACA.OAMAccessGate.ObrarHttpHandler" />
    /// </para>
    /// <para>
    /// [IIS 7/7.5/8 Classic mode]
    /// 1. If your ACA application is not in root path, copy "Accela.ACA.OAMAccessGate.dll" to "bin" folder under the ACA Web Site root folder.
    /// 2. ACA Web Site -> Handler Mappings -> Add Script Map:
    ///     Request Path: /obrar.cgi
    ///     Executable: %SystemRoot%\Microsoft.NET\Framework\{.net version}\aspnet_isapi.dll
    ///     Name: AccessGateObrarHandler
    /// 3. Click the "Request Restrications" button -> Mapping tab -> make sure the "Invoke handler only if request is mapped to:" checkbox is unchecked.
    /// 4. Add below content to the "configuration/system.web/httpHandlers" section in the "web.config" file under the ACA Web Site root folder.
    ///     <add path="/obrar.cgi" verb="*" type="Accela.ACA.OAMAccessGate.ObrarHttpHandler" />
    /// </para>
    /// <para>
    /// [IIS 7/7.5/8 Integrated mode]
    /// 1. If your ACA application is not in root path, copy "Accela.ACA.OAMAccessGate.dll" to "bin" folder under the ACA Web Site root folder.
    /// 2. ACA Web Site -> Handler Mappings -> Add Managed Handler:
    ///     Request Path: /obrar.cgi
    ///     Type: Accela.ACA.OAMAccessGate.ObrarHttpHandler
    ///     Name: AccessGateObrarHandler
    /// 3. Click the "Request Restrications" button -> Mapping tab -> make sure the "Invoke handler only if request is mapped to:" checkbox is unchecked.
    ///   The following content will be generated in the "configuration/system.webServer/handlers" section in the "web.config" file under the ACA Web Site root folder.
    ///     <add name="AccessGateObrarHandler" path="/obrar.cgi" verb="*" type="Accela.ACA.OAMAccessGate.ObrarHttpHandler" />
    /// </para>
    /// </summary>
    public class ObrarHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Gets a value indicating whether another request can use this instance.
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        ///  Handle the http request.
        /// </summary>
        /// <param name="context">Current http context.</param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            if (request.Url.AbsolutePath.Equals("/obrar.cgi", StringComparison.OrdinalIgnoreCase))
            {
                string requestUrl = context.Request.Url.PathAndQuery;

                if (requestUrl.IndexOf('%') >= 0)
                {
                    requestUrl = HttpUtility.UrlDecode(requestUrl);
                }

                string urlPattern = @"/obrar\.cgi\?cookie=(?<SSOCookie>.[^\s]*)\sredirectto=(?<RedirectTo>.[^\s]*)\s(ssoCookie=httponly)?";
                Regex regUrl = new Regex(urlPattern);

                if (regUrl.IsMatch(requestUrl))
                {
                    Match match = regUrl.Match(requestUrl);

                    //Create ObSSOCookie.
                    string cookieValue = match.Result("${SSOCookie}");
                    Common.SetSSOCookie(request, ref response, cookieValue);

                    //Create a cookie to indicates the user just signed in.
                    Common.SetSignInFlagCookie(request, ref response);

                    /*
                     * In IE browser, needs to add the P3P http header to resolve the cookie issue 
                     * that the IE browser can not read the cookie under iframe.
                     * FYI: 
                     * http://www.w3.org/TR/P3P11/
                     * http://hi.baidu.com/ifosc/item/58393b15a8103d088ebde4e4
                     */
                    context.Response.AddHeader("P3P", "CP=CAO PSA OUR");

                    //Redirect user to the requested url.
                    string redirectTo = match.Result("${RedirectTo}");
                    context.Response.SuppressContent = true;
                    context.Response.RedirectLocation = redirectTo;
                    context.Response.StatusCode = 302;
                }
            }
        }
    }
}