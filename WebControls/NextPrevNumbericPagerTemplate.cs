#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: NextPrevNumbericPagerTemplate.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:The common gridview control don't provide next&prev pager.The class works as a next&prev pager which Prev, Next and numbers page buttons for custom paging.
 *
 *  Notes:
 * $Id: NextPrevNumbericPagerTemplate.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/10/2007    daly.zeng    Initial.
 * </pre>
 */

#endregion Header

using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Template for next and preview numeric page
    /// </summary>
    public class NextPrevNumericPagerTemplate : ITemplate
    {
        #region Fields

        /// <summary>
        /// export link button
        /// </summary>
        private AccelaLinkButton _exportLinkButton;

        /// <summary>
        /// foot alignment
        /// </summary>
        private HorizontalAlign _footAlign = HorizontalAlign.Center;

        /// <summary>
        /// the count of the pages
        /// </summary>
        private int _pageCount;

        /// <summary>
        /// the index of the current page
        /// </summary>
        private int _pageIndex;

        /// <summary>
        /// the index of the page size.
        /// </summary>
        private int _pageSize;

        /// <summary>
        /// the count of records;
        /// </summary>
        private int _totalCount;

        /// <summary>
        /// show export link or not
        /// </summary>
        private bool _showExportLink;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPrevNumericPagerTemplate"/> class.
        /// </summary>
        /// <param name="pagedDataSource">The paged data source.</param>
        /// <param name="showExportLink">Show export link or not</param>
        /// <param name="footAlign">the alignment of the foot</param>
        public NextPrevNumericPagerTemplate(PagedDataSource pagedDataSource, bool showExportLink, HorizontalAlign footAlign)
        {
            _pageIndex = pagedDataSource.CurrentPageIndex;
            _pageCount = pagedDataSource.PageCount;
            _pageSize = pagedDataSource.PageSize;
            _totalCount = pagedDataSource.DataSourceCount;

            _showExportLink = showExportLink;

            if (footAlign != HorizontalAlign.NotSet)
            {
                _footAlign = footAlign;
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets export link button
        /// </summary>
        public AccelaLinkButton ExportLinkButton
        {
            get
            {
                return _exportLinkButton;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the loading panel
        /// </summary>
        public bool ShowLoadingPanel
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when pager is instantiated in the provided control.
        /// </summary>
        /// <param name="container">Container of controls</param>
        public void InstantiateIn(Control container)
        {
            int pagerStartIndex = StartPageIndex(_pageIndex, _pageCount);
            int pagerEndIndex = EndPageIndex(pagerStartIndex, _pageCount);

            Table table = new Table();
            table.HorizontalAlign = _footAlign;
            table.CssClass = "aca_pagination";
            table.Attributes.Add("role", "presentation");

            TableRow tableRow = new TableRow();

            if (_showExportLink)
            {
                TableCell tc = new TableCell();
                tc.CssClass = "ACA_Hide";
                tc.Controls.Add(CreateExportButton());
                tableRow.Cells.Add(tc);
            }

            TableCell tableCellForPrev = new TableCell();
            tableCellForPrev.CssClass = "aca_pagination_td aca_pagination_PrevNext";
            CreatePrevButton(tableCellForPrev, _pageIndex, _pageCount);
            tableRow.Cells.Add(tableCellForPrev);

            CreateCorrectPageButtons(tableRow, pagerStartIndex, pagerEndIndex);

            TableCell tableCellForNext = new TableCell();
            tableCellForNext.CssClass = "aca_pagination_td aca_pagination_PrevNext";
            CreateNextButton(tableCellForNext, _pageIndex, _pageCount);
            tableRow.Cells.Add(tableCellForNext);

            table.Rows.Add(tableRow);

            container.Controls.Add(table);
        }

        /// <summary>
        /// Create spacer
        /// </summary>
        /// <param name="container">container control</param>
        /// <param name="count">total count</param>
        private static void CreateSpacer(Control container, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Literal spacer = new Literal();
                spacer = new Literal();
                spacer.Text = "&nbsp;";

                container.Controls.Add(spacer);
            }
        }

        /// <summary>
        /// Create next button
        /// </summary>
        /// <param name="container">container control</param>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageCount">total count of all pages</param>
        private void CreateNextButton(Control container, int pageIndex, int pageCount)
        {
            if (pageIndex == pageCount - 1)
            {
                Label lblNext = new Label();
                lblNext.Text = LabelConvertUtil.GetTextByKey("ACA_NextPrevNumbericPagerTemplate_NextText", string.Empty);
                lblNext.CssClass = "aca_simple_text font11px";
                container.Controls.Add(lblNext);
            }
            else
            {
                LinkButton nextButton = new LinkButton();
                nextButton.CausesValidation = false;
                nextButton.CssClass = "aca_simple_text font11px";
                nextButton.CommandName = "Page";
                nextButton.CommandArgument = "Next";
                nextButton.Text = LabelConvertUtil.GetTextByKey("ACA_NextPrevNumbericPagerTemplate_NextText", string.Empty);

                if (this.ShowLoadingPanel)
                {
                    nextButton.OnClientClick = "showLoadingPanel(this);";
                }

                container.Controls.Add(nextButton);
            }
        }

        /// <summary>
        /// Create Preview button
        /// </summary>
        /// <param name="container">container control</param>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageCount">total count of all pages</param>
        private void CreatePrevButton(Control container, int pageIndex, int pageCount)
        {
            if (pageIndex == 0)
            {
                Label lblPrev = new Label();
                lblPrev.Text = LabelConvertUtil.GetTextByKey("ACA_NextPrevNumbericPagerTemplate_PrevText", string.Empty);
                lblPrev.CssClass = "aca_simple_text font11px";
                container.Controls.Add(lblPrev);
            }
            else
            {
                LinkButton prevButton = new LinkButton();
                prevButton.CausesValidation = false;
                prevButton.CssClass = "aca_simple_text font11px";
                prevButton.CommandName = "Page";
                prevButton.CommandArgument = "Prev";
                prevButton.Text = LabelConvertUtil.GetTextByKey("ACA_NextPrevNumbericPagerTemplate_PrevText", string.Empty);

                if (this.ShowLoadingPanel)
                {
                    prevButton.OnClientClick = "showLoadingPanel(this);";
                }
                
                container.Controls.Add(prevButton);
            }
        }

        /// <summary>
        /// Create correct button for page
        /// </summary>
        /// <param name="container">container control that contains the button</param>
        /// <param name="pageIndexOnButton">page index of the button</param>
        private void CreateCorrectPageButton(Control container, int pageIndexOnButton)
        {
            if (_pageIndex == pageIndexOnButton)
            {
                CreateNumericPageLabel(container, pageIndexOnButton);
            }
            else
            {
                CreateNumericPageButton(container, pageIndexOnButton);
            }

            //CreateSpacer(container, 1);
        }

        /// <summary>
        /// Create correct buttons for page
        /// </summary>
        /// <param name="container">container control that contains the buttons</param>
        /// <param name="pagerStartIndex">page start index</param>
        /// <param name="pagerEndIndex">page end index</param>
        private void CreateCorrectPageButtons(Control container, int pagerStartIndex, int pagerEndIndex)
        {
            TableCell tableCell = new TableCell();
            tableCell.CssClass = "aca_pagination_td";

            //Create prev-ellipsis for page.
            if (PageNumber(pagerStartIndex) > 1)
            {
                CreateEllipsisButton(tableCell, pagerStartIndex - 1);
                container.Controls.Add(tableCell);
            }

            for (int i = pagerStartIndex; i <= pagerEndIndex; i++)
            {
                tableCell = new TableCell();
                tableCell.CssClass = "aca_pagination_td";
                CreateCorrectPageButton(tableCell, i);
                container.Controls.Add(tableCell);
            }

            tableCell = new TableCell();
            tableCell.CssClass = "aca_pagination_td";

            //Creates the ... that allows you to jump forward to more pages in the pager.
            if (PageNumber(pagerEndIndex) < _pageCount)
            {
                CreateEllipsisButton(tableCell, pagerEndIndex + 1);
                container.Controls.Add(tableCell);
            }
        }

        /// <summary>
        /// Create ellipsis button for page
        /// </summary>
        /// <param name="container">container control</param>
        /// <param name="goToIndex">go to index</param>
        private void CreateEllipsisButton(Control container, int goToIndex)
        {
            LinkButton pageButton;
            pageButton = new LinkButton();
            pageButton.CausesValidation = false;
            pageButton.Text = "...";
            pageButton.Attributes.Add("title", LabelConvertUtil.GetTextByKey("aca_common_label_showmorepages", string.Empty));
            pageButton.CommandName = "Page";
            pageButton.CommandArgument = PageNumber(goToIndex).ToString();

            if (this.ShowLoadingPanel)
            {
                pageButton.OnClientClick = "showLoadingPanel(this);";
            }

            container.Controls.Add(pageButton);

            CreateSpacer(container, 1);
        }

        /// <summary>
        /// Create export button
        /// </summary>
        /// <returns>export link button</returns>
        private LinkButton CreateExportButton()
        {
            _exportLinkButton = new AccelaLinkButton();
            _exportLinkButton.ID = "lb4btnExport";
            _exportLinkButton.CausesValidation = false;
            _exportLinkButton.CommandName = "Export"; // "Page";
            _exportLinkButton.CommandArgument = "Export"; // "Next";
            _exportLinkButton.Text = string.Empty;
            return _exportLinkButton;
        }

        /// <summary>
        /// Create numeric button for page
        /// </summary>
        /// <param name="container">container control</param>
        /// <param name="pageIndex">current page index.</param>
        private void CreateNumericPageButton(Control container, int pageIndex)
        {
            LinkButton pageButton;
            pageButton = new LinkButton();
            pageButton.CausesValidation = false;
            pageButton.Text = PageNumber(pageIndex).ToString();
            pageButton.CommandName = "Page";
            pageButton.CommandArgument = PageNumber(pageIndex).ToString();

            if (this.ShowLoadingPanel)
            {
                pageButton.OnClientClick = "showLoadingPanel(this);";
            }

            container.Controls.Add(pageButton);
        }

        /// <summary>
        /// Create numeric label for page
        /// </summary>
        /// <param name="container">container control</param>
        /// <param name="pageIndex">current page index.</param>
        private void CreateNumericPageLabel(Control container, int pageIndex)
        {
            Label currentPageLabel;
            currentPageLabel = new Label();
            currentPageLabel.CssClass = "SelectedPageButton font11px";
            currentPageLabel.Text = PageNumber(pageIndex).ToString();
            container.Controls.Add(currentPageLabel);
        }

        /// <summary>
        /// Finds the ending page index to display on the pager. (Will need conversion to page number)
        /// </summary>
        /// <param name="startPageIndex">Start index of the page.</param>
        /// <param name="totalPageCount">total page count</param>
        /// <returns>index of the end page</returns>
        private int EndPageIndex(int startPageIndex, int totalPageCount)
        {
            int endPageIndex = startPageIndex + 9;

            if ((totalPageCount - startPageIndex) <= ACAConstant.DEFAULT_PAGECOUNT)
            {
                endPageIndex = totalPageCount - 1;
            }

            return endPageIndex;
        }

        /// <summary>
        /// Converts a given page number to a page index.
        /// </summary>
        /// <param name="pageNumber">page number.</param>
        /// <returns>page index.</returns>
        private int PageIndex(int pageNumber)
        {
            return pageNumber - 1;
        }

        /// <summary>
        /// Converts a given page index to a page number.
        /// </summary>
        /// <param name="pageIndex">current page index.</param>
        /// <returns>page number.</returns>
        private int PageNumber(int pageIndex)
        {
            return pageIndex + 1;
        }

        /// <summary>
        /// Finds the starting page index to display on the pager. (Will need conversion to page number)
        /// </summary>
        /// <param name="currentPageIndex">index of current page</param>
        /// <param name="totalPageCount">total page count</param>
        /// <returns>index of the start page</returns>
        private int StartPageIndex(int currentPageIndex, int totalPageCount)
        {
            int startingPageToDisplay = (currentPageIndex / ACAConstant.DEFAULT_PAGECOUNT) * ACAConstant.DEFAULT_PAGECOUNT;

            return startingPageToDisplay;
        }

        #endregion Methods
    }
}