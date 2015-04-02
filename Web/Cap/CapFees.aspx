<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Cap.CapFees"
    MasterPageFile="~/Default.master" ValidateRequest="false" CodeBehind="CapFees.aspx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/CapFeeList.ascx" TagName="CapFeeList" TagPrefix="ACA"%>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<script type="text/javascript" src="../Scripts/Dialog.js"></script>
<div id="divCapFeesContainer" class="ACA_Area_CapFees">
    <script type="text/javascript">
        window.history.forward(1);     
    </script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(PageLoaded);

        var NeedAsk = true;
        var errMessage = '<%=ErrorMessage %>';

        function PageLoaded(sender, args) {
            /*
            Add the CSS class 'CapFeesBeforeSubmit'/'CapFeesAfterSubmit' to the top container.
            Allow agency to customize different style to the same button in different page context.
            */
            var pageContainer = $get('divCapFeesContainer');
            if (pageContainer) {
                if ('<%=IsPayFeeDue %>' == 'True') {
                    Sys.UI.DomElement.addCssClass(pageContainer, 'CapFeesAfterSubmit');
                }
                else {
                    Sys.UI.DomElement.addCssClass(pageContainer, 'CapFeesBeforeSubmit');
                }
            }
        }

        function SetNotAsk(startTimer) {
            NeedAsk = false;
            if (startTimer) {
                window.setTimeout('NeedAsk=true', 1500);
            }
        }

        function print_onclick(url) {
            var a = window.open(url, "_blank", "top=200,left=200,height=600,width=800,status=yes,toolbar=0,menubar=no,location=no,scrollbars=yes");
        }
        
        if (typeof (ExportCSV) != 'undefined') {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(EndRequest);
        }

        function EndRequest(sender, args) {
            //export file.
            ExportCSV(sender, args);
        }
    </script>

    <div class="ACA_CapFeesStyle">        
            <!--<ACA:AccelaLabel ID="per_permitFee_label_estimateFee" Visible="false" runat="server" LabelKey="per_permitFee_label_estimateFee" />-->
            <h1 runat="server" ID="h1PayFeeDue" Visible="False"><ACA:AccelaLabel ID="lblPayFeeDue" Visible="false" runat="server" LabelKey="per_permitFee_lable_payFeeDue" /></h1>
            <ACA:BreadCrumpToolBar ID="BreadCrumpToolBar" runat="server">
            </ACA:BreadCrumpToolBar>
    </div>
    <span id="SecondAnchorInACAMainContent"></span>
    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10" />
        <div class="ACA_Container">
            <ACA:CapFeeList ID="capFeeList" runat="server" />
        </div>
        <div class="ACA_Row fee_button_container">
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="20" />
        <table role='presentation' style="width: 100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 100%">
                    <table role='presentation' cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="divContinueAndConvertApplication" runat="server">
                                <ACA:AccelaButton ID="lnkContinueApplication" LabelKey="per_permitFee_label_continueApp"
                                    DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_DivMargin6"
                                    runat="server" OnClick="ContinueApplicationButton_Click" OnClientClick="return SubmitEP(this)" />
                                <ACA:AccelaButton ID="lnkConvertApplication" Visible="false" LabelKey="per_fee_label_convertApp"
                                    DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_DivMargin6"
                                    runat="server" OnClick="ToConvertApplicationButton_Click" OnClientClick="return SubmitEP(this)" />
                            </td>
                            <td visible="false" runat="server" id="divAddToShoppingCart">
                                <ACA:AccelaButton ID="lnkAddToShoppingCart" LabelKey="per_shoppingCartList_label_checkOut"
                                    runat="server" OnClick="AddToShoppingCartButton_Click" OnClientClick="return SubmitEP(this)"
                                    DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_DivMargin6" />
                            </td>
                            <td>
                                <ACA:AccelaButton EnableConfigureURL="true" Visible="false" OnClientClick="return SubmitEP(this)"
                                    ID="btnCreateAnotherApplication" LabelKey="per_shoppingCartList_label_createAnotherApplication"
                                    runat="server" OnClick="CreateAnotherApplicationButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_DivMargin6"></ACA:AccelaButton>
                            </td>
                            <td id="divPayAtCounter" runat="server" visible="false">
                                <ACA:AccelaButton ID="lnkPayAtCounter" LabelKey="per_permitFee_label_payAtCounter"
                                    runat="server" OnClick="PayAtCounterButton_Click" OnClientClick="return SubmitEP(this)"
                                    DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_DivMargin6" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
<iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</asp:Content>