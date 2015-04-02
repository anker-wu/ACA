<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.Account.SecurityQuestionVerification" ValidateRequest="false" Codebehind="SecurityQuestionVerification.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
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

        function triggerLogin(e) {
            if (triggerPreventDefault(e, 13)) {
                var obj = e.srcElement || e.target;

                if (!isNullOrEmpty(obj.value)) {
                    ShowLoading();  
                    $("#<%=btnLogin.ClientID %>").focus();
                }

                $("#<%=btnLoginHidden.ClientID %>").click();
            }
        }
    </script>
    <asp:Panel ID="pnlSecurityAnswer" runat="server" CssClass="ACA_RightContent">
        <div>
            <h1>
                <ACA:AccelaLabel ID="lblSecurityQuestionVerify" LabelKey="aca_securityquestion_verification_label_title" runat="server">
                </ACA:AccelaLabel>
            </h1>
            <div class="ACA_Page ACA_Page_FontSize SecurityQuestionUpdateDisclaimer">
                <ACA:AccelaLabel ID="lblDisclaimer" LabelKey="aca_securityquestion_verification_label_disclaimer"
                    runat="server" LabelType="BodyText">
                </ACA:AccelaLabel>
            </div>
            <div class="ACA_TabRow">
                <ACA:AccelaLabel ID="lblSecurityQuestionTitle" CssClass="ACA_Label ACA_Label_FontSize"
                    LabelKey="aca_securityquestion_verification_label_securityquestion" runat="server">
                </ACA:AccelaLabel>
                <p>
                    <ACA:AccelaLabel ID="lblSecurityQuestion" runat="server" /><br />
                    <br />
                </p>
            </div>
            <div class="ACA_TabRow ACA_Label">
                <ACA:AccelaTextBox ID="txtSecurityAnswer" AutoPostBack="false" Validate="required"
                    LabelKey="aca_securityquestion_verification_label_securityanswer" runat="server" CssClass="ACA_XLong">
                </ACA:AccelaTextBox>
            </div>
        </div>
        <div class="ACA_Row ACA_LiLeft BottomActionButton">
            <ul>
                <li>
                    <ACA:AccelaButton ID="btnLogin" runat="server" LabelKey="aca_securityquestion_verification_label_continue"
                        OnClick="LoginButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize">
                    </ACA:AccelaButton>
                    <asp:Button ID="btnLoginHidden" runat="server" OnClick="LoginButton_Click" UseSubmitBehavior="false" Style="display: none;" />
                </li>
                <li>
                    <ACA:AccelaButton ID="btnReturnToLogin" runat="server" LabelKey="aca_securityquestion_verification_label_returntologin"
                        OnClick="ReturnToLoginButton_Click" CausesValidation="False" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize">
                    </ACA:AccelaButton>
                </li>
            </ul>
        </div>
    </asp:Panel>
</asp:Content>
