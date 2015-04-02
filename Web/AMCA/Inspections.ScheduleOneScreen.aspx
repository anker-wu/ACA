<%@ Page Language="C#" MasterPageFile="MobileACA.FooterOnly.master" AutoEventWireup="true" Inherits="InspectionsScheduleOneScreen" Codebehind="Inspections.ScheduleOneScreen.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="InspectionsCancel" method="post" action="<% = NextPage %>">
        <% = PageTitle %>
        <div id="pageText"><% = RecordInfo %></div>
        <hr />
        <% = ErrorMessage.ToString()%>
        <div id="pageText"><% = Tip %></div>
        <div class="pageTextInputWrapper">
        <% = MonthLabel %>
        <% = MonthList %>
        <% = DayLabel %> 
        <% = DaysList %>
        <% = TimeLabel %>
        <% = TimeList %>
        <% = Comments %>
        <div id="pageSubmitButton"><% = ButtonCaption %></div>
        </div>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>
