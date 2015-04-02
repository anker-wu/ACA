<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.InspectionList" %>
<%@ Register Src="PopupActions.ascx" TagName="PopupActions" TagPrefix="uc1" %>
<%@ Import Namespace="Accela.ACA.Web.Inspection" %>
<script src="../Scripts/Dialog.js" type="text/javascript"></script>
<script type="text/javascript">

    var inspectionIsLoaded = false;
    var upcomingListLoaded = false;
    var completedListLoaded = false;
    var interval4HideAnimations = "";
    var popupDialogWidth4Scheduling = 640;
    var popupDialogWidth4Details = 750;
    var popupDialogHeight4Scheduling = 456;
    var popupDialogHeight4Details = 456;

    function showInspectionPopupDialog(pageUrl, className, objectTargetID) {
        var popupWidth = className == "InspectionDetailsPageWidth" ? popupDialogWidth4Details : popupDialogWidth4Scheduling;
        var popupHeight = className == "InspectionDetailsPageWidth" ? popupDialogHeight4Details : popupDialogHeight4Scheduling;
        ACADialog.popup({ url: pageUrl, width: popupWidth, height: popupHeight, objectTarget: objectTargetID });
    }

    function adjustPopupDialogWidthHeight4Scheduling() {
        ACADialog.fixHeight(popupDialogHeight4Scheduling);
        ACADialog.fixWidth(popupDialogWidth4Scheduling);
    }

    function closeInpsectionPopupDialog() {
        ACADialog.close();
    }

    function HandleExeception(errorMessage) {
        HideAnimations4WholeList(true);
    }

    function HideAnimations4WholeList(forceToHide) {
        if (upcomingListLoaded && completedListLoaded || forceToHide) {
            $("#inspectionLoding").addClass("ACA_Hide");

            if (interval4HideAnimations != "") {
                window.clearInterval(interval4HideAnimations);
                interval4HideAnimations = "";
            }

            return true;
        }
        else {
            return false;
        }
    }

    function onUpcomingListLoaded() {
        if (!$.global.isAdmin && !inspectionIsLoaded) {
            return;
        }
        
        $("#divInspectionListUpcoming").removeClass("ACA_Hide");
        upcomingListLoaded = true;
        var hideSucceeded = HideAnimations4WholeList();

        if (!hideSucceeded && interval4HideAnimations == "") {
            interval4HideAnimations = window.setInterval(HideAnimations4WholeList, 50);
        }

        if (theForm.elements["__LASTFOCUS_ID"] != null) {
            $("#" + theForm.elements["__LASTFOCUS_ID"].value).focus();
        }
    }

    function onCompletedListLoaded() {
        if (!$.global.isAdmin && !inspectionIsLoaded) {
            return;
        }
        
        $("#divInspectionListCompleted").removeClass("ACA_Hide");
        completedListLoaded = true;
        var hideSucceeded = HideAnimations4WholeList();

        if (!hideSucceeded && interval4HideAnimations == "") {
            interval4HideAnimations = window.setInterval(HideAnimations4WholeList, 50);
        }
    }

    function LoadInitInspectionList(delayLoading) {
        if (delayLoading) {
            window.setTimeout(function () {
                delayShowLoading();
            }, 100);
        }

        upcomingListLoaded = false;
        completedListLoaded = false;
        if ($.global.isAdmin) {
            $("#divInspectionListUpcoming").removeClass("ACA_Hide");
            $("#divInspectionListCompleted").removeClass("ACA_Hide");
        }
        else {
            __doPostBack('<%=btnRefreshGridView.UniqueID %>', null);
        }
    }
</script>
<div id="inspectionLoding" class="ACA_SmLabel ACA_SmLabel_FontSize">
    <%= GetTextByKey("capdetail_message_loading") %></div>
