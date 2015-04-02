/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AccelaPage.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: AppSession.cs 77905 2007-10-15 12:49:28Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  10-10-2009          Dave Brewster           Added new breadcrumb logic.
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Threading;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;
using Accela.ACA.Web.Util;


/// <summary>
/// General Page which contains information to be inherited in other pages which inherits this.
/// </summary>
[SuppressCsrfCheck]
public class AccelaPage : System.Web.UI.Page
{
    public AccelaProxy MyProxy = new AccelaProxy();
    public HTMLFactory HTML = new HTMLFactory();
    // logging for developers machine environment only.
    public string State = string.Empty;
    public const string BREAKTAG = "<br />";
    public const string BreadCrumbStyle = "<a  href=\"";
    public const string ElipseBreadcrumbStyle = "<a id\"BreadcrumbElipse\" href=\"";
    public const string BackLinkStyle = "<a href=\"";
    public const string PageLinkStyle = "<a style=\"color:#040478; font-weight:normal;  text-decoration:underline;\" href=\"";
    public const string ListLinkStyle = "<a class=\"pageListLink\" href=\"";
    public string ErrorFormat = "<div ><table cellpadding=\"1\" cellspacing=\"0\" id=\"errorMessage\"><tr><td valign=\"top\"><img src=\"img/error.png\"/></td><td class=\"errorMessage\">";
    public string ErrorFormatEnd = "</td></tr></table></div>";
    public string SortColumn = string.Empty;
    public string SortDesc = string.Empty;
    public string iPhonePageTitle = string.Empty;
    public string iPhoneBreadcrumbs = string.Empty;
    public string iPhoneHeader = string.Empty;
    public string iPhoneFooter = string.Empty;
    public bool iPhoneGlobalSearchIsActive = false;
    public bool iPhoneHideFooterBar = false;
    public bool iPhoneHideHeaderCollectionsButton = false;
    public bool iPhoneHideHeaderGlobalSearchButton = false;
    public bool iPhoneTitleHasBeenClipped = false;
    public string iPhoneResetPwdBackButton = string.Empty;

    public int GlobalSearch_Result_Page_Size = 5;

    protected int CurrentBreadCrumbIndex = 0;

    public bool enableDevLogging = false;

    protected override void InitializeCulture()
    {
        Thread.CurrentThread.CurrentCulture =CultureInfo.CreateSpecificCulture("en-US");
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        base.InitializeCulture();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if(Session["AMCA_BreadCrumbList"] == null)
        {
            Session["AMCA_BreadCrumbList"] = new List<string>() { "Login.Home.aspx|&Module=|Home|Home" };
        }
    }

    /// <summary>
    /// Page Validation Logic 
    /// </summary>
    /// <param name="PageName"></param>
    public void ValidationChecks(string PageName)
    {
        AppSettingsReader Settings = new AppSettingsReader();

        if (AuthenticationUtil.IsInternalAuthAdapter)
        {
            //if ((!Boolean.Parse(Settings.GetValue(PageName,typeof(string)).ToString())) || Request.QueryString.Count == 0)
            if (Request.QueryString["State"] == null
                || Request.QueryString["State"].ToString() == string.Empty
                || AppSession.User == null)
            {
                Response.Redirect("Default.aspx?" + UrlConstant.RETURN_URL + "=" + Server.UrlEncode(Request.RawUrl));
            }
            // is login valid?
            State = Request.QueryString["State"].ToString();

            hash MyHasher = new hash();
            MyHasher.setPublicKey(Settings.GetValue("TokenKey2", typeof(string)).ToString());
            try
            {
                string DecryptValue = MyHasher.decrypt(State);

                if (DecryptValue.Split('^')[1] != "ValidLogin" && DecryptValue.Split('^')[0] != (DateTime.Now.ToString().Split(' ')[0]))
                {
                    throw new Exception();
                }
            }
            catch
            {
                Response.Redirect("Default.aspx");
            }
        }
        else
        {
            if (!AuthenticationUtil.IsAuthenticated)
            {
                AuthenticationUtil.RedirectToLoginPage();
            }
        }
    }

