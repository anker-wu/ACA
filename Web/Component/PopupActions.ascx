<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupActions.ascx.cs" Inherits="Accela.ACA.Web.Component.PopupActions" %>
<div id="divMenu" class="ACA_FRight" runat="server">
    <table role='presentation' class="NoBorder">
        <tr>
            <td class="ACA_LinkButton">
                <a id="<%=ActionsLinkClientID %>" onclick="ShowActionMenus(this.parentNode.parentNode, '<%= ActionMenu.ClientID %>','<%=IsRTLstring %>');" href="javascript:void(0);" class="nav_more_arrow NotShowLoading actionMenu_LinkAndArrow" title="<%=ActionToolTip %>" >
                    <ACA:AccelaLabel ID="lblActions" runat="server"></ACA:AccelaLabel>
                    <img alt="" border="0" src="<%=ImageUtil.GetImageURL("caret_arrow.gif") %>" />
                </a>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="ActionMenus" runat="server">
                    <div id="ActionMenu" class="ACA_ALeft ACA_Nowrap ACA_LinkButton ACA_Delegate_Action ACA_Hide" onmouseover="EnterActionMenus(this.id);" onmouseout="ExitActionMenus(this.id);" runat="server">
                        <asp:UpdatePanel ID="upActionMenu" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table role='presentation' class="ACA_TDAlignLeftOrRightTopNoBorder">
                                    <tr>
                                        <td class="ActionMenu_Link" id="MenuItemContainer" runat="server">
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr> 
    </table>
</div>
<div id="divIco" class="ACA_FRight" runat="server"></div>