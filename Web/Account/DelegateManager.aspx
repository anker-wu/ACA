<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DelegateManager.aspx.cs" 
   MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.Account.DelegateManager" %>
<%@ Register src="~/Component/DelegateManagement.ascx" tagname="DelegateManagement" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script type="text/javascript">
        function CloseDialog() {
            parent.ACADialog.close();
        }
    </script>
    <uc1:DelegateManagement ID="delegateManagement" runat="server" /> 
</asp:Content>
