#region Header

/**
 *  Accela Citizen Access
 *  File: FunctionItem.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011
 *
 *  Description:
 *   It provides the function item enum.
 *
 *  Notes:
 * $Id: FunctionItem.cs 192687 2011-03-14 05:38:13Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

namespace Accela.ACA.CustomizeAPI
{
    /// <summary>
    /// This the function item
    /// </summary>
    public enum FunctionItem
    {
        /// <summary>
        /// create application
        /// </summary>
        CreateApplication,

        /// <summary>
        /// renew record
        /// </summary>
        RenewRecord,

        /// <summary>
        /// obtain fee estimate
        /// </summary>
        ObtainFeeEstimate,

        /// <summary>
        /// make payment
        /// </summary>
        MakePayment,

        /// <summary>
        /// do schedule inspection
        /// </summary>
        ScheduleInspection,

        /// <summary>
        /// upload document
        /// </summary>
        UploadDocument,

        /// <summary>
        /// clone record
        /// </summary>
        CloneRecord,

        /// <summary>
        /// create amendment
        /// </summary>
        CreateAmendment
    }
}
