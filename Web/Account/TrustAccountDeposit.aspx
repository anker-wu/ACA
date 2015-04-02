<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Default.master" CodeBehind="TrustAccountDeposit.aspx.cs"
    Inherits="Accela.ACA.Web.Account.TrustAccountDeposit" %>

<%@ Register Src="../Component/Payment.ascx" TagName="Payment" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PlaceHolderMain">
   <%-- <div class="ACA_CapFeesStyle">
        <h1>
            <ACA:AccelaLabel ID="lbl" Visible="false" LabelKey="per_permitPayFee_label_payFee"
                runat="server" /></h1>
    </div>--%>
    <div class="ACA_RightContent">
        <div>
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="lblInstructionForTrustAccountDesposite" LabelKey="aca_trustaccountdeposit_label_payfeeoption"
                    runat="server" LabelType="BodyText" />
            </div>
            <div class="ACA_TabRow" runat="server" id="div1">
                <span class="ACA_SmLabel ACA_SmLabel_FontSize ACA_FRight"><span class="ACA_Required_Indicator">
                    *</span>
                    <ACA:AccelaLabel ID="per_permitPayFee_label_indicate" LabelKey="aca_trustaccountdeposit_label_indicate"
                        runat="server" />
                </span>
            </div>
            <div runat="server" id="divDepositMethod">
                <ACA:AccelaLabel ID="lblSelectedDepositMethod" LabelKey="aca_trustaccountdeposit_label_paymentmethod"
                    runat="server" LabelType="SectionTitle" />
            </div>
        </div>
        <div>
            <uc1:Payment ID="Payment" runat="Server"></uc1:Payment>
        </div>
    </div>
</asp:Content>
