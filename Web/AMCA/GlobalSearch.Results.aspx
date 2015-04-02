<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="GlobalSearchResults" CodeBehind="GlobalSearch.Results.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" style="margin-top:0px;" method="post" action="GlobalSearch.Results.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = PageTitle %>
        <% = ErrorMessage.ToString() %>
        <div id="pageTextIndented"><% = SearchResultSummary.ToString() %></div>
        <% = SearchResultLinks.ToString()%>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>