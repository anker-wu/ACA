#region Header

/**
 *  Accela Citizen Access
 *  File: SecurityUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *   Provide security function.
 *
 *  Notes:
 * $Id: SecurityUtil.cs 175327 2013-06-10 13:31:000 ACHIEVO\paul.ou $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Text;
using System.Web.Security;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Security utilities.
    /// </summary>
    public static class SecurityUtil
    {
        /// <summary>
        /// Use Machine Key encode the input data.
        /// </summary>
        /// <param name="originalData">Original input data.</param>
        /// <returns>Encoded data using machine key.</returns>
        public static string MachineKeyEncode(string originalData)
        {
            if (string.IsNullOrEmpty(originalData))
            {
                return null;
            }

            byte[] binaryData = Encoding.Default.GetBytes(originalData);
            string encodedData = MachineKey.Encode(binaryData, MachineKeyProtection.All);

            return encodedData;
        }

        /// <summary>
        /// Decode the encoded string using Machine Key.
        /// </summary>
        /// <param name="encodedData">Encoded data using machine key.</param>
        /// <returns>Decoded data.</returns>
        public static string MachineKeyDecode(string encodedData)
        {
            if (string.IsNullOrEmpty(encodedData))
            {
                return null;
            }

            byte[] binaryData = MachineKey.Decode(encodedData, MachineKeyProtection.All);
            string decodedData = Encoding.Default.GetString(binaryData);

            return decodedData;
        }
    }
}
