#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UIRow.cs
*
*  Accela, Inc.
*  Copyright (C): 2011
*
*  Description: A row of the UI table.
*
*  Notes:
* $Id: UIRow.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Jul 5, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// A row of the UI table
    /// </summary>
    [Serializable]
    public class UIRow
    {
        /// <summary>
        /// Initializes a new instance of the UIRow class.
        /// </summary>
        public UIRow()
        {
            Fields = new List<UIField>();
        }

        /// <summary>
        /// Gets or sets UI fields
        /// </summary>
        public IList<UIField> Fields
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the row index of the table.
        /// </summary>
        public long RowIndex
        {
            get;
            set;
        }
    }
}