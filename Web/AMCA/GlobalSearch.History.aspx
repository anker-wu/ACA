<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" CodeBehind="GlobalSearch.History.aspx.cs" Inherits="GlobalSearchHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" style="margin-top:0px;" method="post" action="GlobalSearch.History.aspx.cs?State=<% = State %>">
        <% = PageTitle %>
        <% = SearchResultLinks.ToString()%>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>