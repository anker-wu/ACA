<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="LicenseInfo" Codebehind="License.Info.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" style="margin-top:0px;" method="post" action="Owner.Info.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = ErrorMessage.ToString()%>
        <% = PageTitle %>
        <% = LicenseDetails %>
        <% = BusinessDetails %>
        <% = ContactDetails %>
     </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>