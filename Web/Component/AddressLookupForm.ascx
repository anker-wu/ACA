<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressLookupForm.ascx.cs" Inherits="Accela.ACA.Web.Component.AddressLookupForm" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
<asp:UpdatePanel ID="AddressLookupEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate> 
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaCountryDropDownList ID="ddlAPOAddressCountry" LabelKey="APO_Search_by_Address_Country"
                runat="server">
            </ACA:AccelaCountryDropDownList>
            <ACA:AccelaRangeNumberText IsNeedDot="false" ID="txtAPO_Search_by_Address_StreetNumber"
                runat="server" LabelKey="APO_Search_by_Address_StreetNumber" ToolTipLabelKeyRangeFrom="aca_apo_label_streetnumberfrom" 
                ToolTipLabelKeyRangeTo="aca_apo_label_streetnumberto" CssClass="ACA_NLonger"
                MaxLength="9"></ACA:AccelaRangeNumberText>
            <ACA:AccelaDropDownList ID="ddlAPO_Search_by_Address_Direction" runat="server" LabelKey="APO_Search_by_Address_Direction">
            </ACA:AccelaDropDownList>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Address_StreetName" runat="server" MaxLength="50"
                LabelKey="APO_Search_by_Address_StreetName" CssClass="ACA_NLonger"></ACA:AccelaTextBox>
            <ACA:AccelaDropDownList ID="ddlStreetSuffix" runat="server" LabelKey="per_permitList_label_streetType">
            </ACA:AccelaDropDownList>
            <ACA:AccelaDropDownList ID="ddlAPO_Search_by_Address_UnitType" runat="server" LabelKey="APO_Search_by_Address_UnitType">
            </ACA:AccelaDropDownList>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Address_UnitNo" runat="server" MaxLength="10"
                LabelKey="APO_Search_by_Address_UnitNo" CssClass="ACA_NLonger"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Address_City" AutoFillType="City" PositionID="LookUpByAddressCity"
                runat="server" LabelKey="APO_Search_by_Address_City" CssClass="ACA_NLonger" MaxLength="30"></ACA:AccelaTextBox>
            <ACA:AccelaStateControl ID="ddlAPO_Search_by_Address_State" PositionID="LookUpByAddressState"
                AutoFillType="State" runat="server" LabelKey="APO_Search_by_Address_State" />
            <ACA:AccelaZipText ID="txtAPO_Search_by_Address_Zip" runat="server" LabelKey="APO_Search_by_Address_Zip"
                IsIgnoreValidate="true" CssClass="ACA_NLonger"></ACA:AccelaZipText>
            <ACA:AccelaCheckBox ID="ddlAPO_Search_by_Address_Status" runat="server" LabelKey="aca_searchbyaddress_label_status" />
            <ACA:AccelaDropDownList ID="ddlStreetSuffixDirection" runat="server" LabelKey="aca_search_by_address_label_streetsuffixdirection"></ACA:AccelaDropDownList>
            
            <ACA:AccelaTextBox ID="txtLevelPrefix" runat="server" LabelKey="aca_aposearch_label_levelprefix"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtLevelNbrStart" runat="server" LabelKey="aca_aposearch_label_levelnumberstart"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtLevelNbrEnd" runat="server" LabelKey="aca_aposearch_label_levelnumberend"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtHouseAlphaStart" runat="server" LabelKey="aca_aposearch_label_housealphastart"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtHouseAlphaEnd" runat="server" LabelKey="aca_aposearch_label_housealphaend"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtStartFraction" runat="server" LabelKey="aca_aposearch_label_startfraction"
                MaxLength="20" Validate="MaxLength"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtEndFraction" runat="server" LabelKey="aca_aposearch_label_endfraction"
            MaxLength="20" Validate="MaxLength"></ACA:AccelaTextBox>

            <uc1:TemplateEdit ID="templateEdit" IsForSearch="true" runat="server"/>
        </ACA:AccelaFormDesignerPlaceHolder>
    </ContentTemplate>               
</asp:UpdatePanel>
