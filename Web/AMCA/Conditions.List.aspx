<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="ConditionsList" CodeBehind="Conditions.List.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="Conditions.List.aspx?State=<% = State %>&Module=<% = ModuleName %>&PageBreadcrumbIndex=<% = PageBreadcrumbIndex %>&SortColumn=<% = ListSortColumn %>&SortDesc=<% = ListSortDesc %>&PageResultPageNo=<% = PageResultPageNo %>">
        <% = DisplayMode %>
        <% = ErrorMessage.ToString() %>
        <div style="margin: 0 0 10px 0;">
        <% = ResultHeader.ToString() %>
        <% = SortOptions.ToString() %>
        <% = ResultOutput.ToString() %>
        </div>
        <% = PagingFooter %>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>