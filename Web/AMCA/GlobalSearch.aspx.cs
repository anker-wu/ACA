/**
* <pre>
* 
*  Accela Citizen Access
*  File:GlobalSearch.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2011
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: GlobalSearch.aspx.cs 201313 2011-08-10 08:16:38Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-21-2009           Dave Brewster           New page added for version 7.0
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
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.GlobalSearch;


public partial class GlobalSearch : AccelaPage
{
    #region Fields

    /// <summary>
    /// copy cap info event name.
    /// </summary>
    private const string IS_NEW_QUERY = "isNewQuery";

    /// <summary>
    /// First column name of CAP grid view
    /// </summary>
    private const string CAP_FIRST_COLUMN_NAME = "createdDate";

    /// <summary>
    /// First column name of LP grid view
    /// </summary>
    private const string LP_FIRST_COLUMN_NAME = "licenseNumber";

    /// <summary>
    /// First column name of APO Address grid view
    /// </summary>
    private const string ADDRESS_FIRST_COLUMN_NAME = "fullAddress";

    /// <summary>
    /// First column name of APO Parcel grid view
    /// </summary>
    private const string PARCEL_FIRST_COLUMN_NAME = "parcelNumber";

    #endregion Fields
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder SearchSection = new StringBuilder();
    public StringBuilder SearchMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();

    public string SearchMode = string.Empty;
    public string PageTitle = string.Empty;
    public string CurrentModule = string.Empty;
    string globalSearchPattern = string.Empty;
    public string aChar = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("GlobalSearch.aspx");
        PageTitle = "Global Search";
        iPhonePageTitle = PageTitle;

        State = Request.QueryString["State"].ToString();

        bool invalidSearchPattern = (GetFieldValue("InvalidSearchPattern", false) == "true");
        bool notReadyYet = (GetFieldValue("NotReadyYet", false) == "true");
        string aChar = char.ConvertFromUtf32(171);
 
        int resultSets = 0;
        if (GetFieldValue("UpdateSession", false) != string.Empty)
        {
            string queryText = string.Empty;
            globalSearchPattern = GetFieldValue("GlobalSearchPattern", false) == string.Empty ? string.Empty : GetFieldValue("GlobalSearchPattern", false);
            if (globalSearchPattern != string.Empty && globalSearchPattern.Length > 2)
            {
                List<CAPView4UI> capList = new List<CAPView4UI>();
                List<LPView4UI> lpList = new List<LPView4UI>();
                List<APOView4UI> apoList = new List<APOView4UI>();
                List<APOView4UI> adrList = new List<APOView4UI>();
                if (Session["AMCA_GlobalSearchPattern"] != null)
                {
                    if (Session["AMCA_GlobalSearchPattern"].ToString() != globalSearchPattern.ToString())
                    {
                        GlobalSearchManager.ClearHistoryAction();
                    }
                }
                Session["AMCA_GlobalSearchPattern"] = globalSearchPattern.ToString();

                queryText = globalSearchPattern.ToString();
                bool isLoadingHistory = CanLoadHistory(queryText);


                try
                {
                    if (!isLoadingHistory)
                    {
                        string[] modules = GlobalSearchUtil.GetAllModuleKeys();
                        // Get query text from search input box
                        capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, queryText, modules, CAP_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_DESC, ACAConstant.DEFAULT_PAGESIZE,null);
                        lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP, queryText, modules, LP_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC, ACAConstant.DEFAULT_PAGESIZE,null);
                        apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.PARCEL, queryText, modules, PARCEL_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC, ACAConstant.DEFAULT_PAGESIZE,null);
                        adrList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, queryText, modules, ADDRESS_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC, ACAConstant.DEFAULT_PAGESIZE,null);
                    }
                    else
                    {
                        // Get query conditions from last history action
                        if (GlobalSearchUtil.IsRecordEnabled())
                        {
                            capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP,null);
                        }
                        if (GlobalSearchUtil.IsLPEnabled())
                        {
                            lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP,null);
                        }
                        if (GlobalSearchUtil.IsAPOEnabled())
                        {
                            GlobalSearchParameter apoHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.PARCEL);
                            apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.PARCEL, apoHistoryParameter.QueryText, apoHistoryParameter.ModuleArray, string.Empty, string.Empty, apoHistoryParameter.PageSize,null);
                            GlobalSearchParameter adrHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.ADDRESS);
                            adrList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, adrHistoryParameter.QueryText, adrHistoryParameter.ModuleArray, string.Empty, string.Empty, adrHistoryParameter.PageSize,null);
                        }
                    }
                    resultSets = capList != null ? capList.Count : 0;
                    resultSets += lpList != null ? lpList.Count : 0;
                    resultSets += apoList != null ? apoList.Count : 0;
                    resultSets += adrList != null ? adrList.Count : 0;
                    string per_globalsearch_label_noresultsnotice = LocalGetTextByKey("per_globalsearch_label_noresultsnotice");
                    if (resultSets == 0)
                    {
                        if (per_globalsearch_label_noresultsnotice != string.Empty)
                        {
                            SearchMessage.Append(string.Format(per_globalsearch_label_noresultsnotice, globalSearchPattern.ToString()));
                        }
                        else
                        {
                            SearchMessage.Append("No records were found for the character pattern that you entered.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append(ex.Message);
                    ErrorMessage.Append(ErrorFormatEnd);
                    ErrorMessage.Append("<br>");
                    capList = null;
                    lpList = null;
                    apoList = null;
                    adrList = null;
                }
            }
            else
            {
                string per_globalsearch_message_searchhint = LocalGetTextByKey("per_globalsearch_message_searchhint");
                SearchMessage.Append("<table cellpadding=\"1\" cellspacing=\"0\" width=\"95%\"><tr><td>");
                if (per_globalsearch_message_searchhint != string.Empty)
                {
                    SearchMessage.Append(string.Format(per_globalsearch_message_searchhint, globalSearchPattern.ToString()));
                }
                else
                {
                    SearchMessage.Append("No records were found for the character pattern that you entered.");
                }
                SearchMessage.Append("</td></tr></table><br>");
            }
            if (resultSets != 0)
            {
                string globalSearchQueries = queryText;
                if (Session["AMCA_GlobalSearchQueries"] != null)
                {
                    string previouSearchQueries = Session["AMCA_GlobalSearchQueries"].ToString();
                    Session.Remove("AMCA_GlobalSearchQueries");
                    string[] history = previouSearchQueries.Split('|');
                    foreach (string qValue in history)
                    {
                        if (qValue == queryText)
                            continue;
                        if (qValue == "<end>")
                        {
                            continue;
                        }
                        globalSearchQueries += ("|" + qValue);
                    }
                }
                globalSearchQueries += ("|<end>");
                Session["AMCA_GlobalSearchQueries"] = globalSearchQueries;
                Session["AMCA_GlobalSearch_Mode"] = "true";
                Response.Redirect("GlobalSearch.Results.aspx?State=" + State
                   + "&Mode=" + SearchMode
                   + "&Module=" + ModuleName
                   + "&ResetList=true"
                   + "&GlobalSearch=true");
            }
        }


        string per_globalsearch_label_search = "Search";
        globalSearchPattern = Session["AMCA_globalSearchPattern"] == null ? string.Empty : Session["AMCA_globalSearchPattern"].ToString();

        SearchSection.Append("<div id=\"globalSearchSection\">");
        SearchSection.Append("<table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" width=\"280px\">");
        SearchSection.Append("<tr>");
        SearchSection.Append("<td><label id=\"pageSectionTitle\">" + per_globalsearch_label_search + " </label>");
        SearchSection.Append("</td></tr>");
        SearchSection.Append("<tr><td width=\"250px\"><input type=\"text\" id=\"globalSearchTextInput\"; id=\"GlobalSearchPattern\" name=\"GlobalSearchPattern\" value=\"" + globalSearchPattern + "\"/>");
        SearchSection.Append("</td>");
        SearchSection.Append("<td width=\"20px%\"> <input type=\"image\" src=\"img/system-search.png\"/>");
        SearchSection.Append("</td>");
        SearchSection.Append("</tr>");

        if (SearchMessage.Length != 0)
        {
            SearchSection.Append("<tr><td id=\"pageText\" colspan=\"2\">");
            SearchSection.Append(SearchMessage.ToString());
            SearchSection.Append("</td></tr>");
            SearchMessage = new StringBuilder();
        }

        if (Session["AMCA_GlobalSearchQueries"] != null)
        {
            SearchSection.Append("<tr>");
            SearchSection.Append("<td style=\"vertical-align:bottom\">");
            SearchSection.Append("<a class=\"pageTextLink\" href=\"GlobalSearch.History.aspx?State=" + State + "\">");
            SearchSection.Append("Recent Searches");
            SearchSection.Append("</a>");
            SearchSection.Append("</td>");
            SearchSection.Append("<td></td></tr>");
        }

        SearchSection.Append("</table>");
        SearchSection.Append("</div>");
        StringBuilder sbWork = new StringBuilder();

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        Breadcrumbs = BreadCrumbHelper("GlobalSearch.aspx", Breadcrumbs, iPhonePageTitle, breadCrumbIndex, isElipseLink, false, false, false);
        iPhoneHideFooterBar = true;
        iPhoneHideHeaderGlobalSearchButton = true;
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
        iPhoneHideFooterBar = false;
        iPhoneHideHeaderGlobalSearchButton = false;

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
    /// Determines whether this instance [can load history] the specified query text.
    /// </summary>
    /// <param name="queryText">The query text.</param>
    /// <returns>
    /// <c>true</c> if this instance [can load history] the specified query text; otherwise, <c>false</c>.
    /// </returns>
    private bool CanLoadHistory(string queryText)
    {
        bool result = false;

        GlobalSearchParameter parameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);

        if (parameter != null && (string.IsNullOrEmpty(queryText) || (!string.IsNullOrEmpty(queryText) && queryText.Equals(parameter.QueryText, StringComparison.OrdinalIgnoreCase))))
        {
            result = true;
        }

        return result;
    }
}

