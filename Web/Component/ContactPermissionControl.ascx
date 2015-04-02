<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactPermissionControl.ascx.cs" Inherits="Accela.ACA.Web.Component.ContactPermissionControl" %>
<div>
  <div id="divContact" runat="server">
<ACA:AccelaRadioButtonList ID="radioListContactPermission" ListType="ContactRelationShips" runat="server" LabelKey="per_appinfoedit_contactpermission" RepeatDirection="Vertical" />
<div class="ACA_Margin_ContactRelationShips">
    <ACA:AccelaCheckBoxList ID="ckbCustomPermissionList" runat="server"></ACA:AccelaCheckBoxList>
</div>
</div>
</div> 