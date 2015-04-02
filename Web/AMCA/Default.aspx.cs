/**
* <pre>
* 
*  Accela Citizen Access
*  File: _Default.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: _Default.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\Jackie.Yu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-17-2008           DWB                     2008 Mobile ACA interface redesign
*  10-09-2008           DWB                     2008 Mobile ACA 6.7.0 interface redesign
*  04-01-2009           Dave Brewster           Removed "Error:" from error messages.
*
* </pre>
*/
using System;
using System.Configuration;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;

/// <summary>
/// Defaulg LOGIN/ MAIN MENU PAGE
/// </summary>
public partial class _Default : AccelaPage
{
    public string Links = string.Empty;
    public string TitleLabel = string.Empty;
    public StringBuilder OutputLinks = new StringBuilder();
    public string ControlWithFocus = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        AppSettingsReader Settings = new AppSettingsReader();

        //Intercepts the requests for external authentication adapter.
        if (!AuthenticationUtil.IsInternalAuthAdapter)
        {
            if (AuthenticationUtil.IsAuthenticated)
            {
                RedirectUrl(Settings);
                return;
            }
            else
            {
                if (ValidationUtil.IsNo(Request.QueryString[UrlConstant.IS_EXTERNAL_ACCOUNT_ASSOCIATED_PUBLICUSER]))
                {
                    string msg = LocalGetTextByKey("aca_amcassologin_msg_norelationship");
                    OutputLinks.Append(ErrorFormat);
                    OutputLinks.Append(msg);
                    OutputLinks.Append(ErrorFormatEnd);
                }
                else
                {
                    AuthenticationUtil.RedirectToLoginPage();
                }
                return;
            }
        }

        /*
         * Notes: Only internal authentication adapter can execute the following codes.
         */

        // is login valid?
        string State = Request.QueryString["State"];

        // LOGIN SECTION
        string sStyle = string.Empty;

        if ((String.IsNullOrEmpty(State)) 
            || (GetFieldValue("login", false) == "failed") 
            || (GetFieldValue("login", false) == "failed2") )
        {
            string sLoginAbout = FileUtil.AppendApplicationRoot("AMCA/Login.About.aspx?login=true&SlidePage=RightToLeft");

            if (Request.Form.Count != 0)
            {
                string UserName = Request.Form["username"];
                string Psswrd = Request.Form["psswrd"];

                if (!IsValidLogin(UserName, Psswrd))
                {
                    string returnUrl = Server.UrlEncode(Request.QueryString[UrlConstant.RETURN_URL]);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "&" + UrlConstant.RETURN_URL + "=" + returnUrl;
                    }

                    Response.Redirect("Default.aspx?login=failed&Message=" + MyProxy.ExceptionMessage + returnUrl);
                    Response.End();
                    return;
                }

                //The LDAP users which are not existing in ACA system can not login to AMCA.
                if (AppSession.User != null && AccountUtil.IsNewLdapUser(AppSession.User.UserModel4WS))
                {
                    Session.Clear();
                    Response.Redirect(sLoginAbout, true);
                }

                if (SecurityQuestionUtil.GetAuthBySecurityQuestionSetting().Enable
                    && !StandardChoiceUtil.IsEnableLdapAuthentication() 
                    && SecurityQuestionUtil.IsExistActiveQuestion(AppSession.User.UserModel4WS.questions))
                {
                    Response.Redirect("Login.SecurityQuestionVerify.aspx?mode=loginSecurity&SlidePage=RightToLeft");
                }
                else
                {
                    RedirectUrl(Settings);
                }

                Response.End();
                return;
            }
            else
            {
                if (Session["globalSearchPattern"] != null)
                {
                    Session.Remove("globalSearchPattern");
                }

                if (GetFieldValue("login", false) == "failed")
                {
                    OutputLinks.Append(ErrorFormat);
                    OutputLinks.Append(GetFieldValue("Message", false));
                    OutputLinks.Append(ErrorFormatEnd);
                }
                OutputLinks.Append("<div class=\"loginPageGreeting\">");
                OutputLinks.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"><tr><td>Login to access your account</td></tr></table>");
                OutputLinks.Append("</div>");
                OutputLinks.Append("<div class=\"loginControlsWrapper\">");
                OutputLinks.Append("<div id=\"pageText\">");
                OutputLinks.Append("Username:<br><input name=\"username\" class=\"loginPageInput\">");
                OutputLinks.Append("</div>");
                OutputLinks.Append("<div id=\"pageText\">");
                OutputLinks.Append("Password:<br><input type=\"password\" name=\"psswrd\" class=\"loginPageInput\"/>");
                OutputLinks.Append("</div>");
                OutputLinks.Append("<div id=\"pageSubmitButton\">");

