#region Header

/**
 *  Accela Citizen Access
 *  File: LogFactory.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *   It provide the logging related utility to serve the framework.
 *
 *  Notes:
 * $Id: LogFactory.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 *  2007/04/26       Michael Mao    Initial.
 */

#endregion Header

using System;

using log4net;

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// Provide a factory class to operation log info..
    /// </summary>
    public sealed class LogFactory
    {
        #region Fields

        /// <summary>
        /// Single instance.
        /// </summary>
        private static readonly LogFactory SingleInstance = new LogFactory();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the LogFactory class from being created.
        /// </summary>
        private LogFactory()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///  Gets Return LogFactory single instance. 
        /// </summary>
        public static LogFactory Instance
        {
            get
            {
                return SingleInstance;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Retrieves or creates a named logger.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Retrieves a logger named as the <paramref name="name"/>
        /// parameter. If the named logger already exists, then the
        /// existing instance will be returned. Otherwise, a new instance is
        /// created.
        /// </para>
        /// <para>By default, loggers do not have a set level but inherit
        /// it from the hierarchy. This is one of the central features of
        /// log4net.
        /// </para>
        /// </remarks>
        /// <param name="name">The name of the logger to retrieve.</param>
        /// <returns>The logger with the name specified.</returns>
        public ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }

        /// <summary>
        /// Shorthand for <see cref="LogFactory.GetLogger(string)"/>.
        /// </summary>
        /// <remarks>
        /// Get the logger for the fully qualified name of the type specified.
        /// </remarks>
        /// <param name="type">The full name of <paramref name="type"/> will be used as the name of the logger to retrieve.</param>
        /// <returns>The logger with the name specified.</returns>
        public ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        #endregion Methods
    }
}