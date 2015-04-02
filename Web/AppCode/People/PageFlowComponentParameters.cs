#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PageFlowComponentParameters.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: Provide the session model for contact.
 *
 *  Notes:
 *      $Id: PageFlowComponentParameters.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
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
    /// Page flow component in Contact Session Parameter
    /// </summary>
    [Serializable]
    public class PageFlowComponentParameters
    {
        /// <summary>
        /// The component IsEditable
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        /// Gets or sets Page flow's component Name.
        /// </summary>
        /// <value> The name of the component.</value>
        public string ComponentName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Page flow's component type.
        /// </summary>
        /// <value> The component id.</value>
        public PageFlowComponent ComponentID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the component data source, validation flag.
        /// </summary>
        /// <value>The component data source.</value>
        public string ComponentDataSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component is editable.
        /// </summary>
        /// <value>The component IsEditable.</value>
        public bool IsEditable
        {
            get { return _isEditable; }
            set { _isEditable = value; }
        }
    }
}