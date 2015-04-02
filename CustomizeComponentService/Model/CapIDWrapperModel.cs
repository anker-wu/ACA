#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapIDWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapIDWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// This class provides one cap id wrapper model for custom component
    /// </summary>
    [System.SerializableAttribute]
    public class CapIDWrapperModel
    {
        /// <summary>
        /// cap id
        /// </summary>
        private CapIDModel capId;

        /// <summary>
        /// Initializes a new instance of the CapIDWrapperModel class
        /// </summary>
        /// <param name="capId">cap id</param>
        internal CapIDWrapperModel(CapIDModel capId)
        {
            this.capId = capId;
        }

        /// <summary>
        /// Gets Agency Code
        /// </summary>
        public string AgencyCode
        {
            get { return capId.serviceProviderCode; }
        }

        /// <summary>
        ///  Gets ID1
        /// </summary>
        public string ID1
        {
            get { return capId.ID1; }
        }

        /// <summary>
        ///  Gets ID2
        /// </summary>
        public string ID2
        {
            get { return capId.ID2; }
        }

        /// <summary>
        ///  Gets ID3
        /// </summary>
        public string ID3
        {
            get { return capId.ID3; }
        }

        /// <summary>
        /// Gets CustomID
        /// </summary>
        public string CustomID
        {
            get { return capId.customID; }
        }
    }
}
