<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="InspectionWizardContacts" Codebehind="InspectionWizardContacts.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" style="margin-top:0px;" method="post" action="InspectionWizardContacts.aspx?State=<% = State %>&Module=<% = ModuleName %>&PagingMode=true">
        <% = PageTitle %>
        <div id="pageText"><% = RecordInfo %></div>
        <hr />
        <% = ErrorMessage.ToString()%>
        <% = PageMessage.ToString() %>
        <div class="pageTextInputWrapper">
        <% = ContactsList.ToString() %>
        <% = NewContact.ToString() %>
        <% = Buttons.ToString() %>
        </div>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>

