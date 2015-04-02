<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CapCompletions.aspx.cs" Inherits="Accela.ACA.Web.Cap.CapCompletions"
    MasterPageFile="~/Default.master" ValidateRequest="false" %>

<%@ Register Src="~/Component/PermitReportBtn.ascx" TagName="ReportBtn" TagPrefix="uc1" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Component/CapConditions.ascx" TagName="CapConditions" TagPrefix="ACA" %>
<%@ Register Src="~/Component/DependentCapTypeList.ascx" TagName="DependentCapTypeList" TagPrefix="ACA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <script type="text/javascript">
        function print_onclick(url)
        {
            var a=window.open(url, "_blank","top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
        }

        function ShowCloneDialog(url, objectTargetID) {
            window.ACADialog.popup({ url: url, width: 745, height: 380, objectTarget: objectTargetID });
        }
        if (typeof (ExportCSV) != 'undefined') {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(EndRequest);
        }

        function EndRequest(sender, args) {
            //export file.
            ExportCSV(sender, args);
        }
	
        function ScheduleExamination() {
            var showLoading = new ProcessLoading();
            showLoading.showLoading();
            showLoading.setTitle('<%=GetTextByKey("aca_exam_schedule_message_waiting").Replace("'","\\'")%>');
            PageMethods.ScheduleExaminations(ShowExamScheduleMessage, ScheduleExaminationsErrorHandler);
        }
        
        function ShowExamScheduleMessage(info) {
            var p = new ProcessLoading();
            p.hideLoading();
            
            if (!info) {
                return;
            }

            var showErrorMessage = '';
            var scheduleFailed = '<%=GetTextByKey("aca_exam_schedule_message_failed").Replace("'","\\'")%>';
            var scheduleSuccessful = '<%=GetTextByKey("aca_exam_schedule_message_successful").Replace("'","\\'")%>';
            var json = JSON.parse(info);

            for (var i = 0; i < json.length; i++) {
                for (var j = 0; j < json[i].Examination.length; j++) {
                    showErrorMessage += json[i].CapID + ': ' + json[i].Examination[j].ExaminationName;
                    if (json[i].Examination[j].Success) {
                        showErrorMessage += ' ' + scheduleSuccessful;
                    } else {
                        showErrorMessage += ' ' + scheduleFailed + json[i].Examination[j].Message;
                    }

                    showErrorMessage += "<br />";
                }
            }

            if (showErrorMessage) {
                showErrorMessage = '<%=GetTextByKey("aca_exam_schedule_message_notifaction").Replace("'","\\'")%>' + showErrorMessage;
                showMessage('examMessage', showErrorMessage, 'Notice', true, 1, true);
            }
        }
        
        function ScheduleExaminationsErrorHandler(error) {
            var p = new ProcessLoading();
            p.hideLoading();

            showNormalMessage(error.get_message(), 'Error');
        }
        
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(<%=ClientID %>_PageLoaded);
        
        function <%=ClientID %>_PageLoaded(sender, args) {
            if (!$.global.isAdmin && <%=HasReady2ScheduleExamination.ToString().ToLower() %>) {
                ScheduleExamination();
            }
        }
    </script>
    <table role='presentation' border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 15px;">&nbsp;</td>
            <td>
                <ACA:BreadCrumpToolBar ID="BreadCrumbBar" runat="server"></ACA:BreadCrumpToolBar>
                <ACA:BreadCrumpToolBar IsForShoppingCart="true" ID="BreadCrumpShoppingCart" runat="server"></ACA:BreadCrumpToolBar>
            </td>
        </tr>
    </table>
    <span id="SecondAnchorInACAMainContent"></span>
    <div id="divContent" class="ACA_ContainerLong_ShoppingCart" runat="server">
        <ACA:AccelaLabel ID="lblPageInstruction" LabelType="PageInstruction" LabelKey="aca_multiplecaps_label_pageinstruction" runat="server" />
        <ACA:AccelaHeightSeparate ID="sepForHeader" runat="server" Height="10" Visible="false" />
        <h1>
            <ACA:AccelaLabel ID="lblReceiptTitle" LabelKey="aca_multiplecaps_label_receipttitle" runat="server" />
        </h1>
        <div id="divHeight" class="ACA_Title_Divide"></div>
        <div class="ACA_Row">
            <uc1:MessageBar ID="resultMessage" runat="Server" />
        </div>
        <div class="ACA_Row">
            <span id="examMessage"></span>
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="10"/>
        </div>
        <div class="ACA_Row MessageBar2Receipt">
            <span runat="server" id="authMessage"></span>
        </div>
        <div class="ACA_TabRow ACA_LiLeft">
            <uc1:ReportBtn ID="ReportBtnTop" IsInShoppingCart="True" runat="server"></uc1:ReportBtn>
        </div>
        <div style="height: 25px"></div>
        <asp:DataList ID="addressList" Width="100%" OnItemDataBound="AddressList_ItemDataBound" runat="server" role='presentation'>
            <ItemTemplate>

                <div id="divAddress" class="ACA_Title_Bar">
                    <h1><ACA:AccelaLabel ID="lblAddress" runat="server"></ACA:AccelaLabel></h1>
                </div>

                <asp:DataList ID="agenciesList" Width="100%" runat="server" OnItemDataBound="AgenciesList_ItemDataBound" role='presentation'>
                    <ItemTemplate>
                        <table role='presentation' id="tbAgency" runat="server">
                            <tr>
                                <td>
                                    <table role='presentation'>
                                        <tr>
                                            <td id="tdLogo" runat="server">
                                                <img id="imgAgencyLogo" runat="server" height="32" />&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <h1><ACA:AccelaLabel ID="lblAgency" Text='<%#DataBinder.Eval(Container.DataItem, "capID.serviceProviderCode")%>' runat="server"></ACA:AccelaLabel></h1>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:DataList ID="capsList" Width="100%" runat="server" OnItemDataBound="CapsList_ItemDataBound" role='presentation'>
                                        <ItemTemplate>
                                            <table role='presentation' style="width: 100%;">
                                                <tr>
                                                    <td id="prefix" style="width: 2%;"></td>
                                                    <td class="ACA_Cap_Completions_TD">
                                                        <div class="Header_h4">
                                                            <ACA:AccelaLinkButton ID="hlCAPDetail" Text='<%# DataBinder.Eval(Container.DataItem,"capID.customID") %>' runat="server" ReportID="0"></ACA:AccelaLinkButton>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="Header_h4">
                                                            <asp:Label runat="server" ID="lblCapType"></asp:Label>&nbsp;
                                                        </div>
                                                    </td>
                                                    <td class="ACA_Cap_Completions_TD">
                                                        <div class="fontbold font11px">
                                                            <ACA:AccelaLinkButton ID="hlCAPPermit" CssClass="NotShowLoading" Visible="false" LabelKey="per_permitissuances_label_viewrecord" runat="server"  ReportID="0"/>
                                                        </div>
                                                    </td>
                                                    <td class="ACA_Cap_Completions_TD">
                                                        <div class="fontbold font11px">
                                                            <ACA:AccelaLinkButton ID="hlCAPReceipt" CssClass="NotShowLoading" Visible="false" LabelKey="per_permitIssuances_label_viewReceipt" runat="server" ReportID="0" />
                                                        </div>
                                                    </td>
                                                    <td class="ACA_Cap_Completions_TD">
                                                        <div class="fontbold font11px">
                                                            <ACA:AccelaLinkButton ID="hlCAPSummary" CssClass="NotShowLoading" Visible="false" LabelKey="per_permitissuances_label_viewsummary" runat="server" ReportID="0" />
                                                        </div>
                                                    </td>
                                                    <td class="ACA_Cap_Completions_TD">
                                                        <div class="fontbold font11px">
                                                            <ACA:AccelaLinkButton ID="lnkLabelPrint" CssClass="NotShowLoading" Visible="false" LabelKey="aca_authagent_receipt_label_printlabel" runat="server" />
                                                        </div>
                                                    </td>
                                                    <td class="ACA_Cap_Completions_TD">
                                                        <div class="fontbold font11px">
                                                            <ACA:AccelaLinkButton ID="hlCloneRecord" CssClass="NotShowLoading" Visible="false" LabelKey="aca_capcompletions_label_clone_record" runat="server" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="8">
                                                        <ACA:DependentCapTypeList ID="dependentCapTypeList" IsMutipleRecord="true" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="8">
                                                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10" />
                                                        <ACA:CapConditions ID="capConditions" HideConditionBar="false" HideLink4ViewMet="true" IsGroupByRecordType="false"
                                                            GeneralConditionsTitleLabelKey="aca_capcompletions_label_generalcondition_sectionheader"
                                                            ConditionsOfApprovalTitleLabelKey="aca_capcompletions_label_conditionofapproval_sectionheader"
                                                            GeneralConditionsPatternLabelKey="aca_capcompletions_generalcondition_pattern"
                                                            ConditionsOfApprovalPatternLabelKey="aca_capcompletions_conditionofapproval_pattern" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </ItemTemplate>
        </asp:DataList>
        <div id="divDependentCapTypeSetting" visible="false" runat="server">
            <div class="Header_h4"><ACA:AccelaLabel LabelKey="aca_label_receiptpage_dependentcaptype"  runat="server"/></div>
        </div>
        <div id="divConditionsSetting" runat="server" visible="false">
            <ACA:CapConditions ID="capConditions" HideConditionBar="false" HideLink4ViewMet="true" IsGroupByRecordType="false"
                GeneralConditionsTitleLabelKey="aca_capcompletions_label_generalcondition_sectionheader"
                ConditionsOfApprovalTitleLabelKey="aca_capcompletions_label_conditionofapproval_sectionheader"
                GeneralConditionsPatternLabelKey="aca_capcompletions_generalcondition_pattern"
                ConditionsOfApprovalPatternLabelKey="aca_capcompletions_conditionofapproval_pattern" runat="server" />
        </div>
        <ACA:AccelaHeightSeparate ID="sepLineForWorkLocation" runat="server" Height="10" />
        <br />
        <div class="ACA_TabRow ACA_LiLeft">
            <uc1:ReportBtn ID="ReportBtn" IsInShoppingCart="True" runat="server"></uc1:ReportBtn>
        </div>
    </div>
    <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</asp:Content>
