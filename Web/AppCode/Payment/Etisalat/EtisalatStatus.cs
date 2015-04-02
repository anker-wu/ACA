#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EtisalatStatus.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EtisalatStatus.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  1-2-2009           Xinter Peng               Initial
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Payment
{
    #region Enumerations

    /// <summary>
    /// ETISALAT Status
    /// </summary>
    public enum EtisalatStatus
    {
        /// <summary>
        /// Registration success
        /// </summary>
        RegistrationSucceeded,

        /// <summary>
        /// Registration failed
        /// </summary>
        RegistrationFailed,

        /// <summary>
        /// Completion success
        /// </summary>
        CompletionSucceeded,

        /// <summary>
        /// Completion failed
        /// </summary>
        CompletionFailed,

        /// <summary>
        /// The registered
        /// </summary>
        Registered,
        
        /// <summary>
        /// The paid
        /// </summary>
        Paid,

        /// <summary>
        /// The unknown
        /// </summary>
        Unknown
    }

    #endregion Enumerations
}