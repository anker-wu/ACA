#region Header

/**
 *  Accela Citizen Access
 *  File: AuthorizedDevice.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *   validate agent or clerk 's pc connected printer.
 *
 *  Notes:
 * $Id: AuthorizedDevice.cs 245662 2013-03-08 08:36:51Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Web.UI;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.AuthorizedAgent.FishingAndHunttingLicense
{
    /// <summary>
    /// validate agent or clerk 's pc connected printer.
    /// </summary>
    public partial class AuthorizedDevice : BasePageWithoutMaster
    {
        #region Fields

        /// <summary>
        /// The printer connected status
        /// </summary>
        protected const string PRINT_STATUS_CHANGE = "PrinterStatusChange";

        /// <summary>
        /// The Client program installed status
        /// </summary>
        protected const string CLIENT_STATUS_CHANGE = "ClientStatusChange";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the last post back source.
        /// </summary>
        /// <value>
        /// The last post back source.
        /// </value>
        protected string LastPostbackSource
        {
            get
            {
                return Request.Form[Page.postEventSourceID];
            }
        }

        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string postSource = Request.Form[Page.postEventSourceID];
            User user = AppSession.User;
            bool authServiceUnDefined = !AuthorizedAgentServiceUtil.HasAuthorizedServiceConfig();

            //when AuthAgent needn't client-side printer, pass the printer validation.
            if (!user.IsAuthAgentNeedPrinter)
            {
                user.AgentClientInstalled = true;
                user.PrinterConnected = true;
            }
            else if (!string.IsNullOrEmpty(postSource))
            {
                string postArg = Request.Form[Page.postEventArgumentID];

                if (postSource.EndsWith(PRINT_STATUS_CHANGE))
                {
                    user.PrinterConnected = string.IsNullOrEmpty(postArg) ? false : bool.Parse(postArg);

                    if (!user.AgentClientInstalled)
                    {
                        user.AgentClientInstalled = true;
                    }
                }
                else if (postSource.EndsWith(CLIENT_STATUS_CHANGE))
                {
                    user.AgentClientInstalled = false;
                    user.PrinterConnected = false;
                }
            }

            if (!authServiceUnDefined && user.AgentClientInstalled && !user.PrinterConnected && (user.IsAgentClerk || user.IsAuthorizedAgent))
            {
                divButtonList.Visible = true;
                liRefresh.Visible = true;
                liOK.Visible = false;
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_auth_agent_message_access_denied"));
            }
            else if (!authServiceUnDefined && !user.AgentClientInstalled && (user.IsAgentClerk || user.IsAuthorizedAgent))
            {
                divButtonList.Visible = true;
                liRefresh.Visible = false;
                liOK.Visible = true;
                MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("aca_auth_agent_message_client_not_installed"));
            }

            if (authServiceUnDefined)
            {
                divButtonList.Visible = true;
                liRefresh.Visible = false;
                liOK.Visible = false;
                MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("aca_authagent_msg_nostddefined"));
            }
            else if ((!user.AgentClientInstalled || (user.AgentClientInstalled && !user.PrinterConnected)) && IsPostBack)
            {
                Response.Redirect("AuthorizedDevice.aspx");
            }
            else if (user.AgentClientInstalled && user.PrinterConnected)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "closeThis", "parent.CloseAuthorizedDevice();", true);
            }
        }
    }
}