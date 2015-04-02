<%@ Page Language="C#" MasterPageFile="MobileACA.master" AutoEventWireup="true" Inherits="LoginResetPassword" Codebehind="Login.ResetPassword.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = HorizontalRow %>
    <% = iPhoneHeader %>
    <form id="form2" method="post" action="<% = NextPage %>"> 
           <% = PageContent %> 
    </form>
    <% = BackForwardLinks %> 
</asp:Content>