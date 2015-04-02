#region Header

/**
 *  Accela Citizen Access
 *  File: CommonEventArgs.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  Represents common event data classes in ACA.
 *
 *  Notes:
 * $Id: CommonEventArgs.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;

namespace Accela.ACA.Common
{
    #region Delegates

    /// <summary>
    /// Represents a method that will handle an event.
    /// An common event handler as delegate.
    /// </summary>
    /// <param name="sender">event sender.</param>
    /// <param name="arg">Event argument.</param>
    public delegate void CommonEventHandler(object sender, CommonEventArgs arg);

    #endregion Delegates

    /// <summary>
    /// Represents common event data classes in ACA.
    /// </summary>
    public class CommonEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// the argument information.
        /// </summary>
        private object _arg;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CommonEventArgs class.
        /// </summary>
        /// <param name="argValue">an object to be passed to the subscriber.</param>
        public CommonEventArgs(object argValue)
        {
            _arg = argValue;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets an event argument value.
        /// </summary>
        public object ArgObject
        {
            get
            {
                return _arg;
            }

            set
            {
                _arg = value;
            }
        }

        #endregion Properties
    }
}