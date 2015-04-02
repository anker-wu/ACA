<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Dialog.master" 
    CodeBehind="AvailableExaminationList.aspx.cs" Inherits="Accela.ACA.Web.Examination.AvailableExaminationList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div class="font11px available_examination_list">
        <asp:UpdatePanel ID="AvailableExaminationPanel" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdAvailableExamination" runat="server" />
            <div class="examination_list">
                <div class="ACA_TabRow ACA_Popup_Title">
                    <ACA:AccelaLabel ID="lblScheduleExamination" LabelKey="aca_exam_schedule_availableexaminations_title" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />
                <div class="ACA_TabRow_Italic">
                    <ACA:AccelaLabel ID="lblNoInspectionTypesFound" LabelKey="aca_exam_schedule_availableexaminations_notfound" runat="server"></ACA:AccelaLabel>
                </div>
                <div class="ACA_Label InspectionTypeGridView">
                    <ACA:AccelaGridView ID="gvAvailableExamination" CssClass="PopUpInspectionRow" runat="server" ShowHeader="false" role="presentation"
                        ShowHeaderWhenEmpty="false" AlternatingRowStyle-CssClass="InspectionTypeRow" RowStyle-CssClass="InspectionTypeRow" 
                        AllowPaging="True" PagerStyle-VerticalAlign="bottom" AutoGenerateCheckBoxColumn="false" OnRowDataBound="AvailableExamination_RowDataBound"
                         OnPageIndexChanging="AvailableExamination_PageIndexChanging" IsAutoWidth="true">
                    <Columns>
                        <ACA:AccelaTemplateField>
                            <ItemTemplate>
                                <ACA:AccelaRadioButton ID="rdAvailableExamination" Enabled='<%# DataBinder.Eval(Container.DataItem, "Enabled")%>' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExaminationName")%>' value='<%# DataBinder.Eval(Container.DataItem, "ExaminationID")%>' 
                                     Checked='<%# DataBinder.Eval(Container.DataItem, "Selected")%>' />
                            </ItemTemplate>
                        </ACA:AccelaTemplateField>
                    </Columns>
                    </ACA:AccelaGridView>
                </div>
            </div>
            <!-- button list -->
            <div class="buttons ACA_Row ACA_LiLeft">
                <ul>
                    <li>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaButton ID="lnkContinue" OnClick="ContinueButton_Click" LabelKey="aca_exam_schedule_action_continue" runat="server"/>
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