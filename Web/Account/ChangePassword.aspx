<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" 
    Inherits="Accela.ACA.Web.Account.ChangePassword" Title="Change Password Page" ValidateRequest="false" %>
<%@ Register Src="~/Component/PasswordSecurityBar.ascx" TagName="PasswordSecurityBar" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<%
    Response.Expires = -1;
    Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
    Response.AddHeader("pragma", "no-cache");
    Response.AddHeader("Cache-Control", "no-cache");
    Response.CacheControl = "no-cache";
    Response.Cache.SetNoStore();
    Response.Cache.SetCacheability(HttpCacheability.NoCache);
%>
<script type="text/javascript">
window.history.forward();

var globalEvent;
var globalValidators;
var passwordSecurityValidatorValid = true;
var repeatCallTimes=0;
var passwordErrorMsg = "";

// If the password field is empty, use default css style.
function password_onblur()
{
    var passwordID = "<%=txbNewPassword1.ClientID %>";

    var password = GetValueById(passwordID);
    
    if (password == "")
    {
         setToDefault();
         return;
    }
}


function CheckPasswordSecurity_onblur(source, arguments)
{
    var passwordID = "<%=txbNewPassword1.ClientID %>";
    var userNameID = "<%=txbUserID.ClientID %>";
   
    var password = GetValueById(passwordID);
    var userName = GetValueById(userNameID);
    
    if (password == "")
    {
         setToDefault();
         return;
    }
    
    var validators = source.previousSibling.Validators;
    if (validators == null) {
        validators = source.previousSibling.previousSibling.Validators;
    }
    if (repeatCallTimes < 1) {
        globalValidators = validators;
        PageMethods.CheckPasswordSecurity(password, userName, PasswordCallSuccess);
    }
    else {
        arguments.IsValid = passwordSecurityValidatorValid;
        repeatCallTimes = 0;
    }
    
    if (!passwordSecurityValidatorValid)
    {
        source.errormessage = passwordErrorMsg;
        arguments.IsValid=passwordSecurityValidatorValid;
    }
}

function PasswordCallSuccess(result)
{
    var elemID = "<%=txbNewPassword1.ClientID %>";

    passwordSecurityValidatorValid = true;    
    var json = eval('(' + result + ')');
    if ((parseInt(json.ErrorCode) >= 100))
    {
        var obj = document.getElementById(elemID);
        passwordSecurityValidatorValid= false;
        passwordErrorMsg = json.ErrorMessage;  
    }
    else
    {
        passwordErrorMsg = "";
    }
    
    doChangePassworStrengthdBar(json.ErrorCode, json.ErrorMessage, json.PwdScore);
    
    repeatCallTimes++;   
    for (var i = 0; i < globalValidators.length; i++)
    {
        ValidatorValidate(globalValidators[i], null, globalEvent);
    }
    ValidatorUpdateIsValid(); 
}

</script>
<div class="ACA_Content">
    <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_change_password" LabelType="PageInstruction"
                runat="server" />
    <ACA:AccelaLabel ID="lblChangePasswordTitle" LabelKey="aca_change_password_label_title"
        runat="server" LabelType="SectionTitle" />

    <div class="ACA_TabRow" id="divUserName" runat="server">
        <ACA:AccelaTextBox runat="server" MinLength="4" MaxLength="32" ID="txbUserID" CssClass="ACA_XLong ACA_ReadOnly"
            ReadOnly="true" Validate="required" LabelKey="aca_change_password_label_user" />
    </div>
    <div class="ACA_TabRow" id="divOldPassword" runat="server">
        <ACA:AccelaPasswordText ID="txbOldPassword" runat="server" CssClass="ACA_XLong"
            Validate="required;minlength;maxlength" LabelKey="aca_change_password_label_oldpassword" 
            TextMode="Password" MinLength="0" MaxLength="21" autocomplete="off"></ACA:AccelaPasswordText>
    </div>
    <div class="ACA_TabRow" id="divNewPassword1" runat="server">
    <table role='presentation'>
    <tr valign ="bottom">
    <td>
        <ACA:AccelaPasswordText ID="txbNewPassword1" runat="server" CssClass="ACA_XLong"
            LabelKey="aca_change_password_label_newpassword"
            TextMode="Password" MinLength="8" MaxLength="21" autocomplete="off"></ACA:AccelaPasswordText>
    </td>
    </tr>
    <tr>
    <td>
        <uc1:PasswordSecurityBar ID="ucPasswordSecurityBar1" runat="server"/>
    </td>
    </tr>
    </table>
    </div>
    <div class="ACA_TabRow" id="divNewPassword2" runat="server">
        <ACA:AccelaPasswordText ID="txbNewPassword2" runat="server" CssClass="ACA_XLong"
            Validate="required;compare" LabelKey="aca_change_password_label_confirmpassword"
            TextMode="Password" MaxLength="21" ToCompare="txbNewPassword1" autocomplete="off"></ACA:AccelaPasswordText>
    </div>
    <ACA:AccelaHeightSeparate ID="heightSeparate"  runat="server" Height="25"/>
    <ACA:AccelaButton ID="btnSubmit" runat="server" CausesValidation="true" LabelKey="aca_change_password_label_buttonsubmit" 
        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" OnClick="SubmitButton_Click"></ACA:AccelaButton>
</div>
</asp:Content>
