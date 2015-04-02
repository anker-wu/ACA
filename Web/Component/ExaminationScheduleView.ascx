<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExaminationScheduleView.ascx.cs" Inherits="Accela.ACA.Web.Component.ExaminationScheduleView" %>
<%@ Register TagPrefix="ACA" TagName="GenericTemplate" Src="~/Component/GenericTemplateView.ascx" %>

<div class="examdetailpage">
    <div id="divType">
        <table role='presentation' cellpadding="0" cellspacing="0">
            <tr>
                <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblExamination" LabelKey="aca_examination_detail_examname" class="ACA_RefEducation_Font" runat="server" />
                </td>
                <td>&nbsp;</td>
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
                    <ACA:AccelaLabel ID="lblScore" LabelKey="aca_examination_detail_score" class="ACA_RefEducation_Font"
                        runat="server" />
                </td>
                <td>&nbsp;</td>
                <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblScoreValue" runat="server" />
                </td>
                <td class="ACA_Table_Align_Top">
                    <img runat="server" ID="imgPassingIcon" visible="false" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table role='presentation' cellpadding="0" cellspacing="0">
            <tr>
                <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblPassingScore" LabelKey="aca_examination_detail_passingscore" class="ACA_RefEducation_Font"
                        runat="server" />
                </td>
                <td>&nbsp;</td>
                <td class="ACA_Table_Align_Top">
                    <ACA:AccelaLabel ID="lblPassingScoreValue" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div id="div2">
        <table role='presentation'>
            <tr>
                <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                    <ACA:AccelaLabel ID="lblTime" LabelKey="aca_examination_detail_dateandtime" class="ACA_RefEducation_Font" runat="server" />
                </td>
                <td>&nbsp;</td>
                <td class="ACA_Table_Align_Top">
                        <ACA:AccelaLabel ID="lblTimeValue" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div id="div1">
        <table role='presentation'>
            <tr>
                <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                    <ACA:AccelaLabel ID="lblRosterId" LabelKey="aca_examination_detail_rosterid" class="ACA_RefEducation_Font" runat="server" />
                </td>
                <td>&nbsp;</td>
                <td class="ACA_Table_Align_Top">
                        <ACA:AccelaLabel ID="lblRosterIdValue" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divContactType">
        <table role='presentation' cellpadding="0" cellspacing="0">
            <tr>
                <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                    <ACA:AccelaLabel ID="lblProvider" LabelKey="aca_examination_detail_provider" class="ACA_RefEducation_Font" runat="server" />
                </td>
                <td>&nbsp;</td>
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
                    <ACA:AccelaLabel ID="lblLanguage" LabelKey="aca_examination_detail_supportedlanguage" class="ACA_RefEducation_Font" runat="server" />
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
                    <ACA:AccelaLabel ID="lblLocation" LabelKey="aca_examination_detail_location" class="ACA_RefEducation_Font" runat="server" />
                </td>
                <td>&nbsp;</td>
                <td class="ACA_Table_Align_Top">
                        <ACA:AccelaLabel ID="lblLocationValue" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div id="div6">
        <table role='presentation' cellpadding="0" cellspacing="0">
            <tr>
                <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm"> 
                    <ACA:AccelaLabel ID="lblAccessibility" LabelKey="aca_examination_detail_accessiblility" class="ACA_RefEducation_Font" runat="server" />
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

    <div id="divInstructions">
        <table class="ACA_FullWidthTable" role='presentation' cellpadding="0" cellspacing="0">
            <tr>
                <td class="ACA_Title_Color fontbold ACA_RightPadding PopupExaminationConfirm ACA_Table_Align_Top"> 
                    <ACA:AccelaLabel ID="lblExamInstructions" LabelKey="aca_examination_detail_instructions" class="ACA_RefEducation_Font" runat="server" />
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
                    <ACA:AccelaLabel ID="lblAccessbilityDesc" LabelKey="aca_examination_detail_accesssibilitydesc" class="ACA_RefEducation_Font" runat="server" />
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
                    <ACA:AccelaLabel ID="lblDrivingDesc" LabelKey="aca_examination_detail_drivingdirection" class="ACA_RefEducation_Font" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="ACA_Table_Align_Top">
                        <ACA:AccelaLabel ID="lblDrivingDescValue" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="10" runat="server" />
    <ACA:GenericTemplate ID="genericTemplate" runat="server" />
</div>