#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Cap_SecurityCodeInfo.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description: security code info
 *
 *  Notes:
 *      $Id: SecurityCodeInfo.aspx.cs 131973 2009-05-22 01:07:37Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Web;

/// <summary>
/// The class for Cap_SecurityCodeInfo.
/// </summary>
public partial class Cap_SecurityCodeInfo : BasePageWithoutMaster
{
    #region Methods

    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Head1.Title = GetTextByKey("ACA_SecurityCodeInfo_PageTitle");
    }

    #endregion Methods
}