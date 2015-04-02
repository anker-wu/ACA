#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ConditionViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ConditionViewModel.cs 209458 2011-3-29 17:06:07Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 04-23-2012           Daly zeng               Initial
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// condition list item view model
    /// </summary>
    [Serializable]
    public class ConditionViewModel
    {
        /// <summary>
        /// Gets or sets the condition number
        /// </summary>
        public long ConditionNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record type
        /// </summary>
        public string RecordType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the group name
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition type
        /// </summary>
        public string ConditionType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition name
        /// </summary>
        public string ConditionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Condition Status
        /// </summary>
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition status type order
        /// </summary>
        public int StatusTypeOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Condition Severity
        /// </summary>
        public string Severity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Condition Severity order
        /// </summary>
        public int SeverityOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status date
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status date
        /// </summary>
        public string PriorityText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition short comment
        /// </summary>
        public string ShortComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition long comment
        /// </summary>
        public string LongComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition applied by department.
        /// </summary>
        public string AppliedByDept
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition applied by user
        /// </summary>
        public string AppliedByUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition action by department
        /// </summary>
        public string ActionByDept
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition action by user
        /// </summary>
        public string ActionByUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status date
        /// </summary>
        public DateTime? StatusDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status date for UI
        /// </summary>
        public string StatusDateString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition applied date
        /// </summary>
        public string AppliedDateString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition effective date
        /// </summary>
        public string EffectiveDateString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition expiration date
        /// </summary>
        public string ExpirationDateString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition additional information
        /// </summary>
        public string AdditionalInformation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is condition of approval
        /// </summary>
        public bool IsConditionOfApproval
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether status is applied or met
        /// </summary>
        public bool IsApplied
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether hide the record type 
        /// </summary>
        public bool HideRecordType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether hide the group name
        /// </summary>
        public bool HideGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether hide the condition type
        /// </summary>
        public bool HideConditionType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Agency Code for the condition record.
        /// </summary>
        public string ServiceProviderCode
        {
            get;
            set;         
        }
    }
}