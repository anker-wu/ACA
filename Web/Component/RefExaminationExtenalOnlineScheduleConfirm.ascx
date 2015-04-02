<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefExaminationExtenalOnlineScheduleConfirm.ascx.cs" Inherits="Accela.ACA.Web.Component.RefExaminationExtenalOnlineScheduleConfirm" %>
<%@ Register src="ExaminationFeeItemTemplate.ascx" tagname="ExaminationFeeItemTemplate" tagprefix="uc1" %>

<div>
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblExamination" LabelKey="aca_exam_schedule_onlineconfirm_name" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblExaminationValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div>
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblProvider" LabelKey="aca_exam_schedule_onlineconfirm_provider" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblProviderValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div>
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblLanguage" LabelKey="aca_exam_schedule_onlineconfirm_languages" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblLanguageValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" Height="10" runat="server" />
    <uc1:ExaminationFeeItemTemplate ID="ExaminationFeeItemTemplate" 
        runat="server" />
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />
<div id="divInstructions" runat="server">
    <table class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblExamInstructions" LabelKey="aca_exam_schedule_onlineconfirm_instructions" class="ACA_RefEducation_Font" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblExamInstructionsValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="10" runat="server" />
<div id="divUserAccount" runat="server">
    <table class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblUserAccount" LabelKey="aca_exam_schedule_onlineconfirm_account" class="ACA_RefEducation_Font" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblUserAccountValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
