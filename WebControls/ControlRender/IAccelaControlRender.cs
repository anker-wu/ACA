#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAccelaControlRender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IAccelaControlRender.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Interface for rendering Accela control
    /// </summary>
    internal interface IAccelaControlRender
    {
        #region Methods

        /// <summary>
        /// determine if to render sub label
        /// </summary>
        /// <param name="subLabel">string sub label</param>
        /// <returns>true to render,otherwise not to render</returns>
        bool IsRenderSubLabel(string subLabel);

        /// <summary>
        /// called on control PreRender event
        /// </summary>
        /// <param name="control">object WebControl</param>
        void OnPreRender(WebControl control);

        #endregion Methods
    }
}