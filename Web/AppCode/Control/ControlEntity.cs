#region Header

/**
* <pre>
*
*  Accela Citizen Access
*  File: ControlEntity.cs
*
*  Accela, Inc.
*  Copyright (C): 2008-2014
*
*  Description:
*  An implement for IControlEntity.
*
*  Notes:
*      $Id: ControlEntity.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
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
    /// An implement for IControlEntity.
    /// </summary>
    public class ControlEntity : IControlEntity
    {
        #region Fields

        /// <summary>
        /// The CSS style
        /// </summary>
        private string _cssClass = "ACA_MLong";
        
        /// <summary>
        /// The item value list
        /// </summary>
        private IList<ItemValue> _items;
        
        /// <summary>
        /// The maximum length
        /// </summary>
        private int _maxLength = 200;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the control Id/control name
        /// </summary>
        public string ControlID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control CSS class.
        /// </summary>
        public string CssClass
        {
            get
            {
                return _cssClass;
            }

            set
            {
                _cssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        public string DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets list items for dropdownlist or radio control. This is available only when ControlType is DropDownList and Radio.
        /// </summary>
        public IList<ItemValue> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<ItemValue>();
                }

                return _items;
            }

            set
            {
                _items = value;
            }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control label key. if there is value in Label property, this property is disabled.
        /// </summary>
        public string LabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets maximum length that user can enter.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }

            set
            {
                _maxLength = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is required.
        /// </summary>
        public bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unit type value
        /// </summary>
        public string UnitType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        public string Instruction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the watermark text.
        /// </summary>
        public string Watermark
        {
            get;
            set;
        }

        #endregion Properties
    }
}