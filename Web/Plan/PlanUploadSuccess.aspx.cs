#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: plan_PlanUploadSuccess.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PlanUploadSuccess.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Web;

/// <summary>
/// the class for PlanUploadSuccess.
/// </summary>
public partial class PlanUploadSuccess : BasePage
{
    #region Methods

    /// <summary>
    /// Page Load Method.
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">EventArgs e</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        urlPlanPay.NavigateUrl = "~/plan/PlanPay.aspx?planSeqNbr=" + Request.Params["planSeqNbr"];
    }

    #endregion Methods
}