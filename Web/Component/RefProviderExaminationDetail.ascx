<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefProviderExaminationDetail.ascx.cs"
    Inherits="Accela.ACA.Web.Component.RefProviderExaminationDetail" %>
<%@ Register Src="ExaminationFeeItemTemplate.ascx" TagName="ExaminationFeeItemTemplate"
    TagPrefix="uc1" %>
 
<div>
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblExamination" LabelKey="aca_provider_exam_detail_name" class="ACA_RefEducation_Font"
                    runat="server" />
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
                <ACA:AccelaLabel ID="lblProvider" LabelKey="aca_provider_exam_detail_provider" class="ACA_RefEducation_Font"
                    runat="server" />
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
                <ACA:AccelaLabel ID="lblTime" LabelKey="aca_provider_exam_detail_time" class="ACA_RefEducation_Font"
                    runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblTimeValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div>
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblLanguage" LabelKey="aca_provider_exam_detail_languages" class="ACA_RefEducation_Font"
                    runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblLanguageValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div>
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblLocation" LabelKey="aca_provider_exam_detail_location" class="ACA_RefEducation_Font"
                    runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblLocationValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div>
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblAvailableSite" LabelKey="aca_provider_exam_detail_seats"
                    class="ACA_RefEducation_Font" runat="server" />
            </td>
            <td class="ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblAvailableSiteValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div>
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblAccessibility" LabelKey="aca_provider_exam_detail_accessibility"
                    class="ACA_RefEducation_Font" runat="server" />
            </td>
             <td>
                 <img runat="server" ID="imgAccessiblity" />
            </td>
            <td class="ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblAccessibilityValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate12" Height="10" runat="server" />
<div id="divFeeDesc" runat="server">
    <uc1:ExaminationFeeItemTemplate ID="ExaminationFeeItemTemplate" runat="server" />
</div>
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />
<div>
    <table class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblExamInstructions" LabelKey="aca_provider_exam_detail_instructions"
                    class="ACA_RefEducation_Font" runat="server" />
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
<div>
    <table class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblAccessbilityDesc" LabelKey="aca_provider_exam_detail_accessbilitydesc"
                    class="ACA_RefEducation_Font" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblAccessbilityDescValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="10" runat="server" /> 
<div>
    <table class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblDrivingDesc" LabelKey="aca_provider_exam_detail_drivingdesc"
                    class="ACA_RefEducation_Font" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ACA_Table_Align_Top">
                <ACA:AccelaLabel ID="lblDrivingDescValue" runat="server" />
            </td>
        </tr>
    </table>
</div>
