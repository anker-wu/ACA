#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PreAccountVerification.aspx.cs 139024 2009-07-14 10:00:30Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Configuration;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Page to present pre account verification
    /// </summary>
    public partial class PreAccountVerification : Page
    {
        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //in order to load into bridgeview.aspx Iframe.
            string defaultPage = FileUtil.AppendApplicationRoot(ConfigurationManager.AppSettings["DefaultPageFile"]);
            Session[ACAConstant.CURRENT_URL] = FileUtil.AppendApplicationRoot(ACAConstant.URL_ACCOUNT_VERIFICATION);
            Server.Transfer(defaultPage);
        }

        #endregion Methods
    }
}