<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.AppSpecInfoTableView" Codebehind="AppSpecInfoTableView.ascx.cs" %>
<div class ="asit_section" style="overflow-x: auto; overflow-y:hidden;height:auto;">
     <asp:PlaceHolder ID="phAppInfoTable" runat="server" EnableTheming="true"></asp:PlaceHolder>
</div> 
<script type="text/javascript">
    var phAppInfoTableID = "<%=phAppInfoTable.ClientID %>";
    var asit_view_previx = phAppInfoTableID.replace("phAppInfoTable","");
</script>