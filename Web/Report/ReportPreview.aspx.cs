#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ReportPreview.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ReportPreview.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;

namespace Accela.ACA.Web.Report
{
    /// <summary>
    /// Report Preview page.
    /// </summary>
    public partial class ReportPreview : BasePageWithoutMaster
    {
        #region property

        /// <summary>
        /// Gets the report URL.
        /// </summary>
        public string ReportURL
        {
            get 
            { 
                return GetReportURL(); 
            }
        }

        #endregion

        /// <summary>
        /// Page Load.
        /// </summary>
        /// <param name="sender">Event Sender</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string licenseReportUrl = string.Format("EmailReport.aspx?{0}", HttpContext.Current.Request.QueryString.ToString());
            btnSendEmail.Attributes.Add("href", "javascript:print_onclick('" + licenseReportUrl + "')");
        }

        /// <summary>
        /// Send Email Button Event.
        /// </summary>
        /// <param name="sender">Event Sender</param>
        /// <param name="e">Event arguments</param>
        protected void SendEmailButton_Click(object sender, EventArgs e)
        {
            //Response.Redirect(String.Format("EmailReport.aspx?{0}", HttpContext.Current.Request.QueryString.ToString()));
        }
        
        /// <summary>
        /// Get Show Report Page's URL.
        /// </summary>
        /// <returns>The report URL.</returns>
        private string GetReportURL()
        {
            System.Text.StringBuilder sbURL = new StringBuilder();
            string reportURL = string.Empty;
            sbURL.Append("ShowReport.aspx");
            NameValueCollection reportParams = new NameValueCollection(Request.QueryString);

            if (reportParams != null && reportParams.Count > 0)
            {
                sbURL.Append(ACAConstant.QUESTION_MARK);

                foreach (string key in reportParams.Keys)
                {
                    if (!ACAConstant.Display_Send_Email.Equals(key))
                    {
                        sbURL.Append(key).Append(ACAConstant.EQUAL_MARK).Append(ScriptFilter.AntiXssUrlEncode(reportParams[key])).Append(ACAConstant.AMPERSAND);
                    }
                }

                reportURL = sbURL.ToString();
                reportURL = reportURL.Substring(0, reportURL.Length - 1);
            }
            else
            {
                reportURL = sbURL.ToString();
            }

            return reportURL;
        }
    }
}
