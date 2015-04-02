<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ScheduleCalendar"
    CodeBehind="ScheduleCalendar.ascx.cs" %>
<div class="ACA_TabRow">
    <h2>
        <i>
            <ACA:AccelaLabel ID="per_scheduleInspection_label_action" LabelKey="per_scheduleInspection_label_action"
                runat="server"></ACA:AccelaLabel>
        </i>
    </h2>
</div>
<br />
<asp:UpdatePanel ID="upScheduleCalendar" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table role='presentation' class="InspectionWizardPageWidth">
            <tr valign="top">
                <td>
                </td>
                <td>
                    <asp:Calendar ID="calendar1" runat="server" ShowNextPrevMonth="true" ShowTitle="false"
                        DayNameFormat="Shortest" OnDayRender="Calendar_OnDayRender" OnSelectionChanged="Calendar_SelectionChanged"
                        CssClass="ACA_Calendar_Container" BorderStyle="none">
                        <DayHeaderStyle CssClass="ACA_Calendar_Day_Header font11px" />
                        <DayStyle CssClass="ACA_Calendar_Day_Active font11px ACA_LinkButton" />
                        <OtherMonthDayStyle CssClass="ACA_Calendar_Other_Month_Day" />
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="calendar2" runat="server" ShowNextPrevMonth="true" ShowTitle="false"
                        DayNameFormat="Shortest" OnDayRender="Calendar_OnDayRender" OnSelectionChanged="Calendar_SelectionChanged"
                        CssClass="ACA_Calendar_Container" BorderStyle="none">
                        <DayHeaderStyle CssClass="ACA_Calendar_Day_Header font11px" />
                        <DayStyle CssClass="ACA_Calendar_Day_Active font11px ACA_LinkButton" />
                        <OtherMonthDayStyle CssClass="ACA_Calendar_Other_Month_Day" />
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="calendar3" runat="server" ShowNextPrevMonth="true" ShowTitle="false"
                        DayNameFormat="Shortest" OnDayRender="Calendar_OnDayRender" CssClass="ACA_Calendar_Container"
                        BorderStyle="none" OnSelectionChanged="Calendar_SelectionChanged">
                        <DayHeaderStyle CssClass="ACA_Calendar_Day_Header font11px" />
                        <DayStyle CssClass="ACA_Calendar_Day_Active font11px ACA_LinkButton" />
                        <OtherMonthDayStyle CssClass="ACA_Calendar_Other_Month_Day" />
                    </asp:Calendar>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <table role='presentation' class="InspectionWizardPageWidth">
            <tr valign="top">
                <td style="width: 50%;">
                    <ACA:AccelaLinkButton CssClass="ACA_Calendar_Prev font11px" runat="server" ID="AccelaLinkButton1"
                        LabelKey="aca_calendar_prev_month" OnClick="LnkPreMonth_Click"></ACA:AccelaLinkButton>
                </td>
                <td>
                    <ACA:AccelaLinkButton CssClass="ACA_Calendar_Next font11px" runat="server" ID="AccelaLinkButton2"
                        LabelKey="aca_calendar_next_month" OnClick="LnkNextMonth_Click"></ACA:AccelaLinkButton>
                </td>
            </tr>
        </table>
        <div class="ACA_TabRow ACA_BkGray">
        </div>
        <ACA:AccelaHideLink ID="hlUseToFocus" runat="server" AltKey="img_alt_form_begin" />
        <div class="ACA_TabRow">
            <div class="Header_h3">
                <ACA:AccelaLabel CssClass="ACA_Title_Color" ID="lblAvaliableTimes" runat="server"></ACA:AccelaLabel>
            </div>
        </div>
        <div id="divMorningEventItems" runat="server">
            <div class="Header_h4">
                <ACA:AccelaLabel CssClass="ACA_Title_Color" ID="lblMorningTime" runat="server" />
            </div>
            <asp:PlaceHolder ID="phMorningEventItems" runat="server" />
        </div>
        <div id="divAfternoonEventItems" runat="server">
            <div class="Header_h4">
                <ACA:AccelaLabel CssClass="ACA_Title_Color" ID="lblAfternoonTime" runat="server" />
            </div>
            <asp:PlaceHolder ID="phAfternoonEventItems" runat="server" />
        </div>
        <div id="divDayEventItems" runat="server">
            <asp:PlaceHolder ID="phDayEventItems" runat="server" />
        </div>
        <div id="divBottomGray" class="ACA_TabRow ACA_BkGray" runat="server" visible="false">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
