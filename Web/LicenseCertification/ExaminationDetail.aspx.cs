#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  Examination detail for license detail.
 * 
 *  Notes:
 *      $Id: EducationDetail.ascx.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
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
    /// examination detail for license detail page
    /// </summary>
    public partial class ExaminationDetail : PopupDialogBasePage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey(string.Empty);
            SetDialogMaxHeight("600");

            // Account contact edit
            if (Request.QueryString.AllKeys.Contains(UrlConstant.CONTACT_SEQ_NUMBER))
            {
                refContactExamDetail.Visible = true;
                this.SetPageTitleKey("aca_contact_examdetail_label_title");
            }
            else
            {
                //Spear form
                examDetail.Visible = true;
                this.SetPageTitleKey("aca_exam_pcompleted_label_title");
            }
        }
    }
}