#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MessageBar.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: MessageBar.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// MessageBar Control.
    /// </summary>
    public partial class MessageBar : BaseUserControl
    {
        /// <summary>
        /// notice message style
        /// </summary>
        private const string ACA_MESSAGE_NOTICE_CLASS = "ACA_Message_Notice ACA_Message_Notice_FontSize";

        /// <summary>
        /// error message style
        /// </summary>
        private const string ACA_MESSAGE_ERROR_CLASS = "ACA_Message_Error ACA_Message_Error_FontSize";

        /// <summary>
        /// success message style
        /// </summary>
        private const string ACA_MESSAGE_SUCCESS_CLASS = "ACA_Message_Success ACA_Message_Success_FontSize";

        /// <summary>
        /// notice title label key.
        /// </summary>
        private const string ACA_NOTICE_MESSAGE_TITILE_KEY = "aca_global_js_shownotice_title";

        /// <summary>
        /// error title label key.
        /// </summary>
        private const string ACA_ERROR_MESSAGE_TITILE_KEY = "aca_global_js_showerror_title";

        /// <summary>
        /// success title label key.
        /// </summary>
        private const string ACA_SUCCESS_MESSAGE_TITILE_KEY = "aca_global_js_showconfirm_title";

        /// <summary>
        /// Occurs when [Link button event].
        /// </summary>
        public event CommonEventHandler LnkButtonEvent;

        /// <summary>
        /// Gets or sets a value indicating whether [is show link button].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is show link button]; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowLinkButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the on client click.
        /// </summary>
        /// <value>
        /// The on client click.
        /// </value>
        public string OnClientClick
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets message label key.
        /// </summary>
        private string MessageLabelKey
        {
            get
            {
                if (ViewState["MessageLabelKey"] == null)
                {
                    ViewState["MessageLabelKey"] = string.Empty;
                }

                return (string)ViewState["MessageLabelKey"];
            }

            set
            {
                ViewState["MessageLabelKey"] = value;
            }
        }

        /// <summary>
        /// Show a message 
        /// </summary>
        /// <param name="msgType">Message type.</param>
        /// <param name="labelKey">Message label key.</param>
        /// <param name="separationType">Message separate type.</param>
        public void Show(MessageType msgType, string labelKey, MessageSeperationType separationType)
        {
            SetPublicProperty(msgType, separationType);

            MessageLabelKey = labelKey;
            lblMessage.LabelKey = labelKey;
            
            messageBar.Visible = true;

            if (!AppSession.IsAdmin && AccessibilityUtil.AccessibilityEnabled && !string.IsNullOrEmpty(labelKey))
            {
                MessageUtil.ShowAlertMessage(this, LabelUtil.GetTextByKey(labelKey, this.ModuleName));
            }
        }

        /// <summary>
        /// Show a message.
        /// </summary>
        /// <param name="msgType">Message type.</param>
        /// <param name="messageText">Message text.</param>
        /// <param name="separationType">Message separate type.</param>
        public void ShowWithText(MessageType msgType, string messageText, MessageSeperationType separationType)
        {
            SetPublicProperty(msgType, separationType);

            lblMessage.Text = messageText;

            messageBar.Visible = true;

            if (AccessibilityUtil.AccessibilityEnabled && !string.IsNullOrEmpty(messageText))
            {
                MessageUtil.ShowAlertMessage(this, System.Web.HttpUtility.HtmlDecode(messageText));
            }
        }

        /// <summary>
        /// hide a message.
        /// </summary>
        public void Hide()
        {
            MessageLabelKey = string.Empty;
            messageBar.Visible = false;
            sepForMessageTop.Visible = false;
            sepForMessageBottom.Visible = false;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.LabelKey = MessageLabelKey;
            lnkEditContact.Visible = Visible && IsShowLinkButton;
            lnkEditContact.OnClientClick = OnClientClick;
            lnkEditContact.Click += delegate(object o, EventArgs args)
            {
                if (LnkButtonEvent != null)
                {
                    LnkButtonEvent(o, new CommonEventArgs(args));
                }
            };
        }

        /// <summary>
        /// Set message property.
        /// </summary>
        /// <param name="msgType">Message type.</param>
        /// <param name="separationType">Message separate type.</param>
        private void SetPublicProperty(MessageType msgType, MessageSeperationType separationType)
        {
            string title = string.Empty;

            switch (msgType)
            {
                case MessageType.Notice:
                    title = GetTextByKey(ACA_NOTICE_MESSAGE_TITILE_KEY);
                    messageBar.Attributes.Add("class", ACA_MESSAGE_NOTICE_CLASS);
                    imgMsgIcon.Attributes.Add("src", ImageUtil.GetImageURL("notice_24.gif"));
                    lblMessageTitle.Text = title + ACAConstant.COLON_CHAR;
                    imgMsgIcon.Attributes.Add("title", title);
                    imgMsgIcon.Attributes.Add("alt", title);
                    break;
                case MessageType.Success:
                    title = GetTextByKey(ACA_SUCCESS_MESSAGE_TITILE_KEY);
                    messageBar.Attributes.Add("class", ACA_MESSAGE_SUCCESS_CLASS);
                    imgMsgIcon.Attributes.Add("src", ImageUtil.GetImageURL("confirmation_24.gif"));
                    lblMessageTitle.Visible = false;
                    imgMsgIcon.Attributes.Add("title", title);
                    imgMsgIcon.Attributes.Add("alt", title);
                    break;
                case MessageType.Error:
                    title = GetTextByKey(ACA_ERROR_MESSAGE_TITILE_KEY);
                    messageBar.Attributes.Add("class", ACA_MESSAGE_ERROR_CLASS);
                    imgMsgIcon.Attributes.Add("src", ImageUtil.GetImageURL("error_24.gif"));
                    lblMessageTitle.Text = GetTextByKey(ACA_ERROR_MESSAGE_TITILE_KEY);
                    imgMsgIcon.Attributes.Add("title", title);
                    imgMsgIcon.Attributes.Add("alt", title);
                    break;
            }

            switch (separationType)
            {
                case MessageSeperationType.Both:
                    sepForMessageTop.Visible = true;
                    sepForMessageBottom.Visible = true;
                    break;
                case MessageSeperationType.Bottom:
                    sepForMessageBottom.Visible = true;
                    break;
                case MessageSeperationType.Top:
                    sepForMessageTop.Visible = true;
                    break;
            }
        }
    }
}
