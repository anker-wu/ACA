<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Account.RegisterConfirm" ValidateRequest="false" Codebehind="RegisterConfirm.aspx.cs" %>
<%@ Register Src="~/Component/AccountContactView.ascx" TagName="ContactInfoView" TagPrefix="ACA" %>
<%@ Register Src="~/Component/UserLicenseList.ascx" TagName="UserLicenseList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div class="ACA_RightContent ACA_RegisterConfirm_Width">
        <asp:Panel ID="panRegisterInfo" runat="server">
            <div class="ACA_Row">
                <ACA:MessageBar ID = "registerSuccessInfo" runat="Server" />
            </div>
            <ACA:AccelaLabel ID="lblRegisterSuccessCongratulaterInfo" LabelKey="acc_registerSuccessInfo_label_congratulate" LabelType="bodyText" runat="server"></ACA:AccelaLabel>
            <ACA:AccelaHeightSeparate ID="sepForDescription" runat="server" Height="10" />
            <ACA:AccelaLabel ID="lblAccInfo" LabelKey="acc_reg_label_accountInfo"
                runat="server" LabelType="SectionTitle">
            </ACA:AccelaLabel>
            <ACA:ContactInfoView ID="ContactInfo" runat="server" />
            <ACA:AccelaHeightSeparate ID="sepForContact" runat="server" Height="10" />
            <div id="licenseInfoTitle" runat="server" visible="false">
                <ACA:AccelaLabel ID="lblLicenseInfo" LabelKey="acc_reg_label_licenseInfo"
                    runat="server" LabelType="SectionTitle">
                </ACA:AccelaLabel>
            </div>
            <ACA:UserLicenseList ID="userLicenseList" runat="server" />
            <div class="AutoLoginButton_RegisterConfirm">
            <ACA:AccelaButton ID="btnLoginNow" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" LabelKey="aca_registerconfirm_label_loginnow" runat="server"
                OnClick="LoginNowButton_Click"/>
            </div>
        </asp:Panel>
        <div id="divBack" runat="server" visible="false" class="ACA_Row ACA_LiLeft ACA_LinkButton">
            <ul>
                <li>
                    <ACA:AccelaLinkButton ID="btnBack" LabelKey="aca_authagent_registerclerkcomplete_label_back" CssClass="font13px"
                        CausesValidation="false" OnClientClick="window.location.href='AccountManager.aspx';var p = new ProcessLoading();p.showLoading(true);return false;" runat="server"></ACA:AccelaLinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>