/**
* <pre>
* 
*  Accela Citizen Access
*  File: Conditions.List.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  View conditions for the current record.
* 
*  Notes:
*      $Id: Conditions.List.aspx.cs 218228 2012-04-27 05:45:03Z ACHIEVO\daly.zeng $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  01-10-2011           Dave Brewster           New page added for version 7.0
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

/// <summary>
/// Conditions View
/// </summary>
public partial class ConditionsList : AccelaPage
{

    public StringBuilder PagingFooter = new StringBuilder();
    public StringBuilder PagingFields = new StringBuilder();
    public StringBuilder ResultHeader = new StringBuilder();
    public StringBuilder ResultOutput = new StringBuilder();
    public StringBuilder SortOptions = new StringBuilder();
    public StringBuilder ParcelDetail = new StringBuilder();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();

    private StringBuilder sbWork = new StringBuilder();

    public string ListSortColumn = string.Empty;
    public string ListSortDesc = string.Empty;
    public string DisplayMode = string.Empty;
    public string BackForwardLinks = string.Empty;
    public string PageResultPageNo = string.Empty;
    public string PageBreadcrumbIndex = string.Empty;

    private int CurrentResultPageNumber = 1;
    private string sortOrder = string.Empty;
    private string gsSortColumn = string.Empty;
    private string gsSortDirection = string.Empty;
    private bool sortByID = false;
    private bool sortByStatus = false;
    private bool sortBySeverity = false;
    private bool sortByAppliedDate = false;
    private bool sortByEffectiveDate = false;
    private bool sortByExpireDate = false;
    private bool isSortOrderChange = false;
    private bool sortDescending = false;
    private bool isBreadcrumbPagingMode = false;
    private bool isBreadcrumbRefeshMode = false;

    private bool objectConditionNameVisible = true;
    private bool appliedDateVisible = true;
    private bool effectDateVisible = true;
    private bool expiredDateVisible = true;
    private bool statusVisible = true;
    private bool impactCodeVisible = true;
    private bool areColumnsVisible = false;

    /// <summary>
    /// Conditions list 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("GlobalSearch.List.aspx");

        // default page number
        int ResultCount = 0;
        string moduleSearchPattern = string.Empty;

        //number of records shown per page - from web.config
        AppSettingsReader Settings = new AppSettingsReader();
        int ResultRecordsPerPage = int.Parse(Settings.GetValue("SearchRecordsPerPage", typeof(string)).ToString());

        //get current page number , by default '1'- used while paging
        PageResultPageNo = GetFieldValue("ResultPage", false);
        if (PageResultPageNo == string.Empty)
        {
            PageResultPageNo = GetFieldValue("PageResultPageNo", false);
        }
        if (PageResultPageNo != string.Empty)
        {

            CurrentResultPageNumber = int.Parse(PageResultPageNo) + 1;
        }

        // get search values - used while paging
        PageBreadcrumbIndex = GetFieldValue("PageBreadcrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        if (breadCrumbIndex == string.Empty)
        {
            breadCrumbIndex = PageBreadcrumbIndex;
        }
        isBreadcrumbPagingMode = GetFieldValue("PagingMode", false) == "true";
        bool resetList = GetFieldValue("ReloadList", false) == "true";
        bool clearFilter = GetFieldValue("ClearFilter", false) == "true";

        GetSortOrder(resetList);

        // If sort options have changed reset the list
        if (isSortOrderChange == true)
        {
            CurrentResultPageNumber = 1;
        }

        iPhonePageTitle = "Conditions";
        if (isiPhone == false)
        {
            DisplayMode = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div><hr />";
        }
        DataTable Results = null;
        try
        {
            // get the cap model
            CapIDModel4WS capIdModel = new CapIDModel4WS();
            capIdModel = AppSession.GetCapModelFromSession(ModuleName).capID;
            CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capIdModel, AppSession.User.UserSeqNum, true);

            if (capWithConditionModel.conditionModelArray != null && capWithConditionModel.conditionModelArray.Length > 0)
            {
                Results = ConditionsUtil.GetConditionDataSource(capWithConditionModel.conditionModelArray);
                DataView dataView = new DataView(Results);
                dataView.Sort = gsSortColumn + " " + gsSortDirection;
                Results = dataView.ToTable();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
            ErrorMessage.Append("<br>");
        }
        
        getColumnsVisibilitySettings();

        sbWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
        sbWork.Append("&Module=" + ModuleName);

        Breadcrumbs = BreadCrumbHelper("Conditions.List.aspx", sbWork, iPhonePageTitle, breadCrumbIndex, isElipseLink, isBreadcrumbRefeshMode, isBreadcrumbPagingMode, false);
        PageBreadcrumbIndex = CurrentBreadCrumbIndex.ToString();
        BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());
        if (Results != null)
        {
            ResultCount = Results.Rows.Count;
        }
        int ResultPageStart = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
        // Populate Records 
        if (ResultCount > 0 && areColumnsVisible == false)
        {
            ResultOutput.Append("<p>You do not have permission to view condition details.  Please contact your system administrator for more details.</p>");
        }
        else if (ResultCount == 0)
        {
            ResultOutput.Append("<p>There are no conditions attached to this record.</p>");
        }
        else if (ResultCount > 0 && areColumnsVisible == true)
        {
            int FirstRecord = ResultPageStart + 1;
            int LastRecord = ResultCount;
            if (ResultCount >= ResultRecordsPerPage)
            {
                LastRecord = (FirstRecord - 1) + ResultRecordsPerPage;
                if (LastRecord > ResultCount)
                {
                    LastRecord = ResultCount;
                }
            }
            ResultHeader.Append("<div style=\"margin-top:5px; margin-bottom:5px; \">" + "Showing ");
            ResultHeader.Append(FirstRecord.ToString() + "-");
            ResultHeader.Append(LastRecord.ToString() + " of ");
            ResultHeader.Append(ResultCount.ToString());
            ResultHeader.Append("</div>");

            int index = 0;
            int rowNumber = 0;

            foreach (DataRowView aRow in Results.DefaultView)
            {
                if (index >= ResultPageStart)
                {
                    rowNumber++;

                    string aLink = BuildConditionRow(rowNumber, aRow);
                    ResultOutput.Append(MyProxy.CreateSelectListCell(aLink, rowNumber - 1, index, ResultCount, ResultPageStart, ResultRecordsPerPage, isiPhone, false));

                    if (rowNumber == ResultRecordsPerPage)
                    {
                        break;
                    }
                }
                index++;
            }
                        
            string sortDirectionIcon = "img/Caret_up_sml.gif";
            if (sortDescending == true)
            {
                sortDirectionIcon = "img/Caret_down_sml.gif";
            }
            string sortDirectionHTML = "<img src=\"" + sortDirectionIcon + "\"/>";
            string inputEndHTML = "|";
            inputEndHTML = "&#124;";

            List<string> tableRows = new List<string>();
            StringBuilder cellWork = new StringBuilder();
            if (objectConditionNameVisible)
            {
                cellWork.Append("<td>");
                cellWork.Append("<input type=\"submit\"  name=\"sortName\" value=\"Condition\">");
                if (sortByID == true)
                {
                    cellWork.Append(sortDirectionHTML);
                }
                cellWork.Append("</td>");
                tableRows.Add(cellWork.ToString());
            }
            
            cellWork = new StringBuilder();
            if (statusVisible)
            {
                cellWork.Append("<td>");
                cellWork.Append("<input class=\"sortSectionInput\" type=\"submit\"  name=\"sortStatus\" value=\"Status\">");
                if (sortByStatus == true)
                {
                    cellWork.Append(sortDirectionHTML);
                }
                cellWork.Append("</td>");
                tableRows.Add(cellWork.ToString());
            }

            cellWork = new StringBuilder();
            if (impactCodeVisible)
            {
                cellWork.Append("<td>");
                cellWork.Append("<input class=\"sortSectionInput\" type=\"submit\"  name=\"sortSeverity\" value=\"Severity\">");
                if (sortBySeverity == true)
                {
                    cellWork.Append(sortDirectionHTML);
                }
                cellWork.Append("</td>");
                tableRows.Add(cellWork.ToString());
            }
            cellWork = new StringBuilder();
            if (appliedDateVisible)
            {
                cellWork.Append("<td>");
                cellWork.Append("<input class=\"sortSectionInput\" type=\"submit\"  name=\"sortAppliedDate\" value=\"Applied Date\">");
                if (sortByAppliedDate == true)
                {
                    cellWork.Append(sortDirectionHTML);
                }
                cellWork.Append("</td>");
                tableRows.Add(cellWork.ToString());
            }
            cellWork = new StringBuilder();
            if (effectDateVisible)
            {
                cellWork.Append("<td>");
                cellWork.Append("<input class=\"sortSectionInput\" type=\"submit\"  name=\"sortEffectiveDate\" value=\"Effective Date\">");
                if (sortByEffectiveDate == true)
                {
                    cellWork.Append(sortDirectionHTML);
                }
                cellWork.Append("</td>");
                tableRows.Add(cellWork.ToString());
            }
            cellWork = new StringBuilder();
            if (expiredDateVisible)
            {
                cellWork.Append("<td>");
                cellWork.Append("<input class=\"sortSectionInput\" type=\"submit\"  name=\"sortExpireDate\" value=\"Expiration Date\">");
                if (sortByExpireDate == true)
                {
                    cellWork.Append(sortDirectionHTML);
                }
                cellWork.Append("</td>");

                tableRows.Add(cellWork.ToString());
            }

            SortOptions.Append("Sort By:");
            SortOptions.Append("<div id=\"sortSection\">");
 
            SortOptions.Append("<table width=\"100%\" cellspacing=\"0px\" cellpadding=\"0px\" border=\"0px\" >");

            //Row one
            SortOptions.Append("<tr>");
            SortOptions.Append(tableRows[0].ToString());

            SortOptions.Append("<td>");
            if (tableRows.Count > 1)
            {
                SortOptions.Append(inputEndHTML);
            }
            SortOptions.Append("</td>");

            if (tableRows.Count > 1)
            {
                SortOptions.Append(tableRows[1].ToString());
            }

            SortOptions.Append("<td>");
            if (tableRows.Count > 1)
            {
                SortOptions.Append(inputEndHTML);
            }
            SortOptions.Append("</td>");

            if (tableRows.Count > 2)
            {
                SortOptions.Append(tableRows[2].ToString());
            }
            SortOptions.Append("</tr>");

            //Row two
            if (tableRows.Count > 3)
            {
                SortOptions.Append("<tr>");

                SortOptions.Append(tableRows[3].ToString());


                SortOptions.Append("<td>");
                if (tableRows.Count > 4)
                {
                    SortOptions.Append(inputEndHTML);
                }
                SortOptions.Append("</td>");

                if (tableRows.Count > 4)
                {
                    SortOptions.Append(tableRows[4].ToString());
                }
                SortOptions.Append("<td>");
                if (tableRows.Count > 5)
                {
                    SortOptions.Append(inputEndHTML);
                }
                SortOptions.Append("</td>");
                if (tableRows.Count > 5)
                {
                    SortOptions.Append(tableRows[5].ToString());
                }
            
                SortOptions.Append("</tr>");
            }
            SortOptions.Append("</table>");
            SortOptions.Append("</div>");

            
            #region Paging Part
            // Paging Part, appends to table footer
            if ((double)ResultCount > (double)ResultRecordsPerPage)
            {
                int ResultPagesCount = (int)Math.Ceiling((double)ResultCount / (double)ResultRecordsPerPage);
                if ((int)Math.Ceiling((double)ResultPagesCount * (double)ResultRecordsPerPage) < ResultCount)
                {
                    ResultPagesCount++;
                }
                int NextPageNumber = 0;
                int PageCount = 0;
                int PageStart = 0;
                int PreviousPageNumber = CurrentResultPageNumber - 1;
                if (CurrentResultPageNumber < 6)
                {
                    PageStart = 1;
                }
                else
                {
                    PageStart = CurrentResultPageNumber - 5;
                }

                string sLinkFormat = "<a id=\"pageNavigationButton\" href=\"";
                StringBuilder PagingLink = new StringBuilder();

                PagingLink.Append(sLinkFormat + "Conditions.List.aspx?State=" + State);
                PagingLink.Append("&PagingMode=true");
                PagingLink.Append("&Module=" + ModuleName);
                PagingLink.Append("&SortColumn=" + SortColumn.ToString());
                PagingLink.Append("&SortDesc=" + SortDesc.ToString());

                PagingFooter.Append("<div id=\"pageNavigation\">");
                string delimiter = string.Empty;
                if (isiPhone == true)
                {
                    PagingFooter.Append("<center>");
                }
                if (PreviousPageNumber > 0)
                {
                    PagingFooter.Append(PagingLink.ToString());
                    PagingFooter.Append("&SlidePage=LeftToRight");
                    PagingFooter.Append("&ResultPage=" + (PreviousPageNumber - 1).ToString());
                    PagingFooter.Append("\">&lt;</a>");
                    delimiter = " | ";
                }
                for (int page = PageStart; page <= ResultPagesCount; page++)
                {
                    PageCount++;
                    if (PageCount > 10)
                    {
                        break;
                    }
                    PagingFooter.Append(delimiter);
                    if (CurrentResultPageNumber == page)
                    {
                        PagingFooter.Append("<span id=\"pageNavigationSelected\">" + page + "</span>");
                        NextPageNumber = page + 1;
                        delimiter = " | ";
                    }
                    else
                    {
                        PagingFooter.Append(PagingLink.ToString());
                        if (CurrentResultPageNumber > page)
                        {
                            PagingFooter.Append("&SlidePage=LeftToRight");
                        }
                        PagingFooter.Append("&ResultPage=" + (page - 1).ToString());
                        PagingFooter.Append("\">");
                        PagingFooter.Append(page.ToString());
                        PagingFooter.Append("</a>");
                        delimiter = " | ";
                    }
                }
                if (NextPageNumber <= ResultPagesCount)
                {
                    PagingFooter.Append(delimiter);
                    PagingFooter.Append(PagingLink.ToString());
                    PagingFooter.Append("&ResultPage=" + (NextPageNumber - 1).ToString());
                    PagingFooter.Append("\">&gt;</a>");
                }
                PagingFooter.Append("</div>");
                if (isiPhone == true)
                {
                    PagingFooter.Append("</center>");
                }
            }
            #endregion
        }
    }
 #region  Misc Subroutines

    /// <summary>
    /// Set page sort order.
    /// </summary>
    /// <param name="restList"> resetList </param>
    /// <returns>void</returns>
    private void GetSortOrder(bool resetList)
    {
        string sortColumn = string.Empty;
        string sortDESC = string.Empty;
        string prevSortColumn = GetFieldValue("SortColumn", false);
        string prevSortDESC = GetFieldValue("SortDesc", false);

        gsSortDirection = "";
        if (resetList == false)
        {
            if (GetFieldValue("sortName", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                sortColumn = "objectConditionName";
                gsSortColumn = "objectConditionName";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByID = true;
                sortOrder = sortColumn + sortDESC;
            }
            else if (GetFieldValue("sortStatus", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                sortColumn = "conditionStatus";
                gsSortColumn = "conditionStatus";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByStatus = true;
                sortOrder = sortColumn + sortDESC;
            }
            else if (GetFieldValue("sortSeverity", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                sortColumn = "impactCode";
                gsSortColumn = "impactCode";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortBySeverity = true;
                sortOrder = sortColumn + sortDESC;
            }
            else if (GetFieldValue("sortAppliedDate", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                sortColumn = "issuedDate";
                gsSortColumn = "issuedDate";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByAppliedDate = true;
                sortOrder = sortColumn + sortDESC;
            }
            else if (GetFieldValue("sortEffectiveDate", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                sortColumn = "effectDate";
                gsSortColumn = "effectDate";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByEffectiveDate = true;
                sortOrder = sortColumn + sortDESC;
            }
            else if (GetFieldValue("sortExpireDate", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                sortColumn = "expireDate";
                gsSortColumn = "expireDate";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByExpireDate = true;
                sortOrder = sortColumn + sortDESC;
            }
            // If not sort column is available then set to default or previous sort column.
            if (sortColumn == string.Empty)
            {
                if (prevSortColumn != string.Empty)
                {
                    sortColumn = prevSortColumn;
                    gsSortColumn = sortColumn;
                    if (sortColumn == "objectConditionName")
                    {
                        sortByID = true;
                    }
                    else if (sortColumn == "AltID")
                    {
                        sortByID = true;
                        gsSortColumn = "PermitNumber";
                    }
                    else if (sortColumn == "conditionStatus")
                    {
                        sortByStatus = true;
                    }
                    else if (sortColumn == "impactCode")
                    {
                        sortBySeverity = true;
                    }
                    else if (sortColumn == "issuedDate")
                    {
                        sortByAppliedDate = true;
                    }
                    else if (sortColumn == "effectDate")
                    {
                        sortByEffectiveDate = true;
                    }
                    else if (sortColumn == "expireDate")
                    {
                        sortByExpireDate = true;
                    }
                    else
                    {
                        sortColumn = string.Empty;
                    }
                    sortDESC = prevSortDESC;
                }
                else
                {
                    sortColumn = "issuedDate";
                    gsSortColumn = "issuedDate";
                    sortDESC = " DESC";
                    sortByAppliedDate = true;
                }
                sortOrder = sortColumn + sortDESC;
            }
        }
        else
        {
            sortColumn = "issuedDate";
            gsSortColumn = "issuedDate";
            sortDESC = " DESC";
            sortByAppliedDate = true;
            sortOrder = sortColumn + sortDESC; 
        }

        // Update Sort Order Session variables
        sortDescending = sortDESC == " DESC";
        if (sortDescending)
        {
            gsSortDirection = ACAConstant.ORDER_BY_DESC;
        }
        else
        {
            gsSortDirection = ACAConstant.ORDER_BY_ASC;
        }
        SortColumn = sortColumn;
        SortDesc = sortDESC;
        ListSortColumn = sortColumn;
        ListSortDesc = sortDESC;
        HiddenFields.Append(HTML.PresentHiddenField("SortColumn", sortColumn));
        HiddenFields.Append(HTML.PresentHiddenField("SortDesc", sortDESC));
    }
    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="FieldName"> Field name </param>
    /// <param name="IsRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
    private string GetFieldValue(string FieldName, bool IsRequired)
    {
        string TheValue = string.Empty;
        try
        {
            TheValue = (Request.QueryString[FieldName] != null)
                   ? Request.QueryString[FieldName] : ((Request[FieldName] != null)
                   ? Request.Form[FieldName].ToString() : string.Empty);
        }
        catch
        {
            TheValue = string.Empty;
        }
        if (IsRequired == true && TheValue.ToString() == "")
        {
            // IsRequiredDataValid = false;
        }
        return TheValue;
    }

    /// <summary>
    /// Format a condition for presentation in the list.
    /// </summary>
    /// <param name="rowNumber"> rowNumber </param>
    /// <param name="condition"> a DataRowView to be formatted</param>
    /// <returns>String</returns>
    private string BuildConditionRow(int rowNumber, DataRowView condition)
    {
        StringBuilder aLink = new StringBuilder();

        aLink.Append("<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
        if (objectConditionNameVisible == true || statusVisible == true || impactCodeVisible == true)
        {
            aLink.Append("<tr>");
            aLink.Append("<td colspan=\"2\">");
            if (objectConditionNameVisible == true || statusVisible == true || impactCodeVisible == true)
            {
                aLink.Append("<b>");
                aLink.Append(condition["objectConditionName"].ToString());
                aLink.Append("</b>");
                if (statusVisible == true || impactCodeVisible == true)
                {
                    aLink.Append(" - ");
                }
            }
            if (statusVisible == true)
            {
                aLink.Append(condition["conditionStatus"].ToString());
                if (impactCodeVisible == true)
                {
                    aLink.Append(" - ");
                }
            }
            if (impactCodeVisible == true)
            {
                aLink.Append(condition["impactCode"].ToString());
            }
            aLink.Append("</td>");
            aLink.Append("</tr>");
        }
        if (appliedDateVisible && condition["issuedDate"] != null && condition["issuedDate"].ToString().Length > 0)
        {
            aLink.Append("<tr>");
            aLink.Append("<td width=\"35%\" class=\"pageListText\">");
            aLink.Append("Applied Date: ");
            aLink.Append("</td>");
            aLink.Append("<td class=\"pageListText\">");
            aLink.Append(I18nDateTimeUtil.FormatToDateStringForUI(condition["issuedDate"]));
            aLink.Append("</td>");
            aLink.Append("</tr>");
        }
        if (effectDateVisible && condition["effectDate"] != null && condition["effectDate"].ToString().Length > 0)
        {
            aLink.Append("<tr>");
            aLink.Append("<td class=\"pageListText\">");
            aLink.Append("Effective Date: ");
            aLink.Append("</td>");
            aLink.Append("<td class=\"pageListText\">");
            aLink.Append(I18nDateTimeUtil.FormatToDateStringForUI(condition["effectDate"]));
            aLink.Append("</td>");
            aLink.Append("</tr>");
        }
        if (expiredDateVisible && condition["expireDate"] != null && condition["expireDate"].ToString().Length > 0)
        {
            aLink.Append("<tr>");
            aLink.Append("<td class=\"pageListText\">");
            aLink.Append("Expiration Date: ");
            aLink.Append("</td>");
            aLink.Append("<td class=\"pageListText\">");
            aLink.Append(I18nDateTimeUtil.FormatToDateStringForUI(condition["expireDate"]));
            aLink.Append("</td>");
            aLink.Append("</tr>");
        }
        if (condition["conditionDescription"] != null && condition["conditionDescription"].ToString().Length > 0)
        {
            aLink.Append("<tr>");
            aLink.Append("<td colspan=\"2\" class=\"pageListText\">");
            aLink.Append("Comment: ");
            aLink.Append(condition["conditionDescription"].ToString());
            aLink.Append("</td>");
            aLink.Append("</tr>");
        }
        aLink.Append("</table>");
        if (isiPhone == false)
        {
            aLink.Append("<br>");
        }
        return aLink.ToString();
    }

    /// <summary>
    /// Read the ACA Admin's visibility settings the record conditions list
    /// </summary>
    private void getColumnsVisibilitySettings()
    {
        try
        {
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(ModuleName, "60102");

            objectConditionNameVisible = IsFieldVisible(models, "lnkNameHeader");
            appliedDateVisible = IsFieldVisible(models, "lnkAppliedDateHeader");
            effectDateVisible = IsFieldVisible(models, "lnkEffectiveDateHeader");
            expiredDateVisible = IsFieldVisible(models, "lnkExpirationDateHeader");
            statusVisible = IsFieldVisible(models, "lnkStatusHeader");
            impactCodeVisible = IsFieldVisible(models, "lnkSeverityHeader");
        }
        catch (Exception ex)
        {
            //do nothing for now until we work out the issues
        }

        areColumnsVisible = objectConditionNameVisible || statusVisible 
            || impactCodeVisible || appliedDateVisible || effectDateVisible 
            || expiredDateVisible; 
    }

    /// <summary>
    /// get field visiblity
    /// </summary>
    /// <param name="models">simple view element modles</param>
    /// <param name="fieldName">field name</param>
    /// <returns>true if field is visible,otherwise,it's false.</returns>
    public static bool IsFieldVisible(SimpleViewElementModel4WS[] models, string fieldName)
    {
        if (models == null ||
            models.Length == 0)
        {
            return true;
        }

        foreach (SimpleViewElementModel4WS model in models)
        {
            if (model.viewElementName == fieldName)
            {
                return model.recStatus == ACAConstant.VALID_STATUS;
            }
        }

        return true;
    }
    #endregion
}
