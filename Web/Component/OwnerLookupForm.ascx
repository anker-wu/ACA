<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OwnerLookupForm.ascx.cs" Inherits="Accela.ACA.Web.Component.OwnerLookupForm" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
 
<asp:UpdatePanel ID="OwnerLookupEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Owner_OwnerName" runat="server" LabelKey="APO_Search_by_Owner_OwnerName"
                CssClass="ACA_NLonger" MaxLength="220"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Owner_AddressLine1" runat="server" LabelKey="APO_Search_by_Owner_AddressLine1"
                CssClass="ACA_NLonger" MaxLength="65"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Owner_AddressLine2" runat="server" LabelKey="APO_Search_by_Owner_AddressLine2"
                CssClass="ACA_NLonger" MaxLength="30" Visible="false"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Owner_AddressLine3" runat="server" LabelKey="APO_Search_by_Owner_AddressLine3"
                CssClass="ACA_NLonger" MaxLength="60" Visible="false"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Owner_City" runat="server" MaxLength="30"
                CssClass="ACA_NLong" LabelKey="APO_Search_by_Owner_City" AutoFillType="City"
                PositionID="LookUpByOwnerCity"></ACA:AccelaTextBox>
            <ACA:AccelaStateControl ID="ddlAPO_Search_by_Owner_State" AutoFillType="State" PositionID="LookUpByOwnerState"
                runat="server" LabelKey="APO_Search_by_Owner_State" />
            <ACA:AccelaZipText ID="txtAPO_Search_by_Owner_Zip" runat="server" LabelKey="APO_Search_by_Owner_Zip"
                IsIgnoreValidate="true" CssClass="ACA_NShot" MaxLength="30"></ACA:AccelaZipText>
            <ACA:AccelaCountryDropDownList ID="ddlAPOOwnerCountry" LabelKey="APO_Search_by_Owner_Country"
                runat="server">
            </ACA:AccelaCountryDropDownList>
            <ACA:AccelaPhoneText ID="txtOwnerPhone" runat="server" LabelKey="apo_owner_phone"
                CssClass="ACA_NLong" IsIgnoreValidate="true"></ACA:AccelaPhoneText>
            <ACA:AccelaPhoneText ID="txtOwnerFax" runat="server" LabelKey="apo_owner_fax" CssClass="ACA_NLong"
                IsIgnoreValidate="true"></ACA:AccelaPhoneText>
            <ACA:AccelaTextBox ID="txtOwnerEmail" MaxLength="70" runat="server" LabelKey="apo_owner_email"
                CssClass="ACA_NLong"></ACA:AccelaTextBox>
            <ACA:AccelaCheckBox ID="ddlAPO_Search_by_Owner_Status" runat="server" LabelKey="aca_searchbyowner_label_status" />
            <uc1:TemplateEdit ID="templateEdit" IsForSearch="true" runat="server"/>
        </ACA:AccelaFormDesignerPlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
