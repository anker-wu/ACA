<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="AdvancedSearchFilter" Codebehind="AdvancedSearch.Filter.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs.ToString() %>
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="AdvancedSearch.Filter.aspx?State=<% = State %>&Mode=<% = SearchMode %>&Module=<% = ModuleName %>&ResultPage=<% = ResultPage %>&CollectionId=<% = CollectionId %>&CollectionModule=<% = CollectionModule %>&UpdateSession=true&PageBreadcrumbIndex=<% = PageBreadcrumbIndex %>">
        <% = ErrorMessage.ToString() %>
        <div class="pageTextInputWrapper">
        <% = SearchSection.ToString() %>
        </div>
        <% = HiddenFields%>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks.ToString() %>
</asp:Content>