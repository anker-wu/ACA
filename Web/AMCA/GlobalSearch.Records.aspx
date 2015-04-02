<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" CodeBehind="GlobalSearch.Records.aspx.cs" Inherits="GlobalSearchRecords" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <form id="form1" style="margin-top:0px;" method="post" action="GlobalSearch.Results.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <div style="font-size:11pt; font-weight:bold; margin-top:10px;"><% = PageTitle %></div>
        <% = ErrorMessage.ToString() %>
        <% = SearchResultSummary.ToString() %>
        <% = SearchResultLinks.ToString()%>
        <% = HiddenFields %>
    </form>
    <% = BackForwardLinks %>
</asp:Content>