#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: StepChangedEventArgs.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: StepChangedEventArgs.cs 151830 2010-04-26 13:39:43Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.Web.Controls
{
    /// <summary>
    /// when step index changed the event is raised.
    /// update the old step index.
    /// </summary>
    public sealed class StepChangedEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// previous step index.
        /// </summary>
        private readonly int _newstepindex;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the StepChangedEventArgs class.
        /// update current step index.
        /// </summary>
        /// <param name="newstepindex">the current page step index.</param>
        public StepChangedEventArgs(int newstepindex)
        {
            _newstepindex = newstepindex;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the last step index.
        /// </summary>
        public int NewStepIndex
        {
            get
            {
                return _newstepindex;
            }
        }

        #endregion Properties
    }
}
