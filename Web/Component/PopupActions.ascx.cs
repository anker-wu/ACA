#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PopupAction.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Show action list.
 *
 *  Notes:
 *      $Id: PopupActions.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class show inspection action list. 
    /// </summary>
    public partial class PopupActions : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the available actions.
        /// </summary>
        /// <value>The available actions.</value>
        public ActionViewModel[] AvailableActions
        {
            get
            {
                if (ViewState["AvailableActions"] != null)
                {
                    return (ActionViewModel[])ViewState["AvailableActions"];
                }

                return null;
            }

            set
            {
                ViewState["AvailableActions"] = value;
            }
        }

        /// <summary>
        /// Gets the Client ID of "Actions" link.
        /// </summary>
        public string ActionsLinkClientID
        {
            get
            {
                return ClientID + "_lnkActions";
            }
        }
       
        /// <summary>
        /// Gets or sets the action items which used by UI page. 
        /// </summary>
        public string ActionLableKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the action tool tip.
        /// </summary>
        /// <value>
        /// The action tool tip.
        /// </value>
        protected string ActionToolTip
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the is RTL string.
        /// </summary>
        protected string IsRTLstring
        {
            get
            {
                return I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft.ToString();
            }
        }

        #endregion Properties

        /// <summary>
        /// Bind the list control.
        /// </summary>
        public void BindListAction()
        {
            MenuItemContainer.Controls.Clear();
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();

            if (AvailableActions == null)
            {
                return;
            }

            if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
            {
                StringBuilder sbIco = new StringBuilder();
                sbIco.Append("<table role='presentation'><tr>");

                foreach (ActionViewModel actionModel in AvailableActions)
                {
                    if (string.IsNullOrEmpty(actionModel.ClientEvent))
                    {
                        continue;
                    }

                    string actionId = string.IsNullOrEmpty(actionModel.ActionId)
                                          ? string.Empty
                                          : string.Format("id={0}", actionModel.ActionId);

                    sbIco.AppendFormat(
                                        "<td><a {0} href=\"#\" class='NotShowLoading' title='{1}' onclick=\"this.focus();{2};return false;\"><img alt='{1}' class='popaction_ico_img' src='{3}'/></a></td>",
                                        actionId,
                                        actionModel.ActionLabel,
                                        actionModel.ClientEvent,
                                        string.IsNullOrEmpty(actionModel.IcoUrl) ? ImageUtil.GetImageURL("bl_check_24.gif") : actionModel.IcoUrl);
                }

                sbIco.Append("</tr></table>");
                divIco.InnerHtml = sbIco.ToString();
                divMenu.Visible = false;
                divIco.Visible = true;
            }
            else
            {
                foreach (ActionViewModel actionModel in AvailableActions)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    if (actionModel.SeparateLine)
                    {
                        div.Attributes.Add("class", "ActionMenu_Line ACA_TabRow");
                    }
                    else if (actionModel.IsHyperLink)
                    {
                        HyperLink hyperLink = new HyperLink();
                        hyperLink.Text = HttpUtility.HtmlEncode(actionModel.ActionLabel);
                        hyperLink.NavigateUrl = actionModel.ClientEvent;

                        div.Controls.Add(hyperLink);
                    }
                    else
                    {
                        LinkButton link = new LinkButton();
                        link.Text = HttpUtility.HtmlEncode(actionModel.ActionLabel);
                        link.CausesValidation = false;

                        if (!string.IsNullOrWhiteSpace(actionModel.ClientEvent))
                        {
                            //In Apple Safari browser, we need add focus logic into onclick event, to make sure the onblur event can be trigger timely.
                            link.OnClientClick = "this.focus();" + actionModel.ClientEvent + ";return false;";
                        }

                        link.Attributes.Add("onfocus", "SetFocusInForActionMenu()");
                        link.Attributes.Add("onblur", "SetBlurOutForActionMenu();");
                        div.Controls.Add(link);
                    }

                    MenuItemContainer.Controls.Add(div);
                }

                divMenu.Visible = true;
                divIco.Visible = false;
            }
        }

        /// <summary>
        /// Initializes ACAMap
        /// </summary>
        /// <param name="e">EventArgs object</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            upActionMenu.Unload += new EventHandler(UPActionMenu_Unload);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblActions.LabelKey = string.IsNullOrEmpty(ActionLableKey) ? "aca_inspection_actions_link" : ActionLableKey;
            ActionToolTip = GetTitleByKey(lblActions.LabelKey, string.Empty);
            if (IsPostBack)
            {
                BindListAction();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("actionmenujavascript"))
            {
                string scriptPath = FileUtil.AppendApplicationRoot("Scripts/ActionMenu.js");
                ScriptManager.RegisterClientScriptInclude(Page, GetType(), "actionmenujavascript", scriptPath);
            }
        }

        /// <summary>
        /// Handles the Unload event of the UPActionMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void UPActionMenu_Unload(object sender, EventArgs e)
        {
            RegisterUpdatePanel(sender as UpdatePanel);
        }

        /// <summary>
        /// Registers the update panel.
        /// </summary>
        /// <param name="panel">The panel.</param>
        private void RegisterUpdatePanel(UpdatePanel panel)
        {
            foreach (MethodInfo methodInfo in typeof(ScriptManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (methodInfo.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel"))
                {
                    methodInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { upActionMenu });
                }
            }
        }
    }
}