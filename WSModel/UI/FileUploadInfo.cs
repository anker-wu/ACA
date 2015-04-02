#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: FileUploadInfo.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: Information class of uploaded file.
*
*  Notes:
* $Id: FileUploadInfo.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using Accela.ACA.WSProxy;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// Information class of uploaded file.
    /// </summary>
    [Serializable]
    public class FileUploadInfo
    {
        /// <summary>
        /// Gets or sets file name.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets file identification.
        /// </summary>
        public string FileId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets file Size.
        /// </summary>
        public long FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets file state.
        /// </summary>
        public string StateString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets max file size.
        /// </summary>
        public long MaxFileSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets parent document number.
        /// </summary>
        public string ParentDocumentNo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets parent agency code.
        /// </summary>
        public string ParentAgencyCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets document model.
        /// </summary>
        public DocumentModel DocumentModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets File selected ID.
        /// </summary>
        public string FileSelectedID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Html5 Uploader Name(especially for resubmit).
        /// </summary>
        public string UploaderName
        {
            get;
            set;
        }
    }
}
