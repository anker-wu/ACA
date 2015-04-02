#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EmseEventType.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EmseEventType.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Common
{
    #region Enumerations

    /// <summary>
    /// Emse For PageFlow
    /// </summary>
    public enum EmseEventType
    {
        /// <summary>
        /// on load event
        /// </summary>
        OnloadEvent = 1,

        /// <summary>
        /// event before button  click
        /// </summary>
        BeforeButtonEvent = 2,

        /// <summary>
        /// event after button click
        /// </summary>
        AfterButtonEvent = 3
    }

    #endregion Enumerations
}