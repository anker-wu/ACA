<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowGeoDocuments.aspx.cs"
   MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.GIS.ShowGeoDocuments" %>

<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Component/GeoDocumentList.ascx" TagName="GeoDocumentList" TagPrefix="uc1" %>

<asp:content id="Content1" contentplaceholderid="phPopup" runat="server">
    <script type="text/javascript" src="../Scripts/textCollapse.js"></script>
    <script type="text/javascript">
        function CloseDialog() {
            parent.ACADialog.close();
        }
    </script>
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div id="dvCondition" runat="server">
             <div class="ACA_Row">
                <ACA:AccelaLabel CssClass="font11px" ID="lblChooseHistory" LabelKey="aca_showgeodocuments_label_choosehistory" runat="server"></ACA:AccelaLabel>
            </div>
            <div class="ACA_Row">
                <ACA:AccelaCheckBox ID="ckbIsHistorical" LabelKey="aca_showgeodocuments_label_ishistory" Checked="true" runat="server" />
            </div>
            <div id="dvDocType" runat="server">
            <div class="ACA_Row">
                <ACA:AccelaLabel CssClass="font11px" ID="lblDocTypes" LabelKey="aca_showgeodocuments_label_selectdoctypes" runat="server"></ACA:AccelaLabel>
            </div>
            <div class="ACA_Row">
                <ACA:AccelaCheckBox ID="ckbALL" LabelKey="aca_showgeodocuments_label_selectall"  runat="server" AutoPostBack="true" OnCheckedChanged="CheckALL_CheckedChanged" />
            </div>
            <div class="documenttype_margin">
                <ACA:AccelaCheckBoxList ID="ckbDocType"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="DocTypeCheckBox_SelectedIndexChanged" ></ACA:AccelaCheckBoxList>
            </div>
            </div>
            <table class="documenttype_button" role='presentation'>
                <tr valign="bottom">
                    <td>
                        <ACA:AccelaButton ID="btnSubmit" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="aca_showgeodocuments_label_submit" OnClick="SubmitButton_Click" runat="server"></ACA:AccelaButton>
                    </td>
                    <td  class="ACA_XShoter"></td>
                    <td>
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkCancel" LabelKey="aca_showgeodocuments_label_cancel" runat="server" OnClientClick="CloseDialog();"></ACA:AccelaLinkButton>
                    </div>
                    </td>
                </tr>
            </table> 
            </div>       
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="20" runat="server"></ACA:AccelaHeightSeparate>
        <div id="dvDocList" runat="server">
            <uc1:GeoDocumentList ID="docList" runat="Server"></uc1:GeoDocumentList>
        <div class="ACA_Row">
            <uc1:MessageBar ID = "noDataMessageDocList" runat="Server" />
        </div>
        </div>
       </ContentTemplate>
    </asp:UpdatePanel>
</asp:content>
