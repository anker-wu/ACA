<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Default.master" 
Inherits="Accela.ACA.Web.Account.AccountVerification" Codebehind="AccountVerification.aspx.cs" %>

<%@ Register Src="~/Component/LoginBox.ascx" TagName="loginBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <script type="text/javascript">
    if(typeof(top.SetLanguageOptionsVisible)!="undefined")
        top.SetLanguageOptionsVisible(true);
    window.onunload=function(){
        if(typeof(top.SetLanguageOptionsVisible)!="undefined")
            top.SetLanguageOptionsVisible(false);
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
        // resolve the iframe session timeout issue
        window.onload = function()
        {
            var p = window.parent;
            var p2 = p;
            var isTop = true;
            while(p.parent != p)
            {
                isTop = false;
                p2 = p;
                p = p.parent;
            }
            if(!isTop)
            {
                p2.location.href = window.location.href;
            }
        }
    </script>
 <div class="ACA_Content">
   <table role='presentation' width="100%" >
    <tr style="vertical-align:top;">
    <td>
     <div class="ACA_RightContent" id="divLeft" runat="server">
        <div id="divSuccess" runat="server">
            <h1>
                <ACA:AccelaLabel ID="IbILoginTitle" LabelKey="acc_login_label_login" runat="server">
                </ACA:AccelaLabel>
            </h1>
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="IbISuccessInfo" LabelKey="acc_verification_account_success_Tip"
                    runat="server" LabelType="BodyText"></ACA:AccelaLabel>
            </div>
        </div>
        <div id="divFail" runat="server" visible="false">
            <h1>
                <ACA:AccelaLabel ID="IbIexpireTitle" LabelKey="acc_verification_label_expire"
                    runat="server"></ACA:AccelaLabel>
            </h1>
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="IbIFailInfo" LabelKey="acc_verification_account_fail_Tip"
                    runat="server" LabelType="BodyText"></ACA:AccelaLabel>
            </div>
        </div>
    </div>
    </td>
    <td>
    <!-- Begin LoginBox -->
   <uc1:loginBox ID="LoginBox" runat="server" />
    <!-- End LoginBox -->
   </td>
   </tr>
  </table> 
 </div>
</asp:Content>