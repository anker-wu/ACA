<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Cap.CapPayment"
    MasterPageFile="~/Default.master" ValidateRequest="false" CodeBehind="CapPayment.aspx.cs" %>

<%@ Register Src="../Component/Payment.ascx" TagName="Payment" TagPrefix="uc1" %>
<%@ Import Namespace="Accela.ACA.Web.Payment" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PlaceHolderMain">

    <script type="text/javascript">
    
       var NeedAsk = true;
	   $.global.RealBalanceValue = NaN;
	   
       if (window.history) 
       {
           window.history.forward(1);
       } 
       window.onunload = DoAdjustHeight;

       function DoAdjustHeight()
       {
           AdjustHeight(<%=GetIFramePageHeight()%>);
       }
    </script>

    <div class="ACA_CapFeesStyle">
        <h1 runat="server" ID="h1PayFee" Visible="False">
            <ACA:AccelaLabel ID="per_permitPayFee_label_payFee" Visible="false" LabelKey="per_permitPayFee_label_payFee"
                runat="server" /></h1>
        <ACA:BreadCrumpToolBar IsForShoppingCart="true" ID="BreadCrumpShoppingCart" runat="server">
        </ACA:BreadCrumpToolBar>
        <ACA:BreadCrumpToolBar ID="ucBreadCrumpToolBar" runat="server">
        </ACA:BreadCrumpToolBar>
    </div>
    <span id="SecondAnchorInACAMainContent"></span>
    <div class="ACA_RightContent">
        <ACA:AccelaHeightSeparate ID="sepForHeader" runat="server" Height="10" />
        <div>
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="per_permitPayFee_text_payFeeOption" LabelKey="per_permitPayFee_text_payFeeOption"
                    runat="server" LabelType="BodyText" />
            </div>
            <div class="ACA_TabRow" runat="server" id="divIndication">
                <span class="ACA_SmLabel ACA_SmLabel_FontSize ACA_FRight"><span class="ACA_Required_Indicator">
                    *</span>
                    <ACA:AccelaLabel ID="per_permitPayFee_label_indicate" LabelKey="per_permitPayFee_label_indicate"
                        runat="server" />
                </span>
            </div>
            <div runat="server" id="divPaymentMethod">
                <ACA:AccelaLabel ID="lblSelectedPaymentMethod" LabelKey="per_permitpayfee_label_paymentmethod"
                    runat="server" LabelType="SectionTitle" />
            </div>
        </div>
        <div>
            <uc1:Payment id="Payment" runat="Server">
            </uc1:Payment>
        </div>
    </div>
</asp:Content>
