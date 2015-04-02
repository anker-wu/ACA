<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.WorkStatus" Codebehind="WorkStatus.ascx.cs" %>
<div id="divLoading" class="ACA_SmLabel ACA_SmLabel_FontSize ACA_Hide">
    <%= GetTextByKey("capdetail_message_loading")%>
</div>
<asp:UpdatePanel ID="workFlowProcess" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div id="divProcessInstruction" runat="server"  visible="false">
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="15" runat="server" />
            <label id="instructionText" runat="server" />
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="15" runat="server" />
            <ACA:AccelaLabel ID="lblUpcomingComboField" LabelKey="acaadmin_workflow_msg_configuration" LabelType="BodyText" runat="server" />
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="25" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>