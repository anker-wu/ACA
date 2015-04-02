<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.RefAPOAddressList"
    CodeBehind="RefAPOAddressList.ascx.cs" %>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="gvAddressResult" runat="server"
    visible="true">
    <asp:UpdatePanel ID="mapPanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <ACA:ACAMap ID="mapAddress" AGISContext="AddressList" runat="server" OnShowOnMap="MapAddress_ShowACAMap" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updatePanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>            
            <ACA:AccelaGridView ID="gvAPOList" runat="server" AutoGenerateColumns="false"
                SummaryKey="gdv_apoaddress_addresslist_summary" CaptionKey="aca_caption_apoaddress_addresslist" ShowCaption="true" AllowSorting="true"
                AllowPaging="true" RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize"
                PagerStyle-HorizontalAlign="center" AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize"
                HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize"
                OnRowDataBound="RefAddressList_RowDataBound" OnRowCommand="RefAddressList_RowCommand" OnPageIndexChanged="RefAddressList_PageIndexChanged"
                OnPageIndexChanging="RefAddressList_PageIndexChanging" OnGridViewSort="RefAddressList_GridViewSort" OnGridViewDownload="RefAddressList_GridViewDownload">
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
                            <div id="divParcelNum" runat="server">
                                <ACA:AccelaLabel ID="lblParcelNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ParcelNumber")%>'
                                    Visible="true"></ACA:AccelaLabel>
                            </div>
                            <div id="divLnkParcelNum" runat="server">
                                <asp:LinkButton ID="lbParcelNumber" runat="server" CommandName="ShowParcel" Visible="true"
                                    CommandArgument="<%#((GridViewRow)Container).RowIndex%>">
                            
                            <strong>
                                <%#DataBinder.Eval(Container.DataItem, "ParcelNumber")%>
                         </strong>                                        
                                </asp:LinkButton>
                                <ACA:AccelaLabel ID="lblParcelNum" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ParcelSequenceNumber")%>'
                                    Visible="false"></ACA:AccelaLabel>
                                <ACA:AccelaLabel ID="lblParcelUID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ParcelUID")%>'
                                    Visible="false"></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ColumnId="Owner" AttributeName="lnkOwnerHeader" ExportDataField="OwnerFullName">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkOwnerHeader" runat="server" SortExpression="OwnerFullName"
                                    LabelKey="aca_searchbyparcel_owner_header"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemStyle Width="128px" />
                        <HeaderStyle Width="128px" />
                        <ItemTemplate>
                            <div id="divOwner" runat="server">
                                <ACA:AccelaLabel ID="lblOwner" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OwnerFullName")%>'></ACA:AccelaLabel>
                            </div>
                            <div id="divlnkOwner" runat="server">
                                <asp:LinkButton ID="lbOwner" runat="server" CommandName="ShowOwner" Visible="true"
                                    CommandArgument="<%#((GridViewRow)Container).RowIndex%>">
                            <strong>
                                <%#Accela.ACA.Common.Common.ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, "OwnerFullName").ToString())%>
                            </strong>                                        
                                </asp:LinkButton>
                                <ACA:AccelaLabel ID="lblOwnerSeq" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OwnerSequenceNumber")%>'
                                    Visible="false"></ACA:AccelaLabel>
                                <ACA:AccelaLabel ID="lblOwnerNum" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OwnerNumber")%>'
                                    Visible="false"></ACA:AccelaLabel>
                                <ACA:AccelaLabel ID="lblOwnerUID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OwnerUID")%>'
                                    Visible="false"></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkAddressHeader" ExportDataField="FullAddress">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" CommandName="Header"
                                    SortExpression="FullAddress" LabelKey="aca_searchbyparcel_address_header"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemStyle Width="268px" />
                        <HeaderStyle Width="268px" />
                        <ItemTemplate>
                            <div id="divAddress" runat="server">
                                <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FullAddress")%>'></ACA:AccelaLabel>
                            </div>
                            <div id="divLnkAddress" runat="server">
                                <asp:LinkButton ID="lbAddress" runat="server" CommandName="ShowAddress" Visible="true"
                                    CommandArgument="<%#((GridViewRow)Container).RowIndex%>">
                            <strong>
                                <%#DataBinder.Eval(Container.DataItem, "FullAddress")%>
                         </strong>                                        
                                </asp:LinkButton>
                                <ACA:AccelaLabel ID="lblAddressID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AddressID")%>'
                                    Visible="false"></ACA:AccelaLabel>
                                <ACA:AccelaLabel ID="lblAddressUID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AddressUID")%>'
                                    Visible="false"></ACA:AccelaLabel>
                                <ACA:AccelaLabel ID="lblAddressNum" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AddressSequenceNumber")%>'
                                    Visible="false"></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkParcelStatusHeader" ExportDataField="ParcelStatus">
                        <HeaderTemplate>
                            <div>
                                <ACA:GridViewHeaderLabel ID="lnkParcelStatusHeader" runat="server" SortExpression="ParcelStatus"
                                    LabelKey="aca_refaddresseslist_label_parcelstatus"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemStyle Width="80px" />
                        <HeaderStyle Width="80px" />
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblParcelStatus" runat="server" Visible="true"></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
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
