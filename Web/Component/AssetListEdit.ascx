<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetListEdit.ascx.cs" Inherits="Accela.ACA.Web.Component.AssetListEdit" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<div class="ACA_Row">
    <ACA:AccelaButton ID="btnLookUp" runat="server" LabelKey="aca_assetlist_label_lookupbutton" CssClass="asset_list_lookupbutton"
        CausesValidation="false" OnClientClick="SetNotAskForSPEAR()" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"></ACA:AccelaButton>
    <asp:LinkButton ID="btnRefreshAssetList" runat="Server" CssClass="ACA_Hide" OnClick="BtnRefreshAssetList_Click"
        TabIndex="-1"></asp:LinkButton>
</div>
<asp:UpdatePanel ID="mapPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:ACAMap ID="mapAsset" AGISContext="AssetDetailSpear" GISButtonLabelKey="aca_assetsearch_label_btngissearch"
            OnShowOnMap="MapAsset_ShowOnMap" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="panAssetList" runat="server" UpdateMode="Conditional" class="asset_section">
    <ContentTemplate>
        <div id="divActionNotice" class="ACA_Row" runat="server" visible="false">
            <table role="presentation">
               <tr>
                   <td>
                        <div runat="server" enableviewstate="False" id="divImgSuccess" visible="false">
                            <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_workflow_complete") %>" src="<%=ImageUtil.GetImageURL("complete.png") %>" />
                        </div>
                        <div runat="server" enableviewstate="False" id="divImgFailed" visible="false">
                            <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>" />
                        </div>
                   </td>
                   <td>
                        <div class="asset_attach_result_notice">
                            <ACA:AccelaLabel ID="lblAttachAssetSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False" LabelKey="aca_assetresult_label_attachsuccess" Visible="false" />
                            <ACA:AccelaLabel ID="lblAttachAssetExisted" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_assetresult_label_attachexisted" Visible="false" />
                            <ACA:AccelaLabel ID="lblAttachNoAssociated" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_assetresult_label_attachnoassetbygis" Visible="false" />
                            <ACA:AccelaLabel ID="lblRemoveAssetSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False" LabelKey="aca_assetlist_label_removeresult" Visible="false" />
                        </div>
                   </td>
               </tr>
            </table>
        </div>
        <div id="divAssetList" runat="server" visible="false">
            <ACA:AccelaGridView ID="gvAssetList" GridViewNumber="60173" runat="server" Visible="false" 
                SummaryKey="aca_summary_asset_list" CaptionKey="aca_caption_asset_list"
                PageSize="10" ShowCaption="true" AllowSorting="true" AllowPaging="true" OnRowDataBound="GvAssetList_RowDataBound"
                IsInSPEARForm="true" OnGridViewSort="GvAssetList_GridViewSort" OnPageIndexChanging="GvAssetList_PageIndexChanging"
                PagerStyle-HorizontalAlign="center">
                <Columns>
                    <ACA:AccelaTemplateField AttributeName="lnkAssetID">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkAssetID" runat="server" SortExpression="g1AssetID"
                                    CommandName="Header" LabelKey="aca_assetlist_label_assetid" CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLinkButton ID="lnkAssetID" runat="server" CausesValidation="False" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetID") %>' />
                                <ACA:AccelaLabel ID="lblAssetSeqNum" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetSequenceNumber") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="128px" />
                        <HeaderStyle Width="128px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkAssetName">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkAssetName" runat="server" SortExpression="g1AssetName"
                                    CommandName="Header" LabelKey="aca_assetlist_label_assetname" CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblAssetName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetName") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="120px" />
                        <HeaderStyle Width="120px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkAssetGroup">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkAssetGroup" runat="server" SortExpression="g1AssetGroup"
                                    CommandName="Header" LabelKey="aca_assetlist_label_assetgroup" CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblAssetGroup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "g1AssetGroup") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="128px" />
                        <HeaderStyle Width="128px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkAssetType">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkAssetType" runat="server" SortExpression="g1AssetType"
                                    CommandName="Header" LabelKey="aca_assetlist_label_assettype" CausesValidation="false"></ACA:GridViewHeaderLabel>
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
                                    LabelKey="aca_assetlist_label_streetstart" CausesValidation="false"></ACA:GridViewHeaderLabel>
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
                                    LabelKey="aca_assetlist_label_streetend" CausesValidation="false"></ACA:GridViewHeaderLabel>
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
                                    LabelKey="aca_assetlist_label_county" CausesValidation="false"></ACA:GridViewHeaderLabel>
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
                                    CommandName="Header" LabelKey="aca_assetlist_label_countryregion" CausesValidation="false"></ACA:GridViewHeaderLabel>
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
                    <ACA:AccelaTemplateField ColumnId="Action" AttributeName="lnkAction">
                        <HeaderTemplate>
                            <div>
                                <ACA:AccelaLabel IsGridViewHeadLabel="true" ID="lnkAction" runat="server" LabelKey="aca_assetlist_label_action"
                                    IsNeedEncode="false"></ACA:AccelaLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_FLeft">
                                <ACA:AccelaLinkButton ID="lnkRemove" runat="server" CommandName='<%# COMMAND_REMOVE_ASSET %>'
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "g1AssetID").ToString() + ACAConstant.SPLIT_CHAR + DataBinder.Eval(Container.DataItem, "g1AssetSequenceNumber")%>'
                                    OnClick="LnkRemove_Click" CausesValidation="false"></ACA:AccelaLinkButton>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                        <HeaderStyle Width="50px" />
                    </ACA:AccelaTemplateField>
                </Columns>
            </ACA:AccelaGridView>
        </div>
        <asp:LinkButton ID="btnPostback" runat="Server" OnClick="BtnPostback_Click" CssClass="ACA_Hide" TabIndex="-1" />
    </ContentTemplate>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">

    function AutoFillAddress(uniqueID) {
        CallPostBackFunction(uniqueID);
        return false;
    }

    function RemoveAsset(uniqueID) {
        var warnMsg = '<%=GetTextByKey("aca_common_delete_alert_message").Replace("'","\\'") %>';

        if (confirmMsg(warnMsg)) {
            CallPostBackFunction(uniqueID);
        }

        return false;
    }

    function ShowAssetInfo(moduleName,isShowAction, args, lnkRelatedRecordID) {
        var url = '<%=FileUtil.AppendApplicationRoot("/Asset/AssetDetail.aspx") %>?module=' + moduleName + '&isShowAction=' + isShowAction + '&args=' + args;
        ACADialog.popup({ url: url, width: 750, height: 500, objectTarget: lnkRelatedRecordID });
    }

    function CallPostBackFunction(uniqueID) {
        
        var p = new ProcessLoading();
        p.showLoading();
        __doPostBack(uniqueID, '');

        return false;
    }

    function ShowSearchFormDialog(sender,moduleName) {
        var url = '<%=FileUtil.AppendApplicationRoot("/Asset/AssetSearch.aspx") %>?module=' + moduleName + '&isPopup=Y';

        ACADialog.popup({ url: url, width: 750, height: 550, objectTarget: sender.id });
        return false;
    }

    function RefreshAssetList(successCount, failCount) {
        __doPostBack('<%=btnRefreshAssetList.UniqueID %>', successCount + '<%=ACAConstant.SPLIT_CHAR4URL1 %>' + failCount);
    }

    function HiddenNotice() {
        var lblNotice = document.getElementById("<%=divActionNotice.ClientID %>");

        if (lblNotice != undefined) {
            lblNotice.style.display = "none";
        }
    }
    
    function GetAsset(info) {
        SetNotAskForSPEAR();
        __doPostBack("<%=btnPostback.UniqueID %>", info);
    }

</script>
