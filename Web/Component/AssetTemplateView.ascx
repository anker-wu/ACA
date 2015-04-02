<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetTemplateView.ascx.cs"
    Inherits="Accela.ACA.Web.Component.AssetTemplateView" %>
<asp:Repeater ID="templateFieldsList" runat="server" OnItemDataBound="TemplateFieldsList_ItemDataBound">
    <ItemTemplate>
        <div>
            <div class="ACA_FRight ACA_HalfWidthTable">
                <ACA:AccelaNameValueLabel ID="lblField1" CssClass="fieldlabel" LayoutType="Horizontal" runat="server"/>
            </div>
            <div class="ACA_FLeft ACA_HalfWidthTable">
                <ACA:AccelaNameValueLabel ID="lblField2" CssClass="fieldlabel" LayoutType="Horizontal" runat="server"/>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
<div id="divGenericTemplateTable" runat="server" visible="False">
    <asp:PlaceHolder ID="phAssetTable" runat="server" EnableTheming="true"></asp:PlaceHolder>
</div>
