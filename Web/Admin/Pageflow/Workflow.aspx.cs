#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Workflow.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: Workflow.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 09/05/2008           daly.zeng               initial version
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;

using Accela.ACA.Web.Common;

/// <summary>
/// Work flow page
/// </summary>
public partial class Admin_Module_Workflow : Page
{
    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!AppSession.IsAdmin)
        {
            Response.Redirect("../login.aspx");
        }
    }
}