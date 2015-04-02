<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.AddressView" Codebehind="AddressView.ascx.cs" %>
<%@ Register Src="TemplateView.ascx" TagName="TemplateView" TagPrefix="uc1" %>
<div class="ACA_Row ACA_WrodWrap">
    <ACA:AccelaLabel ID="lblAddress" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
</div>
<uc1:TemplateView ID="templateView" runat="server" />