                String userAgent = Request.UserAgent;
                if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "iPhone", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    OutputLinks.Append("<center>");
                }
                OutputLinks.Append("<input type=\"submit\" value=\"Login\"/>");
                if (System.Text.RegularExpressions.Regex.IsMatch(userAgent, "iPhone", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    OutputLinks.Append("</center>");
                }

                OutputLinks.Append("</div>"); //pageSubmitButton
                OutputLinks.Append("</div>"); //loginControlsWrapper

                //Hide the Reset password and Create account links if LDAP is enabled.
                if (!StandardChoiceUtil.IsEnableLdapAuthentication())
                {
                    string sRestPassword = FileUtil.AppendApplicationRoot("AMCA/Login.ResetPassword.aspx?login=true&SlidePage=RightToLeft");

                    OutputLinks.Append("<table width=\"95%\" cellpadding=\"0px\" cellspacing=\"0\">");
                    OutputLinks.Append("<tr><td class=\"loginPageLinks\">");
                    OutputLinks.Append("<a href=\"");
                    OutputLinks.Append(sRestPassword);
                    OutputLinks.Append("\">I have forgotten my Password </a>");
                    OutputLinks.Append("</td></tr><tr><td class=\"loginPageLinks\">");
                    OutputLinks.Append("<a href=\"");
                    OutputLinks.Append(sLoginAbout);
                    OutputLinks.Append("\">Don't have an account? </a>");
                    OutputLinks.Append("</td></tr></table>");
                }

                return;
            }
        }
        else
        {
            sStyle = "<a style=\"color:#040478; text-decoration:underline;\" href=\"";

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
            catch (Exception)
            {
                Response.Redirect("Default.aspx");
                Response.End();
            }

            Response.Redirect("Login.Home.aspx?State=" + State);
        }
    }

    /// <summary>
    /// checks to see if the current login is valid
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Psswrd"></param>
    /// <returns></returns>
    private Boolean IsValidLogin(string UserName, string Psswrd)
    {
        return MyProxy.IsUserLoginValid(UserName, Psswrd);
    }

    /// <summary>
    /// Redirect Back to selected feature after user logged in.
    /// </summary>
    /// <param name="Settings">The settings.</param>
    private void RedirectUrl(AppSettingsReader Settings)
    {
        string returnUrl = Server.UrlDecode(Request.QueryString[UrlConstant.RETURN_URL]);
        hash MyHasher = new hash();
        MyHasher.setPublicKey(Settings.GetValue("TokenKey2", typeof(string)).ToString());
        string value = MyHasher.encrypt(((DateTime.Now.ToString().Split(' ')[0]) + "^ValidLogin"));

        //If have reutrn Url, it means Redirect From Request Feature. it will Redirect back to the Request URL.else if will redirect to default url setting in webcofig.
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = "Login.Home.aspx";
        }

        if (returnUrl.IndexOf('?') >= 0)
        {
            returnUrl += "&State=" + value;
        }
        else
        {
            returnUrl += "?State=" + value;
        }

        Response.Redirect(returnUrl);
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
                   ? Request.Form[FieldName] : string.Empty);
        }
        catch
        {
            TheValue = string.Empty;
        }
        if (IsRequired == true && TheValue == "")
        {
            // IsRequiredDataValid = false;
        }
        return TheValue;
    }
}
