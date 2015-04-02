#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactTypeSelect.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ContactTypeSelect.aspx.cs 151830 2013-10-09 13:39:43Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// the class of ContactTypeSelect 
    /// </summary>
    public partial class ContactTypeSelect : PeoplePopupBasePage
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// On Initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(lblSelectTitle.LabelKey);
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("550");

            if (!IsPostBack)
            {
                LoadContactTypeList(ContactSessionParameterModel);
            }

            if (AppSession.IsAdmin)
            {
                lblSelectTitle.Visible = true;
                lblSelectTitle.SectionID = ModuleName + ACAConstant.SPLIT_CHAR + string.Empty + ACAConstant.SPLIT_CHAR + ddlContactType.ClientID + "_";
            }
            else if (!IsPostBack)
            {
                // if there is only one type in the contact type list, immediately go to next page(Contact Add New)
                if ((ddlContactType.Items.Count == 1 && ddlContactType.Items[0].Value != string.Empty)
                    || (ddlContactType.Items.Count == 2 && ddlContactType.Items[0].Value == string.Empty))
                {
                    string contactType = ddlContactType.Items[0].Value != string.Empty
                                             ? ddlContactType.Items[0].Value
                                             : ddlContactType.Items[1].Value;
                    DropDownListBindUtil.SetSelectedValue(ddlContactType, contactType);
                    GetContactTypeAndContinue();
                }
            }

            if (IsPostBack && (ACAConstant.ContactSectionPosition.RegisterAccountConfirm.Equals(ContactSessionParameterModel.ContactSectionPosition) ||
                ACAConstant.ContactSectionPosition.RegisterClerkConfirm.Equals(ContactSessionParameterModel.ContactSectionPosition)))
            {
                //Re-bind contact type drop down to get the latest choices in admin. 
                string contactType = this.Request.Form[ddlContactType.UniqueID];

                DropDownListBindUtil.SetSelectedValue(ddlContactType, contactType);
            }
        }

        /// <summary>
        /// Handles the Click event of the Continue button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            GetContactTypeAndContinue();
        }

        /// <summary>
        /// Gets the contact type and continue.
        /// </summary>
        private void GetContactTypeAndContinue()
        {
            string contactType = ddlContactType.SelectedValue;

            if (string.IsNullOrEmpty(contactType))
            {
                return;
            }

            ContactSessionParameterModel.ContactType = contactType;
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter();

            if (people != null)
            {
                if (!string.Equals(people.contactType, contactType, StringComparison.InvariantCulture))
                {
                    ContactUtil.MergeContactTemplateModel(people, contactType, ModuleName);
                }

                people.contactType = contactType;
            }

            if (ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.EditContactType)
            {
                ContactSessionParameterModel.PageFlowComponent.IsEditable = true;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "editcontacttype", "EditContactType('" + contactType + "');", true);
            }
            else
            {
                Response.Redirect("ContactAddNew.aspx" + Request.Url.Query);
            }
        }

        /// <summary>
        /// Load contact type list.
        /// </summary>
        /// <param name="contactParameters">The contact parameters.</param>
        private void LoadContactTypeList(ContactSessionParameter contactParameters)
        {
            ddlContactType.SourceType = DropDownListDataSourceType.Database;

            switch (contactParameters.ContactSectionPosition)
            {
                case ACAConstant.ContactSectionPosition.RegisterAccount:
                    DropDownListBindUtil.BindContactTypeInRegistration(ddlContactType, XPolicyConstant.CONTACT_TYPE_REGISTERATION);
                    break;
                case ACAConstant.ContactSectionPosition.AddReferenceContact:
                    DropDownListBindUtil.BindContactTypeInRegistration(ddlContactType, XPolicyConstant.CONTACT_TYPE_ASSOCICATION);
                    break;
                case ACAConstant.ContactSectionPosition.RegisterClerk:
                    DropDownListBindUtil.BindContactTypeInRegistration(ddlContactType, XPolicyConstant.CONTACT_TYPE_REGISTERATION_CLERK);
                    break;
                case ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail:
                    DropDownListBindUtil.BindContactType(ddlContactType, ContactTypeSource.Reference);
                    break;
                default:
                    if (AppSession.IsAdmin)
                    {
                        ddlContactType.SourceType = DropDownListDataSourceType.STDandXPolicy;
                    }

                    if (contactParameters.ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm)
                    {
                        DropDownListBindUtil.BindContactTypeWithPageFlow(ddlContactType, ModuleName, IsMultipleContact, contactParameters.PageFlowComponent.ComponentName);
                    }
                    else
                    {
                        DropDownListBindUtil.BindContactType(ddlContactType, ContactTypeSource.Reference);
                    }

                    break;
            }
        }

        #endregion Methods
    }
}