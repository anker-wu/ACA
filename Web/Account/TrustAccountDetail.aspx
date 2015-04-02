<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="TrustAccountDetail.aspx.cs"
    Inherits="Accela.ACA.Web.TrustAccount.TrustAccountDetail" %>

<%@ Register Src="../Component/TrustAccountDetail.ascx" TagName="TrustAccountDetail"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/RefAddressList.ascx" TagName="RefAddressList" TagPrefix="uc1" %>
<%@ Register Src="../Component/RefParcelList.ascx" TagName="RefParcelList" TagPrefix="uc1" %>
<%@ Register Src="../Component/PeopleList.ascx" TagName="PeopleList" TagPrefix="uc1" %>
<%@ Register Src="../Component/TransactionsList.ascx" TagName="TransactionsList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
         prm.add_endRequest(EndRequest);

         function EndRequest(sender, args){
             //export file.
             ExportCSV(sender, args);
         }
    </script>

    <div id="MainContent" class="ACA_Content">
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_trustaccount_detail" LabelType="PageInstruction" runat="server" />
        <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title=""><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
        <ACA:AccelaLabel ID="lblTitle" runat="server" LabelKey="per_trustaccountdetail_title"
            LabelType="SectionTitle" />
        <uc1:TrustAccountDetail ID="trustAcctDetail" runat="server" />
        <ACA:AccelaButton ID="btnDeposit" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize"
            LabelKey="per_trustaccountdetail_deposit" runat="server" OnClick="DepositButton_OnClick" />
        <ACA:AccelaHeightSeparate ID="sepForPermitDetail" runat="server" Height="10" />
        <ACA:AccelaLabel ID="lblAssociatedAddress" runat="server" LabelKey="per_trustaccountdetail_title_associatedaddress"
            LabelType="SectionTitle" />
        <uc1:RefAddressList ID="refAddressList" runat="server" />
        <ACA:AccelaHeightSeparate ID="sepForParcel" runat="server" Height="15" />
        <ACA:AccelaLabel ID="lblAssociatedParcel" runat="server" LabelKey="per_trustaccountdetail_title_associatedparcel"
            LabelType="SectionTitle" />
        <uc1:RefParcelList ID="refParcelList" runat="server" />
        <ACA:AccelaHeightSeparate ID="sepForPeople" runat="server" Height="15" />
        <ACA:AccelaLabel ID="lblPeople" runat="server" LabelKey="per_trustaccountdetail_title_people"
            LabelType="SectionTitle" />
        <uc1:PeopleList ID="peopleList" runat="server"/>
        <ACA:AccelaHeightSeparate ID="sepForTransaction" runat="server" Height="15" />
        <ACA:AccelaLabel ID="lblTransactions" runat="server" LabelKey="per_trustaccountdetail_title_transactions"
            LabelType="SectionTitle" />
        <uc1:TransactionsList ID="transactionsList" runat="server" />
    </div>
</asp:Content>
