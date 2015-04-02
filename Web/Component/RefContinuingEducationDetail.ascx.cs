#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContinuingEducationDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefContinuingEducationDetail.ascx.cs 140932 2009-07-27 07:29:00Z ACHIEVO\jackie.yu $.
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
    /// the class for refContinuingEducationDetail.
    /// </summary>
    public partial class RefContinuingEducationDetail : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Display Continuing Education Detail.
        /// </summary>
        /// <param name="refContinuingEducationModel">a refContinuingEducationModel</param>
        public void Display(RefContinuingEducationModel4WS refContinuingEducationModel)
        {
            if (refContinuingEducationModel == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(refContinuingEducationModel.contEduName))
            {
                lblCourseName.Text = refContinuingEducationModel.contEduName;
                lblCourseNameTitle.Visible = true;
            }

            if (!string.IsNullOrEmpty(refContinuingEducationModel.comments))
            {
                lblComment.Text = refContinuingEducationModel.comments;
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
                lblCourseNameTitle.Visible = true;
                lblCommentTitle.Visible = true;
            }
        }

        #endregion Methods
    }
}
