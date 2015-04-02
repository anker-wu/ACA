#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: plan_PlanList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PlanList.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Web;

/// <summary>
/// the class to operation plan list.
/// </summary>
public partial class PlanList : BasePage
{
    #region Methods

    /// <summary>
    /// Page Load Method.
    /// </summary>
    /// <param name="sender">object sender,</param>
    /// <param name="e">EventArgs e</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
    }

    #endregion Methods
}