#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AutoFillParameter.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AutoFillParameter.cs 275846 2014-07-24 06:50:33Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// the auto fill parameter
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class AutoFillParameter
    {
        /// <summary>
        /// Gets or sets the type of the auto fill.
        /// Contact, License, Address, Parcel, Owner
        /// </summary>
        /// <value>The type of the auto fill.</value>
        [System.Runtime.Serialization.DataMember]
        public ACAConstant.AutoFillType4SpearForm AutoFillType { get; set; }

        /// <summary>
        /// Gets or sets the section id.
        /// auto fill source section
        /// </summary>
        /// <value>The section id.</value>
        [System.Runtime.Serialization.DataMember]
        public string SectionId { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// Reference entity id
        /// </summary>
        /// <value>The entity id.</value>
        [System.Runtime.Serialization.DataMember]
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// Reference entity type
        /// </summary>
        /// <value>The type of the entity.</value>
        [System.Runtime.Serialization.DataMember]
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the entity ref id.
        /// Reference entity sequence number
        /// </summary>
        /// <value>The entity ref id.</value>
        [System.Runtime.Serialization.DataMember]
        public string EntityRefId { get; set; }
    }
}
