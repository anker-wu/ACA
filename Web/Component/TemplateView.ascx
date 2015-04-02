<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.TemplateView" Codebehind="TemplateView.ascx.cs" %>
    
<%@ Import namespace="Accela.ACA.Common.Util" %>
<%@ Import namespace="Accela.ACA.Web.Common" %>
<%@ Import namespace="Accela.ACA.WSProxy" %>

<asp:Panel ID="pnlTemplate" runat="server" Width="100%" Height="100%">
    <asp:Repeater ID="rptAttribute" runat="server" OnItemDataBound="AttributeRepeater_ItemDataBound">
        <ItemTemplate>
            <table role='presentation' class="font11px color666 NoBorder">
                <tr>
                    <td class="ACA_TabRow_Top"><nobr>
                        <%# Accela.ACA.Common.Common.ScriptFilter.FilterScript(Convert.ToString( DataBinder.Eval(Container.DataItem, "attributeLabel")))%>:</nobr>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ACA_Break_Wrod">
                        <%#DisplayTemplateValue(Container.DataItem as TemplateAttributeModel)%>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <ACA:AccelaHeightSeparate ID="sepForTemplete" runat="server" Height="5" />
</asp:Panel>
