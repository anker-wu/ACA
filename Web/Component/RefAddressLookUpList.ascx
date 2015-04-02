<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefAddressLookUpList.ascx.cs" Inherits="Accela.ACA.Web.Component.RefAddressLookUpList" %>
<%@ Register TagPrefix="ACA" TagName="ACAMap" Src="~/Component/ACAMap.ascx" %>
<div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="gvAddressSearchResult" runat="server"
    visible="true">
    <asp:UpdatePanel ID="mapPanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <ACA:ACAMap ID="mapAddress" AGISContext="AddressList" runat="server" OnShowOnMap="MapAddress_ShowACAMap" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="refAddressUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ACA:AccelaGridView ID="dgvRefAddressLookUpList" runat="server" AutoGenerateColumns="false"
                SummaryKey="gdv_apoaddress_addresslist_summary" CaptionKey="aca_caption_apoaddress_addresslist" ShowCaption="true" AllowSorting="true" 
                AllowPaging="true" RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize"
                PagerStyle-HorizontalAlign="center" AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize"
                HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize" 
                OnRowCommand="RefAddress_RowCommand" OnPageIndexChanged="RefAddress_PageIndexChanged"
                OnPageIndexChanging="RefAddress_PageIndexChanging" OnGridViewSort="RefAddress_GridViewSort" OnGridViewDownload="RefAddress_GridViewDownload">
                <Columns>
                    <ACA:AccelaTemplateField AttributeName="lnkAddressHeader" ExportDataField="FullAddress">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" SortExpression="FullAddress"
                                    LabelKey="per_permitList_label_addressListAddress">
                                </ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <asp:LinkButton ID="lblFullAddress" runat="server" Visible="true"
                                    CommandName="ShowAddress" CommandArgument="<%#((GridViewRow)Container).RowIndex%>">
                                    <strong>
                                        <%#DataBinder.Eval(Container.DataItem, "FullAddress")%>
                                    </strong>                                        
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="268px"/>
                        <HeaderStyle Width="268px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkRetrieveAssociatedDataHeader">
                        <HeaderTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lnkRetrieveAssociatedDataHeader" runat="server" LabelKey="aca_apo_label_addresslist_action" IsGridViewHeadLabel="true">
                                </ACA:AccelaLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <asp:LinkButton ID="lblRetrieveAssociatedData" runat="server" Visible="true"
                                    CommandName="RetrieveData" CommandArgument="<%#((GridViewRow)Container).RowIndex%>">
                                    <strong>
                                     <%#LabelUtil.GetTextByKey("aca_apo_label_addresslist_retrieveassociateddata",string.Empty) %>
                                     </strong>                                                              
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="482px" />
                        <HeaderStyle Width="482px" />
                    </ACA:AccelaTemplateField>
                </Columns>
                <EmptyDataRowStyle CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
                <HeaderStyle CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" />
                <RowStyle CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize" />
                <PagerStyle CssClass="ACA_Table_Pages ACA_Table_Pages_FontSize" VerticalAlign="Bottom" />
                <AlternatingRowStyle CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" />
            </ACA:AccelaGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
