<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="InspectionWizard" Codebehind="InspectionWizard.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="Inspection.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = PageTitle %>
        <% = ErrorMessage.ToString()%>
        <% = CategoryList %>
        <% = InspectionList %>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>