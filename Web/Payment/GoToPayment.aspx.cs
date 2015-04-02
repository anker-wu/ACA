#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Payment_GoToPayment.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GoToPayment.aspx.cs 277625 2014-08-19 07:06:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Configuration;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;

/// <summary>
/// This class provide the ability to operation Payment_GoToPayment.
/// </summary>
public partial class Payment_GoToPayment : System.Web.UI.Page
{
    #region Methods

    /// <summary>
    /// Page load method.
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">EventArgs e</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string thirdPartyURL = Request["url"];
        Session[ACAConstant.CURRENT_URL] = thirdPartyURL;
        string defaultPage = FileUtil.AppendApplicationRoot(ConfigurationManager.AppSettings["DefaultPageFile"]);
        Response.Redirect(defaultPage);
    }

    #endregion Methods
}