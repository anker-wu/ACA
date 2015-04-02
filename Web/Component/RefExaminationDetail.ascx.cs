#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefExaminationDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefExaminationDetail.ascx.cs 140932 2009-07-27 07:29:00Z ACHIEVO\jackie.yu $.
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
    /// the class for refExaminationDetail.
    /// </summary>
    public partial class RefExaminationDetail : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Display RefExamination Detail.
        /// </summary>
        /// <param name="refExaminationModel">a refExaminationModel</param>
        public void Display(RefExaminationModel4WS refExaminationModel)
        {
            if (refExaminationModel == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(refExaminationModel.examName))
            {
                lblExamName.Text = refExaminationModel.examName;
                lblExamNameTitle.Visible = true;
            }

            if (!string.IsNullOrEmpty(refExaminationModel.comments))
            {
                lblComment.Text = refExaminationModel.comments;
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
                lblExamNameTitle.Visible = true;
                lblCommentTitle.Visible = true;
            }
        }

        #endregion Methods
    }
}
