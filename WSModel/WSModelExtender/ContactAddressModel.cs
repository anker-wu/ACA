#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ContactAddressModel.cs
*
*  Accela, Inc.
*  Copyright (C): 2013
*
*  Description: Contact Address model.
*
*  Notes:
* $Id: ContactAddressModel.cs 240178 2012-12-20 09:43:23Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Dec 5, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// contact address external model
    /// </summary>
    public partial class ContactAddressModel
    {
        /// <summary>
        /// Gets or sets the index of the row.
        /// </summary>
        /// <value>
        /// The index of the row.
        /// </value>
        public int RowIndex
        {
            get;
            set;
        }
    }
}
