<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccelaTimeSelect.ascx.cs"
    Inherits="Accela.ACA.Web.Component.AccelaTimeSelect" %>
<div class="ACA_TabRow">    
        <ACA:AccelaLabel ID="lblTimeTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
</div>
<div class="ACA_TabRow">
    <table role='presentation'>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblHours" AssociatedControlID="ddlHours"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlHours" CssClass="ACA_New_Label font11px">
                </asp:DropDownList>
            </td>
            <td>
                <p>
                    <asp:Literal ID="lblTimeSeparator" runat="server"></asp:Literal>
                </p>
            </td>
            <td>
                <asp:Label runat="server" ID="lblMinutes" AssociatedControlID="ddlMinutes"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlMinutes" CssClass="ACA_New_Label font11px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Label runat="server" ID="lblAmPm" AssociatedControlID="ddlAmPm"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlAmPm" CssClass="ACA_New_Label font11px">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <ACA:AccelaLabel ID="lblTimeSubTitle" runat="server" CssClass="ACA_Sub_Label ACA_Sub_Label_FontSize"></ACA:AccelaLabel>
</div>
