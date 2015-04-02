#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaButton.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2010
 *
 *  Description:This class is used for rendering control.
 *
 *  Notes:
 * $Id: ControlLayoutType.cs 172069 2010-05-06 07:41:43Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.Web.Controls
{
    /// <summary>
    /// Control Layout Type
    /// </summary>
    public enum ControlLayoutType
    {
        /// <summary>
        /// Control display as vertical
        /// </summary>
        Vertical = 0,

        /// <summary>
        /// Control display as horizontal style
        /// </summary>
        Horizontal = 1 
    }

    /// <summary>
    /// Label Display style.
    /// </summary>
    public enum LabelDisplay
    {
        /// <summary>
        /// Label display in the top of the control
        /// </summary>
        TOP,

        /// <summary>
        /// Label display in the left.
        /// </summary>
        LEFT
    }
}
