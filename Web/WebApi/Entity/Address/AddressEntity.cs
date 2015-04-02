#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: AddressEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Address Entity
    /// </summary>
    public class AddressEntity
    {
        /// <summary>
        /// string address
        /// </summary>
        private string _address;

        /// <summary>
        /// string neighbor hood
        /// </summary>
        private string _neighborhood;

        /// <summary>
        /// string city
        /// </summary>
        private string _city;

        /// <summary>
        /// string street Name
        /// </summary>
        private string _streetName;

        /// <summary>
        /// string street postal
        /// </summary>
        private string _postal;

        /// <summary>
        /// string street Number
        /// </summary>
        private string _streetNumber;

        /// <summary>
        /// string country Code
        /// </summary>
        private string _countryCode;

        /// <summary>
        /// string street Suffix
        /// </summary>
        private string _streetSuffix;

        /// <summary>
        /// string direction
        /// </summary>
        private string _direction;

        /// <summary>
        /// Gets or sets Address
        /// </summary>
        [JsonProperty("address")]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Gets or sets Neighborhood
        /// </summary>
        [JsonProperty("neighborhood")]
        public string Neighborhood
        {
            get { return _neighborhood; }
            set { _neighborhood = value; }
        }

        /// <summary>
        /// Gets or sets City
        /// </summary>
        [JsonProperty("city")]
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>
        /// Gets or sets Street Name
        /// </summary>
        [JsonProperty("streetName")]
        public string StreetName
        {
            get { return _streetName; }
            set { _streetName = value; }
        }

        /// <summary>
        /// Gets or sets Postal
        /// </summary>
        [JsonProperty("postal")]
        public string Postal
        {
            get { return _postal; }
            set { _postal = value; }
        }

        /// <summary>
        /// Gets or sets Street Number
        /// </summary>
        [JsonProperty("streetNumber")]
        public string StreetNumber
        {
            get { return _streetNumber; }
            set { _streetNumber = value; }
        }

        /// <summary>
        /// Gets or sets Country Code
        /// </summary>
        [JsonProperty("countryCode")]
        public string CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        /// <summary>
        /// Gets or sets Street Suffix
        /// </summary>
        [JsonProperty("streetSuffix")]
        public string StreetSuffix
        {
            get { return _streetSuffix; }
            set { _streetSuffix = value; }
        }

        /// <summary>
        /// Gets or sets Direction
        /// </summary>
        [JsonProperty("direction")]
        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
    }
}