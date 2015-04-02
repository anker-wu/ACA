<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GradingStyle.ascx.cs" Inherits="Accela.ACA.Web.Component.GradingStyle" %>
    <div id="divPassFail" runat="server">
        <ACA:AccelaDropDownList ID="ddlPassFail" runat="server">
        </ACA:AccelaDropDownList>
    </div>
    <div id="divPassScore" runat="server">
        <ACA:AccelaNumberText ID="txtPassScore" runat="server" MaxLength="8" DecimalDigitsLength="2" MinimumValue="0" MaximumValue="99999" Validate="MinimumValue;MaximumValue;DecimalDigitsLength"/>
    </div>
    <div id="divPercentageScore" runat="server">
        <ACA:AccelaNumberText ID="txtPercentageScore" runat="server" MaxLength="6" DecimalDigitsLength="2" MinimumValue="0" MaximumValue="100" Validate="MinimumValue;MaximumValue;DecimalDigitsLength"/>
    </div>
    <div id="divScore" runat="server">
        <ACA:AccelaNumberText ID="txtFinalScore" runat="server" MaxLength="8" DecimalDigitsLength="2" MinimumValue="0" MaximumValue="99999" Validate="MinimumValue;MaximumValue;DecimalDigitsLength"/>
    </div>

    <ACA:AccelaTextBox ID="txtGradingStyle" runat="server" />

        