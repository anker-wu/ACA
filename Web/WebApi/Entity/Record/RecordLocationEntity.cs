#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RecordLocateEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: RecordLocateEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
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
    /// Record Locate Entity
    /// </summary>
    public class RecordLocateEntity
    {
        /// <summary>
        /// string id
        /// </summary>
        private string _id;

        /// <summary>
        /// System Entity
        /// </summary>
        private SystemEntity _system;

        /// <summary>
        /// Record By GIS Object Entity List
        /// </summary>
        private IList<RecordByGisObjectEntity> _recordByGisObject;

        /// <summary>
        /// Address list
        /// </summary>
        private IList<RecordByAddressEntity> _recordByAddress;

        /// <summary>
        /// parcel entity list
        /// </summary>
        private IList<RecordByParcelEntity> _recordBypacel;

        /// <summary>
        /// parcel entity list
        /// </summary>
        private IList<CapParcelItemEntity> _capParcelItem;

        /// <summary>
        /// cap parcel item address entity list
        /// </summary>
        private IList<CapParcelItemAddressEntity> _capParcelItemaddress;

        /// <summary>
        /// Gets or sets record by GIS Object
        /// </summary>
        [JsonProperty("recordByGisObject")]
        public IList<RecordByGisObjectEntity> RecordByGisObject
        {
            get { return _recordByGisObject; }
            set { _recordByGisObject = value; }
        }

        /// <summary>
        /// Gets or sets address
        /// </summary>
        [JsonProperty("recordByAddress")]
        public IList<RecordByAddressEntity> RecordByAddress
        {
            get { return _recordByAddress; }
            set { _recordByAddress = value; }
        }

        /// <summary>
        /// Gets or sets parcel
        /// </summary>
        [JsonProperty("recordByParcel")]
        public IList<RecordByParcelEntity> RecordByParcel
        {
            get { return _recordBypacel; }
            set { _recordBypacel = value; }
        }

        /// <summary>
        /// Gets or sets cap parcel item
        /// </summary>
        [JsonProperty("capParcelItem")]
        public IList<CapParcelItemEntity> CapParcelItem
        {
            get { return _capParcelItem; }
            set { _capParcelItem = value; }
        }

        /// <summary>
        /// Gets or sets cap parcel item address
        /// </summary>
        [JsonProperty("capParcelItemaddress")]
        public IList<CapParcelItemAddressEntity> CapParcelItemaddress
        {
            get { return _capParcelItemaddress; }
            set { _capParcelItemaddress = value; }
        }

        /// <summary>
        /// Gets or sets id
        /// </summary>
        [JsonProperty("id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets system
        /// </summary>
        [JsonProperty("system")]
        public SystemEntity System
        {
            get { return _system; }
            set { _system = value; }
        }
    }
}