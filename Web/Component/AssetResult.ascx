<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetResult.ascx.cs" Inherits="Accela.ACA.Web.Component.AssetResult" %>
<script type="text/javascript" language="javascript">
    with (Sys.WebForms.PageRequestManager.getInstance()) {
        if ('<%=AppSession.IsAdmin%>'.toLowerCase() == 'false') {
            add_endRequest(onEndRequest);
        }
    }

    function onEndRequest(sender, arg) {
        var sourceElement = sender._postBackSettings.sourceElement;

        if (sourceElement == null || (sourceElement != null && $(sourceElement).hasClass('NotShowLoading') == false)) {
            var processLoading = new ProcessLoading();
            processLoading.hideLoading();
        }

        //export file.
        ExportCSV(sender, arg);
    }

    function AdjustPopDialogHeight() {
        var ifrm = document.getElementById("ACADialogFrame");
        var assetDetailBodyHeight = ifrm.contentWindow.document.body.scrollHeight;
        var assetDetailDocumentElementHeight = ifrm.contentWindow.document.documentElement.scrollHeight;
        minHeight = Math.min(assetDetailBodyHeight, assetDetailDocumentElementHeight) > 550 ? 550 : Math.min(assetDetailBodyHeight, assetDetailDocumentElementHeight);
    }

    function ShowAssetDetail(moduleName, isShowAction, args, lnkRelatedRecordID) {
        var url = '<%=FileUtil.AppendApplicationRoot("/Asset/AssetDetail.aspx") %>?module=' + moduleName + '&isShowAction=' + isShowAction + '&args=' + args + '&fromSearch=Y';
        ACADialog.popup({ url: url, width: 750, height: 500, objectTarget: lnkRelatedRecordID });
    }

    if (!$.global.isAdmin) {
        var dialogCloseBtn = parent.$get(parent.ACADialog.close_id);

        if (dialogCloseBtn) {
            dialogCloseBtn.onclick = function() {
                CloseAssetResultDialog('0', '0');
                return false;
            };
        }
    }

    function CloseAssetResultDialog(successCount, failCount) {
        parent.SetNotAskForSPEAR();
        parent.ACADialog.close();
		
        if (successCount == "0" && failCount == "0") {
            return;
        }
        parent.RefreshAssetList(successCount, failCount);
    }

</script>

<asp:UpdatePanel ID="updatePanel" runat="server" class="asset_searchresult">
    <ContentTemplate>
        <div id="divResultNotice" runat="server" visible="false" class="ACA_Row">
            <p>
                <ACA:AccelaLabel ID="lblResultNotice" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </p>
        </div>
        <ACA:AccelaGridView ID="gvAssetResultList" runat="server" IsInSPEARForm="true" PagerStyle-HorizontalAlign="center"
            AllowPaging="true" AllowSorting="true" OnRowDataBound="GvAssetResultList_RowDataBound" 
            SummaryKey="aca_summary_asset_result" CaptionKey="aca_caption_asset_searchresult"
            PageSize="10" AutoGenerateCheckBoxColumn="true" CheckBoxColumnIndex="0" ShowCaption="true"
            GridViewNumber="60171" OnPageIndexChanging="GvAssetResultList_PageIndexChanging"
            OnGridViewDownload="GvAssetResultList_GridViewDownload" OnGridViewSort="GvAssetResultList_GridViewSort">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkAssetID" ExportDataField="g1AssetID">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAssetID" runat="server" CommandName="Header" SortExpression="g1AssetID"
                                LabelKey="aca_assetsearchresult_label_assetid" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLinkButton ID="lnkAssetID" runat="server" CausesValidation="False" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetID") %>' />
                            <ACA:AccelaLabel ID="lblAssetSeqNum" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetSequenceNumber") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="180px" />
                    <HeaderStyle Width="180px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAssetName" ExportDataField="g1AssetName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAssetName" runat="server" CommandName="Header" SortExpression="g1AssetName"
                                LabelKey="aca_assetsearchresult_label_assetname" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAssetName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetName") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="160px" />
                    <HeaderStyle Width="160px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAssetGroup" ExportDataField="g1AssetGroup">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAssetGroup" runat="server" CommandName="Header" SortExpression="g1AssetGroup"
                                LabelKey="aca_assetsearchresult_label_assetgroup" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAssetGroup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetGroup") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="160px" />
                    <HeaderStyle Width="160px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAssetType" ExportDataField="g1AssetType">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAssetType" runat="server" SortExpression="g1AssetType"
                                CommandName="Header" LabelKey="aca_assetsearchresult_label_assettype" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAssetType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetType") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="128px" />
                    <HeaderStyle Width="128px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStreetStart" ExportDataField="streetStart">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkStreetStart" runat="server" SortExpression="streetStart" CommandName="Header"
                                LabelKey="aca_assetsearchresult_label_streetstart" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblStreetStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "streetStart") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="128px" />
                    <HeaderStyle Width="128px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStreetEnd" ExportDataField="streetEnd">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkStreetEnd" runat="server" SortExpression="streetEnd" CommandName="Header"
                                LabelKey="aca_assetsearchresult_label_streetend" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblStreetEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "streetEnd") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="128px" />
                    <HeaderStyle Width="128px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCounty" ExportDataField="county">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCounty" runat="server" SortExpression="county" CommandName="Header"
                                LabelKey="aca_assetsearchresult_label_county" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCounty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "county") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="128px" />
                    <HeaderStyle Width="128px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCountry" ExportDataField="countryCode">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCountry" runat="server" SortExpression="countryCode"
                                CommandName="Header" LabelKey="aca_assetsearchresult_label_countryregion" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCountry" runat="server" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="128px" />
                    <HeaderStyle Width="128px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="ACA_Row ACA_LiLeft" runat="server" id="divAttach" visible="False">
    <table role='presentation' class="popup_table_border_collapse">
        <tr valign="bottom">
            <td>
                <ACA:AccelaButton ID="btnAttach" runat="server" CausesValidation="false" LabelKey="aca_assetsearch_label_buttonattach"
                    OnClick="BtnAttach_Click" OnClientClick="SetNotAskForSPEAR()" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                    DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"></ACA:AccelaButton>
            </td>
            <td class="PopupButtonSpace">
                &nbsp;
            </td>
            <td>
                <div class="ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="btnBack" runat="server" CausesValidation="false" LabelKey="aca_assetresult_label_backbutton"
                        OnClick="BtnBack_Click" OnClientClick="SetNotAskForSPEAR()"></ACA:AccelaLinkButton>
                </div>
            </td>
        </tr>
    </table>
    <iframe width="0" height="0" id="iframeExport" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>" class="ACA_Hide">
        <%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</div>
