<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="AdvancedSearchResults" Codebehind="AdvancedSearch.Results.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>   
    <form id="form1" method="post" action="AdvancedSearch.Results.aspx?State=<% = State %>&Mode=<% = SearchMode %>&InspectionStatus=<% = Filter %>&CapsInList=<% = capsInList %>&Module=<% = ModuleName %>&PageBreadcrumbIndex=<% = PageBreadcrumbIndex %>&SortColumn=<% = ListSortColumn %>&SortDesc=<% = ListSortDesc %>&PageResultPageNo=<% = PageResultPageNo %>">
        <% = ErrorMessage.ToString()%>
        <% = PageTitle %>
        <% = ResultHeader.ToString()%>
        <% = SortOptions.ToString()%>
        <% = ResultOutput.ToString()%>
        <% = CollectionLinks.ToString()%>
        <% = PagingFooter%>
        <% = HiddenFields%>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>   
    <% = BackForwardLinks%>
 </asp:Content>
