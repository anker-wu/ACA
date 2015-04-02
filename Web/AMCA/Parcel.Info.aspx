<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MobileACA.FooterOnly.master" Inherits="ParcelInfo" Codebehind="Parcel.Info.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = Breadcrumbs %> 
    <% = iPhoneHeader %>
    <form id="form1" method="post" action="Parcel.Info.aspx.cs?State=<% = State %>&Module=<% = ModuleName %>">
        <% = ErrorMessage.ToString()%>
        <% = PageTitle %>
        <% = ParcelDetail %>
        <% = LegalDescription %>
        <% = AddressList %>
        <% = OwnerList %>
        <% = HiddenFields %>
        <br />
    </form>
    <% = iPhoneBreadcrumbs %>
    <% = iPhoneFooter %>
    <% = BackForwardLinks %>
</asp:Content>