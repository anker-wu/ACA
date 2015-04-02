/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GetSessionState.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2013
 * 
 *  Description: 
 * 
 *  Notes:
 *      $Id: GetSessionState.aspx.cs 244794 2013-02-22 09:34:52Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;

/// <summary>
/// Page to get session state
/// </summary>
[SuppressCsrfCheck]
public partial class GetSessionState : System.Web.UI.Page
{
    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Write(AppSession.IsAdmin ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
        Response.End();
    }
}