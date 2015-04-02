<%@ Control Language="C#" AutoEventWireup="true"
 Inherits="Accela.ACA.Web.Component.LoginBox" Codebehind="LoginBox.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Web.SocialMedia" %>
<%@ Register src="Recaptcha.ascx" tagname="Recaptcha" tagprefix="ACA" %>

<script type="text/javascript">
    function triggerLogin(e) {
        if (triggerPreventDefault(e, 13)) {
            $("#<%=btnHidden.ClientID %>").click();

            if (typeof (Page_IsValid) != "undefined" && Page_IsValid) {
                ShowLoading();
                $("#<%=btnLogin.ClientID %>").focus();
            }
        }
    }
</script>

<div id="fb-root"></div>
<%if(!AppSession.IsAdmin && !string.IsNullOrEmpty(ConfigManager.FacebookAppId)){ %>
<script type="text/javascript">
//insert Facebook JS ref in document.
(function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/all.js#xfbml=1&appId=<%= ConfigManager.FacebookAppId %>";
        fjs.parentNode.insertBefore(js, fjs);
    } (document, 'script', 'facebook-jssdk'));

    function FacebookLogin()
    {
        //Using Facebook login button, return the login status.
        FB.getLoginStatus(function(response) {
            if (response.authResponse) {
                window.location.href = '<%=SocialMediaUtil.PortalPageUrl %>?access_token=' + response.authResponse.accessToken 
                + '&expires=' + response.authResponse.expiresIn;
            }
        });
    }
</script>
<%} %>
<input type="hidden" runat="server" id="hdnValidateResubmit" />
<div class="ACA_LoginBox">
    <div class="ACA_LoginBoxTitle">
	    <ACA:AccelaLabel ID="AccelaLabel1" LabelKey="acc_sign_label_login" CssClass="font13px" runat="server"></ACA:AccelaLabel>
    </div>
    <div class="ACA_TabRow">
	    <ACA:AccelaTextBox runat="server" ID="txtUserId" MaxLength="50" CssClass="ACA_NLonger"
		    Validate="required;maxlength" LabelKey="acc_sign_label_username" HideRequireIndicate="true" />	  
    </div>
    <div class="ACA_TabRow">
	    <ACA:AccelaPasswordText runat="server" ID="txtPassword" CssClass="ACA_NLonger" Validate="required"
		    LabelKey="acc_sign_label_password" EnableStrengthValidate="false" HideRequireIndicate="true" autocomplete="off"/>
    </div>
    <div id="divRecaptcha" class="ACA_TabRow" runat="server">
        <ACA:Recaptcha ID="reCaptcha" runat="server" />
    </div>
    <div>
	    <ACA:AccelaButton ID="btnLogin" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" LabelKey="acc_sign_label_loginBtn" runat="server"
            OnClick="LoginButton_Click">
        </ACA:AccelaButton>
        <asp:Button ID="btnHidden" runat="server" OnClick="LoginButton_Click" UseSubmitBehavior="false" Style="display: none;" />
        <%if (!string.IsNullOrEmpty(ConfigManager.FacebookAppId)) { %>
        <div class="fb-login-button" onlogin="FacebookLogin()" autologoutlink="false" data-show-faces="false" data-width="200" data-max-rows="1"></div>
        <%} %>
    </div>
    <ACA:AccelaHeightSeparate ID="heightseparate1" runat="server" Height="5"/>
    <div class="ACA_TabRow ACA_Line_Content"></div>
    <ACA:AccelaHeightSeparate ID="heightseparate2" runat="server" Height="5"/>
    <div class="ACA_TabRow aca_checkbox aca_checkbox_fontsize">
		<input type="checkbox" name="publicly_owned" runat="server" id="chkRemember" />
		<ACA:AccelaLabel ID="acc_sign_label_rememberMe" AssociatedControlID="chkRemember" LabelKey="acc_sign_label_rememberMe" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" runat="server"/>
    </div>
    <div class="ACA_TabRow" id="divForgotPassword" runat="server">
        <span class="ACA_FRight">
            <a id="hrefForgotPWD" runat="server">
                <ACA:AccelaLabel ID="lblForgotPWD" LabelKey="acc_sign_label_findpassword" runat="server" CssClass="ACA_Link_Text ACA_Link_Text_FontSize"/>
            </a>
        </span>
    </div>
    <div class="ACA_TabRow" id="divRegisterAccount" runat="server">
        <span class="ACA_FRight">
            <a id="hrefRegisterAccount" runat="server"><strong>
                <ACA:AccelaLabel ID="acc_sign_label_newUser" LabelKey="acc_sign_label_newUser" runat="server" CssClass="ACA_Link_Text ACA_Link_Text_FontSize"/></strong>
                <ACA:AccelaLabel ID="acc_sign_label_registerAccount" LabelKey="acc_sign_label_registerAccount" CssClass="ACA_Link_Text ACA_Link_Text_FontSize" runat="server"/>
            </a>
	    </span>
    </div>
</div>