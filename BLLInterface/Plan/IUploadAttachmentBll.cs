/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IUploadAttachmentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: IUploadAttachmentBll.cs 131464 2009-05-20 01:42:02Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */
namespace Accela.ACA.BLL.Plan
{
    /// <summary>
    /// This interface's methods sign references uploaded document action.
    /// </summary>
    public interface IUploadAttachmentBll
    {
        #region Methods

        /// <summary>
        /// Upload attachment.
        /// </summary>
        /// <param name="typeName">attached file's type</param>
        /// <param name="fileName">attached file's name</param>
        /// <param name="filePath">attached file's path</param>
        /// <returns>file name of upload file</returns>
        string UploadAttachment(string typeName, string fileName, string filePath);

        #endregion Methods
    }
}