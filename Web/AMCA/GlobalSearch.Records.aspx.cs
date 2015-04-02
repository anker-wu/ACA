/**
* <pre>
* 
*  Accela Citizen Access
*  File: GlobalSearch.Records.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2009
* 
*  Description:
*  View Global Search results list of module names and record counts.
* 
*  Notes:
*      $Id: GlobalSearch.Records.aspx.cs 153353 2009-10-28 00:01:52Z dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-17-2009           Dave Brewster           New page added for version 7.0
* 
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using System.Collections.Generic;
using Accela.ACA.Web.Common.GlobalSearch;


/// <summary>
/// 
/// </summary>
public partial class GlobalSearchRecords : AccelaPage
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
    private string currentModuleName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("GlobalSearch.Records.aspx");

        //Global Search Related GUI_TEXT entries:
        string per_globalsearch_label_searchresults = LocalGetTextByKey("per_globalsearch_label_searchresults");
        string per_globalsearch_label_conditionnotice = LocalGetTextByKey("per_globalsearch_label_conditionnotice");

        string per_globalsearch_label_apo = LocalGetTextByKey("per_globalsearch_label_apo");
        string per_globalsearch_label_apolink = LocalGetTextByKey("per_globalsearch_label_apolink");
        string per_globalsearch_label_cap = LocalGetTextByKey("per_globalsearch_label_cap");
        string per_globalsearch_label_caplink = LocalGetTextByKey("per_globalsearch_label_caplink");
        string per_globalsearch_label_lp = LocalGetTextByKey("per_globalsearch_label_lp");
        string per_globalsearch_label_lplink = LocalGetTextByKey("per_globalsearch_label_lplink");

        State = GetFieldValue("State", false);
        string SearchMode = GetFieldValue("SearchMode", false);
        string ResultPage = GetFieldValue("ResultPage", false);
        string DisplayMode = GetFieldValue("DisplayMode", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&SearchMode=" + SearchMode);
        sbWork.Append("&ResultPage=" + ResultPage);
        sbWork.Append("&Module=" + ModuleName);
        Breadcrumbs = BreadCrumbHelper("GlobalSearch.Records.aspx", sbWork, per_globalsearch_label_searchresults, null, isElipseLink, false, false, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));

        List<CAPView4UI> capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, "createdDate", "DESC");

        List<string> moduleList = new List<string>();
        List<string> moduleSort = new List<string>();
        List<int> moduleCnt = new List<int>();

        for (int i = 0; i < capList.Count; i++)
        {
            if (!moduleList.Contains(capList[i].ModuleName))
            {
                moduleList.Add(capList[i].ModuleName);
                moduleSort.Add(capList[i].ModuleName);
                moduleCnt.Add(1);
            }
            else
            {
                currentModuleName = capList[i].ModuleName;
                int cntIndex = moduleList.FindIndex(ThisModuleName);
                moduleCnt[cntIndex]++;
            }
        }

        PageTitle.Append("Filter " + per_globalsearch_label_cap);

        SearchResultSummary.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
        SearchResultSummary.Append("<tr><td>");
        SearchResultSummary.Append("Select the record type that you would like to filter your results by:");
        SearchResultSummary.Append("</td></tr>");
        SearchResultSummary.Append("</table>");

        SearchResultLinks.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
        moduleSort.Sort();
        for (int i = 0; i < moduleSort.Count; i++)
        {
            currentModuleName = moduleSort[i];
            int capIndex = moduleList.FindIndex(ThisModuleName);
            SearchResultLinks.Append("<tr><td>");
            SearchResultLinks.Append(ListLinkStyle + "GlobalSearch.List.aspx?State=" + State);
            SearchResultLinks.Append("&PageBreadcrumbIndex=-1"); //+ CurrentBreadCrumbIndex);
            SearchResultLinks.Append("&PagingMode=true");
            SearchResultLinks.Append("&SearchMode=" + SearchMode);
            SearchResultLinks.Append("&Module=" + moduleList[capIndex] + "\">");
            SearchResultLinks.Append(moduleList[capIndex] + "(" + moduleCnt[capIndex].ToString() + ")");
            SearchResultLinks.Append("</a>");
            SearchResultLinks.Append("</td></tr>");
        }
        SearchResultLinks.Append("</table>");

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
    /// Get ACA GUI_TEXT values use on pages.
    /// </summary>
    private string LocalGetTextByKey(string key)
    {
        string textValue = LabelUtil.GetTextByKey(key, ModuleName);
        try
        {
            return textValue.Replace("\n", "<br>");
        }
        catch
        {
            return textValue;
        }

    }
    // Search predicate returns true if a string ends in "saurus".
    private bool ThisModuleName(string s)
    {
        if (s == currentModuleName.ToString()) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
