#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AppSession.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  All of APO session objects should be definted in this class.
 *  Notes:
 *      $Id: ApoQueryInfo.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provide a class to manage APO session objects.
    /// </summary>
    [Serializable]
    public class ApoQueryInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the address filter condition.
        /// </summary>
        public Hashtable APOAddressFilter
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the parcel filter condition.
        /// </summary>
        public Hashtable APOParcelFilter
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the owner filter condition.
        /// </summary>
        public Hashtable APOOwnerFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the address page index.
        /// </summary>
        public int APOAddressPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parcel page index.
        /// </summary>
        public int APOParcelPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owner page index.
        /// </summary>
        public int APOOwnerPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the address page sort.
        /// </summary>
        public string APOAddressPageSort
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the parcel page sort.
        /// </summary>
        public string APOParcelPageSort
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the owner page sort.
        /// </summary>
        public string APOOwnerPageSort
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the address data source.
        /// </summary>
        public DataTable APOAddressSource
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the parcel data source.
        /// </summary>
        public DataTable APOParcelSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owner data source.
        /// </summary>
        public DataTable APOOwnerSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether it from search page.
        /// </summary>
        public bool IsFromSearchPage
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the permit filter condition.
        /// </summary>
        public Hashtable APOPermitFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the permit page index.
        /// </summary>
        public int APOPermitPageIndex 
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the permit page sort.
        /// </summary>
        public string APOPermitPageSort 
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the permit data source.
        /// </summary>
        public DataTable APOPermitSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the search type index.
        /// </summary>
        public int APOSearchTypeIndex
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the licensee filter condition.
        /// </summary>
        public LicenseProfessionalModel APOLicenseeFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the licensee page index.
        /// </summary>
        public int APOLicenseePageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the licensee page sort.
        /// </summary>
        public string APOLicenseePageSort
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the licensee data source.
        /// </summary>
        public IList<LicenseProfessionalModel> APOLicenseeSource
        {
            get;
            set;
        }

        #endregion Properties
    }
}