#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ActionViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ActionViewModel.cs 199755 2011-07-19 10:23:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// Action view model for action column creation.
    /// </summary>
    [Serializable]
    public class ActionViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether dispaly line
        /// </summary>
        public bool SeparateLine
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hyper link.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is hypper link; otherwise, <c>false</c>.
        /// </value>
        public bool IsHyperLink
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the action label.
        /// </summary>
        /// <value>The action label.</value>
        public string ActionLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ClientEvent
        /// </summary>
        public string ClientEvent
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set IcoUrl
        /// </summary>
        public string IcoUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Action Id
        /// </summary>
        public string ActionId
        {
            get;
            set;
        }
    }
}
