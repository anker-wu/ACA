#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: OwnerListSelectEventHandler.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: OwnerListSelectEventHandler.cs 170366 2014-07-30 05:34:25Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.Web
{
    #region Delegates

    /// <summary>
    /// Delegate for OwnerListSelectEventHandler
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">OwnerListSelectEventArgs object</param>
    public delegate void OwnerListSelectEventHandler(object sender, OwnerListSelectEventArgs e);

    #endregion Delegates

    #region OwnerListSelectEventArgs

    /// <summary>
    /// Owner list event args contains selected owner model.
    /// </summary>
    public class OwnerListSelectEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the selected owner model.
        /// </summary>
        public OwnerModel SelectedOwner { get; set; }
    }

    #endregion OwnerListSelectEventArgs
}