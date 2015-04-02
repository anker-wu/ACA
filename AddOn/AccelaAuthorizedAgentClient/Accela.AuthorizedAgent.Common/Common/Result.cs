#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Result.cs
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

namespace Accela.AuthorizedAgent.Common.Common
{
    /// <summary>
    /// Http result
    /// </summary>
    [Serializable]
    public class Result
    {
        #region Fields

        /// <summary>
        /// The DOING
        /// </summary>
        public static readonly int DOING = 1;

        /// <summary>
        /// The OK
        /// </summary>
        public static readonly int OK = 0;

        /// <summary>
        /// The FAIL
        /// </summary>
        public static readonly int FAIL = -1;

        /// <summary>
        /// The CONFLICT
        /// </summary>
        public static readonly int CONFLICT = 2;

        /// <summary>
        /// The N o_ CHANGE
        /// </summary>
        public static readonly int NO_CHANGE = 3;

        /// <summary>
        /// The sequence
        /// </summary>
        private int _sequence;

        /// <summary>
        /// The status
        /// </summary>
        private int _status = FAIL;

        /// <summary>
        /// The message
        /// </summary>
        private string _message;

        /// <summary>
        /// The response time
        /// </summary>
        private long _responseTime;

        #endregion

        /// <summary>
        /// Gets or sets unique ID to denote one request.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        /// <summary>
        /// Gets or sets Communication status.
        /// 0: finished, 1: in progress, -1: failed
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Gets or sets Communication message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// Gets or sets Response time, the unit is the second.
        /// </summary>
        /// <value>
        /// The response time.
        /// </value>
        public long ResponseTime
        {
            get { return _responseTime; }
            set { _responseTime = value; }
        }
    }
}
