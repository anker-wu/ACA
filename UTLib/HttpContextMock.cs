/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: HttpContextMock.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 *  HttpContext mock object
 *  
 *  Notes:
 * $Id: HttpContextMock.cs 122597 2010-03-05 07:29:43Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Web.Hosting;
using System.Reflection;

namespace Accela.Test.Lib
{
    /// <summary>
    /// HttpContext Mock
    /// </summary>
    public static class HttpContextMock
    {
        private const string ContextKeyAspSession = "AspSession";
        private static HttpContext context = null;

        /// <summary>
        /// Initilziie the HttpContext object.
        /// </summary>
        public static void Init()
        {
            SessionStateMock myState = new SessionStateMock(Guid.NewGuid().ToString("N"),
                new SessionStateItemCollection(), new HttpStaticObjectsCollection(),
                5, true, HttpCookieMode.UseUri, SessionStateMode.InProc, false);

            TextWriter tw = new StringWriter();
            // 这个地方是可以修改的，这是设置的Web路径的地方，但文件是可以不存在的
            HttpWorkerRequest wr = new SimpleWorkerRequest("/webapp", "c:\\inetpub\\wwwroot\\webapp\\", "default.aspx", "", tw);
            context = new HttpContext(wr);
            HttpSessionState state = Activator.CreateInstance(
                typeof(HttpSessionState),
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.CreateInstance,
                null,
                new object[] { myState },
                CultureInfo.CurrentCulture) as HttpSessionState;
            context.Items[ContextKeyAspSession] = state;
            HttpContext.Current = context;
        }
        /// <summary>
        /// Get current HttpContext object.
        /// </summary>
        public static HttpContext Context
        {
            get
            {
                return context;
            }
        }
    }
}
