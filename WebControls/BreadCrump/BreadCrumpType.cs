#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: BreadCrumpType.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: BreadCrumpType.cs 131439 2009-05-19 11:07:26Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.Web.Controls
{
    #region Enumerations

    /// <summary>
    /// Indicate from which page 
    /// </summary>
    [Serializable]
    public enum PageFrom
    {
        /// <summary>
        /// from resume application
        /// </summary>
        ResumeApplication = 1,

        /// <summary>
        /// from resume fee estimate
        /// </summary>
        ResumeFeeEstimate = 2,

        /// <summary>
        /// from page fee due
        /// </summary>
        PayFeeDue = 3,

        /// <summary>
        /// from other pages
        /// </summary>
        Normal = 4
    }

    #endregion Enumerations
}