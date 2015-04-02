#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description: Display CAP list
 *
 *  Notes:
 *      $Id: MyCollectionCAPs.ascx.cs 278296 2014-09-01 08:35:43Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation MyCollectionCAPs. 
    /// </summary>
    public partial class MyCollectionCAPs : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets selected caps' alt id
        /// </summary>
        public string[] SelectedAltIDs
        {
            get
            {
                string[] selectedAltIDs = capList.GetSelectedAltIDs();

                return selectedAltIDs;
            }
        }

        /// <summary>
        /// Gets or sets my collection Id
        /// </summary>
        private long? CollectionId
        {
            get
            {
                if (ViewState["CollectionId"] != null)
                {
                    return Convert.ToInt64(ViewState["CollectionId"].ToString());
                }

                return null;
            }

            set
            {
                ViewState["CollectionId"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind CAPs for module of my collection.
        /// </summary>
        /// <param name="collectionId">my collection id</param>
        /// <param name="collectionModuleName">section module name</param>
        /// <param name="moduleNameForDisplay">module name for display</param>
        /// <param name="dtCAPs">data table caps.</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="sort">the sort name</param>
        public void BindCapList(long? collectionId, string collectionModuleName, string moduleNameForDisplay, DataTable dtCAPs, int pageIndex, string sort)
        {
            lblModuleName.Text = moduleNameForDisplay;
            CollectionId = collectionId;
            capList.BindCapList(collectionModuleName, dtCAPs, pageIndex, sort, true);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitalExport();

            if (Page.IsPostBack)
            {
                string gridId = capList.ClientID;

                if (Request.Form["__EVENTTARGET"].IndexOf("btnAppendToCart") > -1 && Request.Form["__EVENTARGUMENT"].IndexOf(gridId) > -1)
                {
                    DataTable selectedCaps = capList.GetSelectedCAPItems();
                    ShoppingCartUtil.AddToCart(Page, selectedCaps, ModuleName);
                }

                if (Request.Form["__EVENTTARGET"].IndexOf("btnCloneRecord") > -1 && Request.Form["__EVENTARGUMENT"].IndexOf(gridId) > -1)
                {
                    DataTable selectedCaps = capList.GetSelectedCAPItems();
                    CloneRecordUtil.ClonePermitListRecord(Page, selectedCaps, ModuleName);
                }

                //Get selected items when click add button.
                if (!string.IsNullOrEmpty(Request.Form["__EVENTTARGET"]))
                {
                    if (Request.Form["__EVENTTARGET"].IndexOf("btnAdd") > -1 || Request.Form["__EVENTTARGET"].IndexOf("lnkRemove") > -1)
                    {
                        MyCollectionModel myCollectionModel = new MyCollectionModel();
                        myCollectionModel.serviceProviderCode = ConfigManager.SuperAgencyCode;
                        myCollectionModel.userId = AppSession.User.PublicUserId;
                        myCollectionModel.collectionId = CollectionId;

                        CollectionOperation.SimpleCapModelList = capList.GetSelectedCAPs();
                        CollectionOperation.MyCollectionModel = myCollectionModel;
                        CollectionOperation.CollectionId = CollectionId;
                    }
                }
            }

            CollectionOperation.SelectedItemsFieldClientID = capList.SelectedItemsFieldClientID;
        }

        /// <summary>
        /// Initial the GridView's export link visible. 
        /// </summary>
        private void InitalExport()
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                capList.ExportFileName = "MyCollectionRecordList";
                capList.InitialExport(true);
            }
            else
            {
                capList.InitialExport(false);
            }
        }

        #endregion Methods
    }
}