/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAdminConfigurationPreview.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 *
 *  Description:
 *  Interface define for admin.
 *
 *  Notes:
 * $Id: IAdminConfigurationPreview.cs 130467 2009-05-13 12:28:31Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.BLL.Admin
{
    /// <summary>
    /// Interface define for admin.
    /// </summary>
    public interface IAdminConfigurationPreview
    {
        #region Methods

        /// <summary>
        /// Set CapModel dummy data for admin page configuration preview
        /// </summary>
        void SetPreviewCapModelDummyData();

        /// <summary>
        /// Set admin configuration temp data into session for preview
        /// </summary>
        void SetPreviewConfigurationTempData();

        #endregion Methods
    }
}