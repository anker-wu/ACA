<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.CapDescriptionView" Codebehind="CapDescriptionView.ascx.cs" %>
<div id="divContent" runat="server" class="ACA_TabRow ACA_ConfigInfo ACA_FLeft ACA_Cap_Description_View">
    <div id="divJob" runat="server" class="ACA_Row">
        <table role='presentation'>
            <tr>
                <td>
                    <p>
                        <strong>
                            <ACA:AccelaLabel ID="capDescriptionView_label_jobValue" LabelKey="capDescriptionEdit_label_jobValue"
                                runat="server"></ACA:AccelaLabel>
                        </strong>
                    </p>
                </td>
                <td>
                    <p>
                        <strong>
                            <ACA:AccelaLabel ID="lbljobValue" runat="server" />
                        </strong>
                    </p>
                </td>
            </tr>
        </table>
    </div>
    <div id="divHourse" runat="server" class="ACA_Row">
        <table role='presentation'>
            <tr>
                <td id="tdHouseUnitLabel" runat="server">
                    <p>
                        <ACA:AccelaLabel ID="capDescriptionView_label_houseUnit" LabelKey="capDescriptionEdit_label_houseUnit"
                            runat="server"></ACA:AccelaLabel></p>
                </td>
                <td id="tdHouseUnitBox" runat="server">
                    <p>
                        <ACA:AccelaLabel ID="lblHouseUnit" runat="server" Visible="true"></ACA:AccelaLabel></p>
                </td>
                <td id="tdSpearFrom" runat="server">
                    <div runat="server" id="divSpearFrom">
                        &nbsp;&nbsp;</div>
                </td>
                <td>
                    <p>
                        <ACA:AccelaLabel ID="capDescriptionView_label_buildNumber" LabelKey="capDescriptionEdit_label_buildNumber"
                            runat="server"></ACA:AccelaLabel></p>
                </td>
                <td>
                    <p>
                        <ACA:AccelaLabel ID="lblBuildNumber" runat="server" Visible="true"></ACA:AccelaLabel></p>
                </td>
            </tr>
        </table>
    </div>
    <div id="divOwner" runat="server" class="ACA_Row">
        <table role='presentation'>
            <tr>
                <td>
                    <p>
                        <ACA:AccelaLabel ID="capDescriptionView_label_publicOwner" LabelKey="capDescriptionEdit_label_publicOwner"
                            runat="server"></ACA:AccelaLabel></p>
                </td>
                <td>
                    <p>
                        <ACA:AccelaLabel ID="lblPublicOwner" runat="server" Visible="true"></ACA:AccelaLabel></p>
                </td>
            </tr>
        </table>
    </div>
    <div id="divConstructType" runat="server" class="ACA_Row">
        <table role='presentation'>
            <tr>
                <td>
                    <p>
                        <ACA:AccelaLabel ID="capDescriptionView_label_constructType" LabelKey="capDescriptionEdit_label_constructType"
                            runat="server"></ACA:AccelaLabel></p>
                </td>
                <td>
                    <p>
                        <ACA:AccelaLabel ID="lblConstructType" IsNeedEncode="false" runat="server" Visible="true"></ACA:AccelaLabel></p>
                </td>
            </tr>
        </table>
    </div>
</div>
