#region Header

/**
* <pre>
*
*  Accela Citizen Access
*  File: IControlEntity.cs
*
*  Accela, Inc.
*  Copyright (C): 2008-2014
*
*  Description:
*  Control entity interface, which is requried when creating a web control dynamically.
*
*  Notes:
*      $Id: IControlEntity.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

#endregion Header

using System.Collections.Generic;

using Accela.ACA.Common;

namespace Accela.ACA.Web.Common.Control
{
    /// <summary>
    /// Control entity interface, which is required when creating a web control dynamically.
    /// </summary>
    public interface IControlEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the control Id/control name
        /// </summary>
        string ControlID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control CSS class.
        /// </summary>
        string CssClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        string DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets list items for dropdownlist or radio control. This is available only when ControlType is DropDownList and Radio.
        /// </summary>
        IList<ItemValue> Items
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control label key. if there is value in Label property, this property is disabled.
        /// </summary>
        string LabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets maximum length that user can enter.
        /// </summary>
        int MaxLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is required.
        /// </summary>
        bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unit type value
        /// </summary>
        string UnitType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        string Instruction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the watermark text.
        /// </summary>
        string Watermark
        {
            get;
            set;
        }

        #endregion Properties
    }
}