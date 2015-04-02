#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccountAddLicense.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: SearchLicense.aspx.cs 277625 2014-08-19 07:06:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// page for searching license
    /// </summary>
    public partial class SearchLicense : BasePage
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
                DropDownListBindUtil.BindLicenseType(ddlLicenseType);
            }
        }

        /// <summary>
        /// Click 'Find License' button. 
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void FindLicenseButton_Click(object sender, EventArgs e)
        {
            string licenseType = ddlLicenseType.Text.Trim();
            string licenseNbr = HttpUtility.UrlEncode(txtLicenseNum.Text.Trim());
            bool isFromAccountManager = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FROM_ACCOUNT_MANAGER]);
            string url = string.Format("LicenseList.aspx?{0}={1}&{2}={3}", UrlConstant.LICENSE_TYPE, licenseType, UrlConstant.LICENSE_NBR, licenseNbr);

            if (isFromAccountManager)
            {
                url += string.Format("&{0}={1}", UrlConstant.IS_FROM_ACCOUNT_MANAGER, ACAConstant.COMMON_Y);
            }

            Response.Redirect(url);
        }

        #endregion Methods
    }
}