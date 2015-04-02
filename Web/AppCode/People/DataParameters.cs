#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DataParameters.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: Provide the session model for contact.
 *
 *  Notes:
 *      $Id: DataParameters.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// Data Parameter in Contact Session parameter
    /// </summary>
    [Serializable]
    public class DataParameters
    {
        /// <summary>
        /// Gets or sets Data in session for process.
        /// </summary>
        /// <value>
        /// The data object.
        /// </value>
        public object DataObject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Contact Address Row Index
        /// </summary>
        public int? ContactAddressRowIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current contact match with the contact in database.
        /// </summary>
        /// <value>
        ///   <c>true</c> if match; otherwise, <c>false</c>.
        /// </value>
        public bool IsCloseMatch
        {
            get;
            set;
        }
    }
}