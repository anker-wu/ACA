<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefAvailableExaminationScheduleSearchForm.ascx.cs" Inherits="Accela.ACA.Web.Component.RefAvailableExaminationScheduleSearchForm" %>
<div class="ACA_TabRow">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr valign="bottom">
            <td>
                <div class="ACA_FLeft">
                    <ACA:AccelaDropDownList ID="ddlProvider" runat="server" LabelKey="aca_exam_schedule_availablelist_provider"
                        ShowType="showdescription">
                    </ACA:AccelaDropDownList>
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaDropDownList ID="ddlCity" runat="server" LabelKey="aca_exam_schedule_availablelist_city"
                        ShowType="showdescription">
                    </ACA:AccelaDropDownList>
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaDropDownList ID="ddlState" runat="server" LabelKey="aca_exam_schedule_availablelist_state"
                        ShowType="showdescription">
                    </ACA:AccelaDropDownList>
                </div>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow">
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr valign ="bottom">
            <td>
                <ACA:AccelaCalendarText ID="dateFromTime" runat="server" LabelKey="aca_exam_schedule_availablelist_from" ShowType="showdescription"></ACA:AccelaCalendarText>
            </td>
            <td>
                <ACA:AccelaCalendarText ID="dateToTime" runat="server" LabelKey="aca_exam_schedule_availablelist_to" ShowType="showdescription"></ACA:AccelaCalendarText>
            </td>
        </tr>
    </table>
</div>