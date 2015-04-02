#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProxyServerSetting.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description: 
 *
 *  Notes:
 *      
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.AuthorizedAgent.Common.Setting
{
    /// <summary>
    /// The proxy server setting.
    /// </summary>
    [Serializable]
    public class ProxyServerSetting
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is using proxy.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is using proxy; otherwise, <c>false</c>.
        /// </value>
        public bool IsUsingProxy { get; set; }

        /// <summary>
        /// Gets or sets the proxy's IP.
        /// </summary>
        /// <value>
        /// The server IP.
        /// </value>
        public string ServerIP { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public string Port { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is by pass local address.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is by pass local addr; otherwise, <c>false</c>.
        /// </value>
        public bool IsByPassLocalAddr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is need authorized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is need authorized; otherwise, <c>false</c>.
        /// </value>
        public bool IsNeedAuthorized { get; set; }

        /// <summary>
        /// Gets or sets the proxy's authorized user name.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the proxy's authorized password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the proxy's domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public string Domain { get; set; }
    }
}
