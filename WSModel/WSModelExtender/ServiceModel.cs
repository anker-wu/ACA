#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ServiceModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ServiceModel.cs 238264 2013-07-10 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

namespace Accela.ACA.WSProxy
{
    public partial class ServiceModel
    {
        #region Nested Class

        /// <summary>
        /// This class provider the ServiceModel comparer.
        /// </summary>
        public class Comparer : IEqualityComparer<ServiceModel>
        {
            /// <summary>
            /// Define the equal method for comparer.
            /// </summary>
            /// <param name="s1">The service model 1.</param>
            /// <param name="s2">The service model 2.</param>
            /// <returns>Return true if s1 equal s2.</returns>
            public bool Equals(ServiceModel s1, ServiceModel s2)
            {
                if (s1.servPorvCode == s2.servPorvCode && s1.serviceName == s2.serviceName)
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Get hash code.
            /// </summary>
            /// <param name="service">The service model.</param>
            /// <returns>The hash code value.</returns>
            public int GetHashCode(ServiceModel service)
            {
                return service.servPorvCode.GetHashCode() ^ service.serviceName.GetHashCode();
            }
        }

        #endregion Nested Class
    }
}