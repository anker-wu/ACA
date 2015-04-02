<%@ Page Language="C#" MasterPageFile="MobileACA.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-top:0px; margin-bottom:0px; margin-left:5px; ">
        <form id="form2" runat="server" style="margin-top:0px;">
           <% = OutputLinks.ToString() %> 
        </form>
    </div>
</asp:Content>