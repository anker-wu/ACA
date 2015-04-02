#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AssetsExtender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AssetsExtender.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI;
using AjaxControlToolkit;

[assembly: WebResource("Accela.Web.Controls.Assets.error_16.gif", "image/gif")]

namespace AccelaWebControlExtender
{
    /// <summary>
    /// Class AssetsExtender.
    /// </summary>
    [TargetControlType(typeof(Accela.Web.Controls.IAccelaControl))]
    public class AssetsExtender : ExtenderControlBase
    {
    }
}
