<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Accela.ACA.Web.Error" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <div class="ACA_Content">
        <uc1:MessageBar ID = "systemErrorMessage" runat="Server" />
        <div class="ACA_Page ACA_Page_FontSize ACA_Row">
            <ACA:AccelaLabel ID="lblGoBack" IsNeedEncode="false" runat="server" />
        </div>
    </div> 
</div>
</asp:Content>
