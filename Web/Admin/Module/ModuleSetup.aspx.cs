#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ModuleSetup.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ModuleSetup.aspx.cs 131973 2009-05-22 01:07:37Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Web.Admin
{
    /// <summary>
    /// module setup page
    /// </summary>
    public partial class ModuleSetup : AdminBasePage
    {
        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "exec", "<script type='text/javascript'>GetModuleSetupLabelKeyInfo();</script>");
            }
        }

        #endregion Methods
    }
}