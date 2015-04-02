#region Header

/**
 *  Accela Citizen Access
 *  File: ResultMessage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011
 *
 *  Description:
 *   The result message that indicate the action result status.
 *
 *  Notes:
 * $Id: ResultMessage.cs 192687 2011-03-14 05:38:13Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

namespace Accela.ACA.CustomizeAPI
{
    /// <summary>
    /// The result message that indicate the action result status.
    /// </summary>
    public class ResultMessage
    {
        /// <summary>
        /// indicating whether it is success.
        /// </summary>
        private bool _isSuccess = true;

        /// <summary>
        /// Gets or sets a value indicating whether it is success.
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }

            set
            {
                _isSuccess = value;
            }
        }

        /// <summary>
        /// Gets or sets the result text message.
        /// </summary>
        /// <value>The result text message.</value>
        public string Message
        {
            get;
            
            set;
        }
    }
}
