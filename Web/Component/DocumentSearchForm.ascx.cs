#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DocumentSearchForm.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DocumentSearchForm.ascx.cs 172830 2010-05-15 09:17:46Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// DocumentSearchForm control.
    /// </summary>
    public partial class DocumentSearchForm : BaseUserControl
    {
        /// <summary>
        /// show map.
        /// </summary>
        public void ShowMap()
        {
            mapDocuments.CloseMap();
            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = mapDocuments.AGISContext;
            gisModel.UserGroups.Add(AppSession.User.IsAnonymous ? GISUserGroup.Anonymous.ToString() : GISUserGroup.Register.ToString());
            GISUtil.SetPostUrl(this.Page, gisModel);
            gisModel.ModuleName = ModuleName;
            gisModel.Windowless = true;

            mapDocuments.ACAGISModel = gisModel;
            mapDocuments.ShowMap();
        }

        /// <summary>
        /// The page load event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                mapDocuments.HideGISButton();
            }
        }
    }
}