/**
* <pre>
* 
*  Accela Citizen Access
*  File: Home.Help.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  Help text for users that forget thier password or do not have an account.
* 
*  Notes:
*      $Id: Login.About.aspx.cs 107118 2008-10-09 22:50:52Z dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-18-2008           DWB           New form - 2008 Mobile ACA interface redesign
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
using Accela.ACA.Web.Common;

public partial class LoginAbout : AccelaPage
{
    public StringBuilder BackForwardLinks = new StringBuilder();
    public string HorizontalRow = string.Empty;
    public string LoginPageText = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        LoginPageText = LabelUtil.GetTextByKey("login_about_page_text", null);

        if (isiPhone == true)
        {
            iPhonePageTitle = "No Account";
            BackForwardLinks.Append(BackLinkHelper("login").ToString());
            // HorizontalRow = "<hr>";
            Session["iPhoneHideLogo"] = "yes";
            // BackForwardLinks.Append("<center>");
        }
        else
        {
            BackForwardLinks.Append("<div id=\"backLink\">");
            if (isiPhone == true)
            {
                HorizontalRow = "<hr>";
                BackForwardLinks.Append("<center>");
            }
            BackForwardLinks.Append("<a href=\"Default.aspx?SlidePage=LeftToRight&login=failed2\">");
            if (isiPhone != true)
            {
                BackForwardLinks.Append("<< ");
            }
            BackForwardLinks.Append("Return to login page.");
            BackForwardLinks.Append("</a>");
            if (isiPhone == true)
            {
                BackForwardLinks.Append("</center>");
            }
            BackForwardLinks.Append("</div>");
        }
    }
}
