<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="OwnerInfo" Codebehind="Owner.Info.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = iPhoneHeader %>
    <% = Breadcrumbs %> 
    <form id="form1" method="post" action="MyCollections.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = ErrorMessage.ToString()%>
        <% = PageTitle %>
        <% = OwnerDetail %>
        <% = AddressList %>
        <% = ParcelList %>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>