<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.AddressSearchList" Codebehind="AddressSearchList.ascx.cs" %>
<div id="divAddressResult" runat="server" class="ACA_TabRow_NoScoll ACA_WrodWrap">
    <ACA:AccelaLinkButton ID="btnSelectAddress" CssClass="ACA_Hide" runat="server" OnClick="SelectAddressButton_OnClick" TabIndex="-1"></ACA:AccelaLinkButton>
    <ACA:AccelaGridView ID="gv" runat="server" IsInSPEARForm="true"
        SummaryKey="gdv_addressedit_addresslist_summary" CaptionKey="aca_caption_addressedit_addresslist"
        ShowCaption="true" AllowPaging="true" AllowSorting="true" PagerStyle-HorizontalAlign="center"
        OnClientSelectSingle="AddressList_Selected()" OnRowCommand="AddressList_RowCommand"
        OnPageIndexChanging="Address_GridViewIndexChanging" OnGridViewSort="Address_GridViewSort">
        <Columns>
            <ACA:AccelaTemplateField AttributeName="lnkAddressHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" CommandName="Header"
                            SortExpression="FullAddress" MaxLength="1024" LabelKey="per_WorkLocation_AddressList_Address"
                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FullAddress") %>'></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="360px" />
                <HeaderStyle Width="360px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkCityHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkCityHeader" runat="server" CommandName="Header" SortExpression="City"
                            LabelKey="per_WorkLocation_AdressList_City" CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "City") %>'></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="80px" />
                <HeaderStyle Width="80px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStateHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStateHeader" runat="server" CommandName="Header"
                            SortExpression="State" LabelKey="per_WorkLocation_AdressList_State" CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="ACA_XShoter">
                        <ACA:AccelaLabel ID="lblState" runat="server" Text='<%# I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "State") as string, DataBinder.Eval(Container.DataItem, "CountryCode") as string)%>'></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="15px" />
                <HeaderStyle Width="15px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkZipHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkZipHeader" runat="server" CommandName="Header" SortExpression="Zip"
                            LabelKey="per_WorkLocation_AdressList_Zip" CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%#ModelUIFormat.FormatZipShow(DataBinder.Eval(Container.DataItem, "Zip") as string, DataBinder.Eval(Container.DataItem, "CountryCode") as string)  %>'></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="80px" />
                <HeaderStyle Width="80px" />
            </ACA:AccelaTemplateField>
        </Columns>
    </ACA:AccelaGridView>
</div>
<script type="text/javascript">
    function AddressList_Selected() {
        if ('<%=AutoPostBackOnSelect.ToString().ToLower() %>' == 'false') {
            return;
        }

        ShowLoading();
        __doPostBack('<%=btnSelectAddress.UniqueID %>', '');
    }
</script>