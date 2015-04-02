#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Log.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description: 
 *
 *  Notes:
 *      
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using log4net;

namespace Accela.AuthorizedAgent.Common.Common
{
    /// <summary>
    /// The Log type
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// The error
        /// </summary>
        Error,

        /// <summary>
        /// The info
        /// </summary>
        Info,

        /// <summary>
        /// The debug
        /// </summary>
        Debug,

        /// <summary>
        /// The fatal
        /// </summary>
        Fatal,

        /// <summary>
        /// The warn
        /// </summary>
        Warn
    }

    /// <summary>
    /// Log object
    /// </summary>
    public class Log
    {
        #region Fields

        /// <summary>
        /// Single instance.
        /// </summary>
        private static readonly Log _singleInstance = new Log();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Log"/> class from being created.
        /// </summary>
        private Log()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///  Gets Return LogFactory single instance. 
        /// </summary>
        public static Log Instance
        {
            get
            {
                return _singleInstance;
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Logs the config.
        /// </summary>
        public void LogConfig()
        {
            // Initialize log4net configuration.
            log4net.Config.XmlConfigurator.Configure();
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

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public void Write(object name, LogType type, object message)
        {
            ILog log;

            if (name is Type)
            {
                log = GetLogger(name as Type);
            }
            else
            {
                log = GetLogger(name.ToString());
            }

            switch (type)
            {
                case LogType.Error:
                    log.Error(message);
                    break;
                case LogType.Debug:
                    log.Debug(message);
                    break;
                case LogType.Info:
                    log.Info(message);
                    break;
                case LogType.Fatal:
                    log.Fatal(message);
                    break;
                case LogType.Warn:
                    log.Warn(message);
                    break;
                default:
                    log.Warn(message);
                    break;
            }
        }

        #endregion
    }
}
