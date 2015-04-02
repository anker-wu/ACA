/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PostParameterEntity.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:PostParameterEntity.cs 77905 2014-09-04 12:49:28Z ACHIEVO\shine.yan $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Entity for web API post parameters.
    /// </summary>
    [Serializable]
    public class PostParameterEntity
    {
        /// <summary>
        /// method AddToCollection in ActionButtonController
        /// </summary>
        public class AddToCollection
        {
            /// <summary>
            /// Gets or sets  Collection name
            /// </summary>
            public string CollcetionName { get; set; }

            /// <summary>
            /// Gets or sets  Collection description
            /// </summary>
            public string CollectionDescription { get; set; }

            /// <summary>
            /// Gets or sets Cap Id
            /// </summary>
            public string CapId { get; set; }

            /// <summary>
            /// Gets or sets Cap Class
            /// </summary>
            public string CapClass { get; set; }

            /// <summary>
            /// Gets or sets Collection Id
            /// </summary>
            public string CollectionId { get; set; }
        }
    }
}