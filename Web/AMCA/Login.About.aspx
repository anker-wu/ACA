<%@ Page Language="C#" MasterPageFile="MobileACA.master" AutoEventWireup="true" Inherits="LoginAbout" Codebehind="Login.About.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% = HorizontalRow %>
    <% = iPhoneHeader %>
    <div id="pageWithNoForm">
        <div id="pageText">
            <% =LoginPageText%>
        </div>
    </div>
    <% = BackForwardLinks %> 
</asp:Content>
