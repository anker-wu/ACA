<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="LoginHome" Codebehind="Login.Home.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = iPhoneHeader %>
    <% = UserLink.ToString() %>
    <form id="form1" method="post" action="Login.Home.aspx?State=<% = State %>&UpdateSession=true">
        <% = PageTitle.ToString() %>
        <% = ErrorMessage.ToString() %>
        <% = SearchSection.ToString() %>
        <% = SearchMessage.ToString() %>
        <% = CollectionsSection.ToString() %>
        <% = ModulesSection.ToString() %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
</asp:Content>