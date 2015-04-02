/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MasterPage.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: MasterPage.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\lytton.cheng $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
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

public partial class MasterPage : System.Web.UI.MasterPage
{
    public String styleSheet = String.Empty;
    public string sBodyWidth = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        String userAgent = Request.UserAgent.ToString();

        StringBuilder styleWork = new StringBuilder();

        if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "iPhone", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleWork.Append("<meta name = \"viewport\" content = \"width = device-width\" />");
            styleWork.Append("<meta name=\"format-detection\" content=\"telephone=no\" />");
            styleWork.Append("<style type=\"text/css\">");
            styleWork.Append(" hr {margin-top:4px; margin-bottom:4px; line-height:4px;}");
            styleWork.Append("</style>");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "BlackBerry", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleWork.Append("<meta name = \"viewport\" content = \"width = device-width\" />");
            styleWork.Append("<style type=\"text/css\">");
            styleWork.Append(" hr {margin-top:4px; margin-bottom:4px; line-height:4px;}");
            styleWork.Append("</style>");
            styleSheet = styleWork.ToString();
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "FireFox", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleSheet = string.Empty;
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "MSIE 8", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            sBodyWidth = "";
            styleWork.Append("<style type=\"text/css\">");
            styleWork.Append(" hr {margin-top:8px; margin-bottom:8px;  line-height:8px;}");
            styleWork.Append("</style>");
            styleSheet = styleWork.ToString();
        }
        else
        {
            sBodyWidth = "";
            styleWork.Append("<meta name = \"viewport\" content = \"width = 400px\" />");
            styleWork.Append("<style type=\"text/css\">");
            styleWork.Append(" hr {margin-top:-4px; margin-bottom:-4px;  line-height:4px;}");
            styleWork.Append("</style>");
            styleSheet = styleWork.ToString();
        }
    }
}
