<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefExaminationExtenalOnSiteScheduleConfirm.ascx.cs" Inherits="Accela.ACA.Web.Component.RefExaminationExtenalOnSiteScheduleConfirm" %>
<%@ Register src="ExaminationFeeItemTemplate.ascx" tagname="ExaminationFeeItemTemplate" tagprefix="uc1" %>

<div >
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblExamination" LabelKey="aca_exam_schedule_onsiteconfirm_name" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblExaminationValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div >
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblProvider" LabelKey="aca_exam_schedule_onsiteconfirm_provider" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblProviderValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div >
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblTime" LabelKey="aca_exam_schedule_onsiteconfirm_time" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblTimeValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div >
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblLanguage" LabelKey="aca_exam_schedule_onsiteconfirm_languages" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblLanguageValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div >
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblLocation" LabelKey="aca_exam_schedule_onsiteconfirm_location" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblLocationValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div >
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblAvailableSeat" LabelKey="aca_exam_schedule_onsiteconfirm_seats" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblAvailableSiteValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div >
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm">
                <ACA:AccelaLabel ID="lblAccessibility" LabelKey="aca_exam_schedule_onsiteconfirm_accessibility" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td>
                 <img runat="server" ID="imgAccessiblity" />
            </td>
            <td>
                <ACA:AccelaLabel ID="lblAccessibilityValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate12" Height="10" runat="server" />
    <uc1:ExaminationFeeItemTemplate ID="ExaminationFeeItemTemplate" 
        runat="server" />
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="10" runat="server" /> 
<div id="divInstructions">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblExamInstructions" LabelKey="aca_exam_schedule_onsiteconfirm_instructions" class="ACA_RefEducation_Font" runat="server" />
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
<div id="divAccessbilityDesc">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblAccessbilityDesc" LabelKey="aca_exam_schedule_onsiteconfirm_accessbilitydesc" class="ACA_RefEducation_Font" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblAccessbilityDescValue" runat="server" />
            </td>
        </tr>
    </table>
</div> 
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" /> 
<div id="divDriving">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblDrivingDesc" LabelKey="aca_exam_schedule_onsiteconfirm_drivingdesc" class="ACA_RefEducation_Font" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblDrivingDescValue" runat="server" />
            </td>
        </tr>
    </table>
</div> 

