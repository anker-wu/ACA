#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressLocationEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: AddressLocationEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Address Location Entity
    /// </summary>
    public class AddressLocationEntity
    {
        /// <summary>
        /// Address INFO ENTITY
        /// </summary>
        private IList<AddressInfoEntity> _addresses;

        /// <summary>
        /// Gets or sets addresses
        /// </summary>
        [JsonProperty("addresses")]
        public IList<AddressInfoEntity> Addresses
        {
            get { return _addresses; }
            set { _addresses = value; }
        }
    }
}