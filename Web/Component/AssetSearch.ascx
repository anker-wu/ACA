<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetSearch.ascx.cs" Inherits="Accela.ACA.Web.Component.AssetSearch" %>
<asp:UpdatePanel ID="panAssetSearchForm" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divAssetSearchPanel" runat="server">
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaTextBox ID="txtAssetId" CssClass="ACA_NShot" MaxLength="100" runat="server" LabelKey="aca_assetsearch_label_assetid" />
            <ACA:AccelaDropDownList ID="ddlAssetGroup" runat="server" LabelKey="aca_assetsearch_label_assetgroup" AutoPostBack="True" OnSelectedIndexChanged="DdlAssetGroup_IndexChanged"
                 ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip" />
            <ACA:AccelaDropDownList ID="ddlClassType" runat="server" LabelKey="aca_assetsearch_label_classtype" />
            <ACA:AccelaDropDownList ID="ddlAssetType" runat="server" LabelKey="aca_assetsearch_label_assettype" />
            <ACA:AccelaTextBox ID="txtCounty" runat="server" LabelKey="aca_assetsearch_label_county" />
            <ACA:AccelaTextBox ID="txtState" runat="server" LabelKey="aca_assetsearch_label_state" />
            <ACA:AccelaCountryDropDownList ID="ddlCountryRegion" runat="server" LabelKey="aca_assetsearch_label_countryregion" />
            <ACA:AccelaTextBox ID="txtAssetName" CssClass="ACA_NShot" MaxLength="70" runat="server" LabelKey="aca_assetsearch_label_assetname" />
            <ACA:AccelaTextBox ID="txtCity" CssClass="ACA_NShot" MaxLength="70" runat="server" LabelKey="aca_assetsearch_label_city" />
            <ACA:AccelaTextBox ID="txtNeighborhood" CssClass="ACA_NShot" MaxLength="70" runat="server" LabelKey="aca_assetsearch_label_neighborhood" />
            <ACA:AccelaRangeNumberText ID="txtStreetStart" CssClass="ACA_NShot" MaxLength="9" runat="server" LabelKey="aca_assetsearch_label_streetstart"
                 ToolTipLabelKeyRangeFrom="aca_assetsearch_label_streetstartfrom" ToolTipLabelKeyRangeTo="aca_assetsearch_label_streetstartto" />
            <ACA:AccelaRangeNumberText ID="txtStreetEnd" CssClass="ACA_NShot" MaxLength="9" runat="server" LabelKey="aca_assetsearch_label_streetend" 
                 ToolTipLabelKeyRangeFrom="aca_assetsearch_label_streetendfrom" ToolTipLabelKeyRangeTo="aca_assetsearch_label_streetendto"/>
            <ACA:AccelaTextBox ID="txtStreetName" CssClass="ACA_NShot" MaxLength="70" runat="server" LabelKey="aca_assetsearch_label_streetname" />
            <ACA:AccelaTextBox ID="txtPrefix" runat="server" LabelKey="aca_assetsearch_label_prefix" />
            <ACA:AccelaTextBox ID="txtZipCode" CssClass="ACA_NShot" MaxLength="70" runat="server" LabelKey="aca_assetsearch_label_zipcode" />
            <ACA:AccelaDropDownList ID="ddlStreetType" runat="server" LabelKey="aca_assetsearch_label_streettype" />
            <ACA:AccelaDropDownList ID="ddlStreetSuffix" runat="server" LabelKey="aca_assetsearch_label_streetsuffix" />
        </ACA:AccelaFormDesignerPlaceHolder>
        </div>
        <table role='presentation' class="popup_table_border_collapse">
          <tr valign="bottom">
            <td>
                 <ACA:AccelaButton ID="btnSearch" runat="server" LabelKey="aca_assetsearch_label_buttonsearch"
                    CausesValidation="true" OnClick="BtnSearch_Click" OnClientClick="SetNotAskForSPEAR()"
                    DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"></ACA:AccelaButton>
            </td>
             <td class="PopupButtonSpace">
                &nbsp;
            </td>
            <td>
                 <ACA:AccelaButton ID="btnClear" runat="server" LabelKey="aca_assetsearch_label_buttonclear"
                    OnClientClick="SetNotAskForSPEAR()" OnClick="BtnClear_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                    DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" CausesValidation="false"></ACA:AccelaButton>
            </td>
             <td class="PopupButtonSpace">
                &nbsp;
            </td>
            <td>
                <div class="ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="btnCancel" runat="server" CssClass="NotShowLoading" CausesValidation="false" LabelKey="aca_assetsearch_label_buttoncancel" 
                        OnClientClick="parent.SetNotAskForSPEAR();parent.ACADialog.close();"></ACA:AccelaLinkButton>
                </div>
            </td>
        </tr>
    </table>

        <ACA:AccelaInlineScript ID="AccelaInlineScript1" runat="server">
            <script type="text/javascript">
                $("#<%=divAssetSearchPanel.ClientID %>").keydown(function (event) {
                    pressEnter2TriggerClick(event, $("#<%=btnSearch.ClientID %>"));
                });
            </script>
        </ACA:AccelaInlineScript>
    </ContentTemplate>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        if (typeof (myValidationErrorPanel) != "undefined") {
            myValidationErrorPanel.registerIDs4Recheck("<%=btnClear.ClientID %>");
        }
    });
    
    //Redirect to result page
    function RedirectToResult(moduleName, args) {
        var url = '<%=FileUtil.AppendApplicationRoot("/Asset/AssetResult.aspx") %>?module=' + moduleName + '&args=' + args + '&isPopup=Y';
        window.location.href = url;
    }
</script>
