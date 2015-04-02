#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PeopleModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PeopleModel.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// The external for people model
    /// </summary>
    public partial class PeopleModel
    {
        /// <summary>
        /// Gets or sets the search row index.
        /// </summary>
        public int SearchRowIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the search category text.
        /// </summary>
        public string SearchCategoryText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact type
        /// </summary>
        public string SearchContactType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact name
        /// </summary>
        public string SearchContactName
        {
            get;
            set;
        }

        /// <summary>
        /// Use for store the relationship for public user & reference contact. (only used in .net side)
        /// </summary>
        public string contractorPeopleStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Use for store the account owner for public user.
        /// </summary>
        public string accountOwner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a JSON string indicating the special parameters of the self or the other object 
        /// such as <see cref="PeopleModel4WS"/>, <see cref="OwnerModel"/>, <see cref="LicenseModel4WS"/>. 
        /// Used in Contact edit for conditions.
        /// </summary>
        public string ConditionParameters
        {
            get;
            set;
        }

        /// <summary>
        /// Used to store agency display text
        /// </summary>
        public string SearchAgencyText
        { 
            get;
            set;
        }
    }
}
