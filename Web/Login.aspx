<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Default.master"
    Inherits="Accela.ACA.Web.Login" Codebehind="Login.aspx.cs" %>
   
<%@ Register Src="~/Component/LoginBox.ascx" TagName="loginBox" TagPrefix="uc1" %>  
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <script type="text/javascript">
    if(typeof(top.SetLanguageOptionsVisible)!="undefined")
        top.SetLanguageOptionsVisible(true);
    window.onunload=function(){
        if(typeof(top.SetLanguageOptionsVisible)!="undefined")
            top.SetLanguageOptionsVisible(false);
            void(0);
        }

        $(function() {
        if (typeof (divAccessibility) != 'undefined') {
            if ($("#" + divAccessibility).get(0)) {
                $("#" + divAccessibility).show();
                }
            }
        });
    </script>
    <script type="text/javascript">

        function noBack() {
            window.history.forward();
        }
        noBack();
        window.onpageshow=function(evt) { if (evt.persisted)noBack(); }
        // resolve the iframe session timeout issue
        window.onload = function() {
            noBack();
            
            var p = window.parent;
            var p2 = p;
            var isTop = true;
            
            while (p.parent != p) {
                isTop = false;
                p2 = p;
                p = p.parent;
            }
            
            if (!isTop) {
                p2.location.href = window.location.href;
            }
        }
    </script>
<div class="ACA_Content">
   <table role='presentation' width="100%" border="0" cellpadding="0" cellspacing="0"><tr style="vertical-align:top;"><td>
    <!-- Start Login Page -->
    <div class="ACA_RightContent" id="divLeft" runat="server">
        <div id="divLogin" runat="server">
            <!-- Start Login Page Content -->
            <h1>
                <ACA:AccelaLabel ID="acc_login_label_login" LabelKey="acc_login_label_login" runat="server"></ACA:AccelaLabel></h1>
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="acc_login_text_loginTip" LabelKey="acc_login_text_loginTip"
                    runat="server" LabelType="BodyText"></ACA:AccelaLabel><br />
                <br />
            </div>
        </div>
        <div id="divRegistration" runat="server">
            <!-- Start Registration Page Content -->
            <h1>
                <ACA:AccelaLabel ID="acc_login_label_newUser" LabelKey="acc_login_label_newUser"
                    runat="server"></ACA:AccelaLabel></h1>
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="acc_login_text_registerTip1" LabelKey="acc_login_text_registerTip1"
                    runat="server" LabelType="BodyText"></ACA:AccelaLabel></div>
            <ACA:AccelaHeightSeparate runat="server" Height="35" />
            <ACA:AccelaButton ID="btnRegisterNow" CausesValidation="false" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" OnClick="RegisterNowButton_Click" runat="server" LabelKey="acc_login_label_registerNow"></ACA:AccelaButton>
            <div class="ACA_TabRow">
                <p>
                    <ACA:AccelaLabel ID="acc_login_sublabel_registerNow" LabelKey="acc_login_sublabel_registerNow"
                        runat="server"></ACA:AccelaLabel></p>
            </div>
        </div>
    </div>
   </td>
   <td> 
    <!-- End Login Page Content -->
    <!-- Begin LoginBox -->
   <uc1:loginBox ID="LoginBox" runat="server" />
    <!-- End LoginBox -->
   </td></tr></table> 
</div>
</asp:Content>
