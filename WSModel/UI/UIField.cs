#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UIField.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2013
*
*  Description: A field of the UI row.
*
*  Notes:
* $Id: UIField.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Jul 5, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// A field of the UI row.
    /// </summary>
    [Serializable]
    public class UIField
    {
        /// <summary>
        /// Gets or sets the field ID.
        /// </summary>
        public string FieldID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the I18n field name
        /// </summary>
        public string ResName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field type.
        /// The type corresponding to the [Accela.ACA.Common.ACAConstant.FieldType] enumeration.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field name for display.
        /// </summary>
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field is hidden.
        /// </summary>
        public bool IsHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field is required.
        /// </summary>
        public bool IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field maximum length.
        /// </summary>
        public int MaxLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field instructions.
        /// </summary>
        public string Instruction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field watermark.
        /// </summary>
        public string Watermark
        {
            get;
            set;
        }
    }
}