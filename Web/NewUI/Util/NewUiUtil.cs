#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: NewUiUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  utitily for common.
 *
 *  Notes:
 * $Id: NewUiUtil.cs 182945 2014-08-20 08:22:49Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accela.ACA.Common;

namespace Accela.ACA.Web.NewUI
{
    /// <summary>
    /// ACA new UI class.
    /// </summary>
    public static class NewUiUtil
    {
        /// <summary>
        /// get url by resource
        /// </summary>
        /// <param name="url">url parameter</param>
        /// <returns>url string</returns>
        public static string GetUrlByResource(string url)
        {
            string splitChar = url.Contains("?") ? "&" : "?";
            url += string.Format(splitChar + "{0}={1}", "isFromNewUi", ACAConstant.COMMON_Y);

            return url;
        }
    }
}