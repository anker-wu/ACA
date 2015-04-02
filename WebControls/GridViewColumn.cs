#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: GridViewColumn.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: GridViewColumn.cs 131439 2009-05-19 11:07:26Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.Web.Controls
{
    #region Enumerations

    /// <summary>
    /// column type for grid view 
    /// </summary>
    public enum GridViewColumnType
    {
        /// <summary>
        /// column is label
        /// </summary>
        Label,

        /// <summary>
        /// column is hyper link
        /// </summary>
        HyperLink
    }

    #endregion Enumerations

    /// <summary>
    /// Define column for grid view 
    /// </summary>
    public class GridViewColumn
    {
        #region Fields

        /// <summary>
        /// column name
        /// </summary>
        private string _columnName;

        /// <summary>
        /// the CSS Class for the header row
        /// </summary>
        private string _cssHeader;

        /// <summary>
        /// the CSS Class for the Item Row
        /// </summary>
        private string _cssItem;

        /// <summary>
        /// data type for grid
        /// </summary>
        private Type _dataType;

        /// <summary>
        /// Grid View Column Type
        /// </summary>
        private GridViewColumnType _gridViewColType;

        /// <summary>
        /// the mask string
        /// </summary>
        private string _mask;

        /// <summary>
        /// indicate the column visible or not
        /// </summary>
        private bool _visible;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GridViewColumn class.
        /// </summary>
        /// <param name="gridViewColType">the column type</param>
        /// <param name="colName">the column name</param>
        /// <param name="dataType">the data type</param>
        /// <param name="visible">whether the column is visible</param>
        public GridViewColumn(GridViewColumnType gridViewColType, string colName, Type dataType, bool visible) : this(gridViewColType, colName, dataType, null, null, null, visible)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GridViewColumn class.
        /// </summary>
        /// <param name="gridViewColType">The Column Type</param>
        /// <param name="colName">The Column Name</param>
        /// <param name="dataType">The Data Type</param>
        /// <param name="cssHeader">The CSS Header Class Name</param>
        /// <param name="cssItem">The CSS Item Class Name</param>
        /// <param name="mask">The Mask string</param>
        /// <param name="visible">Whether is column is visible</param>
        public GridViewColumn(GridViewColumnType gridViewColType, string colName, Type dataType, string cssHeader, string cssItem, string mask, bool visible)
        {
            _gridViewColType = gridViewColType;
            _columnName = colName;
            _dataType = dataType;
            _cssHeader = cssHeader;
            _cssItem = cssItem;
            _mask = mask;
            _visible = visible;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the CSS Class for the Header
        /// </summary>
        public string CSSHeader
        {
            get
            {
                return _cssHeader;
            }

            set
            {
                _cssHeader = value;
            }
        }

        /// <summary>
        /// Gets or sets the CSS Class for the Item Row
        /// </summary>
        public string CSSItem
        {
            get
            {
                return _cssItem;
            }

            set
            {
                _cssItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the column name
        /// </summary>
        public string ColumnName
        {
            get
            {
                return _columnName;
            }

            set
            {
                _columnName = value;
            }
        }

        /// <summary>
        /// Gets or sets the Data Type
        /// </summary>
        public Type DataType
        {
            get
            {
                return _dataType;
            }

            set
            {
                _dataType = value;
            }
        }

        /// <summary>
        /// Gets or sets the Grid View Column Type
        /// </summary>
        public GridViewColumnType GridViewColType
        {
            get
            {
                return _gridViewColType;
            }

            set
            {
                _gridViewColType = value;
            }
        }

        /// <summary>
        /// Gets or sets the Mask
        /// </summary>
        public string Mask
        {
            get
            {
                return _mask;
            }

            set
            {
                _mask = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the column is visible or not
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
            }
        }

        #endregion Properties
    }
}