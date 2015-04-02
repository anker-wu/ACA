#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AGISRequest.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AGISRequest.aspx.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.GIS
{
    /// <summary>
    /// the class of sending request to <c>mapviewer.aspx</c>
    /// </summary>
    public partial class AGISRequest : Page
    {
        /// <summary>
        /// the GIS ULR suffix.
        /// </summary>
        private const string GIS_URL_SUFFIX = "MapViewer.aspx";

        /// <summary>
        /// Page load event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string agencyCode = ConfigManager.AgencyCode;
                string url = StandardChoiceUtil.GetGisAddressLocatorURL(agencyCode);
                if (string.IsNullOrEmpty(url))
                {
                    dvError.Visible = true;
                    dvError.InnerText = string.Format(LabelUtil.GetGlobalTextByKey("aca_gis_msg_noactivemap"), agencyCode);
                    this.ClientScript.RegisterHiddenField("CancelSend", "true");
                }
                else
                {
                    if (!url.EndsWith(GIS_URL_SUFFIX, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (url.EndsWith("/"))
                        {
                            url = url + GIS_URL_SUFFIX;
                        }
                        else
                        {
                            url = url + "/" + GIS_URL_SUFFIX;
                        }
                    }

                    if (!ValidationUtil.IsValidUrl(url.Replace(GIS_URL_SUFFIX, "default.htm")))
                    {
                        dvError.Visible = true;
                        dvError.InnerText = LabelUtil.GetGlobalTextByKey("aca_gis_msg_noconnection_error");
                        this.ClientScript.RegisterHiddenField("CancelSend", "true");
                    }
                    else
                    {
                        form1.Attributes.Add("action", url);
                    }
                }
            }
        }
    }
}