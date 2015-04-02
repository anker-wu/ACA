#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EtisalatRegistrationInput.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EtisalatRegistrationInput.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  1-2-2009           Xinter Peng               Initial
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// ETISALAT input for registration
    /// </summary>
    public class EtisalatRegistrationInput
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether multiple cap or not.
        /// </summary>
        public bool IsMultiCAP
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether pay for existing cap.
        /// </summary>
        public bool IsPay4ExistingCap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets module name.
        /// </summary>
        public string ModuleName
        {
            get;
            set;
        }

        #endregion Properties
    }
}