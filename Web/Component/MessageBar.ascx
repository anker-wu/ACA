<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBar.ascx.cs"
    Inherits="Accela.ACA.Web.Component.MessageBar" %>
 <div class="ACA_Show">
<ACA:AccelaHeightSeparate ID="sepForMessageTop" runat="server" Height="10" visible =false /> 
<div id="messageBar" runat="Server" visible="false">
    <table role='presentation' border="0" cellpadding="0" cellspacing="0" style="table-layout:fixed">
        <tbody>
            <tr>
                <td style ="width:25px;padding-top:2px" vAlign="top">
                    <div>
                        <img id="imgMsgIcon" runat="Server" class="ACA_NoBorder"/>
                    </div>
                </td>
                <td width="15px">
                    <div>
                        &nbsp;
                    </div>
                </td>
                <td vAlign="top">
                    <div>
                        <ACA:AccelaLabel ID="lblMessageTitle" runat="Server" CssClass="ACA_Show" />
                        <ACA:AccelaLabel ID="lblMessage" runat="server" LabelType="BodyText" IsNeedEncode="False" />
                        <br/>
                        <ACA:AccelaButton Visible="False" ID="lnkEditContact" Font-Bold="True" EnableRecordTypeFilter="true" LabelKey="aca_account_contact_label_update_contact" runat="server" CausesValidation="false"/>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<ACA:AccelaHeightSeparate ID="sepForMessageBottom" runat="server" Height="10" visible =false  /> 
</div>  
