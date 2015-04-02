#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FileDownloader.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description: 
 *
 *  Notes:
 *      
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.Net;
using Accela.AuthorizedAgent.Common.Common;
using Accela.AuthorizedAgent.Common.Setting;
using System.Net.Security;

namespace Accela.AuthorizedAgent.Common.Downloader
{
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// The file downloader.
    /// </summary>
    public class FileDownloader
    {
        /// <summary>
        /// the file ext name for migration.
        /// </summary>
        private static string FILE_EXT_NAME_MIGRATION = "migration";

        /// <summary>
        /// the buffer size
        /// </summary>
        private static int BUFFER_SIZE = 8192;

        /// <summary>
        /// Download process
        /// </summary>
        /// <param name="total">The total.</param>
        /// <param name="current">The current.</param>
        public delegate void DownloadProgress(long total, long current);

        /// <summary>
        /// show message when downloading
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public delegate void ShowMessage(string message, string exception);

        /// <summary>
        /// Occurs when [on down load progress].
        /// </summary>
        public event DownloadProgress onDownLoadProgress;

        /// <summary>
        /// Occurs when [on show message].
        /// </summary>
        public event ShowMessage onShowMessage;

        /// <summary>
        /// download file by a URL
        /// </summary>
        /// <param name="url">URL for download</param>
        /// <param name="cookieHeader">The cookie header.</param>
        /// <param name="filename">local file name with full path.</param>
        /// <param name="onProgress">is on progress or not.</param>
        /// <param name="setting">the proxy server setting.</param>
        /// <returns></returns>
        public string DownloadFile(string url, string cookieHeader, string filename, bool onProgress, ProxyServerSetting setting)
        {
            string publishDate = null;
            return DownloadFile(url, cookieHeader, filename, onProgress, out publishDate, setting);
        }

        /// <summary>
        /// Remotes the certificate validation callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslPolicyErrors">The SSL policy errors.</param>
        /// <returns></returns>
        public static bool RemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //Return True to force the certificate to be accepted.
            return true;
        }

