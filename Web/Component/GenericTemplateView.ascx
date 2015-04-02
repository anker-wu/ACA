<%@ Import Namespace="Accela.ACA.Common.Util" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Import Namespace="Accela.ACA.WSProxy" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenericTemplateView.ascx.cs" Inherits="Accela.ACA.Web.Component.GenericTemplateView" %>

<%@ Register Src="~/Component/AppSpecInfoTableView.ascx" TagName="AppSpecInfoTableView" TagPrefix="ACA" %>

<div class="ACA_Row generictemplatedetail">
    <asp:Repeater ID="templateFieldsList" runat="server" OnItemDataBound="TemplateFieldsList_ItemDataBound">
        <ItemTemplate>
            <div class="detailfield">
                <ACA:AccelaNameValueLabel ID="lblFieldName" CssClass="fieldlabel" LayoutType="Horizontal" runat="server"></ACA:AccelaNameValueLabel>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div id="divGenericTemplateTable" class="generictemplatetabledetail" runat="server" visible="False">
        <ACA:AppSpecInfoTableView ID="genericTemplateTable" IsTemplateTable="True" runat="server" />
    </div>
    <ACA:AccelaHeightSeparate ID="sepForGenricTemplete" runat="server" Height="5" />
</div>