<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.AccountView" Codebehind="AccountView.ascx.cs" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls" TagPrefix="ACA" %>

<asp:UpdatePanel ID="accountViewPanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="btnPostback" runat="Server" OnClick="PostbackButton_Click" CssClass="ACA_Hide" TabIndex="-1"></asp:LinkButton>
<div>
    <table role='presentation' class="ACA_Page_FontSize">
        <tr>
            <td class="ACA_FLeft ACA_Page ACA_MLonger">
                <ACA:AccelaLabel ID="acc_userInfoDisplay_label_user" LabelKey="acc_userInfoDisplay_label_user" runat="server" /> 
            </td>
            <td class="ACA_FLeft ACA_Page">                
                <ACA:AccelaLabel ID="lblUser" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ACA_FLeft ACA_Page ACA_MLonger">    
                <ACA:AccelaLabel ID="acc_userInfoDisplay_label_email" LabelKey="acc_userInfoDisplay_label_email" runat="server" />
            </td>
            <td class="ACA_FLeft ACA_Page">  
                <ACA:AccelaLabel ID="lblEmail" runat="server" />
            </td>
        </tr>
        <tr id="passwordInfo" runat="server">
            <td class="ACA_FLeft ACA_Page ACA_MLonger">
                <ACA:AccelaLabel ID="acc_userInfoDisplay_label_password" LabelKey="acc_userInfoDisplay_label_password" runat="server" />
            </td>
            <td class="ACA_FLeft ACA_Page">
                <ACA:AccelaLabel ID="lblPassword" runat="server" />
            </td>
        </tr>
        <tr id="secQuestionInfo" Visible="False" runat="server">
            <td class="ACA_FLeft ACA_Page ACA_MLonger">
                <ACA:AccelaLabel ID="lblSecQuestion" LabelKey="acc_userInfoDisplay_label_securityQuestion" runat="server" />
            </td>
            <td class="ACA_FLeft ACA_Page">
            </td>
        </tr>
        <%=SecurityQuestionBuilder.ToString() %>
        <tr id="mobile" runat="server">
            <td class="ACA_FLeft ACA_Page ACA_MLonger">
                <ACA:AccelaLabel ID="lblCellPhone_Label" LabelKey="aca_account_label_cellphone" runat="server" />
            </td>
            <td class="ACA_FLeft ACA_Page">
                <ACA:AccelaLabel ID="lblCellPhone" IsNeedEncode="false" runat="server" />
            </td>
        </tr>
        <tr id="receiveSMS" runat="server">
            <td class="ACA_FLeft ACA_Page ACA_MLonger">
                <ACA:AccelaLabel ID="lblReceiveSMS_Label" LabelKey="aca_account_label_receivesms" runat="server" />
            </td>
            <td class="ACA_FLeft ACA_Page">
                <ACA:AccelaLabel ID="lblReceiveSMS" runat="server" />
            </td>
        </tr>
    </table>
    <ACA:AccelaHeightSeparate ID="sepHeightForAccount" runat="server" Height="5" />
</div>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function RefreshAccountView() {
        __doPostBack("<%=btnPostback.UniqueID%>", '');
    }
</script>