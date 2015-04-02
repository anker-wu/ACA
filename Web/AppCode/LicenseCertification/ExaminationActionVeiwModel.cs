#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationActionVeiwModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Web.Examination
{
    #region enum
     
    /// <summary>
    /// Examination action.
    /// </summary>
    public enum ExaminationAction
    {
        /// <summary>
        /// None = 0 constant.
        /// </summary>
        None = 0,

        /// <summary>
        /// View examination details
        /// </summary>
        ViewDetails, 

        /// <summary>
        /// Schedule examination details
        /// </summary>
        Schedule, 

        /// <summary>
        /// Re-schedule constant.
        /// </summary>
        Reschedule,

        /// <summary>
        /// Delete constant.
        /// </summary>
        Delete,

        /// <summary>
        /// Cancel constant.
        /// </summary>
        Cancel,

        /// <summary>
        /// Edit examination.
        /// </summary>
        Edit,

        /// <summary>
        /// Take Examination links
        /// </summary>
        TakeExamination,

        /// <summary>
        /// reset the ready to schedule exam to pending
        /// </summary>
        Reset,

        /// <summary>
        /// The retry schedule for ready to schedule exam 
        /// </summary>
        RetrySchedule
    }

    #endregion enum

    /// <summary>
    /// Examination action view model
    /// </summary>
    [Serializable]
    public class ExaminationActionViewModel
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public ExaminationAction Action
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExaminationActionViewModel"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExaminationActionViewModel"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the action label.
        /// </summary>
        /// <value>The action label.</value>
        public string ActionLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target URL.
        /// </summary>
        /// <value>The target URL.</value>
        public string TargetURL
        {
            get;
            set;
        }
    }
}
