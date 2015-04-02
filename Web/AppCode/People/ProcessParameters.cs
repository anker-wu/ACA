#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProcessParameters.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: Provide the session model for contact.
 *
 *  Notes:
 *      $Id: ProcessParameters.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// UI process parameters in Contact Session Parameter
    /// </summary>
    [Serializable]
    public class ProcessParameters
    {
        /// <summary>
        /// Gets or sets callback function name when success to add/edit people callback to refresh base form.
        /// </summary>
        /// <value>
        /// The parent post id.
        /// </value>
        public string CallbackFunctionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the Contact Address callback function.
        /// </summary>
        /// <value>
        /// The name of the contact address callback function.
        /// </value>
        public string CACallbackFunctionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the contact UI process, .
        /// </summary>
        /// <value>
        /// The type of the contact process.
        /// </value>
        public ContactProcessType ContactProcessType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the look up step.
        /// </summary>
        /// <value>
        /// The look up step.
        /// </value>
        public ContactLookUpStep LookUpStep
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the contact address process.
        /// </summary>
        /// <value>
        /// The type of the contact address process.
        /// </value>
        public ContactAddressProcessType ContactAddressProcessType
        {
            get;
            set;
        }
    }
}