<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionReadyTime.ascx.cs"
    Inherits="Accela.ACA.Web.Component.InspectionReadyTime" %>
<%@ Register Src="AccelaTimeSelect.ascx" TagName="AccelaTimeSelect" TagPrefix="uc1" %>
<div class="ACA_TabRow">
    <uc1:AccelaTimeSelect ID="readyTimeSelect" runat="server" LabelKey="per_inspectionshedule_label_selectreadytime"
        SubLabelCssClass="ACA_Sub_Label ACA_Sub_Label_FontSize ACA_Color_Red"></uc1:AccelaTimeSelect>
</div>
<span class="ACA_Sub_Label ACA_Sub_Label_FontSize ACA_LLong">
    <ACA:AccelaLabel ID="lblReadyTimeTip" runat="server" LabelKey="per_scheduleinspection_label_samedaynextdayselectiontip"></ACA:AccelaLabel>
</span>