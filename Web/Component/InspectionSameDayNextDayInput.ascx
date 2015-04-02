<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionSameDayNextDayInput.ascx.cs"
    Inherits="Accela.ACA.Web.Component.InspectionSameDayNextDayInput" %>
<div runat="server" id="divSameDayNextDay" class="ACA_TabRow">
    <div class="ACA_TabRow">
        <p>
            <b>
                <ACA:AccelaLabel ID="lblSameDayNextDay" LabelKey="per_scheduleinspection_label_samedaynextdayselection"
                    runat="server"></ACA:AccelaLabel>
            </b>
        </p>
    </div>
    <div class="ACA_TabRow">
        <table role='presentation' id="tbSameDayNextDay" class="ACA_TDAlignLeftOrRightTop"
            border="0">
            <tr>
                <td>
                    <p>
                        <ACA:AccelaRadioButton runat="server" ID="rblSameDay" LabelKey="per_scheduleinspection_option_sameday"
                            GroupName="grpSameDayNextDay" />
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <ACA:AccelaRadioButton runat="server" ID="rblNextBusinessDay" LabelKey="per_scheduleinspection_option_nextbusinessday"
                            GroupName="grpSameDayNextDay" />
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <ACA:AccelaRadioButton runat="server" ID="rblNextAvailableDay" LabelKey="per_scheduleinspection_option_nextavailableday"
                            GroupName="grpSameDayNextDay" />
                    </p>
                </td>
            </tr>
        </table>
    </div>
    <div class="ACA_TabRow" runat="server" id="divSameDayNextDayTip">
        <span class="ACA_Sub_Label ACA_Sub_Label_FontSize ACA_LLong">
            <ACA:AccelaLabel ID="lblSameDayNextDayTip" runat="server" LabelKey="per_scheduleinspection_label_samedaynextdayselectiontip"></ACA:AccelaLabel>
        </span>
    </div>
</div>
