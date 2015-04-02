#region Header

/**
 *  Accela Citizen Access
 *  File: DataValidateException.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   It defines the data validation exception to provide the user-friendly message to the end user.
 *
 *  Notes:
 *  $Id: DataValidateException.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 *  05/27/2007             Troy Yang            Initial.
 */

#endregion Header

using System;

using Accela.ACA.Common.Util;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Provide a class to defined data exception.
    /// </summary>
    [Serializable]
    public class DataValidateException : ACAException
    {
        #region Fields

        /// <summary>
        /// parameter name, such as: serviceProviderCode,capID
        /// </summary>
        private string[] messageKeys_field;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DataValidateException class.
        /// </summary>
        /// <param name="originalException">original exception</param>
        public DataValidateException(Exception originalException) : base(originalException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DataValidateException class.
        /// </summary>
        /// <param name="message">the message info</param>
        /// <param name="messageKeys">the message keys.</param>
        public DataValidateException(string message, string[] messageKeys) : base(message)
        {
            this.messageKeys_field = messageKeys;
        }

        /// <summary>
        /// Initializes a new instance of the DataValidateException class.
        /// </summary>
        /// <param name="messageKeys">the message keys.</param>
        public DataValidateException(string[] messageKeys)
        {
            this.messageKeys_field = messageKeys;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets format message keys, such as [serviceProviderCode,capID]
        /// </summary>
        public string FormatMessageKeys
        {
            get
            {
                return DataUtil.FormatArray(this.messageKeys_field);
            }
        }

        /// <summary>
        /// Gets Override message property to return user-friendly message.
        /// </summary>
        public override string Message
        {
            get
            {
                string customeMessage = I18nStringUtil.FormatToTableRow(string.Empty, FormatMessageKeys);
                return ExceptionUtil.GetErrorMessage(InnerException, customeMessage);
            }
        }

        /// <summary>
        /// Gets or sets message keys.
        /// </summary>
        public string[] MessageKeys
        {
            get
            {
                return this.messageKeys_field;
            }

            set
            {
                this.messageKeys_field = value;
            }
        }

        #endregion Properties
    }
}