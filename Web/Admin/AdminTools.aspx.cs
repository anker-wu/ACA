/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TreeModule.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: AdminTools.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Web;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Config;
using Accela.ACA.Common.Log;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;

/// <summary>
/// Admin tools page
/// </summary>
public partial class Admin_AdminTools : AdminBasePage
{
    /// <summary>
    /// log4net Logger
    /// </summary>
    private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(Admin_AdminTools));

    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!AppSession.IsAdmin)
        {
            Response.Redirect("./login.aspx");
        }
    }

    /// <summary>
    /// Raises "clear cache" button click event 
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void ClearCacheButton_ServerClick(object sender, EventArgs e)
    {
        AppSession.IsAdmin = false;
        try
        {
            ICacheManager cacheManage = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            cacheManage.ClearCache(CacheConstant.CacheKeys);
            AppSession.IsAdmin = true;
        }
        catch (Exception ex)
        {
            Logger.ErrorFormat("Error occurred, error message:{0}", ex);

            AppSession.IsAdmin = true;
        }
    }

    /// <summary>
    /// Raises "export cache" button click event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void ExportCacheButton_ServerClick(object sender, EventArgs e)
    {
        Response.Write("Cache Items Count : " + HttpRuntime.Cache.Count.ToString() + "<br>");
        IDictionaryEnumerator cacheItem = HttpRuntime.Cache.GetEnumerator();
        int index = 1;

        while (cacheItem.MoveNext())
        {
            Response.Write("<Font color='red'>" + index.ToString() + ". Cache Key : " + cacheItem.Key.ToString() + "</font><br>");
            Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            if (cacheItem.Value == null)
            {
                Response.Write("cache content is null.<br>");
            }
            else if (cacheItem.Value is Hashtable)
            {
                Hashtable ht = cacheItem.Value as Hashtable;
                IDictionaryEnumerator contentEnumerator = ht.GetEnumerator();
                while (contentEnumerator.MoveNext())
                {
                    if (contentEnumerator.Value != null)
                    {
                        Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                        Response.Write("Key : " + contentEnumerator.Key.ToString() + "----" + contentEnumerator.Value.ToString() + "<br>");
                    }
                }
            }
            else
            {
                Response.Write(cacheItem.Value.ToString() + "<br>");
            }

            index++;
        }
    }

    /// <summary>
    /// Raises "clear temp" button click event 
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void ClearTempButton_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string uploadFolder = AttachmentUtil.GetTempDirectory();
            DeleteFileByFolder(uploadFolder, true);
            DeleteFileByFolder(Path.Combine(uploadFolder, "FailedFiles"), false);
            DeleteFileByFolder(Path.Combine(uploadFolder, "TempFiles"), false);
        }
        catch (Exception ex)
        {
            Logger.ErrorFormat("Error occurred, error message:{0}", ex);
        }
    }

    /// <summary>
    /// Raises "export temp files" button click event 
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void ExportTempButton_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string uploadFolder = AttachmentUtil.GetTempDirectory();

            ShowFilesByFolder(uploadFolder);
            ShowFilesByFolder(Path.Combine(uploadFolder, "FailedFiles"));
            ShowFilesByFolder(Path.Combine(uploadFolder, "TempFiles"));
        }
        catch (Exception ex)
        {
            Logger.ErrorFormat("Error occurred, error message:{0}", ex);
        }
    }

    /// <summary>
    /// Raises "admin home" button click event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void AdminHomeButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    /// <summary>
    /// Raises "daily home" button click event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void DailyHomeButton_Click(object sender, EventArgs e)
    {
        AppSession.IsAdmin = false;
        AppSession.User = null;
        Response.Redirect("../Default.aspx");
    }

    /// <summary>
    /// Raises "web config" button click event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void WebConfigButton_ServerClick(object sender, EventArgs e)
    {
        string[] keys = ConfigurationManager.AppSettings.AllKeys;

        if (keys == null || keys.Length == 0)
        {
            Response.Write("There is no AppSettings in web.config.");
            return;
        }

        Response.Write("Web.Config AppSettings values as below:<br>");

        foreach (string key in keys)
        {
            Response.Write(string.Format("[{0}] = \"{1}\".<br>", key, ConfigurationManager.AppSettings[key]));
        }

        WebServiceParameter param = WebServiceConfig.GetDefaultConfigParameter();

        if (param != null)
        {
            Response.Write(string.Format("[{0}] = \"{1}\".<br>", "webSite", param.Url));
        }
    }

    /// <summary>
    /// Raises "IIS Version" button click event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void IISVersionButton_ServerClick(object sender, EventArgs e)
    {
        Response.Write(string.Format("OS Version: {0}<br/>", Environment.OSVersion));
        Response.Write(string.Format(".NET Framework Version: {0}<br/>", Environment.Version));
        Response.Write(string.Format("IIS Server Version: {0}<br/>", Request.ServerVariables["SERVER_SOFTWARE"]));
    }

    /// <summary>
    /// Delete Files
    /// </summary>
    /// <param name="folder">Folder Name</param>
    /// <param name="isExcludeToday">The flag to remove the file created at current day or not</param>
    private void DeleteFileByFolder(string folder, bool isExcludeToday)
    {
        DirectoryInfo tempFileFolder = new DirectoryInfo(folder);

        if (!tempFileFolder.Exists || tempFileFolder.GetFiles("*").Length < 1)
        {
            Response.Write("<br/><br/>There is no file in the folder " + tempFileFolder + ".<br/>");
        }
        else
        {
            Response.Write("<br/><br/>In the folder " + folder + ", below files are removed:<hr/>");

            foreach (FileInfo file in tempFileFolder.GetFiles("*"))
            {
                if (file.Exists)
                {
                    if (isExcludeToday)
                    {
                        if (DateTime.Now.ToShortDateString() != file.CreationTime.ToShortDateString())
                        {
                            File.Delete(file.FullName);
                            Response.Write("File Name:" + file.Name + ".<br/>");
                        }
                    }
                    else
                    {
                        File.Delete(file.FullName);
                        Response.Write("File Name:" + file.Name + ".<br/>");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Show Files
    /// </summary>
    /// <param name="folder">Folder Name</param>
    private void ShowFilesByFolder(string folder)
    {
        DirectoryInfo tempFileFolder = new DirectoryInfo(folder);
        if (!tempFileFolder.Exists || tempFileFolder.GetFiles("*").Length < 1)
        {
            Response.Write("<br/><br/>There is no file in the folder " + folder + ".");
        }
        else
        {
            Response.Write("<br/><br/>In the folder " + folder + ", there are some temporary info files:<br/>");

            foreach (FileInfo file in tempFileFolder.GetFiles("*"))
            {
                string name = file.FullName;
                long size = file.Length;
                DateTime creationTime = file.CreationTime;
                Response.Write(string.Format("File Name:{0}; Size:[{1}B; Creat Time:{2}.<br/>", name, creationTime, size));
            }
        }
    }
}