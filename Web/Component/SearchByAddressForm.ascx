<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchByAddressForm.ascx.cs"
    Inherits="Accela.ACA.Web.Component.SearchByAddressForm" %>
<%@ Register Src="~/Component/TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="ucl" %>
<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaCountryDropDownList ID="ddlCountry" runat="server" LabelKey="per_permitList_label_Country">
    </ACA:AccelaCountryDropDownList>
    <ACA:AccelaRangeNumberText ID="txtNumber" runat="server" LabelKey="per_permitList_label_StreetNo"
         ToolTipLabelKeyRangeFrom="aca_recordsearch_label_streetnofrom" ToolTipLabelKeyRangeTo="aca_recordsearch_label_streetnoto"
         IsNeedDot="false" MaxLength="9"></ACA:AccelaRangeNumberText>
    <ACA:AccelaTextBox ID="txtStartFraction" runat="server" LabelKey="per_permitList_label_StartFraction"
         MaxLength="20"></ACA:AccelaTextBox>
    <ACA:AccelaRangeNumberText ID="txtStreetEnd" runat="server" LabelKey="per_permitList_label_StreetEnd"
         ToolTipLabelKeyRangeFrom="aca_recordsearch_label_streetendfrom" ToolTipLabelKeyRangeTo="aca_recordsearch_label_streetendto"
         IsNeedDot="false" MaxLength="9"></ACA:AccelaRangeNumberText>
    <ACA:AccelaTextBox ID="txtEndFraction" runat="server" LabelKey="per_permitList_label_EndFraction"
         MaxLength="20"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlDirection" runat="server" LabelKey="per_permitList_label_direction">
    </ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtPrefix" runat="server" LabelKey="per_permitList_label_Prefix"
         MaxLength="20"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtStreetName" runat="server" LabelKey="per_permitList_label_street"
         MaxLength="40"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlStreetSuffix" runat="server" LabelKey="per_permitList_label_streetType">
    </ACA:AccelaDropDownList>
    <ACA:AccelaDropDownList ID="ddlStreetSuffixDirection" runat="server" LabelKey="per_permitList_label_StreetSuffixDirection">
    </ACA:AccelaDropDownList>
    <ACA:AccelaDropDownList ID="ddlUnitType" runat="server" LabelKey="per_permitList_label_unitType">
    </ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtUnitNo" runat="server" LabelKey="per_permitList_label_unit"
         MaxLength="21"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtUnitEnd" runat="server" LabelKey="per_permitList_label_UnitEnd"
         MaxLength="10"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtSecondaryRoad" runat="server" LabelKey="per_permitList_label_SecondaryRoad"
         MaxLength="100"></ACA:AccelaTextBox>
    <ACA:AccelaNumberText ID="txtSecondaryRoadNo" runat="server" LabelKey="per_permitList_label_SecondaryRoadNo"
         IsNeedDot="false" MaxLength="20"></ACA:AccelaNumberText>
    <ACA:AccelaTextBox ID="txtNeighborhoodP" runat="server" LabelKey="per_permitList_label_NeighborhoodP"
         MaxLength="6"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtNeighborhood" runat="server" LabelKey="per_permitList_label_Neighborhood"
         MaxLength="30"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4"
        LabelKey="per_permitList_Label_description"  MaxLength="255"
        Validate="MaxLength"></ACA:AccelaTextBox>
    <ACA:AccelaNumberText ID="txtDistance" runat="server" LabelKey="per_permitList_label_Distance"
         MaxLength="9" DecimalDigitsLength="3" Validate="MaxLength;DecimalDigitsLength"></ACA:AccelaNumberText>
    <ACA:AccelaNumberText ID="txtXCoordinator" runat="server" LabelKey="per_permitList_label_Xcoordinator"
         IsNeedNegative="true" MaxLength="18" DecimalDigitsLength="8"
        Validate="MaxLength;DecimalDigitsLength"></ACA:AccelaNumberText>
    <ACA:AccelaNumberText ID="txtYCoordinator" runat="server" LabelKey="per_permitList_label_Ycoordinator"
         IsNeedNegative="true" MaxLength="18" DecimalDigitsLength="8"
        Validate="MaxLength;DecimalDigitsLength"></ACA:AccelaNumberText>
    <ACA:AccelaTextBox ID="txtInspectionDP" runat="server" LabelKey="per_permitList_label_InspectionDP"
         MaxLength="6"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtInspectionD" runat="server" LabelKey="per_permitList_label_InspectionD"
         MaxLength="255"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtCity" PositionID="SearchByAddressCity" AutoFillType="City"
        runat="server" LabelKey="per_permitList_label_city"  MaxLength="32"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtCounty" runat="server" LabelKey="per_permitList_label_County"
         MaxLength="30"></ACA:AccelaTextBox>
    <ACA:AccelaStateControl ID="txtState" PositionID="SearchByAddressState" AutoFillType="State"
        LabelKey="per_permitList_label_state" runat="server" />
    <ACA:AccelaZipText ID="txtAppZipSearchPermit"  runat="server"
        IsIgnoreValidate="true" LabelKey="per_permitList_label_zip"></ACA:AccelaZipText>
    <ACA:AccelaTextBox ID="txtStreetAddress" TextMode="MultiLine" Rows="4" runat="server"
        CssClass="ACA_MLonger" LabelKey="per_permitList_label_StreetFlag" MaxLength="1024"
        Validate="MaxLength"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtAddressLine1" runat="server" LabelKey="per_permitList_label_AddressLine1"
         MaxLength="200"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtAddressLine2" runat="server" LabelKey="per_permitList_label_AddressLine2"
         MaxLength="200"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtLevelPrefix" runat="server" LabelKey="aca_recordhome_label_levelprefix"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtLevelNbrStart" runat="server" LabelKey="aca_recordhome_label_levelnumberstart"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtLevelNbrEnd" runat="server" LabelKey="aca_recordhome_label_levelnumberend"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtHouseAlphaStart" runat="server" LabelKey="aca_recordhome_label_housealphastart"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtHouseAlphaEnd" runat="server" LabelKey="aca_recordhome_label_housealphaend"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ucl:TemplateEdit ID="templateEdit" IsForSearch="true" runat="server" />
</ACA:AccelaFormDesignerPlaceHolder>
