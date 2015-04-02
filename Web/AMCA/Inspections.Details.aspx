<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="InspectionsDetails" Codebehind="Inspections.Details.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="Inspections.Details.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = PageTitle %>
        <div id="pageText"><% = RecordInfo %></div>
        <hr />
        <% = ErrorMessage.ToString()%>
        <% = ResultHeader.ToString() %>
        <% = InspectionList %>
        <% = PagingFooter %>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>