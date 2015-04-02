/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GridViewJson.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GUITextModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// The GridViewJson model.
    /// </summary>
    [DataContract]
    public partial class GridViewJson
    {
        /// <summary>
        /// Initializes a new instance of the GridViewJson class.
        /// </summary>
        public GridViewJson()
        {
            this.GridViewColumnVisibleList = new List<GridViewColumnVisible>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether is admin
        /// </summary>
        [DataMember]
        public bool IsAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default table width
        /// </summary>
        [DataMember]
        public int DefaultTableWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the columns
        /// </summary>
        [DataMember]
        public List<GridViewColumnVisible> GridViewColumnVisibleList
        {
            get;
            set;
        }
    }
}
