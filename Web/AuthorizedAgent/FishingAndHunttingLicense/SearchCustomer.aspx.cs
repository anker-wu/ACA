#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SearchCustomer.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SearchCustomer.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.AuthorizedAgent.FishingAndHunttingLicense
{
    /// <summary>
    /// This class provides the search customer entrance.
    /// </summary>
    public partial class SearchCustomer : BasePage
    {
        #region Event

        /// <summary>
        /// Page load event method
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                // the section id need split into 3 or 4 parts which support form design.
                lblSectionCustomerHeader.SectionID = string.Format("{0}{1}{0}{2}", ACAConstant.SPLIT_CHAR, GviewID.AuthAgentCustomerSearchForm, customerSearchForm.ClientID + "_");
            }

            if (!IsPostBack)
            {
                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                string bizDomainValue = bizBll.GetBizDomainItemDesc(ConfigManager.AgencyCode, BizDomainConstant.STD_AUTHORIZED_SERVICE, BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_FISHING_AND_HUNTING_LICENSE_SALES);

                if (!string.IsNullOrEmpty(bizDomainValue))
                {
                    lblPageHeader.InnerText = bizDomainValue; 
                }
                else
                {
                    lblPageHeader.InnerText = LabelUtil.GetTextByKey("aca_authagent_customer_label_pageheader", ModuleName);
                }
            }
        }

        #endregion Event
    }
}