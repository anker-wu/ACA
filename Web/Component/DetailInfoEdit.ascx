<%@ Control Language="C#" AutoEventWireup="true" Inherits="DetailInfoEdit" CodeBehind="DetailInfoEdit.ascx.cs" %>
<ACA:AccelaHideLink ID="hlBegin" runat="server" AltKey="img_alt_form_begin" CssClass="ACA_Hide"/>
<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaTextBox ID="txtApplicationName" LabelKey="capDescriptionEdit_label_AppName"
        runat="server" MaxLength="255"  />
    <ACA:AccelaTextBox ID="txtDescriptionGeneral" LabelKey="capDescriptionEdit_label_note"
        runat="server" MaxLength="255" Validate="MaxLength" />
    <ACA:AccelaTextBox ID="txtDescriptionDetail" runat="server" CssClass="ACA_XLong"
        Rows="4" TextMode="MultiLine" Validate="MaxLength" LabelKey="per_permitReg_label_descriptionDetailEdit"
        MaxLength="4000">
    </ACA:AccelaTextBox>
</ACA:AccelaFormDesignerPlaceHolder>
<ACA:AccelaHideLink ID="hlEnd" runat="server" AltKey="aca_common_msg_formend" CssClass="ACA_Hide"/>
<ACA:AccelaHeightSeparate ID="sepForDetail" runat="server" Height="15" />
