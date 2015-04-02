#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaControlRender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2012
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaControlRender.cs 227234 2012-07-20 12:44:44Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;

using AccelaWebControlExtender;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Render Accela control
    /// </summary>
    internal static class AccelaControlRender
    {
        #region Methods

        /// <summary>
        /// Indicates current web controls whether need to be presented as admin mode.
        /// </summary>
        /// <param name="control">web control</param>
        /// <returns>true - render it as admin mode.
        /// false - render it as daily mode.
        /// </returns>
        public static bool IsAdminRender(WebControl control)
        {
            bool isAdmin = false;

            if (control.Page is IPage)
            {
                isAdmin = (control.Page as IPage).IsControlRenderAsAdmin;
            }

            return isAdmin;
        }

        /// <summary>
        /// render accela control that implemented the IAccelaControl interface
        /// </summary>
        /// <param name="writer">HtmlTextWriter object</param>
        /// <param name="control">IAccelaControl object</param>
        internal static void Render(HtmlTextWriter writer, IAccelaControl control)
        {
            if (control is AccelaImageButton)
            {
                ImageButtonRender renderWriter = new ImageButtonRender(writer, control);
                renderWriter.Render();
            }
            else if (control.LayoutType == ControlLayoutType.Vertical)
            {
                VerticalControlRender renderWriter = new VerticalControlRender(writer, control);
                renderWriter.Render();
            }
            else
            {
                HorizontalControlRender renderWriter = new HorizontalControlRender(writer, control);
                renderWriter.Render();
            }
        }

        #endregion Methods
    }
}
