<%@ Page Title="" Language="C#" MasterPageFile="~/AMCA/MobileACA.master" AutoEventWireup="true" CodeBehind="Login.SecurityQuestionVerify.aspx.cs" Inherits="LoginSecurityQuestionVerify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <% = HorizontalRow %>
    <% = iPhoneHeader %>
    <form id="form2" method="post" action="<% = NextPage %>"> 
           <% = PageContent %> 
    </form>
    <% = BackForwardLinks %>
</asp:Content>
