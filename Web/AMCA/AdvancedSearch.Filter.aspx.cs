/**
* <pre>
* 
*  Accela Citizen Access
*  File: AdvancedSearch.Filter.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2011
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: AdvancedSearch.Filter.aspx.cs 77905 2007-10-15 12:49:28Z  dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-21-2009           Dave Brewster           New page added for version 7.0
* 
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
//using Accela.ACA.BLL.Common;
//using Accela.ACA.BLL.APO;
//using Accela.ACA.Web.Common;

public partial class AdvancedSearchFilter : AccelaPage
{
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder SearchSection = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public string SearchMode = string.Empty;
    public string ResultPage = string.Empty;
    public string CollectionModule = string.Empty;
    public string CollectionId = string.Empty;
    public string PageBreadcrumbIndex = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        AppSettingsReader Settings = new AppSettingsReader();
        ValidationChecks("AdvancedSearch.Filter.aspx");
        string sStyle = string.Empty;
        SearchMode = GetFieldValue("Mode", false);
        CollectionId = GetFieldValue("CollectionId", false);
        CollectionModule = GetFieldValue("CollectionModule", false);
        ResultPage = GetFieldValue("ResultPage", false);
        bool isAllModuleView = GetFieldValue("isAllModuleView", false) == "true";
        string viewBaseModuleName = GetFieldValue("ViewBaseModuleName", false);
        HiddenFields.Append(HTML.PresentHiddenField("ViewBaseModuleName", viewBaseModuleName));
        HiddenFields.Append(HTML.PresentHiddenField("isAllModuleView", isAllModuleView == true ? "true" : "false"));

        string SearchPattern = string.Empty;
        if (GetFieldValue("FilterError", false) != null && GetFieldValue("FilterError", false) != string.Empty)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(Session["ACMA_FilterError"] != null ? Session["ACMA_FilterError"].ToString() : string.Empty);
            ErrorMessage.Append(" The % or * character cannot be inserted into the pattern text.");
            ErrorMessage.Append("</td></tr><tr><td></td><td  style=\"color:#FF6600; font-weight:bold;\"><br> Valid patterns: %value, value%, %value%");
            ErrorMessage.Append("<br> Invalid patterns: val%ue, %v%alue%, val%ue%");
            ErrorMessage.Append(ErrorFormatEnd);
            Session.Remove("AMCA_FilterError");
        }
        sStyle = "<a style=\"color:#040478; text-decoration:underline;\" href=\"";
        if (GetFieldValue("UpdateSession",false) != string.Empty)
        {
            
            SearchPattern = GetFieldValue("ModuleSearchPattern", false) == string.Empty ? string.Empty : GetFieldValue("ModuleSearchPattern", false);
            int charCount = 0;
            int firstWildcard = 0;
            int middleWildcard = 0;
            int endWildcard = 0;
            string compareType = string.Empty;
            string searchText = string.Empty;
            if (SearchPattern != null && SearchPattern.Trim().Length != 0)
            {
                string trimmedMask = SearchPattern.Trim();
                char[] characterArray = trimmedMask.ToCharArray();
                foreach (char aChar in characterArray)
                {
                    charCount++;
                    if (aChar == '%' || aChar == '*' || aChar == '|')
                    {
                        if (charCount == 1)
                        {
                            firstWildcard = charCount;
                        }
                        else if (endWildcard == 0)
                        {
                            endWildcard = charCount;
                        }
                        else
                        {
                            middleWildcard = endWildcard;
                            endWildcard = charCount;
                        }
                    }
                    else
                    {
                        searchText += aChar.ToString();
                    }
                }
                if (firstWildcard != 0 && endWildcard != 0)
                {
                    compareType = "Contains";
                }
                else if (firstWildcard != 0)
                {
                    compareType = "EndsWith";
                }
                else if (endWildcard != 0)
                {
                    compareType = "StartsWith";
                }
                else
                {
                    compareType = "Equals";
                }
            }
            if (SearchMode == "MyCollections")
            {
                Response.Redirect("MyCollections.List.aspx?State=" + State
                    + "&Mode=" + SearchMode
                    + "&Module=" + ModuleName
                    + "&CollectionId=" + CollectionId
                    + "&CollectionModule=" + CollectionModule
                    + "&isFilterChange=true"
                    + "&PageBreadcrumbIndex=" + GetFieldValue("PageBreadcrumbIndex", false)
                    + "&FilterType=" + compareType
                    + "&FilterMask=" + searchText
                    + "&ResultPage=" + ResultPage);
            }
            else
            {
                Response.Redirect("AdvancedSearch.Results.aspx?State=" + State
                    + "&Mode=" + SearchMode
                    + "&Module=" + ModuleName
                    + "&ReloadList=true"
                    + "&PageBreadcrumbIndex=" + GetFieldValue("PageBreadcrumbIndex", false)
                    + "&ViewBaseModuleName=" + GetFieldValue("ViewBaseModuleName", false)
                    + "&isAllModuleView=" + (isAllModuleView == true ? "true" : "false")
                    + "&FilterType=" + compareType
                    + "&FilterMask=" + searchText
                    + "&isFilterChange=true"
                    + "&ResultPage=" + ResultPage);
            }
        }
        StringBuilder sbWork = new StringBuilder();

        sbWork.Append("&Mode=" + SearchMode);
        sbWork.Append("&Module=" + ModuleName);
        sbWork.Append("&CollectionModule=" + CollectionModule);
        sbWork.Append("&ResultPage=" + ResultPage);
        Breadcrumbs = BreadCrumbHelper("AdvancedSearch.Filter.aspx", sbWork, "Fiter Results", "", false, false, false, false);

        iPhoneHideFooterBar = true;
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
        iPhoneHideFooterBar = false;

        PageBreadcrumbIndex = CurrentBreadCrumbIndex.ToString();

        SearchSection.Append("<div id=\"pageTitle\">");
        SearchPattern = ""; // Session["AMCA_ModuleSearchPattern"] == null ? string.Empty : Session["AMCA_ModuleSearchPattern"].ToString();
        SearchSection.Append("Filter by:");
        SearchSection.Append("</div>");
        SearchSection.Append("<div id=\"pageText\" border=\"0\">");
        SearchSection.Append("<table width=\"95%\" cellspacing=\"0\" cellpadding=\"0\"><tr><td width=\"90%\">");
        SearchSection.Append("<input type=\"text\" class=\"pageTextInput\" id=\"ModuleSearchPattern\" name=\"ModuleSearchPattern\" value=\"" + SearchPattern + "\"/>");
        SearchSection.Append("</td><td width=\"30px\">");
        SearchSection.Append("</td>");
        SearchSection.Append("</td><td>");
        SearchSection.Append(" <input type=\"image\" src=\"img/system-search.png\"/>");
        SearchSection.Append("</td><tr></table>");
        SearchSection.Append("</div>");
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
}

