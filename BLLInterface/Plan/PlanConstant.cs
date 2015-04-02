/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PlanConstant.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PlanConstant.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.BLL.Plan
{
    #region Enumerations

    /// <summary>
    /// Review action ENUM list.
    /// </summary>
    public enum PlanReviewAction
    {
        /// <summary>
        /// 0 means is deleted action.
        /// </summary>
        Delete,

        /// <summary>
        /// 1 means is viewed report action.
        /// </summary>
        ViewReport,

        /// <summary>
        /// 2 means do nothing
        /// </summary>
        None
    }

    /// <summary>
    /// This ENUM list are status of plan reviewing.
    /// </summary>
    public enum PlanReviewStatus
    {
        /// <summary>
        /// 0 means the plan review status is uploaded.
        /// </summary>
        Uploaded,

        /// <summary>
        /// 1 means the plan review status is payment.
        /// </summary>
        Paid,

        /// <summary>
        /// 2 means the plan review status is failed.
        /// </summary>
        Failed,

        /// <summary>
        /// 3 means the plan review status is completed.
        /// </summary>
        Completed,

        /// <summary>
        /// 4 means the plan review doesn't happen until now.
        /// </summary>
        All
    }

    #endregion Enumerations
}