<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Cap.CapType"
    MasterPageFile="~/Default.master" Codebehind="CapType.aspx.cs" %>

<%@ Reference Page="~/Cap/UserLicenseList.aspx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PlaceHolderMain">

    <script type="text/javascript"> 
        function SelectType(obj)
        {
            hideMessage();
        }
    </script>
    <div id="cap_type" class="ACA_RightItem" style="margin-bottom:0px">
        <h1 runat="server" ID="h1SelectType">
            <ACA:AccelaLabel ID="per_applyPermit_label_selectType" LabelKey="per_applyPermit_label_selectType"
                runat="server"></ACA:AccelaLabel>
        </h1>
        <div id="divBlankLineTitle" runat="server" visible="false"><br /></div>
        <h1 runat="server" ID="h1SelectAmendmentType">
            <ACA:AccelaLabel ID="per_applyPermit_label_selectAmendmentType" LabelKey="per_applyPermit_label_selectAmendmentType"
                runat="server"></ACA:AccelaLabel>
        </h1>
        <ACA:BreadCrumpToolBar ID="BreadCrumpToolBar" runat="server"></ACA:BreadCrumpToolBar>
        <span id="SecondAnchorInACAMainContent"></span>
        <ACA:AccelaHeightSeparate ID="sepForCaptype" runat="server" Height="12" />
        <div class="ACA_Page ACA_Page_FontSize">
            <ACA:AccelaLabel ID="per_applyPermit_text_selectType" LabelKey="per_applyPermit_text_selectType"
                runat="server" LabelType="BodyText"></ACA:AccelaLabel>
        </div>
        <div id="divBlankLineBody" runat="server" visible="false"><br /></div>
        <div class="ACA_Page ACA_Page_FontSize">
            <ACA:AccelaLabel ID="per_applyPermit_text_selectAmendmentType" LabelKey="per_applyPermit_text_selectAmendmentType"
                runat="server" LabelType="BodyText"></ACA:AccelaLabel>
        </div>
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="5" />
    </div>
    <div class="ACA_RightItem">
        <ACA:AccelaDropdownList ID="ddlBoardTypeSelection" runat="server"  
            ToolTipLabelKey="aca_common_msg_dropdown_selectrecordtypecategory_tip"
            onselectedindexchanged="BoardTypeSelectionCheckBox_SelectedIndexChanged" 
            AutoPostBack="True" Width="250px" />
    </div>
    <div class="ACA_RightItem">
        <ACA:AccelaRadioButtonList ID="PermitTypeSelect" onclick="SelectType(this);" runat="server" role='presentation'
            CssClass="ACA_Label ACA_Label_FontSize_Smaller" />
        <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="28" />
            <ACA:AccelaButton ID="btnContinue" LabelKey="per_applyPermit_label_continueApp"
                runat="server" OnClick="ContinueButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_Button_Text" />
    </div>
</asp:Content>
