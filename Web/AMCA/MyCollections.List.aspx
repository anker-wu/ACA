<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="MyCollectionsList" CodeBehind="MyCollections.List.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="MyCollections.List.aspx?State=<% = State %>&Mode=<% = SearchMode %>&InspectionStatus=<% = Filter %>&CapsInList=<% = capsInList %>&Module=<% = ModuleName %>&CollectionModule=<% = CollectionModule %>&CollectionId=<% = CollectionId %>&PageBreadcrumbIndex=<% = PageBreadcrumbIndex %>&SortColumn=<% = ListSortColumn %>&SortDesc=<% = ListSortDesc %>&PageResultPageNo=<% = PageResultPageNo %>">
        <% = ListMode %>
        <% = ErrorMessage.ToString() %>
        <div style="margin: 0 0 10px 0;">
        <% = ResultHeader.ToString() %>
        <% = SortOptions.ToString() %>
        <% = ResultOutput.ToString() %>
        </div>
        <% = CollectionLinks.ToString() %>
        <% = PagingFooter %>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>