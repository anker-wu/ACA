#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PrinterServer.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
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
using System.Management;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

using Accela.AuthorizedAgent.Common.Common;
using Accela.AuthorizedAgent.Common.Downloader;
using Accela.AuthorizedAgent.Common.Setting;

namespace Accela.AuthorizedAgent.Common.HttpServer
{
    /// <summary>
    /// The printer server
    /// </summary>
    public class PrinterServer : HttpServer
    {
        #region Fields

        /// <summary>
        /// The query string for printer
        /// </summary>
        private static string QUERY_PRINTER = "Select * from Win32_Printer Where WorkOffline = False";
        
        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterServer" /> class.
        /// </summary>
        /// <param name="port">The port.</param>
        public PrinterServer(int port)
            : base(port)
        {
        }

        /// <summary>
        /// Handles the GET request.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public override void HandleGETRequest(HttpProcessor processor)
        {
            string action = HttpServerHelper.QueryValueByKey("action", processor.HttpUrl);
            string callbackFunName = HttpServerHelper.QueryValueByKey("callback", processor.HttpUrl);

            Log.Instance.Write(this.GetType(), LogType.Debug, "The request url is " + processor.HttpUrl);

            try
            {
                if (action.Equals("PrinterList", StringComparison.CurrentCultureIgnoreCase))
                {
                    StringBuilder printers = new StringBuilder();
                    printers.Append(callbackFunName + "({\"Printers\":[");

                    Log.Instance.Write("PrinterList", LogType.Debug, "1, Starts to printer list");

                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(QUERY_PRINTER);
                    ManagementObjectCollection printerList = searcher.Get();

                    Log.Instance.Write("PrinterList", LogType.Debug, "2, " + printerList.Count + " printers are found.");

                    if (printerList.Count > 0)
                    {
                        int index = 1;

                        foreach (ManagementObject printer in printerList)
                        {
                            printers.AppendFormat("\"{0}\",", printer.Properties["DriverName"].Value);

                            Log.Instance.Write("PrinterList", LogType.Debug, "Printer[" + (index++) + "]: " + printer.Properties["DriverName"].Value);
                        }

                        printers.Length -= 1;
                    }
                    
                    printers.Append("]})");
                    DoJsonpResponse(processor, printers);

                    Log.Instance.Write("PrinterList", LogType.Debug, "3, Ends to printer list");
                }
                else if (Constant.ACTION_PRINT_REPORT.Equals(action, StringComparison.CurrentCultureIgnoreCase))
                {
                    Log.Instance.Write("PrintReport", LogType.Debug, "1, Starts to print report");
                    string savePath = Path.GetTempPath() + "print.pdf";

                    string reportLocation = HttpServerHelper.QueryValueByKey("filepath", processor.HttpUrl);
                    FileDownloader fileDownloader = new FileDownloader();
                    fileDownloader.onShowMessage += FileDownloaderOnOnShowMessage;
                    Log.Instance.Write("PrintReport", LogType.Debug, "2, The url of report file is " + reportLocation);

                    ProxyServerSetting setting = SerializationUtil.ConfigureProxySetting();
                    fileDownloader.DownloadFile(reportLocation, string.Empty, savePath, true, setting);
                    Log.Instance.Write("PrintReport", LogType.Debug, "3, Send the report file to printer[" + ConfigManager.PrinterName + "], the file path is " + savePath);

                    Print(ConfigManager.PrinterName, savePath);
                    Log.Instance.Write("PrintReport", LogType.Debug, "4, Print the report file successfully");

                    File.Delete(savePath);
                    Log.Instance.Write("PrintReport", LogType.Debug, "5, Remove the report file successfully");

                    Log.Instance.Write("PrintReport", LogType.Debug, "6, Send the request to remove the temp report file on ACA web server, the request url is " + reportLocation + "&action=Delete");
                    HttpWebRequest request = WebRequest.Create(reportLocation + "&action=Delete") as HttpWebRequest;
                    request.Method = "GET";
                    HttpUtility.AddProxy(setting, request);
                    WebResponse response = request.GetResponse();
                    response.Close();

                    StringBuilder printStatus = new StringBuilder();
                    printStatus.Append(callbackFunName + "({\"PrintStatus\":");
                    printStatus.Append("\"Success\"");
                    printStatus.Append("})");

                    DoJsonpResponse(processor, printStatus);
                    Log.Instance.Write("PrintReport", LogType.Debug, "7, Ends to print report");
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Write(this.GetType(), LogType.Error, "Failure to handle one request: " + processor.HttpUrl);
                Log.Instance.Write(this.GetType(), LogType.Error, "The detailed error information:\n" + ex);
                ShowMessageEx(ex.Message);
                Result result = new Result();
                result.Message = ex.Message;
                DoJsonpResponse(processor, new StringBuilder(new JavaScriptSerializer().Serialize(result)));   
            }
        }

        /// <summary>
        /// Handle post request for SOAP format. 
        /// </summary>
        /// <param name="processor">A instance of HttpProcessor</param>
        /// <param name="inputData">input data stream.</param>
        public override void HandlePOSTRequest(HttpProcessor processor, StreamReader inputData)
        {
        }

        /// <summary>
        /// Does the jsonp response.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="jsonpData">The jsonp data.</param>
        private void DoJsonpResponse(HttpProcessor processor, StringBuilder jsonpData)
        {
            processor.OutputStream.Write("HTTP/1.1 200 OK\r\n");
            processor.OutputStream.Write("Cache-Control: private, max-age=0\r\n");
            processor.OutputStream.Write("Date: " + DateTime.UtcNow + "\r\n");
            processor.OutputStream.Write("Server: Microsoft-IIS/7.5\r\n");
            processor.OutputStream.Write("X-AspNet-Version: 4.0.30319\r\n");
            processor.OutputStream.Write("X-Powered-By: ASP.NET\r\n");
            processor.OutputStream.Write("Connection: keep-alive\r\n");
            processor.OutputStream.Write("Content-Type: application/json; charset=utf-8\r\n");
            processor.OutputStream.Write("Content-Length: {0}\r\n\r\n", System.Text.Encoding.UTF8.GetByteCount(jsonpData.ToString()));
            processor.OutputStream.Write(jsonpData.ToString());
            processor.OutputStream.Flush();
            processor.OutputStream.Close();
            processor.Socket.Close();
            processor.Socket = null;
            processor.OutputStream = null;
        }

        /// <summary>
        /// Prints the specified printer name.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <param name="fileName">Name of the file.</param>
        private void Print(string printerName, string fileName)
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                string path = AppDomain.CurrentDomain.BaseDirectory;
                proc.StartInfo.FileName = Path.Combine(path, "SumatraPDF.exe");
                proc.StartInfo.WorkingDirectory = path;
                proc.StartInfo.Arguments = "-print-to " + '"' + printerName + '"' + " " + '"' + fileName + '"';
                proc.StartInfo.RedirectStandardError = false;
                proc.StartInfo.RedirectStandardOutput = false;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Log.Instance.Write(this.GetType(), LogType.Error, "Failed to print file. The detailed error information: " + ex);
            }
        }

        /// <summary>
        /// Files the downloader on on show message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        private void FileDownloaderOnOnShowMessage(string message, string exception)
        {
            if (string.IsNullOrEmpty(message))
            {
                ShowMessageEx(message);
            }
            else
            {
                ShowMessageEx(exception);
            }
        }
    }
}
