#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RecordEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: RecordEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Record Entity
    /// </summary>
    public class RecordEntity
    {
        /// <summary>
        /// Cap Id Entity
        /// </summary>
        private CapIdEntity _capid;

        /// <summary>
        /// identifier Display
        /// </summary>
        private string _identifierDisplay;

        /// <summary>
        /// permit type
        /// </summary>
        private string _permitType;

        /// <summary>
        /// permit number
        /// </summary>
        private string _permitNumber;

        /// <summary>
        /// string address
        /// </summary>
        private string _address;

        /// <summary>
        /// string created
        /// </summary>
        private string _created;

        /// <summary>
        /// string EXP date
        /// </summary>
        private string _expDate;
        
        /// <summary>
        /// Gets or sets Cap ID
        /// </summary>
        [JsonProperty("capId")]
        public CapIdEntity CapId
        {
            get { return _capid; }
            set { _capid = value; }
        }

        /// <summary>
        /// Gets or sets identifier display
        /// </summary>
        [JsonProperty("identifierDisplay")]
        public string IdentifierDisplay
        {
            get { return _identifierDisplay; }
            set { _identifierDisplay = value; }
        }

        /// <summary>
        /// Gets or sets permit type
        /// </summary>
        [JsonProperty("permitType")]
        public string PermitType
        {
            get { return _permitType; }
            set { _permitType = value; }
        }

        /// <summary>
        /// Gets or sets permit number
        /// </summary>
        [JsonProperty("permitNumber")]
        public string PermitNumber
        {
            get { return _permitNumber; }
            set { _permitNumber = value; }
        }

        /// <summary>
        /// Gets or sets address
        /// </summary>
        [JsonProperty("address")]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Gets or sets created
        /// </summary>
        [JsonProperty("created")]
        public string Created
        {
            get { return _created; }
            set { _created = value; }
        }

        /// <summary>
        /// Gets or sets EXP date
        /// </summary>
        [JsonProperty("expDate")]
        public string ExpDate
        {
            get { return _expDate; }
            set { _expDate = value; }
        }
    }
}