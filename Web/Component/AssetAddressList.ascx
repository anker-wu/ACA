<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetAddressList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.AssetAddressList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<div id="divAssetAddressList" runat="server" visible="false">
    <asp:UpdatePanel ID="pnlAssetAddress" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ACA:AccelaGridView ID="gvAssetAddress" runat="server" IsInSPEARForm="true" AllowPaging="true" 
                    SummaryKey="aca_summary_asset_address" CaptionKey="aca_caption_asset_address"
                    AllowSorting="true" PagerStyle-HorizontalAlign="center" ShowCaption="true" Visible="false"
                    PageSize="10" GridViewNumber="60172" OnPageIndexChanging="GvAssetAddress_PageIndexChanging"
                    OnRowDataBound="GvAssetAddress_RowDataBound" OnGridViewSort="GvAssetAddress_GridViewSort">
                <Columns>
                     <ACA:AccelaTemplateField AttributeName="lnkStreetStart">
                            <HeaderTemplate>
                                <div>
                                    <ACA:GridViewHeaderLabel ID="lnkStreetStart" runat="server" SortExpression="houseNumberStart" CommandName="Header"
                                        LabelKey="aca_assetaddresslist_label_streetstart" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblStreetStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "houseNumberStart") %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="128px" />
                            <HeaderStyle Width="128px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkStreetEnd">
                            <HeaderTemplate>
                                <div>
                                    <ACA:GridViewHeaderLabel ID="lnkStreetEnd" runat="server" SortExpression="houseNumberEnd" CommandName="Header"
                                        LabelKey="aca_assetaddresslist_label_streetend" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblStreetEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "houseNumberEnd") %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="128px" />
                            <HeaderStyle Width="128px" />
                        </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkStreetName">
                        <HeaderTemplate>
                            <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkStreetName" runat="server" CommandName="Header" SortExpression="streetName"
                                    LabelKey="aca_assetaddresslist_label_streetname" CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblStreetName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "streetName") %>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                        <HeaderStyle Width="80px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkCity">
                        <HeaderTemplate>
                            <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkCity" runat="server" CommandName="Header" SortExpression="city"
                                    LabelKey="aca_assetaddresslist_label_city" CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "city") %>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                        <HeaderStyle Width="80px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkCounty">
                        <HeaderTemplate>
                            <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkCounty" runat="server" CommandName="Header" SortExpression="county"
                                    LabelKey="aca_assetaddresslist_label_county" CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_XShoter">
                                <ACA:AccelaLabel ID="lblCounty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "county") %>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="15px" />
                        <HeaderStyle Width="15px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkCountryRegion">
                        <HeaderTemplate>
                            <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkCountryRegion" runat="server" CommandName="Header"
                                    SortExpression="countryCode" LabelKey="aca_assetaddresslist_label_countryregion"
                                    CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblCountryRegion" runat="server"></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                        <HeaderStyle Width="80px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkZipCode">
                        <HeaderTemplate>
                            <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkZipCode" runat="server" CommandName="Header" SortExpression="zip"
                                    LabelKey="aca_assetaddresslist_label_zipcode" CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblZipCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "zip") %>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="160px" />
                        <HeaderStyle Width="160px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkPrimary">
                        <HeaderTemplate>
                            <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkPrimary" runat="server" CommandName="Header" SortExpression="primaryFlag"
                                    LabelKey="aca_assetaddresslist_label_primary" CausesValidation="false"></ACA:GridViewHeaderLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblPrimary" runat="server" Text='<%# ValidationUtil.IsYes(DataBinder.Eval(Container.DataItem, "primaryFlag").ToString()) ? ACAConstant.COMMON_Yes : ACAConstant.COMMON_No%>'></ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="160px" />
                        <HeaderStyle Width="160px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ColumnId="Action" AttributeName="lnkAction">
                        <HeaderTemplate>
                            <div>
                                <ACA:AccelaLabel IsGridViewHeadLabel="true" ID="lnkAction" runat="server" LabelKey="aca_assetaddresslist_label_action"
                                    IsNeedEncode="false"></ACA:AccelaLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_FLeft">
                                <ACA:AccelaLinkButton ID="lnkGetAddress" runat="server" CommandName='<%# COMMAND_GET_ADDRESS %>'
                                    Visible="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "refAddressId").ToString() %>'
                                    OnClick="LnkGetAddress_Click" CausesValidation="false"></ACA:AccelaLinkButton>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                        <HeaderStyle Width="50px" />
                    </ACA:AccelaTemplateField>
                </Columns>
            </ACA:AccelaGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>