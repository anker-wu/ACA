<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.RefAPOParcelList"
    CodeBehind="RefAPOParcelList.ascx.cs" %>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="gvParcelResult" runat="server"  visible="true">
    <ACA:AccelaLinkButton ID="btnSelectParcel" CssClass="ACA_Hide" runat="server" OnClick="SelectParcelButton_OnClick" TabIndex="-1"></ACA:AccelaLinkButton>
    <ACA:ACAMap ID="mapParcel" AGISContext="ParcelList" runat="server" OnShowOnMap="MapParcel_ShowACAMap" />
    <ACA:AccelaGridView ID="gv" runat="server" AutoGenerateColumns="false" 
       SummaryKey="gdv_apoparcellist_parcellist_summary" CaptionKey="aca_caption_apoparcellist_parcellist" ShowCaption="true"
        AllowSorting="true" AllowPaging="true" RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize"
        AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize"
        PagerStyle-HorizontalAlign="center" CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize"
        OnClientSelectSingle="ParcelList_Selected()" OnRowDataBound="RefParcelList_RowDataBound"
        OnRowCommand="RefParcelList_RowCommand" GridViewNumber="60049" OnPageIndexChanging="RefParcelList_PageIndexChanging" OnGridViewSort="RefParcelList_GridViewSort">
        <Columns>
            <ACA:AccelaTemplateField AttributeName="lnkParcelNumberHeader">
                <HeaderTemplate>
                    <div>
                        <ACA:GridViewHeaderLabel ID="lnkParcelNumberHeader" runat="server" CommandName="Header"
                            SortExpression="ParcelNumber" LabelKey="APO_ParcelView_ParcelNumber_Header"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemStyle Width="128px" />
                <HeaderStyle Width="128px" />
                <ItemTemplate>
                    <div>
                        <asp:LinkButton ID="lnkNumber" runat="server" CommandName="ShowDetail" Visible="true"
                            CommandArgument="<%#((GridViewRow)Container).RowIndex%>">
                            <strong><%#DataBinder.Eval(Container.DataItem, "ParcelNumber")%></strong>
                        </asp:LinkButton>
                        <ACA:AccelaLabel ID="lblSequenceNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ParcelSequenceNumber")%>'
                            Visible="false"></ACA:AccelaLabel>
                        <ACA:AccelaLabel ID="lblNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ParcelNumber")%>'
                            Visible="false"></ACA:AccelaLabel>
                        <ACA:AccelaLabel ID="lblParcelUID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ParcelUID")%>'
                            Visible="false"></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkParcelLotHeader">
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
            <ACA:AccelaTemplateField AttributeName="lnkParcelBlockHeader">
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
            <ACA:AccelaTemplateField AttributeName="lnkParcelSubdivisionHeader">
                <HeaderTemplate>
                    <div>
                        <ACA:GridViewHeaderLabel ID="lnkParcelSubdivisionHeader" runat="server" SortExpression="Subdivision"
                            LabelKey="APO_ParcelView_Subdivision_Header"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemStyle Width="278px" />
                <HeaderStyle Width="278px" />
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblSubdivision" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subdivision")%>'></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
            </ACA:AccelaTemplateField>

            <ACA:AccelaTemplateField AttributeName="lnkAddressDetailParcelStatusHeader">
                <HeaderTemplate>
                    <div>
                        <ACA:GridViewHeaderLabel ID="lnkAddressDetailParcelStatusHeader" runat="server" SortExpression="ParcelStatus"
                            LabelKey="aca_addressdetail_parcelstatus_header"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemStyle Width="128px" />
                <HeaderStyle Width="128px" />
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblParcelStatus" runat="server" Text='<%#GetDisplayParcelStatus(DataBinder.Eval(Container.DataItem, "ParcelStatus").ToString())%>'></ACA:AccelaLabel>
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
    <asp:HiddenField ID="parcelListXml" runat="server" />
</div>
<script type="text/javascript">
    function ParcelList_Selected() {
        if ('<%=AutoPostBackOnSelect.ToString().ToLower() %>' == 'false') {
            return;
        }

        var selectedValues = $("#<%=gv.GetSelectedItemsFieldClientID() %>").val();

        ShowLoading();
        __doPostBack('<%=btnSelectParcel.UniqueID %>', selectedValues);
    }
</script>