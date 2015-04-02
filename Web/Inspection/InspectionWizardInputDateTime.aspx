<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectionWizardInputDateTime.aspx.cs"
    MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.Inspection.InspectionWizardInputDateTime" %>

<%@ Register Src="~/Component/InspectionReadyTime.ascx" TagName="InspectionReadyTime"
    TagPrefix="insp" %>
<%@ Register Src="~/Component/InspectionSameDayNextDayInput.ascx" TagName="SameDayNextDay"
    TagPrefix="insp" %>
<%@ Register Src="~/Component/ScheduleCalendar.ascx" TagName="ScheduleCalendar" TagPrefix="insp" %>
<asp:content id="Content1" contentplaceholderid="phPopup" runat="server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <asp:UpdatePanel ID="SchedulePanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div>
            <div class="ACA_TabRow_Italic font12px">
                <ACA:AccelaLabel ID="lblInpectionType" LabelKey="aca_inspection_type_label" runat="server"></ACA:AccelaLabel>
            </div>
            <div class="ACA_TabRow ACA_BkGray">&nbsp;</div>
            <insp:InspectionReadyTime ID="inspectionReadyTime" runat="server" />
            <insp:ScheduleCalendar ID="calendar" runat="server" />
            <insp:SameDayNextDay ID="sameDayNextDay" runat="server" />
        </div>
        <ACA:AccelaHeightSeparate ID="spAboveButton" Height="40" Visible="true" runat="server" />
        
        <!-- button list -->
        <div class="ACA_TabRow font11px">
        <table role='presentation'>
            <tr valign="bottom">
                <td>
                    <div class="ACA_LgButton ACA_LgButton_FontSize">
                        <ACA:AccelaButton ID="lnkContinue" OnClick="ContinueButton_Click" LabelKey="aca_inspection_action_continue" runat="server"/>
                    </div>
                 </td>
                 <td id="tdBackSpace" runat="server" class="PopupButtonSpace">&nbsp;</td>
                 <td id="tdBack" runat="server">
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkBack" OnClick="BackButton_Click" LabelKey="aca_inspection_action_back" runat="server" />
                    </div>
                 </td>
                 <td class="PopupButtonSpace">&nbsp;</td>
                 <td>
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="parent.ACADialog.close();; return false;" LabelKey="ACA_Inspection_Action_Cancel" runat="server" />
                    </div>
                 </td>
            </tr>
        </table>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    
    <script type="text/javascript">
        $("#lnkBeginFocus", getParentDocument()).focus();

        function calendarDateChanged(theLink) {
            SetWizardButtonDisable('<%= lnkContinue.ClientID %>', true);
        }

        function calendarTimeSelected(theRadioButton) {
            SetWizardButtonDisable('<%= lnkContinue.ClientID %>', false)
        }
    </script>
</asp:content>
