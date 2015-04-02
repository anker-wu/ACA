#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefEducationDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefEducationDetail.ascx.cs 146055 2009-09-04 07:51:13Z ACHIEVO\jackie.yu $.
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
    /// UC for RefEducationDetail
    /// </summary>
    public partial class RefEducationDetail : BaseUserControl
    {
        /// <summary>
        /// Display Education Detail.
        /// </summary>
        /// <param name="refEducationModel">a refEducationModel</param>
        public void DisplayEducationDetail(RefEducationModel4WS refEducationModel)
        {
            if (refEducationModel == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(refEducationModel.refEducationName))
            {
                lblName.Text = refEducationModel.refEducationName;
                lblNameTitle.Visible = true;
            }

            if (!string.IsNullOrEmpty(refEducationModel.degree))
            {
                lblDegree.Text = refEducationModel.degree;
                lblDegreeTitle.Visible = true;
            }

            if (!string.IsNullOrEmpty(refEducationModel.comments))
            {
                lblComment.Text = refEducationModel.comments;
                lblCommentTitle.Visible = true;
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && AppSession.IsAdmin)
            {
                lblNameTitle.Visible = true;
                lblDegreeTitle.Visible = true;
                lblCommentTitle.Visible = true;
            }
        }
    }
}
