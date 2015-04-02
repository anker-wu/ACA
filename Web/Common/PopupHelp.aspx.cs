#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PopupHelp.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PopupHelp.aspx.cs 189549 2011-01-25 09:51:54Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Pop up help
    /// </summary>
    public partial class PopupHelp : PopupDialogBasePage
    {
        /// <summary>
        /// Gets or sets the additional text ID.
        /// </summary>
        /// <value>The additional text ID.</value>
        protected string AdditionalTextID
        {
            get;
            set;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey(Request["titleLabelKey"]);
            this.lblHelpText.LabelKey = Request["contentLabelKey"];
            this.AdditionalTextID = Request["additionalTextID"];
        }
    }
}
