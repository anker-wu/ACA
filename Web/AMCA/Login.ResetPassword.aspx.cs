/**
* <pre>
* 
*  Accela Citizen Access
*  File: Login.ResetPassword.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
*  Reset user password.
* 
*  Notes:
*      $Id: Login.ResetPassword.aspx.cs 125780 2009-04-01 17:45:00Z dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-04-2008           DWB                     New form - 2008 Mobile ACA interface redesign
*  04/01/2009           Dave Brewster           Modified error message by removing the word "ERROR".
* </pre>
*/
using System;
using System.Text;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

/// <summary>
/// Defaulg LOGIN/ MAIN MENU PAGE
/// </summary>
public partial class LoginResetPassword : AccelaPage
{
    public StringBuilder PageContent = new StringBuilder();
    public string NextPage = string.Empty;
    public StringBuilder BackForwardLinks = new StringBuilder();
    public string HorizontalRow = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //Cannot use the "Reset password" functionality if the LDAP is enabled.
        if(StandardChoiceUtil.IsEnableLdapAuthentication())
        {
            Response.Redirect("Default.aspx");
        }

        string sStyle = string.Empty;

        if (isiPhone == true)
        {
            iPhonePageTitle = "Reset Password";
            BackForwardLinks.Append(BackLinkHelper("login").ToString());
            // HorizontalRow = "<hr>";
            Session["iPhoneHideLogo"] = "yes";
            // BackForwardLinks.Append("<center>");
        }
        else
        {
            BackForwardLinks.Append("<div id=\"backLink\">");
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
        if (Request.QueryString.Count == 0 || GetFieldValue("mode", false) == "email" || GetFieldValue("login", false) == "true")
        {
            if (GetFieldValue("result", false) == "verify")
            {
                // try to get user by email
                string eMmail = Request.Form["email"].ToString();
                PublicUserModel4WS publicUser = GetPublicUserByEmail(eMmail);

                if (publicUser == null)
                {
                    Response.Redirect("Login.ResetPassword.aspx?mode=email&result=invalid&SlidePage=LeftToRight&login=true");
                    Response.End();
                    return;
                }
                else if (publicUser.questions == null)
                {
                    ResetPassword();

                    Response.Redirect("Login.ResetPassword.aspx?mode=security&result=success&SlidePage=RightToLeft&login=security");
                    Response.End();
                    return;
                }

                Response.Redirect("Login.ResetPassword.aspx?mode=security&SlidePage=RightToLeft&login=security");
                Response.End();
                return;
            }
            else
            {
                NextPage = "Login.ResetPassword.aspx?mode=email&result=verify&SlidePage=RightToLeft&login=true";
                if (GetFieldValue("result", false) == "invalid")
                {
                    string errorMessage = "The email address you entered is not present in our records.";
                    PageContent.Append(ErrorFormat);
                    PageContent.Append(errorMessage);
                    PageContent.Append(ErrorFormatEnd);
                }

                if (isiPhone == false)
                {
                    PageContent.Append("<div id=\"pageTitle\">");
                    PageContent.Append("<label>Reset Password</label>");
                    PageContent.Append("</div>");
                }
                PageContent.Append("<div id=\"pageText\">");
                PageContent.Append("<label class=\"loginPageGreeting\">If you've forgotten your password we will send you a new one. To begin provide your email address below.</label>");
                PageContent.Append("</div>");

                PageContent.Append("<div class=\"loginControlsWrapper\">");
                PageContent.Append("<div id=\"pageText\">");
                PageContent.Append("Email address:<br><input name=\"email\"  class=\"loginPageInput\" /><br>");
                PageContent.Append("</div>");
                PageContent.Append("<div id=\"pageSubmitButton\">");
                if (isiPhone == true)
                {
                    PageContent.Append("<center>");
                }
                PageContent.Append("<input type=\"submit\" value=\"Next\"/>");
                if (isiPhone == true)
                {
                    PageContent.Append("</center>");
                }
                PageContent.Append("</div>");
                PageContent.Append("</div>"); //loginControlsWrapper
                return;
            }
        }
        if (GetFieldValue("mode", false) == "security")
        {
            if (GetFieldValue("result", false) == "verify")
            {
                // try to get user by email
                string sAnswer = Request.Form["answer"].Trim().ToUpper();
                
                if (!Session["TheAnswer"].ToString().ToUpper().Equals(sAnswer.ToUpper()))
                {
                    Response.Redirect("Login.ResetPassword.aspx?mode=security&result=invalid&SlidePage=LeftToRight&login=security");
                    Response.End();
                    return;
                }

                ResetPassword();

                Response.Redirect("Login.ResetPassword.aspx?mode=security&result=success&SlidePage=RightToLeft&login=security");
                Response.End();
                return;
            }
            if (GetFieldValue("result", false) == "success" ) 
            {
                NextPage = "Login.ResetPassword.aspx?mode=security&result=success&SlidePage=RightToLeft&login=security";
                if (isiPhone == false)
                {
                    PageContent.Append("<div id=\"pageTitle\">");
                    PageContent.Append("<label>Reset Password</label>");
                    PageContent.Append("</div>");
                }
                PageContent.Append("<p id=\"pageText\">Your password has been reset.  An e-mail has been sent containing your new password.  Please use the new password to login.</p>");
                return;
            }
            NextPage = "Login.ResetPassword.aspx?mode=security&result=verify";
            if (isiPhone == false)
            {
                PageContent.Append("<div id=\"pageTitle\">");
                PageContent.Append("<label>Reset Password</label>");
                PageContent.Append("</div>");
            }
            if (GetFieldValue("result", false) == "invalid")
            {
                PageContent.Append(ErrorFormat);
                PageContent.Append("You entered an incorrect Security Answer.  Please check your answer and try again");
                PageContent.Append(ErrorFormatEnd);
            }

            PageContent.Append("<div id=\"pageText\";>");
            PageContent.Append("<label class=\"loginPageGreeting\">The security question you selected when you first registered is displayed below.  Please provide your security answer so we may verify your identity.</label>");
            PageContent.Append("</div>");

            string TheQuestion = string.Empty;

            //Generate security question when enter this page first time.
            if (AppSession.User.UserModel4WS.questions != null && GetFieldValue("result", false) != "invalid")
            {
                PublicUserQuestionModel questionModel = SecurityQuestionUtil.GenerateSecurityQuestionRandomly(AppSession.User.UserModel4WS.questions);
                TheQuestion = questionModel.questionValue;
                Session["TheQuestion"] = TheQuestion;
                Session["TheAnswer"] = questionModel.answerValue.Trim();
            }
            else
            {
                TheQuestion = Convert.ToString(Session["TheQuestion"]);
            }

            PageContent.Append("<div id=\"pageText\">");
            PageContent.Append("<label id=\"pageTextSectionName\">Security Question:</label><br>");
            PageContent.Append("<label class=\"loginPageGreeting\">" + ScriptFilter.EncodeHtml(TheQuestion) + "</label>");
            PageContent.Append("</div>");
            PageContent.Append("<div class=\"loginControlsWrapper\">");
            PageContent.Append("<div id=\"pageText\">");
            PageContent.Append("<label>Security Answer:</label>");
            PageContent.Append("<br><input name=\"answer\" class=\"loginPageInput\"/>");
            PageContent.Append("</div>");
            PageContent.Append("<div id=\"pageSubmitButton\">");
            if (isiPhone == true)
            {
                PageContent.Append("<center>");
            }
            PageContent.Append("<input type=\"submit\" value=\"Send New Password\"/>");
            if (isiPhone == true)
            {
                PageContent.Append("</center>");
            }
            PageContent.Append("</div>");
            PageContent.Append("</div>"); //loginControlsWrapper
            return;
        }
    }

    /// <summary>
    /// Get public user by email address.
    /// </summary>
    /// <param name="email">Email Address</param>
    /// <returns>public user model</returns>
    private PublicUserModel4WS GetPublicUserByEmail(string email)
    {
        return MyProxy.GetPublicUserByEmail(email);
    }

    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="FieldName"> Field name </param>
    /// <param name="IsRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
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
    /// Reset the users password and email the user a new password.
    /// </summary>
    /// <returns>bool</returns>
    private bool ResetPassword()
    {
        IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();

        try
        {
            accountBll.ResetPassword(ConfigManager.AgencyCode, AppSession.User.UserID);
            return true;
        }
        catch 
        {
            return false;
        }
    }


}