    private string _moduleName;
    /// <summary>
    /// Gets the module name from request.
    /// </summary>
    protected string ModuleName
    {
        get
        {
            if (String.IsNullOrEmpty(_moduleName))
            {
                _moduleName = Request.QueryString[ACAConstant.MODULE_NAME];
            }
            return _moduleName;
        }
    }
    /// <summary>
    /// Gets the module name HTML used for the title of the Advanced Search Page.
    /// </summary>
    protected string AdvancedSearchListTitle
    {
        get
        {
            if (Session["AMCA_AdvancedSearch_Result_List_Name"] == null)
            {
                return string.Empty;
            }
            return Session["AMCA_AdvancedSearch_Result_List_Name"].ToString();
        }
    }
    /// <summary>
    /// Gets the module name that AdvancedSearch.Results Page caplist was built for.
    /// </summary>
    protected string AdvancedSearchListTitleBaseModule
    {
        get
        {
            if (Session["AMCA_AdvancedSearch_Result_List_BaseModule"] == null)
            {
                return string.Empty;
            }
            return Session["AMCA_AdvancedSearch_Result_List_BaseModule"].ToString();
        }
    }
    /// <summary>
    /// Create Breadcrumbs for previous pages.
    /// </summary>
    /// <param name="PageBreadCrumbURL">URL of page to launch</param>
    /// <param name="URLParameters"> URL parameters to be passed to target page</param>
    /// <param name="PageBreadCrumbText"> Text (page title) that is displayed in breadcrumb</param>
    /// <param name="isElipseSegment">no longer has any meaning</param>
    /// <param name="isRefreshOnly">Indicates breadcrumbs are to be re-rendered - no new breadcrumb is added</param>
    /// <param name="iPagingMode">Indicates calling page is being repainted - no new breadcrumb is added</param>
    /// <param name="iPageWithoutForm">Indicates calling page does not have a form so div is not included </param>
    /// <returns>(string) HTML for breadcrumbs for target page</returns>
    protected StringBuilder BreadCrumbHelper(string PageBreadCrumbURL, 
        StringBuilder URLParameters, 
        string PageBreadCrumbText, 
        string BreadCrumbIndex, 
        bool isElipseSegment, 
        bool isRefreshOnly, 
        bool isPagingMode, 
        bool isPageWithoutForm)
    {
        List<string> breadCrumbList = new List<string>();
        List<string> breadCrumbListWork = new List<string>();
        StringBuilder breadCrumbURLout = new StringBuilder();
        string basePageName = string.Empty;
        string formattedPageName = string.Empty;
        const string htmlEMPTYs = "&nbsp;&nbsp;";
        string linkStyle = BreadCrumbStyle;
        string elipseStyle = ElipseBreadcrumbStyle;
        string leftPointer = " > ";

        int breadCrumbIndex = 0;
        WriteToDebugLog("--------------------------------------------------------");
        WriteToDebugLog("PageBreadCrumbURL:" + PageBreadCrumbURL);
        WriteToDebugLog("Parmeters:" + URLParameters.ToString());
        WriteToDebugLog("pageBreadCrumbText:" + PageBreadCrumbText);

        // retrieve breadcrumb list.
        if (Session["AMCA_BreadCrumbList"] != null)
        {
            breadCrumbList = (List<string>)Session["AMCA_BreadCrumbList"];
        }
        if (PageBreadCrumbText.Contains("|") == true)
        {
            string[] pageNames = PageBreadCrumbText.Split('|');
            basePageName = pageNames[0];
            formattedPageName = pageNames[1];
        }
        else
        {
            basePageName = PageBreadCrumbText;
            formattedPageName = PageBreadCrumbText;
        }


        // Note - this code ignores the dummy breadcrumb added by the Licensed Professional Detail Page
        // when the user uses that browser backbutton rather than useing the breadcrumbs or 
        // back page link.
        breadCrumbIndex = breadCrumbList.Count - 1;
        int ignoreLastBreadcrumb = 0;
        if (breadCrumbIndex > 2)
        {
            if (breadCrumbList[breadCrumbIndex].Contains("Dummy Breadcrumb") == true)
            {
                ignoreLastBreadcrumb = -1;
            }
        }
        breadCrumbIndex = String.IsNullOrEmpty(BreadCrumbIndex) ? 0 : int.Parse(BreadCrumbIndex);
        if (isRefreshOnly == true || isPagingMode == true || breadCrumbIndex < 0)
        {
            if (breadCrumbIndex < 0)
            {
                breadCrumbIndex = breadCrumbList.Count + breadCrumbIndex;
            }
            else
            {
                breadCrumbIndex = (breadCrumbList.Count - 1) + ignoreLastBreadcrumb;
            }
        }
        else
        {
            if (breadCrumbIndex != 0)
            {
                breadCrumbIndex--;
            }
            if (breadCrumbIndex == 0 || breadCrumbIndex > (breadCrumbList.Count + ignoreLastBreadcrumb) )
            {
                breadCrumbIndex = breadCrumbList.Count + ignoreLastBreadcrumb;
            }
        }

        // Build the bread crumb list and return it to the calling page.
        int stringCnt = 0;
        int elipseCnt = 0;
        int breadCrumbCnt = 0;
        //if (isPageWithoutForm != true && isiPhone == false)
        //{
        //    breadCrumbURLout.Append("<div style=\"margin-top:10px; margin-bottom:10px;\">");
        //}
        StringBuilder newEntry = new StringBuilder();
        newEntry.Append(PageBreadCrumbURL + "|" + URLParameters.ToString());
            if (SortColumn != null && SortColumn != string.Empty)
            {
                newEntry.Append("&SortColumn=" + SortColumn.ToString());
                newEntry.Append("&SortDesc=" + SortDesc.ToString());
            }
        newEntry.Append( "|" + basePageName + "|" + formattedPageName.ToString());
        bool browserRefreshUsed = false;
        if (breadCrumbList.Count > 0 && isPagingMode == false && isRefreshOnly == false & breadCrumbIndex == (breadCrumbList.Count + ignoreLastBreadcrumb))
        {
            browserRefreshUsed = newEntry.ToString() == breadCrumbList[(breadCrumbList.Count - 1 + ignoreLastBreadcrumb)].ToString();
            if (browserRefreshUsed == true)
            {
                breadCrumbIndex--;
            }
        }
        /*
          This code was added to compensate for the times when the user uses the browser
          back button to navigate to previous pages, and then selects a link on that page.
          This loop will scan the existing bread crumb links for the first link that 
          refernece the page that is currently being rendered.  If it finds it in the list it sets
          them breadCrumbIndex to this reference so the bread crumb list is truncated
          at that point.
         */
        for (int aRow = 0; aRow < (breadCrumbList.Count + ignoreLastBreadcrumb); aRow++)
        {
            string breadCrumb = breadCrumbList[aRow].ToString();

            string[] Url_text = breadCrumb.Split('|');

            if (Url_text[0].ToString() == PageBreadCrumbURL)
            {
                if (!(isRefreshOnly || isPagingMode))
                {
                    breadCrumbIndex = aRow;
                }
            }
        }

        if (breadCrumbIndex > 4)
        {
            elipseCnt = breadCrumbIndex - 3;
        }
        breadCrumbURLout.Append("<div id=\"breadcrumbs\">");
        if (isiPhone == true)
        {
            // linkStyle = "<a  href=\"";
            // elipseStyle = "<a href=\"";
            // leftPointer = "";
        }
        for (int aRow = 0; aRow < (breadCrumbList.Count + ignoreLastBreadcrumb); aRow++)
        {
            if (aRow == breadCrumbList.Count - 1
                && browserRefreshUsed == true)
            {
                break;
            }
            string breadCrumb = breadCrumbList[aRow].ToString();
            string[] Url_text = breadCrumb.Split('|');
            stringCnt++;

            if (stringCnt > breadCrumbIndex)
            {
                break;
            }

            WriteToDebugLog("bcIndex:" + breadCrumbIndex.ToString()
                + " elipseCnt:" + elipseCnt.ToString()
                + " breadCrumbCnt:" + breadCrumbCnt.ToString()
                + "[stringCnt:" + stringCnt.ToString() + "]" 
                + breadCrumb.ToString());

            breadCrumbListWork.Add(breadCrumb);
            CurrentBreadCrumbIndex = stringCnt;
            if (stringCnt == 1)
            {
                breadCrumbURLout.Append(linkStyle);
                breadCrumbURLout.Append(Url_text[0]);
                breadCrumbURLout.Append("?State=" + State);
                breadCrumbURLout.Append(Url_text[1]);
                breadCrumbURLout.Append("&BreadCrumbIndex=" + stringCnt.ToString());
                breadCrumbURLout.Append("&SlidePage=LeftToRight");
                breadCrumbURLout.Append("\">");
                if (enableDevLogging == true)
                {
                    breadCrumbURLout.Append("(" + stringCnt.ToString() + ")");
                }
                breadCrumbURLout.Append(Url_text[3]);
                breadCrumbURLout.Append("</a>");
                breadCrumbCnt++;
            }
            else if (stringCnt == elipseCnt)
            {
                breadCrumbURLout.Append(leftPointer);
                breadCrumbURLout.Append(elipseStyle);
                breadCrumbURLout.Append(Url_text[0]);
                breadCrumbURLout.Append("?State=" + State);
                breadCrumbURLout.Append(Url_text[1]);
                breadCrumbURLout.Append("&BreadCrumbIndex=" + stringCnt.ToString());
                breadCrumbURLout.Append("&SlidePage=LeftToRight");
                breadCrumbURLout.Append("&IsElipseLink=" + stringCnt.ToString());
                breadCrumbURLout.Append("\">");
                breadCrumbURLout.Append(htmlEMPTYs + "..." + htmlEMPTYs);
                breadCrumbURLout.Append("</a>");
                breadCrumbCnt++;
            }
            else if (stringCnt > elipseCnt)
            {
                breadCrumbURLout.Append(leftPointer);
                breadCrumbURLout.Append(linkStyle);
                breadCrumbURLout.Append(Url_text[0]);
                breadCrumbURLout.Append("?State=" + State);
                breadCrumbURLout.Append(Url_text[1]);
                breadCrumbURLout.Append("&BreadCrumbIndex=" + stringCnt.ToString());
                breadCrumbURLout.Append("&SlidePage=LeftToRight");
                breadCrumbURLout.Append("\">");
                if (enableDevLogging == true)
                {
                    breadCrumbURLout.Append("(" + stringCnt.ToString() + ")");
                }
                breadCrumbURLout.Append(Url_text[3]);
                breadCrumbURLout.Append("</a>");
                breadCrumbCnt++;
            }
            if (breadCrumbCnt > 4)
            {
                // This code should never be reached.  The end of the breadcrumb
                // list will occurr before breadCrumbCnt can exceed 4.
                break;
            }
        }
        //if (isPageWithoutForm != true || (isiPhone == true))
        //{
            breadCrumbURLout.Append("</div>");
        //}
        if (isRefreshOnly == false || isPagingMode == true)
        {
            if (SortColumn != null && SortColumn != string.Empty)
            {
                URLParameters.Append("&SortColumn=" + SortColumn.ToString());
                URLParameters.Append("&SortDesc=" + SortDesc.ToString());
            }
            breadCrumbListWork.Add(PageBreadCrumbURL + "|" + URLParameters.ToString() + "|" + basePageName + "|" + formattedPageName);
            WriteToDebugLog("");
            WriteToDebugLog("Appended(" + breadCrumbListWork.Count + "): " + PageBreadCrumbURL + "|" + URLParameters.ToString() + "|" + basePageName + "|" + formattedPageName);
        }
        WriteToDebugLog("");
        WriteToDebugLog("");
        if (isRefreshOnly == false || isPagingMode == true)
        {
            Session["AMCA_BreadCrumbList"] = breadCrumbListWork;
        }
        if (isiPhone == true)
        {
            iPhoneBreadcrumbs = breadCrumbURLout.ToString();
            breadCrumbURLout = new StringBuilder();
        }
        return breadCrumbURLout;
    }

