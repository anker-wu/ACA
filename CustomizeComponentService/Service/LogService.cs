#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LogService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LogService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.CustomizeAPI;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// Log service
    /// </summary>
    public class LogService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the LogService class.
        /// </summary>
        /// <param name="context">User Context</param>
        public LogService(UserContext context) : base(context)
        {         
        }

        /// <summary>
        /// Print Debug Information
        /// </summary>
        /// <param name="tag">Tag string</param>
        /// <param name="message">Message info</param>
        public void Debug(string tag, object message)
        {
            Logger.DebugFormat("[{0}-PUBLICUSER{1}] {2} : {3}", AgencyCode, UserSeqNum, tag, message);
        }

        /// <summary>
        /// Print Error Information
        /// </summary>
        /// <param name="tag">Tag string</param>
        /// <param name="message">Message info</param>
        public void Error(string tag, object message)
        {
            Logger.ErrorFormat("[{0}-PUBLICUSER{1}] {2} : {3}", AgencyCode, UserSeqNum, tag, message);
        }

        /// <summary>
        /// Print Info Information
        /// </summary>
        /// <param name="tag">Tag string</param>
        /// <param name="message">Message info</param>
        public void Info(string tag, object message)
        {
            Logger.InfoFormat("[{0}-PUBLICUSER{1}] {2} : {3}", AgencyCode, UserSeqNum, tag, message);
        }

        /// <summary>
        /// Print Fatal Information
        /// </summary>
        /// <param name="tag">Tag string</param>
        /// <param name="message">Message info</param>
        public void Fatal(string tag, object message)
        {
            Logger.FatalFormat("[{0}-PUBLICUSER{1}] {2} : {3}", AgencyCode, UserSeqNum, tag, message);
        }
    }
}
