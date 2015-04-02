<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="PermitsMoreDetail" Codebehind="Permits.MoreDetail.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>
    <div id="pageWithNoForm">
    <% = DisplayNumber %>
    <% = ErrorMessage.ToString()%>
        <div id="pageSectionTitle">Type: <br /><span id="pageLineText"><% = DisplayType %></span></div>
        <div id="pageSectionTitle">Status: <span id="pageLineText"><% = ThisPermit.Status %></span></div>
        <div id="pageSectionTitle">Date: <span id="pageLineText"><% = ThisPermitDate %></span></div>
        <% = OwnerDetails %>
        <% = LicensedProfessional %>
        <div id="pageSectionTitle">Address: <br /><span id="pageLineText"><% = ThisPermit.Address %></span></div>
        <div id="pageSectionTitle">Applicant: <span id="pageLineText"><% = ThisPermit.Application %></span></div>
        <div id="pageSectionTitle">Description: <br /><span id="pageLineText"><% = Description %></span></div>
    </div>
    <% = OutputLinks.ToString()%>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
</asp:Content>
