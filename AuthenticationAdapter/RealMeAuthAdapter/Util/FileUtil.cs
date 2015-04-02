#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FileUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  LabelUtil for getting label by label key.
 *  UI should call this class if need to get text in .cs.
 *
 *  Notes:
 *      $Id: FileUtil.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using Accela.ACA.SSOInterface;

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// This class provide the file utility.
    /// </summary>
    public static class FileUtil
    {
        #region Properties

        /// <summary>
        /// Gets Application root, ends with /
        /// </summary>
        public static string ApplicationRoot
        {
            get
            {
                return GetApplicationRoot(HttpContext.Current);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Application root, ends with /
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <returns>The application root.</returns>
        public static string GetApplicationRoot(HttpContext context)
        {
            string sRoot = context.Request.ApplicationPath;

            if (string.IsNullOrEmpty(sRoot))
            {
                sRoot = ".";
            }

            if (!sRoot.EndsWith("/", StringComparison.InvariantCulture))
            {
                sRoot += "/";
            }

            return sRoot;
        }

        /// <summary>
        /// return full virtual path with application root appended.
        /// </summary>
        /// <param name="partialPath">The partial path.</param>
        /// <returns>The path that append application root.</returns>
        public static string AppendApplicationRoot(string partialPath)
        {
            return CombineWebPath(ApplicationRoot, partialPath);
        }

        /// <summary>
        /// Appends the absolute path, contains host name, port and virtual path.
        /// For example: http://hostname:1234/virtualPath/ + partialPath
        /// </summary>
        /// <param name="partialPath">The partial path.</param>
        /// <returns>The path that append the absolute path.</returns>
        public static string AppendAbsolutePath(string partialPath)
        {
            string absolutePath = ACAContext.Instance.Protocol + "://" + HttpContext.Current.Request.Url.Authority;
            string applicationPath = AppendApplicationRoot(partialPath);

            return CombineWebPath(absolutePath, applicationPath);
        }

        /// <summary>
        /// combine web path
        /// </summary>
        /// <param name="path1">the path 1.</param>
        /// <param name="path2">the path 2.</param>
        /// <returns>the path that combined.</returns>
        public static string CombineWebPath(string path1, string path2)
        {
            if (path1 != null && path2 != null)
            {
                if (!path1.EndsWith("/", StringComparison.InvariantCulture))
                {
                    path1 += "/";
                }

                if (path2.StartsWith("/", StringComparison.InvariantCulture))
                {
                    path2 = path2.Substring(1);
                }
            }

            return path1 + path2;
        }

        #endregion Methods
    }
}