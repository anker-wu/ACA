<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="ViewDetails" Codebehind="View.Details.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>
    <div id="pageWithNoForm">
        <% = DetailsHeader %>
        <hr />
        <% = ErrorMessage.ToString()%>
        <div id="pageText"><% = Details %></div>
        </div>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackLink %>
    <br />
</asp:Content>
