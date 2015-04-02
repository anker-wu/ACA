#region Header

/**
 *  Accela Citizen Access
 *  File: ItemValue.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   Provide Key-Value pair object.
 *
 *  Notes:
 * $Id: ItemValue.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Provide Key-Value pair object.
    /// </summary>
    public class ItemValue
    {
        #region Fields

        /// <summary>
        /// Item value key
        /// </summary>
        private string _key = string.Empty;

        /// <summary>
        /// Item value object.
        /// </summary>
        private object _value;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ItemValue class.
        /// </summary>
        public ItemValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ItemValue class.
        /// </summary>
        /// <param name="key">the key to display.</param>
        /// <param name="value">the real value to store.</param>
        public ItemValue(string key, object value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the ItemValue class.
        /// </summary>
        /// <param name="key">the key to display.</param>
        /// <param name="value">the real value to store.</param>
        public ItemValue(string key, string value)
        {
            _key = key;
            _value = value ?? string.Empty;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the key to be displayed.
        /// </summary>
        public string Key
        {
            get
            {
                return this._key;
            }

            set
            {
                this._key = value;
            }
        }

        /// <summary>
        /// Gets or sets the real object to be stored.
        /// </summary>
        public object Value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;
            }
        }

        #endregion Properties
    }
}