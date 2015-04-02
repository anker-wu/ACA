#region Header

/**
 *  Accela Citizen Access
 *  File: AssetResult.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetResult.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Asset
{
    /// <summary>
    /// Asset result class
    /// </summary>
    public partial class AssetResult : PopupDialogBasePage
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
                lblAssetResultSection.Visible = false;
                SetPageTitleKey("aca_assetresult_label_sectiontitle");
            }
        }
    }
}