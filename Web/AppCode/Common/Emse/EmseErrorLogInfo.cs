#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EmseErrorLogInfo.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EmseErrorLogInfo.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// log the message info for log4net
    /// </summary>
    public class EmseErrorLogInfo
    {
        #region Fields

        /// <summary>
        /// id1 of CapModel.capID
        /// </summary>
        private string _capID1;

        /// <summary>
        /// id2 of CapModel.capID
        /// </summary>
        private string _capID2;

        /// <summary>
        /// id3 of CapModel.capID
        /// </summary>
        private string _capID3;

        /// <summary>
        /// if not null return "Not NULL" else return "Is Null"
        /// </summary>
        private string _capModelIsNull;

        /// <summary>
        /// "-1" means error,"0" means success
        /// </summary>
        private string _errorCode;

        /// <summary>
        /// the error message use written in EMSE Script
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// including <c>BeforeButton Event,AfterButton Event,OnloadEvent</c>
        /// </summary>
        private string _eventType;

        /// <summary>
        /// EMSE script code
        /// </summary>
        private string _scriptCode;

        /// <summary>
        /// system error message
        /// </summary>
        private string _systemErrorMessage;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EmseErrorLogInfo class.
        /// </summary>
        public EmseErrorLogInfo()
        {
            _scriptCode = string.Empty;
            _eventType = string.Empty;
            _capID1 = string.Empty;
            _capID2 = string.Empty;
            _capID3 = string.Empty;
            _capModelIsNull = string.Empty;
            _errorCode = string.Empty;
            _errorMessage = string.Empty;
            _systemErrorMessage = string.Empty;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets CapModel.capID.id1
        /// </summary>
        public string CapID1
        {
            get
            {
                return _capID1;
            }

            set
            {
                _capID1 = value;
            }
        }

        /// <summary>
        /// Gets or sets CapModel.capID.id2
        /// </summary>
        public string CapID2
        {
            get
            {
                return _capID2;
            }

            set
            {
                _capID2 = value;
            }
        }

        /// <summary>
        /// Gets or sets CapModel.capID.id3
        /// </summary>
        public string CapID3
        {
            get
            {
                return _capID3;
            }

            set
            {
                _capID3 = value;
            }
        }

        /// <summary>
        /// Gets or sets the capModel EMSE return,if not null return "Not NULL" else return "Is Null"
        /// </summary>
        public string CapModelIsNull
        {
            get
            {
                return _capModelIsNull;
            }

            set
            {
                _capModelIsNull = value;
            }
        }

        /// <summary>
        /// Gets or sets error code. "-1" means error,"0" means success
        /// </summary>
        public string ErrorCode
        {
            get
            {
                return _errorCode;
            }

            set
            {
                _errorCode = value;
            }
        }

        /// <summary>
        /// Gets or sets event type. <c>BeforeButton Event¡¢AfterButton Event¡¢OnloadEvent</c>
        /// </summary>
        public string EventType
        {
            get
            {
                return _eventType;
            }

            set
            {
                _eventType = value;
            }
        }

        /// <summary>
        /// Gets or sets EMSE script_code
        /// </summary>
        public string ScriptCode
        {
            get
            {
                return _scriptCode;
            }

            set
            {
                _scriptCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the EMSE Script Method  Exception Message
        /// </summary>
        public string SystemErrorMessage
        {
            get
            {
                return _systemErrorMessage;
            }

            set
            {
                _systemErrorMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the error message use written in EMSE Script 
        /// </summary>
        public string UserErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
            }
        }

        #endregion Properties
    }
}