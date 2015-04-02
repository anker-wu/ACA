#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PasswordSecurityBar.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PasswordSecurityBar.ascx.cs 169604 2010-03-30 09:59:38Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// display password strength in account manager page and register page.
    /// </summary>
    public partial class PasswordSecurityBar : BaseUserControl
    {
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // load password requirement
            IAccountBll accountBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));
            passwordRequirements.Text = accountBll.GetPasswordRequirement(ConfigManager.AgencyCode);
        }
    }
}
