#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EtisalatRegistrationOutput.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EtisalatRegistrationOutput.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  1-2-2009           Xinter Peng               Initial
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// ETISALAT output for registration
    /// </summary>
    public class EtisalatRegistrationOutput
    {
        #region Fields

        /// <summary>
        /// The ETISALAT status
        /// </summary>
        private EtisalatStatus _status = EtisalatStatus.Unknown;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        public string ReceiptNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the redirection url.
        /// </summary>
        public string RedirectionURL
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ETISALAT status.
        /// </summary>
        public EtisalatStatus Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        #endregion Properties
    }
}