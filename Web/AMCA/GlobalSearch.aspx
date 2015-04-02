<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="GlobalSearch" Codebehind="GlobalSearch.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="GlobalSearch.aspx?State=<% = State %>&Module=<% = ModuleName %>&UpdateSession=true">
        <% = ErrorMessage %>
        <div class="pageTextInputWrapper">
        <% = SearchSection.ToString() %>
        <% = SearchMessage.ToString() %>
        </div>
   </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>