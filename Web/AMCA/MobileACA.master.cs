/**
* <pre>
* 
*  Accela Citizen Access
*  File: MobileACA.master.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
*  Master page for MobileACA project. 
* 
*  Notes:
*      $Id: MobileACA.master.cs 77905 2008-07-18 12:49:28Z ACCELA\dbrewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-18-2008           DWB           2008 Mobile ACA interface redesign
*  03/19/2009           DWB           Added meta data tags to force the page content to be scaled
*                                     correctly to fit in the iPhone or Blackberry view ports. Also
*                                     added logic to detect when the user's browser is running on the
*                                     iPhone or BlackBerry
*  04/01/2009           Dave Brewster Added global.asa and web.config setting for branding the application name.
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

public partial class CA_MobileACA : System.Web.UI.MasterPage
{
    public StringBuilder PageLogo = new StringBuilder();
    public string styleSheet = string.Empty;
    public string sBodyWidth = string.Empty;
    public string AccelaApplicationName = "Mobile Citizen Access";
    public string AgencyCopyright = string.Empty;
    private string AgencyLogo = string.Empty;
    private StringBuilder AgencyLogoSize = new StringBuilder();
    private string AgencyDescription = string.Empty;
    private string AgencyLogoHeight = string.Empty;
    private string AgencyLogoWidth = string.Empty;
    public string AccelaApplicationIcon = string.Empty;
    public string PageDirection = "RightToLeft";
    public string iPhoneLineBreak = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        // AgencyLogo = "img/MobileACA_Demo_Logo.jpg";
        AppSettingsReader Settings = new AppSettingsReader(); 
        string sRoot = System.Web.HttpContext.Current.Request.ApplicationPath;
        if (!sRoot.EndsWith("/", StringComparison.InvariantCulture))
            sRoot += "/";
        try
        {
            AgencyLogo = sRoot + Settings.GetValue("AgencyLogoLocation", typeof(string));
        }
        catch
        {
            AgencyLogo = string.Empty;
        }
        if (AgencyLogo == string.Empty)
        {
            AgencyLogo = sRoot + "AMCA/img/AccelaLogo2.jpg";
        }

        try
        {
            AgencyLogoHeight = Settings.GetValue("AgencyLogoHeightAdj", typeof(string)).ToString();
        }
        catch
        {
            AgencyLogoHeight = string.Empty;
        }
        if (AgencyLogoHeight != string.Empty)
        {
            AgencyLogoSize.Append("Height=\"" + AgencyLogoHeight + "\"");
        }

        try
        {
            AgencyLogoWidth = Settings.GetValue("AgencyLogoWidthAdj", typeof(string)).ToString();
        }
        catch
        {
            AgencyLogoWidth = string.Empty;
        }
        if (AgencyLogoWidth != string.Empty)
        {
            AgencyLogoSize.Append(" Width=\"" + AgencyLogoWidth + "\"");
        }

        try
        {
            AgencyDescription = Settings.GetValue("AgencyLogoName", typeof(string)).ToString();
        }
        catch
        {
            AgencyDescription = string.Empty;
        }

        try
        {
            AgencyCopyright = Settings.GetValue("AgencyCopyrightName", typeof(string)).ToString();
        }
        catch
        {
            AgencyCopyright = AgencyDescription;
        }

        if (AgencyCopyright == string.Empty)
        {
            AgencyCopyright = "&copy; Accela Inc.";
        }
        else
        {
            AgencyCopyright = "&copy;" + AgencyCopyright;
        }

        try
        {
            AccelaApplicationName = Settings.GetValue("AccelaApplicationName", typeof(string)).ToString();
        }
        catch
        {
            AccelaApplicationName = AgencyDescription;
        }

        if (AccelaApplicationName == string.Empty)
        {
            AccelaApplicationName = "Mobile Citizen Access";
        }

        bool treatSafariAsIphone = false;
        try
        {
            treatSafariAsIphone = Settings.GetValue("TreatSafariAsIphone", typeof(string)).ToString() == "1" ? true : false;
        }
        catch
        {
            treatSafariAsIphone = false;
        }

        String userAgent = Request.UserAgent.ToString();
        StringBuilder styleWork = new StringBuilder();
        bool useiPhoneLogoStyle = false;
        Session["AMCA_App_type"] = "Browser";
        if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "iPhone", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            Session["AMCA_App_type"] = "iPhone";
            iPhoneLineBreak = "<br>";
            useiPhoneLogoStyle = true;
            try
            {
                AccelaApplicationIcon = Settings.GetValue("AccelaApplicationIcon", typeof(string)).ToString();
            }
            catch
            {
                AccelaApplicationIcon = string.Empty;
            }
            if (AccelaApplicationIcon != string.Empty)
            {
                styleWork.Append("<link rel=\"apple-touch-icon\" href=\"" + AccelaApplicationIcon + "\" />");
            }
            else
            {
                styleWork.Append("<link rel=\"apple-touch-icon\" href=\"img/apple-touch-icon.png\" />");
            }
            sBodyWidth = "";
            styleWork.Append("<meta name = \"viewport\" content = \"minimum-scale=1.0, width=device-width, maximum-scale=1.6, user-scalable=no\" />");
            styleWork.Append("<meta name=\"format-detection\" content=\"telephone=no\" />");
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/iphone/iphone.css\" media=\"only screen and (max-width: 480px)\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "Safari", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/Safari/Safari.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "BlackBerry", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleWork.Append("<meta name = \"viewport\" content = \"width = device-width\" />");
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/blackberry/blackberry.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "FireFox", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/firefox/firefox.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "Opera", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            Session["AMCA_App_type"] = "Opera";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/opera/opera.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "MSIE 8", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/explorer8/explorer8.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "MSIE 9", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            || System.Text.RegularExpressions.Regex.IsMatch(userAgent, "MSIE 10", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            || System.Text.RegularExpressions.Regex.IsMatch(userAgent, @"Trident/7.*rv:11", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/explorer9/explorer9.css\" />");
            styleSheet = styleWork.ToString();
        }
        else
        {
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/explorer7/explorer7.css\" />");
            styleSheet = styleWork.ToString();
        }

        if (Session["iPhoneHideLogo"] == null || Session["iPhoneHideLogo"].ToString() == "no")
        {
            PageLogo.Append("<center><table cellpadding=\"0px\" cellspacing=\"0px\">");
            PageLogo.Append("<tr><td rowspan=\"2\" Width=\"" + AgencyLogoHeight.ToString() + "\">");
            PageLogo.Append("<img src=\"" + AgencyLogo + "\" " + AgencyLogoSize + "\" alt=\"Accela\" />");
            PageLogo.Append("</td><td class=\"loginPageAgencyName\">");
            PageLogo.Append(AgencyDescription);
            PageLogo.Append("</td></tr><tr><td class=\"loginPageAppName\">");
            PageLogo.Append(AccelaApplicationName);
            PageLogo.Append("</td></tr>");
            PageLogo.Append("</table></center>");

            if (useiPhoneLogoStyle != true)
            {
                PageLogo.Append("<hr />");
            }
        }
        else
        {
            if (Session["iPhoneHidelogo"] != null)
            {
                Session.Remove("iPhoneHideLogo");
            }
        }
        if (GetFieldValue("State", false) == string.Empty && GetFieldValue("login", false) == string.Empty)
        {
            PageDirection = "";
        }
        else  if (GetFieldValue("SlidePage", false) == "LeftToRight")
        {
            PageDirection = "LeftToRight";
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
