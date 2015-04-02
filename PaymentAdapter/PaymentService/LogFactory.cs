/**
 *  Accela Citizen Access
 *  File: LogFactory.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: LogFactory.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;
using log4net;

namespace Accela.ACA.PaymentAdapter
{
    /// <summary>
    /// the log faactory
    /// </summary>
    public sealed class LogFactory
    {
         /// <summary>
        /// Single instance.
        /// </summary>
        private static LogFactory singleInstance = new LogFactory();

        /// <summary>
        /// Prevents a default instance of the LogFactory class from being created.
        /// </summary>
        private LogFactory()
        {
        }

        /// <summary>
        ///  Gets Return LogHelper single instance. 
        /// </summary>
        public static LogFactory Instance
        {
            get
            {
                return singleInstance;
            }
        }

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
    }
}
