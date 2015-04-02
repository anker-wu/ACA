#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PrintPage.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description: Provide a page to preview before printing.
 *
 *  Notes:
 *      $Id: PrintPage.aspx.cs 183850 2011-03-31 09:43:26Z ACHIEVO\Eleven.Li $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Web.Print
{
    /// <summary>
    /// A class used to preview page before printing.
    /// </summary>
    public partial class PrintPage : BasePageWithoutMaster
    {
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}