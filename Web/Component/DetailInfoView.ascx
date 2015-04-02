<%@ Control Language="C#" AutoEventWireup="true" Inherits="DetailInfoView" Codebehind="DetailInfoView.ascx.cs" %>
    <div id="tblContent" runat="server" class="ACA_SmLabel ACA_SmLabel_FontSize">
        
            <table role='presentation' id="tbDetailInfoView" style="line-height:16px;" runat="server" border='0' cellpadding='0' cellspacing='0'>
                <tr>
                    <td>
                        <ACA:AccelaLabel ID="lblApplicationName" IsNeedEncode="false" runat="server" Visible="true">
                        </ACA:AccelaLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <ACA:AccelaLabel ID="lblnote" IsNeedEncode="false" runat="server" Visible="true">
                        </ACA:AccelaLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <ACA:AccelaLabel ID="lblDetaileDescription" IsNeedEncode="false" runat="server" Visible="true"></ACA:AccelaLabel>
                    </td>
                </tr>
            </table>
        
    </div>
    <ACA:AccelaHeightSeparate ID="sepForDetail" runat="server" Height="10" />
