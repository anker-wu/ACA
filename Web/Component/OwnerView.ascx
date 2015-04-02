<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.OwnerView" Codebehind="OwnerView.ascx.cs" %>
<%@ Register Src="TemplateView.ascx" TagName="TemplateView" TagPrefix="uc1" %>
<div class="ACA_SmLabel ACA_SmLabel_FontSize" runat="server">
 
  <table role='presentation' id="tbOwnerDetail" style="line-height:16px;" runat="server" border='0' cellpadding='0' cellspacing='0'>
    <tr>
        <td>
            <strong>
                <ACA:AccelaLabel ID="lblOwnerTitle" runat="server" />
                <ACA:AccelaLabel ID="lblOwnerName" runat="server" />
            </strong>
        </td>
    </tr>
    <tr>
        <td>
            <ACA:AccelaLabel ID="lblAddress1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <ACA:AccelaLabel ID="lblAddress2" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <ACA:AccelaLabel ID="lblAddress3" runat="server" />
        </td>
    </tr>
    <tr>
         <td>
            <ACA:AccelaLabel ID="lblCity" runat="server" />
            <ACA:AccelaLabel ID="lblState" runat="server" />
            <ACA:AccelaLabel ID="lblZip" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <ACA:AccelaLabel ID="IblCountry" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <ACA:AccelaLabel ID="IblPhone" IsNeedEncode="false" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <ACA:AccelaLabel ID="IblFax" IsNeedEncode="false" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <ACA:AccelaLabel ID="IblEmail" IsNeedEncode="false" runat="server" />
        </td>
    </tr>
  </table>
 
</div>
<uc1:TemplateView id="templateView" runat="server"/>
