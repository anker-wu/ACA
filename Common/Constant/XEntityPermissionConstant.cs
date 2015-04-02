#region Header

/**
 * <pre>
 *  Accela Citizen Access
 *  File: XEntityPermissionConstant.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description: Constant class for X Entity Permission objects.
 *
 *  Notes:
 * $Id: XEntityPermissionConstant.cs 189014 2011-01-18 07:49:03Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,            Who,        What
 *  Nov 8, 2011      Alan Hu     Initial.
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Constant class for X Entity Permission objects.
    /// </summary>
    public static class XEntityPermissionConstant
    {
        /// <summary>
        /// Contact data validation settings.
        /// </summary>
        public const string CONTACT_DATA_VALIDATION = "ACA_CONTACT_DATA_VALIDATION";

        /// <summary>
        /// document type options
        /// </summary> DOCUMENT_TYPE_OPTIONS
        public const string DOCUMENT_TYPE_OPTIONS = "DOCUMENT_TYPE_OPTIONS";

        /// <summary>
        /// Reference contact searchable settings.
        /// </summary>
        public const string REFERENCE_CONTACT_SEARCH = "ACA_REFERENCE_CONTACT_SEARCH";

        /// <summary>
        /// Reference licensed professional searchable settings.
        /// </summary>
        public const string REFERENCE_LICENSED_PROFESSIONAL_SEARCH = "ACA_REFERENCE_LICENSED_PROFESSIONAL_SEARCH";

        /// <summary>
        ///  The license type mapping licensing board key.
        /// </summary>
        public const string LICENSING_BOARD = "ACA_LICENSING_BOARD";

        /// <summary>
        /// The reference contact view editable setting by contact type level.
        /// </summary>
        public const string REFERENCE_CONTACT_EDITABLE_BY_CONTACT_TYPE = "ACA_REFERENCE_CONTACT_EDITABLE_BY_CONTACT_TYPE";
    }
}