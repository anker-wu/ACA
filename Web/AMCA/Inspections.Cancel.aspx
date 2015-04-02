<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="InspectionsCancel" Codebehind="Inspections.Cancel.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="InspectionsCancel" method="post" action="<% = NextPage %>">
        <% = PageTitle %>
        <% = ErrorMessage.ToString()%>
        <div id="pageText">
            <div id="pageSectionTitle">Record No: <span id="pageLineText"><% = DisplayNumber %></span></div>
            <div id="pageSectionTitle">Inspection Type: <span id="pageLineText"><% = ThisInspection.Type %></span></div>
        </div>
        <div id="pageSectionTitle">Status: <span id="pageLineText"><% = ThisInspection.Status %></span></div>
        <div id="pageSectionTitle">Scheduled Date/Time: <span id="pageLineText"><% = InspectionDateTime %></span></div>
        <div id="pageSectionTitle">Inspector: <span id="pageLineText"><% = ThisInspection.Inspector%></span></div>
        <br />
        <div id="pageSubmitButton"><% = SubmitButton %></div>
        <% = HiddenFields%>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>
