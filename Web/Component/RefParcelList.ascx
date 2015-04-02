<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefParcelList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.RefParcelList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<asp:UpdatePanel ID="RefParcelListListPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvRefParcelList" runat="server" AllowPaging="true" AllowSorting="True"
            SummaryKey="gdv_apoparcellist_parcellist_summary" CaptionKey="aca_caption_apoparcellist_parcellist" ShowCaption="true" AutoGenerateColumns="False"
            OnRowCommand="RefParcelList_RowCommand" PagerStyle-HorizontalAlign="center"
            PagerStyle-VerticalAlign="bottom" ShowExportLink="true" GridViewNumber="60111">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkParcelNumberHeader">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkParcelNumberHeader" runat="server" LabelKey="APO_ParcelView_ParcelNumber_Header"
                                SortExpression="ParcelNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblParcelNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.RefParcelList.ParcelNumber.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkParcelLotHeader">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkParcelLotHeader" runat="server" LabelKey="APO_ParcelView_Lot_Header"
                                SortExpression="Lot" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLot" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.RefParcelList.Lot.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkParcelBlockHeader">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkParcelBlockHeader" runat="server" LabelKey="APO_ParcelView_Block_Header"
                                SortExpression="Block" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBlock" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.RefParcelList.Block.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                    <HeaderStyle Width="200px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkParcelSubdivisionHeader">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkParcelSubdivisionHeader" runat="server" LabelKey="APO_ParcelView_Subdivision_Header"
                                SortExpression="Subdivision" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblSubdivision" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.RefParcelList.Subdivision.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField ColumnId="Owner" AttributeName="lnkOwnerHeader">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkOwnerHeader" runat="server" SortExpression="OwnerFullName" LabelKey="APO_refAddressesList_Owner_Header" CommandName="Header" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblOwner" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.RefParcelList.OwnerFullName.ToString()) %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="128px"/>
                    <headerstyle Width="128px"/>
                </ACA:AccelaTemplateField>    
                <ACA:AccelaTemplateField AttributeName="lnkAddressHeader">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" CommandName="Header" SortExpression="FullAddress" LabelKey="APO_refAddressesList_Address_Header" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblOwner" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.RefParcelList.FullAddress.ToString()) %>' />                                     
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="268px"/>
                    <headerstyle Width="268px"/>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
