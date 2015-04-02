<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Cap.CapCompletion"
    MasterPageFile="~/Default.master" Codebehind="CapCompletion.aspx.cs" %>
<%@ Register Src="~/Component/PermitReportBtn.ascx" TagName="ReportBtn" TagPrefix="uc1" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Component/CapConditions.ascx" TagName="CapConditions" TagPrefix="ACA"%>
<%@ Register src="~/Component/DependentCapTypeList.ascx" tagname="DependentCapTypeList" tagprefix="ACA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

    <script type="text/javascript">
        function print_onclick(url) {
            var a = window.open(url, "_blank", "top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
        }

        if (typeof (ExportCSV) != 'undefined') {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(EndRequest);
        }

        function EndRequest(sender, args) {
            //export file.
            ExportCSV(sender, args);
        }
        
        function ScheduleExamination(moduleName) {
            var showLoading = new ProcessLoading();
            showLoading.showLoading();
            showLoading.setTitle('<%=GetTextByKey("aca_exam_schedule_message_waiting").Replace("'","\\'")%>');
            PageMethods.ScheduleExaminations(moduleName, ShowExamScheduleMessage, ScheduleExaminationsErrorHandler);
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
                ScheduleExamination('<% = ModuleName%>');
            }
        }
    </script>

    <table role='presentation' border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width:15px;">&nbsp;</td>
            <td>
                <div class="ACA_Etisalat_CAPType">
                    <ACA:AccelaLabel ID="per_permitIssuance_label_permitIssuance" Visible="false" runat="server" LabelKey="per_permitIssuance_label_permitIssuance" />
                    <div runat="server" id="divEtisalatCAPType" visible="false"></div>
                </div>
                <span runat="server" id="spanShowToolBar">
                    <ACA:BreadCrumpToolBar ID="BreadCrumpToolBar" runat="server"></ACA:BreadCrumpToolBar>
                </span>  
            </td>
        </tr>
    </table>
    <span id="SecondAnchorInACAMainContent"></span>
    <div class="ACA_CapCompletion_Content" id="divContainer">
        <div>
            <h1 runat="server" ID="h1Receipt">
                <ACA:AccelaLabel ID="lblReceipt" LabelType="PageTitle" runat="server" TargetCtrlId="divContainer"></ACA:AccelaLabel>
            </h1>
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate6" runat="server" Height="5" />
        </div>
         
        <!-- begin Etisalat error message-->
        <div class="ACA_Row">
            <uc1:MessageBar ID = "etisalatErrorMessage" runat="Server" />
        </div>
        <!-- end Etisalat error message-->

        <!-- begin custom content -->
        <div id="divContent" runat="server">
            <div class="ACA_Row">
                <uc1:MessageBar ID = "permitSuccessInfo" runat="Server" />
            </div>
            <div class="ACA_Row">
                <span id="examMessage"></span>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate8" runat="server" Height="10"/>
            </div>
            <div class="ACA_Row MessageBar2Receipt">
                <span runat="server" id="authMessage"></span>
            </div>
            <div class="ACA_Row">
                <uc1:MessageBar ID = "registrationSuccessInfo" runat="Server" />
            </div>
            <div id="divRenewalLicenseTopMessage" runat="server" visible="false">
                <div class="ACA_Row">
                    <uc1:MessageBar ID = "renewalLicenseAutoIssueSuccessMessage" runat="Server" />
                </div>
                <div class="ACA_Row">
                    <uc1:MessageBar ID = "renewalLicenseNoAutoIssueSuccessMessage" runat="Server" />
                </div>
                <div class="ACA_Row">
                    <uc1:MessageBar ID = "renewalLicenseDeferredPaymentSuccessMessage" runat="Server" />
                </div>
            </div>
            <p>
                <ACA:AccelaLabel ID="per_permitIssuance_text_welcomeInfo" runat="server" LabelKey="per_permitIssuance_text_welcomeInfo" />
                <ACA:AccelaLabel ID="lblWelcomeInfo4PayFeeDue" runat="server" LabelKey="per_permitissuance_payfeedue_text_welcomeinfo" Visible="false" />
            </p>
            <h2>
                <ACA:AccelaLabel ID="per_permitIssuance_label_permitNum" runat="server" LabelKey="per_permitIssuance_label_permitNum" />
                <ACA:AccelaLabel ID="lblPermitNumText4PayFeeDue" runat="server" LabelKey="per_permitissuance_payfeedue_label_permitnum" Visible="false" />
                <ACA:AccelaLabel ID="per_renewallicenseissuance_label_licensenumber" runat="server" LabelKey="lic_renewallicenseissuance_label_licensenumber" Visible="false"/>            
                <ACA:AccelaLabel ID="lblCapID" dir="ltr" runat="server" />.</h2>
            <ACA:DependentCapTypeList ID="dependentCapTypeList" runat="server" />
            <div id="divDependentCapTypeSetting" Visible="false" runat="server">
                <div class="Header_h4"><ACA:AccelaLabel ID="lblDependentCapType" LabelKey="aca_label_receiptpage_dependentcaptype"  runat="server"/></div>
            </div>
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10"/>
            <!-- begin show conditions //-->
            <ACA:CapConditions ID="capConditions" HideLink4ViewMet="true" IsGroupByRecordType="false"
                GeneralConditionsTitleLabelKey="aca_capcompletion_label_generalcondition_sectionheader" 
                ConditionsOfApprovalTitleLabelKey="aca_capcompletion_label_conditionofapproval_sectionheader" 
                GeneralConditionsPatternLabelKey="aca_capcompletion_generalcondition_pattern" 
                ConditionsOfApprovalPatternLabelKey="aca_capcompletion_conditionofapproval_pattern" runat="server" />
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate7" runat="server" Height="10"/>
            <!-- end show conditions //-->
            <div id="divNormalPermitInstruction" runat="server">
                <div class="ACA_Page ACA_Page_FontSize">
                    <ACA:AccelaLabel ID="per_permitIssuance_text_permitNum" runat="server" LabelKey="per_permitIssuance_text_permitNum" LabelType="BodyText" />
                    <ACA:AccelaLabel ID="lblPermitNum4PayFeeDue" runat="server" LabelKey="per_permitissuance_payfeedue_text_permitnum" LabelType="BodyText" Visible="false" />
                </div>
            </div>
            
            <div id="divRenewalLicenseInstructions" runat="server" visible="false">
                <div id="divRenewalAutoIssuanceInstruction" runat="server" visible="false">
                    <div class="ACA_Page ACA_Page_FontSize">
                        <br /><ACA:AccelaLabel ID="per_licenseAutoIssuance_text_instruction" runat="server" LabelKey="per_renewalLicensePermit_text_autoIssuanceInfo" LabelType="BodyText"/>
                    </div>
                </div>
                <div id="divRenewalNoAutoIssuanceInstruction" runat="server" visible="false">
                    <div class="ACA_Page ACA_Page_FontSize">
                        <br /><ACA:AccelaLabel ID="per_licenseNoAutoIssuance_text_instruction" runat="server" LabelKey="per_renewalLicensePermit_text_noAutoIssuanceInfo" LabelType="BodyText"/>
                    </div>
                </div>
                <div id="divRenewalDeferredPaymentInstruction" runat="server" visible="false">  
                    <div class="ACA_Page ACA_Page_FontSize">
                        <br /><ACA:AccelaLabel ID="per_licenseDeferredPayment_text_instruction" runat="server" LabelKey="per_renewallicenseissuance_text_deferredpaymentbodyinfo" LabelType="BodyText"/>
                    </div>
                </div>
            </div>
        
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="10" />
            <!-- begin Etisalat normal message-->
            <div runat="server" id="spanEtisalatSuccessMessage" visible="false">
                <div class="ACA_Page ACA_Page_FontSize">
                    <ACA:AccelaLabel ID="lblEtisalatReturnMessage" runat="server" IsNeedEncode="false" LabelType="BodyText" />
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" runat="server" Height="10" />
            </div>
            <!-- end Etisalat normal message-->
            <!-- end custom content -->
        
            <div class="ACA_TabRow ACA_LiLeft">
                <uc1:ReportBtn ID="ReportBtn" runat="server"></uc1:ReportBtn>
                <span class="ACA_Sub_Label ACA_Sub_Label_FontSize">
                    <ACA:AccelaLabel ID="per_permitIssuance_sublabel_printPermit" Visible="false" runat="server"
                        LabelKey="per_permitIssuance_label_printPermit|sub" LabelType="BodyText" /></span>
            </div>        
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" runat="server" Height="5" />
            <div runat="server" id="divRowLine" class="ACA_TabRow ACA_Line_Content">
            </div>
            <!-- begin custom content -->
            <div class="ACA_TabRow">
                <ACA:AccelaLabel LabelType="BodyText" ID="per_permitIssuance_text_license" runat="server" LabelKey="per_permitIssuance_text_license" />
                <ACA:AccelaLabel LabelType="BodyText" ID="lblLicense4PayFeeDue" runat="server" LabelKey="per_permitissuance_payfeedue_text_license" Visible="false" />
            </div>
            <!-- end custom content -->
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate5" runat="server" Height="15" />
            <table role='presentation'>
                <tr>
                    <td>
                        <ACA:AccelaButton ID="lnkViewPermitDetail" LabelKey="per_permitIssuance_label_viewPermit"
                            runat="server" OnClick="ViewPermitDetailButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" />
                    </td>
                    <td>
                        <ACA:AccelaLabel ID="per_permitIssuance_sublabel_viewPermit" CssClass="ACA_Sub_Label ACA_Sub_Label_FontSize" runat="server" LabelKey="per_permitIssuance_label_viewPermit|sub" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- /form -->
    <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</asp:Content>
