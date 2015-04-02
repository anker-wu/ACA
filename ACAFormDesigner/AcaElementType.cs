#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AcaElementType.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2012
 *
 *  Description:
 *
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.FormDesigner
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    #region Enumerations

    /// <summary>
    /// ACA Element Type
    /// </summary>
    public enum AcaElementType
    {
        /// <summary>
        /// text box
        /// </summary>
        Text,

        /// <summary>
        /// number box
        /// </summary>
        Number,

        /// <summary>
        /// text area
        /// </summary>
        TextArea,

        /// <summary>
        /// date box
        /// </summary>
        Date,

        /// <summary>
        /// dropdownlist
        /// </summary>
        DropdownList,

        /// <summary>
        /// radio button
        /// </summary>
        Radio,

        /// <summary>
        /// check box
        /// </summary>
        CheckBox,

        /// <summary>
        /// upload control
        /// </summary>
        Upload,

        /// <summary>
        /// text search
        /// </summary>
        TextSearch,

        /// <summary>
        /// separator line
        /// </summary>
        Line,

        /// <summary>
        /// Label control
        /// </summary>
        Label,

        /// <summary>
        /// CheckBox List
        /// </summary>
        CheckBoxList
    }

    #endregion Enumerations
}