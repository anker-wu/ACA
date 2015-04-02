<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="PermitsView" Codebehind="Permits.View.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <div id="pageWithNoForm">
        <% = ErrorMessage.ToString()%>
        <% = PageTitle %>
        <% = DisplayType %>
        <hr />
        <div id="pageSectionTitle">Status:              <span id="pageText"><% = ThisPermit.Status %></span></div>
        <div id="pageSectionTitle">Work Location: <br /><span id="pageText"><% = ThisPermit.Address %> </span></div>
        <% = LicensedPro %>
        <% = OutputLinks.ToString()%>
        <% = Collections %>
        <% = iPhoneBreadcrumbs %>
    </div>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %> 
    
       
    <% = ProcessingStatusView %>
    
</asp:Content>
