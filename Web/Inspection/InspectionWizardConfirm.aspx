<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectionWizardConfirm.aspx.cs" 
    MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.Inspection.InspectionWizardConfirm" %>
<%@ Register Src="~/Component/AddressView.ascx" TagName="AddressView" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <div class="font11px">
        <asp:UpdatePanel ID="SchedulePanel" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <div id="divPageContent">
                
                <div class="ACA_TabRow ACA_Popup_Title">
                    <ACA:AccelaLabel ID="lblConfirm" LabelKey="aca_inspection_confirm_label" runat="server"></ACA:AccelaLabel>
                </div>
                <div class="ACA_Section_Instruction ACA_Page_Instruction_FontSize">
                    <ACA:AccelaLabel ID="lblConfirmContext" LabelKey="aca_inspection_confirm_context" runat="server"></ACA:AccelaLabel>
                </div>
                
                <!-- Confirm information start -->
                <table role="presentation" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="ACA_Title_Color fontbold ACA_RightPadding PopUpInspectionConfirm">
                            <ACA:AccelaLabel ID="lblInspectionType" LabelKey="aca_inspection_confirm_insp_type_label" runat="server"></ACA:AccelaLabel>
                        </td>
                        <td>
                            <ACA:AccelaLabel ID="lblInspectionTypeValue" runat="server"></ACA:AccelaLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Title_Color fontbold ACA_RightPadding">
                            <ACA:AccelaLabel ID="lblDatetime" LabelKey="aca_inspection_confirm_datetime_label" runat="server"></ACA:AccelaLabel>
                        </td>
                        <td>
                            <ACA:AccelaLabel ID="lblDatetimeValue" runat="server"></ACA:AccelaLabel>                
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="ACA_Title_Color fontbold ACA_RightPadding">                    
                            <ACA:AccelaLabel ID="lblLocation" LabelKey="aca_inspection_confirm_location_label" runat="server"></ACA:AccelaLabel>
                        </td>
                        <td>
                             <uc1:AddressView ID="addressView" runat="server" />
                        </td>
                    </tr>
                    <tr class="<% =ContanctVisible ? "" : "ACA_Hide" %>">
                        <td class="ACA_Title_Color fontbold ACA_RightPadding">
                            <ACA:AccelaLabel ID="lblContact" LabelKey="aca_inspection_confirm_contact_label" runat="server"></ACA:AccelaLabel>
                        </td>
                        <td>
                            <ACA:AccelaLabel ID="lblContactContent" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                        </td>
                    </tr>
                </table>
                <!-- Confirm information end -->
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="25" runat="server" />
                <div class="ACA_TabRow" runat="server" id="divAdditionNotes">
                    <span class="ACA_LinkButton nav_bar_separator">
                        <ACA:AccelaLinkButton ID="lnkAdditional" runat="server" OnClientClick="showOrHiddenAdditionNotes(this); return false;" LabelKey="aca_inspection_confirm_additional_link"></ACA:AccelaLinkButton>
                    </span>
                    <div id="divAdditionNotesInput" class="ACA_Hide ACA_Width45em" runat="server">
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="10" runat="server" />
                        <b>
                            <ACA:AccelaLabel ID="per_confirmAppoint_label_comment" AssociatedControlID="tbAdditionNotes" LabelKey="per_confirmAppoint_label_comment" runat="server"  LabelType="BodyText"></ACA:AccelaLabel>
                        </b>
                        <ACA:AccelaTextBox ID="tbAdditionNotes" TextMode="MultiLine" LabelKey="aca_inspection_additionnotes" Validate="MaxLength" runat="server" CssClass="ACA_Width45em" Rows="6" MaxLength="2000"></ACA:AccelaTextBox>
                        <ACA:AccelaLabel ID="per_confirmAppoint_sublabel_comment" LabelKey="per_confirmAppoint_label_comment|sub" runat="server" CssClass="ACA_Sub_Label ACA_Sub_Label_FontSize"></ACA:AccelaLabel>
                    </div>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="5" runat="server" />
                <!-- policy start -->
                <div id="divActionPolicy" runat="server">
                    <ACA:AccelaLabel ID="lblPolicyTitle" CssClass="fontbold" LabelKey="per_scheduleinspection_label_actionpolicytitle" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" Height="5" runat="server" />
                <div id="divReschedulePolicy" runat="server">
                    <ACA:AccelaLabel ID="lblReschedulePolicy" runat="server" IsNeedEncode="false"></ACA:AccelaLabel>
                </div>
                <div id="divCancellationPolicy" runat="server">
                    <ACA:AccelaLabel ID="lblCancellationPolicy" runat="server" IsNeedEncode="false"></ACA:AccelaLabel>
                </div>
                <div runat="server" id="divAdminPartOfPolicy" visible="false">
                    <ACA:AccelaLabel ID="lblRestriction1" LabelKey="per_scheduleinspection_label_restrictionpolicy1" runat="server"></ACA:AccelaLabel>
                    <br />
                    <ACA:AccelaLabel ID="lblRestriction2" LabelKey="per_scheduleinspection_label_restrictionpolicy2" runat="server"></ACA:AccelaLabel>
                    <br />
                    <ACA:AccelaLabel ID="lblRestriction3" LabelKey="per_scheduleinspection_label_restrictionpolicy3" runat="server"></ACA:AccelaLabel>
                    <br />
                    <ACA:AccelaLabel ID="lblRestriction4" LabelKey="per_scheduleinspection_label_restrictionpolicy4" runat="server"></ACA:AccelaLabel>
                    <br />
                    <ACA:AccelaLabel ID="lblRescheduleAction" LabelKey="per_scheduleinspection_label_reschedule" runat="server"></ACA:AccelaLabel>
                    <br />
                    <ACA:AccelaLabel ID="lblCancellationsAction" LabelKey="per_scheduleinspection_label_cancellations" runat="server"></ACA:AccelaLabel>
                </div>
                <!-- policy end -->
            </div>

            <ACA:AccelaHeightSeparate ID="spAboveButton" Height="40" Visible="true" runat="server" />
            
            <!-- button list -->
            <div id="divButtonList" class="ACA_TabRow">
            <table role='presentation'>
                <tr valign="bottom">
                    <td>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaButton ID="lnkFinish" CssClass="NotShowLoading" OnClientClick="inspectionFinish(); return false;" LabelKey="aca_inspection_action_finish" runat="server"/>
                        </div>
                     </td>
                     <td id="tdBackSpace" runat="server" class="PopupButtonSpace">&nbsp;</td>
                     <td id="tdBack" runat="server">
                        <div class="ACA_LinkButton">
                            <ACA:AccelaLinkButton ID="lnkBack" CausesValidation="false" OnClick="BackButton_Click" LabelKey="aca_inspection_action_back" runat="server" />
                        </div>
                     </td>
                     <td class="PopupButtonSpace">&nbsp;</td>
                     <td>
                        <div class="ACA_LinkButton">
                            <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="ACA_Inspection_Action_Cancel" runat="server" />
                        </div>
                     </td>
                </tr>
            </table>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
   
    <script type="text/javascript">
        var isRescheduleAction = "<%=IsRescheduleAction %>";
        var lnkFinish = $("#<%=lnkFinish.ClientID %>");
        var lnkBack = $("#<%=lnkBack.ClientID %>");

        $("#lnkBeginFocus", getParentDocument()).focus();

        function showOrHiddenAdditionNotes(currentObject) {
            var divAdditionNotesInput = $("#<%=divAdditionNotesInput.ClientID %>");
            var tbAdditionNotes = $("#<%=tbAdditionNotes.ClientID %>");
            var divPageContent = $('#divPageContent');

            if (divAdditionNotesInput.hasClass("ACA_Hide")) {
                divAdditionNotesInput.removeClass("ACA_Hide");
                divAdditionNotesInput.addClass("ACA_Show");

                //to avoid causing section 508 input box focus issue
                if ($.browser.msie && $.browser.version == "7.0") {
                    tbAdditionNotes.focus();
                    currentObject.focus();
                }
            } else {
                divAdditionNotesInput.removeClass("ACA_Show");                
                divAdditionNotesInput.addClass("ACA_Hide");
            }
        }

        function inspectionFinish() {
            if (typeof (Page_ClientValidate) == "function" && !Page_ClientValidate()) {
                return;
            }

            // show loading div
            var loadingTitle = '<%=GetTextByKey("aca_inspection_tips_addinginspection").Replace("'","\\'") %>';
            parent.showDialogLoading(loadingTitle);

            // execute schedule submit
            var queryString = '<%= Accela.ACA.Web.Inspection.InspectionParameterUtil.BuildQueryString(InspectionWizardParameter) %>';
            queryString = queryString == null ? "" : Sys.Serialization.JavaScriptSerializer.serialize(queryString);
            var additionNotes = GetValueById('<%=tbAdditionNotes.ClientID %>');
            additionNotes = additionNotes == null ? "" : Sys.Serialization.JavaScriptSerializer.serialize(additionNotes);

            PageMethods.InspectionSubmit(queryString, '<%=CurrentAgency %>', '<%=ModuleName %>', additionNotes, callbackInspectionSubmit);
        }

        function callbackInspectionSubmit(errormsg) {
            // if errormsg not null, show the msg;
            if (errormsg) {
                parent.LoadInitInspectionList();
                parent.hideDialogLoading();

                if (isRescheduleAction.toUpperCase() == "TRUE") {
                    lnkFinish.attr("onclick", "");
                    lnkBack.attr("onclick", "");
                    SetWizardButtonDisable('<%=lnkFinish.ClientID%>', true);
                    SetWizardButtonDisable('<%=lnkBack.ClientID%>', true);
                }

                showMessage4Popup(errormsg, "Error");
            }
            else {
                //set the flag to prevent dialog.master from executing processLoading.initControlLoading() which could cause "loading" text missed.
                if (typeof (needInitControlLoading) != "undefined") {
                    needInitControlLoading = false;
                }

                //Set focus object for Section508.
                //defined in Dialog.Master page.
                SetParentLastFocus(parent.ACADialog.focusObject);

                // refresh the inspection
                parent.ACADialog.close();
                parent.LoadInitInspectionList(true);
            }
        }
    </script>
</asp:Content>