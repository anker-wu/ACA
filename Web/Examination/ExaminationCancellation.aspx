<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExaminationCancellation.aspx.cs" MasterPageFile="~/Dialog.master"
Inherits="Accela.ACA.Web.Examination.ExaminationCancellation" %>

<%@ Register src="../Component/ExaminationReasonList.ascx" tagname="ExaminationReasonList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div class="font11px examination_cancellation">
        <asp:UpdatePanel ID="AvailableExaminationPanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                 <div class="ACA_TabRow ACA_Popup_Title">
                    <ACA:AccelaLabel ID="lblReason" LabelKey="aca_exam_schedule_cancel_confirm_reason" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />        
                <div class="ACA_TabRow_Italic">
                    <ACA:AccelaLabel ID="lblNoInspectionTypesFound" LabelKey="aca_exam_cancel_reason_notfound" runat="server"></ACA:AccelaLabel>
                </div>
                <div class="ACA_Label InspectionTypeGridView">
                    <uc1:ExaminationReasonList ID="ExaminationReasonList" OnPageIndexChanging="ExaminationReasonList_PageIndexChanging" runat="server" />
                </div>
                <!-- button list -->
                <div class="buttons ACA_Row ACA_LiLeft">
                    <ul>
                        <li>
                            <div class="ACA_LgButton ACA_LgButton_FontSize">
                                <ACA:AccelaButton class="NotShowLoading" ID="lnkContinue" OnClick="ContinueButton_Click" LabelKey="aca_exam_schedule_cancel_confirm_button" runat="server"/>
                            </div>
                        </li>
                        <li>
                            <div class="ACA_LinkButton">
                                <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="aca_exam_schedule_cancel_confirm_closebutton" runat="server" />
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

        function DisplayLoading() {
             // show loading div
            var loadingTitle = '<%=GetTextByKey("aca_global_msg_loading").Replace("'","\\'") %>';
            parent.showDialogLoading(loadingTitle);
        }

        function CancelSuccessful() {
            if (typeof (needInitControlLoading) != "undefined") {
                needInitControlLoading = false;
            }
           parent.LoadInitExaminationList();
        }
</script>
</asp:Content>
