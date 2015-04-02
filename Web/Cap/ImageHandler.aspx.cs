#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ImageHandler.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ImageHandler.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

/// <summary>
/// This class provide the ability to operation image handler. 
/// </summary>
public partial class ImageHandler : System.Web.UI.Page
{
    #region Methods

    /// <summary>
    /// The constant for default image format.
    /// </summary>
    private const string DEFAULT_IMAGE_FORMAT = ".png";

    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string agencyCode = Request.QueryString[UrlConstant.AgencyCode];
            string logoType = Request.QueryString["logoType"];
            bool forDownload = ValidationUtil.IsYes(Request.QueryString["forDownload"]);

            if (!string.IsNullOrEmpty(agencyCode) && !string.IsNullOrEmpty(logoType))
            {
                GenerateLogoByType(agencyCode, logoType, forDownload);
            }
            else if (agencyCode != null)
            {
                GenerateASILogo(agencyCode);
            }
        }
    }

    /// <summary>
    /// Generate logo for ASI section.
    /// </summary>
    /// <param name="agencyCode">the agency code.</param>
    private void GenerateASILogo(string agencyCode)
    {
        ILogoBll logo = (ILogoBll)ObjectFactory.GetObject(typeof(ILogoBll));
        LogoModel logoModel = logo.GetAgencyLogo(agencyCode);

        if (logoModel != null)
        {
            byte[] datas = logoModel.docContent;
            Response.OutputStream.Write(datas, 0, datas.Length);
        }
    }

    /// <summary>
    /// Generates the type of the logo by.
    /// </summary>
    /// <param name="agencyCode">The agency code.</param>
    /// <param name="logoType">Type of the logo.</param>
    /// <param name="forDownload">if set to <c>true</c> [for download].</param>
    private void GenerateLogoByType(string agencyCode, string logoType, bool forDownload)
    {
        var logo = ObjectFactory.GetObject<ILogoBll>();
        LogoModel logoModel = logo.GetAgencyLogoByType(agencyCode, logoType);

        if (logoModel != null)
        {
            byte[] datas = logoModel.docContent;

            if (forDownload)
            {
                DoDownload(datas, logoType);
            }
            else
            {
                Response.OutputStream.Write(datas, 0, datas.Length);
            }
        }
    }

    /// <summary>
    /// Does the download.
    /// </summary>
    /// <param name="contentBuffer">The content buffer.</param>
    /// <param name="fileName">Name of the file.</param>
    private void DoDownload(byte[] contentBuffer, string fileName)
    {
        int contentBufferSize = contentBuffer == null ? 0 : contentBuffer.Length;

        if (contentBufferSize > 0)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName).Replace("+", "%20") + DEFAULT_IMAGE_FORMAT); //resolve the filename with empty space char
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(contentBuffer, 0, contentBufferSize);
            Response.Flush();
            Response.Close();
        }
    }

    #endregion Methods
}