/**
* <pre>
* 
*  Accela Citizen Access
*  File: GlobalSearch.History.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2009
* 
*  Description:
*  View list of previous global search patterns
* 
*  Notes:
*      $Id: GlobalSearch.History.aspx.cs 189684 2011-01-27 00:16:01Z dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-05-2009           Dave Brewster           New page added for version 7.0
* 
* </pre>
*/
using System;
using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Collections.Generic;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
//using Accela.ACA.Common;
//using Accela.ACA.Web;
//using Accela.ACA.Web.Common;
//using Accela.ACA.BLL.MyCollection;
//using Accela.ACA.WSProxy;
//using Accela.ACA.WSProxy.WSModel;
//using Accela.ACA.Web.Common.GlobalSearch;


/// <summary>
/// 
/// </summary>
public partial class GlobalSearchHistory : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder PageTitle = new StringBuilder();
    public StringBuilder SearchResultLinks = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("GlobalSearch.History.aspx");
        State = GetFieldValue("State", false);

        iPhonePageTitle = "Recent Searches";
        if (isiPhone == false)
        {
            PageTitle.Append("<div id=\"pageTitle\">" + iPhonePageTitle + "</div>");
        }
        // PageTitle.Append("Recent Searches");

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&Module=" + ModuleName);
        Breadcrumbs = BreadCrumbHelper("GlobalSearch.History.aspx", sbWork, iPhonePageTitle, null, isElipseLink, false, false, false);
        iPhoneHideFooterBar = true;
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
        iPhoneHideFooterBar = false;

        if (Session["AMCA_GlobalSearchQueries"] != null)
        {
            string previouSearchQueries = Session["AMCA_GlobalSearchQueries"].ToString();
            string[] history = previouSearchQueries.Split('|');

            SearchResultLinks.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
            int rowCnt = 0;
            int maxCnt = history.Length - 1;
            foreach (string qValue in history)
            {
                if (qValue == "<end>")
                {
                    break;
                }
                rowCnt++;
                StringBuilder aRow = new StringBuilder();
                // aRow.Append("<tr><td>");
                if (isiPhone == true)
                {
                    aRow.Append("<a class=\"pageListCellBoldText\" href=\"" + "GlobalSearch.aspx?State=" + State);
                }
                else                   
                {
                    aRow.Append("<a class=\"pageListLinkBold \" href=\"" + "Login.Home.aspx?State=" + State);
                }
                aRow.Append("&UpdateSession=true");
                aRow.Append("&GlobalSearchPattern=" + qValue + "\">");
                aRow.Append(qValue);
                if (isiPhone == true)
                {
                    aRow.Append("<img style=\"float:right\" src=\"img/chevron.png\" /> ");
                }
                aRow.Append("</a>");
                SearchResultLinks.Append(MyProxy.CreateSelectListCell(aRow.ToString(), rowCnt - 1, rowCnt - 1, maxCnt, 0, 9999, isiPhone, true));
            }
            if (isiPhone == true)
                SearchResultLinks.Append("</td></tr>");
            SearchResultLinks.Append("</table>");
        }

        if (Session["AMCA_GlobalSearch_Sort_DESC"] != null)
        {
            Session.Remove("AMCA_GlobalSearch_Sort_DESC");
        }
        if (Session["AMCA_GlobalSearch_Sort_Column"] != null)
        {
            Session.Remove("AMCA_GlobalSearch_Sort_Column");
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
}
