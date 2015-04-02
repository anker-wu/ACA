<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.SupervisorEdit" Codebehind="SupervisorEdit.ascx.cs" %>
<asp:Panel ID="pnlEMSETemplate" runat="server" width="100%" Height="100%" CssClass="ACA_Label ACA_Label_FontSize">
</asp:Panel>
<%if(!string.IsNullOrEmpty(ScriptName4Supervisor)){ %>
<script type="text/javascript">
    <%=Script4Supervisor %>
</script>
<%} %>