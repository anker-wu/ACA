#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UploadAttachmentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: UploadAttachmentBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Plan
{
    /// <summary>
    /// This class provide the ability to operation upload attachment.
    /// </summary>
    public class UploadAttachmentBll : BaseBll, IUploadAttachmentBll
    {
        #region Fields

        /// <summary>
        /// Upload attachment.
        /// </summary>
        private readonly UploadAttachmentWS _uploadAttachmentWS = new UploadAttachmentWS();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Upload Attachment
        /// </summary>
        /// <param name="typeName">the file type name.</param>
        /// <param name="fileName">the file name</param>
        /// <param name="filePath">the file path.</param>
        /// <returns>successfully message.</returns>
        public string UploadAttachment(string typeName, string fileName, string filePath)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(fileName))
            {
                throw new DataValidateException(new string[] { "typeName", "fileName" });
            }

            try
            {
                UploadModel4WS uploadModel = new UploadModel4WS();
                uploadModel.fileName = fileName;
                uploadModel.dataHandler = System.IO.File.ReadAllBytes(filePath);

                UploadResult4WS result = _uploadAttachmentWS.UploadAttachment(uploadModel);
                return result.fileName;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}