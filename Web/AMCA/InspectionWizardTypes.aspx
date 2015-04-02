<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="InspectionWizardTypes" Codebehind="InspectionWizardTypes.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="Inspection.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = PageTitle %>
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