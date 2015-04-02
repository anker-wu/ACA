/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CssHandlerController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:CssHandlerController.cs 77905 2014-06-11 12:49:28Z ACHIEVO\dennis.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// CSS Handler Controller
    /// </summary>
    public class CssHandlerController : ApiController
    {
        /// <summary>
        /// Get Themes
        /// </summary>
        /// <returns>Themes response</returns>
        [ActionName("Css")]
        public HttpResponseMessage GetThemes()
        {
            IXPolicyBll _xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string colorTheme = _xPolicyBll.GetValueByKey(XPolicyConstant.COLOR_THEME);

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(colorTheme ?? string.Empty, Encoding.UTF8, "text/css")
            };

            return httpResponseMessage;
        }
    }
}