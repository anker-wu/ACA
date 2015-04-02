<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="AddressInfo" Codebehind="Address.Info.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="Owner.Info.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = ErrorMessage.ToString()%>
        <% = PageTitle %>
        <% = AddressDetail %>
        <% = ParcelList %>
        <% = OwnerList %>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>