<asp:UpdatePanel ID="inspectionUpdatePanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <asp:LinkButton ID="btnRefreshGridView" runat="Server" CssClass="ACA_Hide" OnClick="RefreshGridViewButton_Click" TabIndex="-1"></asp:LinkButton>
        <asp:UpdatePanel ID="upcomingPanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div id="divInspectionListUpcoming" class="ACA_Page ACA_Hide">
                    <h1 class="font13px">
                        <ACA:AccelaLabel ID="lblInspectionUpcoming" LabelKey="aca_inspection_upcoming_label"
                            runat="server"></ACA:AccelaLabel>
                    </h1>
                    <div runat="server" id="divInspectionScheduleLink">
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="5" runat="server" />
                        <div class="ACA_LinkButton ACA_Title_Color font12px">
                            <a id="lnkInspectionSchedule" href="javascript:void(0)" class="NotShowLoading" onclick="showInspectionPopupDialog('<%=GetScheduleOrRequestUrl() %>','InspectionWizardPageWidth','lnkInspectionSchedule')">
                                <ACA:AccelaLabel ID="lblInspectionSchedule" LabelKey="aca_inspection_schedule_link"
                                    runat="server"></ACA:AccelaLabel></a>
                        </div>
                    </div>
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="10" runat="server" />
                    <ACA:AccelaGridView ID="gvListUpcoming" runat="server" PageSize="5" ShowHeader="false" role="presentation"
                        ShowHeaderWhenEmpty="false" AlternatingRowStyle-CssClass="InspectionListRow"
                        RowStyle-CssClass="InspectionListRow" AllowPaging="True" PagerStyle-VerticalAlign="bottom"
                        AutoGenerateCheckBoxColumn="false" OnRowDataBound="ListUpcoming_RowDataBound">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <table role="presentation" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td class="ACA_Width45em">
                                                <%# ((InspectionListItemViewModel)Container.DataItem).CombinedInfo %>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td valign="top">
                                                <uc1:PopupActions ID="paActionMenu" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </ACA:AccelaTemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ACA_TabRow_Italic">
                                <ACA:AccelaLabel ID="lblEmptyUpcoming" LabelKey="aca_inspection_upcoming_emptydata_label"
                                    LabelType="BodyText" runat="server"></ACA:AccelaLabel>
                            </div>
                        </EmptyDataTemplate>
                    </ACA:AccelaGridView>
                    <div id="divUpcomingComboField" visible="false" runat="server">
                        <span>Click the area below to edit inspection list formats Available variables:</span><br />
                        <span>$$RequiredOrOptional$$: The inspection is required or optional</span><br />
                        <span>$$ReadyTimeDateTime$$: The date and time of inspection ready time</span><br />
                        <span>$$ReadyTimeDate$$: The date part of inspection ready time</span><br />
                        <span>$$ReadyTimeTime$$: The time part of inspection ready time</span><br />
                        <span>$$ScheduledOrRequestedDateTime$$: Inspection requested or scheduled date and time</span><br />
                        <span>$$ScheduledOrRequestedDate$$: Inspection requested or scheduled date</span><br />
                        <span>$$ScheduledOrRequestedTime$$: Inspection requested or scheduled time</span><br />
                        <span>$$InspectionType$$: Inspection type</span><br />
                        <span>$$InspectionSequenceNumber$$: Inspection sequence number</span><br />
                        <span>$$Inspector$$: Inspector</span><br />
                        <span>$$Status$$: Inspection status</span><br />
                        <span>$$EstimatedArrivalTime$$: Estimated Arrival Time</span>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="15" runat="server" />
                        <ACA:AccelaLabel ID="lblUpcomingComboField" LabelKey="aca_inspection_upcominglist_combofield_pattern"
                            LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate8" Height="15" runat="server" />
                        <ACA:AccelaLabel ID="lblUpcomingReadyTimeComboField" LabelKey="aca_inspection_upcominglist_readytime_combofield_pattern"
                            LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" Height="25" runat="server" />
                    </div>
                </div>
                <ACA:AccelaInlineScript runat="server" ID="accelaInlineScript1">
                    <script type="text/javascript">
                        if (typeof (onUpcomingListLoaded) != "undefined") {
                            onUpcomingListLoaded();
                        }
                    </script>
                </ACA:AccelaInlineScript>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="completedPanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div id="divInspectionListCompleted" class="ACA_Page ACA_Hide">
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate5" Height="20" runat="server" />
                    <h1 class="font13px">
                        <ACA:AccelaLabel ID="lblInspectionCompleted" LabelKey="aca_inspection_completed_label"
                            runat="server"></ACA:AccelaLabel>
                    </h1>
                    <div id="divInspectionStatusRecordCount" runat="server">
                        <ACA:AccelaLabel ID="lblInspectionStatusRecordCount" runat="server"></ACA:AccelaLabel>
                    </div>
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
                                                <%# ((InspectionListItemViewModel)Container.DataItem).CombinedInfo %>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td valign="top">
                                                <div class="ACA_LinkButton">
                                                    <ACA:AccelaLabel runat="server" ID="lblOnlyOneAction" IsNeedEncode="false"></ACA:AccelaLabel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </ACA:AccelaTemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ACA_TabRow_Italic">
                                <ACA:AccelaLabel ID="lblEmptyCompleted" LabelKey="aca_inspection_completed_emptydata_label"
                                    LabelType="BodyText" runat="server"></ACA:AccelaLabel>
                            </div>
                        </EmptyDataTemplate>
                    </ACA:AccelaGridView>
                    <div id="divCompletedComboField" visible="false" runat="server">
                        <span>Click the area below to edit inspection list formats Available variables:</span><br />
                        <span>$$RequiredOrOptional$$: The inspection is required or optional</span><br />
                        <span>$$OperationDateTime$$: Operation date and time</span><br />
                        <span>$$OperationDate$$: Operation date</span><br />
                        <span>$$OperationTime$$: Operation time</span><br />
                        <span>$$ResultedDateTime$$: Inspection resulted date and time</span><br />
                        <span>$$ResultedDate$$: Inspection resulted date</span><br />
                        <span>$$ResultedTime$$: Inspection resulted time</span><br />
                        <span>$$InspectionType$$: Inspection type</span><br />
                        <span>$$InspectionSequenceNumber$$: Inspection sequence number</span><br />
                        <span>$$Inspector$$: Inspector</span><br />
                        <span>$$Operator$$: Operator</span><br />
                        <span>$$Status$$: Inspection result</span>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate7" Height="15" runat="server" />
                        <ACA:AccelaLabel ID="lblCompletedComboField" LabelKey="aca_inspection_completedlist_combofield_pattern"
                            LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate9" Height="15" runat="server" />
                        <ACA:AccelaLabel ID="lblCompletedRescheduledComboField" LabelKey="aca_inspection_completedlist_rescheduled_combofield_pattern"
                            LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate10" Height="15" runat="server" />
                        <ACA:AccelaLabel ID="lblCompletedCancelledComboField" LabelKey="aca_inspection_completedlist_cancelled_combofield_pattern"
                            LabelType="BodyText" runat="server" />
                    </div>
                </div>
                <ACA:AccelaInlineScript runat="server" ID="accelaInlineScript2">
                    <script type="text/javascript">
                        if (typeof (onCompletedListLoaded) != "undefined") {
                            onCompletedListLoaded();
                        }
                    </script>
                </ACA:AccelaInlineScript>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    $(document).ready(function () {
        if ("<%=Page.IsPostBack.ToString().ToLower() %>" == "false") {
            inspectionIsLoaded = true;
            LoadInitInspectionList();
        }
    });
</script>