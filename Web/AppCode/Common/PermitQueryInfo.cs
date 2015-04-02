#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PermitQueryInfo.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PermitQueryInfo.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;

/// <summary>
/// permit search type
/// </summary>
public enum PermitSearchType
{
    /// <summary>
    /// None used to hide the search section in Cap home page.
    /// </summary>
    None = -1,

    /// <summary>
    /// General search
    /// </summary>
    General = 0,

    /// <summary>
    /// search by address
    /// </summary>
    ByAddress = 1,

    /// <summary>
    /// search by license
    /// </summary>
    ByLicense = 2,

    /// <summary>
    /// search by permit
    /// </summary>
    ByPermit = 3,

    /// <summary>
    /// search by trade name
    /// </summary>
    ByTradeName = 4,

    /// <summary>
    /// search by contact
    /// </summary>
    ByContact = 5
}

/// <summary>
/// record the permit query result information in cap home page
/// </summary>
[Serializable]
public class PermitQueryInfo
{
    #region Fields

    /// <summary>
    /// Has Search Permit.
    /// </summary>
    private bool _hasSearchPermit;

    /// <summary>
    /// is search my permit.
    /// </summary>
    private bool _isSearchMyPermit;

    /// <summary>
    /// label for Permit
    /// </summary>
    private string _labelForPermit;

    /// <summary>
    /// the module name.
    /// </summary>
    private string _moduleName;

    /// <summary>
    /// The Permit List.
    /// </summary>
    private DataTable _permitList;

    /// <summary>
    /// The Permit Result Page index.
    /// </summary>
    private int _permitResultPageIndex;

    /// <summary>
    /// The Permit Result sort.
    /// </summary>
    private string _permitResultSortExpression;

    /// <summary>
    /// The Search Model.
    /// </summary>
    private object _searchModel;

    /// <summary>
    /// The Search Result.
    /// </summary>
    private DataTable _searchResult;

    /// <summary>
    /// Search Result Page Index.
    /// </summary>
    private int _searchResultPageIndex;

    /// <summary>
    /// search Result Sort Expression.
    /// </summary>
    private string _searchResultSortExpression;

    /// <summary>
    /// the search Type
    /// </summary>
    private PermitSearchType _searchType;

