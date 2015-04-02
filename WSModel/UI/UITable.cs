#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UITable.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2013
*
*  Description: To introduce UI Model for apply Expression in any form.
*
*  Notes:
* $Id: UITable.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Jul 5, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// Base UI model for expression execution.
    /// </summary>
    [Serializable]
    public abstract class UITable
    {
        /// <summary>
        /// Initializes a new instance of the UITable class.
        /// </summary>
        protected UITable()
        {
            Rows = new List<UIRow>();
        }

        /// <summary>
        /// Gets or sets instructions.
        /// </summary>
        public string Instruction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets UI rows.
        /// </summary>
        public List<UIRow> Rows
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets template row.
        /// </summary>
        public UIRow TemplateRow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the add row button label
        /// </summary>
        public string ButtonAddLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the add rows button label pattern
        /// </summary>
        public string ButtonAddMoreLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the edit row button label
        /// </summary>
        public string ButtonEditLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the delete row button text
        /// </summary>
        public string ButtonDeleteLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets indicating whether the add button display or not.
        /// </summary>
        public string ButtonAddDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets indicating whether the edit button display or not.
        /// </summary>
        public string ButtonEditDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets indicating whether the delete button display or not.
        /// </summary>
        public string ButtonDeleteDisplay
        {
            get;
            set;
        }
    }
}