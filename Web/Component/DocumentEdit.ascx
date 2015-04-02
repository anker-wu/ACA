<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentEdit.ascx.cs"
    Inherits="Accela.ACA.Web.Component.DocumentEdit" %>
 <%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagName="GenericTemplate" TagPrefix="ACA" %>

<asp:UpdatePanel ID="documentEditContainer" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaDropDownList ID="ddlAssociatedPeople" OnSelectedIndexChanged="AssociatedPeople_SelectedIndexChanged" 
              AutoPostBack="true" IsDBRequired="true" LabelKey="aca_attachment_label_associatedpeople" runat="server"
              ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip" />
            <ACA:AccelaDropDownList ID="ddlDocType" LabelKey="nu_FileLoadPage_label_DocType" IsDBRequired="true" 
                 ToolTipLabelKey="aca_common_msg_dropdown_updateformlayout_tip" AutoPostBack="true" 
                 runat="server" OnSelectedIndexChanged="DocumentType_SelectedIndexChanged" IsHidden="true"/>
            <ACA:AccelaCheckBoxList ID="ckbVirtualFolders" InstructionAlign="Left" LabelKey="aca_attachment_label_virtualfolders" runat="server" IsHidden="true">
            </ACA:AccelaCheckBoxList>
            <ACA:AccelaNameValueLabel ID="lblFileName" IsDBRequired="true" Font-Bold="true" runat="server" LabelKey="per_attachment_Label_file"></ACA:AccelaNameValueLabel>
            <ACA:AccelaTextBox ID="txtAFileDescription" TextMode="MultiLine" Rows="6" Validate="maxlength"
                MaxLength="2000" CssClass="ACA_TextArea" runat="server" LabelKey="per_attachment_Label_fileDescriptiom" />
            <ACA:GenericTemplate ID="genericTemplate" runat="server" />
            <ACA:AccelaDropDownList ID="ddlAlsoAttachTo" LabelKey="aca_attachmentedit_label_alsoattachto" runat="server"/>
        </ACA:AccelaFormDesignerPlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>

<!--Parameters for sliverlight control-->
<input type="hidden" id="languageSetting" value='<%=LanguageCode %>' />
