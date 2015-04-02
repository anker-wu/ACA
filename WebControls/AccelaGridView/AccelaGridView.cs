/**
 * <pre>
 *
 *  Accela
 *  File: AccelaGridView.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaGridView.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/10/2007    daly.zeng    Initial.
 * </pre>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.Web.Controls
{
    #region Delegates

    /// <summary>
    /// delegate for GridViewSortedEventHandler
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">The <see cref="AccelaGridViewSortEventArgs"/> instance containing the event data.</param>
    public delegate void GridViewSortedEventHandler(object sender, AccelaGridViewSortEventArgs e);

    /// <summary>
    /// delegate for GridViewDownloadEventHandler
    /// </summary>
    /// <param name="sender">object sender.</param>
    /// <param name="e">The <see cref="GridViewDownloadEventArgs"/> instance containing the event data.</param>
    public delegate void GridViewDownloadEventHandler(object sender, GridViewDownloadEventArgs e);

    #endregion Delegates

    /// <summary>
    /// provide a grid view to bind data
    /// </summary>
    public class AccelaGridView : GridView, IAccelaWithExtenderControl
    {
        #region Fields

        /// <summary>
        /// check box id
        /// </summary>
        public const string CHECKBOXID = "CheckBoxButton";

        /// <summary>
        /// radio button id
        /// </summary>
        public const string RADIOBUTTONID = "RadioButtonField";

        /// <summary>
        /// radio button group name
        /// </summary>
        public const string RADIOBUTTONGROUPNAME = "RadioButtonFieldGroupName";

        /// <summary>
        /// default width of grid view
        /// </summary>
        public const int DEFAULT_GRIDVIEW_WIDTH = 770;

        /// <summary>
        /// default width of grid view of the template column.
        /// </summary>
        public const int DEFAULT_GRIDVIEW_TEMPLATE_COLUMN_WIDTH = 100;

        /// <summary>
        /// default page size
        /// </summary>
        private const int DEFAULT_PAGE_SIZE = 10;

        /// <summary>
        /// checkbox column header id
        /// </summary>
        private const string CHECKBOX_COLUMN_HEADER_ID = "{0}_HeaderButton_{1}";

        /// <summary>
        /// We don't need a checkbox in the header now so make the header checkbox invisible
        /// </summary>
        private const string CHECKBOX_COLUMN_HEADER_TEMPLATE = "<input class='aca_gridview_checkbox' type='checkbox' hidefocus='true' id='{0}' name='{0}' {1} onclick='CheckAll(this,\"{2}\");{3};' title='{4}'>";

        /// <summary>
        /// default empty row CSS
        /// </summary>
        private const string DEFAULT_EMPTY_ROW_CSS = "ACA_SmLabel ACA_SmLabel_FontSize";

        /// <summary>
        /// default CSS for page style
        /// </summary>
        private const string DEFAULT_PAGE_STYLE_CSS = "ACA_Table_Pages ACA_Table_Pages_FontSize";

        /// <summary>
        /// default CSS for header style
        /// </summary>
        private const string DEFAULT_HEADER_STYLE_CSS = "ACA_TabRow_Header ACA_TabRow_Header_FontSize";

        /// <summary>
        /// default CSS for even row style
        /// </summary>
        private const string DEFAULT_ALTERNATING_ROW_STYLE_CSS = "ACA_TabRow_Even ACA_TabRow_Even_FontSize";

        /// <summary>
        /// default CSS for odd row style
        /// </summary>
        private const string DEFAULT_ROW_STYLE_CSS = "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize";

        /// <summary>
        /// Export file name extension
        /// </summary>
        private const string EXPORT_FILE_EXT = ".csv";

        /// <summary>
        /// Export temp directory
        /// </summary>
        private const string EXPORT_TEMP_DIRECTORY = "TempDirectory";

        /// <summary>
        /// Export file name
        /// </summary>
        private const string EXPORT_FILE_NAME = "ExportFileName";

        /// <summary>
        /// Hidden field ID (The field is used to store selected items).
        /// </summary>
        private const string SELECTED_ITMS_HIDDEN_FIELD_ID = "hfSaveSelectedItems";

        /// <summary>
        /// The short list for GridView
        /// </summary>
        private const string SHORT_LIST = "SHORT_LIST";

        /// <summary>
        /// The previous result list for GridView
        /// </summary>
        private const string PREVIOUS_LIST = "PREVIOUS_LIST";

        /// <summary>
        /// row index prefix 
        /// </summary>
        private const string ROW_INDEX_PREFIX = "CB_";

        /// <summary>
        /// Save selected items, it's used for sorting.
        /// </summary>
        private readonly Hashtable _htSaveSelectedItems = new Hashtable();

        /// <summary>
        /// AccelaLinkButton for add to cart.
        /// </summary>
        private AccelaLinkButton btnAppendToCart;

        /// <summary>
        /// AccelaLinkButton for clone record.
        /// </summary>
        private AccelaLinkButton btnCloneRecord;

        /// <summary>
        /// add collection link button 
        /// </summary>
        private AccelaLinkButton _addCollectionLinkButton4Show;

        /// <summary>
        /// add to shopping cart link button
        /// </summary>
        private AccelaLinkButton _addToCartLinkButton4Show;

        /// <summary>
        /// add clone record link button
        /// </summary>
        private AccelaLinkButton _addToCloneRecordLinkButton4Show;

        /// <summary>
        /// add the short list link button
        /// </summary>
        private AccelaLinkButton _addShortListLinkButton4Show;

        /// <summary>
        /// is auto generate column
        /// </summary>
        private bool _autoGenerateColumns = false;

        //private string _pageIndicator = " Showing {0}-{1} of {2} ";

        /// <summary>
        /// caption CSS
        /// </summary>
        private string _captionCssClass = "ACA_SmLabel ACA_SmLabel_FontSize";

        /// <summary>
        /// export link button
        /// </summary>
        private AccelaLinkButton _exportLinkButton;

        /// <summary>
        /// export link button for showing
        /// </summary>
        private AccelaLinkButton _exportLinkButton4Show;

        /// <summary>
        /// grid column visible string
        /// </summary>
        private string _gridColumnsVisible = string.Empty;

        /// <summary>
        /// grid view number
        /// </summary>
        private string _gridViewNumber;

        /// <summary>
        /// hidden field to store the selected items
        /// </summary>
        private HiddenField _hfSaveSelectedItems = null;

        /// <summary>
        /// is clear selected items
        /// </summary>
        private bool _isClearSelectedItems = false;

        /// <summary>
        /// is in spear form
        /// </summary>
        private bool _isInSPEARForm;

        /// <summary>
        /// is required
        /// </summary>
        private bool _isRequired;

        /// <summary>
        /// required error message
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// check required function
        /// </summary>
        private string _checkRequiredFunction = "CheckRequired4GridView";

        /// <summary>
        /// module name
        /// </summary>
        private string _moduleName = null;

        /// <summary>
        /// grid row index
        /// </summary>
        private int _rowIndex = 0;

        /// <summary>
        /// selected item value string
        /// </summary>
        private string _selectedItemsValue;

        /// <summary>
        /// is show caption
        /// </summary>
        private bool _showCaption;

        /// <summary>
        /// total columns
        /// </summary>
        private int _totalColumns;

        /// <summary>
        /// total row count
        /// </summary>
        private int _totalRowsCount;

        /// <summary>
        /// whether show horizontal scroll or not.
        /// </summary>
        private bool _showHorizontalScroll = true;

        /// <summary>
        /// data row order
        /// </summary>
        private List<int> _dataRowOrder = new List<int>();

        /// <summary>
        /// table's summary property
        /// </summary>
        private string _summaryKey = string.Empty;

        /// <summary>
        /// total number for list
        /// </summary>
        private string _countSummary = string.Empty;

        /// <summary>
        /// The extended properties.
        /// </summary>
        private Hashtable _extendedProperties;

        /// <summary>
        /// The exported content
        /// </summary>
        private string _exportedContent = string.Empty;

        #endregion Fields

        #region Events

        /// <summary>
        /// a event that will be fired when GridView has been sorted. notice that the GridView's Sorted or Sorting event is not validated.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// A event occurs when download in grid view.
        /// </summary>
        public event GridViewDownloadEventHandler GridViewDownload;

        #endregion Events

        #region Enumerations

        /// <summary>
        /// column type
        /// </summary>
        public enum ColumnType
        {
            /// <summary>
            /// check box.
            /// </summary>
            CheckBox,

            /// <summary>
            /// radio button
            /// </summary>
            RadioButton,
        }

        #endregion Enumerations

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether enable/disable MultiColumn Sorting.
        /// </summary>
        [Description("Whether Sorting On more than one column is enabled")]
        [Category("Behavior")]
        [DefaultValue("false")]
        public bool AllowMultiColumnSorting
        {
            get
            {
                object o = ViewState["EnableMultiColumnSorting"];
                bool isAllowMultiColumn = o != null ? (bool)o : false;
                return isAllowMultiColumn;
            }

            set
            {
                AllowSorting = true;
                ViewState["EnableMultiColumnSorting"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a checkbox column is generated automatically at runtime
        /// </summary>
        [Category("Behavior")]
        [Description("Whether a checkbox column is generated automatically at runtime")]
        [DefaultValue(false)]
        public bool AutoGenerateCheckBoxColumn
        {
            get
            {
                object chkObj = ViewState["AutoGenerateCheckBoxColumn"];

                return chkObj == null ? false : (bool)chkObj;
            }

            set
            {
                ViewState["AutoGenerateCheckBoxColumn"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto generate columns
        /// </summary>
        public override bool AutoGenerateColumns
        {
            get
            {
                return _autoGenerateColumns;
            }

            set
            {
                _autoGenerateColumns = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a checkbox column is generated automatically at runtime
        /// </summary>
        [Category("Behavior")]
        [Description("Whether a radiobutton column is generated automatically at runtime")]
        [DefaultValue(false)]
        public bool AutoGenerateRadioButtonColumn
        {
            get
            {
                object rdoObj = ViewState["AutoGenerateRadioButtonColumn"];

                return rdoObj != null && (bool)rdoObj;
            }

            set
            {
                ViewState["AutoGenerateRadioButtonColumn"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the caption of the GridView
        /// </summary>
        [Bindable(BindableSupport.Yes, BindingDirection.OneWay)]
        public override string Caption
        {
            get
            {
                return base.Caption;
            }

            set
            {
                base.Caption = value;
            }
        }

        /// <summary>
        /// Gets or sets the style of caption
        /// </summary>
        [Category("Behavior")]
        [Description("Get or set the style of caption")]
        [DefaultValue("table_text")]
        public string CaptionCssClass
        {
            get
            {
                return _captionCssClass;
            }

            set
            {
                _captionCssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets column index of the checkbox field
        /// </summary>
        [Category("Behavior")]
        [Description("Indicates the 0-based position of the checkbox column")]
        [DefaultValue(0)]
        public int CheckBoxColumnIndex
        {
            get
            {
                object chkColumnIndexObj = ViewState["CheckBoxColumnIndex"];

                //Contains checkbox colunm.
                if (AutoGenerateCheckBoxColumn)
                {
                    return chkColumnIndexObj == null ? 1 : (int)chkColumnIndexObj;
                }
                else
                {
                    return chkColumnIndexObj == null ? 0 : (int)chkColumnIndexObj;
                }
            }

            set
            {
                //Contains checkbox colunm.
                if (AutoGenerateCheckBoxColumn)
                {
                    ViewState["CheckBoxColumnIndex"] = value < 0 ? 1 : value;
                }
                else
                {
                    ViewState["CheckBoxColumnIndex"] = value < 0 ? 0 : value;
                }
            }
        }

        /// <summary>
        /// Gets or sets data source.Override the DataSource for adding GridRowID column.
        /// </summary>
        public override object DataSource
        {
            get
            {
                if (ViewState["DataSource"] != null)
                {
                    return ViewState["DataSource"];
                }
                else
                {
                    return base.DataSource;
                }
            }

            set
            {
                _rowIndex = 0;

                if (value is DataTable || value is DataView)
                {
                    DataTable dt = new DataTable();

                    if (value is DataView)
                    {
                        DataView dataView = value as DataView;
                        dt = dataView.ToTable();
                    }
                    else
                    {
                        dt = value as DataTable;
                    }

                    if (IsCheckBoxOrRadioButtonColumn)
                    {
                        AddGridRowIdColumn(dt);
                    }

                    base.DataSource = dt;
                }
                else if (value != null && value.GetType().FullName.StartsWith("System.Collections.Generic.List`1"))
                {
                    base.DataSource = value;
                    ViewState["DataSource"] = value;
                }
                else
                {
                    base.DataSource = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the grid view name which is used in exporting result file name
        /// </summary>
        public string ExportFileName
        {
            get
            {
                if (this.ViewState[EXPORT_FILE_NAME] == null)
                {
                    return string.Empty;
                }

                return (string)this.ViewState[EXPORT_FILE_NAME];
            }

            set
            {
                this.ViewState[EXPORT_FILE_NAME] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type for export
        /// the type is "LP", "CAP", "PARCEL"
        /// </summary>
        public string GlobalSearchType
        {
            get
            {
                if (this.ViewState["ExportType"] == null)
                {
                    return string.Empty;
                }

                return this.ViewState["ExportType"] as string;
            }

            set
            {
                this.ViewState["ExportType"] = value;
            }
        }

        /// <summary>
        /// Gets footer row
        /// </summary>
        public override GridViewRow FooterRow
        {
            get
            {
                GridViewRow f = base.FooterRow;

                if (f != null)
                {
                    return f;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets a string to store columns' visible property
        /// </summary>
        public string GridColumnsVisible
        {
            get
            {
                return _gridColumnsVisible;
            }

            set
            {
                _gridColumnsVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets a string to identify the GridView in whole aca system
        /// </summary>
        public string GridViewNumber
        {
            get
            {
                return _gridViewNumber;
            }

            set
            {
                _gridViewNumber = value;

                //Reset page size if the GViewID changed.
                PageSize = GetPageSize();
            }
        }

        /// <summary>
        /// Gets or sets a value to identify the real width of this control
        /// </summary>
        public int RealWidth
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets a value to identify the scroll width;
        /// </summary>
        public int ScrollWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value to store the sort direction
        /// </summary>
        public string GridViewSortDirection
        {
            get
            {
                object o = ViewState["SortDirection"];
                return o != null ? o.ToString().ToUpperInvariant() : "ASC";
            }

            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the sort expression
        /// </summary>
        public string GridViewSortExpression
        {
            get
            {
                object o = ViewState["SortExpression"];
                return o != null ? o.ToString() : string.Empty;
            }

            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        /// <summary>
        /// Gets header row
        /// </summary>
        public override GridViewRow HeaderRow
        {
            get
            {
                GridViewRow h = base.HeaderRow;

                if (h != null)
                {
                    return h;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the shopping cart link
        /// </summary>
        public bool ShowCartLink
        {
            get
            {
                if (this.ViewState["ShowCartLink"] == null)
                {
                    return false;
                }

                return (bool)this.ViewState["ShowCartLink"];
            }

            set
            {
                this.ViewState["ShowCartLink"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether clear selected items 
        /// </summary>
        public bool IsClearSelectedItems
        {
            get
            {
                return _isClearSelectedItems;
            }

            set
            {
                _isClearSelectedItems = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the grid view is used in SPEAR form
        /// </summary>
        [Category("Behavior")]
        [Description("If the grid view is used in SPEAR form")]
        [DefaultValue(false)]
        public bool IsInSPEARForm
        {
            get
            {
                return _isInSPEARForm;
            }

            set
            {
                _isInSPEARForm = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the grid view has one row data at least or not
        /// </summary>
        [Category("Behavior"),
         Description("If the grid view has one row data at least"),
         DefaultValue(false),]
        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; }
        }

        /// <summary>
        /// Gets or sets the  error message if the grid view has no any row data
        /// </summary>
        [Category("Behavior"),
         Description("show this error message if the grid view has no any row data"),
         DefaultValue(""),]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// Gets or sets module name
        /// </summary>
        public string ModuleName
        {
            get
            {
                if (string.IsNullOrEmpty(_moduleName))
                {
                    if (this.Page is IPage)
                    {
                        return (Page as IPage).GetModuleName();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return _moduleName;
                }
            }

            set
            {
                _moduleName = value;
            }
        }

        /// <summary>
        /// Gets or sets page index. Override it for reset row index.
        /// </summary>
        public override int PageIndex
        {
            get
            {
                return base.PageIndex;
            }

            set
            {
                base.PageIndex = value;
                _rowIndex = 0;
            }
        }

        /// <summary>
        /// Gets or sets the state of a GridView row 
        /// </summary>
        public DataControlRowState? RowState
        {
            get
            {
                return (DataControlRowState?)ViewState["RowState"];
            }

            set
            {
                ViewState["RowState"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show the add to collection link
        /// </summary>
        public bool ShowAdd2CollectionLink
        {
            get
            {
                if (this.ViewState["ShowAdd2CollectionLink"] == null)
                {
                    return false;
                }

                return (bool)this.ViewState["ShowAdd2CollectionLink"];
            }

            set
            {
                this.ViewState["ShowAdd2CollectionLink"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show the clone record link.
        /// </summary>
        public bool ShowCloneRecordLink
        {
            get
            {
                if (ViewState["ShowCloneRecordLink"] == null)
                {
                    return false;
                }

                return (bool)ViewState["ShowCloneRecordLink"] && FunctionTable.IsEnableCloneRecord();
            }

            set
            {
                ViewState["ShowCloneRecordLink"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show the short list link.
        /// </summary>
        public bool ShowShortListLink
        {
            get
            {
                if (ViewState["ShowShortListLink"] == null)
                {
                    return false;
                }

                return (bool)ViewState["ShowShortListLink"];
            }

            set
            {
                ViewState["ShowShortListLink"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show GridView caption or not
        /// </summary>
        [Category("Behavior")]
        [Description("Determine to show gridview caption or not")]
        [DefaultValue(true)]
        public bool ShowCaption
        {
            get
            {
                return _showCaption;
            }

            set
            {
                _showCaption = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show the export link or not
        /// </summary>
        public bool ShowExportLink
        {
            get
            {
                if (this.ViewState["ShowExportLink"] == null)
                {
                    return false;
                }

                return (bool)this.ViewState["ShowExportLink"];
            }

            set
            {
                this.ViewState["ShowExportLink"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the GridView shows the footer when there is no data to bound
        /// </summary>
        [Category("Behavior")]
        [Themeable(true)]
        [Bindable(BindableSupport.No)]
        public bool ShowFooterWhenEmpty
        {
            get
            {
                if (this.ViewState["ShowFooterWhenEmpty"] == null)
                {
                    this.ViewState["ShowFooterWhenEmpty"] = false;
                }

                return (bool)this.ViewState["ShowFooterWhenEmpty"];
            }

            set
            {
                this.ViewState["ShowFooterWhenEmpty"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the GridView shows the header when there is no data to bound
        /// </summary>
        [Category("Behavior")]
        [Themeable(true)]
        [Bindable(BindableSupport.No)]
        public override bool ShowHeaderWhenEmpty
        {
            get
            {
                if (this.ViewState["ShowHeaderWhenEmpty"] == null)
                {
                    this.ViewState["ShowHeaderWhenEmpty"] = true;
                }

                return (bool)this.ViewState["ShowHeaderWhenEmpty"];
            }

            set
            {
                this.ViewState["ShowHeaderWhenEmpty"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the GridView control when the DataSource is null
        /// </summary>
        [Category("Behaviour")]
        [Themeable(true)]
        [Bindable(BindableSupport.No)]
        [DefaultValue(false)]
        public bool ShowWhenEmpty
        {
            get
            {
                if (ViewState["ShowWhenEmpty"] == null)
                {
                    return false;
                }

                return (bool)ViewState["ShowWhenEmpty"];
            }

            set
            {
                ViewState["ShowWhenEmpty"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Image location to be used to display Ascending Sort order.
        /// </summary>
        [Description("Image to display for Ascending Sort")]
        [Category("Misc")]
        [Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        public string SortAscImageUrl
        {
            get
            {
                object o = ViewState["SortImageAsc"];
                string sortImageUrl = o != null ? o.ToString() : string.Empty;
                return sortImageUrl;
            }

            set
            {
                ViewState["SortImageAsc"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Image location to be used to display Ascending Sort order.
        /// </summary>
        [Description("Image to display for Descending Sort")]
        [Category("Misc")]
        [Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        public string SortDescImageUrl
        {
            get
            {
                object o = ViewState["SortImageDesc"];
                string descImageUrl = o != null ? o.ToString() : string.Empty;
                return descImageUrl;
            }

            set
            {
                ViewState["SortImageDesc"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the customized total count
        /// </summary>
        public int CustomizedTotalCount
        {
            get
            {
                object totalCount = ViewState["TotalCount"];

                return totalCount == null ? 0 : (int)totalCount;
            }

            set
            {
                ViewState["TotalCount"] = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the loading panel
        /// </summary>
        [Category("Behaviour")]
        [Themeable(true)]
        [Bindable(BindableSupport.No)]
        [DefaultValue(false)]
        public bool ShowLoadingPanel
        {
            get
            {
                if (ViewState["ShowLoadingPanel"] == null)
                {
                    return false;
                }

                return (bool)ViewState["ShowLoadingPanel"];
            }

            set
            {
                ViewState["ShowLoadingPanel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the grid view needs horizontal scroll
        /// </summary>
        public bool ShowHorizontalScroll
        {
            get
            {
                return _showHorizontalScroll;
            }

            set
            {
                _showHorizontalScroll = value;
            }
        }

        /// <summary>
        /// Gets or sets SimpleViewElements
        /// </summary>
        public ColumnProperty[] ColumnProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ReArrange Columns index.
        /// </summary>
        public List<int> OrderColumns
        {
            get
            {
                List<int> list = (List<int>)ViewState["OrderColumns"];

                if (list == null)
                {
                    list = new List<int>();
                }

                return list;
            }

            set
            {
                ViewState["OrderColumns"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets table's summary label key.
        /// </summary>
        public string SummaryKey
        {
            get
            {
                return _summaryKey;
            }

            set
            {
                _summaryKey = value;
            }
        }

        /// <summary>
        /// Gets or sets table's caption label key.
        /// </summary>
        public string CaptionKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets total count of current list.
        /// </summary>
        public string CountSummary
        {
            get
            {
                if (!AllowPaging || _totalRowsCount.Equals(0))
                {
                    _countSummary = Convert.ToString(_totalRowsCount);
                }

                return _countSummary;
            }

            set
            {
                _countSummary = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is auto width(100%).
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in dialog; otherwise, <c>false</c>.
        /// </value>
        public bool IsAutoWidth { get; set; }

        /// <summary>
        /// Gets the extended properties.
        /// </summary>
        /// <value>The extended properties.</value>
        public Hashtable ExtendedProperties
        {
            get
            {
                if (_extendedProperties == null)
                {
                    _extendedProperties = new Hashtable();
                }

                return _extendedProperties;
            }
        }

        /// <summary>
        /// Gets or sets the exported content
        /// </summary>
        /// <returns>string for export content</returns>
        public string ExportedContent
        {
            get
            {
                if (!string.IsNullOrEmpty(_exportedContent))
                {
                    return _exportedContent;
                }

                StringBuilder sb = new StringBuilder();
                ArrayList visibleCols = new ArrayList();
                int columnsCount = Columns.Count;

                if (IsCheckBoxOrRadioButtonColumn)
                {
                    columnsCount++;
                }

                for (int i = 0; i < columnsCount; i++)
                {
                    string content = GetHeaderText(i);
                    if (!string.IsNullOrEmpty(content))
                    {
                        sb.Append(content);
                        sb.Append(ACAConstant.CultureInfoSplitChar);
                        visibleCols.Add(i);
                    }
                }

                if (sb.Length > 0)
                {
                    sb.Length -= 1;
                }

                sb.Append("\r\n");

                for (int row = 0; row < Rows.Count; row++)
                {
                    for (int col = 0; col < columnsCount; col++)
                    {
                        if (visibleCols.IndexOf(col) > -1)
                        {
                            AppendCellContent(Rows[row].Cells[col].Controls, sb);
                            sb.Append(ACAConstant.CultureInfoSplitChar);
                        }
                    }

                    sb.Append("\r\n");
                }

                return sb.ToString();
            }

            set
            {
                _exportedContent = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether ShortListType is SHORT_LIST type.
        /// </summary>
        public bool IsShortList
        {
            get
            {
                if (ShortListType == SHORT_LIST)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether need force to generate the validation extender.
        /// It generated by the Initial Extender Control method.
        /// </summary>
        public bool IsForceValidation
        {
            get;
            set; 
        }

        /// <summary>
        /// Gets or sets a appended client script on click the Select All checkbox.
        /// </summary>
        public string OnClientSelectAll
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a appended client script on click the Single checkbox.
        /// </summary>
        public string OnClientSelectSingle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets column index of the RadioButton field
        /// </summary>
        [Category("Behavior")]
        [Description("Indicates the 0-based position of the radiobutton column")]
        [DefaultValue(0)]
        private int RadioButtonColumnIndex
        {
            get
            {
                object rdoColumnIndexObj = ViewState["RadioButtonColumnIndex"];

                return rdoColumnIndexObj == null ? 0 : (int)rdoColumnIndexObj;
            }

            set
            {
                ViewState["RadioButtonColumnIndex"] = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// Gets or sets the previous data source
        /// </summary>
        private object PreviousDataSource
        {
            get
            {
                return ViewState["PreviousDataSource"];
            }

            set
            {
                ViewState["PreviousDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the short list type.
        /// </summary>
        private string ShortListType
        {
            get
            {
                if (ViewState["ShortListType"] != null)
                {
                    return ViewState["ShortListType"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["ShortListType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether sort express changed or not.
        /// </summary>
        private bool IsSortExpressionChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether has checkbox or radio button
        /// </summary>
        private bool IsCheckBoxOrRadioButtonColumn
        {
            get
            {
                return AutoGenerateCheckBoxColumn || AutoGenerateRadioButtonColumn;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Export permit list
        /// </summary>
        public void Export()
        {
            // set the download event arguments
            GridViewDownloadEventArgs exportEventArgs = new GridViewDownloadEventArgs();
            exportEventArgs.ExportParameters = GetExportParameterList();

            // trigger the download event
            if (GridViewDownload != null)
            {
                GridViewDownload(this, exportEventArgs);
            }

            if (PageCount > 1 && !string.IsNullOrEmpty(GlobalSearchType))
            {
                return;
            }

            // set the download content
            string content = string.Empty;

            if (PageCount > 1)
            {
                AllowPaging = false;
                DataBind();
                content = ExportedContent;
                AllowPaging = true;
                _rowIndex = 0;
                DataBind();
            }
            else
            {
                content = ExportedContent;
            }

            // download the file
            string dir = ConfigurationManager.AppSettings[EXPORT_TEMP_DIRECTORY];

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fileName = Guid.NewGuid() + "." + ExportFileName;
            string filePath = string.Format("{0}\\{1}.csv", dir, fileName);

            using (StreamWriter fs = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                fs.Write(content);
                fs.Flush();
            }

            Page.Session[EXPORT_FILE_NAME] = fileName;
        }

        /// <summary>
        /// Export Permit/LP/APO list
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="list">the result collection</param>
        public void Export<T>(List<T> list) where T : class, new()
        {
            string dir = ConfigurationManager.AppSettings[EXPORT_TEMP_DIRECTORY];
            string content = BuildExprotContent(list);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fileName = string.Concat(CommonUtil.GetRandomUniqueID(), ACAConstant.SPOT_CHAR, ExportFileName);
            this.Page.Session[EXPORT_FILE_NAME] = fileName;
            string fullFileName = string.Concat(fileName, EXPORT_FILE_EXT);
            string filePath = Path.Combine(dir, fullFileName);

            using (StreamWriter fs = new StreamWriter(filePath))
            {
                fs.Write(content);
                fs.Flush();
            }
        }

        /// <summary>
        /// Initial the ShortList Action.
        /// </summary>
        public void InitialShortListAction()
        {
            // if search or clear the gridview, the ShortList button need reset to initial status.
            ShortListType = string.Empty;
            IsClearSelectedItems = true;
        }

        /// <summary>
        /// Do the short list action.
        /// </summary>
        public void ShortListAction()
        {
            if (!IsShortList && DataSource != null)
            {
                DataTable dtDataSource = DataSource as DataTable;

                if (dtDataSource != null)
                {
                    DataTable dtShortList = GetSelectedData(dtDataSource);

                    // do short list action.
                    if (dtShortList != null && dtShortList.Rows.Count > 0)
                    {
                        IsClearSelectedItems = true;

                        PreviousDataSource = DataSource;
                        DataSource = dtShortList;
                        PageIndex = 0;
                        DataBind();

                        ShortListType = SHORT_LIST;
                    }
                }
            }
            else if (IsShortList)
            {
                // return to the previous result list
                IsClearSelectedItems = true;
                _rowIndex = 0;

                DataSource = PreviousDataSource;
                DataBind();

                ShortListType = PREVIOUS_LIST;
            }
        }

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module, returns string.Empty
        /// </summary>
        /// <returns>module name.</returns>
        public string GetModuleName()
        {
            if (this.Page is IPage)
            {
                return (Page as IPage).GetModuleName();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get selected item's value.
        /// </summary>
        /// <returns>selected item string</returns>
        public string GetSelectItems()
        {
            if (!string.IsNullOrEmpty(_selectedItemsValue))
            {
                return _selectedItemsValue;
            }
            else if (_hfSaveSelectedItems != null)
            {
                return _hfSaveSelectedItems.Value;
            }

            return GetSelectedItemsValue();
        }

        /// <summary>
        /// Get field client ID for selected items hidden field.
        /// </summary>
        /// <returns>the client ID</returns>
        public string GetSelectedItemsFieldClientID()
        {
            return ClientID + this.ClientIDSeparator + SELECTED_ITMS_HIDDEN_FIELD_ID;
        }

        /// <summary>
        /// Get field unique ID for selected items hidden field.
        /// </summary>
        /// <returns>the Unique ID</returns>
        public string GetSelectedItemsFieldUniqueID()
        {
            return UniqueID + this.IdSeparator + SELECTED_ITMS_HIDDEN_FIELD_ID;
        }

        /// <summary>
        /// It's interface for get selected items.
        /// </summary>
        /// <param name="dt">data source</param>
        /// <returns>a data table</returns>
        public DataTable GetSelectedData(DataTable dt)
        {
            if (!IsCheckBoxOrRadioButtonColumn)
            {
                return null;
            }

            if (dt == null)
            {
                return null;
            }

            DataTable dtSelectedData = dt.Clone();

            string selectedItemsValue = GetSelectedItemsValue();

            if (!string.IsNullOrEmpty(selectedItemsValue))
            {
                string[] selectedItems = selectedItemsValue.Split(',');

                foreach (string item in selectedItems)
                {
                    if (item != string.Empty)
                    {
                        dtSelectedData.ImportRow(dt.Rows[int.Parse(item.Replace(ROW_INDEX_PREFIX, string.Empty))]);
                    }
                }
            }

            return dtSelectedData;
        }

        /// <summary>
        /// It's interface for get selected items.
        /// </summary>
        /// <param name="dt">data source</param>
        /// <param name="pageIndex">the page index</param>
        /// <returns>a data table</returns>
        public DataTable GetSelectedData(DataTable dt, int pageIndex)
        {
            if (!IsCheckBoxOrRadioButtonColumn)
            {
                return null;
            }

            if (dt == null)
            {
                return null;
            }

            DataTable dtSelectedData = dt.Clone();

            string selectedItemsValue = GetSelectedItemsValue();

            if (!string.IsNullOrEmpty(selectedItemsValue))
            {
                string[] selectedItems = selectedItemsValue.Split(',');

                foreach (string item in selectedItems)
                {
                    if (item != string.Empty)
                    {
                        int rowIndex = int.Parse(item.Replace(ROW_INDEX_PREFIX, string.Empty));
                        if (rowIndex >= (PageSize * pageIndex) && rowIndex < PageSize * (pageIndex + 1) && rowIndex < dt.Rows.Count)
                        {
                            dtSelectedData.ImportRow(dt.Rows[rowIndex]);
                        }
                    }
                }
            }

            return dtSelectedData;
        }

        /// <summary>
        /// Get the selected row indexes begin from 0 in the current page.
        /// </summary>
        /// <returns>selected row indexes</returns>
        public virtual List<int> GetSelectedRowIndexesInCurrentPage()
        {
            return GetSelectedRowIndexes(true);
        }

        /// <summary>
        /// Get the selected row indexes in all the pages
        /// </summary>
        /// <returns>selected row indexes</returns>
        public virtual List<int> GetSelectedRowIndexes()
        {
            return GetSelectedRowIndexes(false);
        }

        /// <summary>
        /// Select the checkbox in the specified row in the current page.
        /// </summary>
        /// <param name="row">The row that the checkbox will be selected in the current page.</param>
        public void SelectRow(GridViewRow row)
        {
            if (row == null)
            {
                return;
            }

            if (AutoGenerateCheckBoxColumn)
            {
                CheckBox checkBox = row.FindControl(ROW_INDEX_PREFIX + row.DataItemIndex) as CheckBox;

                if (checkBox != null && !checkBox.Checked)
                {
                    checkBox.Checked = true;
                    IsClearSelectedItems = false;
                    AddSelectedItems2HiddenField();
                    _hfSaveSelectedItems.Value = _hfSaveSelectedItems.Value + "," + ROW_INDEX_PREFIX + row.DataItemIndex + ",";
                }
            }

            if (AutoGenerateRadioButtonColumn)
            {
                Literal output = row.FindControl(ROW_INDEX_PREFIX + row.DataItemIndex) as Literal;

                if (output != null && !output.Text.Contains(@"checked"))
                {
                    output.Text = output.Text.Replace("/>", @"checked=""checked"" />");
                    IsClearSelectedItems = false;
                    AddSelectedItems2HiddenField();
                    _hfSaveSelectedItems.Value = _hfSaveSelectedItems.Value + "," + ROW_INDEX_PREFIX + row.DataItemIndex + ",";
                }
            }
        }

        /// <summary>
        /// UnSelect the checkbox in the specified row in the current page.
        /// </summary>
        /// <param name="row">The row that the checkbox will be selected in the current page.</param>
        public void UnselectRow(GridViewRow row)
        {
            if (row == null)
            {
                return;
            }

            if (row.RowType == DataControlRowType.DataRow)
            {
                if (AutoGenerateCheckBoxColumn)
                {
                    // RadioButton is CheckBox's sub-class
                    CheckBox checkBox = row.FindControl(ROW_INDEX_PREFIX + row.DataItemIndex) as CheckBox;

                    if (checkBox != null && checkBox.Checked)
                    {
                        checkBox.Checked = false;
                        AddSelectedItems2HiddenField();
                        _hfSaveSelectedItems.Value = _hfSaveSelectedItems.Value.Replace("," + ROW_INDEX_PREFIX + row.DataItemIndex + ",", ",");
                    }
                }

                if (AutoGenerateRadioButtonColumn)
                {
                    Literal output = row.FindControl(ROW_INDEX_PREFIX + row.DataItemIndex) as Literal;

                    if (output != null && output.Text.Contains(@"checked"))
                    {
                        output.Text = output.Text.Replace(@"checked=""checked""", string.Empty);
                        AddSelectedItems2HiddenField();
                        _hfSaveSelectedItems.Value = _hfSaveSelectedItems.Value.Replace("," + ROW_INDEX_PREFIX + row.DataItemIndex + ",", ",");
                    }
                }
            }
            else if (row.RowType == DataControlRowType.Header && AutoGenerateCheckBoxColumn)
            {
                string checkBoxId = string.Format(CHECKBOX_COLUMN_HEADER_ID, ClientID, PageIndex.ToString());

                // if unchecked, return.
                if (Page.Request[checkBoxId] == null)
                {
                    return;
                }

                foreach (TableCell cell in row.Cells)
                {
                    if (cell.Text.Contains(checkBoxId))
                    {
                        cell.Text = string.Format(
                            CHECKBOX_COLUMN_HEADER_TEMPLATE,
                            checkBoxId,
                            string.Empty,
                            GetSelectedItemsFieldClientID(),
                            OnClientSelectAll,
                            LabelConvertUtil.GetTextByKey("aca_selectallrecords_checkbox", string.Empty));

                        break;
                    }
                }
            }
        }

        /// <summary>
        ///  Get Sort Expression by Looking up the existing Grid View Sort Expression 
        /// </summary>
        public void SaveAll()
        {
            for (int i = 0; i < this.Rows.Count; i++)
            {
                this.UpdateRow(i, false);
            }
        }

        /// <summary>
        /// Provide a method for user to custom sort the GridView
        /// </summary>
        /// <param name="sortExpression">The sort expression</param>
        /// <param name="sortDirection">The sort direction</param>
        public override void Sort(string sortExpression, SortDirection sortDirection)
        {
            GridViewSortExpression = sortExpression;
            GridViewSortDirection = sortDirection == SortDirection.Ascending ? "ASC" : "DESC";

            base.Sort(sortExpression, sortDirection);
        }

        /// <summary>
        /// Order List Collection
        /// </summary>
        /// <param name="objectList">List Collection</param>
        /// <param name="isPageIndexChanging">is page index changing</param>
        /// <returns>List collection</returns>
        public IList<object> SortList(object objectList, bool isPageIndexChanging)
        {
            List<object> dataList = new List<object>();
            List<object> emptyDataList = new List<object>();
            IList list = objectList as IList;

            if (list != null && list.Count > 0)
            {
                //Add Item to List<object>
                foreach (object item in list)
                {
                    if (item != null)
                    {
                        dataList.Add(item);
                    }
                    else
                    {
                        emptyDataList.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(GridViewSortExpression))
                {
                    Type typeElement = dataList[0].GetType();

                    if (GridViewSortDirection != "DESC")
                    {
                        dataList = dataList.OrderBy(DynamicLambda<object, object>(typeElement, GridViewSortExpression)).ToList();
                    }
                    else
                    {
                        dataList = dataList.OrderByDescending(DynamicLambda<object, object>(typeElement, GridViewSortExpression)).ToList();
                    }

                    if (GridViewSort != null)
                    {
                        GridViewSort(this, new AccelaGridViewSortEventArgs(string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection)));
                    }
                }

                dataList.AddRange(emptyDataList);
            }

            return dataList;
        }

        /// <summary>
        /// add the required extender
        /// </summary>
        public void InitExtenderControl()
        {
            if ((IsRequired && _totalRowsCount == 0) || IsForceValidation)
            {
                AccelaTextBox tb = new AccelaTextBox();
                tb.ID = "4ValidateGridView";
                tb.Attributes.Add("ErrorMsg", string.IsNullOrEmpty(ErrorMessage) ? LabelConvertUtil.GetTextByKey("lbl_gridview_required_message", GetModuleName()) : ErrorMessage);
                tb.Validate = "required";
                tb.IsHidden = true;
                tb.CheckControlValueValidateFunction = _checkRequiredFunction;
                tb.ValidationByHiddenTextBox = true;
                Controls.Add(tb);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Get selected item's index.
        /// </summary>
        /// <returns>check box id</returns>
        internal string GetCheckBoxID()
        {
            if (_rowIndex >= _totalRowsCount && _totalRowsCount <= PageSize)
            {
                _rowIndex = 0;
            }

            return ROW_INDEX_PREFIX + ((PageSize * PageIndex) + (_rowIndex++));
        }

        /// <summary>
        /// add a brand new checkbox column
        /// </summary>
        /// <param name="columnList">ICollection object</param>
        /// <returns>ArrayList for checkbox column</returns>
        protected virtual ArrayList AddCheckBoxColumn(ICollection columnList)
        {
            // Get a new container of type ArrayList that contains the given collection.
            // This is required because ICollection doesn't include Add methods
            ArrayList list = new ArrayList(columnList);

            // Determine the check state for the header checkbox
            string shouldCheck = string.Empty;
            string checkBoxID = string.Format(CHECKBOX_COLUMN_HEADER_ID, ClientID, PageIndex.ToString());

            if (!DesignMode && !IsClearSelectedItems)
            {
                object o = Page.Request[checkBoxID];

                if (o != null)
                {
                    shouldCheck = "checked=\"checked\"";
                }

                IsClearSelectedItems = false;
            }

            if (AutoGenerateCheckBoxColumn)
            {
                // Create a new custom CheckBoxField object
                InputCheckBoxField field = new InputCheckBoxField();
                string hfSaveSelectedItems = GetSelectedItemsFieldClientID();
                field.HeaderText = string.Format(
                    CHECKBOX_COLUMN_HEADER_TEMPLATE,
                    checkBoxID,
                    shouldCheck,
                    hfSaveSelectedItems,
                    OnClientSelectAll,
                    LabelConvertUtil.GetTextByKey("aca_selectallrecords_checkbox", string.Empty));
                field.HeaderStyle.CssClass += "ACA_AlignLeftOrRight";
                field.HeaderStyle.Width = Unit.Pixel(20);
                field.ReadOnly = true;

                // Insert the checkbox field into the list at the specified position
                if (CheckBoxColumnIndex > list.Count)
                {
                    // If the desired position exceeds the number of columns
                    // add the checkbox field to the right. Note that this check
                    // can only be made here because only now we know exactly HOW
                    // MANY columns we're going to have. Checking Columns.Count in the
                    // property setter doesn't work if columns are auto-generated
                    list.Add(field);
                    CheckBoxColumnIndex = list.Count - 1;
                }
                else
                {
                    list.Insert(CheckBoxColumnIndex, field);
                }
            }

            // Return the new list
            return list;
        }

        /// <summary>
        /// add a brand new radio button column
        /// </summary>
        /// <param name="columnList">ICollection object</param>
        /// <returns>ArrayList for radio button column</returns>
        protected virtual ArrayList AddRadioButtonColumn(ICollection columnList)
        {
            // Get a new container of type ArrayList that contains the given collection.
            // This is required because ICollection doesn't include Add methods
            ArrayList list = new ArrayList(columnList);

            if (AutoGenerateRadioButtonColumn)
            {
                // Create a new custom CheckBoxField object
                RadioButtonField field = new RadioButtonField();
                field.ReadOnly = true;
                field.HeaderStyle.Width = Unit.Pixel(20);

                // Insert the checkbox field into the list at the specified position
                if (RadioButtonColumnIndex > list.Count)
                {
                    // If the desired position exceeds the number of columns
                    // add the checkbox field to the right. Note that this check
                    // can only be made here because only now we know exactly HOW
                    // MANY columns we're going to have. Checking Columns.Count in the
                    // property setter doesn't work if columns are auto-generated
                    list.Add(field);
                    RadioButtonColumnIndex = list.Count - 1;
                }
                else
                {
                    list.Insert(RadioButtonColumnIndex, field);
                }
            }

            // Return the new list
            return list;
        }

        /// <summary>
        /// Display an empty row if no data rows created.
        /// </summary>
        /// <param name="dataSource">An System.Collections.IEnumerable that contains the data source for 
        /// the System.Web.UI.WebControls.GridView control.</param>
        /// <param name="dataBinding"> true to indicate that the child controls are bound to data; otherwise, false.</param>
        /// <returns>The number of rows created.</returns>
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            PagedDataSource source = new PagedDataSource();
            source.DataSource = dataSource;

            if (this.IsInGlobalSearch())
            {
                _totalRowsCount = this.CustomizedTotalCount;
            }
            else
            {
                _totalRowsCount = source.Count;
            }

            int _totalRows = base.CreateChildControls(dataSource, dataBinding);

            //  no data rows created, create empty table if enabled
            if (_totalRows == 0 && (this.ShowFooterWhenEmpty || this.ShowHeaderWhenEmpty))
            {
                //  create the table
                Table table = this.CreateChildTable();
                DataControlField[] fields;

                if (this.AutoGenerateColumns)
                {
                    PagedDataSource columSource = source.DataSource != null ? source : null;
                    System.Collections.ICollection autoGeneratedColumns = this.CreateColumns(columSource, true);
                    fields = new DataControlField[autoGeneratedColumns.Count];

                    autoGeneratedColumns.CopyTo(fields, 0);
                }
                else
                {
                    fields = new DataControlField[this.Columns.Count];
                    Columns.CopyTo(fields, 0);
                }

                if (this.ShowHeaderWhenEmpty)
                {
                    //  create a new header row
                    GridViewRow headerRow = this.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);

                    this.InitializeRow(headerRow, fields);
                    SortColumnHeader(headerRow);

                    //  add the header row to the table
                    table.Rows.Add(headerRow);
                }

                //  create the empty row
                GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                TableCell cell = new TableCell();
                cell.ColumnSpan = fields.Length;

                //  respect the precedence order if both EmptyDataTemplate
                //  and EmptyDataText are both supplied ...
                if (EmptyDataTemplate != null)
                {
                    EmptyDataTemplate.InstantiateIn(cell);
                }
                else if (!string.IsNullOrEmpty(this.EmptyDataText))
                {
                    cell.Controls.Add(new LiteralControl(EmptyDataText));
                }

                emptyRow.Cells.Add(cell);
                table.Rows.Add(emptyRow);

                if (this.ShowFooterWhenEmpty)
                {
                    //  create footer row
                    GridViewRow footerRow = this.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
                    this.InitializeRow(footerRow, fields);

                    //  add the footer to the table
                    table.Rows.Add(footerRow);
                }

                Controls.Clear();
                Controls.Add(table);
            }

            if (ShowCartLink && _totalRowsCount > 0)
            {
                AddControlsForAddToCart();
            }

            if (ShowCloneRecordLink && _totalRowsCount > 0)
            {
                AddButtonForCloneRecord();
            }

            return _totalRows;
        }

        /// <summary>
        /// Create checkbox column if AutoGenerateCheckBoxColumn is true
        /// </summary>
        /// <param name="dataSource">A System.Web.UI.WebControls.PagedDataSource that represents the data source.</param>
        /// <param name="useDataSource">true to use the data source specified by the dataSource parameter; otherwise,false.</param>
        /// <returns>A System.Collections.ICollection that contains the fields used to build the
        /// control hierarchy.</returns>
        protected override ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
        {
            // Let the GridView create the default set of columns
            ICollection columnList = base.CreateColumns(dataSource, useDataSource);

            bool isAdmin = AccelaControlRender.IsAdminRender(this);

            foreach (DataControlField field in columnList)
            {
                if (string.IsNullOrEmpty(field.HeaderStyle.CssClass))
                {
                    field.HeaderStyle.CssClass = "ACA_AlignLeftOrRight";
                    field.ItemStyle.CssClass = "ACA_AlignLeftOrRightTop";
                }

                if (isAdmin && !field.ShowHeader)
                {
                    field.HeaderStyle.Width = Unit.Pixel(0);
                }
            }

            if (!IsCheckBoxOrRadioButtonColumn || (dataSource != null && dataSource.Count < 1))
            {
                _totalColumns = columnList.Count;

                return columnList;
            }

            ArrayList extendedColumnList = new ArrayList();

            // Add a checkbox column if required
            if (AutoGenerateCheckBoxColumn)
            {
                extendedColumnList = AddCheckBoxColumn(columnList);
            }

            if (AutoGenerateRadioButtonColumn)
            {
                extendedColumnList = AddRadioButtonColumn(columnList);
            }

            _totalColumns = extendedColumnList.Count;

            return extendedColumnList;
        }

        /// <summary>
        ///  Display a graphic image for the Sort Order along with the sort sequence no.
        /// </summary>
        /// <param name="sortExpression">sort expression</param>
        /// <param name="dgItem">GridViewRow object</param>
        protected void DisplaySortOrderImages(string sortExpression, GridViewRow dgItem)
        {
            string[] sortColumns = sortExpression.Split(",".ToCharArray());

            for (int i = 0; i < dgItem.Cells.Count; i++)
            {
                if (dgItem.Cells[i].Controls.Count > 0 && dgItem.Cells[i].Controls[0] is LinkButton)
                {
                    DisplaySortImages(dgItem, i, sortColumns);
                }
            }
        }

        /// <summary>
        /// retrieve the style object based on the row state
        /// </summary>
        /// <param name="state">DataControlRowState object</param>
        /// <returns>TableItemStyle object</returns>
        protected virtual TableItemStyle GetRowStyleFromState(DataControlRowState state)
        {
            switch (state)
            {
                case DataControlRowState.Alternate:
                    return AlternatingRowStyle;
                case DataControlRowState.Edit:
                    return EditRowStyle;
                case DataControlRowState.Selected:
                    return SelectedRowStyle;
                default:
                    return RowStyle;

                // DataControlRowState.Insert is not relevant here
            }
        }

        /// <summary>
        /// get the sort direction
        /// </summary>
        /// <returns>sort direction string</returns>
        protected string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }

            return GridViewSortDirection;
        }

        /// <summary>
        /// Get the sort expression
        /// </summary>
        /// <param name="e">GridViewSortEventArgs object</param>
        /// <returns>sort expression string</returns>
        protected string GetSortExpression(GridViewSortEventArgs e)
        {
            string[] sortColumns = null;
            string sortAttribute = SortExpression;

            //Check to See if we have an existing Sort Order already in the Grid View.
            //If so get the Sort Columns into an array
            if (sortAttribute != string.Empty)
            {
                sortColumns = sortAttribute.Split(",".ToCharArray());
            }

            //if User clicked on the columns in the existing sort sequence.
            //Toggle the sort order or remove the column from sort appropriately
            if (sortAttribute.IndexOf(e.SortExpression, StringComparison.InvariantCulture) > 0 || sortAttribute.StartsWith(e.SortExpression, StringComparison.InvariantCulture))
            {
                sortAttribute = ModifySortExpression(sortColumns, e.SortExpression);
            }
            else
            {
                sortAttribute += string.Concat(",", e.SortExpression, " ASC ");
            }

            return sortAttribute.TrimStart(",".ToCharArray()).TrimEnd(",".ToCharArray());
        }

        /// <summary>
        /// Initializes the pager row displayed when the paging feature is enabled.
        /// </summary>
        /// <param name="row">A System.Web.UI.WebControls.GridViewRow that represents the pager row to initialize.</param>
        /// <param name="columnSpan">The number of columns the pager row should span.</param>
        /// <param name="pagedDataSource">A System.Web.UI.WebControls.PagedDataSource that represents the data source.</param>
        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            this.CountSummary = SearchResultUtil.GenerateRecordsSummary(this._totalRowsCount, this.PageSize, this.PageIndex);

            NextPrevNumericPagerTemplate template = new NextPrevNumericPagerTemplate(pagedDataSource, ShowExportLink, FooterStyle.HorizontalAlign);
            template.ShowLoadingPanel = this.ShowLoadingPanel;
            this.PagerTemplate = template;
            base.InitializePager(row, columnSpan, pagedDataSource);
            _exportLinkButton = template.ExportLinkButton;
        }

        /// <summary>
        ///  Toggle the sort order or remove the column from sort appropriately
        /// </summary>
        /// <param name="sortColumns">sort column array</param>
        /// <param name="sortExpression">sort expression</param>
        /// <returns>modified sort expression</returns>
        protected string ModifySortExpression(string[] sortColumns, string sortExpression)
        {
            string ascSortExpression = string.Concat(sortExpression, " ASC ");
            string descSortExpression = string.Concat(sortExpression, " DESC ");

            for (int i = 0; i < sortColumns.Length; i++)
            {
                //if (ascSortExpression.Equals(sortColumns[i]))
                if (ascSortExpression.Equals(sortColumns[i], StringComparison.InvariantCulture))
                {
                    sortColumns[i] = descSortExpression;
                }
                else if (descSortExpression.Equals(sortColumns[i], StringComparison.InvariantCulture))
                {
                    Array.Clear(sortColumns, i, 1);
                }
            }

            return string.Join(",", sortColumns).Replace(",,", ",").TrimStart(",".ToCharArray());
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Load the script code GridView needs 
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string root = HttpContext.Current.Request.ApplicationPath;

            if (!root.EndsWith("/", StringComparison.InvariantCulture))
            {
                root += "/";
            }

            string scriptName = root + "Scripts/ACCELAGRIDVIEW_JS.js";

            ScriptManager.RegisterClientScriptInclude(this, GetType(), typeof(AccelaGridView).FullName, scriptName);
        }

        /// <summary>
        ///  illustrates the server code that runs in the pre-render phase and injects any needed OnClick handlers.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (!string.IsNullOrEmpty(GridViewNumber))
            {
                IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));
                render.OnPreRender(this);
            }

            // Do as usual
            base.OnPreRender(e);

            if (AutoGenerateCheckBoxColumn)
            {
                PreRenderCheckBoxColumn();
            }

            if (AutoGenerateRadioButtonColumn)
            {
                PreRenderRadioButtonColumn();
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.WebControls.GridView.RowCommand event.
        /// </summary>
        /// <param name="e">A System.Web.UI.WebControls.GridViewCommandEventArgs that contains event data.</param>
        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            base.OnRowCommand(e);

            if (e.CommandName == "Header")
            {
                var sortExpression = Convert.ToString(e.CommandArgument);
                IsSortExpressionChanged = !sortExpression.Equals(GridViewSortExpression, StringComparison.OrdinalIgnoreCase);
                GridViewSortExpression = sortExpression;

                AccelaGridView_Sorting(null, null);
            }
            else if (e.CommandName == "Export")
            {
                Export();
            }
        }

        /// <summary>
        /// reset the row CSS style
        /// </summary>
        /// <param name="e">A System.Web.UI.WebControls.GridViewRowEventArgs that contains event data.</param>
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            int index = e.Row.RowIndex % 2;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (SortExpression != string.Empty)
                {
                    DisplaySortOrderImages(SortExpression, e.Row);
                }

                e.Row.CssClass = this.HeaderStyle.CssClass; // "table_row_header_full";
                SortColumnHeader(e.Row);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.CssClass = index == 0 ? "table_row_odd_full" : "table_row_even_full";
                e.Row.CssClass = index == 0 ? RowStyle.CssClass : AlternatingRowStyle.CssClass;
                SortColumnData(e.Row);
            }

            base.OnRowCreated(e);
        }

        /// <summary>
        /// Allow multiple columns sorting
        /// </summary>
        /// <param name="e">A System.Web.UI.WebControls.GridViewSortEventArgs that contains event data.</param>
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            //if we want to use AllowMultiColumnSorting ,then we should specify the sorting column
            //such as AccelaGridView.SortExpression = "License Number,Date";
            if (AllowMultiColumnSorting)
            {
                //e.SortExpression = GetSortExpression(e);
                GridViewSortExpression = GetSortExpression(e);
            }
            else
            {
                GridViewSortExpression = e.SortExpression;
            }

            base.OnSorting(e);
        }

        /// <summary>
        /// Establishes the control hierarchy.
        /// </summary>
        protected override void PrepareControlHierarchy()
        {
            if (!HasControls() || !(Controls[0] is Table))
            {
                return;
            }

            Table t = (Table)Controls[0];

            //if contains checkbox column, then add select items to hidden field.
            if (IsCheckBoxOrRadioButtonColumn)
            {
                AddSelectedItems2HiddenField();

                if (IsClearSelectedItems)
                {
                    ClearSelectedItems(t);
                }
            }

            base.PrepareControlHierarchy();

            //Remove the unexpected style from GridView:border-collapse: collapse;
            t.CellSpacing = -1;

            if (BorderWidth.IsEmpty)
            {
                // If use t.Attributes.Add("border", "0"), there will be two "border" attributes in the parsed HTML.
                t.BorderWidth = 0;
            }

            if (string.IsNullOrEmpty(t.CssClass))
            {
                t.CssClass = "ACA_GridView ACA_Grid_Caption";
            }
            else
            {
                t.CssClass += " ACA_GridView ACA_Grid_Caption";
            }

            //if (_captionTemplate != null && string.IsNullOrEmpty(Caption))
            if (ShowCaption)
            {
                TableRow row = new TableRow();

                t.Rows.AddAt(0, row);

                TableCell cell2 = new TableCell();
                Table t2 = new Table();
                t2.Attributes.Add("role", "presentation");
                TableRow row2 = new TableRow();

                row.Cells.Add(cell2);
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("class", "aca_gridview_caption");
                div.Controls.Add(t2);
                cell2.Controls.Add(div);
                t2.Rows.Add(row2);

                TableCell cell = new TableCell();
                cell.Wrap = false;
                row2.Cells.Add(cell);

                if (_totalColumns == 0)
                {
                    _totalColumns = 100;
                }

                cell2.ColumnSpan = _totalColumns;
                Label indicator = new Label();
                indicator.CssClass = CaptionCssClass;

                int start = 0;
                int end = 0;

                if (_totalRowsCount > 0)
                {
                    if (!AllowPaging)
                    {
                        PageSize = _totalRowsCount;
                    }

                    int lastPageSize = PageSize;

                    if ((_totalRowsCount % PageSize) != 0 && PageCount == (PageIndex + 1))
                    {
                        lastPageSize = _totalRowsCount % PageSize;
                    }

                    start = (PageSize * PageIndex) + 1;
                    end = (PageSize * PageIndex) + lastPageSize;
                }

                string _pageIndicator = LabelConvertUtil.GetTextByKey("ACA_AccelaGridView_PageIndicator", GetModuleName());
                indicator.Text = string.Format(_pageIndicator, start, end, CountSummary);
                cell.Controls.Add(indicator);
                cell.DataBind();

                if (ShowExportLink && _totalRowsCount > 0)
                {
                    TableCell cellNbsp = new TableCell();
                    AccelaLabel lblSplit = new AccelaLabel();
                    lblSplit.IsNeedEncode = false;
                    lblSplit.CssClass = "tab_tar_separator font11px";
                    lblSplit.Text = "|";
                    cellNbsp.Controls.Add(lblSplit);
                    cellNbsp.DataBind();
                    row2.Cells.Add(cellNbsp);

                    TableCell cellExport = new TableCell();
                    cellExport.Wrap = false;
                    row2.Cells.Add(cellExport);
                    _exportLinkButton4Show = new AccelaLinkButton();
                    _exportLinkButton4Show.ID = this.ID + "top4btnExport";
                    _exportLinkButton4Show.Text = LabelConvertUtil.GetTextByKey("aca_nextPrevNumbericPagerTemplate_exportLink", string.Empty);
                    _exportLinkButton4Show.CausesValidation = false;
                    _exportLinkButton4Show.CssClass = CaptionCssClass;
                    cellExport.Controls.Add(_exportLinkButton4Show);
                    cellExport.DataBind();
                }

                if (ShowAdd2CollectionLink && _totalRowsCount > 0)
                {
                    CreateAdd2CollectionLink(row2);
                }

                if (ShowCartLink && _totalRowsCount > 0)
                {
                    CreateAddToCartLink(row2);
                }

                if (ShowCloneRecordLink && _totalRowsCount > 0)
                {
                    CreateCloneRecordLink(row2);
                }

                if (ShowShortListLink && _totalRowsCount > 0)
                {
                    CreateShortListLink(row2);
                }
            }
            else
            {
                bool isAdmin = AccelaControlRender.IsAdminRender(this);

                if (isAdmin && !string.IsNullOrEmpty(GridColumnsVisible))
                {
                    GridViewJson gridView = (GridViewJson)JsonConvert.DeserializeObject(GridColumnsVisible, typeof(GridViewJson));

                    if (gridView != null && gridView.GridViewColumnVisibleList != null && gridView.GridViewColumnVisibleList.Count > 0)
                    {
                        List<GridViewColumnVisible> visibleColumns = gridView.GridViewColumnVisibleList.Where(g => g.Visible == true).ToList();

                        if (visibleColumns != null && visibleColumns.Count == 0)
                        {
                            TableRow row = new TableRow();
                            TableCell blankCell = new TableCell();
                            row.CssClass = DEFAULT_HEADER_STYLE_CSS;

                            if (_totalColumns == 0)
                            {
                                _totalColumns = 100;
                            }

                            blankCell.ColumnSpan = _totalColumns;

                            row.Cells.Add(blankCell);
                            HtmlGenericControl div = new HtmlGenericControl("div");
                            div.Attributes.Add("class", "ACA_Width30em");
                            div.InnerHtml = ACAConstant.HTML_NBSP;
                            blankCell.Controls.Add(div);

                            t.Rows.AddAt(0, row);
                        }
                    }
                }
            }

            bool showLink = ShowExportLink && _totalRowsCount > 0 && _totalRowsCount <= PageSize;

            if (showLink)
            {
                TableRow rr = new TableRow();
                rr.CssClass = "ACA_Table_Pages ACA_Table_Pages_FontSize";
                AddExportButton(rr);
                t.Rows.AddAt(t.Rows.Count - 1, rr);
            }

            if (_exportLinkButton != null && _exportLinkButton4Show != null)
            {
                if (!string.IsNullOrEmpty(this.GlobalSearchType) && !showLink)
                {
                    _exportLinkButton4Show.OnClientClick = string.Format("TriggleExport('{0}','{1}','{2}'); return false;", this.GlobalSearchType, this.ClientID, _exportLinkButton.ClientID);
                }
            }

            if (IsRequired && _totalRowsCount == 0)
            {
                TableRow row = new TableRow();
                t.Rows.AddAt(0, row);
                TableCell cell = new TableCell();
                cell.ColumnSpan = _totalColumns;
                row.Cells.Add(cell);

                Label indicator = new Label();
                indicator.ID = "4ValidateGridView_lbl_error_msg";
                indicator.CssClass = "ACA_Label ACA_Label_FontSize";
                cell.Controls.Add(indicator);
                cell.DataBind();
            }
        }

        /// <summary>
        /// Displays the grid view on the client.
        /// </summary>
        /// <param name="output">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(tw);

            // add attribute ti indicating the gridview
            if (this.ShowLoadingPanel)
            {
                htw.AddAttribute("loadingpanel", "true");
            }

            //add summary attribute at gridview to meet section 508
            if (!string.IsNullOrEmpty(SummaryKey))
            {
                this.Attributes.Add(ACAConstant.SUMMARY, LabelConvertUtil.GetTextByKey(SummaryKey, ModuleName));
            }

            // add Caption attribute at gridview to meet WCAG
            Caption = LabelConvertUtil.GetTextByKey(CaptionKey, ModuleName);
 
            //Add the 'cellspacing' attribute to main table manually.
            if (HasControls())
            {
                Table tab = this.Controls[0] as Table;
                if (null != tab)
                {
                    tab.Attributes.Add("cellspacing", "0");
                }
            }

            base.Render(htw);
            sb = sb.Replace("rules=\"all\"", string.Empty).Replace("<!--", "&lt;!--");

            if (ShowHorizontalScroll)
            {
                //append auto overflow div to grid.
                sb.Remove(0, 5);
                if (IsAutoWidth)
                {
                    sb.Insert(0, "<div class=\"ACA_Grid_OverFlow_None_Width\">");
                }
                else
                {
                    sb.Insert(0, "<div class=\"ACA_Grid_OverFlow\">");
                }
            }

            sb.Insert(0, string.Format("<a id='{0}' name='{0}'></a>", this.ClientID + "_4ValidateGridView_anchor"));

            if (IsInSPEARForm)
            {
                /*
                 * Ensure there is method SetNotAskForSPEAR() in onclick instead of href in A control: 
                 * If it has attribute onclick, Add method SetNotAskForSPEAR() to this onclick.
                 * If it only has attribute href, Add attribute onclick include method SetNotAskForSPEAR().
                 */
                output.Write(Regex.Replace(
                    sb.ToString(),
                    @"(?<g1>\<a[^\>]+?onclick\s*?=\s*?[""']{1})|((?<g2>\<a[^\>]+?)(?<g3>href\s*?=\s*?[""']{1}javascript:))",
                    GetReplacement4SetNotAsk,
                    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant));
            }
            else
            {
                output.Write(sb.ToString());
            }
        }

        /// <summary>
        ///  Lookup the Current Sort Expression to determine the Order of a specific item.
        /// </summary>
        /// <param name="sortColumns">string array</param>
        /// <param name="sortColumn">string sort column</param>
        /// <param name="sortOrder">sort order</param>
        /// <param name="sortOrderNo">sort order number</param>
        protected void SearchSortExpression(string[] sortColumns, string sortColumn, out string sortOrder, out int sortOrderNo)
        {
            sortOrder = string.Empty;
            sortOrderNo = -1;

            for (int i = 0; i < sortColumns.Length; i++)
            {
                if (sortColumns[i].StartsWith(sortColumn, StringComparison.InvariantCulture))
                {
                    sortOrderNo = i + 1;

                    if (AllowMultiColumnSorting)
                    {
                        sortOrder = sortColumns[i].Substring(sortColumn.Length).Trim();
                    }
                    else
                    {
                        sortOrder = (SortDirection == SortDirection.Ascending) ? "ASC" : "DESC";
                    }
                }
            }
        }

        /// <summary>
        /// If sorting when clicking header we will change the sort direction. if paging, use the same sort direction
        /// </summary>
        /// <param name="e">EventArgs object</param>
        /// <param name="isPageIndexChanging">is page index changing</param>
        /// <returns>a data view</returns>
        protected DataView Sort(EventArgs e, bool isPageIndexChanging)
        {
            DataView dataView;

            if (DataSource is DataView)
            {
                dataView = DataSource as DataView;
            }
            else
            {
                dataView = new DataView(DataSource as DataTable);
            }

            DataTable dt = dataView.ToTable();

            //Add selected items to hashtable
            if (IsCheckBoxOrRadioButtonColumn)
            {
                string selectedItemValue = GetSelectedItemsValue();

                if (!string.IsNullOrEmpty(selectedItemValue))
                {
                    string[] selectedItems = selectedItemValue.Split(',');

                    foreach (string selectItem in selectedItems)
                    {
                        if (!string.IsNullOrEmpty(selectItem) && !_htSaveSelectedItems.Contains(selectItem))
                        {
                            _htSaveSelectedItems.Add(selectItem, dt.Rows[int.Parse(selectItem.Replace(ROW_INDEX_PREFIX, string.Empty))]["GridRowID"]);
                        }
                    }
                }
            }

            if (GridViewSortExpression != string.Empty)
            {
                string sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);

                if (dataView.Table.Columns.Count > 0 && dataView.Table.Rows.Count > 0)
                {
                    dataView.Sort = sort;

                    if (GridViewSort != null)
                    {
                        GridViewSort(this, new AccelaGridViewSortEventArgs(sort));
                    }
                }
            }

            //Get selected items from hashtable.
            if (IsCheckBoxOrRadioButtonColumn)
            {
                StringBuilder sb = new StringBuilder(",");
                dt = dataView.ToTable();

                foreach (DictionaryEntry selectedItem in _htSaveSelectedItems)
                {
                    string value = selectedItem.Value.ToString();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["GridRowID"].ToString() == value)
                        {
                            sb.Append(ROW_INDEX_PREFIX + i);
                            sb.Append(",");
                            break;
                        }
                    }
                }

                _selectedItemsValue = sb.ToString();
            }

            return dataView;
        }

        /// <summary>
        /// Event menuPager_MenuItemClick
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">object MenuEventArgs</param>
        protected void MenuPager_MenuItemClick(object sender, MenuEventArgs e)
        {
            OnPageIndexChanging(new GridViewPageEventArgs(int.Parse(e.Item.Value)));
        }

        /// <summary>
        /// find table cell which the control is within.
        /// </summary>
        /// <param name="control">Control which is put in a TableCell.</param>
        /// <returns>return the tableCell which the control put in.</returns>
        protected TableCell FindTableCell(Control control)
        {
            if (control.GetType() == typeof(TableCell) || control.GetType() == typeof(DataControlFieldHeaderCell) || control.GetType() == typeof(DataControlFieldCell))
            {
                return control as TableCell;
            }
            else if (control.Parent != null)
            {
                return FindTableCell(control.Parent);
            }
            else
            {
                return null;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Add Controls for add to cart.
        /// </summary>
        private void AddControlsForAddToCart()
        {
            UpdatePanel upPanelForAddToCart = new UpdatePanel();
            upPanelForAddToCart.ID = "updatePanelAddToCart";
            upPanelForAddToCart.UpdateMode = UpdatePanelUpdateMode.Conditional;

            btnAppendToCart = new AccelaLinkButton();
            btnAppendToCart.ID = "btnAppendToCart";
            btnAppendToCart.Attributes.Add("style", "display:none");
            upPanelForAddToCart.ContentTemplateContainer.Controls.Add(btnAppendToCart);

            Controls.Add(upPanelForAddToCart);
        }

        /// <summary>
        /// Add Controls for clone record.
        /// </summary>
        private void AddButtonForCloneRecord()
        {
            UpdatePanel upPanelForCloneRecord = new UpdatePanel();
            upPanelForCloneRecord.ID = "updatePanelCloneRecord";
            upPanelForCloneRecord.UpdateMode = UpdatePanelUpdateMode.Conditional;

            btnCloneRecord = new AccelaLinkButton();
            btnCloneRecord.ID = "btnCloneRecord";
            btnCloneRecord.Attributes.Add("style", "display:none");
            upPanelForCloneRecord.ContentTemplateContainer.Controls.Add(btnCloneRecord);

            Controls.Add(upPanelForCloneRecord);
        }

        /// <summary>
        /// grid view page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">object GridViewPageEventArgs</param>
        private void AccelaGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (!this.IsInGlobalSearch())
            {
                //Do not need to sort data list for global search during page index changing.
                //because of global search will lose previous data and fill them with null, sort null data will meet error.
                if (DataSource == null)
                {
                    return;
                }
                else if (DataSource is DataTable)
                {
                    DataView dv = Sort(e, true);
                    PageIndex = e.NewPageIndex;
                    DataSource = dv;
                    DataBind();
                }
                else if (DataSource.GetType().FullName.StartsWith("System.Collections.Generic.List`1"))
                {
                    PageIndex = e.NewPageIndex;
                    DataSource = SortList(DataSource, true);
                    DataBind();
                }
            }
        }

        /// <summary>
        /// Handle Sorting event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">object GridViewSortEventArgs</param>
        private void AccelaGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Get Sort Condition
            if (!IsSortExpressionChanged)
            {
                GridViewSortDirection = GetSortDirection();
            }

            if (DataSource == null)
            {
                return;
            }
            else if (DataSource is DataTable)
            {
                DataView dv = Sort(e, false);
                PageIndex = 0;
                DataSource = dv;
                DataBind();
            }
            else if (DataSource.GetType().FullName.StartsWith("System.Collections.Generic.List`1"))
            {
                PageIndex = 0;
                DataSource = SortList(DataSource, false);
                DataBind();
            }
        }

        /// <summary>
        /// Build a Expression with Embed Property.
        /// </summary>
        /// <param name="type">Element Type</param>
        /// <param name="propertyName">Property Name</param>
        /// <param name="convertObj">Parameter Object</param>
        /// <returns>the embed property.</returns>
        private Expression EmbedProperty(Type type, string propertyName, Expression convertObj)
        {
            Expression propertyGetter;

            string currentPropertyName = propertyName;

            if (currentPropertyName.IndexOf(ACAConstant.SPOT_CHAR) > 0)
            {
                string prePropertyName = propertyName.Substring(0, propertyName.IndexOf(ACAConstant.SPOT_CHAR));
                currentPropertyName = propertyName.Substring(propertyName.IndexOf(ACAConstant.SPOT_CHAR) + 1);
                propertyGetter = Expression.Property(convertObj, prePropertyName);
                var propertyInfo = type.GetProperty(prePropertyName);
                var nullConstant = Expression.Constant(null, propertyInfo.PropertyType);
                var equalNullChecker = Expression.Equal(propertyGetter, nullConstant);
                var tempPropertyGetter = EmbedProperty(propertyInfo.PropertyType, currentPropertyName, propertyGetter);
                var defaultValue = Expression.Default(tempPropertyGetter.Type);
                propertyGetter = Expression.Condition(equalNullChecker, defaultValue, tempPropertyGetter);
            }
            else
            {
                propertyGetter = Expression.Property(convertObj, type.GetProperty(currentPropertyName));
            }

            return propertyGetter;
        }

        /// <summary>
        /// Build a Dynamic Lambda Expression
        /// </summary>
        /// <typeparam name="T">Object Module</typeparam>
        /// <typeparam name="Tkey">Object Key</typeparam>
        /// <param name="type">Element Type</param>
        /// <param name="propertyName">Order Property Name</param>
        /// <returns>Order Lambda Function</returns>
        private Func<T, Tkey> DynamicLambda<T, Tkey>(Type type, string propertyName)
        {
            var paramObj = Expression.Parameter(typeof(object), "obj");

            var convertObj = Expression.Convert(paramObj, type);

            var propertyGetter = EmbedProperty(type, propertyName, convertObj);

            var returnObj = Expression.Convert(propertyGetter, typeof(object));

            var lambda = Expression.Lambda<Func<T, Tkey>>(returnObj, paramObj);

            return lambda.Compile();
        }

        /// <summary>
        /// Add export button
        /// </summary>
        /// <param name="row">table row.</param>
        private void AddExportButton(TableRow row)
        {
            _exportLinkButton = new AccelaLinkButton();
            TableCell cell = new TableCell();
            cell.Controls.Add(_exportLinkButton);
            row.Cells.Add(cell);
            _exportLinkButton.ID = "ctl13_lb4btnExport";
            _exportLinkButton.Text = string.Empty;
            _exportLinkButton.CausesValidation = false;
            _exportLinkButton.TabIndex = -1;
        }

        /// <summary>
        /// Add GridRowID column to current DataSource
        /// </summary>
        /// <param name="dt">data table</param>
        private void AddGridRowIdColumn(DataTable dt)
        {
            int maxRowIndex = 0;

            //Add GridRowID column and set the value to it.
            if (!dt.Columns.Contains("GridRowID"))
            {
                dt.Columns.Add("GridRowID");
            }
            else
            {
                // Get maximum row index
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string gridRowID = dt.Rows[i]["GridRowID"].ToString();

                    if (!string.IsNullOrEmpty(gridRowID) && maxRowIndex <= int.Parse(gridRowID))
                    {
                        maxRowIndex = int.Parse(gridRowID) + 1;
                    }
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i]["GridRowID"].ToString()))
                {
                    dt.Rows[i]["GridRowID"] = maxRowIndex.ToString();
                    maxRowIndex++;
                }
            }
        }

        /// <summary>
        /// Add selected items  to Hidden field.
        /// </summary>
        private void AddSelectedItems2HiddenField()
        {
            if (IsCheckBoxOrRadioButtonColumn && _hfSaveSelectedItems == null)
            {
                //New one HiddenField to save selected items.
                _hfSaveSelectedItems = new HiddenField();
                _hfSaveSelectedItems.ID = SELECTED_ITMS_HIDDEN_FIELD_ID;

                if (string.IsNullOrEmpty(_selectedItemsValue))
                {
                    if (IsClearSelectedItems)
                    {
                        _hfSaveSelectedItems.Value = string.Empty;
                    }
                    else
                    {
                        _hfSaveSelectedItems.Value = GetSelectedItemsValue();
                    }
                }
                else
                {
                    if (IsClearSelectedItems)
                    {
                        _hfSaveSelectedItems.Value = string.Empty;
                    }
                    else
                    {
                        _hfSaveSelectedItems.Value = _selectedItemsValue;
                    }
                }

                Controls.Add(_hfSaveSelectedItems);
            }
        }

        /// <summary>
        /// Clears the selected items.
        /// </summary>
        /// <param name="t">The grid table.</param>
        private void ClearSelectedItems(Table t)
        {
            if (t != null && t.Rows != null && t.Rows.Count > 0)
            {
                foreach (TableRow currentTableRow in t.Rows)
                {
                    Control currentCheckBox = null;

                    if (currentTableRow.Cells.Count > 0 && currentTableRow.Cells[0].Controls.Count > 0)
                    {
                        currentCheckBox = currentTableRow.Cells[0].Controls[0];
                    }

                    if (currentCheckBox != null && currentCheckBox is CheckBox)
                    {
                        (currentCheckBox as CheckBox).Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// Append cell content
        /// </summary>
        /// <param name="controls">ControlCollection object</param>
        /// <param name="sb">string build</param>
        private void AppendCellContent(ControlCollection controls, StringBuilder sb)
        {
            foreach (Control control in controls)
            {
                if (control.Visible)
                {
                    string text = string.Empty;

                    if (control is AccelaLabel)
                    {
                        text = ((AccelaLabel)control).Text;
                    }

                    if (control is AccelaDateLabel)
                    {
                        text = ((AccelaDateLabel)control).Text;
                    }
                    else if (control is LinkButton)
                    {
                        text = ((LinkButton)control).Text;
                        if (text.Trim() == string.Empty)
                        {
                            AppendCellContent(((LinkButton)control).Controls, sb);
                        }
                    }
                    else if (control is DataBoundLiteralControl)
                    {
                        text = ((DataBoundLiteralControl)control).Text;
                    }
                    else if (control is Literal)
                    {
                        text = ((Literal)control).Text;
                    }
                    else if (control.Controls.Count > 0)
                    {
                        AppendCellContent(control.Controls, sb);
                    }

                    sb.Append(ScriptFilter.FormatCSVContent(text, true));
                }
            }
        }

        /// <summary>
        /// build export content
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="list">the result collection</param>
        /// <returns>the result string</returns>
        private string BuildExprotContent<T>(List<T> list) where T : class, new()
        {
            ArrayList mapping = new ArrayList();

            if (list != null && list.Count > 0)
            {
                T obj = list[0];
                mapping = LoadElementFieldMapping(obj.GetType());
            }

            StringBuilder sb = new StringBuilder();
            int columnsCount = Columns.Count;

            if (IsCheckBoxOrRadioButtonColumn)
            {
                columnsCount++;
            }

            for (int i = 0; i < columnsCount; i++)
            {
                if (Columns[i].Visible)
                {
                    string content = GetHeaderText(i);

                    if (!string.IsNullOrEmpty(content))
                    {
                        sb.Append(content);
                        sb.Append(ACAConstant.CultureInfoSplitChar);
                    }

                    if (mapping != null && mapping.Count > i)
                    {
                        FieldMappingAttribute element = (FieldMappingAttribute)mapping[i];
                        element.Visible = true;
                        element.HeaderText = content;
                    }
                }
            }

            if (sb.Length > 0)
            {
                sb.Length -= 1;
            }

            sb.Append("\r\n");

            for (int row = 0; row < list.Count; row++)
            {
                T item = list[row];

                for (int col = 0; col < mapping.Count; col++)
                {
                    FieldMappingAttribute column = (FieldMappingAttribute)mapping[col];

                    if (column.Visible)
                    {
                        sb.Append(GetContextByElementName<T>(item, column.DataFieldName));
                        sb.Append(ACAConstant.CultureInfoSplitChar);
                    }
                }

                sb.Append("\r\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Create add to collection link.
        /// </summary>
        /// <param name="row2">table row..</param>
        private void CreateAdd2CollectionLink(TableRow row2)
        {
            TableCell cellVerticalLine = new TableCell();
            AccelaLabel lblVerticalLine = new AccelaLabel();
            lblVerticalLine.IsNeedEncode = false;
            lblVerticalLine.CssClass = "tab_tar_separator font11px";
            lblVerticalLine.Text = "|";
            cellVerticalLine.Controls.Add(lblVerticalLine);
            cellVerticalLine.DataBind();
            row2.Cells.Add(cellVerticalLine);

            TableCell cellAddCollection = new TableCell();
            cellAddCollection.Wrap = false;
            row2.Cells.Add(cellAddCollection);
            _addCollectionLinkButton4Show = new AccelaLinkButton();
            _addCollectionLinkButton4Show.ID = this.ID + "top4btnAddCollection";
            _addCollectionLinkButton4Show.Text = LabelConvertUtil.GetTextByKey("mycollection_mypermitspage_label_add", string.Empty);
            _addCollectionLinkButton4Show.CausesValidation = false;
            _addCollectionLinkButton4Show.CssClass = DEFAULT_EMPTY_ROW_CSS;
            _addCollectionLinkButton4Show.Style.Add("cursor", "pointer");
            _addCollectionLinkButton4Show.OnClientClick = "Add2Collection(event,this);return false;";
            cellAddCollection.Controls.Add(_addCollectionLinkButton4Show);
            cellAddCollection.DataBind();
        }

        /// <summary>
        /// Create the short list link button
        /// </summary>
        /// <param name="tableRow">The table row which link button contain.</param>
        private void CreateShortListLink(TableRow tableRow)
        {
            TableCell cellVerticalLine = new TableCell();
            AccelaLabel lblVerticalLine = new AccelaLabel();
            lblVerticalLine.IsNeedEncode = false;
            lblVerticalLine.CssClass = "tab_tar_separator font11px";
            lblVerticalLine.Text = "|";
            cellVerticalLine.Controls.Add(lblVerticalLine);
            cellVerticalLine.DataBind();
            tableRow.Cells.Add(cellVerticalLine);

            TableCell cellAddCollection = new TableCell();
            cellAddCollection.Wrap = false;
            tableRow.Cells.Add(cellAddCollection);
            _addShortListLinkButton4Show = new AccelaLinkButton();
            _addShortListLinkButton4Show.ID = this.ID + "top4btnShortList";

            // set the short list button's text
            if (ShortListType == SHORT_LIST)
            {
                _addShortListLinkButton4Show.Text = LabelConvertUtil.GetTextByKey("aca_certbusinesslist_label_previousresults", string.Empty);
            }
            else
            {
                _addShortListLinkButton4Show.Text = LabelConvertUtil.GetTextByKey("aca_certbusinesslist_label_shortlist", string.Empty);
            }

            _addShortListLinkButton4Show.CausesValidation = false;
            _addShortListLinkButton4Show.CssClass = DEFAULT_EMPTY_ROW_CSS;
            _addShortListLinkButton4Show.Style.Add("cursor", "pointer");

            string shortListClientID = ClientID + "_" + _addShortListLinkButton4Show.ID;
            string hfSaveSelectedItems = GetSelectedItemsFieldClientID();
            _addShortListLinkButton4Show.OnClientClick = string.Format("return triggerShortList('{0}', '{1}', '{2}');", shortListClientID, ClientID, hfSaveSelectedItems);

            cellAddCollection.Controls.Add(_addShortListLinkButton4Show);
            cellAddCollection.DataBind();
        }

        /// <summary>
        /// Create add to shopping Cart link.
        /// </summary>
        /// <param name="row">the GridView row</param>
        private void CreateAddToCartLink(TableRow row)
        {
            TableCell cellVerticalLine = new TableCell();
            AccelaLabel lblVerticalLine = new AccelaLabel();
            lblVerticalLine.IsNeedEncode = false;
            lblVerticalLine.CssClass = "tab_tar_separator font11px";
            lblVerticalLine.Text = "|";
            cellVerticalLine.Controls.Add(lblVerticalLine);
            cellVerticalLine.DataBind();
            row.Cells.Add(cellVerticalLine);

            TableCell cellAddToCart = new TableCell();
            cellAddToCart.Wrap = false;
            row.Cells.Add(cellAddToCart);
            _addToCartLinkButton4Show = new AccelaLinkButton();
            _addToCartLinkButton4Show.ID = this.ID + "top4btnAppendToCart";
            _addToCartLinkButton4Show.Text = LabelConvertUtil.GetTextByKey("per_gridview_label_addtocart", string.Empty);
            _addToCartLinkButton4Show.CausesValidation = false;
            _addToCartLinkButton4Show.CssClass = DEFAULT_EMPTY_ROW_CSS;
            _addToCartLinkButton4Show.Style.Add("cursor", "pointer");
            bool isRightToLeft = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
            _addToCartLinkButton4Show.OnClientClick = "triggleAddToCart(event,'" + btnAppendToCart.ClientID + "','" + isRightToLeft + "',this);return false;";
            cellAddToCart.Controls.Add(_addToCartLinkButton4Show);
            cellAddToCart.DataBind();
        }

        /// <summary>
        /// Create clone record link.
        /// </summary>
        /// <param name="row2">table row..</param>
        private void CreateCloneRecordLink(TableRow row2)
        {
            TableCell cellVerticalLine = new TableCell();
            AccelaLabel lblVerticalLine = new AccelaLabel();
            lblVerticalLine.IsNeedEncode = false;
            lblVerticalLine.CssClass = "tab_tar_separator font11px";
            lblVerticalLine.Text = "|";
            cellVerticalLine.Controls.Add(lblVerticalLine);
            cellVerticalLine.DataBind();
            row2.Cells.Add(cellVerticalLine);

            TableCell cellCloneRecord = new TableCell();
            cellCloneRecord.Wrap = false;
            row2.Cells.Add(cellCloneRecord);
            _addToCloneRecordLinkButton4Show = new AccelaLinkButton();
            _addToCloneRecordLinkButton4Show.ID = this.ID + "top4btnCloneRecord";
            _addToCloneRecordLinkButton4Show.Text = LabelConvertUtil.GetTextByKey("aca_caplist_label_clone_record", string.Empty);
            _addToCloneRecordLinkButton4Show.CausesValidation = false;
            _addToCloneRecordLinkButton4Show.CssClass = DEFAULT_EMPTY_ROW_CSS + " NotShowLoading";
            _addToCloneRecordLinkButton4Show.Style.Add("cursor", "pointer");
            _addToCloneRecordLinkButton4Show.OnClientClick = "triggerCloneRecord('" + btnCloneRecord.ClientID + "',this);return false";
            cellCloneRecord.Controls.Add(_addToCloneRecordLinkButton4Show);
            cellCloneRecord.DataBind();
        }

        /// <summary>
        /// Display sort images
        /// </summary>
        /// <param name="dgItem">grid view row</param>
        /// <param name="index">grid data index</param>
        /// <param name="sortColumns">sort columns</param>
        private void DisplaySortImages(GridViewRow dgItem, int index, string[] sortColumns)
        {
            string sortOrder;
            int sortOrderNo;
            string column = ((LinkButton)dgItem.Cells[index].Controls[0]).CommandArgument;

            SearchSortExpression(sortColumns, column, out sortOrder, out sortOrderNo);

            if (sortOrderNo > 0)
            {
                //string sortImgLoc = (sortOrder.Equals("ASC") ? SortAscImageUrl : SortDescImageUrl);
                string sortImgLoc = sortOrder.Equals("ASC", StringComparison.InvariantCulture) ? SortAscImageUrl : SortDescImageUrl;

                if (sortImgLoc != string.Empty)
                {
                    Image imgSortDirection = new Image();
                    imgSortDirection.ImageUrl = sortImgLoc;
                    dgItem.Cells[index].Controls.Add(imgSortDirection);
                }
                else
                {
                    if (AllowMultiColumnSorting)
                    {
                        Literal litSortSeq = new Literal();
                        litSortSeq.Text = sortOrderNo.ToString();
                        dgItem.Cells[index].Controls.Add(litSortSeq);
                    }
                }
            }
        }

        /// <summary>
        /// Format content
        /// </summary>
        /// <param name="index">grid data index</param>
        /// <param name="needFilterColumnProperty">whether need filter column property or not.</param>
        /// <returns>format content</returns>
        private string GetHeaderText(int index, bool needFilterColumnProperty = true)
        {
            string content = null;
            IAccelaNonInputControl nonInputControl = GetGridHeaderLabel(HeaderRow.Cells[index].Controls);

            if (nonInputControl == null)
            {
                return content;
            }

            if (!string.IsNullOrEmpty(nonInputControl.LabelKey))
            {
                string moduleName = GetModuleName();
                content = LabelConvertUtil.GetTextByKey(nonInputControl.LabelKey, moduleName, this);
            }
            else if (nonInputControl is GridViewHeaderLabel)
            {
                content = ((GridViewHeaderLabel)nonInputControl).Text;
            }
            else if (nonInputControl is AccelaLabel)
            {
                content = ((AccelaLabel)nonInputControl).Text;
            }

            if (needFilterColumnProperty)
            {
                content = FilterCellContent(content, (Control)nonInputControl);
            }

            content = ScriptFilter.FormatCSVContent(content, true);

            return content;
        }

        /// <summary>
        /// Gets the grid header label.
        /// </summary>
        /// <param name="controls">The control collection.</param>
        /// <returns>Return the grid header label.</returns>
        private IAccelaNonInputControl GetGridHeaderLabel(ControlCollection controls)
        {
            if (controls == null || controls.Count == 0)
            {
                return null;
            }

            foreach (Control control in controls)
            {
                if (control is GridViewHeaderLabel || control is AccelaLabel)
                {
                    return (IAccelaNonInputControl)control;
                }

                IAccelaNonInputControl nonInputControl = GetGridHeaderLabel(control.Controls);

                if (nonInputControl != null)
                {
                    return nonInputControl;
                }
            }

            return null;
        }

        /// <summary>
        /// If cell is invisible,return null, other return cell's content.
        /// </summary>
        /// <param name="content">cell content</param>
        /// <param name="control">control in current cell.</param>
        /// <returns>return cell content.</returns>
        private string FilterCellContent(string content, Control control)
        {
            string cellContent = content;
            try
            {
                if (!string.IsNullOrEmpty(content) && ColumnProperties != null && ColumnProperties.Length > 0)
                {
                    string id = control.ID;
                    ColumnProperty cp = null;

                    foreach (ColumnProperty item in ColumnProperties)
                    {
                        if (item.ElementName == id)
                        {
                            cp = item;
                            break;
                        }
                    }

                    //if grid view element is not exist the element. or the element is disabled in grid view element.
                    if ((cp != null && cp.Visible == ACAConstant.INVALID_STATUS) || cp == null)
                    {
                        cellContent = null;
                    }
                }
            }
            catch
            {
                // nothing
            }

            return cellContent;
        }

        /// <summary>
        /// Get selected items value.
        /// </summary>
        /// <returns>selected item value</returns>
        private string GetSelectedItemsValue()
        {
            return Page.Request.Form[GetSelectedItemsFieldUniqueID()];
        }

        /// <summary>
        /// Initialize component
        /// </summary>
        private void InitializeComponent()
        {
            ShowWhenEmpty = true;
            EmptyDataRowStyle.CssClass = string.IsNullOrEmpty(EmptyDataRowStyle.CssClass) ? DEFAULT_EMPTY_ROW_CSS : EmptyDataRowStyle.CssClass;

            // Paging Defaults
            PageSize = GetPageSize();
            PagerStyle.CssClass = string.IsNullOrEmpty(PagerStyle.CssClass) ? DEFAULT_PAGE_STYLE_CSS : PagerStyle.CssClass;
            PagerStyle.VerticalAlign = VerticalAlign.Bottom;
            PagerSettings.Mode = PagerButtons.Numeric;
            HeaderStyle.CssClass = string.IsNullOrEmpty(HeaderStyle.CssClass) ? DEFAULT_HEADER_STYLE_CSS : HeaderStyle.CssClass;
            RowStyle.CssClass = string.IsNullOrEmpty(RowStyle.CssClass) ? DEFAULT_ROW_STYLE_CSS : RowStyle.CssClass;
            AlternatingRowStyle.CssClass = string.IsNullOrEmpty(AlternatingRowStyle.CssClass) ? DEFAULT_ALTERNATING_ROW_STYLE_CSS : AlternatingRowStyle.CssClass;

            Sorting += new GridViewSortEventHandler(AccelaGridView_Sorting);
            PageIndexChanging += new GridViewPageEventHandler(AccelaGridView_PageIndexChanging);
        }

        /// <summary>
        /// Get current page size.
        /// </summary>
        /// <returns>the page size.</returns>
        private int GetPageSize()
        {
            string currentPageSizeString = ControlConfigureProvider.GetPageSize(ModuleName, GridViewNumber);
            int currentPageSize = string.IsNullOrEmpty(currentPageSizeString) ? 0 : Convert.ToInt32(currentPageSizeString);

            if (currentPageSize == 0)
            {
                if (PageSize <= 0)
                {
                    currentPageSize = DEFAULT_PAGE_SIZE;
                }
                else
                {
                    currentPageSize = PageSize;
                }
            }

            return currentPageSize;
        }

        /// <summary>
        /// PreRender checkbox column
        /// </summary>
        private void PreRenderCheckBoxColumn()
        {
            foreach (GridViewRow r in Rows)
            {
                // Get the appropriate style object for the row
                TableItemStyle style = GetRowStyleFromState(r.RowState);

                // Retrieve the reference to the checkbox
                CheckBox cb = (CheckBox)r.FindControl(CHECKBOXID);

                if (cb == null)
                {
                    continue;
                }

                //Add script code to enable single selection
                AppendClientClick(cb, "SingleCheck(this);");

                // Update the style of the checkbox if checked
                if (cb.Checked)
                {
                    r.BackColor = SelectedRowStyle.BackColor;
                    r.ForeColor = SelectedRowStyle.ForeColor;
                    r.Font.Bold = SelectedRowStyle.Font.Bold;
                }
                else
                {
                    r.BackColor = style.BackColor;
                    r.ForeColor = style.ForeColor;
                    r.Font.Bold = style.Font.Bold;
                }
            }
        }

        /// <summary>
        /// pre-render radio button column
        /// </summary>
        private void PreRenderRadioButtonColumn()
        {
            foreach (GridViewRow r in Rows)
            {
                // Get the appropriate style object for the row
                TableItemStyle style = GetRowStyleFromState(r.RowState);

                // Retrieve the reference to the checkbox
                RadioButton radio = (RadioButton)r.FindControl(RADIOBUTTONID);

                if (radio == null)
                {
                    continue;
                }

                //Add script code to enable single selection
                AppendClientClick(radio, "SelectRadioButton(this);");
            }
        }

        /// <summary>
        /// Get context by element name
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="item">the instance of T</param>
        /// <param name="name">the property name</param>
        /// <returns>property value</returns>
        private string GetContextByElementName<T>(T item, string name) where T : class, new()
        {
            string value = string.Empty;

            PropertyInfo[] properties = item.GetType().GetProperties();

            foreach (PropertyInfo pinfo in properties)
            {
                if (pinfo.Name.ToUpper() == name.ToUpper())
                {
                    object obj = pinfo.GetValue(item, null);

                    if (pinfo.PropertyType.Name == "DateTime" || pinfo.PropertyType.FullName.IndexOf("DateTime") > -1)
                    {
                        value = I18nDateTimeUtil.FormatToDateStringForUI(obj);
                    }
                    else if (obj != null)
                    {
                        value = obj.ToString();
                    }

                    break;
                }
            }

            return ScriptFilter.FormatCSVContent(value, false);
        }

        /// <summary>
        /// Get parameter mapping
        /// </summary>
        /// <param name="type">the instance of CAPView4UI or LPView4UI or APOView4UI</param>
        /// <returns>the Parameter Mapping</returns>
        private ArrayList LoadElementFieldMapping(Type type)
        {
            ArrayList dictionary = new ArrayList();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo p in properties)
            {
                object[] keys = p.GetCustomAttributes(typeof(FieldMappingAttribute), true);

                if (keys.Length == 1)
                {
                    FieldMappingAttribute attr = (FieldMappingAttribute)keys[0];
                    attr.DataFieldName = p.Name;
                    dictionary.Add(attr);
                }
            }

            dictionary.Sort(new FieldmappingCompare(this.GlobalSearchType));

            return dictionary;
        }

        /// <summary>
        /// Determines whether [is in global search].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [is in global search]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsInGlobalSearch()
        {
            bool result = !string.IsNullOrEmpty(this.GlobalSearchType);
            return result;
        }

        /// <summary>
        /// Sort column in GridView's row
        /// </summary>
        /// <param name="row">Current GridViewRow</param>
        private void SortColumnHeader(GridViewRow row)
        {
            if (ColumnProperties != null)
            {
                List<int> tempOrder = new List<int>();
                List<TableCell> list = new List<TableCell>();
                foreach (ColumnProperty item in this.ColumnProperties)
                {
                    Control control = row.FindControl(item.ElementName);
                    if (control != null)
                    {
                        TableCell cell = FindTableCell(control);
                        if (cell != null)
                        {
                            int index = row.Cells.GetCellIndex(cell);
                            tempOrder.Add(index);
                            list.Add(cell);
                        }
                    }
                }

                if (list.Count == row.Cells.Count)
                {
                    row.Cells.Clear();
                    row.Cells.AddRange(list.ToArray());
                }
                else
                {
                    int count = row.Cells.Count;
                    TableCell[] fields = new TableCell[count];
                    row.Cells.CopyTo(fields, 0);
                    row.Cells.Clear();
                    int i = 0;
                    while (i < count)
                    {
                        if (tempOrder.IndexOf(i) >= 0)
                        {
                            row.Cells.Add(list[0]);
                            list.RemoveAt(0);
                        }
                        else
                        {
                            row.Cells.Add(fields[i]);
                            tempOrder.Insert(i, i);
                        }

                        i++;
                    }
                }

                _dataRowOrder = tempOrder;
                this.OrderColumns = _dataRowOrder;
            }
        }

        /// <summary>
        /// Sort Data Row
        /// </summary>
        /// <param name="row">GridViewRow row</param>
        private void SortColumnData(GridViewRow row)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (this.OrderColumns != null && this.OrderColumns.Count > 0)
                {
                    TableCell[] fields = new TableCell[row.Cells.Count];
                    row.Cells.CopyTo(fields, 0);
                    row.Cells.Clear();
                    foreach (int item in this.OrderColumns)
                    {
                        row.Cells.Add(fields[item]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the export parameter list.
        /// </summary>
        /// <returns>The export parameter list.</returns>
        private List<ExportParameter> GetExportParameterList()
        {
            if (ColumnProperties == null || ColumnProperties.Count() == 0)
            {
                return null;
            }

            // set the column property dictionary, let it access column property easy by AttributeName
            Dictionary<string, ColumnProperty> dictColumnProperty = new Dictionary<string, ColumnProperty>();
            foreach (ColumnProperty cpItem in ColumnProperties)
            {
                dictColumnProperty.Add(cpItem.ElementName, cpItem);
            }

            // set the columns mapping the real index in the grid view
            Dictionary<int, AccelaTemplateField> columnsMapping = new Dictionary<int, AccelaTemplateField>();
            for (int i = 0; i < HeaderRow.Cells.Count; i++)
            {
                TableCell cell = HeaderRow.Cells[i];
                DataControlFieldHeaderCell headerCell = cell as DataControlFieldHeaderCell;

                if (headerCell == null)
                {
                    continue;
                }

                AccelaTemplateField accelaTemplateField = headerCell.ContainingField as AccelaTemplateField;
                if (accelaTemplateField != null && accelaTemplateField.AttributeName != null && dictColumnProperty.ContainsKey(accelaTemplateField.AttributeName))
                {
                    ColumnProperty cpItem = dictColumnProperty[accelaTemplateField.AttributeName];

                    if (cpItem.Visible == ACAConstant.VALID_STATUS)
                    {
                        columnsMapping.Add(i, accelaTemplateField);
                    }
                }
            }

            // Loop for get the export parameter list
            List<ExportParameter> result = new List<ExportParameter>();

            foreach (KeyValuePair<int, AccelaTemplateField> colMappingItem in columnsMapping)
            {
                int cellIndex = colMappingItem.Key;
                AccelaTemplateField accelaTemplateField = colMappingItem.Value;
                string exportDataField = accelaTemplateField.ExportDataField;
                ExportFormats exportFormat = accelaTemplateField.ExportFormat;

                if (!accelaTemplateField.IsStandard)
                {
                    exportDataField = accelaTemplateField.AttributeName;

                    if (accelaTemplateField.BindDataType == ControlType.Date && exportFormat == ExportFormats.None)
                    {
                        exportFormat = ExportFormats.ShortDate;
                    }
                }

                if (!string.IsNullOrEmpty(exportDataField))
                {
                    ExportParameter exportParameter = new ExportParameter();
                    exportParameter.Header = GetHeaderText(cellIndex, false);
                    exportParameter.ExportDataField = exportDataField;
                    exportParameter.ExportFormat = exportFormat;

                    result.Add(exportParameter);
                }
            }

            return result;
        }

        /// <summary>
        /// Get selected row indexes
        /// </summary>
        /// <param name="onlyCurrentPage">
        /// A value indicating whether to only include the selected row indexes in the current page. 
        /// true - only include the selected row indexes in the current page. 
        /// false - include the selected row indexes in all the pages.
        /// </param>
        /// <returns>selected row indexes</returns>
        private List<int> GetSelectedRowIndexes(bool onlyCurrentPage)
        {
            string selectedValues = GetSelectItems();

            if (string.IsNullOrEmpty(selectedValues))
            {
                return new List<int>();
            }

            string[] selectItems = selectedValues.Split(',');
            Array.Sort(selectItems);
            selectItems = selectItems.ToList().Where(o => o != string.Empty).ToArray();

            List<int> selectedIndices = new List<int>();

            foreach (string selectItem in selectItems)
            {
                int rowIndex = Convert.ToInt32(selectItem.Replace(ROW_INDEX_PREFIX, string.Empty));

                if (onlyCurrentPage)
                {
                    rowIndex = rowIndex - (PageSize * PageIndex);   
                }

                selectedIndices.Add(rowIndex);
            }

            return selectedIndices;
        }

        /// <summary>
        /// Append the specified script to the click of the specified control.
        /// </summary>
        /// <param name="control">The specified control</param>
        /// <param name="appendedScript">The appended script on click</param>
        private void AppendClientClick(WebControl control, string appendedScript)
        {
            if (string.IsNullOrEmpty(appendedScript))
            {
                return;
            }

            if (control != null)
            {
                string clickScript = control.Attributes["onclick"];
                control.Attributes["onclick"] = clickScript + appendedScript;
            }
        }

        /// <summary>
        /// Get a replacement string include method SetNotAskForSPEAR() from the specified <see cref="Match" />.
        /// If it has attribute OnClick, Add method SetNotAskForSPEAR() to this OnClick.
        /// If it only has attribute HREF, Add attribute OnClick include method SetNotAskForSPEAR().
        /// </summary>
        /// <param name="match">match object.</param>
        /// <returns>with no ask value.</returns>
        private string GetReplacement4SetNotAsk(Match match)
        {
            Group group = match.Groups["g1"];

            if (group != null && !string.IsNullOrEmpty(group.Value))
            {
                return match.Value + "SetNotAskForSPEAR();";
            }

            return match.Groups["g2"].Value + "onclick=\"SetNotAskForSPEAR();\" " + match.Groups["g3"].Value;
        }

        #endregion Methods
    }
}
