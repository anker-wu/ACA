<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="InspectionsSchedule" Codebehind="Inspections.Schedule.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="Inspections.Confirmations.aspx?State=<% = State + "&inAdvance="+InAdvance+"&Id=" + ThisInspection.Id +"&Action="+Request["Action"] + "&Module=" + ModuleName %>">
        <% = PageTitle %>
        <div id="pageText"><% = RecordInfo %></div>
        <hr />
        <% = ErrorMessage.ToString()%>
        <div id="pageSectionTitle">
        <label>Status: </label>
        <span id="pageLineText"><% = ThisInspection.Status %></span>
        </div>
        <div id="pageSectionTitle">
        <label>Date & Time:</label>
        <span id="pageLineText"><% = ThisInspectionDate  %></span>
        </div>
        <% = ThisContact %>
        <div id="pageSectionTitle"><label>Comments: </label></div>
        <div id="pageText"><i><% = Comments %></i></div>
        <div id="pageSubmitButton"><% = SubmitButton %></div>
        <% = HiddenFields%>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>
