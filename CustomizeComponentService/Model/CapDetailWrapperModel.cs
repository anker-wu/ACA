#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetailWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapDetailWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// This class provides cap detail wrapper model for custom component
    /// </summary>
    [System.SerializableAttribute]
    public class CapDetailWrapperModel
    {
        /// <summary>
        /// cap detail
        /// </summary>
        private CapDetailModel capDetail = null;

        /// <summary>
        /// Initializes a new instance of the CapDetailWrapperModel class
        /// </summary>
        /// <param name="capDetail">Cap Detail </param>
        internal CapDetailWrapperModel(CapDetailModel capDetail)
        {
            this.capDetail = capDetail;
        }

        /// <summary>
        /// Gets a value indicating whether it is Null Object
        /// </summary>
        public bool IsNullObject
        {
            get
            {
                return this.capDetail == null;
            }
        }

        /// <summary>
        /// Gets Short Notes
        /// </summary>
        public string ShortNotes
        {
            get
            {
                return this.capDetail.shortNotes;
            }
        }
    }
}
