#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ASITDrillDown.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ASITDrillDownUtil.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// the description for ASITDrillDown
    /// </summary>
    [Serializable]
    public class ASITDrillDownUtil
    {
        #region Fields

        /// <summary>
        /// the asit drill down table column count
        /// </summary>
        private int _asitDrillDownTableColumnCount = 0;

        /// <summary>
        /// the current series id
        /// </summary>
        private int _currentSeriesID = 0;

        /// <summary>
        /// the current drill down list
        /// </summary>
        private DataTable _currentDrillDownList = new DataTable();

        /// <summary>
        /// the current finally drill down list
        /// </summary>
        private DataTable _currentFinallyDrillDownList;

        /// <summary>
        /// the drill down list
        /// </summary>
        private DataTable _drillDownList;

        /// <summary>
        /// collection columns of generate drill down data table.
        /// </summary>
        private Dictionary<string, ASITableDrillDSeriesModel4WS> _dynamicColumnCollection = new Dictionary<string, ASITableDrillDSeriesModel4WS>();

        /// <summary>
        /// fixed columns that they are ID,SeriesID,SeriesParentID,ColumnIDPath
        /// </summary>
        private int _fixedColumnCount = 5;

        /// <summary>
        /// is single section or not
        /// </summary>
        private bool _isSingleSection = false;

        /// <summary>
        /// is single section on last step
        /// </summary>
        private bool _isSingleSectionOnLastStep = false;

        /// <summary>
        /// the previous drill down list
        /// </summary>
        private Dictionary<int, DataTable> _previousDrillDownList = new Dictionary<int, DataTable>();

        /// <summary>
        /// the selected column name
        /// </summary>
        private Dictionary<string, string> _selectedColumnName = new Dictionary<string, string>();

        #endregion Fields

        #region Enumerations

        /// <summary>
        /// select type
        /// </summary>
        private enum SelectType
        {
            /// <summary>
            /// single type
            /// </summary>
            S,

            /// <summary>
            /// multiple type
            /// </summary>
            M
        }

        #endregion Enumerations

        #region Properties

        /// <summary>
        /// Gets or sets the asit drill down table column count.
        /// </summary>
        public int ASITDrillDownTableColumnCount
        {
            get
            {
                return _asitDrillDownTableColumnCount;
            }

            set
            {
                _asitDrillDownTableColumnCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the current finally drill down list.
        /// </summary>
        public DataTable CurrentFinallyDrillDownList
        {
            get
            {
                if (_currentFinallyDrillDownList == null)
                {
                    _currentFinallyDrillDownList = new DataTable();
                }

                return _currentFinallyDrillDownList;
            }

            set
            {
                _currentFinallyDrillDownList = value;
            }
        }

        /// <summary>
        /// Gets or sets the current series id.
        /// </summary>
        public int CurrentSeriesID
        {
            get
            {
                return _currentSeriesID;
            }

            set
            {
                _currentSeriesID = value;
            }
        }

        /// <summary>
        /// Gets or sets the dynamic column collection
        /// </summary>
        public Dictionary<string, ASITableDrillDSeriesModel4WS> DynamicColumnCollection
        {
            get
            {
                return _dynamicColumnCollection;
            }

            set
            {
                _dynamicColumnCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the series is single select only or not
        /// </summary>
        public bool IsSingleSection
        {
            get
            {
                return _isSingleSection;
            }

            set
            {
                _isSingleSection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the series is is single select only on last step or not
        /// </summary>
        public bool IsSingleSectionOnLastStep
        {
            get
            {
                return _isSingleSectionOnLastStep;
            }

            set
            {
                _isSingleSectionOnLastStep = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected column name
        /// </summary>
        public Dictionary<string, string> SelectedColumnName
        {
            get
            {
                return _selectedColumnName;
            }

            set
            {
                if (value != null && value.Count > 0)
                {
                    string[] columnKeys = new string[value.Keys.Count];
                    value.Keys.CopyTo(columnKeys, 0);
                    var _selectedColumnNames = new Dictionary<string, string>();

                    foreach (string columnKey in columnKeys)
                    {
                        string[] splitColumnKey = columnKey.Split(ACAConstant.SPLIT_CHAR);
                        string keyNoPageIndex = splitColumnKey[0] + ACAConstant.SPLIT_CHAR + splitColumnKey[2];

                        if (!_selectedColumnNames.ContainsKey(keyNoPageIndex))
                        {
                            _selectedColumnNames.Add(keyNoPageIndex, value[columnKey]);
                        }
                    }

                    _selectedColumnName = _selectedColumnNames;
                }
                else
                {
                    _selectedColumnName = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the asit field I18n name and I18n label
        /// </summary>
        public Dictionary<string, string> AsitFieldsLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the drill down list
        /// </summary>
        private DataTable DrillDownList
        {
            get
            {
                return _drillDownList;
            }

            set
            {
                if (_drillDownList == null)
                {
                    _drillDownList = value.Clone();
                }

                List<string> columnPathList = new List<string>();

                for (int i = 0; i < _drillDownList.Rows.Count; i++)
                {
                    columnPathList.Add(_drillDownList.Rows[i]["ColumnIDPath"].ToString());
                }

                foreach (DataRow dr in value.Rows)
                {
                    //DataRow[] tempDr = _drillDownList.Select("ColumnIDPath ='" + dr["ColumnIDPath"] + "'");
                    if (!columnPathList.Contains(dr["ColumnIDPath"].ToString()))
                    {
                        _drillDownList.ImportRow(dr);
                    }
                }

                _drillDownList.AcceptChanges();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get root data list of ASIT drill-down that it define in V360 system.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="parentGroupName">Parent group name of current ASI Table</param>
        /// <param name="subGroupName">Sub-group name of current ASI Table</param>
        /// <returns>return root data list of ASIT on data table format</returns>
        public DataTable GetFirstDrillDownDataList(string agencyCode, string parentGroupName, string subGroupName)
        {
            IASITableDrillDownBLL asitDrill = ObjectFactory.GetObject(typeof(IASITableDrillDownBLL)) as IASITableDrillDownBLL;
            ASITableDrillDSeriesModel4WS[] aSWS = asitDrill.GetFirstASITableDrillDownDatas(agencyCode, parentGroupName, subGroupName);

            if (aSWS == null)
            {
                return new DataTable();
            }

            if (aSWS.Length > 0)
            {
                _isSingleSectionOnLastStep = aSWS[0].selectTypeList[aSWS[0].selectTypeList.Length - 1] == SelectType.S.ToString() ? true : false;
                ASITDrillDownTableColumnCount = aSWS[0].selectTypeList.Length;
            }

            return GetDrillDownDataSource(aSWS, null);
        }

        /// <summary>
        /// Get next step data list for drill down.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="columnIDPathList">current level drill down column id paths.</param>
        /// <param name="seriesId">DrillDown series ID</param>
        /// <param name="previouseLevel">Previous level</param>
        /// <returns>the next drill down data list</returns>
        public DataTable GetNextDrillDownDataList(string agencyCode, ArrayList columnIDPathList, long seriesId, int previouseLevel)
        {
            if (this._previousDrillDownList.ContainsKey(previouseLevel))
            {
                this._previousDrillDownList.Remove(previouseLevel);
            }

            this._previousDrillDownList.Add(previouseLevel, this._currentDrillDownList);

            IASITableDrillDownBLL asitDrill = ObjectFactory.GetObject(typeof(IASITableDrillDownBLL)) as IASITableDrillDownBLL;
            ASITableDrillDSeriesModel4WS[] aSWS = asitDrill.GetNextASITableDrillDownDatas(agencyCode, (string[])columnIDPathList.ToArray(typeof(string)), seriesId);

            return GetDrillDownDataSource(aSWS, columnIDPathList);
        }

        /// <summary>
        /// This function is split page get data.
        /// </summary>
        /// <param name="pageNumber">page number</param>
        /// <returns>Return a little data list table </returns>
        public DataTable GetSeemlyRowsTable(int pageNumber)
        {
            int pageRows = 10;
            int defaultMaxRowCount = 100;
            pageNumber++;
            int actualRowsCount = pageNumber == 0 ? pageRows : pageRows * pageNumber;
            actualRowsCount += defaultMaxRowCount + pageRows;
            DataTable tempTable = CurrentFinallyDrillDownList.Clone();

            for (int i = 0; i < actualRowsCount; i++)
            {
                if (i < CurrentFinallyDrillDownList.Rows.Count)
                {
                    tempTable.ImportRow(CurrentFinallyDrillDownList.Rows[i]);
                }
            }

            return tempTable;
        }

        /// <summary>
        /// Check current sub-group for ASI table has drill down data.
        /// </summary>
        /// <returns>if is require then return true else return false</returns>
        public bool IsRequireDrillDownData()
        {
            if (this.DrillDownList.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Search content from drill down data table.If last column content of drill down data table has search content
        /// then will convert these content to data table and display it.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="groupName">ASI table group name</param>
        /// <param name="subGroupName">ASI table sub group name who is current ASI table name.</param>
        /// <param name="searchValue">search content</param>
        /// <returns>Drill down data table. It's last column content include search content.</returns>
        public DataTable SearchDrillDown(string agencyCode, string groupName, string subGroupName, string searchValue)
        {
            IASITableDrillDownBLL asitDrill = ObjectFactory.GetObject(typeof(IASITableDrillDownBLL)) as IASITableDrillDownBLL;

            //The searcRusult array data format as below chars
            /*
             *a[0][0]=column name, eg : ColumnName1
             *a[1][0]=languageID : columnValue, eg : 1:country
             */
            string[][] searchResult = asitDrill.LoadSearchResultBySearchValue(agencyCode, groupName, subGroupName, searchValue);
            DataTable drillDownSearchTable = new DataTable("DrillDataSearchResultList");

            if (searchResult == null || searchResult.Length < 1
                || searchResult[0] == null || searchResult[0].Length == 0)
            {
                return drillDownSearchTable;
            }

            var columns = new Dictionary<string, DataColumn>();
            DataTable tempTable = CreateDataTableStructrue();
            columns.Add(tempTable.Columns[0].ColumnName, CloneColumn(tempTable.Columns[0], string.Empty));
            columns.Add(tempTable.Columns[1].ColumnName, CloneColumn(tempTable.Columns[1], string.Empty));
            columns.Add(tempTable.Columns[2].ColumnName, CloneColumn(tempTable.Columns[2], string.Empty));
            columns.Add(tempTable.Columns[3].ColumnName, CloneColumn(tempTable.Columns[3], string.Empty));
            columns.Add(tempTable.Columns[4].ColumnName, CloneColumn(tempTable.Columns[4], string.Empty));

            //searchResult[0] is column name collection
            for (int i = 0; i < searchResult[0].Length; i++)
            {
                if (!columns.ContainsKey(searchResult[0][i]) &&
                    !string.IsNullOrEmpty(searchResult[0][i]))
                {
                    columns.Add(searchResult[0][i], CloneColumn(tempTable.Columns["ColumnName"], GetLabel(searchResult[0][i])));
                }
            }

            foreach (KeyValuePair<string, DataColumn> kvp in columns)
            {
                DataColumn column = new DataColumn();
                column = kvp.Value as DataColumn;
                drillDownSearchTable.Columns.Add(column);
            }

            for (int i = 1; i < searchResult.Length; i++)
            {
                DataRow dr = drillDownSearchTable.NewRow();
                dr[0] = i;
                dr[2] = -1;
                dr[4] = SplitIDValueFromArray(searchResult, i);

                for (int j = 0; j < searchResult[i].Length; j++)
                {
                    int columnIndex = j + 5;
                    dr[columnIndex] = searchResult[i][j].Split(':')[1].ToString();
                }

                drillDownSearchTable.Rows.Add(dr);
            }

            drillDownSearchTable.AcceptChanges();

            if (drillDownSearchTable.Rows.Count == 0)
            {
                DataRow dr = drillDownSearchTable.NewRow();
                drillDownSearchTable.Rows.Add(dr);
            }

            return drillDownSearchTable;
        }

        /// <summary>
        /// collection all ID value from searchResult connect these chars with underline
        /// </summary>
        /// <param name="searchResult">ID value and language collection</param>
        /// <param name="rowIndex">row index.</param>
        /// <returns>ID value String.Such as 1_2_3_4</returns>
        private static string SplitIDValueFromArray(string[][] searchResult, int rowIndex)
        {
            StringBuilder returnValue = new StringBuilder(searchResult[rowIndex][0].Split(':')[0]);

            //Row index zero of searhResult is saved column names.
            for (int i = 1; i < searchResult[rowIndex].Length; i++)
            {
                returnValue.Append("_");
                returnValue.Append(searchResult[rowIndex][i].Split(':')[0]);
            }

            return returnValue.ToString();
        }

        /// <summary>
        /// clone column
        /// </summary>
        /// <param name="tableDC">the data column</param>
        /// <param name="columnName">the column name</param>
        /// <returns>A clone column</returns>
        private DataColumn CloneColumn(DataColumn tableDC, string columnName)
        {
            DataColumn dc = new DataColumn();
            dc.DataType = tableDC.DataType;
            dc.ColumnName = string.IsNullOrEmpty(columnName) ? tableDC.ColumnName : columnName;
            dc.AutoIncrement = tableDC.AutoIncrement;
            dc.Caption = tableDC.Caption;
            dc.ReadOnly = tableDC.ReadOnly;
            dc.Unique = tableDC.Unique;
            dc.AutoIncrementSeed = tableDC.AutoIncrementSeed;
            dc.AutoIncrementStep = tableDC.AutoIncrementStep;
            return dc;
        }

        /// <summary>
        /// Generate table of structure for drill down data table.It include five columns.
        /// ID,SeriesID,SeriesParentID,Content,ColumnName they are a port of all columns
        /// </summary>
        /// <returns>drill down data list that it display on DataTable format</returns>
        private DataTable CreateDataTableStructrue()
        {
            DataTable table = new DataTable("DrillDataList");
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID";

            column.Caption = "ID";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ParentID";
            column.Caption = "ParentID";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "SeriesID";
            column.AutoIncrement = false;
            column.Caption = "SeriesID";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "SeriesParentID";
            column.AutoIncrement = false;
            column.Caption = "SeriesParentID";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ColumnIDPath";
            column.AutoIncrement = false;
            column.Caption = "ColumnIDPath";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Content";
            column.AutoIncrement = false;
            column.Caption = "Actions";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ColumnName";
            column.AutoIncrement = false;
            column.Caption = "ColumnName";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            return table;
        }

        /// <summary>
        /// Get finally data list for drill down .This data list is display UI data list.
        /// </summary>
        /// <returns> finally data list for drill down</returns>
        private DataTable GetFinallyDataList()
        {
            int columnIndex = _fixedColumnCount + DynamicColumnCollection.Count;
            DataTable drillDownTable = new DataTable("DrillDataListFinall");
            var columns = new Dictionary<string, DataColumn>();
            columns.Add(this.DrillDownList.Columns[0].ColumnName, CloneColumn(this.DrillDownList.Columns[0], string.Empty));
            columns.Add(this.DrillDownList.Columns[1].ColumnName, CloneColumn(this.DrillDownList.Columns[1], string.Empty));
            columns.Add(this.DrillDownList.Columns[2].ColumnName, CloneColumn(this.DrillDownList.Columns[2], string.Empty));
            columns.Add(this.DrillDownList.Columns[3].ColumnName, CloneColumn(this.DrillDownList.Columns[3], string.Empty));
            columns.Add(this.DrillDownList.Columns[4].ColumnName, CloneColumn(this.DrillDownList.Columns[4], string.Empty));

            foreach (KeyValuePair<string, ASITableDrillDSeriesModel4WS> kvp in DynamicColumnCollection)
            {
                if (!columns.ContainsKey(kvp.Value.parentColName) &&
                    !string.IsNullOrEmpty(kvp.Value.parentColName))
                {
                    columns.Add(kvp.Value.parentColName, CloneColumn(DrillDownList.Columns["ColumnName"], kvp.Value.parentColName));
                }
            }

            int addColmunCount = 0;

            foreach (KeyValuePair<string, DataColumn> kvp in columns)
            {
                if (addColmunCount >= columnIndex)
                {
                    break;
                }

                DataColumn column = new DataColumn();
                column = kvp.Value as DataColumn;
                drillDownTable.Columns.Add(column);
                addColmunCount++;
            }

            if (_currentDrillDownList.Rows.Count == 0)
            {
                return drillDownTable;
            }

            foreach (DataRow dr in this._currentDrillDownList.Rows)
            {
                DataRow drillDownTableRow = drillDownTable.NewRow();

                for (int i = 0; i < columnIndex; i++)
                {
                    if (i < _fixedColumnCount)
                    {
                        drillDownTableRow[i] = dr[i];
                    }
                    else if (drillDownTable.Columns[i].ColumnName ==
                             dr["ColumnName"].ToString())
                    {
                        drillDownTableRow[i] = dr["Content"];
                        IsSingleSection = DynamicColumnCollection[dr["SeriesID"].ToString()].selectType == SelectType.S.ToString() ? true : false;
                    }
                }

                drillDownTable.Rows.Add(drillDownTableRow);
            }

            drillDownTable.AcceptChanges();

            for (int i = 0; i < drillDownTable.Rows.Count; i++)
            {
                SetPreviousColumnValue(drillDownTable.Rows[i], drillDownTable.Columns);
            }

            return drillDownTable;
        }

        /// <summary>
        /// Get really column id path.this column id path is generate by column id such as id1_id2_id3.
        /// </summary>
        /// <param name="columnIDPathList">the column id path list</param>
        /// <param name="asw">the asi table drill series model</param>
        /// <returns>really column id path</returns>
        private string GetReallyColumnIDPath(ArrayList columnIDPathList, ASITableDrillDSeriesModel4WS asw)
        {
            string reallyIDPath = string.Empty;

            foreach (string var in columnIDPathList)
            {
                if (var == asw.uniqueLabel)
                {
                    reallyIDPath = var;
                    break;
                }
            }

            return reallyIDPath;
        }

        /// <summary>
        /// Set value into previous columns.
        /// </summary>
        /// <param name="dr">the data row</param>
        /// <param name="columns">the data column collection </param>
        private void SetPreviousColumnValue(DataRow dr, DataColumnCollection columns)
        {
            string[] columnIDPaths = dr["ColumnIDPath"].ToString().Split('_');

            for (int i = 5; i < columns.Count - 1; i++)
            {
                int level = i - 5;
                string keyValue = string.Empty;
                int templateIndex = 0;

                while (templateIndex <= level)
                {
                    keyValue += columnIDPaths[templateIndex] + "_";
                    templateIndex++;
                }

                keyValue = keyValue.TrimEnd('_');
                keyValue += ACAConstant.SPLIT_CHAR + level.ToString();

                if (SelectedColumnName.ContainsKey(keyValue))
                {
                    dr[i] = SelectedColumnName[keyValue];
                }
            }
        }

        /// <summary>
        /// Gets the field label
        /// </summary>
        /// <param name="colName">this field column name</param>
        /// <returns>The column label</returns>
        private string GetLabel(string colName)
        {
            string label = colName;

            if (AsitFieldsLabel != null && AsitFieldsLabel.ContainsKey(colName))
            {
                label = AsitFieldsLabel[colName];
            }

            return label;
        }

        /// <summary>
        /// Get the Drill down Data source
        /// </summary>
        /// <param name="asiTableDrillDSeriesModels">the asi table drill series models</param>
        /// <param name="columnIDPathList">the column id path list</param>
        /// <returns>the drill down data source</returns>
        private DataTable GetDrillDownDataSource(ASITableDrillDSeriesModel4WS[] asiTableDrillDSeriesModels, ArrayList columnIDPathList)
        {
            DataTable dt = CreateDataTableStructrue();

            if (asiTableDrillDSeriesModels != null)
            {
                foreach (ASITableDrillDSeriesModel4WS asw in asiTableDrillDSeriesModels)
                {
                    asw.parentColName = GetLabel(asw.parentColName);

                    foreach (ASITableDrillDValMapModel4WS item in asw.asiTableDrillDValMapModel4WSList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = item.childValueId;
                        dr["ParentID"] = item.parentValueId;
                        dr["ColumnName"] = asw.parentColName;
                        dr["SeriesParentID"] = asw.parentSeriesId;
                        dr["SeriesID"] = asw.seriesId;

                        string reallyIDPath = string.Empty;

                        if (columnIDPathList != null)
                        {
                            reallyIDPath = GetReallyColumnIDPath(columnIDPathList, asw) + "_";
                        }

                        dr["ColumnIDPath"] = reallyIDPath + dr["ID"];
                        dr["Content"] = I18nStringUtil.GetString(item.resChildValueName, item.childValueName);
                        dt.Rows.Add(dr);
                    }

                    if (!DynamicColumnCollection.ContainsKey(asw.seriesId))
                    {
                        DynamicColumnCollection.Add(asw.seriesId, asw);
                    }
                }
            }

            dt.AcceptChanges();
            DrillDownList = dt;
            _currentDrillDownList = dt;
            CurrentSeriesID = dt.Rows.Count > 0 ? int.Parse(dt.Rows[0]["SeriesID"].ToString()) : 0;
            CurrentFinallyDrillDownList = GetFinallyDataList();
            return GetSeemlyRowsTable(0);
        }

        #endregion Methods
    }
}