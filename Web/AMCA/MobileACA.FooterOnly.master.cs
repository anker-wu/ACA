/**
* <pre>
* 
*  Accela Citizen Access
*  File: MobileACA.FooterOnly.master.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
*  Master page for MobileACA project. 
* 
*  Notes:
*      $Id: MobileACA.FooterOnly.master.cs 124745 2009-03-23 16:57:26Z dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  08-4-2008           DWB           2008 Mobile ACA interface redesign
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

public partial class MobileACAFooterOnly : System.Web.UI.MasterPage
{
    public string AgencyDescription = string.Empty;
    public string AgencyCopyright = string.Empty;
    public string styleSheet = string.Empty;
    public string sBodyWidth = string.Empty;
    public string AccelaApplicationIcon = string.Empty;
    public string PageDirection = "RightToLeft";
    public string DebugSection = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        AppSettingsReader Settings = new AppSettingsReader();

        // AgencyLogo = "img/MobileACA_Demo_Logo.jpg";
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

        bool debugMode = false;
        try
        {
            debugMode = Settings.GetValue("debugMode", typeof(string)).ToString() == "1" ? true : false;
        }
        catch
        {
            debugMode = false;
        }

        String userAgent = Request.UserAgent.ToString();
        DebugSection = userAgent;
        StringBuilder styleWork = new StringBuilder();
        Session["AMCA_App_type"] = "Browser";
        if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "iPhone", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            DebugSection = "<p>iPhone(7.0.5)</p>";
            Session["AMCA_App_type"] = "iPhone";
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
            DebugSection = "<p>Safari(7.0.5)</p>";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/Safari/Safari.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "BlackBerry", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            DebugSection = "<p>BlackBerry(7.0.5)</p>";
            sBodyWidth = "";
            styleWork.Append("<meta name = \"viewport\" content = \"width = device-width\" />");
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/blackberry/blackberry.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "FireFox", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            DebugSection = "<p>FireFox (7.0.5)</p>";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/firefox/firefox.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "Opera", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            Session["AMCA_App_type"] = "Opera";
            DebugSection = "<p>Opera(7.2.0 FP4)</p>";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/opera/opera.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "MSIE 8", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            DebugSection = "<p>MSIE-8(7.0.5)</p>";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/explorer8/explorer8.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "MSIE 9", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            DebugSection = "<p>MSIE-9(7.2.0)</p>";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/explorer9/explorer9.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "MSIE 10", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            DebugSection = "<p>MSIE-10(7.3.1)</p>";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/explorer9/explorer9.css\" />");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "Trident/7.*rv:11", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            DebugSection = "<p>MSIE-11(7.3.2)</p>";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/explorer9/explorer9.css\" />");
            styleSheet = styleWork.ToString();
        }
        else
        {
            DebugSection = "<p>MSIE-7(7.0.5)</p>";
            sBodyWidth = "";
            styleWork.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"customize/explorer7/explorer7.css\" />");
            styleSheet = styleWork.ToString();
        }

        if (GetFieldValue("SlidePage",false) == "LeftToRight")
        {
            PageDirection = "LeftToRight";
        }
        if (debugMode == false)
            DebugSection = string.Empty;
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
        return TheValue;
    }


}
