#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationQueryInfo.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *  Notes:
 *      $Id: EducationQueryInfo.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// permit search type
    /// </summary>
    public enum GeneralInformationSearchType
    {
        /// <summary>
        /// search type for provider.
        /// </summary>
        Search4Provider = 0,

        /// <summary>
        /// search type for education.
        /// </summary>
        Search4EduAndExam = 1,

        /// <summary>
        /// search type for licensee.
        /// </summary>
        Search4Licensee = 2,

        /// <summary>
        /// search type for food facility inspection
        /// </summary>
        Search4FoodFacilityInspection = 3,

        /// <summary>
        /// search type for certified business
        /// </summary>
        Search4CertifiedBusiness = 4,

        /// <summary>
        /// search type for certified business
        /// </summary>
        Search4Document = 5
    }

    /// <summary>
    /// the class for education query information.
    /// </summary>
    [Serializable]
    public class EducationQueryInfo
    {
        #region Fields
        /// <summary>
        /// a ProviderModel
        /// </summary>
        private object _searchModel;

        /// <summary>
        /// search results.
        /// </summary>
        private Hashtable _searchResultCollection;

        /// <summary>
        /// search type.
        /// </summary>
        private GeneralInformationSearchType _searchType;

        /// <summary>
        /// result page index.
        /// </summary>
        private Hashtable _resultPageIndex;

        /// <summary>
        /// result sort expression.
        /// </summary>
        private Hashtable _resultSortExpression;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the model include the user's query critical
        /// </summary>
        public object SearchModel
        {
            get
            {
                return _searchModel;
            }

            set
            {
                _searchModel = value;
            }
        }

        /// <summary>
        /// Gets or sets Search Type
        /// </summary>
        public GeneralInformationSearchType SearchType
        {
            get
            {
                return _searchType;
            }

            set
            {
                _searchType = value;
            }
        }

        /// <summary>
        /// Gets or sets search result for education or provider
        /// </summary>
        public Hashtable SearchResultCollection
        {
            get
            {
                return _searchResultCollection;
            }

            set
            {
                _searchResultCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets permit grid view current page index
        /// </summary>
        public Hashtable ResultPageIndex
        {
            get
            {
                return _resultPageIndex;
            }

            set
            {
                _resultPageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets result grid view sort expression
        /// </summary>
        public Hashtable ResultSortExpression
        {
            get
            {
                return _resultSortExpression;
            }

            set
            {
                _resultSortExpression = value;
            }
        }

        #endregion Properties
    }
}