/**
* <pre>
* 
*  Accela Citizen Access
*  File: LoginSecurityQuestionVerify.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: LoginSecurityQuestionVerify.aspx.cs 77905 2014-2-25 12:49:28Z ACHIEVO\andy.zhong $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*
* </pre>
*/

using System;
using System.Configuration;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

/// <summary>
/// Login security question verify class
/// </summary>
public partial class LoginSecurityQuestionVerify : AccelaPage
{
    public StringBuilder PageContent = new StringBuilder();
    public string NextPage = "Login.SecurityQuestionVerify.aspx?mode=loginSecurity&SlidePage=RightToLeft&result=verifying";
    public StringBuilder BackForwardLinks = new StringBuilder();
    public string HorizontalRow = string.Empty;

    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string mode = Request.QueryString["mode"];
        string answer = Request.Form["answer"];
        string result = Request.QueryString["result"];
        PublicUserQuestionModel questionModel = (PublicUserQuestionModel)Session["questionModel"];

        //Generate security question when enter this page first time.
        if (string.IsNullOrWhiteSpace(result))
        {
            questionModel = SecurityQuestionUtil.GenerateSecurityQuestionRandomly(AppSession.User.UserModel4WS.questions);
            Session["questionModel"] = questionModel;
        }

        string questionValue = ScriptFilter.EncodeHtml(questionModel.questionValue);

        if ("invalid".Equals(result))
        {
            PageContent .Append(ErrorFormat);
            PageContent.Append(Request.QueryString["Message"]);
            PageContent.Append(ErrorFormatEnd);
        }

        PageContent.AppendFormat(@"
                <div class='loginPageGreeting'>
                    <table width='100%' cellpadding='0' cellspacing='0'><tr><td>Login to access your account</td></tr></table>
                </div>
                <div class='loginControlsWrapper'>
                    <div id='pageText'>
                        Security Question:<br><label class='loginPageGreeting'>{0}</label>
                    </div>
                    <div id='pageText'>
                        Security Answer:<br><input name='answer' class='loginPageInput'/>
                    </div>
                    <div id='pageSubmitButton'>
                        <input type='submit' value='Continue'/>
                        <input type='button' value='Return to Login' onclick='{1}'/>
                    </div>
                </div>",
                questionValue,
                "location.href=\"Default.aspx?SlidePage=RightToLeft\";");

        if ("loginSecurity".Equals(mode) && "verifying".Equals(result))
        {
            bool isAnswerCorrect = questionModel.answerValue.Trim().Equals(answer.Trim());
            string returnUrl = Server.UrlEncode(Request.QueryString[UrlConstant.RETURN_URL]);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "&" + UrlConstant.RETURN_URL + "=" + returnUrl;
            }

            if (MyProxy.IsUserLockedByInvalidAnswer(isAnswerCorrect))
            {
                Response.Redirect("Default.aspx?login=failed&Message=" + MyProxy.ExceptionMessage + returnUrl);
                Response.End();
            }
            else if (!isAnswerCorrect)
            {
                string invalidMsg = MyProxy.GetTextByKey("aca_securityquestion_verification_msg_invalid", string.Empty);
                Response.Redirect("Login.SecurityQuestionVerify.aspx?mode=loginSecurity&SlidePage=RightToLeft&result=invalid&Message=" + invalidMsg + returnUrl);
                Response.End();
            }
            else
            {
                AppSettingsReader Settings = new AppSettingsReader();
                hash MyHasher = new hash();
                MyHasher.setPublicKey(Settings.GetValue("TokenKey2", typeof(string)).ToString());
                string value = MyHasher.encrypt(((DateTime.Now.ToString().Split(' ')[0]) + "^ValidLogin"));

                returnUrl = Server.UrlDecode(Request.QueryString[UrlConstant.RETURN_URL]);

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
        }
    }
}