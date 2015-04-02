#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ASITUITable.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: A derived class from UITable for ASIT.
*
*  Notes:
* $Id: ASITUITable.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Jul 5, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// A derived class from UITable for ASIT.
    /// </summary>
    [Serializable]
    public class ASITUITable : UITable
    {
        /// <summary>
        /// Row unique index column name for convert to DataTable object.
        /// </summary>
        public const string RowIdentityColumnName = "\fRowIndex\f";

        /// <summary>
        /// Private variable for table key property.
        /// </summary>
        private string _tableKey = null;

        /// <summary>
        /// Initializes a new instance of the ASITUITable class.
        /// </summary>
        public ASITUITable()
            : base()
        {
        }
        
        /// <summary>
        /// Gets or sets the agency code.
        /// </summary>
        public string AgencyCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Cap ID model.
        /// </summary>
        public CapIDModel4WS CapID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table key.
        /// Once TableKey be set and is not empty, will not be set again.
        /// </summary>
        public string TableKey
        {
            get
            {
                return _tableKey;
            }

            set
            {
                if (string.IsNullOrEmpty(_tableKey))
                {
                    _tableKey = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table name for display.
        /// </summary>
        public string TableTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the table has drill-down data.
        /// </summary>
        public bool HasDrillDownData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the table is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating wheth
        /// </summary>
        public bool IsTemplateTable
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the maximum row index in the database.
        /// </summary>
        public long MaxRowIndexInDB
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ASIT edit pop page section title
        /// </summary>
        public string SectionTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ASIT edit pop page section index
        /// </summary>
        public int SectionIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table name if Alternative ACA Label file is not null for display.
        /// </summary>
        /// <remarks/>
        public string AlternativeLabel
        {
            get;
            set;
        }
    }
}