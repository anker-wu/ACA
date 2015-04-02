#region Header

/**
* <pre>
*
*  Accela Citizen Access
*  File: AccelaSeparatorLine.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description:
*  Separator Line control
*
*  Notes:
*      $Id: AccelaSeparatorLine.cs 198440 2011-06-30 05:16:09Z ACHIEVO\grady.lu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

#endregion Header

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a separator line control
    /// </summary>
    public partial class AccelaSeparatorLine : WebControl
    {
        /// <summary>
        /// overwrite render 
        /// </summary>
        /// <param name="writer">HtmlTextWriter object</param>
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div id='{0}' class='ACA_TabRow ACA_Line_Content'>&nbsp;</div>", ClientID);
        }
    }
}
