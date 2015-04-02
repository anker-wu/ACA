#region Header

/**
 *  Accela Citizen Access
 *  File: ApplicationException.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   It define the user exception to provide the user-friendly message to the end user.
 *
 *  Notes:
 * $Id: ACAException.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 *  2007/04/26       Michael Mao    Initial.
 */

#endregion Header

using System;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Provide a class to defined ACA exception.
    /// </summary>
    [Serializable]
    public class ACAException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ACAException class.
        /// </summary>
        /// <param name="originalException">original exception</param>
        public ACAException(Exception originalException) : base(string.Empty, originalException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ACAException class.
        /// </summary>
        /// <param name="messge">the message info.</param>
        /// <param name="originalException">original exception</param>
        public ACAException(string messge, Exception originalException) : base(messge, originalException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ACAException class.
        /// </summary>
        /// <param name="messge">the message info.</param>
        public ACAException(string messge) : base(messge)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ACAException class.
        /// </summary>
        public ACAException()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets Override message property to return user-friendly message.
        /// </summary>
        public override string Message
        {
            get
            {
                return ExceptionUtil.GetErrorMessage(InnerException);
            }
        }

        #endregion Properties
    }
}