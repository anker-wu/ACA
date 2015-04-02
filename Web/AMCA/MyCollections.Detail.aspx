<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="MyCollectionsDetail" Codebehind="MyCollections.Detail.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <% = Breadcrumbs %> 
     <% = iPhoneHeader %>
    <form id="form1" style="margin-top:0px;" method="post" action="MyCollections.Detail.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>&CollectionName=<% = CollectionName %>">
        <% = PageTitle %>
        <% = ErrorMessage.ToString()%>
        <% = PageMessage.ToString() %>
        <% = TotalRecords.ToString() %>
        <% = Inspections.ToString() %>
        <% = FeeSummary.ToString() %>
        <% = HiddenFields %>
    </form>

    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>
