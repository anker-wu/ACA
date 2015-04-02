#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CountryAutoFillPhoneObject.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: The regional util
 *
 *  Notes:
 *      $Id: CountryAutoFillPhoneObject.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Regional
{
    /// <summary>
    /// This class provide the country auto fill phone Object.
    /// This class temporary ignore the code standard that property names begin with an upper-case letter.
    /// Because it used in javaScript's json convert in page or component. 
    /// If the property names changed: 
    ///     1. the related javaScript need changed too. OR
    ///     2. use the [JsonProperty("")] in property and do sufficient UT.
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class CountryAutoFillPhoneObject
    {
        /// <summary>
        /// Gets or sets the phone control name.
        /// </summary>
        /// <value>The name.</value>
        [System.Runtime.Serialization.DataMember]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the phone value.
        /// </summary>
        /// <value>The value.</value>
        [System.Runtime.Serialization.DataMember]
        public string value { get; set; }

        /// <summary>
        /// Gets or sets the phone number IDD value.
        /// </summary>
        /// <value>The IDD value.</value>
        [System.Runtime.Serialization.DataMember]
        public string iddvalue { get; set; }
    }
}