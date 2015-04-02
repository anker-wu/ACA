<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/Default.master" CodeBehind="DepositCompletion.aspx.cs"
    Inherits="Accela.ACA.Web.Account.DepositCompletion" %>

<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <script type="text/javascript">
    function print_onclick(url)
    {
        var a=window.open(url, "_blank","top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
    }      
    </script>
    <div class="ACA_RightContent">
        <h1>
            <ACA:AccelaLabel ID="lblDepositCompletionTitle" runat="server" LabelKey="aca_depositcompletion_label_title"/>
        </h1>
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate0" runat="server" Height="5" />
        <div class="ACA_Row">
            <uc1:MessageBar ID="DepositSuccessInfo" runat="Server" />
        </div>
        <div class="ACA_Row">
            <p>
                <ACA:AccelaLabel ID="lblWelcomeInfo" runat="server" LabelKey="aca_depositcompletion_text_welcomeinfo" /></p>
        </div>
        <div class="ACA_Row" style="font-size: 1.3em; color: #003366; font-weight: bold;">
            <ACA:AccelaLabel ID="lblDepositCount" runat="server"/>
        </div>
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="50" />
        <div class="ACA_RightPadding">
            <ACA:AccelaButton ID="lnkPrintReceipt" CssClass="NotShowLoading" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize"
                LabelKey="aca_depositcompletion_printreceipt" runat="server" ReportID="0" /></div>
        <div runat="server" id="divRowLine" class="ACA_TabRow ACA_Line_Content">
        </div>
        <div class="ACA_Row">
            <p>
                <ACA:AccelaLabel LabelType="BodyText" ID="per_permitIssuance_text_license" runat="server"
                    LabelKey="aca_depositcompletion_text_comment"/>
            </p>
        </div>
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="15" />
        <div class="ACA_Row" runat="server">
            <ACA:AccelaButton ID="lnkViewAccountDetail" LabelKey="aca_depositcompletion_label_viewaccontdetail" runat="server" OnClick="ViewAccountDetailButton_Click"
                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" />
        </div>
    </div>
</asp:Content>
