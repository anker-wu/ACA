<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.CapDescriptionEdit"
    CodeBehind="CapDescriptionEdit.ascx.cs" %>
<ACA:AccelaHideLink ID="hlBegin" runat="server" AltKey="img_alt_form_begin" CssClass="ACA_Hide"/>
<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaTextBox ID="txtJobValue" LabelKey="capDescriptionEdit_label_jobValue"  runat="server" MaxLength="12" Width="150" onkeyup="replaceDecimalSeparator(this,event);" onblur="SetValue(this,RoundDecimal(GetValue(this),2,true));" />
    <ACA:AccelaNumberText ID="txtHousingUnit" LabelKey="capDescriptionEdit_label_houseUnit"    runat="server" MaxLength="13" Validate="MaxLength" IsNeedDot="false" />
    <ACA:AccelaNumberText ID="txtBuildingsNumbers" LabelKey="capDescriptionEdit_label_buildNumber"  runat="server" MaxLength="13" Validate="MaxLength" IsNeedDot="false" />
    <ACA:AccelaCheckBox ID="chkPublicOwned" runat="server" LabelKey="capDescriptionEdit_label_publicOwner" />
    <ACA:AccelaDropDownList ID="ddlConstrucType" LabelKey="capDescriptionEdit_label_constructType" runat="server" />
</ACA:AccelaFormDesignerPlaceHolder>
<ACA:AccelaHideLink ID="hlEnd" runat="server" CssClass="ACA_Hide" AltKey="aca_common_msg_formend" />
<ACA:AccelaHeightSeparate ID="sepForAdditional" runat="server" Height="15" />

