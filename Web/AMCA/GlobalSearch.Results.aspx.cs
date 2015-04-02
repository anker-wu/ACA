/**
* <pre>
* 
*  Accela Citizen Access
*  File: GlobalSearch.Results.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  View results of the Global Search query.
* 
*  Notes:
*      $Id: GlobalSearch.Results.aspx.cs 213829 2012-02-16 07:19:27Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-10-2009           Dave Brewster           New page added for version 7.0
* 
* </pre>
*/
using System;
using System.Data;
//using System.Configuration;
//using System.Collections;
using System.Collections.Generic;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.Common;
//using Accela.ACA.Web;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
//using Accela.ACA.BLL.MyCollection;
//using Accela.ACA.WSProxy;
//using Accela.ACA.WSProxy.WSModel;
using Accela.ACA.Web.Common.GlobalSearch;


/// <summary>
/// 
/// </summary>
public partial class GlobalSearchResults : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder PageTitle = new StringBuilder();
    public StringBuilder SearchResultSummary = new StringBuilder();
    public StringBuilder SearchResultLinks = new StringBuilder();
    public StringBuilder ProfessionalsResults = new StringBuilder();
    public StringBuilder PropertyResults = new StringBuilder();
    public StringBuilder NewOption = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("GlobalSearch.Results.aspx");

        //Global Search Related GUI_TEXT entries:
        string per_globalsearch_label_searchresults = LocalGetTextByKey("per_globalsearch_label_searchresults");
        string per_globalsearch_label_conditionnotice = LocalGetTextByKey("per_globalsearch_label_conditionnotice");
        string per_globalsearch_label_apo = LocalGetTextByKey("per_globalsearch_label_apo");
        string per_globalsearch_label_apolink = LocalGetTextByKey("per_globalsearch_label_apolink");
        string per_globalsearch_label_cap = LocalGetTextByKey("per_globalsearch_label_cap");
        string per_globalsearch_label_caplink = LocalGetTextByKey("per_globalsearch_label_caplink");
        string per_globalsearch_label_lp = LocalGetTextByKey("per_globalsearch_label_lp");
        string per_globalsearch_label_lplink = LocalGetTextByKey("per_globalsearch_label_lplink");

        //string globalsearchparcelnavigatelabel = LocalGetTextByKey("globalsearch.parcel.navigate.label");
        //string globalsearchaddressnavigatelabel = LocalGetTextByKey("globalsearch.address.navigate.label");
        // hard code these since I cannot read the values from the GUI_TEXT table.
        string globalsearchparcelnavigatelabel = "Parcels ({0})";
        string globalsearchaddressnavigatelabel = "Addresses ({0})";

        State = GetFieldValue("State", false);

        // HiddenFields.Append(HTML.PresentHiddenField("DisplayNumber", displayNumber));

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        Breadcrumbs = BreadCrumbHelper("GlobalSearch.Results.aspx", Breadcrumbs, per_globalsearch_label_searchresults, breadCrumbIndex, isElipseLink, false, false, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));

        List<CAPView4UI> capList = new List<CAPView4UI>();
        List<LPView4UI> lpList = new List<LPView4UI>();
        List<APOView4UI> apoList = new List<APOView4UI>();
        List<APOView4UI> adrList = new List<APOView4UI>();
        int apoRecCount = 0;
        int adrRecCount = 0;
        GlobalSearchParameter capHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);
        GlobalSearchParameter lpHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.LP);
        GlobalSearchParameter apoHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.PARCEL);
        GlobalSearchParameter adrHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.ADDRESS);

        try
        {
            // throw new Exception("test and exception handeling");

            if (GlobalSearchUtil.IsRecordEnabled())
            {
                capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP,null);
                capHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);
            }
            if (GlobalSearchUtil.IsLPEnabled())
            {

                lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP,null);
                lpHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.LP);
            }
            if (GlobalSearchUtil.IsAPOEnabled())
            {
                apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.PARCEL, apoHistoryParameter.QueryText, apoHistoryParameter.ModuleArray, string.Empty, string.Empty, apoHistoryParameter.PageSize,null);
                apoHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.PARCEL);
                apoRecCount = apoHistoryParameter.TotalRecordsFromWS;

                adrList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, adrHistoryParameter.QueryText, adrHistoryParameter.ModuleArray, string.Empty, string.Empty, adrHistoryParameter.PageSize,null);
                adrHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.ADDRESS);
                adrRecCount = adrHistoryParameter.TotalRecordsFromWS;
            }
        }	
        catch (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
            ErrorMessage.Append("<br>");
            capList = new List<CAPView4UI>();
            lpList = new List<LPView4UI>();
            apoList = new List<APOView4UI>();
            adrList = new List<APOView4UI>();
        }

        iPhonePageTitle = per_globalsearch_label_searchresults;
        if (isiPhone == false)
        {
            PageTitle.Append("<div id=\"pageTitle\">" + iPhonePageTitle + "</div>");
        }

        string searchPattern = Session["AMCA_GlobalSearchPattern"].ToString();
        SearchResultSummary.Append(string.Format(per_globalsearch_label_conditionnotice, searchPattern));
        string linkHTML = PageLinkStyle + "GlobalSearch.List.aspx?State=" + State;

        // Link to CAPS List
        string workStr = string.Empty;
        string workParam = string.Empty;
        int isTopMiddleBottom = 0;
        if (GlobalSearchUtil.IsRecordEnabled())
        {
            workStr = string.Format(
                per_globalsearch_label_caplink, SearchResultUtil.GenerateRecordsSummary(capList.Count, GlobalSearch_Result_Page_Size, 0));
            workParam = string.Empty;
            if (capList != null && capList.Count != 0)
            {
                workParam = "&SearchMode=capList\">";
            }
            if (GlobalSearchUtil.IsLPEnabled() == false && GlobalSearchUtil.IsAPOEnabled() == false)
            {
                isTopMiddleBottom = -1;
            }
            SearchResultLinks.Append(CreateListCell(workStr, isTopMiddleBottom, workParam).ToString());
            isTopMiddleBottom = 1;
        }

        // Link to License Professional List
        if (GlobalSearchUtil.IsLPEnabled())
        {
            workStr = string.Format(per_globalsearch_label_lplink, SearchResultUtil.GenerateRecordsSummary(lpList.Count, GlobalSearch_Result_Page_Size, 0));
            workParam = string.Empty;
            if (lpList != null && lpList.Count != 0)
            {
                workParam = "&SearchMode=lpList\">";
            }
            if (GlobalSearchUtil.IsRecordEnabled() == false && GlobalSearchUtil.IsAPOEnabled() == false)
            {
                isTopMiddleBottom = -1;
            }
            SearchResultLinks.Append(CreateListCell(workStr, isTopMiddleBottom, workParam).ToString());
            isTopMiddleBottom = 1;
        }
        if (GlobalSearchUtil.IsAPOEnabled())
        {
            // Link to Parcel List
            workStr = string.Format(globalsearchparcelnavigatelabel, SearchResultUtil.GenerateRecordsSummary(apoList.Count, GlobalSearch_Result_Page_Size, 0));
            workParam = string.Empty;
            if (apoList != null && apoList.Count != 0)
            {
                workParam = "&SearchMode=apoList\">";
            }
            SearchResultLinks.Append(CreateListCell(workStr, isTopMiddleBottom, workParam).ToString());
            isTopMiddleBottom = 2;
            workStr = string.Format(globalsearchaddressnavigatelabel, SearchResultUtil.GenerateRecordsSummary(adrList.Count, GlobalSearch_Result_Page_Size, 0));
            workParam = string.Empty;
            if (adrList != null && adrList.Count != 0)
            {
                workParam = "&SearchMode=adrList\">";
            }
            SearchResultLinks.Append(CreateListCell(workStr, isTopMiddleBottom, workParam).ToString());
        }
    }

    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="FieldName"> Field name </param>
    /// <param name="IsRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.

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
    /// Create a list cell 
    /// </summary>
    private string CreateListCell(string theText, int isTopMiddleBottom, string searchModeText)
    {
        StringBuilder strWork = new StringBuilder();
        if (isTopMiddleBottom == -1)
        {
            strWork.Append("<div id=\"pageListSingleRow\">");
        }
        else if (isTopMiddleBottom == 0)
        {
            strWork.Append("<div id=\"pageListTop\">");
        }
        else if (isTopMiddleBottom == 1)
        {
            strWork.Append("<div id=\"pageListMiddle\">");
        }
        else if (isTopMiddleBottom == 2)
        {
            strWork.Append("<div id=\"pageListBottom\">");
        }
        if (searchModeText != string.Empty)
        {
            strWork.Append("<a class=\"pageListLinkBold\" href=\"GlobalSearch.List.aspx?State=" + State + searchModeText);
        }
        strWork.Append(theText);
        if (searchModeText != string.Empty)
        {
            if (isiPhone == true)
            {
                strWork.Append("<img style=\"float:right\" src=\"img/chevron.png\" /> ");
            }
            strWork.Append("</a>");
        }
        strWork.Append("</div>");
        return strWork.ToString();
    }
}
