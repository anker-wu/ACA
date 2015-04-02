#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapReviewCertification.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapReviewCertification.ascx.cs 250741 2013-06-04 05:17:01Z ACHIEVO\jone.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Web.UI;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// CapReviewCertification Control
    /// </summary>
    public partial class CapReviewCertification : BaseUserControl
    {
        /// <summary>
        /// Validate re-certification
        /// </summary>
        /// <returns>Error message if any.</returns>
        public bool Validate()
        {
            if (!termReviewAccept.Checked)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ReCertificationError", "chbReviewAccept_OnClick();", true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the certification date
        /// </summary>
        /// <returns>String value of certification date</returns>
        public string GetCertificationDateValue()
        {
            return I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(lblDateValue.Text);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin && !IsPostBack)
            {
                ITimeZoneBll timeBll = ObjectFactory.GetObject<ITimeZoneBll>();
                DateTime currentDate = timeBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);
                lblDateValue.Text = I18nDateTimeUtil.FormatToDateStringForUI(currentDate);
                termReviewAccept.Attributes["title"] = LabelUtil.RemoveHtmlFormat(GetTextByKey("aca_recertification_label_acceptterms"));
            }
        }
    }
}