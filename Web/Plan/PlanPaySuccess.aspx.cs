#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Controls_Plan_PlanPaySuccess.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PlanPaySuccess.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Plan
{
    /// <summary>
    /// the class for PlanPaySuccess.
    /// </summary>
    public partial class PlanPaySuccess : BasePage
    {
        #region Methods

        /// <summary>
        /// page load method.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Session["FirstTime"] = "Success";

            if (!IsPostBack)
            {
                try
                {
                    PaymentModel paymentModel = Session["PLAN_PAYMENTMODEL"] as PaymentModel;

                    if (paymentModel == null)
                    {
                        //MessageUtil.ShowError(Page, "paymentModel is empty.");
                        return;
                    }

                    string id1 = paymentModel.capID.ID1;
                    string id2 = paymentModel.capID.ID2;
                    string id3 = paymentModel.capID.ID3;
                    string servCode = ConfigManager.AgencyCode;

                    try
                    {
                        IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                        string callerID = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_V360_WEB_ACTION_USERNAME);

                        string sessionID = Session["ADMIN_SESSIONID"] as string;

                        if (string.IsNullOrEmpty(sessionID))
                        {
                            string passWord = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_V360_WEB_ACTION_PASSWORD);

                            ISSOBll ssoBll = ObjectFactory.GetObject<ISSOBll>();
                            sessionID = ssoBll.Signon(servCode, callerID, passWord);

                            Session["ADMIN_SESSIONID"] = sessionID;
                        }

                        //set print permit url link
                        string module = ModuleName;
                    }
                    catch (ACAException ex)
                    {
                        MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                    }
                }
                catch (ACAException ex)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                    return;
                }
            }
        }

        #endregion Methods
    }
}
