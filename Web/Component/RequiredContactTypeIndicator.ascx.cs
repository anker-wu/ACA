#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: RequiredContactTypeIndicator.ascx.cs
*
*  Accela, Inc.
*  Copyright (C): 2013
*
*  Description: An indicator for required contact type.
*
*  Notes:
* $Id$.
*  Revision History
*  Date,            Who,        What
*  Sep 20, 2013      Wanllance Zhang     Initial.
* </pre>
*/

#endregion

using System;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Required Contact Type Indicator
    /// </summary>
    public partial class RequiredContactTypeIndicator : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [is show indicator].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is show indicator]; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the component.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public string ComponentName
        {
            get;
            set;
        }

        #endregion Properties

        #region Event

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion Event

        #region Private Function

        #endregion Private Function
    }
}