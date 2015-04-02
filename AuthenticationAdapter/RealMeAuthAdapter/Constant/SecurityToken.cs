#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: SecurityToken.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The security token representation.
*
* </pre>
*/

#endregion

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// Security token returned by RealMe
    /// </summary>
    public sealed class SecurityToken
    {
        /// <summary>
        /// Gets or sets the SAML artifact.
        /// </summary>
        public string SAMLArt { get; set; }

        /// <summary>
        /// Gets or sets the Relay State key.
        /// </summary>
        public string RelayState { get; set; }

        /// <summary>
        /// Gets or sets the signature algorithm.
        /// </summary>
        public string SigAlg { get; set; }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Gets a value indicating whether token has valid value.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SAMLArt)
                    && !string.IsNullOrWhiteSpace(RelayState)
                    && !string.IsNullOrWhiteSpace(SigAlg)
                    && !string.IsNullOrWhiteSpace(Signature);
            }
        }
    }
}
