#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactPermissionControl.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *  Contact Relation Ships cotrol
 *
 *  Notes:
 *      $Id: ContactPermissionControl.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Display Grade Style Information in Education,Examination Form
    /// </summary>
    public partial class ContactPermissionControl : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Set Contact relation ships Permission value.
        /// </summary>
        /// <param name="permission">the permission value</param>
        public void SetValue(string permission)
        {
            SetSelectedItems(radioListContactPermission.Items, permission);
            SetSelectedItems(ckbCustomPermissionList.Items, permission);
        }

        /// <summary>
        /// Add Required Validator.
        /// </summary>
        /// <param name="contactOrder">the contact order</param>
        public void AddRequiredValidator(string contactOrder)
        {
            radioListContactPermission.AddRequiredValidator(contactOrder);
        }

        /// <summary>
        /// Get contact relation ships control value.
        /// </summary>
        /// <returns>Return control value.</returns>
        public string GetValue()
        {
            string contactRelationShipsValue = string.Empty;
            string radioButtonListValue = radioListContactPermission.SelectedValue;

            if (!ContactPermission.Custom.Equals(radioButtonListValue))
            {
                contactRelationShipsValue = radioButtonListValue;
            }
            else
            {
                string checkBoxListValue = string.Empty;

                if (ckbCustomPermissionList.Visible && ckbCustomPermissionList.Items.Count > 0)
                {
                    foreach (ListItem li in ckbCustomPermissionList.Items)
                    {
                        if (li.Selected)
                        {
                            checkBoxListValue += ACAConstant.COMMA + li.Value;
                        }
                    }
                }

                contactRelationShipsValue = string.IsNullOrEmpty(checkBoxListValue) ? ContactPermission.NoAccess : radioButtonListValue + checkBoxListValue;
            }

            return contactRelationShipsValue;
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BindContactPermission(radioListContactPermission, ckbCustomPermissionList);
        }

        /// <summary>
        /// override PreRender event.
        /// </summary>
        /// <param name="e">the event handle</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string script = "ContactRelationShipsRadioListChange('" + radioListContactPermission.ClientID + "','" + ckbCustomPermissionList.ClientID + "');";
            string sScriptKey = radioListContactPermission.ClientID;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), sScriptKey, script, true);
            
            //Add a attribute to indicates the checkbox control id. It's used in Expression.
            radioListContactPermission.Attributes.Add("CustomListControlID", ckbCustomPermissionList.ClientID);

            //Render real value to checkbox item.
            ckbCustomPermissionList.RenderingCompatibility = new Version(4, 0);
        }

        /// <summary>
        /// Set selected item value.
        /// </summary>
        /// <param name="items">the items.</param>
        /// <param name="permission">the permission value.</param>
        private void SetSelectedItems(ListItemCollection items, string permission)
        {
            if (radioListContactPermission.Items.Count > 0 && !string.IsNullOrEmpty(permission))
            {
                foreach (ListItem item in items)
                {
                    item.Selected = permission.IndexOf(item.Value) > -1;
                }
            }
        }

        /// <summary>
        /// Bind the contact permission item to AccelaRadioButtonList control.
        /// </summary>
        /// <param name="rdlContactPermission">AccelaDropDownList control to be bind.</param>
        /// <param name="chkContactPermission">check box list control to be bind.</param>
        private void BindContactPermission(AccelaRadioButtonList rdlContactPermission, AccelaCheckBoxList chkContactPermission)
        {
            List<ListItem> radioItems = new List<ListItem>();

            radioItems.Add(new ListItem(BasePage.GetStaticTextByKey("contact_permission_noaccess"), ContactPermission.NoAccess));
            radioItems.Add(new ListItem(BasePage.GetStaticTextByKey("contact_permission_readonly"), ContactPermission.ReadOnly));
            radioItems.Add(new ListItem(BasePage.GetStaticTextByKey("contact_permission_fullaccess"), ContactPermission.FullAccess));
            radioItems.Add(new ListItem(BasePage.GetStaticTextByKey("aca_contactpermission_label_custom"), ContactPermission.Custom));

            foreach (ListItem item in radioItems)
            {
                item.Attributes.Add("onclick", "ContactRelationShipsRadioListChange('" + GetRealID(rdlContactPermission.ClientID) + "','" + GetRealID(chkContactPermission.ClientID) + "');");
                rdlContactPermission.Items.Add(item);
            }

            List<ListItem> checkboxItems = new List<ListItem>();
            checkboxItems.Add(new ListItem(BasePage.GetStaticTextByKey("aca_contactpermission_label_renewandamend"), ContactPermission.RenewAndAmend));
            checkboxItems.Add(new ListItem(BasePage.GetStaticTextByKey("aca_contactpermission_label_manageinspections"), ContactPermission.ScheduleInspectionOnly));
            checkboxItems.Add(new ListItem(BasePage.GetStaticTextByKey("aca_contactpermission_label_makepayments"), ContactPermission.MakePayments));
            checkboxItems.Add(new ListItem(BasePage.GetStaticTextByKey("aca_contactpermission_label_managedocuments"), ContactPermission.ManageDocuments));

            foreach (ListItem item in checkboxItems)
            {
                item.Attributes.Add("onclick", "ContactRelationShipsCheckListChange('" + GetRealID(rdlContactPermission.ClientID) + "');");
                chkContactPermission.Items.Add(item);
            }
        }

        /// <summary>
        /// Get control real id.
        /// </summary>
        /// <param name="id">the id before convert.</param>
        /// <returns>the real control id.</returns>
        private string GetRealID(string id)
        {
            return id.Replace("Contact1Edit", "Contact1").Replace("Contact2Edit", "Contact2").Replace("Contact3Edit", "Contact3").Replace("ApplicantEdit", "Applicant");
        }

        #endregion
    }
}
