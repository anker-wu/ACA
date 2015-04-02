<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefExaminationScheduleConfirm.ascx.cs" Inherits="Accela.ACA.Web.Component.RefExaminationScheduleConfirm" %>

<%@ Register src="ExaminationFeeItemTemplate.ascx" tagname="ExaminationFeeItemTemplate" tagprefix="uc1" %>

<div id="divType">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblExamination" LabelKey="aca_exam_schedule_internalconfirm_name" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td>&nbsp;</td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblExaminationValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div id="divContactType">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblProvider" LabelKey="aca_exam_schedule_internalconfirm_provider" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td>&nbsp;</td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblProviderValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div id="div2">
    <table role='presentation'>
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblTime" LabelKey="aca_exam_schedule_internalconfirm_time" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td>&nbsp;</td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblTimeValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div id="div3" runat="server">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblLanguage" LabelKey="aca_exam_schedule_internalconfirm_languages" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td>&nbsp;</td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblLanguageValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div id="div4">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblLocation" LabelKey="aca_exam_schedule_internalconfirm_location" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td>&nbsp;</td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblLocationValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div id="div5">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblAvailableSite" LabelKey="aca_exam_schedule_internalconfirm_seats" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td>&nbsp;</td>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblAvailableSiteValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div id="div6">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm"> 
                <ACA:AccelaLabel ID="lblAccessibility" LabelKey="aca_exam_schedule_internalconfirm_accessibility" class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td>&nbsp;</td>
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
    <table class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblExamInstructions" LabelKey="aca_exam_schedule_internalconfirm_instructions" class="ACA_RefEducation_Font" runat="server" />
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
    <table class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblAccessbilityDesc" LabelKey="aca_exam_schedule_internalconfirm_accessbilitydesc" class="ACA_RefEducation_Font" runat="server" />
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
    <table  class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                <ACA:AccelaLabel ID="lblDrivingDesc" LabelKey="aca_exam_schedule_internalconfirm_drivingdesc" class="ACA_RefEducation_Font" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblDrivingDescValue" runat="server" />
            </td>
        </tr>
    </table>
</div> 