#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Error4Popup.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: The error page when popup page occur an error.
 *
 *  Notes:
 *      $Id: Error4Popup.aspx.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;

namespace Accela.ACA.Web
{
    /// <summary>
    /// The error page when popup page occur an error.
    /// </summary>
    public partial class Error4Popup : Error
    {
        /// <summary>
        /// Hand OnLoad event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnLoad(EventArgs e)
        {
            systemErrorMessage.ShowWithText(MessageType.Error, ErrorMessage, MessageSeperationType.Both);
        }

        /// <summary>
        /// Change the master page which this page would not contain any master page.
        /// </summary>
        protected override void ChangeMasterPage()
        {
        }
    }
}
