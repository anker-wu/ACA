#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactCloseMatchConfirm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ContactCloseMatchConfirm.aspx.cs 151830 2013-10-09 13:39:43Z ACHIEVO\jone.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.Services;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// The contact close match confirm page.
    /// </summary>
    public partial class ContactCloseMatchConfirm : PeoplePopupBasePage
    {
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

            sessionParameter.ContactSectionPosition = ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm;

            sessionParameter.Process.CACallbackFunctionName = callbackName;
            AppSession.SetContactSessionParameter(sessionParameter);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("550");

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey("aca_contactclosematchconfirm_label_confirmtitle");
            }

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                ucContactEdit.SetSpearFormCloseMatch();
                ContactSessionParameter parametersModel = AppSession.GetContactSessionParameter();
                PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);
                ucContactEdit.IsEditable = false;
                ucContactEdit.Display(people);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "disablebutton" + Guid.NewGuid(), "<script>SetWizardButtonDisable('" + btnContinue.ClientID + "', true);</script>", false);
            }
        }

        /// <summary>
        /// Handles the Click event of the Continue button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            ContactSessionParameter parametersModel = ucContactEdit.SyncContactPeopleToConfirmCloseMatch();

            if (parametersModel == null)
            {
                return;
            }

            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);
            parametersModel.ContactSectionPosition = ACAConstant.ContactSectionPosition.SpearForm;
            AppSession.SetContactSessionParameter(parametersModel);

            if (ContactUtil.IsPassContactValidateInSpearForm(parametersModel, ModuleName))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "SaveContact", "PopupClose(" + people.contactSeqNumber + ");", true);
            }
            else
            {
                string url = Page.ResolveUrl("~/People/ContactAddNew.aspx") + Request.Url.Query;
                Response.Redirect(url);
            }
        }

        #endregion Methods
    }
}