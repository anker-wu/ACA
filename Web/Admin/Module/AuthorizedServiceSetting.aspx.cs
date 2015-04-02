#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AuthorizedServiceSetting.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AuthorizedServiceSetting.aspx.cs 247660 2013-04-15 09:38:44Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Admin.Module
{
    /// <summary>
    /// Authorized service setting
    /// </summary>
    public partial class AuthorizedServiceSetting : AdminBasePage
    {
        /// <summary>
        /// Raise the page load event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                string bizDomainValue = bizBll.GetBizDomainItemDesc(ConfigManager.AgencyCode, BizDomainConstant.STD_AUTHORIZED_SERVICE, BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_FISHING_AND_HUNTING_LICENSE_SALES);

                if (!string.IsNullOrEmpty(bizDomainValue))
                {
                    lblFHHeader.InnerText = bizDomainValue;
                }
                else
                {
                    lblFHHeader.InnerText = LabelUtil.GetAdminUITextByKey("aca_admin_auth_agent_service_label_title"); 
                }
            }
        }
    }
}