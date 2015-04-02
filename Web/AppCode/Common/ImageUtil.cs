#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  ImageUtil for getting Image Path by Image Name,Culture.
 *  Image should call this class if need to get ImageUrl.
 *
 *  Notes:
 *      $Id: ImageUtil.cs 133942 2009-07-14 10:00:00 ACHIEVO\solt.su $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.IO;
using System.Web;

using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using log4net;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The image utility for getting Image Path by Image Name, Culture.
    /// Image should call this class if need to get ImageUrl.
    /// </summary>
    public static class ImageUtil
    {
        #region Fields

        /// <summary>
        /// Default Culture Name
        /// </summary>
        private const string DEFAULT_LUANGUAGE_FOLDER = "Default";

        /// <summary>
        /// Parent Image Folder Name
        /// </summary>
        private const string APP_THEMES_FOLDER = "app_themes";

        /// <summary>
        /// Image Folder Name
        /// </summary>
        private const string ASSETS_FOLDER = "assets";

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ImageUtil));

        /// <summary>
        /// Image Path HashTable
        /// </summary>
        private static Hashtable _htImages;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets All images.
        /// </summary>
        private static Hashtable Images
        {
            get
            {
                if (_htImages == null)
                {
                    _htImages = GetAllImages();
                }

                if (_htImages.Count == 0)
                {
                    Logger.Debug("Images property : count == 0");
                }

                return _htImages;
            }
        }

        #endregion

        #region methods 

        /// <summary>
        ///  Get Image url by image name, which identifies what culture image should be loaded.
        ///  if Culture Folder include Image file, return image path with Culture folder.else return image path with default folder.
        /// </summary>
        /// <param name="imageName">Image Name</param>
        /// <returns>the image url. </returns>
        public static string GetImageURL(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                Logger.Debug("imageName is empty or null");
                return string.Empty;
            }

            string imageKey = I18nCultureUtil.UserPreferredCulture.ToLower() + imageName.ToLower();

            string imageUrl = imageName;

            // get the image from current culture image folder.
            if (Images.ContainsKey(imageKey))
            {
                imageUrl = Images[imageKey].ToString();
            }
            else
            {
                //Get Default Image
                string defalutImageKey = DEFAULT_LUANGUAGE_FOLDER.ToLower() + imageName.ToLower();

                if (Images.ContainsKey(defalutImageKey))
                {
                    imageUrl = Images[defalutImageKey].ToString();
                }
            }

            return imageUrl;
        }

        /// <summary>
        /// Build Image HashTable, Key= CultureName + fileName , value = fileFullName
        /// if need ,add Image Type Filter.
        /// </summary>
        /// <returns>all images for all kinds of culture.</returns>
        private static Hashtable GetAllImages()
        {
            Hashtable images = new Hashtable();

            string appThemesFolder = HttpContext.Current.Server.MapPath(string.Format("{0}{1}", FileUtil.ApplicationRoot, APP_THEMES_FOLDER));

            if (Directory.Exists(appThemesFolder))
            {
                DirectoryInfo appThemesDirectoryInfo = new DirectoryInfo(appThemesFolder);

                // Get all Culture folder under APP_THEMES folder
                foreach (DirectoryInfo cultureDirectory in appThemesDirectoryInfo.GetDirectories())
                {
                    string assetsFolderName = string.Format("{0}\\{1}", cultureDirectory.FullName, ASSETS_FOLDER);

                    // assets folders
                    if (Directory.Exists(assetsFolderName))
                    {
                        DirectoryInfo assetDirectoryInfo = new DirectoryInfo(assetsFolderName);

                        // get all image name under assets folder
                        foreach (FileInfo fileInfo in assetDirectoryInfo.GetFiles())
                        {
                            // e.g /web/app_themes/en_US/assets/a.gif
                            images.Add(cultureDirectory.Name.ToLower() + fileInfo.Name.ToLower(), string.Format("{0}{1}/{2}/{3}/{4}", FileUtil.ApplicationRoot, APP_THEMES_FOLDER, cultureDirectory.Name, ASSETS_FOLDER, fileInfo.Name));
                        }
                    }
                }
            }

            if (images.Count == 0)
            {
                Logger.Debug("GetAllImages : count == 0");
            }

            return images;
        }

        #endregion 
    }
}
