#region Header

/**
 *  Accela Citizen Access
 *  File: AssetSearch.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetSearch.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Asset
{
    /// <summary>
    /// Asset search class
    /// </summary>
    public partial class AssetSearch : PopupDialogBasePage
    {
        /// <summary>
        /// On initial event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            DialogUtil.RegisterScriptForDialog(Page);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("550");

            if (!AppSession.IsAdmin)
            {
                lblAssetSearchSection.Visible = false;
                SetPageTitleKey("aca_assetsearch_label_sectiontitle");
            }
            else
            {
                string prefix = assetSearch.PrefixForDesigner;
                lblAssetSearchSection.SectionID = string.Format("{1}{0}{2}{0}{3}", ACAConstant.SPLIT_CHAR, ModuleName, GviewID.SearchForAsset, prefix);
            }
        }
    }
}