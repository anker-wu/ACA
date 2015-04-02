<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.MyCollectionCAPs" Codebehind="MyCollectionCAPs.ascx.cs" %>
<%@ Register src="CollectionOperation.ascx" TagName="CollectionOperation" TagPrefix="ACA" %>
<%@ Register src="CapList.ascx" TagName="CapList" TagPrefix="ACA" %>

<%@ import namespace="Accela.ACA.Common" %>

 <script type="text/javascript" src="../Scripts/ShoppingCartMethods.js"></script>
 <div class="ACA_TabRow"> 
    <div>
        <div class="ACA_TabRow ACA_Split_Line">&nbsp;</div>
        <table role='presentation'>
            <tr>
                <td><b><ACA:AccelaLabel ID="lblModuleName" IsNeedEncode="false" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize"></ACA:AccelaLabel></b></td>
            </tr>
        </table>
        <div><ACA:CollectionOperation ID="CollectionOperation" runat="server" /></div> 
    </div>
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
       <ACA:CapList ID="capList" ShowPermitAddress="true" runat="server" Windowless="true" GViewID="60033"/>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>