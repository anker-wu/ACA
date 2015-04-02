/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TabTemplate.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: TabTemplate.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls.Navigation
{
    /// <summary>
    /// Custom the template for the tab bar
    /// </summary>
    [ToolboxItem(false)]
    public class TabTemplate : Control
    {
        #region Properties

        /// <summary>
        /// Gets or sets innerHTML in the template
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal string InnerHtml
        {
            get
            {
                if (this.Controls.Count > 0 && this.Controls[0] is LiteralControl)
                {
                    return ((LiteralControl)this.Controls[0]).Text;
                }

                return string.Empty;
            }

            set
            {
                this.Controls.Clear();
                this.Controls.Add(new LiteralControl(value));
            }
        }

        #endregion Properties
    }
}