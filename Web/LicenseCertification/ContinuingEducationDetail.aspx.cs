#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContinuingEducationDetail.ascx.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using Accela.ACA.Common;

namespace Accela.ACA.Web.LicenseCertification
{
    /// <summary>
    /// ContinuingEducationDetail page
    /// </summary>
    public partial class ContinuingEducationDetail : PopupDialogBasePage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetDialogMaxHeight("600");

            // Account contact edit
            if (Request.QueryString.AllKeys.Contains(UrlConstant.CONTACT_SEQ_NUMBER))
            {
                refContactContEducationDetail1.Visible = true;
                this.SetPageTitleKey("aca_contact_conteducationdetail_label_title");
            }
            else
            {
                //Spear form
                continuingEducationDetail.Visible = true;
                this.SetPageTitleKey("aca_continue_education_detail_label_title");
            }
        }
    }
}