<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExamList.ascx.cs" Inherits="Accela.ACA.Web.Component.ExamList" %>
<%@ Register Src="PopupActions.ascx" TagName="PopupActions" TagPrefix="uc1" %>
<%@ Import Namespace="Accela.ACA.Web.Examination" %>
<script src="../Scripts/Dialog.js" type="text/javascript"></script>

<asp:UpdatePanel ID="examinationUpdatePanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div runat="server" id="divExaminationScheduleLink" class="ACA_Page">
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate8" Height="5" runat="server" />
            <div class="ACA_LinkButton ACA_Title_Color font12px">
                <a id="lnkExaminationSchedule" href="javascript:void(0)" class="NotShowLoading" onclick="ShowExamPopupDialogNoCheckWorkFlow('<%=GetScheduleOrRequestUrl() %>','lnkExaminationSchedule')">
                    <ACA:AccelaLabel ID="lblExaminationScheduledLink" LabelKey="aca_examination_schedule_link"
                        runat="server"></ACA:AccelaLabel></a>
            </div>
        </div>
        <asp:UpdatePanel ID="pendingPanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div id="divExaminationListPending" class="ACA_Page">
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />
                    <h1 class="font13px">
                        <ACA:AccelaLabel ID="lblExaminationPending" LabelKey="aca_examination_pending_label" runat="server"></ACA:AccelaLabel>
                    </h1>
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate10" Height="10" runat="server" />
                    <ACA:AccelaGridView ID="gvListPending" runat="server" PageSize="5" ShowHeader="false" role="presentation"
                        ShowHeaderWhenEmpty="false" AlternatingRowStyle-CssClass="InspectionListRow"
                        RowStyle-CssClass="InspectionListRow" AllowPaging="True" PagerStyle-VerticalAlign="bottom"
                        AutoGenerateCheckBoxColumn="false" OnRowDataBound="ListCompleted_RowDataBound">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <table role="presentation" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td class="ACA_Width45em">
                                                <%# ((ExaminationListItemViewModel)Container.DataItem).CombinedInfo %>
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td valign="top">
                                                <uc1:PopupActions ID="paActionMenu" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate10" Height="5" runat="server" />
                                </ItemTemplate>
                            </ACA:AccelaTemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ACA_TabRow_Italic">
                                <ACA:AccelaLabel ID="lblEmptyPending" LabelKey="aca_examination_pending_emptydata_label"
                                    LabelType="BodyText" runat="server"></ACA:AccelaLabel>
                            </div>
                        </EmptyDataTemplate>
                    </ACA:AccelaGridView>
                    <div id="divPendingComboField" visible="false" runat="server">
                        <span>Click the area below to edit examination list formats available variables:</span><br />
                        <span>$$ExaminationName$$: The examination name</span><br />
                        <span>$$ExaminationRequired$$: The examination is required or optional</span><br />
                        <span>$$ExaminationDateTime$$: The date and time of the examination</span><br />
                        <span>$$ExaminationAddress$$: The examination address</span><br />
                        <span>$$ProviderName$$: The examination provider</span>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate13" Height="10" runat="server" />
                        <ACA:AccelaLabel ID="AccelaLabel2" LabelKey="aca_examination_pendinglist_combofield_pattern" LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate14" Height="10" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="readySchedulePanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div id="div1" class="ACA_Page">
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate5" Height="10" runat="server" />
                    <h1 class="font13px">
                        <ACA:AccelaLabel ID="lblReadyToSchedule" LabelKey="aca_examlist_label_readytoschedule_title" runat="server"></ACA:AccelaLabel>
                    </h1>
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate11" Height="10" runat="server" />
                    <ACA:AccelaGridView ID="gvReadyToSchedule" runat="server" PageSize="5" ShowHeader="false" role="presentation"
                        ShowHeaderWhenEmpty="false" AlternatingRowStyle-CssClass="InspectionListRow"
                        RowStyle-CssClass="InspectionListRow" AllowPaging="True" PagerStyle-VerticalAlign="bottom"
                        AutoGenerateCheckBoxColumn="false" OnRowDataBound="ListCompleted_RowDataBound">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <table role="presentation" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td class="ACA_Width45em">
                                                <%# ((ExaminationListItemViewModel)Container.DataItem).CombinedInfo %>
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td valign="top">
                                                <uc1:PopupActions ID="paActionMenu" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate10" Height="5" runat="server" />
                                </ItemTemplate>
                            </ACA:AccelaTemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ACA_TabRow_Italic">
                                <ACA:AccelaLabel ID="lblEmptyReadyToSchedule" LabelKey="aca_examlist_label_readytoschedule_emptydata"
                                    LabelType="BodyText" runat="server"></ACA:AccelaLabel>
                            </div>
                        </EmptyDataTemplate>
                    </ACA:AccelaGridView>
                    <div id="divReadyToScheduleComboField" visible="false" runat="server">
                        <span>Click the area below to edit examination list formats Available variables:</span><br />
                        <span>$$ExaminationName$$: The examination name</span><br />
                        <span>$$ExaminationRequired$$: The examination is required or optional</span><br />
                        <span>$$ExaminationDateTiem$$: The date and time of the examination</span><br />
                        <span>$$SupportLanguages$$: The examination support languages</span><br />
                        <span>$$ExaminationAddress$$: The examination address</span><br />
                        <span>$$ExaminationAccessibility$$: The examination accessibility</span><br />
                        <span>$$ProviderName$$: The examination provider</span>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate12" Height="10" runat="server" />
                        <ACA:AccelaLabel ID="lblPatternView" LabelKey="aca_examlist_combofield_pattern_readytoschedule" LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate15" Height="10" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="schedulePanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div id="divExaminationListScheduled" class="ACA_Page">
                    <h1 class="font13px">
                        <ACA:AccelaLabel ID="lblExaminationScheduled" LabelKey="aca_examination_scheduled_label" runat="server"></ACA:AccelaLabel>
                    </h1>
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="10" runat="server" />
                    <ACA:AccelaGridView ID="gvListScheduled" runat="server" PageSize="5" ShowHeader="false" ShowHeaderWhenEmpty="false" AlternatingRowStyle-CssClass="InspectionListRow"
                        RowStyle-CssClass="InspectionListRow" AllowPaging="True" PagerStyle-VerticalAlign="bottom" role="presentation"
                        AutoGenerateCheckBoxColumn="false" OnRowDataBound="ListScheduled_RowDataBound">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <table role="presentation" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td class="ACA_Width45em">
                                                <%# ((ExaminationListItemViewModel)Container.DataItem).CombinedInfo %>
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td valign="top">
                                                <uc1:PopupActions ID="paActionMenu" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate10" Height="8" runat="server" />
                                </ItemTemplate>
                            </ACA:AccelaTemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ACA_TabRow_Italic">
                                <ACA:AccelaLabel ID="lblEmptyUpcoming" LabelKey="aca_examintion_upcoming_emptydata_label"
                                    LabelType="BodyText" runat="server"></ACA:AccelaLabel>
                            </div>
                        </EmptyDataTemplate>
                    </ACA:AccelaGridView>
                    <div id="divScheduledComboField" visible="false" runat="server">
                        <span>Click the area below to edit examination list formats Available variables:</span><br />
                        <span>$$ExaminationName$$: The examination name</span><br />
                        <span>$$ExaminationRequired$$: The examination is required or optional</span><br />
                        <span>$$ExaminationDateTiem$$: The date and time of the examination</span><br />
                        <span>$$RosterID$$: The examination roster ID</span><br />
                        <span>$$SupportLanguages$$: The examination support languages</span><br />
                        <span>$$ExaminationAddress$$: The examination address</span><br />
                        <span>$$ExaminationAccessibility$$: The examination accessibility</span><br />
                        <span>$$ProviderName$$: The examination provider</span>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="10" runat="server" />
                        <ACA:AccelaLabel ID="lblUpcomingComboField" LabelKey="aca_examination_scheduledlist_combofield_pattern" LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" Height="10" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="completedPanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div id="divExaminationListCompleted" class="ACA_Page">
                    <h1 class="font13px">
                        <ACA:AccelaLabel ID="lblExaminationCompleted" LabelKey="aca_examination_completed_label" runat="server"></ACA:AccelaLabel>
                    </h1>
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate6" Height="10" runat="server" />
                    <ACA:AccelaGridView ID="gvListCompleted" runat="server" PageSize="5" ShowHeader="false" role="presentation"
                        ShowHeaderWhenEmpty="false" AlternatingRowStyle-CssClass="InspectionListRow"
                        RowStyle-CssClass="InspectionListRow" AllowPaging="True" PagerStyle-VerticalAlign="bottom"
                        AutoGenerateCheckBoxColumn="false" OnRowDataBound="ListCompleted_RowDataBound">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <table role="presentation" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td class="ACA_Width45em">
                                                <%# ((ExaminationListItemViewModel)Container.DataItem).CombinedInfo %>
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td valign="top">
                                                <uc1:PopupActions ID="paActionMenu" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate9" Height="5" runat="server" />
                                </ItemTemplate>
                            </ACA:AccelaTemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ACA_TabRow_Italic">
                                <ACA:AccelaLabel ID="lblEmptyCompleted" LabelKey="aca_examination_completed_emptydata_label" LabelType="BodyText" runat="server"></ACA:AccelaLabel>
                            </div>
                        </EmptyDataTemplate>
                    </ACA:AccelaGridView>
                    <div id="divCompletedComboField" visible="false" runat="server">
                        <span>Click the area below to edit examination list formats Available variables:</span><br />
                        <span>$$ExaminationName$$: The examination name</span><br />
                        <span>$$ExaminationRequired$$: The examination is required or optional</span><br />
                        <span>$$ExaminationDateTime$$: The date and time of the examination</span><br />
                        <span>$$RosterID$$: The examination roster ID</span><br />
                        <span>$$ExaminationAddress$$: The examination address</span><br />
                        <span>$$ProviderName$$: The examination provider</span><br />
                        <span>$$FinalScore$$: The examination final score</span>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate7" Height="10" runat="server" />
                        <ACA:AccelaLabel ID="lblCompletedComboField" LabelKey="aca_examination_completedlist_combofield_pattern"
                            LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate9" Height="10" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button runat="server" ID="btnRefreshExamGV" CausesValidation="false" OnClick="RefreshExamGVButton_Click" CssClass="ACA_Hide" />
        <asp:Button runat="server" ID="btnRedirctPage" CausesValidation="false" OnClick="RedirctPageButton_Click" CssClass="ACA_Hide" />
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">

    var popupDialogWidth4ScheduleExam = 750;
    var popupDialogHeight4ScheduleExam = 600;

    function closeExaminationPopupDialog() {
        ACADialog.close();
    }

    function ShowExaminationPopupDialog(pageUrl, objectTargetID, examNumber, moduleName, status, action) {
        PageMethods.HasWrokflowRestricted(examNumber, moduleName, status, action, pageUrl, objectTargetID, callbackShowExaminationPopupDialog);
    }

    function callbackShowExaminationPopupDialog(result) {
        if (result[0] == "true") {
            alert(result[3]);
        }
        else {
            var pageUrl = result[1];
            var objectTargetID = result[2];
            ACADialog.popup({ url: pageUrl, width: popupDialogWidth4ScheduleExam, height: popupDialogHeight4ScheduleExam, objectTarget: objectTargetID });
        }
    }

    function ShowExamPopupDialogNoCheckWorkFlow(pageUrl, objectTargetID) {
        ACADialog.popup({ url: pageUrl, width: popupDialogWidth4ScheduleExam, height: popupDialogHeight4ScheduleExam, objectTarget: objectTargetID });
    }

    function RedirctToFeePage() {
        ACADialog.close();
        
        window.setTimeout(function() {
             delayShowLoading();
         }, 100);
        
        $get("<%=btnRedirctPage.ClientID %>").click();
     }

     function LoadInitExaminationList() {
         ACADialog.close();

         window.setTimeout(function() {
             delayShowLoading();
         }, 100);
         
         $get("<%=btnRefreshExamGV.ClientID %>").click();
         $("#lnkExaminationSchedule").focus();
     }

     function DeleteExamination(agencyCode, capId1, capId2, capId3, examNbr) {
         if (confirm('<%=GetTextByKey("acc_message_confirm_removeexamination").Replace("'","\\'") %>')) {
             var p = new ProcessLoading();
             p.showLoading();
             PageMethods.DeleteExamination(agencyCode, capId1, capId2, capId3, examNbr, callbackDeleteExamination);
         }
     }

     function callbackDeleteExamination() {
         $get("<%=btnRefreshExamGV.ClientID %>").click();
         $("#lnkExaminationSchedule").focus();
     }

     function getSSOLink(agency, providerNbr, userExamId) {
         PageMethods.GetSSOLink(agency, providerNbr, userExamId, callbackOpenTakeExamination);
     }

     function callbackOpenTakeExamination(result) {
         hideDialogLoading();
         if (result) {
             var a = window.open(result, "_blank", "top=200,left=200,height=550,width=850,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes");
         } else {
             showNormalMessage('<%=GetTextByKey("aca_exam_schedule_getssolink_error").Replace("'","\\'") %>', 'Error');
        }
     }

    function resetExamination(examNbr, moduleName) {
        var p = new ProcessLoading();
        p.showLoading();
        PageMethods.ResetReady2ScheduleExamiation(examNbr, moduleName, resetExaminationCallback);
     }

    function resetExaminationCallback(errMsg) {
        if (!errMsg) {
            $get("<%=btnRefreshExamGV.ClientID %>").click();
        } else {
            showNormalMessage(errMsg, 'Error');
        }
    }

</script>
