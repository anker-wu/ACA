#region Header

/**
 *  Accela Citizen Access
 *  File: ExceptionUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   It provide the exception related utility to serve the framework.
 *
 *  Notes:
 * $Id: ExceptionUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 *  2007/04/26       Michael Mao    Initial.
 */

#endregion Header

using System;
using Accela.ACA.Common.Log;
using log4net;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Provide a class to defined exception utility.
    /// </summary>
    public static class ExceptionUtil
    {
        #region Fields

        /// <summary>
        /// Logger object
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ExceptionUtil));

        /// <summary>
        /// Last Exception.
        /// </summary>
        private static Exception _lastException;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the LastException.
        /// </summary>
        public static Exception LastException
        {
            get
            {
                return _lastException;
            }

            set
            {
                _lastException = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Return user-friendly error message for handling exception.
        /// </summary>
        /// <param name="exception">inner exception</param>
        /// <returns>friendly message</returns>
        public static string GetErrorMessage(Exception exception)
        {
            return GetErrorMessage(exception, string.Empty);
        }

        /// <summary>
        /// Return user-friendly error message for handling exception.
        /// </summary>
        /// <param name="exception">inner exception</param>
        /// <param name="detail">error detail information</param>
        /// <returns>friendly message</returns>
        public static string GetErrorMessage(Exception exception, string detail)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error("error:" + detail, exception);
            }

            string userFriendMessage = string.Empty;

            if (exception != null)
            {
                userFriendMessage = exception.Message;
            }

            userFriendMessage = DataUtil.FilterEscapeChars(userFriendMessage);

            return userFriendMessage;
        }

        #endregion Methods
    }
}
