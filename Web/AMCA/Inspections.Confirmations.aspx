<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="InspectionsConfirmations" Codebehind="Inspections.Confirmations.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %>
    <% = iPhoneHeader %>
    <div id="pageWithNoForm">
        <% = PageTitle %>
        <div id="pageText"><% = RecordInfo %></div>
        <hr />
        <% = ErrorMessage.ToString()%>
        <% = ActionMessage.ToString()%>
        <div id="pageText">
        <% = TheDate  %>
        <% = TheTime %>
        <% = TheComment %>
        </div>
        <% = Restrictions %>
    </div>
	<% = OutputLinks.ToString()%>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
</asp:Content>
