#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FileSelected.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *      $Id: FileSelected.ascx.cs 196284 2014-08-20 10:27:27Z ACHIEVO\Canon.Wu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.Script.Serialization;

using Accela.ACA.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// FileUpload Control.
    /// </summary>
    public partial class FileUpload : FileUploadBase
    {
        /// <summary>
        /// Remove all files function name
        /// </summary>
        private const string REMOVE_ALL_FILES = "_RemoveAllFiles";

        /// <summary>
        /// Remove single files function name
        /// </summary>
        private const string REMOVE_SINGLE_FILE = "_RemoveSingleFile";

        #region Parameters for Upload control

        /// <summary>
        /// Gets or sets a value indicating whether display button or link button or not.
        /// </summary>
        public bool IsDisplayLinkButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether display resubmit icon button or not.
        /// </summary>
        public bool IsDisplayResubmitIconButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value to record error message showed container.
        /// </summary>
        public string ErrorMsgContainerId
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value to get advance button width.
        /// </summary>
        public string AdvanceButtonWidth
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value to run in start upload function.
        /// </summary>
        public string FunctionRunInStartUpload
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value to run in all files finished event.
        /// </summary>
        public string FunctionRunInAllFilesFinished
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value to run in select files complete event.
        /// </summary>
        public string FunctionRunInSelectCompleted
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets a value to run in remove single file event.
        /// </summary>
        public string FunctionRemoveSingleFile
        {
            get
            {
                return ClientID + REMOVE_SINGLE_FILE;
            }
        }

        /// <summary>
        /// Gets a value to run in remove all file event.
        /// </summary>
        public string FunctionRemoveAllFile
        {
            get
            {
                return ClientID + REMOVE_ALL_FILES;
            }
        }

        /// <summary>
        /// Gets or sets a value to show silverlight button text.
        /// </summary>
        public string SilverlightButtonLabelKey
        {
            get;
 
            set;
        }

        /// <summary>
        /// Gets Initial Parameters for Upload control.
        /// </summary>
        public string InitParams
        {
            get
            {
                return string.Format(
                    "{1}{0}isDisplayLinkButton={2}{0}isDisplayResubmitIcoButton={3}",
                    ACAConstant.COMMA,
                    base.InitParams,
                    IsDisplayLinkButton ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                    IsDisplayResubmitIconButton ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
            }
        }

        #endregion

        /// <summary>
        /// Update document state when post back.
        /// </summary>
        /// <param name="document">document model</param>
        /// <returns>Document Model</returns>
        public DocumentModel UpdateFileState(DocumentModel document)
        {
            if (!string.IsNullOrEmpty(hdFinishedFileArray.Value) && document != null)
            {
                var jsSerializer = new JavaScriptSerializer();
                var fileInfoList = jsSerializer.Deserialize<FileUploadInfo[]>(hdFinishedFileArray.Value);

                foreach (var uploadInfo in fileInfoList)
                {
                    if (document.FileId.Equals(uploadInfo.FileId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        document.FileState = uploadInfo.StateString;
                    }
                }
            }

            return document;
        }
    }
}