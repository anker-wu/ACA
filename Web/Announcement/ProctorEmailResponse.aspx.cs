#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProctorEmailResponse.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  ProctorEmailResponse page
 *
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Announcement
{
    /// <summary>
    /// Get the Response for proctor's email.
    /// </summary>
    public partial class ProctorEmailResponse : Page
    {
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            IExaminationBll examBll = ObjectFactory.GetObject<IExaminationBll>();

            string agencyCode = Request.QueryString["agencyCode"];
            string proctorSeq = Request.QueryString["proctorSeq"];
            string isAccept = Request.QueryString["isAccept"];
            string uuid = Request.QueryString["uuid"];

            int status = examBll.UpdateProctorResponseStatus(agencyCode, proctorSeq, isAccept, uuid, ACAConstant.PUBLIC_USER_NAME + "0");
            string message = string.Empty;

            switch (status)
            {
                case 0:
                    message = LabelUtil.GetGlobalTextByKey("aca_examination_msg_proctor_response_accept");
                    break;
                case 1:
                    message = LabelUtil.GetGlobalTextByKey("aca_examination_msg_proctor_response_rejected");
                    break;
                case -1:
                    message = LabelUtil.GetGlobalTextByKey("aca_examination_msg_proctor_response_error");
                    break;
            }

            litMessage.Text = message;
        }
    }
}