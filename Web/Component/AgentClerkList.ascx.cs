#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AgentClerkList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AgentClerkList.ascx.cs 237828 2013-5-1 01:29:52Z ACHIEVO\foxus.lin $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// List view of the agent clerk.
    /// </summary>
    public partial class AgentClerkList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Change clerk status command.
        /// </summary>
        private const string COMMAND_CHANGE_CLERK_STATUS = "ChangeStatus";

        /// <summary>
        /// The Locked status.
        /// </summary>
        private const string LOCKED = "LOCKED";

        /// <summary>
        /// The Active status.
        /// </summary>
        private const string ACTIVE = "ACTIVE";

        /// <summary>
        /// The edit clerk command.
        /// </summary>
        private const string COMMAND_EDIT_CLERK = "edit";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public IList<PublicUserModel4WS> GridViewDataSource 
        {
            get 
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = new List<PublicUserModel4WS>();
                }

                return (IList<PublicUserModel4WS>)ViewState["DataSource"];
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Bind authorized agent clerk list
        /// </summary>
        public void BindClerkList()
        {
            gdvClerkList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_authagent_clerklist_msg_norecordfound");

            if (!AppSession.IsAdmin && AppSession.User.IsAuthorizedAgent)
            {
                IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                PublicUserModel4WS agentUser = AppSession.User.UserModel4WS;
                agentUser.servProvCode = ConfigManager.SuperAgencyCode;
                PublicUserModel4WS[] clerkModel = accountBll.GetClerkList(agentUser);

                if (clerkModel != null)
                {
                    GridViewDataSource = ObjectConvertUtil.ConvertArrayToList<PublicUserModel4WS>(clerkModel);

                    if (!string.IsNullOrEmpty(gdvClerkList.GridViewSortExpression))
                    {
                        IList<PublicUserModel4WS> tmpClerks = new List<PublicUserModel4WS>();
                        IList<object> objs = gdvClerkList.SortList(GridViewDataSource, false);

                        foreach (object obj in objs)
                        {
                            tmpClerks.Add(obj as PublicUserModel4WS);
                        }

                        GridViewDataSource = tmpClerks;
                    }
                }
            }

            gdvClerkList.DataSource = GridViewDataSource;
            gdvClerkList.DataBind();
        }

        /// <summary>
        /// Initializes event handler.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (AppSession.User.IsAuthorizedAgent && !IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
                {
                    gdvClerkList.ShowExportLink = true;
                    gdvClerkList.ExportFileName = "AgentClerkList";
                }
                else
                {
                    gdvClerkList.ShowExportLink = false;
                }
            }

            if (AppSession.User.IsAuthorizedAgent || AppSession.IsAdmin)
            {
                GridViewBuildHelper.SetSimpleViewElements(gdvClerkList, ModuleName, AppSession.IsAdmin);
            }

            base.OnInit(e);
        }

        /// <summary>
        /// GridView clerkList row data bound event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void ClerkList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnAction = e.Row.FindControl("btnAction") as LinkButton;
                AccelaLabel lblStatus = e.Row.FindControl("lblStatus") as AccelaLabel;
                PublicUserModel4WS clerkModel = (PublicUserModel4WS)e.Row.DataItem;
                string status = clerkModel.status;

                if (string.IsNullOrEmpty(status))
                {
                    lblStatus.Text = GetTextByKey("aca_common_inactive");
                    btnAction.Text = GetTextByKey("aca_authagent_clerklist_label_activate");
                }
                else
                {
                    if (LOCKED.Equals(status, StringComparison.InvariantCulture))
                    {
                        lblStatus.Text = GetTextByKey("aca_common_locked");
                        btnAction.Text = GetTextByKey("aca_authagent_clerklist_label_activate");
                    }
                    else if (ACTIVE.Equals(status, StringComparison.InvariantCulture))
                    {
                        lblStatus.Text = GetTextByKey("aca_common_active");
                        btnAction.Text = GetTextByKey("aca_authagent_clerklist_label_deactivate");
                    }
                }
            }
        }

        /// <summary>
        /// GridView ClerkList row command event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridView Command Event argument</param>
        protected void ClerkList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string arg = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case COMMAND_CHANGE_CLERK_STATUS:
                    IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                    PublicUserModel4WS clerkModel = GridViewDataSource.Where(f => f.userSeqNum == arg).SingleOrDefault();

                    try
                    {
                        if (string.IsNullOrEmpty(clerkModel.status) || LOCKED.Equals(clerkModel.status, StringComparison.OrdinalIgnoreCase))
                        {
                            accountBll.ActivateUser(ConfigManager.SuperAgencyCode, clerkModel.UUID);
                        }
                        else if (ACTIVE.Equals(clerkModel.status, StringComparison.OrdinalIgnoreCase))
                        {
                            accountBll.DeactivateUserForClerk(ConfigManager.SuperAgencyCode, clerkModel.UUID);
                        }

                        BindClerkList();
                    }
                    catch (Exception ex)
                    {
                        MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
                    }

                    break;

                case COMMAND_EDIT_CLERK:
                    string url = string.Format(
                        "~/Account/RegisterEdit.aspx?{0}={1}&{2}={3}",
                        UrlConstant.IS_FOR_CLERK,
                        ACAConstant.COMMON_Y,
                        UrlConstant.CLERK_SEQ_NBR,
                        arg);

                    AppSession.IsEditFromClerkFlag = true;
                    AppSession.SetContactSessionParameter(null);
                    AppSession.SetRegisterContactSessionParameter(null);
                    Response.Redirect(url);

                    break;
            }
        }
    }
}