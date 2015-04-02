#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CountryAutoFillJson.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: The regional util
 *
 *  Notes:
 *      $Id: CountryAutoFillJson.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Regional
{
    /// <summary>
    /// This class provide the country auto fill JSON.
    /// This class temporary ignore the code standard that property names begin with an upper-case letter.
    /// Because it used in javaScript's json convert in page or component. 
    /// If the property names changed: 
    ///     1. the related javaScript need changed too. OR
    ///     2. use the [JsonProperty("")] in property and do sufficient UT.
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class CountryAutoFillJson
    {
        /// <summary>
        /// Gets or sets the country client ID.
        /// </summary>
        /// <value>The country client ID.</value>
        [System.Runtime.Serialization.DataMember]
        public string countryClientID { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>The country code.</value>
        [System.Runtime.Serialization.DataMember]
        public string countryCode { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [System.Runtime.Serialization.DataMember]
        public string state { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        [System.Runtime.Serialization.DataMember]
        public string zip { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        [System.Runtime.Serialization.DataMember]
        public CountryAutoFillPhoneObject[] phone { get; set; }
    }
}