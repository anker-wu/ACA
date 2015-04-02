#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressSearchResult.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressSearchResult.aspx.cs 256970 2014-06-19 06:10:05Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.APO
{
    /// <summary>
    /// Page to display address associate detail
    /// </summary>
    public partial class AddressSearchResult : APOPopupBasePage
    {
        #region Methods

        /// <summary>
        /// Load Address detail and associated Parcel/Owner List when the page is loaded.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("550");
            SetDialogFixedWidth("800");

            // Initialize the page's title
            InitTitle();

            if (!IsPostBack)
            {
                ucAddressSearchResult.LoadAPOSearchResult();

                if (!string.IsNullOrEmpty(APOSessionParameter.ErrorMessage))
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "UpdateAddressAndAssociates", "UpdateAddressAndAssociates();", true);
                }
                else if (ucAddressSearchResult.OnlyOneAtMost)
                {
                    SelectAddressAndAssociates();
                }
            }

            if (AppSession.IsAdmin
                || ucAddressSearchResult.SelectedAddress != null
                || ucAddressSearchResult.SelectedParcel != null
                || ucAddressSearchResult.SelectedOwner != null)
            {
                btnSelect.Enabled = true;
            }
        }

        /// <summary>
        /// Click Select button event handler. 
        /// Select an associated parcel and owner information for address and close this page.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void SelectButton_Click(object sender, EventArgs e)
        {
            SelectAddressAndAssociates();
        }

        /// <summary>
        /// Initialize the page's title.
        /// </summary>
        private void InitTitle()
        {
            if (AppSession.IsAdmin)
            {
                lblAddressSearchTitle.Visible = true;
            }
            else
            {
                SetPageTitleKey(lblAddressSearchTitle.LabelKey);
            }
        }

        /// <summary>
        /// Select address and associated parcel/owner. If select successfully, close this page and update parent page.
        /// </summary>
        private void SelectAddressAndAssociates()
        {
            AddressModel address = ucAddressSearchResult.SelectedAddress;
            ParcelModel parcel = ucAddressSearchResult.SelectedParcel;
            OwnerModel owner = ucAddressSearchResult.SelectedOwner;

            // Check condition.
            if (address == null)
            {
                // No record when GIS Send Address
                RefAddressModel refAddress = ucAddressSearchResult.GetGISRefAddress();

                if (refAddress != null)
                {
                    address = CapUtil.ConvertRefAddressModel2AddressModel(refAddress);
                    ucAddressSearchResult.ShowCondition(address);
                }
            }
            else
            {
                ucAddressSearchResult.ShowCondition(address, parcel, owner);   
            }

            if (ucAddressSearchResult.ConditionResult != ConditionResult.Lock)
            {
                APOSessionParameter.SelectedAddress = address;
                APOSessionParameter.SelectedParcel = parcel;
                APOSessionParameter.SelectedOwner = owner;
                APOSessionParameter.ConditionInfo = null;

                if (ucAddressSearchResult.ConditionResult != ConditionResult.None)
                {
                    APOSessionParameter.ConditionInfo = ucAddressSearchResult.RefAddressModelWithCondition;
                }

                AppSession.SetAPOSessionParameter(APOSessionParameter);

                ScriptManager.RegisterStartupScript(Page, GetType(), "UpdateAddressAndAssociates", "UpdateAddressAndAssociates();", true);
            }
        }

        #endregion Methods
    }
}
