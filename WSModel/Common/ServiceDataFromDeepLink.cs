/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MultipleServiceForDeepLink.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012
 * 
 *  Description:
 *  Define the Service information for multiple records creation from deep link.
 * 
 *  Notes:
 *  $Id: MultipleServiceForDeepLink.cs 2012-12-18 16:45:23Z ACHIEVO\Asen.wei $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Define the Multiple Service information for multiple records creation from deep link.
    /// </summary>
    [Serializable]
    public class ServiceDataFromDeepLink
    {
        /// <summary>
        /// Gets or sets the service information list.
        /// </summary>
        public List<ServiceItemFromDeepLink> ServiceList { get; set; }

        /// <summary>
        /// Gets or sets the master record type.
        /// </summary>
        public string MasterRecordType { get; set; }

        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        public string Module { get; set; }
    }

    /// <summary>
    /// Define the Service information for multiple records creation from deep link.
    /// </summary>
    [Serializable]
    public class ServiceItemFromDeepLink
    {
        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the agency code.
        /// </summary>
        public string Agency { get; set; }
    }
}