#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WorkLocation.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description: Select a date for schedule an inspection
 *
 *  Notes:
 *      $Id: WorkLocation.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// the class for WorkLocation.
    /// </summary>
    public partial class WorkLocation : BasePage
    {
        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ValidationUtil.IsYes(Request.QueryString["createRecordByService"]))
                {
                    divAddress.Visible = false;
                    divService.Visible = true;
                    AppSession.SelectedParcelInfo = null;
                    serviceControl.IsForLocation = false;

                    if (!AppSession.IsAdmin)
                    {
                        serviceControl.BindServiceList(null);
                    }
                }
                else
                {
                    WorkLocationEdit.IsValidate = false;
                    WorkLocationEdit.DisplayAddress(null, true, false);

                    lblWorkLocation.SectionID = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR + "{2}" + ACAConstant.SPLIT_CHAR5, ModuleName, GviewID.AddressEdit, WorkLocationEdit.ClientID);

                    //clear session if user return from cap type page.
                    AppSession.SelectedParcelInfo = null;
                    AppSession.SetSelectedServicesToSession(null);
                }
            }
        }

        #endregion Methods
    }
}