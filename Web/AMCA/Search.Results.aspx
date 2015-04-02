<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="SearchResults" Codebehind="Search.Results.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="Search.Results.aspx?State=<% = State %>&SearchBy=<% = SearchBy %>&SearchType=<% = SearchType %>&SearchValue=<% = SearchValue %>&InspectionStatus=<% = Filter %>">
        <% = ErrorMessage.ToString()%>
        <% = PageHeading %>
        <% = InspectionWizardLink %>
        <% = ResultOutput.ToString() %>
        <% = PagingFooter %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>