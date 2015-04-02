#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AsynchronousUpload.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: AsynchronousUpload.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Timers;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.Attachment
{
    /// <summary>
    /// provide asynchronous upload file from web server to app server.
    /// </summary>
    internal class AsynchronousUpload
    {
        #region Fields

        /// <summary>
        /// single pattern.
        /// </summary>
        public static readonly AsynchronousUpload Instance = new AsynchronousUpload();

        /// <summary>
        /// max upload times.
        /// </summary>
        private const int MAX_UPLOAD_TIMES = 3;

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AsynchronousUpload));

        /// <summary>
        /// the interval for time elapsed(unit microsecond)
        /// </summary>
        private static readonly int TimeInterval = 2000;

        /// <summary>
        /// failed director.
        /// </summary>
        private static string _failedDirectory = null;

        /// <summary>
        /// the thread lock
        /// </summary>
        private static object _threadlock = new object();

        /// <summary>
        /// temp info directory.
        /// </summary>
        private static string _tempInfoDirectory = null;

        /// <summary>
        /// failed count.
        /// </summary>
        private Hashtable _failedCount;        

        /// <summary>
        /// distinct the timer can stop
        /// </summary>
        private bool _canTimerStop = false;

        /// <summary>
        /// the timer for execute upload file event.
        /// </summary>
        private Timer _timer = new Timer();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the AsynchronousUpload class from being created.
        /// </summary>
        private AsynchronousUpload()
        {
            _failedCount = new Hashtable();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the failed upload files directory. 
        /// </summary>
        private static string FailedDirectory
        {
            get
            {
                if (_failedDirectory == null)
                {
                    _failedDirectory = Path.Combine(GetTempDir(), "FailedFiles");

                    if (!Directory.Exists(_failedDirectory))
                    {
                        Directory.CreateDirectory(_failedDirectory);
                    }
                }

                return _failedDirectory;
            }
        }

        /// <summary>
        /// Gets the temporary information directory.
        /// </summary>
        /// <value>The temporary information directory.</value>
        private static string TempInfoDirectory
        {
            get
            {
                if (_tempInfoDirectory == null)
                {
                    _tempInfoDirectory = Path.Combine(GetTempDir(), "TempFiles");

                    if (!Directory.Exists(_tempInfoDirectory))
                    {
                        Directory.CreateDirectory(_tempInfoDirectory);
                    }
                }

                return _tempInfoDirectory;
            }
        }

        /// <summary>
        /// Gets an instance of EDMSDocumentUploadService.
        /// </summary>
        private EDMSDocumentUploadWebServiceService EDMSDocumentUploadService
        {
            get
            {
                return WSFactory.Instance.GetWebService3<EDMSDocumentUploadWebServiceService>();
            }
        }

        /// <summary>
        /// Gets the ICommonData instance.
        /// </summary>
        private ICommonData CommonData
        {
            get
            {
                return ObjectFactory.GetObject(typeof(ICommonData)) as ICommonData;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Temp Info File Name.
        /// </summary>
        /// <param name="fileName">the file name.</param>
        /// <returns>temp info file name.</returns>
        internal static string GetTempInfoFileName(string fileName)
        {
            return Path.Combine(TempInfoDirectory, GetFileNameFromFullName(fileName));
        }

        /// <summary>
        /// Start the timer to start upload files
        /// </summary>
        internal void Start()
        {
            if (!_timer.Enabled)
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Try to start async file upload timer.");
                }

                _timer.Interval = TimeInterval;
                _timer.Elapsed += new ElapsedEventHandler(DoUpload);

                _timer.Start();
                _canTimerStop = false;
            }
        }

        /// <summary>
        /// Stop the timer to stop the upload files
        /// </summary>
        internal void Stop()
        {
            if (_timer.Enabled)
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Try to stop async file upload timer.");
                }

                _canTimerStop = true;
            }
        }

        /// <summary>
        /// Get File Name From Full Name.
        /// </summary>
        /// <param name="fullName">the full name.</param>
        /// <returns>the file name</returns>
        private static string GetFileNameFromFullName(string fullName)
        {
            string[] temp = fullName.Split('\\');

            return temp[temp.Length - 1];
        }

        /// <summary>
        /// get the temp directory
        /// </summary>
        /// <returns>Temp Directory</returns>
        private static string GetTempDir()
        {
            string tempDir = ConfigurationManager.AppSettings["TempDirectory"];

            if (string.IsNullOrEmpty(tempDir) || tempDir.Trim() == string.Empty)
            {
                tempDir = ACAConstant.DEFAULT_TEMP_DIRECTORY;
            }

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            return tempDir;
        }
        
        /// <summary>
        /// scan the directory and upload files when time elapsed
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the elapsed event arguments</param>
        private void DoUpload(object sender, ElapsedEventArgs e)
        {
            lock (_threadlock)
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Async file upload timer is running.");
                }

                string[] files = Directory.GetFiles(GetTempDir());

                // scan the target directory to do upload 
                try
                {
                    foreach (string fileName in files)
                    {
                        DoUploadSingle(fileName);
                    }
                }
                catch (Exception ex)
                {
                    if (Logger.IsErrorEnabled)
                    {
                        Logger.ErrorFormat("Failed upload file: {0}", ex);
                    }
                }

                // if application set the flag to stop the timer, stop it.
                if (_canTimerStop)
                {
                    _timer.Stop();

                    if (Logger.IsDebugEnabled)
                    {
                        Logger.Debug("Async file upload timer is stopped.");
                    }
                }
            }            
        }

        /// <summary>
        /// Upload single file called by DoUpload method
        /// </summary>
        /// <param name="fileName">the file name</param>
        private void DoUploadSingle(string fileName)
        {
            string fn = GetFileNameFromFullName(fileName);
            string infoFileName = Path.Combine(TempInfoDirectory, fn);

            // if the info file does not exists, don't delete relative file, only return. because:
            // 1. there is the Export file in the directory. 2. the info file is generated later.
            if (!File.Exists(infoFileName))
            {
                return;
            }

            AttachmentModel um = null;
            using (StreamReader sr = File.OpenText(infoFileName))
            {
                um = (AttachmentModel)SerializationUtil.XmlDeserialize(sr.ReadToEnd(), typeof(AttachmentModel));
                sr.Close();
            }

            if (um == null)
            {
                return;
            }

            string serviceProviderCode = (um.DocumentModel != null && !string.IsNullOrEmpty(um.DocumentModel.serviceProviderCode)) ? um.DocumentModel.serviceProviderCode : ConfigManager.AgencyCode;
            string entityID = (um.DocumentModel != null && um.DocumentModel.entityID != null) ? um.DocumentModel.entityID : string.Empty;
            string entityType = (um.DocumentModel != null && um.DocumentModel.entityType != null) ? um.DocumentModel.entityType : string.Empty;

            Logger.DebugFormat(
                "Upload document asynchronously. \nEntityID: {0}\nEntityType: {1}\nInfo File Name:{2}\nDoc Name:{3}\nFile Name:{4}",
                serviceProviderCode + "-" + entityID,
                entityType,
                infoFileName,
                um.DocumentModel.docName,
                um.DocumentModel.fileName);

            // read all data into memory
            um.DocumentModel.documentContent.docContentStream = File.ReadAllBytes(fileName);

            EMSEResultBaseModel4WS eMSEResultBaseModel = null;

            CapIDModel4WS capId = TempModelConvert.Add4WSForCapIDModel(um.DocumentModel.capID);

            try
            {
                if (um.IsPartialCap)
                {
                    eMSEResultBaseModel = EDMSDocumentUploadService.doUpload4PartialCap(CommonData.AgencyCode, um.ModelName, um.DocumentModel.recFulNam, capId, um.DocumentModel);
                }
                else
                {
                    eMSEResultBaseModel = EDMSDocumentUploadService.doUpload(CommonData.AgencyCode, um.ModelName, um.DocumentModel.recFulNam, um.DocumentModel, um.Category4EMSE);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Upload File failed. \nFile Name:{0}\nException:", fileName, ex);
            }

            if (eMSEResultBaseModel != null)
            {
                try
                {
                    File.Delete(fileName);
                    Logger.DebugFormat("Delete File: " + fileName);
                }
                catch (IOException ex)
                {
                    Logger.ErrorFormat("The temporary file {0} cannot be deleted due to one exception: {1}", fileName, ex);
                }

                try
                {
                    File.Delete(infoFileName);
                    Logger.DebugFormat("Delete Info File: " + infoFileName);
                }
                catch (IOException ex)
                {
                    Logger.ErrorFormat("The temporary info file {0} cannot be deleted due to one exception: {1}", fileName, ex);
                }

                RemoveCount(fileName);
            }
            else
            {
                // if exception occurs, the reponse is null
                HandleUploadFailedFile(fileName, fn, infoFileName);
            }
        }

        /// <summary>
        /// handle upload failed file
        /// </summary>
        /// <param name="fileName">file name of failed file</param>
        /// <param name="fn">the path information.</param>
        /// <param name="infoFileName">Info file name</param>
        private void HandleUploadFailedFile(string fileName, string fn, string infoFileName)
        {
            if (_failedCount.ContainsKey(fileName))
            {
                int count = (int)_failedCount[fileName];

                if (count < MAX_UPLOAD_TIMES - 1)
                {
                    _failedCount[fileName] = count + 1;
                }
                else
                {
                    File.Move(fileName, Path.Combine(FailedDirectory, fn));
                    File.Move(infoFileName, Path.Combine(FailedDirectory, fn + ACAConstant.UPLOAD_FILE_DETAILINFO));
                    RemoveCount(fileName);

                    if (Logger.IsErrorEnabled)
                    {
                        Logger.ErrorFormat("Failed upload file {0}, the file is moved to directory {1}.", fileName, FailedDirectory);
                    }
                }
            }
            else
            {
                _failedCount.Add(fileName, 1);
            }
        }

        /// <summary>
        /// Remove count.
        /// </summary>
        /// <param name="key">the key code.</param>
        private void RemoveCount(string key)
        {
            if (_failedCount.ContainsKey(key))
            {
                _failedCount.Remove(key);
            }
        }

        #endregion Methods      
    }
}