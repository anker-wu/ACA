#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapTypeWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapTypeWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// This class provides one cap type wrapper model for custom component
    /// </summary>
    [System.SerializableAttribute]
    public class CapTypeWrapperModel
    {
        /// <summary>
        /// cap Type
        /// </summary>
        private CapTypeModel capType;

        /// <summary>
        /// Initializes a new instance of the CapTypeWrapperModel class
        /// </summary>
        /// <param name="capType">cap Type</param>
        internal CapTypeWrapperModel(CapTypeModel capType)
        {
            this.capType = capType;
        }

        /// <summary>
        /// Gets Agency Code
        /// </summary>
        public string AgencyCode
        {
            get { return this.capType.serviceProviderCode; }
        }

        /// <summary>
        /// Gets Module
        /// </summary>
        public string Module
        {
            get { return this.capType.moduleName; }
        }

        /// <summary>
        /// Gets Group
        /// </summary>
        public string Group
        {
            get { return this.capType.group; }
        }

        /// <summary>
        /// Gets Type
        /// </summary>
        public string Type
        {
            get { return this.capType.type; }
        }

        /// <summary>
        /// Gets Sub Type
        /// </summary>
        public string SubType
        {
            get { return this.capType.subType; }
        }

        /// <summary>
        /// Gets Category
        /// </summary>
        public string Category
        {
            get { return this.capType.category; }
        }

        /// <summary>
        /// Gets Alias
        /// </summary>
        public string Alias
        {
            get { return this.capType.alias; }
        }

        /// <summary>
        /// Gets Filter Name
        /// </summary>
        public string FilterName
        {
            get { return this.capType.filterName; }
        }
    }
}
