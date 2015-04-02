#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FieldMappingAttribute.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: FieldMappingAttribute.cs 130988 2009-9-4  18:18:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Field mapping attribute class for global search
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldMappingAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the GView element ID
        /// </summary>
        public string GviewElementId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the java property name
        /// </summary>
        public string JavaPropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the header text
        /// </summary>
        public string HeaderText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data field name
        /// </summary>
        public string DataFieldName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the column is visible 
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order of "search by address"
        /// </summary>
        public int SearchbyByAddressOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order of "search by parcel"
        /// </summary>
        public int SearchbyByParcelOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order of "search by owner"
        /// </summary>
        public int SearchbyByOwnerOrder
        {
            get;
            set;
        }
    }
}
