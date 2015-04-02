#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ParcelSearchResult.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ParcelSearchResult.aspx.cs 256970 2014-06-19 06:10:05Z ACHIEVO\blues.gao $.
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
    /// Page to display parcel associate detail
    /// </summary>
    public partial class ParcelSearchResult : APOPopupBasePage
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
                ucParcelSearchResult.LoadAPOSearchResult();

                if (!string.IsNullOrEmpty(APOSessionParameter.ErrorMessage))
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "UpdateParcelAndAssociates", "UpdateParcelAndAssociates();", true);
                }
                else if (ucParcelSearchResult.OnlyOneAtMost)
                {
                    SelectParcelAndAssociates();
                }
            }

            if (AppSession.IsAdmin
                || ucParcelSearchResult.SelectedAddress != null
                || ucParcelSearchResult.SelectedParcel != null
                || ucParcelSearchResult.SelectedOwner != null)
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
            SelectParcelAndAssociates();
        }

        /// <summary>
        /// Initialize the page's title.
        /// </summary>
        private void InitTitle()
        {
            if (AppSession.IsAdmin)
            {
                lblParcelSearchTitle.Visible = true;
            }
            else
            {
                SetPageTitleKey(lblParcelSearchTitle.LabelKey);
            }
        }

        /// <summary>
        /// Select address and associated parcel/owner. If select successfully, close this page and update parent page.
        /// </summary>
        private void SelectParcelAndAssociates()
        {
            ParcelModel parcel = ucParcelSearchResult.SelectedParcel;
            AddressModel address = ucParcelSearchResult.SelectedAddress;

            OwnerModel owner = ucParcelSearchResult.SelectedOwner;

            // Check condition.
            ucParcelSearchResult.ShowCondition(address, parcel, owner);

            if (ucParcelSearchResult.ConditionResult != ConditionResult.Lock)
            {
                APOSessionParameter.SelectedAddress = address;
                APOSessionParameter.SelectedParcel = parcel;
                APOSessionParameter.SelectedOwner = owner;
                APOSessionParameter.ConditionInfo = null;

                if (ucParcelSearchResult.ConditionResult != ConditionResult.None)
                {
                    APOSessionParameter.ConditionInfo = ucParcelSearchResult.ParcelModelWithCondition;
                }

                AppSession.SetAPOSessionParameter(APOSessionParameter);

                ScriptManager.RegisterStartupScript(Page, GetType(), "UpdateParcelAndAssociates", "UpdateParcelAndAssociates();", true);
            }
        }

        #endregion Methods
    }
}
