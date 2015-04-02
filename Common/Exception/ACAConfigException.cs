#region Header

/**
 *  Accela Citizen Access
 *  File: ACAConfigException.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   It defines the config exception include Web.Config and Standard Choice to provide the user-friendly message to the end user.
 *
 *  Notes:
 *  $Id: ACAConfigException.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 *  08/04/2014             James Shi            Initial.
 */

#endregion Header

using System;
using Accela.ACA.Common.Util;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Provide a class to defined ACA config exception.
    /// </summary>
    [Serializable]
    public class ACAConfigException : ACAException
    {
        #region Fields

        /// <summary>
        /// parameter name, such as: serviceProviderCode,capID
        /// </summary>
        private string[] _configKeysFields; 

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ACAConfigException class.
        /// </summary>
        /// <param name="message">The message info.</param>
        /// <param name="configKeys">The config keys array</param>
        public ACAConfigException(string message, string[] configKeys) : base(message)
        {
            this._configKeysFields = configKeys;
        }

        /// <summary>
        /// Initializes a new instance of the ACAConfigException class.
        /// </summary>
        /// <param name="configKeys">the config keys array.</param>
        public ACAConfigException(string[] configKeys)
        {
            this._configKeysFields = configKeys;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets config keys.
        /// </summary>
        public string[] ConfigKeys
        {
            get
            {
                return _configKeysFields;
            }

            set
            {
                _configKeysFields = value;
            }
        }

        /// <summary>
        /// Gets format message keys, such as [serviceProviderCode,capID]
        /// </summary>
        public string FormatMessageKeys
        {
            get
            {
                return DataUtil.FormatArray(_configKeysFields);
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

        #endregion Properties
    }
}