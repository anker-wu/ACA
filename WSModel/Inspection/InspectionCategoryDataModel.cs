#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionCategoryDataModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionCategoryDataModel.cs 184506 2010-11-12 09:03:21Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Inspection
{
    /// <summary>
    /// Inspection category data model
    /// </summary>
    [Serializable]
    public class InspectionCategoryDataModel
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID value.</value>
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder
        {
            get;
            set;
        }
    }
}
