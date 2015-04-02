<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="MyCollections" Codebehind="MyCollections.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="MyCollections.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = PageTitle %>
        <% = ErrorMessage.ToString()%>
        <% = CollectionsList %>

        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>