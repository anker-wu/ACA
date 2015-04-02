<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="MyCollectionsUpdate" Codebehind="MyCollections.Update.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" style="margin-top:0px;" method="post" action="MyCollections.Update.aspx?State=<% = State %>&Mode=<% = SearchMode %>&Module=<% = LocalModuleName %>&ResultPage=<% = ResultPage %>&CollectionOperation=<% = CollectionOperation %>&CollectionModule=<% = CollectionModule %>&PagingMode=true">
        <% = PageTitle %>
        <% = ErrorMessage.ToString()%>
        <% = PageMessage.ToString() %>
        <div class="pageTextInputWrapper">
        <% = ExistingOption.ToString() %>
        <% = NewOption.ToString() %>
        <% = Buttons.ToString() %>
        </div>
        <% = HiddenFields %>
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>
