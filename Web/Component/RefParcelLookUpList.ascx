<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefParcelLookUpList.ascx.cs" Inherits="Accela.ACA.Web.Component.RefParcelLookUpList" %>
<%@ Register TagPrefix="ACA" TagName="ACAMap" Src="~/Component/ACAMap.ascx" %>
<div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="gvParcelSearchResult" runat="server"
    visible="true">
    <asp:UpdatePanel ID="mapPanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <ACA:ACAMap ID="mapParcel" AGISContext="ParcelList" runat="server" OnShowOnMap="MapParcel_ShowACAMap" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="refParcelUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ACA:AccelaGridView ID="dgvRefParcelLookUpList" runat="server" AutoGenerateColumns="false"
                SummaryKey="gdv_apoparcellist_parcellist_summary" CaptionKey="aca_caption_apoparcellist_parcellist" ShowCaption="true"
                AllowSorting="true" AllowPaging="true" RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize"
                AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize"
                PagerStyle-HorizontalAlign="center" CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize"
                OnRowDataBound="RefParcelList_RowDataBound"  OnRowCommand="RefParcelList_RowCommand" OnPageIndexChanged="RefParcelList_PageIndexChanged"
                OnPageIndexChanging="RefParcelList_PageIndexChanging" OnGridViewSort="RefParcelList_GridViewSort" OnGridViewDownload="RefParcelList_GridViewDownload">
                <Columns>
                    <ACA:AccelaTemplateField AttributeName="lnkParcelNumberHeader" ExportDataField="ParcelNumber">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkParcelNumberHeader" runat="server" SortExpression="ParcelNumber"
                                    LabelKey="aca_searchbyparcel_parcelnumber_header"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemStyle Width="128px" />
                        <HeaderStyle Width="128px" />
                        <ItemTemplate>
                            <div id="divLnkParcelNum" runat="server">
                                <asp:LinkButton ID="lbParcelNumber" runat="server" CommandName="ShowParcel" Visible="true"
                                    CommandArgument="<%#((GridViewRow)Container).RowIndex%>">                   
                                    <strong>
                                        <%#DataBinder.Eval(Container.DataItem, "ParcelNumber")%>
                                    </strong>                                        
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkParcelLotHeader" ExportDataField="Lot">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkParcelLotHeader" runat="server" SortExpression="Lot"
                                    LabelKey="APO_ParcelView_Lot_Header"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemStyle Width="54px" />
                        <HeaderStyle Width="54px" />
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblLot" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Lot")%>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkParcelBlockHeader" ExportDataField="Block">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkParcelBlockHeader" runat="server" SortExpression="Block"
                                    LabelKey="APO_ParcelView_Block_Header"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemStyle Width="54px" />
                        <HeaderStyle Width="54px" />
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblBlock" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Block")%>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkParcelSubdivisionHeader" ExportDataField="Subdivision">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkParcelSubdivisionHeader" runat="server" SortExpression="Subdivision"
                                    LabelKey="APO_ParcelView_Subdivision_Header"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemStyle Width="128px" />
                        <HeaderStyle Width="128px" />
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblSubdivision" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subdivision")%>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>

                    <ACA:AccelaTemplateField AttributeName="lnkAddressDetailParcelStatusHeader" ExportDataField="ParcelStatus">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkAddressDetailParcelStatusHeader" runat="server" SortExpression="ParcelStatus"
                                    LabelKey="aca_addressdetail_parcelstatus_header"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemStyle Width="54px" />
                        <HeaderStyle Width="54px" />
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblStatus" runat="server" Text='<%#GetDisplayParcelStatus(DataBinder.Eval(Container.DataItem, "ParcelStatus").ToString())%>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>   
                    <ACA:AccelaTemplateField AttributeName="lnkRetrieveAssociatedDataHeader" ColumnId="Action">
                        <HeaderTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lnkRetrieveAssociatedDataHeader" runat="server" LabelKey="aca_apo_label_parcellist_action" IsGridViewHeadLabel="true">
                                </ACA:AccelaLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <asp:LinkButton ID="lblRetrieveAssociatedData" runat="server" Visible="true"
                                    CommandName="RetrieveData" CommandArgument="<%#((GridViewRow)Container).RowIndex%>">
                                    <strong>
                                     <%#LabelUtil.GetTextByKey(RetieveLinkLabelKey, string.Empty)%>
                                     </strong>                                                              
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="312px" />
                        <HeaderStyle Width="312px" />
                    </ACA:AccelaTemplateField>
                </Columns>
            </ACA:AccelaGridView>   
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