    /// <summary>
    /// Create iPnone page header 
    /// </summary>
    /// <param name="BreadCrumbIndex">Index of breadcrumb that the pages backlink will show</param>
    /// <returns>(string) Back link text + target page URL and return parameters</returns>
    protected void GetiPhoneFooter()
    {
        iPhoneFooter = string.Empty;
        IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
        string userName = peopleBll.GetContactUserName(AppSession.User.UserModel4WS);
        userName = DataUtil.TruncateString(userName, 25);

        if (iPhoneHideFooterBar == false)
        {
            StringBuilder footerHTML = new StringBuilder();
            footerHTML.Append("<div id=\"iPhoneFooterBar\">");
            footerHTML.Append("<table cell-padding=\"0\" cell-spacing=\"0\" width=\"100%\">");
            footerHTML.Append("<tr>");
            footerHTML.Append("<td width=\"15%\"></td>");
            footerHTML.Append("<td width=\"70%\" class=\"iPhoneFooterLogoutId\">" + userName);
            footerHTML.Append("</td><td width=\"15%\">");
            footerHTML.Append(" <a class=\"iPhoneFooterLogoutButton\" href=\"Default.aspx?SlidePage=LeftToRight&login=failed2\">Logout</a>");
            footerHTML.Append("</td></tr></table></div>");

            iPhoneFooter = footerHTML.ToString();
        }
    }
    /// <summary>
    /// Create page "Back To" links from breadcrumbs.
    /// </summary>
    /// <param name="BreadCrumbIndex">Index of breadcrumb that the pages backlink will show</param>
    /// <returns>(string) Back link text + target page URL and return parameters</returns>
    protected string BackLinkHelper(string BreadCrumbIndex)
    {
        List<string> breadCrumbList = new List<string>();
        StringBuilder breadCrumbURLout = new StringBuilder();
        StringBuilder iPhoneBackURL = new StringBuilder();
        bool isResetPassword = false;

        if (BreadCrumbIndex != "login")
        {
            // retrieve breadcrumb list.
            if (Session["AMCA_BreadCrumbList"] != null)
            {
                breadCrumbList = (List<string>)Session["AMCA_BreadCrumbList"];
            }
            int breadCrumbIndex = (BreadCrumbIndex == null || BreadCrumbIndex == string.Empty) ? 0 : int.Parse(BreadCrumbIndex);
            if (breadCrumbIndex == 0)
            {
                breadCrumbIndex = breadCrumbList.Count;
            }

            // Build the bread crumb list and return it to the calling page.
            int stringCnt = 0;

            if (breadCrumbList != null)
            {
                foreach (string breadCrumb in breadCrumbList)
                {
                    stringCnt++;
                    if (stringCnt == breadCrumbIndex)
                    {
                        string[] Url_text = breadCrumb.Split('|');
                        string stringWork = Url_text[2].ToString();
                        string backText = "<< Return To ";
                        if (Url_text[2] == "[use_session_lable]")
                        {
                            stringWork = AdvancedSearchListTitle;
                        }

                        breadCrumbURLout.Append("<div id=\"backlink\">");
                        if (isiPhone == true)
                        {
                            backText = "Return To ";
                            breadCrumbURLout.Append("<center>");
                            //breadCrumbURLout.Append("<a  href=\"");
                        }
                        //else
                        //{
                        //    breadCrumbURLout.Append("<div style=\"margin-left:5px; margin-top:10px; margin-bottom:10px;\">");
                        //    breadCrumbURLout.Append(BackLinkStyle);
                        //}
                        if (isiPhone == true)
                        {
                            int lowerCase = 0;
                            int otherCase = 0;
                            int spaces = 0;
                            for (int x = 0; x < Url_text[3].Length; x++)
                            {
                                int isNumber;
                                if (int.TryParse(Url_text[3].Substring(x, 1), out isNumber))
                                {
                                    if (isNumber != 1)
                                    {
                                        otherCase++;
                                        continue;
                                    }
                                }
                                if (Url_text[3].Substring(x, 1) == " "
                                    || Url_text[3].Substring(x, 1) == "-")
                                {
                                    lowerCase++;
                                    spaces++;
                                    continue;
                                }
                                if (Url_text[3].Substring(x, 1) == "1"
                                    || Url_text[3].Substring(x, 1) == "."
                                    || Url_text[3].Substring(x, 1) == " "
                                    || Url_text[3].Substring(x, 1) == "!"
                                    || Url_text[3].Substring(x, 1) == ","
                                    || Url_text[3].Substring(x, 1) == Url_text[3].Substring(x, 1).ToLower())
                                {
                                    lowerCase++;
                                    continue;
                                }
                                otherCase++;
                            }
                            int clipFontLimit = 8;
                            iPhoneBackURL.Append("<span class=\"iPhoneBackLink\">");
                            iPhoneBackURL.Append("<a  href=\"");
                            iPhoneBackURL.Append(Url_text[0]);
                            iPhoneBackURL.Append("?State=" + State);
                            iPhoneBackURL.Append(Url_text[1]);
                            iPhoneBackURL.Append("&BreadCrumbIndex=" + stringCnt.ToString());
                            iPhoneBackURL.Append("&SlidePage=LeftToRight");
                            iPhoneBackURL.Append("\">");
                            if (Url_text[3].Length > clipFontLimit)
                            {
                                // iPhoneBackURL.Append("&hellip;" + Url_text[3].Substring(0, 8));
                                iPhoneBackURL.Append("..." + Url_text[3].Substring(0, clipFontLimit).ToLower());
                            }
                            else
                            {
                                iPhoneBackURL.Append(Url_text[3]);
                            }
                            iPhoneBackURL.Append("</a>");
                            iPhoneBackURL.Append("</span>");
                        }
                        breadCrumbURLout.Append("<a  href=\"");
                        breadCrumbURLout.Append(Url_text[0]);
                        breadCrumbURLout.Append("?State=" + State);
                        breadCrumbURLout.Append(Url_text[1]);
                        breadCrumbURLout.Append("&BreadCrumbIndex=" + stringCnt.ToString());
                        breadCrumbURLout.Append("\">" + backText);
                        breadCrumbURLout.Append(Url_text[3]);
                        breadCrumbURLout.Append("</a>");
                        if (isiPhone == true)
                        {
                            breadCrumbURLout.Append("</center>");
                        }
                        breadCrumbURLout.Append(iPhoneBackURL.ToString());
                        breadCrumbURLout.Append("</div>");
                        break;
                    }
                }
            }
        }
        else
        {
            iPhoneBackURL.Append("<span class=\"iPhoneBackLink\">");
            iPhoneBackURL.Append("<a href=\"Default.aspx?SlidePage=LeftToRight&login=failed2\">");
            iPhoneBackURL.Append("Login");
            iPhoneBackURL.Append("</a>");
            iPhoneBackURL.Append("</span>");
            isResetPassword = true;
        }
        iPhoneTitleHasBeenClipped = false;
        if (isiPhone == true)
        {
            // iPhonePageTitle = "20XXXXXXXXXXXXXXXXXXXXXX";
            StringBuilder headerHtml = new StringBuilder();

            headerHtml.Append("<div id=\"iPhoneHeaderBar\">");
            headerHtml.Append("<table border=\"0\" cell-padding=\"0\" cell-spacing=\"0\" width=\"100%\">");
            headerHtml.Append("<tr>");
            // headerHtml.Append("<td width=\"20%\">");
            headerHtml.Append("<td width=\"74px\">");
            if (iPhoneBackURL.Length > 0)
            {
                headerHtml.Append(iPhoneBackURL.ToString());
            }
            headerHtml.Append("</td>");

            // this code saves the previous header title and redisplays it if
            // the new page does not set the title to any value.
            if (iPhonePageTitle == string.Empty)
            {
                if (Session["AMCA_iPhonePageTitle"] != null)
                {
                    iPhonePageTitle = Session["AMCA_iPhonePageTitle"].ToString();
                }
            }
            int lowerCase = 0;
            int otherCase = 0;
            int spaces = 0;
            for (int x = 0; x < iPhonePageTitle.Length; x++)
            {   
                int isNumber;
                if (int.TryParse(iPhonePageTitle.Substring(x,1), out isNumber))
                {
                    if (isNumber != 1)
                    {
                        otherCase++;
                        continue;
                    }
                }
                if (iPhonePageTitle.Substring(x,1) == " "
                    || iPhonePageTitle.Substring(x,1) == "-")
                {
                        lowerCase++;
                        spaces++;
                        continue;
                }
                if (iPhonePageTitle.Substring(x,1) == "1" 
                    || iPhonePageTitle.Substring(x,1) == "." 
                    || iPhonePageTitle.Substring(x,1) == " " 
                    || iPhonePageTitle.Substring(x,1) == "!" 
                    || iPhonePageTitle.Substring(x,1) == "," 
                    || iPhonePageTitle.Substring(x,1) == iPhonePageTitle.Substring(x,1).ToLower())
                {
                    lowerCase++;
                    continue;
                }
                otherCase++;
            }

            int largFontLimit = (lowerCase > otherCase) ? 16 : 13;
            int medFontLimit = (lowerCase > otherCase) ? 21 : 15;
            if (otherCase < 4 && spaces != 0 && iPhonePageTitle.Length < 26)
            {
                medFontLimit = 26;
            }
            Session["AMCA_iPhonePageTitle"] = iPhonePageTitle;
            if (iPhonePageTitle.Length > largFontLimit)
            {
                if (iPhonePageTitle.Length > medFontLimit)
                {
                    // headerHtml.Append("<td width=\"58%\" class=\"iPhoneHeaderPageTitle13\">");
                    headerHtml.Append("<td  class=\"iPhoneHeaderPageTitle13\">");
                    headerHtml.Append("<span>");
                    headerHtml.Append("..." + iPhonePageTitle.Substring(0, medFontLimit));
                    iPhoneTitleHasBeenClipped = true;
                }
                else
                {
                    // headerHtml.Append("<td width=\"58%\" class=\"iPhoneHeaderPageTitle13\">");
                    headerHtml.Append("<td  class=\"iPhoneHeaderPageTitle13\">");
                    headerHtml.Append("<span>");
                    headerHtml.Append(iPhonePageTitle);
                }
            }
            else
            {
                // headerHtml.Append("<td width=\"58%\" class=\"iPhoneHeaderPageTitle15\">");
                headerHtml.Append("<td  class=\"iPhoneHeaderPageTitle15\">");
                headerHtml.Append("<span>");
                headerHtml.Append(iPhonePageTitle);
            }
            headerHtml.Append("</span>");

            headerHtml.Append("</td>");

            // headerHtml.Append("<td width=\"22%\">");
            headerHtml.Append("<td width=\"72px\">");
            if (iPhoneHideHeaderCollectionsButton == false && isResetPassword == false)
            {
                headerHtml.Append("<span class=\"iPhoneCollectionLink\">");
                headerHtml.Append("<a  href=\"MyCollections.aspx?State=" + State);
                headerHtml.Append("&BreadCrumbIndex=2");
                headerHtml.Append("\">");
                headerHtml.Append("<img  src=\"img/icon_collections.png\" alt=\"Accela\" />");
                headerHtml.Append("</a>");
                headerHtml.Append("</span>");

            }
            if (Session["AMCA_GlobalSearchIsEnabled"] != null && iPhoneHideHeaderGlobalSearchButton == false && isResetPassword == false)
            {
                headerHtml.Append("<span class=\"iPhoneGlobalSearchLink\">");
                headerHtml.Append("<a href=\"GlobalSearch.aspx?State=" + State);
                headerHtml.Append("&BreadCrumbIndex=2");
                headerHtml.Append("\">");
                headerHtml.Append("<img  src=\"img/icon_search.png\" alt=\"Accela\" />");
                headerHtml.Append("</a>");
                headerHtml.Append("</span>");
            }
           
            headerHtml.Append("</td>");
            headerHtml.Append("</tr></table>");
            
            headerHtml.Append("</div>");

            iPhoneHeader = headerHtml.ToString();
            if (iPhoneFooter == string.Empty && isResetPassword == false)
            {
                GetiPhoneFooter();
            }
            breadCrumbURLout = new StringBuilder();
        }
        return breadCrumbURLout.ToString();
    }
    /// <summary>
    /// Appends a parameter to a breadcrumb that is currently stored in the list.
    /// </summary>
    /// <param name="BreadCrumbIndex">Index of breadcrumb that the pages backlink will show</param>
    /// <param name="ParameterName">Name of parameter to appended to breadcrumb's list of parameters</param>
    /// <param name="ParameterValue">Value of parameter to appended to breadcrumb's list of parameters</param>
    /// <returns>(bool) true if success or false if failure</returns>
    protected bool BreadcrumbAppendParameter(string BreadCrumbIndex, string ParameterName, string ParameterValue)
    {
        List<string> breadCrumbList = new List<string>();
        StringBuilder breadCrumbURLout = new StringBuilder();

        // retrieve breadcrumb list.
        if (Session["AMCA_BreadCrumbList"] != null)
        {
            breadCrumbList = (List<string>)Session["AMCA_BreadCrumbList"];
        }
        if (BreadCrumbIndex == null || BreadCrumbIndex == string.Empty)
        {
            BreadCrumbIndex = breadCrumbList.Count.ToString();
        }
        int breadCrumbIndex = (BreadCrumbIndex == null || BreadCrumbIndex == string.Empty) ? 0 : int.Parse(BreadCrumbIndex);

        breadCrumbIndex--;
        if (breadCrumbIndex > -1 && breadCrumbIndex < breadCrumbList.Count)
        {
            string[] Url_text = breadCrumbList[breadCrumbIndex].Split('|');
            if (!Url_text[1].Contains(ParameterName))
            {
                Url_text[1] += ("&" + ParameterName + "=" + ParameterValue).ToString();
                breadCrumbList[breadCrumbIndex] = Url_text[0] + "|" + Url_text[1] + "|" + Url_text[2] + "|" + Url_text[3];
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// For Development use only - writes message to logfile
    /// </summary>
    /// <param name="message">text to be written to logfile</param>
    /// <returns>void</returns>
    private void WriteToDebugLog(string message)
    {
        if (enableDevLogging == true)
        {
            try
            {
                string FileLoc = "c:\\ACAMobileDevLog.txt";
                if (FileLoc.ToString().Trim() != "")
                {
                    using (System.IO.StreamWriter sw = System.IO.File.AppendText(FileLoc))
                    {
                        sw.WriteLine(message);
                        sw.Close();
                    }

                }
            }
            catch 
            {
                // ignore errors and continue
            }
        }
    }

    protected bool isGlobalSearchMode
    {
        get
        {
            if (Session["AMCA_GlobalSearch_Mode"] == null || Session["AMCA_GlobalSearch_Mode"].ToString() == "false")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Check for iPhonei mode.
    /// </summary>
    protected bool isiPhone
    {
        get
        {
            if (Session["AMCA_App_type"] == null || Session["AMCA_App_type"].ToString() != "iPhone")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Check for Safari mode.
    /// </summary>
    protected bool isSafari
    {
        get
        {
            if (Session["AMCA_App_type"] == null || Session["AMCA_App_type"].ToString() != "Safari")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Check for Opera mode.
    /// </summary>
    public bool isOpera 
    {
        get
        {
            if (Session["AMCA_App_type"] == null || Session["AMCA_App_type"].ToString() != "Opera")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Get actual value from ACA formatted label text.
    /// </summary>
    public string StripHTMLFromLabelText(string labelText, string labelDefault)
    {
        if (labelText == null || labelText.Length == 0)
        {
            return labelDefault;
        }
        try
        {
            int startTag = 0;
            int endTag = labelText.IndexOf(">", 0);
            if (endTag != 0)
            {
                startTag = labelText.IndexOf("<", endTag++);
            }
            if (startTag != 0 && startTag > endTag)
            {
                return labelText.Substring(endTag, startTag - endTag);
            }
            else
            {
                return labelDefault;
            }
        }
        catch
        {
            return labelDefault;
        }
    }
    /// <summary>
    /// Get ACA GUI_TEXT values use on pages.
    /// </summary>
    public string LocalGetTextByKey(string key)
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

    /// <summary>
    /// Locals the get text by key without HTML.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public string LocalGetTextByKeyWithoutHtml(string key)
    {
        return ScriptFilter.RemoveHTMLTag(LabelUtil.GetTextByKey(key, ModuleName));
    }




    #region code copied from ACA modules

    /// <summary>
    /// Format Phone country code to show (+086)
    /// </summary>
    /// <param name="countrycode">the phone country code need to format</param>
    /// <returns>Formated phone country code</returns>
    public string FormatPhoneCountryCodeShow(string countrycode)
    {
        return ModelUIFormat.FormatPhoneCountryCodeShow(countrycode);
    }

    /// <summary>
    /// Add mask "-" into phone number textbox in edit page.
    /// </summary>
    /// <param name="phone">the phone need to format</param>
    /// <param name="countryCode">The country code.</param>
    /// <returns>Formated phone</returns>
    public string FormatPhone4EditPage(string phone, string countryCode)
    {
        return ModelUIFormat.FormatPhone4EditPage(phone, countryCode);
    }

    /// <summary>
    /// Format the phone to show
    /// </summary>
    /// <param name="phoneCountryCode">Phone country code</param>
    /// <param name="phone">phone number</param>
    /// <param name="country">The country.</param>
    /// <returns>Format phone form left to right.</returns>
    public string FormatPhoneShow(string phoneCountryCode, string phone, string country)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return string.Empty;
        }

        phoneCountryCode = ScriptFilter.FilterScript(phoneCountryCode);
        phone = ScriptFilter.FilterScript(phone);
        StringBuilder builder = new StringBuilder();
        builder.Append(string.IsNullOrEmpty(phoneCountryCode) ? string.Empty : FormatPhoneCountryCodeShow(phoneCountryCode));
        builder.Append(string.IsNullOrEmpty(phone) ? string.Empty : FormatPhone4EditPage(phone, country));
        return builder.ToString();
    }

    /// <summary>
    /// Format the zip to show
    /// </summary>
    /// <param name="zip">the zip need to format</param>
    /// <param name="countryCode">The country code.</param>
    /// <returns>Formated zip</returns>
    public string FormatZipShow(string zip, string countryCode)
    {
        return ModelUIFormat.FormatZipShow(zip, countryCode);
    }

    #endregion
}
