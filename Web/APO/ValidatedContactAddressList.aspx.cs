#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ValidatedContactAddressList.aspx.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: A web form to show vlidated contat address list.
*
*  Notes:
* $Id: ValidatedContactAddressList.aspx.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
*  Revision History
*  Date,            Who,        What
*  Oct 15, 2012      wallance     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.APO
{
    /// <summary>
    /// Validated contact address from external source display in a popup.
    /// </summary>
    public partial class ValidatedContactAddressList : PopupDialogBasePage
    {
        #region Properties

        /// <summary>
        /// Gets the parent client ID used to indicates the correct javascript function.
        /// </summary>
        protected string ParentPageClientID
        {
            get
            {
                return Request.QueryString[UrlConstant.PARENT_INSTANCE_ID];
            }
        }

        #endregion Properties

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_validatedcontactaddresslist_label_matchingresult");

            if (!IsPostBack)
            {
                ValidateContactAddress();
            }
        }

        /// <summary>
        /// Validate contact address by XAPO or USPS.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Select_Click(object sender, EventArgs e)
        {
            int selectedIndex = Convert.ToInt32(hfSelectedAddress.Value);
            ContactAddressModel contactAddress = validatedContactAddressList.DataSource.Where(p => p.RowIndex == selectedIndex).SingleOrDefault();
            Session[SessionConstant.SESSION_VALIDATE_CONTACT_ADDRESS] = contactAddress;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SelectValidatedAddress", "SelectValidatedAddress()", true);
        }

        /// <summary>
        /// Validate contact address by third data source.USPS, XAPO
        /// </summary>
        private void ValidateContactAddress()
        {
            if (!AppSession.IsAdmin)
            {
                ContactAddressModel contactAddress = (ContactAddressModel)Session[SessionConstant.SESSION_VALIDATE_CONTACT_ADDRESS];

                //TODO call third web service to validate original contact address
                List<ContactAddressModel> validatedList = new List<ContactAddressModel>();
                IRefAddressBll refAddressBll = (IRefAddressBll)ObjectFactory.GetObject(typeof(IRefAddressBll));
                validatedList.AddRange(refAddressBll.GetContactAddressListFromExternal(CapUtil.GetAgencyCode(ModuleName), contactAddress));

                //Bind contact address list
                validatedContactAddressList.Display(validatedList);

                if (validatedList == null || validatedList.Count < 1)
                {
                    lblMsgBar.Visible = true;
                    string errorMsg = GetTextByKey("aca_validatedcontactaddresslist_msg_nomatching");
                    MessageUtil.ShowMessageInPopup(lblMsgBar, MessageType.Error, errorMsg);
                    return;
                }
            }
            else
            {
                validatedContactAddressList.Display(null);
            }
        }
    }
}