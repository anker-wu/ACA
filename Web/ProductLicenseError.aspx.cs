/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ProductLicenseError.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ProductLicenseError.aspx.cs 277452 2014-08-15 06:36:45Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Text;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Product license error page
    /// </summary>
    public partial class ProductLicenseError : System.Web.UI.Page
    {
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"];
            if (HttpRuntime.Cache.Get(ACAConstant.PRODUCT_LICENSE_KEY) != null & !string.IsNullOrEmpty(type))
            {
                bool isAdmin = type == "1" ? true : false;
                ProductLicenseMessageInfo info = HttpRuntime.Cache.Get(ACAConstant.PRODUCT_LICENSE_KEY) as ProductLicenseMessageInfo;
                HttpRuntime.Cache.Remove(ACAConstant.PRODUCT_LICENSE_KEY);
                ShowCustomError(info, isAdmin);
            }
            else
            {
                ShowDefaultError();
            }
        }

        /// <summary>
        /// Show default error
        /// </summary>
        private void ShowDefaultError()
        {
            StringBuilder msg = new StringBuilder();
            msg.Append(WebConstant.DropDownDefaultText).Append("<br />");

            dailyMessage.ShowWithText(MessageType.Error, msg.ToString(), MessageSeperationType.Both);
        }

        /// <summary>
        /// Show custom error
        /// </summary>
        /// <param name="info">ProductLicenseMessageInfo object</param>
        /// <param name="isAdmin">is admin or not</param>
        private void ShowCustomError(ProductLicenseMessageInfo info, bool isAdmin)
        {
            string errorTitle = LabelUtil.GetTextByKey("aca_global_js_showerror_title", string.Empty) + "<br>";
            string errorInfo = string.Empty;

            if (isAdmin)
            {
                errorInfo = info.AdminMessage;

                if (errorInfo.StartsWith(errorTitle))
                {
                    errorInfo = errorInfo.Remove(0, errorTitle.Length);
                }

                divAdmin.Visible = true;
                adminMessage.ShowWithText(MessageType.Error, errorInfo, MessageSeperationType.Both);
            }
            else
            {
                errorInfo = info.DailyMessage;

                if (errorInfo.StartsWith(errorTitle))
                {
                    errorInfo = errorInfo.Remove(0, errorTitle.Length);
                }

                dailyMessage.ShowWithText(MessageType.Error, errorInfo, MessageSeperationType.Both);
            }
        }
    }
}
