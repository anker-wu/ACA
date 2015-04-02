#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: RegisterAccountConfirm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: RegisterAccountConfirm.aspx.cs 151830 2010-04-26 13:39:43Z ACHIEVO\grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.Services;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.People;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// the class of AccountContact 
    /// </summary>
    public partial class RegisterAccountConfirm : PopupDialogBasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether is Clerk register or edit
        /// </summary>
        protected bool IsForClerk { get; set; }

        /// <summary>
        /// Gets a value indicating whether to need identify check.
        /// </summary>
        protected bool NeedIdentifyCheck
        {
            get
            {
                return string.IsNullOrEmpty(Request.QueryString[UrlConstant.NEED_IDENTIFY_CHECK])
                        || ValidationUtil.IsYes(Request.QueryString[UrlConstant.NEED_IDENTIFY_CHECK]);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactAddressIndex">Index of the contact address.</param>
        /// <param name="processType">Type of the process.</param>
        /// <param name="callbackName">Name of the callback.</param>
        /// <param name="parameterString">The parameter string.</param>
        [WebMethod(Description = "Operation Contact Address Session", EnableSession = true)]
        public static void OperationContactAddressSession(string moduleName, string contactAddressIndex, string processType, string callbackName, string parameterString)
        {
            ContactSessionParameter sessionParameter = AppSession.GetContactSessionParameter();

            if (!string.IsNullOrEmpty(processType))
            {
                sessionParameter.Process.ContactAddressProcessType = EnumUtil<ContactAddressProcessType>.Parse(processType);
            }

            if (!string.IsNullOrEmpty(contactAddressIndex))
            {
                sessionParameter.Data.ContactAddressRowIndex = int.Parse(contactAddressIndex);
            }
            else
            {
                sessionParameter.Data.ContactAddressRowIndex = null;
            }

            sessionParameter.ContactSectionPosition = ACAConstant.ContactSectionPosition.RegisterAccountConfirm;

            sessionParameter.Process.CACallbackFunctionName = callbackName;
            AppSession.SetContactSessionParameter(sessionParameter);
        }

        /// <summary>
        /// overwrite load method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">the event handle</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("550");

            if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]))
            {
                if (!AppSession.IsAdmin && AppSession.User.IsAnonymous)
                {
                    GotoLoginPage();
                }

                IsForClerk = true;
                contactEdit.ContactSectionPosition = ACAConstant.ContactSectionPosition.RegisterClerkConfirm;
                lblAcceptAsOwnContact.LabelKey = "aca_authagent_closematch_label_as_clerk_contact";
                chkTermAccept.Attributes["title"] =
                    HttpUtility.HtmlAttributeEncode(LabelUtil.RemoveHtmlFormat(GetTextByKey("aca_authagent_closematch_label_as_clerk_contact")));
            }
            else
            {
                contactEdit.ContactSectionPosition = ACAConstant.ContactSectionPosition.RegisterAccountConfirm;
                lblAcceptAsOwnContact.LabelKey = "aca_label_accept_as_own_contact";
                chkTermAccept.Attributes["title"] =
                    HttpUtility.HtmlAttributeEncode(LabelUtil.RemoveHtmlFormat(GetTextByKey("aca_label_accept_as_own_contact")));
            }

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(lblContactTitle.LabelKey);
                PublicUserModel4WS model = PeopleUtil.GetPublicUserFromSession();

                if (model.peopleModel != null && model.peopleModel.Length > 0)
                {
                    PeopleModel4WS people = model.peopleModel[0];
                    contactEdit.IsEditable = false;
                    contactEdit.Display(people);
                }
            }
            else
            {
                lblContactTitle.Visible = true;
            }
        }

        /// <summary>
        /// Click on button "Continue Registration"
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void ContinueButton_OnClick(object sender, EventArgs e)
        {
            if (NeedIdentifyCheck)
            {
                PublicUserModel4WS publicUser = PeopleUtil.GetPublicUserFromSession();

                // Replace the input contact with the close match contact in the session.
                if (publicUser == null || publicUser.peopleModel == null || publicUser.peopleModel.Length == 0)
                {
                    return;
                }

                ContactSessionParameter parametersModel = AppSession.GetContactSessionParameter();
                parametersModel.Data.DataObject = publicUser.peopleModel[0];
                AppSession.SetContactSessionParameter(parametersModel);
                ContactSessionParameter parametersModelRegister = ObjectCloneUtil.DeepCopy(parametersModel);
                AppSession.SetRegisterContactSessionParameter(parametersModelRegister);
            }

            ClientScript.RegisterStartupScript(GetType(), "ConfirmContact", "PopupClose();", true);
        }

        #endregion Methods
    }
}