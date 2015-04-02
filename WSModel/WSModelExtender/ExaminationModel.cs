#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExaminationModel.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// The external for examination model.
    /// </summary>
    public partial class ExaminationModel
    {
        /// <summary>
        /// Gets or sets the row index
        /// </summary>
        public int RowIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [from cap associate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [from cap associate]; otherwise, <c>false</c>.
        /// </value>
        public bool FromCapAssociate { get; set; }
    }
}
