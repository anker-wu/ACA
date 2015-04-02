<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectionCancel.aspx.cs" 
    MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.Inspection.InspectionCancel" %>
<%@ Register Src="~/Component/AddressView.ascx" TagName="AddressView" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div class="ACA_Page font11px">
        <asp:UpdatePanel ID="SchedulePanel" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <div>
                <div class="ACA_TabRow ACA_Popup_Title">
                    <ACA:AccelaLabel ID="lblConfirm" LabelKey="aca_inspection_confirm_label" runat="server"></ACA:AccelaLabel>        
                </div>
                <div class="ACA_Section_Instruction ACA_Page_Instruction_FontSize">
                    <ACA:AccelaLabel ID="lblConfirmContext" LabelKey="aca_inspection_confirm_context" runat="server"></ACA:AccelaLabel>
                </div>
               
                <!-- Confirm information start -->
                <table cellpadding="0" cellspacing="0" role="presentation">
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
                    <tr class="<% =ContactVisible ? "" : "ACA_Hide" %>">
                        <td class="ACA_Title_Color fontbold ACA_RightPadding">
                            <ACA:AccelaLabel ID="lblContact" LabelKey="aca_inspection_confirm_contact_label" runat="server"></ACA:AccelaLabel>
                        </td>
                        <td>
                            <ACA:AccelaLabel ID="lblContactContent" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                        </td>
                    </tr>
                </table>
                <!-- Confirm information end -->       
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="30" runat="server" />            
                <!-- policy start -->
                <div id="divActionPolicy" runat="server">
                        <ACA:AccelaLabel ID="lblPolicyTitle" CssClass="fontbold" LabelKey="per_scheduleinspection_label_actionpolicytitle" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="5" runat="server" />
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
            <div class="ACA_TabRow">
            <table role='presentation'>
                <tr valign="bottom">
                    <td>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaButton ID="lnkCancelInspection" OnClientClick="InspectionCancel();return false;" LabelKey="aca_inspection_action_cancelinspection" runat="server"/>
                        </div>
                     </td>
                     <td class="PopupButtonSpace">&nbsp;</td>
                     <td>
                        <div class="ACA_LinkButton">
                            <ACA:AccelaLinkButton ID="lnkClose" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="aca_common_close" runat="server" />
                        </div>
                     </td>
                </tr>
            </table>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
        
    <script type="text/javascript">
        var lnkCancelInspection = $("#<%=lnkCancelInspection.ClientID %>");

        function InspectionCancel() {
            // show loading div
            var loadingTitle = '<%=GetTextByKey("aca_inspection_tips_cancellinginspection").Replace("'","\\'") %>';
            parent.showDialogLoading(loadingTitle);
            
            // execute schedule submit
            var queryString = '<%= Accela.ACA.Web.Inspection.InspectionParameterUtil.BuildQueryString(InspectionWizardParameter) %>';

            PageMethods.InspectionCancelSubmit(queryString, '<%=CurrentAgency %>', callbackInspectionCancel);
        }

        function callbackInspectionCancel(errormsg) {
            // if errormsg not null, show the msg;
            if (errormsg) {
                parent.LoadInitInspectionList();
                parent.hideDialogLoading();
                SetWizardButtonDisable('<%=lnkCancelInspection.ClientID%>', true);
                lnkCancelInspection.attr("onclick", "");
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