<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExaminationRescheduleReason.aspx.cs" MasterPageFile="~/Dialog.master"
Inherits="Accela.ACA.Web.Examination.ExaminationRescheduleReason" %>

<%@ Register src="../Component/ExaminationReasonList.ascx" tagname="ExaminationReasonList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div class="font11px examination_reschedule_reason">
        <asp:UpdatePanel ID="AvailableExaminationPanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div class="ACA_TabRow ACA_Popup_Title">
                    <ACA:AccelaLabel ID="lblReason" LabelKey="aca_exam_schedule_reschedule_reason" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />        
                <div class="ACA_TabRow_Italic">
                    <ACA:AccelaLabel ID="lblNoInspectionTypesFound" LabelKey="aca_exam_schedule_reason_notfound" runat="server"></ACA:AccelaLabel>
                </div>
                <div class="ACA_Label InspectionTypeGridView">
                    <uc1:ExaminationReasonList ID="ExaminationReasonList" OnPageIndexChanging="ExaminationReasonList_PageIndexChanging" runat="server" />
                </div>
                <!-- button list -->
                <div class="buttons ACA_Row ACA_LiLeft">
                    <ul>
                        <li>
                            <div class="ACA_LgButton ACA_LgButton_FontSize">
                                <ACA:AccelaButton ID="lnkContinue" OnClick="ContinueButton_Click" LabelKey="aca_exam_schedule_action_continue" runat="server" />
                            </div>
                        </li>
                        <li>
                            <div class="ACA_LinkButton">
                                <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="aca_exam_schedule_action_cancel" runat="server" />
                            </div>
                        </li>
                    </ul> 
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        function SetButtonStatus() {
            SetWizardButtonDisable('<%=lnkContinue.ClientID%>', false);
        }
    </script>
</asp:Content>