    /// <summary>
    /// is search cross module.
    /// </summary>
    private bool _isSearchCrossModule;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets the permit search list in general search
    /// </summary>
    public DataTable ComplexPermitList
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether has searched permit
    /// </summary>
    public bool HasSearchPermit
    {
        get
        {
            return _hasSearchPermit;
        }

        set
        {
            _hasSearchPermit = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether search my permit only
    /// </summary>
    public bool IsSearchMyPermit
    {
        get
        {
            return _isSearchMyPermit;
        }

        set
        {
            _isSearchMyPermit = value;
        }
    }

    /// <summary>
    /// Gets or sets the when in address or license search type,the label to show in the top of permit grid view
    /// </summary>
    public string LabelForPermit
    {
        get
        {
            return _labelForPermit;
        }

        set
        {
            _labelForPermit = value;
        }
    }

    /// <summary>
    /// Gets or sets the module name.
    /// </summary>
    public string ModuleName
    {
        get
        {
            return _moduleName;
        }

        set
        {
            _moduleName = value;
        }
    }

    /// <summary>
    /// Gets or sets the permit search list
    /// </summary>
    public DataTable PermitList
    {
        get
        {
            return _permitList;
        }

        set
        {
            _permitList = value;
        }
    }

    /// <summary>
    /// Gets or sets the permit grid view current page index
    /// </summary>
    public int PermitResultPageIndex
    {
        get
        {
            return _permitResultPageIndex;
        }

        set
        {
            _permitResultPageIndex = value;
        }
    }

    /// <summary>
    /// Gets or sets the permit grid view sort expression
    /// </summary>
    public string PermitResultSortExpression
    {
        get
        {
            return _permitResultSortExpression;
        }

        set
        {
            _permitResultSortExpression = value;
        }
    }

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
    /// Gets or sets the address or license search data table
    /// </summary>
    public DataTable SearchResult
    {
        get
        {
            return _searchResult;
        }

        set
        {
            _searchResult = value;
        }
    }

    /// <summary>
    /// Gets or sets the address or license grid view current page index
    /// </summary>
    public int SearchResultPageIndex
    {
        get
        {
            return _searchResultPageIndex;
        }

        set
        {
            _searchResultPageIndex = value;
        }
    }

    /// <summary>
    /// Gets or sets the address or license grid view sort expression
    /// </summary>
    public string SearchResultSortExpression
    {
        get
        {
            return _searchResultSortExpression;
        }

        set
        {
            _searchResultSortExpression = value;
        }
    }

    /// <summary>
    /// Gets or sets the search type
    /// </summary>
    public PermitSearchType SearchType
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
    /// Gets or sets a value indicating whether search cross module.
    /// </summary>
    public bool IsSearchCrossModule
    {
        get
        {
            return _isSearchCrossModule;
        }

        set
        {
            _isSearchCrossModule = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the record back from record detail is auto skip.
    /// </summary>
    public bool IsBackFromCapDetailByAutoSkip
    {
        get;

        set;
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// create instance
    /// </summary>
    /// <param name="searchType">the search type</param>
    /// <param name="isSearchMyPermit">is search my permit</param>
    /// <param name="moduleName">the module Name</param>
    /// <param name="searchModel">the search model</param>
    /// <param name="searchResult">the search result</param>
    /// <param name="isSearchCrossModule">is search cross module</param>
    /// <returns>PermitQueryInfo model</returns>
    public static PermitQueryInfo CreateInstance(PermitSearchType searchType, bool isSearchMyPermit, string moduleName, object searchModel, object searchResult, bool isSearchCrossModule)
    {
        PermitQueryInfo queryInfo = new PermitQueryInfo();
        queryInfo.ModuleName = moduleName;
        UpdateInstance(queryInfo, searchType, isSearchMyPermit, searchModel, searchResult, isSearchCrossModule);

        return queryInfo;
    }

    /// <summary>
    /// sort table data with sort expression
    /// </summary>
    /// <param name="dt">data table.</param>
    /// <param name="sort">the sort description</param>
    /// <returns>the data table.</returns>
    public static DataTable SortDatatable(DataTable dt, string sort)
    {
        if (string.IsNullOrEmpty(sort))
        {
            return dt;
        }

        //Sort expression format is: [Column Name] ASC/DESC.
        string[] sortArray = sort.Split(' ');

        if (sortArray.Length > 0 && dt.Columns.Contains(sortArray[0]))
        {
            DataView dataView = new DataView(dt);
            dataView.Sort = sort;
            return dataView.ToTable();
        }

        return dt;
    }

    /// <summary>
    /// update instance
    /// </summary>
    /// <param name="queryInfo">the query Info</param>
    /// <param name="searchType">the search Type</param>
    /// <param name="isSearchMyPermit">is search my permit</param>
    /// <param name="searchModel">the search model</param>
    /// <param name="searchResult">the search result</param>
    /// <param name="isSearchCrossModule">is Search Cross Module</param>
    public static void UpdateInstance(PermitQueryInfo queryInfo, PermitSearchType searchType, bool isSearchMyPermit, object searchModel, object searchResult, bool isSearchCrossModule)
    {
        queryInfo.SearchType = searchType;
        queryInfo.IsSearchMyPermit = isSearchMyPermit;
        queryInfo.SearchModel = searchModel;
        queryInfo.SearchResultPageIndex = 0;
        queryInfo.SearchResultSortExpression = null;
        queryInfo.PermitResultPageIndex = 0;
        queryInfo.PermitResultSortExpression = null;
        queryInfo.IsSearchCrossModule = isSearchCrossModule;

        if (searchType == PermitSearchType.ByPermit)
        {
            queryInfo.PermitList = (DataTable)searchResult;
        }
        else if (searchType == PermitSearchType.General)
        {
            queryInfo.ComplexPermitList = (DataTable)searchResult;
        }
        else
        {
            queryInfo.SearchResult = (DataTable)searchResult;
            queryInfo.PermitList = null;
        }

        queryInfo.HasSearchPermit = false;
        queryInfo.IsBackFromCapDetailByAutoSkip = false;
    }

    #endregion Methods
}