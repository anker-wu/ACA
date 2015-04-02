<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="WorkflowView" Codebehind="Workflow.View.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>
    <div id="pageWithNoForm">
    <% = DisplayNumber %>
    <% = ErrorMessage.ToString()%>
        <div id="pageText">
        <div id="pageSectionTitle"><% = PageTitle %></div>
        </div>
        <div id=pageText"><% = Workflow %></div>
    </div>
    <% = OutputLinks.ToString()%>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
</asp:Content>