        /// <summary>
        /// download file by a URL
        /// </summary>
        /// <param name="url">URL for download</param>
        /// <param name="cookieHeader">The cookie header.</param>
        /// <param name="filename">local file name with full path.</param>
        /// <param name="onProgress">if set to <c>true</c> [on progress].</param>
        /// <param name="publishDate">The publish date.</param>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
        public string DownloadFile(string url, string cookieHeader, string filename, bool onProgress, out string publishDate,ProxyServerSetting setting)
        {
            string result = filename;
            HttpWebResponse response = null;
            System.IO.Stream streamRead = null;
            System.IO.Stream streamWrite = null;
            publishDate = String.Empty;

            try
            {
                if (url.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidationCallback);
                }

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                request.Timeout = Constant.CONNECT_TIMEOUT;
                CookieContainer cookieContainer = new CookieContainer();
                cookieContainer.PerDomainCapacity = 60;
                cookieContainer.SetCookies(request.RequestUri, cookieHeader);
                request.CookieContainer = cookieContainer;
                request.Method = WebRequestMethods.Http.Get;
                request.KeepAlive = false;
                request.ReadWriteTimeout = Constant.CONNECT_TIMEOUT;
                request.ServicePoint.Expect100Continue = false;
                request.AllowAutoRedirect = false;
                request.UserAgent = "Accela Reviewer";
                request.ProtocolVersion = HttpVersion.Version11;

                HttpUtility.AddProxy(setting, request);

                response = (HttpWebResponse)request.GetResponse();

                if (!IsLogin(response))
                {
                    if (onShowMessage != null)
                    {
                        onShowMessage("Please login AA prior to download file.", null);
                    }

                    return null;
                }

                publishDate = response.GetResponseHeader("publishDate");

                string fileExtName = getDownloadedFileExtName(response);

                if (FILE_EXT_NAME_MIGRATION.Equals(fileExtName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return fileExtName;
                }

                if (!string.IsNullOrEmpty(fileExtName))
                {
                    filename = filename.Substring(0, filename.LastIndexOf(".")) + "." + fileExtName;
                }
                else
                {
                    //When the exception thrown at server side, there is no file downloaded.
                    //In this case, we try to get the detailed message for display.
                    streamRead = response.GetResponseStream();
                    StreamReader reader = new StreamReader(streamRead);
                    String contentSnippet = reader.ReadToEnd();
                    contentSnippet = contentSnippet.Trim();
                    Log.Instance.Write(this.GetType(), LogType.Info, contentSnippet);
                    if (onShowMessage != null)
                    {
                        onShowMessage("File download failed.", contentSnippet);
                    }

                    return null;
                }

                result = filename;
                streamRead = response.GetResponseStream();

                streamWrite = new FileStream(filename, FileMode.Create);
                long totalDownloadedByte = 0;
                long totalBytes = response.ContentLength;

                byte[] buffer = new byte[BUFFER_SIZE];

                while (totalBytes > totalDownloadedByte)
                {
                    int length = streamRead.Read(buffer, 0, BUFFER_SIZE);
                    totalDownloadedByte += length;

                    if (length > 0)
                    {
                        streamWrite.Write(buffer, 0, length);
                    }

                    if (onProgress && onDownLoadProgress != null)
                    {
                        if (totalDownloadedByte >= totalBytes)
                        {
                            onDownLoadProgress(totalBytes, totalBytes - 1);
                        }
                        else
                        {
                            onDownLoadProgress(totalBytes, totalDownloadedByte);
                        }
                    }
                }

                // set progress bar to 100%
                if (onProgress && onDownLoadProgress != null)
                {
                    onDownLoadProgress(totalBytes, totalBytes);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Write(this.GetType(), LogType.Error, ex);

                if (onShowMessage != null)
                {
                    onShowMessage("File download failed:" + ex.Message, ex.ToString());
                }

                result = null;
				throw ex;
            }
            finally
            {
                try
                {
                    if (streamWrite != null)
                    {
                        streamWrite.Close();
                    }
                }
                catch (Exception e)
                {
                    Log.Instance.Write(this.GetType(), LogType.Error, e);
                }

                try
                {
                    if (streamRead != null)
                    {
                        streamRead.Close();
                    }
                }
                catch (Exception e)
                {
                    Log.Instance.Write(this.GetType(), LogType.Error, e);
                }

                try
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
                catch (Exception e)
                {
                    Log.Instance.Write(this.GetType(), LogType.Error, e);
                }
            }

            return result;
        }

        /// <summary>
        /// Indicates whether the connecting is logined according to the response header.
        /// </summary>
        /// <param name="response">the HttpWebResponse object.</param>
        /// <returns>true-is login,false-not login or session is timeout</returns>
        private static bool IsLogin(HttpWebResponse response)
        {
            bool isLogin = true;
            WebHeaderCollection headers = response.Headers;
            string setCookieHeader = "Set-Cookie";
            string newSessionInfo = null;
            string newSessionId = null;
            string downloadedFileName = response.GetResponseHeader("Content-Disposition");

            if (downloadedFileName != null)
            {
                return true;
            }

            newSessionInfo = response.GetResponseHeader(setCookieHeader);

            // found session from http header
            if (!String.IsNullOrEmpty(newSessionInfo))
            {
                string sessionId = "JSESSIONID=";
                int beginPos = newSessionInfo.IndexOf(sessionId, StringComparison.InvariantCultureIgnoreCase);

                if (beginPos > -1)
                {
                    beginPos = beginPos + sessionId.Length;
                    int endPos = newSessionInfo.IndexOf(";", beginPos);
                    newSessionId = newSessionInfo.Substring(beginPos, endPos - beginPos);
                }

                if (!String.IsNullOrEmpty(newSessionId))
                {
                    isLogin = false;
                }
            }

            return isLogin;
        }

        /// <summary>
        /// Gets the name of the downloaded file ext.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>file ext name</returns>
        private static string getDownloadedFileExtName(HttpWebResponse response)
        {
            string downloadedFileName = response.GetResponseHeader("Content-Disposition");
            string fileExtName = null;
            if (downloadedFileName != null && downloadedFileName.IndexOf("attachment;") >= 0 && downloadedFileName.IndexOf("filename=") >= 0)
            {
                fileExtName = downloadedFileName.Substring(downloadedFileName.IndexOf("filename=") + 9);
                fileExtName = fileExtName.Substring(fileExtName.LastIndexOf(".") + 1);

                if (fileExtName != null && fileExtName.LastIndexOf("\"") > 0)
                {
                    fileExtName = fileExtName.Substring(0, fileExtName.LastIndexOf("\""));
                }

                if (fileExtName != null && fileExtName.LastIndexOf("'") > 0)
                {
                    fileExtName = fileExtName.Substring(0, fileExtName.LastIndexOf("'"));
                }

                if (fileExtName != null && (fileExtName.EndsWith(";") || fileExtName.EndsWith(",")))
                {
                    fileExtName = fileExtName.Substring(0, fileExtName.Length - 1);
                }

                if (fileExtName != null && fileExtName.Length == 0)
                {
                    fileExtName = null;
                }
            }

            return fileExtName;
        }
    }
}
