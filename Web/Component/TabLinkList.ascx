<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabLinkList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.TabLinkList" %>
<asp:DataList ID="TabsDataList" OnItemDataBound="TabsDataList_ItemDataBound" runat="server"
    RepeatDirection="Horizontal" RepeatColumns="2" CellPadding="0" CellSpacing="6"
    CssClass="ACA_Welcome_Block" role='presentation' >
    <ItemTemplate>
        <div class="Header_h2">
            <ACA:AccelaLabel ID="lblModuleName" runat="server" LabelKey='<%# DataBinder.Eval(Container.DataItem, "Label")%>'
                ModuleName='<%# DataBinder.Eval(Container.DataItem, "Module") %>'></ACA:AccelaLabel>
        </div>
        <asp:DataList ID="LinksDataList" runat="server" RepeatLayout="Flow" OnItemDataBound="LinksDataList_ItemDataBound"
            Width="100%" OnItemCommand="LinkItem_OnItemCommand">
            <ItemTemplate>
                <ACA:AccelaLinkButton ID="LinkItemUrl" LabelKey='<%# DataBinder.Eval(Container.DataItem, "Label") %>'
                    ModuleName='<%# DataBinder.Eval(Container.DataItem, "Module") %>' CausesValidation="false"
                    CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Url") %>'
                    runat="server"></ACA:AccelaLinkButton>
            </ItemTemplate>
        </asp:DataList>
    </ItemTemplate>
</asp:DataList>
