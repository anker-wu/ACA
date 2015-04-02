#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProxyPopupSettings.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ProxyPopupSettings.aspx.cs 184998 2010-11-22 05:51:33Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Proxy Popup Setting.
    /// </summary>
    public partial class ProxyPopupSettings : BasePage
    {
        /// <summary>
        /// page load method.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgEditClose.Src = imgViewClose.Src = imgCreateClose.Src = ImageUtil.GetImageURL("closepopup.png");
            }
        }
    }
}
