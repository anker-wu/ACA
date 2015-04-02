#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FileUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
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
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Security;

namespace Accela.ACA.Web.Common
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
                return GetApplicationRoot(System.Web.HttpContext.Current);
            }
        }

        /// <summary>
        /// Gets the Customize path root, ends with /
        /// </summary>
        public static string CustomizeFolderRoot
        {
            get
            {
                return string.Format(
                    "{0}{1}/{2}/",
                    ApplicationRoot,
                    "Customize",
                    I18nCultureUtil.UserPreferredCulture);
            }
        }

        /// <summary>
        /// Gets the customize folder root without language.
        /// </summary>
        /// <value>The customize folder root without language.</value>
        public static string CustomizeFolderRootWithoutLang
        {
            get
            {
                return string.Format(
                    "{0}{1}/",
                    ApplicationRoot,
                    "Customize");
            }
        }

        /// <summary>
        /// Gets the customize user control folder.
        /// </summary>
        /// <value>The customize user control folder.</value>
        public static string CustomizeUserControlFolder
        {
            get
            {
                return string.Format("{0}Customize/UserControls/", ApplicationRoot);
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
        /// Determines whether [is customize style existing] [the specified style file name].
        /// </summary>
        /// <param name="styleFileName">Name of the style file.</param>
        /// <returns><c>true</c> if [is customize style existing] [the specified style file name]; otherwise, <c>false</c>.</returns>
        public static bool IsCustomizeStyleExisting(string styleFileName)
        {
            return File.Exists(HttpContext.Current.Server.MapPath(CustomizeFolderRoot + styleFileName));
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
        /// return full virtual path with Customize Folder Root appended.
        /// </summary>
        /// <param name="partialPath">The partial path.</param>
        /// <returns>The path that append the customize folder root.</returns>
        public static string AppendCustomizeFolderRoot(string partialPath)
        {
            return CombineWebPath(CustomizeFolderRoot, partialPath);
        }

        /// <summary>
        /// Appends the absolute path, contains host name, port and virtual path.
        /// For example: http://hostname:1234/virtualPath/ + partialPath
        /// </summary>
        /// <param name="partialPath">The partial path.</param>
        /// <returns>The path that append the absolute path.</returns>
        public static string AppendAbsolutePath(string partialPath)
        {
            string absolutePath = ConfigManager.Protocol + "://" + HttpContext.Current.Request.Url.Authority;
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

        /// <summary>
        /// Delete CSV File
        /// </summary>
        /// <param name="filePath">CSV File Path</param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Copies the files in the source directory to destination.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destDirectory">The destination directory.</param>
        public static void CopyFilesInDirectory(string sourceDirectory, string destDirectory)
        {
            if (Directory.Exists(sourceDirectory))
            {
                string[] sourceFullNames = Directory.GetFiles(sourceDirectory);

                foreach (string sourceFullName in sourceFullNames)
                {
                    string fileName = Path.GetFileName(sourceFullName);
                    string destFullName = Path.Combine(destDirectory, fileName);

                    if (!File.Exists(destFullName) || (GetFileMD5Value(sourceFullName) != GetFileMD5Value(destFullName)))
                    {
                        File.Copy(sourceFullName, destFullName, true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the customize mapping config path.
        /// </summary>
        /// <returns>The customize mapping config path.</returns>
        public static string GetCustomizeMappingConfigPath()
        {
            return string.Format("{0}Customize/Mapping/CustomizeMapping.config", ApplicationRoot);
        }

        /// <summary>
        /// Check if the url is external or not
        /// </summary>
        /// <param name="url">the URL</param>
        /// <returns>true - External URL</returns>
        public static bool IsExternalUrl(string url)
        {
            bool isExternalUrl;

            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            // relative url is always not external.
            if (url.StartsWith("/") || url.StartsWith("~/") || url.StartsWith("./") || url.StartsWith("../"))
            {
                isExternalUrl = false;
            }
            else
            {
                if (AntiCsrfAttackUtil.IsTrustedLocalUrl(url, HttpContext.Current))
                {
                    isExternalUrl = false;
                }
                else
                {
                    isExternalUrl = true;
                }
            }

            return isExternalUrl;
        }

        /// <summary>
        /// Get Customization type
        /// </summary>
        /// <param name="path">the path</param>
        /// <param name="culture">current culture</param>
        /// <returns>Customization Type</returns>
        public static CustomizationType GetCustomizationType(string path, string culture)
        {
            CustomizationType customizationType = CustomizationType.None;

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            Hashtable htCustomization = cacheManager.GetCustomizationFileMap(ConfigManager.SuperAgencyCode, culture);
            string customizationDir = ConfigManager.CustomizationDirectory;
            HttpContext context = HttpContext.Current;

            if (htCustomization.ContainsKey(path))
            {
                customizationType = (CustomizationType)htCustomization[path];
            }
            else
            {
                // Append culture extention
                if (!string.IsNullOrWhiteSpace(culture))
                {
                    culture = ACAConstant.SPLIT_CHAR4 + culture;
                }

                if (File.Exists(context.Server.MapPath("~/" + customizationDir + "/" + path + culture + ".css")))
                {
                    customizationType = customizationType | CustomizationType.Css;
                }

                if (File.Exists(context.Server.MapPath("~/" + customizationDir + "/" + path + culture + ".js")))
                {
                    customizationType = customizationType | CustomizationType.Javascript;
                }

                if (File.Exists(context.Server.MapPath("~/" + customizationDir + "/" + path)))
                {
                    customizationType = CustomizationType.CustomPage;
                }

                // To void the duplicated key issue
                if (htCustomization.ContainsKey(path))
                {
                    htCustomization[path] = customizationType;
                }
                else
                {
                    htCustomization.Add(path, customizationType);
                }
            }

            return customizationType;
        }

        /// <summary>
        /// Get the file's MD5 hexadecimal string value.
        /// </summary>
        /// <param name="fileName">The file full name.</param>
        /// <returns>The MD5 hexadecimal string value.</returns>
        private static string GetFileMD5Value(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return string.Empty;
            }

            // get the hash bytes use MD5 algorithm
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            HashAlgorithm algorithm = MD5.Create();
            byte[] hashBytes = algorithm.ComputeHash(fs);
            fs.Close();

            // Use reflect get the method [ByteArrayToHexString] in [MachineKeySection]. 
            // This method used to convert the byte array to hexadecimal string.
            Type type = typeof(System.Web.Configuration.MachineKeySection);
            MethodInfo byteArrayToHexString = type.GetMethod("ByteArrayToHexString", BindingFlags.Static | BindingFlags.NonPublic);

            // byte array convert to hexadecimal string.
            return (string)byteArrayToHexString.Invoke(null, new object[] { hashBytes, 0 });
        }

        #endregion Methods
    }
}