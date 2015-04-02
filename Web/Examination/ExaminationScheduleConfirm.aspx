<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExaminationScheduleConfirm.aspx.cs" 
MasterPageFile="~/Dialog.master" Inherits="Accela.ACA.Web.Examination.ExaminationScheduleConfirm" %>

<%@ Register src="../Component/RefExaminationScheduleConfirm.ascx" tagname="RefExaminationScheduleConfirm" tagprefix="uc1" %>

<%@ Register src="../Component/RefExaminationExtenalOnSiteScheduleConfirm.ascx" tagname="RefExaminationExtenalOnSiteScheduleConfirm" tagprefix="uc2" %>
<%@ Register src="../Component/RefExaminationExtenalOnlineScheduleConfirm.ascx" tagname="RefExaminationExtenalOnlineScheduleConfirm" tagprefix="uc3" %>
<%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagPrefix="ACA" TagName="GenericTemplateEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div class="font11px examination_schedule_confirm">
        <asp:UpdatePanel ID="AvailableExaminationPanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
            <div class="ACA_TabRow">
                    <uc3:RefExaminationExtenalOnlineScheduleConfirm Visible="false" ID="RefExaminationExtenalOnlineScheduleConfirm"
                    runat="server" />
                <uc2:RefExaminationExtenalOnSiteScheduleConfirm Visible="false" ID="RefExaminationExtenalOnSiteScheduleConfirm" 
                    runat="server" />
                <uc1:RefExaminationScheduleConfirm ID="RefExaminationScheduleConfirm" Visible="false" runat="server" />
            </div>
            <div class="ACA_TabRow">
                <ACA:GenericTemplateEdit runat="server" ID="genericTemplate" />
            </div>
            <div class="ACA_TabRow" id="divExaminationExternalOnline" visible="false" runat="server">
                <table id="tableOnsite" role='presentation'>
                    <tr>
                        <td class="ACA_Table_Align_Top"> 
                            <ACA:AccelaRadioButton ID="rdExsitingAccount"  runat="server" value="ExsitingAccount" />
                        </td>
                        <td>&nbsp;</td>
                        <td class="ACA_Table_Align_Top">
                            <ACA:AccelaEmailText ID="txtExsitingAccount" Validate="required" runat="server" LabelKey="aca_exam_schedule_onlineconfirm_existingaccount">
                            </ACA:AccelaEmailText>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="ACA_Table_Align_Top">
                            <ACA:AccelaLabel ID="lblExistingAccountNotice" LabelKey="aca_exam_schedule_onlineconfirm_existingaccount_notice" runat="server"></ACA:AccelaLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top"> 
                            <ACA:AccelaRadioButton ID="rdCreateAccount" runat="server" value="CreateAccount" />
                        </td>
                        <td>&nbsp;</td>
                        <td class="ACA_Table_Align_Top">
                            <ACA:AccelaEmailText ID="txtCreateAccount" Validate="required" runat="server" LabelKey="aca_exam_schedule_onlineconfirm_createaccount">
                            </ACA:AccelaEmailText>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <ACA:AccelaLabel ID="lblNewAccountNotice" LabelKey="aca_exam_schedule_onlineconfirm_createaccount_notice" runat="server"></ACA:AccelaLabel>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="ACA_TabRow" id="divExaminationExternalOnSite" visible="false" runat="server">
                <table role='presentation'>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <ACA:AccelaEmailText ID="txtEMailAddress" runat="server" LabelKey="aca_exam_schedule_onsiteconfirm_email">
                            </ACA:AccelaEmailText>
                        </td>
                    </tr>
                </table>
            </div>
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />
            <!-- button list -->
            <div class="buttons ACA_Row ACA_LiLeft">
                <ul>
                    <li>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaButton ID="lnkPayNow" OnClick="PayNowButton_Click" LabelKey="aca_exam_schedule_label_paynow" runat="server" />
                        </div>
                    </li>
                    <li>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaButton ID="lnkConfirm" OnClick="PayNowButton_Click" LabelKey="aca_exam_schedule_action_paynow" runat="server" />
                        </div>
                    </li>
                    <li>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaButton ID="lnkContinue" OnClick="ContinueButton_Click" LabelKey="aca_exam_schedule_label_defer_payment" runat="server" />
                        </div>
                    </li>
                    <li>
                        <div class="ACA_LinkButton">
                            <ACA:AccelaLinkButton ID="lnkBack" CausesValidation="false" OnClick="BackButton_Click" LabelKey="aca_exam_schedule_action_back" runat="server" />
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
    $("#lnkBeginFocus", getParentDocument()).focus();

    function disablebutton() {
        var textExsitingAccount = $("#<%=txtExsitingAccount.ClientID %>");
        var textCreateAccount = $("#<%=txtCreateAccount.ClientID %>");
        if ($("#<%=rdExsitingAccount.ClientID %>").attr("checked")) {
            textCreateAccount.attr("disabled", true);
            textCreateAccount.val('');
        } else if ($("#<%=rdCreateAccount.ClientID %>").attr("checked")) {
            textExsitingAccount.attr("disabled", true);
            textExsitingAccount.val('');
        } else {
            textCreateAccount.attr("disabled", true);
            textExsitingAccount.attr("disabled", true);
        }
    }

    function RedirectToPayFeePage() {
        parent.RedirctToFeePage();
    }
    
    function BackSuccessful() {
        // show loading div
        var loadingTitle = '<%=GetTextByKey("aca_global_msg_loading").Replace("'","\\'") %>';
        parent.showDialogLoading(loadingTitle);

        if (typeof(needInitControlLoading) != "undefined") {
            needInitControlLoading = false;
        }
        parent.LoadInitExaminationList();

        return false;
    }
    </script>
</asp:Content>
